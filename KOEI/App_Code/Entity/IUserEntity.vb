#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IUserEntity
'	Class Discription	: Interface of table user
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
    Public Interface IUserEntity
        Property id() As Integer
        Property user_name() As String
        Property password() As String
        Property first_name() As String
        Property last_name() As String
        Property account_next_approve() As Integer
        Property purchase_next_approve() As Integer
        Property outsource_next_approve() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
        Property last_login() As String
        Property delete_fg() As Integer
        Property department_id() As Integer

        ' Add by Komsan L. 2013-05-20 (for KTOV01)
        Property department_name() As String
        ' End

        ' Add by Boon 2013-07-10 (for purchase)
        Function CheckApproveUser(ByVal objType As Enums.PurchaseTypes, ByVal intUser_id As Integer) As Integer

        Function AddUser(ByVal objUser As IUserEntity, ByRef intUser_id As Integer) As Boolean
        Function UpdateUser(ByVal objUser As IUserEntity) As Boolean
        Function DeleteUser(ByVal intUser_id As Integer) As Boolean
        Function SearchUser(ByVal intUser_id As Integer) As IUserEntity

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

