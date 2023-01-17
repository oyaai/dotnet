#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : UserLoginDto
'	Class Discription	: Dto class UserLogin 
'	Create User 		: Nisa S.
'	Create Date		    : 10-07-2013
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

    Public Class UserLoginDto

        Private _id As Integer
        Private _user_name As String
        Private _password As String
        Private _first_name As String
        Private _last_name As String
        Private _account_next_approve As String
        Private _purchase_next_approve As String
        Private _outsource_next_approve As String
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String
        Private _last_login As String
        Private _delete_fg As Integer
        Private _department_id As String

        Private _name As String

#Region "Property"
        Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Property user_name() As String
            Get
                Return _user_name
            End Get
            Set(ByVal value As String)
                _user_name = value
            End Set
        End Property

        Property password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Property first_name() As String
            Get
                Return _first_name
            End Get
            Set(ByVal value As String)
                _first_name = value
            End Set
        End Property

        Property last_name() As String
            Get
                Return _last_name
            End Get
            Set(ByVal value As String)
                _last_name = value
            End Set
        End Property

        Property account_next_approve() As String
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As String)
                _account_next_approve = value
            End Set
        End Property

        Property purchase_next_approve() As String
            Get
                Return _purchase_next_approve
            End Get
            Set(ByVal value As String)
                _purchase_next_approve = value
            End Set
        End Property

        Property outsource_next_approve() As String
            Get
                Return _outsource_next_approve
            End Get
            Set(ByVal value As String)
                _outsource_next_approve = value
            End Set
        End Property

        Public Property created_by() As Int32?
            Get
                Return Me._created_by
            End Get
            Set(ByVal value As Int32?)
                Me._created_by = value
            End Set
        End Property

        Public Property created_date() As String
            Get
                Return Me._created_date
            End Get
            Set(ByVal value As String)
                Me._created_date = value
            End Set
        End Property

        Property updated_by() As Integer
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Property updated_date() As Integer
            Get
                Return _updated_date
            End Get
            Set(ByVal value As Integer)
                _updated_date = value
            End Set
        End Property

        Property last_login() As String
            Get
                Return _last_login
            End Get
            Set(ByVal value As String)
                _last_login = value
            End Set
        End Property

        Property delete_fg() As Integer
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Property department_id() As String
            Get
                Return _department_id
            End Get
            Set(ByVal value As String)
                _department_id = value
            End Set
        End Property

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
#End Region

    End Class

End Namespace