#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpAccountingDao
'	Class Discription	: Class of table accounting
'	Create User 		: Boon
'	Create Date		    : 15-05-2013
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
    Public Class ImpAccountingDao
        Implements IAccountingDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private strMsgErr As String = String.Empty


        '/**************************************************************
        '	Function name	: DB_CheckAccountByVendor 
        '	Discription	    : Check Account by vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckAccountByVendor(ByVal intVendor_id As Integer) As Boolean Implements IAccountingDao.DB_CheckAccountByVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckAccountByVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As account_count ")
                    .AppendLine(" FROM accounting ")
                    .AppendLine(" WHERE vendor_id = ?VendorId ")
                    ' assign parameter
                    objConn.AddParameter("?VendorId", intVendor_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckAccountByVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckAccountByVendor = False
                objLog.ErrorLog("DB_CheckAccountByVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckAccountByVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CheckAccountByPurchase
        '	Discription	    : Check Account by purchase
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckAccountByPurchase(ByVal intPurchase_id As Integer) As Boolean Implements IAccountingDao.DB_CheckAccountByPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckAccountByPurchase = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As account_count ")
                    .AppendLine(" FROM accounting ")
                    .AppendLine(" WHERE ref_id = ?PurchaseId ")
                    ' assign parameter
                    objConn.AddParameter("?PurchaseId", intPurchase_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckAccountByPurchase = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckAccountByPurchase = False
                objLog.ErrorLog("DB_CheckAccountByPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckAccountByPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertIncome
        '	Discription	    : Insert Income
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertIncome( _
            ByVal dtValues As System.Data.DataTable _
        ) As Integer Implements IAccountingDao.InsertIncome
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertIncome = -1
            Try
                ' variable keep row effect
                Dim doSuccess As Boolean = False
                Dim voucher_list As String = String.Empty
                Dim intVoucherNo As Integer
                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' Create new voucher number
                intVoucherNo = CreateNewVoucherNumber()
                If intVoucherNo <= 0 Then Return -1
                ' Set voucher list
                HttpContext.Current.Session("voucherList") = intVoucherNo
                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    If row("accID") = Nothing OrElse row("accID") = String.Empty Then
                        ' check row effect 
                        If InsertIncomePayment(row, intVoucherNo) <= 0 Then
                            doSuccess = False
                            Exit For
                        End If
                    Else
                        ' check row effect 
                        If UpdateIncomePayment(row, intVoucherNo) <= 0 Then
                            doSuccess = False
                            Exit For
                        End If
                    End If
                    dtValues.AcceptChanges()
                    doSuccess = True
                Next
                ' check row effect finally
                If doSuccess = True Then
                    ' case row effect > 0 then commit transaction
                    objConn.CommitTrans()
                    ' set return value with rows count
                    InsertIncome = dtValues.Rows.Count
                Else
                    ' case row effect <= 0 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set session datatable
                HttpContext.Current.Session("dtInquiry") = dtValues
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertIncome(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("InsertIncome(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CreateNewVoucherNumber
        '	Discription	    : Create Accounting Voucher Number
        '	Return Value	: voucher number as integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function CreateNewVoucherNumber() As Integer
            CreateNewVoucherNumber = -1
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                With strSql
                    .AppendLine("	SELECT `last_voucher` 	")
                    .AppendLine("	FROM `voucher_running` 	")
                    .AppendLine("	WHERE (`id` = 1);		")
                    ' execute scalar get last voucher number and set new
                    CreateNewVoucherNumber = objConn.ExecuteScalar(strSql.ToString) + 1

                    .Length = 0
                    .AppendLine("		UPDATE `voucher_running`	        ")
                    .AppendLine("		SET `last_voucher` = " & CreateNewVoucherNumber)
                    .AppendLine("		    ,`updated_by` = " & HttpContext.Current.Session("UserID"))
                    .AppendLine("		    ,`updated_date` = REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','') ")
                    .AppendLine("		WHERE (`id` = 1);			        ")
                    ' execute nonquery with sql command update 
                    objConn.ExecuteNonQuery(strSql.ToString)
                End With
            Catch ex As Exception
                CreateNewVoucherNumber = -1
                ' write error log
                objLog.ErrorLog("CreateNewVoucherNumber(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("CreateNewVoucherNumber(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVoucherNumber
        '	Discription	    : Get Accounting Voucher Number
        '	Return Value	: voucher number as integer
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function GetVoucherNumber(ByVal accID As Integer) As Integer
            GetVoucherNumber = -1
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                With strSql
                    .AppendLine("	SELECT IF(ISNULL(new_voucher_no) OR new_voucher_no = '', voucher_no, new_voucher_no) AS voucher_no  ")
                    .AppendLine("   FROM accounting             ")
                    .AppendLine("   WHERE(id = " & accID & ")   ")
                End With
                ' execute scalar get last voucher number and set new
                GetVoucherNumber = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVoucherNumber(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetVoucherNumber(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: InsertIncomePayment
        '	Discription	    : Insert Income
        '	Return Value	: Nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function InsertIncomePayment(ByRef row As DataRow, ByVal intVoucherNo As Integer) As Integer
            InsertIncomePayment = -1
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                With strSql
                    .AppendLine("	INSERT INTO `accounting` (`type`							")
                    .AppendLine("			,`ref_id`					")
                    .AppendLine("			,`voucher_no`					")
                    .AppendLine("			,`new_voucher_no`					")
                    .AppendLine("			,`account_type`					")
                    .AppendLine("			,`vendor_id`					")
                    .AppendLine("			,`vendor_branch_id`					")
                    .AppendLine("			,`bank`					")
                    .AppendLine("			,`account_name`					")
                    .AppendLine("			,`account_no`					")
                    .AppendLine("			,`account_date`					")
                    .AppendLine("			,`job_order`					")
                    .AppendLine("			,`vat_id`					")
                    .AppendLine("			,`wt_id`					")
                    .AppendLine("			,`ie_id`					")
                    .AppendLine("			,`vat_amount`					")
                    .AppendLine("			,`wt_amount`					")
                    .AppendLine("			,`sub_total`					")
                    .AppendLine("			,`remark`					")
                    .AppendLine("			,`cheque_date`					")
                    .AppendLine("			,`cheque_no`					")
                    .AppendLine("			,`status_id`					")
                    .AppendLine("			,`created_by`					")
                    .AppendLine("			,`created_date`					")
                    .AppendLine("			,`updated_by`					")
                    .AppendLine("			,`updated_date`)					")
                    .AppendLine("	VALUES (?type							")
                    .AppendLine("		 ,?ref_id						")
                    .AppendLine("		 ,?voucher_no						")
                    .AppendLine("		 ,?new_voucher_no						")
                    .AppendLine("		 ,?account_type						")
                    .AppendLine("		 ,?vendor_id						")
                    .AppendLine("		 ,?vendor_branch_id						")
                    .AppendLine("		 ,?bank_name						")
                    .AppendLine("		 ,?account_name						")
                    .AppendLine("		 ,?account_no						")
                    .AppendLine("		 ,?account_date						")
                    .AppendLine("		 ,?job_order						")
                    .AppendLine("		 ,?vat_id						")
                    .AppendLine("		 ,?wt_id						")
                    .AppendLine("		 ,?ie_id						")
                    .AppendLine("		 ,?vat_amount						")
                    .AppendLine("		 ,?wt_amount						")
                    .AppendLine("		 ,?sub_total						")
                    .AppendLine("		 ,?remark						")
                    .AppendLine("		 ,?cheque_date						")
                    .AppendLine("		 ,?cheque_no						")
                    .AppendLine("		 ,?status_id						")
                    .AppendLine("		 ,?created_by						")
                    .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':','')	")
                    .AppendLine("		 ,?updated_by						")
                    .AppendLine("		 ,REPLACE(REPLACE(REPLACE(NOW(),'-',''),' ',''),':',''));	")
                End With

                ' assign parameter
                With objConn
                    .ClearParameter()
                    .AddParameter("?type", Convert.ToInt32(row("Type")))
                    .AddParameter("?ref_id", DBNull.Value)
                    .AddParameter("?voucher_no", intVoucherNo)
                    .AddParameter("?new_voucher_no", DBNull.Value)
                    .AddParameter("?account_type", row("AccountType").ToString.Trim)
                    .AddParameter("?vendor_id", IIf(String.IsNullOrEmpty(row("VendorID").ToString.Trim), DBNull.Value, row("VendorID")))
                    .AddParameter("?vendor_branch_id", IIf(String.IsNullOrEmpty(row("VendorBranchID").ToString.Trim), DBNull.Value, row("VendorBranchID")))
                    .AddParameter("?bank_name", IIf(String.IsNullOrEmpty(row("Bank").ToString.Trim), DBNull.Value, row("Bank").ToString.Trim))
                    .AddParameter("?account_name", IIf(String.IsNullOrEmpty(row("AccountName").ToString.Trim), DBNull.Value, row("AccountName").ToString.Trim))
                    .AddParameter("?account_no", IIf(String.IsNullOrEmpty(row("AccountNo").ToString.Trim), DBNull.Value, row("AccountNo").ToString.Trim))
                    .AddParameter("?account_date", objUtility.String2Date(row("ReceiptDate")).ToString("yyyyMMdd"))
                    .AddParameter("?job_order", IIf(String.IsNullOrEmpty(row("JobOrder").ToString.Trim), DBNull.Value, row("JobOrder").ToString.Trim))
                    .AddParameter("?vat_id", row("VatID"))
                    .AddParameter("?wt_id", row("WTID"))
                    .AddParameter("?ie_id", row("IEID"))
                    .AddParameter("?vat_amount", row("VatAmount"))
                    .AddParameter("?wt_amount", row("WTAmount"))
                    .AddParameter("?sub_total", row("SubTotal"))
                    .AddParameter("?remark", IIf(String.IsNullOrEmpty(row("Remark").ToString.Trim), DBNull.Value, row("Remark").ToString.Trim))
                    .AddParameter("?cheque_date", DBNull.Value)
                    .AddParameter("?cheque_no", row("ChequeNo"))
                    .AddParameter("?status_id", Convert.ToInt32(Enums.RecordStatus.Waiting))
                    .AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                End With

                ' execute nonquery with sql command
                InsertIncomePayment = objConn.ExecuteNonQuery(strSql.ToString)

                ' set VoucherNo to datatable
                row.Item("VoucherNo") = intVoucherNo
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertIncomePayment(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("InsertIncomePayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: UpdateIncomePayment
        '	Discription	    : Update Income
        '	Return Value	: Nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function UpdateIncomePayment(ByRef row As DataRow, ByVal intVoucherNo As Integer) As Integer
            UpdateIncomePayment = -1
            ' variable string sql command
            Dim strSql As New Text.StringBuilder
            Try
                If row("Type") = 1 AndAlso row("StatusID") = Enums.RecordStatus.Deleted Then
                    With strSql
                        .AppendLine("	UPDATE accounting 							            ")
                        .AppendLine("	SET new_voucher_no = ?new_voucher_no                    ")
                        .AppendLine("       , status_id = ?status_id, updated_by = ?updated_by  ")
                        .AppendLine("		, updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')  ")
                        .AppendLine("	WHERE id = ?id		                                    ")
                    End With
                    ' assign parameter
                    With objConn
                        .ClearParameter()
                        .AddParameter("?new_voucher_no", intVoucherNo)
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                        .AddParameter("?status_id", Enums.RecordStatus.Deleted)
                        .AddParameter("?id", row("accID"))
                    End With
                    ' execute nonquery with sql command
                    UpdateIncomePayment = objConn.ExecuteNonQuery(strSql.ToString)
                    ' Insert new data afer update
                    If InsertIncomePayment(row, intVoucherNo) <= 0 Then Return -1
                Else
                    With strSql
                        .AppendLine("	UPDATE accounting 							")
                        .AppendLine("	SET voucher_no = ?voucher_no, account_type = ?account_type, vendor_id = ?vendor_id	")
                        .AppendLine("		, bank = ?bank, account_name = ?account_name, account_no = ?account_no			")
                        .AppendLine("		, account_date = ?account_date, job_order = ?job_order, vat_id = ?vat_id		")
                        .AppendLine("		, wt_id = ?wt_id, ie_id = ?ie_id, vendor_branch_id = ?vendor_branch_id			")
                        .AppendLine("		, cheque_no = ?cheque_no, vat_amount = ?vat_amount, wt_amount = ?wt_amount		")
                        .AppendLine("		, sub_total = ?sub_total, remark = ?remark, status_id=?status_id				")
                        .AppendLine("		, updated_by = ?updated_by, updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')	")
                        .AppendLine("	WHERE id = ?id		")
                    End With

                    ' assign parameter
                    With objConn
                        .ClearParameter()
                        .AddParameter("?voucher_no", intVoucherNo)
                        .AddParameter("?account_type", row("AccountType").ToString.Trim)
                        .AddParameter("?vendor_id", IIf(String.IsNullOrEmpty(row("VendorID").ToString.Trim), DBNull.Value, row("VendorID")))
                        .AddParameter("?bank", IIf(String.IsNullOrEmpty(row("Bank").ToString.Trim), DBNull.Value, row("Bank").ToString.Trim))
                        .AddParameter("?account_name", IIf(String.IsNullOrEmpty(row("AccountName").ToString.Trim), DBNull.Value, row("AccountName").ToString.Trim))
                        .AddParameter("?account_no", IIf(String.IsNullOrEmpty(row("AccountNo").ToString.Trim), DBNull.Value, row("AccountNo").ToString.Trim))
                        .AddParameter("?account_date", objUtility.String2Date(row("ReceiptDate")).ToString("yyyyMMdd"))
                        .AddParameter("?job_order", IIf(String.IsNullOrEmpty(row("JobOrder").ToString.Trim), DBNull.Value, row("JobOrder").ToString.Trim))
                        .AddParameter("?vendor_branch_id", IIf(String.IsNullOrEmpty(row("VendorBranchID").ToString.Trim), DBNull.Value, row("VendorBranchID")))
                        .AddParameter("?cheque_no", row("ChequeNo"))
                        .AddParameter("?vat_id", row("VatID"))
                        .AddParameter("?wt_id", row("WTID"))
                        .AddParameter("?ie_id", row("IEID"))
                        .AddParameter("?vat_amount", row("VatAmount"))
                        .AddParameter("?wt_amount", row("WTAmount"))
                        .AddParameter("?sub_total", row("SubTotal"))
                        .AddParameter("?remark", IIf(String.IsNullOrEmpty(row("Remark").ToString.Trim), DBNull.Value, row("Remark").ToString.Trim))
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                        .AddParameter("?status_id", row("StatusID"))
                        .AddParameter("?id", row("accID"))
                    End With
                    ' execute nonquery with sql command
                    UpdateIncomePayment = objConn.ExecuteNonQuery(strSql.ToString)
                    ' set VoucherNo to datatable
                    row.Item("VoucherNo") = intVoucherNo
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateIncomePayment(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("UpdateIncomePayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountingList
        '	Discription	    : Get Accounting list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountingList( _
            ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_AccountingDetailEntity) Implements IAccountingDao.GetAccountingList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetAccountingList = New List(Of Entity.ImpMst_AccountingDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccountingDetail As Entity.ImpMst_AccountingDetailEntity

                With strSql
                    .AppendLine("		SELECT  								")
                    .AppendLine("			acc.id							")
                    .AppendLine("			, acc.type							")
                    .AppendLine("			, CAST(acc.account_date AS DATE) AS account_date							")
                    .AppendLine("			, acc.account_type							")
                    '.AppendLine("			, acc.voucher_no							")
                    .AppendLine("           ,IF(ISNULL(acc.new_voucher_no) OR acc.new_voucher_no = '', acc.voucher_no, acc.new_voucher_no) as voucher_no ")
                    .AppendLine("			, acc.vendor_id							")
                    .AppendLine("			, vendor.name AS vendor_name							")
                    .AppendLine("			, vendor.type1 AS vendor_type1							")
                    .AppendLine("			, vendor.type2 AS vendor_type2							")
                    .AppendLine("			, vendor.type2_no AS vendor_type2_no							")
                    .AppendLine("			, acc.cheque_no							")
                    .AppendLine("			, acc.ie_id							")
                    .AppendLine("			, TRIM(ie.name) AS ie_title							")
                    .AppendLine("			, (CONCAT(TRIM(ie.code), ' - ', TRIM(ie.name))) AS ie_name							")
                    If objAccountingEnt.accType <> "accounting" Then
                        '.AppendLine("			, job.job_id							")
                        '.AppendLine("			, job.part_no							")
                        .AppendLine("			, job.part_name							")
                    End If

                    .AppendLine("			, acc.job_order							")
                    .AppendLine("			, (CASE WHEN iecat.category_type = 2 THEN acc.sub_total ELSE 0.00 END) AS income							")
                    .AppendLine("			, (CASE WHEN iecat.category_type = 1 THEN acc.sub_total ELSE 0.00 END) AS expense							")
                    .AppendLine("			, IFNULL(vat.percent, 0) AS vat_percentage							")
                    .AppendLine("			, IFNULL(wt.percent, 0) AS wt_percentage							")
                    .AppendLine("			, IFNULL(acc.vat_amount, 0.00) AS vat_amount							")
                    .AppendLine("			, IFNULL(acc.wt_amount, 0.00) AS wt_amount							")
                    .AppendLine("			, acc.bank							")
                    .AppendLine("			, acc.account_name							")
                    .AppendLine("			, acc.account_no							")
                    .AppendLine("			, acc.remark							")
                    .AppendLine("			, acc.status_id							")
                    .AppendLine("			, status.name AS status_text							")
                    .AppendLine("			, acc.ref_id							")
                    .AppendLine("			, NULL AS po_id							")
                    '.AppendLine("			, (CASE WHEN acc.type = 3 THEN acc.voucher_no ELSE NULL END) AS po_no							")
                    .AppendLine("			,p.po_no ")
                    .AppendLine("			, acc.created_date							")
                    .AppendLine("			, acc.updated_date							")
                    .AppendLine("			, COALESCE(acc.updated_date, acc.created_date) 							")
                    .AppendLine("			, acc.sub_total			")
                    .AppendLine("			, CONCAT(vendor.address ,vendor.zipcode) as address			")
                    .AppendLine("			, wt.type as wt_type			")
                    .AppendLine("		FROM accounting acc LEFT JOIN status ON acc.status_id = status.id								")
                    If objAccountingEnt.accType = "accounting" Or objAccountingEnt.accType = "withholding" Then
                        'case account and withholding tax
                        '.AppendLine("				      LEFT JOIN mst_vendor vendor ON acc.vendor_id = vendor.id and vendor.type1 = 0 	")
                        .AppendLine("				      LEFT JOIN mst_vendor vendor ON acc.vendor_id = vendor.id ")
                    Else
                        'case cost table detail
                        .AppendLine("				      LEFT JOIN mst_vendor vendor ON acc.vendor_id = vendor.id  	")
                    End If
                    .AppendLine("				      LEFT JOIN mst_ie ie ON acc.ie_id = ie.id						")
                    .AppendLine("				      LEFT JOIN mst_ie_category iecat ON ie.category_id = iecat.id						")
                    .AppendLine("				      LEFT JOIN mst_vat vat ON acc.vat_id = vat.id						")
                    .AppendLine("				      LEFT JOIN mst_wt wt ON acc.wt_id = wt.id						")
                    If objAccountingEnt.accType <> "accounting" Then
                        .AppendLine("				      LEFT JOIN (						")
                        .AppendLine("							SELECT job.id AS job_id			")
                        .AppendLine("								, job.job_order		")
                        .AppendLine("								, job.job_type_id		")
                        .AppendLine("								, job_type.name AS job_type_name		")
                        .AppendLine("								, job.part_no		")
                        .AppendLine("								, job.part_name		")
                        .AppendLine("								, job.customer AS job_vendor_id		")
                        .AppendLine("								, job.status_id AS job_status_id		")
                        .AppendLine("								, status.name as job_status_text		")
                        .AppendLine("								, job.finish_fg AS is_finished		")
                        .AppendLine("								, (CASE WHEN NOT(ISNULL(job.finish_date)) THEN CAST(job.finish_date AS DATE) ELSE NULL END) AS finish_date		")
                        .AppendLine("								, job.remark AS job_remark		")
                        .AppendLine("							FROM job_order job INNER JOIN status ON job.status_id = status.id			")
                        .AppendLine("									     INNER JOIN mst_job_type job_type ON job.job_type_id = job_type.id	")
                        .AppendLine("							UNION			")
                        .AppendLine("							SELECT 			")
                        .AppendLine("								job_special.id AS job_id		")
                        .AppendLine("								, job_special.job_order		")
                        .AppendLine("								, 0 AS job_type_id		")
                        .AppendLine("								, 'Special' AS job_type_name		")
                        .AppendLine("								, NULL AS part_no		")
                        .AppendLine("								, NULL AS part_name		")
                        .AppendLine("								, NULL as job_vendor_id		")
                        .AppendLine("								, NULL AS job_status_id		")
                        .AppendLine("								, NULL AS job_status_text		")
                        .AppendLine("								, 1 AS is_finished		")
                        .AppendLine("								, NULL AS finish_date		")
                        .AppendLine("								, job_special.remark AS job_remark		")
                        .AppendLine("							FROM job_order_special job_special			")
                        .AppendLine("						 ) AS job ON acc.job_order = job.job_order		      		")
                    End If
                    .AppendLine("							LEFT JOIN ( ")
                    .AppendLine("						SELECT a.id AS accounting_id, a.ref_id, poh.po_no ")
                    .AppendLine("						FROM accounting a  ")
                    .AppendLine("						INNER JOIN payment_header pah ON a.ref_id = pah.id ")
                    .AppendLine("						INNER JOIN po_header poh ON pah.po_header_id = poh.id ")
                    .AppendLine("						WHERE a.type=3) AS p ON p.accounting_id = acc.id ")

                    .AppendLine("		WHERE 1=1					")

                    'If objAccountingEnt.accType = "withholding" Then
                    '    .AppendLine("		and vendor.type1 = 0  ")
                    'End If
                    If objAccountingEnt.accType = "withholding" Then
                        .AppendLine("		 AND  wt.percent > 0   ")
                    End If

                    .AppendLine("		AND (acc.id = IFNULL(?account_id, acc.id))								")
                    .AppendLine("		AND ( (ISNULL(?account_month) OR (ISNULL(?account_year))) 								")
                    .AppendLine("		OR (MONTH(CAST(acc.account_date AS DATE)) = ?account_month 								")
                    .AppendLine("		AND YEAR(CAST(acc.account_date AS DATE)) = ?account_year) )								")
                    .AppendLine("		AND ( (ISNULL(?account_startdate) AND ISNULL(?account_enddate))								")
                    .AppendLine("		      OR ( ((NOT ISNULL(?account_startdate)) AND (NOT ISNULL(?account_enddate))) 								")
                    .AppendLine("		      AND (CAST(acc.account_date AS DATE) BETWEEN CAST(?account_startdate AS DATE) 								")
                    .AppendLine("		      AND CAST(?account_enddate AS DATE)) )      								")
                    .AppendLine("		      OR ( (((NOT ISNULL(?account_startdate)) AND ISNULL(?account_enddate) )) 								")
                    .AppendLine("		      AND CAST(acc.account_date AS DATE) >= CAST(?account_startdate AS DATE))								")
                    .AppendLine("		      OR ( ((ISNULL(?account_startdate) AND (NOT ISNULL(?account_enddate)) )) 								")
                    .AppendLine("		      AND CAST(acc.account_date AS DATE) <= CAST(?account_enddate AS DATE))								")
                    .AppendLine("		    )								")
                    .AppendLine("		AND ( (ISNULL(?joborder_start) AND ISNULL(?joborder_end)) 								")
                    .AppendLine("		      OR ( ((NOT ISNULL(?joborder_start)) AND (NOT ISNULL(?joborder_end))) 								")
                    .AppendLine("		      AND (acc.job_order BETWEEN ?joborder_start AND ?joborder_end) )								")
                    .AppendLine("		      OR ( (((NOT ISNULL(?joborder_start)) AND ISNULL(?joborder_end) )) 								")
                    .AppendLine("		      AND (acc.job_order >= ?joborder_start) )								")
                    .AppendLine("		      OR ( ((ISNULL(?joborder_start) AND (NOT ISNULL(?joborder_end)) )) 								")
                    .AppendLine("		      AND (acc.job_order <= ?joborder_end) )								")
                    .AppendLine("		    )    								")
                    .AppendLine("		AND ( acc.account_type = IFNULL(?account_type, acc.account_type) )								")
                    .AppendLine("		AND (ISNULL(?vendor_name) OR (vendor.name LIKE CONCAT('%', ?vendor_name, '%')))								")
                    .AppendLine("										")
                    .AppendLine("		AND (iecat.category_type = IFNULL(?ie_category_type, iecat.category_type))								")
                    .AppendLine("										")
                    .AppendLine("		AND ( (ISNULL(?po_startno) AND ISNULL(?po_endno)) 								")
                    .AppendLine("		      OR ( ((NOT ISNULL(?po_startno)) AND (NOT ISNULL(?po_endno))) 								")
                    .AppendLine("		      AND (acc.type = 3) AND (acc.voucher_no BETWEEN ?po_startno AND ?po_endno) )								")
                    .AppendLine("		      OR ( (((NOT ISNULL(?po_startno)) AND ISNULL(?po_endno) )) 								")
                    .AppendLine("		      AND (acc.type = 3) AND (acc.voucher_no >= ?po_startno) )								")
                    .AppendLine("		      OR ( ((ISNULL(?po_startno) AND (NOT ISNULL(?po_endno)) )) 								")
                    .AppendLine("		      AND (acc.type = 3) AND (acc.voucher_no <= ?po_endno) )								")
                    .AppendLine("		    )								")
                    .AppendLine("										")
                    .AppendLine("		AND ( (ISNULL(?ie_start_code) AND ISNULL(?ie_end_code)) 								")
                    .AppendLine("										")
                    .AppendLine("		      OR (     ((NOT ISNULL(?ie_start_code)) AND (NOT ISNULL(?ie_end_code))) 								")
                    .AppendLine("		           AND (?ie_start_code REGEXP '^[a-z]') AND (?ie_end_code REGEXP '^[a-z]') 								")
                    .AppendLine("		           AND (LENGTH(?ie_start_code) <= 3 AND LENGTH(?ie_end_code) <= 3) 								")
                    .AppendLine("		           AND (    (ie.code LIKE CONCAT(?ie_start_code, '%') OR ie.code LIKE CONCAT(?ie_end_code, '%')) 								")
                    .AppendLine("				 OR (ie.code BETWEEN ?ie_start_code AND ?ie_end_code)) 						")
                    .AppendLine("			 )							")
                    .AppendLine("										")
                    .AppendLine("		      OR (     ((NOT ISNULL(?ie_start_code)) AND (NOT ISNULL(?ie_end_code))) 								")
                    .AppendLine("		           AND ((?ie_start_code REGEXP '^[a-z]') AND (?ie_end_code REGEXP '^[a-z]'))								")
                    .AppendLine("		           AND (LENGTH(?ie_start_code) > 3 AND LENGTH(?ie_end_code) > 3)								")
                    .AppendLine("		           AND ( (ie.code BETWEEN ?ie_start_code AND ?ie_end_code) ) 								")
                    .AppendLine("			 )							")
                    .AppendLine("		      								")
                    .AppendLine("		      OR (     (((NOT ISNULL(?ie_start_code)) AND ISNULL(?ie_end_code) ))								")
                    .AppendLine("			   AND (?ie_start_code REGEXP '^[a-z]')							")
                    .AppendLine("		           AND (ie.code >= ?ie_start_code) )								")
                    .AppendLine("		      								")
                    .AppendLine("										")
                    .AppendLine("		      OR (     ((ISNULL(?ie_start_code) AND (NOT ISNULL(?ie_end_code)) ))								")
                    .AppendLine("			   AND (?ie_end_code REGEXP '^[a-z]')							")
                    .AppendLine("			   AND (ie.code <= ?ie_end_code) )							")
                    .AppendLine("		    )								")
                    .AppendLine("		AND ( ISNULL(?status_ids) OR (FIND_IN_SET(CAST(acc.status_id AS CHAR), ?status_ids) > 0) )								")
                    .AppendLine("       AND (FIND_IN_SET(CAST(acc.type AS CHAR),'1,3') > 0)	    ")
                    '.AppendLine("		ORDER BY COALESCE(acc.updated_date, acc.created_date) DESC;								")
                    '.AppendLine("		ORDER BY acc.account_type,acc.account_date,acc.vendor_id asc;								")
                    .AppendLine("		ORDER BY  		")
                    If objAccountingEnt.accType = "withholding" Then
                        .AppendLine("		vendor.type2							")
                    Else
                        .AppendLine("		    acc.account_type  		")
                        .AppendLine("		    ,acc.account_date 		")
                        .AppendLine("		    ,acc.vendor_id ,acc.voucher_no	 		")
                    End If
                    .AppendLine("		asc;								")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?account_id", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_id), DBNull.Value, objAccountingEnt.strAccount_id))
                objConn.AddParameter("?account_month", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccountMonth), DBNull.Value, objAccountingEnt.strAccountMonth))
                objConn.AddParameter("?account_year", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccountYear), DBNull.Value, objAccountingEnt.strAccountYear))
                objConn.AddParameter("?account_startdate", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_startdate), DBNull.Value, objAccountingEnt.strAccount_startdate))
                objConn.AddParameter("?account_enddate", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_enddate), DBNull.Value, objAccountingEnt.strAccount_enddate))
                objConn.AddParameter("?joborder_start", IIf(String.IsNullOrEmpty(objAccountingEnt.strJoborder_start), DBNull.Value, objAccountingEnt.strJoborder_start))
                objConn.AddParameter("?joborder_end", IIf(String.IsNullOrEmpty(objAccountingEnt.strJoborder_end), DBNull.Value, objAccountingEnt.strJoborder_end))
                objConn.AddParameter("?account_type", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_type), DBNull.Value, objAccountingEnt.strAccount_type))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objAccountingEnt.strVendor_name), DBNull.Value, objAccountingEnt.strVendor_name))
                objConn.AddParameter("?ie_category_type", IIf(String.IsNullOrEmpty(objAccountingEnt.strIe_category_type), DBNull.Value, objAccountingEnt.strIe_category_type))
                objConn.AddParameter("?po_startno", IIf(String.IsNullOrEmpty(objAccountingEnt.strPo_startno), DBNull.Value, objAccountingEnt.strPo_startno))
                objConn.AddParameter("?po_endno", IIf(String.IsNullOrEmpty(objAccountingEnt.strPo_endno), DBNull.Value, objAccountingEnt.strPo_endno))
                objConn.AddParameter("?ie_start_code", IIf(String.IsNullOrEmpty(objAccountingEnt.strIe_start_code), DBNull.Value, objAccountingEnt.strIe_start_code))
                objConn.AddParameter("?ie_end_code", IIf(String.IsNullOrEmpty(objAccountingEnt.strIe_end_code), DBNull.Value, objAccountingEnt.strIe_end_code))
                objConn.AddParameter("?status_ids", IIf(String.IsNullOrEmpty(objAccountingEnt.strStatus_id), DBNull.Value, objAccountingEnt.strStatus_id))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccountingDetail = New Entity.ImpMst_AccountingDetailEntity
                        ' assign data from db to entity object
                        With objAccountingDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .account_date = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendor_type1 = IIf(IsDBNull(dr.Item("vendor_type1")), Nothing, dr.Item("vendor_type1"))
                            .vendor_type2 = IIf(IsDBNull(dr.Item("vendor_type2")), Nothing, dr.Item("vendor_type2"))
                            .vendor_type2_no = IIf(IsDBNull(dr.Item("vendor_type2_no")), Nothing, dr.Item("vendor_type2_no"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .ie_title = IIf(IsDBNull(dr.Item("ie_title")), Nothing, dr.Item("ie_title"))
                            .Ie_name = IIf(IsDBNull(dr.Item("Ie_name")), Nothing, dr.Item("Ie_name"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .income = IIf(IsDBNull(dr.Item("income")), Nothing, dr.Item("income"))
                            .Expense = IIf(IsDBNull(dr.Item("expense")), Nothing, dr.Item("expense"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_percentage = IIf(IsDBNull(dr.Item("wt_percentage")), Nothing, dr.Item("wt_percentage"))
                            .vat_percentage = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_percentage"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .bank = IIf(IsDBNull(dr.Item("bank")), Nothing, dr.Item("bank"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .status_text = IIf(IsDBNull(dr.Item("status_text")), Nothing, dr.Item("status_text"))
                            .ref_id = IIf(IsDBNull(dr.Item("ref_id")), Nothing, dr.Item("ref_id"))
                            .po_id = IIf(IsDBNull(dr.Item("po_id")), Nothing, dr.Item("po_id"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            If objAccountingEnt.accType <> "accounting" Then
                                .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            End If
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .updated_date = IIf(IsDBNull(dr.Item("updated_date")), Nothing, dr.Item("updated_date"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .wt_type = IIf(IsDBNull(dr.Item("wt_type")), Nothing, dr.Item("wt_type"))
                            .address = IIf(IsDBNull(dr.Item("address")), Nothing, dr.Item("address"))
                        End With
                        ' add Accounting to list
                        GetAccountingList.Add(objAccountingDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountingList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAccountingList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountingOverview
        '	Discription	    : Get Accounting Overview List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCostTableOverviewList( _
            ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_AccountingDetailEntity) Implements IAccountingDao.GetCostTableOverviewList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCostTableOverviewList = New List(Of Entity.ImpMst_AccountingDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccountingDetail As Entity.ImpMst_AccountingDetailEntity

                With strSql
                    .AppendLine("		SELECT 						 ")
                    .AppendLine("			acc.id					 ")
                    .AppendLine("			, acc.type					 ")
                    .AppendLine("			, CAST(acc.account_date AS DATE) AS account_date					 ")
                    .AppendLine("			, acc.account_type					 ")
                    .AppendLine("			, acc.voucher_no					 ")
                    .AppendLine("			, acc.vendor_id					 ")
                    .AppendLine("			, vendor.name AS vendor_name					 ")
                    .AppendLine("			, acc.cheque_no					 ")
                    .AppendLine("			,acc.ie_id					 ")
                    .AppendLine("			, (CONCAT(TRIM(ie.code), ' - ', TRIM(ie.name))) AS ie_name					 ")
                    .AppendLine("			, job.job_id					 ")
                    .AppendLine("			, acc.job_order					 ")
                    .AppendLine("			, (CASE WHEN acc.type IN (2, 4) THEN acc.sub_total ELSE 0.00 END) AS income					 ")
                    .AppendLine("			, (CASE WHEN acc.type IN (1, 3) THEN acc.sub_total ELSE 0.00 END) AS expense					 ")
                    .AppendLine("			,IFNULL(vat.percent, 0) AS vat_percentage					 ")
                    .AppendLine("			, IFNULL(wt.percent, 0) AS wt_percentage					 ")
                    .AppendLine("			,IFNULL(acc.vat_amount, 0.00) AS vat_amount					 ")
                    .AppendLine("			, IFNULL(acc.wt_amount, 0.00) AS wt_amount					 ")
                    .AppendLine("			,acc.bank					 ")
                    .AppendLine("			, acc.account_name					 ")
                    .AppendLine("			, acc.account_no					 ")
                    .AppendLine("			, acc.remark					 ")
                    .AppendLine("			, acc.status_id					 ")
                    .AppendLine("			, status.name AS status_text					 ")
                    .AppendLine("			,acc.ref_id					 ")
                    .AppendLine("			,job.job_type_id					 ")
                    .AppendLine("			, job.job_type_name					 ")
                    .AppendLine("			, job.part_no 	 ")
                    .AppendLine("			,  job.part_name					 ")
                    .AppendLine("			, job.job_vendor_id					 ")
                    .AppendLine("			, job.job_status_id					 ")
                    .AppendLine("			, job.job_status_text					 ")
                    .AppendLine("			, job.is_finished					 ")
                    .AppendLine("			, job.finish_date					 ")
                    .AppendLine("			, job.job_remark					 ")
                    .AppendLine("		FROM accounting acc 						 ")
                    .AppendLine("		LEFT JOIN mst_vendor vendor 						 ")
                    .AppendLine("		ON acc.vendor_id = vendor.id						 ")
                    .AppendLine("		LEFT JOIN mst_ie ie 						 ")
                    .AppendLine("		ON acc.ie_id = ie.id						 ")
                    .AppendLine("		LEFT JOIN mst_vat vat 						 ")
                    .AppendLine("		ON acc.vat_id = vat.id						 ")
                    .AppendLine("		LEFT JOIN mst_wt wt 						 ")
                    .AppendLine("		ON acc.wt_id = wt.id						 ")
                    .AppendLine("		LEFT JOIN status 						 ")
                    .AppendLine("		ON acc.status_id = status.id						 ")
                    .AppendLine("		LEFT JOIN  (						 ")
                    .AppendLine("			SELECT 					 ")
                    .AppendLine("				job.id AS job_id				 ")
                    .AppendLine("				, job.job_order				 ")
                    .AppendLine("				, job.job_type_id				 ")
                    .AppendLine("				, job_type.name AS job_type_name				 ")
                    .AppendLine("				, job.part_no				 ")
                    .AppendLine("				, job.part_name				 ")
                    .AppendLine("				, job.customer AS job_vendor_id				 ")
                    .AppendLine("				, job.status_id AS job_status_id				 ")
                    .AppendLine("				, status.name as job_status_text				 ")
                    .AppendLine("				, job.finish_fg AS is_finished				 ")
                    .AppendLine("				, (CASE WHEN NOT(ISNULL(job.finish_date)) THEN CAST(job.finish_date AS DATE) ELSE NULL END) AS finish_date				 ")
                    .AppendLine("				, job.remark AS job_remark				 ")
                    .AppendLine("			FROM job_order job 					 ")
                    .AppendLine("			INNER JOIN status 					 ")
                    .AppendLine("			ON job.status_id = status.id					 ")
                    .AppendLine("			INNER JOIN mst_job_type job_type 					 ")
                    .AppendLine("			ON job.job_type_id = job_type.id					 ")
                    .AppendLine("			UNION					 ")
                    .AppendLine("			SELECT 					 ")
                    .AppendLine("				job_special.id AS job_id				 ")
                    .AppendLine("				, job_special.job_order				 ")
                    .AppendLine("				, 0 AS job_type_id				 ")
                    .AppendLine("				, NULL AS job_type_name				 ")
                    .AppendLine("				, NULL AS part_no				 ")
                    .AppendLine("				, NULL AS part_name				 ")
                    .AppendLine("				, NULL as job_vendor_id				 ")
                    .AppendLine("				, NULL AS job_status_id				 ")
                    .AppendLine("				, NULL AS job_status_text				 ")
                    .AppendLine("				, 1 AS is_finished				 ")
                    .AppendLine("				, NULL AS finish_date				 ")
                    .AppendLine("				, job_special.remark AS job_remark				 ")
                    .AppendLine("			FROM job_order_special job_special					 ")
                    .AppendLine("			) as job 					 ")
                    .AppendLine("		ON acc.job_order = job.job_order						 ")
                    .AppendLine("		WHERE (acc.id = IFNULL(?account_id, acc.id))						 ")
                    .AppendLine("		AND (acc.account_type = IFNULL(?account_type, acc.account_type))						 ")
                    .AppendLine("		AND ((ISNULL(?joborder_start) AND ISNULL(?joborder_end)) OR (acc.job_order BETWEEN ?joborder_start AND ?joborder_end))						 ")
                    .AppendLine("		AND (acc.status_id = IFNULL(?status_id, acc.status_id));						 ")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?account_id", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_id), DBNull.Value, objAccountingEnt.strAccount_id))
                objConn.AddParameter("?account_type", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccount_type), DBNull.Value, objAccountingEnt.strAccount_type))
                objConn.AddParameter("?joborder_start", IIf(String.IsNullOrEmpty(objAccountingEnt.strJoborder_start), DBNull.Value, objAccountingEnt.strJoborder_start))
                objConn.AddParameter("?joborder_end", IIf(String.IsNullOrEmpty(objAccountingEnt.strJoborder_end), DBNull.Value, objAccountingEnt.strJoborder_end))
                objConn.AddParameter("?status_id", IIf(String.IsNullOrEmpty(objAccountingEnt.strStatus_id), DBNull.Value, objAccountingEnt.strStatus_id))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccountingDetail = New Entity.ImpMst_AccountingDetailEntity
                        ' assign data from db to entity object
                        With objAccountingDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .account_date = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .Ie_name = IIf(IsDBNull(dr.Item("Ie_name")), Nothing, dr.Item("Ie_name"))
                            .job_id = IIf(IsDBNull(dr.Item("job_id")), Nothing, dr.Item("job_id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .income = IIf(IsDBNull(dr.Item("income")), Nothing, dr.Item("income"))
                            .Expense = IIf(IsDBNull(dr.Item("Expense")), Nothing, dr.Item("Expense"))
                            .vat_percentage = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_percentage"))
                            .wt_percentage = IIf(IsDBNull(dr.Item("wt_percentage")), Nothing, dr.Item("wt_percentage"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .bank = IIf(IsDBNull(dr.Item("bank")), Nothing, dr.Item("bank"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .status_text = IIf(IsDBNull(dr.Item("status_text")), Nothing, dr.Item("status_text"))
                            .ref_id = IIf(IsDBNull(dr.Item("ref_id")), Nothing, dr.Item("ref_id"))
                            .job_type_id = IIf(IsDBNull(dr.Item("job_type_id")), Nothing, dr.Item("job_type_id"))
                            .job_type_name = IIf(IsDBNull(dr.Item("job_type_name")), Nothing, dr.Item("job_type_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .job_vendor_id = IIf(IsDBNull(dr.Item("job_vendor_id")), Nothing, dr.Item("job_vendor_id"))
                            .job_status_id = IIf(IsDBNull(dr.Item("job_status_id")), Nothing, dr.Item("job_status_id"))
                            .job_status_text = IIf(IsDBNull(dr.Item("job_status_text")), Nothing, dr.Item("job_status_text"))
                            .is_finished = IIf(IsDBNull(dr.Item("is_finished")), Nothing, dr.Item("is_finished"))
                            .finish_date = IIf(IsDBNull(dr.Item("finish_date")), Nothing, dr.Item("finish_date"))
                            .job_remark = IIf(IsDBNull(dr.Item("job_remark")), Nothing, dr.Item("job_remark"))

                        End With
                        ' add Accounting to list
                        GetCostTableOverviewList.Add(objAccountingDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCostTableOverviewList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCostTableOverviewList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountingOverview
        '	Discription	    : Get Accounting Overview List
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 18-10-2013
        '*************************************************************/
        Public Function GetAdvanceIncomeReport( _
           ByVal year As String _
       ) As System.Collections.Generic.List(Of Entity.ImpMst_AccountingDetailEntity) Implements IAccountingDao.GetAdvanceIncomeReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetAdvanceIncomeReport = New List(Of Entity.ImpMst_AccountingDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccountingDetail As Entity.ImpMst_AccountingDetailEntity

                With strSql
                    .AppendLine("		SELECT j.id, j.job_order, if(ifnull(v.abbr,'')='',v.name,v.abbr) AS customer							")
                    .AppendLine("		, IFNULL(jh.mold_amount,0) AS jh_mold_amount							")
                    .AppendLine("		, IFNULL(jo.other_amount,0) AS jo_other_amount							")
                    .AppendLine("		, IFNULL(jh.mold_amount,0) + IFNULL(jo.other_amount,0) AS jho_total							")
                    .AppendLine("		, ifnull(ji.mold_amount,0) as ji_mold_amount							")
                    .AppendLine("		, ifnull(ji.mold_amount,0) / IFNULL(jh.mold_amount,0) as ratio							")
                    .AppendLine("		, ifnull(ji.other_amount,0) as ji_other_amount							")
                    .AppendLine("		, ji.total as ji_total							")

                    '.AppendLine("		, jp.KTS_MTR_Currency							")
                    '.AppendLine("		, jp.KTS_MTR_THB							")
                    '.AppendLine("		, case when isnull(jp.KTS_INV) then '' else cast(jp.KTS_INV as char) end as KTS_INV							")
                    '.AppendLine("		, jp.KTC_MTR_Currency							")
                    '.AppendLine("		, jp.KTC_MTR_THB							")
                    '.AppendLine("		,case when isnull(jp.KTC_INV) then '' else cast(jp.KTC_INV as char) end as KTC_INV							")
                    '.AppendLine("		, jp.OTHER_MTR							")
                    '.AppendLine("		,jp.KTS_MTR_Currency+jp.KTS_MTR_THB+jp.KTC_MTR_Currency+jp.KTC_MTR_THB+jp.OTHER_MTR TotalMTR							")
                    '.AppendLine("		, jp.hh							")

                    .AppendLine("		, jp3.KTS_MTR_Currency 				")
                    .AppendLine("		, jp3.KTS_MTR_THB					")
                    .AppendLine("		, case when isnull(jp1.KTS_INV) then '' else cast(jp1.KTS_INV as char) end as KTS_INV							")
                    .AppendLine("		, jp3.KTC_MTR_Currency			")
                    .AppendLine("		, jp3.KTC_MTR_THB					")
                    .AppendLine("		,case when isnull(jp2.KTC_INV) then '' else cast(jp2.KTC_INV as char) end as KTC_INV							")
                    .AppendLine("		, jp3.OTHER_MTR							")
                    .AppendLine("		,jp3.KTS_MTR_Currency+jp3.KTS_MTR_THB+jp3.KTC_MTR_Currency+jp3.KTC_MTR_THB+jp3.OTHER_MTR TotalMTR					")
                    .AppendLine("		,jp4.hh			")

                    .AppendLine("		FROM job_order j							")
                    .AppendLine("		LEFT JOIN mst_vendor v ON j.customer = v.id AND v.type1 = 1							")
                    .AppendLine("		LEFT JOIN (							")
                    .AppendLine("		 SELECT j.id AS job_order_id, SUM(IFNULL(p1.po_amount,0)) AS mold_amount							")
                    .AppendLine("		 FROM job_order j							")
                    .AppendLine("		 LEFT JOIN job_order_po p1 ON p1.job_order_id = j.id AND p1.delete_fg <> 1 AND p1.po_type = 0							")
                    .AppendLine("		 WHERE j.status_id <> 6							")
                    .AppendLine("		 GROUP BY p1.job_order_id							")
                    .AppendLine("		) jh ON jh.job_order_id = j.id							")
                    .AppendLine("		LEFT JOIN (							")
                    .AppendLine("		 SELECT j.id AS job_order_id, SUM(IFNULL(p2.po_amount,0)) AS other_amount							")
                    .AppendLine("		 FROM job_order j							")
                    .AppendLine("		 LEFT JOIN job_order_po p2 ON p2.job_order_id = j.id AND p2.delete_fg <> 1 AND p2.po_type <> 0							")
                    .AppendLine("		 WHERE j.status_id <> 6							")
                    .AppendLine("		 GROUP BY p2.job_order_id							")
                    .AppendLine("		) jo ON jo.job_order_id = j.id							")
                    .AppendLine("		LEFT JOIN (							")
                    .AppendLine("		  SELECT m.job_order_id, m.mold_amount, o.other_amount, IFNULL(m.mold_amount,0) + IFNULL(o.other_amount,0) AS total							")
                    .AppendLine("		  FROM (SELECT rd.job_order_id, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) AS mold_amount							")
                    .AppendLine("		   FROM receive_detail rd							")
                    .AppendLine("		   INNER JOIN job_order_po jp ON rd.job_order_id = jp.job_order_id 							")
                    .AppendLine("		   INNER JOIN receive_header rh ON rd.receive_header_id = rh.id   							")
                    .AppendLine("		   WHERE jp.delete_fg <> 1 AND jp.po_type = 0 AND rh.status_id <> 6							")
                    .AppendLine("		   GROUP BY rd.job_order_id) m 							")
                    .AppendLine("		  LEFT OUTER JOIN (SELECT rd.job_order_id, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) AS other_amount							")
                    .AppendLine("		   FROM receive_detail rd							")
                    .AppendLine("		   INNER JOIN job_order_po jp ON rd.job_order_id = jp.job_order_id 							")
                    .AppendLine("		   INNER JOIN receive_header rh ON rd.receive_header_id = rh.id   							")
                    .AppendLine("		   WHERE jp.delete_fg <> 1 AND jp.po_type <> 0 AND rh.status_id <> 6						")
                    .AppendLine("		   GROUP BY rd.job_order_id) o 							")
                    .AppendLine("		  ON m.job_order_id = o.job_order_id							")
                    .AppendLine("		) ji ON ji.job_order_id = j.id 							")
                    .AppendLine("		LEFT JOIN (			")

                    '.AppendLine("		  select C.*,D.hh from (							")
                    '.AppendLine("		    select job_order_id,							")
                    '.AppendLine("		    case concat(code,currency) when 'B0405Currency' then sum(delivery_amount) else 0 end KTS_MTR_Currency,							")
                    '.AppendLine("		    case concat(code,currency) when 'B0405THB' then sum(delivery_amount) else 0 end KTS_MTR_THB,							")
                    '.AppendLine("		    case code when 'B0405' then max(inv) else '' end KTS_INV,							")
                    '.AppendLine("		    case concat(code,currency) when 'B0406Currency' then sum(delivery_amount) else 0 end KTC_MTR_Currency,							")
                    '.AppendLine("		    case concat(code,currency) when 'B0406THB' then sum(delivery_amount) else 0 end KTC_MTR_THB,							")
                    '.AppendLine("		    case code when 'B0406' then max(inv) else '' end KTC_INV,							")
                    '.AppendLine("		    case when code in ('B0401','B0402','B0403','B0404') then sum(delivery_amount) else 0 end OTHER_MTR							")
                    '.AppendLine("		    from (							")
                    '.AppendLine("		      select @inv:=if(@GH=job_order_id,concat(coalesce(concat(@inv,','),''),invoice_no),invoice_no) as inv,@GH:=job_order_id as dummy,							")
                    '.AppendLine("		      job_order_id,ie_id,delivery_amount,invoice_no,currency							")
                    '.AppendLine("		      from (							")
                    '.AppendLine("		        select E.id job_order_id,C.ie_id,B.invoice_no,							")
                    '.AppendLine("		        case upper(F.name) when 'THB' then 'THB' else 'Currency' end currency,							")
                    '.AppendLine("		        sum(A.delivery_amount*ifnull(case upper(F.name) when 'THB' then 1 when 'JPY' then G.rate/100 else G.rate end,1)) delivery_amount							")
                    '.AppendLine("		        from po_detail C							")
                    '.AppendLine("		        join po_header D on C.po_header_id=D.id							")
                    '.AppendLine("		        left join mst_currency F on F.id=D.currency_id							")
                    '.AppendLine("		        join payment_detail A on C.id=A.po_detail_id and C.po_header_id=A.po_header_id							")
                    '.AppendLine("		        join payment_header B on B.id=A.payment_header_id							")

                    ''.AppendLine("		        left join (select currency_id,ef_date,rate from mst_schedule_rate where delete_fg=0 order by ef_date desc limit 1) G on G.currency_id=D.currency_id and G.ef_date<=B.delivery_date							")
                    '.AppendLine("		        left join (select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate from ( ")
                    '.AppendLine("		        select C.currency_id,A.delivery_date,max(B.ef_date) max_ef_date ")
                    '.AppendLine("		        from payment_header A join po_header C on A.po_header_id=C.id ")
                    '.AppendLine("		        left join mst_schedule_rate B on B.currency_id=C.currency_id and B.ef_date<=A.delivery_date ")
                    '.AppendLine("		        where B.delete_fg=0 ")
                    '.AppendLine("		        group by C.currency_id,A.delivery_date ")
                    '.AppendLine("		        ) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date ")
                    '.AppendLine("		        ) G on G.currency_id=D.currency_id and G.delivery_date=B.delivery_date ")
                    '' Ping modify schedule rate sql 20 Aug 2013
                    '.AppendLine("		        join job_order E on E.job_order = C.job_order							")
                    '.AppendLine("		        where B.status_id<>6							")
                    '.AppendLine("		        group by C.job_order,C.ie_id,B.invoice_no,F.name							")
                    '.AppendLine("		      ) A							")
                    '.AppendLine("		    ) B join mst_ie C on B.ie_id=C.id							")
                    '.AppendLine("		    group by job_order_id							")
                    '.AppendLine("		  ) C left join (							")
                    '.AppendLine("		    select A.id job_order_id,sum(left(timediff(concat(end_time,'00'),concat(start_time,'00')),2)+mid(timediff(concat(end_time,'00'),concat(start_time,'00')),4,2)/60) hh							")
                    '.AppendLine("		    from wh_detail join job_order A on wh_detail.job_order=A.job_order							")
                    '.AppendLine("		    join wh_header B on wh_detail.wh_header_id=B.id							")
                    '.AppendLine("		    where year(work_date) = ?year1							")
                    '.AppendLine("		    group by wh_detail.job_order							")
                    '.AppendLine("		  ) D on C.job_order_id=D.job_order_id							")
                    '.AppendLine("		) jp ON jp.job_order_id = j.id 							")

                    .AppendLine("	select C.job_order,group_concat(distinct B.invoice_no) as KTS_INV										 ")
                    .AppendLine("	from po_detail C										 ")
                    .AppendLine("	join po_header D on C.po_header_id=D.id										 ")
                    .AppendLine("	join payment_detail A on C.id=A.po_detail_id and C.po_header_id=A.po_header_id										 ")
                    .AppendLine("	join payment_header B on B.id=A.payment_header_id										 ")
                    .AppendLine("	join mst_ie E on C.ie_id=E.id										 ")
                    .AppendLine("	where B.status_id<>6 and E.code='B0405'										 ")
                    .AppendLine("	group by C.job_order										 ")
                    .AppendLine("	) jp1 ON jp1.job_order = j.job_order										 ")
                    .AppendLine("	LEFT JOIN (										 ")
                    .AppendLine("	select C.job_order,group_concat(distinct B.invoice_no) as KTC_INV										 ")
                    .AppendLine("	from po_detail C										 ")
                    .AppendLine("	join po_header D on C.po_header_id=D.id										 ")
                    .AppendLine("	join payment_detail A on C.id=A.po_detail_id and C.po_header_id=A.po_header_id										 ")
                    .AppendLine("	join payment_header B on B.id=A.payment_header_id										 ")
                    .AppendLine("	join mst_ie E on C.ie_id=E.id										 ")
                    .AppendLine("	where B.status_id<>6 and E.code='B0406'										 ")
                    .AppendLine("	group by C.job_order										 ")
                    .AppendLine("	) jp2 ON jp2.job_order = j.job_order										 ")
                    .AppendLine("	LEFT JOIN (										 ")
                    .AppendLine("	select job_order,										 ")
                    .AppendLine("	sum(case concat(code,currency) when 'B0405Currency' then delivery_amount else 0 end) KTS_MTR_Currency,										 ")
                    .AppendLine("	sum(case concat(code,currency) when 'B0405THB' then delivery_amount else 0 end) KTS_MTR_THB,										 ")
                    .AppendLine("	sum(case concat(code,currency) when 'B0406Currency' then delivery_amount else 0 end) KTC_MTR_Currency,										 ")
                    .AppendLine("	sum(case concat(code,currency) when 'B0406THB' then delivery_amount else 0 end) KTC_MTR_THB,										 ")
                    .AppendLine("	sum(case when code in ('B0401','B0402','B0403','B0404') then delivery_amount else 0 end) OTHER_MTR										 ")
                    .AppendLine("	from (										 ")
                    .AppendLine("		select C.job_order,E.code,									 ")
                    .AppendLine("		case upper(F.name) when 'THB' then 'THB' else 'Currency' end currency,									 ")
                    .AppendLine("		sum(A.delivery_amount*ifnull(case upper(F.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1)) delivery_amount									 ")
                    .AppendLine("		from po_detail C									 ")
                    .AppendLine("		join po_header D on C.po_header_id=D.id									 ")
                    .AppendLine("		left join mst_currency F on F.id=D.currency_id									 ")
                    .AppendLine("		join payment_detail A on C.id=A.po_detail_id and C.po_header_id=A.po_header_id									 ")
                    .AppendLine("		join payment_header B on B.id=A.payment_header_id									 ")
                    .AppendLine("		join mst_ie E on C.ie_id=E.id									 ")
                    .AppendLine("		left join (									 ")
                    .AppendLine("			select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate 								 ")
                    .AppendLine("			from ( 								 ")
                    .AppendLine("				select C.currency_id,A.delivery_date,max(B.ef_date) max_ef_date 							 ")
                    .AppendLine("				from payment_header A join po_header C on A.po_header_id=C.id 							 ")
                    .AppendLine("				left join mst_schedule_rate B on B.currency_id=C.currency_id and B.ef_date<=A.delivery_date 							 ")
                    .AppendLine("				where B.delete_fg=0 							 ")
                    .AppendLine("				group by C.currency_id,A.delivery_date 							 ")
                    .AppendLine("			) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date 								 ")
                    .AppendLine("		) G on G.currency_id=D.currency_id and G.delivery_date=B.delivery_date 									 ")
                    .AppendLine("		where B.status_id<>6 and E.code in ('B0401','B0402','B0403','B0404','B0405','B0406')									 ")
                    .AppendLine("		group by C.job_order,C.ie_id,F.name									 ")
                    .AppendLine("	) A										 ")
                    .AppendLine("	group by job_order										 ")
                    .AppendLine("	) jp3 ON jp3.job_order = j.job_order										 ")
                    .AppendLine("	LEFT JOIN (										 ")
                    .AppendLine("	select A.job_order,sum(left(timediff(concat(end_time,'00'),concat(start_time,'00')),2)+mid(timediff(concat(end_time,'00'),concat(start_time,'00')),4,2)/60) hh										 ")
                    .AppendLine("	from wh_detail C join job_order A on C.job_order=A.job_order										 ")
                    .AppendLine("	join wh_header B on C.wh_header_id=B.id										 ")
                    .AppendLine("	where year(B.work_date) = ?year2										 ")
                    .AppendLine("	group by C.job_order										 ")
                    .AppendLine("	) jp4 ON jp4.job_order = j.job_order										 ")

                    .AppendLine("		WHERE j.status_id <> 6 	")
                    .AppendLine("		and year(j.issue_date)<=?year3  ")
                    .AppendLine("		and IFNULL(jh.mold_amount,0) > IFNULL(ji.mold_amount,0) ")
                    .AppendLine("		ORDER BY j.job_order							")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?year1", IIf(String.IsNullOrEmpty(year), DBNull.Value, year))
                objConn.AddParameter("?year2", IIf(String.IsNullOrEmpty(year), DBNull.Value, year))
                objConn.AddParameter("?year3", IIf(String.IsNullOrEmpty(year), DBNull.Value, year))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccountingDetail = New Entity.ImpMst_AccountingDetailEntity
                        ' assign data from db to entity object
                        With objAccountingDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .customer = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .jh_mold_amount = IIf(IsDBNull(dr.Item("jh_mold_amount")), Nothing, dr.Item("jh_mold_amount"))
                            .jo_other_amount = IIf(IsDBNull(dr.Item("jo_other_amount")), Nothing, dr.Item("jo_other_amount"))
                            .jho_total = IIf(IsDBNull(dr.Item("jho_total")), Nothing, dr.Item("jho_total"))
                            .ji_mold_amount = IIf(IsDBNull(dr.Item("ji_mold_amount")), Nothing, dr.Item("ji_mold_amount"))
                            .ratio = IIf(IsDBNull(dr.Item("ratio")), Nothing, dr.Item("ratio"))
                            .ji_other_amount = IIf(IsDBNull(dr.Item("ji_other_amount")), Nothing, dr.Item("ji_other_amount"))
                            .ji_total = IIf(IsDBNull(dr.Item("ji_total")), Nothing, dr.Item("ji_total"))
                            .KTS_MTR_Currency = IIf(IsDBNull(dr.Item("KTS_MTR_Currency")), Nothing, dr.Item("KTS_MTR_Currency"))
                            .KTS_MTR_THB = IIf(IsDBNull(dr.Item("KTS_MTR_THB")), Nothing, dr.Item("KTS_MTR_THB"))
                            .KTS_INV = IIf(IsDBNull(dr.Item("KTS_INV")), Nothing, dr.Item("KTS_INV"))
                            .KTC_MTR_Currency = IIf(IsDBNull(dr.Item("KTC_MTR_Currency")), Nothing, dr.Item("KTC_MTR_Currency"))
                            .KTC_MTR_THB = IIf(IsDBNull(dr.Item("KTC_MTR_THB")), Nothing, dr.Item("KTC_MTR_THB"))
                            .KTC_INV = IIf(IsDBNull(dr.Item("KTC_INV")), Nothing, dr.Item("KTC_INV"))
                            .OTHER_MTR = IIf(IsDBNull(dr.Item("OTHER_MTR")), Nothing, dr.Item("OTHER_MTR"))
                            .TotalMTR = IIf(IsDBNull(dr.Item("TotalMTR")), Nothing, dr.Item("TotalMTR"))
                            .hh = IIf(IsDBNull(dr.Item("hh")), Nothing, dr.Item("hh"))
                        End With
                        ' add Accounting to list
                        GetAdvanceIncomeReport.Add(objAccountingDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAdvanceIncomeReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAdvanceIncomeReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCostTableOverviewReport
        '	Discription	    : Get Accounting Overview Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCostTableOverviewReport( _
            ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_AccountingDetailEntity) Implements IAccountingDao.GetCostTableOverviewReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCostTableOverviewReport = New List(Of Entity.ImpMst_AccountingDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccountingDetail As Entity.ImpMst_AccountingDetailEntity

                With strSql
                    .AppendLine("		SELECT 						 ")
                    .AppendLine("			DATE_FORMAT(CAST(acc.account_date AS DATE), '%Y') AS account_year					 ")
                    .AppendLine("			, DATE_FORMAT(CAST(acc.account_date AS DATE), '%m') AS account_month					 ")
                    .AppendLine("			, IFNULL(SUBSTRING(ie.code, 1, 1),NULL) AS ie_type					 ")
                    .AppendLine("			, ie.code AS ie_code					 ")
                    .AppendLine("			, ie.name AS ie_desc					 ")
                    .AppendLine("			, SUM(acc.sub_total) AS sub_total 					 ")
                    .AppendLine("		FROM accounting acc 						 ")
                    .AppendLine("		LEFT OUTER JOIN mst_ie ie 						 ")
                    .AppendLine("		ON (acc.ie_id = ie.id) 						 ")
                    .AppendLine("		WHERE ( ISNULL(?status_ids) OR (FIND_IN_SET(CAST(acc.status_id AS CHAR), ?status_ids) > 0) )	 ")
                    .AppendLine("		AND (YEAR(CAST(acc.account_date AS DATE)) = ?account_year)		 ")
                    .AppendLine("		GROUP BY job_order						 ")
                    .AppendLine("			, account_year					 ")
                    .AppendLine("			, account_month					 ")
                    .AppendLine("			, ie_type					 ")
                    .AppendLine("			, ie_code					 ")
                    .AppendLine("			, ie_desc					 ")
                    .AppendLine("		ORDER BY 						 ")
                    .AppendLine("			account_year DESC					 ")
                    .AppendLine("			, ie_type					 ")
                    .AppendLine("			, ie_code					 ")
                    .AppendLine("			, account_month					 ")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?status_ids", IIf(String.IsNullOrEmpty(objAccountingEnt.strStatus_id), DBNull.Value, objAccountingEnt.strStatus_id))
                objConn.AddParameter("?account_year", IIf(String.IsNullOrEmpty(objAccountingEnt.strAccountYear), DBNull.Value, objAccountingEnt.strAccountYear))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccountingDetail = New Entity.ImpMst_AccountingDetailEntity
                        ' assign data from db to entity object
                        With objAccountingDetail
                            .account_year = IIf(IsDBNull(dr.Item("account_year")), Nothing, dr.Item("account_year"))
                            .account_month = IIf(IsDBNull(dr.Item("account_month")), Nothing, dr.Item("account_month"))
                            .ie_type = IIf(IsDBNull(dr.Item("ie_type")), Nothing, dr.Item("ie_type"))
                            .ie_code = IIf(IsDBNull(dr.Item("ie_code")), Nothing, dr.Item("ie_code"))
                            .ie_desc = IIf(IsDBNull(dr.Item("ie_desc")), Nothing, dr.Item("ie_desc"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                        End With
                        ' add Accounting to list
                        GetCostTableOverviewReport.Add(objAccountingDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetCostTableOverviewReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCostTableOverviewReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountJobOrder
        '	Discription	    : Count JobOrder From Job_Order and Job_Order_Special
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountJobOrder( _
            ByVal strJobOrder As String _
        ) As Integer Implements IAccountingDao.CountJobOrder
            ' variable for sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountJobOrder = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT (				")
                    .AppendLine("		SELECT COUNT(job_order) AS CNT_JO				")
                    .AppendLine("		FROM job_order				")
                    .AppendLine("		WHERE job_order = ?job_order				")
                    .AppendLine("       AND status_id <> 6          ")
                    .AppendLine("		)				")
                    .AppendLine("		+				")
                    .AppendLine("		(				")
                    .AppendLine("		SELECT COUNT(job_order) AS CNT_JOS				")
                    .AppendLine("		FROM job_order_special				")
                    .AppendLine("		WHERE job_order = ?job_order				")
                    .AppendLine("       AND delete_fg <> 1          ")
                    .AppendLine("		) AS CNT				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)
                ' execute scalar 
                CountJobOrder = objConn.ExecuteScalar(strSql.ToString)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("CountJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountApproveByVoucherNo
        '	Discription	    : Get data approve from accounting
        '	Return Value	: List(Of Entity.ImpAccountingEntity)
        '	Create User	    : Wasan D.
        '	Create Date	    : 10-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountApproveByVoucherNo( _
        ByVal strVoucherNo As String, Optional ByVal strStatusID As String = Nothing) _
        As System.Collections.Generic.List(Of Entity.ImpAccountingEntity) _
        Implements IAccountingDao.GetAccountApproveByVoucherNo
            GetAccountApproveByVoucherNo = New List(Of Entity.ImpAccountingEntity)
            Dim strSql As New StringBuilder
            Try
                Dim objAccEnt As New Entity.ImpAccountingEntity
                Dim dr As MySqlDataReader
                With strSql
                    .AppendLine("   SELECT CASE account_type WHEN 1 THEN 'Current A/C' WHEN 2 THEN 'Saving A/C' 	    ")
                    .AppendLine("   	WHEN 3 THEN 'Cash' ELSE '' END AS account_type, vd.name AS vendor_name		    ")
                    .AppendLine("   	, bank, CONCAT(account_no, '/', account_name) AS accountname, cheque_no		    ")
                    .AppendLine("   	, account_date, job_order, CONCAT(ie.code, ' - ', ie.name) AS account_title	    ")
                    .AppendLine("   	, sub_total, vat_amount, CONCAT(' (', vat.percent, '%)') AS vat_percent         ")
                    .AppendLine("   	, wt_amount, CONCAT(' (', wt.percent, '%)') AS wt_percent, remark	            ")
                    .AppendLine("   FROM accounting acc																    ")
                    .AppendLine("   	LEFT JOIN mst_vendor vd ON acc.vendor_id=vd.id								    ")
                    .AppendLine("   	LEFT JOIN mst_ie ie ON acc.ie_id=ie.id										    ")
                    .AppendLine("   	LEFT JOIN mst_vat vat ON acc.vat_id=vat.id									    ")
                    .AppendLine("   	LEFT JOIN mst_wt wt ON acc.wt_id=wt.id										    ")
                    .AppendLine("   WHERE voucher_no = ?voucher_no 													    ")
                    .AppendLine("   	AND (acc.new_voucher_no IS null OR acc.new_voucher_no = '')				        ")
                    .AppendLine("   	AND (ISNULL(?status_id) OR acc.status_id <> ?status_id)						    ")
                    .AppendLine("   ORDER BY account_date;															    ")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign sql parameter
                objConn.AddParameter("?voucher_no", IIf(strVoucherNo = "", DBNull.Value, strVoucherNo))
                objConn.AddParameter("?status_id", IIf(strStatusID = Nothing, DBNull.Value, strStatusID))
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                If dr.HasRows Then
                    While dr.Read
                        objAccEnt = New Entity.ImpAccountingEntity
                        With objAccEnt
                            .account_type_text = IIf(IsDBNull(dr("account_type")), String.Empty, dr("account_type"))
                            .vendor_name = IIf(IsDBNull(dr("vendor_name")), String.Empty, dr("vendor_name"))
                            .bank = IIf(IsDBNull(dr("bank")), String.Empty, dr("bank"))
                            .account_name = IIf(IsDBNull(dr("accountname")), String.Empty, dr("accountname"))
                            .cheque_no = IIf(IsDBNull(dr("cheque_no")), String.Empty, dr("cheque_no"))
                            .account_date = IIf(IsDBNull(dr("account_date")), String.Empty, dr("account_date"))
                            .job_order = IIf(IsDBNull(dr("job_order")), String.Empty, dr("job_order"))
                            .ie_title = IIf(IsDBNull(dr("account_title")), String.Empty, dr("account_title"))
                            .sub_total = CDec(dr("sub_total"))
                            .vat_amount_text = CDec(dr("vat_amount")).ToString("#,##0.00") & _
                                               IIf(IsDBNull(dr("vat_percent")), String.Empty, dr("vat_percent"))
                            .wt_amount_text = CDec(dr("wt_amount")).ToString("#,##0.00") & _
                                              IIf(IsDBNull(dr("wt_percent")), String.Empty, dr("wt_percent"))
                            .remark = IIf(IsDBNull(dr("remark")), String.Empty, dr("remark"))
                        End With
                        GetAccountApproveByVoucherNo.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("GetAccountApproveByVoucherNo(dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetAccountApproveByVoucherNo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not IsNothing(objConn) Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetAccountApprove
        '	Discription	    : Get data approve from accounting
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountApprove(ByVal strAccId As String) As System.Collections.Generic.List(Of Entity.IAccountingEntity) Implements IAccountingDao.GetAccountApprove
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetAccountApprove = New List(Of Entity.IAccountingEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.IAccountingEntity

                ' assign sql statement
                With strSql
                    .AppendLine("   SELECT id, voucher_no, status_id 							")
                    .AppendLine("   FROM accounting  											")
                    .AppendLine("   WHERE (FIND_IN_SET(CAST(voucher_no AS CHAR), ?id) > 0)	    ")
                    .AppendLine("   	 AND (new_voucher_no IS null OR new_voucher_no = '')	")
                    .AppendLine("   GROUP BY voucher_no;										")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", strAccId)
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAccountingEntity
                        With objAccEnt
                            ' assign data to object account entity
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                        End With
                        ' add object Vendor entity to list
                        GetAccountApprove.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccountApprove(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetAccountApprove(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetYearList
        '	Discription	    : Get year for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 03-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearList() As System.Collections.Generic.List(Of Entity.IAccountingEntity) Implements IAccountingDao.GetYearList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetYearList = New List(Of Entity.IAccountingEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.IAccountingEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT   ")
                    .AppendLine("		 MIN(account_year) AS min_year 	")
                    .AppendLine("		 ,MAX(account_year) AS latest_year 	")
                    .AppendLine("	FROM ( SELECT YEAR(CAST(`account_date` AS DATE)) AS account_year FROM `accounting` ) AS `accounting`; ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAccountingEntity
                        With objAccEnt
                            ' assign data to object Vendor entity
                            .min_year = IIf(IsDBNull(dr.Item("min_year")), Nothing, dr.Item("min_year"))
                            .latest_year = IIf(IsDBNull(dr.Item("latest_year")), Nothing, dr.Item("latest_year"))
                        End With
                        ' add object Vendor entity to list
                        GetYearList.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetYearList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function
        '/**************************************************************
        '	Function name	: chkCategoryAccountTitle
        '	Discription	    : chk Category AccountTitle
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_AccountingDetailEntity) Implements IAccountingDao.chkCategoryAccountTitle
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            chkCategoryAccountTitle = New List(Of Entity.ImpMst_AccountingDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccountingDetail As Entity.ImpMst_AccountingDetailEntity

                With strSql
                    .AppendLine("		SELECT 						 ")
                    .AppendLine("			ec.category_type					 ")
                    .AppendLine("		FROM mst_ie e 		 ")
                    .AppendLine("		INNER JOIN mst_ie_category ec ON e.category_id = ec.id 		 ")
                    .AppendLine("		WHERE e.delete_fg <> 1 AND e.code = ?ie_code ")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?ie_code", IIf(String.IsNullOrEmpty(ieCode), DBNull.Value, ieCode))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccountingDetail = New Entity.ImpMst_AccountingDetailEntity
                        ' assign data from db to entity object
                        With objAccountingDetail
                            .category_id = IIf(IsDBNull(dr.Item("category_type")), Nothing, dr.Item("category_type"))
                        End With
                        ' add Accounting to list
                        chkCategoryAccountTitle.Add(objAccountingDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("chkCategoryAccountTitle(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("chkCategoryAccountTitle(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: chkExitIEMaster
        '	Discription	    : chk Exit IEMaster
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function chkExitIEMaster( _
            ByVal code As String _
        ) As Integer Implements IAccountingDao.chkExitIEMaster
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            chkExitIEMaster = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(id) AS used_count 				")
                    .AppendLine("		FROM mst_ie				")
                    .AppendLine("		WHERE code = ?code				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?code", code)

                ' execute sql command
                chkExitIEMaster = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("chkExitIEMaster(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("chkExitIEMaster(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("chkExitIEMaster(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAcountApproveList
        '	Discription	    : Get Account Approve List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAcountApproveList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Collections.Generic.List(Of Entity.ImpAccountingEntity) Implements IAccountingDao.GetAcountApproveList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetAcountApproveList = New List(Of Entity.ImpAccountingEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccount As Entity.ImpAccountingEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine("   acc.*, MIN(account_date) as pay_date ")
                    .AppendLine("   ,(CASE acc.type WHEN 1 THEN 'Payment' WHEN 3 THEN 'Payment' WHEN 2 THEN 'Income' WHEN 4 THEN 'Income' END) AS account_type_name ")
                    .AppendLine("   , CASE ")
                    .AppendLine("       WHEN (FIND_IN_SET(CAST(acc.type AS CHAR),'1,3') > 0) and (acc.cheque_no is not null and acc.cheque_no <> '') then 'Yes' ")
                    '.AppendLine("       WHEN (FIND_IN_SET(CAST(acc.type AS CHAR),'2,4') > 0) and  (acc.cheque_no is null or acc.cheque_no = '')  then 'No' ")
                    .AppendLine("       WHEN (FIND_IN_SET(CAST(acc.type AS CHAR),'1,3') > 0) and  (acc.cheque_no is null or acc.cheque_no = '')  then 'No' ")
                    .AppendLine("       ELSE '-' END AS cheque_name ")
                    .AppendLine("   , CONCAT(ie.code ,'-', ie.name) AS item_expense ")
                    '.AppendLine("   , requestor.first_name AS requester_first_name ")
                    '.AppendLine("   , requestor.last_name AS requester_last_name ")
                    .AppendLine("   , status.name AS status_name ")
                    .AppendLine("   , vendor.name AS vendor_name ")
                    .AppendLine("   , vendor.type1 AS vendor_type1 ")
                    .AppendLine("   , vendor.type2 AS vendor_type2 ")
                    '.AppendLine("   , CONCAT(acc.created_by, ' | ', requestor.first_name) AS applied_by ")
                    '.AppendLine("   , CONCAT(user.first_name, ' | ', approver.first_name) AS applied_by ")
                    .AppendLine("   , CONCAT(user.first_name, ' ', user.last_name) AS applied_by ")
                    '.AppendLine("   , approver.first_name AS approver_first_name ")
                    '.AppendLine("   , approver.last_name AS approver_last_name ")
                    .AppendLine("   , wt.percent AS wt_percent ")
                    .AppendLine("   , vat.percent AS vat_percent ")
                    .AppendLine(" FROM accounting acc  ")
                    .AppendLine(" INNER JOIN status ON acc.status_id = status.id ")
                    .AppendLine(" LEFT JOIN mst_ie ie ON acc.ie_id = ie.id ")
                    .AppendLine(" LEFT JOIN mst_ie_category iecat ON ie.category_id = iecat.id ")
                    .AppendLine(" LEFT JOIN mst_vat vat ON acc.vat_id = vat.id ")
                    .AppendLine(" LEFT JOIN mst_wt wt ON acc.wt_id = wt.id ")
                    .AppendLine(" LEFT JOIN mst_vendor vendor ON acc.vendor_id = vendor.id ")
                    '.AppendLine(" LEFT JOIN user requestor ON acc.created_by = requestor.id ")
                    '.AppendLine(" LEFT JOIN user approver ON requestor.account_next_approve = approver.id ")
                    .AppendLine(" LEFT JOIN user ON acc.created_by = user.id ")
                    .AppendLine(" LEFT JOIN user approver ON user.account_next_approve = approver.id ")
                    '.AppendLine(" WHERE (approver.account_next_approve = ?approver_id) ")
                    .AppendLine(" WHERE acc.type <> 3 AND (approver.id = ?approver_id OR acc.created_by = ?approver_id)    ")
                    .AppendLine(" AND (acc.new_voucher_no IS null OR acc.new_voucher_no = '')   ")
                    .AppendLine(" AND (ISNULL(?status_ids) OR (FIND_IN_SET(CAST(acc.status_id AS CHAR), ?status_ids) > 0) ) ")
                    .AppendLine(" AND (ISNULL(?account_type) OR (FIND_IN_SET(CAST(acc.type AS CHAR), ?account_type) > 0) ) ")
                    .AppendLine(" AND (ISNULL(?vendor_name) OR (vendor.name LIKE CONCAT('%', ?vendor_name, '%'))) ")
                    .AppendLine(" AND ( (ISNULL(?joborder_start) AND ISNULL(?joborder_end))  ")
                    .AppendLine("   OR ( ((NOT ISNULL(?joborder_start)) AND (NOT ISNULL(?joborder_end))) AND (acc.job_order BETWEEN ?joborder_start AND ?joborder_end) ) ")
                    .AppendLine("   OR ( (((NOT ISNULL(?joborder_start)) AND ISNULL(?joborder_end) )) AND (acc.job_order >= ?joborder_start) ) ")
                    .AppendLine("   OR ( ((ISNULL(?joborder_start) AND (NOT ISNULL(?joborder_end)) )) AND (acc.job_order <= ?joborder_end) ) ")
                    .AppendLine("   ) ")
                    .AppendLine(" AND ( (ISNULL(?ie_start) AND ISNULL(?ie_end))  ")
                    .AppendLine("   OR ( ((NOT ISNULL(?ie_start)) AND (NOT ISNULL(?ie_end))) AND (ie.code BETWEEN ?ie_start AND ?ie_end) )")
                    .AppendLine("   OR ( (((NOT ISNULL(?ie_start)) AND ISNULL(?ie_end) )) AND (ie.code >= ?ie_start) ) ")
                    .AppendLine("   OR ( ((ISNULL(?ie_start) AND (NOT ISNULL(?ie_end)) )) AND (ie.code <= ?ie_end) ) ")
                    .AppendLine("   )")
                    .AppendLine(" AND ( (ISNULL(?issuedate_start) AND ISNULL(?issuedate_end)) ")
                    .AppendLine("   OR ( ((NOT ISNULL(?issuedate_start)) AND (NOT ISNULL(?issuedate_end))) AND (CAST(acc.account_date AS DATE) BETWEEN CAST(?issuedate_start AS DATE) AND CAST(?issuedate_end AS DATE)) )       ")
                    .AppendLine("   OR ( (((NOT ISNULL(?issuedate_start)) AND ISNULL(?issuedate_end) )) AND CAST(acc.account_date AS DATE) >= CAST(?issuedate_start AS DATE)) ")
                    .AppendLine("   OR ( ((ISNULL(?issuedate_start) AND (NOT ISNULL(?issuedate_end)) )) AND CAST(acc.account_date AS DATE) <= CAST(?issuedate_end AS DATE)) ")
                    .AppendLine("   ) ")
                    .AppendLine("   GROUP BY acc.voucher_no ")
                    If Not String.IsNullOrEmpty(objAccountingDto.strStatus_id) AndAlso objAccountingDto.strStatus_id = 3 Then
                        .AppendLine(" ORDER BY acc.created_date, acc.id; ")
                    Else
                        .AppendLine(" ORDER BY acc.created_date DESC, acc.id; ")
                    End If

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?approver_id", HttpContext.Current.Session("UserID"))
                'objConn.AddParameter("?approver_id", HttpContext.Current.Session("AccountNextApprove"))
                objConn.AddParameter("?status_ids", IIf(String.IsNullOrEmpty(objAccountingDto.strStatus_id), DBNull.Value, objAccountingDto.strStatus_id))
                objConn.AddParameter("?account_type", IIf(String.IsNullOrEmpty(objAccountingDto.strAccount_type), DBNull.Value, objAccountingDto.strAccount_type))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objAccountingDto.strVendor_name), DBNull.Value, objAccountingDto.strVendor_name))
                objConn.AddParameter("?joborder_start", IIf(String.IsNullOrEmpty(objAccountingDto.strJoborder_start), DBNull.Value, objAccountingDto.strJoborder_start))
                objConn.AddParameter("?joborder_end", IIf(String.IsNullOrEmpty(objAccountingDto.strJoborder_end), DBNull.Value, objAccountingDto.strJoborder_end))
                objConn.AddParameter("?issuedate_start", IIf(String.IsNullOrEmpty(objAccountingDto.strAccount_startdate), DBNull.Value, objAccountingDto.strAccount_startdate))
                objConn.AddParameter("?issuedate_end", IIf(String.IsNullOrEmpty(objAccountingDto.strAccount_enddate), DBNull.Value, objAccountingDto.strAccount_enddate))
                objConn.AddParameter("?ie_start", IIf(String.IsNullOrEmpty(objAccountingDto.strIe_start_code), DBNull.Value, objAccountingDto.strIe_start_code))
                objConn.AddParameter("?ie_end", IIf(String.IsNullOrEmpty(objAccountingDto.strIe_end_code), DBNull.Value, objAccountingDto.strIe_end_code))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccount = New Entity.ImpAccountingEntity
                        ' assign data from db to entity object
                        With objAccount
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .accType = IIf(IsDBNull(dr.Item("account_type_name")), Nothing, dr.Item("account_type_name"))
                            If IsDBNull(dr.Item("new_voucher_no")) Or dr.Item("new_voucher_no").ToString = "" Then
                                .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            Else
                                .voucher_no = IIf(IsDBNull(dr.Item("new_voucher_no")), Nothing, dr.Item("new_voucher_no"))
                            End If
                            .strVendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .item_expense = IIf(IsDBNull(dr.Item("item_expense")), Nothing, dr.Item("item_expense"))
                            .approve_status = IIf(IsDBNull(dr.Item("status_name")), Nothing, dr.Item("status_name"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_name")), Nothing, dr.Item("cheque_name"))
                            .applied_by = IIf(IsDBNull(dr.Item("applied_by")), Nothing, dr.Item("applied_by"))
                            .account_date = IIf(IsDBNull(dr.Item("pay_date")), Nothing, dr.Item("pay_date"))
                        End With
                        ' add Account Aprove to list
                        GetAcountApproveList.Add(objAccount)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAcountApproveList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAcountApproveList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateAcountApprove
        '	Discription	    : Update Account Approve
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateAcountApprove( _
            ByVal strAcountId As String, _
            ByVal strAcountType As String, _
            ByVal intStatusId As Integer, _
            Optional ByVal objConn1 As Object = Nothing, _
            Optional ByVal flgTransaction As String = "", _
            Optional ByVal dtValues As System.Data.DataTable = Nothing _
        ) As Integer Implements IAccountingDao.UpdateAcountApprove
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateAcountApprove = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'Keep id and type of update
                Dim arrId() As String = Split(strAcountId, ",")
                Dim arrType() As String = Split(strAcountType, ",")
                Dim strId3 As String = ""
                Dim strType3 As String = ""
                Dim strId4 As String = ""
                Dim strType4 As String = ""
                Dim intStatusPayment As Integer

                'keep data from array to string 
                For i As Integer = 0 To arrType.Count - 1
                    'Modiify 2013/09/05 
                    If arrType(i) = "3" Then 'Or arrType(i) = "1" Then 'Keep data type is 3
                        If strId3 = "" Then
                            strId3 = arrId(i)
                        Else
                            strId3 = strId3 & "," & arrId(i)
                        End If
                    ElseIf arrType(i) = "4" Then 'Or arrType(i) = "2" Then 'Keep data type is 4
                        If strId4 = "" Then
                            strId4 = arrId(i)
                        Else
                            strId4 = strId4 & "," & arrId(i)
                        End If
                    End If
                Next


                If objConn1 Is Nothing Then
                    ' new object connection
                    objConn = New Common.DBConnection.MySQLAccess
                    ' begin transaction
                    objConn.BeginTrans(IsolationLevel.Serializable)
                Else
                    objConn = objConn1
                End If

                'Update status on Payment for type is 3(PO Invoice)
                If strId3 <> "" Then
                    'case status is "Approve" set status to "complete"
                    If intStatusId = 4 Then
                        intStatusPayment = 7
                    Else
                        intStatusPayment = intStatusId
                    End If
                    intEff = UpdPaymentHeader(strId3, intStatusPayment)
                    If intEff <= 0 Then
                        ' case row effect less than 1 then rollback transaction
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If
                'Update status on Receive for type is 4(Job Invoice)
                If strId4 <> "" Then
                    intEff = UpdReceiveHeader(strId4, intStatusId)
                    If intEff <= 0 Then
                        ' case row effect less than 1 then rollback transaction
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If

                If strId3 = "" Or strId4 = "" Then
                    intEff = 1
                End If

                'Update status on accounting
                If intEff > 0 Then
                    intEff = UpdAccounting(strAcountId, intStatusId, dtValues)
                End If
                If intEff <= 0 Then
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Update flag on job_order_po (Update Job_order_po when type = 3,4),
                'Modify 2013/09/17 Start
                ' If intStatusId = 6 Or intStatusId = 5 And (Not dtValues Is Nothing And dtValues.Rows.Count > 0) Then
                If intStatusId = 6 And (Not dtValues Is Nothing And dtValues.Rows.Count > 0) Then
                    If intEff > 0 Then
                        intEff = UpdJobOrderPO(intStatusId, dtValues)
                    End If
                    If intEff <= 0 Then
                        ' case row effect less than 1 then rollback transaction
                        objConn.RollbackTrans()
                        Exit Function
                    End If
                End If


                If flgTransaction = "" Then
                    ' check row effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        objConn.CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        objConn.RollbackTrans()
                    End If
                End If

                ' set value to return variable
                UpdateAcountApprove = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateAcountApprove(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateAcountApprove(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If flgTransaction = "" Then
                    If Not objConn Is Nothing Then objConn.Close()
                End If
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdAccounting
        '	Discription	    : Update status to accounting
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdAccounting( _
            ByVal strAcountId As String, _
            ByVal intStatusId As Integer, _
            Optional ByVal dtValues As System.Data.DataTable = Nothing _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdAccounting = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                If intStatusId = 6 And (Not dtValues Is Nothing And dtValues.Rows.Count > 0) Then
                    ' loop table for insert data
                    For Each row As DataRow In dtValues.Rows
                        ' assign sql command
                        With strSql
                            .Length = 0
                            .AppendLine("	UPDATE accounting acc				")
                            .AppendLine("	    SET acc.status_id = ?status_id	")
                            .AppendLine("		,acc.updated_by = ?user_id		")
                            .AppendLine("		,acc.updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')	")
                            .AppendLine("	WHERE type = 4						")
                            .AppendLine("	AND ref_id = ?ref_id						")
                            .AppendLine("	AND voucher_no = ?voucher_no						")
                        End With

                        With objConn
                            ' assign parameter
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                            .AddParameter("?ref_id", row("ref_id"))
                            .AddParameter("?voucher_no", row("voucher_no"))
                            .AddParameter("?status_id", intStatusId)
                            ' execute sql command and return row effect to intEff variable
                            intEff = .ExecuteNonQuery(strSql.ToString)

                            ' check row effect 
                            If intEff <= 0 Then
                                ' case have error then exit for
                                Exit For
                            End If
                        End With

                    Next

                Else

                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("	UPDATE accounting acc							")
                        .AppendLine("	    SET acc.status_id = ?status_id	 ")
                        .AppendLine("		,acc.updated_by = ?user_id							")
                        .AppendLine("		,acc.updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                        '.AppendLine("	WHERE FIND_IN_SET(CAST(acc.id AS CHAR), ?account_id) > 0;							")
                        .AppendLine("	WHERE FIND_IN_SET(CAST(acc.voucher_no AS CHAR), ?account_id) > 0;							")
                    End With

                    With objConn
                        ' assign parameter
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                        .AddParameter("?account_id", strAcountId)
                        .AddParameter("?status_id", intStatusId)

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With

                End If

                ' assign return value
                UpdAccounting = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdAccounting(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))

            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdJobOrderPO
        '	Discription	    : Update flag to job_order_po
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdJobOrderPO( _
            ByVal intStatusId As Integer, _
           Optional ByVal dtValues As System.Data.DataTable = Nothing _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdJobOrderPO = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' loop table for insert data
                For Each row As DataRow In dtValues.Rows
                    '1)Update flag on job_order_po table on case delete

                    With strSql
                        .Length = 0
                        Select Case row("hontai_type").ToString
                            Case "1"
                                'case Hontai is 1st  
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg1 = 0 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            Case "2"
                                'case Hontai is 2nd  
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg2 = 0 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            Case "3"
                                'case Hontai is 3rd   
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET hontai_fg3 = 0 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                            Case Else
                                'case po type : Sample, Material, Delivery, Others
                                .AppendLine("		Update job_order_po 			")
                                .AppendLine("		SET po_fg = 0 			")
                                .AppendLine("		WHERE (id = ?job_order_po_id);			")
                        End Select
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

                    'Delete 2013/09/18 (Mod by user) Start
                    ''2)Update flag finish goods on case delete and decline
                    'With strSql
                    '    .Length = 0
                    '    .AppendLine("		UPDATE job_order 			")
                    '    .AppendLine("		SET finish_fg = 0 			")
                    '    .AppendLine("		, updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    '    .AppendLine("		, updated_by = ?user_id 			")
                    '    .AppendLine("		WHERE (id = ?job_order_id);")
                    'End With

                    'objConn.ClearParameter()
                    'objConn.AddParameter("?job_order_id", row("job_order_id"))
                    'objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                    '' execute nonquery with sql command (update 
                    'intEff = objConn.ExecuteNonQuery(strSql.ToString)

                    '' check row effect 
                    'If intEff <= 0 Then
                    '    ' case have error then exit for
                    '    Exit For
                    'End If
                    'Delete 2013/09/18 (Mod by user) End
                Next

                ' assign return value
                UpdJobOrderPO = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdJobOrderPO(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))

            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdPaymentHeader
        '	Discription	    : Update status to payment header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdPaymentHeader( _
             ByVal strAcountId As String, _
             ByVal intStatusId As Integer _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdPaymentHeader = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   UPDATE `payment_header` payment ")
                    .AppendLine("   INNER JOIN `accounting` acc ON acc.`ref_id` = payment.`id` ")
                    .AppendLine("   INNER JOIN ( ")
                    .AppendLine("       select ref_id,count(*) A,sum(case when ifnull(cheque_no,'')<>'' then 1 else 0 end) B ")
                    .AppendLine("       from accounting where type=3 and status_id<>6 group by ref_id ")
                    .AppendLine("       ) chk on acc.`ref_id` = chk.`ref_id` ")
                    .AppendLine("   SET payment.`status_id` = case when ?status_id in (4,7) then (case when chk.B=chk.A then ?status_id else 1 end) else ?status_id end ")
                    .AppendLine("		,payment.updated_by = ?user_id							")
                    .AppendLine("		,payment.updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("   WHERE (NOT ISNULL(acc.`ref_id`)) AND (acc.type = 3) ")
                    '.AppendLine("   AND (FIND_IN_SET(CAST(acc.`id` AS CHAR), ?account_id) > 0); ")
                    .AppendLine("   AND (FIND_IN_SET(CAST(acc.voucher_no AS CHAR), ?account_id) > 0); ")
                    .AppendLine("")
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?account_id", strAcountId)
                    .AddParameter("?status_id", intStatusId)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                UpdPaymentHeader = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdPaymentHeader(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdPaymentHeader(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdPaymentHeader(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))

            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdReceiveHeader
        '	Discription	    : Update status to Receive header
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdReceiveHeader( _
             ByVal strAcountId As String, _
             ByVal intStatusId As Integer _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdReceiveHeader = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("	UPDATE receive_header receive							")
                    .AppendLine("	INNER JOIN accounting acc ON acc.ref_id = receive.id	")
                    .AppendLine("	    SET receive.status_id = ?status_id	 ")
                    .AppendLine("		,receive.updated_by = ?user_id							")
                    .AppendLine("		,receive.updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("	WHERE (NOT ISNULL(acc.ref_id)) AND (acc.type IN (4))		")
                    '.AppendLine("	AND (FIND_IN_SET(CAST(acc.id AS CHAR), ?account_id) > 0);	")
                    .AppendLine("	AND (FIND_IN_SET(CAST(acc.voucher_no AS CHAR), ?account_id) > 0);	")
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?account_id", strAcountId)
                    .AddParameter("?status_id", intStatusId)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                UpdReceiveHeader = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdReceiveHeader(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdReceiveHeader(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdReceiveHeader(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetWIPYear
        '	Discription	    : Get year for set dropdownlist
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWIPYear() As System.Collections.Generic.List(Of Entity.IAccountingEntity) Implements IAccountingDao.GetWIPYear
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetWIPYear = New List(Of Entity.IAccountingEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable vendor entity
                Dim objAccEnt As Entity.IAccountingEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT   ")
                    .AppendLine("		 distinct year(issue_date) as year  	")
                    .AppendLine("	from job_order ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new vendor entity
                        objAccEnt = New Entity.ImpAccountingEntity
                        With objAccEnt
                            ' assign data to object Vendor entity
                            .min_year = IIf(IsDBNull(dr.Item("year")), Nothing, dr.Item("year"))
                        End With
                        ' add object Vendor entity to list
                        GetWIPYear.Add(objAccEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWIPYear(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetWIPYear(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetChequeApproveList
        '	Discription	    : Get data account
        '	Return Value	: List of ImpAccountingEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetChequeApproveList(ByVal objAccountingDto As Dto.AccountingDto) As System.Collections.Generic.List(Of Entity.ImpAccountingEntity) Implements IAccountingDao.GetChequeApproveList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetChequeApproveList = New List(Of Entity.ImpAccountingEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccount As Entity.ImpAccountingEntity

                ' assign sql command
                With strSql
                    '2013/09/25 Pranitda S. Start-Mod
                    .AppendLine(" SELECT GROUP_CONCAT(cast(acc.id as char)) as ids ")
                    '2013/09/25 Pranitda S. End-Mod
                    .AppendLine(" , 'Payment' AS account_type_name ")
                    .AppendLine(" , acc.voucher_no ")
                    .AppendLine(" , vendor.name as vendor_name ")
                    .AppendLine(" , acc.status_id ")
                    .AppendLine(" , status.name AS status_name ")
                    .AppendLine(" , CASE ")
                    .AppendLine(" 		WHEN (acc.cheque_no is not null and acc.cheque_no <> '') then 'Yes' ")
                    .AppendLine(" 		ELSE '-' END AS cheque_name ")
                    .AppendLine(" , acc.account_date ")
                    .AppendLine(" , CONCAT(requestor.first_name, ' ', requestor.last_name) AS applied_by ")
                    '2013/09/25 Pranitda S. Start-Add
                    .AppendLine(" , case acc.account_type ")
                    .AppendLine("       when 1 then concat('Cheque : ',acc.cheque_no) ")
                    .AppendLine("       when 2 then concat('Transfer : ',acc.bank) ")
                    .AppendLine("       when 3 then 'Cash' ")
                    .AppendLine("   end as cheque_no ")
                    '2013/09/25 Pranitda S. End-Add
                    .AppendLine(" ,acc.cheque_date ")
                    .AppendLine(" FROM accounting acc ")
                    .AppendLine(" JOIN status ON acc.status_id = status.id ")
                    .AppendLine(" LEFT JOIN user requestor ON acc.created_by = requestor.id ")
                    'ping add
                    .AppendLine(" left join mst_vendor vendor on vendor.id=acc.vendor_id ")
                    .AppendLine(" where acc.type=3 ")
                    If Not String.IsNullOrEmpty(objAccountingDto.strStatus_id) AndAlso objAccountingDto.strStatus_id.Equals("3") Then
                        .AppendLine(" and acc.status_id=3 ")
                    Else
                        .AppendLine(" and acc.status_id>=3 ")
                    End If
                    Dim strSuperUser As String = System.Web.Configuration.WebConfigurationManager.AppSettings("AutoApproveAccount")
                    If strSuperUser.ToUpper.IndexOf(HttpContext.Current.Session("UserName").ToString().ToUpper) = -1 Then
                        .AppendLine(" AND (requestor.account_next_approve = ?approver_id) ")
                    End If
                    .AppendLine(" AND (ISNULL(?vendor_name) OR (vendor.name LIKE CONCAT('%', ?vendor_name, '%'))) ")
                    .AppendLine(" AND ( (ISNULL(?joborder_start) AND ISNULL(?joborder_end))  ")
                    .AppendLine("   OR ( ((NOT ISNULL(?joborder_start)) AND (NOT ISNULL(?joborder_end))) AND (acc.job_order BETWEEN ?joborder_start AND ?joborder_end) ) ")
                    .AppendLine(" OR ( (((NOT ISNULL(?joborder_start)) AND ISNULL(?joborder_end) )) AND (acc.job_order >= ?joborder_start) ) ")
                    .AppendLine("   OR ( ((ISNULL(?joborder_start) AND (NOT ISNULL(?joborder_end)) )) AND (acc.job_order <= ?joborder_end) ) ")
                    .AppendLine("   ) ")
                    .AppendLine(" AND ( (ISNULL(?issuedate_start) AND ISNULL(?issuedate_end)) ")
                    .AppendLine("   OR ( ((NOT ISNULL(?issuedate_start)) AND (NOT ISNULL(?issuedate_end))) AND (CAST(acc.cheque_date AS DATE) BETWEEN CAST(?issuedate_start AS DATE) AND CAST(?issuedate_end AS DATE)) )       ")
                    .AppendLine("   OR ( (((NOT ISNULL(?issuedate_start)) AND ISNULL(?issuedate_end) )) AND CAST(acc.cheque_date AS DATE) >= CAST(?issuedate_start AS DATE)) ")
                    .AppendLine("   OR ( ((ISNULL(?issuedate_start) AND (NOT ISNULL(?issuedate_end)) )) AND CAST(acc.cheque_date AS DATE) <= CAST(?issuedate_end AS DATE)) ")
                    .AppendLine("   ) ")
                    .AppendLine(" GROUP BY acc.voucher_no,acc.vendor_id,acc.status_id,acc.account_type,acc.account_date ")
                    If Not String.IsNullOrEmpty(objAccountingDto.strStatus_id) AndAlso objAccountingDto.strStatus_id.Equals("3") Then
                        .AppendLine(" ORDER BY cast(replace(acc.voucher_no,'P-','') as UNSIGNED) asc;")
                    Else
                        .AppendLine(" ORDER BY cast(replace(acc.voucher_no,'P-','') as UNSIGNED) desc;")
                    End If
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?approver_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objAccountingDto.strVendor_name), DBNull.Value, objAccountingDto.strVendor_name))
                objConn.AddParameter("?joborder_start", IIf(String.IsNullOrEmpty(objAccountingDto.strJoborder_start), DBNull.Value, objAccountingDto.strJoborder_start))
                objConn.AddParameter("?joborder_end", IIf(String.IsNullOrEmpty(objAccountingDto.strJoborder_end), DBNull.Value, objAccountingDto.strJoborder_end))
                objConn.AddParameter("?issuedate_start", IIf(String.IsNullOrEmpty(objAccountingDto.strAccount_startdate), DBNull.Value, objAccountingDto.strAccount_startdate))
                objConn.AddParameter("?issuedate_end", IIf(String.IsNullOrEmpty(objAccountingDto.strAccount_enddate), DBNull.Value, objAccountingDto.strAccount_enddate))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccount = New Entity.ImpAccountingEntity
                        ' assign data from db to entity object
                        With objAccount
                            .newid = IIf(IsDBNull(dr.Item("ids")), Nothing, dr.Item("ids"))
                            .accType = IIf(IsDBNull(dr.Item("account_type_name")), Nothing, dr.Item("account_type_name"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .approve_status = IIf(IsDBNull(dr.Item("status_name")), Nothing, dr.Item("status_name"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_name")), Nothing, dr.Item("cheque_name"))
                            .applied_by = IIf(IsDBNull(dr.Item("applied_by")), Nothing, dr.Item("applied_by"))
                            .account_date = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                        End With
                        ' add Account Aprove to list
                        GetChequeApproveList.Add(objAccount)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetChequeApproveList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetChequeApproveList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateChequeApprove
        '	Discription	    : Update status_id of account
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateChequeApprove(ByVal strNewId As String, ByVal strAcountType As String, ByVal intStatusId As Integer, Optional ByVal objConn1 As Object = Nothing, Optional ByVal flgTransaction As String = "") As Integer Implements IAccountingDao.UpdateChequeApprove
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateChequeApprove = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'Keep id and type of update

                If objConn1 Is Nothing Then
                    ' new object connection
                    objConn = New Common.DBConnection.MySQLAccess
                    ' begin transaction
                    objConn.BeginTrans(IsolationLevel.Serializable)
                Else
                    objConn = objConn1
                End If
                ' assign sql command
                strSql = New Text.StringBuilder
                With strSql
                    .AppendLine("   UPDATE `payment_header` payment ")
                    .AppendLine("   INNER JOIN `accounting` acc ON acc.`ref_id` = payment.`id` ")
                    .AppendLine("   INNER JOIN ( ")
                    '2013/09/25 Pranitda S. Start-Mod
                    '.AppendLine("       select ref_id,count(*) A,sum(case when ifnull(cheque_no,'')<>'' then 1 else 0 end) B ")
                    .AppendLine("       select ref_id,count(*) A,sum(case when ifnull(cheque_date,'')<>'' then 1 else 0 end) B ")
                    '2013/09/25 Pranitda S. End-Mod
                    .AppendLine("       from accounting where type=3 and status_id<>6 group by ref_id ")
                    .AppendLine("       ) chk on acc.`ref_id` = chk.`ref_id` ")
                    .AppendLine("   SET payment.`status_id` = case when ?status_id in (4,7) then (case when chk.B=chk.A then ?status_id else 1 end) else ?status_id end ")
                    .AppendLine("		,payment.updated_by = ?user_id							")
                    .AppendLine("		,payment.updated_date = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')							")
                    If strAcountType.Equals("3") Then ' case normal flow
                        .AppendLine("   WHERE (acc.type = 3) And payment.status_id = 3 ")
                    Else ' case super user
                        .AppendLine("   WHERE (acc.type = 3) And acc.status_id = 3 ")
                    End If
                    '2013/09/25 Pranitda S. Start-Mod
                    .AppendLine("   AND FIND_IN_SET(CAST(acc.id AS CHAR), ?selected_ids) > 0; ")
                    '2013/09/25 Pranitda S. End-Mod
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?selected_ids", strNewId)
                    .AddParameter("?status_id", intStatusId)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)
                    objLog.InfoLog("selected_ids(Dao)", strNewId, HttpContext.Current.Session("UserName"))

                End With

                'Update status on accounting
                If intEff > 0 Then

                    If intStatusId = 6 Then
                        ' assign sql command
                        strSql = New Text.StringBuilder
                        With strSql
                            .AppendLine("   UPDATE `po_header` po ")
                            .AppendLine("   INNER JOIN `payment_header` payment ON payment.`po_header_id` = po.`id` ")
                            .AppendLine("   INNER JOIN `accounting` acc ON acc.`ref_id` = payment.`id` ")
                            .AppendLine("   SET po.status_id=4 ")
                            '2013/09/25 Pranitda S. Start-Mod
                            '.AppendLine("  ,payment.updated_by = ?user_id       ")
                            '.AppendLine("  ,payment.updated_date = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')       ")
                            .AppendLine("  ,po.updated_by = ?user_id       ")
                            .AppendLine("  ,po.updated_date = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')       ")
                            '2013/09/25 Pranitda S. End-Mod
                            .AppendLine("   WHERE acc.type = 3 And payment.status_id = ?status_id ")
                            '2013/09/25 Pranitda S. Start-Mod
                            '.AppendLine("   AND FIND_IN_SET(concat((case when new_voucher_no is null then voucher_no else new_voucher_no end),'|',cheque_no,'|',cheque_date), ?selected_ids) > 0; ")
                            .AppendLine("   AND FIND_IN_SET(CAST(acc.id AS CHAR), ?selected_ids) > 0;")
                            '2013/09/25 Pranitda S. End-Mod
                        End With
                        With objConn
                            ' assign parameter
                            .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                            .AddParameter("?selected_ids", strNewId)
                            .AddParameter("?status_id", intStatusId)
                            ' execute sql command and return row effect to intEff variable
                            intEff = .ExecuteNonQuery(strSql.ToString)

                        End With
                    End If

                    'intEff = UpdAccounting(strNewId, intStatusId)
                    strSql = New Text.StringBuilder
                    With strSql
                        .AppendLine("	UPDATE accounting acc							")
                        .AppendLine("	    SET acc.status_id = ?status_id	 ")
                        .AppendLine("		,acc.updated_by = ?user_id							")
                        .AppendLine("		,acc.updated_date = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')			")
                        '2013/09/25 Pranitda S. Start-Mod
                        '.AppendLine("	WHERE type=3 and status_id=3 and FIND_IN_SET(concat((case when new_voucher_no is null then voucher_no else new_voucher_no end),'|',cheque_no,'|',cheque_date), ?selected_ids) > 0;							")
                        .AppendLine("	WHERE type=3 ")
                        .AppendLine("	and status_id=3 ")
                        .AppendLine("	AND FIND_IN_SET(CAST(acc.id AS CHAR), ?selected_ids) > 0;")
                        '2013/09/25 Pranitda S. End-Mod
                    End With

                    With objConn
                        ' assign parameter
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                        .AddParameter("?selected_ids", strNewId)
                        .AddParameter("?status_id", intStatusId)

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With

                End If

                If flgTransaction = "" Then
                    ' check row effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        objConn.CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        objConn.RollbackTrans()
                    End If
                End If

                ' set value to return variable
                UpdateChequeApprove = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateChequeApprove(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateChequeApprove(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If flgTransaction = "" Then
                    If Not objConn Is Nothing Then objConn.Close()
                End If
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SearchIncomePayment
        '	Discription	    : Search Income and Payment
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 27-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SearchIncomePayment(ByVal intType As Integer, ByVal listPara As List(Of String)) As System.Collections.Generic.List(Of Entity.ImpAccountingEntity) Implements IAccountingDao.SearchIncomePayment
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            SearchIncomePayment = New List(Of Entity.ImpAccountingEntity)
            Try
                Dim dr As MySqlDataReader
                Dim objAccEnt As Entity.ImpAccountingEntity
                With strSql
                    .AppendLine("   SELECT a.id, a.job_order, (CASE a.account_type WHEN 1 THEN 'Current Account'            ")
                    .AppendLine("   	WHEN 2 THEN 'Saving Account' WHEN 3 THEN 'Cash' END) AS account_type                ")
                    .AppendLine("   	, v.name as vendor_name, CONCAT(i.code,' - ',i.name) AS account_title               ")
                    .AppendLine("   	, a.account_name, a.account_no, a.account_date, a.sub_total                         ")
                    .AppendLine("   FROM accounting a                                                                       ")
                    .AppendLine("   LEFT JOIN mst_vendor v ON a.vendor_id = v.id AND v.delete_fg <> 1                       ")
                    .AppendLine("   LEFT JOIN mst_ie i ON i.id = a.ie_id AND i.delete_fg <> 1                               ")
                    .AppendLine("   WHERE (a.type = ?Type or a.type = 3) AND a.status_id IN (1, 3, 5)                       ")
                    .AppendLine("   	AND (ISNULL(?job_order_from) OR a.job_order >= ?job_order_from)                     ")
                    .AppendLine("   	AND (ISNULL(?job_order_to) OR a.job_order <= ?job_order_to)                         ")
                    .AppendLine("   	AND (ISNULL(?account_type) OR a.account_type = ?account_type)                       ")
                    .AppendLine("   	AND (ISNULL(?vendor_name) OR v.name LIKE CONCAT('%', ?vendor_name, '%'))            ")
                    .AppendLine("   	AND (ISNULL(?account_title) OR a.ie_id = ?account_title)                            ")
                    .AppendLine("   	AND (ISNULL(?account_name) OR a.account_name LIKE CONCAT('%' ,?account_name, '%'))  ")
                    .AppendLine("   	AND (ISNULL(?account_no) OR a.account_no LIKE CONCAT('%', ?account_no, '%'))        ")
                    .AppendLine("   	AND (ISNULL(?account_date_from) OR a.account_date >= ?account_date_from)            ")
                    .AppendLine("   	AND (ISNULL(?account_date_to) OR a.account_date <= ?account_date_to)	            ")
                    .AppendLine("   	ORDER BY a.job_order 	                                                           ")

                    objConn = New Common.DBConnection.MySQLAccess
                    Dim sqlParameter() As String = {"?job_order_from", "?job_order_to", "?account_type", "?vendor_name", "?account_title", "?account_name", "?account_no", "?account_date_from", "?account_date_to"}
                    objConn.AddParameter("?Type", intType)
                    For i = 0 To sqlParameter.Length - 1
                        objConn.AddParameter(sqlParameter(i), IIf(listPara(i) <> "", listPara(i), Nothing))
                    Next
                    dr = objConn.ExecuteReader(strSql.ToString)
                    If dr.HasRows Then
                        While dr.Read
                            objAccEnt = New Entity.ImpAccountingEntity
                            With objAccEnt
                                .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                                .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                                .accType = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                                .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                                .ie_title = IIf(IsDBNull(dr.Item("account_title")), Nothing, dr.Item("account_title"))
                                .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                                .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                                .account_date = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                                .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            End With
                            SearchIncomePayment.Add(objAccEnt)
                        End While
                    End If
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SearchIncomePayment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("SearchIncomePayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPoForDeleteList
        '	Discription	    : Get Job order po for delete
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPoForDeleteList( _
            ByVal strAcountId As String _
        ) As System.Collections.Generic.List(Of Entity.ImpAccountingEntity) Implements IAccountingDao.GetPoForDeleteList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPoForDeleteList = New List(Of Entity.ImpAccountingEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objAccount As Entity.ImpAccountingEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT   ")
                    .AppendLine(" 	rd.job_order_id   ")
                    .AppendLine("  	, rd.job_order_po_id  ")
                    .AppendLine("   , rd.hontai_type ")
                    .AppendLine("   , a.id ")
                    .AppendLine("   , a.type ")
                    .AppendLine("   , a.ref_id ")
                    .AppendLine("   , a.voucher_no ")
                    .AppendLine(" FROM accounting a   ")
                    .AppendLine(" INNER JOIN receive_header rh ON rh.id = a.ref_id    ")
                    .AppendLine(" INNER JOIN receive_detail rd ON rd.receive_header_id = rh.id   ")
                    .AppendLine(" WHERE rh.status_id <> 6    ")
                    '.AppendLine(" AND FIND_IN_SET(CAST(a.id AS CHAR), ?id) > 0;	   ")
                    .AppendLine(" AND FIND_IN_SET(CAST(a.voucher_no AS CHAR), ?id) > 0;	   ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", strAcountId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objAccount = New Entity.ImpAccountingEntity
                        ' assign data from db to entity object
                        With objAccount
                            .job_order_id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .job_order_po_id = IIf(IsDBNull(dr.Item("job_order_po_id")), Nothing, dr.Item("job_order_po_id"))
                            .hontai_type = IIf(IsDBNull(dr.Item("hontai_type")), Nothing, dr.Item("hontai_type"))
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .ref_id = IIf(IsDBNull(dr.Item("ref_id")), Nothing, dr.Item("ref_id"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))

                        End With
                        ' add Account Aprove to list
                        GetPoForDeleteList.Add(objAccount)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPoForDeleteList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPoForDeleteList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccountingWithID
        '	Discription	    : Get data from Income and Payment 
        '	Return Value	: List
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountingWithID(ByVal intAccID As Integer) As System.Data.DataTable Implements IAccountingDao.GetAccountingWithID
            ' Set default return value
            GetAccountingWithID = New DataTable
            ' variable for keep sql command
            Dim strSql As New StringBuilder
            Try
                Dim dr As MySqlDataReader
                Dim row As DataRow
                Dim dtColumn() As String = {"account_type", "job_order", "vendor_id", "vat_id", "wt_id", _
                                            "ie_id", "bank", "account_name", "account_no", "cheque_no", _
                                            "sub_total", "account_date", "remark", "status_id", "vendor_branch_id"}
                ' Set Sql command
                With strSql
                    .AppendLine("   SELECT account_type, job_order, vendor_id, vat_id, bank, wt_id  ")
                    .AppendLine("       , account_name, ie_id, account_no, cheque_no, sub_total     ")
                    .AppendLine("       , account_date, remark, status_id, vendor_branch_id         ")
                    .AppendLine("   FROM accounting     ")
                    .AppendLine("   WHERE id = ?accID   ")
                End With
                ' Connect database
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter for sql command
                objConn.AddParameter("?accID", intAccID)
                ' return value
                dr = objConn.ExecuteReader(strSql.ToString)
                If dr.HasRows Then
                    For Each strColTmp As String In dtColumn
                        GetAccountingWithID.Columns.Add(strColTmp)
                    Next
                    While dr.Read
                        row = GetAccountingWithID.NewRow
                        row("account_type") = IIf(IsDBNull(dr.Item("account_type")), String.Empty, dr.Item("account_type"))
                        row("job_order") = IIf(IsDBNull(dr.Item("job_order")), String.Empty, dr.Item("job_order"))
                        row("vendor_id") = IIf(IsDBNull(dr.Item("vendor_id")), String.Empty, dr.Item("vendor_id"))
                        row("vat_id") = IIf(IsDBNull(dr.Item("vat_id")), String.Empty, dr.Item("vat_id"))
                        row("wt_id") = IIf(IsDBNull(dr.Item("wt_id")), String.Empty, dr.Item("wt_id"))
                        row("ie_id") = IIf(IsDBNull(dr.Item("ie_id")), String.Empty, dr.Item("ie_id"))
                        row("bank") = IIf(IsDBNull(dr.Item("bank")), String.Empty, dr.Item("bank"))
                        row("account_name") = IIf(IsDBNull(dr.Item("account_name")), String.Empty, dr.Item("account_name"))
                        row("account_no") = IIf(IsDBNull(dr.Item("account_no")), String.Empty, dr.Item("account_no"))
                        row("cheque_no") = IIf(IsDBNull(dr.Item("cheque_no")), String.Empty, dr.Item("cheque_no"))
                        row("sub_total") = IIf(IsDBNull(dr.Item("sub_total")), String.Empty, dr.Item("sub_total"))
                        row("account_date") = IIf(IsDBNull(dr.Item("account_date")), String.Empty, dr.Item("account_date"))
                        row("remark") = IIf(IsDBNull(dr.Item("remark")), String.Empty, dr.Item("remark"))
                        row("status_id") = IIf(IsDBNull(dr.Item("status_id")), String.Empty, dr.Item("status_id"))
                        row("vendor_branch_id") = IIf(IsDBNull(dr.Item("vendor_branch_id")) _
                                                      OrElse dr.Item("vendor_branch_id").ToString = "", 0, dr.Item("vendor_branch_id"))
                        GetAccountingWithID.Rows.Add(row)
                    End While
                End If
            Catch ex As Exception
                ' Write error log 
                objLog.ErrorLog("GetAccountingWithID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAccountingWithID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetListAccountTitle
        '	Discription	    : Get data from Income and Payment 
        '	Return Value	: List
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 28-05-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function GetListAccountTitle(ByVal UserId As Integer) _
            As List(Of Entity.ImpAccountingEntity) _
            Implements IAccountingDao.GetListAccountTitle
            GetListAccountTitle = New List(Of Entity.ImpAccountingEntity)
            Dim strSql As New StringBuilder
            Try
                Dim dr As MySqlDataReader
                Dim row As DataRow
                Dim objIe As Entity.ImpAccountingEntity

                ' Set Sql command
                With strSql
                    .AppendLine("   SELECT id,name  ")
                    .AppendLine("       FROM mst_ie_category     ")
                    .AppendLine("   WHERE delete_fg <> 6")
                End With
                ' Connect database
                objConn = New Common.DBConnection.MySQLAccess
                ' Assign parameter for sql command
                'objConn.AddParameter("?", UserId)
                ' return value
                dr = objConn.ExecuteReader(strSql.ToString)

                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objIe = New Entity.ImpAccountingEntity
                        ' assign data from db to entity object
                        With objIe
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .account_name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                        End With

                        GetListAccountTitle.Add(objIe)
                    End While
                End If

            Catch ex As Exception
                ' Write error log 
                objLog.ErrorLog("GetListAccountTitle(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetListAccountTitle(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function
    End Class
End Namespace

