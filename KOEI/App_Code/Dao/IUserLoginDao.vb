#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IUserLoginDao
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

Imports Microsoft.VisualBasic

Namespace Dao

    Public Interface IUserLoginDao

        Function GetUserLoginList(ByVal strID As String, ByVal strUserName As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strDepartment As String) As List(Of Entity.ImpUserLoginDetailEntity)
        Function CountUsedInPO(ByVal intUserLoginID As Integer) As Integer
        Function DeleteUserLogin(ByVal intUserLoginID As Integer) As Integer
        Function CheckDupUserLogin(ByVal intUserLoginID As String, ByVal strUserName As String) As Integer
        Function InsertUserLogin(ByVal objUserLoginEnt As Entity.IUserLoginEntity) As Integer
        Function UpdateUserLogin(ByVal objUserLoginEnt As Entity.IUserLoginEntity) As Integer
        Function GetDepartmentForList() As System.Collections.Generic.List(Of Entity.IUserLoginEntity)
        Function GetAccount_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity)
        Function GetPurchase_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity)
        Function GetOutsource_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Entity.IUserLoginEntity)
        Function GetUserLoginByID(ByVal intUserLoginID As String) As Entity.IUserLoginEntity
        Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As Entity.ImpUserLoginEntity
    End Interface
End Namespace