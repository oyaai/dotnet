
#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Exception
#End Region

Namespace Dao
    Public Class ImpAcc_PaymentDao
        Implements IAcc_PaymentDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private strMsgErr As String = String.Empty

#Region "Functions"

        '/**************************************************************
        '	Function name	: GetDataForVoucherReport
        '	Discription	    : Get Data For Payment Voucher Report
        '	Return Value	: List of Dto
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForVoucherReport(ByVal voucherList As String) As System.Collections.Generic.List(Of Entity.ImpAcc_PaymentEntity) Implements IAcc_PaymentDao.GetDataForVoucherReport
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetDataForVoucherReport = New List(Of Entity.ImpAcc_PaymentEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.ImpAcc_PaymentEntity

                ' assign sql statement
                With strSql
                    .AppendLine("   SELECT voucher_no, printdate, account_date, account_type						")
                    .AppendLine("   	, vendorName, account_name, account_no, cheque_no, bank, cheque_date		")
                    .AppendLine("   	, SUM(sub_total) AS sub_total, SUM(vat_amount) AS vat_amount				")
                    .AppendLine("   	, SUM(wt_amount) AS wt_amount, SUM(total) AS total, vat_percent, wt_percent	")
                    .AppendLine("   FROM (	SELECT acc.voucher_no, DATE_FORMAT(NOW(), '%Y%m%d') AS printdate		")
                    .AppendLine("   			, account_date, account_type, vendor.name AS vendorName				")
                    .AppendLine("				, account_name, account_no, acc.cheque_no, acc.bank					")
                    .AppendLine("   			, acc.cheque_date, acc.sub_total AS sub_total						")
                    .AppendLine("   			, acc.vat_amount AS vat_amount, acc.wt_amount AS wt_amount			")
                    .AppendLine("   			, acc.sub_total + acc.vat_amount - acc.wt_amount AS total			")
                    .AppendLine("				, vat.`percent` as vat_percent, wt.`percent` as wt_percent			")
                    .AppendLine("			FROM `accounting` acc													")
                    .AppendLine("				INNER JOIN `mst_vendor` vendor ON (acc.`vendor_id` = vendor.`id`)	")
                    .AppendLine("				INNER JOIN `mst_vat` vat ON (acc.`vat_id` = vat.`id`)				")
                    .AppendLine("				INNER JOIN `mst_wt` wt ON (acc.`wt_id` = wt.`id`)					")
                    .AppendLine("			WHERE ((FIND_IN_SET(acc.`voucher_no`, ?voucher_list) > 0))				")
                    .AppendLine("				AND acc.status_id <> 6												")
                    .AppendLine("			ORDER BY acc.account_date) AS DD										")
                    .AppendLine("	GROUP BY DD.voucher_no;															")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?voucher_list", voucherList)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAcc_PaymentEntity
                        With objAccEnt
                            ' assign data to object Vendor entity
                            .voucherNoAsInt = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .dateNow = IIf(IsDBNull(dr.Item("printdate")), Nothing, dr.Item("printdate"))
                            .ReceiptDate = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .AccountType = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .vendorName = IIf(IsDBNull(dr.Item("vendorName")), Nothing, dr.Item("vendorName"))
                            .AccountName = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .AccountNo = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .chequeNo = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .Bank = IIf(IsDBNull(dr.Item("bank")), Nothing, dr.Item("bank"))
                            .chequeDate = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                            .subtotal = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .vatAmount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wtAmount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .Total = IIf(IsDBNull(dr.Item("total")), Nothing, dr.Item("total"))
                            .Vat = IIf(IsDBNull(dr.Item("vat_percent")), Nothing, dr.Item("vat_percent"))
                            .WT = IIf(IsDBNull(dr.Item("wt_percent")), Nothing, dr.Item("wt_percent"))
                        End With
                        ' add object Vendor entity to list
                        GetDataForVoucherReport.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForVoucherReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetDataForVoucherReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataForWTReport
        '	Discription	    : Get Data For Payment Withholding Tax Report
        '	Return Value	: List of Dto
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForWTReport(ByVal voucherList As String) As System.Collections.Generic.List(Of Entity.ImpAcc_PaymentEntity) Implements IAcc_PaymentDao.GetDataForWTReport
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetDataForWTReport = New List(Of Entity.ImpAcc_PaymentEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.ImpAcc_PaymentEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT acc.voucher_no, acc.id, vendor.`name` AS vendor_name						")
                    .AppendLine("		, IF(ISNULL(acc.vendor_branch_id) OR acc.vendor_branch_id = '' 				")
                    .AppendLine("		OR acc.vendor_branch_id = 0, CONCAT(vendor.address, ' ', vendor.zipcode)	")
                    .AppendLine("		, CONCAT(vb.address, ' ', vb.zipcode)) AS vendor_address					")
                    .AppendLine("		, vendor.`type1` AS vendor_type1, vendor.`type2` AS vendor_type2			")
                    .AppendLine("		, vendor.`type2_no` AS vendor_type2_no										")
                    .AppendLine("		, MIN(acc.`account_date`) AS account_date									")
                    .AppendLine("		, SUM(acc.`sub_total`) AS sub_total, wt.`percent` AS wt_percent			    ")
                    .AppendLine("		, SUM(acc.`wt_amount`) AS wt_amount, wt.`type` AS wt_type		            ")
                    .AppendLine("	FROM `accounting` acc															")
                    .AppendLine("		INNER JOIN `mst_vendor` vendor ON acc.`vendor_id` = vendor.`id`				")
                    .AppendLine("		INNER JOIN `mst_wt` wt ON acc.`wt_id` = wt.`id`								")
                    .AppendLine("		LEFT JOIN `mst_vendor_branch` vb ON acc.`vendor_branch_id` = vb.`id`		")
                    .AppendLine("	WHERE (acc.`wt_amount` > 0) AND acc.status_id <> 6								")
                    .AppendLine("		AND ((FIND_IN_SET(acc.voucher_no, ?voucher_list) > 0))                      ")
                    '.AppendLine("	GROUP BY acc.voucher_no, acc.vat_id		                  						")
                    .AppendLine("	GROUP BY acc.voucher_no         		                  						")
                    .AppendLine("	ORDER BY acc.account_date;	                  									")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?voucher_list", voucherList)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAcc_PaymentEntity
                        With objAccEnt
                            ' assign data to object Vendor entity
                            .voucherNo = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .accID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .vendorName = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendorAddress = IIf(IsDBNull(dr.Item("vendor_address")), Nothing, dr.Item("vendor_address"))
                            .vendor_type1 = IIf(IsDBNull(dr.Item("vendor_type1")), Nothing, dr.Item("vendor_type1"))
                            .vendor_type2 = IIf(IsDBNull(dr.Item("vendor_type2")), Nothing, dr.Item("vendor_type2"))
                            .vendor_type2_no = IIf(IsDBNull(dr.Item("vendor_type2_no")), Nothing, dr.Item("vendor_type2_no"))
                            .receiptDate = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .subtotal = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .wt = IIf(IsDBNull(dr.Item("wt_percent")), Nothing, dr.Item("wt_percent"))
                            .WTAmount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .wtType = IIf(IsDBNull(dr.Item("wt_type")), Nothing, dr.Item("wt_type"))
                        End With
                        ' add object Vendor entity to list
                        GetDataForWTReport.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForWTReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetDataForWTReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetDataForWTReport
        '	Discription	    : Get Data For Payment Withholding Tax Report
        '	Return Value	: List of Dto
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForWTReportV2(ByVal voucherList As String) As System.Collections.Generic.List(Of Entity.ImpAcc_PaymentEntity) Implements IAcc_PaymentDao.GetDataForWTReportV2
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetDataForWTReportV2 = New List(Of Entity.ImpAcc_PaymentEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.ImpAcc_PaymentEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT YEAR(DATE(acc.`account_date`)) AS year_account_date                      ")
                    .AppendLine("		, MONTH(DATE(acc.`account_date`)) AS month_account_date                     ")
                    .AppendLine("		, acc.id, vendor.`name` AS vendor_name, vendor.`type1` AS vendor_type1      ")
                    .AppendLine("		, vendor.`type2` AS vendor_type2, vendor.`type2_no` AS vendor_type2_no      ")
                    .AppendLine("		, IF(ISNULL(acc.vendor_branch_id) OR acc.vendor_branch_id = '' 				")
                    .AppendLine("		OR acc.vendor_branch_id = 0, CONCAT(vendor.address, ' ', vendor.zipcode)	")
                    .AppendLine("		, CONCAT(vb.address, ' ', vb.zipcode)) AS vendor_address					")
                    .AppendLine("		, CAST(acc.`account_date` AS DATE) AS account_date, acc.`sub_total`         ")
                    .AppendLine("		, wt.`percent` AS wt_percent, acc.`wt_amount`, wt.`type` AS wt_type         ")
                    .AppendLine("	FROM `accounting` acc                                                           ")
                    .AppendLine("		INNER JOIN `mst_vendor` vendor ON acc.`vendor_id` = vendor.`id`             ")
                    .AppendLine("		INNER JOIN `mst_wt` wt ON acc.`wt_id` = wt.`id`                             ")
                    .AppendLine("		LEFT JOIN `mst_vendor_branch` vb ON acc.`vendor_branch_id` = vb.`id`		")
                    .AppendLine("	WHERE (acc.`wt_amount` > 0) AND acc.status_id <> 6                              ")
                    .AppendLine("		AND ((FIND_IN_SET(acc.`voucher_no`, ?voucher_list) > 0))                    ")
                    .AppendLine("	ORDER BY YEAR(DATE(acc.`account_date`)), MONTH(DATE(acc.`account_date`))        ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter
                objConn.AddParameter("?voucher_list", voucherList)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAcc_PaymentEntity
                        With objAccEnt
                            ' assign data to object Vendor entity
                            .receiptYear = IIf(IsDBNull(dr.Item("year_account_date")), Nothing, dr.Item("year_account_date"))
                            .receiptMonth = IIf(IsDBNull(dr.Item("month_account_date")), Nothing, dr.Item("month_account_date"))
                            .accID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .vendorName = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendorAddress = IIf(IsDBNull(dr.Item("vendor_address")), Nothing, dr.Item("vendor_address"))
                            .vendor_type1 = IIf(IsDBNull(dr.Item("vendor_type1")), Nothing, dr.Item("vendor_type1"))
                            .vendor_type2 = IIf(IsDBNull(dr.Item("vendor_type2")), Nothing, dr.Item("vendor_type2"))
                            .vendor_type2_no = IIf(IsDBNull(dr.Item("vendor_type2_no")), Nothing, dr.Item("vendor_type2_no"))
                            .receiptDate = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .subtotal = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .wt = IIf(IsDBNull(dr.Item("wt_percent")), Nothing, dr.Item("wt_percent"))
                            .wtAmount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .wtType = IIf(IsDBNull(dr.Item("wt_type")), Nothing, dr.Item("wt_type"))
                        End With
                        ' add object Vendor entity to list
                        GetDataForWTReportV2.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForWTReportV2(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetDataForWTReportV2(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

#End Region
    End Class

End Namespace
