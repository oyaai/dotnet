#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_ItemDao
'	Class Discription	: Interface of table mst_item
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
    Public Interface IMst_ItemDao
        Function DB_CheckItemByVendor(ByVal intVendor_id As Integer) As Boolean
        Function DB_GetListItem(Optional ByVal strVendorId As String = Nothing) As List(Of Entity.ImpMst_ItemEntity)
        Function GetItemList(ByVal strItemName As String, ByVal strVendorID As String) As List(Of Entity.ImpMst_ItemDetailEntity)
        Function DeleteItem(ByVal intItemID As Integer) As Integer
        Function GetItemByID(ByVal intItemID As Integer) As Entity.IMst_ItemEntity
        Function InsertItem(ByVal objItemEnt As Entity.IMst_ItemEntity) As Integer
        Function UpdateItem(ByVal objItemEnt As Entity.IMst_ItemEntity) As Integer
        Function CountUsedInPO(ByVal intItemID As Integer) As Integer
    End Interface
End Namespace

