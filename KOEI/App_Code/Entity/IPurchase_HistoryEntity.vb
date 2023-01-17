Imports Microsoft.VisualBasic

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPurchase_HistoryEntity
'	Class Discription	: Interface of table Purchase_History  
'	Create User 		: Nisa S.
'	Create Date		    : 19-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Namespace Entity

    Public Interface IPurchase_HistoryEntity

#Region "Property"

        Property job_order() As String
        Property po_no() As String
        Property invoice_no() As String
        Property delivery_amount() As String
        Property VendorName() As String
        Property ItemName() As String
        Property delivery_date() As String
        Property delivery_qty() As Integer
        Property status_id() As Integer
        Property code() As String

        Property delivery_date1() As String
        Property delivery_date2() As String

#End Region

#Region "Function"
        Function GetPurchaseHistoryReport(ByVal objPurchaseHistoryEnt As Entity.IPurchase_HistoryEntity) As List(Of Entity.ImpPurchase_HistoryEntity)
#End Region

    End Interface

End Namespace