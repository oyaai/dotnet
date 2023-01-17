#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IAcc_PaymentService
'	Class Discription	: Interface class Accounting payment service
'	Create User 		: Wasan D.
'	Create Date		    : 09-08-2013
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
    Public Interface IAcc_PaymentService
        Function GetDataForVoucherReport(ByVal voucherList As String) As DataTable
        Function GetDataForWTReport(ByVal voucherList As String) As DataTable
        Function GetDataForWTReportV2(ByVal voucherList As String) As DataTable
        Function ExcelPaymentWTReport(ByVal dtPaymentWT As DataTable, ByVal callFromPage As String) As String
        Function ExcelPaymentWTReportV2(ByVal dtPaymentWT As DataTable, ByVal callFromPage As String) As String
    End Interface
End Namespace

