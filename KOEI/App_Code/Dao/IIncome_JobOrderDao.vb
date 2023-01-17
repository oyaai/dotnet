#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IIncome_JobOrderDao
'	Class Discription	: Interface of table receive_header
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
#End Region

Namespace Dao
    Public Interface IIncome_JobOrderDao
        Function GetIncomeList(ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
        Function GetMonthlySaleReport(ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
        Function GetSumMonthlySaleReport(ByVal objIncomeEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
    End Interface
End Namespace
