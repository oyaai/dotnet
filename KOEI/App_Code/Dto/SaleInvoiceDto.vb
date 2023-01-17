#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : SaleInvoiceDto
'	Class Discription	: Dto class sale invoice 
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

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class SaleInvoiceDto

        Private _id As Integer
        Private _name As String
        Private _invoice_no As String
        Private _receipt_date As String
        Private _ie_id As Integer
        Private _vendor_id As Integer
        Private _account_type As String
        Private _invoice_type As String
        Private _bank_fee As String
        Private _total_amount As Decimal
        Private _user_id As Integer
        Private _status_id As Integer
        Private _issue_date As String
        Private _customer As String
        Private _job_order As String
        Private _actual_rate As String
        Private _job_order_id As Integer
        Private _currency_id As Integer
        Private _currency As String
        Private _arrJob_order_id As String
        Private _arrJob_order As String
        Private _schedule_rate As String
        Private _account_next_approve As String
        Private _strJobOrder1 As String
        Private _strJobOrder2 As String
        Private _strJobOrder3 As String
        Private _status As String
        Private _hontai_fg1 As String
        Private _hontai_fg2 As String
        Private _hontai_fg3 As String
        Private _po_fg As String
        Private _hontai_type As Integer

        'Receive data from screen (condition search)
        Private _invoice_no_search As String
        Private _invoice_type_search As String
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _receipt_date_from_search As String
        Private _receipt_date_to_search As String

        'Receive data to detail screen
        Private _account_title As String
        Private _customer_name As String
        Private _account_type_name As String
        Private _invoice_type_name As String
        Private _bank_fee_detail As String
        Private _amount As Decimal
        Private _sum_amount As Decimal
        Private _sum_price As Decimal
        Private _sum_vat As Decimal
        Private _sum_wt As Decimal
        Private _total_invoice_amount As Decimal

