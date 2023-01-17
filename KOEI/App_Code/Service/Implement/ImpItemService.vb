#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpItemService
'	Class Discription	: Implement item Service
'	Create User 		: Komsan L.
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
Imports MySql.Data.MySqlClient

Namespace Service
    Public Class ImpItemService
        Implements IItemService

        Private objLog As New Common.Logs.Log
        Private objItemEnt As New Entity.ImpMst_ItemEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: GetItemList
        '	Discription	    : Get item list
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetItemList( _
            ByVal strItemName As String, _
            ByVal strVendorID As String _
        ) As System.Data.DataTable Implements IItemService.GetItemList
            ' set default
            GetItemList = New DataTable
            Try
                ' variable for keep list from item entity
                Dim listItemEnt As New List(Of Entity.ImpMst_ItemDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listItemEnt = objItemEnt.GetItemList(strItemName, strVendorID)

                ' assign column header
                With GetItemList
                    .Columns.Add("item_id")
                    .Columns.Add("item_name")
                    .Columns.Add("vendor_id")
                    .Columns.Add("vendor_name")
                    .Columns.Add("in_used")

                    ' assign row from listItemEny
                    For Each values In listItemEnt
                        row = .NewRow
                        row("item_id") = values.id
                        row("item_name") = values.name
                        row("vendor_id") = values.vendor_id
                        row("vendor_name") = values.vendor_name
                        row("in_used") = values.inuse
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetItemList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteItem
        '	Discription	    : Delete item
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteItem( _
            ByVal intItemID As Integer _
        ) As Boolean Implements IItemService.DeleteItem
            ' set default return value
            DeleteItem = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteItem from Item Entity
                intEff = objItemEnt.DeleteItem(intItemID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteItem = True
                Else
                    ' case row less than 1 then return False
                    DeleteItem = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteItem(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetItemByID
        '	Discription	    : Get item by ID
        '	Return Value	: Item dto object
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetItemByID( _
           ByVal intItemID As Integer _
       ) As Dto.ItemDto Implements IItemService.GetItemByID
            ' set default return value
            GetItemByID = New Dto.ItemDto
            Try
                ' object for return value from Entity
                Dim objItemEntRet As New Entity.ImpMst_ItemEntity
                ' call function GetItemByID from Entity
                objItemEntRet = objItemEnt.GetItemByID(intItemID)

                ' assign value from Entity to Dto
                With GetItemByID
                    .id = objItemEntRet.id
                    .name = objItemEntRet.name
                    .vendor_id = objItemEntRet.vendor_id
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetItemByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertItem
        '	Discription	    : Insert item
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertItem( _
            ByVal objItemDto As Dto.ItemDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IItemService.InsertItem
            ' set default return value
            InsertItem = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertItem from Item Entity
                intEff = objItemEnt.InsertItem(SetDtoToEntity(objItemDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertItem = True
                Else
                    ' case row less than 1 then return False
                    InsertItem = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_04_004"
                Else
                    ' other case
                    strMsg = "KTMS_04_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertItem(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateItem
        '	Discription	    : Update item
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateItem( _
            ByVal objItemDto As Dto.ItemDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IItemService.UpdateItem
            ' set default return value
            UpdateItem = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateItem from Item Entity
                intEff = objItemEnt.UpdateItem(SetDtoToEntity(objItemDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateItem = True
                Else
                    ' case row less than 1 then return False
                    UpdateItem = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_04_007"
                Else
                    ' other case
                    strMsg = "KTMS_04_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateItem(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objItemDto As Dto.ItemDto _
        ) As Entity.IMst_ItemEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_ItemEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objItemDto.id
                    .name = objItemDto.name
                    .vendor_id = objItemDto.vendor_id
                    .delete_fg = objItemDto.delete_fg
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check item in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intItemID As Integer _
        ) As Boolean Implements IItemService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objItemEnt.CountUsedInPO(intItemID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInPO = True
                Else
                    ' case equal 0 then return False
                    IsUsedInPO = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListItem
        '	Discription	    : Set list item to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListItem(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements IItemService.SetListItem
            Try
                ' variable
                Dim objItem As New Entity.ImpMst_ItemEntity
                Dim objListItem As List(Of Entity.ImpMst_ItemEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListItem = False
                ' get data list job_order
                objListItem = objItem.GetListItem
                If objListItem.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListItem, "name", "id")
                If objValue.Items.Count > 0 Then SetListItem = True

            Catch ex As Exception
                ' Write error log
                SetListItem = False
                objLog.ErrorLog("SetListItem", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetItemListForDropdownList
        '	Discription	    : Get data item for DropdownList
        '	Return Value	: List of Dto.ItemDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetItemListForDropdownList() As System.Collections.Generic.List(Of Dto.ItemDto) Implements IItemService.GetItemListForDropdownList
            Try
                Dim objItem As New Entity.ImpMst_ItemEntity
                Dim objListItem As List(Of Entity.ImpMst_ItemEntity)
                Dim objItemDto As Dto.ItemDto

                GetItemListForDropdownList = Nothing
                ' get data list Item
                objListItem = objItem.GetListItem
                If objListItem.Count < 1 Then Exit Function
                GetItemListForDropdownList = New List(Of Dto.ItemDto)
                For Each objItem In objListItem
                    objItemDto = New Dto.ItemDto
                    objItemDto.id = objItem.id
                    objItemDto.name = objItem.name
                    GetItemListForDropdownList.Add(objItemDto)
                Next

            Catch ex As Exception
                ' Write error log
                GetItemListForDropdownList = Nothing
                objLog.ErrorLog("GetItemListForDropdownList", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

        
    End Class
End Namespace

