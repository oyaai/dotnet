Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
Imports System.Web.Services

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Sale Invoice
'	Class Name		    : JobOrder_KTJB10
'	Class Discription	: Webpage for Planned Payment Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-08-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB10
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objPlannedPaymentSer As New Service.ImpPlanned_PaymentService
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
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB10 : Planned Payment Report")
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
    '	Create Date	    : 05-08-2013
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
    '	Function name	: btnExcel_Click
    '	Discription	    : export data to excel file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnExcel.Click
        Try
            Dim dtJobOrderForReport As New DataTable

            ' set search text to session
            Session("ddlYear") = ddlYear.SelectedValue

            'Get data for export excel report
            SearchDataReport()

            ' get table object from session 
            dtJobOrderForReport = Session("dtJobOrderForReport")

            If Not IsNothing(dtJobOrderForReport) AndAlso dtJobOrderForReport.Rows.Count > 0 Then
                'call function ExportExcel
                ExportExcel()
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnExcel_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
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

            ' call function set year dropdownlist
            LoadListYear()

            ddlYear.SelectedValue = Session("ddlYear")

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListYear
    '	Discription	    : Load list year function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 05-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListYear()
        Try
            Dim listYearDto As New DataTable

            ' call function GetYearList from service
            listYearDto = objPlannedPaymentSer.GetYearList()
            ' call function for bound data with dropdownlist
            ddlYear.DataSource = listYearDto
            ddlYear.DataTextField = listYearDto.Columns(1).ToString()
            ddlYear.DataValueField = listYearDto.Columns(1).ToString()
            ddlYear.DataBind()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListYear", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search Sale Invoice data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 06-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtJobOrderForReport As New DataTable
            Dim dtInvoiceForReport As New DataTable
            Dim dtAmountThbForReport As New DataTable
            Dim dtSumAmountThbForReport As New DataTable
            Dim objPlannedPaymentDto As New Dto.PlannedPaymentDto

            ' call function GetJobOrderForReport from IPlanned_PaymentService
            dtJobOrderForReport = objPlannedPaymentSer.GetJobOrderForReport(ddlYear.SelectedValue)
            dtInvoiceForReport = objPlannedPaymentSer.GetInvoiceForReport(ddlYear.SelectedValue)
            dtAmountThbForReport = objPlannedPaymentSer.GetAmountThbForReport(ddlYear.SelectedValue)
            dtSumAmountThbForReport = objPlannedPaymentSer.GetSumAmountThbForReport(ddlYear.SelectedValue)
            objPlannedPaymentDto = objPlannedPaymentSer.GetMaxPayDateForReport(ddlYear.SelectedValue)
             
            ' set table object to session
            Session("dtJobOrderForReport") = dtJobOrderForReport
            Session("dtInvoiceForReport") = dtInvoiceForReport
            Session("dtAmountThbForReport") = dtAmountThbForReport
            Session("dtSumAmountThbForReport") = dtSumAmountThbForReport
            Session("objPlannedPaymentDto") = objPlannedPaymentDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    ' Function name   : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Suwishaya L.
    ' Create Date     : 24-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
            Dim dtJobOrder As DataTable = Nothing
            Dim dtInvoice As DataTable = Nothing
            Dim dtAmountThb As DataTable = Nothing
            Dim dtSumAmountThb As DataTable = Nothing
            Dim objMaxPayDateDto As Dto.PlannedPaymentDto
            Dim rowCount As Integer = 4
            Dim colInvCount As Integer = 0
            Dim sumAmount As Decimal = 0
            Dim columnMonth As Integer = 10
            Dim countMaxCol As Integer = 10
            Dim maxYear As Integer
            Dim minYear As Integer
            Dim maxMonth As Integer
            Dim minMonth As Integer
            Dim chkLoop As Integer
            Dim payDate As String
            Dim payDateHeader As String
            Dim payDateAmount As String

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "PlannedPaymentReport.xlsx"))))
            wBook = pck.Workbook.Worksheets(1)

            'set header
            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)

            dtJobOrder = Session("dtJobOrderForReport")
            dtInvoice = Session("dtInvoiceForReport")
            dtAmountThb = Session("dtAmountThbForReport")
            dtSumAmountThb = Session("dtSumAmountThbForReport")
            objMaxPayDateDto = Session("objPlannedPaymentDto")

            '--write year from search screen to header
            wBook.Cells(2, 1).Value = ddlYear.SelectedValue

            '--Write Header for month/year
            If Not objMaxPayDateDto.max_pay_date Is Nothing Or Not String.IsNullOrEmpty(objMaxPayDateDto.max_pay_date) Then
                Dim arrMaxPayDate As String() = objMaxPayDateDto.max_pay_date.Split("-")
                maxYear = arrMaxPayDate(0)
                maxMonth = arrMaxPayDate(1)
                minYear = CInt(Now.Year)
                minMonth = CInt(Now.Month)
                If minYear >= maxYear Then
                    minYear = maxYear
                    minMonth = maxMonth
                End If

                'year
                For x As Integer = minYear To maxYear
                    chkLoop = 0
                    'Month
                    For y As Integer = 1 To 12
                        If x = maxYear And y > maxMonth Then
                            chkLoop = 1
                            Exit For
                        End If
                        'Copy cell
                        wBook.Cells(4, 9).Copy(wBook.Cells(4, columnMonth))
                        'write data
                        payDate = x & "-" & y & "-" & "01"
                        wBook.Cells(4, columnMonth).Value = CDate(payDate).ToString("MMM yyyy")
                        columnMonth += 1
                    Next
                    If chkLoop = 1 Then
                        Exit For
                    End If
                Next

            End If
           
            '--Write Detail Report
            Dim id1 As Integer = 0
            Dim id_temp As Integer = 0
            For i As Integer = 0 To dtJobOrder.Rows.Count - 1
                '--Write job order
                id1 = dtJobOrder.Rows(i)("job_order_id").ToString()
                wBook.Cells(rowCount + 1, 1).Value = dtJobOrder.Rows(i)("job_order").ToString()
                wBook.Cells(rowCount + 1, 2).Value = dtJobOrder.Rows(i)("customer").ToString()
                wBook.Cells(rowCount + 1, 3).Value = dtJobOrder.Rows(i)("description").ToString()

                '--Write Invoice No
                colInvCount = 4
                For j As Integer = 0 To dtInvoice.Rows.Count - 1
                    If id1.ToString = dtInvoice.Rows(j)("job_order_id").ToString() Then
                        wBook.Cells(rowCount + 1, colInvCount).Value = dtInvoice.Rows(j)("invoice_no").ToString()
                        colInvCount += 1
                    End If
                Next

                '--Write Sum Amount (THB) 
                For k As Integer = 0 To dtSumAmountThb.Rows.Count - 1
                    If id1.ToString = dtSumAmountThb.Rows(k)("job_order_id").ToString() Then
                        wBook.Cells(rowCount + 1, 9).Value = CDec(dtSumAmountThb.Rows(k)("sum_amount_thb").ToString())
                     End If
                Next

                '--Write Amount THB order by month
                For n As Integer = 0 To dtAmountThb.Rows.Count - 1
                    If id1.ToString = dtAmountThb.Rows(n)("job_order_id").ToString() Then
                        For m As Integer = 10 To columnMonth
                            payDateHeader = wBook.Cells(4, m).Value
                            payDateAmount = CDate(dtAmountThb.Rows(n)("pay_date").ToString()).ToString("MMM yyyy")

                            If payDateHeader = payDateAmount Then
                                wBook.Cells(rowCount + 1, m).Value = CDec(dtAmountThb.Rows(n)("amount_thb").ToString())
                            End If
                        Next
                    End If
                Next

            'write border
                wBook.Cells(rowCount + 1, 1).Style.Border.Left.Style = ExcelBorderStyle.Thin
                For z As Integer = 1 To columnMonth - 1
                    wBook.Cells(rowCount + 1, z).Style.Border.Right.Style = ExcelBorderStyle.Thin
                    wBook.Cells(rowCount + 1, z).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Next
            rowCount += 1
            Next

            'Write Total
            wBook.Cells(rowCount + 1, 8).Value = "Total"
            wBook.Cells(rowCount + 1, 8).Style.Font.Bold = True
            wBook.Cells(rowCount + 1, 9).Formula = "=SUM(I5:I" & rowCount & ")"
             
            wBook.Cells(rowCount + 1, 8).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Left.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 9).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 9).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

            'Set Total   
            Dim Total As Decimal
            For y As Integer = 10 To columnMonth - 1
                Total = 0
                For q As Integer = 5 To rowCount
                    Total = Total + wBook.Cells(q, y).Value
                Next
                wBook.Cells(rowCount + 1, y).Value = Total
                wBook.Cells(rowCount + 1, y).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, y).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, y).AutoFitColumns()
            Next

            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=PlannedPaymentReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 06-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of menu
            objAction = objPermission.CheckPermission(43)
            ' set permission button
            btnExcel.Enabled = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page          
            Session("dtJobOrderForReport") = Nothing
            Session("dtInvoiceForReport") = Nothing
            Session("dtAmountThbForReport") = Nothing
            Session("dtSumAmountThbForReport") = Nothing
            Session("objPlannedPaymentDto") = Nothing
            Session("ddlYear") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
