#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPlanned_PaymentService
'	Class Discription	: Interface class Planned Payment Report
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
Imports System.Data
#End Region

Namespace Service
    Public Interface IPlanned_PaymentService
        Function GetYearList() As DataTable
        Function GetJobOrderForReport(ByVal intYear As Integer) As DataTable
        Function GetInvoiceForReport(ByVal intYear As Integer) As DataTable
        Function GetSumAmountThbForReport(ByVal intYear As Integer) As DataTable
        Function GetAmountThbForReport(ByVal intYear As Integer) As DataTable
        Function GetMaxPayDateForReport(ByVal intYear As Integer) As Dto.PlannedPaymentDto
    End Interface
End Namespace
