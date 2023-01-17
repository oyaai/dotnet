#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPo_DetailEntity
'	Class Discription	: Class of purchase
'	Create User 		: Boonyarit
'	Create Date		    : 10-06-2013
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

    Public Class ImpPurchaseEntity
        Inherits ImpPo_HeaderEntity
        'List of Detail
        Private _purchase_detail As List(Of Entity.ImpPurchaseDetailEntity)

        Private _vendor_name As String
        Private _vendor_branch_name As String
        Private _sum_amount As Decimal
        Private _po_type_text As String
        Private _payment_term_text As String
        Private _wt_text As String
        Private _currency_name As String
        Private _vat_text As String
        Private _status As String
        Private _applied_by As String
        Private _issue_date_text As String
        '2013/09/26 Pranitda S. Start-Add
        Private _currency As String
        Private _font_color As String
        Private _delivery_fg As Integer
        '2013/09/26 Pranitda S. End-Add
#Region "Property"
        Public Property purchase_detail() As List(Of Entity.ImpPurchaseDetailEntity)
            Get
                Return _purchase_detail
            End Get
            Set(ByVal value As List(Of Entity.ImpPurchaseDetailEntity))
                _purchase_detail = value
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
        Public Property font_color() As String
            Get
                Return _font_color
            End Get
            Set(ByVal value As String)
                _font_color = value
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
        Public Property status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
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

        Public Property wt_text() As String
            Get
                Return _wt_text
            End Get
            Set(ByVal value As String)
                _wt_text = value
            End Set
        End Property

        Public Property vat_text() As String
            Get
                Return _vat_text
            End Get
            Set(ByVal value As String)
                _vat_text = value
            End Set
        End Property

        Public Property payment_term_text() As String
            Get
                Return _payment_term_text
            End Get
            Set(ByVal value As String)
                _payment_term_text = value
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

        Public Property sum_amount() As Decimal
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As Decimal)
                _sum_amount = value
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

        Public Property applied_by() As String
            Get
                Return _applied_by
            End Get
            Set(ByVal value As String)
                _applied_by = value
            End Set
        End Property

        Public Property issue_date_text() As String
            Get
                Return _issue_date_text
            End Get
            Set(ByVal value As String)
                _issue_date_text = value
            End Set
        End Property
#End Region

    End Class

    'Class Purchase_Detail
    Public Class ImpPurchaseDetailEntity
        Inherits ImpPo_DetailEntity

        Private _no As Integer
        Private _item_name As String
        Private _ie_name As String
        Private _unit_name As String
        Private _discount_type_text As String
        Private _grp As Integer

#Region "Property"
        Public Property grp() As Integer
            Get
                Return _grp
            End Get
            Set(ByVal value As Integer)
                _grp = value
            End Set
        End Property

        Public Property no() As Integer
            Get
                Return _no
            End Get
            Set(ByVal value As Integer)
                _no = value
            End Set
        End Property

        Public Property discount_type_text() As String
            Get
                Return _discount_type_text
            End Get
            Set(ByVal value As String)
                _discount_type_text = value
            End Set
        End Property

        Public Property unit_name() As String
            Get
                Return _unit_name
            End Get
            Set(ByVal value As String)
                _unit_name = value
            End Set
        End Property

        Public Property ie_name() As String
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property

        Public Property item_name() As String
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
            End Set
        End Property
#End Region

    End Class

    'Class Purchase_Detail for PDF
    Public Class ImpPurchasePDFEntty
        Inherits ImpPurchaseEntity

        Private _address As String
        Private _zipcode As String
        Private _tel As String
        Private _fax As String
        Private _amount_text As String
        Private _currency As String
        Private _discount_type_text As String

