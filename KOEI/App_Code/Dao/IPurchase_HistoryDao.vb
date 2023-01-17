Imports Microsoft.VisualBasic

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IPurchase_HistoryDao
'	Class Discription	: Interface of table Purchase History
'	Create User 		: Nisa S.
'	Create Date		    : 23-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Namespace Dao

    Public Interface IPurchase_HistoryDao

        Function GetPurchaseHistoryReport(ByVal objPurchaseHistoryEnt As Entity.IPurchase_HistoryEntity) As List(Of Entity.ImpPurchase_HistoryEntity)

    End Interface

End Namespace