#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IWorkingHourDao
'	Class Discription	: Interface of table Working Hour
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
#End Region

Namespace Dao
    Public Interface IWorkingHourDao
        Function GetWorkingHourList(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourByID(ByVal intId As Integer) As Entity.IWorkingHourEntity
        Function GetWorkingHourReport(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourReportSearch(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourReportList(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function DeleteWorkingHour(ByVal intID As Integer) As Integer
        Function ChountFinishJobOrder(ByVal strJobOrder As String) As Integer
        Function CountExitsJobOrder(ByVal strJobOrder As String) As Integer
        Function CountWorkingHour(ByVal strWorkDate As String, ByVal strStaffId As String, ByVal strStartTtime As String, ByVal strEndTime As String, Optional ByVal strDetailId As String = "") As Integer
        Function InsertWorkingHour(ByVal objWorkingHourEnt As Entity.ImpWorkingHourEntity, ByVal dtWorkHour As System.Data.DataTable) As Integer
        Function UpdateWorkingHour(ByVal intID As Integer, ByVal dtWorkHour As System.Data.DataTable) As Integer
    End Interface
End Namespace
