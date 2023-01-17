#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IInvoice_PurchaseEntity
'	Class Discription	: Interface of Invoice Purchase
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

#End Region

Namespace Entity
    Public Interface IInvoice_PurchaseEntity
        'Receive value from screen search
        Property strSearchType() As String
        Property strPO() As String
        Property strDeliveryDateFrom() As String
        Property strDeliveryDateTo() As String
        Property strPaymentDateFrom() As String
        Property strPaymentDateTo() As String
        Property strVendor_name() As String
        Property strInvoice_start() As String
        Property strInvoice_end() As String
        Property strId() As String

        'insert parameter
        Property strPO_header_id() As String
        Property strAccountNo() As String
        Property strAccountName() As String
        Property strTotal_Amount() As String
        Property strRemark() As String
        Property strVat_Amount() As String

        'Item from Payment_Header
        Property id() As String
        Property po_header_id() As String
        Property delivery_date() As String
        Property payment_date() As String
        Property invoice_no() As String
        Property account_type() As String
        Property account_no() As String
        Property account_name() As String
        Property total_delivery_amount() As String
        Property remark() As String
        Property user_id() As String
        Property status_id() As String
        Property created_by() As String
        Property created_date() As String
        Property updated_by() As String
        Property updated_date() As String
        Property issue_date() As String

        'Item from payment_detail
        Property payment_header_id() As String
        Property po_detail_id() As String
        Property delivery_qty() As String
        Property delivery_amount() As String
        Property stock_fg() As String

        Property strPOFrom() As String
        Property strPOTo() As String
        Property strIssueDateFrom() As String
        Property strIssueDateTo() As String

        Property vendor_id() As String
        Property quotation_no() As String
        Property discount_total() As String
        Property total_amount() As String
        Property attn() As String
        Property deliver_to() As String
        Property contact() As String
        Property payment_term() As String
        Property currency_Name() As String
        Property item_id() As String
        Property ie_id() As String
        Property unit_price() As String
        Property unit_id() As String
        Property discount() As String
        Property vat_amount() As String
        Property wt_amount() As String
        Property item_name() As String
        Property ie_name() As String
        Property unit_name() As String
        Property discount_type() As String
        Property payment_term_id() As String
        Property vat_id() As String
        Property wt_id() As String
        Property currency_id() As String
        Property taskID() As String
        Property strMode() As String

        Function GetInvoice_PurchaseList( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetInvoice_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetInvoice_Detail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function CountVendor_rating(ByVal intItemID As Integer) As Integer
        Function Countaccounting(ByVal intItemID As Integer) As Integer
        Function DeletePayment(ByVal intId As Integer) As Integer
        Function ExecuteAccounting(ByVal itemConfirm As String) As Boolean
        Function GetPurchasePaidReport(ByVal itemConfirm As String _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_List( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)
        Function GetPO_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_Detail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)
        Function GetPO_Detail_Insert( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)
        Function GetPO_Header_Insert( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function InsertPayment( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer

        Function GetPaymentHeader( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPaymentDetail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function UpdatePayment( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer
        Function checkConfirmPayment() As Integer
        Function checkInvoice(ByVal vendor_id As String, ByVal invoice_no As String, ByVal id As String) As Integer
    End Interface
End Namespace

