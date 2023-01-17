#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : PurchaseDto
'	Class Discription	: Dto class Purchase
'	Create User 		: Boonyarit
'	Create Date		    : 11-06-2013
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

Namespace Dto
    Public Class PurchaseDto
        'Use in page KTPU01 
        Private _id As Integer
        Private _po_type As Integer
        'Type of Purchase  	'po_header.po_type
        Private _po_type_text As String
        'PO No.				'po_header.po_no
        Private _po_no As String
        'Vendor Name		'po_header.vendor_id -> mst_vendor.name
        Private _vendor_id As Integer
        Private _vendor_branch_id As Integer
        Private _vendor_name As String
        Private _vendor_branch_name As String
        Private _issue_date As String
        'Delivery Date Plan	'po_header.delivery_date
        Private _delivery_date As String
        Private _sum_amount As Decimal
        Private _status_id As Integer
        Private _status As String
        Private _delivery_fg As Integer

        'Use in page KTPU01_Detail
        'Quotation No.		'po_header.quotation_no
        Private _quotation_no As String
        'Payment Term		'po_header.payment_term_id -> mst_payment_term.term_day
        Private _payment_term_id As Integer
        Private _payment_term_name As String
        'Vat				'po_header.vat_id -> mst_vat.percent
        Private _vat_id As Integer
        Private _vat_name As String
        'W/T				'po_header.wt_id -> mst_wt.percent
        Private _wt_id As Integer
        Private _wt_name As String
        'Currency			'po_header.currency_id -> mst_currentcy.name
        Private _currency_id As Integer
        Private _currency_name As String
        'Remarks			'po_header.remark
        Private _remarks As String
        'ATTN				'po_header.attn
        Private _attn As String
        'Deliver To			'po_header.deliver_to
        Private _deliver_to As String
        'Contact			'po_header.contact
        Private _contact As String
        'Sub Total			'po_header.sub_total
        Private _sub_total As Decimal
        'Discount Total		'po_header.discount_total
        Private _discount_total As Decimal
        'Vat Amount			'po_header.vat_amount
        Private _vat_amount As Decimal
        'W/T Amount			'po_header.wt_amount
        Private _wt_amount As Decimal
        'Total Amount		'po_header.total_amount
        Private _total_amount As Decimal
        Private _user_id As Integer
        Private _schedule_rate As Decimal
        Private _approve_user As Integer

        Private _thb_sub_total As Decimal
        Private _thb_discount_total As Decimal
        Private _thb_vat_amount As Decimal
        Private _thb_wt_amount As Decimal
        Private _thb_total_amount As Decimal

        'List of Detail purchase
        Private _purchase_detail As List(Of Dto.PurchaseDetailDto)
        'Temp of Detail purchase
        Private _purchase_detail_tmp As Dto.PurchaseTemp

