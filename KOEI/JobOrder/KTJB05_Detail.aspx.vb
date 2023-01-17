Imports System.Data
Imports System.IO
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Sale Invoice
'	Class Name		    : JobOrder_KTJB05_Detail
'	Class Discription	: Webpage for Sale Invoice Detail
'	Create User 		: Suwishaya L.
'	Create Date		    : 24-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB05_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objSaleInvoiceSer As New Service.ImpSale_InvoiceService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB05_Detail : Sale Invoice")

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
    '	Create Date	    : 24-07-2013
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
    '	Function name	: rptSaleInvDetail_ItemDataBound
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptSaleInvDetail_ItemDataBound( _
        ByVal sender As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
    ) Handles rptSaleInvDetail.ItemDataBound
        Try
            ' object label 
            Dim lblAmount As New Label

            ' find label amount and assign to variable 
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            'hidden amount
            If Session("menuId") = 3 Or Session("menuId") = 4 Or Session("menuId") = 42 Then
                If Not Session("actAmount") Then
                    lblAmount.Text = "******"
                End If
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptSaleInvDetail_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            'get data for display on header screen
            LoadInitialHeader()
            'get data for display on detail screen
            LoadInitialDetail()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialHeader
    '	Discription	    : Load initial data for display on screen
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialHeader()
        Try
            ' Item Dto object for keep return value from service
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim intID As Integer = 0
            Dim intMenuId As Integer
            Dim sumOtherAmount As Decimal = 0

            ' check id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intID = CInt(objUtility.GetQueryString("id"))
                Session("id") = intID
            Else
                intID = Session("id")
            End If
            'check menu id for check permission then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("menuId")) Then
                intMenuId = CInt(objUtility.GetQueryString("menuId"))
                Session("menuId") = intMenuId
            Else
                intMenuId = Session("menuId")
            End If

            ' call function GetSaleInvoiceHeaderList from service
            objSaleInvoiceDto = objSaleInvoiceSer.GetSaleInvoiceHeaderList(intID)

            ' assign value to control
            With objSaleInvoiceDto
                lblInvoiceNo.Text = .invoice_no
                lblIssueDate.Text = .issue_date
                lblReceiptDate.Text = .receipt_date
                lblAccountTitle.Text = .account_title
                lblCustomer.Text = .customer_name
                lblAccountType.Text = .account_type_name
                lblInvoiceType.Text = .invoice_type_name
                'lblBankFee.Text = .bank_fee_detail
                lblInvoiceAmount.Text = .amount
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialHeader", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialDetail
    '	Discription	    : get sale invoice detail
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialDetail()
        Try
            ' table object keep value from item service
            Dim dtSaleInvoiceDetail As New DataTable

            ' call function GetSaleInvoiceDetailList from SaleInvoice Service
            dtSaleInvoiceDetail = objSaleInvoiceSer.GetSaleInvoiceDetailList(Session("id"))
            ' set table object to session
            Session("dtSaleInvoiceDetail") = dtSaleInvoiceDetail

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialDetail", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtSaleInv As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtSaleInv = Session("dtSaleInvoiceDetail")

            ' check record for display
            If Not IsNothing(dtSaleInv) AndAlso dtSaleInv.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtSaleInv)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptSaleInvDetail.DataSource = pagedData
                rptSaleInvDetail.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtSaleInv.Rows.Count)
            Else
                ' case not exist data
                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptSaleInvDetail.DataSource = Nothing
                rptSaleInvDetail.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            'set menu id
            Dim intMenuId As Integer = 3
            If Not Session("menuId") Is Nothing Then
                intMenuId = Session("menuId")
            End If

            ' check permission of Item menu
            objAction = objPermission.CheckPermission(intMenuId)
            ' set action permission to session
            Session("actAmount") = objAction.actAmount

            'hidden amount on link from job order menu
            If Session("menuId") = 3 Or Session("menuId") = 4 Or Session("menuId") = 42 Then
                If Not Session("actAmount") Then
                    lblInvoiceAmount.Text = "******"
                End If
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region


End Class
