#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS04
'	Class Discription	: Webpage for maintenance item master
'	Create User 		: Komsan Luecha
'	Create Date		    : 03-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Master_KTMS04
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objItemSer As New Service.ImpItemService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS04 : Item Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                InitialPage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            Response.Redirect("KTMS03.aspx?New=Back")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            ' call function ClearControl
            ClearControl()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS04", strConfirmIns, objMessage.GetXMLMessage("KTMS_04_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS04", strConfirmUpd, objMessage.GetXMLMessage("KTMS_04_008"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' call function set Vendor dropdownlist
            LoadListVendor()

            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertItem()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateItem()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListVendor
    '	Discription	    : Load list Vendor function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListVendor()
        Try
            ' object Vendor service
            Dim objVendorSer As New Service.ImpVendorService
            ' listVendorDto for keep value from service
            Dim listVendorDto As New List(Of Dto.VendorDto)
            ' call function GetVendorForList from service
            listVendorDto = objVendorSer.GetVendorForList(0)

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlVendor, listVendorDto, "name", "id", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objItemDto As New Dto.ItemDto
            Dim intItemID As Integer = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intItemID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objItemDto = objItemSer.GetItemByID(intItemID)

            ' assign value to control
            With objItemDto
                txtItemID.Text = .id
                txtItemName.Text = .name
                ddlVendor.SelectedValue = .vendor_id
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtItemName.Text = String.Empty
            ddlVendor.SelectedValue = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objItemDto As New Dto.ItemDto

            ' assign value to dto object
            With objItemDto
                If String.IsNullOrEmpty(txtItemID.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtItemID.Text.Trim
                End If

                .name = txtItemName.Text.Trim
                .vendor_id = CInt(ddlVendor.SelectedValue)
            End With

            ' set dto object to session
            Session("objItemDto") = objItemDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objItemDto As New Dto.ItemDto
            ' set value to dto object from session
            objItemDto = Session("objItemDto")

            ' set value to control
            With objItemDto
                If .id = 0 Then
                    txtItemID.Text = String.Empty
                Else
                    txtItemID.Text = .id
                End If

                txtItemName.Text = .name
                ddlVendor.SelectedValue = .vendor_id
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertItem
    '	Discription	    : Insert Item
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertItem()
        Try
            ' call function set value to control
            SetValueToControl()

            ' call function InsertItem from service and alert message
            If objItemSer.InsertItem(Session("objItemDto"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_04_002"), Nothing, "KTMS03.aspx?New=Insert")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateItem
    '	Discription	    : Update Item
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateItem()
        Try
            ' call function set value to control
            SetValueToControl()

            ' call function UpdateItem from service and alert message
            If objItemSer.UpdateItem(Session("objItemDto"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_04_005"), Nothing, "KTMS03.aspx?New=Update")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
