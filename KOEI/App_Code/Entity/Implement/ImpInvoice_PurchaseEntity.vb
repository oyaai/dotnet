'Imports Microsoft.VisualBasic

'Public Class ImpInvoice_PurchaseEntity

'End Class
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpInvoice_PurchaseEntity
'	Class Discription	: Class of table accounting
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Class ImpInvoice_PurchaseEntity
        Implements IInvoice_PurchaseEntity

        'Receive value from screen search
        Private _strSearchType As String
        Private _strPO As String
        Private _strDeliveryDateFrom As String
        Private _strDeliveryDateTo As String
        Private _strPaymentDateFrom As String
        Private _strPaymentDateTo As String
        Private _strVendor_name As String
        Private _strInvoice_start As String
        Private _strInvoice_end As String
        Private _strMode As String

        'Receive value from detail screen
        Private _strId As String

        Private _strPO_header_id As String
        Private _strAccountNo As String
        Private _strAccountName As String
        Private _strTotal_Amount As String
        Private _strRemark As String
        Private _strVat_Amount As String

        'Item from Payment_Header
        Private _id As String
        Private _po_header_id As String
        Private _delivery_date As String
        Private _payment_date As String
        Private _invoice_no As String
        Private _account_type As String
        Private _account_no As String
        Private _account_name As String
        Private _total_delivery_amount As String
        Private _remark As String
        Private _user_id As String
        Private _status_id As String
        Private _created_by As String
        Private _created_date As String
        Private _updated_by As String
        Private _updated_date As String
        Private _issue_date As String

        'Item payment_detail
        Private _payment_header_id As String
        Private _po_detail_id As String
        Private _delivery_qty As String
        Private _delivery_amount As String
        Private _stock_fg As String

        Private _strPOFrom As String
        Private _strPOTo As String
        Private _strIssueDateFrom As String
        Private _strIssueDateTo As String

        Private _vendor_id As String
        Private _quotation_no As String
        Private _discount_total As String
        Private _vat_amount As String
        Private _wt_amount As String
        Private _total_amount As String
        Private _attn As String
        Private _deliver_to As String
        Private _contact As String
        Private _payment_term As String
        Private _currency_Name As String
        Private _item_id As String
        Private _ie_id As String
        Private _unit_price As String
        Private _unit_id As String
        Private _discount As String
        Private _discount_type As String
        Private _item_name As String
        Private _ie_name As String
        Private _unit_name As String
        Private _payment_term_id As String
        Private _vat_id As String
        Private _wt_id As String
        Private _currency_id As String
        Private _taskID As String
        Private objInvPurchase As New Dao.ImpInvoice_PurchaseDao

