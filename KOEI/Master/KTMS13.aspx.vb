#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS13
'	Class Discription	: Interface class Payment Term service
'	Create User 		: Nisa S.
'	Create Date		    : 19-06-2013
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
Imports Entity
Imports Common.Utilities
Imports Exceptions
Imports System.Reflection
Imports Dto
Imports System.Data
Imports Service
Imports System.Web.Configuration

#End Region

Partial Class Master_KTMS13
    Inherits System.Web.UI.Page

    Private objPaymentTermService As New ImpPaymentTermService
    Private objLog As New Common.Logs.Log
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility
    Private Const strDelete As String = "Result"
    Private Const strEdit As String = "ResultEdit"
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission

#Region "Even"
    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Search Data
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ' call function search data
            SearchData(txtPayment.Text)
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtPayment") = txtPayment.Text.Trim
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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashPaymentTermID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInquery_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intPaymentTermID As Integer = CInt(hashPaymentTermID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intPaymentTermID") = intPaymentTermID

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTMS13", strDelete, objMessage.GetXMLMessage("KTMS_13_001"))
                Case "Edit"
                    ' redirect to KTMS13
                    Response.Redirect("KTMS13.aspx?strEdit=True")
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
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
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
            hashPaymentTermID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS13 : Payment Term Master")
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
    '	Create Date	    : 19-06-2013
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
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' redirect to KTMS14 with Add mode
            Response.Redirect("KTMS14.aspx?Mode=Add")
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
    '	Create Date	    : 19-06-2013
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

    ' Stores the hashPaymentTermID keys in ViewState
    ReadOnly Property hashPaymentTermID() As Hashtable
        Get
            If IsNothing(ViewState("hashPaymentTermID")) Then
                ViewState("hashPaymentTermID") = New Hashtable()
            End If
            Return CType(ViewState("hashPaymentTermID"), Hashtable)
        End Get
    End Property

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Payment Term data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData( _
        ByVal strPayment As String)

        Try
            ' table object keep value from item service
            Dim dtInquiry As New DataTable

            ' call function GetItemList from ItemService
            dtInquiry = objPaymentTermService.GetPaymentList(strPayment.ToString.Trim)
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
    '	Create Date	    : 19-06-2013
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
                SearchData(Session("txtPayment"))
                ' call function display page
                DisplayPage(Session("PageNo"), True)
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)
            End If

            ' set value to txtItemName from session
            txtPayment.Text = Session("txtPayment")
            ' call function check permission
            CheckPermission()

            ' check delete Payment
            If objUtility.GetQueryString(strDelete) = "True" Then
                DeletePaymentTerm()
            End If

            ' check Edit item
            If Request.QueryString("strEdit") = "True" Then
                EditPaymentTerm()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtInquiry") = Nothing
            Session("txtPayment") = Nothing
            'Session("intItemID") = Nothing
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
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(31)
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
    '	Function name	: DeletePaymentTerm
    '	Discription	    : Delete PaymentTerm data
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeletePaymentTerm()
        Try
            ' check state of delete PaymentTerm
            Dim boolInuse As Boolean = objPaymentTermService.IsUsedInPO(Session("intPaymentTermID"))
            ' check flag in_used
            If boolInuse Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_13_002"))
                Exit Sub
            End If

            If objPaymentTermService.DeletePaymentTerm(Session("intPaymentTermID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_13_003"))
                ' call function search new data
                SearchData(Session("txtPayment"))
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"), True)
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_13_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeletePaymentTerm", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: EditPaymentTerm
    '	Discription	    : Edit PaymentTerm
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub EditPaymentTerm()
        Try
            ' check state of delete item
            Dim boolInuse As Boolean = objPaymentTermService.IsUsedInPO(Session("intPaymentTermID"))
            ' check flag in_used
            If boolInuse Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_13_005"))
                Exit Sub
            End If

            Response.Redirect("KTMS14.aspx?Mode=Edit&id=" & Session("intPaymentTermID"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("EditPaymentTerm", ex.Message.ToString, Session("UserName"))
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

