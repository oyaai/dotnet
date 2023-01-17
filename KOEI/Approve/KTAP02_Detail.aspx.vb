#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Accounting Approve Detail
'	Class Name		    : Approve_KTAP02_Detail
'	Class Discription	: Display data detail from [Accounting] table
'	Create User 		: Wasan D.
'	Create Date		    : 10-10-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports System.Data

Partial Class Approve_KTAP02_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility
    Private objAccountingService As New Service.ImpAccountingService

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page init
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTAP02_Detail : Acounting Approve Detail")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#Region "Events"
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' check case new enter
            If IsNothing(Request.QueryString("voucherNo")) = False Then
                Dim dtAccountDetail As New DataTable
                'Get data and display on screen
                dtAccountDetail = SearchAccAPDetail(Request.QueryString("voucherNo"))
                ' call function display page
                ShowData(dtAccountDetail)
            Else
                'close screen popup
                Response.Write("<script>")
                Response.Write("alert('" & objMessage.GetXMLMessage("Common_001") & "');")
                Response.Write("window.parent.close();")
                Response.Write("</script>")
            End If

        Catch ex As Exception

            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : ShowData Item data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData(ByVal dtAccountDetail As DataTable)
        Try
            lblAccountType.Text = dtAccountDetail.Rows(0).Item("account_type")
            lblVendorName.Text = dtAccountDetail.Rows(0).Item("vendor_name")
            lblBank.Text = dtAccountDetail.Rows(0).Item("bank")
            lblAccountName.Text = dtAccountDetail.Rows(0).Item("accountname")
            lblAccountNo.Text = dtAccountDetail.Rows(0).Item("cheque_no")
            lblPaymentDate.Text = objUtility.String2Date(dtAccountDetail.Rows(0).Item("account_date"), "yyyyMMdd").ToString("dd/MMM/yyyy")
            ' Bind data to repeater
            rptInquery.DataSource = dtAccountDetail
            rptInquery.DataBind()
            lblSumSubTotal.Text = CDec(dtAccountDetail.Compute("Sum(sub_total)", "remark<>'Bank Fee'")).ToString("#,##0.00")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ShowData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SearchAccAPDetail(ByVal strVoucherNo As String) As DataTable
        SearchAccAPDetail = New DataTable
        Try
            'call function GetItemList from ItemService
            SearchAccAPDetail = objAccountingService.GetAccountApproveByVoucherNo(strVoucherNo)

            ' set table object to session
            Session("dtAccountingDetail") = SearchAccAPDetail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Function
#End Region
End Class
