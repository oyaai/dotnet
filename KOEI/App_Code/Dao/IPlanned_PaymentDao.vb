#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IPlanned_PaymentDao
'	Class Discription	: Interface of Planned Payment Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-08-2013
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
    Public Interface IPlanned_PaymentDao 
        Function GetYearList() As List(Of Entity.IPlanned_PaymentEntity)
        Function GetJobOrderForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetInvoiceForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetSumAmountThbForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetAmountThbForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetMaxPayDateForReport(ByVal intYear As Integer) As Entity.IPlanned_PaymentEntity
    End Interface
End Namespace
