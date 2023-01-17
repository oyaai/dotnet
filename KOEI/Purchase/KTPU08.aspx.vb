Imports System.Data
Imports System.Web.Configuration
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Net

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : KTPU08
'	Class Name		    : KTPU08
'	Class Discription	: Insert/Update Invoice 
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 18-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTPU08
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    Private searchID As String = ""
    Private searchPo_header_id As String = ""
    Private pagedData As New PagedDataSource
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private Const constCreate As String = "Create"
    Private Const constUpdate As String = "Update"
    Private itemConfirm As String = ""
    Private reportName As String = ""
    Private ReportReportPath As String = "../Report/RptFileSave/"
    Private ExportFileName As String
    Private CommonValidation As New Validations.CommonValidation
    Private autoApproveAccount As String = WebConfigurationManager.AppSettings("AutoApproveAccount")
    Private objValidate As New Common.Validations.Validation
    Private objDate As New Common.Utilities.Utility

    'connect with service
    Private objCheque_PurchaseService As New Service.ImpCheque_PurchaseService

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
            objLog.StartLog("KTPU08", Session("UserName"))
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
                ' case not post back then call function initialpage
                InitialPage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnCreate_Click
    '	Discription	    : add data into purchase_header,purchase_detail table
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Try
            'Check input
            If CheckError() = False Then
                Exit Sub
            End If

            If CheckHeader() = False Then
                Exit Sub
            End If

            If chkBankRate() = False Then
                Exit Sub
            End If

            'Check input data in gridview
            If checkGridview() = False Then
                Exit Sub
            End If

            'Check input data in gridview
            If checkCheckbox() = False Then
                Exit Sub
            End If

            'prepare data before insert into accounting
            SetAccountDetail()

            'Keep data of each record that is already checked
            GetConfirmID()

            SetScreenToSession()

            If Session("mode") = "Add" Then 'case insert
                ' case not used then confirm message to delete
                objMessage.ConfirmMessage("KTPU08", constCreate, objMessage.GetXMLMessage("KTPU_08_001"))
            Else
                ' case not used then confirm message to delete
                objMessage.ConfirmMessage("KTPU08", constCreate, objMessage.GetXMLMessage("KTPU_08_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnCreate_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    ' Function name  : checkCheckbox
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-05-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function checkCheckbox() As Boolean
        checkCheckbox = False
        Try
            'check item checkbox
            Dim chkCheckBox As Long = 0

            For Each item As RepeaterItem In rptInquery.Items
                Dim chkBox As CheckBox = item.FindControl("chkCheque")

                If chkBox.Checked = True Then
                    chkCheckBox = chkCheckBox + 1
                End If
            Next

            If chkCheckBox = 0 Then
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_009"))
                Exit Function
            End If

            checkCheckbox = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("checkCheckbox", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
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
            If Session("mode") = "Add" Then 'case insert
                'check vendor
                If ddlVendor.SelectedIndex = 0 Then
                    ' show message box
                    objMessage.AlertMessage("Please select vendor name")
                    Exit Function
                End If
            End If
            'check end date
            'If txtPaymentDate.Text.Trim <> "" Then
            '    If objValidate.IsDate(txtPaymentDate.Text.Trim) = False Then
            '        ' show message box
            '        objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
            '        Exit Function
            '    End If
            'End If

            'check end date
            If txtChequeDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtChequeDate.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
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
    ' Function name   : CheckHeader
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-05-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function CheckHeader() As Boolean
        CheckHeader = False
        Try
            If rdoCurrentAc.Checked = False And rdoSavingAc.Checked = False And rdoCash.Checked = False Then
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_011"))
                Exit Function
            End If

            If rdoCurrentAc.Checked = True Then
                If txtChequeNo.Text.Trim = "" Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_012"))
                    txtChequeNo.Focus()
                    Exit Function
                End If
            End If

            If rdoSavingAc.Checked = True Then
                If txtBankName.Text.Trim = "" Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_013"))
                    txtBankName.Focus()
                    Exit Function
                End If
                If txtAccountNo.Text.Trim = "" Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_014"))
                    txtAccountNo.Focus()
                    Exit Function
                End If
                If txtAccountName.Text.Trim = "" Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_015"))
                    txtAccountName.Focus()
                    Exit Function
                End If
            End If

            CheckHeader = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckHeader", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    ' Function name   : chkBankRate
    ' Discription     : check input bank Rate 
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 19-09-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function chkBankRate() As Boolean
        chkBankRate = False
        Try
            'check txtBankRate
            If txtBankRate.Text.Trim <> "" AndAlso IsNumeric(txtBankRate.Text.Trim) = False Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                Exit Function
            End If

            'If CDbl(txtBankRate.Text.Trim) < 0 Then
            '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
            '    Exit Function
            'End If
            chkBankRate = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("chkBankRate", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    ' Function name   : CalBankRate
    ' Discription     : check input bank Rate and calculate amount
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 19-09-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Public Function CalBankRate() As Boolean
        Try
            'check input
            If chkBankRate() = False Then
                Exit Function
            End If

            'calculate
            If CalRate() = False Then
                Exit Function
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CalBankRate", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    ' Function name   : checkGridview
    ' Discription     : check input qty and amount in gridview
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-05-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function CalRate() As Boolean
        CalRate = False

        Try
            Dim sumDelAmt As Double = 0
            Dim dt As New DataTable
            Dim sumBankRate As Double = 0
            Dim sumVat As Double = 0
            Dim sumWT As Double = 0

            dt = Session("dtGetAccounting_Detail")
            'dt = Session("dtInsAcc")

            For Each item As RepeaterItem In rptInquery.Items

                Dim boxBankRate As TextBox = item.FindControl("txtBank_rate")
                Dim boxVat As TextBox = item.FindControl("txtVat_amount")
                Dim boxWT As TextBox = item.FindControl("txtWt_amount")

                If boxBankRate.Text.Trim = "" Then
                    boxBankRate.Text = "0"
                End If
                If boxVat.Text.Trim = "" Then
                    boxVat.Text = "0"
                End If
                If boxWT.Text.Trim = "" Then
                    boxWT.Text = "0"
                End If

                'check BankRate
                If chkBankRate() = False Then
                    Exit Function
                End If

                'check Qty
                If IsNumeric(boxBankRate.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxBankRate.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                'check vat
                If IsNumeric(boxVat.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxVat.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                'check wt
                If IsNumeric(boxWT.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxWT.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                Dim dblBankRate As Double = CDbl(txtBankRate.Text)
                Dim hsub_total As Double = dt.Rows(item.ItemIndex)("hsub_total").ToString()
                Dim hvat_amount As Double = dt.Rows(item.ItemIndex)("hvat_amount").ToString()
                Dim hwt_amount As Double = dt.Rows(item.ItemIndex)("hwt_amount").ToString()

                boxBankRate.Text = Format(hsub_total * dblBankRate, "#,##0.00")
                boxVat.Text = Format(CDbl(hvat_amount * dblBankRate), "#,##0.00")
                boxWT.Text = Format(CDbl(hwt_amount * dblBankRate), "#,##0.00")

                sumBankRate = sumBankRate + hsub_total * dblBankRate
                sumVat = sumVat + (CDbl(hvat_amount * dblBankRate))
                sumWT = sumWT + (CDbl(hwt_amount * dblBankRate))

            Next

            lblSumBankRate.Text = Format(Convert.ToDouble(sumBankRate), "#,##0.00")
            lblSumVat.Text = Format(Convert.ToDouble(sumVat), "#,##0.00")
            lblSumWT.Text = Format(Convert.ToDouble(sumWT), "#,##0.00")

            CalRate = True
        Catch ex As Exception
            CalRate = False
            ' write error log
            objLog.ErrorLog("CalBankate", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    ' Function name   : checkGridview
    ' Discription     : check input qty and amount in gridview
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-05-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function checkGridview() As Boolean
        checkGridview = False

        Try
            Dim sumDelAmt As Double = 0
            Dim dt As New DataTable
            Dim sumBankRate As Double = 0
            Dim sumVat As Double = 0
            Dim sumWT As Double = 0

            dt = Session("dtGetAccounting_Detail")
            'dt = Session("dtInsAcc")

            For Each item As RepeaterItem In rptInquery.Items

                Dim boxBankRate As TextBox = item.FindControl("txtBank_rate")
                Dim boxVat As TextBox = item.FindControl("txtVat_amount")
                Dim boxWT As TextBox = item.FindControl("txtWt_amount")

                If boxBankRate.Text.Trim = "" Then
                    boxBankRate.Text = "0"
                End If
                If boxVat.Text.Trim = "" Then
                    boxVat.Text = "0"
                End If
                If boxWT.Text.Trim = "" Then
                    boxWT.Text = "0"
                End If

                'check BankRate
                If chkBankRate() = False Then
                    Exit Function
                End If

                'check Qty
                If IsNumeric(boxBankRate.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxBankRate.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                'check vat
                If IsNumeric(boxVat.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxVat.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                'check wt
                If IsNumeric(boxWT.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                If CDbl(boxWT.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_008"))
                    Exit Function
                End If

                Dim dblBankRate As Double = boxBankRate.Text
                Dim vat_percent As Double = dt.Rows(item.ItemIndex)("vat_percent").ToString()
                Dim wt_percent As Double = dt.Rows(item.ItemIndex)("wt_percent").ToString()

                boxBankRate.Text = Format(dblBankRate, "#,##0.00")
                boxVat.Text = Format(CDbl(dblBankRate * vat_percent / 100), "#,##0.00")
                boxWT.Text = Format(CDbl(dblBankRate * wt_percent / 100), "#,##0.00")

                sumBankRate = sumBankRate + dblBankRate
                sumVat = sumVat + (dblBankRate * vat_percent / 100)
                sumWT = sumWT + (dblBankRate * wt_percent / 100)

            Next

            lblSumBankRate.Text = Format(Convert.ToDouble(sumBankRate), "#,##0.00")
            lblSumVat.Text = Format(Convert.ToDouble(sumVat), "#,##0.00")
            lblSumWT.Text = Format(Convert.ToDouble(sumWT), "#,##0.00")

            checkGridview = True
        Catch ex As Exception
            checkGridview = False
            ' write error log
            objLog.ErrorLog("checkGridview", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
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
            Dim btnDetail As New LinkButton
            Dim chkBox As New CheckBox

            chkBox = DirectCast(e.Item.FindControl("chkCheque"), CheckBox)

            If DataBinder.Eval(e.Item.DataItem, "chkCheque") = "True" Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If

            'Set id to hashtable (for case link to detail page)
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Invoice_PurchaseDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            Session("mode") = Nothing
            clearSession()
            Response.Redirect("KTPU07.aspx?New=Insert")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnBackEdit
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBackEdit_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBackEdit.Click
        Try
            Session("mode") = Nothing
            clearSession()
            Response.Redirect("KTPU07.aspx?New=Insert")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBackEdit", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnClear.Click
        Try
            If Session("mode") = "Add" Then 'Case add
                If Session("search1") = "search" Then
                    GetSearchData()
                End If
            ElseIf Session("mode") = "Edit" Then 'Case Modify
                If Session("search1") = "search" Then
                    GetEditData()
                End If
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 18-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            GetSearchData()
            Session("search1") = "search"
            'setTextToSession()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: txtBank_rate_TextChanged
    '	Discription	    : calculate bank rate ,vat,wt
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub txtBank_rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If checkGridview() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("txtBank_rate_TextChanged", ex.Message.ToString, Session("UserName"))
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
    '	Function name	: SetAccountDetail
    '	Discription	    : Keep data of each record from gridview
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetAccountDetail()
        Try
            Dim txtBank_rate As TextBox
            Dim txtVat_amount As TextBox
            Dim txtWt_amount As TextBox
            Dim dtInsAcc As New DataTable
            Dim row As DataRow
            Dim dtGetAccounting_Detail As New DataTable
            Dim i As Integer = 0
            Dim chkBox As CheckBox
            Dim dtDate As String

            dtGetAccounting_Detail = Session("dtGetAccounting_Detail")

            With dtInsAcc
                .Columns.Add("id")
                .Columns.Add("chkCheque")
                .Columns.Add("vendor_name")
                .Columns.Add("vendor_type")
                .Columns.Add("cheque_no")
                .Columns.Add("cheque_date")
                .Columns.Add("vat_percent")
                .Columns.Add("wt_percent")
                .Columns.Add("voucher_no")
                .Columns.Add("invoice_no")
                .Columns.Add("sub_total") 'bank rate
                .Columns.Add("amount_bank")
                .Columns.Add("vat_name")
                .Columns.Add("vat_amount") 'vat baht
                .Columns.Add("wt_name")
                .Columns.Add("wt_amount") 'wt baht
                .Columns.Add("payment_date")
                .Columns.Add("payment_from")
                'start add new 2013/09/19------
                .Columns.Add("hsub_total")
                .Columns.Add("hvat_amount")
                .Columns.Add("hwt_amount")
                .Columns.Add("account_type")
                .Columns.Add("bank")
                .Columns.Add("account_no")
                .Columns.Add("account_name")
                .Columns.Add("vat_amt")
                .Columns.Add("wt_amt")
                'end add new 2013/09/19------

                For Each item As RepeaterItem In rptInquery.Items
                    row = .NewRow

                    txtBank_rate = item.FindControl("txtBank_rate")
                    txtVat_amount = item.FindControl("txtVat_amount")
                    txtWt_amount = item.FindControl("txtWt_amount")

                    row("id") = dtGetAccounting_Detail.Rows(i)("id").ToString()
                    chkBox = item.FindControl("chkCheque")
                    If chkBox.Checked = True Then
                        row("chkCheque") = "True"
                    Else
                        row("chkCheque") = "False"
                    End If
                    row("vendor_name") = dtGetAccounting_Detail.Rows(i)("vendor_name").ToString()
                    row("vendor_type") = dtGetAccounting_Detail.Rows(i)("vendor_type").ToString()
                    row("cheque_no") = txtChequeNo.Text.Trim
                    dtDate = txtChequeDate.Text.Trim.Substring(6, 4) & txtChequeDate.Text.Trim.Substring(3, 2) & txtChequeDate.Text.Trim.Substring(0, 2)
                    row("cheque_date") = dtDate
                    row("vat_percent") = dtGetAccounting_Detail.Rows(i)("vat_percent").ToString()
                    row("wt_percent") = dtGetAccounting_Detail.Rows(i)("wt_percent").ToString()
                    row("voucher_no") = dtGetAccounting_Detail.Rows(i)("voucher_no").ToString()
                    row("invoice_no") = dtGetAccounting_Detail.Rows(i)("invoice_no").ToString()
                    row("sub_total") = dtGetAccounting_Detail.Rows(i)("sub_total").ToString()
                    row("amount_bank") = Format(Convert.ToDouble(txtBank_rate.Text.Trim), "###0.00")
                    row("vat_name") = dtGetAccounting_Detail.Rows(i)("vat_name").ToString()
                    row("vat_amount") = Format(Convert.ToDouble(txtVat_amount.Text.Trim), "###0.00")
                    row("wt_name") = dtGetAccounting_Detail.Rows(i)("wt_name").ToString()
                    row("wt_amount") = Format(Convert.ToDouble(txtWt_amount.Text.Trim), "###0.00")
                    row("payment_date") = dtGetAccounting_Detail.Rows(i)("payment_date").ToString()
                    row("payment_from") = dtGetAccounting_Detail.Rows(i)("payment_from").ToString()
                    'start add new 2013/09/19------
                    row("hsub_total") = dtGetAccounting_Detail.Rows(i)("hsub_total").ToString()
                    row("hvat_amount") = dtGetAccounting_Detail.Rows(i)("hvat_amount").ToString()
                    row("hwt_amount") = dtGetAccounting_Detail.Rows(i)("hwt_amount").ToString()
                    row("vat_amt") = dtGetAccounting_Detail.Rows(i)("vat_amount").ToString()
                    row("wt_amt") = dtGetAccounting_Detail.Rows(i)("wt_amount").ToString()

                    Dim account_type As String = ""
                    If rdoCurrentAc.Checked = True Then
                        account_type = 1
                    ElseIf rdoSavingAc.Checked = True Then
                        account_type = 2
                    ElseIf rdoCash.Checked = True Then
                        account_type = 3
                    End If

                    row("account_type") = account_type
                    row("bank") = txtBankName.Text.Trim
                    row("account_no") = txtAccountNo.Text.Trim
                    row("account_name") = txtAccountName.Text.Trim
                    'end add new 2013/09/19------

                    Session("sumScheduleRate") = lblSumScheduleRate.Text
                    Session("sumBankRate") = lblSumBankRate.Text
                    Session("sumVat") = lblSumVat.Text
                    Session("sumWT") = lblSumWT.Text

                    ' add data row to table
                    .Rows.Add(row)

                    i += 1
                Next
            End With

            'Set itemConfirm into session
            Session("dtInsAcc") = dtInsAcc
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetAccountDetail", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: getListPaymentYear
    '	Discription	    : getListPaymentYear function
    '	Return Value	: nothing
    '	Create User	    : Ping
    '	Create Date	    : 22-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub getListPaymentYear()
        Dim conn As New Common.DBConnection.MySQLAccess
        Dim cmdText As String = "select '' YYYY from dual union " & _
            "select left(pay_head.payment_date,4) from accounting acc join payment_header pay_head on acc.ref_id=pay_head.id " & _
            "where acc.type in (1,3) and acc.status_id=1 and pay_head.status_id<>6 and ifnull(acc.cheque_date,'')='' order by YYYY;"
        Try
            Dim ds As System.Data.DataSet = conn.ExecuteDataSet(cmdText)
            ddlYear.DataSource = ds.Tables(0)
            ddlYear.DataValueField = "YYYY"
            ddlYear.DataTextField = "YYYY"
            ddlYear.DataBind()
        Catch ex As Exception
            objLog.ErrorLog("getListPaymentYear", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        Finally
            If Not IsNothing(conn) Then conn = Nothing
        End Try
    End Sub

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
            Dim dtDate As String = ""
            Dim arrApproveId() As String
            Dim chkAccApp As Boolean = False

            ' call function set Vendor dropdownlist
            LoadListVendor()
            getListPaymentYear()

            If IsNothing(Request.QueryString("Mode")) = False Then
                Session("mode") = Request.QueryString("Mode")
            End If

            CheckPermission()

            'Check exist Account Approve
            If Session("AccountNextApprove") Is Nothing Or _
                String.IsNullOrEmpty(Session("AccountNextApprove")) Or _
                Session("AccountNextApprove").ToString() = "0" Then
                arrApproveId = Split(autoApproveAccount, ";")
                For i As Integer = 0 To arrApproveId.Length - 1
                    If Session("UserName") = arrApproveId(i) Then
                        chkAccApp = True
                        Exit For
                    End If
                Next

                If chkAccApp = True Then
                    Session("account_next_approve") = Session("UserID")
                Else
                    btnCreate.Enabled = False
                End If
            Else
                Session("account_next_approve") = Session("AccountNextApprove")
            End If

            'check exit data in accounting table ,update data to accounting 
            If objUtility.GetQueryString(constCreate) = "True" Then
                If insertProcess() = True Then
                    Exit Sub
                Else
                    GoTo setScreen
                End If
            End If

            'update data to accounting
            If objUtility.GetQueryString(constUpdate) = "True" Then
                If ExecuteProcess() = True Then
                    Exit Sub
                Else
                    GoTo setScreen
                End If
            End If

            If Session("mode") = "Add" Then 'Case add
                btnCreate.Text = "Create"
                panelSearch.Visible = True
                btnBackEdit.Visible = False
                clearSession()
            ElseIf Session("mode") = "Edit" Then 'Case Modify
                btnCreate.Text = "Update"
                panelSearch.Visible = False

                Session("strId") = Request.QueryString("strId")
                'Session("strChequeNo") = Request.QueryString("strChequeNo")
                ''Session("strChequeDate") = Replace(CDate(Request.QueryString("strChequeDate")).ToString("yyyy/MM/dd"), "/", "")
                'Dim arrPaymentDate() As String = Split(Request.QueryString("strChequeDate"), "/")
                'If UBound(arrPaymentDate) > 0 Then
                '    Session("strChequeDate") = arrPaymentDate(2) & arrPaymentDate(1) & arrPaymentDate(0)
                'End If
                GetEditData()
            End If

setScreen:
            If Session("mode") = "Add" Then 'Case add
                btnCreate.Text = "Create"
                panelSearch.Visible = True
                btnBackEdit.Visible = False
            ElseIf Session("mode") = "Edit" Then 'Case Modify
                btnCreate.Text = "Update"
                panelSearch.Visible = False
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetEditData
    '	Discription	    : Get data to display on screen in case edit
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetEditData()
        Try
            'Get and display detail
            'GetAccounting_Detail(Session("strChequeNo"), Session("strChequeDate"))
            GetAccounting_Detail(Session("strId"), "")
            DisplayHead(Session("dtGetAccounting_Detail"))
            DisplayDetail(Request.QueryString("PageNo"), Session("dtGetAccounting_Detail"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetEditData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetSearchData
    '	Discription	    : Get data to display on screen in case add
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSearchData()
        Try
            Dim dtDate As String = ddlYear.SelectedValue & ddlMonth.SelectedValue
            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            'If txtPaymentDate.Text.Trim <> "" Then
            '    Dim arrPaymentDate() As String = Split(txtPaymentDate.Text.Trim(), "/")
            '    If UBound(arrPaymentDate) > 0 Then
            '        dtDate = arrPaymentDate(2) & arrPaymentDate(1) & arrPaymentDate(0)
            '    End If
            'End If

            'Get and display detail
            GetAccounting_Detail(ddlVendor.SelectedValue, dtDate)
            DisplayHead(Session("dtGetAccounting_Detail"))
            DisplayDetail(Request.QueryString("PageNo"), Session("dtGetAccounting_Detail"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetEditData", ex.Message.ToString, Session("UserName"))
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
                Dim intItemID As String = hashItemID(i)
                Dim chkBox As CheckBox = item.FindControl("chkCheque")
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
    '	Function name	: DisplayHead
    '	Discription	    : Display header
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayHead(ByVal dtPO As DataTable)
        Try
            ' check record for display
            If Not IsNothing(dtPO) AndAlso dtPO.Rows.Count > 0 Then
                'Display data on detail screen
                If Session("mode") = "Add" Then
                    If ddlVendor.SelectedIndex = 0 Then
                        lblVendorName.Text = ""
                        lblVendorType.Text = ""
                    Else
                        lblVendorName.Text = dtPO.Rows(0)("vendor_name").ToString()
                        lblVendorType.Text = dtPO.Rows(0)("vendor_type").ToString()
                    End If
                Else
                    lblVendorName.Text = dtPO.Rows(0)("vendor_name").ToString()
                    lblVendorType.Text = dtPO.Rows(0)("vendor_type").ToString()
                End If
                
                txtChequeNo.Text = dtPO.Rows(0)("cheque_no").ToString()
                txtChequeDate.Text = dtPO.Rows(0)("cheque_date").ToString()

                'start add by Pranitda S. 2013/09/20
                rdoCurrentAc.Checked = (dtPO.Rows(0)("account_type").ToString() = "1")
                rdoSavingAc.Checked = (dtPO.Rows(0)("account_type").ToString() = "2")
                rdoCash.Checked = (dtPO.Rows(0)("account_type").ToString() = "3")
                txtBankName.Text = dtPO.Rows(0)("bank").ToString()
                txtAccountNo.Text = dtPO.Rows(0)("account_no").ToString()
                txtAccountName.Text = dtPO.Rows(0)("account_name").ToString()
                'end add by Pranitda S. 2013/09/20
                txtBankRate.Text = ""
                'set data into sesstion
                SetScreenToSession()
            Else
                Session("lblTotalAmount.Text") = Nothing

                'clear session
                clearSession()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayHead", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DisplayDetail
    '	Discription	    : Display detail
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 18-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayDetail(ByVal intPageNo As Integer, ByVal dtPO As DataTable)
        Try
            'Dim dtGetPO_Detail_Insert As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' check record for display
            If Not IsNothing(dtPO) AndAlso dtPO.Rows.Count > 0 Then
                ' get page source for repeater
                rptInquery.DataSource = dtPO
                rptInquery.DataBind()

                ' call fucntion set description

                lblSumScheduleRate.Text = Format(CDbl(Session("sumScheduleRate")), "#,##0.00")
                lblSumBankRate.Text = Format(CDbl(Session("sumBankRate")), "#,##0.00")
                lblSumVat.Text = Format(CDbl(Session("sumVat")), "#,##0.00")
                lblSumWT.Text = Format(CDbl(Session("sumWT")), "#,##0.00")
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()

                lblSumScheduleRate.Text = "0.00"
                lblSumBankRate.Text = "0.00"
                lblSumVat.Text = "0.00"
                lblSumWT.Text = "0.00"
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayDetail", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 21-06-2013
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
            'lblDescription.Text = "Showing " & intStart.ToString & " to " & intEnd.ToString & _
            '" of " & intAllRecs.ToString & " entries"
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ShowDescription", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchDetailData
    '	Discription	    : Search invoice detail  
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 18-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetAccounting_Detail(ByVal id As String, ByVal dtDate As String)
        Try
            ' table object keep value from item service
            Dim dtGetAccounting_Detail As New DataTable
            Dim sumScheduleRate As Double
            Dim sumBankRate As Double
            Dim sumVat As Double
            Dim sumWT As Double

            'call function GetItemList from ItemService
            dtGetAccounting_Detail = objCheque_PurchaseService.GetAccounting_Detail( _
                                                                                    id, _
                                                                                    dtDate, _
                                                                                    Session("mode"), _
                                                                                    sumScheduleRate, _
                                                                                    sumBankRate, _
                                                                                    sumVat, _
                                                                                    sumWT _
                                                                                    )

            ' set table object to session
            Session("sumScheduleRate") = sumScheduleRate
            Session("sumBankRate") = sumBankRate
            Session("sumVat") = sumVat
            Session("sumWT") = sumWT
            Session("dtGetAccounting_Detail") = dtGetAccounting_Detail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetAccounting_Detail", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
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
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(10)

            ' set permission 
            btnCreate.Enabled = objAction.actCreate
            If Session("mode") = "Edit" Then
                btnSearch.Enabled = False
            Else
                btnSearch.Enabled = objAction.actList
            End If


            ' set action permission to session
            Session("actList") = objAction.actList
            Session("actCreate") = objAction.actCreate

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: insertProcess
    '	Discription	    : upate data into accounting
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function insertProcess() As Boolean
        insertProcess = False
        Try
            'insert data into payment_header,payment_detail
            Dim dtGetPaymentDetail As New DataTable
            Dim cheque_date As String = ""
            Dim cheque_no As String = ""

            cheque_no = Session("txtChequeNo")
            'cheque_date = Replace(CDate(Session("txtChequeDate")).ToString("yyyy/MM/dd"), "/", "")
            Dim arrPaymentDate() As String = Split(Session("txtChequeDate"), "/")
            If UBound(arrPaymentDate) > 0 Then
                cheque_date = arrPaymentDate(2) & arrPaymentDate(1) & arrPaymentDate(0)
            End If

            'check duplicate in case add
            If Session("mode") = "Add" Then 'Case add
                If rdoCurrentAc.Checked AndAlso objCheque_PurchaseService.CheckDupAccounting(cheque_no, cheque_date) = True Then
                    'display confirm message
                    objMessage.ConfirmMessage("KTPU08", constUpdate, objMessage.GetXMLMessage("KTPU_08_007"))
                End If
                If ExecuteProcess() = True Then
                    insertProcess = True
                End If
            Else
                If ExecuteProcess() = True Then
                    insertProcess = True
                End If
            End If

        Catch ex As Exception
            insertProcess = False
            ' write error log
            objLog.ErrorLog("insertProcess", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    '	Function name	: ExecuteProcess
    '	Discription	    : upate data into accounting
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function ExecuteProcess() As Boolean
        ExecuteProcess = False
        Try
            Dim returnInsertAccounting As Boolean
            Dim errorType As String = ""

            'insert accounting
            returnInsertAccounting = objCheque_PurchaseService.UpdateAccounting( _
                                                                                Session("account_next_approve"), _
                                                                                Session("dtInsAcc"), _
                                                                                errorType)

            'insert completed
            If returnInsertAccounting = True Then

                'Get data of pdf report
                GetPurchasePaidReport()
                GetPaymentVoucher()

                'Get data of tax report
                GetTaxReport()
                GetAccountReport()

                'Create report
                reportName = "KTPU03"
                report_KTPU03()
                reportName = "KTPU10"
                report_KTPU10()

                If Session("mode") = "Add" Then 'Case add
                    clearSession()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_002"), Nothing, "KTPU07.aspx?ins_mode=Completed&New=True")
                ElseIf Session("mode") = "Edit" Then 'Case Modify
                    clearSession()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_005"), Nothing, "KTPU07.aspx?ins_mode=Completed&New=True")
                End If
                ExecuteProcess = True
            Else 'insert failed
                SetSessionToScreen()
                'display detail on screen
                DisplayDetail(Request.QueryString("PageNo"), Session("dtInsAcc"))

                If errorType = "2" Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_010"))
                Else
                    If Session("mode") = "Add" Then 'Case add
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_003"))
                    ElseIf Session("mode") = "Edit" Then 'Case Modify
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_006"))
                    End If
                End If

                ExecuteProcess = False
            End If

        Catch ex As Exception
            ExecuteProcess = False
            ' write error log
            objLog.ErrorLog("ExecuteProcess", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Get data for export crystal report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPurchasePaidReport()
        Try
            ' table object keep value from item service
            Dim dtPaymentPaid As New DataTable

            ' call function GetItemList from ItemService
            dtPaymentPaid = objCheque_PurchaseService.GetPurchasePaidReport(Session("itemConfirm"))
            ' set table object to session
            Session("dtPaymentPaid") = dtPaymentPaid
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPurchasePaidReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetPaymentVoucher
    '	Discription	    : Get data for export crystal report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPaymentVoucher()
        Try
            ' table object keep value from item service
            Dim dtPaymentVoucher As New DataTable

            ' call function GetItemList from ItemService
            dtPaymentVoucher = objCheque_PurchaseService.GetPaymentVoucher(Session("itemConfirm"))
            ' set table object to session
            Session("dtPaymentVoucher") = dtPaymentVoucher
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPaymentVoucher", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetTaxReport
    '	Discription	    : Get data for export excel report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetTaxReport()
        Try
            ' table object keep value from item service
            Dim dtTaxReport As New DataTable

            ' call function GetItemList from ItemService
            dtTaxReport = objCheque_PurchaseService.GetTaxReport(Session("itemConfirm"))
            ' set table object to session
            Session("dtTaxReport") = dtTaxReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPaymentVoucher", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetAccountReport
    '	Discription	    : Get data for export excel report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetAccountReport()
        Try
            ' table object keep value from item service
            Dim dtAccountReport As New DataTable

            ' call function GetItemList from ItemService
            dtAccountReport = objCheque_PurchaseService.GetAccountReport(Session("itemConfirm"))
            ' set table object to session
            Session("dtAccountReport") = dtAccountReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPaymentVoucher", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToSession
    '	Discription	    : Set Value To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetScreenToSession()
        Try
            Session("ddlVendor") = ddlVendor.SelectedValue
            'Session("txtPaymentDate") = txtPaymentDate.Text.Trim
            Session("ddlMonth") = ddlMonth.SelectedValue
            Session("ddlYear") = ddlYear.SelectedValue
            Session("lblVendorName") = lblVendorName.Text.Trim
            Session("lblVendorType") = lblVendorType.Text.Trim
            Session("txtChequeDate") = txtChequeDate.Text.Trim
            Session("lblSumScheduleRate") = lblSumScheduleRate.Text.Trim
            Session("lblSumBankRate") = lblSumBankRate.Text.Trim
            Session("lblSumVat") = lblSumVat.Text.Trim
            Session("lblSumWT") = lblSumWT.Text.Trim
            'start add by Pranitda S. 2013/09/20
            Session("txtChequeNo") = If(rdoCurrentAc.Checked, txtChequeNo.Text.Trim, "")
            Session("txtBankName") = If(rdoSavingAc.Checked, txtBankName.Text.Trim, "")
            Session("txtAccountNo") = If(rdoSavingAc.Checked, txtAccountNo.Text.Trim, "")
            Session("txtAccountName") = If(rdoSavingAc.Checked, txtAccountName.Text.Trim, "")
            Session("txtBankRate") = txtBankRate.Text.Trim
            Session("rdoCurrentAc") = rdoCurrentAc.Checked
            Session("rdoSavingAc") = rdoSavingAc.Checked
            Session("rdoCash") = rdoCash.Checked
            'end add by Pranitda S. 2013/09/20
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetScreenToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    ''/**************************************************************
    ''	Function name	: SetValueToSession
    ''	Discription	    : Set Value To Session
    ''	Return Value	: nothing
    ''	Create User	    : Pranitda Sroengklang
    ''	Create Date	    : 01-07-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    Private Sub SetSessionToScreen()
        ddlVendor.SelectedValue = Session("ddlVendor")
        'txtPaymentDate.Text = Session("txtPaymentDate")
        ddlMonth.SelectedValue = Session("ddlMonth")
        ddlYear.SelectedValue = Session("ddlYear")
        lblVendorName.Text = Session("lblVendorName")
        lblVendorType.Text = Session("lblVendorType")
        txtChequeDate.Text = Session("txtChequeDate")
        lblSumScheduleRate.Text = Session("lblSumScheduleRate")
        lblSumBankRate.Text = Session("lblSumBankRate")
        lblSumVat.Text = Session("lblSumVat")
        lblSumWT.Text = Session("lblSumWT")
        'start add by Pranitda S. 2013/09/20
        txtBankRate.Text = Session("txtBankRate")
        rdoCurrentAc.Checked = Session("rdoCurrentAc")
        rdoSavingAc.Checked = Session("rdoSavingAc")
        rdoCash.Checked = Session("rdoCash")
        txtChequeNo.Text = If(rdoCurrentAc.Checked, Session("txtChequeNo"), "")
        txtBankName.Text = If(rdoSavingAc.Checked, Session("txtBankName"), "")
        txtAccountNo.Text = If(rdoSavingAc.Checked, Session("txtAccountNo"), "")
        txtAccountName.Text = If(rdoSavingAc.Checked, Session("txtAccountName"), "")

        'end add by Pranitda S. 2013/09/20
    End Sub
    '/**************************************************************
    '	Function name	: clearSession
    '	Discription	    : clear Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub clearSession()
        Try
            Session("strChequeNo") = Nothing
            Session("strChequeDate") = Nothing
            Session("dtGetAccounting_Detail") = Nothing
            Session("ddlVendor") = Nothing
            'Session("txtPaymentDate") = Nothing
            Session("ddlYear") = Nothing
            Session("ddlMonth") = Nothing
            Session("txtChequeNo") = Nothing
            Session("txtChequeDate") = Nothing
            Session("sumScheduleRate") = Nothing
            Session("sumBankRate") = Nothing
            Session("sumVat") = Nothing
            Session("sumWT") = Nothing
            Session("dtInsAcc") = Nothing
            Session("search1") = Nothing
            'start add by Pranitda S. 2013/09/20
            Session("txtBankName") = Nothing
            Session("txtAccountNo") = Nothing
            Session("txtAccountName") = Nothing
            Session("txtBankRate") = Nothing
            Session("rdoCurrentAc") = Nothing
            Session("rdoSavingAc") = Nothing
            Session("rdoCash") = Nothing
            'end add by Pranitda S. 2013/09/20
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("clearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: LoadListVendor
    '	Discription	    : Load list Vendor function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 03-06-2013
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
            listVendorDto = objVendorSer.GetVendorForList("0")

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlVendor, listVendorDto, "name", "id", True)

            ' set select Vendor from session
            If Not IsNothing(Session("ddlVendor")) And ddlVendor.Items.Count > 0 Then
                ddlVendor.SelectedValue = Session("ddlVendor")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region


#Region "Report"
    '/**************************************************************
    '	Function name	: report_KTPU04
    '	Discription	    : Export Income/Payment Paid
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang.
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU03()
        Dim ds As New DataSet("KTPU03DataSet") 'dataset table name
        Dim dtReport As New DataTable
        'Dim report As New ReportDocument
        Dim objInvoice_PurchaseService As New Service.ImpInvoice_PurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search invoice (KTPU03.aspx) into data table
            'dtReport = objInvoice_PurchaseService.GetTableReport(Session("dtPaymentPaid"))
            dtReport = Session("dtPaymentPaid")
            dtReport.TableName = "KTPU03DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "PurchasePaidReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            Session("FileNamePaidReport") = FileName
            WriteFile(MS, FileName)
            'Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU03", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTPU10
    '	Discription	    : Export Income/Payment Paid
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang.
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU10()
        Dim ds As New DataSet("KTPU10DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objInvoice_PurchaseService As New Service.ImpInvoice_PurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search invoice (KTPU03.aspx) into data table
            dtReport = Session("dtPaymentVoucher")
            dtReport.TableName = "KTPU10DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "PaymentVoucherReport"

            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"

            Session("FileNameVoucherReport") = FileName
            WriteFile(MS, FileName)
            'Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU10", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetReportDocument()
    '	Discription		: Get Report Document
    '	Create User		: Pranitda Sroengklang
    '	Create Date		: 17-06-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Function GetReportDocument(ByVal ds As DataSet) As ReportDocument
        ' Crystal Report File Path
        Dim RptFilePath As String = Server.MapPath("../App_Data\RPT\" & reportName.Trim & ".rpt")

        '** Boon is change report 
        If reportName.Trim = "KTPU01" Then
            ' Boon select new report
            RptFilePath = Server.MapPath("../App_Data\RPT\KTPU01_1.rpt")
        End If

        ' Declare a new Crystal Report Document object and the report file into the report document
        Dim RptDoc = New ReportDocument()
        RptDoc.Load(RptFilePath)

        ' Set the datasource by getting the dataset from business layer and In our case business layer is getCustomerData function
        RptDoc.SetDataSource(ds)

        Return RptDoc
    End Function
    '/**************************************************************
    '	Function name	: WriteFile(MS, FileName)
    '	Discription		: Create File From MemoryStream
    '	Create User		: Pranitda Sroengklang
    '	Create Date		: 17-06-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Sub WriteFile(ByVal MS As MemoryStream, ByVal FileName As String)
        Dim Path As String = Server.MapPath(FileName)
        Dim file As New System.IO.FileStream(Path, FileMode.Create, System.IO.FileAccess.Write)

        Dim bytes(MS.Length) As Byte
        MS.Read(bytes, 0, MS.Length)
        file.Write(bytes, 0, bytes.Length)
        file.Close()
        MS.Close()
    End Sub
#End Region
End Class
