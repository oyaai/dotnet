#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : UserPermissionDto
'	Class Discription	: Dto class user permission
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
Imports Microsoft.VisualBasic

Namespace Dto
    Public Class UserPermissionDto
        Private _user_id As Integer = Nothing
        Private _menu_id As Integer = Nothing
        Private _fn_create As Integer = Nothing
        Private _fn_update As Integer = Nothing
        Private _fn_delete As Integer = Nothing
        Private _fn_list As Integer = Nothing
        Private _fn_confirm As Integer = Nothing
        Private _fn_approve As Integer = Nothing
        Private _fn_amount As Integer = Nothing

        Private _id As Integer = Nothing
        Private _value As String = Nothing
        Private _priority As Integer = Nothing
        Private _created_by As Integer = Nothing
        Private _created_date As String = Nothing
        Private _updated_by As Integer = Nothing
        Private _updated_bydate As String = Nothing
        Private _category_id As Integer = Nothing
        Private _menu_text As String = Nothing


#Region "Property"
        Property UserID() As Integer
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Property MenuID() As Integer
            Get
                Return _menu_id
            End Get
            Set(ByVal value As Integer)
                _menu_id = value
            End Set
        End Property

        Property Fn_Create() As Integer
            Get
                Return _fn_create
            End Get
            Set(ByVal value As Integer)
                _fn_create = value
            End Set
        End Property

        Property Fn_Update() As Integer
            Get
                Return _fn_update
            End Get
            Set(ByVal value As Integer)
                _fn_update = value
            End Set
        End Property

        Property Fn_Delete() As Integer
            Get
                Return _fn_delete
            End Get
            Set(ByVal value As Integer)
                _fn_delete = value
            End Set
        End Property

        Property Fn_List() As Integer
            Get
                Return _fn_list
            End Get
            Set(ByVal value As Integer)
                _fn_list = value
            End Set
        End Property

        Property Fn_Confirm() As Integer
            Get
                Return _fn_confirm
            End Get
            Set(ByVal value As Integer)
                _fn_confirm = value
            End Set
        End Property

        Property Fn_Approve() As Integer
            Get
                Return _fn_approve
            End Get
            Set(ByVal value As Integer)
                _fn_approve = value
            End Set
        End Property

        Property Fn_Amount() As Integer
            Get
                Return _fn_amount
            End Get
            Set(ByVal value As Integer)
                _fn_amount = value
            End Set
        End Property

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property category_id() As Integer
            Get
                Return _category_id
            End Get
            Set(ByVal value As Integer)
                _category_id = value
            End Set
        End Property

        Public Property created_by() As Integer
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property priority() As Integer
            Get
                Return _priority
            End Get
            Set(ByVal value As Integer)
                _priority = value
            End Set
        End Property

        Public Property updated_by() As Integer
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_bydate() As String
            Get
                Return _updated_bydate
            End Get
            Set(ByVal value As String)
                _updated_bydate = value
            End Set
        End Property

        Public Property value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        Public Property menu_text() As String
            Get
                Return _menu_text
            End Get
            Set(ByVal value As String)
                _menu_text = value
            End Set
        End Property
#End Region

    End Class
End Namespace

