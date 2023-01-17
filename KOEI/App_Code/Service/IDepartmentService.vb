#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IDepartmentService
'	Class Discription	: Interface of Department
'	Create User 		: Charoon
'	Create Date		    : 30-05-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Data
#End Region

Namespace Service
    Public Interface IDepartmentService
        Function GetDepartmentList(ByVal strDepartmentName As String) As DataTable
        Function DeleteDepartment(ByVal intDepartmentID As Integer) As Boolean
        Function GetDepartmentByID(ByVal intDepartmentID As Integer) As Dto.DepartmentDto
        Function InsertDepartment(ByVal objDepartmentDto As Dto.DepartmentDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateDepartment(ByVal objDepartmentDto As Dto.DepartmentDto, Optional ByRef strMsg As String = "") As Boolean
        Function CheckDupDepartment(ByVal strDepartmentName As String, Optional ByVal intDepartmentID As Integer = 0) As Boolean
        Function CheckDepartmentForDel(ByVal intDepartmentId As Integer) As Boolean
        Function SaveDepartment(ByVal objDepartmentDto As Dto.DepartmentDto, ByVal strMode As String) As Boolean

        'add for Dropdownlist By Wasan D. On 08-07-2013
        Function GetDepartmentForDDList() As List(Of Dto.DepartmentDto)

    End Interface
End Namespace