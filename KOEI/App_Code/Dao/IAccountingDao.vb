#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IAccountingDao
'	Class Discription	: Interface of table accounting
'	Create User 		: Boon
'	Create Date		    : 15-05-2013
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

Namespace Dao
    Public Interface IAccountingDao
        Function DB_CheckAccountByVendor(ByVal intVendor_id As Integer) As Boolean
        Function DB_CheckAccountByPurchase(ByVal intPurchase_id As Integer) As Boolean
        Function InsertIncome(ByVal dtValues As DataTable) As Integer

        Function GetAccountingList(ByVal objAccountingEnt As Entity.IAccountingEntity) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetCostTableOverviewList(ByVal objAccountingEnt As Entity.IAccountingEntity) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function GetCostTableOverviewReport(ByVal objAccountingEnt As Entity.IAccountingEntity) As List(Of Entity.ImpMst_AccountingDetailEntity)

        Function CountJobOrder(ByVal strJobOrder As String) As Integer
        Function GetYearList() As List(Of Entity.IAccountingEntity)
        Function GetAccountApprove(ByVal strAccId As String) As List(Of Entity.IAccountingEntity)
        Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function chkExitIEMaster(ByVal code As String) As Integer

        'Function for Accounting Approve Screen
        Function GetChequeApproveList(ByVal objAccountingDto As Dto.AccountingDto) As List(Of Entity.ImpAccountingEntity)
        Function GetAcountApproveList(ByVal objAccountingDto As Dto.AccountingDto) As List(Of Entity.ImpAccountingEntity)
        Function GetPoForDeleteList(ByVal strAcountId As String) As List(Of Entity.ImpAccountingEntity)
        Function UpdateAcountApprove( _
                                    ByVal strAcountId As String, _
                                    ByVal strAcountType As String, _
                                    ByVal intStatusId As Integer, _
                                    Optional ByVal objConn As Object = Nothing, _
                                    Optional ByVal flgTransaction As String = "", _
                                    Optional ByVal dtValues As System.Data.DataTable = Nothing _
                                    ) As Integer

        Function UpdateChequeApprove(ByVal strAcountId As String, _
                                     ByVal strAcountType As String, _
                                     ByVal intStatusId As Integer, _
                                     Optional ByVal objConn As Object = Nothing, _
                                     Optional ByVal flgTransaction As String = "" _
                                     ) As Integer

        Function GetWIPYear() As List(Of Entity.IAccountingEntity)
        Function GetAdvanceIncomeReport(ByVal year As String) As List(Of Entity.ImpMst_AccountingDetailEntity)
        Function SearchIncomePayment(ByVal intType As Integer, ByVal listPara As List(Of String)) As List(Of Entity.ImpAccountingEntity)
        Function GetAccountingWithID(ByVal intAccID As Integer) As DataTable
        Function GetAccountApproveByVoucherNo(ByVal strVoucherNo As String, _
                                              Optional ByVal strStatusID As String = Nothing) As List(Of Entity.ImpAccountingEntity)
        Function GetListAccountTitle(ByVal intAccID As Integer) As List(Of Entity.ImpAccountingEntity)

    End Interface
End Namespace

