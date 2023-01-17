#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpJob_OrderEntity
'	Class Discription	: Class of table job_order
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
    Public Class ImpJob_OrderEntity
        Implements IJob_OrderEntity

        Private _id As Integer
        Private _job_order As String
        Private _old_job_order As String
        Private _issue_date As String
        Private _customer As Integer
        Private _end_user As Integer
        Private _receive_po As Integer
        Private _person_in_charge As Integer
        Private _job_type_id As Integer
        Private _is_boi As Integer
        Private _create_at As Integer
        Private _part_name As String
        Private _part_no As String
        Private _part_type As Integer
        Private _payment_term_id As Integer
        Private _currency_id As Integer
        Private _hontai_chk1 As Integer
        Private _hontai_date1 As String
        Private _hontai_amount1 As Decimal
        Private _hontai_condition1 As Integer
        Private _hontai_chk2 As Integer
        Private _hontai_date2 As String
        Private _hontai_amount2 As Decimal
        Private _hontai_condition2 As Integer
        Private _hontai_chk3 As Integer
        Private _hontai_date3 As String
        Private _hontai_amount3 As Decimal
        Private _hontai_condition3 As Integer
        Private _hontai_amount As Decimal
        Private _total_amount As Decimal
        Private _quotation_amount As String
        Private _remark As String
        Private _detail As String
        Private _finish_fg As Integer
        Private _status_id As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _create_at_remark As String
        Private _payment_condition_id As String
        Private _payment_condition_remark As String
        Private _finish_date As String
        Private _job_order_type_Detail As String
        Private _term_day As Integer
        Private _currency_name As String
        Private _customer_name As String
        Private _end_user_name As String
        Private _person_in_charge_name As String
        Private _create_at_name As String
        Private _part_type_name As String
        Private _receive_po_name As String
        Private _is_boi_name As String
        Private _issue_by As String
        Private _job_new As String
        Private _job_Mod As String
        Private _sum_hontai_amount As String
        Private _rpt_hontai_amount As String
        Private _rpt_hontai_amount_thb As String
        Private _payment_condition_name As String

        'Receive data from screen (condition search)
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _finish_date_from_search As String
        Private _finish_date_to_search As String
        Private _receive_po_search As String
        Private _Job_finish_search As String
        Private _part_no_search As String
        Private _part_name_search As String
        Private _job_type_search As String
        Private _boi_search As String
        Private _person_charge_search As String

        'Receive data from menagement job order screen (condition search)
        Private _job_month As Integer
        Private _job_year As Integer
        Private _job_last As Integer
        Private _ip_address As String
        'Receive data from menagement job order po screen 
        Private _po_type As Integer
        Private _po_no As String
        Private _po_amount As Decimal
        Private _po_date As String
        Private _po_file As String
        Private _po_receipt_date As String
        'Receive data from menagement job order quotation screen 
        Private _quo_type As Integer
        Private _quo_no As String
        Private _quo_amount As Decimal
        Private _quo_date As String
        Private _quo_file As String

        Private objJobOrder As New Dao.ImpJob_OrderDao

