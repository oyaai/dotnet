#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IAcc_PaymentEntity
'	Class Discription	: Interface of table accounting for Accounting Imcome & Payment
'	Create User 		: Wasan D.
'	Create Date		    : 09-08-2013
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
    Public Class ImpAcc_PaymentEntity
        Implements IAcc_PaymentEntity

        Private _vendorAddress As String = Nothing
        Private _vendor_type1 As String = Nothing
        Private _vendor_type2 As String = Nothing
        Private _vendor_type2_no As String = Nothing
        Private _WtType As String = Nothing
        Private _accID As String = Nothing
        Private _accountType As String = Nothing
        Private _jobOrder As String = Nothing
        Private _vendorID As String = Nothing
        Private _vat As String = Nothing
        Private _vatAmount As String = Nothing
        Private _wt As String = Nothing
        Private _wtAmount As String = Nothing
        Private _bank As String = Nothing
        Private _accountName As String = Nothing
        Private _itemExpense As String = Nothing
        Private _accountNo As String = Nothing
        Private _total As String = Nothing
        Private _receiptDate As String = Nothing
        Private _receiptYear As String = Nothing
        Private _receiptMonth As String = Nothing
        Private _remark As String = Nothing

        ' Add by Wasan D. on 31-07-2013
        Private _inputTotal As String = Nothing
        Private _currencyID As String = Nothing
        Private _currencyRate As String = Nothing

        Private _voucherNo As String = Nothing
        Private _voucherNoAsInt As Integer = Nothing
        Private _vendorName As String = Nothing
        Private _dateNow As String = Nothing
        Private _chequeNo As String = Nothing
        Private _chequeDate As String = Nothing
        Private _subtotal As String = Nothing

        Private objAccounting As New Dao.ImpAcc_PaymentDao


#Region "Function"

        Public Function GetDataForVoucherReport(ByVal voucherList As String) As System.Collections.Generic.List(Of Entity.ImpAcc_PaymentEntity) Implements IAcc_PaymentEntity.GetDataForVoucherReport
            Return objAccounting.GetDataForVoucherReport(voucherList)
        End Function

        Public Function GetDataForWTReport(ByVal voucherList As String) As System.Collections.Generic.List(Of Entity.ImpAcc_PaymentEntity) Implements IAcc_PaymentEntity.GetDataForWTReport
            Return objAccounting.GetDataForWTReport(voucherList)
        End Function

        Public Function GetDataForWTReportV2(ByVal voucherList As String) As System.Collections.Generic.List(Of ImpAcc_PaymentEntity) Implements IAcc_PaymentEntity.GetDataForWTReportV2
            Return objAccounting.GetDataForWTReportV2(voucherList)
        End Function
#End Region

