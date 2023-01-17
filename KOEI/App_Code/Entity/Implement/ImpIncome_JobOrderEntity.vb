#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IIncome_JobOrderEntity
'	Class Discription	: Class of table receive_header
'	Create User 		: Suwishaya L.
'	Create Date		    : 01-07-2013
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
    Public Class ImpIncome_JobOrderEntity
        Implements IIncome_JobOrderEntity

        Private _id As Integer
        Private _invoice_no As String
        Private _receipt_date As String
        Private _ie_id As Integer
        Private _vendor_id As Integer
        Private _account_type As Integer
        Private _invoice_type As Integer
        Private _bank_fee As Decimal
        Private _total_amount As Decimal
        Private _user_id As Integer
        Private _status_id As Integer
        Private _issue_date As String
        Private _customer As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

        'Receive data from screen (condition search)
        Private _invoice_no_search As String
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _receipt_date_from_search As String
        Private _receipt_date_to_search As String

        'Receive data for detail search
        Private _invoice_type_name As String
        Private _customer_name As String
        Private _amount_detail As String

        'Receive data for report
        Private _receive_header_id As Integer
        Private _receive_detail_id As Integer
        Private _payment_condition_id As Integer
        Private _job_order_id As Integer
        Private _job_order As String
        Private _hontai_type As String
        Private _stage As String
        Private _po_no As String
        Private _percent As String
        Private _price As String
        Private _vat As String
        Private _amount As String
        Private _remark As String
        Private _sum_amount As String
        Private _sum_price As String
        Private _sum_vat As String

        Private _actual_rate As String

        Private objIncomeJobOrder As New Dao.ImpIncome_JobOrderDao

#Region "Function"
        Public Function GetIncomeList(ByVal objIncomeJobOrderEnt As IIncome_JobOrderEntity) As System.Collections.Generic.List(Of ImpIncome_JobOrderEntity) Implements IIncome_JobOrderEntity.GetIncomeList
            Return objIncomeJobOrder.GetIncomeList(objIncomeJobOrderEnt)
        End Function

        Public Function GetMonthlySaleReport(ByVal objIncomeJobOrderEnt As IIncome_JobOrderEntity) As System.Collections.Generic.List(Of ImpIncome_JobOrderEntity) Implements IIncome_JobOrderEntity.GetMonthlySaleReport
            Return objIncomeJobOrder.GetMonthlySaleReport(objIncomeJobOrderEnt)
        End Function

        Public Function GetSumMonthlySaleReport(ByVal objIncomeJobOrderEnt As IIncome_JobOrderEntity) As System.Collections.Generic.List(Of ImpIncome_JobOrderEntity) Implements IIncome_JobOrderEntity.GetSumMonthlySaleReport
            Return objIncomeJobOrder.GetSumMonthlySaleReport(objIncomeJobOrderEnt)
        End Function
#End Region

