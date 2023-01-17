#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpJob_OrderDetailEntity
'	Class Discription	: Class of job order detail
'	Create User 		: Suwishaya L.
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

Namespace Entity
    Public Class ImpJob_OrderDetailEntity
        Inherits Entity.ImpJob_OrderEntity

        Private _receive_po_Detail As String = Nothing
        Private _job_finished_Detail As String = Nothing
        Private _job_order_type_Detail As String = Nothing
        Private _customer_Detail As String = Nothing
        Private _total_amount_Detail As String = Nothing
        Private _quotation_amount_Detail As String = Nothing
        Private _payment_condition_detail As String = Nothing

        'for job order running no 
        Private _job_year_detail As Integer
        Private _job_month_detail As Integer
        Private _job_last_detail As Integer

        'Receive sum po amount from job order po temp
        Private _sum_po_amount As Decimal
        Private _sum_quo_amount As Decimal
        Private _total_po_amount As Decimal

        'Receive for job order detail (PO infomation)
        Private _po_type_detail As String
        Private _po_no_detail As String
        Private _po_amount_detail As Decimal
        Private _po_date_detail As String
        Private _po_receipt_date_detail As String
        Private _po_file_detail As String
        ' Field for Check Use PO Add 2014/04/23
        Private _check_use As String

        'Receive for job order detail(Quo infomation)
        Private _quo_type_detail As String
        Private _quo_no_detail As String
        Private _quo_amount_detail As Decimal
        Private _quo_date_detail As String
        Private _quo_file_detail As String
        Private _no As Integer

        'Receive for job order detail(invoice infomation)
        Private _status As String
        Private _invoice_no As String
        Private _inv_receipt_date As String
        Private _account_title As String
        Private _inv_issue_date As String
        Private _inv_total_amount As Decimal


#Region "Property"
        Property receive_po_Detail() As String
            Get
                Return _receive_po_Detail
            End Get
            Set(ByVal value As String)
                _receive_po_Detail = value
            End Set
        End Property

        Property job_finished_Detail() As String
            Get
                Return _job_finished_Detail
            End Get
            Set(ByVal value As String)
                _job_finished_Detail = value
            End Set
        End Property

        'Property job_order_type_Detail() As String
        '    Get
        '        Return _job_order_type_Detail
        '    End Get
        '    Set(ByVal value As String)
        '        _job_order_type_Detail = value
        '    End Set
        'End Property

        Property customer_Detail() As String
            Get
                Return _customer_Detail
            End Get
            Set(ByVal value As String)
                _customer_Detail = value
            End Set
        End Property

        Property total_amount_Detail() As String
            Get
                Return _total_amount_Detail
            End Get
            Set(ByVal value As String)
                _total_amount_Detail = value
            End Set
        End Property

        Property quotation_amount_Detail() As String
            Get
                Return _quotation_amount_Detail
            End Get
            Set(ByVal value As String)
                _quotation_amount_Detail = value
            End Set
        End Property

        Property payment_condition_detail() As String
            Get
                Return _payment_condition_detail
            End Get
            Set(ByVal value As String)
                _payment_condition_detail = value
            End Set
        End Property

        Property job_year_detail() As Integer
            Get
                Return _job_year_detail
            End Get
            Set(ByVal value As Integer)
                _job_year_detail = value
            End Set
        End Property

        Property job_month_detail() As Integer
            Get
                Return _job_month_detail
            End Get
            Set(ByVal value As Integer)
                _job_month_detail = value
            End Set
        End Property

        Property job_last_detail() As Integer
            Get
                Return _job_last_detail
            End Get
            Set(ByVal value As Integer)
                _job_last_detail = value
            End Set
        End Property

        Property sum_po_amount() As Decimal
            Get
                Return _sum_po_amount
            End Get
            Set(ByVal value As Decimal)
                _sum_po_amount = value
            End Set
        End Property

        Property sum_quo_amount() As Decimal
            Get
                Return _sum_quo_amount
            End Get
            Set(ByVal value As Decimal)
                _sum_quo_amount = value
            End Set
        End Property

        Property total_po_amount() As Decimal
            Get
                Return _total_po_amount
            End Get
            Set(ByVal value As Decimal)
                _total_po_amount = value
            End Set
        End Property

        Property po_type_detail() As String
            Get
                Return _po_type_detail
            End Get
            Set(ByVal value As String)
                _po_type_detail = value
            End Set
        End Property

        Property po_no_detail() As String
            Get
                Return _po_no_detail
            End Get
            Set(ByVal value As String)
                _po_no_detail = value
            End Set
        End Property

        Property po_amount_detail() As Decimal
            Get
                Return _po_amount_detail
            End Get
            Set(ByVal value As Decimal)
                _po_amount_detail = value
            End Set
        End Property

        Property po_date_detail() As String
            Get
                Return _po_date_detail
            End Get
            Set(ByVal value As String)
                _po_date_detail = value
            End Set
        End Property

        Property po_receipt_date_detail() As String
            Get
                Return _po_receipt_date_detail
            End Get
            Set(ByVal value As String)
                _po_receipt_date_detail = value
            End Set
        End Property

        Property po_file_detail() As String
            Get
                Return _po_file_detail
            End Get
            Set(ByVal value As String)
                _po_file_detail = value
            End Set
        End Property

        Property quo_type_detail() As String
            Get
                Return _quo_type_detail
            End Get
            Set(ByVal value As String)
                _quo_type_detail = value
            End Set
        End Property

        Property quo_no_detail() As String
            Get
                Return _quo_no_detail
            End Get
            Set(ByVal value As String)
                _quo_no_detail = value
            End Set
        End Property

        Property quo_amount_detail() As Decimal
            Get
                Return _quo_amount_detail
            End Get
            Set(ByVal value As Decimal)
                _quo_amount_detail = value
            End Set
        End Property

        Property quo_date_detail() As String
            Get
                Return _quo_date_detail
            End Get
            Set(ByVal value As String)
                _quo_date_detail = value
            End Set
        End Property

        Property quo_file_detail() As String
            Get
                Return _quo_file_detail
            End Get
            Set(ByVal value As String)
                _quo_file_detail = value
            End Set
        End Property

        Property status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
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

        Property inv_receipt_date() As String
            Get
                Return _inv_receipt_date
            End Get
            Set(ByVal value As String)
                _inv_receipt_date = value
            End Set
        End Property

        Property account_title() As String
            Get
                Return _account_title
            End Get
            Set(ByVal value As String)
                _account_title = value
            End Set
        End Property

        Property inv_issue_date() As String
            Get
                Return _inv_issue_date
            End Get
            Set(ByVal value As String)
                _inv_issue_date = value
            End Set
        End Property

        Property inv_total_amount() As Decimal
            Get
                Return _inv_total_amount
            End Get
            Set(ByVal value As Decimal)
                _inv_total_amount = value
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

        Property check_use() As Integer
            Get
                Return _check_use
            End Get
            Set(ByVal value As Integer)
                _check_use = value
            End Set
        End Property

#End Region

    End Class
End Namespace
