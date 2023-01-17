#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IInvoice_PurchaseDao
'	Class Discription	: Interface of Invoice Purchase
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
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
    Public Interface IInvoice_PurchaseDao
        Function GetInvoice_PurchaseList( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetInvoice_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetInvoice_Detail( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)
        Function CountVendor_rating(ByVal intItemID As Integer) As Integer
        Function CountAccounting(ByVal intItemID As Integer) As Integer
        Function DeletePayment_Header(ByVal intId As Integer) As Integer
        Function DeletePayment_Detail(ByVal intPaymentHeaderID As Integer) As Integer
        Function DeletePayment(ByVal intId As Integer) As Integer
        Function ExecuteAccounting(ByVal itemConfirm As String) As Boolean
        Function GetNewVoucher() As Integer
        Function InsertAccounting( _
            ByVal itemConfirm As String, _
            ByVal newVoucher As String _
        ) As Integer
        Function UpdateVoucher_running(ByVal newVoucher As Integer) As Integer
        Function Update_Accounting_Con(ByVal itemConfirm As String, ByVal newVoucher As String) As Integer
        Function UpdatePayment_header_Con(ByVal itemConfirm As String) As Integer
        Function GetPurchasePaidReport(ByVal itemConfirm As String) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)
        Function GetPO_List( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_Header( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_Detail( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_Detail_Insert( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPO_Header_Insert( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function InsertPayment( _
                                ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
                                ByVal dtPaymentDetail As DataTable _
        ) As Integer

        Function InsertPayment_Header( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As Integer

        Function InsertPayment_Detail( _
            ByVal strPo_header_id As Integer, _
            ByVal dtPaymentDetail As DataTable _
        ) As Integer

        Function InsertStock( _
            ByVal dtPaymentDetail As DataTable, _
            ByVal row As Integer, _
            ByVal strPo_header_id As Long _
        ) As Integer

        Function updPO_Header_Status( _
            ByVal id As Integer _
        ) As Integer

        Function GetPaymentHeader( _
                                    ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function GetPaymentDetail( _
                                   ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
       ) As List(Of Entity.ImpInvoice_PurchaseDetailEntity)

        Function UpdatePayment( _
                                ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity, _
                                ByVal dtPaymentDetail As DataTable _
        ) As Integer

        Function checkConfirmPayment() As Integer
        Function checkInvoice(ByVal vendor_id As String, ByVal invoice_no As String, ByVal id As String) As Integer
        Function UpdatePayment_Header_Del( _
            ByVal objInvPurchaseEnt As Entity.IInvoice_PurchaseEntity _
        ) As Integer
        Function deleteTask( _
            ByVal taskID As String, _
            ByVal id As String _
        ) As Integer
    End Interface

End Namespace


