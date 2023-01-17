#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpSale_InvoiceDao
'	Class Discription	: Class of Sale Invoice
'	Create User 		: Suwishaya L.
'	Create Date		    : 24-07-2013
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
    Public Class ImpSale_InvoiceDao
        Implements ISale_InvoiceDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"

        '/**************************************************************
        '	Function name	: CountUsedInAccounting
        '	Discription	    : Count item in used accounting
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInAccounting( _
            ByVal intRefID As Integer _
        ) As Integer Implements ISale_InvoiceDao.CountUsedInAccounting
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInAccounting = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS cnt 				    ")
                    .AppendLine("		FROM accounting  				        ")
                    .AppendLine("		WHERE type = 4			                ")
                    .AppendLine("		AND ref_id = ?receive_header_id			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", intRefID)

                ' execute sql command
                CountUsedInAccounting = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInReceiveHeader
        '	Discription	    : Count item in used receive_header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInReceiveHeader( _
            ByVal strInvoice_no As String _
        ) As Integer Implements ISale_InvoiceDao.CountUsedInReceiveHeader
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInReceiveHeader = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(id) AS cnt 				")
                    .AppendLine("		FROM receive_header  				")
                    .AppendLine("		WHERE invoice_no = ?invoice_no			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", strInvoice_no)

                ' execute sql command
                CountUsedInReceiveHeader = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInReceiveHeader(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInReceiveHeader(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteSaleInvoice
        '	Discription	    : Delete Sale Invoice
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    : Rawikarn K.
        '	Update Date	    : 11-03-2014
        '*************************************************************/
        Public Function DeleteSaleInvoice( _
            ByVal intRefID As Integer, _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements ISale_InvoiceDao.DeleteSaleInvoice
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteSaleInvoice = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' assign sql command
                'With strSql
                '    .Length = 0
                '    .AppendLine("       UPDATE receive_header                             ")
                '    .AppendLine("		SET status_id = 6,							")
                '    .AppendLine("		    updated_by = ?update_by,							")
                '    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                '    .AppendLine("		WHERE id = ?id							")
                'End With

                With strSql
                    .Length = 0
                    .AppendLine("       DELETE FROM receive_header   ")
                    .AppendLine("       WHERE id = ?id  ")

                End With

                objLog.InfoLog("Receive_HeaderID:", intRefID)

                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intRefID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)
                If intEff <= 0 Then
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                ' loop table for update data
                For Each row As DataRow In dtValues.Rows
                    '1)Update flag on job_order_po table on case delete
                    'Check hontai type
                    Select Case row("hontai_type").ToString
                        Case "1" 'Hontai_fg1
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                             ")
                                .AppendLine("		SET hontai_fg1 = 0 							")
                                .AppendLine("		WHERE id = ?job_order_po_id							")
                            End With
                        Case "2" 'Hontai_fg2
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                             ")
                                .AppendLine("		SET hontai_fg2 = 0 							")
                                .AppendLine("		WHERE id = ?job_order_po_id							")
                            End With
                        Case "3" 'Hontai_fg3
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                             ")
                                .AppendLine("		SET hontai_fg3 = 0 							")
                                .AppendLine("		WHERE id = ?job_order_po_id							")
                            End With
                        Case Else
                            'po_fg
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                             ")
                                .AppendLine("		SET po_fg = 0 							")
                                .AppendLine("		WHERE id = ?job_order_po_id  ")
                            End With
                    End Select

                    ' assign parameter
                    objConn.ClearParameter()
                    objConn.AddParameter("?job_order_po_id", row("job_order_po_id"))

                    ' execute non query and keep row effect
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)
                    If intEff <= 0 Then
                        ' case row effect <= 0 then rollback transaction
                        objConn.RollbackTrans()
                        Exit For
                    End If

                    'Add 2013/09/28 (Add request by user ) Start
                    '2)Update flag finish goods on case delete 
                    With strSql
                        .Length = 0
                        .AppendLine("		UPDATE job_order 			")
                        .AppendLine("		SET finish_fg = 0 			")
                        .AppendLine("		, updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                        .AppendLine("		, updated_by = ?user_id 			")
                        .AppendLine("		WHERE (id = ?job_order_id);")
                    End With

                    objConn.ClearParameter()
                    objConn.AddParameter("?job_order_id", row("job_order_id"))
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                    ' execute nonquery with sql command (update 
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)

                    ' check row effect 
                    If intEff <= 0 Then
                        ' case have error then exit for
                        Exit For
                    End If

                    'Add 2013/09/28 (Add request by user ) End

                Next

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                DeleteSaleInvoice = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteSaleInvoice(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteSaleInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveSaleInvoice
        '	Discription	    : Save Sale Invoice
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 25-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveSaleInvoice( _
            ByVal intRefID As Integer, _
            ByVal decExchangeRate As Decimal, _
            ByVal decActualAmount As Decimal, _
            ByVal strJobOrder As String _
        ) As Integer Implements ISale_InvoiceDao.SaveSaleInvoice
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            SaveSaleInvoice = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' assign sql command
                With strSql
                    .Length = 0
                    .AppendLine("       UPDATE receive_header                             ")
                    .AppendLine("		    SET bank_rate  = ?bank_rate,							")
                    .AppendLine("		    actual_amount = ?actual_amount,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With

                ' assign parameter
                objConn.ClearParameter()
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intRefID)
                objConn.AddParameter("?bank_rate", decExchangeRate)
                objConn.AddParameter("?actual_amount", decActualAmount)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)
                If intEff <= 0 Then
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If


                ''Update finish_fg on job order table               
                'If strJobOrder <> "" Then

                '    With strSql
                '        .Length = 0
                '        .AppendLine("		UPDATE job_order    			")
                '        .AppendLine("		SET finish_fg = 1 			")
                '        .AppendLine("		,finish_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                '        .AppendLine("		,updated_by = ?user_id							")
                '        .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                '        .AppendLine("		WHERE  	FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                '    End With


                '    objConn.ClearParameter()
                '    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                '    objConn.AddParameter("?id", strJobOrder)

                '    ' execute nonquery with sql command (update 
                '    intEff = objConn.ExecuteNonQuery(strSql.ToString)

                '    If intEff <= 0 Then
                '        ' case row effect <= 0 then rollback transaction
                '        objConn.RollbackTrans()
                '        Exit Function
                '    End If
                'End If


                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                SaveSaleInvoice = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SaveSaleInvoice(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("SaveSaleInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceHeaderByID
        '	Discription	    : Get Sale Invoice Header by ID
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceHeaderByID( _
            ByVal intRefID As Integer _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSaleInvoiceHeaderByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetSaleInvoiceHeaderByID = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	rh.id	")
                    .AppendLine("   , rh.invoice_no ")
                    .AppendLine("   , rh.issue_date ")
                    .AppendLine("   , rh.receipt_date ")
                    .AppendLine("   , rh.ie_id ")
                    .AppendLine("   , rh.vendor_id ")
                    .AppendLine("   , rh.account_type ")
                    .AppendLine("   , rh.invoice_type ")
                    .AppendLine("   , rh.bank_fee ")
                    .AppendLine("   , rh.total_amount ")
                    .AppendLine("	FROM receive_header	rh	")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intRefID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSaleInvoiceHeaderByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .invoice_type = IIf(IsDBNull(dr.Item("invoice_type")), Nothing, dr.Item("invoice_type"))
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))
                            .total_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceHeaderByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceHeaderByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceDetailByID
        '	Discription	    : Get Sale Invoice Detail by ID
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceDetailByID( _
            ByVal intRefID As Integer _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSaleInvoiceDetailByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetSaleInvoiceDetailByID = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	rd.id	")
                    .AppendLine("   , rd.job_order_id ")
                    .AppendLine("   , j.job_order ")
                    .AppendLine("   , j.issue_date ")
                    .AppendLine("   , c.name AS currency ")
                    .AppendLine("   , rd.actual_rate ")
                    .AppendLine("   , j.currency_id ")
                    .AppendLine("   FROM receive_detail rd ")
                    .AppendLine("   LEFT JOIN job_order j  ")
                    .AppendLine("   ON rd.job_order_id = j.id  ")
                    .AppendLine("   AND j.status_id <> 6 ")
                    .AppendLine("   LEFT JOIN mst_currency c  ")
                    .AppendLine("   ON j.currency_id = c.id  ")
                    .AppendLine("   AND c.delete_fg <> 1 ")
                    .AppendLine("	WHERE rd.receive_header_id = ?id		")
                    .AppendLine("   ORDER BY rd.created_date DESC LIMIT 1 ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intRefID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSaleInvoiceDetailByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .currency = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), Nothing, dr.Item("actual_rate"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceDetailByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceDetailByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceList
        '	Discription	    : Get sale invoice list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    : Rawikarn K.
        '	Update Date	    : 28-05-2014
        '*************************************************************/
        Public Function GetSaleInvoiceList( _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetSaleInvoiceList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSaleInvoiceList = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT ")
                    .AppendLine("       rh.id ")
                    .AppendLine("       , rh.invoice_no ")
                    .AppendLine("       , (CASE rh.invoice_type WHEN 1 THEN 'IN' WHEN 2 THEN 'IV' ELSE '' END) AS invoice_type ")
                    .AppendLine("       , rh.issue_date ")
                    .AppendLine("       , rh.receipt_date ")
                    .AppendLine("       , v.name AS customer ")
                    .AppendLine("       , rh.total_amount ")
                    .AppendLine("       , rh.bank_rate AS bank_rate ")
                    .AppendLine("       , rh.actual_amount AS actual_amount ")
                    .AppendLine("       , j.job_order ")
                    .AppendLine("       , c.name AS currency ")
                    .AppendLine("       , rh.status_id, s.name as status ")
                    .AppendLine("   FROM receive_header rh ")
                    ' .AppendLine("   LEFT JOIN  ")
                    .AppendLine("   LEFT JOIN receive_detail rd ")
                    ' Change command Because There isn't Search  Minimum job Order  
                    ' Update 28/05/2014
                    '.AppendLine("       (SELECT MAX(rd.job_order_id) AS job_order_id, rd.receive_header_id FROM receive_detail rd	GROUP BY rd.receive_header_id) AS rd ")
                    .AppendLine("       ON rd.receive_header_id = rh.id ")
                    .AppendLine("   LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine("   LEFT JOIN mst_vendor v ON rh.vendor_id = v.id  AND v.type1 = 1 ")
                    .AppendLine("   LEFT JOIN mst_currency c ON j.currency_id = c.id ")
                    .AppendLine("   LEFT JOIN status s ON rh.status_id = s.id ")
                    .AppendLine("   WHERE rh.status_id <> 6  ")
                    .AppendLine("   AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine("   AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine("   AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine("   AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("   AND (ISNULL(?invoice_type) OR rh.invoice_type = ?invoice_type )  ")
                    .AppendLine("   AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("   AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("   AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine("   AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                    '.AppendLine("  ORDER BY rh.id DESC ")
                    .AppendLine("   GROUP BY rh.invoice_no")
                    .AppendLine("  ORDER BY rh.invoice_no DESC ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_no_search), DBNull.Value, objSaleInvoiceEnt.invoice_no_search))
                objConn.AddParameter("?invoice_type", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_type_search), DBNull.Value, objSaleInvoiceEnt.invoice_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.customer_search), DBNull.Value, objSaleInvoiceEnt.customer_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_from_search), DBNull.Value, objSaleInvoiceEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_to_search), DBNull.Value, objSaleInvoiceEnt.job_order_to_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_from_search), DBNull.Value, objSaleInvoiceEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_to_search), DBNull.Value, objSaleInvoiceEnt.issue_date_to_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_from_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_to_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_to_search))


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .invoice_type_name = IIf(IsDBNull(dr.Item("invoice_type")), Nothing, dr.Item("invoice_type"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .bank_rate = IIf(IsDBNull(dr.Item("bank_rate")), Nothing, dr.Item("bank_rate"))
                            .actual_amount = IIf(IsDBNull(dr.Item("actual_amount")), Nothing, dr.Item("actual_amount"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .currency = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .status = IIf(IsDBNull(dr.Item("status")), Nothing, dr.Item("status"))
                        End With
                        ' add item to list
                        GetSaleInvoiceList.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceReportList
        '	Discription	    : Get sale invoice Report list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceReportList( _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetSaleInvoiceReportList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSaleInvoiceReportList = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT rh.id AS receive_header_id ")
                    .AppendLine("   , rd.id as receive_detail_id ")
                    .AppendLine("   , rd.job_order_id ")
                    .AppendLine("   , rh.invoice_no ")
                    .AppendLine("   , rh.issue_date ")
                    .AppendLine("   , j.job_order ")
                    .AppendLine("   , rh.bank_fee ")
                    .AppendLine("   , if(ifnull(v.abbr,'')='',v.name,v.abbr) AS customer ")
                    .AppendLine("   , rd.hontai_type ")
                    .AppendLine("   , (CASE rd.hontai_type WHEN 1 THEN '1st' WHEN 2 THEN '2nd' WHEN 3 THEN '3rd' ELSE '' END) AS stage ")
                    .AppendLine("   , po.po_no ")
                    .AppendLine("   , j.payment_condition_id ")
                    .AppendLine("   , (CASE rd.hontai_type WHEN 1 THEN pc.1st WHEN 2 THEN pc.2nd WHEN 3 THEN pc.3rd ELSE '' END) AS percent ")
                    .AppendLine("   , IFNULL(rd.actual_rate,1) AS actual_rate ")
                    .AppendLine("   , IFNULL(rd.amount,0) AS price ")
                    .AppendLine("   , IFNULL(rd.vat_amount,0) AS vat ")
                    .AppendLine("   , (IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) + IFNULL(rd.vat_amount,0) as amount ")
                    .AppendLine("   , rd.remark ")
                    .AppendLine(" FROM receive_header rh  ")
                    .AppendLine(" LEFT JOIN receive_detail rd ON rh.id = rd.receive_header_id  ")
                    .AppendLine(" LEFT JOIN mst_vendor v ON rh.vendor_id = v.id AND v.delete_fg <> 1 AND v.type1 = 1  ")
                    .AppendLine(" LEFT JOIN job_order_po po ON rd.job_order_po_id = po.id AND po.delete_fg <> 1 ")
                    .AppendLine(" LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine(" LEFT JOIN mst_payment_condition pc ON j.payment_condition_id = pc.id AND pc.delete_fg <> 1 ")
                    .AppendLine(" WHERE rh.status_id <> 6 ")
                    .AppendLine(" AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine(" AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine(" AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine(" AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?invoice_type) OR rh.invoice_type = ?invoice_type )  ")
                    .AppendLine(" AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine(" AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                    .AppendLine(" AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine(" AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_no_search), DBNull.Value, objSaleInvoiceEnt.invoice_no_search))
                objConn.AddParameter("?invoice_type", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_type_search), DBNull.Value, objSaleInvoiceEnt.invoice_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.customer_search), DBNull.Value, objSaleInvoiceEnt.customer_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_from_search), DBNull.Value, objSaleInvoiceEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_to_search), DBNull.Value, objSaleInvoiceEnt.job_order_to_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_from_search), DBNull.Value, objSaleInvoiceEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_to_search), DBNull.Value, objSaleInvoiceEnt.issue_date_to_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_from_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_to_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            '.id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .stage = IIf(IsDBNull(dr.Item("stage")), Nothing, dr.Item("stage"))
                            .percent = IIf(IsDBNull(dr.Item("percent")), Nothing, dr.Item("percent"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), 1, dr.Item("actual_rate"))
                            .price = IIf(IsDBNull(dr.Item("price")), 0, dr.Item("price"))
                            .vat = IIf(IsDBNull(dr.Item("vat")), 0, dr.Item("vat"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), 0, dr.Item("amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                        End With
                        ' add item to list
                        GetSaleInvoiceReportList.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceReportList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceReportList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumSaleInvoiceReportList
        '	Discription	    : Get Sum sale invoice Report list
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumSaleInvoiceReportList( _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSumSaleInvoiceReportList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumSaleInvoiceReportList = New Entity.ImpSale_InvoiceEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(IFNULL(rd.amount,0)) AS sum_price ")
                    .AppendLine("   ,SUM(IFNULL(rd.vat_amount,0)) AS sum_vat ")
                    .AppendLine("   ,SUM((IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) + IFNULL(rd.vat_amount,0)) as sum_amount ")
                    .AppendLine(" FROM receive_header rh  ")
                    .AppendLine(" LEFT JOIN receive_detail rd ON rh.id = rd.receive_header_id  ")
                    .AppendLine(" LEFT JOIN mst_vendor v ON rh.vendor_id = v.id AND v.delete_fg <> 1 AND v.type1 = 1  ")
                    .AppendLine(" LEFT JOIN job_order_po po ON rd.job_order_po_id = po.id AND po.delete_fg <> 1 ")
                    .AppendLine(" LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine(" LEFT JOIN mst_payment_condition pc ON j.payment_condition_id = pc.id AND pc.delete_fg <> 1 ")
                    .AppendLine(" WHERE rh.status_id <> 6 ")
                    .AppendLine(" AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine(" AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine(" AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine(" AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?invoice_type) OR rh.invoice_type = ?invoice_type )  ")
                    .AppendLine(" AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine(" AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                    .AppendLine(" AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine(" AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_no_search), DBNull.Value, objSaleInvoiceEnt.invoice_no_search))
                objConn.AddParameter("?invoice_type", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_type_search), DBNull.Value, objSaleInvoiceEnt.invoice_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.customer_search), DBNull.Value, objSaleInvoiceEnt.customer_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_from_search), DBNull.Value, objSaleInvoiceEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_to_search), DBNull.Value, objSaleInvoiceEnt.job_order_to_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_from_search), DBNull.Value, objSaleInvoiceEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_to_search), DBNull.Value, objSaleInvoiceEnt.issue_date_to_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_from_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_to_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSumSaleInvoiceReportList
                            .sum_price = IIf(IsDBNull(dr.Item("sum_price")), Nothing, dr.Item("sum_price"))
                            .sum_vat = IIf(IsDBNull(dr.Item("sum_vat")), Nothing, dr.Item("sum_vat"))
                            .sum_amount = IIf(IsDBNull(dr.Item("sum_amount")), Nothing, dr.Item("sum_amount"))

                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumSaleInvoiceReportList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumSaleInvoiceReportList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceHeaderList
        '	Discription	    : Get Sale Invoice header on detail screen
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceHeaderList( _
            ByVal intRefID As Integer _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSaleInvoiceHeaderList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetSaleInvoiceHeaderList = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT 		")
                    .AppendLine("    rh.invoice_no ")
                    .AppendLine("   , rh.issue_date ")
                    .AppendLine("   , rh.receipt_date ")
                    .AppendLine("   , CONCAT(ie.code,' - ',ie.name) AS account_title ")
                    .AppendLine("   , v.name AS customer ")
                    .AppendLine("   , (CASE rh.account_type WHEN 1 THEN 'Current' WHEN 2 THEN 'Saving' WHEN 3 THEN 'Cash' END) AS account_type ")
                    .AppendLine("   , (CASE rh.invoice_type WHEN 1 THEN 'IN' WHEN 2 THEN 'IV' WHEN 3 THEN 'IS' END) AS invoice_type ")
                    .AppendLine("   , IFNULL(rh.bank_fee,0) AS bank_fee ")
                    .AppendLine("   , rh.total_amount  ")
                    .AppendLine(" FROM receive_header rh ")
                    .AppendLine(" LEFT JOIN mst_ie ie ON ie.id = rh.ie_id  ")
                    .AppendLine(" LEFT JOIN mst_vendor v ON v.id = rh.vendor_id ")
                    .AppendLine("	WHERE rh.id = ?receive_header_id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", intRefID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSaleInvoiceHeaderList
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .account_title = IIf(IsDBNull(dr.Item("account_title")), Nothing, dr.Item("account_title"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .account_type_name = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .invoice_type_name = IIf(IsDBNull(dr.Item("invoice_type")), Nothing, dr.Item("invoice_type"))
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))
                            .amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))

                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceHeaderList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceHeaderList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceDetailList
        '	Discription	    : Get sale invoice detail list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceDetailList( _
            ByVal intRefID As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetSaleInvoiceDetailList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSaleInvoiceDetailList = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT rd.id, j.job_order, rd.job_order_po_id ")
                    .AppendLine("   , (CASE po.po_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others' END) AS po_type ")
                    .AppendLine("   , (CASE rd.hontai_type WHEN 1 THEN '1st' WHEN 2 THEN '2nd' WHEN 3 THEN '3rd' END) AS hontai ")
                    .AppendLine("   , po.po_no ")
                    .AppendLine("   , IFNULL(rd.amount,0) * IFNULL(rd.actual_rate,1) AS amount ")
                    .AppendLine("   , CONCAT(vt.percent,'%') AS vat ")
                    .AppendLine("   , CONCAT(wt.percent,'%') AS wt ")
                    .AppendLine("   , rd.remark         ")
                    .AppendLine("   , IFNULL(rd.bank_fee,0) AS bank_fee          ")
                    .AppendLine(" FROM receive_detail rd ")
                    .AppendLine(" LEFT JOIN job_order_po po ON po.id = rd.job_order_po_id AND po.delete_fg <> 1 ")
                    .AppendLine(" LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine(" LEFT JOIN mst_vat vt ON vt.id = rd.vat_id AND vt.delete_fg <> 1 ")
                    .AppendLine(" LEFT JOIN mst_wt wt ON wt.id = rd.wt_id AND wt.delete_fg <> 1 ")
                    .AppendLine(" WHERE  rd.receive_header_id = ?receive_header_id ")
                End With



                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", intRefID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .po_type_name = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .hontai = IIf(IsDBNull(dr.Item("hontai")), Nothing, dr.Item("hontai"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), 0, dr.Item("amount"))
                            .vat = IIf(IsDBNull(dr.Item("vat")), Nothing, dr.Item("vat"))
                            .wt = IIf(IsDBNull(dr.Item("wt")), Nothing, dr.Item("wt"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))
                        End With
                        ' add item to list
                        GetSaleInvoiceDetailList.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceDetailList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceDetailList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTotalSaleInvoiceAmount
        '	Discription	    : Get Total Sale Invoice Amount
        '	Return Value	: dto
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTotalSaleInvoiceAmount( _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetTotalSaleInvoiceAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetTotalSaleInvoiceAmount = New Entity.ImpSale_InvoiceEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT ")
                    .AppendLine("       SUM(rh.total_amount) as total_invoice_amount ")
                    .AppendLine("   FROM receive_header rh ")
                    .AppendLine("   LEFT JOIN  ")
                    .AppendLine("       (SELECT MAX(rd.job_order_id) AS job_order_id, rd.receive_header_id FROM receive_detail rd	GROUP BY rd.receive_header_id) AS rd ")
                    .AppendLine("       ON rd.receive_header_id = rh.id ")
                    .AppendLine("   LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine("   LEFT JOIN mst_vendor v ON rh.vendor_id = v.id  AND v.type1 = 1 ")
                    .AppendLine("   LEFT JOIN mst_currency c ON j.currency_id = c.id ")
                    .AppendLine("   WHERE rh.status_id <> 6  ")
                    .AppendLine("   AND (ISNULL(?invoice_no) OR rh.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine("   AND ((ISNULL(?issue_date_from) OR rh.issue_date >= ?issue_date_from)    ")
                    .AppendLine("   AND (ISNULL(?issue_date_to) OR rh.issue_date <= ?issue_date_to))  ")
                    .AppendLine("   AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("   AND (ISNULL(?invoice_type) OR rh.invoice_type = ?invoice_type )  ")
                    .AppendLine("   AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("   AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("   AND ((ISNULL(?receipt_date_from) OR rh.receipt_date >= ?receipt_date_from)    ")
                    .AppendLine("   AND (ISNULL(?receipt_date_to) OR rh.receipt_date <= ?receipt_date_to))  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_no_search), DBNull.Value, objSaleInvoiceEnt.invoice_no_search))
                objConn.AddParameter("?invoice_type", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.invoice_type_search), DBNull.Value, objSaleInvoiceEnt.invoice_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.customer_search), DBNull.Value, objSaleInvoiceEnt.customer_search))
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_from_search), DBNull.Value, objSaleInvoiceEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.job_order_to_search), DBNull.Value, objSaleInvoiceEnt.job_order_to_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_from_search), DBNull.Value, objSaleInvoiceEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.issue_date_to_search), DBNull.Value, objSaleInvoiceEnt.issue_date_to_search))
                objConn.AddParameter("?receipt_date_from", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_from_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_from_search))
                objConn.AddParameter("?receipt_date_to", IIf(String.IsNullOrEmpty(objSaleInvoiceEnt.receipt_date_to_search), DBNull.Value, objSaleInvoiceEnt.receipt_date_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                objLog.InfoLog("GetTotalSaleInvoiceAmount strSql(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                objLog.InfoLog("job_order_from", objSaleInvoiceEnt.job_order_from_search, HttpContext.Current.Session("UserName"))
                objLog.InfoLog("job_order_to_search", objSaleInvoiceEnt.job_order_to_search, HttpContext.Current.Session("UserName"))


                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetTotalSaleInvoiceAmount
                            .total_invoice_amount = IIf(IsDBNull(dr.Item("total_invoice_amount")), Nothing, dr.Item("total_invoice_amount"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTotalSaleInvoiceAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetTotalSaleInvoiceAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckFinishJobOrder
        '	Discription	    : CheckFinishJobOrder
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function fgFinishGoodsJobOrder(ByVal strJobOrder As String) As Integer
            ' fgFinishGoodsJobOrder = -1
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                For i = 0 To strJobOrder.Split("|").Count - 1
                    If strJobOrder.Split("|")(i) <> "" Then
                        With strSql
                            .Length = 0
                            .AppendLine("	UPDATE job_order    			    ")
                            .AppendLine("	SET finish_fg = ?finish_fg 			")
                            .AppendLine("		,finish_date = ?finish_date     ")
                            .AppendLine("		,updated_by = ?user_id			")
                            .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                            .AppendLine("	WHERE FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                        End With

                        objConn.ClearParameter()
                        objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                        objConn.AddParameter("?id", strJobOrder.Split("|")(i))
                        objConn.AddParameter("?finish_fg", i)
                        objConn.AddParameter("?finish_date", IIf(i = 0, DBNull.Value, Now.ToString("yyyyMMdd")))
                        fgFinishGoodsJobOrder = objConn.ExecuteNonQuery(strSql.ToString)
                        'If fgFinishGoodsJobOrder <= 0 Then
                        '    Exit Function
                        'End If
                    End If
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("fgFinishGoodsJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("fgFinishGoodsJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckFinishJobOrder
        '	Discription	    : CheckFinishJobOrder
        '	Return Value	: Integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function CheckFinishJobOrder(ByVal invoiceHeaderId As String) As String
            CheckFinishJobOrder = ""
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                ' Variable for check finish job_order
                Dim job_order_id As String
                Dim listJOID As New ArrayList
                Dim listJOFinishFG As New ArrayList
                Dim alreadyInList As Boolean = False
                Dim index As Integer = 0
                Dim strJobOrder As String = ""
                Dim finishJobOrder As String = ""
                Dim unFinishJobOrder As String = ""
                Dim po_type As String = ""
                'Check exist Finish Goods on job order table
                With strSql
                    .Length = 0
                    .AppendLine("	SELECT jp.id, jp.job_order_id, (CASE jp.po_type WHEN 0 THEN 'Hontai'		")
                    .AppendLine("		WHEN 1 THEN 'Sample' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others'        ")
                    .AppendLine("		ELSE '' END) AS po_type , IF (jp.po_type = 0, (IF (jp.hontai_fg1 = 1    ")
                    .AppendLine("		AND jp.hontai_fg2 = 1 AND jp.hontai_fg3 = 1, 'Y', 'N'))                 ")
                    .AppendLine("		, (IF (jp.po_fg = 1, 'Y', 'N'))) AS finish_goods                        ")
                    .AppendLine("	FROM job_order_po jp                                                        ")
                    .AppendLine("	WHERE job_order_id IN (														")
                    .AppendLine("		SELECT job_order_id                                                     ")
                    .AppendLine("		FROM receive_detail rd                                                  ")
                    .AppendLine("		WHERE rd.receive_header_id = ?receive_header_id);                       ")
                End With
                objConn.ClearParameter()
                objConn.AddParameter("?receive_header_id", invoiceHeaderId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)
                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                        po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                        If job_order_id > 0 OrElse job_order_id <> Nothing Then
                            ' Check exist job_order_id in arrayList
                            For i = 0 To listJOID.Count - 1
                                If job_order_id = listJOID(i) Then
                                    alreadyInList = True
                                    index = i
                                    Exit For
                                End If
                            Next
                            ' Add or edit data in arrayList
                            If alreadyInList Then
                                ' If listJOFinishFG(index) = "Y" And po_type = "Hontai" Then listJOFinishFG(index) = dr.Item("finish_goods")
                                If po_type = "Hontai" Then
                                    listJOFinishFG(index) = dr.Item("finish_goods")
                                End If
                                alreadyInList = False
                            Else
                                listJOID.Add(dr.Item("job_order_id"))
                                If po_type <> "Hontai" Then
                                    listJOFinishFG(index) = "N"
                                Else
                                    listJOFinishFG.Add(dr.Item("finish_goods"))
                                End If

                            End If
                        End If
                    End While
                End If
                dr.Close()
                'objConn.ExecuteReader.Close()
                ' List finish Job Order by job_order_id
                For i = 0 To listJOID.Count - 1
                    If listJOFinishFG(i) = "Y" Then
                        finishJobOrder &= listJOID(i) & ","
                    Else
                        unFinishJobOrder &= listJOID(i) & ","
                    End If
                Next
                If finishJobOrder <> "" Then finishJobOrder = Left(finishJobOrder, finishJobOrder.Length - 1)
                If unFinishJobOrder <> "" Then unFinishJobOrder = Left(unFinishJobOrder, unFinishJobOrder.Length - 1)
                CheckFinishJobOrder = unFinishJobOrder & "|" & finishJobOrder
                objLog.InfoLog("unFinishJobOrder", unFinishJobOrder)
                objLog.InfoLog("finishJobOrder", finishJobOrder)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckFinishJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("CheckFinishJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: ConfirmReceive
        '	Discription	    : Confirm Receive
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 25-07-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 16-10-2013
        '*************************************************************/
        Public Function ConfirmReceive( _
            ByVal invoiceHeaderId As String, _
            ByVal dtValues As System.Data.DataTable, _
            Optional ByVal dtBankFree As System.Data.DataTable = Nothing _
        ) As Integer Implements ISale_InvoiceDao.ConfirmReceive
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            ConfirmReceive = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer
                Dim strVoucherNo As String
                Dim intVoucherNo As Integer
                Dim intVatId As Integer
                Dim intWtId As Integer
                Dim intIeId As Integer
                Dim intChk As Integer
                Dim strJobOrder As String = ""
                Dim intChkN As Integer = 0
                Dim intChkY As Integer = 0
                Dim count As Integer = 0

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                'Update status to waiting approve 
                With strSql
                    .Length = 0
                    .AppendLine("		UPDATE receive_header 			")
                    .AppendLine("		SET status_id = 3			")
                    .AppendLine("		,updated_by = ?user_id							")
                    .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	    WHERE FIND_IN_SET(CAST(id AS CHAR), ?invoiceHeaderId) > 0; ")
                End With

                objConn.ClearParameter()
                ' assign parameter
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?invoiceHeaderId", invoiceHeaderId)

                ' execute nonquery with sql command   
                intEff = objConn.ExecuteNonQuery(strSql.ToString)
                If intEff <= 0 Then
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                ' set new voucher number
                intVoucherNo = objConn.ExecuteScalar("SELECT last_inv_job + 1 AS last_number FROM `voucher_running`;")
                strVoucherNo = "S-" & intVoucherNo
                HttpContext.Current.Session("VoucherNoConfirm") = strVoucherNo

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    '3.execute scalar get last voucher number
                    intVatId = objConn.ExecuteScalar("SELECT id FROM mst_vat WHERE percent = 0;")
                    ' execute scalar get last voucher number
                    intWtId = objConn.ExecuteScalar("SELECT id FROM mst_wt WHERE percent = 0;")
                    ' execute scalar get last voucher number
                    intIeId = objConn.ExecuteScalar("SELECT id FROM mst_ie e WHERE e.code = 'B2002';")

                    '6.Check exist data on accounting table
                    With strSql
                        .Length = 0
                        .AppendLine("   SELECT COUNT(id) AS cnt FROM accounting         ")
                        .AppendLine("   WHERE type = 4 AND ref_id = ?receive_header_id  ")
                    End With
                    objConn.ClearParameter()
                    objConn.AddParameter("?receive_header_id", row("receive_header_id"))

                    ' execute sql command
                    intChk = objConn.ExecuteScalar(strSql.ToString)

                    'Case don't exist data on accounting
                    If intChk <> 0 Then
                        With strSql
                            .Length = 0
                            .AppendLine("   UPDATE accounting                                   ")
                            .AppendLine("   SET status_id = 6, new_voucher_no = ?voucher_no     ")
                            .AppendLine("   WHERE type = 4 AND ref_id = ?receive_header_id;     ")
                        End With
                        objConn.ClearParameter()
                        objConn.AddParameter("?voucher_no", strVoucherNo)
                        objConn.AddParameter("?receive_header_id", row("receive_header_id"))
                        ' execute nonquery with sql command 
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        If intEff <= 0 Then
                            ' case row effect <= 0 then rollback transaction
                            objConn.RollbackTrans()
                            Exit Function
                        End If
                    End If

                    '7.Insert data to accounting
                    With strSql
                        .Length = 0
                        .AppendLine("   INSERT INTO accounting (	 ")
                        .AppendLine("		type	 ")
                        .AppendLine("		, ref_id	 ")
                        .AppendLine("		, voucher_no	 ")
                        .AppendLine("		, account_type	 ")
                        .AppendLine("		, vendor_id	 ")
                        .AppendLine("		, account_date	 ")
                        .AppendLine("		, job_order	 ")
                        .AppendLine("		, vat_id	 ")
                        .AppendLine("		, wt_id	 ")
                        .AppendLine("		, ie_id	 ")
                        .AppendLine("		, vat_amount	 ")
                        .AppendLine("		, wt_amount	     ")
                        .AppendLine("		, sub_total	     ")
                        .AppendLine("		, remark	     ")
                        .AppendLine("		, item_id	     ")
                        .AppendLine("		, status_id	     ")
                        .AppendLine("		, created_by	 ")
                        .AppendLine("		, created_date	 ")
                        .AppendLine("		, updated_by	 ")
                        .AppendLine("		, updated_date) 	 ")
                        .AppendLine("   SELECT 4 type	 ")
                        .AppendLine("		, rh.id	 ")
                        .AppendLine("		, ?voucher_no	 ")
                        .AppendLine("		, rh.account_type	 ")
                        .AppendLine("		, rh.vendor_id	 ")
                        .AppendLine("		, rh.receipt_date	 ")
                        .AppendLine("		, j.job_order	 ")
                        .AppendLine("		, IFNULL(rd.vat_id,?vat_id)	 ") 'case vat_id is null set 0 (0%)
                        .AppendLine("		, rd.wt_id, rh.ie_id	 ")
                        If String.IsNullOrEmpty(row("exchange_rate").ToString.Trim) Then
                            .AppendLine("		, CAST(CAST((rd.amount * rd.actual_rate * v.percent) AS UNSIGNED) AS CHAR) / 100    ")
                            .AppendLine("		, CAST(CAST((rd.amount * rd.actual_rate * wt.percent) AS UNSIGNED) AS CHAR) / 100   ")
                            .AppendLine("		, CAST(CAST((rd.amount * rd.actual_rate * 100) AS UNSIGNED) AS CHAR) / 100 	        ")
                        Else
                            .AppendLine("		, CAST(CAST((rd.amount * ?exchange_rate * v.percent) AS UNSIGNED) AS CHAR) / 100    ")
                            .AppendLine("		, CAST(CAST((rd.amount * ?exchange_rate * wt.percent) AS UNSIGNED) AS CHAR) / 100   ")
                            .AppendLine("		, CAST(CAST((rd.amount * ?exchange_rate * 100) AS UNSIGNED) AS CHAR) / 100 	        ")
                        End If
                        .AppendLine("		, rd.remark	 ")
                        .AppendLine("		, rd.id AS receive_detail_id	 ")
                        .AppendLine("		, 3	 ")
                        .AppendLine("		, ?user_id	 ")
                        .AppendLine("		, REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')	 ")
                        .AppendLine("		, ?user_id 	 ")
                        .AppendLine("		, REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')	 ")
                        .AppendLine("	FROM receive_detail rd 	 ")
                        .AppendLine("	    INNER JOIN receive_header rh ON rd.receive_header_id = rh.id 	 ")
                        .AppendLine("	    LEFT OUTER JOIN job_order j ON rd.job_order_id = j.id 	 ")
                        .AppendLine("	    LEFT JOIN mst_wt wt ON rd.wt_id = wt.id  	             ")
                        .AppendLine("	    LEFT JOIN mst_vat v ON rd.vat_id = v.id 	             ")
                        .AppendLine("	WHERE rd.receive_header_id = ?receive_header_id 	         ")
                        .AppendLine("	UNION	 ")
                        .AppendLine("	SELECT 4 type	 ")
                        .AppendLine("		, rh.id	 ")
                        .AppendLine("		, ?voucher_no	 ")
                        .AppendLine("		, rh.account_type	 ")
                        .AppendLine("		, rh.vendor_id	 ")
                        .AppendLine("		, rh.receipt_date	 ")
                        .AppendLine("		, j.job_order	 ")
                        .AppendLine("		, ?vat_id	 ")
                        .AppendLine("		, ?wt_id	 ")
                        .AppendLine("		, ?ie_id	 ")
                        .AppendLine("		, 0	 ")
                        .AppendLine("		, 0	 ")
                        .AppendLine("		, rd.bank_fee AS amount	 ")
                        .AppendLine("		, 'Bank Fee'	 ")
                        .AppendLine("		, rd.receive_detail_id	 ")
                        .AppendLine("		, 3	 ")
                        .AppendLine("		, ?user_id	 ")
                        .AppendLine("		, REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')	 ")
                        .AppendLine("		, ?user_id	 ")
                        .AppendLine("		, REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')	 ")
                        .AppendLine("	FROM (	SELECT id AS receive_detail_id, receive_header_id, job_order_id, bank_fee AS bank_fee   ")
                        .AppendLine("			FROM receive_detail  WHERE bank_fee > 0 AND bank_fee IS NOT NULL) rd 					")
                        .AppendLine("		LEFT JOIN receive_header rh ON rd.receive_header_id = rh.id AND rh.status_id <> 6	        ")
                        .AppendLine("		LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6				        ")
                        .AppendLine("	WHERE rh.id =?receive_header_id  	 ")

                    End With

                    ' assign parameter
                    With objConn
                        .ClearParameter()
                        'Mod 2013/09/02
                        '.AddParameter("?user_id", HttpContext.Current.Session("AccountNextApprove"))
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                        .AddParameter("?voucher_no", strVoucherNo)
                        .AddParameter("?receive_header_id", row("receive_header_id"))
                        .AddParameter("?actual_amount", CDec(IIf(String.IsNullOrEmpty(row("actual_amount").ToString.Trim), 1, row("actual_amount"))))
                        .AddParameter("?exchange_rate", CDec(IIf(String.IsNullOrEmpty(row("exchange_rate").ToString.Trim), 1, row("exchange_rate"))))
                        .AddParameter("?vat_id", intVatId)
                        .AddParameter("?wt_id", intWtId)
                        .AddParameter("?ie_id", intIeId)
                    End With

                    ' execute nonquery with sql command
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)

                    ' check row effect 
                    If intEff <= 0 Then
                        ' case have error then exit for
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                Next
                '8.Update last voucher on voucher_running
                With strSql
                    .Length = 0
                    .AppendLine("		UPDATE voucher_running 			")
                    .AppendLine("		SET last_inv_job  = ?last_voucher			")
                    .AppendLine("		,updated_by = ?user_id							")
                    .AppendLine("		,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', ''); ")

                End With

                objConn.ClearParameter()
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?last_voucher", intVoucherNo)

                ' execute nonquery with sql command (update 
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect finally
                If intEff > 0 Then
                    ' case row effect > 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set return value with rows count
                ConfirmReceive = intEff

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ConfirmReceive(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("ConfirmReceive(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrerSaleInvoiceDetail
        '	Discription	    : Get job order ที่ยังไม่ได้ออก sale invoice 
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    : Wasan D.  
        '	Update Date	    : 25-10-2013
        '*************************************************************/
        Public Function GetJobOrerSaleInvoiceDetail( _
            ByVal strJobOrder As String _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetJobOrerSaleInvoiceDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrerSaleInvoiceDetail = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine("   SELECT jp.id, jp.job_order_id, j.job_order, j.customer, j.currency_id		")
                    .AppendLine("   	, jp.po_no, jp.po_type AS type, (CASE jp.po_type WHEN 0 THEN 'Hontai' 	")
                    .AppendLine("   	WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' 		")
                    .AppendLine("   	WHEN 4 THEN 'Others' END) AS po_type, '' AS hontai_type					")
                    .AppendLine("   	, '' AS hontai_type_desc, jp.po_amount AS amount, jp.po_date			")
                    .AppendLine("   	, '' AS hontai_fg, '' as hontai_cond, jt.name AS job_type				")
                    .AppendLine("   FROM job_order_po jp 														")
                    .AppendLine("   	LEFT JOIN job_order j ON jp.job_order_id = j.id AND j.status_id <> 6 	")
                    .AppendLine("   	LEFT JOIN mst_job_type jt on j.job_type_id=jt.id                        ")
                    .AppendLine("   WHERE jp.delete_fg <> 1 AND jp.po_type <> 0 								")
                    .AppendLine("   	AND (jp.po_fg <> 1 OR jp.po_fg IS NULL)	AND j.job_order = ?job_order 	")
                    .AppendLine("   UNION 																		")
                    .AppendLine("   SELECT jp.id, jp.job_order_id, j.job_order, j.customer, j.currency_id		")
                    .AppendLine("   	, jp.po_no, jp.po_type AS type, (CASE jp.po_type WHEN 0 THEN 'Hontai' 	")
                    .AppendLine("   	WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' 		")
                    .AppendLine("   	WHEN 4 THEN 'Others' END) AS po_type , 1 , '1st' AS hontai_type_desc	")
                    .AppendLine("   	, j.hontai_amount1 AS amount, jp.po_date ,jp.hontai_fg1 AS hontai_fg	")
                    .AppendLine("   	, concat(j.hontai_condition1,'%') as hontai_cond, jt.name AS job_type	")
                    .AppendLine("   FROM job_order_po jp 														")
                    .AppendLine("   	LEFT JOIN job_order j ON jp.job_order_id = j.id AND j.status_id <> 6 	")
                    .AppendLine("   	LEFT JOIN mst_job_type jt on j.job_type_id=jt.id                        ")
                    .AppendLine("   WHERE jp.delete_fg <> 1 AND jp.po_type = 0									")
                    .AppendLine("   	AND j.hontai_chk1 = 1 AND j.hontai_amount1 > 0 							")
                    .AppendLine("   	AND (jp.hontai_fg1 <> 1 OR jp.hontai_fg1 IS NULL) 						")
                    .AppendLine("   	AND j.job_order = ?job_order 											")
                    .AppendLine("   UNION 																		")
                    .AppendLine("   SELECT jp.id, jp.job_order_id, j.job_order, j.customer, j.currency_id		")
                    .AppendLine("   	, jp.po_no, jp.po_type AS type, (CASE jp.po_type WHEN 0 THEN 'Hontai' 	")
                    .AppendLine("   	WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' 		")
                    .AppendLine("   	WHEN 4 THEN 'Others' END) AS po_type, 2, '2nd' AS hontai_type_desc		")
                    .AppendLine("   	, j.hontai_amount2 AS amount, jp.po_date ,jp.hontai_fg2 AS hontai_fg	")
                    .AppendLine("   	, concat(j.hontai_condition2,'%') as hontai_cond, jt.name AS job_type	")
                    .AppendLine("   FROM job_order_po jp 														")
                    .AppendLine("   	LEFT JOIN job_order j ON jp.job_order_id = j.id AND j.status_id <> 6 	")
                    .AppendLine("   	LEFT JOIN mst_job_type jt on j.job_type_id=jt.id                        ")
                    .AppendLine("   WHERE jp.delete_fg <> 1 AND jp.po_type = 0 									")
                    .AppendLine("   	AND j.hontai_chk2 = 1 AND j.hontai_amount2 > 0 							")
                    .AppendLine("   	AND (jp.hontai_fg2 <> 1 OR jp.hontai_fg2 IS NULL) 						")
                    .AppendLine("   	AND j.job_order = ?job_order 											")
                    .AppendLine("   UNION 																		")
                    .AppendLine("   SELECT jp.id, jp.job_order_id, j.job_order, j.customer, j.currency_id		")
                    .AppendLine("   	, jp.po_no, jp.po_type AS type, (CASE jp.po_type WHEN 0 THEN 'Hontai' 	")
                    .AppendLine("   	WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' 		")
                    .AppendLine("   	WHEN 4 THEN 'Others' END) AS po_type, 3, '3rd' AS hontai_type_desc		")
                    .AppendLine("   	, j.hontai_amount3 AS amount, jp.po_date ,jp.hontai_fg3 AS hontai_fg	")
                    .AppendLine("   	, concat(j.hontai_condition3,'%') as hontai_cond, jt.name AS job_type	")
                    .AppendLine("   FROM job_order_po jp 														")
                    .AppendLine("   	LEFT JOIN job_order j ON jp.job_order_id = j.id AND j.status_id <> 6  	")
                    .AppendLine("   	LEFT JOIN mst_job_type jt on j.job_type_id=jt.id                        ")
                    .AppendLine("   WHERE jp.delete_fg <> 1 AND jp.po_type = 0 									")
                    .AppendLine("   	AND j.hontai_chk3 = 1 AND j.hontai_amount3 > 0 							")
                    .AppendLine("   	AND (jp.hontai_fg3 <> 1 OR jp.hontai_fg3 IS NULL) 						")
                    .AppendLine("   	AND j.job_order = ?job_order 											")
                    .AppendLine("   ORDER BY po_no, type, hontai_type;											")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .po_type_name = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .hontai = IIf(IsDBNull(dr.Item("hontai_type_desc")), Nothing, dr.Item("hontai_type_desc"))
                            .hontai_type = IIf(IsDBNull(dr.Item("hontai_type")), 0, dr.Item("hontai_type"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), 0, dr.Item("amount"))
                            .po_date = IIf(IsDBNull(dr.Item("po_date")), Nothing, dr.Item("po_date"))
                            .hontai_fg = IIf(IsDBNull(dr.Item("hontai_fg")), Nothing, dr.Item("hontai_fg"))
                            .hontai_cond = IIf(IsDBNull(dr.Item("hontai_cond")), Nothing, dr.Item("hontai_cond"))
                            .job_type = IIf(IsDBNull(dr.Item("job_type")), Nothing, dr.Item("job_type"))
                        End With
                        ' add item to list
                        GetJobOrerSaleInvoiceDetail.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrerSaleInvoiceDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrerSaleInvoiceDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetJobOrerSaleInvoiceDetailEdit
        '	Discription	    : Get job order ที่ยังไม่ได้ออก sale invoice 
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    : Wasan D.  
        '	Update Date	    : 28-10-2013
        '*************************************************************/
        Public Function GetJobOrerSaleInvoiceDetailEdit( _
            ByVal intId As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetJobOrerSaleInvoiceDetailEdit
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrerSaleInvoiceDetailEdit = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT ")
                    .AppendLine("   rd.id ")
                    .AppendLine("   , rd.receive_header_id ")
                    .AppendLine("   , rd.job_order_po_id ")
                    .AppendLine("   , rd.job_order_id ")
                    .AppendLine("   , jp.po_no ")
                    .AppendLine("   , jp.po_type ")
                    .AppendLine("   , (CASE jp.po_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others' END) AS po_type_name ")
                    .AppendLine("   , rd.hontai_type ")
                    .AppendLine("   , (CASE rd.hontai_type WHEN 1 THEN '1 st' WHEN 2 THEN '2 nd' WHEN 3 THEN '3 rd' END) AS hontai_type_desc ")
                    .AppendLine("   , rd.actual_rate ")
                    .AppendLine("   , rd.amount")
                    .AppendLine("   , rd.amount * rd.actual_rate AS amount_thb ")
                    .AppendLine("   , jp.po_date ")
                    .AppendLine("   , rd.vat_id ")
                    .AppendLine("   , rd.wt_id ")
                    .AppendLine("   , rd.remark ")
                    .AppendLine("   , jp.hontai_fg1 ")
                    .AppendLine("   , jp.hontai_fg2 ")
                    .AppendLine("   , jp.hontai_fg3 ")
                    .AppendLine("   , jp.po_fg ")
                    .AppendLine("   , j.job_order ")
                    .AppendLine("   , rd.bank_fee ")
                    .AppendLine("   , jt.name as job_type ")
                    .AppendLine("   , CONCAT(CASE rd.hontai_type WHEN 1 THEN j.hontai_condition1 WHEN 2 THEN j.hontai_condition2 WHEN 3 THEN j.hontai_condition3 END,'%') as hontai_cond ")
                    .AppendLine("   FROM receive_header rh ")
                    .AppendLine("   INNER JOIN receive_detail rd ON rd.receive_header_id = rh.id  ")
                    .AppendLine("       LEFT JOIN job_order_po jp ON rd.job_order_po_id = jp.id AND jp.delete_fg <> 1 ")
                    .AppendLine("       LEFT JOIN job_order j ON jp.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine("       LEFT JOIN mst_job_type jt ON j.job_type_id = jt.id ")
                    .AppendLine("   WHERE rh.id = ?id ")
                    .AppendLine("   ORDER BY jp.po_no, jp.po_type, rd.hontai_type ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intId)

                objLog.InfoLog("intId : ", intId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                objLog.InfoLog("SQL :", strSql.ToString)
                objLog.InfoLog("id :", intId)


                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .receive_header_id = IIf(IsDBNull(dr.Item("receive_header_id")), Nothing, dr.Item("receive_header_id"))
                            .job_order_po_id = IIf(IsDBNull(dr.Item("job_order_po_id")), Nothing, dr.Item("job_order_po_id"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_type_name = IIf(IsDBNull(dr.Item("po_type_name")), Nothing, dr.Item("po_type_name"))
                            .hontai = IIf(IsDBNull(dr.Item("hontai_type_desc")), Nothing, dr.Item("hontai_type_desc"))
                            .total_amount = IIf(IsDBNull(dr.Item("amount")), 0, dr.Item("amount"))
                            .amount = IIf(IsDBNull(dr.Item("amount_thb")), 0, dr.Item("amount_thb"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), Nothing, dr.Item("actual_rate"))
                            .po_date = IIf(IsDBNull(dr.Item("po_date")), Nothing, dr.Item("po_date"))
                            .vat_id = IIf(IsDBNull(dr.Item("vat_id")), Nothing, dr.Item("vat_id"))
                            .wt_id = IIf(IsDBNull(dr.Item("wt_id")), Nothing, dr.Item("wt_id"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .hontai_type = IIf(IsDBNull(dr.Item("hontai_type")), Nothing, dr.Item("hontai_type"))
                            .hontai_fg1 = IIf(IsDBNull(dr.Item("hontai_fg1")), Nothing, dr.Item("hontai_fg1"))
                            .hontai_fg2 = IIf(IsDBNull(dr.Item("hontai_fg2")), Nothing, dr.Item("hontai_fg2"))
                            .hontai_fg3 = IIf(IsDBNull(dr.Item("hontai_fg3")), Nothing, dr.Item("hontai_fg3"))
                            .po_fg = IIf(IsDBNull(dr.Item("po_fg")), Nothing, dr.Item("po_fg"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))
                            .hontai_cond = IIf(IsDBNull(dr.Item("hontai_cond")), Nothing, dr.Item("hontai_cond"))
                            .job_type = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("job_type"))
                        End With
                        ' add item to list
                        GetJobOrerSaleInvoiceDetailEdit.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrerSaleInvoiceDetailEdit(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrerSaleInvoiceDetailEdit(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInJobOrder
        '	Discription	    : Count job order in used job_order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInJobOrder( _
            ByVal strJobOrder As String _
        ) As Integer Implements ISale_InvoiceDao.CountUsedInJobOrder
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInJobOrder = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(id) AS cnt ")
                    .AppendLine("		FROM job_order          ")
                    .AppendLine("		WHERE job_order = ?job_order    ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CountUsedInJobOrder = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInHontai
        '	Discription	    : Count hontai flag on job order table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 06-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInHontai( _
            ByVal strJobOrderId As Integer, _
            ByVal intHontaiFlg As Integer _
        ) As Integer Implements ISale_InvoiceDao.CountUsedInHontai
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInHontai = -1
            Try
                ' assign sql command
                With strSql
                    If intHontaiFlg = 1 Then
                        .AppendLine("		SELECT count(hontai_fg1) AS cnt ")
                    End If
                    If intHontaiFlg = 2 Then
                        .AppendLine("		SELECT count(hontai_fg2) AS cnt ")
                    End If
                    If intHontaiFlg = 3 Then
                        .AppendLine("		SELECT count(hontai_fg3) AS cnt ")
                    End If
                    .AppendLine("		FROM job_order_po          ")
                    .AppendLine("		WHERE job_order_id = ?job_order_id    ")
                    .AppendLine("		AND po_type = 0 ;  ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", strJobOrderId)

                ' execute sql command
                CountUsedInHontai = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInHontai(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInHontai(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSaleInvoiceByJobOrder
        '	Discription	    : Get Sale Invoice by job order
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceByJobOrder( _
            ByVal strJobOrder As String _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSaleInvoiceByJobOrder
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetSaleInvoiceByJobOrder = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim dr1 As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	sr.id	")
                    .AppendLine("   , j.job_order ")
                    .AppendLine("   , j.id AS job_order_id ")
                    .AppendLine("   , j.issue_date ")
                    .AppendLine("   , c.name AS currency ")
                    .AppendLine("   , sr.rate AS actual_rate ")
                    .AppendLine("   , j.currency_id ")
                    .AppendLine("   FROM job_order j ")
                    .AppendLine("   LEFT JOIN mst_currency c  ")
                    .AppendLine("   ON j.currency_id = c.id AND c.delete_fg <> 1 ")
                    .AppendLine("   LEFT JOIN mst_schedule_rate sr  ")
                    .AppendLine("   ON j.currency_id = sr.currency_id AND sr.delete_fg <> 1 ")
                    .AppendLine("   WHERE sr.ef_date <= j.Issue_date  ")
                    .AppendLine("   AND j.job_order  = ?job_order ")
                    .AppendLine("   ORDER BY sr.ef_date DESC LIMIT 1 ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSaleInvoiceByJobOrder
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .currency = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), Nothing, dr.Item("actual_rate"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                        End With
                    End While

                Else
                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("	SELECT 	'0' AS id	")
                        .AppendLine("   , j.job_order ")
                        .AppendLine("   , j.id AS job_order_id ")
                        .AppendLine("   , j.issue_date ")
                        .AppendLine("   , c.name AS currency ")
                        .AppendLine("   , '1.00000' AS actual_rate ")
                        .AppendLine("   , j.currency_id ")
                        .AppendLine("   FROM job_order j ")
                        .AppendLine("   LEFT JOIN mst_currency c  ")
                        .AppendLine("   ON j.currency_id = c.id AND c.delete_fg <> 1 ")
                        .AppendLine("   WHERE j.job_order  = ?job_order ")
                        .AppendLine("   AND c.name = 'THB' ")
                    End With

                    ' assign parameter
                    objConn.ClearParameter()
                    objConn.AddParameter("?job_order", strJobOrder)

                    ' execute sql command with data reader object
                    dr.Close()
                    dr1 = objConn.ExecuteReader(strSql.ToString)

                    If dr1.HasRows Then
                        While dr1.Read
                            ' assign data from db to entity object
                            With GetSaleInvoiceByJobOrder
                                .id = IIf(IsDBNull(dr1.Item("id")), Nothing, dr1.Item("id"))
                                .job_order = IIf(IsDBNull(dr1.Item("job_order")), Nothing, dr1.Item("job_order"))
                                .job_order_id = IIf(IsDBNull(dr1.Item("job_order_id")), Nothing, dr1.Item("job_order_id"))
                                .issue_date = IIf(IsDBNull(dr1.Item("issue_date")), Nothing, dr1.Item("issue_date"))
                                .currency = IIf(IsDBNull(dr1.Item("currency")), Nothing, dr1.Item("currency"))
                                .actual_rate = IIf(IsDBNull(dr1.Item("actual_rate")), Nothing, dr1.Item("actual_rate"))
                                .currency_id = IIf(IsDBNull(dr1.Item("currency_id")), Nothing, dr1.Item("currency_id"))
                            End With
                        End While
                    End If
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceByJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceByJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: CountCustomerUsedInJobOrder
        '	Discription	    : Count customer in used job_order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountCustomerUsedInJobOrder( _
            ByVal strJobOrder As String, _
            ByVal intCustomer As Integer _
        ) As Integer Implements ISale_InvoiceDao.CountCustomerUsedInJobOrder
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountCustomerUsedInJobOrder = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(customer) AS cnt ")
                    .AppendLine("		FROM job_order          ")
                    .AppendLine("		WHERE job_order = ?job_order    ")
                    .AppendLine("		AND customer = ?customer    ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)
                objConn.AddParameter("?customer", intCustomer)

                ' execute sql command
                CountCustomerUsedInJobOrder = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountCustomerUsedInJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountCustomerUsedInJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertSaleInvoice
        '	Discription	    : Insert Sale Invoice
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertSaleInvoice( _
            ByVal strJobPoId1 As String, _
            ByVal strJobPoId2 As String, _
            ByVal strJobPoId3 As String, _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements ISale_InvoiceDao.InsertSaleInvoice
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertSaleInvoice = -1
            Try

                ' variable keep row effect
                Dim intEff As Integer
                Dim intReceiveHeaderId As Integer = 0
                Dim vatPercent As Integer
                Dim wtPercent As Integer
                Dim wt_amount As Decimal
                Dim vat_amount As Decimal
                Dim strJobOrder As String = ""

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                '---Insert Header of sale invoice
                With strSql
                    .Length = 0
                    .AppendLine("		INSERT INTO receive_header (invoice_no					")
                    .AppendLine("				, issue_date				")
                    .AppendLine("				, receipt_date				")
                    .AppendLine("				, ie_id				")
                    .AppendLine("				, vendor_id				")
                    .AppendLine("				, account_type				")
                    .AppendLine("				, invoice_type				")
                    '.AppendLine("               , bank_fee ")
                    .AppendLine("               , total_amount ")
                    .AppendLine("               , user_id ")
                    .AppendLine("               , status_id ")
                    .AppendLine("               , created_by ")
                    .AppendLine("               , created_date ")
                    .AppendLine("               , updated_by ")
                    .AppendLine("               , updated_date) ")
                    .AppendLine("       VALUES (?invoice_no ")
                    .AppendLine("               ,?issue_date ")
                    .AppendLine("               ,?receipt_date ")
                    .AppendLine("               ,?ie_id ")
                    .AppendLine("               ,?vendor_id ")
                    .AppendLine("               ,?account_type ")
                    .AppendLine("               ,?invoice_type ")
                    '.AppendLine("               ,?bank_fee ")
                    .AppendLine("               ,?total_amount ")
                    .AppendLine("               ,?account_next_approve ")
                    .AppendLine("               ,1 ")
                    .AppendLine("			    ,?user_id					")
                    .AppendLine("			    ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			    ,?user_id					")
                    .AppendLine("			    ,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			   );					")

                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?invoice_no", objSaleInvoiceEnt.invoice_no)
                    .AddParameter("?issue_date", objSaleInvoiceEnt.issue_date)
                    .AddParameter("?receipt_date", objSaleInvoiceEnt.receipt_date)
                    .AddParameter("?ie_id", objSaleInvoiceEnt.account_title)
                    .AddParameter("?vendor_id", objSaleInvoiceEnt.customer)
                    .AddParameter("?account_type", objSaleInvoiceEnt.account_type)
                    .AddParameter("?invoice_type", objSaleInvoiceEnt.invoice_type)
                    '.AddParameter("?bank_fee", objSaleInvoiceEnt.bank_fee)
                    .AddParameter("?total_amount", CDec(objSaleInvoiceEnt.total_amount).ToString("0.00"))
                    .AddParameter("?account_next_approve", objSaleInvoiceEnt.account_next_approve)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)
                    If intEff <= 0 Then
                        ' case row effect <= 0 then rollback transaction
                        objConn.RollbackTrans()
                        Exit Function
                    End If

                End With

                '1.Get last Voucher No 
                With strSql
                    .Length = 0
                    .AppendLine("	SELECT MAX(id) AS last_number   			")
                    .AppendLine("	FROM `receive_header` 			")
                End With
                ' execute scalar get last voucher number
                intReceiveHeaderId = objConn.ExecuteScalar(strSql.ToString)

                '----Insert Detail of sale invoice
                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    ' assign sql command
                    'Get percent vat
                    With strSql
                        .Length = 0
                        .AppendLine("	SELECT `percent` 			")
                        .AppendLine("	FROM `mst_vat` 			")
                        .AppendLine("	WHERE (`id` = ?vat_id);			")

                        objConn.ClearParameter()
                        objConn.AddParameter("?vat_id", row("vat_id"))
                        ' execute scalar get last voucher number
                        vatPercent = objConn.ExecuteScalar(strSql.ToString)
                        vat_amount = 0
                        vat_amount = CDec((vatPercent / 100) * (row("amount") * row("actual_rate"))).ToString("0.00")

                    End With
                    'Get percent wt
                    With strSql
                        .Length = 0
                        .AppendLine("	SELECT `percent` 			")
                        .AppendLine("	FROM `mst_wt` 			")
                        .AppendLine("	WHERE (`id` = ?wt_id);			")

                        objConn.ClearParameter()
                        objConn.AddParameter("?wt_id", row("wt_id"))
                        ' execute scalar get last voucher number
                        wtPercent = objConn.ExecuteScalar(strSql.ToString)
                        wt_amount = 0
                        wt_amount = CDec((wtPercent / 100) * (row("amount") * row("actual_rate"))).ToString("0.00")

                    End With

                    With strSql
                        .Length = 0
                        .AppendLine("	INSERT INTO receive_detail (receive_header_id							")
                        .AppendLine("			,`job_order_id`					")
                        .AppendLine("			,`job_order_po_id`					")
                        .AppendLine("			,`hontai_type`					")
                        .AppendLine("			,`actual_rate`					")
                        .AppendLine("			,`amount`					")
                        .AppendLine("			,`vat_id`					")
                        .AppendLine("			,`wt_id`					")
                        .AppendLine("			,`vat_amount`					")
                        .AppendLine("			,`wt_amount`					")
                        .AppendLine("			,`remark`					")
                        .AppendLine("           ,`bank_fee`                  ")
                        .AppendLine("			,`created_by`					")
                        .AppendLine("			,`created_date`					")
                        .AppendLine("			,`updated_by`					")
                        .AppendLine("			,`updated_date`)					")
                        .AppendLine("	VALUES (?receive_header_id							")
                        .AppendLine("		 ,?job_order_id						")
                        .AppendLine("		 ,?job_order_po_id						")
                        .AppendLine("		 ,?hontai_type						")
                        .AppendLine("		 ,?actual_rate						")
                        .AppendLine("		 ,?amount						")
                        .AppendLine("		 ,?vat_id						")
                        .AppendLine("		 ,?wt_id						")
                        .AppendLine("		 ,?vat_amount						")
                        .AppendLine("		 ,?wt_amount						")
                        .AppendLine("		 ,?remark						")
                        .AppendLine("		 ,?bank_fee						")
                        .AppendLine("		 ,?user_id						")
                        .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')						")
                        .AppendLine("		 ,?user_id						")
                        .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':',''));						")
                    End With

                    ' assign parameter
                    With objConn
                        .ClearParameter()
                        .AddParameter("?receive_header_id", intReceiveHeaderId)
                        .AddParameter("?job_order_id", row("job_order_id"))
                        .AddParameter("?job_order_po_id", row("job_order_po_id"))
                        .AddParameter("?hontai_type", IIf(String.IsNullOrEmpty(row("hontai_type").ToString.Trim), DBNull.Value, row("hontai_type")))
                        .AddParameter("?actual_rate", IIf(String.IsNullOrEmpty(row("actual_rate").ToString.Trim), DBNull.Value, row("actual_rate")))
                        .AddParameter("?amount", CDec(IIf(String.IsNullOrEmpty(row("amount").ToString.Trim), 0, row("amount"))).ToString("0.00"))
                        .AddParameter("?vat_id", row("vat_id"))
                        .AddParameter("?wt_id", row("wt_id"))
                        .AddParameter("?vat_amount", vat_amount)
                        .AddParameter("?wt_amount", wt_amount)
                        .AddParameter("?remark", row("remark"))
                        .AddParameter("?bank_fee", CDec(IIf(String.IsNullOrEmpty(row("bank_fee").ToString.Trim), 0, row("bank_fee"))))
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    End With

                    ' execute nonquery with sql command
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)

                    ' check row effect 
                    If intEff <= 0 Then
                        ' case have error then exit for
                        Exit For
                    End If

                    'Update flag on job_order_po table
                    With strSql
                        .Length = 0
                        If row("po_type") <> 0 Or row("po_type") = "" Then
                            'case po type : Sample, Material, Delivery, Others
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET po_fg = 1 			")
                            .AppendLine("		WHERE (id = ?job_order_po_id);			")
                        ElseIf row("po_type") = 0 And row("hontai_type") = "1" Then
                            'case Hontai is 1st  
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg1 = 1 			")
                            .AppendLine("		WHERE (id = ?job_order_po_id);			")
                        ElseIf row("po_type") = 0 And row("hontai_type") = "2" Then
                            'case Hontai is 2nd  
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg2 = 1 			")
                            .AppendLine("		WHERE (id = ?job_order_po_id);			")
                        ElseIf row("po_type") = 0 And row("hontai_type") = "3" Then
                            'case Hontai is 3rd   
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg3 = 1 			")
                            .AppendLine("		WHERE (id = ?job_order_po_id);			")
                        End If
                    End With

                    objConn.ClearParameter()
                    objConn.AddParameter("?job_order_po_id", row("job_order_po_id"))

                    ' execute nonquery with sql command (update 
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)

                    ' check row effect 
                    If intEff <= 0 Then
                        ' case have error then exit for
                        Exit For
                    End If

                    'case payment condition = 0,update hontai_fg1 on job_order_po table
                    If strJobPoId1 <> "" Then
                        With strSql
                            .Length = 0
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg1 = 1 			")
                            .AppendLine("		WHERE  	FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                        End With
                        objConn.ClearParameter()
                        objConn.AddParameter("?id", strJobPoId1)

                        ' execute nonquery with sql command (update 
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If
                    End If

                    'case payment condition = 0,update hontai_fg2 on job_order_po table
                    If strJobPoId2 <> "" Then
                        With strSql
                            .Length = 0
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg2= 1 			")
                            .AppendLine("		WHERE  	FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                        End With
                        objConn.ClearParameter()
                        objConn.AddParameter("?id", strJobPoId2)

                        ' execute nonquery with sql command (update 
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If
                    End If

                    'case payment condition = 0,update hontai_fg3 on job_order_po table
                    If strJobPoId3 <> "" Then
                        With strSql
                            .Length = 0
                            .AppendLine("		Update job_order_po 			")
                            .AppendLine("		SET hontai_fg3= 1 			")
                            .AppendLine("		WHERE  	FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                        End With
                        objConn.ClearParameter()
                        objConn.AddParameter("?id", strJobPoId3)

                        ' execute nonquery with sql command (update 
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If
                    End If
                Next
                ' check row effect finally
                If intEff > 0 Then
                    ' case row effect > 0 then commit transaction
                    objConn.CommitTrans()
                    ' Call function CheckFinishJobOrder to get finish job order 
                    strJobOrder = CheckFinishJobOrder(intReceiveHeaderId)
                    ' Update finish_fg on job order table               
                    If strJobOrder <> "" Then fgFinishGoodsJobOrder(strJobOrder)
                    InsertSaleInvoice = intEff
                Else
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertSaleInvoice(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("InsertSaleInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateSaleInvoice
        '	Discription	    : Update Sale Invoice
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateSaleInvoice( _
            ByVal strIdDelete As String, _
            ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements ISale_InvoiceDao.UpdateSaleInvoice
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateSaleInvoice = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer
                Dim vatPercent As Integer
                Dim wtPercent As Integer
                Dim vat_amount As Decimal
                Dim wt_amount As Decimal
                Dim strJobOrder As String = ""
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                '--Delete receive_detail
                With strSql
                    .Length = 0
                    .AppendLine("		DELETE FROM receive_detail  			")
                    .AppendLine("		WHERE  FIND_IN_SET(CAST(id AS CHAR), ?id) > 0;	")
                End With
                objConn.ClearParameter()
                objConn.AddParameter("?id", strIdDelete)

                ' execute nonquery with sql command (update 
                intEff = objConn.ExecuteNonQuery(strSql.ToString)
                If intEff < 0 Then
                    ' case row effect < 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                '---Insert Header of sale invoice
                With strSql
                    .Length = 0
                    .AppendLine("		UPDATE receive_header				")
                    .AppendLine("		SET invoice_no = ?invoice_no				")
                    .AppendLine("		, issue_date = ?issue_date		")
                    .AppendLine("		, receipt_date = ?receipt_date		")
                    .AppendLine("		, ie_id = ?ie_id		")
                    .AppendLine("		, vendor_id = ?vendor_id			")
                    .AppendLine("		, account_type = ?account_type			")
                    .AppendLine("       , invoice_type = ?invoice_type ")
                    '.AppendLine("       , bank_fee = ?bank_fee ")
                    .AppendLine("       , total_amount = ?total_amount ")
                    .AppendLine("       , user_id = ?account_next_approve ")
                    .AppendLine("       , updated_date= REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("       , updated_by=?user_id ")
                    .AppendLine("        WHERE id = ?id; ")
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?id", objSaleInvoiceEnt.id)
                    .AddParameter("?invoice_no", objSaleInvoiceEnt.invoice_no)
                    .AddParameter("?issue_date", objSaleInvoiceEnt.issue_date)
                    .AddParameter("?receipt_date", objSaleInvoiceEnt.receipt_date)
                    .AddParameter("?ie_id", objSaleInvoiceEnt.account_title)
                    .AddParameter("?vendor_id", objSaleInvoiceEnt.customer)
                    .AddParameter("?account_type", objSaleInvoiceEnt.account_type)
                    .AddParameter("?invoice_type", objSaleInvoiceEnt.invoice_type)
                    '.AddParameter("?bank_fee", objSaleInvoiceEnt.bank_fee)
                    .AddParameter("?total_amount", objSaleInvoiceEnt.total_amount)
                    .AddParameter("?account_next_approve", objSaleInvoiceEnt.account_next_approve)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)
                    If intEff <= 0 Then
                        ' case row effect <= 0 then rollback transaction
                        objConn.RollbackTrans()
                        Exit Function
                    End If

                End With

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    ' assign sql command
                    'Get percent vat
                    With strSql
                        .Length = 0
                        .AppendLine("	SELECT `percent` 			")
                        .AppendLine("	FROM `mst_vat` 			")
                        .AppendLine("	WHERE (`id` = ?vat_id);			")

                        objConn.ClearParameter()
                        objConn.AddParameter("?vat_id", row("vat_id"))
                        ' execute scalar get last voucher number
                        vatPercent = objConn.ExecuteScalar(strSql.ToString)
                        vat_amount = 0
                        vat_amount = (vatPercent / 100) * (row("amount") * row("actual_rate"))

                    End With
                    'Get percent wt
                    With strSql
                        .Length = 0
                        .AppendLine("	SELECT `percent` 			")
                        .AppendLine("	FROM `mst_wt` 			")
                        .AppendLine("	WHERE (`id` = ?wt_id);			")

                        objConn.ClearParameter()
                        objConn.AddParameter("?wt_id", row("wt_id"))
                        ' execute scalar get last voucher number
                        wtPercent = objConn.ExecuteScalar(strSql.ToString)
                        wt_amount = 0
                        wt_amount = (wtPercent / 100) * (row("amount") * row("actual_rate"))

                    End With

                    If row("receive_header_id") = "" Or row("receive_header_id") Is Nothing Then
                        With strSql
                            .Length = 0
                            .AppendLine("	INSERT INTO receive_detail (receive_header_id")
                            .AppendLine("			,`job_order_id`					")
                            .AppendLine("			,`job_order_po_id`					")
                            .AppendLine("			,`hontai_type`					")
                            .AppendLine("			,`actual_rate`					")
                            .AppendLine("			,`amount`					")
                            .AppendLine("			,`vat_id`					")
                            .AppendLine("			,`wt_id`					")
                            .AppendLine("			,`vat_amount`					")
                            .AppendLine("			,`wt_amount`					")
                            .AppendLine("			,`remark`					")
                            .AppendLine("			,`bank_fee`					")
                            .AppendLine("			,`created_by`					")
                            .AppendLine("			,`created_date`					")
                            .AppendLine("			,`updated_by`					")
                            .AppendLine("			,`updated_date`)					")
                            .AppendLine("	VALUES (?receive_header_id							")
                            .AppendLine("		 ,?job_order_id						")
                            .AppendLine("		 ,?job_order_po_id						")
                            .AppendLine("		 ,?hontai_type						")
                            .AppendLine("		 ,?actual_rate						")
                            .AppendLine("		 ,?amount						")
                            .AppendLine("		 ,?vat_id						")
                            .AppendLine("		 ,?wt_id						")
                            .AppendLine("		 ,?vat_amount						")
                            .AppendLine("		 ,?wt_amount						")
                            .AppendLine("		 ,?remark						")
                            .AppendLine("		 ,?bank_fee						")
                            .AppendLine("		 ,?user_id						")
                            .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','') 						")
                            .AppendLine("		 ,?user_id						")
                            .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')); 						")
                        End With

                        ' assign parameter
                        With objConn
                            .ClearParameter()
                            .AddParameter("?receive_header_id", objSaleInvoiceEnt.id)
                            .AddParameter("?job_order_id", row("job_order_id"))
                            .AddParameter("?job_order_po_id", row("job_order_po_id"))
                            .AddParameter("?hontai_type", IIf(String.IsNullOrEmpty(row("hontai_type").ToString.Trim), DBNull.Value, row("hontai_type")))
                            .AddParameter("?actual_rate", IIf(String.IsNullOrEmpty(row("actual_rate").ToString.Trim), DBNull.Value, row("actual_rate")))
                            .AddParameter("?amount", CDec(IIf(String.IsNullOrEmpty(row("amount").ToString.Trim), 0, row("amount"))))
                            .AddParameter("?vat_id", row("vat_id"))
                            .AddParameter("?wt_id", row("wt_id"))
                            .AddParameter("?vat_amount", vat_amount)
                            .AddParameter("?wt_amount", wt_amount)
                            .AddParameter("?remark", row("remark"))
                            .AddParameter("?bank_fee", CDec(IIf(String.IsNullOrEmpty(row("bank_fee").ToString.Trim), 0, row("bank_fee"))))
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                        End With

                        ' execute nonquery with sql command
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)

                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If

                        'Update flag on job_order_po table
                        With strSql
                            .Length = 0
                            If row("po_type") <> 0 Or row("po_type") = "" Then
                                'case po type : Sample, Material, Delivery, Others
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET po_fg = 1 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            ElseIf row("po_type") = 0 And row("hontai_type") = "1" Then
                                'case Hontai is 1st  
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg1 = 1 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            ElseIf row("po_type") = 0 And row("hontai_type") = "2" Then
                                'case Hontai is 2nd  
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg2 = 1 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            ElseIf row("po_type") = 0 And row("hontai_type") = "3" Then
                                'case Hontai is 3rd   
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg3 = 1 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            End If
                        End With

                        objConn.ClearParameter()
                        objConn.AddParameter("?job_order_po_id", row("job_order_po_id"))

                        ' execute nonquery with sql command (update 
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)

                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If
                    Else
                        With strSql
                            .Length = 0
                            .AppendLine("	UPDATE receive_detail 							")
                            .AppendLine("	SET actual_rate = ?actual_rate				")
                            .AppendLine("	, vat_id = ?vat_id				")
                            .AppendLine("	, wt_id = ?wt_id				")
                            .AppendLine("	, vat_amount = ?vat_amount ")
                            .AppendLine("	, wt_amount = ?wt_amount					")
                            .AppendLine("	, remark = ?remark					")
                            .AppendLine("	, bank_fee = ?bank_fee					")
                            .AppendLine("	, updated_date= REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','') ")
                            .AppendLine("	, updated_by=?user_id 					")
                            .AppendLine("	WHERE id = ?id	;				")

                        End With

                        ' assign parameter
                        With objConn
                            .ClearParameter()
                            .AddParameter("?id", row("id"))
                            .AddParameter("?actual_rate", IIf(String.IsNullOrEmpty(row("actual_rate").ToString.Trim), DBNull.Value, row("actual_rate")))
                            .AddParameter("?vat_id", row("vat_id"))
                            .AddParameter("?wt_id", row("wt_id"))
                            .AddParameter("?vat_amount", vat_amount)
                            .AddParameter("?wt_amount", wt_amount)
                            .AddParameter("?remark", row("remark"))
                            .AddParameter("?bank_fee", CDec(IIf(String.IsNullOrEmpty(row("bank_fee").ToString.Trim), 0, row("bank_fee"))))
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                        End With

                        ' execute nonquery with sql command
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)

                        ' check row effect 
                        If intEff <= 0 Then
                            ' case have error then exit for
                            Exit For
                        End If
                    End If

                Next
                ' check row effect finally
                If intEff > 0 Then
                    ' case row effect > 0 then commit transaction
                    objConn.CommitTrans()
                    ' Call function CheckFinishJobOrder to get finish job order 
                    strJobOrder = CheckFinishJobOrder(objSaleInvoiceEnt.id)
                    ' Update finish_fg on job order table               
                    fgFinishGoodsJobOrder(strJobOrder)
                    UpdateSaleInvoice = intEff
                Else
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateSaleInvoice(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("UpdateSaleInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderHontai
        '	Discription	    : Get job order for update hontai flag
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderHontai( _
            ByVal dtValues As System.Data.DataTable _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetJobOrderHontai
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetJobOrderHontai = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim str1st As String
                Dim str2nd As String
                Dim str3rd As String
                Dim strJobPoId1 As String = ""
                Dim strJobPoId2 As String = ""
                Dim strJobPoId3 As String = ""

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows

                    'Check payment condition = 0 on job_order_po table
                    With strSql
                        .Length = 0
                        .AppendLine("   SELECT jp.id, jp.job_order_id, jp.po_type, j.payment_condition_id, ")
                        .AppendLine("    pc.1st, pc.2nd, pc.3rd ")
                        .AppendLine("   FROM job_order j ")
                        .AppendLine("   LEFT JOIN job_order_po jp ON jp.job_order_id = j.id AND jp.delete_fg <> 1  ")
                        .AppendLine("   LEFT JOIN mst_payment_condition pc  ")
                        .AppendLine("   ON j.payment_condition_id = pc.id  ")
                        .AppendLine("   AND pc.delete_fg <> 1 ")
                        .AppendLine("   WHERE jp.po_type = 0 ")
                        .AppendLine("   AND (pc.1st = 0 or pc.2nd = 0 or pc.3rd = 0) ")
                        .AppendLine("   AND jp.job_order_id IN (?job_order_id); ")
                    End With

                    ' new connection object
                    objConn = New Common.DBConnection.MySQLAccess
                    objConn.ClearParameter()
                    objConn.AddParameter("?job_order_id", row("job_order_id"))

                    ' execute reader
                    dr = objConn.ExecuteReader(strSql.ToString)

                    ' check exist data
                    If dr.HasRows Then
                        While dr.Read
                            str1st = IIf(IsDBNull(dr.Item("1st")), Nothing, dr.Item("1st"))
                            If str1st = "0" Then
                                If strJobPoId1 = "" Then
                                    strJobPoId1 = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                Else
                                    strJobPoId1 = strJobPoId1 & "," & IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                End If
                            End If
                            str2nd = IIf(IsDBNull(dr.Item("2nd")), Nothing, dr.Item("2nd"))
                            If str2nd = "0" Then
                                If strJobPoId2 = "" Then
                                    strJobPoId2 = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                Else
                                    strJobPoId2 = strJobPoId2 & "," & IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                End If
                            End If
                            str3rd = IIf(IsDBNull(dr.Item("3rd")), Nothing, dr.Item("3rd"))
                            If str3rd = "0" Then
                                If strJobPoId3 = "" Then
                                    strJobPoId3 = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                Else
                                    strJobPoId3 = strJobPoId3 & "," & IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                End If
                            End If
                        End While
                    End If
                Next

                ' assign data from db to entity object
                With GetJobOrderHontai
                    .strJobOrder1 = strJobPoId1
                    .strJobOrder2 = strJobPoId2
                    .strJobOrder3 = strJobPoId3
                End With

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderHontai(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderHontai(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountTitleForList
        '	Discription	    : Get data Account Title for set dropdownlist
        '	Return Value	: List 
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of Entity.ISale_InvoiceEntity) Implements ISale_InvoiceDao.GetAccountTitleForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetAccountTitleForList = New List(Of Entity.ISale_InvoiceEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable job type entity
                Dim objAccTitleEnt As Entity.ISale_InvoiceEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT i.id, 		")
                    .AppendLine("		 CONCAT(i.code,' - ',i.name) AS name 	")
                    .AppendLine("	FROM mst_ie i 		")
                    .AppendLine("   INNER JOIN mst_ie_category c ON i.category_id = c.id ")
                    .AppendLine("   WHERE i.delete_fg <> 1 ")
                    .AppendLine("   AND c.category_type = 2 ")
                    .AppendLine("	ORDER BY i.id		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new Account Title entity
                        objAccTitleEnt = New Entity.ImpSale_InvoiceEntity
                        With objAccTitleEnt
                            ' assign data to object job type entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With
                        ' add object Account Title entity to list
                        GetAccountTitleForList.Add(objAccTitleEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountTitleForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetAccountTitleForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderPOFlag
        '	Discription	    : Update flag on job_order_po when delete data on detail
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderPOFlag( _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements ISale_InvoiceDao.DeleteJobOrderPOFlag
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderPOFlag = -1

            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    'case hontai_fg1,hontai_fg2,hontai_fg3,po_fg is nothing
                    If (row("hontai_fg1") <> "" And Not row("hontai_fg1") Is Nothing) _
                        Or (row("hontai_fg2") <> "" And Not row("hontai_fg2") Is Nothing) _
                        Or (row("hontai_fg3") <> "" And Not row("hontai_fg3") Is Nothing) _
                        Or (row("po_fg") <> "" And Not row("po_fg") Is Nothing) Then

                        ' assign sql command
                        ' Case hontai
                        If row("po_type") = "0" Then
                            Select Case row("hontai_type").ToString
                                Case "1" 'Hontai_fg1
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg1 = 0 							")
                                        .AppendLine("		WHERE id = ?job_order_po_id							")
                                    End With
                                Case "2" 'Hontai_fg2
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg2 = 0 							")
                                        .AppendLine("		WHERE id = ?job_order_po_id							")
                                    End With
                                Case "3" 'Hontai_fg3
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg3 = 0 							")
                                        .AppendLine("		WHERE id = ?job_order_po_id							")
                                    End With
                            End Select
                        Else
                            'po_fg
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                     ")
                                .AppendLine("		SET po_fg = 0 							")
                                .AppendLine("		WHERE id = ?job_order_po_id  ")
                                .AppendLine("		AND po_type = ?po_type  ")
                            End With
                        End If

                        ' assign parameter
                        objConn.ClearParameter()
                        objConn.AddParameter("?job_order_po_id", row("job_order_po_id"))
                        objConn.AddParameter("?po_type", row("po_type"))

                        objLog.InfoLog("job_order_po_id :", row("job_order_po_id"))

                        ' execute non query and keep row effect
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        If intEff <= 0 Then
                            ' case row effect <= 0 then rollback transaction
                            objConn.RollbackTrans()
                            Exit For
                        End If
                    End If
                Next
                ' check row effect
                If intEff >= 0 Then
                    If strSql.ToString = "" Then
                        DeleteJobOrderPOFlag = 1
                        Exit Function
                    Else
                        ' case row effect more than 0 then commit transaction
                        objConn.CommitTrans()
                    End If
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                DeleteJobOrderPOFlag = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderPOFlag(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderPOFlag(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If strSql.ToString <> "" Then
                    If Not objConn Is Nothing Then objConn.Close()
                End If
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrderPOFlag
        '	Discription	    : Update flag on job_order_po when cancel  process
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrderPOFlag( _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements ISale_InvoiceDao.UpdateJobOrderPOFlag
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateJobOrderPOFlag = -1

            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    'case hontai_fg1,hontai_fg2,hontai_fg3,po_fg is nothing
                    If (row("hontai_fg1") <> "" And Not row("hontai_fg1") Is Nothing) _
                        Or (row("hontai_fg2") <> "" And Not row("hontai_fg2") Is Nothing) _
                        Or (row("hontai_fg3") <> "" And Not row("hontai_fg3") Is Nothing) _
                        Or (row("po_fg") <> "" And Not row("po_fg") Is Nothing) Then

                        ' assign sql command
                        'Case hontai
                        If row("po_type") = "0" Then
                            Select Case row("hontai_type")
                                Case "1" 'Hontai_fg1
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg1 = ?hontai_fg1					")
                                        .AppendLine("		WHERE id = ?job_order_po_id                     ")
                                    End With
                                Case "2" 'Hontai_fg2
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg2 = ?hontai_fg2					")
                                        .AppendLine("		WHERE id = ?job_order_po_id                     ")
                                    End With
                                Case "3" 'Hontai_fg3
                                    With strSql
                                        .Length = 0
                                        .AppendLine("       UPDATE job_order_po                             ")
                                        .AppendLine("		SET hontai_fg3 = ?hontai_fg3					")
                                        .AppendLine("		WHERE id = ?job_order_po_id						")
                                    End With
                            End Select
                        Else
                            'po_fg
                            With strSql
                                .Length = 0
                                .AppendLine("       UPDATE job_order_po                             ")
                                .AppendLine("		SET po_fg = ?po_fg 							")
                                .AppendLine("		WHERE id = ?job_order_po_id  ")
                                .AppendLine("		AND po_type = ?po_type  ")
                            End With
                        End If

                        ' assign parameter
                        objConn.ClearParameter()
                        objConn.AddParameter("?job_order_po_id", row("job_order_po_id"))
                        objConn.AddParameter("?po_type", row("po_type"))
                        objConn.AddParameter("?hontai_fg1", row("hontai_fg1"))
                        objConn.AddParameter("?hontai_fg2", row("hontai_fg2"))
                        objConn.AddParameter("?hontai_fg3", row("hontai_fg3"))
                        objConn.AddParameter("?po_fg", row("po_fg"))

                        ' execute non query and keep row effect
                        intEff = objConn.ExecuteNonQuery(strSql.ToString)
                        If intEff <= 0 Then
                            ' case row effect <= 0 then rollback transaction
                            objConn.RollbackTrans()
                            Exit For
                        End If
                    End If
                Next
                ' check row effect
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    If strSql.ToString = "" Then
                        UpdateJobOrderPOFlag = 1
                        Exit Function
                    Else
                        objConn.CommitTrans()
                    End If

                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                UpdateJobOrderPOFlag = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrderPOFlag(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateJobOrderPOFlag(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If strSql.ToString <> "" Then
                    If Not objConn Is Nothing Then objConn.Close()
                End If
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataBankFreeList
        '	Discription	    : Get data for update bank free
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 26-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataBankFreeList( _
            ByVal strReceive_header_id As String _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetDataBankFreeList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetDataBankFreeList = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT DISTINCT rh.id, rh.vendor_id, rh.receipt_date, jb.job_order						")
                    .AppendLine("		, rd.vat_id, rd.wt_id, rh.ie_id, rd.amount * v.percent / 100 AS vat_amount			")
                    .AppendLine("		, rd.amount * wt.percent / 100 AS wt_amount		                                    ")
                    .AppendLine("		, rd.amount AS sub_total, rd.actual_rate, rd.remark		                            ")
                    .AppendLine("	FROM receive_header rh 																	")
                    .AppendLine("		LEFT JOIN  receive_detail rd ON rh.id = rd.receive_header_id 						")
                    .AppendLine("		LEFT JOIN job_order jb ON jb.id = rd.job_order_id 									")
                    .AppendLine("		LEFT JOIN (	SELECT id, ref_id FROM accounting 										")
                    .AppendLine("								WHERE type = 4) acc ON acc.ref_id = rh.id  					")
                    .AppendLine("									LEFT JOIN mst_wt wt ON rd.wt_id = wt.id 				")
                    .AppendLine("									LEFT JOIN mst_vat v ON rd.vat_id = v.id 				")
                    .AppendLine("	WHERE (FIND_IN_SET(CAST(rh.id AS CHAR), ?receive_header_id) > 0)  						")
                    .AppendLine("	UNION																					")
                    .AppendLine("	SELECT rh.id, rh.vendor_id, rh.receipt_date, j.job_order, 1, 1, 122, 0, 0  				")
                    .AppendLine("	  , rd.bank_fee AS sub_total, rd.actual_rate, 'Bank Fee'    							")
                    .AppendLine("	FROM (	SELECT receive_header_id, job_order_id, SUM(bank_fee) AS bank_fee, actual_rate	")
                    .AppendLine("					FROM receive_detail  GROUP BY receive_header_id) rd 					")
                    .AppendLine("		LEFT JOIN receive_header rh ON rd.receive_header_id = rh.id AND rh.status_id <> 6	")
                    .AppendLine("		LEFT JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6				")
                    .AppendLine("	WHERE (FIND_IN_SET(CAST(rh.id AS CHAR), ?receive_header_id) > 0);						")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", strReceive_header_id)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .receive_header_id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .vat_id = IIf(IsDBNull(dr.Item("vat_id")), Nothing, dr.Item("vat_id"))
                            .wt_id = IIf(IsDBNull(dr.Item("wt_id")), Nothing, dr.Item("wt_id"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .actual_rate = IIf(IsDBNull(dr.Item("actual_rate")), Nothing, dr.Item("actual_rate"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                        End With
                        ' add item to list
                        GetDataBankFreeList.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataBankFreeList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetDataBankFreeList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataReceiveDetail
        '	Discription	    : Get data for update job_order_po
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataReceiveDetail( _
            ByVal strReceive_header_id As String _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetDataReceiveDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetDataReceiveDetail = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT	 ")
                    .AppendLine("	id 	 ")
                    .AppendLine("	,job_order_id	 ")
                    .AppendLine("	,job_order_po_id	 ")
                    .AppendLine("	,hontai_type	 ")
                    .AppendLine("	from receive_detail ")
                    .AppendLine("	WHERE receive_header_id = ?receiver_header_id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receiver_header_id", strReceive_header_id)


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .job_order_po_id = IIf(IsDBNull(dr.Item("job_order_po_id")), Nothing, dr.Item("job_order_po_id"))
                            .hontai_type = IIf(IsDBNull(dr.Item("hontai_type")), Nothing, dr.Item("hontai_type"))
                        End With
                        ' add item to list
                        GetDataReceiveDetail.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataReceiveDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetDataReceiveDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderFinish
        '	Discription	    : Get hontai for update finish flag
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 17-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetHontaiFinish( _
            ByVal intRefID As Integer _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetHontaiFinish
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetHontaiFinish = New Entity.ImpSale_InvoiceEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader
                Dim job_order_id As String
                Dim count As Integer = 0
                Dim before_id As Integer
                Dim intChkN As Integer = 0
                Dim intChkY As Integer = 0
                Dim strJobOrder As String = ""

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With strSql
                    .Length = 0
                    .AppendLine("   SELECT rh.id, rh.invoice_no, rd.id AS receive_detail_id, rd.job_order_id , rd.job_order_po_id ")
                    .AppendLine("       , (CASE jp.po_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others' ELSE '' END) AS po_type ")
                    .AppendLine("       , IF (jp.po_type = 0, (IF (jp.hontai_fg1 = 1 AND jp.hontai_fg2 = 1 AND jp.hontai_fg3 = 1, 'Y', 'N')), (IF (jp.po_fg = 1, 'Y', 'N'))) AS finish_goods ")
                    .AppendLine("   FROM receive_header rh ")
                    .AppendLine("   LEFT JOIN receive_detail rd ON rd.receive_header_id = rh.id  ")
                    .AppendLine("   LEFT JOIN job_order_po jp ON rd.job_order_po_id = jp.id ")
                    .AppendLine("   WHERE rh.status_id <> 6    ")
                    .AppendLine("   AND (FIND_IN_SET(CAST(rh.id AS CHAR), ?receive_header_id) > 0);  ")
                End With
                objConn.ClearParameter()
                objConn.AddParameter("?receive_header_id", intRefID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                        count += 1
                        If job_order_id > 0 Then
                            'Check Job order id = before job order id
                            If before_id = job_order_id Then
                                If intChkN = 0 Then
                                    'Check finish_goods is 'N' or 'Y'
                                    If dr.Item("finish_goods") = "N" Then
                                        intChkN += 1
                                    ElseIf dr.Item("finish_goods") = "Y" Then
                                        intChkY += 1
                                    End If
                                Else
                                    Dim arrJobOrder() As String = Split(strJobOrder, ",")
                                    For i As Integer = 0 To arrJobOrder.Length - 1
                                        strJobOrder = ""
                                        If job_order_id <> arrJobOrder(i) Then
                                            If strJobOrder = "" Then
                                                strJobOrder = arrJobOrder(i)
                                            Else
                                                strJobOrder = strJobOrder & "," & arrJobOrder(i)
                                            End If
                                        End If
                                    Next
                                    intChkY = 0
                                End If
                            Else
                                intChkN = 0
                                intChkY = 0

                                'Check finish_goods is 'N' or 'Y'
                                If dr.Item("finish_goods") = "N" Then
                                    intChkN += 1
                                ElseIf dr.Item("finish_goods") = "Y" Then
                                    'Set Job order Id to array
                                    If strJobOrder = "" Then
                                        strJobOrder = job_order_id
                                    Else
                                        strJobOrder = strJobOrder & "," & job_order_id
                                    End If
                                    intChkY += 1
                                End If

                            End If
                            'Keep before job order 
                            before_id = job_order_id
                        End If
                    End While
                End If


                ' assign data from db to entity object
                With GetHontaiFinish
                    .strJobOrder1 = strJobOrder
                End With

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetHontaiFinish(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetHontaiFinish(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetConfirmReceiveForReport
        '	Discription	    : Get data confirm receive Report list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    : Rawikarn K.
        '	Update Date	    : 18-04-2014
        '*************************************************************/
        Public Function GetConfirmReceiveForReport( _
            ByVal strReceiceHeaderId As String _
        ) As System.Collections.Generic.List(Of Entity.ImpSale_InvoiceEntity) Implements ISale_InvoiceDao.GetConfirmReceiveForReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetConfirmReceiveForReport = New List(Of Entity.ImpSale_InvoiceEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objSaleInvoiceDetail As Entity.ImpSale_InvoiceEntity

                ' assign sql command
                With strSql
                    .AppendLine("   SELECT rh.id AS receive_header_id, rd.id AS receive_detail_id				")
                    .AppendLine("   	, rd.job_order_id, j.job_order, v.name AS customer_name				    ")
                    .AppendLine("   	, IFNULL(rd.bank_fee,0) AS bank_fee   			            	        ")
                    .AppendLine("   	, IF(rh.actual_amount IS NULL OR rh.actual_amount = ''					")
                    .AppendLine("   		, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0) * 				")
                    .AppendLine("   		(vat.percent / 100)), SUM(IFNULL(rh.bank_rate,1) * 					")
                    .AppendLine("   		IFNULL(rd.amount,0) * (vat.percent / 100))) as vat_amount_thb		")
                    .AppendLine("       , IF(rh.actual_amount IS NULL OR rh.actual_amount = ''					")
                    .AppendLine("   		, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0) *				")
                    .AppendLine("   		(wt.percent / 100)), SUM(IFNULL(rh.bank_rate,1) *					")
                    .AppendLine("   		IFNULL(rd.amount,0) * (wt.percent / 100))) as wt_amount_thb			")
                    .AppendLine("       , IF(rh.actual_amount IS NULL OR rh.actual_amount = ''					")
                    .AppendLine("   		, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0))				")
                    .AppendLine("   		, SUM(IFNULL(rh.bank_rate,1) * IFNULL(rd.amount,0))) as amount_thb	")
                    .AppendLine("       , rh.invoice_no, rh.receipt_date, rh.invoice_type										")
                    .AppendLine("   	, CONCAT(u.first_name,' ',u.last_name) as user_login					")
                    .AppendLine("   FROM receive_header rh														")
                    .AppendLine("   	LEFT JOIN receive_detail rd ON rd.receive_header_id = rh.id				")
                    .AppendLine("   	INNER JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6	")
                    .AppendLine("   	LEFT JOIN mst_vendor v ON j.customer = v.id AND v.delete_fg <> 1		")
                    .AppendLine("   	LEFT JOIN user u on u.id = ?user_id										")
                    .AppendLine("   	LEFT JOIN mst_vat vat ON rd.vat_id = vat.id								")
                    .AppendLine("   	LEFT JOIN mst_wt wt ON rd.wt_id = wt.id									")
                    .AppendLine("   WHERE rh.status_id <> 6														")
                    .AppendLine("   	AND (FIND_IN_SET(CAST(rh.id AS CHAR), ?receive_header_id) > 0)			")
                    .AppendLine("   GROUP BY rh.invoice_no										                ")
                    .AppendLine("   ORDER BY rh.invoice_type DESC,rh.invoice_no ASC	;														")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", strReceiceHeaderId)
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                objLog.InfoLog("receive_header_id", strReceiceHeaderId)
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objSaleInvoiceDetail = New Entity.ImpSale_InvoiceEntity
                        ' assign data from db to entity object
                        With objSaleInvoiceDetail
                            .receive_header_id = IIf(IsDBNull(dr.Item("receive_header_id")), Nothing, dr.Item("receive_header_id"))
                            .receive_detail_id = IIf(IsDBNull(dr.Item("receive_detail_id")), Nothing, dr.Item("receive_detail_id"))
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer_name")), Nothing, dr.Item("customer_name"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount_thb")), Nothing, dr.Item("vat_amount_thb"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount_thb")), Nothing, dr.Item("wt_amount_thb"))
                            .amount = IIf(IsDBNull(dr.Item("amount_thb")), Nothing, dr.Item("amount_thb"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .name = IIf(IsDBNull(dr.Item("user_login")), Nothing, dr.Item("user_login"))
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))

                        End With
                        ' add item to list
                        GetConfirmReceiveForReport.Add(objSaleInvoiceDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetConfirmReceiveForReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetConfirmReceiveForReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumConfirmReport
        '	Discription	    : Get Sum vat and sum w/t for Report 
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumConfirmReport( _
             ByVal strReceiceHeaderId As String _
        ) As Entity.ISale_InvoiceEntity Implements ISale_InvoiceDao.GetSumConfirmReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumConfirmReport = New Entity.ImpSale_InvoiceEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(rd.vat_amount) AS sum_vat ")
                    .AppendLine("   ,SUM(rd.wt_amount) AS sum_wt ")
                    .AppendLine(" FROM receive_header rh  ")
                    .AppendLine(" LEFT JOIN receive_detail rd ON rd.receive_header_id = rh.id  ")
                    .AppendLine(" INNER JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine(" WHERE rh.status_id <> 6 ")
                    .AppendLine(" AND (FIND_IN_SET(CAST(rh.id AS CHAR), ?receive_header_id) > 0) 	 ")
                    .AppendLine(" ORDER BY j.job_order; ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?receive_header_id", strReceiceHeaderId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSumConfirmReport
                            .sum_wt = IIf(IsDBNull(dr.Item("sum_wt")), Nothing, dr.Item("sum_wt"))
                            .sum_vat = IIf(IsDBNull(dr.Item("sum_vat")), Nothing, dr.Item("sum_vat"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumConfirmReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumConfirmReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DelInvReceiveDetail                       '
        '	Discription	    : Delete Invoice in Receive Detail          '
        '	Return Value	: Boolean                                   '
        '	Create User	    : Rawikarn K.                               '
        '	Create Date	    : 11-03-2014                                '
        '	Update User	    :                                           '
        '	Update Date	    :                                           '    
        '*************************************************************/
        Public Function DelInvReceiveDetail( _
             ByVal strIntID As Integer _
        ) As Integer Implements ISale_InvoiceDao.DelInvReceiveDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder

            Try
                ' fd keep row effect
                Dim fd As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   DELETE receive_detail     ")
                    .AppendLine("   WHERE id = ?id   ")
                End With

                objConn.ClearParameter()
                objConn.AddParameter("?id", strIntID)

                ' execute nonquery with sql command (update 
                fd = objConn.ExecuteNonQuery(strSql.ToString)
                If fd < 0 Then
                    ' case row effect < 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DelInvReceiveDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("DelInvReceiveDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdateReciveDetail                       '
        '	Discription	    : Update JobOrderPO in Receive Detail          '
        '	Return Value	: Boolean                                   '
        '	Create User	    : Rawikarn K.                               '
        '	Create Date	    : 06-05-2014                                '
        '	Update User	    :                                           '
        '	Update Date	    :                                           '    
        '*************************************************************/
        Public Function UpdateReciveDetail( _
            ByVal tbIntIdJBPO As System.Data.DataTable, _
            ByVal objIDSaleInv As System.Data.DataTable, _
            ByVal intJobOrderID As Integer) _
        As Boolean Implements ISale_InvoiceDao.UpdateReciveDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            Try
                ' fd keep row effect
                Dim fd As Integer
                ' assign sql command
                With strSql
                    .AppendLine("   Update receive_detail                       ")
                    .AppendLine("       SET job_order_po_id = ?job_ordre_po_id  ")
                    .AppendLine("   WHERE id = ?id                              ")
                    .AppendLine("       AND    hontai_type = ?hontai_type       ")
                    .AppendLine("       AND    job_order_id = ?job_order_id     ")
                End With

                objConn.ClearParameter()
                With objConn
                    .AddParameter("?job_ordre_po_id", tbIntIdJBPO.Rows.Item("id").ToString)
                    .AddParameter("?id", objIDSaleInv.Rows.Item("id").ToString)
                    .AddParameter("?hontai_type", objIDSaleInv.Rows.Item("hontai_type").ToString)
                    .AddParameter("?job_order_id", objIDSaleInv.Rows.Item("job_order_id").ToString)

                End With

                ' execute nonquery with sql command (update 
                fd = objConn.ExecuteNonQuery(strSql.ToString)


            Catch ex As Exception
                objLog.ErrorLog("UpdateReciveDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SumBankfeeConfirmReport
        '	Discription	    : Get Sum Bank fee for Report 
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 07-05-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SumBankfeeConfirmReport( _
            ByVal strReceiceHeaderId As String _
            ) As Entity.ISale_InvoiceEntity _
        Implements ISale_InvoiceDao.SumBankfeeConfirmReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            SumBankfeeConfirmReport = New Entity.ImpSale_InvoiceEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(bank_fee) as bank_fee ")
                    .AppendLine(" FROM receive_detail rd  ")
                    .AppendLine("   WHERE (FIND_IN_SET(CAST(receive_header_id AS CHAR), ?receive_header_id) > 0) ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                With objConn
                    .AddParameter("?receive_header_id", strReceiceHeaderId)
                End With


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With SumBankfeeConfirmReport
                            .bank_fee = IIf(IsDBNull(dr.Item("bank_fee")), Nothing, dr.Item("bank_fee"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SumBankfeeConfirmReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("SumBankfeeConfirmReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function


        '/**************************************************************
        '	Function name	: GetSaleInvoiceforUpdate
        '	Discription	    : Get Sum vat and sum w/t for Report 
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSaleInvoiceforUpdate( _
            ByVal strReceiceHeaderId As Integer _
            ) As Entity.ISale_InvoiceEntity _
        Implements ISale_InvoiceDao.GetSaleInvoiceforUpdate
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSaleInvoiceforUpdate = New Entity.ImpSale_InvoiceEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT rd.id, rh.invoice_no, rd.hontai_type ")
                    .AppendLine(" FROM receive_header rh  ")
                    .AppendLine("   LEFT JOIN receive_detail rd ON rd.receive_header_id = rh.id  ")
                    .AppendLine(" INNER JOIN job_order j ON rd.job_order_id = j.id AND j.status_id <> 6 ")
                    .AppendLine(" WHERE rh.status_id <> 6 ")
                    .AppendLine("   AND rd.job_order_id = ?job_order_id	 ")
                    .AppendLine(" ORDER BY j.job_order; ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", strReceiceHeaderId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetSaleInvoiceforUpdate
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .hontai_type = IIf(IsDBNull(dr.Item("hontai_type")), Nothing, dr.Item("hontai_type"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSaleInvoiceforUpdate(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSaleInvoiceforUpdate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: UpExChangeRate
        '	Discription	    : Update ExChange Rate for Confirm 
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 15-09-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Function UpExChangeRate( _
            ByVal strReceiveHeaderId As String, _
            ByVal ExchangeRate As Integer _
        ) As Boolean _
        Implements ISale_InvoiceDao.UpExChangeRate
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            UpExChangeRate = False
            Try
                ' fd keep row effect
                Dim fd As Integer
                ' assign sql command
                With strSql
                    .AppendLine("   UPDATE receive_detail                           ")
                    .AppendLine("       SET actual_rate =  ?actual_rate             ")
                    .AppendLine("   WHERE id IN (?id)                               ")
                End With

                objConn.ClearParameter()
                With objConn
                    .AddParameter("?actual_rate", ExchangeRate)
                    .AddParameter("?id", strReceiveHeaderId)
                End With

                ' execute nonquery with sql command (update )
                fd = objConn.ExecuteNonQuery(strSql.ToString)

                If fd < 0 Then
                    ' case row effect < 0 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                Else
                    UpExChangeRate = True
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpExChangeRate(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))

                objLog.InfoLog("UpExChangeRate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetActualRate
        '	Discription	    : Get Actual Rate 
        '	Return Value	: ISale_InvoiceEntity Object
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 15-09-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Function GetActualRate(ByVal IntID As Integer) As Integer Implements ISale_InvoiceDao.GetActualRate
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            Try
                ' fd keep row effect
                Dim fd As Integer
                With strSql
                    .AppendLine("   SELECT * ")
                    .AppendLine("   FROM po_header ")
                    .AppendLine("   where id = ?id")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IntID)

                ' execute reader
                fd = objConn.ExecuteNonQuery(strSql.ToString)

            Catch ex As Exception
                objLog.InfoLog("GetActualRate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateExChangeRate
        '	Discription	    : Update Actual Rate 
        '	Return Value	: Boolean 
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 15-09-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Function UpdateExChangeRate(ByVal intID As Integer, ByVal objExChangeRate As System.Data.DataTable) As Boolean Implements ISale_InvoiceDao.UpdateExChangeRate
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            Try
                ' fd keep row effect
                Dim fd As Integer
                With strSql
                    .AppendLine("   UPDATE receive_detail SET               ")
                    .AppendLine("           actual_rate = ?objExChangeRate  ")
                    .AppendLine("   where id IN (?id)                          ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intID)

                ' execute reader
                fd = objConn.ExecuteNonQuery(strSql.ToString)

            Catch ex As Exception
                objLog.InfoLog("UpdateExChangeRate(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


#End Region
    End Class
End Namespace
