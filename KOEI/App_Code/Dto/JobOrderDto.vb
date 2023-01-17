
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : JobOrderDto
'	Class Discription	: Dto class Job Order
'	Create User 		: Suwishaya L.
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

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class JobOrderDto

#Region "Fields"
        Private _id As Integer
        Private _job_order As String
        Private _old_job_order As String
        Private _issue_date As String
        Private _customer As Integer
        Private _end_user As Integer
        Private _receive_po As Integer
        Private _person_in_charge As Integer
        Private _job_type_id As Integer
        Private _is_boi As Integer
        Private _create_at As Integer
        Private _part_name As String
        Private _part_no As String
        Private _part_type As Integer
        Private _payment_term_id As Integer
        Private _currency_id As Integer
        Private _hontai_chk1 As Integer
        Private _hontai_date1 As String
        Private _hontai_amount1 As Decimal
        Private _hontai_condition1 As Integer
        Private _hontai_chk2 As Integer
        Private _hontai_date2 As String
        Private _hontai_amount2 As Decimal
        Private _hontai_condition2 As Integer
        Private _hontai_chk3 As Integer
        Private _hontai_date3 As String
        Private _hontai_amount3 As Decimal
        Private _hontai_condition3 As Integer
        Private _hontai_amount As Decimal
        Private _total_amount As Decimal
        Private _quotation_amount As String
        Private _remark As String
        Private _detail As String
        Private _finish_fg As Integer
        Private _status_id As Integer
        Private _finish_date As String
        Private _create_at_remark As String
        Private _payment_condition_id As String
        Private _payment_condition_remark As String
        Private _job_order_type_Detail As String
        Private _term_day As Integer
        Private _currency_name As String
        Private _customer_name As String
        Private _end_user_name As String
        Private _person_in_charge_name As String
        Private _create_at_name As String
        Private _part_type_name As String
        Private _receive_po_name As String
        Private _is_boi_name As String
        Private _person_charge_name As String
        Private _payment_condition_name As String

        'Receive data from screen (condition search)
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _finish_date_from_search As String
        Private _finish_date_to_search As String
        Private _receive_po_search As String
        Private _Job_finish_search As String
        Private _part_no_search As String
        Private _part_name_search As String
        Private _job_type_search As String
        Private _boi_search As String
        Private _person_charge_search As String

        'Receive data for running job order no        
        Private _job_month As Integer
        Private _job_year As Integer
        Private _job_last As Integer
        Private _ip_address As String

        'Receive data from menagement job order po screen 
        Private _po_type As Integer
        Private _po_no As String
        Private _po_amount As Decimal
        Private _po_date As String
        Private _po_file As String
        Private _po_receipt_date As String
        'Receive data from menagement job order quotation screen 
        Private _quo_type As Integer
        Private _quo_no As String
        Private _quo_amount As Decimal
        Private _quo_date As String
        Private _quo_file As String

#End Region

