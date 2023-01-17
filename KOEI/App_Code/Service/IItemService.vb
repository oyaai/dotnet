#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IItemService
'	Class Discription	: Interface class item service
'	Create User 		: Komsan Luecha
'	Create Date		    : 04-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic
Imports System.Data

Namespace Service
    Public Interface IItemService
        Function GetItemListForDropdownList() As List(Of Dto.ItemDto)
        Function SetListItem(ByRef objValue As DropDownList) As Boolean
        Function GetItemList(ByVal strItemName As String, ByVal strVendorID As String) As DataTable
        Function DeleteItem(ByVal intItemID As Integer) As Boolean
        Function GetItemByID(ByVal intItemID As Integer) As Dto.ItemDto
        Function InsertItem(ByVal objItemDto As Dto.ItemDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateItem(ByVal objItemDto As Dto.ItemDto, Optional ByRef strMsg As String = "") As Boolean
        Function IsUsedInPO(ByVal intItemID As Integer) As Boolean
    End Interface
End Namespace

