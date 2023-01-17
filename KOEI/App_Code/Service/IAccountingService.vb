#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IAccountingService
'	Class Discription	: Interface class Accounting service
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic
Imports System.Data

Namespace Service
    Public Interface IAccountingService
        Function GetAccountingList( _
            ByVal objAccountingDto As Dto.AccountingDto, _
            ByVal dataType As String _
        ) As DataTable
        Function GetAccountingDetail( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As DataTable
        Function GetWithholdingList( _
            ByVal objAccountingDto As Dto.AccountingDto, _
            ByVal dataType As String _
        ) As DataTable
        Function GetCostTableDetailList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As DataTable
        Function GetCostTableOverviewList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As DataTable
        Function GetCostTableOverviewReport( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As DataTable
        Function InsertIncome( _
            ByVal dtValues As DataTable _
        ) As Boolean
        Function GetTableReport( _
            ByVal dtValues As DataTable _
        ) As DataTable
        Function GetTableReport( _
            ByVal dtValues As DataTable, _
            ByVal sortsortColumn As String _
        ) As DataTable
        Function IsExistJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean
        Function GetYearList() As DataTable
        Function chkCategoryAccountTitle( _
            ByVal ieCode As String _
        ) As String
        Function chkExitIEMaster(ByVal code As String) As Boolean

        'Function for Accounting Approve Screen
        Function GetAcountApproveList(ByVal objAccountingDto As Dto.AccountingDto) As DataTable
        Function GetPoForDeleteList(ByVal strAcountId As String) As DataTable
        Function UpdateAcountApprove( _
            ByVal strAcountId As String, _
            ByVal strAcountType As String, _
            ByVal intStatusId As Integer, _
            Optional ByVal objConn As Object = Nothing, _
            Optional ByVal flgTransaction As String = "", _
            Optional ByVal dtValues As System.Data.DataTable = Nothing, _
            Optional ByRef strMsg As String = "" _
            ) As Boolean

        Function GetWIPYear() As DataTable
        Function GetAdvanceIncomeReport(ByVal year As String) As DataTable
        Function GetAccountApprove(ByVal strAccId As String) As DataTable

        'Function for Cheque Approve Screen (Boonyarit KTAP03)
        Function GetChequeApproveList( _
            ByVal objAccountingDto As Dto.AccountingDto _
        ) As System.Data.DataTable

        Function UpdateChequeApprove( _
            ByVal strNewId As String, _
            ByVal strAcountType As String, _
            ByVal intStatusId As Integer, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean

        Function SearchIncomePayment( _
            ByVal intType As Integer, _
            ByVal listPara As List(Of String) _
        ) As DataTable

        Function GetAccountingWithID(ByVal intAccID As Integer) As DataTable
        Function GetAccountApproveByVoucherNo(ByVal strVoucherNo As String, Optional ByVal strStatusID As String = Nothing) As DataTable
        Function GetListAccountTitle(ByVal UserId As Integer) As List(Of Entity.ImpAccountingEntity)

    End Interface
End Namespace

