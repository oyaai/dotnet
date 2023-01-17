#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IAcc_PaymentDao
'	Class Discription	: Interface of table accounting for Accounting Imcome & Payment
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

#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Data

#End Region

Namespace Dao
    Public Interface IAcc_PaymentDao
        Function GetDataForVoucherReport(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
        Function GetDataForWTReport(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
        Function GetDataForWTReportV2(ByVal voucherList As String) As List(Of Entity.ImpAcc_PaymentEntity)
    End Interface

End Namespace
