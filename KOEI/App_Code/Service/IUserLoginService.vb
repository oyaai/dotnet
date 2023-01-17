#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IUserLoginService
'	Class Discription	: Interface class UserLogin service
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
Imports System.Data

Namespace Service
    Public Interface IUserLoginService


        Function GetUserLoginList(ByVal strID As String, ByVal strUserName As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strDepartment As String) As DataTable
        Function IsUsedInPO(ByVal intUserLoginID As Integer) As Boolean
        Function DeleteUserLogin(ByVal intUserLoginID As Integer) As Boolean
        Function CheckDupUserLogin(ByVal intUserLoginID As String, ByVal strUserName As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function InsertUserLogin(ByVal objUserLoginDto As Dto.UserLoginDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateUserLogin(ByVal objUserLoginDto As Dto.UserLoginDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetDepartmentForList() As List(Of Dto.UserLoginDto)
        Function GetAccount_Next_ApproveForList(ByVal id As String) As List(Of Dto.UserLoginDto)
        Function GetPurchase_Next_ApproveForList(ByVal id As String) As List(Of Dto.UserLoginDto)
        Function GetOutsource_Next_ApproveForList(ByVal id As String) As List(Of Dto.UserLoginDto)
        Function GetUserLoginByID(ByVal intUserLoginID As String) As Dto.UserLoginDto
        Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As Dto.UserLoginDto

    End Interface
End Namespace
