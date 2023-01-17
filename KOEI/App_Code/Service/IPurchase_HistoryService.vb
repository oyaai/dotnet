Imports Microsoft.VisualBasic
Imports System.Data

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPurchase_HistoryService
'	Class Discription	: Interface of Purchase_History  
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

Namespace Service

    Public Interface IPurchase_HistoryService

        Function GetPurchaseHistoryReport(ByVal objPurchaseHistoryDto As Dto.Purchase_HistoryDto) As DataTable

    End Interface

End Namespace