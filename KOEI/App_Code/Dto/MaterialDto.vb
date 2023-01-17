#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : MaterialDto
'	Class Discription	: Dto class Material
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
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
    Public Class MaterialDto

#Region "Fields"
        'Receive data from screen (condition search)
        Private _job_order As String
        Private _vendor As String
        Private _invoice_no As String
        Private _po_no As String
        Private _item_name As String
        Private _delivery_date_from As String
        Private _delivery_date_to As String

        'Receive data for detail search and report    
        Private _id As Integer
        Private _amount As String
        Private _delivery_date_in As String
        Private _qty_in As String
        Private _delivery_date_out As String
        Private _qty_out As String
        Private _qty_left As String
#End Region

#Region "Property"
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property amount() As String
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property delivery_date_from() As String
            Get
                Return _delivery_date_from
            End Get
            Set(ByVal value As String)
                _delivery_date_from = value
            End Set
        End Property

        Public Property delivery_date_to() As String
            Get
                Return _delivery_date_to
            End Get
            Set(ByVal value As String)
                _delivery_date_to = value
            End Set
        End Property

        Public Property delivery_date_in() As String
            Get
                Return _delivery_date_in
            End Get
            Set(ByVal value As String)
                _delivery_date_in = value
            End Set
        End Property

        Public Property delivery_date_out() As String
            Get
                Return _delivery_date_out
            End Get
            Set(ByVal value As String)
                _delivery_date_out = value
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

        Public Property item_name() As String
            Get
                Return _item_name
            End Get
            Set(ByVal value As String)
                _item_name = value
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

        Public Property qty_in() As String
            Get
                Return _qty_in
            End Get
            Set(ByVal value As String)
                _qty_in = value
            End Set
        End Property

        Public Property qty_left() As String
            Get
                Return _qty_left
            End Get
            Set(ByVal value As String)
                _qty_left = value
            End Set
        End Property

        Public Property qty_out() As String
            Get
                Return _qty_out
            End Get
            Set(ByVal value As String)
                _qty_out = value
            End Set
        End Property

        Public Property vendor() As String
            Get
                Return _vendor
            End Get
            Set(ByVal value As String)
                _vendor = value
            End Set
        End Property


#End Region

    End Class
End Namespace
