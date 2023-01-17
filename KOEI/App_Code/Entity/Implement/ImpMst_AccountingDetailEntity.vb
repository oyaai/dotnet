#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_AccountingDetailEntity
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
    Public Class ImpMst_AccountingDetailEntity
        Inherits Entity.ImpAccountingEntity

        Private _vendor_name As String
        Private _Ie_name As String
        Private _income As Double
        Private _Expense As Double
        Private _wt_percentage As Double
        Private _vat_percentage As Double
        Private _status_text As String
        Private _po_no As String
        Private _part_name As String
        Private _vendor_type1 As String
        Private _vendor_type2 As String
        Private _vendor_type2_no As String

        Private _job_id As String
        Private _job_type_id As String
        Private _job_type_name As String
        Private _job_vendor_id As String
        Private _job_status_id As String
        Private _job_status_text As String
        Private _is_finished As String
        Private _finish_date As String
        Private _job_remark As String
        Private _part_no As String

        Private _account_year As String
        Private _account_month As String
        Private _ie_type As String
        Private _ie_code As String
        Private _ie_desc As String
        Private _wt_type As String
        Private _address As String

        Private _customer As String
        Private _jh_mold_amount As String
        Private _jo_other_amount As String
        Private _jho_total As String
        Private _ji_mold_amount As String
        Private _ratio As String
        Private _ji_other_amount As String
        Private _ji_total As String
        Private _KTS_MTR_Currency As String
        Private _KTS_MTR_THB As String
        Private _KTS_INV As String
        Private _KTC_MTR_Currency As String
        Private _KTC_MTR_THB As String
        Private _KTC_INV As String
        Private _OTHER_MTR As String
        Private _TotalMTR As String
        Private _hh As String

