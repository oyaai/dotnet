#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpWorkingHourEntity
'	Class Discription	: Class of table Working Hour  
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
    Public Class ImpWorkingHourEntity
        Implements IWorkingHourEntity

        Private objWorkingHour As New Dao.ImpWorkingHourDao

#Region "Fields"
        Private _id As Integer
        Private _work_date As String
        Private _staff_id As Integer
        Private _work_category_id As Integer
        Private _staff_id_search As String
        Private _work_category_id_search As String

        Private _staff_name As String
        Private _category As String

        'job order
        Private _Lap0830 As String
        Private _Lap0900 As String
        Private _Lap1000 As String
        Private _Lap1100 As String
        Private _Lap1200 As String
        Private _Lap1245 As String
        Private _Lap1300 As String
        Private _Lap1400 As String
        Private _Lap1500 As String
        Private _Lap1515 As String
        Private _Lap1530 As String
        Private _Lap1600 As String
        Private _Lap1700 As String
        Private _Lap1730 As String
        Private _Lap1800 As String
        Private _Lap1900 As String
        Private _Lap2000 As String
        Private _Lap2100 As String
        Private _Lap2200 As String
        Private _Lap2300 As String
        Private _Lap0000 As String
        Private _Lap0045 As String
        Private _Lap0100 As String
        Private _Lap0200 As String
        Private _Lap0300 As String
        Private _Lap0315 As String
        Private _Lap0330 As String
        Private _Lap0400 As String
        Private _Lap0500 As String
        Private _Lap0600 As String
        Private _Lap0700 As String
        Private _Lap0800 As String
        'detail
        Private _detail_1 As String
        Private _detail_2 As String
        Private _detail_3 As String
        Private _detail_4 As String
        Private _detail_5 As String
        Private _detail_6 As String
        Private _detail_7 As String
        Private _detail_8 As String
        Private _detail_9 As String
        Private _detail_10 As String
        Private _detail_11 As String
        Private _detail_12 As String
        Private _detail_13 As String
        Private _detail_14 As String
        Private _detail_15 As String
        Private _detail_16 As String
        Private _detail_17 As String
        Private _detail_18 As String
        Private _detail_19 As String
        Private _detail_20 As String
        Private _detail_21 As String
        Private _detail_22 As String
        Private _detail_23 As String
        Private _detail_24 As String
        Private _detail_25 As String
        Private _detail_26 As String
        'wh_detail_id
        Private _detail_id_1 As String
        Private _detail_id_2 As String
        Private _detail_id_3 As String
        Private _detail_id_4 As String
        Private _detail_id_5 As String
        Private _detail_id_6 As String
        Private _detail_id_7 As String
        Private _detail_id_8 As String
        Private _detail_id_9 As String
        Private _detail_id_10 As String
        Private _detail_id_11 As String
        Private _detail_id_12 As String
        Private _detail_id_13 As String
        Private _detail_id_14 As String
        Private _detail_id_15 As String
        Private _detail_id_16 As String
        Private _detail_id_17 As String
        Private _detail_id_18 As String
        Private _detail_id_19 As String
        Private _detail_id_20 As String
        Private _detail_id_21 As String
        Private _detail_id_22 As String
        Private _detail_id_23 As String
        Private _detail_id_24 As String
        Private _detail_id_25 As String
        Private _detail_id_26 As String

        Private _job_status As String
        Private _start_work_date As String
        Private _end_work_date As String
        Private _work_year As String
        Private _work_month As String
        Private _period_time As String
        Private _job_order As String
        Private _finish_fg As Integer
        Private _cnt As Integer

        Private _wh_header_id As String
        Private _user_id As String
        Private _user_name As String
        Private _work_category_name As String
        Private _wh_detail_id As String
        Private _start_time As String
        Private _end_time As String
        Private _detail As String
#End Region

