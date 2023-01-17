#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IWorkingHourService
'	Class Discription	: Interface of Working Hour  
'	Create User 		: Suwishaya L.
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
Imports System.Data
#End Region

Namespace Service
    Public Interface IWorkingHourService
        Function GetWorkingHourList(ByVal objWorkingHourDto As Dto.WorkingHourDto) As DataTable
        Function GetWorkingHourByID(ByVal intID As Integer) As Dto.WorkingHourDto
        Function GetWorkingHourReportlist(ByVal objWorkingHourDto As Dto.WorkingHourDto) As DataTable
        Function GetWorkingHourReportSearch(ByVal objWorkingHourDto As Dto.WorkingHourDto) As DataTable
        Function GetWorkingHourReport(ByVal objWorkingHourDto As Dto.WorkingHourDto) As DataTable
        Function DeleteWorkingHour(ByVal intID As Integer) As Boolean
        Function GetTableReportSearch(ByVal dtValues As DataTable) As DataTable
        Function GetTableReport(ByVal dtValues As DataTable, ByVal status As String) As DataTable
        Function IsUsedInJobOrder(ByVal strJobOrder As String) As Boolean
        Function IsFinishJobOrder(ByVal strJobOrder As String) As Boolean
        Function IsUseWorkingHour(ByVal strWorkDate As String, ByVal strStaffId As String, ByVal strStartTtime As String, ByVal strEndTime As String, Optional ByVal strDetailId As String = "") As Boolean
        Function InsertWorkingHour(ByVal objWorkingHourDto As Dto.WorkingHourDto, ByVal dtWorkHour As DataTable, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateWorkingHour(ByVal intID As Integer, ByVal dtWorkHour As DataTable, Optional ByRef strMsg As String = "") As Boolean

    End Interface
End Namespace
