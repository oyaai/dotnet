
Partial Class Purchase_KTPU01_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 14-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTPU01_Detail: Purchase Detail", Session("UserName"))
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
    '	Create Date	    : 14-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Session("UserName") Is Nothing Then Session("UserName") = "Boonyarit"
            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 14-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDF.Click
        Try
            Dim intPurchaseId As Integer = Session("PurchaseId")
            'Dim objCom As New Common.Utilities.Message

            'objCom.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU01&ID=" & intPurchaseId, 1000, 990)
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler
            Dim strPage As String = "../Report/ReportViewer.aspx?ReportName=KTPU01&ID=" & intPurchaseId
            strPage = "javascript:showpopup('" & strPage & "','_self');"
            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), "ShowPDF", strPage, True)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnPDF_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 14-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objDT As New System.Data.DataTable
            Dim objService As New Service.ImpPurchaseService
            Dim objPurchaseDto As New Dto.PurchaseDto
            Dim intPurchaseId As Integer = CInt(objComm.GetQueryString("ID"))
            Session("PurchaseId") = intPurchaseId

            Call ClearForm()
            objPurchaseDto = objService.GetPurchaseForDetail(intPurchaseId, objDT)
            If (Not objPurchaseDto Is Nothing) Then
                Call ShowData(objPurchaseDto, objDT)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data purchase
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 14-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData(ByVal objPurchase As Dto.PurchaseDto, ByVal objDT As System.Data.DataTable)
        Try
            Dim strType2_TextAll As String = String.Empty
            Dim objCom As New Common.Utilities.Utility

            With objPurchase
                lblType.Text = .po_type_text
                lblPONo.Text = .po_no
                lblDeliveryDate.Text = .delivery_date
                lblQuotation.Text = .quotation_no
                lblVendor.Text = .vendor_name
                lblPayTerm.Text = .payment_term_name
                lblVat.Text = .vat_name
                lblWT.Text = .wt_name
                lblCurrency.Text = .currency_name
                lblHeadRemark.Text = .remarks
                lblAttn.Text = .attn
                lblDeliverTo.Text = .deliver_to
                lblContact.Text = .contact

                lblSubTotalAmt.Text = objCom.FormatNumeric(.sub_total, 2, True)
                lblDiscountAmt.Text = objCom.FormatNumeric(.discount_total, 2, True)
                lblVatAmt.Text = objCom.FormatNumeric(.vat_amount, 2, True)
                lblWTAmt.Text = objCom.FormatNumeric(.wt_amount, 2, True)
                lblTotalAmt.Text = objCom.FormatNumeric(.total_amount, 2, True)

                rptViewPurchase.DataSource = objDT
                rptViewPurchase.DataBind()

            End With

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear page form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 14-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            lblType.Text = String.Empty
            lblPONo.Text = String.Empty
            lblDeliveryDate.Text = String.Empty
            lblQuotation.Text = String.Empty
            lblVendor.Text = String.Empty
            lblPayTerm.Text = String.Empty
            lblVat.Text = String.Empty
            lblWT.Text = String.Empty
            lblCurrency.Text = String.Empty
            lblHeadRemark.Text = String.Empty
            lblAttn.Text = String.Empty
            lblDeliverTo.Text = String.Empty
            lblContact.Text = String.Empty
            lblSubTotalAmt.Text = String.Empty
            lblDiscountAmt.Text = String.Empty
            lblVatAmt.Text = String.Empty
            lblWTAmt.Text = String.Empty
            lblTotalAmt.Text = String.Empty

            rptViewPurchase.DataSource = Nothing
            rptViewPurchase.DataBind()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

    
End Class
