#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IFinish_GoodsDao
'	Class Discription	: Interface of table job order
'	Create User 		: Suwishaya L.
'	Create Date		    : 02-07-2013
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
    Public Interface IFinish_GoodsDao
        Function GetFinishGoodsList(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetFinishGoodsReport(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetSumFinishGoodsReport(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetPersonInChangeForList() As List(Of Entity.IFinish_GoodsEntity)
    End Interface
End Namespace
