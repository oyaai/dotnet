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
'	Package Name	    : Accounting Approve
'	Class Name		    : Approve_KTAP02
'	Class Discription	: Webpage for Accounting Approve
'	Create User 		: Suwishaya L.
'	Create Date		    : 08-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Approve_KTAP02
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objAccountingSer As New Service.ImpAccountingService
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
    Private accountType As String = ""

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAP02 : Acounting Approve")
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
    '	Create Date	    : 08-07-2013
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
    '	Create Date	    : 08-07-2013
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnApprove_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnApprove.Click
        Try
            'Keep data of each record that is already checked
            GetAccountID() 'Keep Id
            GetAccountType() 'Keep  Type

            'Check item is approve status
            If CheckApproveStatus() = False Then
                'Set enable/disable to button
                SetButton()
                Exit Sub
            End If
            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP02", constApprove, objMessage.GetXMLMessage("KTAP_02_003"))
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnDecline_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnDecline.Click
        Try
            'Keep data of each record that is already checked
            GetAccountID() 'Keep Id
            GetAccountType() 'Keep  Type

            'Check item is approve status
            If CheckApproveStatus() = False Then
                'Set enable/disable to button
                SetButton()
                Exit Sub
            End If
            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP02", constDecline, objMessage.GetXMLMessage("KTAP_02_004"))
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnDelete_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnDelete.Click
        Try
            'Keep data of each record that is already checked
            GetAccountID() 'Keep Id
            GetAccountType() 'Keep  Type

            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTAP02", constDelete, objMessage.GetXMLMessage("KTAP_02_005"))
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
    '	Create Date	    : 08-07-2013
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
            hashType.Clear()
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptApprove_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptApprove.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim strVoucherNo As String = hashID(e.Item.ItemIndex).ToString()
            Dim intType As Integer = CInt(hashType(e.Item.ItemIndex).ToString())
            ' set ID to session
            Session("intID") = strVoucherNo
            Session("type") = intType

            Select Case e.CommandName
                Case "Detail"
                    'redirect to KTAC01_Detail
                    objMessage.ShowPagePopup("KTAP02_Detail.aspx?voucherNo=" & strVoucherNo, 920, 600, "", True)
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
    '	Create Date	    : 08-07-2013
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
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "voucher_no"))
            hashStatusID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "status_id"))
            hashType.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "type"))
            Dim intStatusID As Integer = CInt(hashStatusID(e.Item.ItemIndex).ToString())

            'set check box disable for case complete and delete 
            If intStatusID = 6 Or intStatusID = 7 Then
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

    ' Stores the status id keys in ViewState
    ReadOnly Property hashStatusID() As Hashtable
        Get
            If IsNothing(ViewState("hashStatusID")) Then
                ViewState("hashStatusID") = New Hashtable()
            End If
            Return CType(ViewState("hashStatusID"), Hashtable)
        End Get
    End Property

    ' Stores the type keys in ViewState
    ReadOnly Property hashType() As Hashtable
        Get
            If IsNothing(ViewState("hashType")) Then
                ViewState("hashType") = New Hashtable()
            End If
            Return CType(ViewState("hashType"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
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
                UpdateAccount(4) 'Status approve is 4
            End If

            ' check Decline item
            If objUtility.GetQueryString(constDecline) = "True" Then
                UpdateAccount(5) 'Status Decline is 5
            End If

            ' check delete item
            If objUtility.GetQueryString(constDelete) = "True" Then
                UpdateAccount(6) 'Status delete is 6
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
    '	Create Date	    : 08-07-2013
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

            Dim strVoucherNo As String = ""
            Dim strErrorMsg As String = ""
            Dim dtApproveStatus As New DataTable
            ' call function GetAccountApprove from AcountService
            dtApproveStatus = objAccountingSer.GetAccountApprove(Session("itemConfirm"))

            If Not IsNothing(dtApproveStatus) AndAlso dtApproveStatus.Rows.Count > 0 Then
                For i As Integer = 0 To dtApproveStatus.Rows.Count - 1
                    If dtApproveStatus.Rows(i)("status_id").ToString = "4" Then
                        If strVoucherNo = "" Then
                            strVoucherNo = dtApproveStatus.Rows(i)("voucher_no").ToString
                        Else
                            strVoucherNo = strVoucherNo & "," & dtApproveStatus.Rows(i)("voucher_no").ToString
                        End If
                    End If
                Next
            End If

            'Set message for show voucher no
            strErrorMsg = objMessage.GetXMLMessage("KTAP_02_006") & Space(1) & "(" & strVoucherNo & " )"


            'Check Voucher No is approve
            If Not String.IsNullOrEmpty(strVoucherNo) Then
                objMessage.AlertMessage(strErrorMsg)
                Exit Function
            End If

            CheckApproveStatus = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckApproveStatus", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Account Approve data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtAccountApprove As New DataTable

            ' check case new enter
            If strEvent = "Page_Load" Then
                'Set data from condition search into Dto
                SetValueToDtoPageLoad()
            Else
                'Set data from condition search into Dto
                SetValueToDto()
            End If

            ' call function GetAcountApproveList from AcountService
            dtAccountApprove = objAccountingSer.GetAcountApproveList(Session("objAccountApproveDto"))
            ' set table object to session
            Session("dtAccountApprove") = dtAccountApprove
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Acounting Approve dto object
            Dim objAccountingDto As New Dto.AccountingDto
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtStartIssueDate.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtEndIssueDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objAccountingDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If

                If rbtType.SelectedValue = "1" Then
                    .strAccount_type = "1,3"
                ElseIf rbtType.SelectedValue = "2" Then
                    .strAccount_type = "2,4"
                Else
                    .strAccount_type = ""
                End If

                .strVendor_name = txtVendorName.Text
                .strJoborder_start = txtStartJobOrder.Text
                .strJoborder_end = txtEndJobOrder.Text
                .strAccount_startdate = issueDateDateFrom
                .strAccount_enddate = issueDateDateTo
                .strIe_start_code = txtStartIE.Text
                .strIe_end_code = txtEndIE.Text

                Dim list As String = ""
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
                .strStatus_id = list

            End With

            ' set dto object to session
            Session("objAccountApproveDto") = objAccountingDto

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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDtoPageLoad()
        Try
            ' Acounting Approve dto object
            Dim objAccountingDto As New Dto.AccountingDto
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtStartIssueDate.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtEndIssueDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objAccountingDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If

                .strAccount_type = rbtType.SelectedValue
                .strVendor_name = txtVendorName.Text
                .strJoborder_start = txtStartJobOrder.Text
                .strJoborder_end = txtEndJobOrder.Text
                .strAccount_startdate = issueDateDateFrom
                .strAccount_enddate = issueDateDateTo
                .strIe_start_code = txtStartIE.Text
                .strIe_end_code = txtEndIE.Text
                .strStatus_id = "3"

            End With

            ' set dto object to session
            Session("objAccountApproveDto") = objAccountingDto

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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("rbtType") = rbtType.SelectedValue
            Session("txtVendorName") = txtVendorName.Text.Trim
            Session("txtStartJobOrder") = txtStartJobOrder.Text.Trim
            Session("txtEndJobOrder") = txtEndJobOrder.Text.Trim
            Session("txtStartIssueDate") = txtStartIssueDate.Text.Trim
            Session("txtEndIssueDate") = txtEndIssueDate.Text.Trim
            Session("txtStartIE") = txtStartIE.Text.Trim
            Session("txtEndIE") = txtEndIE.Text.Trim
            'Session("chkApprovalStatus") = chkApprovalStatus.SelectedValue

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
            Dim dtAccountApprove As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtAccountApprove = Session("dtAccountApprove")

            ' check record for display
            If Not IsNothing(dtAccountApprove) AndAlso dtAccountApprove.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtAccountApprove)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptApprove.DataSource = pagedData
                rptApprove.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtAccountApprove.Rows.Count)
            Else
                ' case not exist data
                ' show message box     
                If strEvent <> "Page_Load" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page  
            Session("dtAccountApprove") = Nothing
            Session("rbtType") = Nothing
            Session("txtVendorName") = Nothing
            Session("txtStartJobOrder") = Nothing
            Session("txtEndJobOrder") = Nothing
            Session("txtStartIssueDate") = Nothing
            Session("txtEndIssueDate") = Nothing
            Session("txtStartIE") = Nothing
            Session("txtEndIE") = Nothing
            Session("chkApprovalStatus") = Nothing
            Session("chkWaiting") = Nothing
            Session("chkApproval") = Nothing
            Session("chkDecline") = Nothing
            Session("intID") = Nothing
            Session("intType") = Nothing
            Session("itemConfirm") = Nothing
            Session("accountType") = Nothing
            Session("actApprove") = Nothing 
            Session("actButton") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
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
            rbtType.SelectedValue = Session("rbtType")
            txtVendorName.Text = Session("txtVendorName")
            txtStartJobOrder.Text = Session("txtStartJobOrder")
            txtEndJobOrder.Text = Session("txtEndJobOrder")
            txtStartIssueDate.Text = Session("txtStartIssueDate")
            txtEndIssueDate.Text = Session("txtEndIssueDate")
            txtStartIE.Text = Session("txtStartIE")
            txtEndIE.Text = Session("txtEndIE")
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
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(19)
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
    '	Function name	: GetAccountID
    '	Discription	    : Keep data of each record that is already checkedn
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetAccountID()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            For Each item As RepeaterItem In rptApprove.Items
                Dim intID As String = hashID(i)
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
            objLog.ErrorLog("GetAccountID", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetAccountType
    '	Discription	    : Keep data of each record that is already checkedn
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetAccountType()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            For Each item As RepeaterItem In rptApprove.Items
                Dim intType As Integer = CInt(hashType(i))
                Dim chkBox As HtmlInputCheckBox
                chkBox = item.FindControl("chkApprove")
                If chkBox.Checked = True Then
                    If i = 0 Then
                        accountType = intType
                    Else
                        accountType = accountType & "," & intType
                    End If
                End If
                i = i + 1
            Next

            'Set accountType into session
            Session("accountType") = accountType
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetAccountType", ex.Message.ToString, Session("UserName"))
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
    '	Function name	: UpdateAccount
    '	Discription	    : Update status data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateAccount(ByVal intStatus As Integer)
        Try
            Dim dtValue As New DataTable
            dtValue = objAccountingSer.GetPoForDeleteList(Session("itemConfirm"))

            ' check state of delete item
            If objAccountingSer.UpdateAcountApprove(Session("itemConfirm"), Session("accountType"), intStatus, Nothing, "", dtValue) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAP_02_001"))
                strEvent = "Update"
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAP_02_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateAccount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "WebMethod"
    '/**************************************************************
    '	Function name	: IsCheckPermission
    '	Discription	    : Check permission on button
    '	Return Value	: Boolean
    '	Create User	    : Suwishaya L
    '	Create Date	    : 08-07-2013
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
