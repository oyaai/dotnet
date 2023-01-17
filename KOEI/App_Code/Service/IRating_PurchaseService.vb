#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IRating_PurchaseService
'	Class Discription	: Interface Rating Purchase service
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

Imports Microsoft.VisualBasic
Imports System.Data

Namespace Service
    Public Interface IRating_PurchaseService
        Function GetRating_PurchaseList(ByVal objRatingDto As Dto.Rating_PurchaseDto) As DataTable
        Function GetVendorRatingReport(ByVal objRatingDto As Dto.Rating_PurchaseDto) As DataTable
        Function GetYearVendorRatingReport(ByVal objRatingDto As Dto.Rating_PurchaseDto) As DataTable
        Function DeleteRatingInvoice(ByVal intRatingId As Integer) As Boolean
        Function GetPurchaseList(ByVal objRatingDto As Dto.Rating_PurchaseDto) As DataTable
        Function GetRatingVendor(ByVal ratingId As String, ByVal payment_header_id As String) As DataTable
        Function CheckDupVendor_Rating(ByVal id As String, ByVal payment_header_id As String, Optional ByRef strMsg As String = "") As Boolean
        Function InsUpdVendor_Rating(ByVal mode As String, _
                                     ByVal strId As String, _
                                     ByVal strPayment_header_id As String, _
                                     ByVal strQuality As String, _
                                     ByVal strDelivery As String, _
                                     ByVal strService As String, _
                                     Optional ByRef strMsg As String = "") As Boolean
    End Interface
End Namespace

