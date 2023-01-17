#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IInvoice_PurchaseService
'	Class Discription	: Interface Invoice Purchase service
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
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
Imports System.Data

Namespace Service
    Public Interface IInvoice_PurchaseService
        Function GetInvoice_PurchaseList(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetInvoice_Header(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetInvoice_Detail(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function IsUsed(ByVal intItemID As Integer) As Boolean
        Function DeletePayment(ByVal intPaymentHeaderID As Integer) As Boolean
        Function ExecuteAccounting(ByVal itemConfirm As String) As Boolean
        Function GetTableReport(ByVal dtValues As DataTable) As DataTable
        Function GetPurchasePaidReport(ByVal itemConfirm As String) As System.Data.DataTable
        Function GetPO_List(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetPO_Header(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetPO_Detail(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetPO_Detail_Insert(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetPO_Header_Insert(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function InsertPayment(ByVal objInvHeadDto As Dto.Invoice_PurchaseDto, ByVal dtPaymentDetail As DataTable) As Boolean
        Function GetPaymentHeader(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function GetPaymentDetail(ByVal objInvPurchaseDto As Dto.Invoice_PurchaseDto) As DataTable
        Function UpdatePayment(ByVal objInvHeadDto As Dto.Invoice_PurchaseDto, ByVal dtPaymentDetail As DataTable) As Boolean
        Function checkConfirmPayment() As Boolean
        Function checkInvoice(ByVal vendor_id As String, ByVal invoice_no As String, ByVal id As String) As Boolean
    End Interface
End Namespace

