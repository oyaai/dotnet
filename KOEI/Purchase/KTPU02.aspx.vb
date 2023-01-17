Imports Enums
Imports System.Web.Services
Imports System.Web.Configuration
Imports MySql.Data.MySqlClient

Partial Class Purchase_KTPU02
    Inherits MessageInUpdatePanel

    Private objLog As New Common.Logs.Log
    Private check_status_id As RecordStatus = RecordStatus.Declined
    Private check_menu_id As MenuId = MenuId.PurchaseRequest
    Private objPurchaseDto As New Dto.PurchaseDto
    Private intApproveUser As Integer
    Private strSuperUser As String = String.Empty
    '2013/09/27 Pranitda S. Start-Add
    Private thbIndex As Integer = 0
    '2013/09/27 Pranitda S. Start-Add
#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTPU02: PURCHASE REQUEST MANAGEMENT", Session("UserName"))
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
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim objMsg As New Common.Utilities.Message

            strSuperUser = System.Web.Configuration.WebConfigurationManager.AppSettings("AutoApprovePurchase")

            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckUserPer()
                Call CheckMode()
                Call CheckChangePage()

            Else
                '* จัดเก็บข้อมูลลง Dto
                Call SetDataToDto()

                '2. Check Type of Currency
                'Call CheckTypeCurrency()
                '3. Check Type of Vat and W/T
                'Call CheckTypeVatAndWT()
                '4. Check Type of Discount_type
                Call CheckTypeDiscount()
                '5. Check Type of Vendor (Change List_Item)
                Call CheckVendorToItemList()

            End If

            '1. Check Type of Purchase, Check Type of Vat and W/T (มีการเรียกอยู่ข้างใน)
            Call CheckTypePurchase()
            '6. Check show row (THB)
            Call CheckShowTHB()



            lblMsgDeliveryDate1.Text = objMsg.GetXMLMessage("KTPU_02_015")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnAddDetail_Click"
    ''/**************************************************************
    ''	Function name	: btnAddDetail_Click
    ''	Discription	    : btnAddDetail_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 19-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnAddDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDetail.Click
    '    Try
    '        'Check data purchase
    '        If CheckDataPurchase() = False Then Exit Sub
    '        'Check data purchase_detail
    '        If CheckDataDetail() = False Then Exit Sub

    '        Call ConfirmMsg(ResConfirm.Add)

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnAddDetail_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnAddDetail_Click
    '	Discription	    : btnAddDetail_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAddDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Check data purchase
            If CheckDataPurchase() = False Then Exit Sub
            'Check data purchase_detail
            If CheckDataDetail() = False Then Exit Sub

            Call ConfirmMsg(ResConfirm.Add)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAddDetail_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnEditDetail_Click"
    ''/**************************************************************
    ''	Function name	: btnEditDetail_Click
    ''	Discription	    : btnEditDetail_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 20-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnEditDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditDetail.Click
    '    Try
    '        'Check data purchase_detail
    '        If CheckDataDetail(True) = False Then Exit Sub

    '        Call ConfirmMsg(ResConfirm.Edit)

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnEditDetail_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnEditDetail_Click
    '	Discription	    : btnEditDetail_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnEditDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Check data purchase_detail
            If CheckDataDetail(True) = False Then Exit Sub
            If Session("Mode") = "Modify" Then
                Call ConfirmMsg(ResConfirm.Modify)
            Else
                Call ConfirmMsg(ResConfirm.Edit)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnEditDetail_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnCancelDetail_Click"
    ''/**************************************************************
    ''	Function name	: btnCancelDetail_Click
    ''	Discription	    : btnCancelDetail_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 20-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnCancelDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDetail.Click
    '    Try
    '        Call ClearSubDetail()
    '        'Clear message *Require
    '        Call ClearMsg()

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnCancelDetail_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnCancelDetail_Click
    '	Discription	    : btnCancelDetail_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCancelDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Call ClearSubDetail()
            'Clear message *Require
            Call ClearMsg()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnCancelDetail_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    Protected Sub rptPurchase_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPurchase.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)

            If Session("Mode") = "Modify" Then
                btnDel.Enabled = False
                btnDel.CssClass = "icon_del2 icon_center15"
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptPurchase_ItemDataBound", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptPurchase_ItemCommand
    '	Discription	    : rptPurchase_ItemCommand
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    : Boonyarit
    '	Update Date	    : 05-08-2013
    '*************************************************************/
    Protected Sub rptPurchase_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) 'Handles rptPurchase.ItemCommand
        Try
            'Dim objMsg As New Common.Utilities.Message
            Dim strDetailId As String = e.CommandArgument.ToString
            Session("DetailID") = strDetailId

            Select Case e.CommandName.Trim
                Case "Edit"
                    Call ShowPurchaeDetail()

                Case "Del"
                    If btnEditDetail.Visible = True Then
                        'objMsg.AlertMessage(String.Empty, "KTPU_02_016")
                        Call ShowMsgInPanel(String.Empty, "KTPU_02_016")
                    Else
                        Call ConfirmMsg(ResConfirm.Delete)
                    End If

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptVendor_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : btnBack_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles btnBack.Click
        Try
            Call ClearSession()
            Response.Redirect("KTPU01.aspx?New=Back")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnClear_Click"
    ''/**************************************************************
    ''	Function name	: btnClear_Click
    ''	Discription	    : btnClear_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 20-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    '    Try
    '        Call ClearForm()
    '        Call ClearTB()
    '        Call ClearSessionForBtnClear()
    '        Call ClearDetailOfDto()
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnClear_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : btnClear_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Call ClearForm()
            Call ClearTB()
            Call ClearSessionForBtnClear()
            Call ClearDetailOfDto()
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#Region "Old btnSave_Click"
    ''/**************************************************************
    ''	Function name	: btnSave_Click
    ''	Discription	    : btnSave_Click
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 20-06-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
    '    Try
    '        'Check data purchase
    '        If CheckDataPurchase(True) = False Then Exit Sub

    '        Call ConfirmMsg(ResConfirm.Save)

    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("btnSave_Click", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : btnSave_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Check data purchase
            If CheckDataPurchase(True) = False Then Exit Sub

            Call ConfirmMsg(ResConfirm.Save)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            'Dim objCom As New Common.Utilities.Utility

            'Set data to ddl
            Call SetDataToDDL()

            'Clear data on Form
            Call ClearForm()

            '2013/09/27 Pranitda S. Start-Add

            ddlCurrency.SelectedIndex = thbIndex

            If ddlCurrency.Items.Count > 0 Then
                For i As Integer = 0 To ddlCurrency.Items.Count - 1
                    If GetValueDDL(ddlCurrency.Items(i).Text) = thbIndex Then
                        ddlCurrency.Text.Split(" ")
                        Exit For
                    End If
                Next
            End If

            CheckTypeCurrency()
            '2013/09/27 Pranitda S. End-Add

            'Clear data on Table
            Call ClearTB()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            'lblPoNo.Text = String.Empty
            If Session("Mode") = "Add" Then Call GetPoNoNew()
            rblPoType.SelectedIndex = 0
            ddlVendor.Enabled = True

            '2013/10/2013 Pranitda S. Start-Mod
            'ddlVendor.SelectedIndex = 0
            ddlVendor.Text = String.Empty
            '2013/10/2013 Pranitda S. End-Mod

            txtQuotationNo.Text = String.Empty
            txtDeliveryDate.Text = String.Empty
            ddlPayTerm.SelectedIndex = 0
            ddlVat.SelectedIndex = 0
            ddlWT.SelectedIndex = 0
            ddlCurrency.SelectedIndex = 0
            lblScheduleRate.Text = "0.00000"
            txtHeadRemark.Text = String.Empty

            txtAttn.Text = String.Empty
            txtDeliverTo.Text = String.Empty
            txtContact.Text = String.Empty

            lblSubTotal.Text = String.Empty
            lblDiscountTotal.Text = String.Empty
            lblVatAmount.Text = String.Empty
            lblWTAmount.Text = String.Empty
            lblTotalAmount.Text = String.Empty

            lblThbSubTotal.Text = String.Empty
            lblThbDiscountTotal.Text = String.Empty
            lblThbVatAmount.Text = String.Empty
            lblThbWTAmount.Text = String.Empty
            lblThbTotalAmount.Text = String.Empty

            Call ClearSubDetail()
            Call ClearMsg()

            If (Not Session("Mode") Is Nothing) AndAlso (Session("Mode").ToString.Trim = "Edit" Or Session("Mode").ToString.Trim = "Modify") Then
                If Session("PurchaseDto") Is Nothing Then Exit Sub
                objPurchaseDto = Session("PurchaseDto")
                lblPoNo.Text = objPurchaseDto.po_no
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    Private Sub ClearSubDetail()
        Try
            '2013/10/2013 Pranitda S. Start-Mod
            'ddlItem.SelectedIndex = 0
            ddlItem.Text = String.Empty
            '2013/10/2013 Pranitda S. End-Mod

            txtJobOrder.Text = String.Empty
            'ddlIE.SelectedIndex = 0
            '03/10/2013 Ping Start - Mod
            ddlIE.Text = String.Empty
            '03/10/2013 Ping End - Mod
            txtUnitPrice.Text = String.Empty
            txtQty.Text = String.Empty
            ddlUnit.SelectedIndex = 0
            txtDiscount.Text = String.Empty
            ddlDiscountType.SelectedIndex = 0
            Call CheckTypeDiscount()
            txtDetailRemark.Text = String.Empty

            If Session("Mode") = "Modify" Then
                btnAddDetail.Visible = False
                btnAddDetail.Enabled = False
                btnEditDetail.Visible = True
            Else
                btnAddDetail.Visible = True
                btnAddDetail.Enabled = True
                btnEditDetail.Visible = False
                'btnCancelDetail.Visible = False
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSubDetail", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTB
    '	Discription	    : Clear data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 17-06-2013
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
    '	Function name	: ClearMsg
    '	Discription	    : Clear data msg
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 26-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearMsg()
        Try
            lblMsgCurrency.Visible = False
            lblMsgDeliveryDate.Visible = False
            lblMsgDeliveryDate1.Visible = False
            lblMsgIE.Visible = False
            lblMsgItem.Visible = False
            lblMsgJobOrder.Visible = False
            lblMsgPayTerm.Visible = False
            lblMsgPoType.Visible = False
            lblMsgQty.Visible = False
            lblMsgQuotationNo.Visible = False
            lblMsgUnit.Visible = False
            lblMsgUnitPrice.Visible = False
            'lblMsgVendorName.Visible = False
            lblMsgDiscount.Visible = False
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearMsg", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToDDL
    '	Discription	    : Set data to ddl
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToDDL()
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objValueDDL As New List(Of DropDownList)

            With objValueDDL
                .Add(ddlCurrency)
                .Add(ddlDiscountType)
                '23/10/2013 Ping Start-Mod
                '.Add(ddlIE)
                '3/10/2013 Ping End-Mod
                '2013/10/2013 Pranitda S. Start-Mod
                '.Add(ddlItem)
                '2013/10/2013 Pranitda S. End-Mod
                .Add(ddlPayTerm)
                .Add(ddlUnit)
                .Add(ddlVat)
                '2013/10/2013 Pranitda S. Start-Mod
                '.Add(ddlVendor)
                '.Add(dropVendor)
                '2013/10/2013 Pranitda S. End-Mod
                .Add(ddlWT)
            End With

            If objService.SetAllDDL(objValueDDL, thbIndex) = False Then
                Call EnabledButton(False, False, False, False)
            Else
                If Session("CheckCurrency") Is Nothing Then
                    Session("CheckCurrency") = ""
                End If
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDDL", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: EnabledButton
    '	Discription	    : Set enable button
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub EnabledButton(ByVal flgAdd As Boolean, ByVal flgEdit As Boolean, ByVal flgCancel As Boolean, ByVal flgSave As Boolean)
        Try
            btnAddDetail.Enabled = flgAdd
            btnEditDetail.Enabled = flgEdit
            btnCancelDetail.Enabled = flgCancel
            btnSave.Enabled = flgSave
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("EnabledButton", ex.Message.Trim, Session("UserName"))
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

            btnSave.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckMode
    '	Discription	    : Check Mode page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckMode()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strMode As String = objComm.GetQueryString("Mode")
            Dim strResConfirm As String = objComm.GetQueryString("ResConfirm")
            Dim strPurchaseId As String

            Select Case strMode.Trim
                Case "Add"
                    Session("Mode") = "Add"
                    Call GetPoNoNew()
                    Session("vendor") = ""
                Case "Edit"
                    Session("Mode") = "Edit"
                    strPurchaseId = objComm.GetQueryString("ID")
                    Call GetDataPurchaseById(CInt(strPurchaseId))
                    Call ShowDataFromDto()
                Case "Modify"
                    Session("Mode") = "Modify"
                    strPurchaseId = objComm.GetQueryString("ID")
                    Call GetDataPurchaseById(CInt(strPurchaseId))
                    Call ShowDataFromDto()
            End Select
            If strResConfirm = "True" Then
                Call ShowDataFromDto()
                Select Case Session("Confirm").ToString.Trim
                    Case "Add"
                        Call LinkAdd()
                    Case "Edit"
                        Call LinkEdit()
                    Case "Modify"
                        Call LinkModify()
                    Case "Delete"
                        Call LinkDel()
                    Case "Save"
                        Call LinkSave()
                End Select
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckMode", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckTypeVatAndWT
    '	Discription	    : Check Type vat and w/t
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckTypeVatAndWT()
        Try
            Dim objDT As New System.Data.DataTable
            'ตรวจสอบข้อมูล purchase_detail ว่ามีข้อมูลหรือไม่ ถ้ามีให้ทำการแก้ไขข้อมูล
            If (Not Session("PurchaseDetailDT") Is Nothing) Then
                objDT = Session("PurchaseDetailDT")
                If objDT.Rows.Count > 0 Then Call ChangeVatAndWT()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckTypeVatAndWT", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckTypePurchase
    '	Discription	    : Check Type of purchase
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckTypePurchase()
        Try
            'Dim objCom As New Common.Utilities.Message
            Dim objService As New Service.ImpPurchaseService
            Dim objPoType As Enums.PurchaseTypes = CInt(rblPoType.SelectedValue)
            Dim strUserName As String = String.Empty

            intApproveUser = objService.CheckTypePurchaeApprove(objPoType)
            If intApproveUser = 0 Then
                'กรณีไม่มีสิทธิ์ ต้องทำการตรวจสอบว่าเป็น SuperUser หรือไม่
                strUserName = Session("UserName").ToString
                'ตรวจสอบค่าใน AutoApprovePurchase
                If strSuperUser.ToUpper.IndexOf(strUserName.ToUpper) = -1 Then
                    Call EnabledButton(False, False, False, False)
                    'objCom.AlertMessage(String.Empty, "KTPU_02_012")
                    Call ShowMsgInPanel(String.Empty, "KTPU_02_012")
                End If
            End If

            If objPoType = 1 Then
                ddlWT.Enabled = True
                hiddenWT.Value = "true"
                If ddlWT.Items.Count > 0 AndAlso ddlWT.SelectedIndex = 0 Then
                    'กำหนดค่าเริ่มต้น
                    Dim intWT As Integer = WebConfigurationManager.AppSettings("wt_thb")
                    For indexWT As Integer = 0 To ddlWT.Items.Count - 1
                        If GetValueDDL(ddlWT.Items(indexWT).Text) = intWT Then
                            ddlWT.SelectedIndex = indexWT
                            Exit For
                        End If
                    Next
                End If
            Else
                ddlWT.SelectedIndex = 0
                ddlWT.Enabled = False
                hiddenWT.Value = "false"
            End If

            '* ทำการตรวจสอบค่าของ Vat and W/T
            Call CheckTypeVatAndWT()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckTypePurchase", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckTypeDiscount
    '	Discription	    : Check Type of discount_type
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckTypeDiscount()
        Try
            If ddlDiscountType.SelectedIndex = 0 Then
                txtDiscount.Text = String.Empty
                txtDiscount.Enabled = False
            Else
                If Session("Mode") = "Modify" Then
                    txtDiscount.Enabled = False
                Else
                    txtDiscount.Enabled = True
                End If
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckTypeDiscount", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    ''/**************************************************************
    ''	Function name	: CheckVendorToItemList
    ''	Discription	    : Check Type of vendor to item_list
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 31-07-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Private Sub CheckVendorToItemList()
    '    Try
    '        Dim objService As New Service.ImpPurchaseService

    '        'ตรวจสอบ vendor
    '        If ddlVendor.SelectedIndex = 0 Then
    '            ddlItem.SelectedIndex = 0
    '        Else

    '            'ตรวจสอบและเลือก Item
    '            If (Not Session("PurchaseDto") Is Nothing) Then
    '                objPurchaseDto = Session("PurchaseDto")

    '                If objPurchaseDto.vendor_id > 0 Then
    '                    ddlVendor.SelectedValue = objPurchaseDto.vendor_id
    '                End If
    '                'ดึงรายการ Item ใหม่ตาม vendor
    '                Call objService.SetItemList(ddlItem, objPurchaseDto.vendor_id)

    '                'ถ้า Vendor เดิม เลือก Item เดิม
    '                If objPurchaseDto.purchase_detail_tmp.item_id > 0 Then
    '                    ddlItem.SelectedValue = objPurchaseDto.purchase_detail_tmp.item_id
    '                End If

    '            Else
    '                'ดึงรายการ Item ใหม่ตาม vendor
    '                Call objService.SetItemList(ddlItem, ddlVendor.SelectedValue)
    '                'กำหนดรายการ Item รายการแรก
    '                ddlItem.SelectedIndex = 0
    '            End If

    '        End If
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("CheckTypeDiscount", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub

    '/**************************************************************
    '	Function name	: CheckShowTHB
    '	Discription	    : Check Type of Currency
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckShowTHB()
        Try
            If ddlCurrency.Items(ddlCurrency.SelectedIndex).Text.Trim = "THB" Or ddlCurrency.SelectedIndex = 0 Then
                panelTHB.Visible = False
            Else
                panelTHB.Visible = True
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckShowTHB", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckTypeCurrency
    '	Discription	    : Check Type of Currency
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub CheckTypeCurrency()
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objCom As New Common.Utilities.Utility
            Dim intSRate As Decimal = 0
            Dim strDate As String = String.Empty
            Dim intVat As Integer = WebConfigurationManager.AppSettings("vat_thb")


            If ddlCurrency.SelectedIndex = 0 Then
                panelTHB.Visible = False
                lblScheduleRate.Text = "0.00000"
                Exit Sub
            End If

            ddlCurrency.Text.Split(" ")

            '1. ดึงยอดของ Sehedule Rate ใหม่ทุกครั้ง
            If ddlCurrency.Items(ddlCurrency.SelectedIndex).Text.Trim = "THB" Then
                lblScheduleRate.Text = "1.00000"
                If ddlVat.Items.Count > 0 Then
                    'กำหนดค่าให้เริ่มต้นให้กับหน่วยเงิน THB
                    For indexVat As Integer = 0 To ddlVat.Items.Count - 1
                        If GetValueDDL(ddlVat.Items(indexVat).Text) = intVat Then
                            ddlVat.SelectedIndex = indexVat
                            Exit For
                        End If
                    Next
                Else

                End If
            Else
                If Session("Mode") = "Add" Then
                    intSRate = objService.GetS_Rate(ddlCurrency.SelectedValue)
                    strDate = Now().ToString("dd-MMM-yyyy")
                ElseIf Session("Mode") = "Edit" Or Session("Mode") = "Modify" Then
                    If (Not Session("PurchaseDto") Is Nothing) Then
                        objPurchaseDto = Session("PurchaseDto")
                        strDate = objCom.String2Date(objPurchaseDto.issue_date).ToString("yyyyMMdd")
                        intSRate = objService.GetS_Rate(ddlCurrency.SelectedValue, strDate)
                        strDate = objCom.String2Date(objPurchaseDto.issue_date).ToString("dd-MMM-yyyy")
                    End If
                End If

                'If intSRate < 1 Then
                '    lblScheduleRate.Text = "1.00000"
                '    lblMsgScheduleRate.Text = String.Format("Please setting schedule rate for {0}", strDate)
                '    Call ShowMsgInPanel(lblMsgScheduleRate.Text.Trim)
                'Else
                '    lblScheduleRate.Text = objCom.FormatNumeric(intSRate, 5)
                'End If
                lblScheduleRate.Text = objCom.FormatNumeric(intSRate, 5)
            End If
            '2. จัดการข้อมูลของ Discount Type ใหม่ทุกครั้งที่มีการเปลี่ยนค่า Currency
            If Session("CheckCurrency") <> ddlCurrency.Items(ddlCurrency.SelectedIndex).Text.Trim Then
                Call objService.SetDiscountType(ddlDiscountType, ddlCurrency.Items(ddlCurrency.SelectedIndex).Text)
                Call CheckTypeDiscount()
                Session("CheckCurrency") = ddlCurrency.Items(ddlCurrency.SelectedIndex).Text.Trim
            End If

            '3. จัดการข้อมูลใน Data Table ปรับปรุงใหม่ทุกครั้ง
            Call ChangeCurrency()


        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckTypeCurrency", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: CheckDataDetail
    '	Discription	    : Check data to table_detail
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 04-06-2014
    '*************************************************************/
    Private Function CheckDataDetail(Optional ByVal modeEdit As Boolean = False) As Boolean
        Try
            Dim objService As New Service.ImpJobOrderService
            'Dim objCom As New Common.Utilities.Message
            Dim objDT As New System.Data.DataTable
            Dim iCheck As Boolean = False
            Dim strDetailId As String = 0

            CheckDataDetail = False

            'Clear message *Require
            Call ClearMsg()

            If txtJobOrder.Text <> String.Empty Then
                'Check data job_order in use
                'Modify by Aey 2014-02-25
                'If objService.CheckJobOrderByPurchase(txtJobOrder.Text.Trim) = False Then
                '    'objCom.AlertMessage(String.Empty, "KTPU_02_008")
                '    Call ShowMsgInPanel(String.Empty, "KTPU_02_008")
                '    Exit Function
                'End If
            End If

            '2013/10/2013 Pranitda S. Start-Mod
            'If ddlItem.SelectedIndex < 1 Then
            If ddlItem.Text.Trim = "" Then
                '2013/10/2013 Pranitda S. End-Mod

                lblMsgItem.Visible = True
                If iCheck = False Then iCheck = True
                'Else
                '    If (Not Session("PurchaseDetailDT") Is Nothing) Then
                '        objDT = Session("PurchaseDetailDT")
                '        If objDT.Rows.Count > 0 Then
                '            If modeEdit = False Then
                '                For Each objItem In objDT.Rows
                '                    '2013/10/2013 Pranitda S. Start-Mod
                '                    'If CInt(objItem("item_id")) = GetValueDDL(ddlItem.SelectedValue) Then
                '                    If ddlItem.Text.Trim <> "" Then
                '                        Dim strItem As String() = ddlItem.Text.Split("#")
                '                        If CInt(objItem("item_id")) = GetValueDDL(strItem(strItem.Length - 1).Trim()) And objItem("job_order").ToString() = txtJobOrder.Text Then
                '                            '2013/10/2013 Pranitda S. End-Mod
                '                            'lblMsgItem.Visible = True
                '                            'If iCheck = False Then iCheck = True
                '                            'Exit For
                '                            'objCom.AlertMessage(String.Empty, "KTPU_02_014")
                '                            Call ShowMsgInPanel(String.Empty, "KTPU_02_014")
                '                            Exit Function
                '                        End If
                '                    End If
                '                Next
                '            Else
                '                strDetailId = Session("DetailID")
                '                For Each objItem In objDT.Rows
                '                    '2013/10/2013 Pranitda S. Start-Mod
                '                    'If CInt(objItem("item_id")) = GetValueDDL(ddlItem.SelectedValue) And objItem("id").ToString <> strDetailId Then
                '                    Dim strItem As String() = ddlItem.Text.Split("#")
                '                    If CInt(objItem("item_id")) = GetValueDDL(strItem(strItem.Length - 1).Trim()) And objItem("job_order").ToString() = txtJobOrder.Text And objItem("id").ToString <> strDetailId Then
                '                        '2013/10/2013 Pranitda S. End-Mod

                '                        'lblMsgItem.Visible = True
                '                        'If iCheck = False Then iCheck = True
                '                        'Exit For
                '                        'objCom.AlertMessage(String.Empty, "KTPU_02_014")
                '                        Call ShowMsgInPanel(String.Empty, "KTPU_02_014")
                '                        Exit Function
                '                    End If
                '                Next
                '            End If
                '        End If
                '    End If

            End If

            If txtJobOrder.Text.Trim = String.Empty Then
                lblMsgJobOrder.Visible = True
                If iCheck = False Then iCheck = True
            End If

            '3/10/2013 Ping Start-Mod
            'If ddlIE.SelectedIndex < 1 Then
            '3/10/2013 Ping End-Mod
            If ddlIE.Text.Trim = "" Then
                lblMsgIE.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If txtUnitPrice.Text.Trim = String.Empty Then
                'เช็คค่าว่าง
                lblMsgUnitPrice.Visible = True
                If iCheck = False Then iCheck = True
            ElseIf Val(txtUnitPrice.Text) = 0 Then
                'เช็คค่า 0
                lblMsgUnitPrice.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If txtQty.Text.Trim = String.Empty Then
                'เช็คค่าว่าง
                lblMsgQty.Visible = True
                If iCheck = False Then iCheck = True
            ElseIf Val(txtQty.Text) = 0 Then
                'เช็คค่า 0
                lblMsgQty.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If ddlUnit.SelectedIndex < 1 Then
                lblMsgUnit.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If iCheck = True Then Exit Function
            CheckDataDetail = True

        Catch ex As Exception
            ' Write error log
            CheckDataDetail = False
            objLog.ErrorLog("CheckDataDetail", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetDataDetail
    '	Discription	    : Set data to table_detail
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataDetail(Optional ByVal strDetailId As String = "")
        Try
            Dim objDT As New System.Data.DataTable
            Dim objDR As System.Data.DataRow
            Dim objCom As New Common.Utilities.Utility
            Dim decDiscount As Decimal = 0
            Dim decAmount As Decimal = 0
            Dim decVatAmount As Decimal = 0
            Dim decWTAmount As Decimal = 0
            Dim intIndex As Integer = Session("IndexRow")

            Dim strDiscountText As String = String.Empty

            If (Not Session("PurchaseDetailDT") Is Nothing) Then
                objDT = Session("PurchaseDetailDT")
                intIndex = intIndex + 1
            Else
                Call SetColumnsDT(objDT)
            End If

            objDR = objDT.NewRow

            If strDetailId <> String.Empty Then
                'Mode Edit data purchase_detail
                For Each objItem As System.Data.DataRow In objDT.Rows
                    If objItem("id").ToString.Trim = strDetailId.Trim Then
                        objDR = objItem
                        Exit For
                    End If
                Next
            Else
                'Mode New data purchase_detail
                If Session("Mode") = "Add" Then
                    '.id
                    objDR("id") = "A" & intIndex
                ElseIf Session("Mode") = "Edit" Or Session("Mode") = "Modify" Then
                    '.id
                    objDR("id") = "E" & intIndex
                End If

            End If

            '2013/10/2013 Pranitda S. Start-Mod
            'objDR("item_id") = ddlItem.SelectedValue
            'objDR("item_name") = ddlItem.Items(ddlItem.SelectedIndex).Text
            If ddlItem.Text.Trim <> "" Then
                Dim strItem As String() = ddlItem.Text.Split("#")
                objDR("item_id") = strItem(strItem.Length - 1).Trim()
                objDR("item_name") = strItem(0).Trim()
            End If
            '2013/10/2013 Pranitda S. End-Mod

            objDR("job_order") = txtJobOrder.Text
            '3/10/2013 Ping Start-Mod
            'objDR("ie_id") = ddlIE.SelectedValue
            'objDR("ie_name") = ddlIE.Items(ddlIE.SelectedIndex).Text
            If ddlIE.Text.Trim <> "" Then
                Dim strIE As String() = ddlIE.Text.Split("#")
                objDR("ie_id") = strIE(strIE.Length - 1).Trim()
                objDR("ie_name") = strIE(0).Trim()
            End If
            '3/10/2013 Ping Start-Mod
            objDR("unit_price") = objCom.FormatMoney(txtUnitPrice.Text)
            objDR("quantity") = objCom.FormatNumeric(CDec(txtQty.Text), 0, True)
            objDR("unit_id") = ddlUnit.SelectedValue
            objDR("unit_name") = ddlUnit.Items(ddlUnit.SelectedIndex).Text

            Select Case GetValueDDL(ddlDiscountType.SelectedValue)
                Case 0
                    decDiscount = 0
                    strDiscountText = ddlDiscountType.Items(0).Text
                Case 1
                    decDiscount = CDec(txtDiscount.Text)
                    strDiscountText = ddlDiscountType.Items(1).Text
                Case 2
                    decDiscount = ((CDec(txtUnitPrice.Text) * CDec(txtQty.Text)) * CDec(txtDiscount.Text) / 100)
                    strDiscountText = ddlDiscountType.Items(2).Text
            End Select
            '.discount
            If txtDiscount.Text.Trim <> String.Empty Then
                objDR("discount") = objCom.FormatMoney(txtDiscount.Text)
            Else
                objDR("discount") = "0.00"
            End If
            '.discount_type
            objDR("discount_type") = ddlDiscountType.SelectedValue
            '.discount_type_text
            objDR("discount_type_text") = strDiscountText

            'Amount = (unit price * qty) - @Discount
            decAmount = (CDec(txtUnitPrice.Text) * CDec(txtQty.Text)) - decDiscount
            '.amount
            objDR("amount") = objCom.FormatMoney(decAmount)
            '.vat_amount

            ' Modify function Modulate By Rawikarn 
            ' 14/08/2014 
            'Dim a As Integer = 0
            If ddlVat.SelectedIndex > 0 Then
                decVatAmount = Math.Round(CDec(decAmount * GetValueDDL(ddlVat.Items(ddlVat.SelectedIndex).Text) / 100), 2)
                'a = decAmount Mod GetValueDDL(ddlVat.Items(ddlVat.SelectedIndex).Text)

                objDR("vat_amount") = objCom.FormatMoney(decVatAmount)
            Else
                objDR("vat_amount") = "0.00"
            End If

            '.wt_amount
            If ddlWT.SelectedIndex > 0 Then
                decWTAmount = (decAmount * GetValueDDL(ddlWT.Items(ddlWT.SelectedIndex).Text) / 100)
                objDR("wt_amount") = objCom.FormatMoney(decWTAmount)
            Else
                objDR("wt_amount") = "0.00"
            End If

            '.remark
            objDR("remark") = txtDetailRemark.Text

            'Mode "Edit" ทำการเปลี่ยนแปลงข้อมูล Detail โดยทันทีไม่ต้องทำการเพิ่ม row ใหม่
            'Mode "Add" ทำการเพิ่มข้อมูล Detail โดยการทำการเพิ่ม row ใหม่ ตรวจสอบจากค่าของ strDetailId
            If strDetailId = String.Empty Then
                objDT.Rows.Add(objDR)
            End If

            '* จัดเก็บข้อมูลลง Session
            Session("IndexRow") = intIndex
            Session("PurchaseDetailDT") = objDT

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataDetail", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDataPurchaseDetail
    '	Discription	    : Show data purchase_detail
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDataPurchaseDetail()
        Try
            ' variable
            Dim objDT As System.Data.DataTable = Session("PurchaseDetailDT")
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
            objLog.ErrorLog("ShowDataPurchaseDetail", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckChangePage()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strPageNo As String = objComm.GetQueryString("PageNo")

            If (Not strPageNo Is Nothing) AndAlso strPageNo.Trim <> String.Empty Then
                Session("PageNo") = strPageNo
                Call ShowDataFromDto()
                Call ClearSubDetail()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckChangePage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ChangeVatAndWT
    '	Discription	    : Change value vat and w/t
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ChangeVatAndWT()
        Try
            Dim objDT As New System.Data.DataTable
            Dim objCom As New Common.Utilities.Utility
            Dim decVat As Decimal = GetValueDDL(ddlVat.Items(ddlVat.SelectedIndex).ToString)
            Dim decWT As Decimal = GetValueDDL(ddlWT.Items(ddlWT.SelectedIndex).ToString)
            Dim decAmount As Decimal = 0
            Dim decVatAmount As Decimal = 0
            Dim decWTAmount As Decimal = 0

            If Session("PurchaseDetailDT") Is Nothing Then Exit Sub

            objDT = Session("PurchaseDetailDT")
            For Each objItem In objDT.Rows
                decAmount = objItem("amount")
                decVatAmount = (decAmount * decVat / 100)
                objItem("vat_amount") = objCom.FormatMoney(decVatAmount)
                decWTAmount = (decAmount * decWT / 100)
                objItem("wt_amount") = objCom.FormatMoney(decWTAmount)
            Next

            'Show data purchase_detail to table
            Session("PurchaseDetailDT") = objDT
            Call ShowDataPurchaseDetail()
            Call CalSumTotal()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ChangeVatAndWT", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ChangeCurrency
    '	Discription	    : Change value currency
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ChangeCurrency()
        Try
            Dim objCom As New Common.Utilities.Utility
            Dim objDT As New System.Data.DataTable

            If Session("PurchaseDetailDT") Is Nothing Then Exit Sub

            objDT = Session("PurchaseDetailDT")
            For Each objItem In objDT.Rows
                If objItem("discount_type") = 1 Then
                    objItem("discount_type_text") = ddlDiscountType.Items(1).Text
                End If
            Next

            'Show data purchase_detail to table
            Session("PurchaseDetailDT") = objDT
            Call ShowDataPurchaseDetail()
            Call CalSumTotal()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ChangeCurrency", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CulTotal
    '	Discription	    : Calcula sum total amount
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CalSumTotal()
        Try
            Dim objDT As New System.Data.DataTable
            Dim objCom As New Common.Utilities.Utility
            Dim decDiscount As Decimal = 0
            Dim decSRate As Decimal = CDec(lblScheduleRate.Text)

            'Sub Total 		Sum(Amount)
            Dim sumAmount As Decimal = 0
            'Discount Total	Sum(Discount)
            Dim sumDiscount As Decimal = 0
            'Vat Amount		Sum(Vat)
            Dim sumVatAmount As Decimal = 0
            'W/T Amount		Sum(W/T)
            Dim sumWTAmount As Decimal = 0
            'Total Amount	Sum(Amount+Vat-W/T)
            Dim sumTotalAmount As Decimal = 0

            'Set Default
            lblSubTotal.Text = objCom.FormatMoney(sumAmount)
            lblDiscountTotal.Text = objCom.FormatMoney(sumDiscount)
            lblVatAmount.Text = objCom.FormatMoney(sumVatAmount)
            lblWTAmount.Text = objCom.FormatMoney(sumWTAmount)
            lblTotalAmount.Text = objCom.FormatMoney(sumTotalAmount)

            lblThbSubTotal.Text = objCom.FormatMoney(sumAmount)
            lblThbDiscountTotal.Text = objCom.FormatMoney(sumDiscount)
            lblThbVatAmount.Text = objCom.FormatMoney(sumVatAmount)
            lblThbWTAmount.Text = objCom.FormatMoney(sumWTAmount)
            lblThbTotalAmount.Text = objCom.FormatMoney(sumTotalAmount)

            If Session("PurchaseDetailDT") Is Nothing Then Exit Sub

            objDT = Session("PurchaseDetailDT")
            If objDT.Rows.Count = 0 Then Exit Sub

            For Each objItem In objDT.Rows
                Select Case CInt(objItem("discount_type"))
                    Case 0, 1
                        If objItem("discount") Is Nothing Or objItem("discount").ToString.Trim = String.Empty Then
                            decDiscount = 0
                        Else
                            decDiscount = CDec(objItem("discount"))
                        End If
                    Case 2
                        decDiscount = ((CDec(objItem("unit_price")) * CDec(objItem("quantity"))) * CDec(objItem("discount")) / 100)
                End Select

                sumAmount += CDec(objItem("amount"))
                'sumDiscount += CDec(objItem("discount"))
                sumDiscount += decDiscount
                sumVatAmount += CDec(objItem("vat_amount"))
                sumWTAmount += CDec(objItem("wt_amount"))
                sumTotalAmount += (CDec(objItem("amount")) + CDec(objItem("vat_amount")) - CDec(objItem("wt_amount")))
            Next

            lblSubTotal.Text = objCom.FormatMoney(sumAmount)
            lblDiscountTotal.Text = objCom.FormatMoney(sumDiscount)
            lblVatAmount.Text = objCom.FormatMoney(sumVatAmount)
            lblWTAmount.Text = objCom.FormatMoney(sumWTAmount)
            lblTotalAmount.Text = objCom.FormatMoney(sumTotalAmount)

            If decSRate > 0 Then
                lblThbSubTotal.Text = objCom.FormatMoney(sumAmount * decSRate)
                lblThbDiscountTotal.Text = objCom.FormatMoney(sumDiscount * decSRate)
                lblThbVatAmount.Text = objCom.FormatMoney(sumVatAmount * decSRate)
                lblThbWTAmount.Text = objCom.FormatMoney(sumWTAmount * decSRate)
                lblThbTotalAmount.Text = objCom.FormatMoney(sumTotalAmount * decSRate)
            End If

            If sumAmount > 0 Or sumTotalAmount > 0 Then
                ddlVendor.Enabled = False
            Else
                ddlVendor.Enabled = True
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CalSumTotal", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkDel
    '	Discription	    : Click Link button delete
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LinkDel()
        Try
            Dim objDT As New System.Data.DataTable
            Dim objItem As System.Data.DataRow
            Dim intIndex As Integer = -1
            Dim strDetailId As String = Session("DetailID")

            If Session("PurchaseDetailDT") Is Nothing Then Exit Sub
            objDT = Session("PurchaseDetailDT")
            For i As Integer = 0 To objDT.Rows.Count - 1
                objItem = objDT.Rows(i)
                If objItem("id").ToString.Trim = strDetailId Then
                    objDT.Rows.RemoveAt(i)
                    Exit For
                End If
            Next

            'Set data purchase_detail to session
            Session("PurchaseDetailDT") = objDT
            Session("IndexRow") = intIndex

            'Show data purchase_detail to table
            If objDT.Rows.Count = 0 Then
                Call ClearTB()
            Else
                Call ShowDataPurchaseDetail()
            End If
            Call CalSumTotal()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkAdd
    '	Discription	    : Click Link button add
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LinkAdd()
        Try
            'Set data purchase_detail to Session
            Call SetDataDetail()
            'Show data purchase_detail to table
            Call ShowDataPurchaseDetail()
            'Show data sum amount and total
            Call CalSumTotal()
            'Clear data purchase_detail
            Call ClearSubDetail()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkAdd", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkEdit
    '	Discription	    : Click Link button edit
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LinkEdit()
        Try
            'Set data purchase_detail to Session
            Call SetDataDetail(Session("DetailID"))
            'Show data purchase_detail to table
            Call ShowDataPurchaseDetail()
            'Show data sum amount and total
            Call CalSumTotal()
            'Clear data purchase_detail
            Call ClearSubDetail()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkEdit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
    Private Sub LinkModify()
        Try
            'Set data purchase_detail to Session
            Call SetDataDetail(Session("DetailID"))
            'Show data purchase_detail to table
            Call ShowDataPurchaseDetail()
            'Show data sum amount and total
            Call CalSumTotal()
            'Clear data purchase_detail
            Call ClearSubDetail()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkModify", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkSave
    '	Discription	    : Click Link button Save
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LinkSave()
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim objComm As New Common.Utilities.Message
            Dim strMsg As String = String.Empty
            Dim strMsgAutoApprove As String = String.Empty
            Dim strPoNo_New As String = String.Empty
            Dim intPoId_New As Integer = 0
            Dim strMode As String = Session("Mode")
            objPurchaseDto = Session("PurchaseDto")

            If objService.SavePurchase(objPurchaseDto, strMode, strPoNo_New, intPoId_New) Then
                'Save OK 'successful

                'Check auto approve (SuperUser)
                If CheckAutoApprove(intPoId_New) = False Then
                    strMsgAutoApprove = objComm.GetXMLMessage("KTPU_02_017")
                End If

                strMsg = "PO No. : " & strPoNo_New & vbCrLf
                Select Case strMode
                    Case "Add"
                        'เพิ่ม function add task process
                        Call objService.SaveTaskPurchase(strMode, intPoId_New)

                        strMsg &= (objComm.GetXMLMessage("KTPU_02_002"))
                        'objComm.AlertMessage(strMsg, String.Empty, "KTPU01.aspx")
                    Case "Edit"
                        'เพิ่ม function edit task process
                        Call objService.SaveTaskPurchase(strMode, intPoId_New)

                        strMsg &= (objComm.GetXMLMessage("KTPU_02_006"))
                        'objComm.AlertMessage(strMsg, String.Empty, "KTPU01.aspx")
                    Case "Modify"
                        'เพิ่ม function edit task process
                        Call objService.SaveTaskPurchase(strMode, intPoId_New)

                        strMsg &= (objComm.GetXMLMessage("KTPU_02_006"))
                End Select
                If strMsgAutoApprove.Trim <> String.Empty Then
                    strMsg &= (" and" & vbCrLf & strMsgAutoApprove)
                End If
                'ลบค่าในตัวแปร Session
                Call ClearSession()
                'แสดงข้อความแจ้งผลลัทธ์
                objComm.AlertMessageAndPopup(strMsg, "../Report/ReportViewer.aspx?ReportName=KTPU01&ID=" & intPoId_New, "KTPU01.aspx?New=Back", 1000, 990)
                'Call ShowMsgAndPopupInPanel(strMsg, "../Report/ReportViewer.aspx?ReportName=KTPU01&ID=" & intPoId_New, "KTPU01.aspx?New=Back", 1000, 990)
            Else
                'Save not OK 'not successful
                Select Case strMode
                    Case "Add"
                        'objComm.AlertMessage(String.Empty, "KTPU_02_003")
                        Call ShowMsgInPanel(String.Empty, "KTPU_02_003")
                    Case "Edit"
                        'objComm.AlertMessage(String.Empty, "KTPU_02_007")
                        Call ShowMsgInPanel(String.Empty, "KTPU_02_007")
                    Case "Modify"
                        'objComm.AlertMessage(String.Empty, "KTPU_02_007")
                        Call ShowMsgInPanel(String.Empty, "KTPU_02_007")
                End Select
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkSave", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserApprove
    '	Discription	    : Check flag user approve
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckUserApprove() As Boolean
        Try
            Dim FlagCheck As Boolean = False
            Dim objPoType As Enums.PurchaseTypes = CInt(rblPoType.SelectedValue)

            CheckUserApprove = False

            'Session("PurchaseNextApprove")
            'Session("OutSourceNextApprove")

            Select Case objPoType
                Case PurchaseTypes.Purchase
                    'ตรวจสอบค่า PurchaseNextApprove
                    If Session("PurchaseNextApprove") Is Nothing Then
                        FlagCheck = True
                    ElseIf Session("PurchaseNextApprove").ToString = String.Empty Then
                        FlagCheck = True
                    ElseIf (IsNumeric(Session("PurchaseNextApprove").ToString) AndAlso Val(Session("PurchaseNextApprove").ToString) = 0) Then
                        FlagCheck = True
                    End If

                Case PurchaseTypes.OutSource
                    'ตรวจสอบค่า OutSourceNextApprove
                    If Session("OutSourceNextApprove") Is Nothing Then
                        FlagCheck = True
                    ElseIf Session("OutSourceNextApprove").ToString = String.Empty Then
                        FlagCheck = True
                    ElseIf (IsNumeric(Session("OutSourceNextApprove").ToString) AndAlso Val(Session("OutSourceNextApprove").ToString) = 0) Then
                        FlagCheck = True
                    End If
            End Select

            If FlagCheck = False Then
                'มีคนหรือสิทธิ์ในการ Approve
                CheckUserApprove = True
            Else
                'ไม่มีคนหรือสิทธิ์ในการ Approve
                CheckUserApprove = False
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserApprove", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckAutoApprove
    '	Discription	    : Check auto user approve
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckAutoApprove(ByVal intPurchaseId As String) As Boolean
        Try
            Dim objService As New Service.ImpPurchaseService
            Dim strUserName As String = String.Empty
            Dim FlagCheck As Boolean = CheckUserApprove()
            'Dim objMsg As New Common.Utilities.Message

            CheckAutoApprove = False

            'ตรวจสอบสิทธิ์ว่ามีหรือไม่มีสิทธิ์ (มีสิทธิ์ไม่ต้องทำอะไร)
            If FlagCheck = False Then
                'กรณีไม่มีสิทธิ์ ต้องทำการตรวจสอบว่าเป็น SuperUser หรือไม่
                strUserName = Session("UserName").ToString
                'ตรวจสอบค่าใน AutoApprovePurchase
                If strSuperUser.ToUpper.IndexOf(strUserName.ToUpper) > -1 Then
                    'เป็น SuperUser ให้ทำการ UpdatePurchaseStatus ทันที (Approve=4)
                    If objService.UpdatePurchaseStatus(intPurchaseId, 4) = False Then
                        'objMsg.AlertMessage(String.Empty, "KTPU_02_017")
                        Exit Function
                    Else
                        'ทำการ UpdatePurchaseStatus(Approve=4) สำเร็จ
                        CheckAutoApprove = True
                    End If
                End If
            Else
                CheckAutoApprove = True
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckAutoApprove", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: ConfirmMsg
    '	Discription	    : Check confirm message
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ConfirmMsg(ByVal objType As ResConfirm)
        Try
            'Dim objComm As New Common.Utilities.Message
            Dim objService As New Service.ImpPurchaseService

            'Check data purchase in use before Edit
            If Session("Mode") = "Edit" Then 'Or Session("Mode") = "Modify"
                objPurchaseDto = Session("PurchaseDto")
                'Check data purchase before Edit
                If objService.CheckPurchase(objPurchaseDto.id) = False Then
                    'objComm.AlertMessage(String.Empty, "KTPU_02_007")
                    Call ShowMsgInPanel(String.Empty, "KTPU_02_007")
                    Exit Sub
                End If
            End If

            'Show message confirm
            Call SetDataToDto()

            'Check type confirm
            Select Case objType
                Case ResConfirm.Add
                    Session("Confirm") = "Add"
                    'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_009")
                    Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_009")
                Case ResConfirm.Edit
                    Session("Confirm") = "Edit"
                    'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_010")
                    Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_010")
                Case ResConfirm.Modify
                    Session("Confirm") = "Modify"
                    'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_010")
                    Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_010")
                Case ResConfirm.Delete
                    Session("Confirm") = "Delete"
                    'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_011")
                    Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_011")
                Case ResConfirm.Save
                    Session("Confirm") = "Save"
                    If Session("Mode") = "Add" Then
                        'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_001")
                        Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_001")
                    ElseIf Session("Mode") = "Edit" Or Session("Mode") = "Modify" Then
                        'objComm.ConfirmMessage("KTPU02", "ResConfirm", String.Empty, "KTPU_02_004")
                        Call ShowConfirmInPanel("KTPU02.aspx", "ResConfirm", String.Empty, "KTPU_02_004")
                    End If

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ConfirmMsg", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetValueDDL
    '	Discription	    : Get value ddl(DropDownList)
    '	Return Value	: Integer
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function GetValueDDL(ByVal strValue As String) As Integer
        Try
            If strValue = String.Empty Then
                GetValueDDL = 0
            Else
                GetValueDDL = CDec(strValue)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetValueDDL", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetDataToDto
    '	Discription	    : Set data to session PurchaseDto
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToDto()
        Try
            If (Not Session("PurchaseDto") Is Nothing) Then
                objPurchaseDto = Session("PurchaseDto")
            Else
                objPurchaseDto = New Dto.PurchaseDto
            End If

            With objPurchaseDto
                'ApproveUser
                .approve_user = intApproveUser
                'lblPoNo
                .po_no = lblPoNo.Text.Trim
                'rblPoType
                .po_type = CInt(rblPoType.SelectedValue)
                'ddlVendor
                '2013/10/2013 Pranitda S. Start-Mod
                '.vendor_id = GetValueDDL(ddlVendor.SelectedValue)
                Dim vendor As String() = ddlVendor.Text.Split("#")
                If vendor.Length > 0 Then
                    .vendor_id = GetValueDDL(vendor(vendor.Length - 1).Trim())
                End If
                Dim vendor_branch As String() = ddlVendorBranch.Text.Split("#")
                If vendor_branch.Length > 0 Then
                    .vendor_branch_id = GetValueDDL(vendor_branch(vendor_branch.Length - 1).Trim())
                End If
                '2013/10/2013 Pranitda S. Start-Mod
                'ddlVendor
                '2013/10/2013 Pranitda S. Start-Mod
                '.vendor_name = ddlVendor.Items(ddlVendor.SelectedIndex).Text.Trim
                If vendor.Length > 0 Then
                    .vendor_name = vendor(0).Trim()
                End If
                If vendor_branch.Length > 0 Then
                    .vendor_branch_name = vendor_branch(0).Trim()
                End If
                '2013/10/2013 Pranitda S. End-Mod
                'txtQuotationNo.Text
                .quotation_no = txtQuotationNo.Text.Trim
                'txtDeliveryDate.Text = String.Empty
                .delivery_date = txtDeliveryDate.Text.Trim
                'ddlPayTerm.SelectedIndex = 0
                .payment_term_id = GetValueDDL(ddlPayTerm.SelectedValue)
                .payment_term_name = ddlPayTerm.Items(ddlPayTerm.SelectedIndex).Text.Trim
                'ddlVat.SelectedIndex = 0
                .vat_id = GetValueDDL(ddlVat.SelectedValue)
                .vat_name = ddlVat.Items(ddlVat.SelectedIndex).Text.Trim
                'ddlWT.SelectedIndex = 0
                .wt_id = GetValueDDL(ddlWT.SelectedValue)
                .wt_name = ddlWT.Items(ddlWT.SelectedIndex).Text.Trim
                'ddlCurrency.SelectedIndex = 0
                .currency_id = GetValueDDL(ddlCurrency.SelectedValue)
                .currency_name = ddlCurrency.Items(ddlCurrency.SelectedIndex).Text.Trim
                'lblScheduleRate.Text
                .schedule_rate = CDec(lblScheduleRate.Text.Replace(",", String.Empty))
                'txtHeadRemark.Text = String.Empty
                .remarks = txtHeadRemark.Text.Trim
                'txtAttn.Text = String.Empty
                .attn = txtAttn.Text.Trim
                'txtDeliverTo.Text = String.Empty
                .deliver_to = txtDeliverTo.Text.Trim
                'txtContact.Text = String.Empty
                .contact = txtContact.Text.Trim

                'lblSubTotal.Text = String.Empty
                If lblSubTotal.Text <> String.Empty Then
                    .sub_total = CDec(lblSubTotal.Text.Replace(",", String.Empty))
                End If
                'lblDiscountTotal.Text = String.Empty
                If lblDiscountTotal.Text <> String.Empty Then
                    .discount_total = CDec(lblDiscountTotal.Text.Replace(",", String.Empty))
                End If
                'lblVatAmount.Text = String.Empty
                If lblVatAmount.Text <> String.Empty Then
                    .vat_amount = CDec(lblVatAmount.Text.Replace(",", String.Empty))
                End If
                'lblWTAmount.Text = String.Empty
                If lblWTAmount.Text <> String.Empty Then
                    .wt_amount = CDec(lblWTAmount.Text.Replace(",", String.Empty))
                End If
                'lblTotalAmount.Text = String.Empty
                If lblTotalAmount.Text <> String.Empty Then
                    .total_amount = CDec(lblTotalAmount.Text.Replace(",", String.Empty))
                End If

                'lblThbSubTotal.Text
                If lblThbSubTotal.Text <> String.Empty Then
                    .thb_sub_total = CDec(lblThbSubTotal.Text.Replace(",", String.Empty))
                End If
                'lblThbDiscountTotal.Text
                If lblThbDiscountTotal.Text <> String.Empty Then
                    .thb_discount_total = CDec(lblThbDiscountTotal.Text.Replace(",", String.Empty))
                End If
                'lblThbVatAmount.Text
                If lblThbVatAmount.Text <> String.Empty Then
                    .thb_vat_amount = CDec(lblThbVatAmount.Text.Replace(",", String.Empty))
                End If
                'lblThbWTAmount.Text
                If lblThbWTAmount.Text <> String.Empty Then
                    .thb_wt_amount = CDec(lblThbWTAmount.Text.Replace(",", String.Empty))
                End If
                'lblThbTotalAmount.Text
                If lblThbTotalAmount.Text <> String.Empty Then
                    .thb_total_amount = CDec(lblThbTotalAmount.Text.Replace(",", String.Empty))
                End If

                .purchase_detail_tmp = New Dto.PurchaseTemp
                With .purchase_detail_tmp
                    .id = objPurchaseDto.id
                    'ddlItem.SelectedIndex = 0
                    '2013/10/2013 Pranitda S. Start-Mod
                    '.item_id = GetValueDDL(ddlItem.SelectedValue)
                    '.item_name = ddlItem.Items(ddlItem.SelectedIndex).Text.Trim
                    If ddlItem.Text.Trim <> "" Then
                        Dim strItem As String() = ddlItem.Text.Split("#")
                        .item_id = GetValueDDL(strItem(strItem.Length - 1).Trim())
                        .item_name = strItem(0).Trim()
                    End If
                    '2013/10/2013 Pranitda S. End-Mod
                    'txtJobOrder.Text = String.Empty
                    .job_order = txtJobOrder.Text.Trim
                    'ddlIE.SelectedIndex = 0
                    '3/10/2013 Ping Start-Mod
                    '.ie_id = GetValueDDL(ddlIE.SelectedValue)
                    '.ie_name = ddlIE.Items(ddlIE.SelectedIndex).Text.Trim
                    If ddlIE.Text.Trim <> "" Then
                        Dim strIE As String() = ddlIE.Text.Split("#")
                        .ie_id = GetValueDDL(strIE(strIE.Length - 1).Trim())
                        .ie_name = strIE(0).Trim()
                    End If
                    '3/10/2013 Ping Start-Mod
                    'txtUnitPrice.Text = String.Empty
                    If txtUnitPrice.Text <> String.Empty Then
                        .unit_price = CDec(txtUnitPrice.Text.Replace(",", String.Empty))
                    End If
                    'txtQty.Text = String.Empty
                    If txtQty.Text <> String.Empty Then
                        .qty = CInt(txtQty.Text.Replace(",", String.Empty))
                    End If
                    'ddlUnit.SelectedIndex = 0
                    .unit_id = GetValueDDL(ddlUnit.SelectedValue)
                    .unit_name = ddlUnit.Items(ddlUnit.SelectedIndex).Text.Trim
                    'txtDiscount.Text = String.Empty
                    If txtDiscount.Text <> String.Empty Then
                        .discount = CDec(txtDiscount.Text.Replace(",", String.Empty))
                    End If
                    'ddlDiscountType.SelectedIndex = 0
                    .discount_type = GetValueDDL(ddlDiscountType.SelectedValue)
                    .discount_type_text = ddlDiscountType.Items(ddlDiscountType.SelectedIndex).Text.Trim
                    'txtDetailRemark.Text = String.Empty
                    .remark = txtDetailRemark.Text.Trim
                End With

            End With

            'จัดการข้อมูลของ purchase_detail
            If (Not Session("PurchaseDetailDT") Is Nothing) Then
                Call SetDataToDetailDto(objPurchaseDto)
            End If

            Session("PurchaseDto") = objPurchaseDto

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToDetailDto
    '	Discription	    : Set data to session PurchaseDto (purchase_detail)
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToDetailDto(ByRef objPurchaseDto As Dto.PurchaseDto)
        Try
            Dim objDT As New System.Data.DataTable
            Dim objDetail As Dto.PurchaseDetailDto

            objPurchaseDto.purchase_detail = New List(Of Dto.PurchaseDetailDto)
            objDT = Session("PurchaseDetailDT")
            For Each objItem In objDT.Rows
                objDetail = New Dto.PurchaseDetailDto
                With objDetail
                    '.id
                    .id = objItem("id")
                    '.item_id
                    .item_id = objItem("item_id")
                    '.item_name
                    .item_name = objItem("item_name")
                    '.job_order
                    .job_order = objItem("job_order")
                    '.ie_id
                    .ie_id = objItem("ie_id")
                    '.ie_name
                    .ie_name = objItem("ie_name")
                    '.unit_price
                    .unit_price = objItem("unit_price")
                    '.quantity
                    .qty = objItem("quantity")
                    '.unit_id
                    .unit_id = objItem("unit_id")
                    '.unit_name
                    .unit_name = objItem("unit_name")
                    '.discount
                    If (Not objItem("discount") Is Nothing) AndAlso objItem("discount").ToString.Trim <> String.Empty Then
                        .discount = CDec(objItem("discount"))
                    End If
                    '.discount_type
                    .discount_type = objItem("discount_type")
                    '.discount_type_text
                    .discount_type_text = objItem("discount_type_text")
                    '.vat_amount
                    If (Not objItem("vat_amount") Is Nothing) AndAlso objItem("vat_amount").ToString.Trim <> String.Empty Then
                        .vat = objItem("vat_amount")
                    End If
                    '.wt_amount
                    If (Not objItem("wt_amount") Is Nothing) AndAlso objItem("wt_amount").ToString.Trim <> String.Empty Then
                        .wt = objItem("wt_amount")
                    End If
                    '.amount
                    .amount = objItem("amount")
                    '.remark
                    .remarks = objItem("remark")
                End With
                objPurchaseDto.purchase_detail.Add(objDetail)
            Next

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToDetailDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDataFromDto
    '	Discription	    : Show data from session PurchaseDto
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDataFromDto()
        Try
            Dim objCom As New Common.Utilities.Utility
            Dim objService As New Service.ImpPurchaseService
            Dim objDT As New System.Data.DataTable

            If Session("PurchaseDto") Is Nothing Then Exit Sub

            objPurchaseDto = Session("PurchaseDto")

            With objPurchaseDto
                If Session("Mode") = "Modify" Then
                    Call EnabledButton(False, True, True, True)
                    rblPoType.Enabled = False
                    ddlVendor.Enabled = False
                    'txtDeliveryDate.Enabled = False
                    ddlVat.Enabled = False
                    ddlWT.Enabled = False
                    ddlCurrency.Enabled = False
                    ddlItem.Enabled = False
                    txtJobOrder.Enabled = False
                    txtUnitPrice.Enabled = False
                    txtQty.Enabled = False
                    txtDiscount.Enabled = False
                    ddlDiscountType.Enabled = False
                    txtDetailRemark.Enabled = False
                ElseIf Session("Mode") = "Edit" Then
                    'เช็ค status_id <> 5 ให้ล๊อกปุ่ม
                    If Not (.status_id = 5 Or .delivery_fg = 0) Then Call EnabledButton(False, False, False, False)
                End If

                If .po_no.Trim <> String.Empty Then
                    lblPoNo.Text = .po_no
                End If
                Select Case .po_type
                    Case 0
                        rblPoType.SelectedIndex = 0
                    Case 1
                        rblPoType.SelectedIndex = 1
                End Select

                '2013/10/2013 Pranitda S. Start-Mod
                'ddlVendor.SelectedValue = .vendor_id
                'Call objService.SetItemList(ddlItem, .vendor_id)
                ddlVendor.Text = String.Format("{0} # {1}", .vendor_name, .vendor_id)
                Session("vendor") = .vendor_id
                ddlVendorBranch.Text = String.Format("{0} # {1}", .vendor_branch_name, .vendor_branch_id)
                'Call objService.SetItemList(ddlItem, .vendor_id)
                '2013/10/2013 Pranitda S. End-Mod

                txtQuotationNo.Text = .quotation_no
                txtDeliveryDate.Text = .delivery_date
                ddlPayTerm.SelectedValue = .payment_term_id
                'Edit By Aey fix Bug When Edit don't Selected Value
                ddlVat.SelectedValue = .vat_id
                ddlWT.SelectedValue = .wt_id
                ddlCurrency.SelectedValue = .currency_id
                lblScheduleRate.Text = objCom.FormatNumeric(.schedule_rate, 5)
                Call objService.SetDiscountType(ddlDiscountType, ddlCurrency.Items(ddlCurrency.SelectedIndex).Text)
                txtHeadRemark.Text = .remarks
                txtAttn.Text = .attn
                txtDeliverTo.Text = .deliver_to
                txtContact.Text = .contact

                lblSubTotal.Text = objCom.FormatMoney(.sub_total)
                lblDiscountTotal.Text = objCom.FormatMoney(.discount_total)
                lblVatAmount.Text = objCom.FormatMoney(.vat_amount)
                lblWTAmount.Text = objCom.FormatMoney(.wt_amount)
                lblTotalAmount.Text = objCom.FormatMoney(.total_amount)

                lblThbSubTotal.Text = objCom.FormatMoney(.thb_sub_total)
                lblThbDiscountTotal.Text = objCom.FormatMoney(.thb_discount_total)
                lblThbVatAmount.Text = objCom.FormatMoney(.thb_vat_amount)
                lblThbWTAmount.Text = objCom.FormatMoney(.thb_wt_amount)
                lblThbTotalAmount.Text = objCom.FormatMoney(.thb_total_amount)
            End With

            If (Not objPurchaseDto.purchase_detail_tmp Is Nothing) Then
                With objPurchaseDto.purchase_detail_tmp
                    '2013/10/2013 Pranitda S. Start-Mod
                    'ddlItem.SelectedValue = .item_id
                    ddlItem.Text = String.Format("{0} # {1}", .item_name, .item_id)
                    '2013/10/2013 Pranitda S. End-Mod

                    txtJobOrder.Text = .job_order
                    '3/10/2013 Ping Start-Mod
                    'ddlIE.SelectedValue = .ie_id
                    ddlIE.Text = String.Format("{0} # {1}", .ie_name, .ie_id)
                    '3/10/2013 Ping End-Mod
                    txtUnitPrice.Text = IIf(.unit_price > 0, .unit_price, String.Empty)
                    txtQty.Text = IIf(.qty > 0, .qty, String.Empty)
                    ddlUnit.SelectedValue = .unit_id
                    txtDiscount.Text = .discount
                    ddlDiscountType.SelectedValue = .discount_type
                    Call CheckTypeDiscount()
                    txtDetailRemark.Text = .remark
                End With
            End If

            If (Not Session("PurchaseDetailDT") Is Nothing) Then
                'Show data purchase_detail to table
                objDT = Session("PurchaseDetailDT")
                Call ShowDataPurchaseDetail()

            ElseIf (Not objPurchaseDto.purchase_detail Is Nothing) Then
                Session("PurchaseDetailDT") = ChangePurchaseDetailListToDT(objPurchaseDto.purchase_detail)
                'Show data purchase_detail to table
                Call ShowDataPurchaseDetail()
            End If

            'ปรับข้อมูลของ DataTabale กับ List of purchase_detail
            If (Not Session("PurchaseDetailDT") Is Nothing) And (Not objPurchaseDto.purchase_detail Is Nothing) Then
                If objDT.Rows.Count <> objPurchaseDto.purchase_detail.Count Then
                    Call SetDataToDetailDto(objPurchaseDto)
                    Session("PurchaseDto") = objPurchaseDto
                End If
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowDataFromDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ChangePurchaseDetailListToDT
    '	Discription	    : Change data purchase_detail to datatable
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function ChangePurchaseDetailListToDT(ByVal objValue As List(Of Dto.PurchaseDetailDto)) As System.Data.DataTable
        Try
            ' variable
            Dim objDT As New System.Data.DataTable
            Dim objDR As System.Data.DataRow
            Dim objCom As New Common.Utilities.Utility

            ChangePurchaseDetailListToDT = Nothing
            Call SetColumnsDT(objDT)

            For Each objItem In objValue
                With objItem
                    objDR = objDT.NewRow
                    '.id
                    objDR("id") = .id
                    '.item_id
                    objDR("item_id") = .item_id
                    '.item_name
                    objDR("item_name") = .item_name
                    '.job_order
                    objDR("job_order") = .job_order
                    '.ie_id
                    objDR("ie_id") = .ie_id
                    '.ie_name
                    objDR("ie_name") = .ie_name
                    '.unit_price
                    objDR("unit_price") = objCom.FormatMoney(.unit_price)
                    '.quantity
                    objDR("quantity") = objCom.FormatNumeric(.qty, 0, True)
                    '.unit_id
                    objDR("unit_id") = .unit_id
                    '.unit_name
                    objDR("unit_name") = .unit_name
                    '.discount
                    objDR("discount") = objCom.FormatMoney(.discount)
                    '.discount_type
                    objDR("discount_type") = .discount_type
                    '.discount_type_text
                    objDR("discount_type_text") = .discount_type_text
                    '.vat_amount
                    objDR("vat_amount") = objCom.FormatMoney(.vat)
                    '.wt_amount
                    objDR("wt_amount") = objCom.FormatMoney(.wt)
                    '.amount
                    objDR("amount") = objCom.FormatMoney(.amount)
                    '.remark
                    objDR("remark") = .remarks
                    objDT.Rows.Add(objDR)
                End With
            Next
            ChangePurchaseDetailListToDT = objDT

        Catch ex As Exception
            ' Write error log
            ChangePurchaseDetailListToDT = Nothing
            objLog.ErrorLog("ChangePurchaseDetailListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetColumnsDT
    '	Discription	    : Set data columns datatable
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetColumnsDT(ByRef objDT As System.Data.DataTable)
        Try
            With objDT.Columns
                '.item_id
                .Add("id")
                '.item_id
                .Add("item_id")
                '.item_name
                .Add("item_name")
                '.job_order
                .Add("job_order")
                '.ie_id
                .Add("ie_id")
                '.ie_name
                .Add("ie_name")
                '.unit_price
                .Add("unit_price")
                '.quantity
                .Add("quantity")
                '.unit_id
                .Add("unit_id")
                '.unit_name
                .Add("unit_name")
                '.discount
                .Add("discount")
                '.discount_type
                .Add("discount_type")
                '.discount_type_text
                .Add("discount_type_text")
                '.vat_amount
                .Add("vat_amount")
                '.wt_amount
                .Add("wt_amount")
                '.amount
                .Add("amount")
                '.remark
                .Add("remark")
            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetColumnsDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowPurchaeDetail
    '	Discription	    : Show data purchase_detail
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowPurchaeDetail()
        Try
            Dim strDetailId As String = Session("DetailID")
            'Dim objDetail As New Dto.PurchaseDetailDto
            Dim objDT As New System.Data.DataTable

            If Session("PurchaseDetailDT") Is Nothing Then Exit Sub
            objDT = Session("PurchaseDetailDT")

            'Show data purchase_detail
            For Each objItem In objDT.Rows
                If strDetailId = objItem("id") Then
                    '2013/10/2013 Pranitda S. Start-Mod
                    'ddlItem.SelectedValue = objItem("item_id")
                    ddlItem.Text = String.Format("{0} # {1}", objItem("item_name"), objItem("item_id"))
                    '2013/10/2013 Pranitda S. End-Mod

                    txtJobOrder.Text = objItem("job_order")
                    '3/10/2013 Ping Start-Mod
                    'ddlIE.SelectedValue = objItem("ie_id")
                    ddlIE.Text = String.Format("{0} # {1}", objItem("ie_name"), objItem("ie_id"))
                    '3/10/2013 Ping End-Mod
                    txtUnitPrice.Text = objItem("unit_price")
                    txtQty.Text = objItem("quantity")
                    ddlUnit.SelectedValue = objItem("unit_id")
                    If CDec(objItem("discount").ToString.Replace(",", String.Empty)) > 0 Then
                        txtDiscount.Text = CDec(objItem("discount").ToString.Replace(",", String.Empty))
                    End If
                    ddlDiscountType.SelectedValue = CInt(objItem("discount_type"))
                    Call CheckTypeDiscount()
                    txtDetailRemark.Text = objItem("remark")
                    Exit For

                End If
            Next

            'Set button
            btnAddDetail.Visible = False
            btnEditDetail.Visible = True
            btnCancelDetail.Visible = True

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowPurchaeDetail", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckDataPurchase
    '	Discription	    : Check data purchase
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDataPurchase(Optional ByVal objByDetail As Boolean = False) As Boolean
        Try
            Dim iCheck As Boolean = False
            Dim objCom As New Common.Validations.Validation
            'Dim objComM As New Common.Utilities.Message
            Dim check_pass_date As String = System.Web.Configuration.WebConfigurationManager.AppSettings("check_pass_date")
            CheckDataPurchase = False

            'Clear message *Require
            Call ClearMsg()

            If rblPoType.SelectedValue.Trim <> "0" And rblPoType.SelectedValue.Trim <> "1" Then
                ' lblMsgPoType.Visible = True
                If iCheck = False Then iCheck = True
            End If

            '2013/10/2013 Pranitda S. Start-Mod
            'If ddlVendor.SelectedIndex < 1 Then
            'If ddlVendor.Text.Trim = "" Then
            '2013/10/2013 Pranitda S. End-Mod

            'lblMsgVendorName.Visible = True
            'If iCheck = False Then iCheck = True
            'End If

            If txtQuotationNo.Text.Trim = String.Empty Then
                lblMsgQuotationNo.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If txtDeliveryDate.Text.Trim = String.Empty Then
                lblMsgDeliveryDate.Visible = True
                If iCheck = False Then iCheck = True
            Else
                'ตรวจสอบว่าเป็นวันที่ถูกต้องหรือไม่
                If objCom.IsDate(txtDeliveryDate.Text.Trim) = False Then
                    'objComM.AlertMessage(String.Empty, "Common_004")
                    Call ShowMsgInPanel(String.Empty, "Common_004")
                    If iCheck = False Then iCheck = True
                End If

                'ตรวจสอบว่าเป็นวันที่ย้อนหลังหรือไม่
                If check_pass_date.ToUpper.Equals("YES") Then
                    If objCom.IsDateFromTo(Now.ToString("dd/MM/yyyy"), txtDeliveryDate.Text) = False AndAlso Session("Mode") = "Add" Then
                        lblMsgDeliveryDate1.Visible = True
                        If iCheck = False Then iCheck = True
                    End If
                End If
            End If

            If ddlPayTerm.SelectedIndex < 1 Then
                lblMsgPayTerm.Visible = True
                If iCheck = False Then iCheck = True
            End If

            If ddlCurrency.SelectedIndex < 1 Then
                lblMsgCurrency.Visible = True
                If iCheck = False Then iCheck = True
            End If

            'ตรวจสอบข้อมูลของ purchase_detail ด้วย
            If objByDetail = True Then
                Dim objDT As New System.Data.DataTable
                If Session("PurchaseDetailDT") Is Nothing Then
                    'objComM.AlertMessage(String.Empty, "KTPU_02_013")
                    Call ShowMsgInPanel(String.Empty, "KTPU_02_013")
                    If iCheck = False Then iCheck = True
                Else
                    objDT = Session("PurchaseDetailDT")
                    If objDT.Rows.Count = 0 Then
                        'objComM.AlertMessage(String.Empty, "KTPU_02_013")
                        Call ShowMsgInPanel(String.Empty, "KTPU_02_013")
                        If iCheck = False Then iCheck = True
                    End If
                End If
            End If

            'Check data for save
            If iCheck = True Then Exit Function
            CheckDataPurchase = True

        Catch ex As Exception
            ' Write error log
            CheckDataPurchase = False
            objLog.ErrorLog("CheckDataPurchase", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: GetPoNoNew
    '	Discription	    : Get data po_no (New)
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPoNoNew()
        Try
            Dim objService As New Service.ImpPurchaseService
            lblPoNo.Text = objService.GetPoNo

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetPoNoNew", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetDataPurchaseById
    '	Discription	    : Get data purchase by id
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetDataPurchaseById(ByVal intPurchaseId As Integer)
        Try
            Dim objService As New Service.ImpPurchaseService

            objPurchaseDto = objService.GetPurchaseById(intPurchaseId)
            Session("PurchaseDto") = objPurchaseDto

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetDataPurchaseById", ex.Message.Trim, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session all
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("Mode")
            Session.Remove("PageNo")
            Session.Remove("ActUser")
            Session.Remove("Confirm")
            Session.Remove("DetailID")
            Session.Remove("IndexRow")
            Session.Remove("PurchaseDto")
            Session.Remove("CheckCurrency")
            Session.Remove("PurchaseDetailDT")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSessionForBtnClear
    '	Discription	    : Clear session for btnClear
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSessionForBtnClear()
        Try
            Session.Remove("PageNo")
            Session.Remove("Confirm")
            Session.Remove("DetailID")
            Session.Remove("IndexRow")
            Session.Remove("CheckCurrency")
            Session.Remove("PurchaseDetailDT")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSessionForBtnClear", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearDetailOfDto
    '	Discription	    : Clear data for purchase dto
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearDetailOfDto()
        Try
            If (Not Session("PurchaseDto") Is Nothing) Then
                objPurchaseDto = Session("PurchaseDto")
                objPurchaseDto.purchase_detail = Nothing
                objPurchaseDto.purchase_detail_tmp = Nothing
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearDetailOfDto", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearDiscount
    '	Discription	    : Clear discount
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    'Private Sub ClearDiscount()
    '    Try
    '        txtDiscount.Text = String.Empty
    '        txtDiscount.Enabled = False
    '        ddlDiscountType.SelectedIndex = 0
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("ClearDiscount", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub

#End Region

#Region "WebMethod"
    '/**************************************************************
    '	Function name	: IsExistJobOrder
    '	Discription	    : Check exist Job Order
    '	Return Value	: Boolean
    '	Create User	    : Boonyarit
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    <WebMethod()> _
   Public Shared Function IsExistJobOrder( _
        ByVal strJobOrder As String _
    ) As Boolean
        Dim objLog As New Common.Logs.Log
        Try
            Dim objPurchaseSer As New Service.ImpPurchaseService
            Return objPurchaseSer.CheckPurchaseByJobOrder(strJobOrder)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("IsExistJobOrder", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function

    ''/**************************************************************
    ''	Function name	: ddlCurrencyInChange
    ''	Discription	    : Check ddlCurrency
    ''	Return Value	: Boolean
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 05-08-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    '<WebMethod()> _
    'Public Shared Function ddlCurrencyInChange() As Boolean
    '    'Call CheckTypeCurrency()
    '    Dim aa As String = ""
    '    Return False
    'End Function

    '########################################################################################
    '########################################################################################
    '########################################################################################

    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function IsExistVendorClient(ByVal strVendor As String) As Boolean
        Return strVendor.IndexOf(" # ") > 0
    End Function

    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function GetVendorList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess

        Dim cmdText As String = "select cast(concat(name,' # ',id) as char) vdname from mst_vendor where type1=0 and delete_fg <> 1 "
        If prefixText.Trim <> "" Then
            cmdText = cmdText & "and name like '%" & prefixText & "%' "
        End If
        If count > 0 Then
            cmdText = cmdText & "limit " & count.ToString()
        End If

        Dim vendors As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                vendors.Add(sdr("vdname").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If vendors.Count > 0 Then
            Return vendors.ToArray()
        Else
            Return Nothing
        End If
    End Function

    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function GetItemList(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess
        Dim cmdText As String = "select cast(concat(name,' # ',id) as char) itname from mst_item where delete_fg <> 1"
        If IsNothing(HttpContext.Current.Session("vendor")) = False Then
            cmdText = cmdText & " and vendor_id = " & HttpContext.Current.Session("vendor")
        End If
        If prefixText.Trim <> "" Then
            cmdText = cmdText & " and name like '%" & prefixText & "%'"
        End If
        If count > 0 Then
            cmdText = cmdText & " limit " & count.ToString()
        End If

        Dim items As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                items.Add(sdr("itname").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If items.Count > 0 Then
            Return items.ToArray()
        Else
            Return Nothing
        End If
    End Function

    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function GetVendorBranchList(ByVal prefixText As String, ByVal count As Integer) As String()

        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess
        Dim cmdText As String = "select cast(concat('Head Office # ',0) as char) branch from dual "
        If IsNothing(HttpContext.Current.Session("vendor")) = False Then
            cmdText = cmdText & "union " & _
                        "select cast(concat(name,' # ',id) as char) from mst_vendor_branch where delete_fg <> 1 " & _
                        "and vendor_id = " & HttpContext.Current.Session("vendor")
            If prefixText.Trim <> "" Then
                cmdText = cmdText & " and name like '%" & prefixText & "%'"
            End If
        End If
        If count > 0 Then
            cmdText = cmdText & " limit " & count.ToString()
        End If

        Dim branchs As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                branchs.Add(sdr("branch").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If branchs.Count > 0 Then
            Return branchs.ToArray()
        Else
            Return Nothing
        End If
    End Function

    Protected Sub ddlVendor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVendor.TextChanged
        If ddlVendor.Text.Trim <> "" Then
            Dim vendor As String() = ddlVendor.Text.Split("#")
            Session("vendor") = vendor(vendor.Length - 1).Trim()
        Else
            Session("vendor") = String.Empty
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod(), _
    System.Web.Services.WebMethod()> _
    Public Shared Function GetIEList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim conn As Common.DBConnection.MySQLAccess = New Common.DBConnection.MySQLAccess

        Dim cmdText As String = "SELECT cast(concat(code,'-',name,' # ',id) as char) as iename FROM mst_ie where delete_fg <> 1"
        If prefixText.Trim <> "" Then
            cmdText = cmdText & " and (name like '%" & prefixText & "%' or code like '%" & prefixText & "%')"
        End If
        cmdText = cmdText & " limit " & count.ToString()

        Dim IEs As List(Of String) = New List(Of String)
        Try
            If conn.ConnectionClose Then
                conn.Open()
            End If
            Dim sdr As MySqlDataReader = conn.ExecuteReader(cmdText)
            While sdr.Read
                IEs.Add(sdr("iename").ToString)
            End While
        Catch ex As Exception
        Finally
            If conn.ConnectionOpen Then
                conn.Close()
            End If
        End Try
        If IEs.Count > 0 Then
            Return IEs.ToArray()
        Else
            Return Nothing
        End If
    End Function

    '/**************************************************************
    '	Function name	: CheckVendorToItemList
    '	Discription	    : Check Type of vendor to item_list
    '	Return Value	: 
    '	Create User	    : Pranitda S.
    '	Create Date	    : 01-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckVendorToItemList()
        Try
            'Dim objService As New Service.ImpPurchaseService

            ''ตรวจสอบ vendor
            'If ddlVendor.SelectedIndex = 0 Then
            '    ddlItem.SelectedIndex = 0
            'Else

            '    'ตรวจสอบและเลือก Item
            '    If (Not Session("PurchaseDto") Is Nothing) Then
            '        objPurchaseDto = Session("PurchaseDto")

            '        If objPurchaseDto.vendor_id > 0 Then
            '            ddlVendor.SelectedValue = objPurchaseDto.vendor_id
            '        End If
            '        'ดึงรายการ Item ใหม่ตาม vendor
            '        Call objService.SetItemList(ddlItem, objPurchaseDto.vendor_id)

            '        'ถ้า Vendor เดิม เลือก Item เดิม
            '        If objPurchaseDto.purchase_detail_tmp.item_id > 0 Then
            '            ddlItem.SelectedValue = objPurchaseDto.purchase_detail_tmp.item_id
            '        End If

            '    Else
            '        'ดึงรายการ Item ใหม่ตาม vendor
            '        Call objService.SetItemList(ddlItem, ddlVendor.SelectedValue)
            '        'กำหนดรายการ Item รายการแรก
            '        ddlItem.SelectedIndex = 0
            '    End If

            'End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckTypeDiscount", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub


#End Region


End Class




