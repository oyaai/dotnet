#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpWorkingHourDao
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
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Exception
#End Region

Namespace Dao
    Public Class ImpWorkingHourDao
        Implements IWorkingHourDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: GetWorkingHourList
        '	Discription	    : Get Working Hour List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourList( _
            ByVal objWorkingHourEntity As Entity.IWorkingHourEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpWorkingHourEntity) Implements IWorkingHourDao.GetWorkingHourList

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWorkingHourList = New List(Of Entity.ImpWorkingHourEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWorkingHour As Entity.ImpWorkingHourEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   id,work_date,staff_name,name, ")
                    .AppendLine("   max(case when start_time='0830' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0830, ")
                    .AppendLine("   max(case when start_time='0900' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0900, ")
                    .AppendLine("   max(case when start_time='1000' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1000, ")
                    .AppendLine("   max(case when start_time='1100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1100, ")
                    .AppendLine("   'Break' Lap1200, ")
                    .AppendLine("   max(case when start_time='1245' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1245, ")
                    .AppendLine("   max(case when start_time='1300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1300, ")
                    .AppendLine("   max(case when start_time='1400' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1400, ")
                    .AppendLine("   max(case when start_time='1500' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1500, ")
                    .AppendLine("   'Break' Lap1515, ")
                    .AppendLine("   max(case when start_time='1530' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1530, ")
                    .AppendLine("   max(case when start_time='1600' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1600, ")
                    .AppendLine("   max(case when start_time='1700' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1700, ")
                    .AppendLine("   'Break' Lap1730, ")
                    .AppendLine("   max(case when start_time='1800' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1800, ")
                    .AppendLine("   max(case when start_time='1900' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1900, ")
                    .AppendLine("   max(case when start_time='2000' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2000, ")
                    .AppendLine("   max(case when start_time='2100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2100, ")
                    .AppendLine("   max(case when start_time='2200' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2200, ")
                    .AppendLine("   max(case when start_time='2300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2300, ")
                    .AppendLine("   'Break' Lap0000, ")
                    .AppendLine("   max(case when start_time='0045' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0045, ")
                    .AppendLine("   max(case when start_time='0100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0100, ")
                    .AppendLine("   max(case when start_time='0200' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0200, ")
                    .AppendLine("   max(case when start_time='0300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0300, ")
                    .AppendLine("   'Break' Lap0315, ")
                    .AppendLine("   max(case when start_time='0330' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0330, ")
                    .AppendLine("   max(case when start_time='0400' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0400, ")
                    .AppendLine("   max(case when start_time='0500' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0500, ")
                    .AppendLine("   max(case when start_time='0600' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0600, ")
                    .AppendLine("   max(case when start_time='0700' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0700, ")
                    .AppendLine("   'Break' Lap0800 ")
                    .AppendLine(" FROM (")
                    .AppendLine("   SELECT A.id, A.work_date, concat(B.first_name,' ',B.last_name) AS staff_name,C.name,D.start_time,D.end_time,D.job_order,E.finish_fg ")
                    .AppendLine("   FROM wh_header A ")
                    .AppendLine("   LEFT JOIN mst_staff B ON A.staff_id = B.id  ")
                    .AppendLine("   LEFT JOIN mst_work_category C ON A.work_category_id = C.id ")
                    .AppendLine("   join wh_detail D on D.wh_header_id=A.id ")
                    .AppendLine("   left join job_order E on D.job_order=E.job_order ")
                    .AppendLine("   WHERE (ISNULL(?work_date) OR A.work_date = ?work_date  ) ")
                    .AppendLine("   AND (ISNULL(?staff_id) OR A.staff_id = ?staff_id  ) ")
                    .AppendLine("   AND (ISNULL(?work_category_id) OR A.work_category_id = ?work_category_id  ) ")
                    .AppendLine(" ) A ")
                    .AppendLine(" group by id,work_date,staff_name,name ")
                    .Append(" order by work_date desc,staff_name,name ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?work_date", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_date), DBNull.Value, objWorkingHourEntity.work_date))
                objConn.AddParameter("?staff_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.staff_id_search), DBNull.Value, objWorkingHourEntity.staff_id_search))
                objConn.AddParameter("?work_category_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_category_id_search), DBNull.Value, objWorkingHourEntity.work_category_id_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWorkingHour = New Entity.ImpWorkingHourEntity
                        ' assign data from db to entity object
                        With objWorkingHour
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .work_date = IIf(IsDBNull(dr.Item("work_date")), Nothing, dr.Item("work_date"))
                            .staff_name = IIf(IsDBNull(dr.Item("staff_name")), Nothing, dr.Item("staff_name"))
                            .category = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .Lap0830 = IIf(IsDBNull(dr.Item("Lap0830")), Nothing, dr.Item("Lap0830"))
                            .Lap0900 = IIf(IsDBNull(dr.Item("Lap0900")), Nothing, dr.Item("Lap0900"))
                            .Lap1000 = IIf(IsDBNull(dr.Item("Lap1000")), Nothing, dr.Item("Lap1000"))
                            .Lap1100 = IIf(IsDBNull(dr.Item("Lap1100")), Nothing, dr.Item("Lap1100"))
                            .Lap1200 = IIf(IsDBNull(dr.Item("Lap1200")), Nothing, dr.Item("Lap1200"))
                            .Lap1245 = IIf(IsDBNull(dr.Item("Lap1245")), Nothing, dr.Item("Lap1245"))
                            .Lap1300 = IIf(IsDBNull(dr.Item("Lap1300")), Nothing, dr.Item("Lap1300"))
                            .Lap1400 = IIf(IsDBNull(dr.Item("Lap1400")), Nothing, dr.Item("Lap1400"))
                            .Lap1500 = IIf(IsDBNull(dr.Item("Lap1500")), Nothing, dr.Item("Lap1500"))
                            .Lap1515 = IIf(IsDBNull(dr.Item("Lap1515")), Nothing, dr.Item("Lap1515"))
                            .Lap1530 = IIf(IsDBNull(dr.Item("Lap1530")), Nothing, dr.Item("Lap1530"))
                            .Lap1600 = IIf(IsDBNull(dr.Item("Lap1600")), Nothing, dr.Item("Lap1600"))
                            .Lap1700 = IIf(IsDBNull(dr.Item("Lap1700")), Nothing, dr.Item("Lap1700"))
                            .Lap1730 = IIf(IsDBNull(dr.Item("Lap1730")), Nothing, dr.Item("Lap1730"))
                            .Lap1800 = IIf(IsDBNull(dr.Item("Lap1800")), Nothing, dr.Item("Lap1800"))
                            .Lap1900 = IIf(IsDBNull(dr.Item("Lap1900")), Nothing, dr.Item("Lap1900"))
                            .Lap2000 = IIf(IsDBNull(dr.Item("Lap2000")), Nothing, dr.Item("Lap2000"))
                            .Lap2100 = IIf(IsDBNull(dr.Item("Lap2100")), Nothing, dr.Item("Lap2100"))
                            .Lap2200 = IIf(IsDBNull(dr.Item("Lap2200")), Nothing, dr.Item("Lap2200"))
                            .Lap2300 = IIf(IsDBNull(dr.Item("Lap2300")), Nothing, dr.Item("Lap2300"))
                            .Lap0000 = IIf(IsDBNull(dr.Item("Lap0000")), Nothing, dr.Item("Lap0000"))
                            .Lap0045 = IIf(IsDBNull(dr.Item("Lap0045")), Nothing, dr.Item("Lap0045"))
                            .Lap0100 = IIf(IsDBNull(dr.Item("Lap0100")), Nothing, dr.Item("Lap0100"))
                            .Lap0200 = IIf(IsDBNull(dr.Item("Lap0200")), Nothing, dr.Item("Lap0200"))
                            .Lap0300 = IIf(IsDBNull(dr.Item("Lap0300")), Nothing, dr.Item("Lap0300"))
                            .Lap0315 = IIf(IsDBNull(dr.Item("Lap0315")), Nothing, dr.Item("Lap0315"))
                            .Lap0330 = IIf(IsDBNull(dr.Item("Lap0330")), Nothing, dr.Item("Lap0330"))
                            .Lap0400 = IIf(IsDBNull(dr.Item("Lap0400")), Nothing, dr.Item("Lap0400"))
                            .Lap0500 = IIf(IsDBNull(dr.Item("Lap0500")), Nothing, dr.Item("Lap0500"))
                            .Lap0600 = IIf(IsDBNull(dr.Item("Lap0600")), Nothing, dr.Item("Lap0600"))
                            .Lap0700 = IIf(IsDBNull(dr.Item("Lap0700")), Nothing, dr.Item("Lap0700"))
                            .Lap0800 = IIf(IsDBNull(dr.Item("Lap0800")), Nothing, dr.Item("Lap0800"))
                        End With
                        ' add Working Hour to list
                        GetWorkingHourList.Add(objWorkingHour)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingHourList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
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
            ByVal objWorkingHourEntity As Entity.IWorkingHourEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpWorkingHourEntity) Implements IWorkingHourDao.GetWorkingHourReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWorkingHourReport = New List(Of Entity.ImpWorkingHourEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWorkingHour As Entity.ImpWorkingHourEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   MIN(A.work_date) AS start_work_date ")
                    .AppendLine("   , MAX(A.work_date) AS end_work_date ")
                    .AppendLine("   , MID(A.work_date,1,4) AS work_year ")
                    .AppendLine("   , MID(A.work_date,5,2) AS work_month ")
                    .AppendLine("   , B.start_time AS period_time ")
                    .AppendLine("   , B.job_order ")
                    .AppendLine("   , C.finish_fg ")
                    .AppendLine("   , COUNT(B.job_order) AS cnt  ")
                    .AppendLine(" FROM wh_header A  ")
                    .AppendLine(" LEFT JOIN wh_detail B ON A.id=B.wh_header_id  ")
                    .AppendLine(" LEFT JOIN job_order C ON B.job_order=C.job_order ")
                    .AppendLine(" where B.job_order<>'' ")
                    .AppendLine(" AND (ISNULL(?finish_fg) OR C.finish_fg = ?finish_fg  ) ")
                    .AppendLine(" AND ((ISNULL(?work_date_from) OR A.work_date >= ?work_date_from)    ")
                    .AppendLine(" AND (ISNULL(?work_date_to) OR A.work_date <= ?work_date_to))  ")
                    .AppendLine(" GROUP BY MID(A.work_date,1,4),MID(A.work_date,5,2),B.job_order,B.start_time ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?work_date_from", IIf(String.IsNullOrEmpty(objWorkingHourEntity.start_work_date), DBNull.Value, objWorkingHourEntity.start_work_date))
                objConn.AddParameter("?work_date_to", IIf(String.IsNullOrEmpty(objWorkingHourEntity.end_work_date), DBNull.Value, objWorkingHourEntity.end_work_date))
                objConn.AddParameter("?finish_fg", IIf(String.IsNullOrEmpty(objWorkingHourEntity.job_status), DBNull.Value, objWorkingHourEntity.job_status))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWorkingHour = New Entity.ImpWorkingHourEntity
                        ' assign data from db to entity object
                        With objWorkingHour
                            .start_work_date = IIf(IsDBNull(dr.Item("start_work_date")), Nothing, dr.Item("start_work_date"))
                            .end_work_date = IIf(IsDBNull(dr.Item("end_work_date")), Nothing, dr.Item("end_work_date"))
                            .work_year = IIf(IsDBNull(dr.Item("work_year")), Nothing, dr.Item("work_year"))
                            .work_month = IIf(IsDBNull(dr.Item("work_month")), Nothing, dr.Item("work_month"))
                            .period_time = IIf(IsDBNull(dr.Item("period_time")), Nothing, dr.Item("period_time"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .finish_fg = IIf(IsDBNull(dr.Item("finish_fg")), Nothing, dr.Item("finish_fg"))
                            .cnt = IIf(IsDBNull(dr.Item("cnt")), Nothing, dr.Item("cnt"))

                        End With
                        ' add Working Hour to list
                        GetWorkingHourReport.Add(objWorkingHour)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingHourReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
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
            ByVal objWorkingHourEntity As Entity.IWorkingHourEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpWorkingHourEntity) Implements IWorkingHourDao.GetWorkingHourReportSearch
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWorkingHourReportSearch = New List(Of Entity.ImpWorkingHourEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWorkingHour As Entity.ImpWorkingHourEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   A.id AS wh_header_id ")
                    .AppendLine("   , A.work_date ")
                    .AppendLine("   , A.staff_id AS user_id ")
                    .AppendLine("   , concat(C.first_name,' ',C.last_name) AS user_name ")
                    .AppendLine("   , A.work_category_id  ")
                    .AppendLine("   , D.name AS work_category_name ")
                    .AppendLine("   , B.id AS wh_detail_id ")
                    .AppendLine("   , B.start_time ")
                    .AppendLine("   , B.end_time ")
                    .AppendLine("   , B.job_order ")
                    .AppendLine("   , B.detail  ")
                    If objWorkingHourEntity.work_date = "" And objWorkingHourEntity.staff_id_search = "" And objWorkingHourEntity.work_category_id_search = "" Then
                        .AppendLine("   , 'All' as category  ")
                    Else
                        .AppendLine("   , '' as category  ")
                    End If
                    .AppendLine(" FROM wh_header A ")
                    .AppendLine(" JOIN wh_detail B ON A.id=B.wh_header_id  ")
                    .AppendLine(" LEFT JOIN mst_staff C ON A.staff_id=C.id  ")
                    .AppendLine(" LEFT JOIN mst_work_category D ON A.work_category_id=D.id  ")
                    .AppendLine(" WHERE (ISNULL(?work_date) OR A.work_date = ?work_date  ) ")
                    .AppendLine(" AND (ISNULL(?staff_id) OR A.staff_id = ?staff_id  ) ")
                    .AppendLine(" AND (ISNULL(?work_category_id) OR A.work_category_id = ?work_category_id  ) ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?work_date", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_date), DBNull.Value, objWorkingHourEntity.work_date))
                objConn.AddParameter("?staff_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.staff_id_search), DBNull.Value, objWorkingHourEntity.staff_id_search))
                objConn.AddParameter("?work_category_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_category_id_search), DBNull.Value, objWorkingHourEntity.work_category_id_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWorkingHour = New Entity.ImpWorkingHourEntity
                        ' assign data from db to entity object
                        With objWorkingHour
                            .wh_header_id = IIf(IsDBNull(dr.Item("wh_header_id")), Nothing, dr.Item("wh_header_id"))
                            .work_date = IIf(IsDBNull(dr.Item("work_date")), Nothing, dr.Item("work_date"))
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .user_name = IIf(IsDBNull(dr.Item("user_name")), Nothing, dr.Item("user_name"))
                            .work_category_id = IIf(IsDBNull(dr.Item("work_category_id")), Nothing, dr.Item("work_category_id"))
                            .work_category_name = IIf(IsDBNull(dr.Item("work_category_name")), Nothing, dr.Item("work_category_name"))
                            .wh_detail_id = IIf(IsDBNull(dr.Item("wh_detail_id")), Nothing, dr.Item("wh_detail_id"))
                            .start_time = IIf(IsDBNull(dr.Item("start_time")), Nothing, dr.Item("start_time"))
                            .end_time = IIf(IsDBNull(dr.Item("end_time")), Nothing, dr.Item("end_time"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .detail = IIf(IsDBNull(dr.Item("detail")), Nothing, dr.Item("detail"))
                            .category = IIf(IsDBNull(dr.Item("category")), Nothing, dr.Item("category"))
                        End With
                        ' add Working Hour to list
                        GetWorkingHourReportSearch.Add(objWorkingHour)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReportSearch(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingHourReportSearch(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteWorkingHour
        '	Discription	    : Delete Working Hour
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWorkingHour( _
            ByVal intID As Integer _
        ) As Integer Implements IWorkingHourDao.DeleteWorkingHour
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteWorkingHour = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect                
                intEff = DeleteWhDetail(intID)
                If intEff >= 0 Then
                    intEff = DeleteWhHeader(intID)
                End If

                ' check row effect
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                DeleteWorkingHour = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWorkingHour(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteWorkingHour(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteWhHeader
        '	Discription	    : Delete WH Header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWhHeader( _
            ByVal intID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteWhHeader = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE FROM wh_header                             ")
                    .AppendLine("		WHERE id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteWhHeader = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteWhDetail
        '	Discription	    : Delete WH Detail
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWhDetail( _
            ByVal intID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteWhDetail = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE FROM wh_detail                             ")
                    .AppendLine("		WHERE wh_header_id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteWhDetail = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWhDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteWhDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourReportList
        '	Discription	    : Get Working Hour Report List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourReportList( _
            ByVal objWorkingHourEntity As Entity.IWorkingHourEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpWorkingHourEntity) Implements IWorkingHourDao.GetWorkingHourReportList

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWorkingHourReportList = New List(Of Entity.ImpWorkingHourEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWorkingHour As Entity.ImpWorkingHourEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   id,work_date,staff_name,name, ")
                    .AppendLine("   max(case when start_time='0830' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0830, ")
                    .AppendLine("   max(case when start_time='0900' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0900, ")
                    .AppendLine("   max(case when start_time='1000' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1000, ")
                    .AppendLine("   max(case when start_time='1100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1100, ")
                    .AppendLine("   'Break' Lap1200, ")
                    .AppendLine("   max(case when start_time='1245' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1245, ")
                    .AppendLine("   max(case when start_time='1300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1300, ")
                    .AppendLine("   max(case when start_time='1400' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1400, ")
                    .AppendLine("   max(case when start_time='1500' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1500, ")
                    .AppendLine("   'Break' Lap1515, ")
                    .AppendLine("   max(case when start_time='1530' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1530, ")
                    .AppendLine("   max(case when start_time='1600' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1600, ")
                    .AppendLine("   max(case when start_time='1700' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1700, ")
                    .AppendLine("   'Break' Lap1730, ")
                    .AppendLine("   max(case when start_time='1800' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1800, ")
                    .AppendLine("   max(case when start_time='1900' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap1900, ")
                    .AppendLine("   max(case when start_time='2000' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2000, ")
                    .AppendLine("   max(case when start_time='2100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2100, ")
                    .AppendLine("   max(case when start_time='2200' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2200, ")
                    .AppendLine("   max(case when start_time='2300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap2300, ")
                    .AppendLine("   'Break' Lap0000, ")
                    .AppendLine("   max(case when start_time='0045' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0045, ")
                    .AppendLine("   max(case when start_time='0100' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0100, ")
                    .AppendLine("   max(case when start_time='0200' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0200, ")
                    .AppendLine("   max(case when start_time='0300' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0300, ")
                    .AppendLine("   'Break' Lap0315, ")
                    .AppendLine("   max(case when start_time='0330' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0330, ")
                    .AppendLine("   max(case when start_time='0400' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0400, ")
                    .AppendLine("   max(case when start_time='0500' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0500, ")
                    .AppendLine("   max(case when start_time='0600' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0600, ")
                    .AppendLine("   max(case when start_time='0700' then if(finish_fg is null,'',concat(job_order,',',finish_fg)) else '' end) Lap0700, ")
                    .AppendLine("   'Break' Lap0800 ")
                    .AppendLine(" FROM (")
                    .AppendLine("   SELECT A.id, A.work_date, concat(B.first_name,' ',B.last_name) AS staff_name,C.name,D.start_time,D.end_time,D.job_order,E.finish_fg ")
                    .AppendLine("   FROM wh_header A ")
                    .AppendLine("   LEFT JOIN mst_staff B ON A.staff_id = B.id  ")
                    .AppendLine("   LEFT JOIN mst_work_category C ON A.work_category_id = C.id ")
                    .AppendLine("   join wh_detail D on D.wh_header_id=A.id ")
                    .AppendLine("   left join job_order E on D.job_order=E.job_order ")
                    .AppendLine("   WHERE (ISNULL(?work_date) OR A.work_date = ?work_date  ) ")
                    .AppendLine("   AND (ISNULL(?staff_id) OR A.staff_id = ?staff_id  ) ")
                    .AppendLine("   AND (ISNULL(?work_category_id) OR A.work_category_id = ?work_category_id  ) ")
                    .AppendLine(" ) A ")
                    .AppendLine(" group by id,work_date,staff_name,name ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?work_date", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_date), DBNull.Value, objWorkingHourEntity.work_date))
                objConn.AddParameter("?staff_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.staff_id_search), DBNull.Value, objWorkingHourEntity.staff_id_search))
                objConn.AddParameter("?work_category_id", IIf(String.IsNullOrEmpty(objWorkingHourEntity.work_category_id_search), DBNull.Value, objWorkingHourEntity.work_category_id_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWorkingHour = New Entity.ImpWorkingHourEntity
                        ' assign data from db to entity object
                        With objWorkingHour
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .work_date = IIf(IsDBNull(dr.Item("work_date")), Nothing, dr.Item("work_date"))
                            .staff_name = IIf(IsDBNull(dr.Item("staff_name")), Nothing, dr.Item("staff_name"))
                            .category = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .Lap0830 = IIf(IsDBNull(dr.Item("Lap0830")), Nothing, dr.Item("Lap0830"))
                            .Lap0900 = IIf(IsDBNull(dr.Item("Lap0900")), Nothing, dr.Item("Lap0900"))
                            .Lap1000 = IIf(IsDBNull(dr.Item("Lap1000")), Nothing, dr.Item("Lap1000"))
                            .Lap1100 = IIf(IsDBNull(dr.Item("Lap1100")), Nothing, dr.Item("Lap1100"))
                            .Lap1200 = IIf(IsDBNull(dr.Item("Lap1200")), Nothing, dr.Item("Lap1200"))
                            .Lap1245 = IIf(IsDBNull(dr.Item("Lap1245")), Nothing, dr.Item("Lap1245"))
                            .Lap1300 = IIf(IsDBNull(dr.Item("Lap1300")), Nothing, dr.Item("Lap1300"))
                            .Lap1400 = IIf(IsDBNull(dr.Item("Lap1400")), Nothing, dr.Item("Lap1400"))
                            .Lap1500 = IIf(IsDBNull(dr.Item("Lap1500")), Nothing, dr.Item("Lap1500"))
                            .Lap1515 = IIf(IsDBNull(dr.Item("Lap1515")), Nothing, dr.Item("Lap1515"))
                            .Lap1530 = IIf(IsDBNull(dr.Item("Lap1530")), Nothing, dr.Item("Lap1530"))
                            .Lap1600 = IIf(IsDBNull(dr.Item("Lap1600")), Nothing, dr.Item("Lap1600"))
                            .Lap1700 = IIf(IsDBNull(dr.Item("Lap1700")), Nothing, dr.Item("Lap1700"))
                            .Lap1730 = IIf(IsDBNull(dr.Item("Lap1730")), Nothing, dr.Item("Lap1730"))
                            .Lap1800 = IIf(IsDBNull(dr.Item("Lap1800")), Nothing, dr.Item("Lap1800"))
                            .Lap1900 = IIf(IsDBNull(dr.Item("Lap1900")), Nothing, dr.Item("Lap1900"))
                            .Lap2000 = IIf(IsDBNull(dr.Item("Lap2000")), Nothing, dr.Item("Lap2000"))
                            .Lap2100 = IIf(IsDBNull(dr.Item("Lap2100")), Nothing, dr.Item("Lap2100"))
                            .Lap2200 = IIf(IsDBNull(dr.Item("Lap2200")), Nothing, dr.Item("Lap2200"))
                            .Lap2300 = IIf(IsDBNull(dr.Item("Lap2300")), Nothing, dr.Item("Lap2300"))
                            .Lap0000 = IIf(IsDBNull(dr.Item("Lap0000")), Nothing, dr.Item("Lap0000"))
                            .Lap0045 = IIf(IsDBNull(dr.Item("Lap0045")), Nothing, dr.Item("Lap0045"))
                            .Lap0100 = IIf(IsDBNull(dr.Item("Lap0100")), Nothing, dr.Item("Lap0100"))
                            .Lap0200 = IIf(IsDBNull(dr.Item("Lap0200")), Nothing, dr.Item("Lap0200"))
                            .Lap0300 = IIf(IsDBNull(dr.Item("Lap0300")), Nothing, dr.Item("Lap0300"))
                            .Lap0315 = IIf(IsDBNull(dr.Item("Lap0315")), Nothing, dr.Item("Lap0315"))
                            .Lap0330 = IIf(IsDBNull(dr.Item("Lap0330")), Nothing, dr.Item("Lap0330"))
                            .Lap0400 = IIf(IsDBNull(dr.Item("Lap0400")), Nothing, dr.Item("Lap0400"))
                            .Lap0500 = IIf(IsDBNull(dr.Item("Lap0500")), Nothing, dr.Item("Lap0500"))
                            .Lap0600 = IIf(IsDBNull(dr.Item("Lap0600")), Nothing, dr.Item("Lap0600"))
                            .Lap0700 = IIf(IsDBNull(dr.Item("Lap0700")), Nothing, dr.Item("Lap0700"))
                            .Lap0800 = IIf(IsDBNull(dr.Item("Lap0800")), Nothing, dr.Item("Lap0800"))
                        End With
                        ' add Working Hour to list
                        GetWorkingHourReportList.Add(objWorkingHour)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourReportList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingHourReportList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWorkingHourByID
        '	Discription	    : Get Working Hour Report By ID
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWorkingHourByID( _
            ByVal intId As Integer _
        ) As Entity.IWorkingHourEntity Implements IWorkingHourDao.GetWorkingHourByID

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list 
            GetWorkingHourByID = New Entity.ImpWorkingHourEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   id,work_date,staff_id,staff_name,work_category_id,category, ")
                    .AppendLine("	max(case when start_time='0830' then if(finish_fg is null,'',job_order) else '' end) job_order_1,	  ")
                    .AppendLine("	max(case when start_time='0900' then if(finish_fg is null,'',job_order) else '' end) job_order_2,	  ")
                    .AppendLine("	max(case when start_time='1000' then if(finish_fg is null,'',job_order) else '' end) job_order_3,	  ")
                    .AppendLine("	max(case when start_time='1100' then if(finish_fg is null,'',job_order) else '' end) job_order_4,	  ")
                    .AppendLine("	max(case when start_time='0830' then if(finish_fg is null,'',detail) else '' end) detail_1,	  ")
                    .AppendLine("	max(case when start_time='0900' then if(finish_fg is null,'',detail) else '' end) detail_2,	  ")
                    .AppendLine("	max(case when start_time='1000' then if(finish_fg is null,'',detail) else '' end) detail_3,	  ")
                    .AppendLine("	max(case when start_time='1100' then if(finish_fg is null,'',detail) else '' end) detail_4,	  ")
                    .AppendLine("	max(case when start_time='0830' then detail_id else '' end) detail_id_1,	  ")
                    .AppendLine("	max(case when start_time='0900' then detail_id else '' end) detail_id_2,	  ")
                    .AppendLine("	max(case when start_time='1000' then detail_id else '' end) detail_id_3,	  ")
                    .AppendLine("	max(case when start_time='1100' then detail_id else '' end) detail_id_4,	  ")
                    .AppendLine("	'Break' break_1,	  ")
                    .AppendLine("	max(case when start_time='1245' then if(finish_fg is null,'',job_order) else '' end) job_order_5,	  ")
                    .AppendLine("	max(case when start_time='1300' then if(finish_fg is null,'',job_order) else '' end) job_order_6,	  ")
                    .AppendLine("	max(case when start_time='1400' then if(finish_fg is null,'',job_order) else '' end) job_order_7,	  ")
                    .AppendLine("	max(case when start_time='1500' then if(finish_fg is null,'',job_order) else '' end) job_order_8,	  ")
                    .AppendLine("	max(case when start_time='1245' then if(finish_fg is null,'',detail) else '' end) detail_5,	  ")
                    .AppendLine("	max(case when start_time='1300' then if(finish_fg is null,'',detail) else '' end) detail_6,	  ")
                    .AppendLine("	max(case when start_time='1400' then if(finish_fg is null,'',detail) else '' end) detail_7,	  ")
                    .AppendLine("	max(case when start_time='1500' then if(finish_fg is null,'',detail) else '' end) detail_8,	  ")
                    .AppendLine("	max(case when start_time='1245' then detail_id else '' end) detail_id_5,	  ")
                    .AppendLine("	max(case when start_time='1300' then detail_id else '' end) detail_id_6,	  ")
                    .AppendLine("	max(case when start_time='1400' then detail_id else '' end) detail_id_7,	  ")
                    .AppendLine("	max(case when start_time='1500' then detail_id else '' end) detail_id_8,	  ")
                    .AppendLine("	'Break' break_2,	  ")
                    .AppendLine("	max(case when start_time='1530' then if(finish_fg is null,'',job_order) else '' end) job_order_9,	  ")
                    .AppendLine("	max(case when start_time='1600' then if(finish_fg is null,'',job_order) else '' end) job_order_10,	  ")
                    .AppendLine("	max(case when start_time='1700' then if(finish_fg is null,'',job_order) else '' end) job_order_11,	  ")
                    .AppendLine("	max(case when start_time='1530' then if(finish_fg is null,'',detail) else '' end) detail_9,	  ")
                    .AppendLine("	max(case when start_time='1600' then if(finish_fg is null,'',detail) else '' end) detail_10,	  ")
                    .AppendLine("	max(case when start_time='1700' then if(finish_fg is null,'',detail) else '' end) detail_11,	  ")
                    .AppendLine("	max(case when start_time='1530' then detail_id else '' end) detail_id_9,	  ")
                    .AppendLine("	max(case when start_time='1600' then detail_id else '' end) detail_id_10,	  ")
                    .AppendLine("	max(case when start_time='1700' then detail_id else '' end) detail_id_11,	  ")
                    .AppendLine("	'Break' break_3,	  ")
                    .AppendLine("	max(case when start_time='1800' then if(finish_fg is null,'',job_order) else '' end) job_order_12,	  ")
                    .AppendLine("	max(case when start_time='1900' then if(finish_fg is null,'',job_order) else '' end) job_order_13,	  ")
                    .AppendLine("	max(case when start_time='2000' then if(finish_fg is null,'',job_order) else '' end) job_order_14,	  ")
                    .AppendLine("	max(case when start_time='2100' then if(finish_fg is null,'',job_order) else '' end) job_order_15,	  ")
                    .AppendLine("	max(case when start_time='2200' then if(finish_fg is null,'',job_order) else '' end) job_order_16,	  ")
                    .AppendLine("	max(case when start_time='2300' then if(finish_fg is null,'',job_order) else '' end) job_order_17,	  ")
                    .AppendLine("	max(case when start_time='1800' then if(finish_fg is null,'',detail) else '' end) detail_12,	  ")
                    .AppendLine("	max(case when start_time='1900' then if(finish_fg is null,'',detail) else '' end) detail_13,	  ")
                    .AppendLine("	max(case when start_time='2000' then if(finish_fg is null,'',detail) else '' end) detail_14,	  ")
                    .AppendLine("	max(case when start_time='2100' then if(finish_fg is null,'',detail) else '' end) detail_15,	  ")
                    .AppendLine("	max(case when start_time='2200' then if(finish_fg is null,'',detail) else '' end) detail_16,	  ")
                    .AppendLine("	max(case when start_time='2300' then if(finish_fg is null,'',detail) else '' end) detail_17,	  ")
                    .AppendLine("	max(case when start_time='1800' then detail_id else '' end) detail_id_12,	  ")
                    .AppendLine("	max(case when start_time='1900' then detail_id else '' end) detail_id_13,	  ")
                    .AppendLine("	max(case when start_time='2000' then detail_id else '' end) detail_id_14,	  ")
                    .AppendLine("	max(case when start_time='2100' then detail_id else '' end) detail_id_15,	  ")
                    .AppendLine("	max(case when start_time='2200' then detail_id else '' end) detail_id_16,	  ")
                    .AppendLine("	max(case when start_time='2300' then detail_id else '' end) detail_id_17,	  ")
                    .AppendLine("	'Break' break_4,	  ")
                    .AppendLine("	max(case when start_time='0045' then if(finish_fg is null,'',job_order) else '' end) job_order_18,	  ")
                    .AppendLine("	max(case when start_time='0100' then if(finish_fg is null,'',job_order) else '' end) job_order_19,	  ")
                    .AppendLine("	max(case when start_time='0200' then if(finish_fg is null,'',job_order) else '' end) job_order_20,	  ")
                    .AppendLine("	max(case when start_time='0300' then if(finish_fg is null,'',job_order) else '' end) job_order_21,	  ")
                    .AppendLine("	max(case when start_time='0045' then if(finish_fg is null,'',detail) else '' end) detail_18,	  ")
                    .AppendLine("	max(case when start_time='0100' then if(finish_fg is null,'',detail) else '' end) detail_19,	  ")
                    .AppendLine("	max(case when start_time='0200' then if(finish_fg is null,'',detail) else '' end) detail_20,	  ")
                    .AppendLine("	max(case when start_time='0300' then if(finish_fg is null,'',detail) else '' end) detail_21,	  ")
                    .AppendLine("	max(case when start_time='0045' then detail_id else '' end) detail_id_18,	  ")
                    .AppendLine("	max(case when start_time='0100' then detail_id else '' end) detail_id_19,	  ")
                    .AppendLine("	max(case when start_time='0200' then detail_id else '' end) detail_id_20,	  ")
                    .AppendLine("	max(case when start_time='0300' then detail_id else '' end) detail_id_21,	  ")
                    .AppendLine("	'Break' break_5,	  ")
                    .AppendLine("	max(case when start_time='0330' then if(finish_fg is null,'',job_order) else '' end) job_order_22,	  ")
                    .AppendLine("	max(case when start_time='0400' then if(finish_fg is null,'',job_order) else '' end) job_order_23,	  ")
                    .AppendLine("	max(case when start_time='0500' then if(finish_fg is null,'',job_order) else '' end) job_order_24,	  ")
                    .AppendLine("	max(case when start_time='0600' then if(finish_fg is null,'',job_order) else '' end) job_order_25,	  ")
                    .AppendLine("	max(case when start_time='0700' then if(finish_fg is null,'',job_order) else '' end) job_order_26,	  ")
                    .AppendLine("	max(case when start_time='0330' then if(finish_fg is null,'',detail) else '' end) detail_22,	  ")
                    .AppendLine("	max(case when start_time='0400' then if(finish_fg is null,'',detail) else '' end) detail_23,	  ")
                    .AppendLine("	max(case when start_time='0500' then if(finish_fg is null,'',detail) else '' end) detail_24,	  ")
                    .AppendLine("	max(case when start_time='0600' then if(finish_fg is null,'',detail) else '' end) detail_25,	  ")
                    .AppendLine("	max(case when start_time='0700' then if(finish_fg is null,'',detail) else '' end) detail_26,	  ")
                    .AppendLine("	max(case when start_time='0330' then detail_id else '' end) detail_id_22,	  ")
                    .AppendLine("	max(case when start_time='0400' then detail_id else '' end) detail_id_23,	  ")
                    .AppendLine("	max(case when start_time='0500' then detail_id else '' end) detail_id_24,	  ")
                    .AppendLine("	max(case when start_time='0600' then detail_id else '' end) detail_id_25,	  ")
                    .AppendLine("	max(case when start_time='0700' then detail_id else '' end) detail_id_26,	  ")
                    .AppendLine("	'Break' break_6	  ")
                    .AppendLine(" FROM (")
                    .AppendLine("   SELECT A.id,D.id AS detail_id, A.work_date,A.staff_id, concat(B.first_name,' ',B.last_name) AS staff_name,A.work_category_id,C.name AS category ,D.start_time,D.end_time,D.job_order,E.finish_fg,D.detail ")
                    .AppendLine("   FROM wh_header A ")
                    .AppendLine("   LEFT JOIN mst_staff B ON A.staff_id = B.id  ")
                    .AppendLine("   LEFT JOIN mst_work_category C ON A.work_category_id = C.id ")
                    .AppendLine("   join wh_detail D on D.wh_header_id=A.id ")
                    .AppendLine("   left join job_order E on D.job_order=E.job_order ")
                    .AppendLine("   WHERE A.id = ?id ")
                    .AppendLine(" ) A ")
                    .AppendLine(" group by id,work_date,staff_name,category ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(intId), DBNull.Value, intId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetWorkingHourByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .work_date = IIf(IsDBNull(dr.Item("work_date")), Nothing, dr.Item("work_date"))
                            .staff_id = IIf(IsDBNull(dr.Item("staff_id")), Nothing, dr.Item("staff_id"))
                            .staff_name = IIf(IsDBNull(dr.Item("staff_name")), Nothing, dr.Item("staff_name"))
                            .category = IIf(IsDBNull(dr.Item("category")), Nothing, dr.Item("category"))
                            .work_category_id = IIf(IsDBNull(dr.Item("work_category_id")), Nothing, dr.Item("work_category_id"))
                            .Lap0830 = IIf(IsDBNull(dr.Item("job_order_1")), Nothing, dr.Item("job_order_1"))
                            .Lap0900 = IIf(IsDBNull(dr.Item("job_order_2")), Nothing, dr.Item("job_order_2"))
                            .Lap1000 = IIf(IsDBNull(dr.Item("job_order_3")), Nothing, dr.Item("job_order_3"))
                            .Lap1100 = IIf(IsDBNull(dr.Item("job_order_4")), Nothing, dr.Item("job_order_4"))
                            .Lap1200 = IIf(IsDBNull(dr.Item("break_1")), Nothing, dr.Item("break_1"))
                            .Lap1245 = IIf(IsDBNull(dr.Item("job_order_5")), Nothing, dr.Item("job_order_5"))
                            .Lap1300 = IIf(IsDBNull(dr.Item("job_order_6")), Nothing, dr.Item("job_order_6"))
                            .Lap1400 = IIf(IsDBNull(dr.Item("job_order_7")), Nothing, dr.Item("job_order_7"))
                            .Lap1500 = IIf(IsDBNull(dr.Item("job_order_8")), Nothing, dr.Item("job_order_8"))
                            .Lap1515 = IIf(IsDBNull(dr.Item("break_2")), Nothing, dr.Item("break_2"))
                            .Lap1530 = IIf(IsDBNull(dr.Item("job_order_9")), Nothing, dr.Item("job_order_9"))
                            .Lap1600 = IIf(IsDBNull(dr.Item("job_order_10")), Nothing, dr.Item("job_order_10"))
                            .Lap1700 = IIf(IsDBNull(dr.Item("job_order_11")), Nothing, dr.Item("job_order_11"))
                            .Lap1730 = IIf(IsDBNull(dr.Item("break_3")), Nothing, dr.Item("break_3"))
                            .Lap1800 = IIf(IsDBNull(dr.Item("job_order_12")), Nothing, dr.Item("job_order_12"))
                            .Lap1900 = IIf(IsDBNull(dr.Item("job_order_13")), Nothing, dr.Item("job_order_13"))
                            .Lap2000 = IIf(IsDBNull(dr.Item("job_order_14")), Nothing, dr.Item("job_order_14"))
                            .Lap2100 = IIf(IsDBNull(dr.Item("job_order_15")), Nothing, dr.Item("job_order_15"))
                            .Lap2200 = IIf(IsDBNull(dr.Item("job_order_16")), Nothing, dr.Item("job_order_16"))
                            .Lap2300 = IIf(IsDBNull(dr.Item("job_order_17")), Nothing, dr.Item("job_order_17"))
                            .Lap0000 = IIf(IsDBNull(dr.Item("break_4")), Nothing, dr.Item("break_4"))
                            .Lap0045 = IIf(IsDBNull(dr.Item("job_order_18")), Nothing, dr.Item("job_order_18"))
                            .Lap0100 = IIf(IsDBNull(dr.Item("job_order_19")), Nothing, dr.Item("job_order_19"))
                            .Lap0200 = IIf(IsDBNull(dr.Item("job_order_20")), Nothing, dr.Item("job_order_20"))
                            .Lap0300 = IIf(IsDBNull(dr.Item("job_order_21")), Nothing, dr.Item("job_order_21"))
                            .Lap0315 = IIf(IsDBNull(dr.Item("break_5")), Nothing, dr.Item("break_5"))
                            .Lap0330 = IIf(IsDBNull(dr.Item("job_order_22")), Nothing, dr.Item("job_order_22"))
                            .Lap0400 = IIf(IsDBNull(dr.Item("job_order_23")), Nothing, dr.Item("job_order_23"))
                            .Lap0500 = IIf(IsDBNull(dr.Item("job_order_24")), Nothing, dr.Item("job_order_24"))
                            .Lap0600 = IIf(IsDBNull(dr.Item("job_order_25")), Nothing, dr.Item("job_order_25"))
                            .Lap0700 = IIf(IsDBNull(dr.Item("job_order_26")), Nothing, dr.Item("job_order_26"))
                            .Lap0800 = IIf(IsDBNull(dr.Item("break_6")), Nothing, dr.Item("break_6"))
                            .detail_1 = IIf(IsDBNull(dr.Item("detail_1")), Nothing, dr.Item("detail_1"))
                            .detail_2 = IIf(IsDBNull(dr.Item("detail_2")), Nothing, dr.Item("detail_2"))
                            .detail_3 = IIf(IsDBNull(dr.Item("detail_3")), Nothing, dr.Item("detail_3"))
                            .detail_4 = IIf(IsDBNull(dr.Item("detail_4")), Nothing, dr.Item("detail_4"))
                            .detail_5 = IIf(IsDBNull(dr.Item("detail_5")), Nothing, dr.Item("detail_5"))
                            .detail_6 = IIf(IsDBNull(dr.Item("detail_6")), Nothing, dr.Item("detail_6"))
                            .detail_7 = IIf(IsDBNull(dr.Item("detail_7")), Nothing, dr.Item("detail_7"))
                            .detail_8 = IIf(IsDBNull(dr.Item("detail_8")), Nothing, dr.Item("detail_8"))
                            .detail_9 = IIf(IsDBNull(dr.Item("detail_9")), Nothing, dr.Item("detail_9"))
                            .detail_10 = IIf(IsDBNull(dr.Item("detail_10")), Nothing, dr.Item("detail_10"))
                            .detail_11 = IIf(IsDBNull(dr.Item("detail_11")), Nothing, dr.Item("detail_11"))
                            .detail_12 = IIf(IsDBNull(dr.Item("detail_12")), Nothing, dr.Item("detail_12"))
                            .detail_13 = IIf(IsDBNull(dr.Item("detail_13")), Nothing, dr.Item("detail_13"))
                            .detail_14 = IIf(IsDBNull(dr.Item("detail_14")), Nothing, dr.Item("detail_14"))
                            .detail_15 = IIf(IsDBNull(dr.Item("detail_15")), Nothing, dr.Item("detail_15"))
                            .detail_16 = IIf(IsDBNull(dr.Item("detail_16")), Nothing, dr.Item("detail_16"))
                            .detail_17 = IIf(IsDBNull(dr.Item("detail_17")), Nothing, dr.Item("detail_17"))
                            .detail_18 = IIf(IsDBNull(dr.Item("detail_18")), Nothing, dr.Item("detail_18"))
                            .detail_19 = IIf(IsDBNull(dr.Item("detail_19")), Nothing, dr.Item("detail_19"))
                            .detail_20 = IIf(IsDBNull(dr.Item("detail_20")), Nothing, dr.Item("detail_20"))
                            .detail_21 = IIf(IsDBNull(dr.Item("detail_21")), Nothing, dr.Item("detail_21"))
                            .detail_22 = IIf(IsDBNull(dr.Item("detail_22")), Nothing, dr.Item("detail_22"))
                            .detail_23 = IIf(IsDBNull(dr.Item("detail_23")), Nothing, dr.Item("detail_23"))
                            .detail_24 = IIf(IsDBNull(dr.Item("detail_24")), Nothing, dr.Item("detail_24"))
                            .detail_25 = IIf(IsDBNull(dr.Item("detail_25")), Nothing, dr.Item("detail_25"))
                            .detail_26 = IIf(IsDBNull(dr.Item("detail_26")), Nothing, dr.Item("detail_26"))
                            .detail_id_1 = IIf(IsDBNull(dr.Item("detail_id_1")), Nothing, dr.Item("detail_id_1"))
                            .detail_id_2 = IIf(IsDBNull(dr.Item("detail_id_2")), Nothing, dr.Item("detail_id_2"))
                            .detail_id_3 = IIf(IsDBNull(dr.Item("detail_id_3")), Nothing, dr.Item("detail_id_3"))
                            .detail_id_4 = IIf(IsDBNull(dr.Item("detail_id_4")), Nothing, dr.Item("detail_id_4"))
                            .detail_id_5 = IIf(IsDBNull(dr.Item("detail_id_5")), Nothing, dr.Item("detail_id_5"))
                            .detail_id_6 = IIf(IsDBNull(dr.Item("detail_id_6")), Nothing, dr.Item("detail_id_6"))
                            .detail_id_7 = IIf(IsDBNull(dr.Item("detail_id_7")), Nothing, dr.Item("detail_id_7"))
                            .detail_id_8 = IIf(IsDBNull(dr.Item("detail_id_8")), Nothing, dr.Item("detail_id_8"))
                            .detail_id_9 = IIf(IsDBNull(dr.Item("detail_id_9")), Nothing, dr.Item("detail_id_9"))
                            .detail_id_10 = IIf(IsDBNull(dr.Item("detail_id_10")), Nothing, dr.Item("detail_id_10"))
                            .detail_id_11 = IIf(IsDBNull(dr.Item("detail_id_11")), Nothing, dr.Item("detail_id_11"))
                            .detail_id_12 = IIf(IsDBNull(dr.Item("detail_id_12")), Nothing, dr.Item("detail_id_12"))
                            .detail_id_13 = IIf(IsDBNull(dr.Item("detail_id_13")), Nothing, dr.Item("detail_id_13"))
                            .detail_id_14 = IIf(IsDBNull(dr.Item("detail_id_14")), Nothing, dr.Item("detail_id_14"))
                            .detail_id_15 = IIf(IsDBNull(dr.Item("detail_id_15")), Nothing, dr.Item("detail_id_15"))
                            .detail_id_16 = IIf(IsDBNull(dr.Item("detail_id_16")), Nothing, dr.Item("detail_id_16"))
                            .detail_id_17 = IIf(IsDBNull(dr.Item("detail_id_17")), Nothing, dr.Item("detail_id_17"))
                            .detail_id_18 = IIf(IsDBNull(dr.Item("detail_id_18")), Nothing, dr.Item("detail_id_18"))
                            .detail_id_19 = IIf(IsDBNull(dr.Item("detail_id_19")), Nothing, dr.Item("detail_id_19"))
                            .detail_id_20 = IIf(IsDBNull(dr.Item("detail_id_20")), Nothing, dr.Item("detail_id_20"))
                            .detail_id_21 = IIf(IsDBNull(dr.Item("detail_id_21")), Nothing, dr.Item("detail_id_21"))
                            .detail_id_22 = IIf(IsDBNull(dr.Item("detail_id_22")), Nothing, dr.Item("detail_id_22"))
                            .detail_id_23 = IIf(IsDBNull(dr.Item("detail_id_23")), Nothing, dr.Item("detail_id_23"))
                            .detail_id_24 = IIf(IsDBNull(dr.Item("detail_id_24")), Nothing, dr.Item("detail_id_24"))
                            .detail_id_25 = IIf(IsDBNull(dr.Item("detail_id_25")), Nothing, dr.Item("detail_id_25"))
                            .detail_id_26 = IIf(IsDBNull(dr.Item("detail_id_26")), Nothing, dr.Item("detail_id_26"))

                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWorkingHourById(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWorkingHourById(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExitsJobOrder
        '	Discription	    : Count data in used job order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountExitsJobOrder( _
            ByVal strJobOrder As String _
        ) As Integer Implements IWorkingHourDao.CountExitsJobOrder
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountExitsJobOrder = 0
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) FROM job_order 				")
                    .AppendLine("       WHERE status_id <> 6 ")
                    .AppendLine("		AND (FIND_IN_SET(CAST(job_order AS CHAR), ?job_order) > 0) 			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CountExitsJobOrder = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountExitsJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountExitsJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountWorkingHour
        '	Discription	    : Check duplicate of working hour
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 16-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountWorkingHour( _
            ByVal strWorkDate As String, _
            ByVal strStaffId As String, _
            ByVal strStartTtime As String, _
            ByVal strEndTime As String, _
            Optional ByVal strDetailId As String = "" _
        ) As Integer Implements IWorkingHourDao.CountWorkingHour
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountWorkingHour = 0
            Try
                ' assign sql command
                With strSql
                    .AppendLine("   SELECT count(*) AS cnt 				")
                    .AppendLine("   FROM wh_header a  ")
                    .AppendLine("   JOIN wh_detail b ON a.id=b.wh_header_id ")
                    .AppendLine("   WHERE a.work_date = ?work_date ")
                    .AppendLine("   AND a.staff_id = ?staff_id ")
                    .AppendLine("   AND b.start_time = ?start_time  ")
                    .AppendLine("   AND b.end_time = ?end_time  ")
                    .AppendLine("   AND ( ISNULL(?id) or  b.id <> ?id ) ")
                    .AppendLine("   AND ifnull(job_order,'')<>''; ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?work_date", strWorkDate)
                objConn.AddParameter("?staff_id", strStaffId)
                objConn.AddParameter("?start_time", strStartTtime)
                objConn.AddParameter("?end_time", strEndTime)
                objConn.AddParameter("?id", strDetailId)

                ' execute sql command
                CountWorkingHour = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountWorkingHour(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountWorkingHour(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChountFinishJobOrder
        '	Discription	    : Count data finish in used job order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ChountFinishJobOrder( _
            ByVal strJobOrder As String _
        ) As Integer Implements IWorkingHourDao.ChountFinishJobOrder
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            ChountFinishJobOrder = 0
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT SUM(finish_fg) FROM job_order  				")
                    .AppendLine("		WHERE (FIND_IN_SET(CAST(job_order AS CHAR), ?job_order) > 0) 			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                ChountFinishJobOrder = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ChountFinishJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("ChountFinishJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWorkingHour
        '	Discription	    : Insert data into work hour
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWorkingHour( _
            ByVal objWorkingHourEnt As Entity.ImpWorkingHourEntity, _
            ByVal dtWorkHour As System.Data.DataTable _
        ) As Integer Implements IWorkingHourDao.InsertWorkingHour
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertWorkingHour = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' execute non query and keep row effect

                ' Insert data into wh_header
                intEff = InsertWHHeader(objWorkingHourEnt)
                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'get last id of wh_header
                intEff = GetHeaderID()
                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Insert data into wh_detail
                If intEff > 0 Then
                    intEff = InsertWHDetail(dtWorkHour, intEff)
                End If

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                InsertWorkingHour = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWorkingHour(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertWorkingHour(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetHeaderID
        '	Discription	    : get last id of wh_header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetHeaderID() As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetHeaderID = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT max(id) AS last_id FROM wh_header; ")
                End With

                ' execute sql command
                GetHeaderID = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetHeaderID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("GetHeaderID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWHHeader
        '	Discription	    : Insert data to wh_header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWHHeader( _
            ByVal objWorkingHourEnt As Entity.IWorkingHourEntity _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertWHHeader = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine(" INSERT INTO wh_header 	 ")
                    .AppendLine("	(work_date	 ")
                    .AppendLine("	, staff_id	 ")
                    .AppendLine("	, work_category_id	 ")
                    .AppendLine("	, created_by	 ")
                    .AppendLine("	, created_date	 ")
                    .AppendLine("	, updated_by	 ")
                    .AppendLine("	, updated_date	) ")
                    .AppendLine(" VALUES ( ")
                    .AppendLine("   ?work_date ")
                    .AppendLine("   ,?staff_id ")
                    .AppendLine("   ,?work_category_id ")
                    .AppendLine("   ,?user_id ")
                    .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("   ,?user_id ")
                    .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	);	 ")
                End With

                With objConn
                    ' assign parameter                    
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?work_date", objWorkingHourEnt.work_date)
                    .AddParameter("?staff_id", objWorkingHourEnt.staff_id)
                    .AddParameter("?work_category_id", objWorkingHourEnt.work_category_id)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                InsertWHHeader = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertWHHeader(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWHHeader(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertWHHeader(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWHDetail
        '	Discription	    : Insert data to wh_detail
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWHDetail( _
            ByVal dtWorkHour As System.Data.DataTable, _
            ByVal intLastId As Integer _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertWHDetail = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer
                ' loop table for insert data
                For Each row As DataRow In dtWorkHour.Rows
                    If row("job_order") <> "" Then
                        ' assign sql command
                        With strSql
                            .Length = 0
                            .AppendLine(" INSERT INTO wh_detail 	 ")
                            .AppendLine("	(wh_header_id	 ")
                            .AppendLine("	, job_order	 ")
                            .AppendLine("	, start_time	 ")
                            .AppendLine("	, end_time	 ")
                            .AppendLine("	, detail	 ")
                            .AppendLine("	, created_by	 ")
                            .AppendLine("	, created_date	 ")
                            .AppendLine("	, updated_by	 ")
                            .AppendLine("	, updated_date	) ")
                            .AppendLine(" VALUES ( ")
                            .AppendLine("   ?wh_header_id ")
                            .AppendLine("   ,?job_order ")
                            .AppendLine("   ,?start_time ")
                            .AppendLine("   ,?end_time ")
                            .AppendLine("   ,?detail ")
                            .AppendLine("   ,?user_id ")
                            .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                            .AppendLine("   ,?user_id ")
                            .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                            .AppendLine("	);	 ")
                        End With

                        With objConn
                            ' assign parameter                    
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                            .AddParameter("?wh_header_id", intLastId)
                            .AddParameter("?job_order", row("job_order"))
                            .AddParameter("?start_time", row("start_time"))
                            .AddParameter("?end_time", row("end_time"))
                            .AddParameter("?detail", row("detail"))

                            ' execute sql command and return row effect to intEff variable
                            intEff = .ExecuteNonQuery(strSql.ToString)

                            ' check row effect 
                            If intEff <= 0 Then
                                ' case have error then exit for
                                Exit For
                            End If
                        End With

                    End If
                Next

                ' assign return value
                InsertWHDetail = intEff

            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertWHDetail(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWHDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertWHDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWorkingHour
        '	Discription	    : update data on work hour
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWorkingHour( _
            ByVal intID As Integer, _
            ByVal dtWorkHour As System.Data.DataTable _
        ) As Integer Implements IWorkingHourDao.UpdateWorkingHour
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateWorkingHour = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' loop table for insert data
                For Each row As DataRow In dtWorkHour.Rows
                    'Case add new data on  wh_detail
                    If row("detail_id") = "" And row("job_order") <> "" Then
                        ' assign sql command
                        With strSql
                            .Length = 0
                            .AppendLine(" INSERT INTO wh_detail 	 ")
                            .AppendLine("	(wh_header_id	 ")
                            .AppendLine("	, job_order	 ")
                            .AppendLine("	, start_time	 ")
                            .AppendLine("	, end_time	 ")
                            .AppendLine("	, detail	 ")
                            .AppendLine("	, created_by	 ")
                            .AppendLine("	, created_date	 ")
                            .AppendLine("	, updated_by	 ")
                            .AppendLine("	, updated_date	) ")
                            .AppendLine(" VALUES ( ")
                            .AppendLine("   ?wh_header_id ")
                            .AppendLine("   ,?job_order ")
                            .AppendLine("   ,?start_time ")
                            .AppendLine("   ,?end_time ")
                            .AppendLine("   ,?detail ")
                            .AppendLine("   ,?user_id ")
                            .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                            .AppendLine("   ,?user_id ")
                            .AppendLine("   ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                            .AppendLine("	);	 ")
                        End With

                        With objConn
                            ' assign parameter                    
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                            .AddParameter("?wh_header_id", intID)
                            .AddParameter("?job_order", row("job_order"))
                            .AddParameter("?start_time", row("start_time"))
                            .AddParameter("?end_time", row("end_time"))
                            .AddParameter("?detail", row("detail"))

                            ' execute sql command and return row effect to intEff variable
                            intEff = .ExecuteNonQuery(strSql.ToString)

                            ' check row effect 
                            If intEff <= 0 Then
                                ' case have error then exit for
                                Exit For
                            End If
                        End With
                    Else
                        'Case delete job_order on wh_detail
                        If row("job_order") = "" Then
                            ' assign sql command
                            With strSql
                                .Length = 0
                                .AppendLine("       DELETE FROM wh_detail               ")
                                .AppendLine("		WHERE id  = ?id                     ")
                                .AppendLine("		AND wh_header_id  = ?wh_header_id   ")
                            End With

                            ' assign parameter
                            objConn.AddParameter("?id", row("detail_id"))
                            objConn.AddParameter("?wh_header_id", intID)

                            ' execute non query and keep row effect
                            intEff = objConn.ExecuteNonQuery(strSql.ToString)

                            If intEff = 0 Then intEff = 1

                            ' check row effect 
                            If intEff < 0 Then
                                ' case have error then exit for
                                Exit For
                            End If

                        Else
                            'Case Modify job_order or detail on wh_detail
                            ' assign sql command
                            With strSql
                                .Length = 0
                                .AppendLine("	UPDATE wh_detail							")
                                .AppendLine("	    SET job_order = ?job_order	 ")
                                .AppendLine("	    ,detail = ?detail	 ")
                                .AppendLine("		,updated_by = ?user_id							")
                                .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                                .AppendLine("	WHERE (wh_header_id = ?wh_header_id) ")
                                .AppendLine("	AND (id = ?id) ")
                            End With

                            With objConn
                                ' assign parameter
                                .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                                .AddParameter("?wh_header_id", intID)
                                .AddParameter("?job_order", row("job_order"))
                                .AddParameter("?detail", row("detail"))
                                .AddParameter("?id", row("detail_id"))

                                ' execute sql command and return row effect to intEff variable
                                intEff = .ExecuteNonQuery(strSql.ToString)

                                ' check row effect 
                                If intEff <= 0 Then
                                    ' case have error then exit for
                                    Exit For
                                End If
                            End With
                        End If
                    End If

                    '--Start Delte by Wall 2013/08/29 (Mod Spec)
                    '' assign sql command
                    'With strSql
                    '    .Length = 0
                    '    .AppendLine("	UPDATE wh_detail							")
                    '    .AppendLine("	    SET job_order = ?job_order	 ")
                    '    .AppendLine("	    ,detail = ?detail	 ")
                    '    .AppendLine("		,updated_by = ?user_id							")
                    '    .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    '    .AppendLine("	WHERE (wh_header_id = ?wh_header_id) ")
                    '    .AppendLine("	AND (id = ?id) ")
                    'End With

                    '' new connection object
                    'objConn = New Common.DBConnection.MySQLAccess

                    'With objConn
                    '    ' assign parameter
                    '    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    '    .AddParameter("?wh_header_id", intID)
                    '    .AddParameter("?job_order", row("job_order"))
                    '    .AddParameter("?detail", row("detail"))
                    '    .AddParameter("?id", row("detail_id"))

                    '    ' execute sql command and return row effect to intEff variable
                    '    intEff = .ExecuteNonQuery(strSql.ToString)

                    '    ' check row effect 
                    '    If intEff <= 0 Then
                    '        ' case have error then exit for
                    '        Exit For
                    '    End If
                    'End With

                    'dtWorkHour.AcceptChanges()

                    '--End Delte by Wall 2013/08/29 (Mod Spec)
                Next

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' assign return value
                UpdateWorkingHour = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateWorkingHour(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWorkingHour(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateWorkingHour(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

    End Class
End Namespace
