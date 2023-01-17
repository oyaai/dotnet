Imports System.Data
Imports System.Web.Configuration
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Invoice Detail
'	Class Name		    : KTPU04_Detail
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
Partial Class KTPU07_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    Private pagedData As New PagedDataSource

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
            objLog.StartLog("KTPU07_Detail", Session("UserName"))
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
    '	Create Date	    : 17-07-2013
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
            Dim objDate As New Common.Utilities.Utility

            ' check case new enter
            If IsNothing(Request.QueryString("strId")) = False Then
                Session("strId") = Request.QueryString("strId")
                'Get invoice head and display on screen
                SearchHeadData()
                'Get invoice detail and display on gridview
                SearchDetailData()

                'call function to display invoice header on screen
                DisplayHead()

                'call function to display invoice detail on screen
                DisplayDetail(Request.QueryString("PageNo"))
            Else
                If IsNothing(Session("strId")) = False Then
                    'call function to display invoice header on screen
                    DisplayHead()

                    'call function to display invoice detail on screen
                    DisplayDetail(Request.QueryString("PageNo"))
                Else
                    Session("strId") = Nothing

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
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayHead()
        Try
            Dim dtGetChequeHead As New DataTable

            ' get table object from session 
            dtGetChequeHead = Session("dtGetChequeHead")

            ' check record for display
            If Not IsNothing(dtGetChequeHead) AndAlso dtGetChequeHead.Rows.Count > 0 Then
                'Display data on detail screen
                lblChequeNo.Text = dtGetChequeHead.Rows(0)("cheque_no").ToString()
                lblVendorName.Text = dtGetChequeHead.Rows(0)("vendor_name").ToString()
                lblVendorType.Text = dtGetChequeHead.Rows(0)("vendor_type").ToString()
                lblChequeDate.Text = dtGetChequeHead.Rows(0)("cheque_date").ToString()
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
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayDetail(ByVal intPageNo As Integer)
        Try
            Dim dtGetCheque_Detail As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtGetCheque_Detail = Session("dtGetCheque_Detail")

            ' check record for display
            If Not IsNothing(dtGetCheque_Detail) AndAlso dtGetCheque_Detail.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtGetCheque_Detail)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery_Detail.DataSource = pagedData
                rptInquery_Detail.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtGetCheque_Detail.Rows.Count - 1)
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
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchHeadData()
        Try
            ' table object keep value from item service
            Dim dtGetChequeHead As New DataTable

            'call function GetItemList from ItemService
            dtGetChequeHead = objCheque_PurchaseService.GetCheque_Head(Session("strId"), "")

            ' set table object to session
            Session("dtGetChequeHead") = dtGetChequeHead
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
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDetailData()
        Try
            ' table object keep value from item service
            Dim dtGetCheque_Detail As New DataTable

            'call function GetItemList from ItemService
            dtGetCheque_Detail = objCheque_PurchaseService.GetCheque_Detail(Session("strId"), "")

            ' set table object to session
            Session("dtGetCheque_Detail") = dtGetCheque_Detail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDetailData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
