#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Overview
'	Class Name		    : Overview_KTOV01
'	Class Discription	: Webpage for login
'	Create User 		: Komsan Luecha
'	Create Date		    : 20-05-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Common

Partial Class Overview_KTOV01
    Inherits System.Web.UI.Page

    Private cLog As New Common.Logs.Log
    Private cMessage As New Common.Utilities.Message
    Private csUser As New Service.ImpUserService
    Private objUtility As New Common.Utilities.Utility


#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' Write info log
            cLog.StartLog("KTOV01")
        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 20-05-2013
    '	Update User	    : Lin
    '	Update Date	    : 24-06-2013
    '   Descripyion     : Add process InitialPage() in event Page Load
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            'Lin start add 18/6/2013
            ' check postback page 
            If Not IsPostBack Then
                ' case not post back then call function initialpage b 
                InitialPage()
            End If
            'Lin end add 18/6/2013 
            'Boon add version
            lblVersion.Text = "Version:1.0-20141003-001"
            txtUsername.Focus()

        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: btnLogin_Click
    '	Discription	    : Event Login button is click
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnLogin_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnLogin.Click
        Try
            ' check input
            If CheckInput() Then
                ' set session
                SetSession()
            End If
        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("btnLogin_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: CheckInput
    '	Discription	    : Check input  
    '	Return Value	: True , False
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckInput() As Boolean
        CheckInput = False
        Try
            If txtUsername.Text.Trim = String.Empty Then
                cMessage.AlertMessage(cMessage.GetXMLMessage("KTOV_01_001"))
                Exit Function
            End If

            If txtPassword.Text.Trim = String.Empty Then
                cMessage.AlertMessage(cMessage.GetXMLMessage("KTOV_01_002"))
                Exit Function
            End If

            CheckInput = True
        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("CheckInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetSession
    '	Discription	    : Set session value
    '	Return Value	: Nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSession()
        Try
            ' Dto variable for keep user data
            Dim cdUser As New Dto.UserDto
            ' call GetUserLogin from user service
            cdUser = csUser.GetUserLogin(txtUsername.Text.Trim, _
                                         txtPassword.Text.Trim)
            ' check exist value and assign
            If cdUser.UserID <> String.Empty Then
                ' Description: Keep UserDto in Session
                ' Author: Prasert Simla
                ' Date: 6/6/2013
                Session("User") = cdUser

                Session("UserID") = cdUser.UserID
                Session("UserName") = cdUser.UserName
                Session("DepartmentName") = cdUser.DepartmentName
                Session("AccountNextApprove") = cdUser.ACNextApprove
                Session("PurchaseNextApprove") = cdUser.PUNextApprove
                Session("OutSourceNextApprove") = cdUser.OSNextApprove
                ' update last login
                csUser.UpdateLastLogin(cdUser.UserID)
                ' get user permission
                LoadPermission(cdUser.UserID)
                ' get menu
                LoadMenu(cdUser.UserID)
                ' redirect to KTOV02
                Response.Redirect("KTOV02.aspx")
            Else
                ' case not exist value then show error message
                cMessage.AlertMessage(cMessage.GetXMLMessage("KTOV_01_003"))
            End If
        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("SetSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadPermission
    '	Discription	    : Load permission
    '	Return Value	: Nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadPermission(ByVal intUserID As Integer)
        Try
            ' variable keep list of permission
            Dim listUserPermission As New List(Of Common.UserPermissions.UserPermissionDto)
            ' call service to get user permission
            listUserPermission = csUser.GetUserPermission(intUserID)
            ' keep to session
            Session("ListPermission") = listUserPermission
        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("LoadPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadMenu
    '	Discription	    : Load menu
    '	Return Value	: Nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadMenu(ByVal intUserID As Integer)
        Try
            ' variable service menu object
            Dim csMenu As New Service.ImpMenuService
            ' variable list
            Dim listMenu As New List(Of Dto.MenuDto)

            ' list menu 
            listMenu = csMenu.GetMenuList(intUserID)

            ' assign list to session
            Session("ListMenu") = listMenu
        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("LoadMenu", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Lin
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            Dim strPageEnter As String = objUtility.GetQueryString("Result")

            If strPageEnter = "True" Then
                ''Clear session Username
                Session("UserName") = Nothing
                Session.Remove("UserName")
                Session.Clear()

                'Clear all session
                Session.RemoveAll()
                Session.Clear()
            End If
        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("InitialPage", ex.Message.ToString, "")
        End Try
    End Sub
#End Region

End Class


















