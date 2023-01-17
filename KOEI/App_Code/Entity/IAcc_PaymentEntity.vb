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
Imports System.Data

#End Region

Namespace Entity
    Public Interface IAcc_PaymentEntity

        Property accID() As String
        Property accountType() As String
        Property jobOrder() As String
        Property vendorID() As String
        Property vat() As String
        Property vatAmount() As String
        Property wt() As String
        Property wtAmount() As String
        Property wtType() As String
        Property bank() As String
        Property accountName() As String
        Property itemExpense() As String
        Property accountNo() As String
        Property total() As String
        Property receiptDate() As String
        Property receiptYear() As String
        Property receiptMonth() As String
        Property remark() As String

        ' Add by Wasan D. on 31-07-2013
        Property inputTotal() As String
        Property currencyID() As String
        Property currencyRate() As String

        Property voucherNo() As String
        Property voucherNoAsInt() As Integer
        Property vendorName() As String
        Property dateNow() As String
        Property chequeNo() As String
        Property chequeDate() As String
        Property subtotal() As String

        Property vendorAddress() As String
        Property vendor_type1() As String
        Property vendor_type2() As String
        Property vendor_type2_no() As String

        Function GetDataForVoucherReport(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
        Function GetDataForWTReport(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
        Function GetDataForWTReportV2(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
    End Interface
End Namespace

