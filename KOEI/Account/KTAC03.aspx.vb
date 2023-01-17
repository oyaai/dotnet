Imports System.IO
Imports System.Web
Imports System.Data
Imports CrystalDecisions
Imports System.Web.Services
Imports System.Globalization
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Account
'	Class Name		    : KTAC03
'	Class Discription	: Webpage for input payment
'	Create User 		: Komsan Luecha
'	Create Date		    : 24-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Namespace Account
    Partial Class KTAC03
        Inherits System.Web.UI.Page

        Private objLog As New Common.Logs.Log
        Private pagedData As New PagedDataSource
        Private objMessage As New Common.Utilities.Message
        Private objUtility As New Common.Utilities.Utility
        Private objValidate As New Validations.CommonValidation
        Private objPermission As New Common.UserPermissions.UserPermission
        Private objAction As New Common.UserPermissions.ActionPermission
        Private Const strInsResult As String = "InsResult"
        Private Const strUpdResult As String = "UpdResult"
        Private Const strDelResult As String = "DelResult"
        Private Const strAppResult As String = "AppResult"

#Region "Enum"
        Enum Action
            Insert = 1
            Update = 2
            Delete = 3
        End Enum
#End Region

#Region "Event"
        '/**************************************************************
        '	Function name	: Page_Init
        '	Discription	    : Event page initial
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub Page_Init( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles Me.Init
            Try
                ' write start log
                objLog.StartLog("KTAC03 : Accounting Payment")
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: Page_Load
        '	Discription	    : Event page load
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
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
        '	Function name	: rptInquery_DataBinding
        '	Discription	    : Event repeater binding data
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_DataBinding( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles rptInquery.DataBinding
            Try
                ' clear hashtable data
                hashIndex.Clear()
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: rptInquery_ItemCommand
        '	Discription	    : Event repeater item command
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_ItemCommand( _
            ByVal source As Object, _
            ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
        ) Handles rptInquery.ItemCommand
            Try
                ' variable for keep data from hashtable
                Dim intIndex As Integer = CInt(hashIndex(e.Item.ItemIndex).ToString())

                ' set Index to session
                Session("intIndex") = intIndex

                Select Case e.CommandName
                    Case "Delete"
                        ' case not used then confirm message to delete
                        objMessage.ConfirmMessage("KTAC03", strDelResult, objMessage.GetXMLMessage("KTAC_03_001"))
                    Case "Edit"
                        Session("Action") = Action.Update
                        ' set data to control for edit
                        UpdateInitial(intIndex)
                    Case "View"
                        ' call view data function
                        ViewDetails()
                End Select
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_ItemCommand", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: rptInquery_ItemDataBound
        '	Discription	    : Event repeater item data bound
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub rptInquery_ItemDataBound( _
            ByVal sender As Object, _
            ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
        ) Handles rptInquery.ItemDataBound
            Try
                ' Set ItemID to hashtable
                hashIndex.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "Index"))

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("rptInquery_ItemDataBound", ex.Message.ToString, Session("UserName"))
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
        '	Function name	: btnAdd_Click
        '	Discription	    : Event button add click
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub btnAdd_Click( _
           ByVal sender As Object, _
           ByVal e As System.EventArgs _
       ) Handles btnAdd.Click
            Try
                ' set data to Dto
                SetDataToDto()
                SetDataToControl()
                ' set Action to session
                If Session("Action") = Action.Update Then
                    ' confirm update message
                    objMessage.ConfirmMessage("KTAC03", strUpdResult, objMessage.GetXMLMessage("KTAC_03_010"))
                Else
                    If hideAccountingID.Value <> Nothing Or hideAccountingID.Value <> String.Empty Then
                        ' confirm insert message
                        objMessage.ConfirmMessage("KTAC03", strInsResult, objMessage.GetXMLMessage("KTAC_03_019"))
                    Else
                        ' confirm insert message
                        objMessage.ConfirmMessage("KTAC03", strInsResult, objMessage.GetXMLMessage("KTAC_03_002"))
                    End If
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: btnCancel_Click
        '	Discription	    : Event button cancel click
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub btnCancel_Click( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles btnCancel.Click
            Try
                ' clear session Action
                Session("Action") = Nothing
                ' clear control
                ClearControl()
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("btnCancel_Click", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: btnApply_Click
        '	Discription	    : Event button Apply click
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub btnApply_Click( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles btnApply.Click
            Try
                Session("dtReport") = Session("dtInquiry")
                objMessage.ConfirmMessage("KTAC03", strAppResult, objMessage.GetXMLMessage("KTAC_03_015"))
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("btnApply_Click", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ddlCurrency_SelectedIndexChanged
        '	Discription	    : Event Dropdownlist ddlCurrency Selected change
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 16-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, _
                                                       ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
            Try
                'txtCurrencyRate.Text = IIf(ddlCurrency.SelectedItem.ToString = "THB", "1.00", Nothing)

                If ddlCurrency.SelectedItem.ToString = "THB" Then
                    txtCurrencyRate.Text = "1.00"
                    txtAmountTHB.Text = txtTotal.Text
                Else
                    ' Select all string in textbox
                    txtCurrencyRate.Focus()
                    Dim strJS As String = "document.getElementById('" & txtCurrencyRate.ClientID & "').select();"
                    ClientScript.RegisterClientScriptBlock(Me.GetType(), "onload", _
                                                           "window.onload = function() { " & strJS & " }", True)
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("ddlCurrency_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ddlVendor_SelectedIndexChanged
        '	Discription	    : Event ddlVendor SelectedIndex Changed
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 21-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub ddlVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVendor.SelectedIndexChanged
            Try
                If ddlVendor.SelectedItem.ToString.Trim <> "" Then
                    LoadListVendorAddress(ddlVendor.SelectedValue)
                Else
                    ddlVendorAddress.Items.Clear()
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("ddlCurrency_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: btnSearch_Click
        '	Discription	    : Event button search click
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 28-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Dim Page As New UI.Page
            Dim sb As New System.Text.StringBuilder()
            ' Gets the executing web page 
            Page = HttpContext.Current.CurrentHandler

            sb.Append("<script type = 'text/javascript'>")
            sb.Append("window.onload = function OpenPopup() {")
            sb.Append("popupSearch = window.open('")
            sb.Append("KTAC02_Search.aspx?New=True&Type2=Payment")
            sb.Append("','")
            sb.Append("winPopSearch")
            sb.Append("','width=1234")
            sb.Append(",height=600")
            sb.Append(",toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes');")
            sb.Append("popupSearch.focus();")
            sb.Append("};</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "OpenPopup", sb.ToString())
        End Sub
#End Region

#Region "Function"

        '/**************************************************************
        '	Function name	: IsDuplication
        '	Discription	    : Check add Payment duplicate function
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function IsDuplication(ByVal strMode As String) As Boolean
            IsDuplication = True
            Try
                Dim dtTemp As New DataTable
                Dim objPaymentDto As New Dto.IncomeDto
                Dim strQueryDatatable As New StringBuilder

                objPaymentDto = Session("objPaymentDto")
                dtTemp = Session("dtInquiry")

                If Not IsNothing(dtTemp) Then
                    With strQueryDatatable
                        If strMode = "Search" Then
                            .Append(" accID = '" & hideAccountingID.Value & "'")
                        Else
                            .Append(" AccountType = '" & objPaymentDto.AccountType & "' AND ")
                            .Append(" JobOrder = '" & objPaymentDto.JobOrder & "' AND ")
                            .Append(" VendorID = '" & objPaymentDto.VendorID & "' AND ")
                            .Append(" IEID = '" & objPaymentDto.ItemExpense & "' AND ")
                            .Append(" VatID = '" & objPaymentDto.Vat & "' AND ")
                            .Append(" WTID = '" & objPaymentDto.WT & "' AND ")
                            .Append(" SubTotal = '" & CDec(objPaymentDto.Total).ToString & "' AND ")
                            .Append(" ReceiptDate = '" & objPaymentDto.ReceiptDate & "' ")
                            If strMode = "Edit" Then
                                .Append(" AND Index <> '" & Session("intIndex") & "' ")
                            End If
                        End If
                    End With

                    If dtTemp.Select(strQueryDatatable.ToString).Count() > 0 Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage(IIf(strMode = "Search", "KTAC_03_020", "KTAC_03_016")))
                        Return True
                    End If
                End If
                IsDuplication = False
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("IsDuplication", ex.Message.ToString, Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InitialPage
        '	Discription	    : Initial page function
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub InitialPage()
            Try
                ' set validate message
                SetValidateErrorMessage()
                ' Set button cancel text as Clear (Mode:Add)
                btnCancel.Text = "Clear"

                ' check case new enter
                If objUtility.GetQueryString("New") = "True" Then
                    ' call function clear session
                    ClearSession()
                    Dim sb As New StringBuilder
                    With sb
                        .AppendLine("<script type = 'text/javascript'>")
                        .AppendLine("   popupSearch = window.open('','winPopSearch','width=20,height=15,toolbar=no" _
                                    & ",location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');")
                        .AppendLine("   popupSearch.close();")
                        .AppendLine("</script>")
                    End With
                    ClientScript.RegisterStartupScript(Page.GetType(), "test", sb.ToString)
                End If

                ' call function check permission
                CheckPermission()

                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"), True)

                ' call function set dropdownlist
                LoadListVendor()
                LoadListVat()
                LoadListWT()
                LoadListAccountTitle()
                LoadListCurrency()

                ' check query string (confirm message)
                CheckQueryString()
                lblSumSubTotal.Text = CDec(CType(Session("dtInquiry"),  _
                                            DataTable).Compute("Sum(SubTotal)", "")).ToString("#,##0.00")
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
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
        Private Sub LoadListVendor()
            Try
                ' object Vendor service
                Dim objVendorSer As New Service.ImpVendorService
                ' listVendorDto for keep value from service
                Dim listVendorDto As New List(Of Dto.VendorDto)
                ' call function GetVendorForList from service
                listVendorDto = objVendorSer.GetVendorForList()

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlVendor, listVendorDto, "name", "id", True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListVendorAddress
        '	Discription	    : Load list Vendor Address function
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 14-10-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListVendorAddress(ByVal intVendorID As Integer)
            Try
                ' object Vendor service
                Dim objVBSer As New Service.ImpVendorBranchService
                ' listVendorDto for keep value from service
                Dim listVBDto As New List(Of Dto.VendorBranchDto)
                ' call function GetVendorForList from service
                listVBDto = objVBSer.GetVendorBranchForDDLList(intVendorID)

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlVendorAddress, listVBDto, "fullAddress", "id")

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListAccountTitle
        '	Discription	    : Load list Account Title function
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 15-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListAccountTitle()
            Try
                ' object Vendor service
                Dim objAccTitleSer As New Service.ImpIEService
                ' listVendorDto for keep value from service
                Dim listAccTitleDto As New List(Of Dto.IEDto)
                ' call function GetVendorForList from service
                listAccTitleDto = objAccTitleSer.GetListAccountTitleToDDL(Enums.AccountRecordTypes.Payment)

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlItemExpense, listAccTitleDto, "Name", "ID", True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListCurrency
        '	Discription	    : Load list Currency function
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 16-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListCurrency()
            Try
                ' object Vendor service
                Dim objCurrencySer As New Service.ImpCurrencyService
                ' listVendorDto for keep value from service
                Dim listCurrencyDto As New List(Of Dto.CurrencyDto)
                ' call function GetVendorForList from service
                listCurrencyDto = objCurrencySer.GetCurrencyForDropdownList()

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlCurrency, listCurrencyDto, "Name", "id", True)
                ddlCurrency.SelectedValue = ddlCurrency.Items(1).Value
                txtCurrencyRate.Text = CDec(1).ToString("#,##0.00")
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListCurrency", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListVat
        '	Discription	    : Load list Vat function
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListVat()
            Try
                ' object Vat service
                Dim objVatSer As New Service.ImpVatService
                ' listVatDto for keep value from service
                Dim listVatDto As New List(Of Dto.VatDto)
                ' call function GetVatForList from service
                listVatDto = objVatSer.GetVatForList

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlVat, listVatDto, "PercentString", "id", True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListVat", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: LoadListWT
        '	Discription	    : Load list WT function
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub LoadListWT()
            Try
                ' object Vat service
                Dim objWTSer As New Service.ImpWTService
                ' listVatDto for keep value from service
                Dim listWTDto As New List(Of Dto.WTDto)
                ' call function GetWTForList from service
                listWTDto = objWTSer.GetWTForList

                ' call function for bound data with dropdownlist
                objUtility.LoadList(ddlWT, listWTDto, "PercentString", "id", True)

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("LoadListWT", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ClearSession
        '	Discription	    : Clear session
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub ClearSession()
            Try
                ' clase all session used in this page
                Session("dtInquiry") = Nothing
                Session("objPaymentDto") = Nothing
                Session("Index") = Nothing
                Session("intIndex") = Nothing
                Session("Action") = Nothing
                Session("rowDetails") = Nothing
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: CheckPermission
        '	Discription	    : Check permission
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub CheckPermission()
            Try
                ' check permission of Item menu
                objAction = objPermission.CheckPermission(Enums.MenuId.Payment)
                ' set permission Create
                btnAdd.Enabled = objAction.actCreate
                btnApply.Enabled = objAction.actCreate

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetDataToDto
        '	Discription	    : Set data to dto
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetDataToDto()
            Try
                Dim objPaymentDto As New Dto.IncomeDto
                ' set data from control to dto object
                With objPaymentDto
                    .accountID = hideAccountingID.Value
                    .AccountType = rbtAccountType.SelectedValue
                    .JobOrder = txtJobOrder.Text.Trim
                    .VendorID = ddlVendor.SelectedValue
                    .Vat = ddlVat.SelectedValue
                    ' Calculate Vat amount
                    .VatAmount = Math.Round(((CDec(txtTotal.Text.Trim) _
                                              * CDec(txtCurrencyRate.Text.Trim)) _
                                              * CDec(Replace(ddlVat.SelectedItem.Text.Trim, "%", ""))) / 100, 2)
                    .WT = ddlWT.SelectedValue
                    ' Calculate WT amount
                    .WTAmount = Math.Round(((CDec(txtTotal.Text.Trim) _
                                             * CDec(txtCurrencyRate.Text.Trim)) _
                                             * CDec(Replace(ddlWT.SelectedItem.Text.Trim, "%", ""))) / 100, 2)
                    .Bank = txtBank.Text.Trim
                    .AccountName = txtAccountName.Text.Trim
                    .ItemExpense = ddlItemExpense.SelectedValue
                    .AccountNo = txtAccountNo.Text.Trim
                    .inputTotal = txtTotal.Text.Trim
                    .currencyID = ddlCurrency.SelectedValue
                    .currencyRate = txtCurrencyRate.Text.Trim
                    ' Calculate Subtotal amount
                    .Total = Math.Round(CDec(txtTotal.Text.Trim) * CDec(txtCurrencyRate.Text.Trim), 2)
                    .ReceiptDate = txtReceiptDate.Text.Trim
                    .Remark = txtRemark.Text.Trim
                    .statusID = hideStatusID.Value
                    .VendorBranchID = ddlVendorAddress.SelectedValue
                    .BranchName = ddlVendorAddress.SelectedItem.ToString
                    .chequeNo = txtChequeNo.Text
                End With
                ' set object to session
                Session("objPaymentDto") = objPaymentDto
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDataToDto", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetDataToControl
        '	Discription	    : Set data to control
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetDataToControl()
            Try
                Dim objPaymentDto As New Dto.IncomeDto
                objPaymentDto = Session("objPaymentDto")
                ' set data from dto to control
                With objPaymentDto
                    hideAccountingID.Value = .accountID
                    rbtAccountType.SelectedValue = .AccountType
                    txtJobOrder.Text = .JobOrder
                    ddlVendor.SelectedValue = .VendorID
                    ddlVat.SelectedValue = .Vat
                    hideWTAmount.Value = .Vat
                    txtVatAmount.Text = CDec(.VatAmount).ToString("#,##0.00")
                    ddlWT.SelectedValue = .WT
                    txtWTAmount.Text = CDec(.WTAmount).ToString("#,##0.00")
                    txtBank.Text = .Bank
                    txtAccountName.Text = .AccountName
                    ddlItemExpense.SelectedValue = .ItemExpense
                    txtAccountNo.Text = .AccountNo
                    txtTotal.Text = CDec(.inputTotal).ToString("#,##0.00")
                    ddlCurrency.SelectedValue = .currencyID
                    txtCurrencyRate.Text = .currencyRate
                    txtAmountTHB.Text = CDec(.Total).ToString("#,##0.00")
                    txtReceiptDate.Text = .ReceiptDate
                    txtRemark.Text = .Remark
                    hideStatusID.Value = .statusID
                    LoadListVendorAddress(.VendorID)
                    ddlVendorAddress.SelectedValue = .VendorBranchID
                    txtChequeNo.Text = .chequeNo
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDataToControl", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetDataToControl
        '	Discription	    : Set data to control
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 29-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetDataToControl(ByVal dtAccounting As DataTable)
            Try
                ' set data from dto to control with search mode
                With dtAccounting.Rows(0)
                    rbtAccountType.SelectedValue = CInt(.Item("account_type"))
                    txtJobOrder.Text = .Item("job_order")
                    ddlVendor.SelectedValue = CInt(.Item("vendor_id"))
                    ddlVat.SelectedValue = .Item("vat_id")
                    hideVatAmount.Value = .Item("vat_id")
                    ' Calculate Vat amount to set into textbox
                    txtVatAmount.Text = CDec(CInt(Replace(ddlVat.SelectedItem.ToString, "%", "")) _
                                            * CDec(.Item("sub_total")) / 100).ToString("#,##0.00")
                    ddlWT.SelectedValue = .Item("wt_id")
                    hideWTAmount.Value = .Item("wt_id")
                    ' Calculate WT amount to set into textbox
                    txtWTAmount.Text = CDec(CInt(Replace(ddlWT.SelectedItem.ToString, "%", "")) _
                                            * CDec(.Item("sub_total")) / 100).ToString("#,##0.00")
                    txtBank.Text = .Item("bank")
                    txtAccountName.Text = .Item("account_name")
                    ddlItemExpense.SelectedValue = CInt(.Item("ie_id"))
                    txtAccountNo.Text = .Item("account_no")
                    txtTotal.Text = CDec(.Item("sub_total")).ToString("#,##0.00")
                    ddlCurrency.SelectedValue = 1
                    txtCurrencyRate.Text = "1.00"
                    txtAmountTHB.Text = CDec(.Item("sub_total")).ToString("#,##0.00")
                    txtReceiptDate.Text = objUtility.String2Date(.Item("account_date"), "yyyyMMdd").ToString("dd/MM/yyyy")
                    txtRemark.Text = .Item("remark")
                    LoadListVendorAddress(CInt(.Item("vendor_id")))
                    ddlVendorAddress.SelectedValue = CInt(.Item("vendor_branch_id"))
                    txtChequeNo.Text = .Item("cheque_no")
                    ' Set status_id
                    hideStatusID.Value = IIf(.Item("status_id") = Enums.RecordStatus.Declined, _
                                             Enums.RecordStatus.Deleted, Enums.RecordStatus.Waiting)
                End With
                btnCancel.Text = "Cancel"
            Catch ex As Exception
                ' write error log 
                objLog.ErrorLog("SetDataToControl", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: CheckInput
        '	Discription	    : Check input
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function CheckInput() As Boolean
            CheckInput = False
            Try
                ' variable validation object
                Dim objValidate As New Common.Validations.Validation

                ' validation receipt date
                If Not (objValidate.IsDate(txtReceiptDate.Text.Trim)) Then
                    objMessage.AlertMessage(String.Empty, "KTAC_03_021")
                    txtReceiptDate.Focus()
                    Exit Function
                End If

                CheckInput = True
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckInput", ex.Message.ToString, Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: AddDataToTable
        '	Discription	    : Add data to datatable
        '	Return Value	: Nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub AddDataToTable()
            Try
                ' variable table object
                Dim dt As DataTable
                ' variable row object
                Dim row As DataRow
                ' get index
                Dim intIndex As Integer = CInt(IIf(IsNothing(Session("Index")), 0, Session("Index"))) + 1
                ' get datatable from session
                dt = Session("dtInquiry")

                ' check first record and add data to datatable
                If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                    row = dt.NewRow()
                    row("Index") = intIndex
                    row("accID") = hideAccountingID.Value
                    row("Type") = Convert.ToInt32(Enums.AccountRecordTypes.Payment)
                    row("AccountType") = rbtAccountType.SelectedValue
                    row("AccountTypeName") = rbtAccountType.SelectedItem.ToString
                    row("JobOrder") = txtJobOrder.Text.Trim
                    row("VendorID") = ddlVendor.SelectedValue
                    row("VendorName") = ddlVendor.SelectedItem.ToString.Trim
                    row("VendorBranchID") = ddlVendorAddress.SelectedValue
                    row("BranchName") = ddlVendorAddress.SelectedItem
                    row("ChequeNo") = txtChequeNo.Text
                    row("VatID") = ddlVat.SelectedValue
                    row("VatText") = ddlVat.SelectedItem.ToString.Trim
                    row("VatAmount") = Convert.ToDecimal(txtVatAmount.Text.Trim)
                    row("Bank") = txtBank.Text.Trim
                    row("WTID") = ddlWT.SelectedValue
                    row("WTText") = ddlWT.SelectedItem.ToString.Trim
                    row("WTAmount") = Convert.ToDecimal(txtWTAmount.Text.Trim)
                    row("AccountName") = txtAccountName.Text.Trim
                    row("IEID") = ddlItemExpense.SelectedValue
                    row("IEName") = ddlItemExpense.SelectedItem.ToString.Trim
                    row("AccountNo") = txtAccountNo.Text.Trim
                    row("inputTotal") = txtTotal.Text.Trim
                    row("currencyID") = ddlCurrency.SelectedValue
                    row("currencyRate") = txtCurrencyRate.Text.Trim
                    row("SubTotal") = Convert.ToDecimal(txtAmountTHB.Text.Trim)
                    row("ReceiptDate") = txtReceiptDate.Text.Trim
                    row("ReceiptDateShow") = objUtility.String2Date(txtReceiptDate.Text.Trim, _
                                                                    "dd/MM/yyyy").ToString("dd/MMM/yyyy")
                    row("Remark") = txtRemark.Text.Trim
                    row("StatusID") = hideStatusID.Value
                    row("VoucherNo") = 0
                    dt.Rows.Add(row)
                Else
                    dt = New DataTable
                    dt.Columns.Add("Index")
                    dt.Columns.Add("accID")
                    dt.Columns.Add("Type")
                    dt.Columns.Add("AccountType")
                    dt.Columns.Add("AccountTypeName")
                    dt.Columns.Add("JobOrder")
                    dt.Columns.Add("VendorID")
                    dt.Columns.Add("VendorName")
                    dt.Columns.Add("VendorBranchID")
                    dt.Columns.Add("BranchName")
                    dt.Columns.Add("ChequeNo")
                    dt.Columns.Add("VatID")
                    dt.Columns.Add("VatText")
                    dt.Columns.Add("VatAmount", System.Type.GetType("System.Decimal"))
                    dt.Columns.Add("Bank")
                    dt.Columns.Add("WTID")
                    dt.Columns.Add("WTText")
                    dt.Columns.Add("WTAmount", System.Type.GetType("System.Decimal"))
                    dt.Columns.Add("AccountName")
                    dt.Columns.Add("IEID")
                    dt.Columns.Add("IEName")
                    dt.Columns.Add("AccountNo")
                    dt.Columns.Add("inputTotal")
                    dt.Columns.Add("currencyID")
                    dt.Columns.Add("currencyRate")
                    dt.Columns.Add("SubTotal", System.Type.GetType("System.Decimal"))
                    dt.Columns.Add("ReceiptDate")
                    dt.Columns.Add("ReceiptDateShow")
                    dt.Columns.Add("Remark")
                    dt.Columns.Add("StatusID")
                    dt.Columns.Add("VoucherNo", System.Type.GetType("System.Int32"))

                    row = dt.NewRow()
                    row("Index") = intIndex
                    row("accID") = hideAccountingID.Value
                    row("Type") = Convert.ToInt32(Enums.AccountRecordTypes.Payment)
                    row("AccountType") = rbtAccountType.SelectedValue
                    row("AccountTypeName") = rbtAccountType.SelectedItem.ToString
                    row("JobOrder") = txtJobOrder.Text.Trim
                    row("VendorID") = ddlVendor.SelectedValue
                    row("VendorName") = ddlVendor.SelectedItem.ToString.Trim
                    row("VendorBranchID") = ddlVendorAddress.SelectedValue
                    row("BranchName") = ddlVendorAddress.SelectedItem
                    row("ChequeNo") = txtChequeNo.Text
                    row("VatID") = ddlVat.SelectedValue
                    row("VatText") = ddlVat.SelectedItem.ToString.Trim
                    row("VatAmount") = Convert.ToDecimal(txtVatAmount.Text.Trim)
                    row("Bank") = txtBank.Text.Trim
                    row("WTID") = ddlWT.SelectedValue
                    row("WTText") = ddlWT.SelectedItem.ToString.Trim
                    row("WTAmount") = Convert.ToDecimal(txtWTAmount.Text.Trim)
                    row("AccountName") = txtAccountName.Text.Trim
                    row("IEID") = ddlItemExpense.SelectedValue
                    row("IEName") = ddlItemExpense.SelectedItem.ToString.Trim
                    row("AccountNo") = txtAccountNo.Text.Trim
                    row("inputTotal") = txtTotal.Text.Trim
                    row("currencyID") = ddlCurrency.SelectedValue
                    row("currencyRate") = txtCurrencyRate.Text.Trim
                    row("SubTotal") = Convert.ToDecimal(txtAmountTHB.Text.Trim)
                    row("ReceiptDate") = txtReceiptDate.Text.Trim
                    row("ReceiptDateShow") = objUtility.String2Date(txtReceiptDate.Text.Trim, _
                                                                    "dd/MM/yyyy").ToString("dd/MMM/yyyy")
                    row("Remark") = txtRemark.Text.Trim
                    row("StatusID") = hideStatusID.Value
                    row("VoucherNo") = 0
                    dt.Rows.Add(row)
                End If

                ' set data to session
                Session("dtInquiry") = dt
                Session("index") = intIndex
                DisplayPage(Session("PageNo"), True)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("AddDataToTable", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: DisplayPage
        '	Discription	    : Display page
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
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
                Session("PageNo") = intPageNo

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
                    btnApply.Enabled = True
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
                    btnApply.Enabled = False
                End If
                Session("Action") = Action.Insert
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
            Finally
                objUtility.RemQueryString("PageNo")
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: UpdateInitial
        '	Discription	    : Update initial data
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub UpdateInitial(ByVal intIndex As Integer)
            Try
                ' get datatable from session
                Dim dt As DataTable = Session("dtInquiry")
                ' filter row with Index
                Dim row() As DataRow = dt.Select(" Index = '" & intIndex & "'")

                ' set data to control
                With row(0)
                    hideAccountingID.Value = .Item("accID")
                    rbtAccountType.SelectedValue = .Item("AccountType")
                    txtJobOrder.Text = .Item("JobOrder")
                    ddlVendor.SelectedValue = .Item("VendorID")
                    ddlVat.SelectedValue = .Item("VatID")
                    txtVatAmount.Text = CDec(.Item("VatAmount")).ToString("#,##0.00")
                    txtBank.Text = .Item("Bank")
                    ddlWT.SelectedValue = .Item("WTID")
                    txtWTAmount.Text = CDec(.Item("WTAmount")).ToString("#,##0.00")
                    txtAccountName.Text = .Item("AccountName")
                    ddlItemExpense.SelectedValue = .Item("IEID")
                    txtAccountNo.Text = .Item("AccountNo")
                    txtTotal.Text = CDec(.Item("inputTotal")).ToString("#,##0.00")
                    ddlCurrency.SelectedValue = .Item("currencyID")
                    txtCurrencyRate.Text = .Item("currencyRate")
                    txtAmountTHB.Text = CDec(.Item("SubTotal")).ToString("#,##0.00")
                    txtReceiptDate.Text = .Item("ReceiptDate")
                    txtRemark.Text = .Item("Remark")
                    LoadListVendorAddress(CInt(.Item("VendorID")))
                    ddlVendorAddress.SelectedValue = CInt(.Item("VendorBranchID"))
                    txtChequeNo.Text = .Item("ChequeNo")
                    hideStatusID.Value = .Item("StatusID")
                End With

                btnAdd.Text = "Save"
                btnCancel.Text = "Cancel"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateInitial", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: CheckQueryString
        '	Discription	    : Check query string
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub CheckQueryString()
            Try
                btnCancel.Text = "Clear"
                ' check query string
                If objUtility.GetQueryString(strInsResult) = "True" Then
                    ' add row to datatable
                    SetDataToControl()
                    If IsDuplication("Add") Then Exit Sub
                    AddDataToTable()
                    hideAccountingID.Value = Nothing
                    If txtRemark.Text <> "Do not clear field" Then
                        ClearControl()
                    End If
                ElseIf objUtility.GetQueryString(strUpdResult) = "True" Then
                    ' update row of datatable with control's data
                    SetDataToControl()
                    If IsDuplication("Edit") Then Exit Sub
                    UpdateTable()
                    hideAccountingID.Value = Nothing
                    If txtRemark.Text <> "Do not clear field" Then
                        ClearControl()
                    End If
                ElseIf objUtility.GetQueryString(strDelResult) = "True" Then
                    ' remove row from data table 
                    DeleteRow()
                ElseIf objUtility.GetQueryString(strAppResult) = "True" Then
                    ' insert Payment 
                    InsertPayment()
                ElseIf objUtility.GetQueryString("Mode") = "Edit" Then
                    hideAccountingID.Value = objUtility.GetQueryString("accID")
                    ' update row of datatable with control's data
                    If IsDuplication("Search") Then
                        hideAccountingID.Value = Nothing
                    Else
                        Dim dtAccSearchByID As New DataTable
                        dtAccSearchByID = New Service.ImpAccountingService().GetAccountingWithID(hideAccountingID.Value)
                        If Not IsNothing(dtAccSearchByID) Then
                            SetDataToControl(dtAccSearchByID)
                        Else
                            objMessage.AlertMessage("Cannot get data from database, error occurred during get data process")
                        End If
                    End If
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckQueryString", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ClearControl
        '	Discription	    : Clear control
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub ClearControl()
            Try
                ' clear control
                If IsNothing(Session("dtInquiry")) Then
                    rbtAccountType.SelectedValue = Nothing
                    ddlVendor.SelectedValue = String.Empty
                    ddlVendorAddress.Items.Clear()
                    txtBank.Text = String.Empty
                    txtAccountName.Text = String.Empty
                    txtAccountNo.Text = String.Empty
                    txtChequeNo.Text = String.Empty
                    ddlVat.SelectedValue = String.Empty
                    ddlWT.SelectedValue = String.Empty
                Else
                    rbtAccountType.SelectedValue = CInt(CType(Session("dtInquiry"), DataTable).Rows(0).Item("AccountType"))
                    ddlVendor.SelectedValue = CInt(CType(Session("dtInquiry"), DataTable).Rows(0).Item("VendorID"))
                    ddlVendorAddress.SelectedValue = CInt(CType(Session("dtInquiry"), DataTable).Rows(0).Item("VendorBranchID"))
                    txtBank.Text = CStr(CType(Session("dtInquiry"), DataTable).Rows(0).Item("Bank"))
                    txtAccountName.Text = CStr(CType(Session("dtInquiry"), DataTable).Rows(0).Item("AccountName"))
                    txtAccountNo.Text = CStr(CType(Session("dtInquiry"), DataTable).Rows(0).Item("AccountNo"))
                    txtChequeNo.Text = CStr(CType(Session("dtInquiry"), DataTable).Rows(0).Item("ChequeNo"))
                    ddlVat.SelectedValue = CInt(CType(Session("dtInquiry"), DataTable).Rows(0).Item("VatID"))
                    ddlWT.SelectedValue = CInt(CType(Session("dtInquiry"), DataTable).Rows(0).Item("WTID"))
                End If
                txtJobOrder.Text = String.Empty
                hideAccountingID.Value = Nothing
                hideStatusID.Value = Nothing
                txtVatAmount.Text = String.Empty
                txtWTAmount.Text = String.Empty
                ddlItemExpense.SelectedValue = String.Empty
                txtTotal.Text = String.Empty
                ddlCurrency.SelectedValue = ddlCurrency.Items(1).Value
                txtCurrencyRate.Text = CDec(1).ToString("#,##0.00")
                txtAmountTHB.Text = String.Empty
                txtReceiptDate.Text = String.Empty
                txtRemark.Text = String.Empty
                btnAdd.Text = "Save"
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: UpdateTable
        '	Discription	    : Update control
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub UpdateTable()
            Try
                ' variable table object
                Dim dt As DataTable = Session("dtInquiry")
                ' variable row object
                Dim row() As DataRow = dt.Select(" Index = " & Session("intIndex"))
                Dim intRowIndex As Integer = dt.Rows.IndexOf(row(0))

                ' update data from control
                With dt.Rows(intRowIndex)
                    .Item("accID") = hideAccountingID.Value
                    .Item("AccountType") = rbtAccountType.SelectedValue
                    .Item("AccountTypeName") = rbtAccountType.SelectedItem.ToString
                    .Item("JobOrder") = txtJobOrder.Text.Trim
                    .Item("VendorID") = ddlVendor.SelectedValue
                    .Item("VendorName") = ddlVendor.SelectedItem.ToString.Trim
                    .Item("VendorBranchID") = ddlVendorAddress.SelectedValue
                    .Item("BranchName") = ddlVendorAddress.SelectedItem
                    .Item("ChequeNo") = txtChequeNo.Text
                    .Item("VatID") = ddlVat.SelectedValue
                    .Item("VatText") = ddlVat.SelectedItem.ToString.Trim
                    .Item("VatAmount") = txtVatAmount.Text.Trim
                    .Item("Bank") = txtBank.Text.Trim
                    .Item("WTID") = ddlWT.SelectedValue
                    .Item("WTText") = ddlWT.SelectedItem.ToString.Trim
                    .Item("WTAmount") = txtWTAmount.Text.Trim
                    .Item("AccountName") = txtAccountName.Text.Trim
                    .Item("IEID") = ddlItemExpense.SelectedValue
                    .Item("IEName") = ddlItemExpense.SelectedItem.ToString.Trim
                    .Item("AccountNo") = txtAccountNo.Text.Trim
                    .Item("inputTotal") = txtTotal.Text.Trim
                    .Item("currencyID") = ddlCurrency.SelectedValue
                    .Item("currencyRate") = txtCurrencyRate.Text.Trim
                    .Item("SubTotal") = txtAmountTHB.Text.Trim
                    .Item("ReceiptDate") = txtReceiptDate.Text.Trim
                    ' Chenge format date to dd/MM/yyyy
                    .Item("ReceiptDateShow") = objUtility.String2Date(txtReceiptDate.Text.Trim, _
                                                                      "dd/MM/yyyy").ToString("dd/MMM/yyyy")
                    .Item("Remark") = txtRemark.Text.Trim
                    .Item("StatusID") = hideStatusID.Value
                    .AcceptChanges()
                End With
                Session("dtInquiry") = dt
                DisplayPage(Session("PageNo"), True)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateTable", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: DeleteRow
        '	Discription	    : Delete Row
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub DeleteRow()
            Try
                ' variable table object
                Dim dt As DataTable = Session("dtInquiry")
                ' variable row object
                Dim row() As DataRow = dt.Select(" Index = '" & Session("intIndex") & "'")
                Dim intRowIndex As Integer = dt.Rows.IndexOf(row(0))

                ' delete data from control
                dt.Rows(intRowIndex).Delete()
                dt.AcceptChanges()

                ' set session datatable
                Session("dtInquiry") = dt
                ' display active data
                DisplayPage(Session("PageNo"), True)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteRow", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ViewDetails
        '	Discription	    : View Row
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub ViewDetails()
            Try
                ' variable table object
                Dim dt As DataTable = Session("dtInquiry")
                ' variable row object
                Dim rows() As DataRow = dt.Select(" Index = '" & Session("intIndex") & "'")
                ' get data to row
                Dim row As DataRow = rows(0)

                ' set data to session
                Session("rowDetails") = row

                objMessage.ShowPagePopup("KTAC02_Detail.aspx?Type=Payment", 500, 500)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteRow", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: PrepareRptVoucher
        '	Discription	    : Prepare data table for report
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-08-2013
        '	Update User	    : 
        '	Update Date	    :
        '*************************************************************/
        Private Sub PrepareRptVoucher()
            Try
                Dim objVoucher As New Service.ImpAcc_PaymentService
                Dim dtVoucher As New DataTable

                'Modify At 2014/09/29 By Rawikarn KateKeaw
                'Dim dtWTRpt As New DataTable
                'Dim DuplicatedtTWRpt As New DataTable
                'Dim firstRow As DataRow = dtWTRpt.Select("select * from dtWTRpt").FirstOrDefault()
                'Dim dr As DataRow
                'Dim dtReport As New DataTable


                Session("dtReport") = Session("dtInquiry")
                Session("dtVoucherRpt") = objVoucher.GetDataForVoucherReport(Session("voucherList"))
                Session("dtWTRpt") = objVoucher.GetDataForWTReport(Session("voucherList"))

                'dtWTRpt = objVoucher.GetDataForWTReport(Session("voucherList"))

                'If Not firstRow Is Nothing Then
                '    dr = dtReport.NewRow
                '    With dr
                '        dr.Item("vendor_name") = dtWTRpt.Rows(0).Item("vendor_name").ToString
                '        dr.Item("vendor_address") = dtWTRpt.Rows(0).Item("vendor_address").ToString
                '        dr.Item("vendor_type2_no") = dtWTRpt.Rows(0).Item("vendor_type2_no").ToString
                '        dr.Item("account_date") = dtWTRpt.Rows(0).Item("account_date").ToString
                '        dr.Item("subtotal") = dtWTRpt.Rows(0).Item("subtotal").ToString
                '        dr.Item("wt_amount") = dtWTRpt.Rows(0).Item("wt_amount").ToString
                '    End With
                '    dtReport.Rows.Add(dr)
                'End If

                'DuplicatedtTWRpt = dtReport

                'Session("dtWTRpt") = DuplicatedtTWRpt
            Catch ex As Exception
                ' Write error log 
                objLog.ErrorLog("PrepareRptVoucher", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: ShowReport
        '	Discription	    : Show multi report
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub ShowReport(ByVal strPath() As String, ByVal urlOldPage As String)
            Try
                Dim Page As New UI.Page
                Dim sb As New System.Text.StringBuilder()
                'Gets the executing web page 
                Page = HttpContext.Current.CurrentHandler
                sb.Append("<script type = 'text/javascript'>")
                sb.Append("window.onload = function OpenPopup() {")
                sb.Append("alert('" & objMessage.GetXMLMessage("KTAC_03_011") & "');")
                For Each strTmpPath As String In strPath
                    If strTmpPath <> Nothing Then
                        sb.Append("window.open('")
                        sb.Append(strTmpPath)
                        sb.Append("', '_blank', 'width=800, height=600, toolbar=no, location=no")
                        sb.Append(", directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes');")
                    End If
                Next
                sb.Append("window.location = '" & urlOldPage & "';")
                sb.Append("return false;")
                sb.Append("};</script>")
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "OpenPopup", sb.ToString())
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("PrepareRptVoucher", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: InsertPayment
        '	Discription	    : Insert Payment
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub InsertPayment()
            Try
                ' Account service object
                Dim objAccountSer As New Service.ImpAccountingService
                Dim objAccPaymentSer As New Service.ImpAcc_PaymentService
                Dim strPathArray(2) As String

                ' Call function InsertPayment
                If objAccountSer.InsertIncome(Session("dtInquiry")) Then
                    ' case success alert msg KTAC_03_011 and refresh new page
                    PrepareRptVoucher()
                    strPathArray(0) = "../Report/ReportViewer.aspx?ReportName=KTAC&Page=KTAC03"
                    strPathArray(1) = "../Report/ReportViewer.aspx?ReportName=KTAC_Voucher&Page=KTAC03"
                    strPathArray(2) = objAccPaymentSer.ExcelPaymentWTReport(Session("dtWTRpt"), "Payment")
                    'strPathArray(3) = objAccPaymentSer.ExcelPaymentWTReportV2(Session("dtWTRpV2t"), "Payment")
                    ShowReport(strPathArray, "KTAC03.aspx?New=True")
                    objValidate.DeleteReportFile()
                Else
                    ' case failure alert msg KTAC_03_013
                    objMessage.AlertMessage("", "KTAC_03_013")
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPayment", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

        '/**************************************************************
        '	Function name	: SetValidateErrorMessage
        '	Discription	    : Set Validate Error Message
        '	Return Value	: nothing
        '	Create User	    : Wasan D.
        '	Create Date	    : 31-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub SetValidateErrorMessage()
            Try
                reqInputVat.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_004")
                reqInputWT.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_005")
                reqReceiptDate.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_008")
                reqDateInvalid.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_021")
                reqSubTotal.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_006")
                reqSameVendor.ErrorMessage = objMessage.GetXMLMessage("KTAC_03_017")
                reqSameAccountType.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_022")
                reqVendorAddress.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_023")
                reqSameBank.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_024")
                reqAccountName.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_025")
                reqAccountNo.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_026")
                reqSameChequeNo.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_029")
                'reqVat.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_027")
                reqSameWT.ErrorMessage = objMessage.GetXMLMessage("KTAC_02_028")
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetValidateErrorMessage", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub
#End Region

#Region "WebMethod"
        '/**************************************************************
        '	Function name	: IsExistJobOrder
        '	Discription	    : Check exist Job Order
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        <WebMethod()> _
       Public Shared Function IsExistJobOrder( _
            ByVal strJobOrder As String _
        ) As Boolean
            Dim objLog As New Common.Logs.Log
            Try
                Dim objAccountSer As New Service.ImpAccountingService
                Return objAccountSer.IsExistJobOrder(strJobOrder)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsExistJobOrder", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsExistJobOrder
        '	Discription	    : Check exist Job Order
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        <WebMethod()> _
       Public Shared Function CheckSameVendor( _
            ByVal strValueID As String, _
            ByVal strColumnName As String _
        ) As Boolean
            Dim objLog As New Common.Logs.Log
            Try
                Dim dt As DataTable = HttpContext.Current.Session("dtInquiry")
                Dim intMode As Integer
                If HttpContext.Current.Session("Action") = Action.Update Then
                    intMode = 1
                Else
                    intMode = 0
                End If

                If Not IsNothing(dt) AndAlso dt.Rows.Count > intMode Then
                    If strValueID = dt.Rows(0).Item(strColumnName) Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckSameVendor", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsValidDate
        '	Discription	    : Check valid date format
        '	Return Value	: Boolean
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        <WebMethod()> _
        Public Shared Function IsValidDate( _
            ByVal strDate As String, _
            ByVal name As String _
        ) As Boolean

            Dim specificCulture As System.Globalization.CultureInfo = CultureInfo.CreateSpecificCulture(name)
            Dim valid As Boolean = False
            Dim parsedDate As Date

            If (String.IsNullOrEmpty(strDate)) Then
                Return False
            End If

            valid = Date.TryParse(strDate, specificCulture, DateTimeStyles.None, parsedDate)

            Return valid
        End Function
#End Region


    End Class
End Namespace