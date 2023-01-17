#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.

' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpUserLoginEntity
'	Class Discription	: Class of table user
'	Create User 		: Nisa S.
'	Create Date		    : 10-07-2013

' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:

'******************************************************************/
#End Region


#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity

    Public Class ImpUserLoginEntity
        Implements IUserLoginEntity

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

        Private objUserLogin As New ImpUserLoginDao

#Region "Function"
        Public Function GetUserLoginList(ByVal strID As String, ByVal strUserName As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strDepartment As String) As System.Collections.Generic.List(Of ImpUserLoginDetailEntity) Implements IUserLoginEntity.GetUserLoginList
            Return objUserLogin.GetUserLoginList(strID, strUserName, strFirstName, strLastName, strDepartment)
        End Function

        Public Function CountUsedInPO(ByVal intUserLoginID As Integer) As Integer Implements IUserLoginEntity.CountUsedInPO
            Return objUserLogin.CountUsedInPO(intUserLoginID)
        End Function

        Public Function DeleteUserLogin(ByVal intUserLoginID As Integer) As Integer Implements IUserLoginEntity.DeleteUserLogin
            Return objUserLogin.DeleteUserLogin(intUserLoginID)
        End Function

        Public Function GetDepartmentForList() As System.Collections.Generic.List(Of IUserLoginEntity) Implements IUserLoginEntity.GetDepartmentForList
            Return objUserLogin.GetDepartmentForList
        End Function

        Public Function GetAccount_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of IUserLoginEntity) Implements IUserLoginEntity.GetAccount_Next_ApproveForList
            Return objUserLogin.GetAccount_Next_ApproveForList(id)
        End Function

        Public Function GetPurchase_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of IUserLoginEntity) Implements IUserLoginEntity.GetPurchase_Next_ApproveForList
            Return objUserLogin.GetPurchase_Next_ApproveForList(id)
        End Function

        Public Function GetOutsource_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of IUserLoginEntity) Implements IUserLoginEntity.GetOutsource_Next_ApproveForList
            Return objUserLogin.GetOutsource_Next_ApproveForList(id)
        End Function

        Public Function CheckDupUserLogin(ByVal intUserLoginID As String, ByVal strUserName As String) As Integer Implements IUserLoginEntity.CheckDupUserLogin
            Return objUserLogin.CheckDupUserLogin(intUserLoginID, strUserName)
        End Function

        Public Function InsertUserLogin(ByVal objUserLoginEnt As IUserLoginEntity) As Integer Implements IUserLoginEntity.InsertUserLogin
            Return objUserLogin.InsertUserLogin(objUserLoginEnt)
        End Function

        Public Function UpdateUserLogin(ByVal objUserLoginEnt As IUserLoginEntity) As Integer Implements IUserLoginEntity.UpdateUserLogin
            Return objUserLogin.UpdateUserLogin(objUserLoginEnt)
        End Function

        Public Function GetUserLoginByID(ByVal intUserLoginID As String) As IUserLoginEntity Implements IUserLoginEntity.GetUserLoginByID
            Return objUserLogin.GetUserLoginByID(intUserLoginID)
        End Function

        Public Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As ImpUserLoginEntity Implements IUserLoginEntity.GetUserLoginForDetail
            Return objUserLogin.GetUserLoginForDetail(intUserLoginID)
        End Function
#End Region


#Region "Property"

        Public Property id() As Integer Implements IUserLoginEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property user_name() As String Implements IUserLoginEntity.user_name
            Get
                Return _user_name
            End Get
            Set(ByVal value As String)
                _user_name = value
            End Set
        End Property

        Public Property password() As String Implements IUserLoginEntity.password
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property first_name() As String Implements IUserLoginEntity.first_name
            Get
                Return _first_name
            End Get
            Set(ByVal value As String)
                _first_name = value
            End Set
        End Property

        Public Property last_name() As String Implements IUserLoginEntity.last_name
            Get
                Return _last_name
            End Get
            Set(ByVal value As String)
                _last_name = value
            End Set
        End Property

        Public Property account_next_approve() As String Implements IUserLoginEntity.account_next_approve
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As String)
                _account_next_approve = value
            End Set
        End Property

        Public Property purchase_next_approve() As String Implements IUserLoginEntity.purchase_next_approve
            Get
                Return _purchase_next_approve
            End Get
            Set(ByVal value As String)
                _purchase_next_approve = value
            End Set
        End Property

        Public Property outsource_next_approve() As String Implements IUserLoginEntity.outsource_next_approve
            Get
                Return _outsource_next_approve
            End Get
            Set(ByVal value As String)
                _outsource_next_approve = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IUserLoginEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IUserLoginEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IUserLoginEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IUserLoginEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property last_login() As String Implements IUserLoginEntity.last_login
            Get
                Return _last_login
            End Get
            Set(ByVal value As String)
                _last_login = value
            End Set
        End Property


        Public Property delete_fg() As Integer Implements IUserLoginEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property


        Public Property department_id() As String Implements IUserLoginEntity.department_id
            Get
                Return _department_id
            End Get
            Set(ByVal value As String)
                _department_id = value
            End Set
        End Property

        Public Property name() As String Implements IUserLoginEntity.name
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