#Region "Property"
        Public Property discount_type_text() As String
            Get
                Return _discount_type_text
            End Get
            Set(ByVal value As String)
                _discount_type_text = value
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

        Public Property amount_text() As String
            Get
                Return _amount_text
            End Get
            Set(ByVal value As String)
                _amount_text = value
            End Set
        End Property

        Public Property fax() As String
            Get
                Return _fax
            End Get
            Set(ByVal value As String)
                _fax = value
            End Set
        End Property

        Public Property tel() As String
            Get
                Return _tel
            End Get
            Set(ByVal value As String)
                _tel = value
            End Set
        End Property

        Public Property zipcode() As String
            Get
                Return _zipcode
            End Get
            Set(ByVal value As String)
                _zipcode = value
            End Set
        End Property

        Public Property address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property
#End Region

    End Class

    'Class Purchase_Detail for Excel
    Public Class ImpPurchaseReportEntity
        Inherits ImpPurchaseEntity

        'PO Date  
        'PO No. 
        'Supplier Name
        'Group
        'Type
        'Description
        'Quantity
        'Unit
        'Unit Price
        'Discount
        'VAT

        Private _ie_name As String
        Private _item_name As String
        Private _quantity As Integer
        Private _unit_name As String
        Private _unit_price As Decimal
        Private _discount_amt As Decimal
        Private _amount As Decimal

        Private _job_order As String
        Private _name As String
        Private _delivery_plan As String
        Private _invoice_no As String
        Private _quality50 As String
        Private _quality25 As String
        Private _quality0 As String
        Private _delivery30 As String
        Private _delivery15 As String
        Private _delivery0 As String
        Private _service20 As String
        Private _service10 As String
        Private _service0 As String
        Private _Score As String
        Private _Grade As String
#Region "Property"
        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Public Property delivery_plan() As String
            Get
                Return _delivery_plan
            End Get
            Set(ByVal value As String)
                _delivery_plan = value
            End Set
        End Property
        Public Property invoice_no() As String
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property
        Public Property quality50() As String
            Get
                Return _quality50
            End Get
            Set(ByVal value As String)
                _quality50 = value
            End Set
        End Property
        Public Property quality25() As String
            Get
                Return _quality25
            End Get
            Set(ByVal value As String)
                _quality25 = value
            End Set
        End Property
        Public Property quality0() As String
            Get
                Return _quality0
            End Get
            Set(ByVal value As String)
                _quality0 = value
            End Set
        End Property
        Public Property delivery30() As String
            Get
                Return _delivery30
            End Get
            Set(ByVal value As String)
                _delivery30 = value
            End Set
        End Property
        Public Property delivery15() As String
            Get
                Return _delivery15
            End Get
            Set(ByVal value As String)
                _delivery15 = value
            End Set
        End Property
        Public Property delivery0() As String
            Get
                Return _delivery0
            End Get
            Set(ByVal value As String)
                _delivery0 = value
            End Set
        End Property
        Public Property service20() As String
            Get
                Return _service20
            End Get
            Set(ByVal value As String)
                _service20 = value
            End Set
        End Property
        Public Property service10() As String
            Get
                Return _service10
            End Get
            Set(ByVal value As String)
                _service10 = value
            End Set
        End Property
        Public Property service0() As String
            Get
                Return _service0
            End Get
            Set(ByVal value As String)
                _service0 = value
            End Set
        End Property
        Public Property Score() As String
            Get
                Return _Score
            End Get
            Set(ByVal value As String)
                _Score = value
            End Set
        End Property
        Public Property Grade() As String
            Get
                Return _Grade
            End Get
            Set(ByVal value As String)
                _Grade = value
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
        Public Property amount() As Decimal
            Get
                Return _amount
            End Get
            Set(ByVal value As Decimal)
                _amount = value
            End Set
        End Property

        Public Property discount_amt() As Decimal
            Get
                Return _discount_amt
            End Get
            Set(ByVal value As Decimal)
                _discount_amt = value
            End Set
        End Property

        Public Property unit_price() As Decimal
            Get
                Return _unit_price
            End Get
            Set(ByVal value As Decimal)
                _unit_price = value
            End Set
        End Property

        Public Property unit_name() As String
            Get
                Return _unit_name
            End Get
            Set(ByVal value As String)
                _unit_name = value
            End Set
        End Property

        Public Property quantity() As Integer
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        Public Property item_name() As String
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
            End Set
        End Property

        Public Property ie_name() As String
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace

