#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpSale_InvoiceEntity
'	Class Discription	: Class of table mst_job_type
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

Namespace Entity
    Public Class ImpSale_InvoiceEntity
        Implements ISale_InvoiceEntity

        Private _id As Integer
        Private _name As String
        Private _invoice_no As String
        Private _receipt_date As String
        Private _ie_id As Integer
        Private _vendor_id As Integer
        Private _account_type As Integer
        Private _invoice_type As Integer
        Private _bank_fee As String
        Private _total_amount As Decimal
        Private _user_id As Integer
        Private _status_id As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String
        Private _issue_date As String
        Private _customer As Integer
        Private _job_order_id As Integer
        Private _currency_id As Integer
        Private _po_date As String
        Private _po_type As Integer
        Private _receive_header_id As Integer
        Private _receive_detail_id As Integer
        Private _vat_id As Integer
        Private _wt_id As Integer
        Private _hontai_type As String
        Private _job_order_po_id As Integer
        Private _hontai_fg As String
        Private _account_next_approve As String
        Private _status As String
        Private _hontai_fg1 As String
        Private _hontai_fg2 As String
        Private _hontai_fg3 As String
        Private _hontai_cond As String
        Private _job_type As String
        Private _po_fg As String
        Private _vat_amount As Decimal
        Private _wt_amount As Decimal
        Private _sub_total As Decimal
        Private _remark_detail As String

        'Receive data from screen (condition search)
        Private _invoice_no_search As String
        Private _invoice_type_search As String
        Private _job_order_from_search As String
        Private _job_order_to_search As String
        Private _customer_search As String
        Private _issue_date_from_search As String
        Private _issue_date_to_search As String
        Private _receipt_date_from_search As String
        Private _receipt_date_to_search As String

        'Receive data for detail search
        Private _invoice_type_name As String
        Private _customer_name As String
        Private _amount As String
        Private _account_title As String
        Private _account_type_name As String
        Private _job_order As String
        Private _po_type_name As String
        Private _hontai As String
        Private _po_no As String
        Private _vat As String
        Private _wt As String
        Private _remark As String
        Private _sum_amount As Decimal
        Private _sum_vat As Decimal
        Private _sum_wt As Decimal
        Private _sum_price As Decimal
        Private _stage As String
        Private _percent As String
        Private _actual_rate As String
        Private _price As String
        Private _bank_rate As String
        Private _actual_amount As String
        Private _currency As String
        Private _total_invoice_amount As String
        Private _strJobOrder1 As String
        Private _strJobOrder2 As String
        Private _strJobOrder3 As String

        Private objSaleInvoiceDao As New Dao.ImpSale_InvoiceDao

