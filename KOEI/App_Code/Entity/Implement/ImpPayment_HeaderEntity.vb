#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpPayment_HeaderEntity
'	Class Discription	: Class of table payment_header
'	Create User 		: Boonyarit
'	Create Date		    : 14-06-2013
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
    Public Class ImpPayment_HeaderEntity
        Implements IPayment_HeaderEntity

        Private _id As Integer
        Private _po_header_id As Integer
        Private _delivery_date As String
        Private _payment_date As String
        Private _invoice_no As String
        Private _account_type As Integer
        Private _account_no As String
        Private _account_name As String
        Private _total_delivery_amount As Decimal
        Private _remark As String
        Private _user_id As Integer
        Private _status_id As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

#Region "Function"
        Public Function CheckPaymentByPurchase(ByVal intPurchase_id As Integer) As Boolean Implements IPayment_HeaderEntity.CheckPaymentByPurchase
            Dim objDao As New Dao.ImpPayment_HeaderDao
            Return objDao.DB_CheckPaymentByPurchase(intPurchase_id)
        End Function
#End Region

#Region "Property"
        Public Property account_name() As String Implements IPayment_HeaderEntity.account_name
            Get
                Return _account_name
            End Get
            Set(ByVal value As String)
                _account_name = value
            End Set
        End Property

        Public Property account_no() As String Implements IPayment_HeaderEntity.account_no
            Get
                Return _account_no
            End Get
            Set(ByVal value As String)
                _account_no = value
            End Set
        End Property

        Public Property account_type() As Integer Implements IPayment_HeaderEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IPayment_HeaderEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IPayment_HeaderEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delivery_date() As String Implements IPayment_HeaderEntity.delivery_date
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property

        Public Property id() As Integer Implements IPayment_HeaderEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IPayment_HeaderEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property payment_date() As String Implements IPayment_HeaderEntity.payment_date
            Get
                Return _payment_date
            End Get
            Set(ByVal value As String)
                _payment_date = value
            End Set
        End Property

        Public Property po_header_id() As Integer Implements IPayment_HeaderEntity.po_header_id
            Get
                Return _po_header_id
            End Get
            Set(ByVal value As Integer)
                _po_header_id = value
            End Set
        End Property

        Public Property remark() As String Implements IPayment_HeaderEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IPayment_HeaderEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property total_delivery_amount() As Decimal Implements IPayment_HeaderEntity.total_delivery_amount
            Get
                Return _total_delivery_amount
            End Get
            Set(ByVal value As Decimal)
                _total_delivery_amount = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IPayment_HeaderEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IPayment_HeaderEntity.update_date
            Get
                Return _update_date
            End Get
            Set(ByVal value As String)
                _update_date = value
            End Set
        End Property

        Public Property user_id() As Integer Implements IPayment_HeaderEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property
#End Region

    End Class
End Namespace

