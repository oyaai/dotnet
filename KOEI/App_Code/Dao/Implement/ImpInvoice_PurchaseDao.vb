#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpInvoice_Purchase
'	Class Discription	: Execute process Invoice Purchase
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
    Public Class ImpInvoice_PurchaseDao
        Implements IInvoice_PurchaseDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private objUtility As New Common.Utilities.Utility
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: GetInvoice_PurchaseList
        '	Discription	    : Get Invoice Purchase list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_PurchaseList( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetInvoice_PurchaseList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetInvoice_PurchaseList = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("   Select distinct pay_head.id                                                                ")
                    .AppendLine("   ,case when po_type=0 then 'Purchase' when po_type=1 then 'Outsource' else '' end po_type   ")
                    .AppendLine("   ,po_no,ven_mst.name as vendor_name,po_head.sub_total,invoice_no                            ")
                    .AppendLine("   ,CAST(pay_head.delivery_date AS DATE) AS delivery_date                                     ")
                    .AppendLine("   ,CAST(pay_head.payment_date AS DATE) AS payment_date                                       ")
                    '2013/09/25 Pranitda S. Start-Mod
                    '.AppendLine(",pay_head.total_delivery_amount					")
                    .AppendLine(",pay_head.total_delivery_amount*(1+vat.percent/100) as total_delivery_amount                   ")
                    '2013/09/25 Pranitda S. End-Mod
                    .AppendLine(",case pay_head.status_id                                                                       ")
                    .AppendLine(" when 1 then 'Normal' when 2 then 'Delivery' when 3 then 'Waiting'                             ")
                    .AppendLine(" when 4 then 'Approve' when 5 then 'Decline' when 6 then 'Delete'                              ")
                    .AppendLine(" when 7 then 'Complete' else ''                                                                ")
                    .AppendLine("end as status                                                                                  ")
                    .AppendLine(",pay_head.status_id                                                                            ")
                    .AppendLine(",case when acc.status_id is null or acc.status_id=5 then 1	else 0 end canConfirm               ")
                    .AppendLine(",pay_head.old_id,po_head.id as po_id                                                           ")
                    .AppendLine("From payment_header pay_head                                                                   ")
                    .AppendLine("Join po_header po_head on pay_head.po_header_id=po_head.id                                     ")
                    .AppendLine("Left Join mst_vendor ven_mst on po_head.vendor_id=ven_mst.id                                   ")
                    .AppendLine("Left Join accounting acc on pay_head.id = acc.ref_id and acc.type = '3'                        ")
                    '2013/09/25 Pranitda S. Start-Add
                    .AppendLine("Left Join mst_vat vat on po_head.vat_id=vat.id                                                 ")
                    '2013/09/25 Pranitda S. End-Add
                    .AppendLine("Where pay_head.status_id <> 6")
                    '2013.10.10 Ping Start-Add [ default list waiting confirm ]
                    If objInvPurchaseEnt.strMode.Equals("Page_Load") Then
                        .AppendLine("And (acc.status_id is null or acc.status_id=5)")
                    End If
                    '2013.10.10 Ping End-Add
                    .AppendLine("And (po_type = IFNULL(?po_type, po_type))")

                    .AppendLine("And (po_no = IFNULL(?po_no, po_no))")

                    .AppendLine("AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date)) ")
                    .AppendLine("		OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(pay_head.delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) ) ")
                    .AppendLine("		OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(pay_head.delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))	")
                    .AppendLine("		OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(pay_head.delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE)) ")
                    .AppendLine("	)")

                    .AppendLine("AND ( (ISNULL(?payment_start_date) AND ISNULL(?payment_end_date)) ")
                    .AppendLine("		OR ( ((NOT ISNULL(?payment_start_date)) AND (NOT ISNULL(?payment_end_date))) AND (CAST(pay_head.payment_date AS DATE) BETWEEN CAST(?payment_start_date AS DATE) AND CAST(?payment_end_date AS DATE)) ) ")
                    .AppendLine("		OR ( (((NOT ISNULL(?payment_start_date)) AND ISNULL(?payment_end_date) )) AND CAST(pay_head.payment_date AS DATE) >= CAST(?payment_start_date AS DATE))	")
                    .AppendLine("		OR ( ((ISNULL(?payment_start_date) AND (NOT ISNULL(?payment_end_date)) )) AND CAST(pay_head.payment_date AS DATE) <= CAST(?payment_end_date AS DATE)) ")
                    .AppendLine("	)")

                    .AppendLine("AND (ISNULL(?vendor_name) OR (ven_mst.name LIKE CONCAT('%', ?vendor_name, '%'))) ")

                    .AppendLine("AND ( (ISNULL(?Invoice_start) AND ISNULL(?Invoice_end)) ")
                    .AppendLine("		OR ( ((NOT ISNULL(?Invoice_start)) AND (NOT ISNULL(?Invoice_end))) AND (pay_head.invoice_no >= ?Invoice_start AND pay_head.invoice_no <= ?Invoice_end) ) ")
                    .AppendLine("		OR ( (((NOT ISNULL(?Invoice_start)) AND ISNULL(?Invoice_end) )) AND (pay_head.invoice_no >= ?Invoice_start) ) ")
                    .AppendLine("		OR ( ((ISNULL(?Invoice_start) AND (NOT ISNULL(?Invoice_end)) )) AND (pay_head.invoice_no <= ?Invoice_end) )	")
                    .AppendLine("	)")
                    .AppendLine("Order by id desc")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strSearchType), DBNull.Value, objInvPurchaseEnt.strSearchType))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPO), DBNull.Value, objInvPurchaseEnt.strPO))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strDeliveryDateFrom), DBNull.Value, objInvPurchaseEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strDeliveryDateTo), DBNull.Value, objInvPurchaseEnt.strDeliveryDateTo))
                objConn.AddParameter("?payment_start_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPaymentDateFrom), DBNull.Value, objInvPurchaseEnt.strPaymentDateFrom))
                objConn.AddParameter("?payment_end_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPaymentDateTo), DBNull.Value, objInvPurchaseEnt.strPaymentDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strVendor_name), DBNull.Value, objInvPurchaseEnt.strVendor_name))
                objConn.AddParameter("?Invoice_start", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strInvoice_start), DBNull.Value, objInvPurchaseEnt.strInvoice_start))
                objConn.AddParameter("?Invoice_end", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strInvoice_end), DBNull.Value, objInvPurchaseEnt.strInvoice_end))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .total_delivery_amount = IIf(IsDBNull(dr.Item("total_delivery_amount")), Nothing, dr.Item("total_delivery_amount"))
                            .status_name = IIf(IsDBNull(dr.Item("status")), Nothing, dr.Item("status"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .canConfirm = IIf(IsDBNull(dr.Item("canConfirm")), Nothing, dr.Item("canConfirm"))
                            .old_id = IIf(IsDBNull(dr.Item("old_id")), Nothing, dr.Item("old_id"))
                            .po_header_id = IIf(IsDBNull(dr.Item("po_id")), Nothing, dr.Item("po_id"))
                        End With
                        ' add Accounting to list
                        GetInvoice_PurchaseList.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_PurchaseList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetInvoice_PurchaseList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetInvoice_Header
        '	Discription	    : Get Invoice Header list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetInvoice_Header
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetInvoice_Header = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("   select A.id,A.po_header_id		")
                    .AppendLine("		,CAST(A.delivery_date AS DATE) AS delivery_date		")
                    .AppendLine("		,CAST(A.payment_date AS DATE) AS payment_date		")
                    .AppendLine("		,A.invoice_no 		")
                    .AppendLine("		,case A.account_type 		")
                    .AppendLine("		    when 1 then 'Current Account'		")
                    .AppendLine("			when 2 then 'Saving Account'	")
                    .AppendLine("			when 3 then 'Cash' 	")
                    .AppendLine("			else '' 	")
                    .AppendLine("		end as account_type 	")
                    .AppendLine("		,A.account_no,A.account_name		")
                    .AppendLine("		,A.total_delivery_amount		")
                    .AppendLine("		,A.remark,A.user_id,A.status_id,A.created_by,A.created_date,A.updated_by,A.updated_date		")
                    .AppendLine(",A.total_delivery_amount*(vat.percent/100) as vat_amount")
                    .AppendLine("   from payment_header A ")
                    '2013/09/25 Pranitda S. Start-Add
                    .AppendLine("Join po_header po_head on A.po_header_id=po_head.id")
                    .AppendLine("Left join mst_vat vat on po_head.vat_id=vat.id")
                    '2013/09/25 Pranitda S. End-Add
                    .AppendLine("	where A.id = IFNULL(?id, A.id);")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_header_id = IIf(IsDBNull(dr.Item("po_header_id")), Nothing, dr.Item("po_header_id"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .total_delivery_amount = IIf(IsDBNull(dr.Item("total_delivery_amount")), Nothing, dr.Item("total_delivery_amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .created_by = IIf(IsDBNull(dr.Item("created_by")), Nothing, dr.Item("created_by"))
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .updated_by = IIf(IsDBNull(dr.Item("updated_by")), Nothing, dr.Item("updated_by"))
                            .updated_date = IIf(IsDBNull(dr.Item("updated_date")), Nothing, dr.Item("updated_date"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                        End With
                        ' add Accounting to list
                        GetInvoice_Header.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_Header(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetInvoice_Header(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetInvoice_Header
        '	Discription	    : Get Invoice Header list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoice_Detail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetInvoice_Detail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetInvoice_Detail = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 				")
                    .AppendLine("			mst_item.ITName			")
                    .AppendLine("			,po_d.job_order			")
                    .AppendLine("			,mst_ie.IEName			")
                    .AppendLine("			,po_d.quantity			")
                    .AppendLine("			,po_d.amount			")
                    .AppendLine("			,pay_d.id			")
                    .AppendLine("			,pay_d.payment_header_id			")
                    .AppendLine("			,pay_d.po_header_id			")
                    .AppendLine("			,pay_d.po_detail_id			")
                    .AppendLine("			,pay_d.delivery_qty			")
                    .AppendLine("			,pay_d.delivery_amount			")
                    .AppendLine("			,pay_d.stock_fg			")
                    .AppendLine("			,pay_d.created_by			")
                    .AppendLine("			,pay_d.created_date			")
                    .AppendLine("			,pay_d.updated_by			")
                    .AppendLine("			,pay_d.updated_date			")
                    .AppendLine("		from payment_detail pay_d				")
                    .AppendLine("		join po_detail po_d 				")
                    .AppendLine("		on pay_d.po_detail_id=po_d.id				")
                    .AppendLine("		left join 				")
                    .AppendLine("			(			")
                    .AppendLine("				select 		")
                    .AppendLine("					id	")
                    .AppendLine("					,name as ITName 	")
                    .AppendLine("				from mst_item		")
                    .AppendLine("			) mst_item 			")
                    .AppendLine("		on po_d.item_id=mst_item.id				")
                    .AppendLine("		left join 				")
                    .AppendLine("			(			")
                    .AppendLine("				select 		")
                    .AppendLine("					id,	")
                    .AppendLine("					name as IEName 	")
                    .AppendLine("				from mst_ie		")
                    .AppendLine("			) mst_ie 			")
                    .AppendLine("		on po_d.ie_id=mst_ie.id				")
                    .AppendLine("	where 						")
                    .AppendLine("	    (pay_d.payment_header_id = IFNULL(?id, pay_d.payment_header_id))	")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .payment_header_id = IIf(IsDBNull(dr.Item("payment_header_id")), Nothing, dr.Item("payment_header_id"))
                            .po_header_id = IIf(IsDBNull(dr.Item("po_header_id")), Nothing, dr.Item("po_header_id"))
                            .po_detail_id = IIf(IsDBNull(dr.Item("po_detail_id")), Nothing, dr.Item("po_detail_id"))
                            .delivery_qty = IIf(IsDBNull(dr.Item("delivery_qty")), Nothing, dr.Item("delivery_qty"))
                            .delivery_amount = IIf(IsDBNull(dr.Item("delivery_amount")), Nothing, dr.Item("delivery_amount"))
                            .stock_fg = IIf(IsDBNull(dr.Item("stock_fg")), Nothing, dr.Item("stock_fg"))
                            .created_by = IIf(IsDBNull(dr.Item("created_by")), Nothing, dr.Item("created_by"))
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .updated_by = IIf(IsDBNull(dr.Item("updated_by")), Nothing, dr.Item("updated_by"))
                            .updated_date = IIf(IsDBNull(dr.Item("updated_date")), Nothing, dr.Item("updated_date"))
                            .ITName = IIf(IsDBNull(dr.Item("ITName")), Nothing, dr.Item("ITName"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .IEName = IIf(IsDBNull(dr.Item("IEName")), Nothing, dr.Item("IEName"))
                            .quantity = IIf(IsDBNull(dr.Item("quantity")), Nothing, dr.Item("quantity"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))

                        End With
                        ' add Accounting to list
                        GetInvoice_Detail.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoice_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetInvoice_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeletePayment
        '	Discription	    : Delete Payment_Header,Payment_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePayment( _
            ByVal intId As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.DeletePayment
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePayment = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()

                'Delete table payment_detail first ,and then delete table payment_header
                'intEff = DeletePayment_Detail(intId)
                intEff = DeletePayment_Header(intId)
                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                    'Else
                    'intEff = intEff + DeletePayment_Header(intId)
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
                DeletePayment = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePayment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeletePayment_Header
        '	Discription	    : Delete Payment_Header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePayment_Header( _
            ByVal intId As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.DeletePayment_Header
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePayment_Header = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("update payment_header set status_id=6,updated_by=?loginid,updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                    .AppendLine("where id = ?id;")
                End With

                objConn.AddParameter("?id", intId)
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect
                If intEff > 0 Then
                    strSql.Length = 0
                    objConn.ClearParameter()
                    With strSql
                        .AppendLine("update po_header A join payment_header B on B.po_header_id=A.id ")
                        .AppendLine("left join (")
                        .AppendLine("	select A.po_header_id,sum(ifnull(delivery_qty,0)) deliveryqty")
                        .AppendLine("	from payment_detail A join payment_header B on A.payment_header_id=B.id")
                        .AppendLine("	where A.po_header_id in (select po_header_id from payment_detail where payment_header_id=?id1) and B.status_id<>6")
                        .AppendLine("	group by A.po_header_id")
                        .AppendLine(") C on A.id=C.po_header_id ")
                        .AppendLine("set A.status_id = if(ifnull(C.deliveryqty,0) > 0,2,5)")
                        .AppendLine(",A.updated_by=?loginid ")
                        .AppendLine(",A.updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                        .AppendLine("where B.id = ?id2;")
                    End With
                    ' assign parameter
                    objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?id1", intId)
                    objConn.AddParameter("?id2", intId)
                    objConn.ExecuteNonQuery(strSql.ToString)
                End If
                ' set value to return variable
                DeletePayment_Header = intEff
            Catch ex As Exception
                DeletePayment_Header = -1
                ' write error log
                objLog.ErrorLog("DeletePayment_Header(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePayment_Header(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeletePayment_Detail
        '	Discription	    : Delete Payment_Detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePayment_Detail( _
            ByVal id As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.DeletePayment_Detail
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePayment_Detail = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("update payment_header set status_id=6,updated_by=?loginid,updated_date=date_format(now(),'%Y%m%d%H%i%s') ")
                    .AppendLine("where id = ?id ")
                End With

                'objConn.AddParameter("?payment_header_id", DeletePayment)
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(id), DBNull.Value, id))
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeletePayment_Detail = intEff
            Catch ex As Exception
                DeletePayment_Detail = -1
                ' write error log
                objLog.ErrorLog("DeletePayment_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePayment_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: ExecuteAccounting
        '	Discription	    : Insert Accounting
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ExecuteAccounting( _
            ByVal itemConfirm As String _
        ) As Boolean Implements IInvoice_PurchaseDao.ExecuteAccounting
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            ExecuteAccounting = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                Dim newVoucher As Integer = 0

                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()

                'get newVoucher for insert into Accounting
                newVoucher = GetNewVoucher()

                'Insert data into Accounting Table
                intEff = InsertAccounting(itemConfirm, "P-" & newVoucher)
                If intEff <= 0 Then 'Case cound't insert data into Accounting -->rollback
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Update Voucher_running table
                If UpdateVoucher_running(newVoucher) = False Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Update Payment_header.status = 6 in case status = 5 (Decline)
                intEff = UpdatePayment_header_Con(itemConfirm)
                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'Update accounting.status = 6 in case status = 5 (Decline)
                intEff = Update_Accounting_Con(itemConfirm, "P-" & newVoucher)
                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If
                
                'process complete --> commit transaction
                objConn.CommitTrans()

                Dim cmdText As String = String.Format("select * from accounting where type=3 and status_id=6 and FIND_IN_SET(CAST(ref_id AS CHAR), '{0}') > 0;", itemConfirm)
                Dim ds As DataSet = objConn.ExecuteDataSet(cmdText)
                strSql.Length = 0
                For Each dr As DataRow In ds.Tables(0).Rows
                    For Each dc As DataColumn In ds.Tables(0).Columns
                        strSql.Append(dr(dc).ToString() & "|")
                    Next
                    WriteLog("accounting", HttpContext.Current.Session("UserName").ToString() & vbTab & strSql.ToString())
                    strSql.Length = 0
                Next
                ds.Clear()
                ds.Dispose()

                strSql.Length = 0
                objConn.ClearParameter()
                strSql.AppendLine("delete from accounting where type=3 and status_id=6 and FIND_IN_SET(CAST(ref_id AS CHAR), ?itemConfirm) > 0;")
                objConn.AddParameter("?itemConfirm", itemConfirm)
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                ExecuteAccounting = True
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ExecuteAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("ExecuteAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        Private Sub WriteLog(ByVal table As String, ByVal data As String)
            System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/FileSave/" & table & ".dat"), _
                System.DateTime.Now.ToString("s") & vbTab & data & vbCrLf)
        End Sub

        '/**************************************************************
        '	Function name	: UpdateVoucher_running
        '	Discription	    : Update Voucher_running
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateVoucher_running( _
            ByVal newVoucher As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.UpdateVoucher_running
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateVoucher_running = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE voucher_running            ")
                    .AppendLine("       SET last_inv_po=?newVoucher               ")
                    .AppendLine("           ,updated_by=?loginid               ")
                    .AppendLine("           ,updated_date= date_format(now(),'%Y%m%d%H%i%s')            ")
                End With

                'objConn.AddParameter("?payment_header_id", DeletePayment)
                objConn.AddParameter("?newVoucher", IIf(String.IsNullOrEmpty(newVoucher), DBNull.Value, newVoucher))
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                UpdateVoucher_running = intEff
            Catch ex As Exception
                UpdateVoucher_running = -1
                ' write error log
                objLog.ErrorLog("UpdateVoucher_running(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateVoucher_running(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdatePayment_header
        '	Discription	    : Update Payment_header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePayment_header_Con( _
            ByVal itemConfirm As String _
        ) As Integer Implements IInvoice_PurchaseDao.UpdatePayment_header_Con
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePayment_header_Con = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE payment_header          ")
                    .AppendLine("       SET status_id = 1              ")
                    .AppendLine("       WHERE  FIND_IN_SET(CAST(id AS CHAR), ?itemConfirm) > 0     ")
                    .AppendLine("       and status_id = 5              ")
                End With

                objConn.AddParameter("?itemConfirm", itemConfirm)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                UpdatePayment_header_Con = intEff
            Catch ex As Exception
                UpdatePayment_header_Con = -1
                ' write error log
                objLog.ErrorLog("UpdatePayment_header_Con(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdatePayment_header_Con(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: Update_Accounting_Con
        '	Discription	    : Update Update_Accounting_Con
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 05-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function Update_Accounting_Con( _
            ByVal itemConfirm As String, _
            ByVal newVoucher As String _
        ) As Integer Implements IInvoice_PurchaseDao.Update_Accounting_Con
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            Update_Accounting_Con = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE accounting          ")
                    .AppendLine("       SET status_id = 6              ")
                    '2013/09/26 Pranitda S. Start-Add
                    .AppendLine("       ,new_voucher_no = ?newVoucher  ")
                    .AppendLine("       ,updated_by = ?loginid  ")
                    .AppendLine("       ,updated_date = date_format(now(),'%Y%m%d%H%i%s')  ")
                    '2013/09/26 Pranitda S. End-Add
                    .AppendLine("       WHERE  FIND_IN_SET(CAST(ref_id AS CHAR), ?itemConfirm) > 0     ")
                    .AppendLine("       and status_id = 5              ")
                    .AppendLine("       and type = 3              ")
                End With

                objConn.AddParameter("?newVoucher", newVoucher)
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?itemConfirm", itemConfirm)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                Update_Accounting_Con = intEff
            Catch ex As Exception
                Update_Accounting_Con = -1
                ' write error log
                objLog.ErrorLog("Update_Accounting_Con(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("Update_Accounting_Con(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertAccounting
        '	Discription	    : Insert Accounting
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertAccounting( _
            ByVal itemConfirm As String, _
            ByVal newVoucher As String _
        ) As Integer Implements IInvoice_PurchaseDao.InsertAccounting
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertAccounting = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    '2013/09/26 Pranitda S. Start-Mod
                    '.AppendLine("		insert into accounting			")
                    '.AppendLine("			(		")
                    '.AppendLine("			type		")
                    '.AppendLine("			,ref_id		")
                    '.AppendLine("			,voucher_no		")
                    '.AppendLine("			,new_voucher_no		")
                    '.AppendLine("			,account_type		")
                    '.AppendLine("			,vendor_id		")
                    '.AppendLine("			,account_name		")
                    '.AppendLine("			,account_no		")
                    '.AppendLine("			,account_date		")
                    '.AppendLine("			,job_order		")
                    '.AppendLine("			,vat_id		")
                    '.AppendLine("			,wt_id		")
                    '.AppendLine("			,ie_id		")
                    '.AppendLine("			,vat_amount		")
                    '.AppendLine("			,wt_amount		")
                    '.AppendLine("			,sub_total		")
                    '.AppendLine("			,status_id		")
                    '.AppendLine("			,created_by		")
                    '.AppendLine("			,created_date		")
                    '.AppendLine("			,updated_by		")
                    '.AppendLine("			,updated_date		")
                    '.AppendLine("			,item_id		")
                    '.AppendLine("			)		")
                    '.AppendLine("		select 					")
                    '.AppendLine("			3				")
                    '.AppendLine("			,pay_head.id				")
                    ''.AppendLine("			,case when acc.voucher_no is null then ?newVoucher else acc.voucher_no end				")
                    ''.AppendLine("			,case when acc.voucher_no is null then null else ?newVoucher end				")
                    '' edit by ping
                    '.AppendLine("			,IF(acc.voucher_no is null,if(pay_head.old_id is null,?newVoucher,ov.oldVoucher),acc.voucher_no)	")
                    '.AppendLine("			,IF(acc.voucher_no is null,if(pay_head.old_id is null,null,?newVoucher),?newVoucher)	")
                    '.AppendLine("			,pay_head.account_type				")
                    '.AppendLine("			,po_head.vendor_id				")
                    '.AppendLine("			,pay_head.account_name				")
                    '.AppendLine("			,pay_head.account_no				")
                    '.AppendLine("			,payment_date				")
                    '.AppendLine("			,po_det.job_order				")
                    '.AppendLine("			,po_head.vat_id				")
                    '.AppendLine("			,ifnull(po_head.wt_id,0)		")
                    '.AppendLine("			,po_det.ie_id 		")
                    '.AppendLine("			,round(pay_del.delivery_amount*vat.Percent/100*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/100 else rate.rate end,1) ,2)	")
                    '.AppendLine("			,round(pay_del.delivery_amount*wt.Percent/100*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/100 else rate.rate end,1) ,2)	")
                    '.AppendLine("			,round(pay_del.delivery_amount*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/100 else rate.rate end,1) ,2)	")
                    '.AppendLine("			,1				")
                    '.AppendLine("			,?loginid				")
                    '.AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')				")
                    '.AppendLine("			,?loginid				")
                    '.AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')				")
                    '.AppendLine("			,po_det.item_id		")
                    '.AppendLine("		from payment_header pay_head					")
                    '.AppendLine("		join payment_detail pay_del					")
                    '.AppendLine("		on pay_head.id=pay_del.payment_header_id					")
                    '.AppendLine("		join po_header po_head					")
                    '.AppendLine("		on pay_del.po_header_id=po_head.id					")
                    '.AppendLine("		join po_detail po_det 					")
                    '.AppendLine("		on pay_del.po_detail_id=po_det.id					")
                    '.AppendLine("		left join mst_vat vat 					")
                    '.AppendLine("		on po_head.vat_id=vat.id					")
                    '.AppendLine("		left join mst_wt wt 					")
                    '.AppendLine("		on po_head.wt_id=wt.id					")
                    '.AppendLine("		left join mst_currency curr 					")
                    '.AppendLine("		on po_head.currency_id=curr.id					")

                    '.AppendLine("		        left join (select Z.currency_id,Z.issue_date,Y.ef_date,Y.rate ")
                    '.AppendLine("		from ( ")
                    '.AppendLine("		  select A.currency_id,A.issue_date,max(B.ef_date) max_ef_date ")
                    '.AppendLine("		 from po_header A ")
                    '.AppendLine("		  left join mst_schedule_rate B on B.currency_id=A.currency_id and B.ef_date<=A.issue_date ")
                    '.AppendLine("		  join payment_header C on C.po_header_id=A.id ")
                    ''.AppendLine("		  where B.delete_fg=0 and C.status_id=1 and C.id in (@select) ")
                    '.AppendLine("		  where B.delete_fg=0 and C.status_id in (1,5)  ")
                    '.AppendLine("		  and  FIND_IN_SET(CAST(c.id AS CHAR), ?itemConfirm) > 0 	")
                    '.AppendLine("		 group by A.currency_id,A.issue_date")
                    '.AppendLine("		 ) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date ")
                    '.AppendLine("		) rate on rate.currency_id=po_head.currency_id and rate.issue_date=po_head.issue_date ")


                    '.AppendLine("		left join accounting acc 	")
                    '.AppendLine("		on acc.type=3 					")
                    '.AppendLine("		and acc.ref_id=pay_head.id and acc.job_order=po_det.job_order AND acc.ie_id=po_det.ie_id	")
                    '' add by ping
                    '.AppendLine("		left join (select ref_id,GROUP_CONCAT(DISTINCT voucher_no) oldVoucher from accounting where type=3 GROUP BY ref_id) ov on pay_head.old_id=ov.ref_id ")
                    '.AppendLine("		where  FIND_IN_SET(CAST(pay_head.id AS CHAR), ?itemConfirm) > 0 	")
                    '.AppendLine("		and pay_head.status_id in (1,5)					")

                    .AppendLine("		insert into accounting			")
                    .AppendLine("			(		")
                    .AppendLine("			type		")
                    .AppendLine("			,ref_id		")
                    .AppendLine("			,voucher_no		")
                    .AppendLine("			,account_type		")
                    .AppendLine("			,vendor_id		")
                    .AppendLine("			,vendor_branch_id		")
                    .AppendLine("			,account_name		")
                    .AppendLine("			,account_no		")
                    .AppendLine("			,account_date		")
                    .AppendLine("			,job_order		")
                    .AppendLine("			,vat_id		")
                    .AppendLine("			,wt_id		")
                    .AppendLine("			,ie_id		")
                    .AppendLine("			,item_id		")
                    .AppendLine("			,vat_amount		")
                    .AppendLine("			,wt_amount		")
                    .AppendLine("			,sub_total		")
                    .AppendLine("			,status_id		")
                    .AppendLine("			,created_by		")
                    .AppendLine("			,created_date		")
                    .AppendLine("			,updated_by		")
                    .AppendLine("			,updated_date		")
                    .AppendLine("			)		")
                    .AppendLine("		select 					")
                    .AppendLine("			3				")
                    .AppendLine("			,pay_head.id				")
                    ' edit by ping
                    .AppendLine("			,?newVoucher ")
                    .AppendLine("			,pay_head.account_type				")
                    .AppendLine("			,po_head.vendor_id				")
                    .AppendLine("			,ifnull(po_head.vendor_branch_id,0)		")
                    .AppendLine("			,pay_head.account_name				")
                    .AppendLine("			,pay_head.account_no				")
                    .AppendLine("			,payment_date				")
                    .AppendLine("			,po_det.job_order				")
                    .AppendLine("			,po_head.vat_id				")
                    .AppendLine("			,ifnull(po_head.wt_id,0)		")
                    .AppendLine("			,po_det.ie_id 		")
                    .AppendLine("			,po_det.item_id 		")
                    .AppendLine("			,round(pay_del.delivery_amount*vat.Percent/100*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/1 else rate.rate end,1) ,2)	")
                    .AppendLine("			,round(pay_del.delivery_amount*wt.Percent/100*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/1 else rate.rate end,1) ,2)	")
                    .AppendLine("			,round(pay_del.delivery_amount*ifnull(case upper(curr.name) when 'THB' then 1 when 'JPY' then rate.rate/1 else rate.rate end,1) ,2)	")
                    .AppendLine("			,1				")
                    .AppendLine("			,?loginid				")
                    .AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')				")
                    .AppendLine("			,?loginid				")
                    .AppendLine("			,date_format(now(),'%Y%m%d%H%i%s')				")
                    .AppendLine("		from payment_header pay_head					")
                    .AppendLine("		join payment_detail pay_del					")
                    .AppendLine("		on pay_head.id=pay_del.payment_header_id					")
                    .AppendLine("		join po_header po_head					")
                    .AppendLine("		on pay_del.po_header_id=po_head.id					")
                    .AppendLine("		join po_detail po_det 					")
                    .AppendLine("		on pay_del.po_detail_id=po_det.id					")
                    .AppendLine("		left join mst_vat vat 					")
                    .AppendLine("		on po_head.vat_id=vat.id					")
                    .AppendLine("		left join mst_wt wt 					")
                    .AppendLine("		on po_head.wt_id=wt.id					")
                    .AppendLine("		left join mst_currency curr 					")
                    .AppendLine("		on po_head.currency_id=curr.id					")

                    .AppendLine("		        left join (select Z.currency_id,Z.issue_date,Y.ef_date,Y.rate ")
                    .AppendLine("		from ( ")
                    .AppendLine("		  select A.currency_id,A.issue_date,max(B.ef_date) max_ef_date ")
                    .AppendLine("		 from po_header A ")
                    .AppendLine("		  left join mst_schedule_rate B on B.currency_id=A.currency_id and B.ef_date<=A.issue_date ")
                    .AppendLine("		  join payment_header C on C.po_header_id=A.id ")
                    '.AppendLine("		  where B.delete_fg=0 and C.status_id=1 and C.id in (@select) ")
                    .AppendLine("		  where B.delete_fg=0 and C.status_id in (1,5)  ")
                    .AppendLine("		  and  FIND_IN_SET(CAST(c.id AS CHAR), ?itemConfirm) > 0 	")
                    .AppendLine("		 group by A.currency_id,A.issue_date")
                    .AppendLine("		 ) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date ")
                    .AppendLine("		) rate on rate.currency_id=po_head.currency_id and rate.issue_date=po_head.issue_date ")


                    .AppendLine("		where  FIND_IN_SET(CAST(pay_head.id AS CHAR), ?itemConfirm) > 0 	")
                    .AppendLine("		and pay_head.status_id in (1,5)					")
                    '2013/09/26 Pranitda S. End-Mod
                End With

                'Set parameter
                objConn.AddParameter("?newVoucher", IIf(String.IsNullOrEmpty(itemConfirm), DBNull.Value, newVoucher))
                ' ping edit
                'objConn.AddParameter("?approverid", HttpContext.Current.Session("AccountNextApprove"))
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?itemConfirm", itemConfirm)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

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
        '	Function name	: GetNewVoucher
        '	Discription	    : Get NewVoucher  
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetNewVoucher() As Integer Implements IInvoice_PurchaseDao.GetNewVoucher
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetNewVoucher = 0
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT last_inv_po+1 AS newVoucher	")
                    .AppendLine("		FROM voucher_running  				")
                End With

                '' new connection object
                'objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter

                ' execute sql command
                GetNewVoucher = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetNewVoucher(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("GetNewVoucher(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CountVendor_rating
        '	Discription	    : Count item in vendor_rating  
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountVendor_rating( _
            ByVal intItemID As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.CountVendor_rating
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountVendor_rating = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("SELECT COUNT(*) AS used_count")
                    .AppendLine("FROM vendor_rating")
                    .AppendLine("WHERE payment_header_id = ?item_id")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intItemID)

                ' execute sql command
                CountVendor_rating = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountVendor_rating(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountVendor_rating(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: CountAccounting
        '	Discription	    : Count item in vendor_rating  
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountAccounting( _
            ByVal intItemID As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.CountAccounting
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountAccounting = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("SELECT COUNT(*) AS used_count")
                    .AppendLine("FROM accounting")
                    .AppendLine("WHERE ref_id = ?item_id")
                    .AppendLine("AND type=3 and status_id<>6;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intItemID)

                ' execute sql command
                CountAccounting = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetCostTableOverviewReport
        '	Discription	    : Get Accounting Overview Report
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPurchasePaidReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPurchasePaidReport = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    '2013/09/26 Pranitda S. Start-Mod
                    '.AppendLine("		select 					")
                    '.AppendLine("				 @ROWNUM:=@ROWNUM+1 NO			")
                    '.AppendLine("					,invoice_no		")
                    '.AppendLine("					, vendor_name		")
                    '.AppendLine("					,CAST(delivery_date AS DATE) AS delivery_date		")
                    '.AppendLine("					,total_delivery_amount		")
                    '.AppendLine("					,VAT		")
                    '.AppendLine("					,TAX		")
                    '.AppendLine("					,amount		")
                    '.AppendLine("					,po_no		")
                    '.AppendLine("					,CAST(payment_date AS DATE) AS payment_date		")
                    '.AppendLine("					,usercreate		")
                    '.AppendLine("					,voucher_no		")
                    '.AppendLine("							")
                    '.AppendLine("		 from (					")

                    '.AppendLine("		select distinct					")
                    ''.AppendLine("			@ROWNUM:=@ROWNUM+1 NO				")
                    '.AppendLine("			invoice_no				")
                    '.AppendLine("			,G.VName as vendor_name				")
                    '.AppendLine("			,A.delivery_date				")
                    '.AppendLine("			,total_delivery_amount*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then B.rate/100 else B.rate end,1) total_delivery_amount				")
                    '.AppendLine("			,total_delivery_amount*E.Percent/100*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then B.rate/100 else B.rate end,1) as VAT				")
                    '.AppendLine("			,total_delivery_amount*ifnull(F.Percent,0)/100*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then B.rate/100 else B.rate end,1) as TAX				")
                    '.AppendLine("			,(total_delivery_amount+total_delivery_amount*E.Percent/100-total_delivery_amount*ifnull(F.Percent,0)/100)*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then B.rate/100 else B.rate end,1) as amount				")
                    '.AppendLine("			,C.po_no				")
                    '.AppendLine("			,payment_date				")
                    '.AppendLine("			,concat(first_name,' ',last_name) usercreate				")
                    ''edit by ping
                    '.AppendLine("			,if(J.new_voucher_no is null,J.voucher_no,J.new_voucher_no) voucher_no				")
                    '.AppendLine("		from 					")
                    '.AppendLine("		payment_header A					")
                    '.AppendLine("		join po_header C 					")
                    '.AppendLine("		on A.po_header_id=C.id					")
                    '.AppendLine("		left join mst_vat E 					")
                    '.AppendLine("		on C.vat_id=E.id					")
                    '.AppendLine("		left join mst_wt F 					")
                    '.AppendLine("		on C.wt_id=F.id					")
                    '.AppendLine("		left join (select *,name as VName from mst_vendor) G 					")
                    '.AppendLine("		on C.vendor_id=G.id					")
                    '.AppendLine("		left join user D 					")
                    '.AppendLine("		on A.created_by=D.id					")
                    '.AppendLine("		left join mst_currency H 					")
                    '.AppendLine("		on C.currency_id=H.id					")
                    ''.AppendLine("		left join 					")
                    ''.AppendLine("			(select currency_id,ef_date,rate 				")
                    ''.AppendLine("			from mst_schedule_rate 				")
                    ''.AppendLine("			where delete_fg=0 order by ef_date desc limit 1				")
                    ''.AppendLine("		) B 					")
                    ''.AppendLine("		on B.currency_id=C.currency_id 					")
                    ''.AppendLine("		and B.ef_date<=C.issue_date					")


                    '.AppendLine("		left join (select Z.currency_id,Z.issue_date,Y.ef_date,Y.rate	")
                    '.AppendLine("		from (	")
                    '.AppendLine("		 select A.currency_id,A.issue_date,max(B.ef_date) max_ef_date	")
                    '.AppendLine("		  from po_header A	")
                    '.AppendLine("		 left join mst_schedule_rate B on B.currency_id=A.currency_id and B.ef_date<=A.issue_date	")
                    '.AppendLine("		 join payment_header C on C.po_header_id=A.id 	")
                    ''.AppendLine("		  where B.delete_fg=0 and C.id in (@select)	")
                    '.AppendLine("		  where B.delete_fg=0 	")
                    '.AppendLine("		  and  FIND_IN_SET(CAST(C.id AS CHAR), ?itemConfirm) > 0 ")
                    '.AppendLine("		  group by A.currency_id,A.issue_date	")
                    '.AppendLine("		) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date	")
                    '.AppendLine("		) B on B.currency_id=C.currency_id and B.issue_date=C.issue_date	")
                    '.AppendLine("		left join accounting J 					")
                    '.AppendLine("		on J.ref_id=A.id and J.type=3					")
                    ''edit by ping
                    '.AppendLine("		where  FIND_IN_SET(CAST(A.id AS CHAR), ?itemConfirm) > 0 and J.status_id<>6 ")
                    '.AppendLine("		) a ,(select @ROWNUM:=0) as I; ")

                    .AppendLine("			select @ROWNUM:=@ROWNUM+1 NO ")
                    .AppendLine("			    ,B.invoice_no ")
                    .AppendLine("			    ,E.name as vendor_name									 ")
                    .AppendLine("				,CAST(B.delivery_date AS DATE) AS delivery_date								 ")
                    .AppendLine("				,A.sub_total as total_delivery_amount ")
                    .AppendLine("			    ,A.vat_amount as VAT ")
                    .AppendLine("			    ,A.wt_amount as TAX								 ")
                    .AppendLine("				,A.sub_total+A.vat_amount-A.wt_amount as amount								 ")
                    .AppendLine("				,C.po_no ")
                    .AppendLine("			    ,CAST(B.payment_date AS DATE) AS payment_date								 ")
                    .AppendLine("				,concat(D.first_name,' ',D.last_name) as usercreate								 ")
                    .AppendLine("				,A.voucher_no								 ")
                    .AppendLine("			from (select @ROWNUM:=0) as F,									 ")
                    .AppendLine("			(select ref_id,voucher_no,created_by,									 ")
                    .AppendLine("				sum(sub_total) sub_total,sum(vat_amount) vat_amount,sum(wt_amount) wt_amount								 ")
                    .AppendLine("			  from accounting									 ")
                    .AppendLine("			  where type in (1,3) and status_id<>6 and FIND_IN_SET(CAST(ref_id AS CHAR), ?itemConfirm) > 0									 ")
                    .AppendLine("			  group by ref_id,voucher_no,created_by									 ")
                    .AppendLine("			) A join payment_header B on A.ref_id=B.id									 ")
                    .AppendLine("			join po_header C on B.po_header_id=C.id									 ")
                    .AppendLine("			left join mst_vendor E on C.vendor_id=E.id									 ")
                    .AppendLine("			left join user D on A.created_by=D.id;									 ")
                    '2013/09/26 Pranitda S. End-Mod
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
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
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
        '	Function name	: GetPO_List
        '	Discription	    : Get PO list
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_List( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPO_List
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPO_List = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 						")
                    .AppendLine("			po_head.id					")
                    .AppendLine("			,case 					")
                    .AppendLine("				when po_type=0 then 'Purchase' 				")
                    .AppendLine("				when po_type=1 then 'Outsource' 				")
                    .AppendLine("				else '' 				")
                    .AppendLine("			end po_type					")
                    .AppendLine("			,po_no					")
                    .AppendLine("			,vendor.name as vendor_name	")
                    .AppendLine("			,CAST(issue_date AS DATE) AS issue_date					")
                    .AppendLine("			,CAST(delivery_date AS DATE) AS delivery_date					")
                    .AppendLine("			,sub_total		")
                    .AppendLine("		from 						")
                    .AppendLine("			po_header po_head		")
                    .AppendLine("		left join mst_vendor vendor 						")
                    .AppendLine("		on po_head.vendor_id=vendor.id				")
                    .AppendLine("		where 						")
                    .AppendLine("			(po_type = IFNULL(?po_type, po_type))					")

                    .AppendLine("		AND ( (ISNULL(?po_from) AND ISNULL(?po_to)) 						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?po_from)) AND (NOT ISNULL(?po_to))) AND (po_no >= ?po_from AND po_no <= ?po_to) )		")
                    .AppendLine("		      OR ( (((NOT ISNULL(?po_from)) AND ISNULL(?po_to) )) AND (po_no >= ?po_from) )						")
                    .AppendLine("		      OR ( ((ISNULL(?po_from) AND (NOT ISNULL(?po_to)) )) AND (po_no <= ?po_to) )						")
                    .AppendLine("			)					")

                    .AppendLine("		AND ( (ISNULL(?issue_start_date) AND ISNULL(?issue_end_date))						")
                    .AppendLine("				OR ( ((NOT ISNULL(?issue_start_date)) AND (NOT ISNULL(?issue_end_date))) AND (CAST(issue_date AS DATE) BETWEEN CAST(?issue_start_date AS DATE) AND CAST(?issue_end_date AS DATE)) )      				")
                    .AppendLine("				OR ( (((NOT ISNULL(?issue_start_date)) AND ISNULL(?issue_end_date) )) AND CAST(issue_date AS DATE) >= CAST(?issue_start_date AS DATE))				")
                    .AppendLine("				OR ( ((ISNULL(?issue_start_date) AND (NOT ISNULL(?issue_end_date)) )) AND CAST(issue_date AS DATE) <= CAST(?issue_end_date AS DATE))				")
                    .AppendLine("			)					")

                    .AppendLine("		AND ( (ISNULL(?delivery_start_date) AND ISNULL(?delivery_end_date))						")
                    .AppendLine("		      OR ( ((NOT ISNULL(?delivery_start_date)) AND (NOT ISNULL(?delivery_end_date))) AND (CAST(delivery_date AS DATE) BETWEEN CAST(?delivery_start_date AS DATE) AND CAST(?delivery_end_date AS DATE)) )      						")
                    .AppendLine("		      OR ( (((NOT ISNULL(?delivery_start_date)) AND ISNULL(?delivery_end_date) )) AND CAST(delivery_date AS DATE) >= CAST(?delivery_start_date AS DATE))						")
                    .AppendLine("		      OR ( ((ISNULL(?delivery_start_date) AND (NOT ISNULL(?delivery_end_date)) )) AND CAST(delivery_date AS DATE) <= CAST(?delivery_end_date AS DATE))						")
                    .AppendLine("		    )						")

                    .AppendLine("		AND (ISNULL(?vendor_name) OR (vendor.name LIKE CONCAT('%', ?vendor_name, '%')))						")

                    .AppendLine("		and status_id in (2,4) 					")
                    .AppendLine("		order by po_head.id desc						")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?po_type", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strSearchType), DBNull.Value, objInvPurchaseEnt.strSearchType))
                objConn.AddParameter("?po_from", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPOFrom), DBNull.Value, objInvPurchaseEnt.strPOFrom))
                objConn.AddParameter("?po_to", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPOTo), DBNull.Value, objInvPurchaseEnt.strPOTo))
                objConn.AddParameter("?issue_start_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strIssueDateFrom), DBNull.Value, objInvPurchaseEnt.strIssueDateFrom))
                objConn.AddParameter("?issue_end_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strIssueDateTo), DBNull.Value, objInvPurchaseEnt.strIssueDateTo))
                objConn.AddParameter("?delivery_start_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strDeliveryDateFrom), DBNull.Value, objInvPurchaseEnt.strDeliveryDateFrom))
                objConn.AddParameter("?delivery_end_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strDeliveryDateTo), DBNull.Value, objInvPurchaseEnt.strDeliveryDateTo))
                objConn.AddParameter("?vendor_name", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strVendor_name), DBNull.Value, objInvPurchaseEnt.strVendor_name))
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                        End With
                        ' add Accounting to list
                        GetPO_List.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_List(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPO_List(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPO_Header
        '	Discription	    : Get PO_Header
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPO_Header
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPO_Header = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 			                        	                    ")
                    .AppendLine("			case po_type 			                                    ")
                    .AppendLine("				when 0 then 'Purchase' 		                            ")
                    .AppendLine("				when 1 then 'Outsource' 		                        ")
                    .AppendLine("				else '' 		                                        ")
                    .AppendLine("			end as po_type_name			                                ")
                    .AppendLine("			,vendor.vendor_name			                                ")
                    .AppendLine("			,concat(term.term_day,' day(s)') as payment_term			")
                    .AppendLine("			,vat.percent as VAT			                                ")
                    .AppendLine("			,wt.percent as WT			                                ")
                    .AppendLine("			,curr.currency_Name			                                ")
                    .AppendLine("		,po_head.id		                                                ")
                    .AppendLine("		,po_head.po_no		                                            ")
                    .AppendLine("		,po_head.po_type		                                        ")
                    .AppendLine("		,po_head.vendor_id		                                        ")
                    .AppendLine("		,po_head.quotation_no		                                    ")
                    .AppendLine("		,CAST(po_head.issue_date AS DATE) AS issue_date		            ")
                    .AppendLine("		,CAST(po_head.delivery_date AS DATE) delivery_date		        ")
                    .AppendLine("		,po_head.payment_term_id		                                ")
                    .AppendLine("		,po_head.vat_id		                                            ")
                    .AppendLine("		,po_head.wt_id		                                            ")
                    .AppendLine("		,po_head.currency_id		                                    ")
                    .AppendLine("		,po_head.remark		                                            ")
                    .AppendLine("		,po_head.discount_total		                                    ")
                    .AppendLine("		,po_head.sub_total		                                        ")
                    .AppendLine("		,po_head.vat_amount		                                        ")
                    .AppendLine("		,po_head.wt_amount		                                        ")
                    .AppendLine("		,po_head.total_amount		                                    ")
                    .AppendLine("		,po_head.attn		                                            ")
                    .AppendLine("		,po_head.deliver_to		                                        ")
                    .AppendLine("		,po_head.contact		                                        ")
                    .AppendLine("		,po_head.user_id		                                        ")
                    .AppendLine("		,po_head.status_id		                                        ")
                    .AppendLine("		,po_head.created_by		                                        ")
                    .AppendLine("		,po_head.created_date		                                    ")
                    .AppendLine("		,po_head.updated_by		                                        ")
                    .AppendLine("		,po_head.updated_date		                                    ")
                    .AppendLine("		from po_header po_head				                            ")
                    .AppendLine("		left join 				                                        ")
                    .AppendLine("			(select 			                                        ")
                    .AppendLine("				*		                                                ")
                    .AppendLine("				,name as vendor_name 		                            ")
                    .AppendLine("			from mst_vendor			                                    ")
                    .AppendLine("			) vendor 			                                        ")
                    .AppendLine("		on po_head.vendor_id=vendor.id				                    ")
                    .AppendLine("		left join mst_payment_term term 				                ")
                    .AppendLine("		on po_head.payment_term_id=term.id				                ")
                    .AppendLine("		left join mst_vat vat 				                            ")
                    .AppendLine("		on po_head.vat_id=vat.id				                        ")
                    .AppendLine("		left join mst_wt wt 				                            ")
                    .AppendLine("		on po_head.wt_id=wt.id				                            ")
                    .AppendLine("		left join 				                                        ")
                    .AppendLine("			(select 			                                        ")
                    .AppendLine("				*		                                                ")
                    .AppendLine("				,name as currency_Name 		                            ")
                    .AppendLine("			from 			                                            ")
                    .AppendLine("				mst_currency		                                    ")
                    .AppendLine("			) curr 			                                            ")
                    .AppendLine("		on po_head.currency_id=curr.id				                    ")
                    .AppendLine("		where po_head.id=IFNULL(?id,po_head.id)				            ")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_type_name = IIf(IsDBNull(dr.Item("po_type_name")), Nothing, dr.Item("po_type_name"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .quotation_no = IIf(IsDBNull(dr.Item("quotation_no")), Nothing, dr.Item("quotation_no"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .payment_term_id = IIf(IsDBNull(dr.Item("payment_term_id")), Nothing, dr.Item("payment_term_id"))
                            .vat_id = IIf(IsDBNull(dr.Item("vat_id")), Nothing, dr.Item("vat_id"))
                            .wt_id = IIf(IsDBNull(dr.Item("wt_id")), Nothing, dr.Item("wt_id"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .discount_total = IIf(IsDBNull(dr.Item("discount_total")), Nothing, dr.Item("discount_total"))
                            .sub_total = IIf(IsDBNull(dr.Item("sub_total")), Nothing, dr.Item("sub_total"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .total_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .attn = IIf(IsDBNull(dr.Item("attn")), Nothing, dr.Item("attn"))
                            .deliver_to = IIf(IsDBNull(dr.Item("deliver_to")), Nothing, dr.Item("deliver_to"))
                            .contact = IIf(IsDBNull(dr.Item("contact")), Nothing, dr.Item("contact"))
                            .user_id = IIf(IsDBNull(dr.Item("user_id")), Nothing, dr.Item("user_id"))
                            .status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .created_by = IIf(IsDBNull(dr.Item("created_by")), Nothing, dr.Item("created_by"))
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .updated_by = IIf(IsDBNull(dr.Item("updated_by")), Nothing, dr.Item("updated_by"))
                            .updated_date = IIf(IsDBNull(dr.Item("updated_date")), Nothing, dr.Item("updated_date"))
                            .po_type = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .payment_term = IIf(IsDBNull(dr.Item("payment_term")), Nothing, dr.Item("payment_term"))
                            .vat = IIf(IsDBNull(dr.Item("VAT")), Nothing, dr.Item("VAT"))
                            .WT = IIf(IsDBNull(dr.Item("WT")), Nothing, dr.Item("WT"))
                            .currency_Name = IIf(IsDBNull(dr.Item("currency_Name")), Nothing, dr.Item("currency_Name"))
                        End With
                        ' add Accounting to list
                        GetPO_Header.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Header(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPO_Header(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPO_Detail
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Detail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPO_Detail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPO_Detail = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 				                        ")
                    .AppendLine("			item.name as item_name			        ")
                    .AppendLine("			,ie.name as ie_name			            ")
                    .AppendLine("			,unit.name as unit_name			        ")
                    .AppendLine("			,case discount_type 			        ")
                    .AppendLine("				when 0 then 'No Discount' 		    ")
                    .AppendLine("				when 1 then curr.name 		        ")
                    .AppendLine("				when 2 then 'Percent(%)' 		    ")
                    .AppendLine("				else '' 		                    ")
                    .AppendLine("			end as discount_type			        ")
                    .AppendLine("		,del.id		                                ")
                    .AppendLine("		,del.po_header_id		                    ")
                    .AppendLine("		,del.item_id		                        ")
                    .AppendLine("		,del.job_order		                        ")
                    .AppendLine("		,del.ie_id		                            ")
                    .AppendLine("		,del.unit_price		                        ")
                    .AppendLine("		,del.quantity		                        ")
                    .AppendLine("		,del.unit_id		                        ")
                    .AppendLine("		,del.discount		                        ")
                    .AppendLine("		,del.discount_type		                    ")
                    .AppendLine("		,del.amount		                            ")
                    .AppendLine("		,del.vat_amount		                        ")
                    .AppendLine("		,del.wt_amount		                        ")
                    .AppendLine("		,del.remark		                            ")
                    .AppendLine("		,del.created_by		                        ")
                    .AppendLine("		,del.created_date		                    ")
                    .AppendLine("		,del.updated_by		                        ")
                    .AppendLine("		,del.updated_date		                    ")
                    .AppendLine("		,unit.name as unit_name	                    ")
                    .AppendLine("		from po_detail del				            ")
                    .AppendLine("		left join mst_item item				        ")
                    .AppendLine("		on del.item_id=item.id				        ")
                    .AppendLine("		left join mst_ie ie				            ")
                    .AppendLine("		on del.ie_id = ie.id				        ")
                    .AppendLine("		left join mst_unit unit				        ")
                    .AppendLine("		on del.unit_id=unit.id				        ")
                    .AppendLine("		left join po_header po_head 				")
                    .AppendLine("		on del.po_header_id=po_head.id				")
                    .AppendLine("		left join mst_currency curr				    ")
                    .AppendLine("		on po_head.currency_id = curr.id			")
                    .AppendLine("		where po_header_id=IFNULL(?id,po_header_id)	")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_header_id = IIf(IsDBNull(dr.Item("po_header_id")), Nothing, dr.Item("po_header_id"))
                            .item_id = IIf(IsDBNull(dr.Item("item_id")), Nothing, dr.Item("item_id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .unit_price = IIf(IsDBNull(dr.Item("unit_price")), Nothing, dr.Item("unit_price"))
                            .quantity = IIf(IsDBNull(dr.Item("quantity")), Nothing, dr.Item("quantity"))
                            .unit_id = IIf(IsDBNull(dr.Item("unit_id")), Nothing, dr.Item("unit_id"))
                            .discount = IIf(IsDBNull(dr.Item("discount")), Nothing, dr.Item("discount"))
                            .discount_type = IIf(IsDBNull(dr.Item("discount_type")), Nothing, dr.Item("discount_type"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .vat_amount = IIf(IsDBNull(dr.Item("vat_amount")), Nothing, dr.Item("vat_amount"))
                            .wt_amount = IIf(IsDBNull(dr.Item("wt_amount")), Nothing, dr.Item("wt_amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .created_by = IIf(IsDBNull(dr.Item("created_by")), Nothing, dr.Item("created_by"))
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .updated_by = IIf(IsDBNull(dr.Item("updated_by")), Nothing, dr.Item("updated_by"))
                            .updated_date = IIf(IsDBNull(dr.Item("updated_date")), Nothing, dr.Item("updated_date"))
                            .item_name = IIf(IsDBNull(dr.Item("item_name")), Nothing, dr.Item("item_name"))
                            .ie_name = IIf(IsDBNull(dr.Item("ie_name")), Nothing, dr.Item("ie_name"))
                            .unit_name = IIf(IsDBNull(dr.Item("unit_name")), Nothing, dr.Item("unit_name"))
                            .discount_type = IIf(IsDBNull(dr.Item("discount_type")), Nothing, dr.Item("discount_type"))
                            .unit_name = IIf(IsDBNull(dr.Item("unit_name")), Nothing, dr.Item("unit_name"))
                        End With
                        ' add Accounting to list
                        GetPO_Detail.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPO_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPO_Detail_Insert
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Detail_Insert( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPO_Detail_Insert
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPO_Detail_Insert = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 			                                                        ")
                    .AppendLine("			po_det.id		                                                    ")
                    .AppendLine("			,item.name as item_name		                                        ")
                    .AppendLine("			,job_order		                                                    ")
                    .AppendLine("			,ie.name as ie_name		                                            ")
                    .AppendLine("			,quantity		                                                    ")
                    .AppendLine("			,amount		                                                        ")
                    '.AppendLine("			,po_det.unit_price	")
                    .AppendLine("			,amount/quantity as unit_price	                                    ")
                    .AppendLine("			,quantity-ifnull(sumDeliQty,0) as remain_qty		                ")
                    '.AppendLine("			,po_det.unit_price*(quantity-ifnull(sumDeliQty,0)) as remain_amt		")
                    .AppendLine("			,amount/quantity*(quantity-ifnull(sumDeliQty,0)) as remain_amt		")
                    .AppendLine("			,quantity-ifnull(sumDeliQty,0) as delivery_qty			")
                    '.AppendLine("			,po_det.unit_price*(quantity-ifnull(sumDeliQty,0)) as delivery_amt	")
                    .AppendLine("			,amount/quantity*(quantity-ifnull(sumDeliQty,0)) as delivery_amt	")
                    .AppendLine("			,po_head.po_no		                                                ")
                    .AppendLine("			,po_det.ie_id	                                                	")
                    .AppendLine("			,po_head.vendor_id	                                            	")
                    .AppendLine("			,po_det.item_id		                                                ")
                    .AppendLine("			,if(INSTR(po_det.remark,'#')>0,REPLACE(substring_index(po_det.remark,'#',2),'#',''),'') base		")
                    .AppendLine("		from po_detail po_det		                                        	")
                    .AppendLine("		left join                                                           	")
                    .AppendLine("		    mst_item item		                                            	")
                    .AppendLine("		on                                                                      ")
                    .AppendLine("		    po_det.item_id = item.id		                                	")
                    .AppendLine("		left join                                                               ")
                    .AppendLine("		    mst_ie ie			                                                ")
                    .AppendLine("		on  		                                                            ")
                    .AppendLine("		    po_det.ie_id = ie.id		                                    	")
                    .AppendLine("		left join    	                                                    	")
                    .AppendLine("		    po_header po_head		                                        	")
                    .AppendLine("		on  			                                                        ")
                    .AppendLine("		    po_det.po_header_id=po_head.id		                            	")
                    .AppendLine("		left join    		                                                    ")
                    .AppendLine("	        (select                                                              ")
                    .AppendLine("	            A.po_header_id                                                 ")
                    .AppendLine("	            ,po_detail_id                                                   ")
                    .AppendLine("	            ,sum(delivery_qty) as sumDeliQty                                  ")
                    .AppendLine("	            ,sum(delivery_amount) as sumAmount                              ")
                    .AppendLine("	        from payment_detail A                                             ")
                    .AppendLine("	        join payment_header B on A.payment_header_id=B.id                ")
                    .AppendLine("	        where(B.status_id <> 6)                                         ")
                    .AppendLine("	        and A.po_header_id=?id                                            ")
                    .AppendLine("	        group by po_header_id,po_detail_id                               ")
                    .AppendLine("	        ) sumDeli                                                        ")
                    .AppendLine("       on sumDeli.po_header_id=po_det.po_header_id                     	")
                    .AppendLine("       and sumDeli.po_detail_id=po_det.id                              	")
                    .AppendLine("		where 			                                                    ")
                    .AppendLine("		    po_det.po_header_id=?id1			                            ")
                    .AppendLine("		and quantity-ifnull(sumDeliQty,0) > 0	                            ")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))
                objConn.AddParameter("?id1", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .item_name = IIf(IsDBNull(dr.Item("item_name")), Nothing, dr.Item("item_name"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .ie_name = IIf(IsDBNull(dr.Item("ie_name")), Nothing, dr.Item("ie_name"))
                            .quantity = IIf(IsDBNull(dr.Item("quantity")), Nothing, dr.Item("quantity"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .unit_price = IIf(IsDBNull(dr.Item("unit_price")), Nothing, dr.Item("unit_price"))
                            .remain_qty = IIf(IsDBNull(dr.Item("remain_qty")), Nothing, dr.Item("remain_qty"))
                            .remain_amt = IIf(IsDBNull(dr.Item("remain_amt")), Nothing, dr.Item("remain_amt"))
                            .delivery_qty = IIf(IsDBNull(dr.Item("delivery_qty")), Nothing, dr.Item("delivery_qty"))
                            .delivery_amt = IIf(IsDBNull(dr.Item("delivery_amt")), Nothing, dr.Item("delivery_amt"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .item_id = IIf(IsDBNull(dr.Item("item_id")), Nothing, dr.Item("item_id"))
                            .base = IIf(IsDBNull(dr.Item("base")), Nothing, dr.Item("base"))
                        End With
                        ' add Accounting to list
                        GetPO_Detail_Insert.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Detail_Insert(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPO_Detail_Insert(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPO_Detail_Insert
        '	Discription	    : Get PO_Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPO_Header_Insert( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPO_Header_Insert
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPO_Header_Insert = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 							                                            ")
                    .AppendLine("			A.orderamt-ifnull(B.delivery,0) as delivery_amount 						")
                    .AppendLine("			,po_hed.vendor_id					                                	")
                    .AppendLine("		from (							                                            ")
                    .AppendLine("				select 				                                            	")
                    .AppendLine("					po_header_id			                                    	")
                    .AppendLine("					,sum(amount) orderamt 			                            	")
                    .AppendLine("				from po_detail					                                    ")
                    .AppendLine("				where po_header_id=?id				                            	")
                    .AppendLine("				group by po_header_id				                            	")
                    .AppendLine("				) A 					                                            ")
                    .AppendLine("		left join 						                                        	")
                    .AppendLine("				(select 				                                            ")
                    .AppendLine("					po_header_id			                                    	")
                    .AppendLine("					,sum(ifnull(total_delivery_amount,0)) as delivery 				")
                    .AppendLine("				from payment_header				                                	")
                    .AppendLine("				where status_id <> 6 and po_header_id=?id1			        		")
                    .AppendLine("				group by po_header_id				                            	")
                    .AppendLine("		) B 							                                            ")
                    .AppendLine("		on A.po_header_id=B.po_header_id				                			")
                    .AppendLine("		left join po_header po_hed						                        	")
                    .AppendLine("		on A.po_header_id = po_hed.id;						                    	")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))
                objConn.AddParameter("?id1", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .delivery_amount = IIf(IsDBNull(dr.Item("delivery_amount")), Nothing, dr.Item("delivery_amount"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                        End With
                        ' add Accounting to list
                        GetPO_Header_Insert.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPO_Header_Insert(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPO_Header_Insert(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertPayment
        '	Discription	    : Insert Payment Header,payment detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPayment( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer Implements IInvoice_PurchaseDao.InsertPayment
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertPayment = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()

                'insert payment header
                intEff = InsertPayment_Header(objInvPurchaseEnt)
                If intEff <= 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'insert payment detail
                intEff = InsertPayment_Detail(objInvPurchaseEnt.strPO_header_id, dtPaymentDetail)
                If intEff <= 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                intEff = updPO_Header_Status(objInvPurchaseEnt.strPO_header_id)

                If intEff < 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'delete task table
                intEff = deleteTask(objInvPurchaseEnt.taskID, objInvPurchaseEnt.strId)

                'process completed
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                    Exit Function
                End If

                ' set value to return variable
                InsertPayment = 1
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPayment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertPayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertPayment_Header
        '	Discription	    : Insert Payment_Header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPayment_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As Integer Implements IInvoice_PurchaseDao.InsertPayment_Header
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertPayment_Header = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine(" insert into payment_header         ")
                    .AppendLine(" (                                  ")
                    .AppendLine("   po_header_id                     ")
                    .AppendLine("   ,delivery_date                   ")
                    .AppendLine("   ,payment_date                    ")
                    .AppendLine("   ,invoice_no                      ")
                    .AppendLine("   ,account_type                    ")
                    .AppendLine("   ,account_no                      ")
                    .AppendLine("   ,account_name                    ")
                    .AppendLine("   ,total_delivery_amount           ")
                    .AppendLine("   ,remark                          ")
                    .AppendLine("   ,user_id ")
                    .AppendLine("   ,status_id ")
                    .AppendLine("   ,created_by ")
                    .AppendLine("   ,created_date ")
                    .AppendLine("   ,updated_by ")
                    .AppendLine("   ,updated_date ")
                    .AppendLine("   ,old_id ")
                    .AppendLine(" )      ")
                    .AppendLine(" VALUES             ")
                    .AppendLine(" (                  ")
                    .AppendLine("   ?po_header_id	 ")
                    .AppendLine("   ,?delivery_date ")
                    .AppendLine("   ,?payment_date ")
                    .AppendLine("   ,?invoice_no ")
                    .AppendLine("   ,?account_type ")
                    .AppendLine("   ,?account_no ")
                    .AppendLine("   ,?account_name ")
                    .AppendLine("   ,?total_delivery_amount ")
                    .AppendLine("   ,?remark ")
                    .AppendLine("   ,?user_id ")
                    .AppendLine("   ,1 ")
                    .AppendLine("   ,?created_by ")
                    .AppendLine("   ,date_format(now(),'%Y%m%d%H%i%s')	 ")
                    .AppendLine("   ,?created_by1 ")
                    .AppendLine("   ,date_format(now(),'%Y%m%d%H%i%s')	 ")
                    .AppendLine("	,?old_id		")
                    .AppendLine(" );					            ")
                End With

                'objConn.AddParameter("?payment_header_id", DeletePayment)
                objConn.AddParameter("?po_header_id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPO_header_id), DBNull.Value, objInvPurchaseEnt.strPO_header_id))
                objConn.AddParameter("?delivery_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strDeliveryDateFrom), DBNull.Value, objInvPurchaseEnt.strDeliveryDateFrom))
                objConn.AddParameter("?payment_date", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPaymentDateFrom), DBNull.Value, objInvPurchaseEnt.strPaymentDateFrom))
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strInvoice_start), DBNull.Value, objInvPurchaseEnt.strInvoice_start))
                objConn.AddParameter("?account_type", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strSearchType), DBNull.Value, objInvPurchaseEnt.strSearchType))
                objConn.AddParameter("?account_no", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strAccountNo), DBNull.Value, objInvPurchaseEnt.strAccountNo))
                objConn.AddParameter("?account_name", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strAccountName), DBNull.Value, objInvPurchaseEnt.strAccountName))
                objConn.AddParameter("?total_delivery_amount", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strTotal_Amount), DBNull.Value, objInvPurchaseEnt.strTotal_Amount))
                objConn.AddParameter("?remark", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strRemark), DBNull.Value, objInvPurchaseEnt.strRemark))
                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?created_by1", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?old_id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId) Or objInvPurchaseEnt.strId.Equals(objInvPurchaseEnt.strPO_header_id), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                InsertPayment_Header = intEff
            Catch ex As Exception
                InsertPayment_Header = -1
                ' write error log
                objLog.ErrorLog("InsertPayment_Header(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertPayment_Header(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
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
        Public Function InsertPayment_Detail( _
            ByVal strPo_header_id As Integer, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer Implements IInvoice_PurchaseDao.InsertPayment_Detail
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertPayment_Detail = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                'Dim j As Integer = 0
                Dim listID As New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
                'Dim id As Integer

                If Not IsNothing(dtPaymentDetail) AndAlso dtPaymentDetail.Rows.Count > 0 Then
                    'Loop insert detail into payment detail
                    For i As Integer = 0 To dtPaymentDetail.Rows.Count - 1
                        'insert into detail only input qty more than 0
                        If CDbl(dtPaymentDetail.Rows(i)("delivery_qty").ToString()) > 0 Then
                            strSql = New Text.StringBuilder
                            'id = GetID(strPo_header_id)

                            ' assign sql command
                            With strSql
                                .AppendLine(" insert into payment_detail         ")
                                .AppendLine(" (              ")
                                .AppendLine("   payment_header_id              ")
                                .AppendLine("   ,po_header_id ")
                                .AppendLine("   ,po_detail_id ")
                                .AppendLine("   ,delivery_qty ")
                                .AppendLine("   ,delivery_amount ")
                                .AppendLine("   ,stock_fg ")
                                .AppendLine("   ,created_by ")
                                .AppendLine("   ,created_date ")
                                .AppendLine("   ,updated_by ")
                                .AppendLine("   ,updated_date ")
                                .AppendLine(" )      ")
                                .AppendLine(" VALUES             ")
                                .AppendLine(" (                  ")
                                .AppendLine("   (select max(id) as id 	 ")
                                .AppendLine("       FROM payment_header 	 ")
                                .AppendLine("       WHERE 	 ")
                                .AppendLine("           po_header_id = ?po_header_id1 AND created_by = ?created_by1 )	 ")
                                '.AppendLine("   ,?payment_header_id ")
                                .AppendLine("   ,?po_header_id ")
                                .AppendLine("   ,?po_detail_id ")
                                .AppendLine("   ,?delivery_qty ")
                                .AppendLine("   ,?delivery_amount ")
                                .AppendLine("   ,?stock_fg ")
                                .AppendLine("   ,?created_by ")
                                .AppendLine("   ,date_format(now(),'%Y%m%d%H%i%s')	 ")
                                .AppendLine("   ,?created_by2 ")
                                .AppendLine("   ,date_format(now(),'%Y%m%d%H%i%s')	 ")
                                .AppendLine(" );					            ")
                            End With

                            Dim po_header_id As String = dtPaymentDetail.Rows(i)("po_header_id").ToString()
                            Dim po_detail_id As String = dtPaymentDetail.Rows(i)("po_detail_id").ToString()
                            Dim delivery_qty As String = dtPaymentDetail.Rows(i)("delivery_qty").ToString().Replace(",", "")
                            Dim delivery_amount As String = dtPaymentDetail.Rows(i)("delivery_amount").ToString().Replace(",", "")
                            Dim stock_fg As String = dtPaymentDetail.Rows(i)("stock_fg").ToString()

                            'objConn.AddParameter("?payment_header_id", IIf(String.IsNullOrEmpty(id), DBNull.Value, id))
                            objConn.AddParameter("?po_header_id1", strPo_header_id)
                            objConn.AddParameter("?created_by1", HttpContext.Current.Session("UserID"))

                            objConn.AddParameter("?po_header_id", IIf(String.IsNullOrEmpty(po_header_id), DBNull.Value, po_header_id))
                            objConn.AddParameter("?po_detail_id", IIf(String.IsNullOrEmpty(po_detail_id), DBNull.Value, po_detail_id))
                            objConn.AddParameter("?delivery_qty", IIf(String.IsNullOrEmpty(delivery_qty), DBNull.Value, delivery_qty))
                            objConn.AddParameter("?delivery_amount", IIf(String.IsNullOrEmpty(delivery_amount), DBNull.Value, delivery_amount))
                            objConn.AddParameter("?stock_fg", IIf(String.IsNullOrEmpty(stock_fg), DBNull.Value, stock_fg))
                            objConn.AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                            objConn.AddParameter("?created_by2", HttpContext.Current.Session("UserID"))

                            ' execute non query and keep row effect
                            intEff = objConn.ExecuteNonQuery(strSql.ToString)
                            If intEff < 1 Then
                                InsertPayment_Detail = 0
                                Exit Function
                            End If

                            'insert into stock
                            intEff = InsertStock(dtPaymentDetail, i, strPo_header_id)
                            If intEff < 0 Then
                                InsertPayment_Detail = 0
                                Exit Function
                            End If

                            'update stock case installment
                            'j = UpdateStock(dtPaymentDetail, i, strPo_header_id)
                        End If
                    Next

                Else
                    InsertPayment_Detail = 0
                    Exit Function
                End If

                ' set value to return variable
                InsertPayment_Detail = intEff
            Catch ex As Exception
                InsertPayment_Detail = -1
                ' write error log
                objLog.ErrorLog("InsertPayment_Detail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertPayment_Detail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: InsertStock
        '	Discription	    : Insert Stock
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertStock( _
            ByVal dtPaymentDetail As DataTable, _
            ByVal row As Integer, _
            ByVal strPo_header_id As Long _
        ) As Integer Implements IInvoice_PurchaseDao.InsertStock
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertStock = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0

                If Not IsNothing(dtPaymentDetail) AndAlso dtPaymentDetail.Rows.Count > 0 Then

                    Dim job_order As String = dtPaymentDetail.Rows(row)("job_order").ToString()
                    Dim item_id As String = dtPaymentDetail.Rows(row)("item_id").ToString()
                    Dim qty_in As String = dtPaymentDetail.Rows(row)("qty_in").ToString()
                    Dim qty_out As String = dtPaymentDetail.Rows(row)("qty_out").ToString()

                    If Not dtPaymentDetail.Rows(row)("base").ToString().Equals("") Then
                        If dtPaymentDetail.Rows(row)("remain_qty") = dtPaymentDetail.Rows(row)("quantity") Then
                            qty_in = Convert.ToString(Convert.ToDecimal(dtPaymentDetail.Rows(row)("quantity").ToString()) / Convert.ToDecimal(dtPaymentDetail.Rows(row)("base").ToString()))
                            qty_out = qty_in
                        Else
                            qty_in = "0"
                            qty_out = "0"
                        End If
                    End If
                    ' assign sql command
                    With strSql
                        .AppendLine(" insert into stock         ")
                        .AppendLine(" (              ")
                        .AppendLine("   job_order, po_no, invoice_no, ie_id, vendor_id, item_id, delivery_date ")
                        .AppendLine("   ,qty_in, qty_out, qty_adjust, balance, created_by, created_date, amount, payment_detail_id ")
                        .AppendLine(" )      ")
                        .AppendLine(" VALUES             ")
                        .AppendLine(" (                  ")
                        .AppendLine("   ?job_order, ?po_no, ?invoice_no, ?ie_id, ?vendor_id, ?item_id, ?delivery_date ")
                        .AppendLine("   ,?qty_in, ?qty_out, ?qty_adjust, ?balance, ?created_by, date_format(now(),'%Y%m%d%H%i%s'), ?amount ")
                        .AppendLine("   ,(select max(id) as id from payment_detail   ")
                        .AppendLine("   where payment_header_id = (select max(id)  ")
                        .AppendLine("       FROM payment_header 	 ")
                        .AppendLine("       WHERE po_header_id = ?po_header_id1 AND created_by = ?created_by1))	 ")
                        .Append(" );")
                    End With

                    Dim po_no As String = dtPaymentDetail.Rows(row)("po_no").ToString()
                    Dim invoice_no As String = dtPaymentDetail.Rows(row)("invoice_no").ToString()
                    Dim ie_id As String = dtPaymentDetail.Rows(row)("ie_id").ToString()
                    Dim vendor_id As String = dtPaymentDetail.Rows(row)("vendor_id").ToString()
                    Dim delivery_date As String = dtPaymentDetail.Rows(row)("delivery_date").ToString()
                    Dim qty_adjust As String = dtPaymentDetail.Rows(row)("qty_adjust").ToString()
                    Dim balance As String = dtPaymentDetail.Rows(row)("balance").ToString()
                    Dim delivery_amount As String = dtPaymentDetail.Rows(row)("delivery_amount").ToString().Replace(",", "")

                    objConn.AddParameter("?job_order", IIf(String.IsNullOrEmpty(job_order), DBNull.Value, job_order))
                    objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(po_no), DBNull.Value, po_no))
                    objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(invoice_no), DBNull.Value, invoice_no))
                    objConn.AddParameter("?ie_id", IIf(String.IsNullOrEmpty(ie_id), DBNull.Value, ie_id))
                    objConn.AddParameter("?vendor_id", IIf(String.IsNullOrEmpty(vendor_id), DBNull.Value, vendor_id))
                    objConn.AddParameter("?item_id", IIf(String.IsNullOrEmpty(item_id), DBNull.Value, item_id))
                    objConn.AddParameter("?delivery_date", IIf(String.IsNullOrEmpty(delivery_date), DBNull.Value, delivery_date))
                    objConn.AddParameter("?qty_in", IIf(String.IsNullOrEmpty(qty_in), DBNull.Value, qty_in))
                    objConn.AddParameter("?qty_out", IIf(String.IsNullOrEmpty(qty_out), DBNull.Value, qty_in))
                    objConn.AddParameter("?qty_adjust", IIf(String.IsNullOrEmpty(qty_adjust), DBNull.Value, qty_adjust))
                    objConn.AddParameter("?balance", IIf(String.IsNullOrEmpty(balance), DBNull.Value, balance))
                    objConn.AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?amount", IIf(String.IsNullOrEmpty(balance), DBNull.Value, delivery_amount))
                    objConn.AddParameter("?po_header_id1", strPo_header_id)
                    objConn.AddParameter("?created_by1", HttpContext.Current.Session("UserID"))

                    ' execute non query and keep row effect
                    intEff = objConn.ExecuteNonQuery(strSql.ToString)
                    If intEff < 1 Then
                        InsertStock = 0
                        Exit Function
                    End If

                Else
                    InsertStock = 0
                    Exit Function
                End If

                ' set value to return variable
                InsertStock = intEff
            Catch ex As Exception
                InsertStock = -1
                ' write error log
                objLog.ErrorLog("InsertStock(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertStock(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: deleteTask
        '	Discription	    : delete Task
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function deleteTask( _
            ByVal taskID As String, _
            ByVal id As String _
        ) As Integer Implements IInvoice_PurchaseDao.deleteTask
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            deleteTask = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0

                ' assign sql command
                With strSql
                    .AppendLine("		delete from task  				")
                    .AppendLine("		where 			                ")
                    .AppendLine("		    task = 'Create Invoice'   	")
                    .AppendLine("		and user_id = ?user_id 			")
                    If taskID <> "" Then
                        .AppendLine("		and id=?tskID		        ")
                    Else
                        .AppendLine("		and refkey=?id		        ")
                    End If

                End With

                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                If taskID <> "" Then
                    objConn.AddParameter("?tskID", taskID)
                Else
                    objConn.AddParameter("?id", id)
                End If

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                deleteTask = intEff
            Catch ex As Exception
                deleteTask = -1
                ' write error log
                objLog.ErrorLog("deleteTask(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("deleteTask(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: updPO_Header_Status
        '	Discription	    : update status in PO_Header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function updPO_Header_Status( _
            ByVal id As Integer _
        ) As Integer Implements IInvoice_PurchaseDao.updPO_Header_Status
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            updPO_Header_Status = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0

                ' assign sql command
                With strSql
                    .AppendLine("		update 				")
                    .AppendLine("			po_header A 			")
                    .AppendLine("		join 				")
                    .AppendLine("			(select 			")
                    .AppendLine("				po_header_id		")
                    .AppendLine("				,sum(ifnull(quantity,0)) orderqty		")
                    .AppendLine("			from 			")
                    .AppendLine("				po_detail A 		")
                    .AppendLine("			join 			")
                    .AppendLine("				po_header B 		")
                    .AppendLine("			on A.po_header_id=B.id			")
                    .AppendLine("			where status_id<>6 group by po_header_id			")
                    .AppendLine("			) B 			")
                    .AppendLine("		on A.id=B.po_header_id				")
                    .AppendLine("		join 				")
                    .AppendLine("			(select 			")
                    .AppendLine("				A.po_header_id			")
                    .AppendLine("				,sum(ifnull(delivery_qty,0)) deliveryqty			")
                    .AppendLine("			from 			")
                    .AppendLine("				payment_detail A join payment_header C on A.payment_header_id=C.id 		")
                    .AppendLine("			join po_header B  			")
                    .AppendLine("			on A.po_header_id=B.id				")
                    .AppendLine("			where C.status_id<>6 group by A.po_header_id		")
                    .AppendLine("			) C 			")
                    .AppendLine("		on A.id=C.po_header_id				")
                    .AppendLine("		set 				")
                    .AppendLine("			A.status_id = if(C.deliveryqty >= B.orderqty,7,2)			")
                    .AppendLine("		where 				")
                    .AppendLine("			C.deliveryqty > 0 		")
                    .AppendLine("		and A.id=?id			")
                End With

                objConn.AddParameter("?id", id)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                updPO_Header_Status = intEff
            Catch ex As Exception
                updPO_Header_Status = -1
                ' write error log
                objLog.ErrorLog("updPO_Header_Status(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("updPO_Header_Status(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentHeader
        '	Discription	    : Get Payment Header
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentHeader( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPaymentHeader
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentHeader = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 							")
                    .AppendLine("			pay_head.id						")
                    .AppendLine("			,CAST(pay_head.delivery_date AS DATE) AS delivery_date						")
                    .AppendLine("			,CAST(pay_head.payment_date AS DATE) AS payment_date						")
                    .AppendLine("			,pay_head.invoice_no						")
                    .AppendLine("			,pay_head.account_type						")
                    .AppendLine("			,pay_head.account_no						")
                    .AppendLine("			,pay_head.account_name						")
                    .AppendLine("			,pay_head.total_delivery_amount as delivery_amount						")
                    .AppendLine("			,pay_head.remark 						")
                    .AppendLine("		    ,po_head.vendor_id							")
                    .AppendLine("		from							")
                    .AppendLine("			payment_header pay_head						")
                    .AppendLine("		left JOIN							")
                    .AppendLine("			po_header po_head						")
                    .AppendLine("		ON							")
                    .AppendLine("			po_head.id = pay_head.po_header_id						")
                    .AppendLine("		where 							")
                    .AppendLine("			pay_head.id	 = ?id					")

                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .delivery_date = IIf(IsDBNull(dr.Item("delivery_date")), Nothing, dr.Item("delivery_date"))
                            .payment_date = IIf(IsDBNull(dr.Item("payment_date")), Nothing, dr.Item("payment_date"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .account_type = IIf(IsDBNull(dr.Item("account_type")), Nothing, dr.Item("account_type"))
                            .account_no = IIf(IsDBNull(dr.Item("account_no")), Nothing, dr.Item("account_no"))
                            .account_name = IIf(IsDBNull(dr.Item("account_name")), Nothing, dr.Item("account_name"))
                            .delivery_amount = IIf(IsDBNull(dr.Item("delivery_amount")), Nothing, dr.Item("delivery_amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                        End With
                        ' add Accounting to list
                        GetPaymentHeader.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentHeader(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentHeader(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: GetPaymentDetail
        '	Discription	    : Get Payment Detail
        '	Return Value	: List
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentDetail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseDao.GetPaymentDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentDetail = New List(Of Entity.ImpInvoice_PurchaseDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objInvPurchaseDetail As Entity.ImpInvoice_PurchaseDetailEntity

                With strSql
                    .AppendLine("		select 			")
                    .AppendLine("			pay_det.id		")
                    .AppendLine("			,item.name as item_name		")
                    .AppendLine("			,job_order		")
                    .AppendLine("			,ie.name as ie_name		")
                    .AppendLine("			,quantity		")
                    .AppendLine("			,amount		")
                    '.AppendLine("		    ,unit_price			")
                    .AppendLine("		    ,amount/quantity as unit_price 			")
                    .AppendLine("			,quantity-ifnull(sumDeliQty,0)+delivery_qty as remain_qty		")
                    .AppendLine("			,amount-ifnull(sumAmount,0)+delivery_amount as  remain_amt		")
                    .AppendLine("			,delivery_qty 		")
                    .AppendLine("			,delivery_amount as delivery_amt		")
                    .AppendLine("			,po_head.po_no		")
                    .AppendLine("			,po_det.ie_id		")
                    .AppendLine("			,po_head.vendor_id		")
                    .AppendLine("			,po_det.item_id		")
                    .AppendLine("			,if(INSTR(po_det.remark,'#')>0,REPLACE(substring_index(po_det.remark,'#',2),'#',''),'') base		")
                    .AppendLine("			,pay_det.po_detail_id		")
                    .AppendLine("		from 			")
                    .AppendLine("			payment_detail pay_det		")
                    .AppendLine("		join 			")
                    .AppendLine("			po_detail po_det 		")
                    .AppendLine("		on 			")
                    .AppendLine("			pay_det.po_detail_id=po_det.id 		")
                    .AppendLine("		and 			")
                    .AppendLine("			pay_det.po_header_id=po_det.po_header_id		")
                    .AppendLine("		left join 			")
                    .AppendLine("			mst_item item		")
                    .AppendLine("		on			")
                    .AppendLine("			po_det.item_id=item.id		")
                    .AppendLine("		left join 			")
                    .AppendLine("			mst_ie ie		")
                    .AppendLine("		on			")
                    .AppendLine("			po_det.ie_id=ie.id		")
                    .AppendLine("		join 			")
                    .AppendLine("			po_header po_head		")
                    .AppendLine("		on 			")
                    .AppendLine("			pay_det.po_header_id=po_head.id		")
                    .AppendLine("		left join (				")
                    .AppendLine("		 select 				")
                    .AppendLine("			A.po_header_id			")
                    .AppendLine("			,po_detail_id			")
                    .AppendLine("			,sum(delivery_qty) sumDeliQty			")
                    .AppendLine("		,sum(delivery_amount) sumAmount				")
                    .AppendLine("		from payment_detail	A			")
                    .AppendLine("       join ")
                    .AppendLine("       payment_header B ")
                    .AppendLine("       on A.payment_header_id=B.id ")
                    .AppendLine("		where B.status_id<>6			") '17
                    .AppendLine("		  group by A.po_header_id,A.po_detail_id	")
                    .AppendLine("		) sumDeli on sumDeli.po_header_id=po_det.po_header_id and sumDeli.po_detail_id=po_det.id				")
                    .AppendLine("		where 			")
                    .AppendLine("			pay_det.payment_header_id=?id		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                objLog.InfoLog("id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strId), DBNull.Value, objInvPurchaseEnt.strId))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objInvPurchaseDetail = New Entity.ImpInvoice_PurchaseDetailEntity
                        ' assign data from db to entity object
                        With objInvPurchaseDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .item_name = IIf(IsDBNull(dr.Item("item_name")), Nothing, dr.Item("item_name"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .ie_name = IIf(IsDBNull(dr.Item("ie_name")), Nothing, dr.Item("ie_name"))
                            .quantity = IIf(IsDBNull(dr.Item("quantity")), Nothing, dr.Item("quantity"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .unit_price = IIf(IsDBNull(dr.Item("unit_price")), Nothing, dr.Item("unit_price"))
                            .remain_qty = IIf(IsDBNull(dr.Item("remain_qty")), Nothing, dr.Item("remain_qty"))
                            .remain_amt = IIf(IsDBNull(dr.Item("remain_amt")), Nothing, dr.Item("remain_amt"))
                            .delivery_qty = IIf(IsDBNull(dr.Item("delivery_qty")), Nothing, dr.Item("delivery_qty"))
                            .delivery_amt = IIf(IsDBNull(dr.Item("delivery_amt")), Nothing, dr.Item("delivery_amt"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .ie_id = IIf(IsDBNull(dr.Item("ie_id")), Nothing, dr.Item("ie_id"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .item_id = IIf(IsDBNull(dr.Item("item_id")), Nothing, dr.Item("item_id"))
                            .base = IIf(IsDBNull(dr.Item("base")), Nothing, dr.Item("base"))
                            .po_detail_id = IIf(IsDBNull(dr.Item("po_detail_id")), Nothing, dr.Item("po_detail_id"))
                        End With
                        ' add Accounting to list
                        GetPaymentDetail.Add(objInvPurchaseDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdatePayment
        '	Discription	    : Update Payment Header,payment detail
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePayment( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer Implements IInvoice_PurchaseDao.UpdatePayment
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePayment = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()

                'Update satus of payment_header to 6:Del
                intEff = UpdatePayment_Header_Del(objInvPurchaseEnt)
                If intEff <= 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'insert payment header
                intEff = InsertPayment_Header(objInvPurchaseEnt)
                If intEff <= 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                'insert new payment detail
                intEff = InsertPayment_Detail(objInvPurchaseEnt.strPO_header_id, dtPaymentDetail)
                If intEff <= 0 Then
                    objConn.RollbackTrans()
                    Exit Function
                End If

                intEff = updPO_Header_Status(objInvPurchaseEnt.strPO_header_id)

                'process completed
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                UpdatePayment = 1
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePayment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdatePayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: UpdatePayment_Header_Del
        '	Discription	    : Update Payment_Header
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePayment_Header_Del( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As Integer Implements IInvoice_PurchaseDao.UpdatePayment_Header_Del
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdatePayment_Header_Del = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer = 0
                ' assign sql command
                With strSql
                    .AppendLine("		update 			")
                    .AppendLine("			payment_header 		")
                    .AppendLine("		set 			")
                    .AppendLine("			status_id = 6		")
                    .AppendLine("			,updated_by=?user_id		")
                    .AppendLine("			,updated_date=date_format(now(),'%Y%m%d%H%i%s')		")
                    .AppendLine("		where 			")
                    .AppendLine("			id=?id		")

                End With

                objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(objInvPurchaseEnt.strPO_header_id), DBNull.Value, objInvPurchaseEnt.strId))


                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                UpdatePayment_Header_Del = intEff
            Catch ex As Exception
                UpdatePayment_Header_Del = -1
                ' write error log
                objLog.ErrorLog("UpdatePayment_Header_Del(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdatePayment_Header_Del(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
                'Finally
                '    If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: checkConfirmPayment
        '	Discription	    : check permission of ConfirmPayment 
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkConfirmPayment() As Integer Implements IInvoice_PurchaseDao.checkConfirmPayment
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            checkConfirmPayment = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(user.id) AS used_count 		")
                    .AppendLine("		FROM user 				")
                    .AppendLine("		join mst_department mst				")
                    .AppendLine("		on user.department_id=mst.id  				")
                    .AppendLine("		WHERE user.id = ?loginid				")
                    .AppendLine("		AND mst.name = 'Accounting'		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?loginid", HttpContext.Current.Session("UserID"))

                ' execute sql command
                checkConfirmPayment = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkConfirmPayment(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("checkConfirmPayment(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
        '/**************************************************************
        '	Function name	: checkInvoice
        '	Discription	    : check Invoice 
        '	Return Value	: Integer
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkInvoice(ByVal vendor_id As String, _
                                     ByVal invoice_no As String, _
                                     ByVal id As String _
        ) As Integer Implements IInvoice_PurchaseDao.checkInvoice
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            checkInvoice = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		select 							")
                    .AppendLine("			count(*)						")
                    .AppendLine("		from 							")
                    .AppendLine("			payment_header A 						")
                    .AppendLine("		left join po_header B 							")
                    .AppendLine("		on A.po_header_id=B.id 							")
                    .AppendLine("		where A.status_id<>6 							")
                    .AppendLine("		and A.invoice_no=?invoice_no 							")
                    .AppendLine("		and B.vendor_id=?vendor_id						")
                    .AppendLine("		and A.id<>?id						")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?invoice_no", invoice_no)
                objConn.AddParameter("?vendor_id", vendor_id)
                objConn.AddParameter("?id", id)

                ' execute sql command
                checkInvoice = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkInvoice(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("checkInvoice(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace


