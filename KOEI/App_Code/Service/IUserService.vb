#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IUserService
'	Class Discription	: Interface of user Service
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

Namespace Service
    Public Interface IUserService
        Function GetUserLogin(ByVal strUserName As String, ByVal strPassword As String, Optional ByVal strUserId As String = "") As Dto.UserDto
        Function UpdateLastLogin(ByVal intUserID As Integer) As Boolean
        Function GetUserPermission(ByVal intUserID As Integer) As List(Of Common.UserPermissions.UserPermissionDto)
        ' Add by Charoon 2013-05-30 (for KTMS23)
        Function GetUserPermission(ByVal intUserID As Integer, ByVal intMenuID As Integer) As List(Of Dto.UserPermissionDto)
        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Function GetUserForList(Optional ByVal strDepartmentName As String = Nothing) As List(Of Dto.UserDto)
    End Interface
End Namespace

