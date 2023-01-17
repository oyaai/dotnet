#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpWorkingHourService
'	Class Discription	: Implement staff Service
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
Imports MySql.Data.MySqlClient
#End Region

Namespace Service
    Public Class ImpWorkingHourService
        Implements IWorkingHourService

        Private objLog As New Common.Logs.Log
        Private objWorkingHourEnt As New Entity.ImpWorkingHourEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetWorkingHourList
        '	Discription	    : Get Working Hour list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourList( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto _
        ) As System.Data.DataTable Implements IWorkingHourService.GetWorkingHourList
            ' set default
            GetWorkingHourList = New DataTable
            Try
                ' variable for keep list
                Dim listWorkingHourEnt As New List(Of Entity.ImpWorkingHourEntity)
                ' data row object
                Dim row As DataRow
                Dim strWorkDate As String = ""

                ' call function GetWorkingHourList from entity
                listWorkingHourEnt = objWorkingHourEnt.GetWorkingHourList(SetDtoToEntity(objWorkingHourDto))

                ' assign column header
                With GetWorkingHourList
                    .Columns.Add("id")
                    .Columns.Add("work_date")
                    .Columns.Add("staff_name")
                    .Columns.Add("category")
                    .Columns.Add("Lap0830")
                    .Columns.Add("Lap0900")
                    .Columns.Add("Lap1000")
                    .Columns.Add("Lap1100")
                    .Columns.Add("Lap1200")
                    .Columns.Add("Lap1245")
                    .Columns.Add("Lap1300")
                    .Columns.Add("Lap1400")
                    .Columns.Add("Lap1500")
                    .Columns.Add("Lap1515")
                    .Columns.Add("Lap1530")
                    .Columns.Add("Lap1600")
                    .Columns.Add("Lap1700")
                    .Columns.Add("Lap1730")
                    .Columns.Add("Lap1800")
                    .Columns.Add("Lap1900")
                    .Columns.Add("Lap2000")
                    .Columns.Add("Lap2100")
                    .Columns.Add("Lap2200")
                    .Columns.Add("Lap2300")
                    .Columns.Add("Lap0000")
                    .Columns.Add("Lap0045")
                    .Columns.Add("Lap0100")
                    .Columns.Add("Lap0200")
                    .Columns.Add("Lap0300")
                    .Columns.Add("Lap0315")
                    .Columns.Add("Lap0330")
                    .Columns.Add("Lap0400")
                    .Columns.Add("Lap0500")
                    .Columns.Add("Lap0600")
                    .Columns.Add("Lap0700")
                    .Columns.Add("Lap0800")

                    ' assign row from listWorkingHourEnt
                    For Each values In listWorkingHourEnt
                        row = .NewRow

                        If values.work_date <> Nothing Or values.work_date <> "" Then
                            strWorkDate = Left(values.work_date, 4) & "/" & Mid(values.work_date, 5, 2) & "/" & Right(values.work_date, 2)
                            row("work_date") = CDate(strWorkDate).ToString("dd/MMM/yyyy")
                        Else
                            row("work_date") = values.work_date
                        End If

                        row("id") = values.id
                        row("staff_name") = values.staff_name
                        row("category") = values.category
                        row("Lap0830") = values.Lap0830
                        row("Lap0900") = values.Lap0900
                        row("Lap1000") = values.Lap1000
                        row("Lap1100") = values.Lap1100
                        row("Lap1200") = values.Lap1200
                        row("Lap1245") = values.Lap1245
                        row("Lap1300") = values.Lap1300
                        row("Lap1400") = values.Lap1400
                        row("Lap1500") = values.Lap1500
                        row("Lap1515") = values.Lap1515
                        row("Lap1530") = values.Lap1530
                        row("Lap1600") = values.Lap1600
                        row("Lap1700") = values.Lap1700
                        row("Lap1730") = values.Lap1730
                        row("Lap1800") = values.Lap1800
                        row("Lap1900") = values.Lap1900
                        row("Lap2000") = values.Lap2000
                        row("Lap2100") = values.Lap2100
                        row("Lap2200") = values.Lap2200
                        row("Lap2300") = values.Lap2300
                        row("Lap0000") = values.Lap0000
                        row("Lap0045") = values.Lap0045
                        row("Lap0100") = values.Lap0100
                        row("Lap0200") = values.Lap0200
                        row("Lap0300") = values.Lap0300
                        row("Lap0315") = values.Lap0315
                        row("Lap0330") = values.Lap0330
                        row("Lap0400") = values.Lap0400
                        row("Lap0500") = values.Lap0500
                        row("Lap0600") = values.Lap0600
                        row("Lap0700") = values.Lap0700
                        row("Lap0800") = values.Lap0800

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourReport
        '	Discription	    : Get Working Hour Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourReport( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto _
        ) As System.Data.DataTable Implements IWorkingHourService.GetWorkingHourReport
            ' set default
            GetWorkingHourReport = New DataTable
            Try
                ' variable for keep list
                Dim listWorkingHourEnt As New List(Of Entity.ImpWorkingHourEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetWorkingHourReport from entity
                listWorkingHourEnt = objWorkingHourEnt.GetWorkingHourReport(SetDtoToEntity(objWorkingHourDto))

                ' assign column header
                With GetWorkingHourReport
                    .Columns.Add("start_work_date")
                    .Columns.Add("end_work_date")
                    .Columns.Add("work_year")
                    .Columns.Add("work_month")
                    .Columns.Add("period_time")
                    .Columns.Add("job_order")
                    .Columns.Add("finish_fg")
                    .Columns.Add("cnt")

                    ' assign row from listWorkingHourEnt
                    For Each values In listWorkingHourEnt
                        row = .NewRow
                        row("start_work_date") = values.start_work_date
                        row("end_work_date") = values.end_work_date
                        row("work_year") = values.work_year
                        row("work_month") = values.work_month
                        row("period_time") = values.period_time
                        row("job_order") = values.job_order
                        row("finish_fg") = values.finish_fg
                        row("cnt") = values.cnt

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourReportSearch
        '	Discription	    : Get Working Hour Report on search screen
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourReportSearch( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto _
        ) As System.Data.DataTable Implements IWorkingHourService.GetWorkingHourReportSearch
            ' set default
            GetWorkingHourReportSearch = New DataTable
            Try
                ' variable for keep list
                Dim listWorkingHourEnt As New List(Of Entity.ImpWorkingHourEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetWorkingHourReport from entity
                listWorkingHourEnt = objWorkingHourEnt.GetWorkingHourReportSearch(SetDtoToEntity(objWorkingHourDto))

                ' assign column header
                With GetWorkingHourReportSearch
                    .Columns.Add("wh_header_id")
                    .Columns.Add("work_date")
                    .Columns.Add("user_id")
                    .Columns.Add("user_name")
                    .Columns.Add("work_category_id")
                    .Columns.Add("work_category_name")
                    .Columns.Add("wh_detail_id")
                    .Columns.Add("start_time")
                    .Columns.Add("end_time")
                    .Columns.Add("job_order")
                    .Columns.Add("detail")
                    .Columns.Add("category_name")
                    ' assign row from listWorkingHourEnt
                    For Each values In listWorkingHourEnt
                        row = .NewRow
                        row("wh_header_id") = values.wh_header_id
                        row("work_date") = values.work_date
                        row("user_id") = values.user_id
                        row("user_name") = values.user_name
                        row("work_category_id") = values.work_category_id
                        row("work_category_name") = values.work_category_name
                        row("wh_detail_id") = values.wh_detail_id
                        row("start_time") = values.start_time
                        row("end_time") = values.end_time
                        row("job_order") = values.job_order
                        row("detail") = values.detail
                        row("category_name") = values.category
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReportSearch(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto _
        ) As Entity.IWorkingHourEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpWorkingHourEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search screen 
                    .work_date = objWorkingHourDto.work_date
                    .staff_id_search = objWorkingHourDto.staff_id_search
                    .work_category_id_search = objWorkingHourDto.work_category_id_search

                    'Receive data from menagement screen 
                    .staff_id = objWorkingHourDto.staff_id
                    .work_category_id = objWorkingHourDto.work_category_id

                    'Receive data from report screen 
                    .start_work_date = objWorkingHourDto.start_work_date
                    .end_work_date = objWorkingHourDto.end_work_date
                    .job_status = objWorkingHourDto.job_status

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteWorkingHour
        '	Discription	    : Delete Working Hour
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWorkingHour( _
            ByVal intID As Integer _
        ) As Boolean Implements IWorkingHourService.DeleteWorkingHour
            ' set default return value
            DeleteWorkingHour = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteWorkingHour from job order Entity
                intEff = objWorkingHourEnt.DeleteWorkingHour(intID)

                ' check row effect
                If intEff >= 0 Then
                    ' case row more than 0 then return True
                    DeleteWorkingHour = True
                Else
                    ' case row less than 1 then return False
                    DeleteWorkingHour = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWorkingHour(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourByID
        '	Discription	    : Get Working Hour By ID
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourByID( _
            ByVal intID As Integer _
        ) As Dto.WorkingHourDto Implements IWorkingHourService.GetWorkingHourByID
            ' set default return value
            GetWorkingHourByID = New Dto.WorkingHourDto
            Try
                ' object for return value from Entity
                Dim objWorkingHourEntRet As New Entity.ImpWorkingHourEntity
                Dim strWorkDate As String
                ' call function GetWorkingHourByID from Entity
                objWorkingHourEntRet = objWorkingHourEntRet.GetWorkingHourByID(intID)

                ' assign value from Entity to Dto
                With GetWorkingHourByID

                    If objWorkingHourEntRet.work_date <> Nothing Or objWorkingHourEntRet.work_date <> "" Then
                        strWorkDate = Right(objWorkingHourEntRet.work_date, 2) & "/" & Mid(objWorkingHourEntRet.work_date, 5, 2) & "/" & Left(objWorkingHourEntRet.work_date, 4)
                        .work_date = strWorkDate
                    Else
                        .work_date = objWorkingHourEntRet.work_date
                    End If
                    'header part
                    .staff_id = objWorkingHourEntRet.staff_id
                    .staff_name = objWorkingHourEntRet.staff_name
                    .work_category_id = objWorkingHourEntRet.work_category_id
                    .category = objWorkingHourEntRet.category
                    'job order
                    .Lap0830 = objWorkingHourEntRet.Lap0830
                    .Lap0900 = objWorkingHourEntRet.Lap0900
                    .Lap1000 = objWorkingHourEntRet.Lap1000
                    .Lap1100 = objWorkingHourEntRet.Lap1100
                    .Lap1200 = objWorkingHourEntRet.Lap1200
                    .Lap1245 = objWorkingHourEntRet.Lap1245
                    .Lap1300 = objWorkingHourEntRet.Lap1300
                    .Lap1400 = objWorkingHourEntRet.Lap1400
                    .Lap1500 = objWorkingHourEntRet.Lap1500
                    .Lap1515 = objWorkingHourEntRet.Lap1515
                    .Lap1530 = objWorkingHourEntRet.Lap1530
                    .Lap1600 = objWorkingHourEntRet.Lap1600
                    .Lap1700 = objWorkingHourEntRet.Lap1700
                    .Lap1730 = objWorkingHourEntRet.Lap1730
                    .Lap1800 = objWorkingHourEntRet.Lap1800
                    .Lap1900 = objWorkingHourEntRet.Lap1900
                    .Lap2000 = objWorkingHourEntRet.Lap2000
                    .Lap2100 = objWorkingHourEntRet.Lap2100
                    .Lap2200 = objWorkingHourEntRet.Lap2200
                    .Lap2300 = objWorkingHourEntRet.Lap2300
                    .Lap0000 = objWorkingHourEntRet.Lap0000
                    .Lap0045 = objWorkingHourEntRet.Lap0045
                    .Lap0100 = objWorkingHourEntRet.Lap0100
                    .Lap0200 = objWorkingHourEntRet.Lap0200
                    .Lap0300 = objWorkingHourEntRet.Lap0300
                    .Lap0315 = objWorkingHourEntRet.Lap0315
                    .Lap0330 = objWorkingHourEntRet.Lap0330
                    .Lap0400 = objWorkingHourEntRet.Lap0400
                    .Lap0500 = objWorkingHourEntRet.Lap0500
                    .Lap0600 = objWorkingHourEntRet.Lap0600
                    .Lap0700 = objWorkingHourEntRet.Lap0700
                    .Lap0800 = objWorkingHourEntRet.Lap0800
                    'detail
                    .detail_1 = objWorkingHourEntRet.detail_1
                    .detail_2 = objWorkingHourEntRet.detail_2
                    .detail_3 = objWorkingHourEntRet.detail_3
                    .detail_4 = objWorkingHourEntRet.detail_4
                    .detail_5 = objWorkingHourEntRet.detail_5
                    .detail_6 = objWorkingHourEntRet.detail_6
                    .detail_7 = objWorkingHourEntRet.detail_7
                    .detail_8 = objWorkingHourEntRet.detail_8
                    .detail_9 = objWorkingHourEntRet.detail_9
                    .detail_10 = objWorkingHourEntRet.detail_10
                    .detail_11 = objWorkingHourEntRet.detail_11
                    .detail_12 = objWorkingHourEntRet.detail_12
                    .detail_13 = objWorkingHourEntRet.detail_13
                    .detail_14 = objWorkingHourEntRet.detail_14
                    .detail_15 = objWorkingHourEntRet.detail_15
                    .detail_16 = objWorkingHourEntRet.detail_16
                    .detail_17 = objWorkingHourEntRet.detail_17
                    .detail_18 = objWorkingHourEntRet.detail_18
                    .detail_19 = objWorkingHourEntRet.detail_19
                    .detail_20 = objWorkingHourEntRet.detail_20
                    .detail_21 = objWorkingHourEntRet.detail_21
                    .detail_22 = objWorkingHourEntRet.detail_22
                    .detail_23 = objWorkingHourEntRet.detail_23
                    .detail_24 = objWorkingHourEntRet.detail_24
                    .detail_25 = objWorkingHourEntRet.detail_25
                    .detail_26 = objWorkingHourEntRet.detail_26
                    'wh_dtail_id
                    .detail_id_1 = objWorkingHourEntRet.detail_id_1
                    .detail_id_2 = objWorkingHourEntRet.detail_id_2
                    .detail_id_3 = objWorkingHourEntRet.detail_id_3
                    .detail_id_4 = objWorkingHourEntRet.detail_id_4
                    .detail_id_5 = objWorkingHourEntRet.detail_id_5
                    .detail_id_6 = objWorkingHourEntRet.detail_id_6
                    .detail_id_7 = objWorkingHourEntRet.detail_id_7
                    .detail_id_8 = objWorkingHourEntRet.detail_id_8
                    .detail_id_9 = objWorkingHourEntRet.detail_id_9
                    .detail_id_10 = objWorkingHourEntRet.detail_id_10
                    .detail_id_11 = objWorkingHourEntRet.detail_id_11
                    .detail_id_12 = objWorkingHourEntRet.detail_id_12
                    .detail_id_13 = objWorkingHourEntRet.detail_id_13
                    .detail_id_14 = objWorkingHourEntRet.detail_id_14
                    .detail_id_15 = objWorkingHourEntRet.detail_id_15
                    .detail_id_16 = objWorkingHourEntRet.detail_id_16
                    .detail_id_17 = objWorkingHourEntRet.detail_id_17
                    .detail_id_18 = objWorkingHourEntRet.detail_id_18
                    .detail_id_19 = objWorkingHourEntRet.detail_id_19
                    .detail_id_20 = objWorkingHourEntRet.detail_id_20
                    .detail_id_21 = objWorkingHourEntRet.detail_id_21
                    .detail_id_22 = objWorkingHourEntRet.detail_id_22
                    .detail_id_23 = objWorkingHourEntRet.detail_id_23
                    .detail_id_24 = objWorkingHourEntRet.detail_id_24
                    .detail_id_25 = objWorkingHourEntRet.detail_id_25
                    .detail_id_26 = objWorkingHourEntRet.detail_id_26


                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourReportlist
        '	Discription	    : Get Working Hour Report list
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourReportlist( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto _
        ) As System.Data.DataTable Implements IWorkingHourService.GetWorkingHourReportlist
            ' set default
            GetWorkingHourReportlist = New DataTable
            Try
                ' variable for keep list
                Dim listWorkingHourEnt As New List(Of Entity.ImpWorkingHourEntity)
                ' data row object
                Dim row As DataRow
                Dim strWorkDate As String = ""

                ' call function GetWorkingHourList from entity
                listWorkingHourEnt = objWorkingHourEnt.GetWorkingHourReportList(SetDtoToEntity(objWorkingHourDto))

                ' assign column header
                With GetWorkingHourReportlist
                    .Columns.Add("id")
                    .Columns.Add("work_date")
                    .Columns.Add("staff_name")
                    .Columns.Add("category")
                    .Columns.Add("Lap0830")
                    .Columns.Add("Lap0900")
                    .Columns.Add("Lap1000")
                    .Columns.Add("Lap1100")
                    .Columns.Add("Lap1200")
                    .Columns.Add("Lap1245")
                    .Columns.Add("Lap1300")
                    .Columns.Add("Lap1400")
                    .Columns.Add("Lap1500")
                    .Columns.Add("Lap1515")
                    .Columns.Add("Lap1530")
                    .Columns.Add("Lap1600")
                    .Columns.Add("Lap1700")
                    .Columns.Add("Lap1730")
                    .Columns.Add("Lap1800")
                    .Columns.Add("Lap1900")
                    .Columns.Add("Lap2000")
                    .Columns.Add("Lap2100")
                    .Columns.Add("Lap2200")
                    .Columns.Add("Lap2300")
                    .Columns.Add("Lap0000")
                    .Columns.Add("Lap0045")
                    .Columns.Add("Lap0100")
                    .Columns.Add("Lap0200")
                    .Columns.Add("Lap0300")
                    .Columns.Add("Lap0315")
                    .Columns.Add("Lap0330")
                    .Columns.Add("Lap0400")
                    .Columns.Add("Lap0500")
                    .Columns.Add("Lap0600")
                    .Columns.Add("Lap0700")
                    .Columns.Add("Lap0800")

                    ' assign row from listWorkingHourEnt
                    For Each values In listWorkingHourEnt
                        row = .NewRow

                        If values.work_date <> Nothing Or values.work_date <> "" Then
                            strWorkDate = Left(values.work_date, 4) & "/" & Mid(values.work_date, 5, 2) & "/" & Right(values.work_date, 2)
                            row("work_date") = CDate(strWorkDate).ToString("dd/MMM/yyyy")
                        Else
                            row("work_date") = values.work_date
                        End If

                        row("id") = values.id
                        row("staff_name") = values.staff_name
                        row("category") = values.category
                        row("Lap0830") = values.Lap0830
                        row("Lap0900") = values.Lap0900
                        row("Lap1000") = values.Lap1000
                        row("Lap1100") = values.Lap1100
                        row("Lap1200") = values.Lap1200
                        row("Lap1245") = values.Lap1245
                        row("Lap1300") = values.Lap1300
                        row("Lap1400") = values.Lap1400
                        row("Lap1500") = values.Lap1500
                        row("Lap1515") = values.Lap1515
                        row("Lap1530") = values.Lap1530
                        row("Lap1600") = values.Lap1600
                        row("Lap1700") = values.Lap1700
                        row("Lap1730") = values.Lap1730
                        row("Lap1800") = values.Lap1800
                        row("Lap1900") = values.Lap1900
                        row("Lap2000") = values.Lap2000
                        row("Lap2100") = values.Lap2100
                        row("Lap2200") = values.Lap2200
                        row("Lap2300") = values.Lap2300
                        row("Lap0000") = values.Lap0000
                        row("Lap0045") = values.Lap0045
                        row("Lap0100") = values.Lap0100
                        row("Lap0200") = values.Lap0200
                        row("Lap0300") = values.Lap0300
                        row("Lap0315") = values.Lap0315
                        row("Lap0330") = values.Lap0330
                        row("Lap0400") = values.Lap0400
                        row("Lap0500") = values.Lap0500
                        row("Lap0600") = values.Lap0600
                        row("Lap0700") = values.Lap0700
                        row("Lap0800") = values.Lap0800

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReportlist(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTableReportSearch
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReportSearch( _
            ByVal dtValues As System.Data.DataTable _
        ) As System.Data.DataTable Implements IWorkingHourService.GetTableReportSearch
            ' set default return value
            GetTableReportSearch = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("wh_header_id")
                    .Columns.Add("work_date")
                    .Columns.Add("user_id")
                    .Columns.Add("user_name")
                    .Columns.Add("work_category_id")
                    .Columns.Add("work_category_name")
                    .Columns.Add("wh_detail_id")
                    .Columns.Add("start_time")
                    .Columns.Add("end_time")
                    .Columns.Add("job_order")
                    .Columns.Add("detail")
                    .Columns.Add("category_name")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow

                    dr.Item("wh_header_id") = values.Item("wh_header_id")
                    dr.Item("work_date") = values.Item("work_date")
                    dr.Item("user_id") = values.Item("user_id")
                    dr.Item("user_name") = values.Item("user_name")
                    dr.Item("work_category_id") = values.Item("work_category_id")
                    dr.Item("work_category_name") = values.Item("work_category_name")
                    dr.Item("wh_detail_id") = values.Item("wh_detail_id")
                    dr.Item("start_time") = values.Item("start_time")
                    dr.Item("end_time") = values.Item("end_time")
                    dr.Item("job_order") = values.Item("job_order")
                    dr.Item("detail") = values.Item("detail")
                    dr.Item("category_name") = values.Item("category_name")
                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                Return dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableReportSearch(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReport( _
            ByVal dtValues As System.Data.DataTable, _
            ByVal status As String _
        ) As System.Data.DataTable Implements IWorkingHourService.GetTableReport
            ' set default return value
            GetTableReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("period_time")
                    .Columns.Add("status")
                    .Columns.Add("job_order")
                    .Columns.Add("cnt")
                    .Columns.Add("work_year")
                    .Columns.Add("work_month")
                    .Columns.Add("start_work_date")
                    .Columns.Add("end_work_date")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow

                    dr.Item("period_time") = values.Item("period_time")
                    dr.Item("status") = status
                    dr.Item("job_order") = values.Item("job_order")
                    dr.Item("cnt") = values.Item("cnt")
                    dr.Item("work_year") = values.Item("work_year")
                    dr.Item("work_month") = values.Item("work_month")
                    dr.Item("start_work_date") = values.Item("start_work_date")
                    dr.Item("end_work_date") = values.Item("end_work_date")

                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                Return dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsFinishJobOrder
        '	Discription	    : Check exist job order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsFinishJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean Implements IWorkingHourService.IsFinishJobOrder
            ' set default return value
            IsFinishJobOrder = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function IsFinishJobOrder from entity
                intCount = objWorkingHourEnt.ChountFinishJobOrder(strJobOrder)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsFinishJobOrder = True
                Else
                    ' case equal 0 then return False
                    IsFinishJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsFinishJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInJobOrder
        '	Discription	    : Check finish flag on job order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean Implements IWorkingHourService.IsUsedInJobOrder
            ' set default return value
            IsUsedInJobOrder = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer
                Dim jobOrder As String
                Dim arrJobOrder As String() = Split(strJobOrder.Trim, ",")
                Dim intCntJobOrder As Integer = arrJobOrder.Count

                ' call function IsFinishJobOrder from entity
                For i As Integer = 0 To arrJobOrder.Count - 1
                    jobOrder = arrJobOrder(i)
                    If jobOrder <> "" Then
                        intCount = objWorkingHourEnt.CountExitsJobOrder(strJobOrder)
                        If intCount <= 0 Then
                            ' case not equal 0 then return True
                            IsUsedInJobOrder = True
                            Exit Function
                        End If
                    End If
                Next


                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInJobOrder = False
                Else
                    ' case equal 0 then return False
                    IsUsedInJobOrder = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWorkingHour
        '	Discription	    : Insert Work Hour
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWorkingHour( _
            ByVal objWorkingHourDto As Dto.WorkingHourDto, _
            ByVal dtWorkHour As System.Data.DataTable, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWorkingHourService.InsertWorkingHour
            ' set default return value
            InsertWorkingHour = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertWorkingHour from WorkHour Entity
                intEff = objWorkingHourEnt.InsertWorkingHour(SetDtoToEntity(objWorkingHourDto), dtWorkHour)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertWorkingHour = True
                Else
                    ' case row less than 1 then return False
                    InsertWorkingHour = False
                End If

            Catch exSql As MySqlException
                ' other case
                strMsg = "KTWH_02_005"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWorkingHour(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWorkingHour
        '	Discription	    : Update Work Hour
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWorkingHour( _
            ByVal intID As Integer, _
            ByVal dtWorkHour As System.Data.DataTable, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IWorkingHourService.UpdateWorkingHour
            ' set default return value
            UpdateWorkingHour = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateWorkingHour from Job Order Entity
                intEff = objWorkingHourEnt.UpdateWorkingHour(intID, dtWorkHour)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateWorkingHour = True
                Else
                    ' case row less than 1 then return False
                    UpdateWorkingHour = False
                End If

            Catch exSql As MySqlException
                strMsg = "KTWH_02_008"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWorkingHour(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUseWorkingHour
        '	Discription	    : Check duplicate of working hour
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 16-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUseWorkingHour( _
            ByVal strWorkDate As String, _
            ByVal strStaffId As String, _
            ByVal strStartTtime As String, _
            ByVal strEndTime As String, _
            Optional ByVal strDetailId As String = "" _
        ) As Boolean Implements IWorkingHourService.IsUseWorkingHour
            ' set default return value
            IsUseWorkingHour = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                intCount = objWorkingHourEnt.CountWorkingHour(strWorkDate, strStaffId, strStartTtime, strEndTime, strDetailId)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUseWorkingHour = False
                Else
                    ' case equal 0 then return False
                    IsUseWorkingHour = True
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUseWorkingHour(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

    End Class
End Namespace