#Region "Function"
        Public Function GetInvoice_PurchaseList( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetInvoice_PurchaseList
            Return objInvPurchase.GetInvoice_PurchaseList(objInvPurchaseEnt)
        End Function

        Public Function GetInvoice_Header( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetInvoice_Header
            Return objInvPurchase.GetInvoice_Header(objInvPurchaseEnt)
        End Function

        Public Function GetInvoice_Detail( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetInvoice_Detail
            Return objInvPurchase.GetInvoice_Detail(objInvPurchaseEnt)
        End Function

        Public Function CountVendor_rating(ByVal intItemID As Integer) As Integer Implements IInvoice_PurchaseEntity.CountVendor_rating
            Return objInvPurchase.CountVendor_rating(intItemID)
        End Function
        Public Function CountAccounting(ByVal intItemID As Integer) As Integer Implements IInvoice_PurchaseEntity.Countaccounting
            Return objInvPurchase.CountAccounting(intItemID)
        End Function

        Public Function DeletePayment( _
            ByVal id As Integer _
        ) As Integer Implements IInvoice_PurchaseEntity.DeletePayment
            Return objInvPurchase.DeletePayment(id)
        End Function
        Public Function ExecuteAccounting( _
            ByVal itemConfirm As String _
        ) As Boolean Implements IInvoice_PurchaseEntity.ExecuteAccounting
            Return objInvPurchase.ExecuteAccounting(itemConfirm)
        End Function
        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPurchasePaidReport
            Return objInvPurchase.GetPurchasePaidReport(itemConfirm)
        End Function
        Public Function GetPO_List( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPO_List
            Return objInvPurchase.GetPO_List(objInvPurchaseEnt)
        End Function
        Public Function GetPO_Header( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPO_Header
            Return objInvPurchase.GetPO_Header(objInvPurchaseEnt)
        End Function

        Public Function GetPO_Detail( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPO_Detail
            Return objInvPurchase.GetPO_Detail(objInvPurchaseEnt)
        End Function
        Public Function GetPO_Detail_Insert( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPO_Detail_Insert
            Return objInvPurchase.GetPO_Detail_Insert(objInvPurchaseEnt)
        End Function
        Public Function GetPO_Header_Insert( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPO_Header_Insert
            Return objInvPurchase.GetPO_Header_Insert(objInvPurchaseEnt)
        End Function
        Public Function InsertPayment( _
            ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer Implements IInvoice_PurchaseEntity.InsertPayment
            Return objInvPurchase.InsertPayment(objInvPurchaseEnt, dtPaymentDetail)
        End Function
        Public Function GetPaymentHeader( _
           ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
       ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPaymentHeader
            Return objInvPurchase.GetPaymentHeader(objInvPurchaseEnt)
        End Function
        Public Function GetPaymentDetail( _
           ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity _
       ) As System.Collections.Generic.List(Of ImpInvoice_PurchaseDetailEntity) Implements IInvoice_PurchaseEntity.GetPaymentDetail
            Return objInvPurchase.GetPaymentDetail(objInvPurchaseEnt)
        End Function
        Public Function UpdatePayment( _
           ByVal objInvPurchaseEnt As IInvoice_PurchaseEntity, _
           ByVal dtPaymentDetail As DataTable _
       ) As Integer Implements IInvoice_PurchaseEntity.UpdatePayment
            Return objInvPurchase.UpdatePayment(objInvPurchaseEnt, dtPaymentDetail)
        End Function
        Public Function checkConfirmPayment() As Integer Implements IInvoice_PurchaseEntity.checkConfirmPayment
            Return objInvPurchase.checkConfirmPayment()
        End Function
        Public Function checkInvoice(ByVal vendor_id As String, ByVal invoice_no As String, ByVal id As String) As Integer Implements IInvoice_PurchaseEntity.checkInvoice
            Return objInvPurchase.checkInvoice(vendor_id, invoice_no, id)
        End Function
#End Region

#Region "Property"
        Public Property strMode() As String Implements IInvoice_PurchaseEntity.strMode
            Get
                Return _strMode
            End Get
            Set(ByVal value As String)
                _strMode = value
            End Set
        End Property
        Public Property taskID() As String Implements IInvoice_PurchaseEntity.taskID
            Get
                Return _taskID
            End Get
            Set(ByVal value As String)
                _taskID = value
            End Set
        End Property
        Public Property strPO_header_id() As String Implements IInvoice_PurchaseEntity.strPO_header_id
            Get
                Return _strPO_header_id
            End Get
            Set(ByVal value As String)
                _strPO_header_id = value
            End Set
        End Property
        Public Property strAccountNo() As String Implements IInvoice_PurchaseEntity.strAccountNo
            Get
                Return _strAccountNo
            End Get
            Set(ByVal value As String)
                _strAccountNo = value
            End Set
        End Property
        Public Property strAccountName() As String Implements IInvoice_PurchaseEntity.strAccountName
            Get
                Return _strAccountName
            End Get
            Set(ByVal value As String)
                _strAccountName = value
            End Set
        End Property
        Public Property strTotal_Amount() As String Implements IInvoice_PurchaseEntity.strTotal_Amount
            Get
                Return _strTotal_Amount
            End Get
            Set(ByVal value As String)
                _strTotal_Amount = value
            End Set
        End Property
        Public Property strVAT_Amount() As String Implements IInvoice_PurchaseEntity.strVAT_Amount
            Get
                Return _strVAT_Amount
            End Get
            Set(ByVal value As String)
                _strVAT_Amount = value
            End Set
        End Property
        Public Property strRemark() As String Implements IInvoice_PurchaseEntity.strRemark
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
            End Set
        End Property
        Public Property payment_term_id() As String Implements IInvoice_PurchaseEntity.payment_term_id
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As String)
                _payment_term_id = value
            End Set
        End Property
        Public Property vat_id() As String Implements IInvoice_PurchaseEntity.vat_id
            Get
                Return _vat_id
            End Get
            Set(ByVal value As String)
                _vat_id = value
            End Set
        End Property
        Public Property wt_id() As String Implements IInvoice_PurchaseEntity.wt_id
            Get
                Return _wt_id
            End Get
            Set(ByVal value As String)
                _wt_id = value
            End Set
        End Property
        Public Property currency_id() As String Implements IInvoice_PurchaseEntity.currency_id
            Get
                Return _currency_id
            End Get
            Set(ByVal value As String)
                _currency_id = value
            End Set
        End Property
        Public Property vendor_id() As String Implements IInvoice_PurchaseEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As String)
                _vendor_id = value
            End Set
        End Property
        Public Property quotation_no() As String Implements IInvoice_PurchaseEntity.quotation_no
            Get
                Return _quotation_no
            End Get
            Set(ByVal value As String)
                _quotation_no = value
            End Set
        End Property
        Public Property discount_total() As String Implements IInvoice_PurchaseEntity.discount_total
            Get
                Return _discount_total
            End Get
            Set(ByVal value As String)
                _discount_total = value
            End Set
        End Property
        Public Property total_amount() As String Implements IInvoice_PurchaseEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As String)
                _total_amount = value
            End Set
        End Property
        Public Property attn() As String Implements IInvoice_PurchaseEntity.attn
            Get
                Return _attn
            End Get
            Set(ByVal value As String)
                _attn = value
            End Set
        End Property
        Public Property deliver_to() As String Implements IInvoice_PurchaseEntity.deliver_to
            Get
                Return _deliver_to
            End Get
            Set(ByVal value As String)
                _deliver_to = value
            End Set
        End Property
        Public Property contact() As String Implements IInvoice_PurchaseEntity.contact
            Get
                Return _contact
            End Get
            Set(ByVal value As String)
                _contact = value
            End Set
        End Property
        Public Property payment_term() As String Implements IInvoice_PurchaseEntity.payment_term
            Get
                Return _payment_term
            End Get
            Set(ByVal value As String)
                _payment_term = value
            End Set
        End Property
        Public Property currency_Name() As String Implements IInvoice_PurchaseEntity.currency_Name
            Get
                Return _currency_Name
            End Get
            Set(ByVal value As String)
                _currency_Name = value
            End Set
        End Property
        Public Property item_id() As String Implements IInvoice_PurchaseEntity.item_id
            Get
                Return _item_id
            End Get
            Set(ByVal value As String)
                _item_id = value
            End Set
        End Property
        Public Property ie_id() As String Implements IInvoice_PurchaseEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As String)
                _ie_id = value
            End Set
        End Property
        Public Property unit_price() As String Implements IInvoice_PurchaseEntity.unit_price
            Get
                Return _unit_price
            End Get
            Set(ByVal value As String)
                _unit_price = value
            End Set
        End Property
        Public Property unit_id() As String Implements IInvoice_PurchaseEntity.unit_id
            Get
                Return _unit_id
            End Get
            Set(ByVal value As String)
                _unit_id = value
            End Set
        End Property
        Public Property discount() As String Implements IInvoice_PurchaseEntity.discount
            Get
                Return _discount
            End Get
            Set(ByVal value As String)
                _discount = value
            End Set
        End Property
        Public Property vat_amount() As String Implements IInvoice_PurchaseEntity.vat_amount
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As String)
                _vat_amount = value
            End Set
        End Property
        Public Property wt_amount() As String Implements IInvoice_PurchaseEntity.wt_amount
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As String)
                _wt_amount = value
            End Set
        End Property
        Public Property item_name() As String Implements IInvoice_PurchaseEntity.item_name
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
            End Set
        End Property
        Public Property ie_name() As String Implements IInvoice_PurchaseEntity.ie_name
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property
        Public Property unit_name() As String Implements IInvoice_PurchaseEntity.unit_name
            Get
                Return _unit_name
            End Get
            Set(ByVal value As String)
                _unit_name = value
            End Set
        End Property
        Public Property discount_type() As String Implements IInvoice_PurchaseEntity.discount_type
            Get
                Return _discount_type
            End Get
            Set(ByVal value As String)
                _discount_type = value
            End Set
        End Property
        Public Property issue_date() As String Implements IInvoice_PurchaseEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property
        Public Property strPOFrom() As String Implements IInvoice_PurchaseEntity.strPOFrom
            Get
                Return _strPOFrom
            End Get
            Set(ByVal value As String)
                _strPOFrom = value
            End Set
        End Property
        Public Property strPOTo() As String Implements IInvoice_PurchaseEntity.strPOTo
            Get
                Return _strPOTo
            End Get
            Set(ByVal value As String)
                _strPOTo = value
            End Set
        End Property
        Public Property strIssueDateFrom() As String Implements IInvoice_PurchaseEntity.strIssueDateFrom
            Get
                Return _strIssueDateFrom
            End Get
            Set(ByVal value As String)
                _strIssueDateFrom = value
            End Set
        End Property
        Public Property strIssueDateTo() As String Implements IInvoice_PurchaseEntity.strIssueDateTo
            Get
                Return _strIssueDateTo
            End Get
            Set(ByVal value As String)
                _strIssueDateTo = value
            End Set
        End Property
        Public Property strSearchType() As String Implements IInvoice_PurchaseEntity.strSearchType
            Get
                Return _strSearchType
            End Get
            Set(ByVal value As String)
                _strSearchType = value
            End Set
        End Property
        Public Property strPO() As String Implements IInvoice_PurchaseEntity.strPO
            Get
                Return _strPO
            End Get
            Set(ByVal value As String)
                _strPO = value
            End Set
        End Property
        Public Property strDeliveryDateFrom() As String Implements IInvoice_PurchaseEntity.strDeliveryDateFrom
            Get
                Return _strDeliveryDateFrom
            End Get
            Set(ByVal value As String)
                _strDeliveryDateFrom = value
            End Set
        End Property
        Public Property strDeliveryDateTo() As String Implements IInvoice_PurchaseEntity.strDeliveryDateTo
            Get
                Return _strDeliveryDateTo
            End Get
            Set(ByVal value As String)
                _strDeliveryDateTo = value
            End Set
        End Property
        Public Property strPaymentDateFrom() As String Implements IInvoice_PurchaseEntity.strPaymentDateFrom
            Get
                Return _strPaymentDateFrom
            End Get
            Set(ByVal value As String)
                _strPaymentDateFrom = value
            End Set
        End Property
        Public Property strPaymentDateTo() As String Implements IInvoice_PurchaseEntity.strPaymentDateTo
            Get
                Return _strPaymentDateTo
            End Get
            Set(ByVal value As String)
                _strPaymentDateTo = value
            End Set
        End Property
        Public Property strVendor_name() As String Implements IInvoice_PurchaseEntity.strVendor_name
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Public Property strInvoice_start() As String Implements IInvoice_PurchaseEntity.strInvoice_start
            Get
                Return _strInvoice_start
            End Get
            Set(ByVal value As String)
                _strInvoice_start = value
            End Set
        End Property
        Public Property strInvoice_end() As String Implements IInvoice_PurchaseEntity.strInvoice_end
            Get
                Return _strInvoice_end
            End Get
            Set(ByVal value As String)
                _strInvoice_end = value
            End Set
        End Property
        Public Property strId() As String Implements IInvoice_PurchaseEntity.strId
            Get
                Return _strId
            End Get
            Set(ByVal value As String)
                _strId = value
            End Set
        End Property
        Public Property id() As String Implements IInvoice_PurchaseEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property po_header_id() As String Implements IInvoice_PurchaseEntity.po_header_id
            Get
                Return _po_header_id
            End Get
            Set(ByVal value As String)
                _po_header_id = value
            End Set
        End Property
        Public Property delivery_date() As String Implements IInvoice_PurchaseEntity.delivery_date
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property
        Public Property payment_date() As String Implements IInvoice_PurchaseEntity.payment_date
            Get
                Return _payment_date
            End Get
            Set(ByVal value As String)
                _payment_date = value
            End Set
        End Property
        Public Property invoice_no() As String Implements IInvoice_PurchaseEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property
        Public Property account_type() As String Implements IInvoice_PurchaseEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As String)
                _account_type = value
            End Set
        End Property
        Public Property account_no() As String Implements IInvoice_PurchaseEntity.account_no
            Get
                Return _account_no
            End Get
            Set(ByVal value As String)
                _account_no = value
            End Set
        End Property
        Public Property account_name() As String Implements IInvoice_PurchaseEntity.account_name
            Get
                Return _account_name
            End Get
            Set(ByVal value As String)
                _account_name = value
            End Set
        End Property
        Public Property total_delivery_amount() As String Implements IInvoice_PurchaseEntity.total_delivery_amount
            Get
                Return _total_delivery_amount
            End Get
            Set(ByVal value As String)
                _total_delivery_amount = value
            End Set
        End Property
        Public Property remark() As String Implements IInvoice_PurchaseEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property
        Public Property user_id() As String Implements IInvoice_PurchaseEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As String)
                _user_id = value
            End Set
        End Property
        Public Property status_id() As String Implements IInvoice_PurchaseEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As String)
                _status_id = value
            End Set
        End Property
        Public Property created_by() As String Implements IInvoice_PurchaseEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As String)
                _created_by = value
            End Set
        End Property
        Public Property created_date() As String Implements IInvoice_PurchaseEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property
        Public Property updated_by() As String Implements IInvoice_PurchaseEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As String)
                _updated_by = value
            End Set
        End Property
        Public Property updated_date() As String Implements IInvoice_PurchaseEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property
        Public Property payment_header_id() As String Implements IInvoice_PurchaseEntity.payment_header_id
            Get
                Return _payment_header_id
            End Get
            Set(ByVal value As String)
                _payment_header_id = value
            End Set
        End Property
        Public Property po_detail_id() As String Implements IInvoice_PurchaseEntity.po_detail_id
            Get
                Return _po_detail_id
            End Get
            Set(ByVal value As String)
                _po_detail_id = value
            End Set
        End Property
        Public Property delivery_qty() As String Implements IInvoice_PurchaseEntity.delivery_qty
            Get
                Return _delivery_qty
            End Get
            Set(ByVal value As String)
                _delivery_qty = value
            End Set
        End Property
        Public Property delivery_amount() As String Implements IInvoice_PurchaseEntity.delivery_amount
            Get
                Return _delivery_amount
            End Get
            Set(ByVal value As String)
                _delivery_amount = value
            End Set
        End Property
        Public Property stock_fg() As String Implements IInvoice_PurchaseEntity.stock_fg
            Get
                Return _stock_fg
            End Get
            Set(ByVal value As String)
                _stock_fg = value
            End Set
        End Property
#End Region


    End Class
End Namespace


