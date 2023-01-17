#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPo_HeaderEntity
'	Class Discription	: Interface of table po_header
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
#End Region

Namespace Entity
    Public Interface IPo_HeaderEntity

#Region "Property"
        Property id() As Integer
        Property po_no() As String
        Property po_type() As Integer
        Property vendor_id() As Integer
        Property vendor_branch_id() As Integer
        Property quotation_no() As String
        Property issue_date() As DateString
        Property delivery_date() As DateString
        Property payment_term_id() As Integer
        Property vat_id() As Integer
        Property wt_id() As Integer
        Property currency_id() As Integer
        Property remark() As String
        Property discount_total() As Decimal
        Property sub_total() As Decimal
        Property vat_amount() As Decimal
        Property wt_amount() As Decimal
        Property total_amount() As Decimal
        Property attn() As String
        Property deliver_to() As String
        Property contact() As String
        Property user_id() As Integer
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As DateString
        Property updated_by() As Integer
        Property updated_date() As DateString

#End Region

        Function CheckPoByVendor(ByVal intVendor_id As Integer) As Boolean
        Function SearchPurchase(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseEntity)
        Function SearchPurchase(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity
        Function DeletePurchase(ByVal intPurchaseId As Integer) As Boolean
        Function SearchPurchaseDetail(ByVal intPurchaseId As Integer) As Entity.ImpPurchaseEntity
        Function SearchPurchaseReport(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseReportEntity)
        Function SearchPurchasePDF(ByVal intPurchaseId As Integer) As Entity.ImpPurchasePDFEntty
        Function InsertPurchase(ByVal objPurchase As Entity.ImpPurchaseEntity, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Integer
        Function UpdatePurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer
        Function ModifyPurchase(ByRef objPurchase As Entity.ImpPurchaseEntity) As Integer
        Function GetPoNo(Optional ByRef intPoNo As Integer = 0) As String
        Function InsertTaskPurchase(ByVal intPoId_New As Integer) As Integer
        Function UpdateTaskPurchase(ByVal intPoId_New As Integer) As Integer
        'Function for Purchase Approve Screen
        Function GetPurchaseApproveList(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of Entity.ImpPurchaseEntity)
        Function UpdatePurchaseStatus(ByVal strPurchaseId As String, ByVal intStatus As Integer) As Integer
        Function GetPOApprove(ByVal strPoId As String) As List(Of Entity.IPo_HeaderEntity)
    End Interface
End Namespace



