Imports Microsoft.VisualBasic

Namespace Dao
    Public Class ImpTaskDao
        Implements ITaskDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        Public Function GetListTaskOfDDL(ByVal intUserId As Integer) As System.Collections.Generic.List(Of Entity.ITaskEntity) Implements ITaskDao.GetListTaskOfDDL
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objItem As System.Data.DataRow
                Dim objTask As Entity.ImpTaskEntity

                GetListTaskOfDDL = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine("SELECT DISTINCT task id,task value")
                    .AppendLine("FROM task")
                    .AppendLine("WHERE user_id = ?user_id")
                    .Append("AND convert(schedule, DATE) < now();")

                    ' assign parameter
                    objConn.AddParameter("?user_id", intUserId)

                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function

                GetListTaskOfDDL = New List(Of Entity.ITaskEntity)
                For Each objItem In objDT.Rows
                    objTask = New Entity.ImpTaskEntity
                    With objTask
                        'task() As string
                        .task = objConn.CheckDBNull(objItem("value"), Common.DBConnection.DBType.DBString)
                    End With
                    GetListTaskOfDDL.Add(objTask)
                Next

            Catch ex As Exception
                ' write error log
                GetListTaskOfDDL = Nothing
                objLog.ErrorLog("GetListTaskOfDDL(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetListTaskOfDDL(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        Public Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer) As System.Collections.Generic.List(Of Entity.ITaskEntity) Implements ITaskDao.GetTaskSearch
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objItem As System.Data.DataRow
                Dim objTask As Entity.ImpTaskEntity

                GetTaskSearch = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine("SELECT a.* ")
                    .AppendLine("FROM task a join po_header b on a.refkey=b.id ")
                    .AppendLine("left join payment_header c on b.id=c.po_header_id")
                    .AppendLine("WHERE ( a.task = ?task OR ifnull(?task,'') = '' ) ")
                    .AppendLine("AND a.user_id = ?user_id ")
                    .AppendLine("AND convert(a.schedule, DATE) < now() ")
                    .AppendLine("AND b.status_id=4 ")
                    .AppendLine("and c.id is null") ' AND b.status_id<>6
                    .Append("ORDER BY a.id;")

                    ' assign parameter
                    objConn.AddParameter("?task", strTask)
                    objConn.AddParameter("?user_id", intUserId)
                End With

                ' execute by datatable
                objDT = New System.Data.DataTable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function

                Dim dt As Date
                Dim cm As New Common.Utilities.Utility
                GetTaskSearch = New List(Of Entity.ITaskEntity)
                For Each objItem In objDT.Rows
                    objTask = New Entity.ImpTaskEntity
                    With objTask
                        'id() As integer
                        .id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                        'schedule() As string
                        dt = cm.String2Date(objItem("schedule").ToString(), "yyyyMMdd")
                        .schedule = dt.ToString("dd-MMM-yyyy")
                        'task() As string
                        .task = objConn.CheckDBNull(objItem("task"), Common.DBConnection.DBType.DBString)
                        'note() As string
                        .note = objConn.CheckDBNull(objItem("note"), Common.DBConnection.DBType.DBString)
                        'refpage() As string
                        .refpage = objConn.CheckDBNull(objItem("refpage"), Common.DBConnection.DBType.DBString)
                        'tskpage() As string
                        .tskpage = objConn.CheckDBNull(objItem("tskpage"), Common.DBConnection.DBType.DBString)
                        'user_id() As integer
                        .user_id = objConn.CheckDBNull(objItem("user_id"), Common.DBConnection.DBType.DBDecimal)
                        'refkey() As integer
                        .refkey = objConn.CheckDBNull(objItem("refkey"), Common.DBConnection.DBType.DBDecimal)
                    End With
                    GetTaskSearch.Add(objTask)
                Next

            Catch ex As Exception
                ' write error log
                GetTaskSearch = Nothing
                objLog.ErrorLog("GetTaskSearch(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetTaskSearch(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        Public Function CheckTaskProcess(ByVal intUsedId As Integer) As Integer Implements ITaskDao.CheckTaskProcess
            Dim strSQL As New Text.StringBuilder
            Try
                Dim intFlag As Integer = 0
                CheckTaskProcess = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and assign parameter
                With strSQL
                    .AppendLine("SELECT count(*) AS cnt")
                    .AppendLine("FROM task a join po_header b on a.refkey=b.id")
                    .AppendLine("left join payment_header c on b.id=c.po_header_id")
                    .AppendLine("WHERE a.user_id = ?user_id")
                    .AppendLine("AND convert(a.schedule, DATE) < now()")
                    .AppendLine("AND b.status_id=4")
                    .AppendLine("and c.id is null;")
                    '.AppendLine("AND b.status_id<>6")

                    ' assign parameter
                    objConn.AddParameter("?user_id", intUsedId)

                End With

                ' execute by datatable
                intFlag = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                CheckTaskProcess = intFlag

            Catch ex As Exception
                ' write error log
                CheckTaskProcess = Nothing
                objLog.ErrorLog("CheckTaskProcess(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("CheckTaskProcess(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace

