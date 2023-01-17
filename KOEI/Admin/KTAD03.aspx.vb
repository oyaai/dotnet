#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Admin
'	Class Name		    : Admin_KTAD03
'	Class Discription	: Webpage for User_Permission
'	Create User 		: Wasan D.
'	Create Date		    : 05-07-2013
'
' UPDATE INFORMATION
'	Update User		: 
'	Update Date		: 
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Imports System.Data

Partial Class Admin_KTAD03
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objUserPermissionSer As New service.ImpUserPermissionService
    Private pagedData As New PagedDataSource
    Private Const strResult As String = "Result"
#Region "Properties"

    ' Stores the PayID keys in ViewState
    ReadOnly Property hashUserID() As Hashtable
        Get
            If IsNothing(ViewState("hashUserID")) Then
                ViewState("hashUserID") = New Hashtable()
            End If
            Return CType(ViewState("hashUserID"), Hashtable)
        End Get
    End Property

#End Region

#Region "Events"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAD03 : User Permission Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptPayCondition_DataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUserPermission_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUserPermission.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                ' Set textbox value to session
                SetTXTSession()
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            ' Set Payment Condition to hashtable
            hashUserID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptUserPermission_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUserPermission_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles rptUserPermission.DataBinding
        Try
            ' clear hashtable data
            hashUserID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptUserPermission_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptPayCondition_Command
    '	Discription	    : Event repeater Payment Condition command
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUserPermission_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptUserPermission.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intUserID As Integer = CInt(hashUserID(e.Item.ItemIndex).ToString())

            ' set Payment_Condition to session
            SetTXTSession()

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTAD03", strResult, objMessage.GetXMLMessage("KTAD_03_001"))
                    ' Set UserID to session
                    Session("intUserID") = intUserID
                Case "Edit"
                    ' redirect to KTMS26
                    Response.Redirect("KTAD04.aspx?Mode=Edit&ID=" & intUserID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptPayCondition_Command", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event click button Search
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            ' table object keep value from User Permission service
            Dim dtInquiry As New DataTable

            ' set text to session
            SetTXTSession()
            ' call function search data
            SearchData(txtFirstName.Text, txtLastName.Text, txtUserName.Text, ddlDepartment.SelectedValue)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Event button Add click
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' Set textbox value to Session
            SetTXTSession()
            ' redirect to KTMS26 with Add mode
            Response.Redirect("KTAD04.aspx?Mode=Add")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                initialpage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


#End Region

#Region "Functions"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub initialpage()
        Try
            Dim strPageEnter As String = objUtility.GetQueryString("New")

            ' check case new enter
            If strPageEnter = "True" Then
                ' call function clear session
                ClearSession()
            ElseIf strPageEnter = "Update" Or strPageEnter = "Insert" Then
                ' call function search new data
                SearchData(Session("txtFirstName"), Session("txtLastName"), Session("txtUserName"), Session("ddlDepartment"))
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)
            End If
            ' call function set Department dropdownlist
            LoadDepartmentList()
            ' set value to txtItemName from session
            txtFirstName.Text = Session("txtFirstName")
            txtLastName.Text = Session("txtLastName")
            txtUserName.Text = Session("txtUserName")
            ddlDepartment.SelectedValue = Session("ddlDepartment")

            '' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteFunction()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadDepartmentList
    '	Discription	    : Load data department to dropdownlist
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadDepartmentList()
        Try
            ' object Department service
            Dim objDepSer As New Service.ImpDepartmentService
            ' listDepDto for keep value from service
            Dim listDepDto As New List(Of Dto.DepartmentDto)
            ' call function GetDepartmentList from service
            listDepDto = objDepSer.GetDepartmentForDDList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlDepartment, listDepDto, "name", "id", True)

            ' set select Department from session
            If Not IsNothing(Session("ddlDepartment")) And ddlDepartment.Items.Count > 0 Then
                ddlDepartment.SelectedValue = Session("ddlDepartment")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadDepartmentList", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Payment Condition menu
            objAction = objPermission.CheckPermission(Enums.MenuId.UserPermission)
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
    '	Discription	    : Search Userpermission data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData(ByVal strFName As String, _
                           ByVal strLName As String, _
                           ByVal strUName As String, _
                           ByVal intDepartment As String)
        Try
            ' table object keep value from Payment Condition service
            Dim dtInquiry As New DataTable

            ' call function GetUserPermissionList from ImpUserPermissionService
            dtInquiry = objUserPermissionSer.GetUserPermissionList(strFName.ToString.Trim, strLName.ToString.Trim, strUName.ToString.Trim, intDepartment.ToString.Trim)
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
    '	Create User	    : Wasan D.
    '	Create Date	    : 03-07-2013
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
                rptUserPermission.DataSource = pagedData
                rptUserPermission.DataBind()
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
                rptUserPermission.DataSource = Nothing
                rptUserPermission.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("txtFirstName") = Nothing
            Session("txtLastName") = Nothing
            Session("txtUserName") = Nothing
            Session("ddlDepartment") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("PageNo") = Nothing
            Session("dtInquiry") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetTXTSession
    '	Discription	    : set session from Textbox
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetTXTSession()
        Try
            ' set search text to session
            Session("txtFirstName") = txtFirstName.Text.Trim
            Session("txtLastName") = txtLastName.Text.Trim
            Session("txtUserName") = txtUserName.Text.Trim
            Session("ddlDepartment") = ddlDepartment.SelectedValue
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetTXTSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeletePaymentCond
    '	Discription	    : Delete Payment Condition data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteFunction()
        Try

            If objUserPermissionSer.DeleteUserPermission(Session("intUserID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_03_002"))
                ' call function search new data
                SearchData(Session("txtFirstName"), _
                           Session("txtLastName"), _
                           Session("txtUserName"), _
                           Session("ddlDepartment"))
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"), True)
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_03_003"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteFunction", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
