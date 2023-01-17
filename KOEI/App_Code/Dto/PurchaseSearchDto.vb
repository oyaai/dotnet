#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : PurchaseSearchDto
'	Class Discription	: Dto class Purchase Search
'	Create User 		: Boonyarit
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

Namespace Dto
    Public Class PurchaseSearchDto
    
        Private _type_purchase As Integer
        Private _po_no_from As String
        Private _po_no_to As String
        Private _issue_date_from As DateString
        Private _issue_date_to As DateString
        Private _delivery_date_from As DateString
        Private _delivery_date_to As DateString
        Private _vendor_name As String
        'Receive data from Purchase Approve screen (condition search)
        Private _job_order_from As String
        Private _job_order_to As String
        Private _issue_date_start As String
        Private _issue_date_end As String
        Private _status_ids As String
        Private _po_type As String 

#Region "Property"

        Public Property po_type() As String
            Get
                Return _po_type
            End Get
            Set(ByVal value As String)
                _po_type = value
            End Set
        End Property

        Public Property status_ids() As String
            Get
                Return _status_ids
            End Get
            Set(ByVal value As String)
                _status_ids = value
            End Set
        End Property

        Public Property issue_date_start() As String
            Get
                Return _issue_date_start
            End Get
            Set(ByVal value As String)
                _issue_date_start = value
            End Set
        End Property

        Public Property issue_date_end() As String
            Get
                Return _issue_date_end
            End Get
            Set(ByVal value As String)
                _issue_date_end = value
            End Set
        End Property

        Public Property job_order_from() As String
            Get
                Return _job_order_from
            End Get
            Set(ByVal value As String)
                _job_order_from = value
            End Set
        End Property

        Public Property job_order_to() As String
            Get
                Return _job_order_to
            End Get
            Set(ByVal value As String)
                _job_order_to = value
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

        Public Property delivery_date_to() As DateString
            Get
                Return _delivery_date_to
            End Get
            Set(ByVal value As DateString)
                _delivery_date_to = value
            End Set
        End Property

        Public Property delivery_date_from() As DateString
            Get
                Return _delivery_date_from
            End Get
            Set(ByVal value As DateString)
                _delivery_date_from = value
            End Set
        End Property

        Public Property issue_date_to() As DateString
            Get
                Return _issue_date_to
            End Get
            Set(ByVal value As DateString)
                _issue_date_to = value
            End Set
        End Property

        Public Property issue_date_from() As DateString
            Get
                Return _issue_date_from
            End Get
            Set(ByVal value As DateString)
                _issue_date_from = value
            End Set
        End Property

        Public Property po_no_to() As String
            Get
                Return _po_no_to
            End Get
            Set(ByVal value As String)
                _po_no_to = value
            End Set
        End Property

        Public Property po_no_from() As String
            Get
                Return _po_no_from
            End Get
            Set(ByVal value As String)
                _po_no_from = value
            End Set
        End Property

        Public Property type_purchase() As Integer
            Get
                Return _type_purchase
            End Get
            Set(ByVal value As Integer)
                _type_purchase = value
            End Set
        End Property
#End Region

    End Class
End Namespace