#Region "Function"

        Public Function GetTotalSaleInvoiceAmount(ByVal objSaleInvoiceEnt As ISale_InvoiceEntity) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetTotalSaleInvoiceAmount
            Return objSaleInvoiceDao.GetTotalSaleInvoiceAmount(objSaleInvoiceEnt)
        End Function

        Public Function CountUsedInAccounting(ByVal intRefID As Integer) As Integer Implements ISale_InvoiceEntity.CountUsedInAccounting
            Return objSaleInvoiceDao.CountUsedInAccounting(intRefID)
        End Function

        Public Function DeleteSaleInvoice(ByVal intRefID As Integer, ByVal dtValues As System.Data.DataTable) As Integer Implements ISale_InvoiceEntity.DeleteSaleInvoice
            Return objSaleInvoiceDao.DeleteSaleInvoice(intRefID, dtValues)
        End Function

        Public Function GetSaleInvoiceHeaderByID(ByVal intRefID As Integer) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSaleInvoiceHeaderByID
            Return objSaleInvoiceDao.GetSaleInvoiceHeaderByID(intRefID)
        End Function

        Public Function GetSaleInvoiceDetailByID(ByVal intRefID As Integer) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSaleInvoiceDetailByID
            Return objSaleInvoiceDao.GetSaleInvoiceDetailByID(intRefID)
        End Function

        Public Function GetSaleInvoiceList(ByVal objSaleInvoiceEnt As ISale_InvoiceEntity) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetSaleInvoiceList
            Return objSaleInvoiceDao.GetSaleInvoiceList(objSaleInvoiceEnt)
        End Function

        Public Function GetSaleInvoiceDetailList(ByVal intRefID As Integer) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetSaleInvoiceDetailList
            Return objSaleInvoiceDao.GetSaleInvoiceDetailList(intRefID)
        End Function

        Public Function GetSaleInvoiceHeaderList(ByVal intRefID As Integer) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSaleInvoiceHeaderList
            Return objSaleInvoiceDao.GetSaleInvoiceHeaderList(intRefID)
        End Function

        Public Function GetSaleInvoiceReportList(ByVal objSaleInvoiceEnt As ISale_InvoiceEntity) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetSaleInvoiceReportList
            Return objSaleInvoiceDao.GetSaleInvoiceReportList(objSaleInvoiceEnt)
        End Function

        Public Function GetSumSaleInvoiceReportList(ByVal objSaleInvoiceEnt As ISale_InvoiceEntity) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSumSaleInvoiceReportList
            Return objSaleInvoiceDao.GetSumSaleInvoiceReportList(objSaleInvoiceEnt)
        End Function

        Public Function SaveSaleInvoice(ByVal intRefID As Integer, ByVal decExchangeRate As Decimal, ByVal decActualAmount As Decimal, ByVal strJobOrder As String) As Integer Implements ISale_InvoiceEntity.SaveSaleInvoice
            Return objSaleInvoiceDao.SaveSaleInvoice(intRefID, decExchangeRate, decActualAmount, strJobOrder)
        End Function

        Public Function ConfirmReceive(ByVal invoiceHeaderId As String, ByVal dtValues As System.Data.DataTable, Optional ByVal dtBankFree As System.Data.DataTable = Nothing) As Integer Implements ISale_InvoiceEntity.ConfirmReceive
            Return objSaleInvoiceDao.ConfirmReceive(invoiceHeaderId, dtValues, dtBankFree)
        End Function

        Public Function GetJobOrerSaleInvoiceDetail(ByVal strJobOrder As String) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetJobOrerSaleInvoiceDetail
            Return objSaleInvoiceDao.GetJobOrerSaleInvoiceDetail(strJobOrder)
        End Function

        Public Function GetJobOrerSaleInvoiceDetailEdit(ByVal intRefID As Integer) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetJobOrerSaleInvoiceDetailEdit
            Return objSaleInvoiceDao.GetJobOrerSaleInvoiceDetailEdit(intRefID)
        End Function

        Public Function CountUsedInReceiveHeader(ByVal strInvoice_no As String) As Integer Implements ISale_InvoiceEntity.CountUsedInReceiveHeader
            Return objSaleInvoiceDao.CountUsedInReceiveHeader(strInvoice_no)
        End Function

        Public Function CountUsedInJobOrder(ByVal strJobOrder As String) As Integer Implements ISale_InvoiceEntity.CountUsedInJobOrder
            Return objSaleInvoiceDao.CountUsedInJobOrder(strJobOrder)
        End Function

        Public Function GetSaleInvoiceByJobOrder(ByVal strJobOrder As String) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSaleInvoiceByJobOrder
            Return objSaleInvoiceDao.GetSaleInvoiceByJobOrder(strJobOrder)
        End Function

        Public Function CountCustomerUsedInJobOrder(ByVal strJobOrder As String, ByVal intCustomer As Integer) As Integer Implements ISale_InvoiceEntity.CountCustomerUsedInJobOrder
            Return objSaleInvoiceDao.CountCustomerUsedInJobOrder(strJobOrder, intCustomer)
        End Function

        Public Function InsertSaleInvoice(ByVal strJobOrder1 As String, ByVal strJobOrder2 As String, ByVal strJobOrder3 As String, ByVal objSaleInvoiceEnt As ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer Implements ISale_InvoiceEntity.InsertSaleInvoice
            Return objSaleInvoiceDao.InsertSaleInvoice(strJobOrder1, strJobOrder2, strJobOrder3, objSaleInvoiceEnt, dtValues)
        End Function

        Public Function UpdateSaleInvoice(ByVal strIdDelete As String, ByVal objSaleInvoiceEnt As ISale_InvoiceEntity, ByVal dtValues As System.Data.DataTable) As Integer Implements ISale_InvoiceEntity.UpdateSaleInvoice
            Return objSaleInvoiceDao.UpdateSaleInvoice(strIdDelete, objSaleInvoiceEnt, dtValues)
        End Function

        Public Function GetJobOrderHontai(ByVal dtValues As System.Data.DataTable) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetJobOrderHontai
            Return objSaleInvoiceDao.GetJobOrderHontai(dtValues)
        End Function

        Public Function GetHontaiFinish(ByVal intRefID As Integer) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetHontaiFinish
            Return objSaleInvoiceDao.GetHontaiFinish(intRefID)
        End Function

        Public Function CountUsedInHontai(ByVal intJobOrderId As Integer, ByVal intHontaiFlg As Integer) As Integer Implements ISale_InvoiceEntity.CountUsedInHontai
            Return objSaleInvoiceDao.CountUsedInHontai(intJobOrderId, intHontaiFlg)
        End Function

        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of ISale_InvoiceEntity) Implements ISale_InvoiceEntity.GetAccountTitleForList
            Return objSaleInvoiceDao.GetAccountTitleForList()
        End Function

        Public Function UpdateJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer Implements ISale_InvoiceEntity.UpdateJobOrderPOFlag
            Return objSaleInvoiceDao.UpdateJobOrderPOFlag(dtValues)
        End Function

        Public Function DeleteJobOrderPOFlag(ByVal dtValues As System.Data.DataTable) As Integer Implements ISale_InvoiceEntity.DeleteJobOrderPOFlag
            Return objSaleInvoiceDao.DeleteJobOrderPOFlag(dtValues)
        End Function

        Public Function GetDataBankFreeList(ByVal strReceive_header_id As String) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetDataBankFreeList
            Return objSaleInvoiceDao.GetDataBankFreeList(strReceive_header_id)
        End Function

        Public Function GetDataReceiveDetail(ByVal strReceive_header_id As String) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetDataReceiveDetail
            Return objSaleInvoiceDao.GetDataReceiveDetail(strReceive_header_id)
        End Function

        Public Function GetConfirmReceiveForReport(ByVal strReceiceHeaderId As String) As System.Collections.Generic.List(Of ImpSale_InvoiceEntity) Implements ISale_InvoiceEntity.GetConfirmReceiveForReport
            Return objSaleInvoiceDao.GetConfirmReceiveForReport(strReceiceHeaderId)
        End Function

        Public Function GetSumConfirmReport(ByVal strReceiceHeaderId As String) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSumConfirmReport
            Return objSaleInvoiceDao.GetSumConfirmReport(strReceiceHeaderId)
        End Function

        Public Function DelInvReceiveDetail(ByVal strIntID As Integer) As Integer Implements ISale_InvoiceEntity.DelInvReceiveDetail
            Return objSaleInvoiceDao.DelInvReceiveDetail(strIntID)
        End Function

        Public Function UpdateReciveDetail(ByVal tbIntIdJBPO As System.Data.DataTable, ByVal objIDSaleInv As Dao.ISale_InvoiceDao, ByVal intJobOrderID As Integer) As Boolean Implements ISale_InvoiceEntity.UpdateReciveDetail
            Return objSaleInvoiceDao.UpdateReciveDetail(tbIntIdJBPO, objIDSaleInv, intJobOrderID)
        End Function

        Public Function SumBankfeeConfirmReport(ByVal strReceiceHeaderId As String) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.SumBankfeeConfirmReport
            Return objSaleInvoiceDao.SumBankfeeConfirmReport(strReceiceHeaderId)
        End Function

        Public Function GetSaleInvoiceforUpdate(ByVal strReceiceHeaderId As String) As ISale_InvoiceEntity Implements ISale_InvoiceEntity.GetSaleInvoiceforUpdate
            Return objSaleInvoiceDao.GetSaleInvoiceforUpdate(strReceiceHeaderId)
        End Function

        Public Function UpExChangeRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean Implements ISale_InvoiceEntity.UpExChangeRate
            Return objSaleInvoiceDao.UpExChangeRate(strReceiveHeaderId, ExchangeRate)
        End Function

        Public Function GetActualRate(ByVal intID As Integer) As Integer Implements ISale_InvoiceEntity.GetActualRate
            Return objSaleInvoiceDao.GetActualRate(intID)
        End Function

        Public Function UpdateExChangeRate(ByVal intID As Integer, ByVal objExChangeRate As System.Data.DataTable) As Boolean Implements ISale_InvoiceEntity.UpdateExChangeRate
            Return objSaleInvoiceDao.UpdateExChangeRate(intID, objExChangeRate)
        End Function


