Imports Dto
Imports System.Globalization
Imports Common.Utilities
Imports Common.Logs
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Extensions
Imports System.Reflection
Imports Common.UserPermissions

Public MustInherit Class AuthorizedPageBase
    Inherits Page

#Region "Fields"

    ' Alert and Get message from XML
    Private Shared _message As Common.Utilities.Message = Nothing

    ' Log
    Private Shared _logger As Common.Logs.Log = Nothing

    ' Permission
    Private ReadOnly _permission As Common.UserPermissions.UserPermission
    Private _menuActions As Common.UserPermissions.ActionPermission

    ' Paging
    Private _pagedDataSource As PagedDataSource
    Private ReadOnly _paging As Paging

    Private _menuID As Int32 = -1

    Protected Const SessionKeyFormat As String = "{0}_{1}"

#End Region

#Region "Constructors"

    Public Sub New()
        _paging = New Paging()
        _pagedDataSource = New PagedDataSource()
        _permission = New Common.UserPermissions.UserPermission()
        _menuActions = New ActionPermission() With {.actAmount = False, .actApprove = False, .actConfirm = False, .actCreate = False, .actDelete = False, .actList = False, .actUpdate = False}
    End Sub

#End Region

#Region "Properties"

    Protected MustOverride ReadOnly Property ClassName() As String
    Protected MustOverride ReadOnly Property PagedDataSource() As PagedDataSource

    Protected Property MenuID() As Int32
        Get
            Return _menuID
        End Get
        Set(ByVal value As Int32)
            _menuID = value
        End Set
    End Property


    ''' <summary>
    ''' Get current authenticated user
    ''' </summary>
    ''' <value></value>
    ''' <returns>UserDto</returns>
    ''' <remarks></remarks>
    Protected Shadows ReadOnly Property User() As UserDto
        Get
            Return CType(Session("User"), UserDto)
        End Get
    End Property

    Protected ReadOnly Property DateCulture() As CultureInfo
        Get
            Return CultureInfo.CreateSpecificCulture("en-AU")
        End Get
    End Property

    Protected ReadOnly Property Message() As Common.Utilities.Message
        Get
            If (IsNothing(_message)) Then
                _message = New Message()
            End If

            Return _message
        End Get
    End Property

    Protected ReadOnly Property Logger() As Common.Logs.Log
        Get
            If (IsNothing(_logger)) Then
                _logger = New Log()
            End If

            Return _logger
        End Get
    End Property

    Protected ReadOnly Property Paging() As Paging
        Get
            Return _paging
        End Get
    End Property

    Protected ReadOnly Property MenuActions() As ActionPermission
        Get
            Return _menuActions
        End Get
    End Property

#End Region

#Region "Events"

    Protected Overloads Overrides Sub OnInit(ByVal e As System.EventArgs)
        'Be sure to call the base class's OnInit method!
        MyBase.OnInit(e)
    End Sub

    Protected Overloads Overrides Sub OnLoad(ByVal e As System.EventArgs)
        GetMenuActions()
        EvaluateButtonsPermission()
        If (Not IsPostBack) Then
            InitialPage()
        End If

        'Be sure to call the base class's OnLoad method!
        MyBase.OnLoad(e)
    End Sub

    Protected Overloads Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        'Be sure to call the base class's OnPreRender method!
        MyBase.OnPreRender(e)
    End Sub

    Protected Overloads Overrides Sub OnPreRenderComplete(ByVal e As System.EventArgs)
        'Be sure to call the base class's OnPreRenderComplete method!
        MyBase.OnPreRenderComplete(e)
    End Sub

#End Region

#Region "Functions"

    ''' <summary>
    ''' Page Initialization
    ''' </summary>
    ''' <remarks></remarks>
    Protected MustOverride Sub InitialPage()

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Protected MustOverride Sub TryFillSearchForm()

    ''' <summary>
    ''' Get menu's action by specified ID.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetMenuActions()
        _menuActions = _permission.CheckPermission(MenuID)
    End Sub

    ''' <summary>
    ''' Evaluate user's permission againt Button's CommandName
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub EvaluateButtonsPermission()
    End Sub

#End Region

End Class
