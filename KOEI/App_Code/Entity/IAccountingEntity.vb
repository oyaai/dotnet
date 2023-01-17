#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IAccountingEntity
'	Class Discription	: Interface of table accounting
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
Imports System.Data

#End Region

Namespace Entity
    Public Interface IAccountingEntity
        Property id() As Integer
        Property type() As Integer
        Property ref_id() As Integer
        Property voucher_no() As String
        Property new_voucher_no() As String
        Property account_type() As Integer
        Property vendor_id() As Integer
        Property vendor_name() As String
        Property bank() As String
        Property account_name() As String
        Property account_no() As String
        Property account_date() As String
        Property job_order() As String
        Property vat_id() As Integer
        Property wt_id() As Integer
        Property ie_id() As Integer
        Property vat_amount() As Decimal
        Property wt_amount() As Decimal
        Property sub_total() As Decimal
        Property remark() As String
        Property cheque_date() As String
        Property cheque_no() As String
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String

        Property category_id() As String

        'Receive data from screen (condition search)
        Property strAccount_id() As String
        Property strAccount_startdate() As String
        Property strAccount_enddate() As String
        Property strJoborder_start() As String
        Property strJoborder_end() As String
        Property strAccount_type() As String
        Property strIe_id() As String
        Property strVendor_name() As String
        Property strPo_startno() As String
        Property strPo_endno() As String
        Property strIe_start_code() As String
        Property strIe_end_code() As String
        Property strStatus_id() As String

        Property strJob_order_text() As String

        Property min_year() As Integer
        Property latest_year() As Integer

        Property ie_title() As String
        Property po_id() As String
        Property strAccountMonth() As String
        Property strAccountYear() As String
        Property strIe_category_type() As String
        Property accType() As String

        Property item_expense() As String
        Property approve_status() As String
        Property applied_by() As String
        Property job_order_id() As String
        Property job_order_po_id() As String
        Property hontai_type() As String

        Property newid() As String

        Property vat_amount_text() As String
        Property wt_amount_text() As String
        Property account_type_text() As String


        Function CheckAccountByVendor(ByVal intVendor_id As Integer) As Boolean
        Function CheckAccountByPurchase(ByVal intPurchase_id As Integer) As Boolean
        Function InsertIncome(ByVal dtValues As DataTable) As Integer
        Function CountJobOrder(ByVal strJobOrder As String) As Integer

        Function GetAccountingList(ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetAccountingDetail(ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetCostTableOverviewList(ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetCostTableOverviewReport(ByVal objAccountingEnt As Entity.IAccountingEntity _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetYearList() As List(Of Entity.IAccountingEntity)
        Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function chkExitIEMaster(ByVal code As String) As Integer

        'Function for Accounting Approve Screen
        Function GetAcountApproveList(ByVal objAccountingDto As Dto.AccountingDto) As List(Of Entity.ImpAccountingEntity)
        Function GetPoForDeleteList(ByVal strAcountId As String) As List(Of Entity.ImpAccountingEntity)
        Function GetChequeApproveList(ByVal objAccountingDto As Dto.AccountingDto) As List(Of Entity.ImpAccountingEntity)
        Function UpdateAcountApprove(ByVal strAcountId As String, _
                                     ByVal strAcountType As String, _
                                     ByVal intStatusId As Integer, _
                                     Optional ByVal objConn As Object = Nothing, _
                                     Optional ByVal flgTransaction As String = "", _
                                     Optional ByVal dtValues As System.Data.DataTable = Nothing) As Integer
        Function UpdateChequeApprove(ByVal strAcountId As String, ByVal strAcountType As String, ByVal intStatusId As Integer) As Integer
        Function GetWIPYear() As List(Of Entity.IAccountingEntity)
        Function GetAdvanceIncomeReport(ByVal year As String) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetAccountApprove(ByVal strAccId As String) As List(Of Entity.IAccountingEntity)
        Function SearchIncomePayment(ByVal intType As Integer, ByVal listPara As List(Of String)) As List(Of Entity.ImpAccountingEntity)
        Function GetAccountingWithID(ByVal intAccID As Integer) As DataTable
        Function GetAccountApproveByVoucherNo(ByVal strVoucherNo As String, _
                                              Optional ByVal strStatusID As String = Nothing) As List(Of Entity.ImpAccountingEntity)
        Function GetListAccountTitle(ByVal UserId As Integer) As List(Of Entity.ImpAccountingEntity)
    End Interface
End Namespace

