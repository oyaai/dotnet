#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : PurchaseDto
'	Class Discription	: Dto class Purchase
'	Create User 		: Boonyarit
'	Create Date		    : 12-06-2013
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
    Public Class PurchaseTemp
        Private _job_order As String
        Private _item_id As Integer
        Private _item_name As String
        Private _ie_id As Integer
        Private _ie_name As String
        Private _unit_price As Decimal
        Private _qty As Integer
        Private _unit_id As Integer
        Private _unit_name As String
        Private _discount As Decimal
        Private _discount_type As Integer
        Private _discount_type_text As String
        Private _remark As String
        Private _id As String

#Region "Property"
        Public Property item_id() As Integer
            Get
                Return _item_id
            End Get
            Set(ByVal value As Integer)
                _item_id = value
            End Set
        End Property

        Public Property remark() As String
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
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

        Public Property discount_type() As Integer
            Get
                Return _discount_type
            End Get
            Set(ByVal value As Integer)
                _discount_type = value
            End Set
        End Property

        Public Property discount() As Decimal
            Get
                Return _discount
            End Get
            Set(ByVal value As Decimal)
                _discount = value
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

        Public Property unit_id() As Integer
            Get
                Return _unit_id
            End Get
            Set(ByVal value As Integer)
                _unit_id = value
            End Set
        End Property

        Public Property qty() As Integer
            Get
                Return _qty
            End Get
            Set(ByVal value As Integer)
                _qty = value
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

        Public Property ie_name() As String
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property

        Public Property ie_id() As Integer
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
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

        Public Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property
        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
#End Region

    End Class

    Public Class PurchaseDetailDto
        'Item Name			'po_detail.item_id
        Private _item_id As Integer
        Private _item_name As String
        'Job Order			'po_detail.job_order
        Private _job_order As String
        'Item of Expense	'po_detail.ie_id
        Private _ie_id As Integer
        Private _ie_name As String
        'Unit Price			'po_detail.unit_price
        Private _unit_price As Decimal
        'Qty				'po_detail.quantity
        Private _qty As Integer
        'Unit				'po_detail.unit_id
        Private _unit_id As Integer
        Private _unit_name As String
        'Discount			'po_detail.discount
        Private _discount As Decimal
        'Discount Type		'po_detail.discount_type
        Private _discount_type As Integer
        Private _discount_type_text As String
        'Vat				'po_detail.vat_amount
        Private _vat As Decimal
        'W/T				'po_detail.wt_amount
        Private _wt As Decimal
        'Amount				'po_detail.amount
        Private _amount As Decimal
        'Remarks			'po_detail.remark
        Private _remarks As String
        'id() As Integer => String '** Boon Change
        Private _id As String
        'po_header_id() As Integer
        Private _po_header_id As Integer


#Region "Property"
        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property po_header_id() As Integer
            Get
                Return _po_header_id
            End Get
            Set(ByVal value As Integer)
                _po_header_id = value
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

        Public Property remarks() As String
            Get
                Return _remarks
            End Get
            Set(ByVal value As String)
                _remarks = value
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

        Public Property wt() As Decimal
            Get
                Return _wt
            End Get
            Set(ByVal value As Decimal)
                _wt = value
            End Set
        End Property

        Public Property vat() As Decimal
            Get
                Return _vat
            End Get
            Set(ByVal value As Decimal)
                _vat = value
            End Set
        End Property

        Public Property discount_type() As Integer
            Get
                Return _discount_type
            End Get
            Set(ByVal value As Integer)
                _discount_type = value
            End Set
        End Property

        Public Property discount() As Decimal
            Get
                Return _discount
            End Get
            Set(ByVal value As Decimal)
                _discount = value
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

        Public Property unit_id() As Integer
            Get
                Return _unit_id
            End Get
            Set(ByVal value As Integer)
                _unit_id = value
            End Set
        End Property

        Public Property qty() As Integer
            Get
                Return _qty
            End Get
            Set(ByVal value As Integer)
                _qty = value
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

        Public Property ie_name() As String
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property

        Public Property ie_id() As Integer
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
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

        Public Property item_name() As String
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
            End Set
        End Property

        Public Property item_id() As Integer
            Get
                Return _item_id
            End Get
            Set(ByVal value As Integer)
                _item_id = value
            End Set
        End Property
#End Region

    End Class
End Namespace


