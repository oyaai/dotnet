#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IWorkingHourEntity
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

Namespace Entity
    Public Interface IWorkingHourEntity
#Region "Property"
        Property id() As Integer
        Property work_date() As String
        Property staff_id() As Integer
        Property work_category_id() As Integer
        Property staff_id_search() As String
        Property work_category_id_search() As String

        Property staff_name() As String
        Property category() As String
        'job order
        Property Lap0830() As String
        Property Lap0900() As String
        Property Lap1000() As String
        Property Lap1100() As String
        Property Lap1200() As String
        Property Lap1245() As String
        Property Lap1300() As String
        Property Lap1400() As String
        Property Lap1500() As String
        Property Lap1515() As String
        Property Lap1530() As String
        Property Lap1600() As String
        Property Lap1700() As String
        Property Lap1730() As String
        Property Lap1800() As String
        Property Lap1900() As String
        Property Lap2000() As String
        Property Lap2100() As String
        Property Lap2200() As String
        Property Lap2300() As String
        Property Lap0000() As String
        Property Lap0045() As String
        Property Lap0100() As String
        Property Lap0200() As String
        Property Lap0300() As String
        Property Lap0315() As String
        Property Lap0330() As String
        Property Lap0400() As String
        Property Lap0500() As String
        Property Lap0600() As String
        Property Lap0700() As String
        Property Lap0800() As String
        'detail
        Property detail_1() As String
        Property detail_2() As String
        Property detail_3() As String
        Property detail_4() As String
        Property detail_5() As String
        Property detail_6() As String
        Property detail_7() As String
        Property detail_8() As String
        Property detail_9() As String
        Property detail_10() As String
        Property detail_11() As String
        Property detail_12() As String
        Property detail_13() As String
        Property detail_14() As String
        Property detail_15() As String
        Property detail_16() As String
        Property detail_17() As String
        Property detail_18() As String
        Property detail_19() As String
        Property detail_20() As String
        Property detail_21() As String
        Property detail_22() As String
        Property detail_23() As String
        Property detail_24() As String
        Property detail_25() As String
        Property detail_26() As String
        'wh_detail_id
        Property detail_id_1() As String
        Property detail_id_2() As String
        Property detail_id_3() As String
        Property detail_id_4() As String
        Property detail_id_5() As String
        Property detail_id_6() As String
        Property detail_id_7() As String
        Property detail_id_8() As String
        Property detail_id_9() As String
        Property detail_id_10() As String
        Property detail_id_11() As String
        Property detail_id_12() As String
        Property detail_id_13() As String
        Property detail_id_14() As String
        Property detail_id_15() As String
        Property detail_id_16() As String
        Property detail_id_17() As String
        Property detail_id_18() As String
        Property detail_id_19() As String
        Property detail_id_20() As String
        Property detail_id_21() As String
        Property detail_id_22() As String
        Property detail_id_23() As String
        Property detail_id_24() As String
        Property detail_id_25() As String
        Property detail_id_26() As String

        ''Receive data for report
        Property job_status() As String
        Property start_work_date() As String
        Property end_work_date() As String
        Property work_year() As String
        Property work_month() As String
        Property period_time() As String
        Property job_order() As String
        Property finish_fg() As Integer
        Property cnt() As Integer

        Property wh_header_id() As String
        Property user_id() As String
        Property user_name() As String
        Property work_category_name() As String
        Property wh_detail_id() As String
        Property start_time() As String
        Property end_time() As String 
        Property detail() As String

#End Region

#Region "Function"
        Function GetWorkingHourList(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourByID(ByVal intID As Integer) As Entity.IWorkingHourEntity
        Function GetWorkingHourReport(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourReportSearch(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function GetWorkingHourReportList(ByVal objWorkingHourEntity As Entity.IWorkingHourEntity) As List(Of Entity.ImpWorkingHourEntity)
        Function DeleteWorkingHour(ByVal intID As Integer) As Integer
        Function ChountFinishJobOrder(ByVal strJobOrder As String) As Integer
        Function CountExitsJobOrder(ByVal strJobOrder As String) As Integer
        Function CountWorkingHour(ByVal strWorkDate As String, ByVal strStaffId As String, ByVal strStartTtime As String, ByVal strEndTime As String, Optional ByVal strDetailId As String = "") As Integer
        Function InsertWorkingHour(ByVal objJWorkingHourEnt As Entity.IWorkingHourEntity, ByVal dtWorkHour As System.Data.DataTable) As Integer
        Function UpdateWorkingHour(ByVal intID As Integer, ByVal dtWorkHour As System.Data.DataTable) As Integer
#End Region

    End Interface
End Namespace