#Region "Property"

        Property payment_condition_name() As String
            Get
                Return _payment_condition_name
            End Get
            Set(ByVal value As String)
                _payment_condition_name = value
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

        Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Property old_job_order() As String
            Get
                Return _old_job_order
            End Get
            Set(ByVal value As String)
                _old_job_order = value
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

        Property customer() As Integer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
            End Set
        End Property

        Property end_user() As Integer
            Get
                Return _end_user
            End Get
            Set(ByVal value As Integer)
                _end_user = value
            End Set
        End Property

        Property receive_po() As Int32
            Get
                Return _receive_po
            End Get
            Set(ByVal value As Int32)
                _receive_po = value
            End Set
        End Property

        Property person_in_charge() As Integer
            Get
                Return _person_in_charge
            End Get
            Set(ByVal value As Integer)
                _person_in_charge = value
            End Set
        End Property

        Property job_type_id() As Int32
            Get
                Return _job_type_id
            End Get
            Set(ByVal value As Int32)
                _job_type_id = value
            End Set
        End Property

        Property is_boi() As Int32
            Get
                Return _is_boi
            End Get
            Set(ByVal value As Int32)
                _is_boi = value
            End Set
        End Property

        Property create_at() As Int32
            Get
                Return _create_at
            End Get
            Set(ByVal value As Int32)
                _create_at = value
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

        Property part_no() As String
            Get
                Return _part_no
            End Get
            Set(ByVal value As String)
                _part_no = value
            End Set
        End Property

        Property part_type() As Int32
            Get
                Return _part_type
            End Get
            Set(ByVal value As Int32)
                _part_type = value
            End Set
        End Property

        Property payment_term_id() As Integer
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Property currency_id() As Int32
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Int32)
                _currency_id = value
            End Set
        End Property

        Property hontai_chk1() As Int32
            Get
                Return _hontai_chk1
            End Get
            Set(ByVal value As Int32)
                _hontai_chk1 = value
            End Set
        End Property

        Property hontai_date1() As String
            Get
                Return _hontai_date1
            End Get
            Set(ByVal value As String)
                _hontai_date1 = value
            End Set
        End Property

        Property hontai_amount1() As Decimal
            Get
                Return _hontai_amount1
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount1 = value
            End Set
        End Property

        Property hontai_chk2() As Int32
            Get
                Return _hontai_chk2
            End Get
            Set(ByVal value As Int32)
                _hontai_chk2 = value
            End Set
        End Property

        Property hontai_date2() As String
            Get
                Return _hontai_date2
            End Get
            Set(ByVal value As String)
                _hontai_date2 = value
            End Set
        End Property

        Property hontai_amount2() As Decimal
            Get
                Return _hontai_amount2
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount2 = value
            End Set
        End Property

        Property hontai_chk3() As Int32
            Get
                Return _hontai_chk3
            End Get
            Set(ByVal value As Int32)
                _hontai_chk3 = value
            End Set
        End Property

        Property hontai_date3() As String
            Get
                Return _hontai_date3
            End Get
            Set(ByVal value As String)
                _hontai_date3 = value
            End Set
        End Property

        Property hontai_amount3() As Decimal
            Get
                Return _hontai_amount3
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount3 = value
            End Set
        End Property

        Property hontai_amount() As Decimal
            Get
                Return _hontai_amount
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount = value
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

        Property quotation_amount() As String
            Get
                Return _quotation_amount
            End Get
            Set(ByVal value As String)
                _quotation_amount = value
            End Set
        End Property

        Property remark() As String
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Property detail() As String
            Get
                Return _detail
            End Get
            Set(ByVal value As String)
                _detail = value
            End Set
        End Property

        Property finish_fg() As Int32
            Get
                Return _finish_fg
            End Get
            Set(ByVal value As Int32)
                _finish_fg = value
            End Set
        End Property

        Property status_id() As Int32
            Get
                Return _status_id
            End Get
            Set(ByVal value As Int32)
                _status_id = value
            End Set
        End Property

        Property hontai_condition1() As Int32
            Get
                Return _hontai_condition1
            End Get
            Set(ByVal value As Int32)
                _hontai_condition1 = value
            End Set
        End Property

        Property hontai_condition2() As Int32
            Get
                Return _hontai_condition2
            End Get
            Set(ByVal value As Int32)
                _hontai_condition2 = value
            End Set
        End Property

        Property hontai_condition3() As Int32
            Get
                Return _hontai_condition3
            End Get
            Set(ByVal value As Int32)
                _hontai_condition3 = value
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

        Property create_at_remark() As String
            Get
                Return _create_at_remark
            End Get
            Set(ByVal value As String)
                _create_at_remark = value
            End Set
        End Property

        Property payment_condition_id() As String
            Get
                Return _payment_condition_id
            End Get
            Set(ByVal value As String)
                _payment_condition_id = value
            End Set
        End Property

        Property payment_condition_remark() As String
            Get
                Return _payment_condition_remark
            End Get
            Set(ByVal value As String)
                _payment_condition_remark = value
            End Set
        End Property

        Property job_order_type_Detail() As String
            Get
                Return _job_order_type_Detail
            End Get
            Set(ByVal value As String)
                _job_order_type_Detail = value
            End Set
        End Property

        Property term_day() As Integer
            Get
                Return _term_day
            End Get
            Set(ByVal value As Integer)
                _term_day = value
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

        Property customer_name() As String
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Property end_user_name() As String
            Get
                Return _end_user_name
            End Get
            Set(ByVal value As String)
                _end_user_name = value
            End Set
        End Property

        Property person_in_charge_name() As String
            Get
                Return _person_in_charge_name
            End Get
            Set(ByVal value As String)
                _person_in_charge_name = value
            End Set
        End Property

        Property create_at_name() As String
            Get
                Return _create_at_name
            End Get
            Set(ByVal value As String)
                _create_at_name = value
            End Set
        End Property

        Property part_type_name() As String
            Get
                Return _part_type_name
            End Get
            Set(ByVal value As String)
                _part_type_name = value
            End Set
        End Property

        Property receive_po_name() As String
            Get
                Return _receive_po_name
            End Get
            Set(ByVal value As String)
                _receive_po_name = value
            End Set
        End Property

        Property is_boi_name() As String
            Get
                Return _is_boi_name
            End Get
            Set(ByVal value As String)
                _is_boi_name = value
            End Set
        End Property

        Property person_charge_name() As String
            Get
                Return _person_charge_name
            End Get
            Set(ByVal value As String)
                _person_charge_name = value
            End Set
        End Property

        'Property for condition search

        Property job_order_from_search() As String
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Property job_order_to_search() As String
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Property customer_search() As String
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Property issue_date_from_search() As String
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Property issue_date_to_search() As String
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Property finish_date_from_search() As String
            Get
                Return _finish_date_from_search
            End Get
            Set(ByVal value As String)
                _finish_date_from_search = value
            End Set
        End Property

        Property finish_date_to_search() As String
            Get
                Return _finish_date_to_search
            End Get
            Set(ByVal value As String)
                _finish_date_to_search = value
            End Set
        End Property

        Property receive_po_search() As String
            Get
                Return _receive_po_search
            End Get
            Set(ByVal value As String)
                _receive_po_search = value
            End Set
        End Property

        Property Job_finish_search() As String
            Get
                Return _Job_finish_search
            End Get
            Set(ByVal value As String)
                _Job_finish_search = value
            End Set
        End Property

        Property part_no_search() As String
            Get
                Return _part_no_search
            End Get
            Set(ByVal value As String)
                _part_no_search = value
            End Set
        End Property

        Property part_name_search() As String
            Get
                Return _part_name_search
            End Get
            Set(ByVal value As String)
                _part_name_search = value
            End Set
        End Property

        Property job_type_search() As String
            Get
                Return _job_type_search
            End Get
            Set(ByVal value As String)
                _job_type_search = value
            End Set
        End Property

        Property boi_search() As String
            Get
                Return _boi_search
            End Get
            Set(ByVal value As String)
                _boi_search = value
            End Set
        End Property

        Property person_charge_search() As String
            Get
                Return _person_charge_search
            End Get
            Set(ByVal value As String)
                _person_charge_search = value
            End Set
        End Property

        Property job_month() As Integer
            Get
                Return _job_month
            End Get
            Set(ByVal value As Integer)
                _job_month = value
            End Set
        End Property

        Property job_year() As Integer
            Get
                Return _job_year
            End Get
            Set(ByVal value As Integer)
                _job_year = value
            End Set
        End Property

        Property job_last() As Integer
            Get
                Return _job_last
            End Get
            Set(ByVal value As Integer)
                _job_last = value
            End Set
        End Property

        Property ip_address() As String
            Get
                Return _ip_address
            End Get
            Set(ByVal value As String)
                _ip_address = value
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

        Public Property po_no() As String
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property po_amount() As Decimal
            Get
                Return _po_amount
            End Get
            Set(ByVal value As Decimal)
                _po_amount = value
            End Set
        End Property

        Public Property po_date() As String
            Get
                Return _po_date
            End Get
            Set(ByVal value As String)
                _po_date = value
            End Set
        End Property

        Public Property po_file() As String
            Get
                Return _po_file
            End Get
            Set(ByVal value As String)
                _po_file = value
            End Set
        End Property

        Public Property po_receipt_date() As String
            Get
                Return _po_receipt_date
            End Get
            Set(ByVal value As String)
                _po_receipt_date = value
            End Set
        End Property

        Public Property quo_amount() As Decimal
            Get
                Return _quo_amount
            End Get
            Set(ByVal value As Decimal)
                _quo_amount = value
            End Set
        End Property

        Public Property quo_date() As String
            Get
                Return _quo_date
            End Get
            Set(ByVal value As String)
                _quo_date = value
            End Set
        End Property

        Public Property quo_file() As String
            Get
                Return _quo_file
            End Get
            Set(ByVal value As String)
                _quo_file = value
            End Set
        End Property

        Public Property quo_no() As String
            Get
                Return _quo_no
            End Get
            Set(ByVal value As String)
                _quo_no = value
            End Set
        End Property

        Public Property quo_type() As Integer
            Get
                Return _quo_type
            End Get
            Set(ByVal value As Integer)
                _quo_type = value
            End Set
        End Property

#End Region

    End Class
End Namespace