#Region "Function"
        Public Function CheckJobOrderByPurchase(ByVal strJobOrder As String) As Boolean Implements IJob_OrderEntity.CheckJobOrderByPurchase
            Dim objJobOrder As New Dao.ImpJob_OrderDao
            Return objJobOrder.DB_CheckJobOrderByPurchase(strJobOrder)
        End Function

        Public Function GetJobOrderForList() As System.Collections.Generic.List(Of IJob_OrderEntity) Implements IJob_OrderEntity.GetJobOrderForList
            Dim objJobOrder As New Dao.ImpJob_OrderDao
            Return objJobOrder.DB_GetJobOrderForList
        End Function

        Public Function CheckJobOrderByVendor(ByVal intVendor_id As Integer) As Boolean Implements IJob_OrderEntity.CheckJobOrderByVendor
            Dim objJobOrder As New Dao.ImpJob_OrderDao
            Return objJobOrder.DB_CheckJobOrderByVendor(intVendor_id)
        End Function

        Public Function GetJobOrderList(ByVal objJobOrderEnt As IJob_OrderEntity) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderList
            Return objJobOrder.GetJobOrderList(objJobOrderEnt)
        End Function

        Public Function GetJobOrderReportList(ByVal objJobOrderEnt As IJob_OrderEntity) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderReportList
            Return objJobOrder.GetJobOrderReportList(objJobOrderEnt)
        End Function

        Public Function GetSumHontaiAmountReport(ByVal objJobOrderEnt As IJob_OrderEntity) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetSumHontaiAmountReport
            Return objJobOrder.GetSumHontaiAmountReport(objJobOrderEnt)
        End Function

        Public Function GetDeleteJobOrderList(ByVal objJobOrderEnt As IJob_OrderEntity) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetDeleteJobOrderList
            Return objJobOrder.GetDeleteJobOrderList(objJobOrderEnt)
        End Function

        Public Function DeleteJobOrder(ByVal intJobOrderID As Integer, ByVal strJobOrder As String) As Integer Implements IJob_OrderEntity.DeleteJobOrder
            Return objJobOrder.DeleteJobOrder(intJobOrderID, strJobOrder)
        End Function

        Public Function CheckUseInPodetail(ByVal strJobOrder As String) As Integer Implements IJob_OrderEntity.CheckUseInPodetail
            Return objJobOrder.CheckUseInPodetail(strJobOrder)
        End Function

        Public Function CheckUseInOrderPo(ByVal intJobOrderID As Integer) As Integer Implements IJob_OrderEntity.CheckUseInOrderPo
            Return objJobOrder.CheckUseInOrderPo(intJobOrderID)
        End Function

        Public Function CheckUseInJobOrderPo(ByVal intJobOrderID As Integer) As Integer Implements IJob_OrderEntity.CheckUseInJobOrderPo
            Return objJobOrder.CheckUseInJobOrderPo(intJobOrderID)
        End Function

        Public Function CheckUseInOrderQuo(ByVal intJobOrderID As Integer) As Integer Implements IJob_OrderEntity.CheckUseInOrderQuo
            Return objJobOrder.CheckUseInOrderQuo(intJobOrderID)
        End Function

        Public Function CheckUseInRecDetail(ByVal intJobOrderID As Integer) As Integer Implements IJob_OrderEntity.CheckUseInRecDetail
            Return objJobOrder.CheckUseInRecDetail(intJobOrderID)
        End Function

        Public Function CheckUseInAccounting(ByVal strJobOrder As String) As Integer Implements IJob_OrderEntity.CheckUseInAccounting
            Return objJobOrder.CheckUseInAccounting(strJobOrder)
        End Function

        Public Function CheckUseInStock(ByVal strJobOrder As String) As Integer Implements IJob_OrderEntity.CheckUseInStock
            Return objJobOrder.CheckUseInStock(strJobOrder)
        End Function

        Public Function CheckUseInStockOut(ByVal strJobOrder As String) As Integer Implements IJob_OrderEntity.CheckUseInStockOut
            Return objJobOrder.CheckUseInStockOut(strJobOrder)
        End Function

        Public Function DeleteJobOrderTemp(ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.DeleteJobOrderTemp
            Return objJobOrder.DeleteJobOrderTemp(strIpAddress)
        End Function

        Public Function DeleteJobTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.DeleteJobTemp
            Return objJobOrder.DeleteJobTemp(intJobOrderID, strIpAddress)
        End Function

        Public Function GetJobOrderByID(ByVal intJobOrderID As Integer) As IJob_OrderEntity Implements IJob_OrderEntity.GetJobOrderByID
            Return objJobOrder.GetJobOrderByID(intJobOrderID)
        End Function

        Public Function GetJobOrderRunning(ByVal intIssueMonth As Integer, ByVal intIssueYear As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderRunning
            Return objJobOrder.GetJobOrderRunning(intIssueMonth, intIssueYear)
        End Function

        Public Function InsertJobOrderTemp(ByVal intJobOrderID As Integer, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.InsertJobOrderTemp
            Return objJobOrder.InsertJobOrderTemp(intJobOrderID, strIpAddress)
        End Function

        Public Function GetSumPoAmount(ByVal strIpAddress As String) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetSumPoAmount
            Return objJobOrder.GetSumPoAmount(strIpAddress)
        End Function

        Public Function GetSumQuoAmount(ByVal strIpAddress As String) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetSumQuoAmount
            Return objJobOrder.GetSumQuoAmount(strIpAddress)
        End Function

        Public Function GetTotalAmount(ByVal strIpAddress As String) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetTotalAmount
            Return objJobOrder.GetTotalAmount(strIpAddress)
        End Function

        Public Function InsertJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderEnt As IJob_OrderEntity) As Integer Implements IJob_OrderEntity.InsertJobOrder
            Return objJobOrder.InsertJobOrder(strYear, strMonth, strJobLast, strJobOrder, objJobOrderEnt)
        End Function

        Public Function UpdateJobOrder(ByVal strYear As String, ByVal strMonth As String, ByVal strJobLast As String, ByVal strJobOrder As String, ByVal objJobOrderEnt As IJob_OrderEntity) As Integer Implements IJob_OrderEntity.UpdateJobOrder
            Return objJobOrder.UpdateJobOrder(strYear, strMonth, strJobLast, strJobOrder, objJobOrderEnt)
        End Function

        Public Function RestoreJobOrder(ByVal objJobOrderEnt As IJob_OrderEntity) As Integer Implements IJob_OrderEntity.RestoreJobOrder
            Return objJobOrder.RestoreJobOrder(objJobOrderEnt)
        End Function

        Public Function GetJobOrderDetailList(ByVal intJobOrderID As Integer) As IJob_OrderEntity Implements IJob_OrderEntity.GetJobOrderDetailList
            Return objJobOrder.GetJobOrderDetailList(intJobOrderID)
        End Function

        Public Function GetJobOrderPOList(ByVal intJobOrderID As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderPOList
            Return objJobOrder.GetJobOrderPOList(intJobOrderID)
        End Function

        Public Function GetJobOrderQuoList(ByVal intJobOrderID As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderQuoList
            Return objJobOrder.GetJobOrderQuoList(intJobOrderID)
        End Function

        Public Function GetJobOrderInvoiceList(ByVal intJobOrderID As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderInvoiceList
            Return objJobOrder.GetJobOrderInvoiceList(intJobOrderID)
        End Function

        Public Function GetPaymentConditionDetail(ByVal intPayment_condition_id As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetPaymentConditionDetail
            Return objJobOrder.GetPaymentConditionDetail(intPayment_condition_id)
        End Function

        Public Function GetJobOrderPOTempList(ByVal strIpAddress As String) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderPOTempList
            Return objJobOrder.GetJobOrderPOTempList(strIpAddress)
        End Function

        Public Function GetJobOrderQuoTempList(ByVal strIpAddress As String) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetJobOrderQuoTempList
            Return objJobOrder.GetJobOrderQuoTempList(strIpAddress)
        End Function

        Public Function CheckExistPoFile(ByVal strPoFile As String, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.CheckExistPoFile
            Return objJobOrder.CheckExistPoFile(strPoFile, strIpAddress)
        End Function

        Public Function CheckExistPoNo(ByVal strPoNo As String, Optional ByVal strJobOrderId As String = "") As Integer Implements IJob_OrderEntity.CheckExistPoNo
            Return objJobOrder.CheckExistPoNo(strPoNo, strJobOrderId)
        End Function

        Public Function CheckExistPoNoTemp(ByVal strPoNo As String) As Integer Implements IJob_OrderEntity.CheckExistPoNoTemp
            Return objJobOrder.CheckExistPoNoTemp(strPoNo)
        End Function

        Public Function CheckExistPoTemp(ByVal intId As Integer, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.CheckExistPoTemp
            Return objJobOrder.CheckExistPoTemp(intId, strIpAddress)
        End Function

        Public Function CheckExistPoType(ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.CheckExistPoType
            Return objJobOrder.CheckExistPoType(strIpAddress)
        End Function

        Public Function CheckExistQuoFile(ByVal strQuoFile As String, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.CheckExistQuoFile
            Return objJobOrder.CheckExistQuoFile(strQuoFile, strIpAddress)
        End Function

        Public Function CheckExistQuoNo(ByVal strQuoNo As String, Optional ByVal strJobOrderId As String = "") As Integer Implements IJob_OrderEntity.CheckExistQuoNo
            Return objJobOrder.CheckExistQuoNo(strQuoNo, strJobOrderId)
        End Function

        Public Function CheckExistQuoNoTemp(ByVal strQuoNo As String) As Integer Implements IJob_OrderEntity.CheckExistQuoNoTemp
            Return objJobOrder.CheckExistQuoNoTemp(strQuoNo)
        End Function

        Public Function CheckExistReceiveDetail(ByVal intJobOrderId As Integer) As Integer Implements IJob_OrderEntity.CheckExistReceiveDetail
            Return objJobOrder.CheckExistReceiveDetail(intJobOrderId)
        End Function

        Public Function DeletePOTemp(ByVal intId As Integer) As Integer Implements IJob_OrderEntity.DeletePOTemp
            Return objJobOrder.DeletePOTemp(intId)
        End Function

        Public Function DeleteQuoTemp(ByVal intId As Integer) As Integer Implements IJob_OrderEntity.DeleteQuoTemp
            Return objJobOrder.DeleteQuoTemp(intId)
        End Function

        Public Function InsertPoTemp(ByVal objJobOrderEnt As IJob_OrderEntity) As Integer Implements IJob_OrderEntity.InsertPoTemp
            Return objJobOrder.InsertPoTemp(objJobOrderEnt)
        End Function

        Public Function InsertQuoTemp(ByVal objJobOrderEnt As IJob_OrderEntity) As Integer Implements IJob_OrderEntity.InsertQuoTemp
            Return objJobOrder.InsertQuoTemp(objJobOrderEnt)
        End Function

        Public Function GetSumUploadPoAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetSumUploadPoAmount
            Return objJobOrder.GetSumUploadPoAmount(strIpAddress, intJobOrderID, intMode)
        End Function

        Public Function GetUploadTotalAmount(ByVal strIpAddress As String, ByVal intJobOrderID As Integer, ByVal intMode As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetUploadTotalAmount
            Return objJobOrder.GetUploadTotalAmount(strIpAddress, intJobOrderID, intMode)
        End Function

        Public Function GetOneJobOrderPOTempList(ByVal intPoId As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetOneJobOrderPOTempList
            Return objJobOrder.GetOneJobOrderPOTempList(intPoId)
        End Function

        Public Function UpdateJobOrderPOToTempList(ByVal intJobOrderId As Integer, ByVal objJobOrderEnt As Entity.IJob_OrderEntity, ByVal strIpAddress As String) As Integer Implements IJob_OrderEntity.UpdateJobOrderPOToTempList
            Return objJobOrder.UpdateJobOrderPOToTempList(intJobOrderId, objJobOrderEnt, strIpAddress)
        End Function

        Public Function GetOneQuoFromTmp(ByVal intQuoId As Integer) As System.Collections.Generic.List(Of ImpJob_OrderDetailEntity) Implements IJob_OrderEntity.GetOneQuoFromTmp
            Return objJobOrder.GetOneQuoFromTmp(intQuoId)
        End Function

        Public Function UpdateQuotationToTmp(ByVal objJobQuo As Entity.IJob_OrderEntity, ByVal intQuoId As Integer) As Integer Implements IJob_OrderEntity.UpdateQuotationToTmp
            Return objJobOrder.UpdateQuotationToTmp(objJobQuo, intQuoId)
        End Function



#End Region

#Region "Property"
        Public Property payment_condition_name() As String Implements IJob_OrderEntity.payment_condition_name
            Get
                Return _payment_condition_name
            End Get
            Set(ByVal value As String)
                _payment_condition_name = value
            End Set
        End Property

        Public Property create_at() As Integer Implements IJob_OrderEntity.create_at
            Get
                Return _create_at
            End Get
            Set(ByVal value As Integer)
                _create_at = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IJob_OrderEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IJob_OrderEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property currency_id() As Integer Implements IJob_OrderEntity.currency_id
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property customer() As Integer Implements IJob_OrderEntity.customer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
            End Set
        End Property

        Public Property end_user() As Integer Implements IJob_OrderEntity.end_user
            Get
                Return _end_user
            End Get
            Set(ByVal value As Integer)
                _end_user = value
            End Set
        End Property

        Public Property finish_fg() As Integer Implements IJob_OrderEntity.finish_fg
            Get
                Return _finish_fg
            End Get
            Set(ByVal value As Integer)
                _finish_fg = value
            End Set
        End Property

        Public Property hontai_amount1() As Decimal Implements IJob_OrderEntity.hontai_amount1
            Get
                Return _hontai_amount1
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount1 = value
            End Set
        End Property

        Public Property hontai_amount2() As Decimal Implements IJob_OrderEntity.hontai_amount2
            Get
                Return _hontai_amount2
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount2 = value
            End Set
        End Property

        Public Property hontai_amount3() As Decimal Implements IJob_OrderEntity.hontai_amount3
            Get
                Return _hontai_amount3
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount3 = value
            End Set
        End Property

        Public Property hontai_chk1() As Integer Implements IJob_OrderEntity.hontai_chk1
            Get
                Return _hontai_chk1
            End Get
            Set(ByVal value As Integer)
                _hontai_chk1 = value
            End Set
        End Property

        Public Property hontai_chk2() As Integer Implements IJob_OrderEntity.hontai_chk2
            Get
                Return _hontai_chk2
            End Get
            Set(ByVal value As Integer)
                _hontai_chk2 = value
            End Set
        End Property

        Public Property hontai_chk3() As Integer Implements IJob_OrderEntity.hontai_chk3
            Get
                Return _hontai_chk3
            End Get
            Set(ByVal value As Integer)
                _hontai_chk3 = value
            End Set
        End Property

        Public Property hontai_condition1() As Integer Implements IJob_OrderEntity.hontai_condition1
            Get
                Return _hontai_condition1
            End Get
            Set(ByVal value As Integer)
                _hontai_condition1 = value
            End Set
        End Property

        Public Property hontai_condition2() As Integer Implements IJob_OrderEntity.hontai_condition2
            Get
                Return _hontai_condition2
            End Get
            Set(ByVal value As Integer)
                _hontai_condition2 = value
            End Set
        End Property

        Public Property hontai_condition3() As Integer Implements IJob_OrderEntity.hontai_condition3
            Get
                Return _hontai_condition3
            End Get
            Set(ByVal value As Integer)
                _hontai_condition3 = value
            End Set
        End Property

        Public Property hontai_date1() As String Implements IJob_OrderEntity.hontai_date1
            Get
                Return _hontai_date1
            End Get
            Set(ByVal value As String)
                _hontai_date1 = value
            End Set
        End Property

        Public Property hontai_date2() As String Implements IJob_OrderEntity.hontai_date2
            Get
                Return _hontai_date2
            End Get
            Set(ByVal value As String)
                _hontai_date2 = value
            End Set
        End Property

        Public Property hontai_date3() As String Implements IJob_OrderEntity.hontai_date3
            Get
                Return _hontai_date3
            End Get
            Set(ByVal value As String)
                _hontai_date3 = value
            End Set
        End Property

        Public Property id() As Integer Implements IJob_OrderEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property is_boi() As Integer Implements IJob_OrderEntity.is_boi
            Get
                Return _is_boi
            End Get
            Set(ByVal value As Integer)
                _is_boi = value
            End Set
        End Property

        Public Property issue_date() As String Implements IJob_OrderEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property job_order() As String Implements IJob_OrderEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property old_job_order() As String Implements IJob_OrderEntity.old_job_order
            Get
                Return _old_job_order
            End Get
            Set(ByVal value As String)
                _old_job_order = value
            End Set
        End Property

        Public Property job_type_id() As Integer Implements IJob_OrderEntity.job_type_id
            Get
                Return _job_type_id
            End Get
            Set(ByVal value As Integer)
                _job_type_id = value
            End Set
        End Property

        Public Property part_name() As String Implements IJob_OrderEntity.part_name
            Get
                Return _part_name
            End Get
            Set(ByVal value As String)
                _part_name = value
            End Set
        End Property

        Public Property part_no() As String Implements IJob_OrderEntity.part_no
            Get
                Return _part_no
            End Get
            Set(ByVal value As String)
                _part_no = value
            End Set
        End Property

        Public Property part_type() As Integer Implements IJob_OrderEntity.part_type
            Get
                Return _part_type
            End Get
            Set(ByVal value As Integer)
                _part_type = value
            End Set
        End Property

        Public Property payment_term_id() As Integer Implements IJob_OrderEntity.payment_term_id
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Public Property person_in_charge() As Integer Implements IJob_OrderEntity.person_in_charge
            Get
                Return _person_in_charge
            End Get
            Set(ByVal value As Integer)
                _person_in_charge = value
            End Set
        End Property

        Public Property receive_po() As Integer Implements IJob_OrderEntity.receive_po
            Get
                Return _receive_po
            End Get
            Set(ByVal value As Integer)
                _receive_po = value
            End Set
        End Property

        Public Property remark() As String Implements IJob_OrderEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property detail() As String Implements IJob_OrderEntity.detail
            Get
                Return _detail
            End Get
            Set(ByVal value As String)
                _detail = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IJob_OrderEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property hontai_amount() As Decimal Implements IJob_OrderEntity.hontai_amount
            Get
                Return _hontai_amount
            End Get
            Set(ByVal value As Decimal)
                _hontai_amount = value
            End Set
        End Property

        Public Property total_amount() As Decimal Implements IJob_OrderEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property quotation_amount() As String Implements IJob_OrderEntity.quotation_amount
            Get
                Return _quotation_amount
            End Get
            Set(ByVal value As String)
                _quotation_amount = value
            End Set
        End Property

        Property create_at_remark() As String Implements IJob_OrderEntity.create_at_remark
            Get
                Return _create_at_remark
            End Get
            Set(ByVal value As String)
                _create_at_remark = value
            End Set
        End Property

        Property payment_condition_id() As String Implements IJob_OrderEntity.payment_condition_id
            Get
                Return _payment_condition_id
            End Get
            Set(ByVal value As String)
                _payment_condition_id = value
            End Set
        End Property

        Property payment_condition_remark() As String Implements IJob_OrderEntity.payment_condition_remark
            Get
                Return _payment_condition_remark
            End Get
            Set(ByVal value As String)
                _payment_condition_remark = value
            End Set
        End Property

        Property finish_date() As String Implements IJob_OrderEntity.finish_date
            Get
                Return _finish_date
            End Get
            Set(ByVal value As String)
                _finish_date = value
            End Set
        End Property

        'Property for condition search

        Property job_order_from_search() As String Implements IJob_OrderEntity.job_order_from_search
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Property job_order_to_search() As String Implements IJob_OrderEntity.job_order_to_search
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Property customer_search() As String Implements IJob_OrderEntity.customer_search
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Property issue_date_from_search() As String Implements IJob_OrderEntity.issue_date_from_search
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Property issue_date_to_search() As String Implements IJob_OrderEntity.issue_date_to_search
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Property finish_date_from_search() As String Implements IJob_OrderEntity.finish_date_from_search
            Get
                Return _finish_date_from_search
            End Get
            Set(ByVal value As String)
                _finish_date_from_search = value
            End Set
        End Property

        Property finish_date_to_search() As String Implements IJob_OrderEntity.finish_date_to_search
            Get
                Return _finish_date_to_search
            End Get
            Set(ByVal value As String)
                _finish_date_to_search = value
            End Set
        End Property

        Property receive_po_search() As String Implements IJob_OrderEntity.receive_po_search
            Get
                Return _receive_po_search
            End Get
            Set(ByVal value As String)
                _receive_po_search = value
            End Set
        End Property

        Property Job_finish_search() As String Implements IJob_OrderEntity.Job_finish_search
            Get
                Return _Job_finish_search
            End Get
            Set(ByVal value As String)
                _Job_finish_search = value
            End Set
        End Property

        Property part_no_search() As String Implements IJob_OrderEntity.part_no_search
            Get
                Return _part_no_search
            End Get
            Set(ByVal value As String)
                _part_no_search = value
            End Set
        End Property

        Property part_name_search() As String Implements IJob_OrderEntity.part_name_search
            Get
                Return _part_name_search
            End Get
            Set(ByVal value As String)
                _part_name_search = value
            End Set
        End Property

        Property job_type_search() As String Implements IJob_OrderEntity.job_type_search
            Get
                Return _job_type_search
            End Get
            Set(ByVal value As String)
                _job_type_search = value
            End Set
        End Property

        Property boi_search() As String Implements IJob_OrderEntity.boi_search
            Get
                Return _boi_search
            End Get
            Set(ByVal value As String)
                _boi_search = value
            End Set
        End Property

        Property person_charge_search() As String Implements IJob_OrderEntity.person_charge_search
            Get
                Return _person_charge_search
            End Get
            Set(ByVal value As String)
                _person_charge_search = value
            End Set
        End Property

        Property job_month() As Integer Implements IJob_OrderEntity.job_month
            Get
                Return _job_month
            End Get
            Set(ByVal value As Integer)
                _job_month = value
            End Set
        End Property

        Property job_year() As Integer Implements IJob_OrderEntity.job_year
            Get
                Return _job_year
            End Get
            Set(ByVal value As Integer)
                _job_year = value
            End Set
        End Property

        Property job_last() As Integer Implements IJob_OrderEntity.job_last
            Get
                Return _job_last
            End Get
            Set(ByVal value As Integer)
                _job_last = value
            End Set
        End Property

        Property ip_address() As String Implements IJob_OrderEntity.ip_address
            Get
                Return _ip_address
            End Get
            Set(ByVal value As String)
                _ip_address = value
            End Set
        End Property

        Public Property currency_name() As String Implements IJob_OrderEntity.currency_name
            Get
                Return _currency_name
            End Get
            Set(ByVal value As String)
                _currency_name = value
            End Set
        End Property

        Public Property customer_name() As String Implements IJob_OrderEntity.customer_name
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Property end_user_name() As String Implements IJob_OrderEntity.end_user_name
            Get
                Return _end_user_name
            End Get
            Set(ByVal value As String)
                _end_user_name = value
            End Set
        End Property

        Property receive_po_name() As String Implements IJob_OrderEntity.receive_po_name
            Get
                Return _receive_po_name
            End Get
            Set(ByVal value As String)
                _receive_po_name = value
            End Set
        End Property

        Property is_boi_name() As String Implements IJob_OrderEntity.is_boi_name
            Get
                Return _is_boi_name
            End Get
            Set(ByVal value As String)
                _is_boi_name = value
            End Set
        End Property

        Property person_in_charge_name() As String Implements IJob_OrderEntity.person_in_charge_name
            Get
                Return _person_in_charge_name
            End Get
            Set(ByVal value As String)
                _person_in_charge_name = value
            End Set
        End Property

        Property create_at_name() As String Implements IJob_OrderEntity.create_at_name
            Get
                Return _create_at_name
            End Get
            Set(ByVal value As String)
                _create_at_name = value
            End Set
        End Property

        Property part_type_name() As String Implements IJob_OrderEntity.part_type_name
            Get
                Return _part_type_name
            End Get
            Set(ByVal value As String)
                _part_type_name = value
            End Set
        End Property

        Property issue_by() As String Implements IJob_OrderEntity.issue_by
            Get
                Return _issue_by
            End Get
            Set(ByVal value As String)
                _issue_by = value
            End Set
        End Property

        Property job_new() As String Implements IJob_OrderEntity.job_new
            Get
                Return _job_new
            End Get
            Set(ByVal value As String)
                _job_new = value
            End Set
        End Property

        Property job_Mod() As String Implements IJob_OrderEntity.job_Mod
            Get
                Return _job_Mod
            End Get
            Set(ByVal value As String)
                _job_Mod = value
            End Set
        End Property

        Property sum_hontai_amount() As String Implements IJob_OrderEntity.sum_hontai_amount
            Get
                Return _sum_hontai_amount
            End Get
            Set(ByVal value As String)
                _sum_hontai_amount = value
            End Set
        End Property

        Property rpt_hontai_amount() As String Implements IJob_OrderEntity.rpt_hontai_amount
            Get
                Return _rpt_hontai_amount
            End Get
            Set(ByVal value As String)
                _rpt_hontai_amount = value
            End Set
        End Property

        Public Property job_order_type_Detail() As String Implements IJob_OrderEntity.job_order_type_Detail
            Get
                Return _job_order_type_Detail
            End Get
            Set(ByVal value As String)
                _job_order_type_Detail = value
            End Set
        End Property

        Public Property term_day() As Integer Implements IJob_OrderEntity.term_day
            Get
                Return _term_day
            End Get
            Set(ByVal value As Integer)
                _term_day = value
            End Set
        End Property

        Public Property po_amount() As Decimal Implements IJob_OrderEntity.po_amount
            Get
                Return _po_amount
            End Get
            Set(ByVal value As Decimal)
                _po_amount = value
            End Set
        End Property

        Public Property po_date() As String Implements IJob_OrderEntity.po_date
            Get
                Return _po_date
            End Get
            Set(ByVal value As String)
                _po_date = value
            End Set
        End Property

        Public Property po_file() As String Implements IJob_OrderEntity.po_file
            Get
                Return _po_file
            End Get
            Set(ByVal value As String)
                _po_file = value
            End Set
        End Property

        Public Property po_no() As String Implements IJob_OrderEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property po_receipt_date() As String Implements IJob_OrderEntity.po_receipt_date
            Get
                Return _po_receipt_date
            End Get
            Set(ByVal value As String)
                _po_receipt_date = value
            End Set
        End Property

        Public Property po_type() As Integer Implements IJob_OrderEntity.po_type
            Get
                Return _po_type
            End Get
            Set(ByVal value As Integer)
                _po_type = value
            End Set
        End Property

        Public Property quo_amount() As Decimal Implements IJob_OrderEntity.quo_amount
            Get
                Return _quo_amount
            End Get
            Set(ByVal value As Decimal)
                _quo_amount = value
            End Set
        End Property

        Public Property quo_date() As String Implements IJob_OrderEntity.quo_date
            Get
                Return _quo_date
            End Get
            Set(ByVal value As String)
                _quo_date = value
            End Set
        End Property

        Public Property quo_file() As String Implements IJob_OrderEntity.quo_file
            Get
                Return _quo_file
            End Get
            Set(ByVal value As String)
                _quo_file = value
            End Set
        End Property

        Public Property quo_no() As String Implements IJob_OrderEntity.quo_no
            Get
                Return _quo_no
            End Get
            Set(ByVal value As String)
                _quo_no = value
            End Set
        End Property

        Public Property quo_type() As Integer Implements IJob_OrderEntity.quo_type
            Get
                Return _quo_type
            End Get
            Set(ByVal value As Integer)
                _quo_type = value
            End Set
        End Property

        Public Property rpt_hontai_amount_thb() As String Implements IJob_OrderEntity.rpt_hontai_amount_thb
            Get
                Return _rpt_hontai_amount_thb
            End Get
            Set(ByVal value As String)
                _rpt_hontai_amount_thb = value
            End Set
        End Property

#End Region

    End Class
End Namespace

