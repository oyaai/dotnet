Imports System.Data
Imports System.Web.Configuration
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Accounting Detail
'	Class Name		    : KTAC01_Detail
'	Class Discription	: Display data detail from [Accounting] table
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 09-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTAC01_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objMessage As New Common.Utilities.Message
    Private searchID As String = ""

    'connect with service
    Private objAccountingService As New Service.ImpAccountingService

#Region "Event"
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
                searchID = Request.QueryString("id")

                'Get data and display on screen
                SearchData()

                ' call function display page
                DisplayPage()
            Else
                searchID = ""

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
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage()
        Try
            Dim dtAccountingDetail As New DataTable

            ' get table object from session 
            dtAccountingDetail = Session("dtAccountingDetail")

            ' check record for display
            If Not IsNothing(dtAccountingDetail) AndAlso dtAccountingDetail.Rows.Count > 0 Then
                'Display data on detail screen
                Select Case dtAccountingDetail.Rows(0)("AccountType").ToString
                    Case "1"
                        lblAccountType.Text = "Current Account"
                    Case "2"
                        lblAccountType.Text = "Saving Account"
                    Case "3"
                        lblAccountType.Text = "Cash"
                    Case Else
                        lblAccountType.Text = ""
                End Select

                lblVendorName.Text = dtAccountingDetail.Rows(0)("VendorName").ToString()
                lblBank.Text = dtAccountingDetail.Rows(0)("Bank").ToString()
                lblAccountName.Text = dtAccountingDetail.Rows(0)("AccountName").ToString()
                lblAccountNo.Text = dtAccountingDetail.Rows(0)("AccountNo").ToString()
                lblPaymentDate.Text = dtAccountingDetail.Rows(0)("PaymentDate").ToString()
                lblJobNo.Text = dtAccountingDetail.Rows(0)("JobNo").ToString()
                lblVat.Text = dtAccountingDetail.Rows(0)("vat_amount").ToString() & " (" & dtAccountingDetail.Rows(0)("vat_percentage").ToString() & "%)"
                lblWT.Text = dtAccountingDetail.Rows(0)("wt_amount").ToString() & " (" & dtAccountingDetail.Rows(0)("wt_percentage").ToString() & "%)"
                lblIE.Text = dtAccountingDetail.Rows(0)("IE").ToString()
                lblSubTotal.Text = dtAccountingDetail.Rows(0)("SubTotal").ToString()
                lblRemarks.Text = dtAccountingDetail.Rows(0)("Remarks").ToString()
            Else

            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtAccountingDetail As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            'call function GetItemList from ItemService
            dtAccountingDetail = objAccountingService.GetAccountingDetail(Session("objAccountingDetailDto"))

            ' set table object to session
            Session("dtAccountingDetail") = dtAccountingDetail
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
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
            ' Accounting dto object
            Dim objAccountingDetailDto As New Dto.AccountingDto

            'set data from condition search into dto object
            With objAccountingDetailDto
                .strAccount_id = searchID.Trim
            End With

            ' set dto object to session
            Session("objAccountingDetailDto") = objAccountingDetailDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
    
End Class
