#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : AccountingDto
'	Class Discription	: Dto class Accounting 
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

Namespace Dto
    Public Class AccountingDto
        'Get data from base and display on screen
        Private _voucher_no As String
        Private _account_date As String
        Private _cheque_no As String
        Private _vendor_name As String
        Private _Ie_name As String
        Private _job_order As String
        Private _income As Double
        Private _Expense As Double

        'Receive data from screen (condition search)
        Private _strAccount_id As String
        Private _strAccount_startdate As String
        Private _strAccount_enddate As String
        Private _strJoborder_start As String
        Private _strJoborder_end As String
        Private _strAccount_type As String
        Private _strIe_id As String
        Private _strVendor_name As String
        Private _strPo_startno As String
        Private _strPo_endno As String
        Private _strIe_start_code As String
        Private _strIe_end_code As String
        Private _strStatus_id As String

        Private _strJob_order_text As String

        Private _min_year As Integer
        Private _latest_year As Integer
        Private _strAccountMonth As String
        Private _strAccountYear As String

        'Receive data from screen (Detail screen)
        Private _id As String
        Private _strIe_category_type As String
        Private _accType As String
#Region "Property"
        'Get data from base and display on screen
        Property strIe_category_type() As String
            Get
                Return _strIe_category_type
            End Get
            Set(ByVal value As String)
                _strIe_category_type = value
            End Set
        End Property
        Property accType() As String
            Get
                Return _accType
            End Get
            Set(ByVal value As String)
                _accType = value
            End Set
        End Property
        Property strAccountMonth() As String
            Get
                Return _strAccountMonth
            End Get
            Set(ByVal value As String)
                _strAccountMonth = value
            End Set
        End Property
        Property strAccountYear() As String
            Get
                Return _strAccountYear
            End Get
            Set(ByVal value As String)
                _strAccountYear = value
            End Set
        End Property
        Property latest_year() As Integer
            Get
                Return _latest_year
            End Get
            Set(ByVal value As Integer)
                _latest_year = value
            End Set
        End Property
        Property min_year() As Integer
            Get
                Return _min_year
            End Get
            Set(ByVal value As Integer)
                _min_year = value
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

        Property account_date() As String
            Get
                Return _account_date
            End Get
            Set(ByVal value As String)
                _account_date = value
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
        Property job_order() As String
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
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

        'Receive data from screen (condition search)
        Property strStatus_id() As String
            Get
                Return _strStatus_id
            End Get
            Set(ByVal value As String)
                _strStatus_id = value
            End Set
        End Property
        Property strIe_end_code() As String
            Get
                Return _strIe_end_code
            End Get
            Set(ByVal value As String)
                _strIe_end_code = value
            End Set
        End Property
        Property strIe_start_code() As String
            Get
                Return _strIe_start_code
            End Get
            Set(ByVal value As String)
                _strIe_start_code = value
            End Set
        End Property
        Property strPo_endno() As String
            Get
                Return _strPo_endno
            End Get
            Set(ByVal value As String)
                _strPo_endno = value
            End Set
        End Property
        Property strPo_startno() As String
            Get
                Return _strPo_startno
            End Get
            Set(ByVal value As String)
                _strPo_startno = value
            End Set
        End Property
        Property strVendor_name() As String
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Property strIe_id() As String
            Get
                Return _strIe_id
            End Get
            Set(ByVal value As String)
                _strIe_id = value
            End Set
        End Property
        Property strAccount_type() As String
            Get
                Return _strAccount_type
            End Get
            Set(ByVal value As String)
                _strAccount_type = value
            End Set
        End Property
        Property strJoborder_end() As String
            Get
                Return _strJoborder_end
            End Get
            Set(ByVal value As String)
                _strJoborder_end = value
            End Set
        End Property
        Property strJoborder_start() As String
            Get
                Return _strJoborder_start
            End Get
            Set(ByVal value As String)
                _strJoborder_start = value
            End Set
        End Property
        Property strAccount_enddate() As String
            Get
                Return _strAccount_enddate
            End Get
            Set(ByVal value As String)
                _strAccount_enddate = value
            End Set
        End Property
        Property strAccount_startdate() As String
            Get
                Return _strAccount_startdate
            End Get
            Set(ByVal value As String)
                _strAccount_startdate = value
            End Set
        End Property
        Property strAccount_id() As String
            Get
                Return _strAccount_id
            End Get
            Set(ByVal value As String)
                _strAccount_id = value
            End Set
        End Property
        'Receive data from screen (Detail screen)
        Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Property strJob_order_text() As String
            Get
                Return _strJob_order_text
            End Get
            Set(ByVal value As String)
                _strJob_order_text = value
            End Set
        End Property
#End Region

    End Class
End Namespace

