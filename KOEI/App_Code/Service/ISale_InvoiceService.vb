#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ISale_InvoiceService
'	Class Discription	: Interface class sale invoice service
'	Create User 		: Suwishaya L.
'	Create Date		    : 28-06-2013
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

Namespace Service
    Public Interface ISale_InvoiceService
        Function IsUsedInAccounting(ByVal intRefID As Integer) As Boolean
        Function IsUsedInHontai(ByVal intJobOrder As Integer, ByVal intHontaiFlg As Integer) As Boolean
        Function IsUsedInJobOrder(ByVal strJobOrder As String) As Boolean
        Function IsUsedInReceiveHeader(ByVal strInvoice_no As String) As Boolean
        Function IsCustomerUsedInJobOrder(ByVal strJobOrder As String, ByVal intCustomer As Integer) As Boolean
        Function GetSaleInvoiceList(ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto) As DataTable
        Function DeleteSaleInvoice(ByVal intRefID As Integer, ByVal dtValues As System.Data.DataTable) As Boolean
        Function SaveSaleInvoice(ByVal intRefID As Integer, ByVal decExchangeRate As Decimal, ByVal decActualAmount As Decimal, ByVal strJobOrder As String) As Boolean
        Function GetSaleInvoiceHeaderByID(ByVal intRefID As Integer) As Dto.SaleInvoiceDto
        Function GetSaleInvoiceDetailByID(ByVal intRefID As Integer) As Dto.SaleInvoiceDto
        Function GetSaleInvoiceByJobOrder(ByVal strJobOrder As String) As Dto.SaleInvoiceDto
        Function GetSaleInvoiceReportList(ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto) As DataTable
        Function GetSumSaleInvoiceReportList(ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto) As Dto.SaleInvoiceDto
        Function GetSaleInvoiceDetailList(ByVal intRefID As Integer) As DataTable
        Function GetSaleInvoiceHeaderList(ByVal intRefID As Integer) As Dto.SaleInvoiceDto
        Function GetTotalSaleInvoiceAmount(ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto) As Dto.SaleInvoiceDto
        Function ConfirmReceive(ByVal invoiceHeaderId As String, ByVal dtValues As System.Data.DataTable, Optional ByVal dtBankFree As System.Data.DataTable = Nothing) As Boolean
        Function GetJobOrerSaleInvoiceDetail(ByVal strJobOrder As String) As DataTable
        Function GetJobOrerSaleInvoiceDetailEdit(ByVal intRefID As Integer) As DataTable
        Function InsertSaleInvoice(ByVal strJobOrder1 As String, ByVal strJobOrder2 As String, ByVal strJobOrder3 As String, ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, ByVal dtValues As System.Data.DataTable) As Boolean
        Function UpdateSaleInvoice(ByVal strIdDelete As String, ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, ByVal dtValues As System.Data.DataTable) As Boolean
        Function GetJobOrderHontai(ByVal dtValues As System.Data.DataTable) As Dto.SaleInvoiceDto
        Function GetHontaiFinish(ByVal intRefID As Integer) As Dto.SaleInvoiceDto
        Function GetAccountTitleForList() As List(Of Dto.SaleInvoiceDto)
        Function UpdateJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Boolean
        Function DeleteJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Boolean
        Function GetDataBankFreeList(ByVal strReceive_header_id As String) As DataTable
        Function GetDataReceiveDetail(ByVal strReceive_header_id As String) As DataTable
        Function GetConfirmReceiveForReport(ByVal strReceiceHeaderId As String) As DataTable
        Function GetSumConfirmReport(ByVal strReceiceHeaderId As String) As Dto.SaleInvoiceDto
        Function GetTableConfirmReport(ByVal dtValues As DataTable, ByVal objSaleInvoiceDto As Dto.SaleInvoiceDto, ByVal strVoucherNo As String, ByVal dtValuesBankFree As Dto.SaleInvoiceDto) As DataTable
        Function UpdateReciveDetail(ByVal tbIntIdJBPO As DataTable, ByVal objIDSaleInv As Dto.SaleInvoiceDto, ByVal intJobOrderID As Integer) As Boolean
        Function SumBankfeeConfirmReport(ByVal strReceive_header_id As String) As Dto.SaleInvoiceDto
        Function GetSaleInvoiceforUpdate(ByVal strJobOrder As String) As Dto.SaleInvoiceDto
        Function NewExChangeRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean
        Function GetExChangeRate(ByVal intID As Integer) As Integer
        Function UpdateExChangeRate(ByVal strReceiveHeaderId As String, ByVal objExChangeRate As System.Data.DataTable) As Boolean

    End Interface
End Namespace

