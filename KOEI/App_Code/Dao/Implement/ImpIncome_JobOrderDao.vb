#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpIncome_JobOrderDao
'	Class Discription	: Class of table receive_header
'	Create User 		: Suwishaya L.
'	Create Date		    : 01-07-2013
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
Imports MySql.Data.MySqlClient
#End Region

Namespace Dao
    Public Class ImpIncome_JobOrderDao
        Implements IIncome_JobOrderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: GetIncomeList
        '	Discription	    : Get income list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIncomeList( _
            ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpIncome_JobOrderEntity) Implements IIncome_JobOrderDao.GetIncomeList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetIncomeList = New List(Of Entity.ImpIncome_JobOrderEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objIncome As Entity.ImpIncome_JobOrderEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   DISTINCT rh.id ")
                    .AppendLine("   , rh.invoice_no ")
                    .AppendLine("   , (CASE invoice_type WHEN 1 THEN 'IN' WHEN 2 THEN 'IV' WHEN 3 THEN 'IS' ELSE '' END) AS invoice_type ")
                    .AppendLine("   , rh.issue_date ")
                    .AppendLine("   , rh.receipt_date ")
                    .AppendLine("   , v.name AS customer ")
                    .AppendLine("   , rh.total_amount ")
                    .AppendLine(" FROM receive_header rh  ")
                    .AppendLine(" LEFT JOIN mst_vendor v  ")
                    .AppendLine(" ON rh.vendor_id = v.id  ")
                    .AppendLine(" AND v.type1 = 1 ")
                    .AppendLine(" LEFT JOIN  ")
                    '.AppendLine("   (SELECT DISTINCT job_order ")
                    '.AppendLine("   , receive_header_id FROM receive_detail) rd  ")

                    .AppendLine("    (SELECT DISTINCT d.job_order_id, d.receive_header_id, j.job_order  ")
                    .AppendLine("   FROM receive_detail d ")
                    .AppendLine("   INNER JOIN job_order j ON d.job_order_id = j.id ")
                    .AppendLine("   WHERE j.status_id <> 6 ) rd ")

                    .AppendLine(" ON rh.id = rd.receive_header_id ")
                    .AppendLine(" WHERE rh.status_id= 4  ")
                    .AppendLine(" AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine(" AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine(" AND ((ISNULL(?job_order_from) OR rd.job_order >= ?job_order_from)    ")
                    .AppendLine(" AND (ISNULL(?job_order_to) OR rd.job_order <= ?job_order_to))  ")
                    .AppendLine(" AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine(" AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine(" AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine(" AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objIncomeEnt.invoice_no_search), DBNull.Value, objIncomeEnt.invoice_no_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objIncomeEnt.customer_search), DBNull.Value, objIncomeEnt.customer_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_from_search), DBNull.Value, objIncomeEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_to_search), DBNull.Value, objIncomeEnt.job_order_to_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_from_search), DBNull.Value, objIncomeEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_to_search), DBNull.Value, objIncomeEnt.issue_date_to_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_from_search), DBNull.Value, objIncomeEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_to_search), DBNull.Value, objIncomeEnt.receipt_date_to_search))
                 
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objIncome = New Entity.ImpIncome_JobOrderEntity
                        ' assign data from db to entity object
                        With objIncome
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .invoice_type_name = IIf(IsDBNull(dr.Item("invoice_type")), Nothing, dr.Item("invoice_type"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                        End With
                        ' add income job order to list
                        GetIncomeList.Add(objIncome)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetIncomeList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetIncomeList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMonthlySaleReport
        '	Discription	    : Get data for Monthly Sale Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMonthlySaleReport( _
            ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpIncome_JobOrderEntity) Implements IIncome_JobOrderDao.GetMonthlySaleReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetMonthlySaleReport = New List(Of Entity.ImpIncome_JobOrderEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objIncome As Entity.ImpIncome_JobOrderEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT 				 ")
                    .AppendLine("			rd.id			 ")
                    .AppendLine("			, rh.id AS receive_header_id			 ")
                    .AppendLine("			,  rd.job_order_id			 ")
                    .AppendLine("			, rh.issue_date			 ")
                    .AppendLine("			, rh.invoice_no			 ")
                    .AppendLine("			,  v.name AS customer			 ")
                    .AppendLine("			, j.job_order			 ")
                    .AppendLine("			, IFNULL(rd.actual_rate,1) AS actual_rate			 ")
                    .AppendLine("			, IFNULL(rd.amount,0) AS price			 ")
                    .AppendLine("			, IFNULL(rd.vat_amount,0) AS vat			 ")
                    .AppendLine("			, (IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) + IFNULL(rd.vat_amount,0) as amount_thb			 ")
                    .AppendLine("			, rd.remark			 ")
                    .AppendLine("		FROM receive_header rh 				 ")
                    .AppendLine("		INNER JOIN receive_detail rd 				 ")
                    .AppendLine("		ON rh.id = rd.receive_header_id 				 ")
                    .AppendLine("		LEFT JOIN mst_vendor v 				 ")
                    .AppendLine("		ON rh.vendor_id = v.id  				 ")
                    .AppendLine("		AND v.delete_fg <> 1				 ")
                    .AppendLine("		LEFT JOIN job_order_po po 				 ")
                    .AppendLine("		ON rd.job_order_po_id = po.id 				 ")
                    .AppendLine("		AND po.delete_fg <> 1				 ")
                    .AppendLine("		LEFT JOIN job_order j 				 ")
                    .AppendLine("		ON rd.job_order_id = j.id  				 ")
                    .AppendLine("		AND j.status_id <> 6				 ")
                    .AppendLine("		LEFT JOIN mst_payment_condition pc 				 ")
                    .AppendLine("		ON j.payment_condition_id = pc.id 				 ")
                    .AppendLine("		AND pc.delete_fg <> 1				 ")
                    .AppendLine("		WHERE rh.status_id = 4 				 ")
                    .AppendLine("		AND v.type1 = 1				 ")
                    .AppendLine("       AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine("       AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine("       AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine("       AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("       AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine("       AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                    .AppendLine("       AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("       AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("		ORDER BY rh.issue_date 				 ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objIncomeEnt.invoice_no_search), DBNull.Value, objIncomeEnt.invoice_no_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_from_search), DBNull.Value, objIncomeEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_to_search), DBNull.Value, objIncomeEnt.issue_date_to_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objIncomeEnt.customer_search), DBNull.Value, objIncomeEnt.customer_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_from_search), DBNull.Value, objIncomeEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_to_search), DBNull.Value, objIncomeEnt.receipt_date_to_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_from_search), DBNull.Value, objIncomeEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_to_search), DBNull.Value, objIncomeEnt.job_order_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objIncome = New Entity.ImpIncome_JobOrderEntity
                        ' assign data from db to entity object
                        With objIncome
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .receive_header_id = IIf(IsDBNull(dr.Item("receive_header_id")), Nothing, dr.Item("receive_header_id"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .price = IIf(IsDBNull(dr.Item("price")), Nothing, dr.Item("price"))
                            .vat = IIf(IsDBNull(dr.Item("vat")), Nothing, dr.Item("vat"))
                            .amount = IIf(IsDBNull(dr.Item("amount_thb")), Nothing, dr.Item("amount_thb"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), Nothing, dr.Item("actual_rate"))
                        End With
                        ' add Job order to list
                        GetMonthlySaleReport.Add(objIncome)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMonthlySaleReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetMonthlySaleReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumMonthlySaleReport
        '	Discription	    : Get data for Sum Monthly Sale Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumMonthlySaleReport( _
            ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpIncome_JobOrderEntity) Implements IIncome_JobOrderDao.GetSumMonthlySaleReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumMonthlySaleReport = New List(Of Entity.ImpIncome_JobOrderEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objIncome As Entity.ImpIncome_JobOrderEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT SUM(IFNULL(rd.amount,0)) AS sum_price						 ")
                    .AppendLine("		, SUM(IFNULL(rd.vat_amount,0)) AS sum_vat						 ")
                    .AppendLine("		, SUM((IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) + IFNULL(rd.vat_amount,0)) as sum_amount_thb						 ")
                    .AppendLine("		FROM receive_header rh 						 ")
                    .AppendLine("		INNER JOIN receive_detail rd 						 ")
                    .AppendLine("		ON rh.id = rd.receive_header_id 						 ")
                    .AppendLine("		LEFT JOIN mst_vendor v 						 ")
                    .AppendLine("		ON rh.vendor_id = v.id  						 ")
                    .AppendLine("		AND v.delete_fg <> 1						 ")
                    .AppendLine("		LEFT JOIN job_order_po po 						 ")
                    .AppendLine("		ON rd.job_order_po_id = po.id 						 ")
                    .AppendLine("		AND po.delete_fg <> 1						 ")
                    .AppendLine("		LEFT JOIN job_order j 						 ")
                    .AppendLine("		ON rd.job_order_id = j.id  						 ")
                    .AppendLine("		AND j.status_id <> 6						 ")
                    .AppendLine("		WHERE rh.status_id = 4 				 ")
                    .AppendLine("		AND v.type1 = 1				 ")
                    .AppendLine("       AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine("       AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine("       AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine("       AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("       AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine("       AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                    .AppendLine("       AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("       AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("		ORDER BY rh.issue_date 				 ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objIncomeEnt.invoice_no_search), DBNull.Value, objIncomeEnt.invoice_no_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_from_search), DBNull.Value, objIncomeEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.issue_date_to_search), DBNull.Value, objIncomeEnt.issue_date_to_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objIncomeEnt.customer_search), DBNull.Value, objIncomeEnt.customer_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_from_search), DBNull.Value, objIncomeEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objIncomeEnt.receipt_date_to_search), DBNull.Value, objIncomeEnt.receipt_date_to_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_from_search), DBNull.Value, objIncomeEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objIncomeEnt.job_order_to_search), DBNull.Value, objIncomeEnt.job_order_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objIncome = New Entity.ImpIncome_JobOrderEntity
                        ' assign data from db to entity object
                        With objIncome
                            .sum_amount = IIf(IsDBNull(dr.Item("sum_amount_thb")), Nothing, dr.Item("sum_amount_thb"))
                            .sum_price = IIf(IsDBNull(dr.Item("sum_price")), Nothing, dr.Item("sum_price"))
                            .sum_vat = IIf(IsDBNull(dr.Item("sum_vat")), Nothing, dr.Item("sum_vat"))
                        End With
                        ' add Job order to list
                        GetSumMonthlySaleReport.Add(objIncome)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumMonthlySaleReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumMonthlySaleReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region

    End Class
End Namespace

