#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPlanned_PaymentEntity
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

Namespace Entity
    Public Interface IPlanned_PaymentEntity

#Region "Property"
        'Receive data for search screen
        Property min_year() As String
        Property max_year() As String

        'Receive data for excel report
        Property job_order_id() As Integer
        Property job_order() As String
        Property customer() As String
        Property description() As String
        Property receive_header_id() As Integer
        Property invoice_no() As String
        Property pay_date() As String
        Property amount_thb() As String
        Property sum_amount_thb() As String
        Property max_pay_date() As String

#End Region

#Region "Function" 
        Function GetYearList() As List(Of Entity.IPlanned_PaymentEntity)
        Function GetJobOrderForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetInvoiceForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetSumAmountThbForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetAmountThbForReport(ByVal intYear As Integer) As List(Of Entity.ImpPlanned_PaymentEntity)
        Function GetMaxPayDateForReport(ByVal intYear As Integer) As Entity.IPlanned_PaymentEntity
#End Region

    End Interface
End Namespace
