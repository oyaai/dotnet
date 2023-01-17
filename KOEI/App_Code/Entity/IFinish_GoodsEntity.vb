#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IFinish_GoodsEntity
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

Namespace Entity
    Public Interface IFinish_GoodsEntity

#Region "Property"
        'Receive data from screen (condition search)
        Property job_order_from_search() As String
        Property job_order_to_search() As String
        Property job_type_search() As String
        Property part_no_search() As String
        Property part_name_search() As String
        Property customer_search() As String
        Property person_charge_search() As String
        Property issue_date_from_search() As String
        Property issue_date_to_search() As String
        Property finish_date_from_search() As String
        Property finish_date_to_search() As String
        Property boi_search() As String
        Property receive_po_search() As String

        'Receive data detail 
        Property id() As Integer
        Property job_order() As String
        Property issue_date() As String
        Property finish_date() As String
        Property customer_name() As String
        Property job_order_type_name() As String
        Property part_name() As String
        Property part_no() As String
        Property amount() As String
        Property sum_amount() As String
        Property person_in_charge_id() As Integer
        Property person_in_charge_name() As String

        Property receive_header_id() As String
        Property job_order_id() As String
#End Region

#Region "Function"
        Function GetFinishGoodsList(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetFinishGoodsReport(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetSumFinishGoodsReport(ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity) As List(Of Entity.ImpFinish_GoodsEntity)
        Function GetPersonInChangeForList() As List(Of Entity.IFinish_GoodsEntity)
#End Region

    End Interface
End Namespace
