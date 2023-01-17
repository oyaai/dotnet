#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IUser_PermissionEntity
'	Class Discription	: Interface of table user_permission
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

Imports System.Data
Imports Microsoft.VisualBasic

Namespace Entity
    Public Interface IUser_PermissionEntity
        Property user_id() As Integer
        Property menu_id() As Integer
        Property fn_create() As Integer
        Property fn_update() As Integer
        Property fn_delete() As Integer
        Property fn_list() As Integer
        Property fn_confirm() As Integer
        Property fn_approve() As Integer
        Property fn_amount() As Integer

        Property id() As Integer
        Property value() As String
        Property priority() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_bydate() As String
        Property category_id() As Integer
        Property menu_text() As String


        Function GetUserPermission(ByVal intUserID As Integer) As List(Of Entity.IUser_PermissionEntity)
        ' Add by Charoon 2013-05-30 (for KTMS23)
        Function GetUserPermission(ByVal intUserID As Integer, ByVal intMenuID As Integer) As List(Of Entity.IUser_PermissionEntity)

        ' Add by Wasan 08-07-2013 (for KTAD03)
        Function GetUserPermissionList(ByVal strFName As String, ByVal strLName As String, ByVal strUName As String, ByVal intDepartment As String) As List(Of Entity.ImpUserEntity)
        Function GetUserSetPermissionList() As List(Of Entity.ImpUserEntity)
        Function GetUserNonSetPermissionList() As List(Of Entity.ImpUserEntity)
        Function GetPermissionList() As List(Of Entity.ImpUser_PermissionEntity)
        Function GetUserDetail(ByVal strUserName As String) As List(Of Entity.ImpUserEntity)
        Function GetUserPermissionSettingList(ByVal strUserID As String) As List(Of Entity.ImpUser_PermissionEntity)
        Function InsertUserPermission(ByVal listUserPDto As List(Of Entity.ImpUser_PermissionEntity), ByVal intUserID As String) As Integer
        Function UpdateUserPermission(ByVal listUserPDto As List(Of Entity.ImpUser_PermissionEntity)) As Integer
        Function DeleteUserPermission(ByVal intUserID As String) As Integer
    End Interface
End Namespace