#Region "Property"
        Public Property accountName() As String Implements IAcc_PaymentEntity.accountName
            Get
                Return _accountName
            End Get
            Set(ByVal value As String)
                _accountName = value
            End Set
        End Property

        Public Property accountNo() As String Implements IAcc_PaymentEntity.accountNo
            Get
                Return _accountNo
            End Get
            Set(ByVal value As String)
                _accountNo = value
            End Set
        End Property

        Public Property accountType() As String Implements IAcc_PaymentEntity.accountType
            Get
                Return _accountType
            End Get
            Set(ByVal value As String)
                _accountType = value
            End Set
        End Property

        Public Property bank() As String Implements IAcc_PaymentEntity.bank
            Get
                Return _bank
            End Get
            Set(ByVal value As String)
                _bank = value
            End Set
        End Property

        Public Property chequeDate() As String Implements IAcc_PaymentEntity.chequeDate
            Get
                Return _chequeDate
            End Get
            Set(ByVal value As String)
                _chequeDate = value
            End Set
        End Property

        Public Property chequeNo() As String Implements IAcc_PaymentEntity.chequeNo
            Get
                Return _chequeNo
            End Get
            Set(ByVal value As String)
                _chequeNo = value
            End Set
        End Property

        Public Property currencyID() As String Implements IAcc_PaymentEntity.currencyID
            Get
                Return _currencyID
            End Get
            Set(ByVal value As String)
                _currencyID = value
            End Set
        End Property

        Public Property currencyRate() As String Implements IAcc_PaymentEntity.currencyRate
            Get
                Return _currencyRate
            End Get
            Set(ByVal value As String)
                _currencyRate = value
            End Set
        End Property

        Public Property dateNow() As String Implements IAcc_PaymentEntity.dateNow
            Get
                Return _dateNow
            End Get
            Set(ByVal value As String)
                _dateNow = value
            End Set
        End Property

        Public Property inputTotal() As String Implements IAcc_PaymentEntity.inputTotal
            Get
                Return _inputTotal
            End Get
            Set(ByVal value As String)
                _inputTotal = value
            End Set
        End Property

        Public Property itemExpense() As String Implements IAcc_PaymentEntity.itemExpense
            Get
                Return _itemExpense
            End Get
            Set(ByVal value As String)
                _itemExpense = value
            End Set
        End Property

        Public Property jobOrder() As String Implements IAcc_PaymentEntity.jobOrder
            Get
                Return _jobOrder
            End Get
            Set(ByVal value As String)
                _jobOrder = value
            End Set
        End Property

        Public Property receiptDate() As String Implements IAcc_PaymentEntity.receiptDate
            Get
                Return _receiptDate
            End Get
            Set(ByVal value As String)
                _receiptDate = value
            End Set
        End Property

        Public Property remark() As String Implements IAcc_PaymentEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property subtotal() As String Implements IAcc_PaymentEntity.subtotal
            Get
                Return _subtotal
            End Get
            Set(ByVal value As String)
                _subtotal = value
            End Set
        End Property

        Public Property total() As String Implements IAcc_PaymentEntity.total
            Get
                Return _total
            End Get
            Set(ByVal value As String)
                _total = value
            End Set
        End Property

        Public Property vat() As String Implements IAcc_PaymentEntity.vat
            Get
                Return _vat
            End Get
            Set(ByVal value As String)
                _vat = value
            End Set
        End Property

        Public Property vatAmount() As String Implements IAcc_PaymentEntity.vatAmount
            Get
                Return _vatAmount
            End Get
            Set(ByVal value As String)
                _vatAmount = value
            End Set
        End Property

        Public Property vendorID() As String Implements IAcc_PaymentEntity.vendorID
            Get
                Return _vendorID
            End Get
            Set(ByVal value As String)
                _vendorID = value
            End Set
        End Property

        Public Property vendorName() As String Implements IAcc_PaymentEntity.vendorName
            Get
                Return _vendorName
            End Get
            Set(ByVal value As String)
                _vendorName = value
            End Set
        End Property

        Public Property voucherNo() As String Implements IAcc_PaymentEntity.voucherNo
            Get
                Return _voucherNo
            End Get
            Set(ByVal value As String)
                _voucherNo = value
            End Set
        End Property

        Public Property wt() As String Implements IAcc_PaymentEntity.wt
            Get
                Return _wt
            End Get
            Set(ByVal value As String)
                _wt = value
            End Set
        End Property

        Public Property wtAmount() As String Implements IAcc_PaymentEntity.wtAmount
            Get
                Return _wtAmount
            End Get
            Set(ByVal value As String)
                _wtAmount = value
            End Set
        End Property

        Public Property accID() As String Implements IAcc_PaymentEntity.accID
            Get
                Return _accID
            End Get
            Set(ByVal value As String)
                _accID = value
            End Set
        End Property

        Public Property vendor_type1() As String Implements IAcc_PaymentEntity.vendor_type1
            Get
                Return _vendor_type1
            End Get
            Set(ByVal value As String)
                _vendor_type1 = value
            End Set
        End Property

        Public Property vendor_type2() As String Implements IAcc_PaymentEntity.vendor_type2
            Get
                Return _vendor_type2
            End Get
            Set(ByVal value As String)
                _vendor_type2 = value
            End Set
        End Property

        Public Property vendor_type2_no() As String Implements IAcc_PaymentEntity.vendor_type2_no
            Get
                Return _vendor_type2_no
            End Get
            Set(ByVal value As String)
                _vendor_type2_no = value
            End Set
        End Property

        Public Property vendorAddress() As String Implements IAcc_PaymentEntity.vendorAddress
            Get
                Return _vendorAddress
            End Get
            Set(ByVal value As String)
                _vendorAddress = value
            End Set
        End Property

        Public Property wtType() As String Implements IAcc_PaymentEntity.wtType
            Get
                Return _WtType
            End Get
            Set(ByVal value As String)
                _WtType = value
            End Set
        End Property

        Public Property receiptMonth() As String Implements IAcc_PaymentEntity.receiptMonth
            Get
                Return _receiptMonth
            End Get
            Set(ByVal value As String)
                _receiptMonth = value
            End Set
        End Property

        Public Property receiptYear() As String Implements IAcc_PaymentEntity.receiptYear
            Get
                Return _receiptYear
            End Get
            Set(ByVal value As String)
                _receiptYear = value
            End Set
        End Property

        Public Property voucherNoAsInt() As Integer Implements IAcc_PaymentEntity.voucherNoAsInt
            Get
                Return _voucherNoAsInt
            End Get
            Set(ByVal value As Integer)
                _voucherNoAsInt = value
            End Set
        End Property
#End Region

    End Class
End Namespace

