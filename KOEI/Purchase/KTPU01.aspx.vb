Imports Enums

Partial Class Purchase_KTPU01
    Inherits MessageInUpdatePanel

    Private objLog As New Common.Logs.Log
    Private check_status_id As RecordStatus = RecordStatus.Declined
    Private check_menu_id As MenuId = MenuId.PurchaseRequest
    Private strSuperUser As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTPU01: SEARCH PURCHASE REQUEST", Session("UserName"))
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Init", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            strSuperUser = System.Web.Configuration.WebConfigurationManager.AppSettings("AutoApprovePurchase")

            'Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Page.Form.Attributes.Add("enctype", "multipart/form-data")

            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckUserPer()
                Call CheckModeDel()
                Call CheckChangePage()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : btnAdd_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Call ClearSubSession()
            Response.Redirect("KTPU02.aspx?Mode=Add")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnSearch_Click"
    ''/**************************************************************
    ''	Function name	: btnSearch_Click
    ''	Discription	    : btnSearch_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 11-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    Try
    '        Dim objService As New Service.ImpPurchaseService
    '        Dim objDT As New System.Data.DataTable
    '        Dim objSearch As New Dto.PurchaseSearchDto
    '        Dim objCom As New Common.Utilities.Message

    '        If CheckDataSearch() = False Then Exit Sub

    '        objSearch = SetDataSearch()
    '        If objService.GetPurchaseForSearch(objSearch, objDT) Then
    '            'พบข้อมูล
    '            Session("DataSearch") = objSearch
    '            Call SetDataToSession(objDT)
    '            Call ShowDataTable()
    '        Else
    '            'ไม่พบข้อมูล
    '            Call ClearTB()
    '            objCom.AlertMessage(String.Empty, "Common_001")
    '        End If

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnSearch_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : btnSearch_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objDT As New System.Data.DataTable
            Dim objSearch As New Dto.PurchaseSearchDto
            'Dim objCom As New Common.Utilities.Message

            If CheckDataSearch() = False Then Exit Sub

            objSearch = SetDataSearch()
            If objService.GetPurchaseForSearch(objSearch, objDT) Then
                'พบข้อมูล
                Session("DataSearch") = objSearch
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                'ไม่พบข้อมูล
                Call ClearTB()
                Call ShowMsgInPanel(String.Empty, "Common_001")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnExcel_Click"
    ''/**************************************************************
    ''	Function name	: btnExcel_Click
    ''	Discription	    : btnExcel_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 11-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
    '    Try
    '        Dim objService As New Service.ImpPurchaseService
    '        Dim objSearch As New Dto.PurchaseSearchDto
    '        Dim objCom As New Common.Utilities.Message

    '        If CheckDataSearch() = False Then Exit Sub

    '        objSearch = SetDataSearch()
    '        If objService.GetPurchaseForReport(objSearch) = False Then
    '            'ไม่พบข้อมูล
    '            objCom.AlertMessage(String.Empty, "Common_002")
    '        End If

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnExcel_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : btnExcel_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objSearch As New Dto.PurchaseSearchDto
            'Dim objCom As New Common.Utilities.Message


            If CheckDataSearch() = False Then Exit Sub

            objSearch = SetDataSearch()
            If objService.GetPurchaseForReport(objSearch) = False Then
                'ไม่พบข้อมูล
                Call ShowMsgInPanel(String.Empty, "Common_002")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnExcel_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub


#Region "Old rptPurchase_ItemCommand"
    ''/**************************************************************
    ''	Function name	: rptPurchase_ItemCommand
    ''	Discription	    : rptPurchase_ItemCommand
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 11-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub rptPurchase_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptPurchase.ItemCommand
    '    Try
    '        Dim strAry() As String = e.CommandArgument.ToString.Split(",")
    '        Dim intPurchaseId As Integer = CInt(strAry(0))
    '        Dim intStatusId As Integer = CInt(strAry(1))
    '        Dim strPurchaseType As String = strAry(2)
    '        Session("PurchaseID") = intPurchaseId
    '        Session("StatusID") = intStatusId

    '        Select Case e.CommandName.Trim
    '            Case "View"
    '                Call LinkDetails(intPurchaseId)

    '            Case "Edit"
    '                Select Case strPurchaseType
    '                    Case "Purchase"
    '                        Call LinkEdit(intPurchaseId, PurchaseTypes.Purchase)
    '                    Case "Outsource"
    '                        Call LinkEdit(intPurchaseId, PurchaseTypes.OutSource)
    '                End Select

    '            Case "Del"
    '                Call LinkDel()

    '        End Select

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("rptPurchase_ItemCommand", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: rptPurchase_ItemCommand
    '	Discription	    : rptPurchase_ItemCommand
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptPurchase_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Try
            Dim strAry() As String = e.CommandArgument.ToString.Split(",")
            Dim intPurchaseId As Integer = CInt(strAry(0))
            Dim intStatusId As Integer = CInt(strAry(1))
            Dim strPurchaseType As String = strAry(2)
            Session("PurchaseID") = intPurchaseId
            Session("StatusID") = intStatusId

            Select Case e.CommandName.Trim
                Case "View"
                    Call LinkDetails(intPurchaseId)

                Case "Edit"
                    Select Case strPurchaseType
                        Case "Purchase"
                            Call LinkEdit(intPurchaseId, PurchaseTypes.Purchase)
                        Case "Outsource"
                            Call LinkEdit(intPurchaseId, PurchaseTypes.OutSource)
                    End Select

                Case "Modify"
                    Select Case strPurchaseType
                        Case "Purchase"
                            Call LinkModify(intPurchaseId, PurchaseTypes.Purchase)
                        Case "Outsource"
                            Call LinkModify(intPurchaseId, PurchaseTypes.OutSource)
                    End Select

                Case "Del"
                    Call LinkDel()

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptPurchase_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptPurchase_ItemDataBound
    '	Discription	    : rptPurchase_ItemDataBound
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptPurchase_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPurchase.ItemDataBound
        Try
            Dim objActUser = GetActUser()
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton
            Dim btnModify As New LinkButton
            Dim intStatusId As Integer

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            btnModify = DirectCast(e.Item.FindControl("btnModify"), LinkButton)
            intStatusId = DataBinder.Eval(e.Item.DataItem, "status_id")

            ' set permission on button and status_id is Declined
            If intStatusId = check_status_id Then 'Or DataBinder.Eval(e.Item.DataItem, "delivery_fg") = 0 
                btnEdit.Enabled = objActUser.actUpdate
                If objActUser.actUpdate = False Then
                    btnEdit.CssClass = "icon_edit2 icon_center15"
                End If
                btnDel.Enabled = objActUser.actDelete
                If objActUser.actDelete = False Then
                    btnDel.CssClass = "icon_del2 icon_center15"
                End If
            Else
                btnDel.CssClass = "icon_del2 icon_center15"
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnDel.Enabled = False
                btnEdit.Enabled = False
            End If

            If objActUser.actUpdate Then
                btnEdit.Visible = btnEdit.Enabled
                btnModify.Visible = Not btnEdit.Enabled
            Else
                btnModify.Visible = False
            End If

            'set font color 
            Dim lblPo_type_text As New Label
            Dim lblPo_no As New Label
            Dim lblVendor_name As New Label
            Dim lblIssue_date As New Label
            Dim lblDelivery_date As New Label
            Dim lblCurrency As New Label
            Dim lblSub_total As New Label
            Dim lblStatus As New Label

            lblPo_type_text = DirectCast(e.Item.FindControl("lblPo_type_text"), Label)
            lblPo_no = DirectCast(e.Item.FindControl("lblPo_no"), Label)
            lblVendor_name = DirectCast(e.Item.FindControl("lblVendor_name"), Label)
            lblIssue_date = DirectCast(e.Item.FindControl("lblIssue_date"), Label)
            lblDelivery_date = DirectCast(e.Item.FindControl("lblDelivery_date"), Label)
            lblCurrency = DirectCast(e.Item.FindControl("lblCurrency"), Label)
            lblSub_total = DirectCast(e.Item.FindControl("lblSub_total"), Label)
            lblStatus = DirectCast(e.Item.FindControl("lblStatus"), Label)

            If DataBinder.Eval(e.Item.DataItem, "font_color") = "Red" Then
                lblPo_type_text.CssClass = "font_red"
                lblPo_no.CssClass = "font_red"
                lblVendor_name.CssClass = "font_red"
                lblIssue_date.CssClass = "font_red"
                lblDelivery_date.CssClass = "font_red"
                lblCurrency.CssClass = "font_red"
                lblSub_total.CssClass = "font_red"
                lblStatus.CssClass = "font_red"
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptPurchase_ItemDataBound", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objCom As New Common.Utilities.Utility
            Dim strNew As String = objCom.GetQueryString("New")

            'Clear data on Form
            Call ClearForm()
            'Clear data on Table
            Call ClearTB()
            'Clear Session in page
            'If strNew.Trim = "True" Then Call ClearSession()
            Select Case strNew.Trim
                Case "True"
                    Call ClearSession()
                Case "Back"
                    Call CheckBackPage()
            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTB
    '	Discription	    : Clear data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTB()
        Try
            rptPurchase.DataSource = Nothing
            rptPurchase.DataBind()
            lblFootTB1.Text = "&nbsp;"
            lblFootTB2.Text = "&nbsp;"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearTB", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            rblPoType.SelectedIndex = 0
            txtPONoFrom.Text = String.Empty
            txtPONoTo.Text = String.Empty
            txtIssueDateFrom.Text = String.Empty
            txtIssueDateTo.Text = String.Empty
            txtDeliveryDateFrom.Text = String.Empty
            txtDeliveryDateTo.Text = String.Empty
            txtVendor.Text = String.Empty

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear Session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("objDT")
            Session.Remove("PageNo")
            Session.Remove("ActUser")
            Session.Remove("StatusID")
            Session.Remove("PurchaseID")
            Session.Remove("DataSearch")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSubSession
    '	Discription	    : Clear Session (Go to KTPU02)
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSubSession()
        Try
            Session.Remove("objDT")
            Session.Remove("PageNo")
            Session.Remove("ActUser")
            Session.Remove("StatusID")
            Session.Remove("PurchaseID")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSubSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserPer
    '	Discription	    : Check user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            objActUser = objComUser.CheckPermission(check_menu_id)
            Session("ActUser") = objActUser

            btnSearch.Enabled = objActUser.actList
            btnExcel.Enabled = objActUser.actList
            btnAdd.Enabled = objActUser.actCreate
            '2013/11/06 Ping Start-Add
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
            '2013/11/06 Ping End-Add

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckDataSearch
    '	Discription	    : Check Data for search
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDataSearch() As Boolean
        CheckDataSearch = False
        Try
            'Dim objComM As New Common.Utilities.Message
            Dim objComV As New Common.Validations.Validation

            'Issue Date
            If txtIssueDateFrom.Text.Trim <> String.Empty Then
                If objComV.IsDate(txtIssueDateFrom.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_004")
                    Call ShowMsgInPanel(String.Empty, "Common_004")
                    Exit Function
                End If
            End If

            If txtIssueDateTo.Text.Trim <> String.Empty Then
                If objComV.IsDate(txtIssueDateTo.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_004")
                    Call ShowMsgInPanel(String.Empty, "Common_004")
                    Exit Function
                End If
            End If

            If txtIssueDateFrom.Text.Trim <> String.Empty And txtIssueDateTo.Text.Trim <> String.Empty Then
                If objComV.IsDateFromTo(txtIssueDateFrom.Text.Trim, txtIssueDateTo.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_005")
                    Call ShowMsgInPanel(String.Empty, "Common_005")
                    Exit Function
                End If
            End If

            'Delivery Date
            If txtDeliveryDateFrom.Text.Trim <> String.Empty Then
                If objComV.IsDate(txtDeliveryDateFrom.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_004")
                    Call ShowMsgInPanel(String.Empty, "Common_004")
                    Exit Function
                End If
            End If

            If txtDeliveryDateTo.Text.Trim <> String.Empty Then
                If objComV.IsDate(txtDeliveryDateTo.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_004")
                    Call ShowMsgInPanel(String.Empty, "Common_004")
                    Exit Function
                End If
            End If

            If txtDeliveryDateFrom.Text.Trim <> String.Empty And txtDeliveryDateTo.Text.Trim <> String.Empty Then
                If objComV.IsDateFromTo(txtDeliveryDateFrom.Text.Trim, txtDeliveryDateTo.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_005")
                    Call ShowMsgInPanel(String.Empty, "Common_005")
                    Exit Function
                End If
            End If

            CheckDataSearch = True

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetDataSearch
    '	Discription	    : Set Data for search
    '	Return Value	: Dto.PurchaseSearchDto
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SetDataSearch() As Dto.PurchaseSearchDto
        Try
            Dim objValue As New Dto.PurchaseSearchDto
            SetDataSearch = Nothing
            With objValue
                'Check and set data type of purchase
                For Each objItem As ListItem In rblPoType.Items
                    If objItem.Selected = True Then
                        If objItem.Value.Trim <> String.Empty Then
                            .type_purchase = CInt(objItem.Value)
                            Exit For
                        End If
                    End If
                Next
                'Check and set data PO No.
                If txtPONoFrom.Text.Trim <> String.Empty Then
                    .po_no_from = txtPONoFrom.Text.Trim
                End If
                If txtPONoTo.Text.Trim <> String.Empty Then
                    .po_no_to = txtPONoTo.Text.Trim
                End If
                'Check and set data Issue Date
                If txtIssueDateFrom.Text.Trim <> String.Empty Then
                    .issue_date_from = txtIssueDateFrom.Text.Trim
                End If
                If txtIssueDateTo.Text.Trim <> String.Empty Then
                    .issue_date_to = txtIssueDateTo.Text.Trim
                End If
                'Check and set data Delivery Date
                If txtDeliveryDateFrom.Text.Trim <> String.Empty Then
                    .delivery_date_from = txtDeliveryDateFrom.Text.Trim
                End If
                If txtDeliveryDateTo.Text.Trim <> String.Empty Then
                    .delivery_date_to = txtDeliveryDateTo.Text.Trim
                End If
                'Check and set data vendor name
                If txtVendor.Text.Trim <> String.Empty Then
                    .vendor_name = txtVendor.Text.Trim
                End If

            End With
            SetDataSearch = objValue

        Catch ex As Exception
            ' Write error log
            SetDataSearch = Nothing
            objLog.ErrorLog("SetDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data table to session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession(ByVal objDT As System.Data.DataTable)
        Try
            Session("objDT") = objDT
            Session("PageNo") = "1"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDataTable
    '	Discription	    : Set Show data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDataTable()
        Try
            ' variable
            Dim objDT As System.Data.DataTable = Session("objDT")
            Dim objPageNo As Integer = Session("PageNo")
            Dim objComm As New Common.Utilities.Paging
            Dim objDataShow As PagedDataSource = objComm.DoPaging(objPageNo, objDT)
            Dim strFootTB As String
            ' set data show table
            rptPurchase.DataSource = objDataShow
            rptPurchase.DataBind()
            ' set data show foot table_1
            strFootTB = objComm.WriteDescription(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB1.Text = strFootTB
            ' set data show foot table_2
            strFootTB = objComm.DrawPaging(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB2.Text = strFootTB
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowDataTable", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetActUser
    '	Discription	    : Get action user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function GetActUser() As Common.UserPermissions.ActionPermission
        Try
            GetActUser = CType(Session("ActUser"), Common.UserPermissions.ActionPermission)
        Catch ex As Exception
            ' Write error log
            GetActUser = Nothing
            objLog.ErrorLog("GetActUser", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckChangePage()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strPageNo As String = objComm.GetQueryString("PageNo")

            If (Not strPageNo Is Nothing) AndAlso strPageNo.Trim <> String.Empty Then
                Session("PageNo") = strPageNo
                Call SetDataSearch(Session("DataSearch"))
                Call ShowDataTable()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckChangePage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataSearch
    '	Discription	    : Check mode change page show data search
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataSearch(ByVal objDataSearch As Dto.PurchaseSearchDto)
        Try
            With objDataSearch
                'Check and set data type of purchase
                Select Case .type_purchase
                    Case 0
                        rblPoType.Items(0).Selected = False
                        rblPoType.Items(1).Selected = True
                        rblPoType.Items(2).Selected = False
                    Case 1
                        rblPoType.Items(0).Selected = False
                        rblPoType.Items(1).Selected = False
                        rblPoType.Items(2).Selected = True
                    Case Else
                        rblPoType.Items(0).Selected = True
                        rblPoType.Items(1).Selected = False
                        rblPoType.Items(2).Selected = False
                End Select
                'Check and set data PO No.
                If (Not .po_no_from Is Nothing) AndAlso .po_no_from.Trim <> String.Empty Then
                    txtPONoFrom.Text = .po_no_from
                End If
                If (Not .po_no_to Is Nothing) AndAlso .po_no_to.Trim <> String.Empty Then
                    txtPONoTo.Text = .po_no_to
                End If
                'Check and set data Issue Date
                If (Not .issue_date_from Is Nothing) AndAlso .issue_date_from.ToString.Trim <> String.Empty Then
                    txtIssueDateFrom.Text = .issue_date_from.ToString
                End If
                If (Not .issue_date_to Is Nothing) AndAlso .issue_date_to.ToString.Trim <> String.Empty Then
                    txtIssueDateTo.Text = .issue_date_to.ToString
                End If
                'Check and set data Delivery Date
                If (Not .delivery_date_from Is Nothing) AndAlso .delivery_date_from.ToString.Trim <> String.Empty Then
                    txtDeliveryDateFrom.Text = .delivery_date_from.ToString
                End If
                If (Not .delivery_date_to Is Nothing) AndAlso .delivery_date_to.ToString.Trim <> String.Empty Then
                    txtDeliveryDateTo.Text = .delivery_date_to.ToString
                End If
                'Check and set data vendor name
                If (Not .vendor_name Is Nothing) AndAlso .vendor_name.Trim <> String.Empty Then
                    txtVendor.Text = .vendor_name
                End If

            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkDel
    '	Discription	    : Set Link button delete
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDel()
        Try
            'Dim objCom As New Common.Utilities.Message
            'objCom.ConfirmMessage("KTPU01", "ModeDel", String.Empty, "KTPU_01_001")

            Call ShowConfirmInPanel("KTPU01.aspx", "ModeDel", String.Empty, "KTPU_01_001")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: LinkDetails
    '	Discription	    : Set Link button details
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDetails(ByVal intPurchaseId As Integer)
        Try
            'Dim objComm As New Common.Utilities.Message
            Dim strPage As String = "KTPU01_Detail.aspx?ID=" & intPurchaseId.ToString()

            'objComm.ShowPagePopup(strPage, 800, 1000, "_blank", True)

            'Call ShowPopupInPanel(strPage, 800, 1000, "_blank", True)
            strPage = "javascript:showpopup('" & strPage & "','_blank');"
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", strPage, True)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDetails", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkEdit
    '	Discription	    : Set Link button edit
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkEdit(ByVal intPurchaseId As Integer, ByVal objPurchaseType As Enums.PurchaseTypes)
        Try
            If CheckUserApprove(objPurchaseType) = False Then Exit Sub
            Call ClearSubSession()
            Response.Redirect("KTPU02.aspx?Mode=Edit&ID=" & intPurchaseId.ToString)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkEdit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
    Protected Sub LinkModify(ByVal intPurchaseId As Integer, ByVal objPurchaseType As Enums.PurchaseTypes)
        Try
            If CheckUserApprove(objPurchaseType) = False Then Exit Sub
            Call ClearSubSession()
            Response.Redirect("KTPU02.aspx?Mode=Modify&ID=" & intPurchaseId.ToString)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkModify", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserApprove
    '	Discription	    : Check user approve
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckUserApprove(ByVal objPurchaseType As Enums.PurchaseTypes) As Boolean
        Try
            Dim FlagCheck As Boolean = False
            Dim strUserName As String = String.Empty
            Dim strApprove As String = String.Empty
            'Dim objMsg As New Common.Utilities.Message

            CheckUserApprove = False

            'Session("PurchaseNextApprove")
            'Session("OutSourceNextApprove")

            Select Case objPurchaseType
                Case PurchaseTypes.Purchase
                    strApprove = Session("PurchaseNextApprove").ToString
                    'ตรวจสอบค่า PurchaseNextApprove
                    If strApprove Is Nothing Then
                        FlagCheck = True
                    ElseIf strApprove = String.Empty Then
                        FlagCheck = True
                    ElseIf (IsNumeric(strApprove) AndAlso Val(strApprove) = 0) Then
                        FlagCheck = True
                    End If

                Case PurchaseTypes.OutSource
                    strApprove = Session("OutSourceNextApprove").ToString
                    'ตรวจสอบค่า OutSourceNextApprove
                    If strApprove Is Nothing Then
                        FlagCheck = True
                    ElseIf strApprove = String.Empty Then
                        FlagCheck = True
                    ElseIf (IsNumeric(strApprove) AndAlso Val(strApprove) = 0) Then
                        FlagCheck = True
                    End If

            End Select

            'ตรวจสอบค่าใน AutoApprovePurchase
            If FlagCheck = True Then
                strUserName = Session("UserName").ToString

                'ตรวจสอบค่าใน AutoApprovePurchase
                'Dim aaa = strSuperUser.ToUpper.IndexOf(strUserName.ToUpper)
                If strSuperUser.ToUpper.IndexOf(strUserName.ToUpper) = -1 Then
                    'ตรวจสอบค่าใน AutoApprovePurchase ไม่พบ, ไม่มี
                    'objMsg.AlertMessage(String.Empty, "KTPU_02_012")
                    Call ShowMsgInPanel(String.Empty, "KTPU_02_012")
                    Exit Function
                End If
            End If

            CheckUserApprove = True

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserApprove", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckModeDel
    '	Discription	    : Check mode delete 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckModeDel()
        Try
            Dim objComm As New Common.Utilities.Utility
            'Dim objComMsg As New Common.Utilities.Message
            Dim objService As New Service.ImpPurchaseService
            Dim strValue As String = objComm.GetQueryString("ModeDel")
            Dim intPurchaseId As Integer = CInt(Session("PurchaseID"))

            If (Not strValue Is Nothing) AndAlso strValue.Trim = "True" Then
                'Check data purchase before Delete
                If objService.CheckPurchase(intPurchaseId) = False Then
                    'objComMsg.AlertMessage(String.Empty, "KTPU_01_002", "KTPU01.aspx?PageNo=1")
                    Call ShowMsgInPanel(String.Empty, "KTPU_01_002", "KTPU01.aspx?PageNo=1")
                    Exit Sub
                End If

                'Delete data purchase
                If objService.DeletePurchase(intPurchaseId) Then
                    'Delete or Cancel Purchase is successful
                    'objComMsg.AlertMessage(String.Empty, "KTPU_01_003", "KTPU01.aspx?New=Back")
                    Call ShowMsgInPanel(String.Empty, "KTPU_01_003", "KTPU01.aspx?New=Back")
                Else
                    'Delete or Cancel Purchase is fail
                    'objComMsg.AlertMessage(String.Empty, "KTPU_01_004", "KTPU01.aspx?PageNo=1")
                    Call ShowMsgInPanel(String.Empty, "KTPU_01_004", "KTPU01.aspx?PageNo=1")
                End If

            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckModeDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckBackPage
    '	Discription	    : Check mode back page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckBackPage()
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objDT As New System.Data.DataTable
            Dim objSearch As Dto.PurchaseSearchDto = Session("DataSearch")
            'Dim objCom As New Common.Utilities.Message

            'ไม่มีการค้นหาข้อมูลไว้ก่อน ให้ออกได้ทันที
            If objSearch Is Nothing Then Exit Sub

            Call SetDataSearch(objSearch)
            If objService.GetPurchaseForSearch(objSearch, objDT) Then
                'พบข้อมูล
                Session("DataSearch") = objSearch
                Call SetDataToSession(objDT)
                'Call ShowDataTable()

                '* ทำการแสดงรายการใหม่
                Response.Redirect("KTPU01.aspx?PageNo=1")
            Else
                'ไม่พบข้อมูล
                'Call ClearTB()
                Response.Redirect("KTPU01.aspx?PageNo=1")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckBackPage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class


