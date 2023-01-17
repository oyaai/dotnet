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
'	Package Name	    : Invoice Purchase
'	Class Name		    : KTPU03
'	Class Discription	: Searching data of Invoice Purchase
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 20-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTPU03
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const constDelete As String = "Delete"
    Private Const constConfirm As String = "Confirm"
    Private itemConfirm As String = ""
    Private objValidate As New Common.Validations.Validation
    Private strEvent As String = ""
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
            objLog.StartLog("KTPU03", Session("UserName"))
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
    '	Create Date	    : 07-06-2013
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
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            strEvent = "Search"
            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            'set search text to session
            SetScreenToSession()

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
            hashPO_Header_id.Clear()
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_Invoice_PurchaseDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInquery.ItemDataBound
        Try
            ' object link button
            Dim chkBox As New CheckBox
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            'find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            chkBox = DirectCast(e.Item.FindControl("chkConfirm"), CheckBox)

            'set enable checkbox
            If DataBinder.Eval(e.Item.DataItem, "canConfirm") = "0" Then
                chkBox.Enabled = False
            Else
                chkBox.Enabled = True
            End If

            'set enable Edit,Delete button only case status_id = 5 (Decline) or 1 (Normal)
            If DataBinder.Eval(e.Item.DataItem, "status_id") = "5" Or _
                    (DataBinder.Eval(e.Item.DataItem, "status_id") = "1" And DataBinder.Eval(e.Item.DataItem, "canConfirm") = "1") Then
                btnEdit.Enabled = True
                btnDel.Enabled = True
            Else
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnDel.CssClass = "icon_del2 icon_center15"
                btnEdit.Enabled = False
                btnDel.Enabled = False
            End If

            'Set id to hashtable (for case link to detail page)
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashPO_Header_id.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "po_header_id"))
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
    '	Create Date	    : 09-06-2013
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
            Dim po_header_id As Integer = CInt(hashPO_Header_id(e.Item.ItemIndex).ToString())
            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Edit"
                    ' redirect to KTMS04
                    Response.Redirect("KTPU04_Delivery.aspx?Mode=Edit&id=" & intItemID & "&po_header_id=" & po_header_id)
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTPU03", constDelete, objMessage.GetXMLMessage("KTPU_03_001"))
                Case "Detail"
                    'redirect to KTAC01_Detail
                    'objMessage.ShowPagePopup("KTPU03_Detail.aspx?id=" & intItemID, 990, 890)
                    Dim strPage As String = "KTPU03_Detail.aspx?id=" & intItemID.ToString()
                    Dim sb As New System.Text.StringBuilder()
                    sb.Append("var w = parseInt(screen.availWidth * 0.70);")
                    sb.Append("var h = parseInt(screen.availHeight * 0.60);")
                    sb.Append("var l = parseInt((screen.availWidth / 2) - (w / 2));")
                    sb.Append("var t = parseInt((screen.availHeight / 2) - (h / 2));")
                    sb.AppendFormat("popup = window.open('{0}','{1}'", strPage, "_blank")
                    sb.Append(",'width='+w+',height='+h+',left='+l+',top='+t+'")
                    sb.Append(",resizable,scrollbars=1');")
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", sb.ToString(), True)
                    'strPage = "javascript:showpopup('" & strPage & "','_blank');"
                    'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowDetail", strPage, True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Invoice_PurchaseCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ShowConfirmInPanel
    '	Discription		: Show confirm Message in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowConfirmInPanel(ByVal strPageName As String, _
            ByVal strResultName As String, _
            ByVal strMessage As String, _
            Optional ByVal strMsgCode As String = "")

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler

            Dim strMsgValue As String = String.Empty
            If strMsgCode.Trim = String.Empty Then
                strMsgValue = strMessage
            Else
                strMsgValue = objMessage.GetXMLMessage(strMsgCode)
            End If

            Dim sb As New System.Text.StringBuilder()
            sb.Append("if (confirm('")
            sb.Append(strMsgValue)
            sb.Append("')){ ")
            If strPageName.IndexOf(".aspx") = -1 Then
                strPageName &= ".aspx"
            End If
            sb.Append("window.location = '" & strPageName & "?" & strResultName & "=True';")
            sb.Append("}")
            sb.Append("else{")
            sb.Append("};")

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            objLog.ErrorLog("ShowConfirmInPanel", ex.Message.ToString)
        End Try

    End Sub
    '/**************************************************************
    '	Function name	: ShowMsgInPanel
    '	Discription		: Show message in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowMsgInPanel(ByVal strMessage As String, _
                               Optional ByVal strMsgCode As String = "", _
                               Optional ByVal strURL As String = "")

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler

            Dim strMsgValue As String = String.Empty
            If strMsgCode.Trim = String.Empty Then
                strMsgValue = strMessage
            Else
                strMsgValue = objMessage.GetXMLMessage(strMsgCode)
            End If

            Dim sb As New System.Text.StringBuilder()
            sb.Append("alert('")
            sb.Append(strMsgValue)
            sb.Append("'); ")
            If strURL <> String.Empty Then
                sb.Append("window.location = '" & strURL & "'; ")
            End If

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            objLog.ErrorLog("ShowMsgInPanel", ex.Message.ToString)
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: btnConfirmPayment_Click
    '	Discription	    : Confirm payment
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnConfirmPayment_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnConfirmPayment.Click
        Try
            'Keep data of each record that is already checked
            GetConfirmID()

            itemConfirm = Session("itemConfirm")

            If itemConfirm = "" Then
                'confirm failed
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_006"))
                Exit Sub
            End If

            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTPU03", constConfirm, objMessage.GetXMLMessage("KTPU_03_005"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnConfirmPayment_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Open add screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' redirect to KTMS20 with Add mode
            Response.Redirect("KTPU04.aspx?Mode=Add")
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
    ReadOnly Property hashPO_Header_id() As Hashtable
        Get
            If IsNothing(ViewState("hashPO_Header_id")) Then
                ViewState("hashPO_Header_id") = New Hashtable()
            End If
            Return CType(ViewState("hashPO_Header_id"), Hashtable)
        End Get
    End Property
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0
                SearchData()
            End If

            DisplayPage(Request.QueryString("PageNo"))
            'set value to from session to textbox 
            SetSessionToScreen()

            ' call function check permission
            CheckPermission()

            '2013/09/26 Pranitda S. Start-Add
            'check task -----------
            Dim objTaskService As New Service.ImpTaskService
            Dim intTask As Integer = 1

            'Check task process by user_id (Boon add 13/08/2013)
            'If btnAdd.Enabled Then
            '    intTask = objTaskService.CheckTaskProcess(CInt(Session("UserID")))
            '    If intTask > 0 Then
            '        btnAdd.Enabled = False
            '    Else
            '        btnAdd.Enabled = True
            '    End If
            'End If
            '2013/09/26 Pranitda S. End-Add

            ' check delete item
            If objUtility.GetQueryString(constDelete) = "True" Then
                DeleteItem()
            End If

            'case post back from Confirm button
            If objUtility.GetQueryString(constConfirm) = "True" Then
                ConfirmProcess()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetSessionToScreen
    '	Discription	    : Set Session To Screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToScreen()
        Try
            'set value to from session to textbox 
            rblSearchType.Text = Session("rblSearchType")
            txtPO.Text = Session("txtPO")
            txtPaymentDateFrom.Text = Session("txtDeliveryDateFrom")
            txtDeliveryDateTo.Text = Session("txtDeliveryDateTo")
            txtPaymentDateFrom.Text = Session("txtPaymentDateFrom")
            txtPaymentDateTo.Text = Session("txtPaymentDateTo")
            txtVendor_Name.Text = Session("txtVendor_Name")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToScreen", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetScreenToSession
    '	Discription	    : Set Screen To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetScreenToSession()
        Try
            ' set search text to session
            Session("rblSearchType") = rblSearchType.Text.Trim
            Session("txtPO") = txtPO.Text.Trim
            Session("txtDeliveryDateFrom") = txtPaymentDateFrom.Text.Trim
            Session("txtDeliveryDateTo") = txtDeliveryDateTo.Text.Trim
            Session("txtPaymentDateFrom") = txtPaymentDateFrom.Text.Trim
            Session("txtPaymentDateTo") = txtPaymentDateTo.Text.Trim
            Session("txtVendor_Name") = txtVendor_Name.Text.Trim
            Session("txtInvoice_Start") = txtInvoice_Start.Text.Trim
            Session("txtInvoice_End") = txtInvoice_End.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToScreen", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtInvPurchase As New DataTable

            'Set data from condition search into Dto
            SetValueToDto(strEvent)

            ' call function GetItemList from ItemService
            dtInvPurchase = objInvPurchaseService.GetInvoice_PurchaseList(Session("objInvPurchaseDto"))
            ' set table object to session
            Session("dtInvPurchase") = dtInvPurchase
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DeleteItem
    '	Discription	    : Delete data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteItem()
        Try
            Dim intItemID As Integer = 0
            intItemID = Session("intItemID")
            Dim boolInuse As Boolean = False

            'check if there is some function use this item 
            boolInuse = objInvPurchaseService.IsUsed(intItemID)
            If boolInuse Then
                'case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_002"))
                Exit Sub
            End If

            ' check state of delete item
            If objInvPurchaseService.DeletePayment(intItemID) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto(ByVal mode As String)
        Try
            ' InvPurchase dto object
            Dim objInvPurchaseDto As New Dto.Invoice_PurchaseDto
            Dim startDeliveryDate As String = ""
            Dim endDeliveryDate As String = ""
            Dim arrDeliveryStartDate() As String = Split(txtDeliveryDateFrom.Text.Trim(), "/")
            Dim arrDeliveryEndDate() As String = Split(txtDeliveryDateTo.Text.Trim(), "/")

            Dim startPaymentDate As String = ""
            Dim endPaymentDate As String = ""
            Dim arrPaymentStartDate() As String = Split(txtPaymentDateFrom.Text.Trim(), "/")
            Dim arrPaymentEndDate() As String = Split(txtPaymentDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objInvPurchaseDto
                If UBound(arrDeliveryStartDate) > 0 Then
                    startDeliveryDate = arrDeliveryStartDate(2) & arrDeliveryStartDate(1) & arrDeliveryStartDate(0)
                End If
                If UBound(arrDeliveryEndDate) > 0 Then
                    endDeliveryDate = arrDeliveryEndDate(2) & arrDeliveryEndDate(1) & arrDeliveryEndDate(0)
                End If

                If UBound(arrPaymentStartDate) > 0 Then
                    startPaymentDate = arrPaymentStartDate(2) & arrPaymentStartDate(1) & arrPaymentStartDate(0)
                End If
                If UBound(arrPaymentEndDate) > 0 Then
                    endPaymentDate = arrPaymentEndDate(2) & arrPaymentEndDate(1) & arrPaymentEndDate(0)
                End If

                .strSearchType = rblSearchType.SelectedValue.ToString
                .strPO = txtPO.Text.Trim
                .strDeliveryDateFrom = startDeliveryDate
                .strDeliveryDateTo = endDeliveryDate
                .strPaymentDateFrom = startPaymentDate
                .strPaymentDateTo = endPaymentDate
                .strVendor_name = txtVendor_Name.Text.Trim()
                .strInvoice_start = txtInvoice_Start.Text.Trim
                .strInvoice_end = txtInvoice_End.Text.Trim
                .strMode = mode
            End With

            ' set dto object to session
            Session("objInvPurchaseDto") = objInvPurchaseDto

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
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtInvPurchase As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtInvPurchase = Session("dtInvPurchase")

            ' check record for display
            If Not IsNothing(dtInvPurchase) AndAlso dtInvPurchase.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInvPurchase)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtInvPurchase.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                If strEvent <> "Page_Load" Then
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
    ' Function name : IsDate
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-05-2013
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
            If txtPaymentDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtPaymentDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtPaymentDateTo.Text.Trim <> "" And txtPaymentDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateFrom.Text.Trim) > objValidate.IsDate(txtPaymentDateTo.Text.Trim) Then
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
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            Dim cblConfirmPayment As Boolean = False
            ' check permission of Item menu
            If Session("DepartmentName").ToString().Equals("Accounting") Then
                objAction = objPermission.CheckPermission(46)
            Else
                objAction = objPermission.CheckPermission(8)
            End If


            ' set permission 
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList

            'check comfirm_payment button
            Dim strSuperUser As String = System.Web.Configuration.WebConfigurationManager.AppSettings("ConfirmPayment")

            cblConfirmPayment = objInvPurchaseService.checkConfirmPayment()
            btnConfirmPayment.Enabled = cblConfirmPayment Or (strSuperUser.ToUpper.IndexOf(Session("UserName").ToString().ToUpper) > -1)
            ' set action permission to session
            Session("actList") = objAction.actList
            Session("cblConfirmPayment") = cblConfirmPayment

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
    '	Create Date	    : 07-06-2013
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
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtInvPurchase") = Nothing
            Session("rblSearchType") = Nothing
            Session("txtPO") = Nothing
            Session("txtDeliveryDateFrom") = Nothing
            Session("txtDeliveryDateTo") = Nothing
            Session("txtPaymentDateFrom") = Nothing
            Session("txtPaymentDateTo") = Nothing
            Session("txtVendor_Name") = Nothing
            Session("txtInvoice_Start") = Nothing
            Session("txtInvoice_End") = Nothing
            Session("actList") = Nothing
            Session("itemConfirm") = Nothing
            Session("cblConfirmPayment") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetConfirmID
    '	Discription	    : Keep data of each record that is already checkedn
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetConfirmID()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            For Each item As RepeaterItem In rptInquery.Items
                Dim intItemID As Integer = CInt(hashItemID(i))
                Dim chkBox As CheckBox = item.FindControl("chkConfirm")
                'Dim chkBox As HtmlInputCheckBox
                'chkBox = item.FindControl("chkConfirm")
                If chkBox.Checked = True Then
                    If itemConfirm = "" Then
                        itemConfirm = intItemID
                    Else
                        itemConfirm = itemConfirm & "," & intItemID
                    End If
                End If
                i = i + 1
            Next

            'Set itemConfirm into session
            Session("itemConfirm") = itemConfirm
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetConfirmID", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub
    '/**************************************************************
    '	Function name	: ConfirmProcess
    '	Discription	    : Execute confirm data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ConfirmProcess()
        Try
            'Keep data of each record that is already checked from session
            itemConfirm = Session("itemConfirm")

            'Send data to Accounting
            Dim returnInsertAccounting As Boolean
            returnInsertAccounting = objInvPurchaseService.ExecuteAccounting(itemConfirm)

            'Confirm completed
            If returnInsertAccounting = True Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_007"))

                ' call function search data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))

                'Export pdf
                exportPdf()
            Else
                'confirm failed
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_03_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ConfirmProcess", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: exportPdf
    '	Discription	    : export Pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub exportPdf()
        Try
            Dim dtPaymentPaid As New DataTable

            'Get data
            SearchDataReport()

            ' get table object from session 
            dtPaymentPaid = Session("dtPaymentPaid")

            If Not IsNothing(dtPaymentPaid) AndAlso dtPaymentPaid.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU03", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("exportPdf", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Get data for export crystal report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtPaymentPaid As New DataTable

            'Set data from condition search into Dto
            SetValueToDto("")

            ' call function GetItemList from ItemService
            dtPaymentPaid = objInvPurchaseService.GetPurchasePaidReport(itemConfirm)
            ' set table object to session
            Session("dtPaymentPaid") = dtPaymentPaid
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