#End Region

#Region "Property"

        Public Property po_fg() As String Implements ISale_InvoiceEntity.po_fg
            Get
                Return _po_fg
            End Get
            Set(ByVal value As String)
                _po_fg = value
            End Set
        End Property

        Public Property job_type() As String Implements ISale_InvoiceEntity.job_type
            Get
                Return _job_type
            End Get
            Set(ByVal value As String)
                _job_type = value
            End Set
        End Property

        Public Property hontai_cond() As String Implements ISale_InvoiceEntity.hontai_cond
            Get
                Return _hontai_cond
            End Get
            Set(ByVal value As String)
                _hontai_cond = value
            End Set
        End Property

        Public Property hontai_fg3() As String Implements ISale_InvoiceEntity.hontai_fg3
            Get
                Return _hontai_fg3
            End Get
            Set(ByVal value As String)
                _hontai_fg3 = value
            End Set
        End Property

        Public Property hontai_fg2() As String Implements ISale_InvoiceEntity.hontai_fg2
            Get
                Return _hontai_fg2
            End Get
            Set(ByVal value As String)
                _hontai_fg2 = value
            End Set
        End Property

        Public Property hontai_fg1() As String Implements ISale_InvoiceEntity.hontai_fg1
            Get
                Return _hontai_fg1
            End Get
            Set(ByVal value As String)
                _hontai_fg1 = value
            End Set
        End Property

        Public Property status() As String Implements ISale_InvoiceEntity.status
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property

        Public Property strJobOrder1() As String Implements ISale_InvoiceEntity.strJobOrder1
            Get
                Return _strJobOrder1
            End Get
            Set(ByVal value As String)
                _strJobOrder1 = value
            End Set
        End Property
        Public Property strJobOrder2() As String Implements ISale_InvoiceEntity.strJobOrder2
            Get
                Return _strJobOrder2
            End Get
            Set(ByVal value As String)
                _strJobOrder2 = value
            End Set
        End Property

        Public Property strJobOrder3() As String Implements ISale_InvoiceEntity.strJobOrder3
            Get
                Return _strJobOrder3
            End Get
            Set(ByVal value As String)
                _strJobOrder3 = value
            End Set
        End Property


        Public Property hontai_fg() As String Implements ISale_InvoiceEntity.hontai_fg
            Get
                Return _hontai_fg
            End Get
            Set(ByVal value As String)
                _hontai_fg = value
            End Set
        End Property

        Public Property job_order_po_id() As Integer Implements ISale_InvoiceEntity.job_order_po_id
            Get
                Return _job_order_po_id
            End Get
            Set(ByVal value As Integer)
                _job_order_po_id = value
            End Set
        End Property

        Public Property currency_id() As Integer Implements ISale_InvoiceEntity.currency_id
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property job_order_id() As Integer Implements ISale_InvoiceEntity.job_order_id
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As Integer)
                _job_order_id = value
            End Set
        End Property

        Public Property total_invoice_amount() As String Implements ISale_InvoiceEntity.total_invoice_amount
            Get
                Return _total_invoice_amount
            End Get
            Set(ByVal value As String)
                _total_invoice_amount = value
            End Set
        End Property

        Public Property invoice_type_search() As String Implements ISale_InvoiceEntity.invoice_type_search
            Get
                Return _invoice_type_search
            End Get
            Set(ByVal value As String)
                _invoice_type_search = value
            End Set
        End Property

        Public Property customer_name() As String Implements ISale_InvoiceEntity.customer_name
            Get
                Return _customer_name
            End Get
            Set(ByVal value As String)
                _customer_name = value
            End Set
        End Property

        Public Property invoice_type_name() As String Implements ISale_InvoiceEntity.invoice_type_name
            Get
                Return _invoice_type_name
            End Get
            Set(ByVal value As String)
                _invoice_type_name = value
            End Set
        End Property

        Public Property amount() As String Implements ISale_InvoiceEntity.amount
            Get
                Return _amount
            End Get
            Set(ByVal value As String)
                _amount = value
            End Set
        End Property

        Public Property customer_search() As String Implements ISale_InvoiceEntity.customer_search
            Get
                Return _customer_search
            End Get
            Set(ByVal value As String)
                _customer_search = value
            End Set
        End Property

        Public Property invoice_no_search() As String Implements ISale_InvoiceEntity.invoice_no_search
            Get
                Return _invoice_no_search
            End Get
            Set(ByVal value As String)
                _invoice_no_search = value
            End Set
        End Property

        Public Property issue_date_from_search() As String Implements ISale_InvoiceEntity.issue_date_from_search
            Get
                Return _issue_date_from_search
            End Get
            Set(ByVal value As String)
                _issue_date_from_search = value
            End Set
        End Property

        Public Property issue_date_to_search() As String Implements ISale_InvoiceEntity.issue_date_to_search
            Get
                Return _issue_date_to_search
            End Get
            Set(ByVal value As String)
                _issue_date_to_search = value
            End Set
        End Property

        Public Property job_order_from_search() As String Implements ISale_InvoiceEntity.job_order_from_search
            Get
                Return _job_order_from_search
            End Get
            Set(ByVal value As String)
                _job_order_from_search = value
            End Set
        End Property

        Public Property job_order_to_search() As String Implements ISale_InvoiceEntity.job_order_to_search
            Get
                Return _job_order_to_search
            End Get
            Set(ByVal value As String)
                _job_order_to_search = value
            End Set
        End Property

        Public Property receipt_date_from_search() As String Implements ISale_InvoiceEntity.receipt_date_from_search
            Get
                Return _receipt_date_from_search
            End Get
            Set(ByVal value As String)
                _receipt_date_from_search = value
            End Set
        End Property

        Public Property receipt_date_to_search() As String Implements ISale_InvoiceEntity.receipt_date_to_search
            Get
                Return _receipt_date_to_search
            End Get
            Set(ByVal value As String)
                _receipt_date_to_search = value
            End Set
        End Property

        Public Property account_type() As Integer Implements ISale_InvoiceEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Public Property bank_fee() As String Implements ISale_InvoiceEntity.bank_fee
            Get
                Return _bank_fee
            End Get
            Set(ByVal value As String)
                _bank_fee = value
            End Set
        End Property

        Public Property created_by() As Integer Implements ISale_InvoiceEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements ISale_InvoiceEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property customer() As Integer Implements ISale_InvoiceEntity.customer
            Get
                Return _customer
            End Get
            Set(ByVal value As Integer)
                _customer = value
            End Set
        End Property

        Public Property id() As Integer Implements ISale_InvoiceEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ie_id() As Integer Implements ISale_InvoiceEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Public Property invoice_no() As String Implements ISale_InvoiceEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property invoice_type() As Integer Implements ISale_InvoiceEntity.invoice_type
            Get
                Return _invoice_type
            End Get
            Set(ByVal value As Integer)
                _invoice_type = value
            End Set
        End Property

        Public Property issue_date() As String Implements ISale_InvoiceEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As String)
                _issue_date = value
            End Set
        End Property

        Public Property receipt_date() As String Implements ISale_InvoiceEntity.receipt_date
            Get
                Return _receipt_date
            End Get
            Set(ByVal value As String)
                _receipt_date = value
            End Set
        End Property

        Public Property status_id() As Integer Implements ISale_InvoiceEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property total_amount() As Decimal Implements ISale_InvoiceEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements ISale_InvoiceEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements ISale_InvoiceEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property user_id() As Integer Implements ISale_InvoiceEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements ISale_InvoiceEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Public Property account_title() As String Implements ISale_InvoiceEntity.account_title
            Get
                Return _account_title
            End Get
            Set(ByVal value As String)
                _account_title = value
            End Set
        End Property

        Public Property account_type_name() As String Implements ISale_InvoiceEntity.account_type_name
            Get
                Return _account_type_name
            End Get
            Set(ByVal value As String)
                _account_type_name = value
            End Set
        End Property

        Public Property hontai() As String Implements ISale_InvoiceEntity.hontai
            Get
                Return _hontai
            End Get
            Set(ByVal value As String)
                _hontai = value
            End Set
        End Property

        Public Property job_order() As String Implements ISale_InvoiceEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property po_no() As String Implements ISale_InvoiceEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property po_type_name() As String Implements ISale_InvoiceEntity.po_type_name
            Get
                Return _po_type_name
            End Get
            Set(ByVal value As String)
                _po_type_name = value
            End Set
        End Property

        Public Property remark() As String Implements ISale_InvoiceEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property vat() As String Implements ISale_InvoiceEntity.vat
            Get
                Return _vat
            End Get
            Set(ByVal value As String)
                _vat = value
            End Set
        End Property

        Public Property wt() As String Implements ISale_InvoiceEntity.wt
            Get
                Return _wt
            End Get
            Set(ByVal value As String)
                _wt = value
            End Set
        End Property

        Public Property sum_amount() As Decimal Implements ISale_InvoiceEntity.sum_amount
            Get
                Return _sum_amount
            End Get
            Set(ByVal value As Decimal)
                _sum_amount = value
            End Set
        End Property

        Public Property sum_price() As Decimal Implements ISale_InvoiceEntity.sum_price
            Get
                Return _sum_price
            End Get
            Set(ByVal value As Decimal)
                _sum_price = value
            End Set
        End Property

        Public Property sum_vat() As Decimal Implements ISale_InvoiceEntity.sum_vat
            Get
                Return _sum_vat
            End Get
            Set(ByVal value As Decimal)
                _sum_vat = value
            End Set
        End Property

        Public Property sum_wt() As Decimal Implements ISale_InvoiceEntity.sum_wt
            Get
                Return _sum_wt
            End Get
            Set(ByVal value As Decimal)
                _sum_wt = value
            End Set
        End Property

        Public Property actual_rate() As String Implements ISale_InvoiceEntity.actual_rate
            Get
                Return _actual_rate
            End Get
            Set(ByVal value As String)
                _actual_rate = value
            End Set
        End Property

        Public Property percent() As String Implements ISale_InvoiceEntity.percent
            Get
                Return _percent
            End Get
            Set(ByVal value As String)
                _percent = value
            End Set
        End Property

        Public Property price() As String Implements ISale_InvoiceEntity.price
            Get
                Return _price
            End Get
            Set(ByVal value As String)
                _price = value
            End Set
        End Property

        Public Property stage() As String Implements ISale_InvoiceEntity.stage
            Get
                Return _stage
            End Get
            Set(ByVal value As String)
                _stage = value
            End Set
        End Property

        Public Property actual_amount() As String Implements ISale_InvoiceEntity.actual_amount
            Get
                Return _actual_amount
            End Get
            Set(ByVal value As String)
                _actual_amount = value
            End Set
        End Property

        Public Property bank_rate() As String Implements ISale_InvoiceEntity.bank_rate
            Get
                Return _bank_rate
            End Get
            Set(ByVal value As String)
                _bank_rate = value
            End Set
        End Property

        Public Property currency() As String Implements ISale_InvoiceEntity.currency
            Get
                Return _currency
            End Get
            Set(ByVal value As String)
                _currency = value
            End Set
        End Property

        Public Property po_date() As String Implements ISale_InvoiceEntity.po_date
            Get
                Return _po_date
            End Get
            Set(ByVal value As String)
                _po_date = value
            End Set
        End Property

        Public Property po_type() As Integer Implements ISale_InvoiceEntity.po_type
            Get
                Return _po_type
            End Get
            Set(ByVal value As Integer)
                _po_type = value
            End Set
        End Property

        Public Property receive_header_id() As Integer Implements ISale_InvoiceEntity.receive_header_id
            Get
                Return _receive_header_id
            End Get
            Set(ByVal value As Integer)
                _receive_header_id = value
            End Set
        End Property

        Public Property receive_detail_id() As Integer Implements ISale_InvoiceEntity.receive_detail_id
            Get
                Return _receive_detail_id
            End Get
            Set(ByVal value As Integer)
                _receive_detail_id = value
            End Set
        End Property

        Public Property vat_id() As Integer Implements ISale_InvoiceEntity.vat_id
            Get
                Return _vat_id
            End Get
            Set(ByVal value As Integer)
                _vat_id = value
            End Set
        End Property

        Public Property wt_id() As Integer Implements ISale_InvoiceEntity.wt_id
            Get
                Return _wt_id
            End Get
            Set(ByVal value As Integer)
                _wt_id = value
            End Set
        End Property

        Public Property hontai_type() As String Implements ISale_InvoiceEntity.hontai_type
            Get
                Return _hontai_type
            End Get
            Set(ByVal value As String)
                _hontai_type = value
            End Set
        End Property

        Public Property account_next_approve() As String Implements ISale_InvoiceEntity.account_next_approve
            Get
                Return _account_next_approve
            End Get
            Set(ByVal value As String)
                _account_next_approve = value
            End Set
        End Property

        Public Property name() As String Implements ISale_InvoiceEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property sub_total() As Decimal Implements ISale_InvoiceEntity.sub_total
            Get
                Return _sub_total
            End Get
            Set(ByVal value As Decimal)
                _sub_total = value
            End Set
        End Property

        Public Property vat_amount() As Decimal Implements ISale_InvoiceEntity.vat_amount
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As Decimal)
                _vat_amount = value
            End Set
        End Property

        Public Property wt_amount() As Decimal Implements ISale_InvoiceEntity.wt_amount
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As Decimal)
                _wt_amount = value
            End Set
        End Property

        Public Property remark_detail() As String Implements ISale_InvoiceEntity.remark_detail
            Get
                Return _remark_detail
            End Get
            Set(ByVal value As String)
                _remark_detail = value
            End Set
        End Property
#End Region

    End Class
End Namespace
