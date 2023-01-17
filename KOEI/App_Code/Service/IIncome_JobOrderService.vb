#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IIncome_JobOrderService
'	Class Discription	: Interface of Income Job Order
'	Create User 		: Suwishaya L.
'	Create Date		    : 01-07-2013
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
    Public Interface IIncome_JobOrderService
        Function GetIncomeList(ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto) As DataTable
        Function GetMonthlySaleReport(ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto) As DataTable
        Function GetSumMonthlySaleReport(ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto) As DataTable
    End Interface
End Namespace
