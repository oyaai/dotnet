#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IPo_HeaderDao
'	Class Discription	: Interface of table po_header
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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

Namespace Dao
    Public Interface IPo_HeaderDao
        Function DB_CheckPoByVendor(ByVal intVendor_id As Integer) As Boolean
        Function DB_SearchPurchase(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity
        Function DB_SearchPurchase(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseEntity)
        Function DB_DeletePurchase(ByVal intPurchaseId As Integer) As Boolean
        Function DB_SearchPurchaseDetail(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity
        Function DB_SearchPurchaseReport(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseReportEntity)
        Function DB_SearchPurchasePDF(ByVal intPurchaseId As Integer) As Entity.ImpPurchasePDFEntty
        Function DB_InsertPurchase(ByVal objPurchase As Entity.ImpPurchaseEntity, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Integer
        Function DB_UpdatePurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer
        Function DB_ModifyPurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer
        Function DB_GetPoNo(Optional ByRef intPoNo As Integer = 0) As String
        Function DB_InsertTaskPurchase(ByVal intPoId_New As Integer) As Integer
        Function DB_UpdateTaskPurchase(ByVal intPoId_New As Integer) As Integer
        'Function for Purchase Approve Screen
        Function GetPurchaseApproveList(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseEntity)
        Function UpdatePurchaseStatus(ByVal strPurchaseId As String, ByVal intStatus As Integer) As Integer
        Function GetPOApprove(ByVal strPoId As String) As List(Of Entity.IPo_HeaderEntity)
    End Interface
End Namespace