#Region "Property"

        Public Property po_fg() As String
            Get
                Return _po_fg
            End Get
            Set(ByVal value As String)
                _po_fg = value
            End Set
        End Property

        Public Property hontai_fg3() As String
            Get
                Return _hontai_fg3
            End Get
            Set(ByVal value As String)
                _hontai_fg3 = value
            End Set
        End Property

        Public Property hontai_fg2() As String
            Get
                Return _hontai_fg2
            End Get
            Set(ByVal value As String)
                _hontai_fg2 = value
            End Set
        End Property

        Public Property hontai_fg1() As String
            Get
                Return _hontai_fg1
            End Get
            Set(ByVal value As String)
                _hontai_fg1 = value
            End Set
        End Property

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
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

        Public Property strJobOrder1() As String
            Get
                Return _strJobOrder1
            End Get
            Set(ByVal value As String)
                _strJobOrder1 = value
            End Set
        End Property

        Public Property strJobOrder2() As String
            Get
                Return _strJobOrder2
            End Get
            Set(ByVal value As String)
                _strJobOrder2 = value
            End Set
        End Property

        Public Property strJobOrder3() As String
            Get
                Return _strJobOrder3
            End Get
            Set(ByVal value As String)
                _strJobOrder3 = value
            End Set
        End Property

        Public Property account_next_approve() As String
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As String)
                _account_next_approve = value
            End Set
        End Property

        Public Property schedule_rate() As String
            Get
                Return _schedule_rate
            End Get
            Set(ByVal value As String)
                _schedule_rate = value
            End Set
        End Property

        Public Property arrJob_order_id() As String
            Get
                Return _arrJob_order_id
            End Get
            Set(ByVal value As String)
                _arrJob_order_id = value
            End Set
        End Property

        Public Property arrJob_order() As String
            Get
                Return _arrJob_order
            End Get
            Set(ByVal value As String)
                _arrJob_order = value
            End Set
        End Property

        Public Property currency() As String
            Get
                Return _currency
            End Get
            Set(ByVal value As String)
                _currency = value
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

        Public Property job_order_id() As Integer
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As Integer)
                _job_order_id = value
            End Set
        End Property

        Public Property actual_rate() As String
            Get
                Return _actual_rate
            End Get
            Set(ByVal value As String)
                _actual_rate = value
            End Set
        End Property


        Public Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property total_invoice_amount() As Decimal
            Get
                Return _total_invoice_amount
            End Get
            Set(ByVal value As Decimal)
                _total_invoice_amount = value
            End Set
        End Property

        Public Property sum_vat() As Decimal
            Get
                Return _sum_vat
            End Get
            Set(ByVal value As Decimal)
                _sum_vat = value
            End Set
        End Property

        Public Property sum_wt() As Decimal
            Get
                Return _sum_wt
            End Get
            Set(ByVal value As Decimal)
                _sum_wt = value
            End Set
        End Property

        Public Property sum_price() As Decimal
            Get
                Return _sum_price
            End Get
            Set(ByVal value As Decimal)
                _sum_price = value
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

        Public Property amount() As Decimal
            Get
                Return _amount
            End Get
            Set(ByVal value As Decimal)
                _amount = value
            End Set
        End Property

        Public Property bank_fee_detail() As String
            Get
                Return _bank_fee_detail
            End Get
            Set(ByVal value As String)
                _bank_fee_detail = value
            End Set
        End Property

        Public Property invoice_type_name() As String
            Get
                Return _invoice_type_name
            End Get
            Set(ByVal value As String)
                _invoice_type_name = value
            End Set
        End Property

        Public Property account_type_name() As String
            Get
                Return _account_type_name
            End Get
            Set(ByVal value As String)
                _account_type_name = value
            End Set
        End Property

        Public Property account_title() As String
            Get
                Return _account_title
            End Get
            Set(ByVal value As String)
                _account_title = value
            End Set
        End Property

        Public Property customer_name() As String
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Public Property customer_search() As String
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Public Property invoice_no_search() As String
            Get
                Return _invoice_no_search
            End Get
            Set(ByVal value As String)
                _invoice_no_search = value
            End Set
        End Property

        Public Property invoice_type_search() As String
            Get
                Return _invoice_type_search
            End Get
            Set(ByVal value As String)
                _invoice_type_search = value
            End Set
        End Property

        Public Property issue_date_from_search() As String
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Public Property issue_date_to_search() As String
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Public Property job_order_from_search() As String
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Public Property job_order_to_search() As String
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Public Property receipt_date_from_search() As String
            Get
                Return _receipt_date_from_search
            End Get
            Set(ByVal value As String)
                _receipt_date_from_search = value
            End Set
        End Property

        Public Property receipt_date_to_search() As String
            Get
                Return _receipt_date_to_search
            End Get
            Set(ByVal value As String)
                _receipt_date_to_search = value
            End Set
        End Property

        Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Property invoice_no() As String
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Property receipt_date() As String
            Get
                Return _receipt_date
            End Get
            Set(ByVal value As String)
                _receipt_date = value
            End Set
        End Property

        Property ie_id() As Integer
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Property vendor_id() As Integer
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Property account_type() As String
            Get
                Return _account_type
            End Get
            Set(ByVal value As String)
                _account_type = value
            End Set
        End Property

        Property invoice_type() As String
            Get
                Return _invoice_type
            End Get
            Set(ByVal value As String)
                _invoice_type = value
            End Set
        End Property

        Property bank_fee() As String
            Get
                Return _bank_fee
            End Get
            Set(ByVal value As String)
                _bank_fee = value
            End Set
        End Property

        Property total_amount() As Decimal
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Property user_id() As Integer
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Property status_id() As Integer
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Property issue_date() As String
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Property customer() As String
            Get
                Return _customer
            End Get
            Set(ByVal value As String)
                _customer = value
            End Set
        End Property

        Property hontai_type() As String
            Get
                Return _hontai_type
            End Get
            Set(ByVal value As String)
                _hontai_type = value
            End Set
        End Property

#End Region

    End Class
End Namespace
