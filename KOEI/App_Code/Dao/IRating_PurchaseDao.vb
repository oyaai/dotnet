#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IRating_PurchaseDao
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

Namespace Dao
    Public Interface IRating_PurchaseDao
        Function GetRating_PurchaseList( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As List(Of Entity.ImpRating_PurchaseDetailEntity)
        Function GetVendorRatingReport( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As List(Of Entity.ImpRating_PurchaseDetailEntity)
        Function GetYearVendorRatingReport( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As List(Of Entity.ImpRating_PurchaseDetailEntity)
        Function DeleteRatingInvoice(ByVal intRatingId As Integer) As Integer
        Function GetPurchaseList( _
            ByVal objRatingEnt As Entity.IRating_PurchaseEntity _
        ) As List(Of Entity.ImpRating_PurchaseDetailEntity)
        Function GetRatingVendor( _
            ByVal ratingId As String, _
            ByVal payment_header_id As String _
        ) As List(Of Entity.ImpRating_PurchaseDetailEntity)
        Function InsUpdVendor_Rating( _
            ByVal mode As String, _
            ByVal strId As String, _
            ByVal strPayment_header_id As String, _
            ByVal strQuality As String, _
            ByVal strDelivery As String, _
            ByVal strService As String) As Integer
    End Interface

End Namespace


