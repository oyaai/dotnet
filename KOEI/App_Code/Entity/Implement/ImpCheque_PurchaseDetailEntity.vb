#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpCheque_PurchaseDetailEntity
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
    Public Class ImpCheque_PurchaseDetailEntity
        Inherits Entity.ImpCheque_PurchaseEntity

        Private _vendor_name As String
        Private _vendor_type As String
        Private _account_title_name As String
        Private _vat_name As String
        Private _WTName As String
        Private _payment_from As String

        Private _cheque_no As String
        Private _cheque_date As String
        Private _updated_by As String
        Private _updated_date As String
        Private _voucher_no As String
        Private _job_order As String
        Private _sub_total As String
        Private _amount_bank As String
        Private _vat_amount As String
        Private _wt_amount As String
        Private _payment_date As String
        Private _ie_name As String
        Private _wt_name As String
        Private _vat_percent As String
        Private _wt_percent As String
        Private _account_next_approve As String

        Private _print_date As String
        Private _bank As String
        Private _total As String
        Private _vat As String
        Private _wt As String
        Private _no As String
        Private _invoice_no As String
        Private _delivery_date As String
        Private _gross As String
        Private _amount As String
        Private _po_no As String
        Private _paid_date As String
        Private _usercreate As String
        Private _name As String
        Private _address As String
        Private _type2_no As String
        Private _type2 As String

        Private _account_type As String
        Private _account_date As String
        Private _vendor_id As String
        Private _Expense As String
        Private _income As String
        Private _chkCheque As String

        Private _hsub_total As String
        Private _hvat_amount As String
        Private _hwt_amount As String
        Private _account_no As String
        Private _account_name As String
        Private _vat_amt As String
        Private _wt_amt As String
        Private _currency_id As String ' support Calculate Report JBPU08
        Private _currency_name As String ' Support Calculate Report JBPU08

