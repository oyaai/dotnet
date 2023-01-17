#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IJob_OrderEntity
'	Class Discription	: Interface of table job_order
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Interface IJob_OrderEntity
        Property id() As Integer
        Property job_order() As String
        Property old_job_order() As String
        Property issue_date() As String
        Property customer() As Integer
        Property end_user() As Integer
        Property receive_po() As Integer
        Property person_in_charge() As Integer
        Property job_type_id() As Integer
        Property is_boi() As Integer
        Property create_at() As Integer
        Property part_name() As String
        Property part_no() As String
        Property part_type() As Integer
        Property payment_term_id() As Integer
        Property currency_id() As Integer
        Property hontai_chk1() As Integer
        Property hontai_date1() As String
        Property hontai_amount1() As Decimal
        Property hontai_condition1() As Integer
        Property hontai_chk2() As Integer
        Property hontai_date2() As String
        Property hontai_amount2() As Decimal
        Property hontai_condition2() As Integer
        Property hontai_chk3() As Integer
        Property hontai_date3() As String
        Property hontai_amount3() As Decimal
        Property hontai_condition3() As Integer
        Property hontai_amount() As Decimal
        Property total_amount() As Decimal
        Property quotation_amount() As String
        Property remark() As String
        Property detail() As String
        Property finish_fg() As Integer
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property create_at_remark() As String
        Property payment_condition_id() As String
        Property payment_condition_remark() As String
        Property finish_date() As String

        'Receive data from screen (condition search)
        Property job_order_from_search() As String
        Property job_order_to_search() As String
        Property customer_search() As String
        Property issue_date_from_search() As String
        Property issue_date_to_search() As String
        Property finish_date_from_search() As String
        Property finish_date_to_search() As String
        Property receive_po_search() As String
        Property Job_finish_search() As String
        Property part_no_search() As String
        Property part_name_search() As String
        Property job_type_search() As String
        Property boi_search() As String
        Property person_charge_search() As String

        'Receive data for detail
        Property job_order_type_Detail() As String
        Property term_day() As Integer
        Property currency_name() As String
        Property customer_name() As String
        Property receive_po_name() As String
        Property end_user_name() As String
        Property person_in_charge_name() As String
        Property create_at_name() As String
        Property part_type_name() As String
        Property is_boi_name() As String
        Property issue_by() As String
        Property job_new() As String
        Property job_Mod() As String
        Property sum_hontai_amount() As String
        Property rpt_hontai_amount() As String
        Property rpt_hontai_amount_thb() As String
        Property payment_condition_name() As String

        'Receive data from menagement job order screen (condition search)
        Property job_month() As Integer
        Property job_year() As Integer
        Property job_last() As Integer
        Property ip_address() As String

        'Receive data from menagement job order po screen 
        Property po_type() As Integer
        Property po_no() As String
        Property po_amount() As Decimal
        Property po_date() As String
        Property po_file() As String 
        Property po_receipt_date() As String
        'Receive data from menagement job order quotation screen 
        Property quo_type() As Integer
        Property quo_no() As String
        Property quo_amount() As Decimal
        Property quo_date() As String
        Property quo_file() As String 


        Function CheckJobOrderByVendor(ByVal intVendor_id As Integer) As Boolean
        Function GetJobOrderForList() As List(Of IJob_OrderEntity)
        Function CheckJobOrderByPurchase(ByVal strJobOrder As String) As Boolean

        'function for job order screen
        Function GetJobOrderList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetDeleteJobOrderList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetJobOrderReportList(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
        Function GetSumHontaiAmountReport(ByVal objJobOrderEnt As Entity.IJob_OrderEntity) As List(Of Entity.ImpJob_OrderDetailEntity)
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

