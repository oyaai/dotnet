Imports System.Data

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Account
'	Class Name		    : KTAC02_Detail
'	Class Discription	: Webpage for view income detail
'	Create User 		: Komsan Luecha
'	Create Date		    : 17-06-2013
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
    Partial Class KTAC02_Detail
        Inherits System.Web.UI.Page

        Private objLog As New Common.Logs.Log
        Private objMessage As New Common.Utilities.Message
        Private objUtility As New Common.Utilities.Utility

#Region "Event"
        '/**************************************************************
        '	Function name	: Page_Init
        '	Discription	    : Event page initial
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Protected Sub Page_Init( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles Me.Init
            Try
                ' write start log
                objLog.StartLog("KTAC02_Detail : Accounting Income Detail")
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
        '	Create Date	    : 17-06-2013
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
        '	Discription	    : Initial Page
        '	Return Value	: nothing
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Sub InitialPage()
            Try
                ' variable row object
                Dim row As DataRow
                ' get data from session
                row = Session("rowDetails")

                ' set data to control
                With row
                    If Request.QueryString("Type") = "Income" Then
                        lblDate.Text = "Receipt Date"
                    ElseIf Request.QueryString("Type") = "Payment" Then
                        lblDate.Text = "Payment Date"
                    End If

                    lblAccountType.Text = .Item("AccountTypeName")
                    lblVendorName.Text = .Item("VendorName")
                    lblVendorAddress.Text = .Item("BranchName")
                    lblBank.Text = .Item("Bank")
                    lblAccountName.Text = .Item("AccountName")
                    lblAccountNo.Text = .Item("AccountNo")
                    lblChequeNo.Text = .Item("ChequeNo")
                    lblReceiptDate.Text = .Item("ReceiptDateShow")
                    lblJobOrder.Text = .Item("JobOrder")
                    lblVat.Text = CDec(.Item("VatAmount")).ToString("#,##0.00") & " (" & .Item("VatText") & ")"
                    lblWT.Text = CDec(.Item("WTAmount")).ToString("#,##0.00") & " (" & .Item("WTText") & ")"
                    lblAccountTitle.Text = .Item("IEName")
                    lblSubTotal.Text = Convert.ToDecimal(.Item("SubTotal")).ToString("#,##0.00")
                    lblRemark.Text = .Item("Remark")
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
            End Try
        End Sub

#End Region
    End Class
End Namespace