#Region "Function"
        Public Function GetWorkingHourList(ByVal objWorkingHourEntity As IWorkingHourEntity) As System.Collections.Generic.List(Of ImpWorkingHourEntity) Implements IWorkingHourEntity.GetWorkingHourList
            Return objWorkingHour.GetWorkingHourList(objWorkingHourEntity)
        End Function

        Public Function GetWorkingHourReport(ByVal objWorkingHourEntity As IWorkingHourEntity) As System.Collections.Generic.List(Of ImpWorkingHourEntity) Implements IWorkingHourEntity.GetWorkingHourReport
            Return objWorkingHour.GetWorkingHourReport(objWorkingHourEntity)
        End Function

        Public Function DeleteWorkingHour(ByVal intID As Integer) As Integer Implements IWorkingHourEntity.DeleteWorkingHour
            Return objWorkingHour.DeleteWorkingHour(intID)
        End Function

        Public Function GetWorkingHourByID(ByVal intID As Integer) As IWorkingHourEntity Implements IWorkingHourEntity.GetWorkingHourByID
            Return objWorkingHour.GetWorkingHourByID(intID)
        End Function

        Public Function GetWorkingHourReportList(ByVal objWorkingHourEntity As IWorkingHourEntity) As System.Collections.Generic.List(Of ImpWorkingHourEntity) Implements IWorkingHourEntity.GetWorkingHourReportList
            Return objWorkingHour.GetWorkingHourReportList(objWorkingHourEntity)
        End Function

        Public Function GetWorkingHourReportSearch(ByVal objWorkingHourEntity As IWorkingHourEntity) As System.Collections.Generic.List(Of ImpWorkingHourEntity) Implements IWorkingHourEntity.GetWorkingHourReportSearch
            Return objWorkingHour.GetWorkingHourReportSearch(objWorkingHourEntity)
        End Function

        Public Function ChountFinishJobOrder(ByVal strJobOrder As String) As Integer Implements IWorkingHourEntity.ChountFinishJobOrder
            Return objWorkingHour.ChountFinishJobOrder(strJobOrder)
        End Function

        Public Function CountExitsJobOrder(ByVal strJobOrder As String) As Integer Implements IWorkingHourEntity.CountExitsJobOrder
            Return objWorkingHour.CountExitsJobOrder(strJobOrder)
        End Function

        Public Function InsertWorkingHour(ByVal objJWorkingHourEnt As IWorkingHourEntity, ByVal dtWorkHour As System.Data.DataTable) As Integer Implements IWorkingHourEntity.InsertWorkingHour
            Return objWorkingHour.InsertWorkingHour(objJWorkingHourEnt, dtWorkHour)
        End Function

        Public Function UpdateWorkingHour(ByVal intID As Integer, ByVal dtWorkHour As System.Data.DataTable) As Integer Implements IWorkingHourEntity.UpdateWorkingHour
            Return objWorkingHour.UpdateWorkingHour(intID, dtWorkHour)
        End Function

        Public Function CountWorkingHour(ByVal strWorkDate As String, ByVal strStaffId As String, ByVal strStartTtime As String, ByVal strEndTime As String, Optional ByVal strDetailId As String = "") As Integer Implements IWorkingHourEntity.CountWorkingHour
            Return objWorkingHour.CountWorkingHour(strWorkDate, strStaffId, strStartTtime, strEndTime, strDetailId)
        End Function

#End Region

