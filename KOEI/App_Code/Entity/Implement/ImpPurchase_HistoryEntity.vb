Imports Microsoft.VisualBasic

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpPurchase_HistoryEntity
'	Class Discription	: Class of table Purchase_History  
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

Namespace Entity

    Public Class ImpPurchase_HistoryEntity
        Implements IPurchase_HistoryEntity

        Private objPurchaseHistory As New ImpPurchase_HistoryDao

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


#Region "Function"

        Public Function GetPurchaseHistoryReport(ByVal objPurchaseHistoryEnt As IPurchase_HistoryEntity) As System.Collections.Generic.List(Of ImpPurchase_HistoryEntity) Implements IPurchase_HistoryEntity.GetPurchaseHistoryReport
            Return objPurchaseHistory.GetPurchaseHistoryReport(objPurchaseHistoryEnt)
        End Function

#End Region

#Region "Property"

        Public Property delivery_date2() As String Implements IPurchase_HistoryEntity.delivery_date2
            Get
                Return _delivery_date2
            End Get
            Set(ByVal value As String)
                _delivery_date2 = value
            End Set
        End Property

        Public Property delivery_date1() As String Implements IPurchase_HistoryEntity.delivery_date1
            Get
                Return _delivery_date1
            End Get
            Set(ByVal value As String)
                _delivery_date1 = value
            End Set
        End Property

        Public Property code() As String Implements IPurchase_HistoryEntity.code
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IPurchase_HistoryEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property delivery_qty() As Integer Implements IPurchase_HistoryEntity.delivery_qty
            Get
                Return _delivery_qty
            End Get
            Set(ByVal value As Integer)
                _delivery_qty = value
            End Set
        End Property

        Public Property delivery_date() As String Implements IPurchase_HistoryEntity.delivery_date
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property

        Public Property ItemName() As String Implements IPurchase_HistoryEntity.ItemName
            Get
                Return _ItemName
            End Get
            Set(ByVal value As String)
                _ItemName = value
            End Set
        End Property

        Public Property VendorName() As String Implements IPurchase_HistoryEntity.VendorName
            Get
                Return _VendorName
            End Get
            Set(ByVal value As String)
                _VendorName = value
            End Set
        End Property

        Public Property delivery_amount() As String Implements IPurchase_HistoryEntity.delivery_amount
            Get
                Return _delivery_amount
            End Get
            Set(ByVal value As String)
                _delivery_amount = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IPurchase_HistoryEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property po_no() As String Implements IPurchase_HistoryEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property job_order() As String Implements IPurchase_HistoryEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property
#End Region

    End Class

End Namespace