#Region "Property"
        Public Property purchase_detail() As List(Of Dto.PurchaseDetailDto)
            Get
                Return _purchase_detail
            End Get
            Set(ByVal value As List(Of Dto.PurchaseDetailDto))
                _purchase_detail = value
            End Set
        End Property

        Public Property purchase_detail_tmp() As Dto.PurchaseTemp
            Get
                Return _purchase_detail_tmp
            End Get
            Set(ByVal value As Dto.PurchaseTemp)
                _purchase_detail_tmp = value
            End Set
        End Property

        Public Property approve_user() As Integer
            Get
                Return _approve_user
            End Get
            Set(ByVal value As Integer)
                _approve_user = value
            End Set
        End Property

        Public Property thb_total_amount() As Decimal
            Get
                Return _thb_total_amount
            End Get
            Set(ByVal value As Decimal)
                _thb_total_amount = value
            End Set
        End Property

        Public Property thb_wt_amount() As Decimal
            Get
                Return _thb_wt_amount
            End Get
            Set(ByVal value As Decimal)
                _thb_wt_amount = value
            End Set
        End Property

        Public Property thb_vat_amount() As Decimal
            Get
                Return _thb_vat_amount
            End Get
            Set(ByVal value As Decimal)
                _thb_vat_amount = value
            End Set
        End Property

        Public Property thb_discount_total() As Decimal
            Get
                Return _thb_discount_total
            End Get
            Set(ByVal value As Decimal)
                _thb_discount_total = value
            End Set
        End Property

        Public Property thb_sub_total() As Decimal
            Get
                Return _thb_sub_total
            End Get
            Set(ByVal value As Decimal)
                _thb_sub_total = value
            End Set
        End Property

        Public Property schedule_rate() As Decimal
            Get
                Return _schedule_rate
            End Get
            Set(ByVal value As Decimal)
                _schedule_rate = value
            End Set
        End Property

        Public Property status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property

        Public Property delivery_fg() As Integer
            Get
                Return _delivery_fg
            End Get
            Set(ByVal value As Integer)
                _delivery_fg = value
            End Set
        End Property

        Public Property user_id() As Integer
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property po_type() As Integer
            Get
                Return _po_type
            End Get
            Set(ByVal value As Integer)
                _po_type = value
            End Set
        End Property

        Public Property vendor_id() As Integer
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Public Property vendor_branch_id() As Integer
            Get
                Return _vendor_branch_id
            End Get
            Set(ByVal value As Integer)
                _vendor_branch_id = value
            End Set
        End Property

        Public Property total_amount() As Decimal
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property wt_amount() As Decimal
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As Decimal)
                _wt_amount = value
            End Set
        End Property

        Public Property vat_amount() As Decimal
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As Decimal)
                _vat_amount = value
            End Set
        End Property

        Public Property discount_total() As Decimal
            Get
                Return _discount_total
            End Get
            Set(ByVal value As Decimal)
                _discount_total = value
            End Set
        End Property

        Public Property sub_total() As Decimal
            Get
                Return _sub_total
            End Get
            Set(ByVal value As Decimal)
                _sub_total = value
            End Set
        End Property

        Public Property contact() As String
            Get
                Return _contact
            End Get
            Set(ByVal value As String)
                _contact = value
            End Set
        End Property

        Public Property deliver_to() As String
            Get
                Return _deliver_to
            End Get
            Set(ByVal value As String)
                _deliver_to = value
            End Set
        End Property

        Public Property attn() As String
            Get
                Return _attn
            End Get
            Set(ByVal value As String)
                _attn = value
            End Set
        End Property

        Public Property remarks() As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
            End Set
        End Property

        Public Property currency_name() As String
            Get
                Return _currency_name
            End Get
            Set(ByVal value As String)
                _currency_name = value
            End Set
        End Property

        Public Property currency_id() As Integer
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property wt_name() As String
            Get
                Return _wt_name
            End Get
            Set(ByVal value As String)
                _wt_name = value
            End Set
        End Property

        Public Property wt_id() As Integer
            Get
                Return _wt_id
            End Get
            Set(ByVal value As Integer)
                _wt_id = value
            End Set
        End Property

        Public Property vat_name() As String
            Get
                Return _vat_name
            End Get
            Set(ByVal value As String)
                _vat_name = value
            End Set
        End Property

        Public Property vat_id() As Integer
            Get
                Return _vat_id
            End Get
            Set(ByVal value As Integer)
                _vat_id = value
            End Set
        End Property

        Public Property payment_term_name() As String
            Get
                Return _payment_term_name
            End Get
            Set(ByVal value As String)
                _payment_term_name = value
            End Set
        End Property

        Public Property payment_term_id() As Integer
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Public Property quotation_no() As String
            Get
                Return _quotation_no
            End Get
            Set(ByVal value As String)
                _quotation_no = value
            End Set
        End Property

        Public Property status_id() As Integer
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property sum_amount() As Decimal
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As Decimal)
                _sum_amount = value
            End Set
        End Property

        Public Property delivery_date() As String
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property

        Public Property issue_date() As String
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property vendor_name() As String
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property
        Public Property vendor_branch_name() As String
            Get
                Return _vendor_branch_name
            End Get
            Set(ByVal value As String)
                _vendor_branch_name = value
            End Set
        End Property

        Public Property po_no() As String
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property po_type_text() As String
            Get
                Return _po_type_text
            End Get
            Set(ByVal value As String)
                _po_type_text = value
            End Set
        End Property

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
#End Region

    End Class
End Namespace

