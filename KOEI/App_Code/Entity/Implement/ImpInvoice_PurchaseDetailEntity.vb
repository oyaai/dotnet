#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpInvoice_PurchaseDetailEntity
'	Class Discription	: Class of Accounting detail
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic
Namespace Entity
    Public Class ImpInvoice_PurchaseDetailEntity
        Inherits Entity.ImpInvoice_PurchaseEntity

        Private _po_type As String
        Private _po_no As String
        Private _vendor_name As String
        Private _sub_total As String
        Private _status_name As String
        Private _ITName As String
        Private _job_order As String
        Private _IEName As String
        Private _quantity As String
        Private _amount As String

        Private _no As Integer
        Private _gross As Double
        Private _vat As Double
        Private _WT As Double
        Private _paid_date As String
        Private _usercreate As String
        Private _voucher_no As String
        Private _po_type_name As String

        Private _remain_qty As Double
        Private _remain_amt As Double
        Private _delivery_amt As Double
        Private _canConfirm As String
        Private _old_id As String
        Private _base As String

#Region "Property"
        Property base() As String
            Get
                Return _base
            End Get
            Set(ByVal value As String)
                _base = value
            End Set
        End Property
        Property canConfirm() As String
            Get
                Return _canConfirm
            End Get
            Set(ByVal value As String)
                _canConfirm = value
            End Set
        End Property
        Property old_id() As String
            Get
                Return _old_id
            End Get
            Set(ByVal value As String)
                _old_id = value
            End Set
        End Property
        Property remain_amt() As Double
            Get
                Return _remain_amt
            End Get
            Set(ByVal value As Double)
                _remain_amt = value
            End Set
        End Property
        Property delivery_amt() As Double
            Get
                Return _delivery_amt
            End Get
            Set(ByVal value As Double)
                _delivery_amt = value
            End Set
        End Property
        Property remain_qty() As Double
            Get
                Return _remain_qty
            End Get
            Set(ByVal value As Double)
                _remain_qty = value
            End Set
        End Property
        Property po_type_name() As String
            Get
                Return _po_type_name
            End Get
            Set(ByVal value As String)
                _po_type_name = value
            End Set
        End Property
        Property usercreate() As String
            Get
                Return _usercreate
            End Get
            Set(ByVal value As String)
                _usercreate = value
            End Set
        End Property
        Property voucher_no() As String
            Get
                Return _voucher_no
            End Get
            Set(ByVal value As String)
                _voucher_no = value
            End Set
        End Property
        Property paid_date() As String
            Get
                Return _paid_date
            End Get
            Set(ByVal value As String)
                _paid_date = value
            End Set
        End Property
        Property WT() As Double
            Get
                Return _WT
            End Get
            Set(ByVal value As Double)
                _WT = value
            End Set
        End Property
        Property vat() As Double
            Get
                Return _vat
            End Get
            Set(ByVal value As Double)
                _vat = value
            End Set
        End Property
        Property gross() As Double
            Get
                Return _gross
            End Get
            Set(ByVal value As Double)
                _gross = value
            End Set
        End Property
        Property no() As Integer
            Get
                Return _no
            End Get
            Set(ByVal value As Integer)
                _no = value
            End Set
        End Property
        Property po_type() As String
            Get
                Return _po_type
            End Get
            Set(ByVal value As String)
                _po_type = value
            End Set
        End Property
        Property po_no() As String
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property
        Property vendor_name() As String
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property
        Property sub_total() As String
            Get
                Return _sub_total
            End Get
            Set(ByVal value As String)
                _sub_total = value
            End Set
        End Property
        Property status_name() As String
            Get
                Return _status_name
            End Get
            Set(ByVal value As String)
                _status_name = value
            End Set
        End Property
        Property ITName() As String
            Get
                Return _ITName
            End Get
            Set(ByVal value As String)
                _ITName = value
            End Set
        End Property
        Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property
        Property IEName() As String
            Get
                Return _IEName
            End Get
            Set(ByVal value As String)
                _IEName = value
            End Set
        End Property
        Property quantity() As String
            Get
                Return _quantity
            End Get
            Set(ByVal value As String)
                _quantity = value
            End Set
        End Property
        Property amount() As String
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property
#End Region

    End Class
End Namespace

