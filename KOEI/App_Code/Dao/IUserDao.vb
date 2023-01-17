#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IUserDao
'	Class Discription	: Interface of table user
'	Create User 		: Boon
'	Create Date		    : 15-05-2013
'
' UPDATE INFORMATION
'	Update User		: Charoon
'	Update Date		: 30-05-2013
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Dao
    Public Interface IUserDao
        Function DB_CheckApproveUser(ByVal objType As Enums.PurchaseTypes, ByVal intUser_id As Integer) As Integer
        Function DB_AddUser(ByVal objUser As Entity.IUserEntity, ByRef intUser_id As Integer) As Boolean
        Function DB_UpdateUser(ByVal objUser As Entity.IUserEntity) As Boolean
        Function DB_DeleteUser(ByVal intUser_id As Integer) As Boolean
        Function DB_SearchUser(ByVal intUser_id As Integer) As Entity.IUserEntity

        ' Add by Komsan L. 2013-05-20 (for KTOV01)
        Function GetUserLogin(ByVal strUserName As String, ByVal strPassword As String, Optional ByVal strUserId As String = "") As Entity.IUserEntity
        Function UpdateLastLogin(ByVal intUserID As Integer) As Integer
        ' Add by Charoon 2013-05-30 (for KTMS23)
        Function CheckUserByDepartment(ByVal intDepartment_id As Integer) As Integer
        ' End
        ' Add by Suwishaya L. 2013-06-17 (for Job Order)
        Function GetUserForList(Optional ByVal strDepartmentName As String = Nothing) As List(Of Entity.IUserEntity)

    End Interface
End Namespace