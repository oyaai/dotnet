#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : IncomeJobOrderDto
'	Class Discription	: Dto class Income Job Order
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

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class IncomeJobOrderDto

#Region "Fields"
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

        'Receive data from screen (condition search)
        Private _invoice_no_search As String
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _receipt_date_from_search As String
        Private _receipt_date_to_search As String

#End Region

#Region "Property"
        Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Property invoice_no() As Integer
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As Integer)
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

        Property account_type() As Integer
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Property invoice_type() As Integer
            Get
                Return _invoice_type
            End Get
            Set(ByVal value As Integer)
                _invoice_type = value
            End Set
        End Property

        Property bank_fee() As Decimal
            Get
                Return _bank_fee
            End Get
            Set(ByVal value As Decimal)
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

        Property customer() As Integer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
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

#End Region
    End Class
End Namespace

