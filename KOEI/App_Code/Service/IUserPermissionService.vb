#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IUserPermissionService
'	Class Discription	: Interface class UserPermission service
'	Create User 		: Wasan D.
'	Create Date		    : 08-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports System.Data
Imports Microsoft.VisualBasic

Namespace Service
    Public Interface IUserPermissionService
        Function GetUserPermissionList(ByVal strFName As String, ByVal strLName As String, ByVal strUName As String, ByVal intDepartment As String) As DataTable
        Function GetUserSetPermissionList() As List(Of Dto.UserDto)
        Function GetUserNonSetPermissionList() As List(Of Dto.UserDto)
        Function GetPermissionList() As DataTable
        Function GetUserDetail(ByVal strUserName As String) As DataTable
        Function GetUserPermissionSettingList(ByVal strUserID As String) As DataTable
        Function InsertUserPermission(ByVal listUserPDto As List(Of Dto.UserPermissionDto), ByVal IntUserID As String, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateUserPermission(ByVal listUserPDto As List(Of Dto.UserPermissionDto), Optional ByRef strMsg As String = "") As Boolean
        Function DeleteUserPermission(ByVal intUserID As String) As Boolean
    End Interface
End Namespace
