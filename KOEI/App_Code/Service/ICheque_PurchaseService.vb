#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ICheque_PurchaseService
'	Class Discription	: Interface Rating Purchase service
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
    Public Interface ICheque_PurchaseService
        Function GetCheque_PurchaseList(ByVal objRatingDto As Dto.Cheque_PurchaseDto) As DataTable
        Function GetCheque_Head(ByVal strChequeNo As String, ByVal strChequeDate As String) As DataTable
        Function GetCheque_Detail(ByVal strChequeNo As String, ByVal strChequeDate As String) As DataTable
        'Function GetApprover() As DataTable
        Function GetPurchasePaidReport(ByVal itemConfirm As String) As System.Data.DataTable
        Function GetPaymentVoucher(ByVal itemConfirm As String) As System.Data.DataTable
        Function GetTaxReport(ByVal itemConfirm As String) As System.Data.DataTable
        Function GetAccountReport(ByVal itemConfirm As String) As System.Data.DataTable
        Function DeleteCheque( ByVal strId As String) As Boolean
        Function GetAccounting_Detail( _
            ByVal id As String, _
            ByVal dtDate As String, _
            ByVal mode As String, _
            ByRef sumScheduleRate As Double, _
            ByRef sumBankRate As Double, _
            ByRef sVat As Double, _
            ByRef sumWT As Double _
        ) As DataTable

        Function UpdateAccounting(ByVal strApprover As String, ByVal dtInsAcc As DataTable, ByRef errorType As String) As Boolean

        'Function GetRatingVendor(ByVal ratingId As String, ByVal payment_header_id As String) As DataTable
        Function CheckDupAccounting(ByVal cheque_no As String, ByVal cheque_date As String) As Boolean
        'Function InsUpdVendor_Rating(ByVal mode As String, _
        '                             ByVal strId As String, _
        '                             ByVal strPayment_header_id As String, _
        '                             ByVal strQuality As String, _
        '                             ByVal strDelivery As String, _
        '                             ByVal strService As String, _
        '                             Optional ByRef strMsg As String = "") As Boolean
    End Interface
End Namespace