#Region "Property"
        Property account_name() As String
            Get
                Return _account_name
            End Get
            Set(ByVal value As String)
                _account_name = value
            End Set
        End Property
        Property account_no() As String
            Get
                Return _account_no
            End Get
            Set(ByVal value As String)
                _account_no = value
            End Set
        End Property
        Property hsub_total() As String
            Get
                Return _hsub_total
            End Get
            Set(ByVal value As String)
                _hsub_total = value
            End Set
        End Property
        Property hvat_amount() As String
            Get
                Return _hvat_amount
            End Get
            Set(ByVal value As String)
                _hvat_amount = value
            End Set
        End Property
        Property hwt_amount() As String
            Get
                Return _hwt_amount
            End Get
            Set(ByVal value As String)
                _hwt_amount = value
            End Set
        End Property
        Property chkCheque() As String
            Get
                Return _chkCheque
            End Get
            Set(ByVal value As String)
                _chkCheque = value
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
        Property account_date() As String
            Get
                Return _account_date
            End Get
            Set(ByVal value As String)
                _account_date = value
            End Set
        End Property
        Property vendor_id() As String
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As String)
                _vendor_id = value
            End Set
        End Property
        Property Expense() As String
            Get
                Return _Expense
            End Get
            Set(ByVal value As String)
                _Expense = value
            End Set
        End Property
        Property income() As String
            Get
                Return _income
            End Get
            Set(ByVal value As String)
                _income = value
            End Set
        End Property
        Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Property address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property
        Property type2_no() As String
            Get
                Return _type2_no
            End Get
            Set(ByVal value As String)
                _type2_no = value
            End Set
        End Property
        Property type2() As String
            Get
                Return _type2
            End Get
            Set(ByVal value As String)
                _type2 = value
            End Set
        End Property
        Property no() As String
            Get
                Return _no
            End Get
            Set(ByVal value As String)
                _no = value
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
        Property delivery_date() As String
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property
        Property gross() As String
            Get
                Return _gross
            End Get
            Set(ByVal value As String)
                _gross = value
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
        Property po_no() As String
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
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
        Property usercreate() As String
            Get
                Return _usercreate
            End Get
            Set(ByVal value As String)
                _usercreate = value
            End Set
        End Property
        Property wt() As String
            Get
                Return _wt
            End Get
            Set(ByVal value As String)
                _wt = value
            End Set
        End Property
        Property vat() As String
            Get
                Return _vat
            End Get
            Set(ByVal value As String)
                _vat = value
            End Set
        End Property
        Property print_date() As String
            Get
                Return _print_date
            End Get
            Set(ByVal value As String)
                _print_date = value
            End Set
        End Property
        Property bank() As String
            Get
                Return _bank
            End Get
            Set(ByVal value As String)
                _bank = value
            End Set
        End Property
        Property total() As String
            Get
                Return _total
            End Get
            Set(ByVal value As String)
                _total = value
            End Set
        End Property
        Property account_next_approve() As String
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As String)
                _account_next_approve = value
            End Set
        End Property
        Property vat_percent() As String
            Get
                Return _vat_percent
            End Get
            Set(ByVal value As String)
                _vat_percent = value
            End Set
        End Property
        Property wt_percent() As String
            Get
                Return _wt_percent
            End Get
            Set(ByVal value As String)
                _wt_percent = value
            End Set
        End Property
        Property ie_name() As String
            Get
                Return _ie_name
            End Get
            Set(ByVal value As String)
                _ie_name = value
            End Set
        End Property
        Property wt_name() As String
            Get
                Return _wt_name
            End Get
            Set(ByVal value As String)
                _wt_name = value
            End Set
        End Property
        Property cheque_no() As String
            Get
                Return _cheque_no
            End Get
            Set(ByVal value As String)
                _cheque_no = value
            End Set
        End Property
        Property cheque_date() As String
            Get
                Return _cheque_date
            End Get
            Set(ByVal value As String)
                _cheque_date = value
            End Set
        End Property
        Property updated_by() As String
            Get
                Return _updated_by
            End Get
            Set(ByVal value As String)
                _updated_by = value
            End Set
        End Property
        Property updated_date() As String
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
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
        Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
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
        Property amount_bank() As String
            Get
                Return _amount_bank
            End Get
            Set(ByVal value As String)
                _amount_bank = value
            End Set
        End Property
        Property vat_amount() As String
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As String)
                _vat_amount = value
            End Set
        End Property
        Property wt_amount() As String
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As String)
                _wt_amount = value
            End Set
        End Property
        Property payment_date() As String
            Get
                Return _payment_date
            End Get
            Set(ByVal value As String)
                _payment_date = value
            End Set
        End Property
        Property payment_from() As String
            Get
                Return _payment_from
            End Get
            Set(ByVal value As String)
                _payment_from = value
            End Set
        End Property
        Property WTName() As String
            Get
                Return _WTName
            End Get
            Set(ByVal value As String)
                _WTName = value
            End Set
        End Property
        Property vat_name() As String
            Get
                Return _vat_name
            End Get
            Set(ByVal value As String)
                _vat_name = value
            End Set
        End Property
        Property account_title_name() As String
            Get
                Return _account_title_name
            End Get
            Set(ByVal value As String)
                _account_title_name = value
            End Set
        End Property
        Property vendor_type() As String
            Get
                Return _vendor_type
            End Get
            Set(ByVal value As String)
                _vendor_type = value
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
        Property vat_amt() As String
            Get
                Return _vat_amt
            End Get
            Set(ByVal value As String)
                _vat_amt = value
            End Set
        End Property
        Property wt_amt() As String
            Get
                Return _wt_amt
            End Get
            Set(ByVal value As String)
                _wt_amt = value
            End Set
        End Property
        Property currency_id() As String
            Get
                Return _currency_id
            End Get
            Set(ByVal value As String)
                _currency_id = value
            End Set
        End Property

        Property currency_name() As String
            Get
                Return _currency_name
            End Get
            Set(ByVal value As String)
                _currency_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace

