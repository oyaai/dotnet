Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : KTPU04_Delivery
'	Class Name		    : KTPU04_Delivery
'	Class Discription	: Insert/Update Invoice 
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 21-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTPU04_Delivery
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    Private objValidate As New Common.Validations.Validation
    Private searchID As String = ""
    Private searchPo_header_id As String = ""
    Private pagedData As New PagedDataSource
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private Const constCreate As String = "Create"

    'connect with service
    Private objGetPO_DetailService As New Service.ImpInvoice_PurchaseService

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
            objLog.StartLog("KTPU04_Delivery", Session("UserName"))
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
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 03-06-2014
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            'Dim a As String = lblTotalAmount.Text
            Dim a As String = txtTotalAmount.Text
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



            'prepare data before insert into payment_header,payment_detail
            SetPaymentHeader()
            SetPaymentDetail()

            SetScreenToSession()

            If Session("mode") = "Add" Then 'case insert
                ' case not used then confirm message to delete
                objMessage.ConfirmMessage("KTPU04_Delivery", constCreate, objMessage.GetXMLMessage("KTPU_04_003"))
            Else
                ' case not used then confirm message to delete
                objMessage.ConfirmMessage("KTPU04_Delivery", constCreate, objMessage.GetXMLMessage("KTPU_04_006"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnCreate_Click", ex.Message.ToString, Session("UserName"))
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
            If txtDeliveryDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtPaymentDate.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDate.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check duplicate vendor_id
            If txtInvoiceNo.Text.Trim <> "" Then
                If objGetPO_DetailService.checkInvoice(Session("vendor_id"), txtInvoiceNo.Text.Trim, Session("id")) = True Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_009"))
                    Exit Function
                End If
            End If

            If txtTotalAmount.Text <> "" Then
                If txtTotalAmount.Text <> hidTotalAmount.Value Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_012"))
                    Exit Function
                End If
            End If


            'Check input data in gridview
            If checkGridview("1") = False Then
                Exit Function
            End If

            CheckError = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckError", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
    Private Function checkGridview(Optional ByVal flg As String = "") As Boolean
        checkGridview = False

        Try
            Dim sumDelAmt As Double = 0
            Dim dt As New DataTable

            For Each item As RepeaterItem In rptInquery.Items

                Dim boxQty As TextBox = item.FindControl("txtDelivery_qty")
                Dim boxUnitPrice As Label = item.FindControl("lblUnitPrice")
                Dim boxDelAmt As TextBox = item.FindControl("txtDelivery_amt")

                If Session("mode") = "Add" Then 'case insert
                    dt = Session("dtGetPO_Detail_Insert")
                Else
                    dt = Session("dtGetPaymentDetail")
                End If

                If boxQty.Text.Trim = "" Then
                    boxQty.Text = "0"
                End If
                If boxDelAmt.Text.Trim = "" Then
                    boxDelAmt.Text = "0"
                End If

                'check Qty
                If IsNumeric(boxQty.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                    Exit Function
                End If

                If CDbl(boxQty.Text.Trim) > CDbl(dt.Rows(item.ItemIndex)("remain_qty").ToString()) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                    Exit Function
                End If

                If CDbl(boxQty.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                    Exit Function
                End If

                Dim intQty As Integer = boxQty.Text
                Dim dblUnitPrice As Double = boxUnitPrice.Text
                Dim dblDelAmt As Double = boxDelAmt.Text

                If flg <> "1" Then
                    boxDelAmt.Text = intQty * dblUnitPrice
                    sumDelAmt = sumDelAmt + (intQty * dblUnitPrice)
                Else
                    sumDelAmt = sumDelAmt + dblDelAmt
                End If

                'Check amount
                If IsNumeric(boxDelAmt.Text.Trim) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                    Exit Function
                End If

                'If CDbl(boxDelAmt.Text.Trim) > CDbl(dt.Rows(item.ItemIndex)("remain_amt").ToString()) Then
                '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                '    Exit Function
                'End If

                If CDbl(boxDelAmt.Text.Trim) < 0 Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_001"))
                    Exit Function
                End If
            Next

            'lblTotalAmount.Text = Format(Convert.ToDouble(sumDelAmt), "#,##0.00")
            hidTotalAmount.Value = Format(Convert.ToDouble(sumDelAmt), "#,##0.00")

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

            'Set id to hashtable (for case link to detail page)
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Invoice_PurchaseDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnCancel_Click
    '	Discription	    : back to previous screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            'Clear value
            If Session("Mode") = "Add" Then
                clear()
                Response.Redirect("KTPU04.aspx?New=")
            ElseIf Session("Mode") = "Edit" Then
                clear()
                Response.Redirect("KTPU03.aspx?New=")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnCancel_Click", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub
    '/**************************************************************
    '	Function name	: txtDelivery_qty_TextChanged
    '	Discription	    : calculate price
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub txtDelivery_qty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If checkGridview() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("txtDelivery_qty_TextChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: txtDelivery_amt_TextChanged
    '	Discription	    : calculate price
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub txtDelivery_amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If checkGridview("1") = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("txtDelivery_qty_TextChanged", ex.Message.ToString, Session("UserName"))
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
    '	Function name	: SetPaymentHeader
    '	Discription	    : Keep data of each record from gridview
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub clear()
        Try
            Session("id") = Nothing
            Session("vendor_id") = Nothing
            Session("taskID") = Nothing
            Session("mode") = Nothing
            Session("searchPo_header_id") = Nothing
            Session("searchID") = Nothing
            Session("dtGetPO_Detail_Insert") = Nothing
            Session("dtGetPaymentDetail") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("clear", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetPaymentHeader
    '	Discription	    : Keep data of each record from gridview
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetPaymentHeader()
        Try
            Dim objInvHeadDto As New Dto.Invoice_PurchaseDto
            Dim startDeliveryDate As String = ""
            Dim arrDeliveryStartDate() As String = Split(txtDeliveryDate.Text.Trim(), "/")

            Dim startPaymentDate As String = ""
            Dim arrPaymentStartDate() As String = Split(txtPaymentDate.Text.Trim(), "/")

            'set data from condition search into dto object
            With objInvHeadDto
                If Session("mode") = "Add" Then 'case insert
                    .strPO_header_id = Session("searchID") 'ID from previous screen
                Else
                    .strPO_header_id = Session("searchPo_header_id")
                End If

                .strId = Session("searchID")
                .taskID = Session("taskID")

                If UBound(arrDeliveryStartDate) > 0 Then
                    startDeliveryDate = arrDeliveryStartDate(2) & arrDeliveryStartDate(1) & arrDeliveryStartDate(0)
                End If
                If UBound(arrPaymentStartDate) > 0 Then
                    startPaymentDate = arrPaymentStartDate(2) & arrPaymentStartDate(1) & arrPaymentStartDate(0)
                End If

                .strDeliveryDateFrom = startDeliveryDate
                .strPaymentDateFrom = startPaymentDate
                .strInvoice_start = txtInvoiceNo.Text.Trim()
                .strSearchType = rblAccountType.SelectedValue.ToString
                .strAccountNo = txtAccountNo.Text.Trim
                .strAccountName = txtAccountName.Text.Trim
                '.strTotal_Amount = Replace(lblTotalAmount.Text.Trim, ",", "")
                .strTotal_Amount = Replace(txtTotalAmount.Text.Trim, ",", "")
                .strRemark = txtRemark.Text.Trim
            End With

            ' set dto object to session
            Session("objInvHeadDto") = objInvHeadDto
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetPaymentHeader", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub
    '/**************************************************************
    '	Function name	: SetPaymentDetail
    '	Discription	    : Keep data of each record from gridview
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 25-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetPaymentDetail()
        Try
            Dim strDelivery_qty As String = ""
            Dim txtDelivery_qty As TextBox
            Dim txtDelivery_amt As TextBox
            Dim dtPaymentDetail As New DataTable
            Dim row As DataRow
            Dim dtGetPO_Detail_Insert As New DataTable
            Dim i As Integer = 0
            Dim startDeliveryDate As String = ""
            Dim arrDeliveryStartDate() As String = Split(txtDeliveryDate.Text.Trim(), "/")

            'Get data from table PO_Detail
            If Session("mode") = "Add" Then 'case insert
                dtGetPO_Detail_Insert = Session("dtGetPO_Detail_Insert")
            Else
                dtGetPO_Detail_Insert = Session("dtGetPaymentDetail")
            End If

            If UBound(arrDeliveryStartDate) > 0 Then
                startDeliveryDate = arrDeliveryStartDate(2) & arrDeliveryStartDate(1) & arrDeliveryStartDate(0)
            End If

            With dtPaymentDetail
                '.Columns.Add("payment_header_id")
                .Columns.Add("po_header_id")
                .Columns.Add("po_detail_id")
                .Columns.Add("delivery_qty")
                .Columns.Add("delivery_amount")
                .Columns.Add("stock_fg")

                'for stock
                .Columns.Add("job_order")
                .Columns.Add("po_no")
                .Columns.Add("invoice_no")
                .Columns.Add("ie_id")
                .Columns.Add("vendor_id")
                .Columns.Add("item_id")
                .Columns.Add("delivery_date")
                .Columns.Add("qty_in")
                .Columns.Add("qty_ori")
                .Columns.Add("amount_ori")
                .Columns.Add("amount")
                .Columns.Add("qty_out")
                .Columns.Add("qty_adjust")
                .Columns.Add("balance")
                .Columns.Add("item_name")
                .Columns.Add("ie_name")
                .Columns.Add("quantity")
                .Columns.Add("unit_price")
                .Columns.Add("remain_qty")
                .Columns.Add("remain_amt")
                .Columns.Add("delivery_amt")
                .Columns.Add("base")

                For Each item As RepeaterItem In rptInquery.Items
                    'set in list only account_type = 1 or 3 
                    row = .NewRow

                    txtDelivery_qty = item.FindControl("txtDelivery_qty")
                    txtDelivery_amt = item.FindControl("txtDelivery_amt")

                    If Session("mode") = "Add" Then 'case insert
                        row("po_header_id") = Session("searchID")
                    Else
                        row("po_header_id") = Session("searchPo_header_id")
                    End If
                    If Session("mode") = "Add" Then 'case insert
                        row("po_detail_id") = dtGetPO_Detail_Insert.Rows(i)("id").ToString()
                    Else
                        row("po_detail_id") = dtGetPO_Detail_Insert.Rows(i)("po_detail_id").ToString()
                    End If

                    row("delivery_qty") = txtDelivery_qty.Text
                    row("delivery_amount") = txtDelivery_amt.Text
                    row("stock_fg") = "1"

                    'for stock
                    row("job_order") = dtGetPO_Detail_Insert.Rows(i)("job_order").ToString()
                    row("po_no") = dtGetPO_Detail_Insert.Rows(i)("po_no").ToString()
                    row("invoice_no") = txtInvoiceNo.Text.Trim()
                    row("ie_id") = dtGetPO_Detail_Insert.Rows(i)("ie_id").ToString()
                    row("vendor_id") = dtGetPO_Detail_Insert.Rows(i)("vendor_id").ToString()
                    row("item_id") = dtGetPO_Detail_Insert.Rows(i)("item_id").ToString()
                    row("delivery_date") = startDeliveryDate
                    row("qty_in") = txtDelivery_qty.Text
                    row("qty_ori") = dtGetPO_Detail_Insert.Rows(i)("delivery_qty").ToString()
                    row("amount_ori") = dtGetPO_Detail_Insert.Rows(i)("delivery_amt").ToString()
                    row("amount") = dtGetPO_Detail_Insert.Rows(i)("amount").ToString()
                    row("qty_out") = txtDelivery_qty.Text
                    row("qty_adjust") = "0"
                    row("balance") = "0"
                    row("item_name") = dtGetPO_Detail_Insert.Rows(i)("item_name").ToString()
                    row("ie_name") = dtGetPO_Detail_Insert.Rows(i)("ie_name").ToString()
                    row("quantity") = dtGetPO_Detail_Insert.Rows(i)("quantity").ToString()
                    row("unit_price") = dtGetPO_Detail_Insert.Rows(i)("unit_price").ToString()
                    row("remain_qty") = dtGetPO_Detail_Insert.Rows(i)("remain_qty").ToString()
                    row("remain_amt") = Format(Convert.ToDouble(dtGetPO_Detail_Insert.Rows(i)("remain_amt").ToString()), "#,##0.00")
                    row("delivery_amt") = txtDelivery_amt.Text.Replace(",", "")
                    row("base") = dtGetPO_Detail_Insert.Rows(i)("base").ToString()
                    ' add data row to table
                    .Rows.Add(row)
                    i = i + 1
                Next
            End With

            'Set itemConfirm into session
            Session("dtPaymentDetail") = dtPaymentDetail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetPaymentDetail", ex.Message.ToString, Session("UserName"))
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
            Dim mode As String = ""
            Dim dtGetPO_Detail_Insert As New DataTable
            Dim dtGetPO_Header_Insert As New DataTable
            Dim dtGetPaymentHeader As New DataTable
            Dim dtGetPaymentDetail As New DataTable
            Dim exeProcess As Boolean = False

            If objUtility.GetQueryString(constCreate) = "True" Then
                exeProcess = True
            End If

            ' check case new enter
            If IsNothing(Request.QueryString("id")) = False Then
                searchID = Request.QueryString("id")
                If IsNothing(Request.QueryString("TskID")) = False Then
                    Session("taskID") = Request.QueryString("TskID")
                End If


                'for case modify
                If IsNothing(Request.QueryString("po_header_id")) = False Then
                    searchPo_header_id = Request.QueryString("po_header_id")
                End If
                Session("searchID") = searchID.Trim
                Session("searchPo_header_id") = searchPo_header_id.Trim

                If IsNothing(Request.QueryString("Mode")) = False Then
                    mode = Request.QueryString("Mode")
                    Session("mode") = mode
                Else
                    mode = ""
                End If

                If mode = "Add" Then 'Case add
                    btnCreate.Text = "Create"
                    'Get header
                    SearchHeaderData()
                    ' get table object from session 
                    dtGetPO_Header_Insert = Session("dtGetPO_Header_Insert")
                    'display header
                    DisplayHead(dtGetPO_Header_Insert)

                    'Get invoice detail and display on gridview
                    SearchDetailData()

                    'get table object from session 
                    dtGetPO_Detail_Insert = Session("dtGetPO_Detail_Insert")
                    'display detail on screen
                    DisplayDetail(Request.QueryString("PageNo"), dtGetPO_Detail_Insert)
                Else 'Case Modify
                    btnCreate.Text = "Update"
                    'Get data from payment_header
                    GetPaymentHeader()
                    'get table object from session 
                    dtGetPaymentHeader = Session("dtGetPaymentHeader")
                    'display header
                    DisplayHead(dtGetPaymentHeader)

                    'Get data from payment_detail
                    GetPaymentDetail()
                    'get table object from session 
                    dtGetPaymentDetail = Session("dtGetPaymentDetail")
                    'display detail on screen
                    DisplayDetail(Request.QueryString("PageNo"), dtGetPaymentDetail)
                End If
                ' call function check permission
                CheckPermission()
            Else
                If Session("searchID") = Nothing Or Session("searchID") = "" Then
                    searchID = ""
                    Session("searchID") = Nothing

                    'close screen popup
                    Response.Write("<script>")
                    Response.Write("alert('" & objMessage.GetXMLMessage("Common_001") & "');")
                    Response.Write("window.parent.close();")
                    Response.Write("</script>")
                Else
                    If Session("mode") = "Add" Then 'Case add
                        'display detail on screen
                        DisplayDetail(Request.QueryString("PageNo"), Session("dtGetPO_Detail_Insert"))
                    Else
                        'display detail on screen
                        If exeProcess = True Then
                            DisplayDetail(Request.QueryString("PageNo"), Session("dtPaymentDetail"))
                        Else
                            DisplayDetail(Request.QueryString("PageNo"), Session("dtGetPaymentDetail"))
                        End If
                    End If
                End If
            End If

            'set data from session to screen item
            SetSessionToScreen()

            'case post back from Confirm button
            If exeProcess = True Then
                If Session("mode") = "Add" Then 'case insert
                    insertProcess()
                Else
                    updateProcess()
                End If
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: DisplayHead
    '	Discription	    : Display header
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 06-03-2014
    '*************************************************************/
    Private Sub DisplayHead(ByVal dtPO As DataTable)
        Try
            'Dim dtGetPO_Header_Insert As New DataTable

            '' get table object from session 
            'dtGetPO_Header_Insert = Session("dtGetPO_Header_Insert")

            ' check record for display
            If Not IsNothing(dtPO) AndAlso dtPO.Rows.Count > 0 Then
                'Display data on detail screen
                txtDeliveryDate.Text = dtPO.Rows(0)("delivery_date").ToString()
                txtPaymentDate.Text = dtPO.Rows(0)("payment_date").ToString()
                txtInvoiceNo.Text = dtPO.Rows(0)("invoice_no").ToString()
                rblAccountType.Text = dtPO.Rows(0)("account_type").ToString()
                txtAccountNo.Text = dtPO.Rows(0)("account_no").ToString()
                txtAccountName.Text = dtPO.Rows(0)("account_name").ToString()
                'lblTotalAmount.Text = dtPO.Rows(0)("delivery_amount").ToString()
                hidTotalAmount.Value = dtPO.Rows(0)("delivery_amount").ToString()
                txtRemark.Text = dtPO.Rows(0)("remark").ToString()
                Session("vendor_id") = dtPO.Rows(0)("vendor_id").ToString()
                Session("id") = dtPO.Rows(0)("id").ToString()

                'set data into sesstion
                SetScreenToSession()
            Else
                'lblTotalAmount.Text = "0.00"
                'Session("lblTotalAmount.Text") = Nothing
                txtTotalAmount.Text = "0.00"
                Session("txtTotalAmount.Text") = Nothing

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
    '	Create Date	    : 07-06-2013
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
                lblDescription.Text = "showing " & dtPO.Rows.Count & " entries"
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblDescription.Text = "&nbsp;"
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()

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
            lblDescription.Text = "Showing " & intStart.ToString & " to " & intEnd.ToString & _
            " of " & intAllRecs.ToString & " entries"
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
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDetailData()
        Try
            ' table object keep value from item service
            Dim dtGetPO_Detail_Insert As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetPO_Detail_Insert = objGetPO_DetailService.GetPO_Detail_Insert(Session("objPO_DetailDto"))

            ' set table object to session
            Session("dtGetPO_Detail_Insert") = dtGetPO_Detail_Insert
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDetailData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchDetailData
    '	Discription	    : Search invoice detail  
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchHeaderData()
        Try
            ' table object keep value from item service
            Dim dtGetPO_Header_Insert As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetPO_Header_Insert = objGetPO_DetailService.GetPO_Header_Insert(Session("objPO_DetailDto"))

            ' set table object to session
            Session("dtGetPO_Header_Insert") = dtGetPO_Header_Insert
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDetailData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetPaymentHeader
    '	Discription	    : Get Payment Header  
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPaymentHeader()
        Try
            ' table object keep value from item service
            Dim dtGetPaymentHeader As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetPaymentHeader = objGetPO_DetailService.GetPaymentHeader(Session("objPO_DetailDto"))

            ' set table object to session
            Session("dtGetPaymentHeader") = dtGetPaymentHeader
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPaymentHeader", ex.Message.ToString, Session("UserName"))
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
    Private Sub SetValueToDto()
        Try
            ' Invoice_Purchase dto object
            Dim objPO_DetailDto As New Dto.Invoice_PurchaseDto

            'set data from condition search into dto object
            With objPO_DetailDto
                .strId = Session("searchID")
            End With

            ' set dto object to session
            Session("objPO_DetailDto") = objPO_DetailDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
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
            objAction = objPermission.CheckPermission(8)

            ' set permission 
            btnCreate.Enabled = objAction.actCreate
            btnCancel.Enabled = True

            ' set action permission to session
            Session("actList") = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: insertProcess
    '	Discription	    : insert data into payment_header,payment_detail
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub insertProcess()
        Try
            'insert data into payment_header,payment_detail
            Dim returnInsertAccounting As Integer
            Dim dtGetPaymentDetail As New DataTable

            returnInsertAccounting = objGetPO_DetailService.InsertPayment( _
                                                                            Session("objInvHeadDto"), _
                                                                            Session("dtPaymentDetail"))

            'Confirm completed
            If returnInsertAccounting = True Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_004"), Nothing, "KTPU04.aspx?Mode=")
            Else
                SetSessionToScreen()
                'get table object from session 
                dtGetPaymentDetail = Session("dtGetPO_Detail_Insert")
                'display detail on screen
                DisplayDetail(Request.QueryString("PageNo"), dtGetPaymentDetail)

                'insert failed
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_005"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("insertProcess", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: updateProcess
    '	Discription	    : update data into payment_header,payment_detail
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub updateProcess()
        Try
            'insert data into payment_header,payment_detail
            Dim returnInsertAccounting As Integer
            Dim dtGetPaymentDetail As New DataTable

            returnInsertAccounting = objGetPO_DetailService.UpdatePayment( _
                                                                            Session("objInvHeadDto"), _
                                                                            Session("dtPaymentDetail"))

            'Confirm completed
            If returnInsertAccounting = True Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_007"), Nothing, "KTPU03.aspx?Mode=Edit&New=True")
            Else
                SetSessionToScreen()
                'get table object from session 
                dtGetPaymentDetail = Session("dtGetPaymentDetail")
                'display detail on screen
                DisplayDetail(Request.QueryString("PageNo"), dtGetPaymentDetail)

                'insert failed
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_04_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("insertProcess", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchDetailData
    '	Discription	    : Search invoice detail  
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPaymentDetail()
        Try
            ' table object keep value from item service
            Dim dtGetPaymentDetail As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetPaymentDetail = objGetPO_DetailService.GetPaymentDetail(Session("objPO_DetailDto"))

            ' set table object to session
            Session("dtGetPaymentDetail") = dtGetPaymentDetail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDetailData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToSession
    '	Discription	    : Set Value To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 03-06-2014
    '*************************************************************/
    Private Sub SetScreenToSession()
        Session("txtDeliveryDate") = txtDeliveryDate.Text.Trim()
        Session("txtPaymentDate") = txtPaymentDate.Text.Trim()
        Session("txtInvoiceNo") = txtInvoiceNo.Text.Trim()
        Session("rblAccountType") = rblAccountType.SelectedValue.ToString
        Session("txtAccountNo") = txtAccountNo.Text.Trim
        Session("txtAccountName") = txtAccountName.Text.Trim
        'Session("lblTotalAmount") = Replace(lblTotalAmount.Text.Trim, ",", "")
        Session("txtTotalAmount") = Replace(txtTotalAmount.Text.Trim, ",", "")
        Session("txtRemark") = txtRemark.Text.Trim
    End Sub
    '/**************************************************************
    '	Function name	: SetValueToSession
    '	Discription	    : Set Value To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 03-06-2014
    '*************************************************************/
    Private Sub SetSessionToScreen()
        txtDeliveryDate.Text = Session("txtDeliveryDate")
        txtPaymentDate.Text = Session("txtPaymentDate")
        txtInvoiceNo.Text = Session("txtInvoiceNo")
        rblAccountType.SelectedValue = Session("rblAccountType")
        txtAccountNo.Text = Session("txtAccountNo")
        txtAccountName.Text = Session("txtAccountName")
        'lblTotalAmount.Text = Format(Convert.ToDouble(Session("lblTotalAmount")), "#,##0.00")
        txtTotalAmount.Text = Format(Convert.ToDouble(Session("txtTotalAmount")), "#,##0.00")
        txtRemark.Text = Session("txtRemark")
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
        Session("txtDeliveryDate") = Nothing
        Session("txtPaymentDate") = Nothing
        Session("txtInvoiceNo") = Nothing
        Session("rblAccountType") = Nothing
        Session("txtAccountNo") = Nothing
        Session("txtAccountName") = Nothing
        'Session("lblTotalAmount") = Nothing
        Session("txtTotalAmount") = Nothing
        Session("txtRemark") = Nothing
    End Sub
#End Region
End Class
