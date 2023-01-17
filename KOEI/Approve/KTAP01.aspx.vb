Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
Imports System.Web.Services

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Purchase Approve
'	Class Name		    : Approve_KTAP01
'	Class Discription	: Webpage for Purchase Approve
'	Create User 		: Suwishaya L.
'	Create Date		    : 04-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Approve_KTAP01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objPurchaseSer As New Service.ImpPurchaseService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
    Private Const constApprove As String = "Approve"
    Private Const constDecline As String = "Decline"
    Private Const constDelete As String = "Delete"
    Private strEvent As String = ""
    Private itemConfirm As String = "" 

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAP01 : Purchase Approve")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
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
                'set event status
                strEvent = "Page_Load"
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click

        Try
            'set event status
            strEvent = "Search"

            'Check input Criteria data
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()

            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            SetDataToSession()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnApprove_Click
    '	Discription	    : Event btnApprove is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnApprove_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnApprove.Click
        Try
            'Keep data of each record that is already checked
            GetPurchaseID()
            'Check item is approve status
            If CheckApproveStatus() = False Then
                'Set enable/disable to button
                SetButton()
                Exit Sub
            End If
            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP01", constApprove, objMessage.GetXMLMessage("KTAP_01_003"))
            'Set enable/disable to button
            SetButton()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnApprove_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnDecline_Click
    '	Discription	    : Event btnDecline is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnDecline_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnDecline.Click
        Try
            'Keep data of each record that is already checked
            GetPurchaseID()
            'Check item is approve status
            If CheckApproveStatus() = False Then
                'Set enable/disable to button
                SetButton()
                Exit Sub
            End If
            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP01", constDecline, objMessage.GetXMLMessage("KTAP_01_004"))
            'Set enable/disable to button
            SetButton()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnDecline_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnDelete_Click
    '	Discription	    : Event btnDelete is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnDelete_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnDelete.Click
        Try
            'Keep data of each record that is already checked
            GetPurchaseID()

            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP01", constDelete, objMessage.GetXMLMessage("KTAP_01_005"))
            'Set enable/disable to button
            SetButton()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnDelete_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptApprove_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptApprove_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptApprove.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashStatusID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptApprove_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptApprove_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptApprove_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptApprove.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())

            ' set ID to session
            Session("intID") = intID

            Select Case e.CommandName
                Case "Detail"
                    'redirect to KTPU01_Detail
                    'objMessage.ShowPagePopup("../Purchase/KTPU01_Detail.aspx?id=" & intID, 900, 950, "", True)
                    Dim strPath = "../Purchase/KTPU01_Detail.aspx?id=" & intID.ToString()
                    'strPath = "showpopup('" & strPath & "','_blank');"
                    'ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", strPath, True)
                    Dim sb As New System.Text.StringBuilder()
                    sb.Append("var w = parseInt(screen.availWidth * 0.85);")
                    sb.Append("var h = parseInt(screen.availHeight * 0.75);")
                    sb.Append("var l = parseInt((screen.availWidth / 2) - (w / 2));")
                    sb.Append("var t = parseInt((screen.availHeight / 2) - (h / 2));")
                    sb.AppendFormat("popup = window.open('{0}','{1}'", strPath, "_blank")
                    sb.Append(",'width='+w+',height='+h+',left='+l+',top='+t+'")
                    sb.Append(",resizable,scrollbars=1');")
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", sb.ToString(), True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptApprove_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptApprove_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptApprove_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptApprove.ItemDataBound
        Try
            ' object link button   
            Dim chkApprove As HtmlInputCheckBox
            Dim lblStatus As New Label
            ' find linkbutton and assign to variable
            chkApprove = e.Item.FindControl("chkApprove")
            lblStatus = DirectCast(e.Item.FindControl("lblStatus"), Label)

            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashStatusID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "status_id"))
            Dim intStatusID As Integer = CInt(hashStatusID(e.Item.ItemIndex).ToString())

            'set check box disable for case delete,complete,approve,decline
            If intStatusID = 6 Or intStatusID = 7 Or intStatusID = 4 Or intStatusID = 5 Then
                chkApprove.Disabled = True
            End If

            'set font color of status
            Select Case intStatusID
                Case 3 '//Waiting
                    lblStatus.CssClass = "font_blue"
                Case 4 '//Approve 
                    lblStatus.CssClass = "font_green"
                Case 5 '//Decline 
                    lblStatus.CssClass = "font_red"
            End Select

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptApprove_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the id keys in ViewState
    ReadOnly Property hashID() As Hashtable
        Get
            If IsNothing(ViewState("hashID")) Then
                ViewState("hashID") = New Hashtable()
            End If
            Return CType(ViewState("hashID"), Hashtable)
        End Get
    End Property

    ' Stores the id keys in ViewState
    ReadOnly Property hashStatusID() As Hashtable
        Get
            If IsNothing(ViewState("hashStatusID")) Then
                ViewState("hashStatusID") = New Hashtable()
            End If
            Return CType(ViewState("hashStatusID"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
                SearchData()
            End If

            ' case not new enter then display page with page no
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            SetSessionToItem()

            'Set default button 
            btnApprove.Enabled = False
            btnDecline.Enabled = False
            btnDelete.Enabled = False 

            ' call function check permission
            CheckPermission()

            'Status for update : Approve = '4' ,Decline = '5',Delete = '6' 

            ' check approve item
            If objUtility.GetQueryString(constApprove) = "True" Then
                UpdatePurchase(4)
            End If

            ' check Decline item
            If objUtility.GetQueryString(constDecline) = "True" Then
                UpdatePurchase(5)
            End If

            ' check delete item
            If objUtility.GetQueryString(constDelete) = "True" Then
                UpdatePurchase(6)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check job order from > job order to
            If txtStartJobOrder.Text.Trim.Length > 0 And txtEndJobOrder.Text.Trim.Length > 0 Then
                If txtStartJobOrder.Text > txtEndJobOrder.Text Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_005"))
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date From
            If txtStartIssueDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtStartIssueDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date To
            If txtEndIssueDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtEndIssueDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check Issue Date From > Issue Date To
            If txtStartIssueDate.Text.Trim <> "" And txtEndIssueDate.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtStartIssueDate.Text.Trim, txtEndIssueDate.Text.Trim) = False Then 
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            CheckCriteriaInput = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCriteriaInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Purchase Approve data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtPurchaseApprove As New DataTable

            ' check case new enter
            If strEvent = "Page_Load" Then
                'Set data from condition search into Dto
                SetValueToDtoPageLoad()
            Else
                'Set data from condition search into Dto
                SetValueToDto()
            End If

            ' call function GetPurchaseApproveList from PurchaseService
            dtPurchaseApprove = objPurchaseSer.GetPurchaseApproveList(Session("objPurchaseApproveDto"))
            ' set table object to session
            Session("dtPurchaseApprove") = dtPurchaseApprove
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Purchase Approve dto object
            Dim objPurchaseDto As New Dto.PurchaseSearchDto
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtStartIssueDate.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtEndIssueDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objPurchaseDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If

                .po_type = rbtPurchaseType.SelectedValue
                .po_no_from = txtStartPONo.Text
                .po_no_to = txtEndPONo.Text
                .job_order_from = txtStartJobOrder.Text
                .job_order_to = txtEndJobOrder.Text
                .issue_date_start = issueDateDateFrom
                .issue_date_end = issueDateDateTo
                .vendor_name = txtVendorName.Text

                Dim list As String = ""
                'Del 2013/08/09
                'Dim i As Integer
                'For Each item As ListItem In chkApprovalStatus.Items
                '    If item.Selected Then
                '        If i = 0 Then
                '            'case select status is "Approve" , search data of "Approve" and "Completed"
                '            If item.Value = "4" Then
                '                list = "4,7"
                '            Else
                '                list = item.Value
                '            End If
                '        Else
                '            'case select status is "Approve" , search data of "Approve" and "Completed"
                '            If item.Value = "4" Then
                '                list = list & "," & "4,7"
                '            Else
                '                list = list & "," & item.Value
                '            End If
                '        End If
                '        i = i + 1
                '    End If
                'Next
                .status_ids = list

            End With

            ' set dto object to session
            Session("objPurchaseApproveDto") = objPurchaseDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDtoPageLoad
    '	Discription	    : Set value to Dto for onload
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDtoPageLoad()
        Try
            ' Purchase Approve dto object
            Dim objPurchaseDto As New Dto.PurchaseSearchDto
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtStartIssueDate.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtEndIssueDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objPurchaseDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If

                .po_type = rbtPurchaseType.SelectedValue
                .po_no_from = txtStartPONo.Text
                .po_no_to = txtEndPONo.Text
                .job_order_from = txtStartJobOrder.Text
                .job_order_to = txtEndJobOrder.Text
                .issue_date_start = issueDateDateFrom
                .issue_date_end = issueDateDateTo
                .vendor_name = txtVendorName.Text
                'Set default search status is waiting(3)
                .status_ids = "3"

            End With

            ' set dto object to session
            Session("objPurchaseApproveDto") = objPurchaseDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("rbtPurchaseType") = rbtPurchaseType.SelectedValue
            Session("txtStartPONo") = txtStartPONo.Text.Trim
            Session("txtEndPONo") = txtEndPONo.Text.Trim
            Session("txtStartJobOrder") = txtStartJobOrder.Text.Trim
            Session("txtEndJobOrder") = txtEndJobOrder.Text.Trim
            Session("txtStartIssueDate") = txtStartIssueDate.Text.Trim
            Session("txtEndIssueDate") = txtEndIssueDate.Text.Trim
            Session("txtVendorName") = txtVendorName.Text.Trim
            'Session("chkApprovalStatus") = chkApprovalStatus.SelectedValue
            'Del 2013/08/09
            'Dim list As String = "" 
            'For Each item As ListItem In chkApprovalStatus.Items
            '    If item.Selected Then
            '        If item.Text = "Waiting" Then
            '            Session("chkWaiting") = item.Selected
            '        ElseIf item.Text = "Approve" Then
            '            Session("chkApproval") = item.Selected
            '        ElseIf item.Text = "Decline" Then
            '            Session("chkDecline") = item.Selected
            '        End If
            '    End If
            'Next

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtPurchaseApprove As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtPurchaseApprove = Session("dtPurchaseApprove")

            ' check record for display
            If Not IsNothing(dtPurchaseApprove) AndAlso dtPurchaseApprove.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtPurchaseApprove)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptApprove.DataSource = pagedData
                rptApprove.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtPurchaseApprove.Rows.Count)
            Else
                ' case not exist data
                ' show message box     
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptApprove.DataSource = Nothing
                rptApprove.DataBind()
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page  
            Session("dtPurchaseApprove") = Nothing
            Session("rbtPurchaseType") = Nothing
            Session("txtStartPONo") = Nothing
            Session("txtEndPONo") = Nothing
            Session("txtStartJobOrder") = Nothing
            Session("txtEndJobOrder") = Nothing
            Session("txtStartIssueDate") = Nothing
            Session("txtEndIssueDate") = Nothing
            Session("txtVendorName") = Nothing
            Session("chkWaiting") = Nothing
            Session("chkApproval") = Nothing
            Session("chkDecline") = Nothing
            Session("intID") = Nothing
            Session("itemConfirm") = Nothing
            Session("actApprove") = Nothing
            Session("actButton") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetButton
    '	Discription	    : Set enable/disable buttton
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetButton()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            For Each item As RepeaterItem In rptApprove.Items
                Dim chkBox As HtmlInputCheckBox
                chkBox = item.FindControl("chkApprove")
                If chkBox.Checked = True Then
                    i = i + 1
                End If
            Next

            If Session("actApprove") = True Then
                Session("actButton") = False
            Else
                Session("actButton") = True
            End If
            'Set true/False into session
            If True = 0 Then
                btnApprove.Enabled = False
                btnDecline.Enabled = False
                btnDelete.Enabled = False
            Else
                btnApprove.Enabled = Session("actButton")
                btnDecline.Enabled = Session("actButton")
                btnDelete.Enabled = Session("actButton")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetButton", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSessionToItem
    '	Discription	    : Set data to item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set session to text search
            rbtPurchaseType.SelectedValue = Session("rbtPurchaseType")
            txtStartPONo.Text = Session("txtStartPONo")
            txtEndPONo.Text = Session("txtEndPONo")
            txtStartJobOrder.Text = Session("txtStartJobOrder")
            txtEndJobOrder.Text = Session("txtEndJobOrder")
            txtStartIssueDate.Text = Session("txtStartIssueDate")
            txtEndIssueDate.Text = Session("txtEndIssueDate")
            txtVendorName.Text = Session("txtVendorName")
            'Del 2013/08/09
            'Dim list As String = ""
            'For Each item As ListItem In chkApprovalStatus.Items
            '    If item.Text = "Waiting" Then
            '        item.Selected = Session("chkWaiting")
            '    ElseIf item.Text = "Approve" Then
            '        item.Selected = Session("chkApproval")
            '    ElseIf item.Text = "Decline" Then
            '        item.Selected = Session("chkDecline")
            '    End If
            'Next

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(18)
            ' set permission Create 
            btnSearch.Enabled = objAction.actList
            'case true : set session is "False" ,False: set session is "True" 
            If objAction.actApprove Then
                Session("actApprove") = False
            Else
                Session("actApprove") = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetPurchaseID
    '	Discription	    : Keep data of each record that is already checkedn
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPurchaseID()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            For Each item As RepeaterItem In rptApprove.Items
                Dim intID As Integer = CInt(hashID(i))
                Dim chkBox As HtmlInputCheckBox
                chkBox = item.FindControl("chkApprove")
                If chkBox.Checked = True Then
                    If i = 0 Then
                        itemConfirm = intID
                    Else
                        itemConfirm = itemConfirm & "," & intID
                    End If
                End If
                i = i + 1
            Next

            'Set itemConfirm into session
            Session("itemConfirm") = itemConfirm
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPurchaseID", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: UpdatePurchase
    '	Discription	    : Update status data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdatePurchase(ByVal intStatus As Integer)
        Try

            ' check state of delete item
            If objPurchaseSer.UpdatePurchaseStatus(Session("itemConfirm"), intStatus) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAP_01_001"))
                strEvent = "Page_Load"
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAP_01_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdatePurchase", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckApproveStatus
    '	Discription	    : Check Approve Status
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckApproveStatus() As Boolean
        Try
            CheckApproveStatus = False

            Dim strPONo As String = ""
            Dim strErrorMsg As String = ""
            Dim dtApproveStatus As New DataTable
            ' call function GetPOApprove from PurchaseService
            dtApproveStatus = objPurchaseSer.GetPOApprove(Session("itemConfirm"))

            If Not IsNothing(dtApproveStatus) AndAlso dtApproveStatus.Rows.Count > 0 Then
                For i As Integer = 0 To dtApproveStatus.Rows.Count - 1
                    If dtApproveStatus.Rows(i)("status_id").ToString = "7" Or dtApproveStatus.Rows(i)("status_id").ToString = "4" Then
                        If strPONo = "" Then
                            strPONo = dtApproveStatus.Rows(i)("po_no").ToString
                        Else
                            strPONo = strPONo & "," & dtApproveStatus.Rows(i)("po_no").ToString
                        End If
                    End If
                Next
            End If

            'Set message for show voucher no
            strErrorMsg = objMessage.GetXMLMessage("KTAP_01_006") & Space(1) & "(" & strPONo & " )"

            'Check PO no is approve
            If Not String.IsNullOrEmpty(strPONo) Then
                objMessage.AlertMessage(strErrorMsg)
                Exit Function
            End If

            CheckApproveStatus = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckApproveStatus", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

#End Region

#Region "WebMethod"
    '/**************************************************************
    '	Function name	: IsCheckPermission
    '	Discription	    : Check permission on button
    '	Return Value	: Boolean
    '	Create User	    : Suwishaya L
    '	Create Date	    : 05-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
   Public Shared Function IsCheckPermission( _
        ByVal blnItem As Boolean _
    ) As Boolean
        Dim objLog As New Common.Logs.Log
        Try

            Return HttpContext.Current.Session("actApprove")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("IsCheckPermission", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
#End Region

End Class