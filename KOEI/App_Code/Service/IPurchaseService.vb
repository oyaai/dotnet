#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPurchaseService
'	Class Discription	: Interface of Purchase
'	Create User 		: Boonyarit
'	Create Date		    : 11-06-2013
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
    Public Interface IPurchaseService
        Function SaveTaskPurchase(ByVal strMode As String, ByVal intPoId_New As Integer) As Boolean
        Function CheckPurchaseByJobOrder(ByVal strJobOrder As String) As Boolean
        Function GetS_Rate(ByVal intCurrencyId As Integer, Optional ByVal strDate As String = "") As Decimal
        Function CheckTypePurchaeApprove(ByVal objType As Enums.PurchaseTypes) As Integer
        Function SetDiscountType(ByRef objValue As DropDownList, Optional ByVal strCurrencyName As String = "Currency Name") As Boolean
        Function SetItemList(ByRef objValue As DropDownList, Optional ByVal strVendorId As String = Nothing) As Boolean
        Function GetPurchaseForSearch(ByVal objValue As Dto.PurchaseSearchDto, ByRef objDT As System.Data.DataTable) As Boolean
        Function GetPurchaseForDetail(ByVal intPurchaseId As Integer, ByRef objDT As System.Data.DataTable) As Dto.PurchaseDto
        Function GetPurchaseForReport(ByVal objValue As Dto.PurchaseSearchDto) As Boolean
        Function DeletePurchase(ByVal intPurchaseId As Integer) As Boolean
        Function CheckPurchase(ByVal intPurchaseId As Integer) As Boolean
        Function SetAllDDL(ByRef objValue As List(Of DropDownList), ByRef thbIndex As String) As Boolean
        Function SavePurchase(ByVal objPurchaseDto As Dto.PurchaseDto, ByVal strMode As String, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Boolean
        Function GetPurchaseById(ByVal intPurchaseId As Integer) As Dto.PurchaseDto
        Function GetPoNo() As String
        Function GetDataReport(ByVal intPurchaseId As Integer) As System.Data.DataSet
        'Function for Purchase Approve Screen
        Function GetPurchaseApproveList(ByVal objPurchaseS As Dto.PurchaseSearchDto) As DataTable
        Function UpdatePurchaseStatus(ByVal strPurchaseId As String, ByVal intStatus As Integer, Optional ByRef strMsg As String = "") As Boolean
        Function GetPOApprove(ByVal strPoId As String) As DataTable
    End Interface
End Namespace