#Region "Property"
        Public Property actual_rate() As String Implements IIncome_JobOrderEntity.actual_rate
            Get
                Return _actual_rate
            End Get
            Set(ByVal value As String)
                _actual_rate = value
            End Set
        End Property
        Public Property customer_search() As String Implements IIncome_JobOrderEntity.customer_search
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Public Property invoice_no_search() As String Implements IIncome_JobOrderEntity.invoice_no_search
            Get
                Return _invoice_no_search
            End Get
            Set(ByVal value As String)
                _invoice_no_search = value
            End Set
        End Property

        Public Property issue_date_from_search() As String Implements IIncome_JobOrderEntity.issue_date_from_search
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Public Property issue_date_to_search() As String Implements IIncome_JobOrderEntity.issue_date_to_search
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Public Property job_order_from_search() As String Implements IIncome_JobOrderEntity.job_order_from_search
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Public Property job_order_to_search() As String Implements IIncome_JobOrderEntity.job_order_to_search
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Public Property receipt_date_from_search() As String Implements IIncome_JobOrderEntity.receipt_date_from_search
            Get
                Return _receipt_date_from_search
            End Get
            Set(ByVal value As String)
                _receipt_date_from_search = value
            End Set
        End Property

        Public Property receipt_date_to_search() As String Implements IIncome_JobOrderEntity.receipt_date_to_search
            Get
                Return _receipt_date_to_search
            End Get
            Set(ByVal value As String)
                _receipt_date_to_search = value
            End Set
        End Property

        Public Property account_type() As Integer Implements IIncome_JobOrderEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Public Property bank_fee() As Decimal Implements IIncome_JobOrderEntity.bank_fee
            Get
                Return _bank_fee
            End Get
            Set(ByVal value As Decimal)
                _bank_fee = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IIncome_JobOrderEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IIncome_JobOrderEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property customer() As Integer Implements IIncome_JobOrderEntity.customer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
            End Set
        End Property

        Public Property id() As Integer Implements IIncome_JobOrderEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ie_id() As Integer Implements IIncome_JobOrderEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IIncome_JobOrderEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property invoice_type() As Integer Implements IIncome_JobOrderEntity.invoice_type
            Get
                Return _invoice_type
            End Get
            Set(ByVal value As Integer)
                _invoice_type = value
            End Set
        End Property

        Public Property issue_date() As String Implements IIncome_JobOrderEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property receipt_date() As String Implements IIncome_JobOrderEntity.receipt_date
            Get
                Return _receipt_date
            End Get
            Set(ByVal value As String)
                _receipt_date = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IIncome_JobOrderEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property total_amount() As Decimal Implements IIncome_JobOrderEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IIncome_JobOrderEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IIncome_JobOrderEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property user_id() As Integer Implements IIncome_JobOrderEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements IIncome_JobOrderEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Public Property amount_detail() As String Implements IIncome_JobOrderEntity.amount_detail
            Get
                Return _amount_detail
            End Get
            Set(ByVal value As String)
                _amount_detail = value
            End Set
        End Property

        Public Property customer_name() As String Implements IIncome_JobOrderEntity.customer_name
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Public Property invoice_type_name() As String Implements IIncome_JobOrderEntity.invoice_type_name
            Get
                Return _invoice_type_name
            End Get
            Set(ByVal value As String)
                _invoice_type_name = value
            End Set
        End Property

        Public Property amount() As String Implements IIncome_JobOrderEntity.amount
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property hontai_type() As String Implements IIncome_JobOrderEntity.hontai_type
            Get
                Return _hontai_type
            End Get
            Set(ByVal value As String)
                _hontai_type = value
            End Set
        End Property

        Public Property job_order() As String Implements IIncome_JobOrderEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property job_order_id() As Integer Implements IIncome_JobOrderEntity.job_order_id
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As Integer)
                _job_order_id = value
            End Set
        End Property

        Public Property payment_condition_id() As Integer Implements IIncome_JobOrderEntity.payment_condition_id
            Get
                Return _payment_condition_id
            End Get
            Set(ByVal value As Integer)
                _payment_condition_id = value
            End Set
        End Property

        Public Property percent() As String Implements IIncome_JobOrderEntity.percent
            Get
                Return _percent
            End Get
            Set(ByVal value As String)
                _percent = value
            End Set
        End Property

        Public Property po_no() As String Implements IIncome_JobOrderEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property price() As String Implements IIncome_JobOrderEntity.price
            Get
                Return _price
            End Get
            Set(ByVal value As String)
                _price = value
            End Set
        End Property

        Public Property receive_detail_id() As Integer Implements IIncome_JobOrderEntity.receive_detail_id
            Get
                Return _receive_detail_id
            End Get
            Set(ByVal value As Integer)
                _receive_detail_id = value
            End Set
        End Property

        Public Property receive_header_id() As Integer Implements IIncome_JobOrderEntity.receive_header_id
            Get
                Return _receive_header_id
            End Get
            Set(ByVal value As Integer)
                _receive_header_id = value
            End Set
        End Property

        Public Property remark() As String Implements IIncome_JobOrderEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property stage() As String Implements IIncome_JobOrderEntity.stage
            Get
                Return _stage
            End Get
            Set(ByVal value As String)
                _stage = value
            End Set
        End Property

        Public Property vat() As String Implements IIncome_JobOrderEntity.vat
            Get
                Return _vat
            End Get
            Set(ByVal value As String)
                _vat = value
            End Set
        End Property

        Public Property sum_amount() As String Implements IIncome_JobOrderEntity.sum_amount
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As String)
                _sum_amount = value
            End Set
        End Property

        Public Property sum_price() As String Implements IIncome_JobOrderEntity.sum_price
            Get
                Return _sum_price
            End Get
            Set(ByVal value As String)
                _sum_price = value
            End Set
        End Property

        Public Property sum_vat() As String Implements IIncome_JobOrderEntity.sum_vat
            Get
                Return _sum_vat
            End Get
            Set(ByVal value As String)
                _sum_vat = value
            End Set
        End Property

#End Region

    End Class
End Namespace
