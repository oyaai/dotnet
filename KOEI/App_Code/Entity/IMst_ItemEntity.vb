#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_ItemEntity
'	Class Discription	: Interface of table mst_item
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
    Public Interface IMst_ItemEntity
        Property id() As Integer
        Property name() As String
        Property vendor_id() As Integer
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String

        Function CheckItemByVendor(ByVal intVendor_id As Integer) As Boolean
        Function GetItemList(ByVal strItemName As String, ByVal strVendorID As String) As List(Of Entity.ImpMst_ItemDetailEntity)
        Function DeleteItem(ByVal intItemID As Integer) As Integer
        Function GetItemByID(ByVal intItemID As Integer) As Entity.IMst_ItemEntity
        Function InsertItem(ByVal objItemEnt As Entity.IMst_ItemEntity) As Integer
        Function UpdateItem(ByVal objItemEnt As Entity.IMst_ItemEntity) As Integer
        Function CountUsedInPO(ByVal intItemID As Integer) As Integer
        Function GetListItem(Optional ByVal strVendorId As String = Nothing) As List(Of ImpMst_ItemEntity)
    End Interface
End Namespace

