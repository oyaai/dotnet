Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Admin
'	Class Name		    : Admin_KTAD01
'	Class Discription	: Webpage for User Login Admin
'	Create User 		: Nisa S.
'	Create Date		    : 10-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Partial Class Admin_KTAD01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUserLoginSer As New Service.ImpUserLoginService
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility
    Private Const strResult As String = "Result"
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission


    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAD01 : User Login Admin")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
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
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try

            ' call function search data
            SearchData("", _
                        txtUserName.Text, _
                        txtFirstName.Text, _
                        txtLastName.Text, _
                        ddlDepartment.SelectedValue)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtUserName") = txtUserName.Text.Trim
            Session("txtFirstName") = txtFirstName.Text.Trim
            Session("txtLastName") = txtLastName.Text.Trim
            Session("ddlDepartment") = ddlDepartment.SelectedValue
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search User Login data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData( _
        ByVal strID As String, _
        ByVal strUserName As String, _
        ByVal strFirstName As String, _
        ByVal strLastName As String, _
        ByVal strDepartment As String)
        Try
            ' table object keep value from UserLogin service
            Dim dtInquiry As New DataTable

            ' call function GetUserLoginList from UserLoginService
            dtInquiry = objUserLoginSer.GetUserLoginList(strID.ToString.Trim, _
                                                        strUserName.ToString.Trim, _
                                                        strFirstName.ToString.Trim, _
                                                        strLastName.ToString.Trim, _
                                                        strDepartment.ToString.Trim)
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
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
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
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashUserLoginID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_UserLoginCommand
    '	Discription	    : Event repeater UserLogin command
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_UserLoginCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intUserLoginID As String = CInt(hashUserLoginID(e.Item.ItemIndex).ToString())

            ' set UserLoginID to session
            Session("intUserLoginID") = intUserLoginID

            Select Case e.CommandName
                Case "View"
                    Call LinkDetails(intUserLoginID)
                Case "Del"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTAD01", strResult, objMessage.GetXMLMessage("KTAD_01_001"))
                Case "Edit"
                    ' redirect to KTAD02
                    Response.Redirect("KTAD02.aspx?Mode=Edit&id=" & intUserLoginID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_UserLoginCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_UserLoginDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_IEDataBound( _
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

            ' Set UserLoginID to hashtable
            hashUserLoginID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_UserLoginDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the UserLogin_ID keys in ViewState
    ReadOnly Property hashUserLoginID() As Hashtable
        Get
            If IsNothing(ViewState("hashUserLoginID")) Then
                ViewState("hashUserLoginID") = New Hashtable()
            End If
            Return CType(ViewState("hashUserLoginID"), Hashtable)
        End Get
    End Property

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
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
            ElseIf strPageEnter = "Update" Or strPageEnter = "Insert" Then
                ' call function search new data
                SearchData("", _
                            Session("txtUserName"), _
                           Session("txtFirstName"), _
                           Session("txtLastName"), _
                           Session("ddlDepartment"))
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)
            End If

            ' call function set Department dropdownlist
            LoadListDepartment()

            ' set value to textbox from session
            txtUserName.Text = Session("txtUserName")
            ddlDepartment.SelectedValue = Session("ddlDepartment")
            txtFirstName.Text = Session("txtFirstName")
            txtLastName.Text = Session("txtLastName")
            ' call function check permission
            CheckPermission()

            ' check delete UserLogin
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteUserLogin()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of user login menu
            objAction = objPermission.CheckPermission(37)
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
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("ddlDepartment") = Nothing
            Session("txtUserName") = Nothing
            Session("txtFirstName") = Nothing
            Session("txtLastName") = Nothing
            Session("intUserLoginID") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("PageNo") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListDepartment
    '	Discription	    : Load list Department function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListDepartment()
        Try
            ' object Category service
            Dim objDepartmentSer As New Service.ImpUserLoginService
            ' listIECategoryDto for keep value from service
            Dim listDepartmentDto As New List(Of Dto.UserLoginDto)
            ' call function GetAll from service
            listDepartmentDto = objDepartmentSer.GetDepartmentForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlDepartment, listDepartmentDto, "name", "id", True)

            ' set select Category from session
            If Not IsNothing(Session("ddlDepartment")) And ddlDepartment.Items.Count > 0 Then
                ddlDepartment.SelectedValue = Session("ddlDepartment")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListDepartment", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
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
    '	Function name	: DeleteUserLogin
    '	Discription	    : Delete UserLogin data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteUserLogin()
        Try
            ' check state of delete Staff
            Dim boolInuse As Boolean = objUserLoginSer.IsUsedInPO(Session("intUserLoginID"))
            ' check flag in_used
            If boolInuse Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_01_004"))
                Exit Sub
            End If

            If objUserLoginSer.DeleteUserLogin(Session("intUserLoginID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_01_002"))
                ' call function search new data
                SearchData("", _
                            Session("txtUserName"), _
                           Session("txtFirstName"), _
                           Session("txtLastName"), _
                           Session("ddlDepartment"))
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"), True)
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_01_003"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteUserLogin", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ''/**************************************************************
    ''	Function name	: EditUserLogin
    ''	Discription	    : Edit UserLogin
    ''	Return Value	: nothing
    ''	Create User	    : Nisa S.
    ''	Create Date	    : 10-07-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Private Sub EditUserLogin()
    '    Try
    '        ' check state of delete item
    '        Dim boolInuse As Boolean = objUserLoginSer.IsUsedInPO(Session("intUserLoginID"))
    '        ' check flag in_used
    '        If boolInuse Then
    '            ' case in_used then alert message
    '            objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_01_005"))
    '            Exit Sub
    '        End If

    '        Response.Redirect("KTAD02.aspx?Mode=Edit&id=" & Session("intUserLoginID"))
    '    Catch ex As Exception
    '        ' write error log
    '        objLog.ErrorLog("EditVat", ex.Message.ToString, Session("UserName"))
    '    End Try
    'End Sub
        

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnAdd.Click
        Try
            ' redirect to KTAD02 with Add mode
            Response.Redirect("KTAD02.aspx?Mode=Add")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkDetails
    '	Discription	    : Set Link button details
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDetails(ByVal intUserLoginID As Integer)
        Try
            Dim objComm As New Common.Utilities.Message
            Dim strPage As String = "KTAD01_Detail.aspx?ID=" & intUserLoginID.ToString

            objComm.ShowPagePopup(strPage, 500, 500)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDetails", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub


End Class
