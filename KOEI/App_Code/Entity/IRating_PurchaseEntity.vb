#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IRating_PurchaseEntity
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
    Public Interface IRating_PurchaseEntity
        'Receive value from screen search
        Property strSearchType() As String
        Property strPO() As String
        Property strDeliveryDateFrom() As String
        Property strDeliveryDateTo() As String
        Property strPaymentDateFrom() As String
        Property strPaymentDateTo() As String
        Property strVendor_name() As String
        Property strRating_start() As String
        Property strRating_end() As String
        Property strInvoce_no() As String

        'item from database vendor_rating
        Property id() As String
        Property payment_header_id() As String
        Property quality() As String
        Property delivery() As String
        Property service() As String
        Property created_by() As String
        Property created_date() As String
        Property updated_by() As String
        Property updated_date() As String

        'item from database 
        Property invoice_no() As String
        Property po_no() As String
        Property payment_date() As String
        Property delivery_date() As String
        Property score() As String


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
        Function InsUpdVendor_Rating(ByVal mode As String, _
                                     ByVal strId As String, _
                                     ByVal strPayment_header_id As String, _
                                     ByVal strQuality As String, _
                                     ByVal strDelivery As String, _
                                     ByVal strService As String _
        ) As Integer
    End Interface
End Namespace

