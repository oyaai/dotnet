#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpUserEntity
'	Class Discription	: Class of table user
'	Create User 		: Boon
'	Create Date		    : 15-05-2013
'
' UPDATE INFORMATION
'	Update User		: Charoon
'	Update Date		: 30-05-2013
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Class ImpUserEntity
        Implements IUserEntity

        Private _id As Integer
        Private _user_name As String
        Private _password As String
        Private _first_name As String
        Private _last_name As String
        Private _account_next_approve As Integer
        Private _purchase_next_approve As Integer
        Private _outsource_next_approve As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String
        Private _last_login As String
        Private _delete_fg As Integer
        Private _department_id As Integer

        ' Add by Komsan L. 2013-05-20 (for KTOV01)
        Private _department_name As String
        ' End
        Private daoUser As New Dao.ImpUserDao

#Region "Function"
        Public Function CheckApproveUser(ByVal objType As Enums.PurchaseTypes, ByVal intUser_id As Integer) As Integer Implements IUserEntity.CheckApproveUser
            Return daoUser.DB_CheckApproveUser(objType, intUser_id)
        End Function

        Public Function AddUser(ByVal objUser As IUserEntity, ByRef intUser_id As Integer) As Boolean Implements IUserEntity.AddUser
            Return Nothing
        End Function

        Public Function DeleteUser(ByVal intUser_id As Integer) As Boolean Implements IUserEntity.DeleteUser
            Return Nothing
        End Function

        Public Function SearchUser(ByVal intUser_id As Integer) As IUserEntity Implements IUserEntity.SearchUser
            Dim objUser As New Dao.ImpUserDao
            Return objUser.DB_SearchUser(intUser_id)
        End Function

        Public Function UpdateUser(ByVal objUser As IUserEntity) As Boolean Implements IUserEntity.UpdateUser
            Return Nothing
        End Function

        Public Function GetUserLogin(ByVal strUserName As String, ByVal strPassword As String, Optional ByVal strUserId As String = "") As IUserEntity Implements IUserEntity.GetUserLogin
            Return daoUser.GetUserLogin(strUserName, strPassword, strUserId)
        End Function

        Public Function UpdateLastLogin(ByVal intUserID As Integer) As Integer Implements IUserEntity.UpdateLastLogin
            Return daoUser.UpdateLastLogin(intUserID)
        End Function

        ' Add by Charoon 2013-05-30 (for KTMS23)
        Public Function CheckUserByDepartment(ByVal intDepartment_id As Integer) As Integer Implements IUserEntity.CheckUserByDepartment
            Return daoUser.CheckUserByDepartment(intDepartment_id)
        End Function

        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Public Function GetUserForList(Optional ByVal strDepartmentName As String = Nothing) As System.Collections.Generic.List(Of IUserEntity) Implements IUserEntity.GetUserForList
            Return daoUser.GetUserForList(strDepartmentName)
        End Function

#End Region

#Region "Property"
        Public Property account_next_approve() As Integer Implements IUserEntity.account_next_approve
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As Integer)
                _account_next_approve = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IUserEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IUserEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IUserEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property department_id() As Integer Implements IUserEntity.department_id
            Get
                Return _department_id
            End Get
            Set(ByVal value As Integer)
                _department_id = value
            End Set
        End Property

        Public Property first_name() As String Implements IUserEntity.first_name
            Get
                Return _first_name
            End Get
            Set(ByVal value As String)
                _first_name = value
            End Set
        End Property

        Public Property id() As Integer Implements IUserEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property last_login() As String Implements IUserEntity.last_login
            Get
                Return _last_login
            End Get
            Set(ByVal value As String)
                _last_login = value
            End Set
        End Property

        Public Property last_name() As String Implements IUserEntity.last_name
            Get
                Return _last_name
            End Get
            Set(ByVal value As String)
                _last_name = value
            End Set
        End Property

        Public Property outsource_next_approve() As Integer Implements IUserEntity.outsource_next_approve
            Get
                Return _outsource_next_approve
            End Get
            Set(ByVal value As Integer)
                _outsource_next_approve = value
            End Set
        End Property

        Public Property password() As String Implements IUserEntity.password
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property purchase_next_approve() As Integer Implements IUserEntity.purchase_next_approve
            Get
                Return _purchase_next_approve
            End Get
            Set(ByVal value As Integer)
                _purchase_next_approve = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IUserEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IUserEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property user_name() As String Implements IUserEntity.user_name
            Get
                Return _user_name
            End Get
            Set(ByVal value As String)
                _user_name = value
            End Set
        End Property

        Public Property department_name() As String Implements IUserEntity.department_name
            Get
                Return _department_name
            End Get
            Set(ByVal value As String)
                _department_name = value
            End Set
        End Property
#End Region

        
    End Class
End Namespace

