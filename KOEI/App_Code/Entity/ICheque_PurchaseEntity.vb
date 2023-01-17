#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ICheque_PurchaseEntity
'	Class Discription	: Interface of Rating Purchase
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

Namespace Entity
    Public Interface ICheque_PurchaseEntity
        'Receive value from screen search
        Property strSearchType() As String
        Property strChequeNo() As String
        Property strChequeDateFrom() As String
        Property strChequeDateTo() As String
        Property strVendor_name() As String

        'item from database vendor_rating
        Property id() As String


        Function GetCheque_PurchaseList( _
            ByVal objRatingEnt As Entity.ICheque_PurchaseEntity _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetCheque_Head( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetCheque_Detail( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetAccounting_Detail( _
            ByVal id As String, _
            ByVal dtDate As String, _
            ByVal mode As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        'Function GetApprover( _
        ') As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function UpdateAccounting( _
            ByVal strApprover As String, _
            ByVal dtInsAcc As DataTable, _
            ByRef errorType As String _
        ) As Integer

        Function GetPurchasePaidReport(ByVal itemConfirm As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetPaymentVoucher(ByVal itemConfirm As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetTaxReport(ByVal itemConfirm As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function GetAccountReport(ByVal itemConfirm As String _
        ) As List(Of Entity.ImpCheque_PurchaseDetailEntity)

        Function DeleteCheque( ByVal strId As String) As Integer

        Function CheckDupAccounting( _
            ByVal cheque_no As String, _
            ByVal cheque_date As String _
        ) As Integer

    End Interface
End Namespace

