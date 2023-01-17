Namespace Dto
    Public Class AccountingSearchDto

#Region "Fields"

        Private _id As Int32?
        Private _start_date As DateString
        Private _end_date As DateString
        Private _start_job_order As String
        Private _end_job_order As String
        Private _account_type As Int32?
        Private _item_id As Int32?
        Private _start_item_code As String
        Private _end_item_code As String
        Private _vendor_name As String
        Private _start_po_no As String
        Private _end_po_no As String
        Private _status_id As Int32

#End Region

#Region "Properties"

        Public Property ID() As Int32?
            Get
                Return _id
            End Get
            Set(ByVal value As Int32?)
                _id = value
            End Set
        End Property

        Public Property StartDate() As DateString
            Get
                Return _start_date
            End Get
            Set(ByVal value As DateString)
                _start_date = value
            End Set
        End Property

        Public Property EndDate() As DateString
            Get
                Return _end_date
            End Get
            Set(ByVal value As DateString)
                _end_date = value
            End Set
        End Property

        Public Property StartJobOrder() As String
            Get
                Return _start_job_order
            End Get
            Set(ByVal value As String)
                _start_job_order = value
            End Set
        End Property

        Public Property EndJobOrder() As String
            Get
                Return _end_job_order
            End Get
            Set(ByVal value As String)
                _end_job_order = value
            End Set
        End Property

        Public Property AccountType() As Int32?
            Get
                Return _account_type
            End Get
            Set(ByVal value As Int32?)
                _account_type = value
            End Set
        End Property

        Public Property ItemID() As Int32?
            Get
                Return _item_id
            End Get
            Set(ByVal value As Int32?)
                _item_id = value
            End Set
        End Property

        Public Property StartItemCode() As String
            Get
                Return _start_item_code
            End Get
            Set(ByVal value As String)
                _start_item_code = value
            End Set
        End Property

        Public Property EndItemCode() As String
            Get
                Return _end_item_code
            End Get
            Set(ByVal value As String)
                _end_item_code = value
            End Set
        End Property

        Public Property VendorName() As String
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property

        Public Property StartPurchaseOrder() As String
            Get
                Return _start_po_no
            End Get
            Set(ByVal value As String)
                _start_po_no = value
            End Set
        End Property

        Public Property EndPurchaseOrder() As String
            Get
                Return _end_po_no
            End Get
            Set(ByVal value As String)
                _end_po_no = value
            End Set
        End Property

        Public Property StatusID() As Int32
            Get
                Return _status_id
            End Get
            Set(ByVal value As Int32)
                _status_id = value
            End Set
        End Property

#End Region

    End Class
End Namespace