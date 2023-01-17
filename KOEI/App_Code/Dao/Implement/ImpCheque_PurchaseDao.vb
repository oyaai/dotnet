#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpCheque_Purchase
'	Class Discription	: Execute process Rating Purchase
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
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
    Public Class ImpCheque_PurchaseDao
        Implements ICheque_PurchaseDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private strMsgErr As String = String.Empty
        Private IAccountingService As New Dao.ImpAccountingDao
        Private CommonValidation As New Validations.CommonValidation
        '/**************************************************************
        '	Function name	: GetCheque_PurchaseList
        '	Discription	    : Get Rating Purchase list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_PurchaseList( _
            ByVal objChequeEnt As Entity.ICheque_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetCheque_PurchaseList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCheque_PurchaseList = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("Select GROUP_CONCAT(cast(acc.id as char(50))) ids")
                    .AppendLine(",acc.voucher_no")
                    .AppendLine(",case acc.account_type when 1 then concat('Cheque :',acc.cheque_no) when 2 then concat('Transfer :',acc.bank) when 3 then 'Cash' end as cheque_no")
                    .AppendLine(",ven.name as vendor_name")
                    .AppendLine(",case ven.type2 when 0 then 'Person' when 1 then 'Company' else '' end vendor_type")
                    .AppendLine(",CAST(acc.cheque_date AS DATE) AS cheque_date")
                    .AppendLine("from accounting acc")
                    .AppendLine("join mst_vendor ven on acc.vendor_id = ven.id")
                    .AppendLine("where type in (1,3) and status_id not in (5,6)")
                    .AppendLine("and ifnull(cheque_date,'') <> ''")

                    'cheque_no
                    .AppendLine("AND (ifnull(acc.cheque_no,'') = ?cheque_no or ifnull(acc.bank,'') = ?cheque_no)	")

                    'cheque date
                    .AppendLine("AND ( (ISNULL(?cheque_start_date) AND ISNULL(?cheque_end_date))")
                    .AppendLine("   OR ( ((NOT ISNULL(?cheque_start_date)) AND (NOT ISNULL(?cheque_end_date))) AND (CAST(acc.cheque_date AS DATE) BETWEEN CAST(?cheque_start_date AS DATE) AND CAST(?cheque_end_date AS DATE)) )")
                    .AppendLine("   OR ( (((NOT ISNULL(?cheque_start_date)) AND ISNULL(?cheque_end_date) )) AND CAST(acc.cheque_date AS DATE) >= CAST(?cheque_start_date AS DATE))")
                    .AppendLine("   OR ( ((ISNULL(?cheque_start_date) AND (NOT ISNULL(?cheque_end_date)) )) AND CAST(acc.cheque_date AS DATE) <= CAST(?cheque_end_date AS DATE))")
                    .AppendLine(")")

                    'vendor name
                    .AppendLine("AND (ISNULL(?vendor_name) OR (ven.name LIKE CONCAT('%', ?vendor_name, '%')))")

                    'vendor type
                    .AppendLine("AND (ven.type2 = IFNULL(?vendor_type, ven.type2))")
                    .AppendLine("group by acc.voucher_no,acc.account_type,acc.vendor_id,acc.cheque_date")
                    .AppendLine("order by cast(replace(acc.voucher_no,'P-','') as UNSIGNED) desc, acc.cheque_date desc")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?cheque_no", IIf(String.IsNullOrEmpty(objChequeEnt.strChequeNo), "", objChequeEnt.strChequeNo))
                objConn.AddParameter("?cheque_start_date", IIf(String.IsNullOrEmpty(objChequeEnt.strChequeDateFrom), DBNull.Value, objChequeEnt.strChequeDateFrom))
                objConn.AddParameter("?cheque_end_date", IIf(String.IsNullOrEmpty(objChequeEnt.strChequeDateTo), DBNull.Value, objChequeEnt.strChequeDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objChequeEnt.strVendor_name), DBNull.Value, objChequeEnt.strVendor_name))
                objConn.AddParameter("?vendor_type", IIf(String.IsNullOrEmpty(objChequeEnt.strSearchType), DBNull.Value, objChequeEnt.strSearchType))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("ids")), Nothing, dr.Item("ids"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendor_type = IIf(IsDBNull(dr.Item("vendor_type")), Nothing, dr.Item("vendor_type"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                        End With
                        ' add Accounting to list
                        GetCheque_PurchaseList.Add(objInvPurchaseDetail)
                    End While
                End If

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetCheque_PurchaseList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCheque_PurchaseList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCheque_Head
        '	Discription	    : Get Cheque Head
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_Head( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetCheque_Head
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCheque_Head = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("select distinct ven.name as vendor_name")
                    .AppendLine(",case ven.type2 when 0 then 'Person' when 1 then 'Company' else '' end vendor_type")
                    .AppendLine(",case acc.account_type when 1 then concat('Cheque : ',acc.cheque_no) when 2 then concat('Transfer : ',acc.bank) when 3 then 'Cash' end as cheque_no")
                    .AppendLine(",CAST(cheque_date AS DATE) AS cheque_date")
                    .AppendLine("from accounting acc")
                    .AppendLine("left join mst_vendor ven on acc.vendor_id=ven.id")
                    .AppendLine("where acc.type in (1,3)")
                    .AppendLine("and FIND_IN_SET(CAST(acc.id AS CHAR), ?ids) > 0;")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?ids", strChequeNo)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendor_type = IIf(IsDBNull(dr.Item("vendor_type")), Nothing, dr.Item("vendor_type"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                        End With
                        ' add Accounting to list
                        GetCheque_Head.Add(objInvPurchaseDetail)
                    End While
                End If

                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetCheque_Head(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCheque_Head(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCheque_Detail
        '	Discription	    : Get Cheque Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 12-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetCheque_Detail( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetCheque_Detail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetCheque_Detail = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("select acc.voucher_no as voucher_no,concat(it.name,' : ',ie.name) as ie_name,acc.job_order")
                    .AppendLine(",acc.sub_total,concat(vat.percent,'%')  as vat_name,acc.vat_amount,concat(wt.percent,'%') as wt_name,acc.wt_amount")
                    .AppendLine(",case when acc.cheque_date is null then CAST(pay_head.payment_date AS DATE) else CAST(acc.cheque_date AS DATE) end AS payment_date,'Purchase' as payment_from")
                    .AppendLine("from accounting acc")
                    .AppendLine("join payment_header pay_head on acc.ref_id=pay_head.id")
                    .AppendLine("left join mst_ie ie on acc.ie_id=ie.id")
                    .AppendLine("left join mst_vat vat on acc.vat_id=vat.id")
                    .AppendLine("left join mst_wt wt on acc.wt_id=wt.id")
                    .AppendLine("left join mst_item it on it.id=acc.item_id")
                    .AppendLine("where acc.type in (1,3)")
                    .AppendLine("and FIND_IN_SET(CAST(acc.id AS CHAR), ?ids) > 0;")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?ids", strChequeNo)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .ie_name = IIf(IsDBNull(dr.Item("ie_name")), Nothing, dr.Item("ie_name"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))

                            .vat_name = IIf(IsDBNull(dr.Item("vat_name")), Nothing, dr.Item("vat_name"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_name = IIf(IsDBNull(dr.Item("wt_name")), Nothing, dr.Item("wt_name"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .payment_from = IIf(IsDBNull(dr.Item("payment_from")), Nothing, dr.Item("payment_from"))
                        End With
                        ' add Accounting to list
                        GetCheque_Detail.Add(objInvPurchaseDetail)
                    End While
                End If

                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetCheque_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetCheque_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccounting_Header
        '	Discription	    : Get Accounting
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 18-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccounting_Detail( _
            ByVal id As String, _
            ByVal dtDate As String, _
            ByVal mode As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetAccounting_Detail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetAccounting_Detail = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("select GROUP_CONCAT(cast(acc.id as char(50))) ids      ")
                    .AppendLine("   ,'False' as chkCheque,ven.name as vendor_name   ")
                    .AppendLine("   ,case ven.type2 when 0 then 'Person' when 1 then 'Company' else ''end vendor_type,acc.cheque_no,CAST(acc.cheque_date AS DATE) AS cheque_date")
                    .AppendLine("   ,acc.account_type,bank,acc.account_no,acc.account_name  ")
                    .AppendLine("   ,voucher_no,GROUP_CONCAT(DISTINCT pay_head.invoice_no order by pay_head.invoice_no) invoice_no,po_head.currency_id  ")
                    .AppendLine("   ,sum(pay_del.delivery_amount) hsub_total,sum(acc.sub_total) sub_total, sum(acc.sub_total) as amount_bank    ")
                    .AppendLine("   ,sum(pay_del.delivery_amount*vat.percent/100) as hvat_amount, vat.percent as vat_percent,concat(vat.percent,'%') as vat_name, sum(acc.vat_amount) as vat_amount")
                    .AppendLine("   ,sum(pay_del.delivery_amount*wt.percent/100) as hwt_amount, wt.percent as wt_percent,concat(wt.percent,'%') as wt_name, sum(acc.wt_amount) as wt_amount")
                    .AppendLine("   ,CAST(pay_head.payment_date AS DATE) AS payment_date    ")
                    .AppendLine("   ,case acc.type when 1 then 'Payment' when 3 then 'Purchase' else '' end as payment_from ")
                    .AppendLine("from accounting acc    ")
                    .AppendLine("   join payment_header pay_head on acc.ref_id=pay_head.id  ")
                    .AppendLine("   join payment_detail pay_del on pay_del.payment_header_id = pay_head.id  ")
                    .AppendLine("   join po_detail po_det on pay_del.po_header_id=po_det.po_header_id and pay_del.po_detail_id=po_det.id and po_det.job_order=acc.job_order and po_det.ie_id=acc.ie_id and po_det.item_id=acc.item_id ")
                    .AppendLine("   join po_header po_head on po_det.po_header_id=po_head.id    ")
                    .AppendLine("   left join mst_vendor ven on acc.vendor_id = ven.id          ")
                    .AppendLine("   left join mst_vat vat on acc.vat_id=vat.id                  ")
                    .AppendLine("   left join mst_wt wt on acc.wt_id=wt.id                      ")
                    .AppendLine("where acc.type in (1,3)                                        ")
                    .AppendLine("   and acc.status_id<>6 and pay_head.status_id<>6              ")
                    If mode = "Add" Then 'Add
                        .AppendLine("and acc.status_id=1")
                        .AppendLine("and ifnull(acc.cheque_date,'')=''")
                        'vendor id
                        .AppendLine("AND (acc.vendor_id = IFNULL(?id, acc.vendor_id))")
                        'payment month
                        '.AppendLine("AND ( IFNULL(?dtDate,'')='' or (year(pay_head.payment_date)=year(?dtDate) and month(pay_head.payment_date)=month(?dtDate)) )	")
                        If dtDate.Length > 2 Then
                            .AppendFormat("AND left(pay_head.payment_date,{0})=?dtDate	", dtDate.Length)
                        ElseIf dtDate.Length = 2 Then
                            .AppendLine("AND substr(pay_head.payment_date,5,2)=?dtDate	")
                        End If
                    Else 'Modify
                        'id
                        .AppendLine("AND (FIND_IN_SET(CAST(acc.id AS CHAR), ?id) > 0)")
                        'payment month
                        '.AppendLine("AND (acc.cheque_date = IFNULL(?dtDate, acc.cheque_date))")
                    End If
                    .AppendLine("group by voucher_no,acc.vendor_id,po_head.currency_id,vat.percent,wt.percent,pay_head.payment_date,acc.type")
                    .AppendLine("order by acc.voucher_no")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(id), DBNull.Value, id))
                objConn.AddParameter("?dtDate", IIf(String.IsNullOrEmpty(dtDate), DBNull.Value, dtDate))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("ids")), Nothing, dr.Item("ids"))
                            .chkCheque = IIf(IsDBNull(dr.Item("chkCheque")), Nothing, dr.Item("chkCheque"))
                            '********************
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .vendor_type = IIf(IsDBNull(dr.Item("vendor_type")), Nothing, dr.Item("vendor_type"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .bank = IIf(IsDBNull(dr.Item("bank")), Nothing, dr.Item("bank"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                            .vat_percent = IIf(IsDBNull(dr.Item("vat_percent")), Nothing, dr.Item("vat_percent"))
                            .wt_percent = IIf(IsDBNull(dr.Item("wt_percent")), Nothing, dr.Item("wt_percent"))
                            '********************
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .amount_bank = IIf(IsDBNull(dr.Item("amount_bank")), Nothing, dr.Item("amount_bank"))
                            .vat_name = IIf(IsDBNull(dr.Item("vat_name")), Nothing, dr.Item("vat_name"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_name = IIf(IsDBNull(dr.Item("wt_name")), Nothing, dr.Item("wt_name"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .payment_from = IIf(IsDBNull(dr.Item("payment_from")), Nothing, dr.Item("payment_from"))
                            .hsub_total = IIf(IsDBNull(dr.Item("hsub_total")), Nothing, dr.Item("hsub_total"))
                            .hvat_amount = IIf(IsDBNull(dr.Item("hvat_amount")), Nothing, dr.Item("hvat_amount"))
                            .hwt_amount = IIf(IsDBNull(dr.Item("hwt_amount")), Nothing, dr.Item("hwt_amount"))
                            .vat_amt = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_amt = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                        End With
                        ' add Accounting to list
                        GetAccounting_Detail.Add(objInvPurchaseDetail)
                    End While
                End If

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetAccounting_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAccounting_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CheckDupAccounting
        '	Discription	    : Check Dup Accounting  
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupAccounting( _
            ByVal cheque_no As String, _
            ByVal cheque_date As String _
        ) As Integer Implements ICheque_PurchaseDao.CheckDupAccounting
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupAccounting = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(id) AS id 				    ")
                    .AppendLine("		FROM accounting  				        ")
                    .AppendLine("		WHERE type in (1,3)                     ")
                    .AppendLine("		and cheque_no  = ?cheque_no				")
                    .AppendLine("		and cheque_date  = ?cheque_date			")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?cheque_no", cheque_no)
                objConn.AddParameter("?cheque_date", cheque_date)

                ' execute sql command
                CheckDupAccounting = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountVendor_rating(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountVendor_rating(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        ''/**************************************************************
        ''	Function name	: GetCheque_Head
        ''	Discription	    : Get Cheque Head
        ''	Return Value	: List
        ''	Create User	    : Pranitda Sroengklang
        ''	Create Date	    : 12-07-2013
        ''	Update User	    :
        ''	Update Date	    :
        ''*************************************************************/
        'Public Function GetApprover( _
        ') As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetApprover
        '    ' variable for keep sql command
        '    Dim strSql As New Text.StringBuilder
        '    ' set default list
        '    GetApprover = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
        '    Try
        '        ' data reader object
        '        Dim dr As MySqlDataReader
        '        Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

        '        With strSql
        '            .AppendLine("		select account_next_approve 				")
        '            .AppendLine("		from user				")
        '            .AppendLine("		where id = ?user_id 				")
        '        End With
        '        ' new connection
        '        objConn = New Common.DBConnection.MySQLAccess

        '        ' assign parameter
        '        objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))

        '        ' execute reader
        '        dr = objConn.ExecuteReader(strSql.ToString)

        '        ' check exist data
        '        If dr.HasRows Then
        '            While dr.Read
        '                ' new object entity
        '                objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
        '                ' assign data from db to entity object
        '                With objInvPurchaseDetail
        '                    .account_next_approve = IIf(IsDBNull(dr.Item("account_next_approve")), Nothing, dr.Item("account_next_approve"))
        '                End With
        '                ' add Accounting to list
        '                GetApprover.Add(objInvPurchaseDetail)
        '            End While
        '        End If

        '    Catch ex As Exception
        '        If IsNothing(objConn) Then objConn = Nothing
        '        objConn.Close()
        '        ' write error log
        '        objLog.ErrorLog("GetApprover(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        '        ' write sql command
        '        objLog.InfoLog("GetApprover(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
        '    Finally
        '        If Not objConn Is Nothing Then objConn.Close()
        '    End Try
        'End Function
        '/**************************************************************
        '	Function name	: UpdateAccounting
        '	Discription	    : Update Accounting
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateAccounting( _
            ByVal strApprover As String, _
            ByVal dtInsAcc As DataTable, _
            ByRef errorType As String _
        ) As Integer Implements ICheque_PurchaseDao.UpdateAccounting
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateAccounting = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()

                'insert payment detail
                intEff = InsertAccounting(strApprover, dtInsAcc, errorType)
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                ' set value to return variable
                UpdateAccounting = 1
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertPayment_Detail
        '	Discription	    : Insert Payment_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertAccounting( _
            ByVal strApprover As String, _
            ByVal dtInsAcc As DataTable, _
            ByRef errorType As String _
        ) As Integer Implements ICheque_PurchaseDao.InsertAccounting
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertAccounting = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                Dim listID As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                Dim strAcountId As String = ""
                Dim strAcountType As String = ""
                Dim intStatusId As Integer = 0

                If Not IsNothing(dtInsAcc) AndAlso dtInsAcc.Rows.Count > 0 Then
                    'Loop insert detail into payment detail
                    For i As Integer = 0 To dtInsAcc.Rows.Count - 1
                        If dtInsAcc.Rows(i)("chkCheque").ToString() = "True" Then
                            Dim ids As String = dtInsAcc.Rows(i)("id").ToString()
                            Dim cheque_no As String = dtInsAcc.Rows(i)("cheque_no").ToString()
                            Dim cheque_date As String = dtInsAcc.Rows(i)("cheque_date").ToString()
                            Dim sub_total As String = dtInsAcc.Rows(i)("sub_total").ToString().Replace(",", "")
                            Dim amount_bank As String = dtInsAcc.Rows(i)("amount_bank").ToString()
                            Dim vat_amount As String = dtInsAcc.Rows(i)("vat_amount").ToString()
                            Dim vat_amt As String = dtInsAcc.Rows(i)("vat_amt").ToString().Replace(",", "")
                            Dim vat_percent As String = dtInsAcc.Rows(i)("vat_percent").ToString()
                            Dim wt_amount As String = dtInsAcc.Rows(i)("wt_amount").ToString()
                            Dim wt_amt As String = dtInsAcc.Rows(i)("wt_amt").ToString().Replace(",", "")
                            Dim wt_percent As String = dtInsAcc.Rows(i)("wt_percent").ToString()
                            'start add by Pranitda S. 2013/09/20
                            Dim account_type As String = dtInsAcc.Rows(i)("account_type").ToString()
                            Dim bank As String = dtInsAcc.Rows(i)("bank").ToString()
                            Dim account_no As String = dtInsAcc.Rows(i)("account_no").ToString()
                            Dim account_name As String = dtInsAcc.Rows(i)("account_name").ToString()
                            'end add by Pranitda S. 2013/09/20

                            strSql = New Text.StringBuilder
                            ' assign sql command
                            With strSql
                                .AppendLine("Update accounting acc")
                                .AppendLine("join payment_header pyh on acc.ref_id=pyh.id")
                                .AppendLine("join payment_detail pyd on pyd.payment_header_id = pyh.id ")
                                .AppendLine("join po_detail pod on pyd.po_header_id=pod.po_header_id and pyd.po_detail_id=pod.id")
                                .AppendLine("and pod.job_order=acc.job_order and pod.ie_id=acc.ie_id and pod.item_id=acc.item_id")
                                .Append("set acc.account_type = ?account_type,")
                                'start add by Pranitda S. 2013/09/20
                                If account_type = "1" Then 'Current Account
                                    .Append("acc.cheque_no = ?cheque_no, acc.bank = null, acc.account_no = null, acc.account_name = null, ")
                                ElseIf account_type = "2" Then 'Saving Account
                                    .Append("acc.cheque_no = null, acc.bank = ?bank, acc.account_no = ?account_no, acc.account_name = ?account_name, ")
                                ElseIf account_type = "3" Then 'Cash
                                    .Append("acc.cheque_no = null, acc.bank = null, acc.account_no = null, acc.account_name = null, ")
                                End If
                                'end add by Pranitda S. 2013/09/20
                                .Append("acc.cheque_date = ?cheque_date, ")
                                .Append("acc.sub_total = (?amount_bank / ?sub_total) * acc.sub_total, ")
                                .Append("acc.vat_amount = (?vat_amount / ?vat_amt) * acc.vat_amount, ")
                                .Append("acc.wt_amount = (?wt_amount / ?wt_amt) * acc.wt_amount, ")
                                .Append("acc.status_id = 3, ")
                                .Append("acc.updated_by = ?updated_by, ")
                                .AppendLine("acc.updated_date = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')")
                                .AppendLine("where acc.type in (1,3) and acc.status_id<>6 and pyh.status_id<>6")
                                .AppendLine("and FIND_IN_SET(CAST(acc.id AS CHAR), ?ids) > 0")
                            End With

                            'start add by Pranitda S. 2013/09/20
                            objConn.AddParameter("?account_type", IIf(String.IsNullOrEmpty(account_type), DBNull.Value, account_type))
                            If account_type = "1" Then 'Current Account
                                objConn.AddParameter("?cheque_no", IIf(String.IsNullOrEmpty(cheque_no), DBNull.Value, cheque_no))
                            End If
                            If account_type = "2" Then 'Current Account
                                objConn.AddParameter("?bank", IIf(String.IsNullOrEmpty(bank), DBNull.Value, bank))
                                objConn.AddParameter("?account_no", IIf(String.IsNullOrEmpty(account_no), DBNull.Value, account_no))
                                objConn.AddParameter("?account_name", IIf(String.IsNullOrEmpty(account_name), DBNull.Value, account_name))
                            End If
                            'end add by Pranitda S. 2013/09/20
                            objConn.AddParameter("?cheque_date", IIf(String.IsNullOrEmpty(cheque_date), DBNull.Value, cheque_date))
                            objConn.AddParameter("?amount_bank", IIf(String.IsNullOrEmpty(amount_bank), DBNull.Value, amount_bank))
                            objConn.AddParameter("?sub_total", IIf(String.IsNullOrEmpty(sub_total), DBNull.Value, sub_total))
                            objConn.AddParameter("?vat_amount", IIf(String.IsNullOrEmpty(vat_amount), DBNull.Value, vat_amount))
                            objConn.AddParameter("?vat_amt", IIf(String.IsNullOrEmpty(vat_amt), DBNull.Value, vat_amt))
                            objConn.AddParameter("?wt_amount", IIf(String.IsNullOrEmpty(wt_amount), DBNull.Value, wt_amount))
                            objConn.AddParameter("?wt_amt", IIf(String.IsNullOrEmpty(wt_amt), DBNull.Value, wt_amt))
                            objConn.AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                            objConn.AddParameter("?ids", IIf(String.IsNullOrEmpty(ids), DBNull.Value, ids))

                            'execute data
                            intEff = objConn.ExecuteNonQuery(strSql.ToString)
                            If intEff < 1 Then
                                InsertAccounting = 0
                                Exit Function
                            End If

                            If HttpContext.Current.Session("AccountNextApprove") Is Nothing Or _
                                String.IsNullOrEmpty(HttpContext.Current.Session("AccountNextApprove")) Or _
                                HttpContext.Current.Session("AccountNextApprove").ToString = "0" Then
                                'update accounting approve
                                If CommonValidation.IsExistAccountApprove = True Then
                                    strAcountId = ids
                                    strAcountType = "1" ' apply for super user
                                    intStatusId = 4
                                    intEff = IAccountingService.UpdateChequeApprove(strAcountId, _
                                                                                    strAcountType, _
                                                                                    intStatusId, _
                                                                                    objConn, _
                                                                                    "1")
                                    If intEff <= 0 Then
                                        InsertAccounting = 0
                                        errorType = "2"
                                        Exit Function
                                    End If
                                End If
                            Else
                                'update payment_header
                                intEff = UpdPayment_header(ids, strApprover)
                                If intEff < 1 Then
                                    InsertAccounting = 0
                                    Exit Function
                                End If
                            End If
                        End If
                    Next
                Else
                    InsertAccounting = 0
                    Exit Function
                End If

                ' set value to return variable
                InsertAccounting = intEff
            Catch ex As Exception
                InsertAccounting = -1
                ' write error log
                objLog.ErrorLog("InsertAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdPayment_header
        '	Discription	    : Upd Payment_header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdPayment_header( _
            ByVal id As String, _
            ByVal strApprover As String _
        ) As Integer Implements ICheque_PurchaseDao.UpdPayment_header
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdPayment_header = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0

                With strSql
                    .AppendLine("Update")
                    .AppendLine("payment_header A")
                    .AppendLine("join accounting B on A.id=B.ref_id")
                    .AppendLine("set A.user_id=?strApprover")
                    .AppendLine(",A.status_id=3")
                    .AppendLine(",A.updated_by=?updated_by")
                    .AppendLine(",A.updated_date=DATE_FORMAT(NOW(), '%Y%m%d%H%i%s')")
                    .AppendLine("where FIND_IN_SET(CAST(B.id AS CHAR), ?ids) > 0")
                    .Append("and B.type in (1,3);")
                End With

                objConn.AddParameter("?strApprover", IIf(String.IsNullOrEmpty(strApprover), DBNull.Value, strApprover))
                objConn.AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?ids", IIf(String.IsNullOrEmpty(id), DBNull.Value, id))

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                UpdPayment_header = intEff
            Catch ex As Exception
                UpdPayment_header = -1
                ' write error log
                objLog.ErrorLog("UpdPayment_header(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdPayment_header(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPurchasePaidReport
        '	Discription	    : Get Purchase PaidReport
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    : Rawikarn Katekeaw
        '	Update Date	    : 29-08-2014
        '*************************************************************/
        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) _
        Implements ICheque_PurchaseDao.GetPurchasePaidReport

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPurchasePaidReport = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 					                                                        ")
                    .AppendLine("			@ROWNUM:=@ROWNUM+1 NO				                                        ")
                    .AppendLine("			,B.invoice_no				                                                ")
                    .AppendLine("			,E.name as vendor_name				                                        ")
                    .AppendLine("			,CAST(B.delivery_date AS DATE) AS delivery_date 				            ")
                    .AppendLine("			,A.sub_total as total_delivery_amount				                        ")
                    .AppendLine("			,A.vat_amount as VAT				                                        ")
                    .AppendLine("			,A.wt_amount as TAX				                                            ")
                    .AppendLine("			,A.sub_total+A.vat_amount-A.wt_amount as amount				                ")
                    .AppendLine("			,C.po_no				                                                    ")
                    .AppendLine("			,CAST(A.cheque_date AS DATE) AS payment_date				                ")
                    .AppendLine("           ,C.currency_id AS currency_id                                                             ")
                    '2013/09/26 Pranitda S. Start-Mod
                    '.AppendLine("			,concat(D.first_name,' ',D.last_name,' | ',G.first_name,' ',G.last_name) as usercreate				")
                    .AppendLine("			,concat(D.first_name,' ',D.last_name) as usercreate	                        ")
                    .AppendLine("			,A.voucher_no	                                                            ")
                    .AppendLine("           ,cur.name	as currency_name                                                                ")
                    '2013/09/26 Pranitda S. End-Mod
                    .AppendLine("		from (select @ROWNUM:=0) as F,					                                    ")
                    .AppendLine("(select ref_id,vendor_id,cheque_date,voucher_no,created_by,                                ")
                    .AppendLine("sum(sub_total) sub_total,sum(vat_amount) vat_amount,sum(wt_amount) wt_amount               ")
                    .AppendLine("from accounting                                                                            ")
                    .AppendLine("where type in (1,3) and status_id<>6                                                       ")
                    .AppendLine("and FIND_IN_SET(CAST(id AS CHAR), ?itemConfirm) > 0                                        ")
                    .AppendLine("group by ref_id,vendor_id,cheque_date,voucher_no,created_by                                ")
                    .AppendLine(") A join payment_header B on A.ref_id=B.id                                                 ")
                    .AppendLine("		join po_header C on B.po_header_id=C.id			                                    ")
                    .AppendLine("		left join mst_vendor E on A.vendor_id=E.id			                                ")
                    .AppendLine("		left join user D on A.created_by=D.id			                                    ")
                    .AppendLine("       Left join (                                                                         ")
                    .AppendLine("               select cu.id, cu.name from mst_currency cu                                           ")
                    .AppendLine("               LEFT JOIN po_header on cu.id = po_header.currency_id                        ")
                    '.AppendLine("               where cu.id = 1                                                             ")
                    .AppendLine("               ) cur on cur.id = c.currency_id                                             ")
                    .AppendLine("               GROUP BY invoice_no, vendor_name , delivery_date, total_delivery_amount     ")
                    .AppendLine("               , vat, tax, amount, po_no, payment_date, voucher_no                         ")

                    '2013/09/26 Pranitda S. Start-Del
                    '.AppendLine("		left join user G on B.user_id=G.id			")
                    '2013/09/26 Pranitda S. Start-Del
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?itemConfirm", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, itemConfirm))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .no = IIf(IsDBNull(dr.Item("NO")), Nothing, dr.Item("NO"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .gross = IIf(IsDBNull(dr.Item("total_delivery_amount")), Nothing, dr.Item("total_delivery_amount"))
                            .vat = IIf(IsDBNull(dr.Item("VAT")), Nothing, dr.Item("VAT"))
                            .WT = IIf(IsDBNull(dr.Item("TAX")), Nothing, dr.Item("TAX"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .paid_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .usercreate = IIf(IsDBNull(dr.Item("usercreate")), Nothing, dr.Item("usercreate"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                            .currency_name = IIf(IsDBNull(dr.Item("currency_name")), Nothing, dr.Item("currency_name"))
                        End With
                        ' add Accounting to list
                        GetPurchasePaidReport.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchasePaidReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPurchasePaidReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentVoucher
        '	Discription	    : Get Payment Voucher
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 23-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentVoucher( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) _
        Implements ICheque_PurchaseDao.GetPaymentVoucher

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentVoucher = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    '.AppendLine("		select distinct 					")
                    '.AppendLine("			case when A.new_voucher_no is null then A.voucher_no else A.new_voucher_no end voucher_no				")
                    '.AppendLine("			,date_format(now(),'%d-%M-%Y') as print_date				")
                    '.AppendLine("			,C.name vendor_name				")
                    '.AppendLine("			,A.cheque_no				")
                    '.AppendLine("			,A.bank				")
                    '.AppendLine("			,CAST(A.cheque_date AS DATE) AS cheque_date				")
                    '.AppendLine("			,B.subtotal	 as sub_total			")
                    '.AppendLine("			,concat('Vat ',F.percent,'%') as vat_percent				")
                    '.AppendLine("			,B.vat				")
                    '.AppendLine("			,case when B.tax>0 then concat('W/T ',G.percent,'%') else '' end wt_percent				")
                    '.AppendLine("			,case when B.tax>0 then B.tax else null end wt				")
                    '.AppendLine("			,B.subtotal+B.vat-IFNULL(B.tax,0) total		")
                    '.AppendLine("		from accounting A 					")
                    '.AppendLine("		join (					")
                    '.AppendLine("			  select 				")
                    '.AppendLine("			  	case when new_voucher_no is null then voucher_no else new_voucher_no end as voucher_no			")
                    '.AppendLine("			  	,cheque_no			")
                    '.AppendLine("			  	,sum(sub_total) subtotal			")
                    '.AppendLine("			  	,sum(vat_amount) vat			")
                    '.AppendLine("			  	,sum(ifnull(wt_amount,0)) tax			")
                    '.AppendLine("			  from accounting				")
                    '.AppendLine("			  where type in (1,3) 				")
                    '.AppendLine("			  and status_id<>6 				")
                    '.AppendLine("		      and  FIND_IN_SET(CAST(id AS CHAR), ?itemConfirm) > 0 ")
                    '.AppendLine("			  group by case when new_voucher_no is null then voucher_no else new_voucher_no end,cheque_no				")
                    '.AppendLine("			) B 				")
                    '.AppendLine("		on case when A.new_voucher_no is null then A.voucher_no else A.new_voucher_no end=B.voucher_no 					")
                    '.AppendLine("		and A.cheque_no=B.cheque_no					")
                    '.AppendLine("		left join mst_vendor C 					")
                    '.AppendLine("		on A.vendor_id=C.id					")
                    '.AppendLine("		join payment_header D 					")
                    '.AppendLine("		on A.ref_id=D.id					")
                    '.AppendLine("		join po_header E 					")
                    '.AppendLine("		on D.po_header_id=E.id					")
                    '.AppendLine("		left join mst_vat F 					")
                    '.AppendLine("		on E.vat_id=F.id					")
                    '.AppendLine("		left join mst_wt G 					")
                    '.AppendLine("		on E.wt_id=G.id					")
                    '.AppendLine("	where  FIND_IN_SET(CAST(A.id AS CHAR), ?itemConfirm1) > 0 ")

                    .AppendLine("		select distinct 						")
                    .AppendLine("			A.voucher_no					")
                    .AppendLine("			,date_format(now(),'%d-%M-%Y') as print_date					")
                    .AppendLine("			,C.name vendor_name					")
                    .AppendLine("			,A.cheque_no					")
                    .AppendLine("			,A.bank ")
                    '2013/09/25 Pranitda S. Start-Add
                    .AppendLine("			,A.account_type ")
                    .AppendLine("			,A.account_no ")
                    .AppendLine("			,A.account_name	")
                    '2013/09/25 Pranitda S. End-Add
                    .AppendLine("			,CAST(A.cheque_date AS DATE) AS cheque_date					")
                    .AppendLine("			,B.subtotal	 as sub_total				")
                    .AppendLine("			,concat('Vat ',F.percent,'%') as vat_percent					")
                    .AppendLine("			,B.vat					")
                    .AppendLine("			,case when B.tax>0 then concat('W/T ',G.percent,'%') else '' end wt_percent					")
                    .AppendLine("			,case when B.tax>0 then B.tax else null end wt					")
                    .AppendLine("			,B.subtotal+B.vat-IFNULL(B.tax,0) total					")
                    .AppendLine("		from accounting A 						")
                    .AppendLine("		join (						")
                    .AppendLine("			  select 					")
                    .AppendLine("			  	voucher_no				")
                    .AppendLine("			  	,account_type				")
                    .AppendLine("			  	,sum(sub_total) subtotal				")
                    .AppendLine("			  	,sum(vat_amount) vat				")
                    .AppendLine("			  	,sum(ifnull(wt_amount,0)) tax				")
                    .AppendLine("			  from accounting					")
                    .AppendLine("			  where type in (1,3) 					")
                    .AppendLine("			  and status_id<>6 					")
                    .AppendLine("		      and  FIND_IN_SET(CAST(id AS CHAR), ?itemConfirm) > 0						")
                    .AppendLine("			  group by voucher_no,account_type					")
                    .AppendLine("			) B 					")
                    .AppendLine("		on A.voucher_no=B.voucher_no 						")
                    .AppendLine("		and A.account_type=B.account_type						")
                    .AppendLine("		left join mst_vendor C on A.vendor_id=C.id						")
                    .AppendLine("		join payment_header D on A.ref_id=D.id						")
                    .AppendLine("		join po_header E on D.po_header_id=E.id						")
                    .AppendLine("		left join mst_vat F on E.vat_id=F.id						")
                    .AppendLine("		left join mst_wt G on E.wt_id=G.id						")
                    .AppendLine("		where  FIND_IN_SET(CAST(A.id AS CHAR), ?itemConfirm1) > 0						")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?itemConfirm", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, itemConfirm))
                objConn.AddParameter("?itemConfirm1", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, itemConfirm))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .print_date = IIf(IsDBNull(dr.Item("print_date")), Nothing, dr.Item("print_date"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .bank = IIf(IsDBNull(dr.Item("bank")), Nothing, dr.Item("bank"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .vat_percent = IIf(IsDBNull(dr.Item("vat_percent")), Nothing, dr.Item("vat_percent"))
                            .vat = IIf(IsDBNull(dr.Item("vat")), Nothing, dr.Item("vat"))
                            .wt_percent = IIf(IsDBNull(dr.Item("wt_percent")), Nothing, dr.Item("wt_percent"))
                            .wt = IIf(IsDBNull(dr.Item("wt")), Nothing, dr.Item("wt"))
                            .total = IIf(IsDBNull(dr.Item("total")), Nothing, dr.Item("total"))
                        End With
                        ' add Accounting to list
                        GetPaymentVoucher.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentVoucher(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentVoucher(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeleteCheque
        '	Discription	    : Delete Cheque
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteCheque( ByVal strId As String) As Integer Implements ICheque_PurchaseDao.DeleteCheque
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteCheque = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("UPDATE accounting ")
                    .AppendLine("set status_id=6 ")
                    .AppendLine(",updated_by=?loginid ")
                    .AppendLine(",updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                    .AppendLine("WHERE type in (1,3) ")
                    .Append("AND FIND_IN_SET(CAST(id AS CHAR), ?ids) > 0;")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?ids", strId)

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect
                If intEff > 0 Then
                    strSql.Length = 0
                    objConn.ClearParameter()
                    With strSql
                        .AppendLine("update payment_header A join accounting B on A.id=B.ref_id")
                        .AppendLine("set A.status_id=5")
                        .AppendLine(",A.updated_by=?loginid ")
                        .AppendLine(",A.updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                        .AppendLine("WHERE B.type in (1,3) ")
                        .Append("AND FIND_IN_SET(CAST(B.id AS CHAR), ?ids) > 0;")
                    End With
                    ' assign parameter
                    objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?ids", strId)
                    objConn.ExecuteNonQuery(strSql.ToString)
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                DeleteCheque = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteCheque(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteCheque(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetTaxReport
        '	Discription	    : Get Tax Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTaxReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetTaxReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetTaxReport = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("select if(IFNULL(A.vendor_branch_id,0)=0,concat(B.name,' (',if(left(hex(left(B.name,1)),2)='E0','สำนักงานใหญ่','Head office'),')'),concat(B.name,' (',I.name,')')) as name ")
                    .AppendLine(",if(IFNULL(A.vendor_branch_id,0)=0,B.address,I.address) as address ")
                    .AppendLine(",B.type2_no ")
                    .AppendLine(",B.type2 ")
                    .AppendLine(",cast(A.cheque_date as date) cheque_date ")
                    .AppendLine(",sum(A.sub_total) amount ")
                    .AppendLine(",sum(ifnull(A.wt_amount,0)) wt_amount ")
                    .AppendLine("from accounting A ")
                    .AppendLine("left join mst_vendor B on A.vendor_id=B.id ")
                    .AppendLine("LEFT JOIN mst_vendor_branch I ON A.vendor_branch_id = I.id ")
                    .AppendLine("where A.type in (1,3) ")
                    .AppendLine("and  FIND_IN_SET(CAST(A.id AS CHAR), ?itemConfirm) > 0 ")
                    .AppendLine("group by B.name,IFNULL(A.vendor_branch_id,0),B.type2_no,B.type2,A.cheque_date ")
                    .AppendLine("order by B.type2;")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?itemConfirm", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, itemConfirm))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .address = IIf(IsDBNull(dr.Item("address")), Nothing, dr.Item("address"))
                            .type2_no = IIf(IsDBNull(dr.Item("type2_no")), Nothing, dr.Item("type2_no"))
                            .type2 = IIf(IsDBNull(dr.Item("type2")), Nothing, dr.Item("type2"))
                            .cheque_date = IIf(IsDBNull(dr.Item("cheque_date")), Nothing, dr.Item("cheque_date"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                        End With
                        ' add Accounting to list
                        GetTaxReport.Add(objInvPurchaseDetail)
                    End While
                End If

                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetTaxReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetTaxReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetAccountReport
        '	Discription	    : Get Account Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccountReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of Entity.ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseDao.GetAccountReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetAccountReport = New List(Of Entity.ImpCheque_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpCheque_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 				")
                    .AppendLine("			A.account_type			")
                    .AppendLine("			,cast(A.cheque_date as date) as account_date			")
                    .AppendLine("			,A.vendor_id			")
                    .AppendLine("			,A.cheque_no			")
                    '.AppendLine("			,case when A.new_voucher_no is null then A.voucher_no else A.new_voucher_no end as voucher_no			")
                    .AppendLine("			,A.voucher_no as voucher_no			")
                    .AppendLine("			,B.name as vendor_name   			")
                    .AppendLine("		 	,sum(ifnull(sub_total,0)+ifnull(vat_amount,0)-ifnull(wt_amount,0)) Expense    			")
                    .AppendLine("		 	,0 income			")
                    .AppendLine("		From accounting A     				")
                    .AppendLine("		left join mst_vendor B on A.vendor_id=B.id     				")
                    .AppendLine("		where type in (1,3) and status_id<>6      				")
                    .AppendLine("		and  FIND_IN_SET(CAST(A.id AS CHAR), ?itemConfirm) > 0 ")
                    .AppendLine("		group by A.account_type,A.cheque_date,A.vendor_id,A.cheque_no,case when A.new_voucher_no is null then A.voucher_no else A.new_voucher_no end				")
                    .AppendLine("		order by A.cheque_date,case when A.new_voucher_no is null then A.voucher_no else A.new_voucher_no end,A.cheque_no;				")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?itemConfirm", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, itemConfirm))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpCheque_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .account_date = IIf(IsDBNull(dr.Item("account_date")), Nothing, dr.Item("account_date"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .cheque_no = IIf(IsDBNull(dr.Item("cheque_no")), Nothing, dr.Item("cheque_no"))
                            .voucher_no = IIf(IsDBNull(dr.Item("voucher_no")), Nothing, dr.Item("voucher_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .Expense = IIf(IsDBNull(dr.Item("Expense")), Nothing, dr.Item("Expense"))
                            .income = IIf(IsDBNull(dr.Item("income")), Nothing, dr.Item("income"))
                        End With
                        ' add Accounting to list
                        GetAccountReport.Add(objInvPurchaseDetail)
                    End While
                End If

                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()

            Catch ex As Exception
                If IsNothing(objConn) Then objConn = Nothing
                objConn.Close()
                ' write error log
                objLog.ErrorLog("GetAccountReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetAccountReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace


