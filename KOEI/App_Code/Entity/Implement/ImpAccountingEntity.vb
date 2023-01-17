#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpAccountingEntity
'	Class Discription	: Class of table accounting
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
    Public Class ImpAccountingEntity
        Implements IAccountingEntity

        Private _id As Integer
        Private _type As Integer
        Private _ref_id As Integer
        Private _voucher_no As String
        Private _new_voucher_no As String
        Private _account_type As Integer
        Private _vendor_id As Integer
        Private _vendor_name As String
        Private _bank As String
        Private _account_name As String
        Private _account_no As String
        Private _account_date As String
        Private _job_order As String
        Private _vat_id As Integer
        Private _wt_id As Integer
        Private _ie_id As Integer
        Private _vat_amount As Decimal
        Private _wt_amount As Decimal
        Private _sub_total As Decimal
        Private _remark As String
        Private _cheque_date As String
        Private _cheque_no As String
        Private _status_id As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _updated_by As Integer
        Private _updated_date As String
        Private _category_id As String
        'Receive data from screen (condition search)
        Private _strAccount_id As String
        Private _strAccount_startdate As String
        Private _strAccount_enddate As String
        Private _strJoborder_start As String
        Private _strJoborder_end As String
        Private _strAccount_type As String
        Private _strIe_id As String
        Private _strVendor_name As String
        Private _strPo_startno As String
        Private _strPo_endno As String
        Private _strIe_start_code As String
        Private _strIe_end_code As String
        Private _strStatus_id As String

        Private _min_year As Integer
        Private _latest_year As Integer

        Private _ie_title As String
        Private _po_id As String

        Private _strAccountMonth As String
        Private _strAccountYear As String
        Private _strIe_category_type As String

        Private _strJob_order_text As String
        Private _accType As String
        Private _item_expense As String
        Private _approve_status As String
        Private _applied_by As String
        Private _job_order_id As String
        Private _job_order_po_id As String
        Private _hontai_type As String

        Private _newid As String

        Private _vat_amount_text As String
        Private _wt_amount_text As String
        Private _account_type_text As String

        Private objAccounting As New Dao.ImpAccountingDao


#Region "Function"
        Public Function UpdateChequeApprove(ByVal strAcountId As String, ByVal strAcountType As String, ByVal intStatusId As Integer) As Integer Implements IAccountingEntity.UpdateChequeApprove
            Return objAccounting.UpdateChequeApprove(strAcountId, "3", intStatusId)
        End Function

        Public Function GetChequeApproveList(ByVal objAccountingDto As Dto.AccountingDto) As System.Collections.Generic.List(Of ImpAccountingEntity) Implements IAccountingEntity.GetChequeApproveList
            Return objAccounting.GetChequeApproveList(objAccountingDto)
        End Function

        Public Function CheckAccountByPurchase(ByVal intPurchase_id As Integer) As Boolean Implements IAccountingEntity.CheckAccountByPurchase
            Dim objAccounDao As New Dao.ImpAccountingDao
            Return objAccounDao.DB_CheckAccountByPurchase(intPurchase_id)
        End Function

        Public Function CheckAccountByVendor(ByVal intVendor_id As Integer) As Boolean Implements IAccountingEntity.CheckAccountByVendor
            Dim objAccounDao As New Dao.ImpAccountingDao
            Return objAccounDao.DB_CheckAccountByVendor(intVendor_id)
        End Function

        Public Function InsertIncome(ByVal dtValues As System.Data.DataTable) As Integer Implements IAccountingEntity.InsertIncome
            Dim objAccounDao As New Dao.ImpAccountingDao
            Return objAccounDao.InsertIncome(dtValues)
        End Function
        Public Function GetAccountingList( _
            ByVal objAccountingEnt As IAccountingEntity _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.GetAccountingList
            Return objAccounting.GetAccountingList(objAccountingEnt)
        End Function
        Public Function GetAccountingDetail( _
            ByVal objAccountingEnt As IAccountingEntity _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.GetAccountingDetail
            Return objAccounting.GetAccountingList(objAccountingEnt)
        End Function
        Public Function GetCostTableOverviewList( _
            ByVal objAccountingEnt As IAccountingEntity _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.GetCostTableOverviewList
            Return objAccounting.GetCostTableOverviewList(objAccountingEnt)
        End Function
        Public Function GetCostTableOverviewReport( _
            ByVal objAccountingEnt As IAccountingEntity _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.GetCostTableOverviewReport
            Return objAccounting.GetCostTableOverviewReport(objAccountingEnt)
        End Function

        Public Function CountJobOrder(ByVal strJobOrder As String) As Integer Implements IAccountingEntity.CountJobOrder
            Return objAccounting.CountJobOrder(strJobOrder)
        End Function
        Public Function GetYearList() As System.Collections.Generic.List(Of IAccountingEntity) Implements IAccountingEntity.GetYearList
            Return objAccounting.GetYearList
        End Function

        Public Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.chkCategoryAccountTitle
            Return objAccounting.chkCategoryAccountTitle(ieCode)
        End Function

        Public Function chkExitIEMaster(ByVal code As String) As Integer Implements IAccountingEntity.chkExitIEMaster
            Dim objAccounDao As New Dao.ImpAccountingDao
            Return objAccounDao.chkExitIEMaster(code)
        End Function

        Public Function GetAcountApproveList(ByVal objAccountingDto As Dto.AccountingDto) As System.Collections.Generic.List(Of ImpAccountingEntity) Implements IAccountingEntity.GetAcountApproveList
            Return objAccounting.GetAcountApproveList(objAccountingDto)
        End Function

        Public Function UpdateAcountApprove( _
            ByVal strAcountId As String, _
            ByVal strAcountType As String, _
            ByVal intStatusId As Integer, _
            Optional ByVal objConn As Object = Nothing, _
            Optional ByVal flgTransaction As String = "", _
            Optional ByVal dtValues As System.Data.DataTable = Nothing _
            ) As Integer Implements IAccountingEntity.UpdateAcountApprove

            Return objAccounting.UpdateAcountApprove(strAcountId, strAcountType, intStatusId, objConn, flgTransaction, dtValues)

        End Function
        Public Function GetWIPYear() As System.Collections.Generic.List(Of IAccountingEntity) Implements IAccountingEntity.GetWIPYear
            Return objAccounting.GetWIPYear
        End Function
        Public Function GetAdvanceIncomeReport( _
            ByVal year As String _
        ) As System.Collections.Generic.List(Of ImpMst_AccountingDetailEntity) Implements IAccountingEntity.GetAdvanceIncomeReport
            Return objAccounting.GetAdvanceIncomeReport(year)
        End Function

        Public Function GetAccountApprove(ByVal strAccId As String) As System.Collections.Generic.List(Of IAccountingEntity) Implements IAccountingEntity.GetAccountApprove
            Return objAccounting.GetAccountApprove(strAccId)
        End Function

        Public Function SearchIncomePayment(ByVal intType As Integer, ByVal listPara As System.Collections.Generic.List(Of String)) As System.Collections.Generic.List(Of ImpAccountingEntity) Implements IAccountingEntity.SearchIncomePayment
            Return objAccounting.SearchIncomePayment(intType, listPara)
        End Function

        Public Function GetPoForDeleteList(ByVal strAcountId As String) As System.Collections.Generic.List(Of ImpAccountingEntity) Implements IAccountingEntity.GetPoForDeleteList
            Return objAccounting.GetPoForDeleteList(strAcountId)
        End Function

        Public Function GetAccountingWithID(ByVal intAccID As Integer) As System.Data.DataTable Implements IAccountingEntity.GetAccountingWithID
            Return objAccounting.GetAccountingWithID(intAccID)
        End Function

        Public Function GetAccountApproveByVoucherNo(ByVal strVoucherNo As String, Optional ByVal strStatusID As String = Nothing) As System.Collections.Generic.List(Of ImpAccountingEntity) Implements IAccountingEntity.GetAccountApproveByVoucherNo
            Return objAccounting.GetAccountApproveByVoucherNo(strVoucherNo, strStatusID)
        End Function
        Public Function GetListAccountTitle(ByVal UserId As Integer) As List(Of Entity.ImpAccountingEntity) Implements IAccountingEntity.GetListAccountTitle
            Return objAccounting.GetListAccountTitle(UserId)
        End Function

#End Region

#Region "Property"
        Public Property category_id() As String Implements IAccountingEntity.category_id
            Get
                Return _category_id
            End Get
            Set(ByVal value As String)
                _category_id = value
            End Set
        End Property
        Public Property accType() As String Implements IAccountingEntity.accType
            Get
                Return _accType
            End Get
            Set(ByVal value As String)
                _accType = value
            End Set
        End Property

        Public Property item_expense() As String Implements IAccountingEntity.item_expense
            Get
                Return _item_expense
            End Get
            Set(ByVal value As String)
                _item_expense = value
            End Set
        End Property

        Public Property approve_status() As String Implements IAccountingEntity.approve_status
            Get
                Return _approve_status
            End Get
            Set(ByVal value As String)
                _approve_status = value
            End Set
        End Property

        Public Property applied_by() As String Implements IAccountingEntity.applied_by
            Get
                Return _applied_by
            End Get
            Set(ByVal value As String)
                _applied_by = value
            End Set
        End Property

        Public Property strAccountMonth() As String Implements IAccountingEntity.strAccountMonth
            Get
                Return _strAccountMonth
            End Get
            Set(ByVal value As String)
                _strAccountMonth = value
            End Set
        End Property
        Public Property strAccountYear() As String Implements IAccountingEntity.strAccountYear
            Get
                Return _strAccountYear
            End Get
            Set(ByVal value As String)
                _strAccountYear = value
            End Set
        End Property
        Public Property strIe_category_type() As String Implements IAccountingEntity.strIe_category_type
            Get
                Return _strIe_category_type
            End Get
            Set(ByVal value As String)
                _strIe_category_type = value
            End Set
        End Property
        Public Property ie_title() As String Implements IAccountingEntity.ie_title
            Get
                Return _ie_title
            End Get
            Set(ByVal value As String)
                _ie_title = value
            End Set
        End Property
        Public Property po_id() As String Implements IAccountingEntity.po_id
            Get
                Return _po_id
            End Get
            Set(ByVal value As String)
                _po_id = value
            End Set
        End Property
        Public Property min_year() As Integer Implements IAccountingEntity.min_year
            Get
                Return _min_year
            End Get
            Set(ByVal value As Integer)
                _min_year = value
            End Set
        End Property
        Public Property latest_year() As Integer Implements IAccountingEntity.latest_year
            Get
                Return _latest_year
            End Get
            Set(ByVal value As Integer)
                _latest_year = value
            End Set
        End Property
        Public Property account_date() As String Implements IAccountingEntity.account_date
            Get
                Return _account_date
            End Get
            Set(ByVal value As String)
                _account_date = value
            End Set
        End Property

        Public Property account_name() As String Implements IAccountingEntity.account_name
            Get
                Return _account_name
            End Get
            Set(ByVal value As String)
                _account_name = value
            End Set
        End Property

        Public Property account_no() As String Implements IAccountingEntity.account_no
            Get
                Return _account_no
            End Get
            Set(ByVal value As String)
                _account_no = value
            End Set
        End Property

        Public Property account_type() As Integer Implements IAccountingEntity.account_type
            Get
                Return _account_type
            End Get
            Set(ByVal value As Integer)
                _account_type = value
            End Set
        End Property

        Public Property bank() As String Implements IAccountingEntity.bank
            Get
                Return _bank
            End Get
            Set(ByVal value As String)
                _bank = value
            End Set
        End Property

        Public Property cheque_date() As String Implements IAccountingEntity.cheque_date
            Get
                Return _cheque_date
            End Get
            Set(ByVal value As String)
                _cheque_date = value
            End Set
        End Property

        Public Property cheque_no() As String Implements IAccountingEntity.cheque_no
            Get
                Return _cheque_no
            End Get
            Set(ByVal value As String)
                _cheque_no = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IAccountingEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IAccountingEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property id() As Integer Implements IAccountingEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ie_id() As Integer Implements IAccountingEntity.ie_id
            Get
                Return _ie_id
            End Get
            Set(ByVal value As Integer)
                _ie_id = value
            End Set
        End Property

        Public Property job_order() As String Implements IAccountingEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property new_voucher_no() As String Implements IAccountingEntity.new_voucher_no
            Get
                Return _new_voucher_no
            End Get
            Set(ByVal value As String)
                _new_voucher_no = value
            End Set
        End Property

        Public Property ref_id() As Integer Implements IAccountingEntity.ref_id
            Get
                Return _ref_id
            End Get
            Set(ByVal value As Integer)
                _ref_id = value
            End Set
        End Property

        Public Property remark() As String Implements IAccountingEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IAccountingEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property sub_total() As Decimal Implements IAccountingEntity.sub_total
            Get
                Return _sub_total
            End Get
            Set(ByVal value As Decimal)
                _sub_total = value
            End Set
        End Property

        Public Property type() As Integer Implements IAccountingEntity.type
            Get
                Return _type
            End Get
            Set(ByVal value As Integer)
                _type = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IAccountingEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As String Implements IAccountingEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property

        Public Property vat_amount() As Decimal Implements IAccountingEntity.vat_amount
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As Decimal)
                _vat_amount = value
            End Set
        End Property

        Public Property vat_id() As Integer Implements IAccountingEntity.vat_id
            Get
                Return _vat_id
            End Get
            Set(ByVal value As Integer)
                _vat_id = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements IAccountingEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Public Property voucher_no() As String Implements IAccountingEntity.voucher_no
            Get
                Return _voucher_no
            End Get
            Set(ByVal value As String)
                _voucher_no = value
            End Set
        End Property

        Public Property wt_amount() As Decimal Implements IAccountingEntity.wt_amount
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As Decimal)
                _wt_amount = value
            End Set
        End Property

        Public Property wt_id() As Integer Implements IAccountingEntity.wt_id
            Get
                Return _wt_id
            End Get
            Set(ByVal value As Integer)
                _wt_id = value
            End Set
        End Property
        Public Property strJob_order_text() As String Implements IAccountingEntity.strJob_order_text
            Get
                Return _strJob_order_text
            End Get
            Set(ByVal value As String)
                _strJob_order_text = value
            End Set
        End Property


        'Receive data from screen (condition search)
        Public Property strAccount_id() As String Implements IAccountingEntity.strAccount_id
            Get
                Return _strAccount_id
            End Get
            Set(ByVal value As String)
                _strAccount_id = value
            End Set
        End Property
        Public Property strAccount_startdate() As String Implements IAccountingEntity.strAccount_startdate
            Get
                Return _strAccount_startdate
            End Get
            Set(ByVal value As String)
                _strAccount_startdate = value
            End Set
        End Property
        Public Property strAccount_enddate() As String Implements IAccountingEntity.strAccount_enddate
            Get
                Return _strAccount_enddate
            End Get
            Set(ByVal value As String)
                _strAccount_enddate = value
            End Set
        End Property
        Public Property strJoborder_start() As String Implements IAccountingEntity.strJoborder_start
            Get
                Return _strJoborder_start
            End Get
            Set(ByVal value As String)
                _strJoborder_start = value
            End Set
        End Property
        Public Property strJoborder_end() As String Implements IAccountingEntity.strJoborder_end
            Get
                Return _strJoborder_end
            End Get
            Set(ByVal value As String)
                _strJoborder_end = value
            End Set
        End Property
        Public Property strAccount_type() As String Implements IAccountingEntity.strAccount_type
            Get
                Return _strAccount_type
            End Get
            Set(ByVal value As String)
                _strAccount_type = value
            End Set
        End Property
        Public Property strIe_id() As String Implements IAccountingEntity.strIe_id
            Get
                Return _strIe_id
            End Get
            Set(ByVal value As String)
                _strIe_id = value
            End Set
        End Property
        Public Property strVendor_name() As String Implements IAccountingEntity.strVendor_name
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Public Property strPo_startno() As String Implements IAccountingEntity.strPo_startno
            Get
                Return _strPo_startno
            End Get
            Set(ByVal value As String)
                _strPo_startno = value
            End Set
        End Property
        Public Property strPo_endno() As String Implements IAccountingEntity.strPo_endno
            Get
                Return _strPo_endno
            End Get
            Set(ByVal value As String)
                _strPo_endno = value
            End Set
        End Property
        Public Property strIe_start_code() As String Implements IAccountingEntity.strIe_start_code
            Get
                Return _strIe_start_code
            End Get
            Set(ByVal value As String)
                _strIe_start_code = value
            End Set
        End Property
        Public Property strIe_end_code() As String Implements IAccountingEntity.strIe_end_code
            Get
                Return _strIe_end_code
            End Get
            Set(ByVal value As String)
                _strIe_end_code = value
            End Set
        End Property
        Public Property strStatus_id() As String Implements IAccountingEntity.strStatus_id
            Get
                Return _strStatus_id
            End Get
            Set(ByVal value As String)
                _strStatus_id = value
            End Set
        End Property
        Public Property vendor_name() As String Implements IAccountingEntity.vendor_name
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property

        Public Property newid() As String Implements IAccountingEntity.newid
            Get
                Return _newid
            End Get
            Set(ByVal value As String)
                _newid = value
            End Set
        End Property

        Public Property hontai_type() As String Implements IAccountingEntity.hontai_type
            Get
                Return _hontai_type
            End Get
            Set(ByVal value As String)
                _hontai_type = value
            End Set
        End Property

        Public Property job_order_id() As String Implements IAccountingEntity.job_order_id
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As String)
                _job_order_id = value
            End Set
        End Property

        Public Property job_order_po_id() As String Implements IAccountingEntity.job_order_po_id
            Get
                Return _job_order_po_id
            End Get
            Set(ByVal value As String)
                _job_order_po_id = value
            End Set
        End Property

        Public Property account_type_text() As String Implements IAccountingEntity.account_type_text
            Get
                Return _account_type_text
            End Get
            Set(ByVal value As String)
                _account_type_text = value
            End Set
        End Property

        Public Property vat_amount_text() As String Implements IAccountingEntity.vat_amount_text
            Get
                Return _vat_amount_text
            End Get
            Set(ByVal value As String)
                _vat_amount_text = value
            End Set
        End Property

        Public Property wt_amount_text() As String Implements IAccountingEntity.wt_amount_text
            Get
                Return _wt_amount_text
            End Get
            Set(ByVal value As String)
                _wt_amount_text = value
            End Set
        End Property

#End Region

        
    End Class
End Namespace

