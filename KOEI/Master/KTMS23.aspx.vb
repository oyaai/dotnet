Imports System.Data
Imports System.Web.Configuration

Partial Class Master_KTMS23
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objUtility As New Common.Utilities.Utility
    Private objDepartmentSer As New Service.ImpDepartmentService
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	: Event page initial
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 03-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS23 : Department Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	: Event page load
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 03-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    '	Discription	: Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs ) Handles btnSearch.Click
        Try
            ' call function search data
            SearchData(True)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtDepartment") = txtDepartment.Text.Trim
            Session("btnSearch") = "search"
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	: Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs ) Handles btnAdd.Click
        Try
            ' redirect to KTMS24 with Add mode
            Response.Redirect("KTMS24.aspx?Mode=Add")
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

    ' Stores the in_used keys in ViewState
    ReadOnly Property hashInUsed() As Hashtable
        Get
            If IsNothing(ViewState("hashInUsed")) Then
                ViewState("hashInUsed") = New Hashtable()
            End If
            Return CType(ViewState("hashInUsed"), Hashtable)
        End Get
    End Property
    '/**************************************************************
    '	Function name	: rptDepartment_DataBinding
    '	Discription	: Event repeater binding data
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub rptDepartment_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles rptDepartment.DataBinding
        Try
            ' clear hashtable data
            hashItemID.Clear()
            hashInUsed.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDepartment_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptDepartment_ItemCommand
    '	Discription	: Event repeater Department command
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub rptDepartment_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptDepartment.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intItemID As Integer = CInt(hashItemID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS23", strResult, objMessage.GetXMLMessage("KTMS_23_001"))

                Case "Edit"
                    ' redirect to KTMS24
                    Response.Redirect("KTMS23.aspx?strEdit=True")
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDepartment_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDepartment_ItemDataBound
    '	Discription	: Event repeater bound data
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Protected Sub rptDepartment_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptDepartment.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2"
                btnDel.Enabled = False
            End If

            ' Set ItemID to hashtable
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDepartment_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	: Clear value in Session
    '	Return Value	: nothing
    '	Create User	: Boonyarit
    '	Create Date	: 15-07-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("txtDepartment")
            Session.Remove("btnSearch")
            Session.Remove("intItemID")
            Session.Remove("actUpdate")
            Session.Remove("actDelete")
            Session.Remove("dtInquiry")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	: Initial page function
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub InitialPage()
        Try
            Dim strMode As String = String.Empty

            ' check case new enter
            strMode = objUtility.GetQueryString("New")
            If strMode = "True" Then
                '' call function clear session
                Call ClearSession()

                ' call function check permission
                CheckPermission()

            ElseIf strMode = "Back" Then
                ' call function check permission
                CheckPermission()

                If (Not Session("txtDepartment") Is Nothing) Then
                    ' set value to txtDepartment from session
                    txtDepartment.Text = Session("txtDepartment")

                    ' search data auto (Back Button)
                    SearchData()
                End If

                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))

            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteItem()
            End If

            ' check edit item
            If objUtility.GetQueryString("strEdit") = "True" Then
                EditItem()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	: Check permission
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(Enums.MenuId.Department)
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
    '	Discription	: Search Item data
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub SearchData(Optional ByVal CheckSearchNew As Boolean = False)
        Try
            ' table object keep value from item service
            Dim dtInquiry As New DataTable

            ' call function GetItemList from ItemService
            dtInquiry = objDepartmentSer.GetDepartmentList(txtDepartment.Text.Trim)

            ' show message box
            If CheckSearchNew = True AndAlso (IsNothing(dtInquiry) Or dtInquiry.Rows.Count = 0) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                Session.Remove("dtInquiry")
                Exit Sub
            End If

            ' set table object to session
            If (Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0) Then
                Session("dtInquiry") = dtInquiry
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	: Display page
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
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
                rptDepartment.DataSource = pagedData
                rptDepartment.DataBind()
                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtInquiry.Rows.Count)
            Else
                ' case not exist data

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptDepartment.DataSource = Nothing
                rptDepartment.DataBind()
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
    '	Discription	: Show description
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
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
    '	Discription	: Delete item data
    '	Return Value	: nothing
    '	Create User	: Charoon Morawichit
    '	Create Date	: 04-06-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub DeleteItem()
        Try
            Dim intItemID As Integer = 0
            intItemID = Session("intItemID")
            Dim boolInuse As Boolean = objDepartmentSer.CheckDepartmentForDel(intItemID)

            ' check flag in_used
            If boolInuse Then
                'case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_23_002"), "", "KTMS23.aspx?New=Back")
                Exit Sub
            End If

            ' check state of delete item
            If objDepartmentSer.DeleteDepartment(Session("intItemID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_23_003"), "", "KTMS23.aspx?New=Back")
                '' call function search new data
                'SearchData()
                '' call function display page
                'DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_23_004"), "", "KTMS23.aspx?New=Back")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: EditItem
    '	Discription	: Edit item data
    '	Return Value	: nothing
    '	Create User	: Wasan D.
    '	Create Date	: 05-09-2013
    '	Update User	:
    '	Update Date	:
    '*************************************************************/
    Private Sub EditItem()
        Try
            Dim intItemID As Integer = 0
            intItemID = Session("intItemID")
            Dim boolInuse As Boolean = objDepartmentSer.CheckDepartmentForDel(intItemID)

            ' check flag in_used
            If boolInuse Then
                'case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_23_005"), "", "KTMS23.aspx?New=Back")
            Else
                ' redirect to KTMS24
                Response.Redirect("KTMS24.aspx?Mode=Edit&id=" & intItemID)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


#End Region

End Class
