#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpReceive_HeaderEntity
'	Class Discription	: Class of table receive_header
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Class ImpReceive_HeaderEntity
        Implements IReceive_HeaderEntity

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
        Private _customer As Integer
        Private _issue_date As String
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String

#Region "Function"
        Public Function CheckReceiveByVendor(ByVal intVendor_id As Integer) As Boolean Implements IReceive_HeaderEntity.CheckReceiveByVendor
            Dim objReceive As New Dao.ImpReceive_HeaderDao
            Return objReceive.DB_CheckReceiveByVendor(intVendor_id)
        End Function
#End Region

#Region "Property"
        Public Property account_type() As Integer Implements IReceive_HeaderEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Public Property bank_fee() As Decimal Implements IReceive_HeaderEntity.bank_fee
            Get
                Return _bank_fee
            End Get
            Set(ByVal value As Decimal)
                _bank_fee = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IReceive_HeaderEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IReceive_HeaderEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property customer() As Integer Implements IReceive_HeaderEntity.customer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
            End Set
        End Property

        Public Property id() As Integer Implements IReceive_HeaderEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ie_id() As Integer Implements IReceive_HeaderEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IReceive_HeaderEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property invoice_type() As Integer Implements IReceive_HeaderEntity.invoice_type
            Get
                Return _invoice_type
            End Get
            Set(ByVal value As Integer)
                _invoice_type = value
            End Set
        End Property

        Public Property issue_date() As String Implements IReceive_HeaderEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property receipt_date() As String Implements IReceive_HeaderEntity.receipt_date
            Get
                Return _receipt_date
            End Get
            Set(ByVal value As String)
                _receipt_date = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IReceive_HeaderEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property total_amount() As Decimal Implements IReceive_HeaderEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IReceive_HeaderEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IReceive_HeaderEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property user_id() As Integer Implements IReceive_HeaderEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements IReceive_HeaderEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property
#End Region

    End Class
End Namespace

