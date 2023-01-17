#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpCheque_PurchaseEntity
'	Class Discription	: Class of table accounting
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Class ImpCheque_PurchaseEntity
        Implements ICheque_PurchaseEntity


        'Receive value from screen search
        Private _strSearchType As String
        Private _strChequeNo As String
        Private _strChequeDateFrom As String
        Private _strChequeDateTo As String
        Private _strVendor_name As String

        'item from database vendor_rating
        Private _id As String

        Private objInvPurchase As New Dao.ImpCheque_PurchaseDao

#Region "Function"
        Public Function GetCheque_PurchaseList( _
            ByVal objRatingEnt As ICheque_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetCheque_PurchaseList
            Return objInvPurchase.GetCheque_PurchaseList(objRatingEnt)
        End Function

        Public Function GetCheque_Head( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetCheque_Head
            Return objInvPurchase.GetCheque_Head(strChequeNo, strChequeDate)
        End Function

        Public Function GetCheque_Detail( _
            ByVal strChequeNo As String, _
            ByVal strChequeDate As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetCheque_Detail
            Return objInvPurchase.GetCheque_Detail(strChequeNo, strChequeDate)
        End Function

        Public Function GetAccounting_Detail( _
            ByVal id As String, _
            ByVal dtDate As String, _
            ByVal mode As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetAccounting_Detail
            Return objInvPurchase.GetAccounting_Detail(id, dtDate, mode)
        End Function

        'Public Function GetApprover( _
        ') As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetApprover
        '    Return objInvPurchase.GetApprover()
        'End Function
        Public Function UpdateAccounting( _
            ByVal strApprover As String, _
            ByVal dtInsAcc As DataTable, _
            ByRef errorType As String _
        ) As Integer Implements ICheque_PurchaseEntity.UpdateAccounting
            Return objInvPurchase.UpdateAccounting(strApprover, dtInsAcc, errorType)
        End Function

        Public Function GetPurchasePaidReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetPurchasePaidReport
            Return objInvPurchase.GetPurchasePaidReport(itemConfirm)
        End Function

        Public Function GetPaymentVoucher( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetPaymentVoucher
            Return objInvPurchase.GetPaymentVoucher(itemConfirm)
        End Function

        Public Function GetTaxReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetTaxReport
            Return objInvPurchase.GetTaxReport(itemConfirm)
        End Function

        Public Function GetAccountReport( _
            ByVal itemConfirm As String _
        ) As System.Collections.Generic.List(Of ImpCheque_PurchaseDetailEntity) Implements ICheque_PurchaseEntity.GetAccountReport
            Return objInvPurchase.GetAccountReport(itemConfirm)
        End Function

        Public Function DeleteCheque( ByVal strId As String) As Integer Implements ICheque_PurchaseEntity.DeleteCheque
            Return objInvPurchase.DeleteCheque(strId)
        End Function

        Public Function CheckDupAccounting( _
            ByVal cheque_no As String, _
            ByVal cheque_date As String _
        ) As Integer Implements ICheque_PurchaseEntity.CheckDupAccounting
            Return objInvPurchase.CheckDupAccounting(cheque_no, cheque_date)
        End Function
#End Region

#Region "Property"
        Public Property strChequeNo() As String Implements ICheque_PurchaseEntity.strChequeNo
            Get
                Return _strChequeNo
            End Get
            Set(ByVal value As String)
                _strChequeNo = value
            End Set
        End Property
        Public Property strSearchType() As String Implements ICheque_PurchaseEntity.strSearchType
            Get
                Return _strSearchType
            End Get
            Set(ByVal value As String)
                _strSearchType = value
            End Set
        End Property
        Public Property strChequeDateFrom() As String Implements ICheque_PurchaseEntity.strChequeDateFrom
            Get
                Return _strChequeDateFrom
            End Get
            Set(ByVal value As String)
                _strChequeDateFrom = value
            End Set
        End Property
        Public Property strChequeDateTo() As String Implements ICheque_PurchaseEntity.strChequeDateTo
            Get
                Return _strChequeDateTo
            End Get
            Set(ByVal value As String)
                _strChequeDateTo = value
            End Set
        End Property
        Public Property strVendor_name() As String Implements ICheque_PurchaseEntity.strVendor_name
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Public Property id() As String Implements ICheque_PurchaseEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

#End Region

    End Class
End Namespace


