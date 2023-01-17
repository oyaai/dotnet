#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_JobTypeDao
'	Class Discription	: Interface of table receive_header
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
#End Region

Namespace Dao
    Public Interface ISale_InvoiceDao
        Function GetSaleInvoiceList(ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity) As List(Of Entity.ImpSale_InvoiceEntity)
        Function DeleteSaleInvoice(ByVal intRefID As Integer, ByVal dtValues As System.Data.DataTable) As Integer
        Function SaveSaleInvoice(ByVal intRefID As Integer, ByVal decExchangeRate As Decimal, ByVal decActualAmount As Decimal, ByVal strJobOrder As String) As Integer
        Function GetSaleInvoiceHeaderByID(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceDetailByID(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceByJobOrder(ByVal strJobOrder As String) As Entity.ISale_InvoiceEntity
        Function CountUsedInHontai(ByVal strJobOrderId As Integer, ByVal intHontaiFlg As Integer) As Integer
        Function CountUsedInReceiveHeader(ByVal strInvoice_no As String) As Integer
        Function CountUsedInJobOrder(ByVal strJobOrder As String) As Integer
        Function CountUsedInAccounting(ByVal intRefID As Integer) As Integer
        Function CountCustomerUsedInJobOrder(ByVal strJobOrder As String, ByVal intCustomer As Integer) As Integer
        Function GetSaleInvoiceDetailList(ByVal intRefID As Integer) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSaleInvoiceHeaderList(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceReportList(ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSumSaleInvoiceReportList(ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity) As Entity.ISale_InvoiceEntity
        Function GetTotalSaleInvoiceAmount(ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity) As Entity.ISale_InvoiceEntity
        Function ConfirmReceive(ByVal invoiceHeaderId As String, ByVal dtValues As System.Data.DataTable, Optional ByVal dtBankFree As System.Data.DataTable = Nothing) As Integer
        Function GetJobOrerSaleInvoiceDetail(ByVal strJobOrder As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetJobOrerSaleInvoiceDetailEdit(ByVal intRefID As Integer) As List(Of Entity.ImpSale_InvoiceEntity)
        Function InsertSaleInvoice(ByVal strJobOrder1 As String, ByVal strJobOrder2 As String, ByVal strJobOrder3 As String, ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer
        Function UpdateSaleInvoice(ByVal strIdDelete As String, ByVal objSaleInvoiceEnt As Entity.ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer
        Function GetJobOrderHontai(ByVal dtValues As System.Data.DataTable) As Entity.ISale_InvoiceEntity
        Function GetHontaiFinish(ByVal intRefID As Integer) As Entity.ISale_InvoiceEntity
        Function GetAccountTitleForList() As List(Of Entity.ISale_InvoiceEntity)
        Function UpdateJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer
        Function DeleteJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer
        Function GetDataBankFreeList(ByVal strReceive_header_id As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetDataReceiveDetail(ByVal strReceive_header_id As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetConfirmReceiveForReport(ByVal strReceiceHeaderId As String) As List(Of Entity.ImpSale_InvoiceEntity)
        Function GetSumConfirmReport(ByVal strReceiceHeaderId As String) As Entity.ISale_InvoiceEntity
        Function DelInvReceiveDetail(ByVal strIntID As Integer) As Integer
        Function UpdateReciveDetail(ByVal tbIntIdJBPO As System.Data.DataTable, ByVal objIDSaleInv As System.Data.DataTable, ByVal intJobOrderID As Integer) As Boolean
        Function SumBankfeeConfirmReport(ByVal strReceiceHeaderId As String) As Entity.ISale_InvoiceEntity
        Function GetSaleInvoiceforUpdate(ByVal strReceiceHeaderId As Integer) As Entity.ISale_InvoiceEntity
        Function UpExChangeRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean
        Function GetActualRate(ByVal intID As Integer) As Integer
        Function UpdateExChangeRate(ByVal intID As Integer, ByVal objExChangeRate As System.Data.DataTable) As Boolean


    End Interface
End Namespace
