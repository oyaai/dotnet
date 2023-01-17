#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : IEDto
'	Class Discription	: Dto class IE 
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


#Region "Imports"

Imports Interfaces
Imports Microsoft.VisualBasic

#End Region


Namespace Dto

    <Serializable()> _
    Public Class IEDto

#Region "Constructors"

        Public Sub New()
            _ie_category = New IECategoryDto
        End Sub

#End Region

#Region "Fields"

        Private _id As Integer
        Private _category_id As String
        Private _code As String
        Private _name As String
        Private _delete_flag As Byte
        Private _created_by As Int32?
        Private _created_date As String
        Private _updated_by As Int32?
        Private _updated_date As String

        Private _isInUsed As Boolean

        Private _ie_category As IECategoryDto
        Private _user As UserDto

#End Region

#Region "Properties"

        Public Property ID() As Integer
            Get
                Return Me._id
            End Get
            Set(ByVal value As Integer)
                Me._id = value
            End Set
        End Property


        '''<summary>
        '''
        '''</summary>
        Public Property CategoryID() As String
            Get
                Return Me._category_id
            End Get
            Set(ByVal value As String)
                Me._category_id = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property Code() As String
            Get
                Return Me._code
            End Get
            Set(ByVal value As String)
                Me._code = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property DeleteFlag() As Byte
            Get
                Return Me._delete_flag
            End Get
            Set(ByVal value As Byte)
                Me._delete_flag = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>

        Public Property CreatedBy() As Int32?
            Get
                Return Me._created_by
            End Get
            Set(ByVal value As Int32?)
                Me._created_by = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property CreatedDate() As String
            Get
                Return Me._created_date
            End Get
            Set(ByVal value As String)
                Me._created_date = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property UpdatedBy() As Int32?
            Get
                Return Me._updated_by
            End Get
            Set(ByVal value As Int32?)
                Me._updated_by = value
            End Set
        End Property

        Public Property UpdatedDate() As String
            Get
                Return Me._updated_date
            End Get
            Set(ByVal value As String)
                Me._updated_date = value
            End Set
        End Property

        Public Property User() As UserDto
            Get
                Return _user
            End Get
            Set(ByVal value As UserDto)
                _user = value
            End Set
        End Property

        Public Property IsInUsed() As Boolean
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
            End Set
        End Property

#End Region

#Region "Related DTOs"

        Public Property Category() As IECategoryDto
            Get
                Return _ie_category
            End Get
            Set(ByVal value As IECategoryDto)
                _ie_category = value
            End Set
        End Property

#End Region

    End Class
End Namespace