#Region "Property"
        Public Property category() As String Implements IWorkingHourEntity.category
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property

        Public Property id() As Integer Implements IWorkingHourEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Lap0000() As String Implements IWorkingHourEntity.Lap0000
            Get
                Return _Lap0000
            End Get
            Set(ByVal value As String)
                _Lap0000 = value
            End Set
        End Property

        Public Property Lap0045() As String Implements IWorkingHourEntity.Lap0045
            Get
                Return _Lap0045
            End Get
            Set(ByVal value As String)
                _Lap0045 = value
            End Set
        End Property

        Public Property Lap0100() As String Implements IWorkingHourEntity.Lap0100
            Get
                Return _Lap0100
            End Get
            Set(ByVal value As String)
                _Lap0100 = value
            End Set
        End Property

        Public Property Lap0200() As String Implements IWorkingHourEntity.Lap0200
            Get
                Return _Lap0200
            End Get
            Set(ByVal value As String)
                _Lap0200 = value
            End Set
        End Property

        Public Property Lap0300() As String Implements IWorkingHourEntity.Lap0300
            Get
                Return _Lap0300
            End Get
            Set(ByVal value As String)
                _Lap0300 = value
            End Set
        End Property

        Public Property Lap0315() As String Implements IWorkingHourEntity.Lap0315
            Get
                Return _Lap0315
            End Get
            Set(ByVal value As String)
                _Lap0315 = value
            End Set
        End Property

        Public Property Lap0330() As String Implements IWorkingHourEntity.Lap0330
            Get
                Return _Lap0330
            End Get
            Set(ByVal value As String)
                _Lap0330 = value
            End Set
        End Property

        Public Property Lap0400() As String Implements IWorkingHourEntity.Lap0400
            Get
                Return _Lap0400
            End Get
            Set(ByVal value As String)
                _Lap0400 = value
            End Set
        End Property

        Public Property Lap0500() As String Implements IWorkingHourEntity.Lap0500
            Get
                Return _Lap0500
            End Get
            Set(ByVal value As String)
                _Lap0500 = value
            End Set
        End Property

        Public Property Lap0600() As String Implements IWorkingHourEntity.Lap0600
            Get
                Return _Lap0600
            End Get
            Set(ByVal value As String)
                _Lap0600 = value
            End Set
        End Property

        Public Property Lap0700() As String Implements IWorkingHourEntity.Lap0700
            Get
                Return _Lap0700
            End Get
            Set(ByVal value As String)
                _Lap0700 = value
            End Set
        End Property

        Public Property Lap0800() As String Implements IWorkingHourEntity.Lap0800
            Get
                Return _Lap0800
            End Get
            Set(ByVal value As String)
                _Lap0800 = value
            End Set
        End Property

        Public Property Lap0830() As String Implements IWorkingHourEntity.Lap0830
            Get
                Return _Lap0830
            End Get
            Set(ByVal value As String)
                _Lap0830 = value
            End Set
        End Property

        Public Property Lap0900() As String Implements IWorkingHourEntity.Lap0900
            Get
                Return _Lap0900
            End Get
            Set(ByVal value As String)
                _Lap0900 = value
            End Set
        End Property

        Public Property Lap1000() As String Implements IWorkingHourEntity.Lap1000
            Get
                Return _Lap1000
            End Get
            Set(ByVal value As String)
                _Lap1000 = value
            End Set
        End Property

        Public Property Lap1100() As String Implements IWorkingHourEntity.Lap1100
            Get
                Return _Lap1100
            End Get
            Set(ByVal value As String)
                _Lap1100 = value
            End Set
        End Property

        Public Property Lap1200() As String Implements IWorkingHourEntity.Lap1200
            Get
                Return _Lap1200
            End Get
            Set(ByVal value As String)
                _Lap1200 = value
            End Set
        End Property

        Public Property Lap1245() As String Implements IWorkingHourEntity.Lap1245
            Get
                Return _Lap1245
            End Get
            Set(ByVal value As String)
                _Lap1245 = value
            End Set
        End Property

        Public Property Lap1300() As String Implements IWorkingHourEntity.Lap1300
            Get
                Return _Lap1300
            End Get
            Set(ByVal value As String)
                _Lap1300 = value
            End Set
        End Property

        Public Property Lap1400() As String Implements IWorkingHourEntity.Lap1400
            Get
                Return _Lap1400
            End Get
            Set(ByVal value As String)
                _Lap1400 = value
            End Set
        End Property

        Public Property Lap1500() As String Implements IWorkingHourEntity.Lap1500
            Get
                Return _Lap1500
            End Get
            Set(ByVal value As String)
                _Lap1500 = value
            End Set
        End Property

        Public Property Lap1515() As String Implements IWorkingHourEntity.Lap1515
            Get
                Return _Lap1515
            End Get
            Set(ByVal value As String)
                _Lap1515 = value
            End Set
        End Property

        Public Property Lap1530() As String Implements IWorkingHourEntity.Lap1530
            Get
                Return _Lap1530
            End Get
            Set(ByVal value As String)
                _Lap1530 = value
            End Set
        End Property

        Public Property Lap1600() As String Implements IWorkingHourEntity.Lap1600
            Get
                Return _Lap1600
            End Get
            Set(ByVal value As String)
                _Lap1600 = value
            End Set
        End Property

        Public Property Lap1700() As String Implements IWorkingHourEntity.Lap1700
            Get
                Return _Lap1700
            End Get
            Set(ByVal value As String)
                _Lap1700 = value
            End Set
        End Property

        Public Property Lap1730() As String Implements IWorkingHourEntity.Lap1730
            Get
                Return _Lap1730
            End Get
            Set(ByVal value As String)
                _Lap1730 = value
            End Set
        End Property

        Public Property Lap1800() As String Implements IWorkingHourEntity.Lap1800
            Get
                Return _Lap1800
            End Get
            Set(ByVal value As String)
                _Lap1800 = value
            End Set
        End Property

        Public Property Lap1900() As String Implements IWorkingHourEntity.Lap1900
            Get
                Return _Lap1900
            End Get
            Set(ByVal value As String)
                _Lap1900 = value
            End Set
        End Property

        Public Property Lap2000() As String Implements IWorkingHourEntity.Lap2000
            Get
                Return _Lap2000
            End Get
            Set(ByVal value As String)
                _Lap2000 = value
            End Set
        End Property

        Public Property Lap2100() As String Implements IWorkingHourEntity.Lap2100
            Get
                Return _Lap2100
            End Get
            Set(ByVal value As String)
                _Lap2100 = value
            End Set
        End Property

        Public Property Lap2200() As String Implements IWorkingHourEntity.Lap2200
            Get
                Return _Lap2200
            End Get
            Set(ByVal value As String)
                _Lap2200 = value
            End Set
        End Property

        Public Property Lap2300() As String Implements IWorkingHourEntity.Lap2300
            Get
                Return _Lap2300
            End Get
            Set(ByVal value As String)
                _Lap2300 = value
            End Set
        End Property

        Public Property staff_id() As Integer Implements IWorkingHourEntity.staff_id
            Get
                Return _staff_id
            End Get
            Set(ByVal value As Integer)
                _staff_id = value
            End Set
        End Property

        Public Property staff_name() As String Implements IWorkingHourEntity.staff_name
            Get
                Return _staff_name
            End Get
            Set(ByVal value As String)
                _staff_name = value
            End Set
        End Property

        Public Property work_category_id() As Integer Implements IWorkingHourEntity.work_category_id
            Get
                Return _work_category_id
            End Get
            Set(ByVal value As Integer)
                _work_category_id = value
            End Set
        End Property

        Public Property work_date() As String Implements IWorkingHourEntity.work_date
            Get
                Return _work_date
            End Get
            Set(ByVal value As String)
                _work_date = value
            End Set
        End Property

        Public Property start_work_date() As String Implements IWorkingHourEntity.start_work_date
            Get
                Return _start_work_date
            End Get
            Set(ByVal value As String)
                _start_work_date = value
            End Set
        End Property

        Public Property end_work_date() As String Implements IWorkingHourEntity.end_work_date
            Get
                Return _end_work_date
            End Get
            Set(ByVal value As String)
                _end_work_date = value
            End Set
        End Property

        Public Property work_year() As String Implements IWorkingHourEntity.work_year
            Get
                Return _work_year
            End Get
            Set(ByVal value As String)
                _work_year = value
            End Set
        End Property

        Public Property work_month() As String Implements IWorkingHourEntity.work_month
            Get
                Return _work_month
            End Get
            Set(ByVal value As String)
                _work_month = value
            End Set
        End Property

        Public Property period_time() As String Implements IWorkingHourEntity.period_time
            Get
                Return _period_time
            End Get
            Set(ByVal value As String)
                _period_time = value
            End Set
        End Property

        Public Property job_order() As String Implements IWorkingHourEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property finish_fg() As Integer Implements IWorkingHourEntity.finish_fg
            Get
                Return _finish_fg
            End Get
            Set(ByVal value As Integer)
                _finish_fg = value
            End Set
        End Property

        Public Property cnt() As Integer Implements IWorkingHourEntity.cnt
            Get
                Return _cnt
            End Get
            Set(ByVal value As Integer)
                _cnt = value
            End Set
        End Property

        Public Property job_status() As String Implements IWorkingHourEntity.job_status
            Get
                Return _job_status
            End Get
            Set(ByVal value As String)
                _job_status = value
            End Set
        End Property

        Public Property staff_id_search() As String Implements IWorkingHourEntity.staff_id_search
            Get
                Return _staff_id_search
            End Get
            Set(ByVal value As String)
                _staff_id_search = value
            End Set
        End Property

        Public Property work_category_id_search() As String Implements IWorkingHourEntity.work_category_id_search
            Get
                Return _work_category_id_search
            End Get
            Set(ByVal value As String)
                _work_category_id_search = value
            End Set
        End Property

        Public Property detail() As String Implements IWorkingHourEntity.detail
            Get
                Return _detail
            End Get
            Set(ByVal value As String)
                _detail = value
            End Set
        End Property

        Public Property end_time() As String Implements IWorkingHourEntity.end_time
            Get
                Return _end_time
            End Get
            Set(ByVal value As String)
                _end_time = value
            End Set
        End Property

        Public Property start_time() As String Implements IWorkingHourEntity.start_time
            Get
                Return _start_time
            End Get
            Set(ByVal value As String)
                _start_time = value
            End Set
        End Property

        Public Property user_id() As String Implements IWorkingHourEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As String)
                _user_id = value
            End Set
        End Property

        Public Property user_name() As String Implements IWorkingHourEntity.user_name
            Get
                Return _user_name
            End Get
            Set(ByVal value As String)
                _user_name = value
            End Set
        End Property

        Public Property wh_detail_id() As String Implements IWorkingHourEntity.wh_detail_id
            Get
                Return _wh_detail_id
            End Get
            Set(ByVal value As String)
                _wh_detail_id = value
            End Set
        End Property

        Public Property wh_header_id() As String Implements IWorkingHourEntity.wh_header_id
            Get
                Return _wh_header_id
            End Get
            Set(ByVal value As String)
                _wh_header_id = value
            End Set
        End Property

        Public Property work_category_name() As String Implements IWorkingHourEntity.work_category_name
            Get
                Return _work_category_name
            End Get
            Set(ByVal value As String)
                _work_category_name = value
            End Set
        End Property

        Public Property detail_1() As String Implements IWorkingHourEntity.detail_1
            Get
                Return _detail_1
            End Get
            Set(ByVal value As String)
                _detail_1 = value
            End Set
        End Property

        Public Property detail_10() As String Implements IWorkingHourEntity.detail_10
            Get
                Return _detail_10
            End Get
            Set(ByVal value As String)
                _detail_10 = value
            End Set
        End Property

        Public Property detail_11() As String Implements IWorkingHourEntity.detail_11
            Get
                Return _detail_11
            End Get
            Set(ByVal value As String)
                _detail_11 = value
            End Set
        End Property

        Public Property detail_12() As String Implements IWorkingHourEntity.detail_12
            Get
                Return _detail_12
            End Get
            Set(ByVal value As String)
                _detail_12 = value
            End Set
        End Property

        Public Property detail_13() As String Implements IWorkingHourEntity.detail_13
            Get
                Return _detail_13
            End Get
            Set(ByVal value As String)
                _detail_13 = value
            End Set
        End Property

        Public Property detail_14() As String Implements IWorkingHourEntity.detail_14
            Get
                Return _detail_14
            End Get
            Set(ByVal value As String)
                _detail_14 = value
            End Set
        End Property

        Public Property detail_15() As String Implements IWorkingHourEntity.detail_15
            Get
                Return _detail_15
            End Get
            Set(ByVal value As String)
                _detail_15 = value
            End Set
        End Property

        Public Property detail_16() As String Implements IWorkingHourEntity.detail_16
            Get
                Return _detail_16
            End Get
            Set(ByVal value As String)
                _detail_16 = value
            End Set
        End Property

        Public Property detail_17() As String Implements IWorkingHourEntity.detail_17
            Get
                Return _detail_17
            End Get
            Set(ByVal value As String)
                _detail_17 = value
            End Set
        End Property

        Public Property detail_18() As String Implements IWorkingHourEntity.detail_18
            Get
                Return _detail_18
            End Get
            Set(ByVal value As String)
                _detail_18 = value
            End Set
        End Property

        Public Property detail_19() As String Implements IWorkingHourEntity.detail_19
            Get
                Return _detail_19
            End Get
            Set(ByVal value As String)
                _detail_19 = value
            End Set
        End Property

        Public Property detail_2() As String Implements IWorkingHourEntity.detail_2
            Get
                Return _detail_2
            End Get
            Set(ByVal value As String)
                _detail_2 = value
            End Set
        End Property

        Public Property detail_20() As String Implements IWorkingHourEntity.detail_20
            Get
                Return _detail_20
            End Get
            Set(ByVal value As String)
                _detail_20 = value
            End Set
        End Property

        Public Property detail_21() As String Implements IWorkingHourEntity.detail_21
            Get
                Return _detail_21
            End Get
            Set(ByVal value As String)
                _detail_21 = value
            End Set
        End Property

        Public Property detail_22() As String Implements IWorkingHourEntity.detail_22
            Get
                Return _detail_22
            End Get
            Set(ByVal value As String)
                _detail_22 = value
            End Set
        End Property

        Public Property detail_23() As String Implements IWorkingHourEntity.detail_23
            Get
                Return _detail_23
            End Get
            Set(ByVal value As String)
                _detail_23 = value
            End Set
        End Property

        Public Property detail_24() As String Implements IWorkingHourEntity.detail_24
            Get
                Return _detail_24
            End Get
            Set(ByVal value As String)
                _detail_24 = value
            End Set
        End Property

        Public Property detail_25() As String Implements IWorkingHourEntity.detail_25
            Get
                Return _detail_25
            End Get
            Set(ByVal value As String)
                _detail_25 = value
            End Set
        End Property

        Public Property detail_26() As String Implements IWorkingHourEntity.detail_26
            Get
                Return _detail_26
            End Get
            Set(ByVal value As String)
                _detail_26 = value
            End Set
        End Property

        Public Property detail_3() As String Implements IWorkingHourEntity.detail_3
            Get
                Return _detail_3
            End Get
            Set(ByVal value As String)
                _detail_3 = value
            End Set
        End Property

        Public Property detail_4() As String Implements IWorkingHourEntity.detail_4
            Get
                Return _detail_4
            End Get
            Set(ByVal value As String)
                _detail_4 = value
            End Set
        End Property

        Public Property detail_5() As String Implements IWorkingHourEntity.detail_5
            Get
                Return _detail_5
            End Get
            Set(ByVal value As String)
                _detail_5 = value
            End Set
        End Property

        Public Property detail_6() As String Implements IWorkingHourEntity.detail_6
            Get
                Return _detail_6
            End Get
            Set(ByVal value As String)
                _detail_6 = value
            End Set
        End Property

        Public Property detail_7() As String Implements IWorkingHourEntity.detail_7
            Get
                Return _detail_7
            End Get
            Set(ByVal value As String)
                _detail_7 = value
            End Set
        End Property

        Public Property detail_8() As String Implements IWorkingHourEntity.detail_8
            Get
                Return _detail_8
            End Get
            Set(ByVal value As String)
                _detail_8 = value
            End Set
        End Property

        Public Property detail_9() As String Implements IWorkingHourEntity.detail_9
            Get
                Return _detail_9
            End Get
            Set(ByVal value As String)
                _detail_9 = value
            End Set
        End Property

        Public Property detail_id_1() As String Implements IWorkingHourEntity.detail_id_1
            Get
                Return _detail_id_1
            End Get
            Set(ByVal value As String)
                _detail_id_1 = value
            End Set
        End Property

        Public Property detail_id_10() As String Implements IWorkingHourEntity.detail_id_10
            Get
                Return _detail_id_10
            End Get
            Set(ByVal value As String)
                _detail_id_10 = value
            End Set
        End Property

        Public Property detail_id_11() As String Implements IWorkingHourEntity.detail_id_11
            Get
                Return _detail_id_11
            End Get
            Set(ByVal value As String)
                _detail_id_11 = value
            End Set
        End Property

        Public Property detail_id_12() As String Implements IWorkingHourEntity.detail_id_12
            Get
                Return _detail_id_12
            End Get
            Set(ByVal value As String)
                _detail_id_12 = value
            End Set
        End Property

        Public Property detail_id_13() As String Implements IWorkingHourEntity.detail_id_13
            Get
                Return _detail_id_13
            End Get
            Set(ByVal value As String)
                _detail_id_13 = value
            End Set
        End Property

        Public Property detail_id_14() As String Implements IWorkingHourEntity.detail_id_14
            Get
                Return _detail_id_14
            End Get
            Set(ByVal value As String)
                _detail_id_14 = value
            End Set
        End Property

        Public Property detail_id_15() As String Implements IWorkingHourEntity.detail_id_15
            Get
                Return _detail_id_15
            End Get
            Set(ByVal value As String)
                _detail_id_15 = value
            End Set
        End Property

        Public Property detail_id_16() As String Implements IWorkingHourEntity.detail_id_16
            Get
                Return _detail_id_16
            End Get
            Set(ByVal value As String)
                _detail_id_16 = value
            End Set
        End Property

        Public Property detail_id_17() As String Implements IWorkingHourEntity.detail_id_17
            Get
                Return _detail_id_17
            End Get
            Set(ByVal value As String)
                _detail_id_17 = value
            End Set
        End Property

        Public Property detail_id_18() As String Implements IWorkingHourEntity.detail_id_18
            Get
                Return _detail_id_18
            End Get
            Set(ByVal value As String)
                _detail_id_18 = value
            End Set
        End Property

        Public Property detail_id_19() As String Implements IWorkingHourEntity.detail_id_19
            Get
                Return _detail_id_19
            End Get
            Set(ByVal value As String)
                _detail_id_19 = value
            End Set
        End Property

        Public Property detail_id_2() As String Implements IWorkingHourEntity.detail_id_2
            Get
                Return _detail_id_2
            End Get
            Set(ByVal value As String)
                _detail_id_2 = value
            End Set
        End Property

        Public Property detail_id_20() As String Implements IWorkingHourEntity.detail_id_20
            Get
                Return _detail_id_20
            End Get
            Set(ByVal value As String)
                _detail_id_20 = value
            End Set
        End Property

        Public Property detail_id_21() As String Implements IWorkingHourEntity.detail_id_21
            Get
                Return _detail_id_21
            End Get
            Set(ByVal value As String)
                _detail_id_21 = value
            End Set
        End Property

        Public Property detail_id_22() As String Implements IWorkingHourEntity.detail_id_22
            Get
                Return _detail_id_22
            End Get
            Set(ByVal value As String)
                _detail_id_22 = value
            End Set
        End Property

        Public Property detail_id_23() As String Implements IWorkingHourEntity.detail_id_23
            Get
                Return _detail_id_23
            End Get
            Set(ByVal value As String)
                _detail_id_23 = value
            End Set
        End Property

        Public Property detail_id_24() As String Implements IWorkingHourEntity.detail_id_24
            Get
                Return _detail_id_24
            End Get
            Set(ByVal value As String)
                _detail_id_24 = value
            End Set
        End Property

        Public Property detail_id_25() As String Implements IWorkingHourEntity.detail_id_25
            Get
                Return _detail_id_25
            End Get
            Set(ByVal value As String)
                _detail_id_25 = value
            End Set
        End Property

        Public Property detail_id_26() As String Implements IWorkingHourEntity.detail_id_26
            Get
                Return _detail_id_26
            End Get
            Set(ByVal value As String)
                _detail_id_26 = value
            End Set
        End Property

        Public Property detail_id_3() As String Implements IWorkingHourEntity.detail_id_3
            Get
                Return _detail_id_3
            End Get
            Set(ByVal value As String)
                _detail_id_3 = value
            End Set
        End Property

        Public Property detail_id_4() As String Implements IWorkingHourEntity.detail_id_4
            Get
                Return _detail_id_4
            End Get
            Set(ByVal value As String)
                _detail_id_4 = value
            End Set
        End Property

        Public Property detail_id_5() As String Implements IWorkingHourEntity.detail_id_5
            Get
                Return _detail_id_5
            End Get
            Set(ByVal value As String)
                _detail_id_5 = value
            End Set
        End Property

        Public Property detail_id_6() As String Implements IWorkingHourEntity.detail_id_6
            Get
                Return _detail_id_6
            End Get
            Set(ByVal value As String)
                _detail_id_6 = value
            End Set
        End Property

        Public Property detail_id_7() As String Implements IWorkingHourEntity.detail_id_7
            Get
                Return _detail_id_7
            End Get
            Set(ByVal value As String)
                _detail_id_7 = value
            End Set
        End Property

        Public Property detail_id_8() As String Implements IWorkingHourEntity.detail_id_8
            Get
                Return _detail_id_8
            End Get
            Set(ByVal value As String)
                _detail_id_8 = value
            End Set
        End Property

        Public Property detail_id_9() As String Implements IWorkingHourEntity.detail_id_9
            Get
                Return _detail_id_9
            End Get
            Set(ByVal value As String)
                _detail_id_9 = value
            End Set
        End Property

#End Region

    End Class
End Namespace
