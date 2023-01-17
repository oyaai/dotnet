#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IJob_OrderDao
'	Class Discription	: Interface of table job_order
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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
    Public Interface IJob_OrderDao
        Function DB_CheckJobOrderByVendor(ByVal intVendor_id As Integer) As Boolean
        Function DB_GetJobOrderForList() As List(Of Entity.IJob_OrderEntity)
        Function DB_CheckJobOrderByPurchase(ByVal strJobOrder As String) As Boolean
        'function for menagement job order 
        Function GetJobOrderList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetDeleteJobOrderList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetSumHontaiAmountReport(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderReportList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function DeleteJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Integer
        Function CheckUseInPodetail(ByVal strJobOrder As String) As Integer
        Function CheckUseInOrderPo(ByVal intJobOrderID As Integer) As Integer
        Function CheckUseInJobOrderPo(ByVal intJobOrderID As Integer) As Integer
        Function CheckUseInOrderQuo(ByVal intJobOrderID As Integer) As Integer
        Function CheckUseInRecDetail(ByVal intJobOrderID As Integer) As Integer
        Function CheckUseInAccounting(ByVal strJobOrder As String) As Integer
        Function CheckUseInStock(ByVal strJobOrder As String) As Integer
        Function CheckUseInStockOut(ByVal strJobOrder As String) As Integer
        Function DeleteJobOrderTemp(ByVal strIpAddress As String) As Integer
        Function DeleteJobTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Integer
        Function GetJobOrderByID(ByVal intJobOrderID As Integer) As Entity.IJob_OrderEntity
        Function GetJobOrderRunning(ByVal intIssueMonth As Integer, ByVal intIssueYear As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function InsertJobOrderTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Integer
        Function GetSumPoAmount(ByVal strIpAddress As String) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetSumQuoAmount(ByVal strIpAddress As String) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetTotalAmount(ByVal strIpAddress As String) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetSumUploadPoAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetUploadTotalAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function InsertJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As Integer
        Function UpdateJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As Integer
        Function RestoreJobOrder(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As Integer
        Function GetJobOrderDetailList(ByVal intJobOrderID As Integer) As Entity.IJob_OrderEntity
        Function GetJobOrderPOList(ByVal intJobOrderID As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderQuoList(ByVal intJobOrderID As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderInvoiceList(ByVal intJobOrderID As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetPaymentConditionDetail(ByVal intPayment_condition_id As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderPOTempList(ByVal strIpAddress As String) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderQuoTempList(ByVal strIpAddress As String) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetOneJobOrderPOTempList(ByVal intPoId As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)

        'function for menagement job order po temp 
        Function CheckExistPoType(ByVal strIpAddress As String) As Integer
        Function CheckExistPoNoTemp(ByVal strPoNo As String) As Integer
        Function CheckExistPoNo(ByVal strPoNo As String, Optional ByVal strJobOrderId As String = "") As Integer
        Function CheckExistPoFile(ByVal strPoFile As String, ByVal strIpAddress As String) As Integer
        Function CheckExistPoTemp(ByVal intId As Integer, ByVal strIpAddress As String) As Integer
        Function InsertPoTemp(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As Integer
        Function DeletePOTemp(ByVal intId As Integer) As Integer
        Function UpdateJobOrderPOToTempList(ByVal intJobOrderId As Integer, ByVal objJobOrderEnt As Entity.IJob_OrderEntity, ByVal strIpAddress As String) As Integer

        'function for menagement job order quo temp 
        Function CheckExistQuoNoTemp(ByVal strQuoNo As String) As Integer
        Function CheckExistQuoNo(ByVal strQuoNo As String, Optional ByVal strJobOrderId As String = "") As Integer
        Function CheckExistQuoFile(ByVal strQuoFile As String, ByVal strIpAddress As String) As Integer
        Function CheckExistReceiveDetail(ByVal intJobOrderId As Integer) As Integer
        Function InsertQuoTemp(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As Integer
        Function DeleteQuoTemp(ByVal intId As Integer) As Integer
        Function GetOneQuoFromTmp(ByVal intQuoId As Integer) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function UpdateQuotationToTmp(ByVal objJobQuo As Entity.IJob_OrderEntity, ByVal intQuoId As Integer) As Integer

    End Interface
End Namespace

