#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_ScheduleRateDao
'	Class Discription	: Class of table Mst_Schedule_Rate
'	Create User 		: Boon
'	Create Date		    : 02-07-2013
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
Imports Entity
'Imports MySql.Data.MySqlClient
Imports System.Data
#End Region

Namespace Dao
    Public Class ImpMst_ScheduleRateDao
        Implements IMst_ScheduleRateDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_GetScheduleRateByPurchase
        '	Discription	    : Get data schedulr_rate
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetScheduleRateByPurchase(ByVal intCurrency_id As Integer, Optional ByVal strDate As String = "") As Decimal Implements IMst_ScheduleRateDao.DB_GetScheduleRateByPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intRate As Decimal = 0

                If strDate = String.Empty Then
                    strDate = Now.ToString("yyyyMMdd")
                End If

                DB_GetScheduleRateByPurchase = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT 1.00000 as rate from mst_currency where id=?currency_id and name='THB' ")
                    .AppendLine(" union ")
                    .AppendLine(" SELECT * from ( ")
                    .AppendLine(" SELECT rate ")
                    .AppendLine(" FROM mst_schedule_rate ")
                    .AppendLine(" WHERE delete_fg = 0 ")
                    .AppendLine(" 	AND currency_id = ?currency_id ")
                    .AppendLine(" 	AND ef_date <= ?issue_date ")
                    .AppendLine(" 	AND month(ef_date) = substr(?issue_date,5,2) ")
                    .AppendLine(" ORDER BY ef_date DESC limit 1 ")
                    .AppendLine(" ) A;")
                    ' assign parameter
                    objConn.AddParameter("?currency_id", intCurrency_id)
                    objConn.AddParameter("?issue_date", strDate)
                End With

                ' execute by scalar
                intRate = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intRate > 0 Then
                    DB_GetScheduleRateByPurchase = intRate
                End If

            Catch ex As Exception
                ' write error log
                DB_GetScheduleRateByPurchase = 0
                objLog.ErrorLog("DB_GetScheduleRateByPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetScheduleRateByPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        'Private Function getDataSet(ByVal ds_name As String, ByVal sql As String, ByRef ds As DataSet) As Integer
        '    Dim rtn As Integer = 0
        '    Using conn As New MySql.Data.MySqlClient.MySqlConnection([String].Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}", _
        '            "192.168.1.45", "test", "root", "1234"))
        '        Try
        '            If conn.State <> ConnectionState.Open Then
        '                conn.Open()
        '            End If
        '            Dim adp As New MySqlDataAdapter()
        '            adp.SelectCommand = New MySqlCommand(sql, conn)
        '            adp.Fill(ds, ds_name)
        '            rtn = ds.Tables(ds_name).Rows.Count
        '        Catch ex As Exception
        '            Console.WriteLine("{0}" & vbTab & "Error : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message)
        '        Finally
        '            If conn.State = ConnectionState.Open Then
        '                conn.Close()
        '            End If
        '        End Try
        '    End Using
        '    Return rtn
        'End Function

        '/**************************************************************
        '	Function name	: DB_GetScheduleRateByCurrency
        '	Discription	    : Get data schedulr_rate
        '	Return Value	: Entity IMst_ScheduleRateEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetScheduleRateByCurrency(ByVal intCurrency_id As Integer, _
                                                     Optional ByVal strEFDate As String = "", _
                                                     Optional ByVal intRanking As Integer = 11 _
                        ) As System.Collections.Generic.List(Of Entity.IMst_ScheduleRateEntity) Implements IMst_ScheduleRateDao.DB_GetScheduleRateByCurrency
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objSRate As ImpMst_ScheduleRateEntity

                DB_GetScheduleRateByCurrency = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objDT = New System.Data.DataTable
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL

                    If intCurrency_id = 0 Then
                        .AppendLine(" SELECT RowID, A.id ")
                        .AppendLine(" 	,A.currency_id ")
                        .AppendLine(" 	,B.NAME As currency ")
                        .AppendLine(" 	,ef_date ")
                        .AppendLine(" 	,rate ")
                        .AppendLine(" FROM ( ")
                        .AppendLine(" 	SELECT @ROWNUM:=IF(@GH=currency_id, @ROWNUM + 1, 1) AS RowID,@GH:=currency_id AS DUMMY,A.* ")
                        .AppendLine(" 		FROM mst_schedule_rate A,(select @ROWNUM:=0) B ")
                        .AppendLine(" 		WHERE delete_fg = 0 ")
                        If (Not strEFDate Is Nothing) AndAlso strEFDate.Trim <> String.Empty Then
                            .AppendLine(" 			AND(ef_date = ?ef_date OR ?ef_date = '') ")
                        End If
                        .AppendLine(" 		ORDER BY currency_id ")
                        .AppendLine(" 			,ef_date DESC ")
                        .AppendLine(" 	) A ")
                        .AppendLine(" LEFT JOIN mst_currency B ON A.currency_id = B.id ")
                        .AppendLine(" WHERE RowID < ?ranking; ")

                        ' assign parameter
                        If (Not strEFDate Is Nothing) AndAlso strEFDate.Trim <> String.Empty Then
                            objConn.AddParameter("?ef_date", strEFDate.Trim)
                        End If
                        objConn.AddParameter("?ranking", intRanking)

                        ' execute by datatable
                        objDT = objConn.ExecuteDataTable(strSQL.ToString)
                        objDT.Clear()

                        ' assign parameter
                        If (Not strEFDate Is Nothing) AndAlso strEFDate.Trim <> String.Empty Then
                            objConn.AddParameter("?ef_date", strEFDate.Trim)
                        End If
                        objConn.AddParameter("?ranking", intRanking)

                        ' execute by datatable
                        objDT = objConn.ExecuteDataTable(strSQL.ToString)
                        strMsgErr = objConn.MessageError
                    Else
                        .AppendLine(" SELECT A.id ")
                        .AppendLine(" 	,A.currency_id ")
                        .AppendLine(" 	,B.NAME As currency ")
                        .AppendLine(" 	,ef_date ")
                        .AppendLine(" 	,rate ")
                        .AppendLine(" FROM mst_schedule_rate A ")
                        .AppendLine(" LEFT JOIN mst_currency B ON A.currency_id = B.id ")
                        .AppendLine(" WHERE A.delete_fg <> 1 ")
                        .AppendLine(" 	AND A.currency_id = ?currency_id ")
                        If (Not strEFDate Is Nothing) AndAlso strEFDate.Trim <> String.Empty Then
                            .AppendLine(" 	AND (ef_date = ?ef_date OR ?ef_date = '') ")
                        End If
                        .AppendLine(" ORDER BY A.ef_date DESC; ")

                        ' assign parameter
                        objConn.AddParameter("?currency_id", intCurrency_id)
                        If (Not strEFDate Is Nothing) AndAlso strEFDate.Trim <> String.Empty Then
                            objConn.AddParameter("?ef_date", strEFDate.Trim)
                        End If

                        ' execute by datatable
                        objDT = objConn.ExecuteDataTable(strSQL.ToString)
                        strMsgErr = objConn.MessageError
                    End If

                    '.AppendLine(" SELECT A.id ")
                    '.AppendLine(" 	,A.currency_id ")
                    '.AppendLine(" 	,B.name As currency ")
                    '.AppendLine(" 	,A.ef_date ")
                    '.AppendLine(" 	,A.rate ")
                    '.AppendLine(" FROM mst_schedule_rate A ")
                    '.AppendLine(" LEFT JOIN mst_currency B ON A.currency_id = B.id ")
                    '.AppendLine(" WHERE A.delete_fg <> 1 ")
                    'If intCurrency_id > 0 Then
                    '    .AppendLine(" 	AND A.currency_id = ?currency_id ")
                    '    ' assign parameter
                    '    objConn.AddParameter("?currency_id", intCurrency_id)
                    'End If
                    '.AppendLine(" ORDER BY A.ef_date DESC; ")
                End With

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetScheduleRateByCurrency = New List(Of IMst_ScheduleRateEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objSRate = New Entity.ImpMst_ScheduleRateEntity
                    With objSRate
                        .id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                        .currency_id = objConn.CheckDBNull(objItem("currency_id"), Common.DBConnection.DBType.DBDecimal)
                        .currency = objConn.CheckDBNull(objItem("currency"), Common.DBConnection.DBType.DBString).ToString
                        .ef_date = objConn.CheckDBNull(objItem("ef_date"), Common.DBConnection.DBType.DBString).ToString
                        .rate = objConn.CheckDBNull(objItem("rate"), Common.DBConnection.DBType.DBDecimal)
                    End With
                    DB_GetScheduleRateByCurrency.Add(objSRate)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetScheduleRateByCurrency = Nothing
                objLog.ErrorLog("DB_GetScheduleRateByCurrency(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetScheduleRateByCurrency(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CancelScheduleRate
        '	Discription	    : Cancel data schedulr_rate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CancelScheduleRate(ByVal intScheduleRate_id As Integer) As Boolean Implements IMst_ScheduleRateDao.DB_CancelScheduleRate
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_CancelScheduleRate = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_schedule_rate ")
                    .AppendLine(" SET delete_fg = 1 ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?id ")
                    ' assign parameter
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?id", intScheduleRate_id)

                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_CancelScheduleRate = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CancelScheduleRate = False
                objLog.ErrorLog("DB_CancelScheduleRate(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CancelScheduleRate(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CheckIsDupScheduleRate
        '	Discription	    : Check data schedulr_rate is duplicate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckIsDupScheduleRate(ByVal intCurrency_id As Integer, ByVal strEF_date As String, Optional ByVal intScheduleRate_id As Integer = 0) As Boolean Implements IMst_ScheduleRateDao.DB_CheckIsDupScheduleRate
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckIsDupScheduleRate = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) ")
                    .AppendLine(" FROM mst_schedule_rate ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" 	AND currency_id = ?currency_id ")
                    .AppendLine(" 	AND ef_date = ?ef_date ")

                    ' assign parameter
                    objConn.AddParameter("?currency_id", intCurrency_id)
                    objConn.AddParameter("?ef_date", strEF_date)

                    ' check value ScheduleRate_id
                    If intScheduleRate_id > 0 Then
                        .AppendLine(" 	AND id <> ?id ")
                        objConn.AddParameter("?id", intScheduleRate_id)
                    End If
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckIsDupScheduleRate = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckIsDupScheduleRate = False
                objLog.ErrorLog("DB_CheckIsDupScheduleRate(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckIsDupScheduleRate(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_InsertScheduleRate
        '	Discription	    : Insert data schedulr_rate 
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_InsertScheduleRate(ByVal objScheduleRate As Entity.IMst_ScheduleRateEntity) As Integer Implements IMst_ScheduleRateDao.DB_InsertScheduleRate
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_InsertScheduleRate = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" INSERT INTO mst_schedule_rate ( ")
                    .AppendLine(" 	currency_id ")
                    .AppendLine(" 	,ef_date ")
                    .AppendLine(" 	,rate ")
                    .AppendLine(" 	,delete_fg ")
                    .AppendLine(" 	,created_by ")
                    .AppendLine(" 	,created_date ")
                    .AppendLine(" 	,updated_by ")
                    .AppendLine(" 	,updated_date ")
                    .AppendLine(" 	) ")
                    .AppendLine(" VALUES ( ")
                    .AppendLine(" 	?currency_id ")
                    .AppendLine(" 	,?ef_date ")
                    .AppendLine(" 	,?rate ")
                    .AppendLine(" 	,0 ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	) ")

                End With

                ' assign parameter
                With objScheduleRate
                    objConn.AddParameter("?currency_id", .currency_id)
                    objConn.AddParameter("?ef_date", .ef_date.ToString)
                    objConn.AddParameter("?rate", .rate)
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_InsertScheduleRate = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_InsertScheduleRate = 0
                objLog.ErrorLog("DB_InsertScheduleRate(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_InsertScheduleRate(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetScheduleRateId
        '	Discription	    : Get data schedulr_rate_id
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function DB_GetScheduleRateId(ByVal objScheduleRate As Entity.IMst_ScheduleRateEntity) As Integer
            Dim strSQL As New Text.StringBuilder
            Try
                Dim intFlag As Integer = 0
                DB_GetScheduleRateId = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" FROM mst_schedule_rate ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" 	AND currency_id = ?currency_id ")
                    .AppendLine(" 	AND ef_date = ?ef_date ")
                    .AppendLine(" 	AND rate = ?rate ")
                End With

                ' assign parameter
                With objScheduleRate
                    objConn.AddParameter("?currency_id", .currency_id)
                    objConn.AddParameter("?ef_date", .ef_date)
                    objConn.AddParameter("?rate", .rate)
                End With

                ' execute by ExecuteScalar
                intFlag = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If intFlag > 0 Then
                    DB_GetScheduleRateId = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_GetScheduleRateId = 0
                objLog.ErrorLog("DB_GetScheduleRateId(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetScheduleRateId(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetScheduleRateById
        '	Discription	    : Get data schedulr_rate by schedulr_rate_id
        '	Return Value	: IMst_ScheduleRateEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetScheduleRateById(ByVal intScheduleRate_id As Integer) As Entity.IMst_ScheduleRateEntity Implements IMst_ScheduleRateDao.DB_GetScheduleRateById
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objDR As System.Data.DataRow

                DB_GetScheduleRateById = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,currency_id ")
                    .AppendLine(" 	,ef_date ")
                    .AppendLine(" 	,rate ")
                    .AppendLine(" FROM mst_schedule_rate ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" 	AND id = ?id ")

                    objConn.AddParameter("?id", intScheduleRate_id)

                End With

                ' set new object
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                objDR = objDT.Rows(0)

                DB_GetScheduleRateById = New ImpMst_ScheduleRateEntity
                ' assign value to entity object
                With DB_GetScheduleRateById
                    .id = objConn.CheckDBNull(objDR("id"), Common.DBConnection.DBType.DBDecimal)
                    .currency_id = objConn.CheckDBNull(objDR("currency_id"), Common.DBConnection.DBType.DBDecimal)
                    .ef_date = objConn.CheckDBNull(objDR("ef_date"), Common.DBConnection.DBType.DBString).ToString
                    .rate = objConn.CheckDBNull(objDR("rate"), Common.DBConnection.DBType.DBDecimal)

                End With

            Catch ex As Exception
                ' write error log
                DB_GetScheduleRateById = Nothing
                objLog.ErrorLog("DB_GetScheduleRateById(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetScheduleRateById(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdateScheduleRate
        '	Discription	    : Update data schedulr_rate 
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdateScheduleRate(ByVal objScheduleRate As Entity.IMst_ScheduleRateEntity) As Integer Implements IMst_ScheduleRateDao.DB_UpdateScheduleRate
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_UpdateScheduleRate = 0

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_schedule_rate ")
                    .AppendLine(" SET currency_id = ?currency_id ")
                    .AppendLine(" 	,ef_date = ?ef_date ")
                    .AppendLine(" 	,rate = ?rate ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?id ")
                End With

                ' assign parameter
                With objScheduleRate
                    objConn.AddParameter("?currency_id", .currency_id)
                    objConn.AddParameter("?ef_date", .ef_date.ToString)
                    objConn.AddParameter("?rate", .rate)
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID").ToString.Trim)
                    objConn.AddParameter("?id", .id)
                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_UpdateScheduleRate = intFlag
                End If

            Catch ex As Exception
                ' write error log
                DB_UpdateScheduleRate = 0
                objLog.ErrorLog("DB_UpdateScheduleRate(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_UpdateScheduleRate(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


    End Class
End Namespace

