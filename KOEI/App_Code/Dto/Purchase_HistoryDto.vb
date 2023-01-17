Imports Microsoft.VisualBasic

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : Purchase_HistoryDto
'	Class Discription	: Dto class Purchase History  
'	Create User 		: Nisa S.
'	Create Date		    : 19-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Namespace Dto

    Public Class Purchase_HistoryDto

        Private _job_order As String
        Private _po_no As String
        Private _invoice_no As String
        Private _delivery_amount As String
        Private _VendorName As String
        Private _ItemName As String
        Private _delivery_date As String
        Private _delivery_qty As Integer
        Private _status_id As Integer
        Private _code As String

        Private _delivery_date1 As String
        Private _delivery_date2 As String


#Region "Property"

        Public Property delivery_date2() As String
            Get
                Return _delivery_date2
            End Get
            Set(ByVal value As String)
                _delivery_date2 = value
            End Set
        End Property

        Public Property delivery_date1() As String
            Get
                Return _delivery_date1
            End Get
            Set(ByVal value As String)
                _delivery_date1 = value
            End Set
        End Property

        Public Property code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property status_id() As Integer
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property delivery_qty() As Integer
            Get
                Return _delivery_qty
            End Get
            Set(ByVal value As Integer)
                _delivery_qty = value
            End Set
        End Property

        Public Property delivery_date() As String
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property

        Public Property ItemName() As String
            Get
                Return _ItemName
            End Get
            Set(ByVal value As String)
                _ItemName = value
            End Set
        End Property

        Public Property VendorName() As String
            Get
                Return _VendorName
            End Get
            Set(ByVal value As String)
                _VendorName = value
            End Set
        End Property

        Public Property delivery_amount() As String
            Get
                Return _delivery_amount
            End Get
            Set(ByVal value As String)
                _delivery_amount = value
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

        Public Property po_no() As String
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property invoice_no() As String
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

#End Region

    End Class

End Namespace