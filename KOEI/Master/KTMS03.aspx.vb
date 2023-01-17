Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS03
'	Class Discription	: Webpage for item master
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

Partial Class Master_KTMS03
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objItemSer As New Service.ImpItemService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS03 : Item Master")
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
    '	Create Date	    : 03-06-2013
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
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            ' call function search data
            SearchData(txtItemName.Text, _
                       ddlVendor.SelectedValue)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("ddlVendor") = ddlVendor.SelectedValue
            Session("txtItemName") = txtItemName.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashItemID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intItemID As Integer = CInt(hashItemID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS03", strResult, objMessage.GetXMLMessage("KTMS_03_001"))
                Case "Edit"
                    ' redirect to KTMS04
                    Response.Redirect("KTMS04.aspx?Mode=Edit&id=" & intItemID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInquery.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            ' Set ItemID to hashtable
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "item_id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnAdd.Click
        Try
            ' redirect to KTMS04 with Add mode
            Response.Redirect("KTMS04.aspx?Mode=Add")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashItemID() As Hashtable
        Get
            If IsNothing(ViewState("hashItemID")) Then
                ViewState("hashItemID") = New Hashtable()
            End If
            Return CType(ViewState("hashItemID"), Hashtable)
        End Get
    End Property
#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            Dim strPageEnter As String = objUtility.GetQueryString("New")

            ' check case new enter
            If strPageEnter = "True" Then
                ' call function clear session
                ClearSession()
            ElseIf strPageEnter = "Update" Or strPageEnter = "Insert" Or strPageEnter = "Back" Then
                ' call function search new data
                SearchData(Session("txtItemName"), _
                           Session("ddlVendor"))
                ' call function display page
                DisplayPage("0", True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' call function set Vendor dropdownlist
            LoadListVendor()
            ' set value to txtItemName from session
            txtItemName.Text = Session("txtItemName")
            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteItem()
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
    '	Create Date	    : 03-06-2013
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

            ' set select Vendor from session
            If Not IsNothing(Session("ddlVendor")) And ddlVendor.Items.Count > 0 Then
                ddlVendor.SelectedValue = Session("ddlVendor")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(26)
            ' set permission Create
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData( _
        ByVal strItemName As String, _
        ByVal strVendorID As String)
        Try
            ' table object keep value from item service
            Dim dtInquiry As New DataTable

            ' call function GetItemList from ItemService
            dtInquiry = objItemSer.GetItemList(strItemName.ToString.Trim, _
                                               strVendorID.ToString.Trim)
            ' set table object to session
            Session("dtInquiry") = dtInquiry
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage( _
        ByVal intPageNo As Integer, _
        Optional ByVal boolNotAlertMsg As Boolean = False)
        Try
            Dim dtInquiry As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtInquiry = Session("dtInquiry")

            ' check record for display
            If Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInquiry)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtInquiry.Rows.Count)
                Session("PageNo") = intPageNo
            Else
                ' case not exist data
                ' show message box
                If Not (boolNotAlertMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDescription( _
        ByVal intPageNo As Integer, _
        ByVal intPageCount As Integer, _
        ByVal intAllRecs As Integer)
        Try
            ' variable page size get from web.config
            Dim intPageSize As Integer = CInt(WebConfigurationManager.AppSettings("PageSize"))
            Dim intStart As Integer
            Dim intEnd As Integer

            ' check page no
            If intPageNo = 0 Then
                intPageNo = 1
            End If

            ' set record start
            intStart = ((intPageNo - 1) * intPageSize) + 1

            ' set record end
            If intPageNo = intPageCount Then
                intEnd = intAllRecs
            Else
                intEnd = intPageNo * intPageSize
            End If

            ' set wording 
            lblDescription.Text = "Showing " & intStart.ToString & " to " & intEnd.ToString & _
            " of " & intAllRecs.ToString & " entries"
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ShowDescription", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteItem
    '	Discription	    : Delete item data
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteItem()
        Try
            ' check state of delete item
            Dim boolInuse As Boolean = objItemSer.IsUsedInPO(Session("intItemID"))
            ' check flag in_used
            If boolInuse Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_03_002"))
                Exit Sub
            End If

            If objItemSer.DeleteItem(Session("intItemID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_03_003"))
                ' call function search new data
                SearchData(Session("txtItemName"), _
                           Session("ddlVendor"))
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"), True)
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_03_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtInquiry") = Nothing
            Session("ddlVendor") = Nothing
            Session("txtItemName") = Nothing
            Session("intItemID") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("PageNo") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
