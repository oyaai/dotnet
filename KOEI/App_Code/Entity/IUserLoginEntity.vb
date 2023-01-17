#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IUserLoginEntity
'	Class Discription	: Interface of table user
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

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Interface IUserLoginEntity

        Property id() As Integer
        Property user_name() As String
        Property password() As String
        Property first_name() As String
        Property last_name() As String
        Property account_next_approve() As String
        Property purchase_next_approve() As String
        Property outsource_next_approve() As String
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
        Property last_login() As String
        Property delete_fg() As Integer
        Property department_id() As String

        Property name() As String

        Function GetUserLoginList(ByVal strID As String, ByVal strUserName As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strDepartment As String) As List(Of Entity.ImpUserLoginDetailEntity)
        Function CountUsedInPO(ByVal intUserLoginID As Integer) As Integer
        Function DeleteUserLogin(ByVal intUserLoginID As Integer) As Integer
        Function CheckDupUserLogin(ByVal intUserLoginID As String, ByVal strUserName As String) As Integer
        Function InsertUserLogin(ByVal objUserLoginEnt As Entity.IUserLoginEntity) As Integer
        Function UpdateUserLogin(ByVal objUserLoginEnt As Entity.IUserLoginEntity) As Integer
        Function GetDepartmentForList() As List(Of Entity.IUserLoginEntity)
        Function GetAccount_Next_ApproveForList(ByVal id As String) As List(Of Entity.IUserLoginEntity)
        Function GetPurchase_Next_ApproveForList(ByVal id As String) As List(Of Entity.IUserLoginEntity)
        Function GetOutsource_Next_ApproveForList(ByVal id As String) As List(Of Entity.IUserLoginEntity)
        Function GetUserLoginByID(ByVal intUserLoginID As String) As Entity.IUserLoginEntity
        Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As Entity.ImpUserLoginEntity

    End Interface
End Namespace
