#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IJobOrderService
'	Class Discription	: Interface class Job Order service
'	Create User 		: Suwishaya L.
'	Create Date		    : 10-06-2013
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
    Public Interface IJobOrderService
        Function CheckJobOrderByPurchase(ByVal strJobOrderId As String) As Boolean
        Function SetListJobOrder(ByRef objValue As DropDownList) As Boolean

        'function for menagement job order
        Function GetJobOrderList(ByVal objJobOrderDto As Dto.JobOrderDto) As DataTable
        Function GetJobOrderReportList(ByVal objJobOrderDto As Dto.JobOrderDto, ByVal blnPermission As Boolean) As DataTable
        Function GetSumHontaiAmountReport(ByVal objJobOrderDto As Dto.JobOrderDto, ByVal blnPermission As Boolean) As DataTable
        Function GetDeleteJobOrderList(ByVal objJobOrderDto As Dto.JobOrderDto) As DataTable
        Function DeleteJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Boolean
        Function IsUsedInJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Boolean
        Function IsUsedInJobOrderPo(ByVal intJobOrderID As Integer) As Boolean
        Function DeleteJobOrderTemp(ByVal strIpAddress As String) As Boolean
        Function DeleteJobTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Boolean
        Function GetJobOrderByID(ByVal intJobOrderID As Integer) As Dto.JobOrderDto
        Function GetJobOrderRunning(ByVal intIssueMonth As Integer, ByVal intIssueYear As Integer) As DataTable
        Function InsertJobOrderTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Boolean
        Function GetSumPoAmount(ByVal strIpAddress As String) As DataTable
        Function GetSumQuoAmount(ByVal strIpAddress As String) As DataTable
        Function GetTotalAmount(ByVal strIpAddress As String) As DataTable
        Function GetSumUploadPoAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As DataTable
        Function GetUploadTotalAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As DataTable
        Function InsertJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderDto As Dto.JobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderDto As Dto.JobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function RestoreJobOrder(ByVal objJobOrderDto As Dto.JobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetJobOrderDetailList(ByVal intJobOrderID As Integer) As Dto.JobOrderDto
        Function GetJobOrderPOList(ByVal intJobOrderID As Integer) As DataTable
        Function GetJobOrderQuoList(ByVal intJobOrderID As Integer) As DataTable
        Function GetJobOrderInvoiceList(ByVal intJobOrderID As Integer) As DataTable
        Function GetPaymentConditionDetail(ByVal intPayment_condition_id As Integer) As DataTable
        Function GetJobOrderPOTempList(ByVal strIpAddress As String) As DataTable
        Function GetJobOrderQuoTempList(ByVal strIpAddress As String) As DataTable
        Function GetTableReport(ByVal dtValues As DataTable, ByVal dtSumValues As DataTable) As DataTable

        'function for menagement job order po temp
        Function CheckExistPoType(ByVal strIpAddress As String) As Boolean
        Function CheckExistPoNoTemp(ByVal strPoNo As String) As Boolean
        Function CheckExistPoNo(ByVal strPoNo As String, Optional ByVal strJobOrderId As String = "") As Boolean
        Function CheckExistPoFile(ByVal strPoFile As String, ByVal strIpAddress As String) As Boolean
        Function CheckExistPoTemp(ByVal intId As Integer, ByVal strIpAddress As String) As Boolean
        Function InsertPoTemp(ByVal objJobOrderDto As Dto.JobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function DeletePOTemp(ByVal intId As Integer) As Boolean
        Function GetOneJobOrderPOTempList(ByVal intPoId As Integer) As DataTable
        Function UpdateJobOrderPOToTempList(ByVal intPOId As Integer, ByVal objJobOrderDto As Dto.JobOrderDto, ByVal strIpAddress As String, Optional ByRef strMsg As String = "") As Boolean

        'function for menagement job order quo temp 
        Function CheckExistQuoNoTemp(ByVal strQuoNo As String) As Boolean
        Function CheckExistQuoNo(ByVal strQuoNo As String, Optional ByVal strJobOrderId As String = "") As Boolean
        Function CheckExistQuoFile(ByVal strQuoFile As String, ByVal strIpAddress As String) As Boolean
        Function CheckExistReceiveDetail(ByVal intJobOrderId As Integer) As Boolean
        Function InsertQuoTemp(ByVal objJobOrderDto As Dto.JobOrderDto, Optional ByRef strMsg As String = "") As Boolean
        Function DeleteQuoTemp(ByVal intId As Integer) As Boolean
        Function GetOneQuoFromTmp(ByVal intQuoId As Integer) As DataTable
        Function UpdateQuotationToTmp(ByVal objJobQuoDto As Dto.JobOrderDto, ByVal intQuoId As Integer, Optional ByRef Msg As String = "") As Boolean

    End Interface
End Namespace
