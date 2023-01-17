#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_Payment_TermDao
'	Class Discription	: Class of table mst_payment_term
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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
Imports Common.DBConnection
Imports System.Data
Imports Extensions
Imports MySql.Data.MySqlClient
Imports Exceptions
#End Region

Namespace Dao
    Public Class ImpMst_Payment_TermDao
        Implements IMst_Payment_TermDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"

        '/**************************************************************
        '	Function name	: DB_GetListPaymentDay
        '	Discription	    : Get List Payment day
        '	Return Value	: IList
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetListPaymentDay() As System.Collections.Generic.List(Of Entity.IMst_Payment_TermEntity) Implements IMst_Payment_TermDao.DB_GetListPaymentDay
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim objDT As System.Data.DataTable
                Dim objPayment As Entity.IMst_Payment_TermEntity

                DB_GetListPaymentDay = Nothing

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine("SELECT id,case id when 1 then 'Cash' when 2 then 'Cheque' when 3 then 'By (..)' else concat(term_day,' day(s)') end term_day")
                    .AppendLine("FROM mst_payment_term")
                    .AppendLine("WHERE delete_fg <> 1")
                    .AppendLine("ORDER BY id;")
                End With

                objDT = New System.Data.DataTable
                ' execute by scalar
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetListPaymentDay = New List(Of Entity.IMst_Payment_TermEntity)
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objPayment = New Entity.ImpMst_Payment_TermEntity
                    objPayment.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objPayment.term_day = objConn.CheckDBNull(objItem("term_day"), Common.DBConnection.DBType.DBString)
                    ' add object PaymentDay entity to list
                    DB_GetListPaymentDay.Add(objPayment)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetListPaymentDay = Nothing
                objLog.ErrorLog("DB_GetListPaymentDay(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetListPaymentDay(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentList
        '	Discription	    : Get Payment list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentList(ByVal strPayment As String) _
        As System.Collections.Generic.List(Of Entity.IMst_Payment_TermEntity) Implements IMst_Payment_TermDao.GetPaymentList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentList = New List(Of Entity.IMst_Payment_TermEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objPaymentDetail As Entity.ImpMst_Payment_TermEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,term_day ")
                    .AppendLine(" FROM mst_payment_term ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine("	AND (ISNULL(?payment) OR term_day LIKE CONCAT('%', ?payment , '%'))	")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?payment", IIf(String.IsNullOrEmpty(strPayment), DBNull.Value, strPayment))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPaymentDetail = New Entity.ImpMst_Payment_TermEntity
                        ' assign data from db to entity object
                        With objPaymentDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .term_day = IIf(IsDBNull(dr.Item("term_day")), Nothing, dr.Item("term_day"))

                        End With
                        ' add item to list
                        GetPaymentList.Add(objPaymentDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count Payment Term in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intPaymentTermID As Integer _
        ) As Integer Implements IMst_Payment_TermDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job.payment_term_id) AS used_count 				")
                    .AppendLine("		FROM job_order  job 				")
                    .AppendLine("		WHERE job.payment_term_id = ?payment_term_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?payment_term_id", intPaymentTermID)

                ' execute sql command
                CountUsedInPO = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO2
        '	Discription	    : Count Payment Term in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO2( _
            ByVal intPaymentTermID As Integer _
        ) As Integer Implements IMst_Payment_TermDao.CountUsedInPO2
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO2 = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(job.payment_term_id) AS used_count 				")
                    .AppendLine("		FROM po_header  job 				")
                    .AppendLine("		WHERE job.payment_term_id = ?payment_term_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?payment_term_id", intPaymentTermID)

                ' execute sql command
                CountUsedInPO2 = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO2(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO2(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: DeletePaymentTerm
        '	Discription	    : Delete PaymentTerm
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePaymentTerm( _
            ByVal intPaymentTermID As Integer _
        ) As Integer Implements IMst_Payment_TermDao.DeletePaymentTerm
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePaymentTerm = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_payment_term                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by=?user_id,							")
                    .AppendLine("		    updated_date=date_format(now(),'%Y%m%d%H%i%s') 							")
                    .AppendLine("		WHERE id = ?id						")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intPaymentTermID)
                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                DeletePaymentTerm = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePaymentTerm(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePaymentTerm(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPaymentTermByID
        '	Discription	    : Get PaymentTerm by ID
        '	Return Value	: IMst_Payment_TermEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentTermByID( _
            ByVal intPaymentTermID As Integer _
        ) As Entity.IMst_Payment_TermEntity Implements IMst_Payment_TermDao.GetPaymentTermByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetPaymentTermByID = New Entity.ImpMst_Payment_TermEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,term_day	")
                    .AppendLine("	FROM mst_payment_term		")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intPaymentTermID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetPaymentTermByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .term_day = IIf(IsDBNull(dr.Item("term_day")), Nothing, dr.Item("term_day"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentTermByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentTermByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertPaymentTerm
        '	Discription	    : Insert PaymentTerm to mst_payment_term
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPaymentTerm( _
            ByVal objPaymentTermEntity As Entity.IMst_Payment_TermEntity _
        ) As Integer Implements IMst_Payment_TermDao.InsertPaymentTerm
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertPaymentTerm = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_payment_term (term_day					")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?term_day						")
                    .AppendLine("			,0					")
                    .AppendLine("			,?user_id					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?term_day", objPaymentTermEntity.term_day)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

                    ' begin transaction
                    .BeginTrans()

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    ' check effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        .CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        .RollbackTrans()
                    End If
                End With

                ' assign return value
                InsertPaymentTerm = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdatePaymentTerm(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPaymentTerm(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertPaymentTerm(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdatePaymentTerm
        '	Discription	    : Update PaymentTerm to mst_payment_term
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePaymentTerm( _
            ByVal objPaymentTermEntity As Entity.IMst_Payment_TermEntity _
        ) As Integer Implements IMst_Payment_TermDao.UpdatePaymentTerm
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePaymentTerm = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_payment_term 							")
                    .AppendLine("		SET term_day = ?term_day							")
                    .AppendLine("		  ,updated_by = ?user_id							")
                    .AppendLine("		  ,updated_date = date_format(now(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?term_day", objPaymentTermEntity.term_day)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objPaymentTermEntity.id)

                    ' begin transaction
                    .BeginTrans()

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    ' check effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        .CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        .RollbackTrans()
                    End If
                End With

                ' assign return value
                UpdatePaymentTerm = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdatePaymentTerm(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePaymentTerm(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdatePaymentTerm(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupInsert
        ' Discription     : Check duplication PaymentTerm Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 20-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupInsert( _
            ByVal intPayment As String _
        ) As Integer Implements IMst_Payment_TermDao.CheckDupInsert
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupInsert = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT * FROM mst_payment_term     ")
                    .AppendLine("  WHERE delete_fg <> 1     ")
                    .AppendLine("   AND term_day=?term_day  ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?term_day", intPayment)

                ' execute sql command
                CheckDupInsert = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupInsert(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupUpdate
        ' Discription     : Check duplication PaymentTerm Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 20-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupUpdate( _
            ByVal intPaymentTermID As String, _
            ByVal intPayment As String _
        ) As Integer Implements IMst_Payment_TermDao.CheckDupUpdate
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupUpdate = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT * FROM mst_payment_term    ")
                    .AppendLine("  WHERE delete_fg <> 1     ")
                    .AppendLine("   AND id<>?id  ")
                    .AppendLine("   AND term_day=?term_day  ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intPaymentTermID)
                objConn.AddParameter("?term_day", intPayment)

                ' execute sql command
                CheckDupUpdate = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupUpdate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

#End Region
    End Class

End Namespace

