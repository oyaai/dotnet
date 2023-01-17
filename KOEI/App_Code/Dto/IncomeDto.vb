#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : IncomeDto
'	Class Discription	: Dto class income 
'	Create User 		: Komsan L.
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
Imports Microsoft.VisualBasic

Namespace Dto
    Public Class IncomeDto

#Region "Variable"
        Private _accountID As String = Nothing
        Private _accountType As String = Nothing
        Private _jobOrder As String = Nothing
        Private _jobOrderTo As String = Nothing
        Private _vendorID As String = Nothing
        Private _VendorBranchID As String = Nothing
        Private _BranchName As String = Nothing
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
        Private _receiptDateTo As String = Nothing
        Private _remark As String = Nothing

        ' Add by Wasan D. on 31-07-2013
        Private _inputTotal As String = Nothing
        Private _currencyID As String = Nothing
        Private _currencyRate As String = Nothing

        Private _voucherNo As String = Nothing
        Private _vendorName As String = Nothing
        Private _dateNow As String = Nothing
        Private _chequeNo As String = Nothing
        Private _chequeDate As String = Nothing
        Private _subtotal As String = Nothing
        Private _accountTitle As String = Nothing
        Private _statusID As String = Nothing
#End Region

#Region "Property"
        Property AccountType() As String
            Get
                Return _accountType
            End Get
            Set(ByVal value As String)
                _accountType = value
            End Set
        End Property

        Property JobOrder() As String
            Get
                Return _jobOrder
            End Get
            Set(ByVal value As String)
                _jobOrder = value
            End Set
        End Property

        Property VendorID() As String
            Get
                Return _vendorID
            End Get
            Set(ByVal value As String)
                _vendorID = value
            End Set
        End Property

        Property VendorBranchID() As String
            Get
                Return _VendorBranchID
            End Get
            Set(ByVal value As String)
                _VendorBranchID = value
            End Set
        End Property

        Property BranchName() As String
            Get
                Return _BranchName
            End Get
            Set(ByVal value As String)
                _BranchName = value
            End Set
        End Property

        Property Vat() As String
            Get
                Return _vat
            End Get
            Set(ByVal value As String)
                _vat = value
            End Set
        End Property

        Property VatAmount() As String
            Get
                Return _vatAmount
            End Get
            Set(ByVal value As String)
                _vatAmount = value
            End Set
        End Property

        Property WT() As String
            Get
                Return _wt
            End Get
            Set(ByVal value As String)
                _wt = value
            End Set
        End Property

        Property WTAmount() As String
            Get
                Return _wtAmount
            End Get
            Set(ByVal value As String)
                _wtAmount = value
            End Set
        End Property

        Property Bank() As String
            Get
                Return _bank
            End Get
            Set(ByVal value As String)
                _bank = value
            End Set
        End Property

        Property AccountName() As String
            Get
                Return _accountName
            End Get
            Set(ByVal value As String)
                _accountName = value
            End Set
        End Property

        Property ItemExpense() As String
            Get
                Return _itemExpense
            End Get
            Set(ByVal value As String)
                _itemExpense = value
            End Set
        End Property

        Property AccountNo() As String
            Get
                Return _accountNo
            End Get
            Set(ByVal value As String)
                _accountNo = value
            End Set
        End Property

        Property Total() As String
            Get
                Return _total
            End Get
            Set(ByVal value As String)
                _total = value
            End Set
        End Property

        Property ReceiptDate() As String
            Get
                Return _receiptDate
            End Get
            Set(ByVal value As String)
                _receiptDate = value
            End Set
        End Property

        Property Remark() As String
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Property inputTotal() As String
            Get
                Return _inputTotal
            End Get
            Set(ByVal value As String)
                _inputTotal = value
            End Set
        End Property

        Property currencyID() As String
            Get
                Return _currencyID
            End Get
            Set(ByVal value As String)
                _currencyID = value
            End Set
        End Property

        Property currencyRate() As String
            Get
                Return _currencyRate
            End Get
            Set(ByVal value As String)
                _currencyRate = value
            End Set
        End Property

        Property voucherNo() As String
            Get
                Return _voucherNo
            End Get
            Set(ByVal value As String)
                _voucherNo = value
            End Set
        End Property

        Property vendorName() As String
            Get
                Return _vendorName
            End Get
            Set(ByVal value As String)
                _vendorName = value
            End Set
        End Property

        Property dateNow() As String
            Get
                Return _dateNow
            End Get
            Set(ByVal value As String)
                _dateNow = value
            End Set
        End Property

        Property chequeNo() As String
            Get
                Return _chequeNo
            End Get
            Set(ByVal value As String)
                _chequeNo = value
            End Set
        End Property

        Property chequeDate() As String
            Get
                Return _chequeDate
            End Get
            Set(ByVal value As String)
                _chequeDate = value
            End Set
        End Property

        Property subtotal() As String
            Get
                Return _subtotal
            End Get
            Set(ByVal value As String)
                _subtotal = value
            End Set
        End Property

        Property jobOrderTo() As String
            Get
                Return _jobOrderTo
            End Get
            Set(ByVal value As String)
                _jobOrderTo = value
            End Set
        End Property

        Property receiptDateTo() As String
            Get
                Return _receiptDateTo
            End Get
            Set(ByVal value As String)
                _receiptDateTo = value
            End Set
        End Property

        Property accountTitle() As String
            Get
                Return _accountTitle
            End Get
            Set(ByVal value As String)
                _accountTitle = value
            End Set
        End Property

        Property accountID() As String
            Get
                Return _accountID
            End Get
            Set(ByVal value As String)
                _accountID = value
            End Set
        End Property

        Property statusID() As String
            Get
                Return _statusID
            End Get
            Set(ByVal value As String)
                _statusID = value
            End Set
        End Property
#End Region
    End Class
End Namespace