#Region "Property"
        Property customer() As String
            Get
                Return _customer
            End Get
            Set(ByVal value As String)
                _customer = value
            End Set
        End Property
        Property jh_mold_amount() As String
            Get
                Return _jh_mold_amount
            End Get
            Set(ByVal value As String)
                _jh_mold_amount = value
            End Set
        End Property
        Property jo_other_amount() As String
            Get
                Return _jo_other_amount
            End Get
            Set(ByVal value As String)
                _jo_other_amount = value
            End Set
        End Property
        Property jho_total() As String
            Get
                Return _jho_total
            End Get
            Set(ByVal value As String)
                _jho_total = value
            End Set
        End Property
        Property ji_mold_amount() As String
            Get
                Return _ji_mold_amount
            End Get
            Set(ByVal value As String)
                _ji_mold_amount = value
            End Set
        End Property
        Property ratio() As String
            Get
                Return _ratio
            End Get
            Set(ByVal value As String)
                _ratio = value
            End Set
        End Property
        Property ji_other_amount() As String
            Get
                Return _ji_other_amount
            End Get
            Set(ByVal value As String)
                _ji_other_amount = value
            End Set
        End Property
        Property ji_total() As String
            Get
                Return _ji_total
            End Get
            Set(ByVal value As String)
                _ji_total = value
            End Set
        End Property
        Property KTS_MTR_Currency() As String
            Get
                Return _KTS_MTR_Currency
            End Get
            Set(ByVal value As String)
                _KTS_MTR_Currency = value
            End Set
        End Property
        Property KTS_MTR_THB() As String
            Get
                Return _KTS_MTR_THB
            End Get
            Set(ByVal value As String)
                _KTS_MTR_THB = value
            End Set
        End Property
        Property KTS_INV() As String
            Get
                Return _KTS_INV
            End Get
            Set(ByVal value As String)
                _KTS_INV = value
            End Set
        End Property
        Property KTC_MTR_Currency() As String
            Get
                Return _KTC_MTR_Currency
            End Get
            Set(ByVal value As String)
                _KTC_MTR_Currency = value
            End Set
        End Property
        Property KTC_MTR_THB() As String
            Get
                Return _KTC_MTR_THB
            End Get
            Set(ByVal value As String)
                _KTC_MTR_THB = value
            End Set
        End Property
        Property KTC_INV() As String
            Get
                Return _KTC_INV
            End Get
            Set(ByVal value As String)
                _KTC_INV = value
            End Set
        End Property
        Property OTHER_MTR() As String
            Get
                Return _OTHER_MTR
            End Get
            Set(ByVal value As String)
                _OTHER_MTR = value
            End Set
        End Property
        Property TotalMTR() As String
            Get
                Return _TotalMTR
            End Get
            Set(ByVal value As String)
                _TotalMTR = value
            End Set
        End Property
        Property hh() As String
            Get
                Return _hh
            End Get
            Set(ByVal value As String)
                _hh = value
            End Set
        End Property
        Property wt_type() As String
            Get
                Return _wt_type
            End Get
            Set(ByVal value As String)
                _wt_type = value
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
        Property ie_desc() As String
            Get
                Return _ie_desc
            End Get
            Set(ByVal value As String)
                _ie_desc = value
            End Set
        End Property
        Property ie_code() As String
            Get
                Return _ie_code
            End Get
            Set(ByVal value As String)
                _ie_code = value
            End Set
        End Property
        Property ie_type() As String
            Get
                Return _ie_type
            End Get
            Set(ByVal value As String)
                _ie_type = value
            End Set
        End Property
        Property account_month() As String
            Get
                Return _account_month
            End Get
            Set(ByVal value As String)
                _account_month = value
            End Set
        End Property
        Property account_year() As String
            Get
                Return _account_year
            End Get
            Set(ByVal value As String)
                _account_year = value
            End Set
        End Property
        Property vendor_type2_no() As String
            Get
                Return _vendor_type2_no
            End Get
            Set(ByVal value As String)
                _vendor_type2_no = value
            End Set
        End Property
        Property vendor_type2() As String
            Get
                Return _vendor_type2
            End Get
            Set(ByVal value As String)
                _vendor_type2 = value
            End Set
        End Property
        Property vendor_type1() As String
            Get
                Return _vendor_type1
            End Get
            Set(ByVal value As String)
                _vendor_type1 = value
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
        Property Ie_name() As String
            Get
                Return _Ie_name
            End Get
            Set(ByVal value As String)
                _Ie_name = value
            End Set
        End Property
        Property income() As Double
            Get
                Return _income
            End Get
            Set(ByVal value As Double)
                _income = value
            End Set
        End Property
        Property Expense() As Double
            Get
                Return _Expense
            End Get
            Set(ByVal value As Double)
                _Expense = value
            End Set
        End Property
        Property wt_percentage() As Double
            Get
                Return _wt_percentage
            End Get
            Set(ByVal value As Double)
                _wt_percentage = value
            End Set
        End Property
        Property vat_percentage() As Double
            Get
                Return _vat_percentage
            End Get
            Set(ByVal value As Double)
                _vat_percentage = value
            End Set
        End Property
        Property status_text() As String
            Get
                Return _status_text
            End Get
            Set(ByVal value As String)
                _status_text = value
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
        Property part_name() As String
            Get
                Return _part_name
            End Get
            Set(ByVal value As String)
                _part_name = value
            End Set
        End Property
        Property job_id() As String
            Get
                Return _job_id
            End Get
            Set(ByVal value As String)
                _job_id = value
            End Set
        End Property
        Property job_type_id() As String
            Get
                Return _job_type_id
            End Get
            Set(ByVal value As String)
                _job_type_id = value
            End Set
        End Property
        Property job_type_name() As String
            Get
                Return _job_type_name
            End Get
            Set(ByVal value As String)
                _job_type_name = value
            End Set
        End Property
        Property job_vendor_id() As String
            Get
                Return _job_vendor_id
            End Get
            Set(ByVal value As String)
                _job_vendor_id = value
            End Set
        End Property
        Property job_status_id() As String
            Get
                Return _job_status_id
            End Get
            Set(ByVal value As String)
                _job_status_id = value
            End Set
        End Property
        Property job_status_text() As String
            Get
                Return _job_status_text
            End Get
            Set(ByVal value As String)
                _job_status_text = value
            End Set
        End Property
        Property is_finished() As String
            Get
                Return _is_finished
            End Get
            Set(ByVal value As String)
                _is_finished = value
            End Set
        End Property
        Property finish_date() As String
            Get
                Return _finish_date
            End Get
            Set(ByVal value As String)
                _finish_date = value
            End Set
        End Property
        Property job_remark() As String
            Get
                Return _job_remark
            End Get
            Set(ByVal value As String)
                _job_remark = value
            End Set
        End Property
        Property part_no() As String
            Get
                Return _part_no
            End Get
            Set(ByVal value As String)
                _part_no = value
            End Set
        End Property

#End Region

    End Class
End Namespace

