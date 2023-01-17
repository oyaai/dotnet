#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpUser_PermissionEntity
'	Class Discription	: Implement of user_permission table
'	Create User 		: Komsan L.
'	Create Date		    : 20-05-2013
'
' UPDATE INFORMATION
'	Update User		: Charoon
'	Update Date		: 30-05-2013
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic

Namespace Entity
    Public Class ImpUser_PermissionEntity
        Implements Entity.IUser_PermissionEntity

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

        Private daoUserPermission As New Dao.ImpUser_PermissionDao
        Private objUserPermissionDao As New Dao.ImpUser_PermissionDao
#Region "Property"
        Public Property fn_amount() As Integer Implements Entity.IUser_PermissionEntity.fn_amount
            Get
                Return _fn_amount
            End Get
            Set(ByVal value As Integer)
                _fn_amount = value
            End Set
        End Property

        Public Property fn_approve() As Integer Implements Entity.IUser_PermissionEntity.fn_approve
            Get
                Return _fn_approve
            End Get
            Set(ByVal value As Integer)
                _fn_approve = value
            End Set
        End Property

        Public Property fn_confirm() As Integer Implements Entity.IUser_PermissionEntity.fn_confirm
            Get
                Return _fn_confirm
            End Get
            Set(ByVal value As Integer)
                _fn_confirm = value
            End Set
        End Property

        Public Property fn_create() As Integer Implements Entity.IUser_PermissionEntity.fn_create
            Get
                Return _fn_create
            End Get
            Set(ByVal value As Integer)
                _fn_create = value
            End Set
        End Property

        Public Property fn_delete() As Integer Implements Entity.IUser_PermissionEntity.fn_delete
            Get
                Return _fn_delete
            End Get
            Set(ByVal value As Integer)
                _fn_delete = value
            End Set
        End Property

        Public Property fn_list() As Integer Implements Entity.IUser_PermissionEntity.fn_list
            Get
                Return _fn_list
            End Get
            Set(ByVal value As Integer)
                _fn_list = value
            End Set
        End Property

        Public Property fn_update() As Integer Implements Entity.IUser_PermissionEntity.fn_update
            Get
                Return _fn_update
            End Get
            Set(ByVal value As Integer)
                _fn_update = value
            End Set
        End Property

        Public Property menu_id() As Integer Implements Entity.IUser_PermissionEntity.menu_id
            Get
                Return _menu_id
            End Get
            Set(ByVal value As Integer)
                _menu_id = value
            End Set
        End Property

        Public Property user_id() As Integer Implements Entity.IUser_PermissionEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property id() As Integer Implements IUser_PermissionEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property category_id() As Integer Implements IUser_PermissionEntity.category_id
            Get
                Return _category_id
            End Get
            Set(ByVal value As Integer)
                _category_id = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IUser_PermissionEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IUser_PermissionEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property priority() As Integer Implements IUser_PermissionEntity.priority
            Get
                Return _priority
            End Get
            Set(ByVal value As Integer)
                _priority = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IUser_PermissionEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_bydate() As String Implements IUser_PermissionEntity.updated_bydate
            Get
                Return _updated_bydate
            End Get
            Set(ByVal value As String)
                _updated_bydate = value
            End Set
        End Property

        Public Property value() As String Implements IUser_PermissionEntity.value
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        Public Property menu_text() As String Implements IUser_PermissionEntity.menu_text
            Get
                Return _menu_text
            End Get
            Set(ByVal value As String)
                _menu_text = value
            End Set
        End Property
#End Region

#Region "Function"
        Public Function GetUserPermission(ByVal intUserID As Integer) As System.Collections.Generic.List(Of IUser_PermissionEntity) Implements IUser_PermissionEntity.GetUserPermission
            Return daoUserPermission.GetUserPermission(intUserID)
        End Function

        ' Add by Charoon 2013-05-30 (for KTMS23)
        Public Function GetUserPermission(ByVal intUserID As Integer, ByVal intMenuID As Integer) As System.Collections.Generic.List(Of IUser_PermissionEntity) Implements IUser_PermissionEntity.GetUserPermission
            Return daoUserPermission.GetUserPermission(intUserID, intMenuID)
        End Function

        ' Add by Wasan 08-07-2013 (for KTAD03)
        Public Function GetUserPermissionList(ByVal strFName As String, ByVal strLName As String, ByVal strUName As String, ByVal intDepartment As String) As System.Collections.Generic.List(Of ImpUserEntity) Implements IUser_PermissionEntity.GetUserPermissionList
            Return objUserPermissionDao.GetUserPermissionList(strFName, strLName, strUName, intDepartment)
        End Function

        ' Add by Wasan 09-07-2013 (for KTAD03)
        Public Function GetUserSetPermissionList() As System.Collections.Generic.List(Of ImpUserEntity) Implements IUser_PermissionEntity.GetUserSetPermissionList
            Return objUserPermissionDao.GetUserSetPermissionList
        End Function

        Public Function GetUserNonSetPermissionList() As System.Collections.Generic.List(Of ImpUserEntity) Implements IUser_PermissionEntity.GetUserNonSetPermissionList
            Return objUserPermissionDao.GetUserNonSetPermissionList
        End Function

        Public Function GetPermissionList() As System.Collections.Generic.List(Of ImpUser_PermissionEntity) Implements IUser_PermissionEntity.GetPermissionList
            Return objUserPermissionDao.GetPermissionList
        End Function

        Public Function GetUserPermissionSettingList(ByVal strUserID As String) As System.Collections.Generic.List(Of ImpUser_PermissionEntity) Implements IUser_PermissionEntity.GetUserPermissionSettingList
            Return objUserPermissionDao.GetUserPermissionSettingList(strUserID)
        End Function

        Public Function GetUserDetail(ByVal strUserName As String) As System.Collections.Generic.List(Of ImpUserEntity) Implements IUser_PermissionEntity.GetUserDetail
            Return objUserPermissionDao.GetUserDetail(strUserName)
        End Function

        Public Function InsertUserPermission(ByVal listUserPDto As System.Collections.Generic.List(Of ImpUser_PermissionEntity), ByVal intUserID As String) As Integer Implements IUser_PermissionEntity.InsertUserPermission
            Return objUserPermissionDao.InsertUserPermission(listUserPDto, intUserID)
        End Function

        Public Function UpdateUserPermission(ByVal listUserPDto As System.Collections.Generic.List(Of ImpUser_PermissionEntity)) As Integer Implements IUser_PermissionEntity.UpdateUserPermission
            Return objUserPermissionDao.UpdateUserPermission(listUserPDto)
        End Function

        Public Function DeleteUserPermission(ByVal intUserID As String) As Integer Implements IUser_PermissionEntity.DeleteUserPermission
            Return objUserPermissionDao.DeleteUserPermission(intUserID)
        End Function
#End Region

    End Class
End Namespace

