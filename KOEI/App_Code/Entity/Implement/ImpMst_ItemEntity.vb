#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_ItemEntity
'	Class Discription	: Class of table mst_item
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
    Public Class ImpMst_ItemEntity
        Implements IMst_ItemEntity

        Private _id As Integer
        Private _name As String
        Private _vendor_id As Integer
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As String
        Private _update_by As Integer
        Private _update_date As String

        Private objItem As New Dao.ImpMst_ItemDao
#Region "Function"
        Public Function GetListItem(Optional ByVal strVendorId As String = Nothing) As System.Collections.Generic.List(Of ImpMst_ItemEntity) Implements IMst_ItemEntity.GetListItem
            Return objItem.DB_GetListItem(strVendorId)
        End Function

        Public Function CheckItemByVendor(ByVal intVendor_id As Integer) As Boolean Implements IMst_ItemEntity.CheckItemByVendor
            Return objItem.DB_CheckItemByVendor(intVendor_id)
        End Function

        Public Function GetItemList(ByVal strItemName As String, ByVal strVendorID As String) As System.Collections.Generic.List(Of ImpMst_ItemDetailEntity) Implements IMst_ItemEntity.GetItemList
            Return objItem.GetItemList(strItemName, strVendorID)
        End Function

        Public Function DeleteItem(ByVal intItemID As Integer) As Integer Implements IMst_ItemEntity.DeleteItem
            Return objItem.DeleteItem(intItemID)
        End Function

        Public Function GetItemByID(ByVal intItemID As Integer) As IMst_ItemEntity Implements IMst_ItemEntity.GetItemByID
            Return objItem.GetItemByID(intItemID)
        End Function

        Public Function InsertItem(ByVal objItemEnt As IMst_ItemEntity) As Integer Implements IMst_ItemEntity.InsertItem
            Return objItem.InsertItem(objItemEnt)
        End Function

        Public Function UpdateItem(ByVal objItemEnt As IMst_ItemEntity) As Integer Implements IMst_ItemEntity.UpdateItem
            Return objItem.UpdateItem(objItemEnt)
        End Function

        Public Function CountUsedInPO(ByVal intItemID As Integer) As Integer Implements IMst_ItemEntity.CountUsedInPO
            Return objItem.CountUsedInPO(intItemID)
        End Function
#End Region

#Region "Property"
        Public Property created_by() As Integer Implements IMst_ItemEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As String Implements IMst_ItemEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_ItemEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_ItemEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property name() As String Implements IMst_ItemEntity.name
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property update_by() As Integer Implements IMst_ItemEntity.update_by
            Get
                Return _update_by
            End Get
            Set(ByVal value As Integer)
                _update_by = value
            End Set
        End Property

        Public Property update_date() As String Implements IMst_ItemEntity.update_date
            Get
                Return _update_date
            End Get
            Set(ByVal value As String)
                _update_date = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements IMst_ItemEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property
#End Region

        
    End Class
End Namespace

