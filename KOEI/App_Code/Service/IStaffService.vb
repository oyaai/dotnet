
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IStaffService
'	Class Discription	: Interface class Staff service
'	Create User 		: Nisa S.
'	Create Date		    : 04-07-2013
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
    Public Interface IStaffService

        Function GetStaffList(ByVal strID As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal strWorkCategoryID As String) As DataTable
        Function DeleteStaff(ByVal intStaffID As Integer) As Boolean
        Function GetStaffByID(ByVal intStaffID As Integer) As Dto.StaffDto
        Function InsertStaff(ByVal objStaffDto As Dto.StaffDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateStaff(ByVal objStaffDto As Dto.StaffDto, Optional ByRef strMsg As String = "") As Boolean
        Function IsUsedInPO(ByVal intStaffID As Integer) As Boolean
        Function CheckDupInsert(ByVal strfirst_name As String, ByVal strlast_name As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function CheckDupUpdate(ByVal intStaffID As String, ByVal strfirst_name As String, ByVal strlast_name As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean

        Function GetWorkCategoryForList() As List(Of Dto.StaffDto)
        Function GetStaffForList() As List(Of Dto.StaffDto)
    End Interface
End Namespace
