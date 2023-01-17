Imports Service
Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS15
'	Class Discription	: Interface class Job Order Type service
'	Create User 		: Nisa S.
'	Create Date		    : 02-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region


Partial Class Master_KTMS15
    Inherits System.Web.UI.Page

    Private objJobTypeService As New ImpJobTypeService
    Private objLog As New Common.Logs.Log
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility
    Private Const strResult As String = "Result"
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private Const strDelete As String = "Result"
    Private Const strEdit As String = "ResultEdit"

#Region "Even"
    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Search Data
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ' call function search data
            SearchData(txtJobType.Text)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtJobType") = txtJobType.Text.Trim
            Session("btnSearch") = "search"
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashJobTypeID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_JobTypeCommand
    '	Discription	    : Event repeater JobType command
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_JobTypeCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intJobTypeID As Integer = CInt(hashJobTypeID(e.Item.ItemIndex).ToString())

            ' set JobTypeID to session
            Session("intJobTypeID") = intJobTypeID

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS15", strDelete, objMessage.GetXMLMessage("KTMS_15_001"))
                Case "Edit"
                    ' redirect to KTMS15
                    Response.Redirect("KTMS15.aspx?strEdit=True")
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_JobTypeCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_JobTypeDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_JobTypeDataBound( _
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

            ' Set JobTypeID to hashtable
            hashJobTypeID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_JobTypeDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS15 : Job Order Type Master")
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
    '	Create Date	    : 02-07-2013
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
                Session("ini") = "ini"
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' redirect to KTMS16 with Add mode
            Response.Redirect("KTMS16.aspx?Mode=Add")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region 'Even

#Region "Function"
    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
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

    ' Stores the hashJobTypeID keys in ViewState
    ReadOnly Property hashJobTypeID() As Hashtable
        Get
            If IsNothing(ViewState("hashJobTypeID")) Then
                ViewState("hashJobTypeID") = New Hashtable()
            End If
            Return CType(ViewState("hashJobTypeID"), Hashtable)
        End Get
    End Property

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Job Order Type data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData( _
        ByVal strJobType As String)

        Try
            ' table object keep value from Job Order service
            Dim dtInquiry As New DataTable

            ' call function GetJobTypeList from JobTypeService
            dtInquiry = objJobTypeService.GetJobTypeList(strJobType.ToString.Trim)
            ' set table object to session
            Session("dtInquiry") = dtInquiry
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
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
                SearchData(Session("txtJobType"))
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)
            End If

            ' set value to txtJobType from session
            txtJobType.Text = Session("txtJobType")
            ' call function check permission
            CheckPermission()

            ' check delete JobType
            If objUtility.GetQueryString(strDelete) = "True" Then
                DeleteJobType()
            End If

            ' check Edit item
            If Request.QueryString("strEdit") = "True" Then
                EditJobType()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: EditJobType
    '	Discription	    : EditJobType
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub EditJobType()
        Try
            '' check state of delete item
            'Dim boolInuse As Boolean = objJobTypeService.IsUsedInPO(Session("intJobTypeID"))
            '' check flag in_used
            'If boolInuse Then
            '    ' case in_used then alert message
            '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_15_006"))
            '    Exit Sub
            'End If

            Response.Redirect("KTMS16.aspx?Mode=Edit&id=" & Session("intJobTypeID"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("EditJobType", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtInquiry") = Nothing
            Session("txtJobType") = Nothing
            Session("intJobTypeID") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("PageNo") = Nothing
            Session("btnSearch") = Nothing
            Session("ini") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of JobType menu
            objAction = objPermission.CheckPermission(32)
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
    '	Function name	: DeleteJobType
    '	Discription	    : Delete JobType data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteJobType()
        Try
            ' check state of delete JobType
            Dim boolInuse As Boolean = objJobTypeService.IsUsedInPO(Session("intJobTypeID"))
            ' check flag in_used
            If boolInuse Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_15_002"))
                Exit Sub
            End If

            If objJobTypeService.DeleteJobType(Session("intJobTypeID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_15_003"))
                ' call function search new data
                SearchData(Session("txtJobType"))
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"), True)
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_15_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteJobType", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 02-07-2013
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
#End Region 'Function




End Class
