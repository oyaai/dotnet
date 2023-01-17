Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Purchase History Report
'	Class Name		    : Purchase_KTPU09
'	Class Discription	: Webpage for Purchase History  Report
'	Create User 		: Nisa S.
'	Create Date		    : 19-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Purchase_KTPU09
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objPurchaseSer As New Service.ImpPurchase_HistoryService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message



    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTPU09 : Purchase History Report")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
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
    '	Function name	: btnPDF_Click
    '	Discription	    : export data to pdf file
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDF.Click
        Try
            Dim dtPurchaseHistoryReport As New DataTable
            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data
            SearchDataReport()


            ' get table object from session 
            dtPurchaseHistoryReport = Session("dtPurchaseHistoryReport")

            If Not IsNothing(dtPurchaseHistoryReport) AndAlso dtPurchaseHistoryReport.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU09", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnPDF_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check format date of field Start Date 
            If txtDeliveryDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDeliveryDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtDeliveryDateFrom.Focus()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field end Date 
            If txtDeliveryDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDeliveryDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    txtDeliveryDateTo.Focus()
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check txtStartDate > txtEndDate
            If txtDeliveryDateFrom.Text.Trim <> "" And txtDeliveryDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtDeliveryDateFrom.Text.Trim, txtDeliveryDateTo.Text.Trim) = False Then
                    txtDeliveryDateFrom.Focus()
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If



            CheckCriteriaInput = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCriteriaInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search Purchase History data for report
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtPurchaseHistoryReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetWorkingHourReport from WorkingHourService
            dtPurchaseHistoryReport = objPurchaseSer.GetPurchaseHistoryReport(Session("objPurchaseHistoryDto"))
            ' set table object to session
            Session("dtPurchaseHistoryReport") = dtPurchaseHistoryReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            End If

            ' set search text to session
            txtJobOrder.Text = Session("txtJobOrder")
            txtPoNo.Text = Session("txtPoNo")
            txtItemName.Text = Session("txtItemName")
            txtInvoiceNo.Text = Session("txtInvoiceNo")
            txtVendorName.Text = Session("txtVendorName")
            txtDeliveryDateFrom.Text = Session("txtDeliveryDateFrom")
            txtDeliveryDateTo.Text = Session("txtDeliveryDateTo")

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            'Working Hour dto object
            Dim objPurchaseHistoryDto As New Dto.Purchase_HistoryDto
            Dim startDate As String = ""
            Dim endDate As String = ""
            Dim arrStartDate() As String = Split(txtDeliveryDateFrom.Text.Trim(), "/")
            Dim arrEndDate() As String = Split(txtDeliveryDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objPurchaseHistoryDto
                'Set start date to format yyymmdd
                If UBound(arrStartDate) > 0 Then
                    startDate = arrStartDate(2) & arrStartDate(1) & arrStartDate(0)
                End If
                'Set end date to format yyymmdd
                If UBound(arrEndDate) > 0 Then
                    endDate = arrEndDate(2) & arrEndDate(1) & arrEndDate(0)
                End If

                .delivery_date1 = startDate
                .delivery_date2 = endDate
                .job_order = txtJobOrder.Text.Trim
                .po_no = txtPoNo.Text.Trim
                .ItemName = txtItemName.Text.Trim
                .invoice_no = txtInvoiceNo.Text.Trim
                .VendorName = txtVendorName.Text.Trim

            End With

            ' set dto object to session
            Session("objPurchaseHistoryDto") = objPurchaseHistoryDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtPurchaseHistoryReport") = Nothing
            Session("txtJobOrder") = Nothing
            Session("txtPoNo") = Nothing
            Session("txtItemName") = Nothing
            Session("txtInvoiceNo") = Nothing
            Session("txtVendorName") = Nothing
            Session("txtDeliveryDateFrom") = Nothing
            Session("txtDeliveryDateTo") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Purchase History menu
            objAction = objPermission.CheckPermission(11)
            ' set permission Create 
            btnPDF.Enabled = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from Purchase History to session
            Session("txtJobOrder") = txtJobOrder.Text.Trim
            Session("txtPoNo") = txtPoNo.Text.Trim
            Session("txtItemName") = txtItemName.Text.Trim
            Session("txtInvoiceNo") = txtInvoiceNo.Text.Trim
            Session("txtVendorName") = txtVendorName.Text.Trim
            Session("txtDeliveryDateFrom") = txtDeliveryDateFrom.Text.Trim
            Session("txtDeliveryDateTo") = txtDeliveryDateTo.Text.Trim


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
End Class
