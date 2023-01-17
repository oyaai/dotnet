Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization


#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Invoice Management
'	Class Name		    : KTPU04
'	Class Discription	: Searching data of Invoice
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 26-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTPU04
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private objValidate As New Common.Validations.Validation

    'connect with service
    Private objInvPurchaseService As New Service.ImpInvoice_PurchaseService

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : ini load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            objLog.StartLog("KTPU04", Session("UserName"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
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
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("rblSearchType") = rblSearchType.Text.Trim
            Session("txtPOFrom") = txtPOFrom.Text.Trim
            Session("txtPOTo") = txtPOTo.Text.Trim
            Session("txtIssueDateFrom") = txtIssueDateFrom.Text.Trim
            Session("txtIssueDateTo") = txtIssueDateTo.Text.Trim
            Session("txtDeliveryDateFrom") = txtDeliveryDateFrom.Text.Trim
            Session("txtDeliveryDateTo") = txtDeliveryDateTo.Text.Trim
            Session("txtVendor_Name") = txtVendor_Name.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
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
    '	Function name	: rptInquery_Invoice_PurchaseDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_Invoice_PurchaseDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInquery.ItemDataBound
        Try
            ' object link button
            Dim btnDetail As New LinkButton
            Dim chkBox As New CheckBox
            ' find linkbutton and assign to variable
            btnDetail = DirectCast(e.Item.FindControl("btnDetail"), LinkButton)

            'Set id to hashtable (for case link to detail page)
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Invoice_PurchaseDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_InvPurchaseCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_InvPurchaseCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intItemID As Integer = CInt(hashItemID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Edit"
                    ' redirect to KTPU04_Delivery
                    Response.Redirect("KTPU04_Delivery.aspx?Mode=Add&id=" & intItemID)
                Case "Detail"
                    'redirect to KTAC01_Detail
                    'objMessage.ShowPagePopup("KTPU04_Detail.aspx?id=" & intItemID, 1000, 990, "_blank", True)
                    Dim strPage As String = "KTPU04_Detail.aspx?id=" & intItemID.ToString()
                    'strPage = "javascript:showpopup('" & strPage & "','_blank');"
                    'ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", strPage, True)
                    Dim sb As New System.Text.StringBuilder()
                    sb.Append("var w = parseInt(screen.availWidth * 0.70);")
                    sb.Append("var h = parseInt(screen.availHeight * 0.60);")
                    sb.Append("var l = parseInt((screen.availWidth / 2) - (w / 2));")
                    sb.Append("var t = parseInt((screen.availHeight / 2) - (h / 2));")
                    sb.AppendFormat("popup = window.open('{0}','{1}'", strPage, "_blank")
                    sb.Append(",'width='+w+',height='+h+',left='+l+',top='+t+'")
                    sb.Append(",resizable,scrollbars=1');")
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", sb.ToString(), True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Invoice_PurchaseCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : back to main screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            Response.Redirect("KTPU03.aspx?New=True")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
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
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0
            Else
                ' check case new enter
                If objUtility.GetQueryString("Mode") = "Add" Then
                    ' call function clear session
                    ClearSession()
                    'set default of Type of Purchase
                    rblSearchType.SelectedIndex = 0
                Else
                    ' set value to from session to textbox 
                    rblSearchType.Text = Session("rblSearchType")
                    txtPOFrom.Text = Session("txtPOFrom")
                    txtPOTo.Text = Session("txtPOTo")
                    txtIssueDateFrom.Text = Session("txtIssueDateFrom")
                    txtIssueDateTo.Text = Session("txtIssueDateTo")
                    txtDeliveryDateFrom.Text = Session("txtDeliveryDateFrom")
                    txtDeliveryDateTo.Text = Session("txtDeliveryDateTo")
                    txtVendor_Name.Text = Session("txtVendor_Name")

                    SearchData()
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                End If
            End If

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtInvManage As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtInvManage = objInvPurchaseService.GetPO_List(Session("objInvManageDto"))
            ' set table object to session
            Session("dtInvManage") = dtInvManage
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' InvPurchase dto object
            Dim objInvManageDto As New Dto.Invoice_PurchaseDto
            Dim startDeliveryDate As String = ""
            Dim endDeliveryDate As String = ""
            Dim arrDeliveryStartDate() As String = Split(txtDeliveryDateFrom.Text.Trim(), "/")
            Dim arrDeliveryEndDate() As String = Split(txtDeliveryDateTo.Text.Trim(), "/")

            Dim startIssueDate As String = ""
            Dim endIssueDate As String = ""
            Dim arrIssueStartDate() As String = Split(txtIssueDateFrom.Text.Trim(), "/")
            Dim arrIssueEndDate() As String = Split(txtIssueDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objInvManageDto
                If UBound(arrDeliveryStartDate) > 0 Then
                    startDeliveryDate = arrDeliveryStartDate(2) & arrDeliveryStartDate(1) & arrDeliveryStartDate(0)
                End If
                If UBound(arrDeliveryEndDate) > 0 Then
                    endDeliveryDate = arrDeliveryEndDate(2) & arrDeliveryEndDate(1) & arrDeliveryEndDate(0)
                End If

                If UBound(arrIssueStartDate) > 0 Then
                    startIssueDate = arrIssueStartDate(2) & arrIssueStartDate(1) & arrIssueStartDate(0)
                End If
                If UBound(arrIssueEndDate) > 0 Then
                    endIssueDate = arrIssueEndDate(2) & arrIssueEndDate(1) & arrIssueEndDate(0)
                End If

                .strSearchType = rblSearchType.SelectedValue.ToString
                .strPOFrom = txtPOFrom.Text.Trim
                .strPOTo = txtPOTo.Text.Trim
                .strDeliveryDateFrom = startDeliveryDate
                .strDeliveryDateTo = endDeliveryDate
                .strIssueDateFrom = startIssueDate
                .strIssueDateTo = endIssueDate
                .strVendor_name = txtVendor_Name.Text.Trim()
            End With

            ' set dto object to session
            Session("objInvManageDto") = objInvManageDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtInvManage As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtInvManage = Session("dtInvManage")

            ' check record for display
            If Not IsNothing(dtInvManage) AndAlso dtInvManage.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInvManage)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtInvManage.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

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
    ' Function name : IsDate
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 26-05-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function CheckError() As Boolean
        CheckError = False
        Try
            'check start date
            If txtDeliveryDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtDeliveryDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtDeliveryDateTo.Text.Trim <> "" And txtDeliveryDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateFrom.Text.Trim) > objValidate.IsDate(txtDeliveryDateTo.Text.Trim) Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            'check start date
            If txtIssueDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtIssueDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtIssueDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtIssueDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtIssueDateTo.Text.Trim <> "" And txtIssueDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtIssueDateFrom.Text.Trim) > objValidate.IsDate(txtIssueDateTo.Text.Trim) Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If


            CheckError = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckError", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(8)

            ' set permission 
            btnBack.Enabled = True
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actList") = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
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
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtInvManage") = Nothing
            Session("rblSearchType") = Nothing
            Session("txtPOFrom") = Nothing
            Session("txtPOTo") = Nothing
            Session("txtDeliveryDateFrom") = Nothing
            Session("txtDeliveryDateTo") = Nothing
            Session("txtIssueDateFrom") = Nothing
            Session("txtIssueDateTo") = Nothing
            Session("txtVendor_Name") = Nothing
            Session("actList") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
   
#End Region
End Class
