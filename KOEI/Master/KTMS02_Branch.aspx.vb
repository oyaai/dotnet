Imports System.Data
Partial Class Master_KTMS02_Branch
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private objVendorBranchSer As New Service.ImpVendorBranchService

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS02_Branch : Vendor Branch Address")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                initialPage()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptBranch_ItemCommand
    '	Discription	    : rptBranch_ItemCommand
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptBranch_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) _
    Handles rptBranch.ItemCommand
        Try
            Session("ItemIndex") = CInt(hashIndex(e.Item.ItemIndex).ToString)
            Session("selectRowIndex") = e.Item.ItemIndex
            Select Case e.CommandName
                Case "Edit"
                    SetDataToControl(Session("ItemIndex"))
                    Session("ActionMode") = "Edit"
                Case "Delete"
                    objMessage.ConfirmMessage("KTMS02_Branch", "DeleteBranch", objMessage.GetXMLMessage("KTMS_02_BRANCH_008"))
            End Select
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptBranch_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptBranch_ItemDataBound
    '	Discription	    : rptBranch_ItemDataBound
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptBranch_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) _
    Handles rptBranch.ItemDataBound
        Try
            objAction = Session("objAction")
            ' Set edit button permission
            If Not objAction.actUpdate Then
                CType(e.Item.FindControl("btnEdit"), LinkButton).Enabled = False
                CType(e.Item.FindControl("btnEdit"), LinkButton).CssClass = "icon_edit2 icon_center15"
            End If
            ' Set delete button permission
            If Not objAction.actDelete Then
                CType(e.Item.FindControl("btnDelete"), LinkButton).Enabled = False
                CType(e.Item.FindControl("btnDelete"), LinkButton).CssClass = "icon_del2 icon_center15"
            End If
            hashIndex.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "index"))
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptBranch_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the Index keys in ViewState
    ReadOnly Property hashIndex() As Hashtable
        Get
            If IsNothing(ViewState("hashIndex")) Then
                ViewState("hashIndex") = New Hashtable()
            End If
            Return CType(ViewState("hashIndex"), Hashtable)
        End Get
    End Property

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : btnSave_Click
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            SetDataDto()
            If Session("ActionMode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS02_Branch", "UpdateBranch", objMessage.GetXMLMessage("KTMS_02_BRANCH_007"))
            Else
                objMessage.ConfirmMessage("KTMS02_Branch", "InsertBranch", objMessage.GetXMLMessage("KTMS_02_BRANCH_001"))
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClose_Click
    '	Discription	    : btnClose_Click
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Try
            Dim sb As New StringBuilder
            With sb
                .AppendLine("<script type = 'text/javascript'>")
                .AppendLine("   window.opener.focus();")
                .AppendLine("   window.close();")
                .AppendLine("</script>")
            End With

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClosePopup", sb.ToString)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnClose_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : btnClear_Click
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            ClearDataInContorl()
            Session("ActionMode") = "Add"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: initialPage
    '	Discription	    : initialPage
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub initialPage()
        Try
            Dim pageStart As String = objUtility.GetQueryString("New")
            Dim objCountrySer As New Service.ImpCountryService
            If pageStart = "True" Then
                Session("ActionMode") = "Add"
                'Session("dtBranchInquiry") = Nothing
                If Session("Mode") = "Edit" AndAlso IsNothing(Session("dtBranchInquiry")) Then
                    Session("VendorBID") = objUtility.GetQueryString("VendorID")
                    Session("dtBranchInquiry") = objVendorBranchSer.GetBranchWithVendorID(Session("VendorBID"))
                End If
                CheckPermission()
            End If
            ' Load dropdown country list
            LoadListCountry()
            
            If objUtility.GetQueryString("InsertBranch") = "True" Then
                ' Check Duplicate Branch Name and Add data to datatable
                If Not CheckDuplicateBranchName() Then AddDataToDatatable()
            End If
            If objUtility.GetQueryString("UpdateBranch") = "True" Then
                ' Check Duplicate Branch Name and Edit select data to datatable
                If Not CheckDuplicateBranchName(Session("selectRowIndex")) Then UpdateDataToDataTable()
            End If
            If objUtility.GetQueryString("DeleteBranch") = "True" Then
                ' Edit select data to datatable
                DeleteDataFromDataTable()
            End If
            ' case not new enter then display page with page no
            DisplayPage(Request.QueryString("PageNo"), True)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("inintialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListVendor
    '	Discription	    : Load list Vendor function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 14-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListCountry()
        Try
            Dim objCountry As New Entity.ImpMst_CountryEntity
            Dim objListCountry As List(Of Entity.IMst_CountryEntity)

            ' get data list country_name
            objListCountry = objCountry.GetListCountryName
            'Get country list to DropDownList
            objUtility.LoadList(ddlCountry, objListCountry, "name", "id", True)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: AddDataToDatatable
    '	Discription	    : AddDataToDatatable
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub AddDataToDatatable()
        Try
            Dim objVendorBranchDto As New Dto.VendorBranchDto
            Dim dtBranch As New DataTable
            Dim dr As DataRow
            Dim ItemIndex As Integer
            ' Variable datatable column name
            Dim columnName() As String = {"index", "id", "name", "vendorID", "address", "zipcode", "countryID", "countryName", _
                                          "fullAddress", "telNo", "faxNo", "email", "contact", "remarks", "delete_fg"}
            If Not IsNothing(Session("dtBranchInquiry")) Then
                ' Get datatable from session
                dtBranch = Session("dtBranchInquiry")
                If dtBranch.Rows.Count > 0 Then
                    ItemIndex = CInt(dtBranch.Rows(dtBranch.Rows.Count - 1).Item("index")) + 1
                Else
                    ItemIndex = 0
                End If
            Else
                For Each strTmp As String In columnName
                    dtBranch.Columns.Add(strTmp)
                Next
            End If
            ' Insert row to datatable
            objVendorBranchDto = Session("VendorBranchDto")
            With objVendorBranchDto
                dr = dtBranch.NewRow
                dr("index") = ItemIndex
                dr("id") = String.Empty
                dr("name") = .name.ToUpper
                dr("vendorID") = IIf(IsNothing(Session("VendorBID")), String.Empty, Session("VendorBID"))
                dr("address") = .address
                dr("zipcode") = .zipcode
                dr("countryID") = .countryID
                dr("countryName") = .countryName
                dr("fullAddress") = .fullAddress
                dr("telNo") = .telNo
                dr("faxNo") = .faxNo
                dr("email") = .email
                dr("contact") = .contact
                dr("remarks") = .remarks
                dr("delete_fg") = .delete_fg
                dtBranch.Rows.Add(dr)
            End With
            ' Return datatable to session
            Session("dtBranchInquiry") = dtBranch
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_003"))
        Catch ex As Exception
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_004"))
            ' Write error log
            objLog.ErrorLog("AddDataToDatatable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateDataToDataTable
    '	Discription	    : UpdateDataToDataTable
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateDataToDataTable()
        Try
            Dim objBranchDto As New Dto.VendorBranchDto
            Dim dtBranch As New DataTable
            Dim rowSelectedIndex As Integer
            ' Get row index
            objBranchDto = Session("VendorBranchDto")
            dtBranch = Session("dtBranchInquiry")
            ' Get Datatable row index
            rowSelectedIndex = dtBranch.Rows.IndexOf(CType(dtBranch.Select("index = '" & Session("ItemIndex") & "'")(0), DataRow))
            With dtBranch.Rows(rowSelectedIndex)
                .Item("name") = objBranchDto.name.ToUpper
                .Item("address") = objBranchDto.address
                .Item("zipcode") = objBranchDto.zipcode
                .Item("countryID") = objBranchDto.countryID
                .Item("countryName") = objBranchDto.countryName
                .Item("fullAddress") = objBranchDto.fullAddress
                .Item("telNo") = objBranchDto.telNo
                .Item("faxNo") = objBranchDto.faxNo
                .Item("email") = objBranchDto.email
                .Item("contact") = objBranchDto.contact
                .Item("remarks") = objBranchDto.remarks
                .Item("delete_fg") = objBranchDto.delete_fg
            End With
            ' Return datatable to session
            Session("dtBranchInquiry") = dtBranch
            Session("ActionMode") = "Add"
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_005"))
        Catch ex As Exception
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_006"))
            ' Write error log
            objLog.ErrorLog("UpdateDataToDataTable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteDataFromDataTable
    '	Discription	    : DeleteDataFromDataTable
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteDataFromDataTable()
        Try
            Dim dtBranch As New DataTable
            Dim rowSelectedIndex As Integer
            dtBranch = Session("dtBranchInquiry")
            ' Get row index 
            rowSelectedIndex = dtBranch.Rows.IndexOf(CType(dtBranch.Select("index = '" & Session("ItemIndex") & "'")(0), DataRow))
            If dtBranch.Rows(rowSelectedIndex).Item("id") <> "" Then
                If objVendorBranchSer.CheckBranchIsInUse(dtBranch.Rows(rowSelectedIndex).Item("id")) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_009"))
                    Exit Sub
                Else
                    dtBranch.Rows(rowSelectedIndex).Item("delete_fg") = 1
                End If
            Else
                dtBranch.Rows(rowSelectedIndex).Delete()
            End If
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_010"))
        Catch ex As Exception
            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_011"))
            ' Write error log
            objLog.ErrorLog("DeleteDataFromDataTable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToControl
    '	Discription	    : SetDataToControl
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToControl(ByVal ItemIndex As Integer)
        Try
            ' Get row index
            Dim row() As DataRow = CType(Session("dtBranchInquiry"), DataTable).Select("index = '" & ItemIndex & "'")
            With row(0)
                txtName.Text = .Item("name").ToString.ToUpper
                txtAddress.Text = .Item("address")
                txtZipCode.Text = .Item("zipcode")
                ddlCountry.SelectedValue = IIf(.Item("countryID") = "0", "", .Item("countryID"))
                txtTelNo.Text = .Item("telNo")
                txtFaxNo.Text = .Item("faxNo")
                txtEMail.Text = .Item("email")
                txtContact.Text = .Item("contact")
                txtRemark.Text = .Item("remarks")
            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDtoToControl
    '	Discription	    : SetDtoToControl
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDtoToControl()
        Try
            With CType(Session("VendorBranchDto"), Dto.VendorBranchDto)
                txtName.Text = .name.ToUpper
                txtAddress.Text = .address
                txtZipCode.Text = .zipcode
                ddlCountry.SelectedValue = IIf(.countryID = 0, "", .countryID)
                txtTelNo.Text = .telNo
                txtFaxNo.Text = .faxNo
                txtEMail.Text = .email
                txtContact.Text = .contact
                txtRemark.Text = .remarks
            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataDto
    '	Discription	    : SetDataDto
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataDto()
        Try
            Dim objVendorBranchDto As New Dto.VendorBranchDto
            With objVendorBranchDto
                .id = String.Empty
                .name = txtName.Text.ToUpper
                .vendorID = Session("VendorBID")
                .address = txtAddress.Text
                .zipcode = txtZipCode.Text
                .countryID = IIf(ddlCountry.SelectedValue = "", 0, ddlCountry.SelectedValue)
                .countryName = ddlCountry.SelectedItem.ToString
                .fullAddress = txtAddress.Text & "  " & txtZipCode.Text & "  " & ddlCountry.SelectedItem.ToString
                .telNo = txtTelNo.Text
                .faxNo = txtFaxNo.Text
                .email = txtEMail.Text
                .contact = txtContact.Text
                .remarks = txtRemark.Text
                .delete_fg = 0
            End With
            Session("VendorBranchDto") = objVendorBranchDto
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckDuplicateBranchName
    '	Discription	    : CheckDuplicateBranchName
    '	Return Value	: Boolean
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDuplicateBranchName(Optional ByVal strSelectedRow As String = "") As Boolean
        Try
            If Not IsNothing(Session("dtBranchInquiry")) Then
                Dim drBranch() As DataRow = CType(Session("dtBranchInquiry"), DataTable).Select("delete_fg<>1")
                Dim objVBDto As Dto.VendorBranchDto = Session("VendorBranchDto")
                For i = 0 To drBranch.Count - 1
                    If strSelectedRow <> CStr(i) Then
                        If objVBDto.name.Trim = drBranch(i).Item("name").ToString.Trim Then
                            SetDtoToControl()
                            objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_02_BRANCH_002"))
                            txtName.Focus()
                            Return True
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDuplicateBranchName", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : CheckPermission
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            Dim objPermission As New Common.UserPermissions.UserPermission
            objAction = objPermission.CheckPermission(Enums.MenuId.Vendor)
            If Not objAction.actCreate OrElse Not objAction.actUpdate Then
                btnSave.Enabled = False
            End If
            Session("objAction") = objAction
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 17-06-2013
    '	Update User	    : Wasan D.
    '	Update Date	    : 07-10-2013
    '*************************************************************/
    Private Sub DisplayPage( _
        ByVal intPageNo As Integer, _
        Optional ByVal boolNotAlertMsg As Boolean = False)
        Try
            Dim dtInquiry As New DataTable
            Dim objPage As New Common.Utilities.Paging
            dtInquiry = CType(Session("dtBranchInquiry"), DataTable).Clone
            ' get table object from session 
            For Each row As DataRow In CType(Session("dtBranchInquiry"), DataTable).Select("delete_fg<>1")
                dtInquiry.ImportRow(row)
            Next
            Session("PageNo") = intPageNo

            ' check record for display
            If Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInquiry)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptBranch.DataSource = pagedData
                rptBranch.DataBind()
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
                rptBranch.DataSource = Nothing
                rptBranch.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearDataInContorl
    '	Discription	    : Clear Data In Contorl
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 07-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearDataInContorl()
        Try
            txtName.Text = String.Empty
            txtAddress.Text = String.Empty
            txtZipCode.Text = String.Empty
            ddlCountry.SelectedIndex = 0
            txtTelNo.Text = String.Empty
            txtFaxNo.Text = String.Empty
            txtEMail.Text = String.Empty
            txtContact.Text = String.Empty
            txtRemark.Text = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearDataInContorl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
