#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ISale_InvoiceEntity
'	Class Discription	: Interface of table receive_header
'	Create User 		: Suwishaya L.
'	Create Date		    : 28-06-2013
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
#End Region

Namespace Entity
    Public Interface ISale_InvoiceEntity

#Region "Property"
        Property id() As Integer
        Property name() As String
        Property invoice_no() As String
        Property receipt_date() As String
        Property ie_id() As Integer
        Property vendor_id() As Integer
        Property account_type() As Integer
        Property invoice_type() As Integer
        Property bank_fee() As String
        Property total_amount() As Decimal
        Property user_id() As Integer
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
        Property issue_date() As String
        Property customer() As Integer
        Property job_order_id() As Integer
        Property currency_id() As Integer
        Property po_type() As Integer
        Property po_date() As String
        Property receive_header_id() As Integer
        Property receive_detail_id() As Integer
        Property vat_id() As Integer
        Property wt_id() As Integer
        Property hontai_type() As String
        Property job_order_po_id() As Integer
        Property hontai_fg() As String
        Property account_next_approve() As String
        Property strJobOrder1() As String
        Property strJobOrder2() As String
        Property strJobOrder3() As String
        Property status() As String
        Property hontai_fg1() As String
        Property hontai_fg2() As String
        Property hontai_fg3() As String
        Property hontai_cond() As String
        Property job_type() As String
        Property po_fg() As String
        Property vat_amount() As Decimal
        Property wt_amount() As Decimal
        Property sub_total() As Decimal
        Property remark_detail() As String

        'Receive data from screen (condition search)
        Property invoice_no_search() As String
        Property invoice_type_search() As String
        Property issue_date_from_search() As String
        Property issue_date_to_search() As String
        Property customer_search() As String
        Property receipt_date_from_search() As String
        Property receipt_date_to_search() As String
        Property job_order_from_search() As String
        Property job_order_to_search() As String

        'Receive data for detail search
        Property invoice_type_name() As String
        Property customer_name() As String
        Property amount() As String
        Property account_title() As String
        Property account_type_name() As String
        Property job_order() As String
        Property po_type_name() As String
        Property hontai() As String
        Property po_no() As String
        Property vat() As String
        Property wt() As String
        Property remark() As String
        Property sum_amount() As Decimal
        Property sum_vat() As Decimal
        Property sum_wt() As Decimal
        Property sum_price() As Decimal
        Property stage() As String
        Property percent() As String
        Property actual_rate() As String
        Property price() As String
        Property bank_rate() As String
        Property actual_amount() As String
        Property currency() As String
        Property total_invoice_amount() As String

#End Region

#Region "Function"
        Function GetAccountTitleForList() As List(Of Entity.ISale_InvoiceEntity)
        Function CountUsedInAccounting(ByVal intRefID As Integer) As Integer
        Function CountUsedInReceiveHeader(ByVal strInvoice_no As String) As Integer
        Function CountUsedInJobOrder(ByVal strJobOrder As String) As Integer
        Function CountUsedInHontai(ByVal intJobOrderId As Integer, ByVal intHontaiFlg As Integer) As Integer
        Function CountCustomerUsedInJobOrder(ByVal strJobOrder As String, ByVal intCustomer As Integer) As Integer
        Function GetSaleInvoiceList(ByVal objSale_InvoicEnt As Entity.ISale_InvoiceEntity) As List(Of Entity.ImpSale_InvoiceEntity)
        Function DeleteSaleInvoice(ByVal intRefID As Integer, ByVal dtValues As System.Data.DataTable) As Integer
        Function SaveSaleInvoice(ByVal intRefID As Integer, ByVal decExchangeRate As Decimal, ByVal decActualAmount As Decimal, ByVal strJobOrder As String) As Integer
        Function GetSaleInvoiceHeaderByID(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceDetailByID(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceByJobOrder(ByVal strJobOrder As String) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceDetailList(ByVal intRefID As Integer) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSaleInvoiceHeaderList(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceReportList(ByVal objSale_InvoicEnt As Entity.ISale_InvoiceEntity) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSumSaleInvoiceReportList(ByVal objSale_InvoicEnt As Entity.ISale_InvoiceEntity) As Entity.ISale_InvoiceEntity
        Function GetTotalSaleInvoiceAmount(ByVal objSale_InvoicEnt As Entity.ISale_InvoiceEntity) As Entity.ISale_InvoiceEntity
        Function ConfirmReceive(ByVal invoiceHeaderId As String, ByVal dtValues As System.Data.DataTable, Optional ByVal dtBankFree As System.Data.DataTable = Nothing) As Integer
        Function GetJobOrerSaleInvoiceDetail(ByVal strJobOrder As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetJobOrerSaleInvoiceDetailEdit(ByVal intRefID As Integer) As List(Of Entity.ImpSale_InvoiceEntity)
        Function InsertSaleInvoice(ByVal strJobOrder1 As String, ByVal strJobOrder2 As String, ByVal strJobOrder3 As String, ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer
        Function UpdateSaleInvoice(ByVal strIdDelete As String, ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer
        Function GetJobOrderHontai(ByVal dtValues As System.Data.DataTable) As Entity.ISale_InvoiceEntity
        Function GetHontaiFinish(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function UpdateJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer
        Function DeleteJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer
        Function GetDataBankFreeList(ByVal strReceive_header_id As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetDataReceiveDetail(ByVal strReceive_header_id As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetConfirmReceiveForReport(ByVal strReceiceHeaderId As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSumConfirmReport(ByVal strReceiceHeaderId As String) As Entity.ISale_InvoiceEntity
        Function DelInvReceiveDetail(ByVal strIntID As Integer) As Integer
        Function UpdateReciveDetail(ByVal tbIntIdJBPO As System.Data.DataTable, ByVal IntIDSaleInv As Dao.ISale_InvoiceDao, ByVal intJobOrderID As Integer) As Boolean
        Function SumBankfeeConfirmReport(ByVal strReceiceHeaderId As String) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceforUpdate(ByVal strReceiceHeaderId As String) As Entity.ISale_InvoiceEntity
        Function UpExChangeRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean
        Function GetActualRate(ByVal intID As Integer) As Integer
        Function UpdateExChangeRate(ByVal intID As Integer, ByVal objExChangeRate As System.Data.DataTable) As Boolean

#End Region

    End Interface
End Namespace
