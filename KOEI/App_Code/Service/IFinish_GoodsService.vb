#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IFinish_GoodsService
'	Class Discription	: Interface of Finish Goods
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
Imports System.Data
#End Region

Namespace Service
    Public Interface IFinish_GoodsService
        Function GetFinishGoodsList(ByVal objFinishGoodsDto As Dto.FinishGoodsDto) As DataTable
        Function GetFinishGoodsReport(ByVal objFinishGoodsDto As Dto.FinishGoodsDto) As DataTable
        Function GetSumFinishGoodsReport(ByVal objFinishGoodsDto As Dto.FinishGoodsDto) As DataTable
        Function GetPersonInChangeForList() As List(Of Dto.FinishGoodsDto)
    End Interface
End Namespace
