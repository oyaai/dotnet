Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Invoice Detail
'	Class Name		    : KTPU03_Detail
'	Class Discription	: Display data detail from [Invoice] table
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

Partial Class KTPU03_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    'Private searchID As String = ""
    Private pagedData As New PagedDataSource

    'connect with service
    Private objInv_PurchaseService As New Service.ImpInvoice_PurchaseService

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
            objLog.StartLog("KTPU03_Detail", Session("UserName"))
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
            If IsNothing(Request.QueryString("id")) = False Then
                Session("searchID") = Request.QueryString("id")

                'Get invoice head and display on screen
                SearchHeadData()
                'Get invoice detail and display on gridview
                SearchDetailData()

                'call function to display invoice header on screen
                DisplayHead()

                'call function to display invoice detail on screen
                DisplayDetail(Request.QueryString("PageNo"))
            Else
                If IsNothing(Session("searchID")) = False Then
                    'call function to display invoice header on screen
                    DisplayHead()

                    'call function to display invoice detail on screen
                    DisplayDetail(Request.QueryString("PageNo"))
                Else
                    Session("searchID") = Nothing

                    'close screen popup
                    Response.Write("<script>")
                    Response.Write("alert('" & objMessage.GetXMLMessage("Common_001") & "');")
                    Response.Write("window.parent.close();")
                    Response.Write("</script>")
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
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayHead()
        Try
            Dim dtGetInvoice_Header As New DataTable

            ' get table object from session 
            dtGetInvoice_Header = Session("dtGetInvoice_Header")

            ' check record for display
            If Not IsNothing(dtGetInvoice_Header) AndAlso dtGetInvoice_Header.Rows.Count > 0 Then
                'Display data on detail screen
                lblInvoiceNo.Text = dtGetInvoice_Header.Rows(0)("invoice_no").ToString()
                lblDelivery_date.Text = dtGetInvoice_Header.Rows(0)("delivery_date").ToString()
                lblPayment_date.Text = dtGetInvoice_Header.Rows(0)("payment_date").ToString()
                lblAccountType.Text = dtGetInvoice_Header.Rows(0)("account_type").ToString()
                lblAccountNo.Text = dtGetInvoice_Header.Rows(0)("account_no").ToString()
                lblAccountName.Text = dtGetInvoice_Header.Rows(0)("account_name").ToString()
                lblDeliveryAmt.Text = dtGetInvoice_Header.Rows(0)("total_delivery_amount").ToString()
                lblRemark.Text = dtGetInvoice_Header.Rows(0)("remark").ToString()
                lblVatAmt.Text = dtGetInvoice_Header.Rows(0)("vat_amount").ToString()
                lblTotal.Text = Format(Convert.ToDouble(lblDeliveryAmt.Text.Replace(",", "")) + Convert.ToDouble(lblVatAmt.Text.Replace(",", "")), "#,##0.00")
            Else

            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayHead", ex.Message.ToString, Session("UserName"))
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
    Private Sub DisplayDetail(ByVal intPageNo As Integer)
        Try
            Dim dtGetInvoice_Detail As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtGetInvoice_Detail = Session("dtGetInvoice_Detail")

            ' check record for display
            If Not IsNothing(dtGetInvoice_Detail) AndAlso dtGetInvoice_Detail.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtGetInvoice_Detail)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery_Detail.DataSource = pagedData
                rptInquery_Detail.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtGetInvoice_Detail.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptInquery_Detail.DataSource = Nothing
                rptInquery_Detail.DataBind()

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
    '	Function name	: SearchHeadData
    '	Discription	    : Search invoice header  
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchHeadData()
        Try
            ' table object keep value from item service
            Dim dtGetInvoice_Header As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetInvoice_Header = objInv_PurchaseService.GetInvoice_Header(Session("objInv_PurchaseDetailDto"))

            ' set table object to session
            Session("dtGetInvoice_Header") = dtGetInvoice_Header
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchHeadData", ex.Message.ToString, Session("UserName"))
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
            Dim dtGetInvoice_Detail As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtGetInvoice_Detail = objInv_PurchaseService.GetInvoice_Detail(Session("objInv_PurchaseDetailDto"))

            ' set table object to session
            Session("dtGetInvoice_Detail") = dtGetInvoice_Detail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDetailData", ex.Message.ToString, Session("UserName"))
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
            Dim objInv_PurchaseDetailDto As New Dto.Invoice_PurchaseDto

            'set data from condition search into dto object
            With objInv_PurchaseDetailDto
                .strId = Session("searchID")
            End With

            ' set dto object to session
            Session("objInv_PurchaseDetailDto") = objInv_PurchaseDetailDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
