#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpPlanned_PaymentDao
'	Class Discription	: Class of Planned Payment Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-08-2013
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
    Public Class ImpPlanned_PaymentDao
        Implements IPlanned_PaymentDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetYearList
        '	Discription	    : Get job order for update hontai flag
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearList() As System.Collections.Generic.List(Of Entity.IPlanned_PaymentEntity) Implements IPlanned_PaymentDao.GetYearList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetYearList = New List(Of Entity.IPlanned_PaymentEntity)
            Try
                ' object variable Planned_Payment entity
                Dim objPlannedPaymentEnt As Entity.IPlanned_PaymentEntity
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess


                'Check payment condition = 0 on job_order_po table
                With strSql
                    .Length = 0
                    .AppendLine("SELECT MIN(left(issue_date,4)) min_year, MAX(left(issue_date,4)) max_year")
                    .AppendLine("FROM job_order where status_id<>6;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new Planned Payment entity
                        objPlannedPaymentEnt = New Entity.ImpPlanned_PaymentEntity
                        ' assign data from db to entity object
                        With objPlannedPaymentEnt
                            .min_year = IIf(IsDBNull(dr.Item("min_year")), Nothing, dr.Item("min_year"))
                            .max_year = IIf(IsDBNull(dr.Item("max_year")), Nothing, dr.Item("max_year"))
                        End With
                        ' add object Vendor entity to list
                        GetYearList.Add(objPlannedPaymentEnt)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetYearList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderForReport
        '	Discription	    : Get Job order ,customer and description for Planned Payment Report
        '	Return Value	: IPlanned_PaymentEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderForReport( _
            ByVal intYear As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpPlanned_PaymentEntity) Implements IPlanned_PaymentDao.GetJobOrderForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value

            GetJobOrderForReport = New List(Of Entity.ImpPlanned_PaymentEntity)
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim objPlannedPayment As Entity.ImpPlanned_PaymentEntity

                ' assign sql command
                With strSql
                    .AppendLine("SELECT j.id, j.job_order")
                    .AppendLine("   , if(ifnull(v.abbr,'')='',v.name,v.abbr) as customer")
                    .AppendLine("   , if((j.hontai_chk1 = 1) OR (j.hontai_chk2 = 1) OR (j.hontai_chk3 = 1), concat(j.hontai_condition1,'%',j.hontai_condition2,'%', j.hontai_condition3,'%'), '100%') as description")
                    .AppendLine("FROM job_order j")
                    .AppendLine("LEFT JOIN mst_vendor v ON j.customer = v.id")
                    .AppendLine("WHERE ISNULL(?plan_year) OR YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("ORDER BY j.job_order;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?plan_year", intYear)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPlannedPayment = New Entity.ImpPlanned_PaymentEntity
                        ' assign data from db to entity object
                        With objPlannedPayment
                            .job_order_id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .description = IIf(IsDBNull(dr.Item("description")), Nothing, dr.Item("description"))
                        End With
                        ' add Job order to list
                        GetJobOrderForReport.Add(objPlannedPayment)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetInvoiceForReport
        '	Discription	    : Get Job order Id ,invoice no and receive_header_id for Planned Payment Report
        '	Return Value	: IPlanned_PaymentEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    : Wasan D.  
        '	Update Date	    : 29-10-2013
        '   Update          : SQL Command "AND h.status_id <> 6"
        '*************************************************************/
        Public Function GetInvoiceForReport( _
            ByVal intYear As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpPlanned_PaymentEntity) Implements IPlanned_PaymentDao.GetInvoiceForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value

            GetInvoiceForReport = New List(Of Entity.ImpPlanned_PaymentEntity)
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim objPlannedPayment As Entity.ImpPlanned_PaymentEntity

                ' assign sql command
                With strSql
                    .AppendLine("SELECT distinct r.job_order_id, h.id AS receive_header_id, h.invoice_no")
                    .AppendLine("FROM receive_detail r")
                    .AppendLine("Join receive_header h on r.receive_header_id = h.id")
                    .AppendLine("join job_order j on r.job_order_id = j.id")
                    .AppendLine("WHERE h.status_id <> 6")
                    .AppendLine("AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("ORDER BY r.job_order_id, h.receipt_date;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?plan_year", intYear)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPlannedPayment = New Entity.ImpPlanned_PaymentEntity
                        ' assign data from db to entity object
                        With objPlannedPayment
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .receive_header_id = IIf(IsDBNull(dr.Item("receive_header_id")), Nothing, dr.Item("receive_header_id"))
                        End With
                        ' add Job order to list
                        GetInvoiceForReport.Add(objPlannedPayment)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoiceForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetInvoiceForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumAmountThbForReport
        '	Discription	    : Get Job order Id ,pay_date and sum amount_thb for Planned Payment Report
        '	Return Value	: IPlanned_PaymentEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumAmountThbForReport( _
            ByVal intYear As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpPlanned_PaymentEntity) Implements IPlanned_PaymentDao.GetSumAmountThbForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value

            GetSumAmountThbForReport = New List(Of Entity.ImpPlanned_PaymentEntity)
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim objPlannedPayment As Entity.ImpPlanned_PaymentEntity

                ' assign sql command
                With strSql
                    .AppendLine("select job_order_id, '' pay_date, SUM(amount_thb) AS sum_amount_thb from (")
                    .AppendLine("	# Hontai 1st")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day) AS pay_date,sum(j.hontai_amount1) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg1,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg1,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# Hontai 2nd")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day) AS pay_date,sum(j.hontai_amount2) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg2,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg2,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# Hontai 3rd")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day) AS pay_date,sum(j.hontai_amount3) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg3,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg3,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# po other type : Materil, Delevery, Sample, Other")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day) AS pay_date,sum(jp.po_amount) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type <> 0 AND jp.po_fg <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type <> 0 AND jp.po_fg <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# invoice  receive")
                    .AppendLine("	SELECT rd.job_order_id,Date(rh.receipt_date) AS pay_date,sum(ifnull(rh.actual_amount,0)) amount_thb")
                    .AppendLine("	FROM receive_header rh")
                    .AppendLine("	JOIN receive_detail rd ON rd.receive_header_id = rh.id")
                    .AppendLine("	JOIN job_order j ON rd.job_order_id = j.id")
                    .AppendLine("	WHERE rh.status_id <> 6")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("	GROUP BY rd.job_order_id,rh.receipt_date")
                    .AppendLine(") A")
                    .AppendLine("GROUP BY job_order_id;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?plan_year", intYear)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPlannedPayment = New Entity.ImpPlanned_PaymentEntity
                        ' assign data from db to entity object
                        With objPlannedPayment
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .pay_date = IIf(IsDBNull(dr.Item("pay_date")), Nothing, dr.Item("pay_date"))
                            .sum_amount_thb = IIf(IsDBNull(dr.Item("sum_amount_thb")), Nothing, dr.Item("sum_amount_thb"))
                        End With
                        ' add Job order to list
                        GetSumAmountThbForReport.Add(objPlannedPayment)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumAmountThbForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumAmountThbForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAmountThbForReport
        '	Discription	    : Get Job order Id ,pay_date and amount_thb for Planned Payment Report
        '	Return Value	: IPlanned_PaymentEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAmountThbForReport( _
            ByVal intYear As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpPlanned_PaymentEntity) Implements IPlanned_PaymentDao.GetAmountThbForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value

            GetAmountThbForReport = New List(Of Entity.ImpPlanned_PaymentEntity)
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim objPlannedPayment As Entity.ImpPlanned_PaymentEntity

                ' assign sql command
                With strSql
                    .AppendLine("select job_order_id, pay_date, SUM(amount_thb) AS amount_thb from (")
                    .AppendLine("	# Hontai 1st")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day) AS pay_date,sum(j.hontai_amount1) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg1,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg1,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date1),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# Hontai 2nd")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day) AS pay_date,sum(j.hontai_amount2) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg2,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg2,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date2),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# Hontai 3rd")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day) AS pay_date,sum(j.hontai_amount3) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg3,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg3,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(j.hontai_date3),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# po other type : Materil, Delevery, Sample, Other")
                    .AppendLine("	Select E.job_order_id,E.pay_date,amt * CASE upper(H.NAME) WHEN 'THB' THEN 1 ELSE G.rate END amount_thb")
                    .AppendLine("	from (")
                    .AppendLine("		SELECT j.id AS job_order_id,j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day) AS pay_date,sum(jp.po_amount) amt")
                    .AppendLine("		FROM job_order j")
                    .AppendLine("		LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("		LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("		WHERE t.delete_fg <> 1 AND jp.po_type <> 0 AND jp.po_fg <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("		AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("		GROUP BY j.id,j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("	) E")
                    .AppendLine("	LEFT JOIN ( ")
                    .AppendLine("		SELECT Z.currency_id,Z.pay_date,Y.ef_date,ifnull(Y.rate,1) rate")
                    .AppendLine("		FROM ( ")
                    .AppendLine("			SELECT j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day) pay_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("			FROM job_order j")
                    .AppendLine("			LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("			LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("			LEFT JOIN mst_schedule_rate B ON j.currency_id = B.currency_id AND DATE(B.ef_date) <= ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("			WHERE t.delete_fg <> 1 AND jp.po_type <> 0 AND jp.po_fg <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("			GROUP BY j.currency_id,ADDDATE(DATE(jp.receipt_date),t.term_day)")
                    .AppendLine("		) Z ")
                    .AppendLine("		LEFT JOIN mst_schedule_rate Y ON Y.currency_id=Z.currency_id AND Y.ef_date=Z.max_ef_date ")
                    .AppendLine("	) G ON G.currency_id=E.currency_id AND G.pay_date=E.pay_date ")
                    .AppendLine("	LEFT JOIN mst_currency H ON E.currency_id = H.id ")
                    .AppendLine("	UNION")
                    .AppendLine("	# invoice  receive")
                    .AppendLine("	SELECT rd.job_order_id,Date(rh.receipt_date) AS pay_date,sum(ifnull(rh.actual_amount,0)) amount_thb")
                    .AppendLine("	FROM receive_header rh")
                    .AppendLine("	JOIN receive_detail rd ON rd.receive_header_id = rh.id")
                    .AppendLine("	JOIN job_order j ON rd.job_order_id = j.id")
                    .AppendLine("	WHERE rh.status_id <> 6")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("	GROUP BY rd.job_order_id,rh.receipt_date")
                    .AppendLine(") A")
                    .AppendLine("WHERE pay_date is not null")
                    .AppendLine("GROUP BY job_order_id, pay_date;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?plan_year", intYear)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objPlannedPayment = New Entity.ImpPlanned_PaymentEntity
                        ' assign data from db to entity object
                        With objPlannedPayment
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .pay_date = IIf(IsDBNull(dr.Item("pay_date")), Nothing, dr.Item("pay_date"))
                            .amount_thb = IIf(IsDBNull(dr.Item("amount_thb")), Nothing, dr.Item("amount_thb"))
                        End With
                        ' add Job order to list
                        GetAmountThbForReport.Add(objPlannedPayment)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAmountThbForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAmountThbForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMaxPayDateForReport
        '	Discription	    : Get max pay date for Planned Payment Report
        '	Return Value	: IPlanned_PaymentEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMaxPayDateForReport( _
            ByVal intYear As Integer _
        ) As Entity.IPlanned_PaymentEntity Implements IPlanned_PaymentDao.GetMaxPayDateForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value

            GetMaxPayDateForReport = New Entity.ImpPlanned_PaymentEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader 

                ' assign sql command
                With strSql
                    .AppendLine("SELECT cast(max(p.pay_date) as char) AS max_pay_date")
                    .AppendLine("FROM")
                    .AppendLine("(# Hontai 1st")
                    .AppendLine("	SELECT max(ADDDATE(DATE(j.hontai_date1),t.term_day)) AS pay_date")
                    .AppendLine("	FROM job_order j")
                    .AppendLine("	LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("	LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("	WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg1,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("UNION")
                    .AppendLine("# Hontai 2nd")
                    .AppendLine("	SELECT max(ADDDATE(DATE(j.hontai_date2),t.term_day)) AS pay_date")
                    .AppendLine("	FROM job_order j")
                    .AppendLine("	LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("	LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("	WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg2,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("UNION")
                    .AppendLine("# Hontai 3rd")
                    .AppendLine("	SELECT max(ADDDATE(DATE(j.hontai_date3),t.term_day)) AS pay_date")
                    .AppendLine("	FROM job_order j")
                    .AppendLine("	LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("	LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("	WHERE t.delete_fg <> 1 AND jp.po_type = 0 AND ifnull(jp.hontai_fg3,0) <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("UNION")
                    .AppendLine("# po other type : Materil, Delevery, Sample, Other")
                    .AppendLine("	SELECT max(ADDDATE(DATE(jp.receipt_date),t.term_day)) AS pay_date")
                    .AppendLine("	FROM job_order j")
                    .AppendLine("	LEFT JOIN job_order_po jp ON jp.job_order_id = j.id")
                    .AppendLine("	LEFT JOIN mst_payment_term t ON j.payment_term_id = t.id")
                    .AppendLine("	WHERE t.delete_fg <> 1 AND jp.po_type <> 0 AND jp.po_fg <> 1 AND jp.delete_fg <> 1")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine("UNION")
                    .AppendLine("# invoice receive")
                    .AppendLine("	SELECT max(DATE(rh.receipt_date)) AS pay_date")
                    .AppendLine("	FROM receive_header rh")
                    .AppendLine("	INNER JOIN receive_detail rd ON rd.receive_header_id = rh.id")
                    .AppendLine("	INNER JOIN job_order j ON rd.job_order_id = j.id")
                    .AppendLine("	WHERE rh.status_id <> 6")
                    .AppendLine("	AND YEAR(DATE(j.issue_date)) = ?plan_year")
                    .AppendLine(") AS p;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?plan_year", intYear)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetMaxPayDateForReport
                            .max_pay_date = IIf(IsDBNull(dr.Item("max_pay_date")), Nothing, dr.Item("max_pay_date"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaxPayDateForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetMaxPayDateForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
 
    End Class
End Namespace
