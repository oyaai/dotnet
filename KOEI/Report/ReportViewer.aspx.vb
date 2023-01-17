#Region "Imports"
Imports CrystalDecisions.Web
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Net
#End Region

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Cost Table Detail
'	Class Name		    : KTAC06
'	Class Discription	: Export report from [Accounting] table
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 13-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class ReportViewer
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objMessage As New Common.Utilities.Message
    Private reportName As String = ""
    Private ReportReportPath As String = "RptFileSave/"
    Private ExportFileName As String
    Private iDate As DateTime = Now()
#Region "Even"
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            'Me.Page.Request.ServerVariables("REMOTE_ADDR") & ""

            If Not Page.IsPostBack Then
                DeleteOldDocument()
                reportName = Request.QueryString("ReportName") & ""
                Select Case reportName
                    Case "KTAC"
                        report_KTAC(reportName)
                    Case "KTAC05"
                        report_KTAC05(reportName)
                    Case "KTAC06"
                        report_KTAC06(reportName)
                    Case "KTAC_Voucher"
                        report_KTAC_Voucher(reportName)
                    Case "KTPU01"
                        'Purchase (Boon)
                        report_KTPU01(reportName)
                    Case "KTPU03" 'Payment Paid
                        report_KTPU03(reportName)
                    Case "KTJB02_Issue" 'Job Order
                        report_KTJB02_Issue(reportName)
                    Case "KTJB05" 'Confirm Receive
                        report_KTJB05(reportName)
                    Case "KTWH02" 'Working Hour
                        report_KTWH02(reportName)
                    Case "KTWH03" 'Working Hour report
                        report_KTWH03(reportName)
                    Case "KTPU06" 'Rating purchase
                        report_KTPU06(reportName)
                    Case "KTPU06_2" 'Yearly Rating purchase
                        report_KTPU06_2(reportName)
                    Case "KTPU09" 'Purchase History report
                        report_KTPU09(reportName)
                End Select
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: DeleteOldDocument()
    '	Discription		: Delete Temporary Report File
    '	Create User		: Pranitda Sroengklang
    '	Create Date		: 17-06-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Sub DeleteOldDocument()
        Dim FillPath As String = HttpContext.Current.Server.MapPath(ReportReportPath)
        If Not Directory.Exists(FillPath) Then Directory.CreateDirectory(FillPath)

        Dim DI As New DirectoryInfo(FillPath)

        For Each FI As FileInfo In DI.GetFiles()
            If FI.CreationTime < Today.AddDays(-7) Then
                Try
                    FI.Delete()
                Catch
                End Try
            End If
        Next
    End Sub
    '/**************************************************************
    '	Function name	: GetReportDocument()
    '	Discription		: Get Report Document
    '	Create User		: Pranitda Sroengklang
    '	Create Date		: 17-06-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Function GetReportDocument(ByVal ds As DataSet) As ReportDocument
        ' Crystal Report File Path
        Dim RptFilePath As String = Server.MapPath("../App_Data\RPT\" & reportName.Trim & ".rpt")

        '** Boon is change report 
        If reportName.Trim = "KTPU01" Then
            '** Boon select new report
            RptFilePath = Server.MapPath("../App_Data\RPT\KTPU01_1.rpt")
        End If

        ' Declare a new Crystal Report Document object and the report file into the report document
        Dim RptDoc = New ReportDocument()
        RptDoc.Load(RptFilePath)

        ' Set the datasource by getting the dataset from business layer and In our case business layer is getCustomerData function
        RptDoc.SetDataSource(ds)

        Return RptDoc
    End Function
    '/**************************************************************
    '	Function name	: WriteFile(MS, FileName)
    '	Discription		: Create File From MemoryStream
    '	Create User		: Pranitda Sroengklang
    '	Create Date		: 17-06-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Sub WriteFile(ByVal MS As MemoryStream, ByVal FileName As String)
        Dim Path As String = Server.MapPath(FileName)
        Dim file As New System.IO.FileStream(Path, FileMode.Create, System.IO.FileAccess.Write)

        Dim bytes(MS.Length) As Byte
        MS.Read(bytes, 0, MS.Length)
        file.Write(bytes, 0, bytes.Length)
        file.Close()
        MS.Close()
    End Sub
    '/**************************************************************
    '	Function name	: report_KTAC06
    '	Discription	    : Export Cost Table Detail
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTAC06(ByVal reportName As String)
        Dim ds As New DataSet("CostTableDetail")
        Dim table As New DataTable("CostTableDetail")
        Dim row As DataRow
        Dim report As New ReportDocument
        Dim dtCostTableDetail As New DataTable
        Try

            'Set data from searching page(KTAC06.aspx) into data table
            dtCostTableDetail = Session("dtCostTableDetail")

            'add column header
            With table
                .Columns.Add("job_order")
                .Columns.Add("Ie_name")
                .Columns.Add("account_date")
                .Columns.Add("account_type")
                .Columns.Add("vendor_name")
                .Columns.Add("income")
                .Columns.Add("expense")
                .Columns.Add("remark")
            End With

            If Not IsNothing(dtCostTableDetail) AndAlso dtCostTableDetail.Rows.Count > 0 Then
                For i As Integer = 0 To dtCostTableDetail.Rows.Count - 1
                    row = table.NewRow

                    'add row detail
                    row("job_order") = dtCostTableDetail.Rows(i)("job_order").ToString()
                    row("Ie_name") = dtCostTableDetail.Rows(i)("Ie_name").ToString()
                    row("account_date") = dtCostTableDetail.Rows(i)("account_date").ToString()
                    row("account_type") = dtCostTableDetail.Rows(i)("account_type").ToString()
                    row("vendor_name") = dtCostTableDetail.Rows(i)("vendor_name").ToString()
                    row("income") = dtCostTableDetail.Rows(i)("income").ToString()
                    row("expense") = dtCostTableDetail.Rows(i)("expense").ToString()
                    row("remark") = dtCostTableDetail.Rows(i)("remark").ToString()

                    'add row detail into table
                    table.Rows.Add(row)
                Next

            Else ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(table)

            Dim RptDoc As ReportDocument = GetReportDocument(ds)
            Dim ExportFileType As ExportFormatType
            Dim ContentType As String = ""

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"

            Dim MS As New MemoryStream
            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "CostTableDetailed"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTAC06", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTAC06
    '	Discription	    : Export Cost Table Overview
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTAC05(ByVal reportName As String)
        Dim ds As New DataSet("KTAC05DataSet") 'dataset table name
        Dim table As New DataTable("KTAC05DataSet")
        Dim row As DataRow
        Dim report As New ReportDocument
        Dim dtCostTableOverViewReport As New DataTable
        Try

            'Set data from searching page(KTAC06.aspx) into data table
            dtCostTableOverViewReport = Session("dtCostTableOverViewReport")

            'add column header
            With table
                .Columns.Add("job_order")
                .Columns.Add("account_year")
                .Columns.Add("account_month")
                .Columns.Add("ie_type")
                .Columns.Add("ie_code")
                .Columns.Add("ie_desc")
                .Columns.Add("sub_total")
            End With


            If Not IsNothing(dtCostTableOverViewReport) AndAlso dtCostTableOverViewReport.Rows.Count > 0 Then
                For i As Integer = 0 To dtCostTableOverViewReport.Rows.Count - 1
                    row = table.NewRow

                    'add row detail
                    row("job_order") = dtCostTableOverViewReport.Rows(i)("job_order").ToString()
                    row("account_year") = dtCostTableOverViewReport.Rows(i)("account_year").ToString()
                    row("account_month") = dtCostTableOverViewReport.Rows(i)("account_month").ToString()
                    row("ie_type") = dtCostTableOverViewReport.Rows(i)("ie_type").ToString()
                    row("ie_code") = dtCostTableOverViewReport.Rows(i)("ie_code").ToString()
                    row("ie_desc") = dtCostTableOverViewReport.Rows(i)("ie_desc").ToString()
                    row("sub_total") = dtCostTableOverViewReport.Rows(i)("sub_total").ToString()

                    'add row detail into table
                    table.Rows.Add(row)
                Next

            Else ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(table)

            Dim RptDoc As ReportDocument = GetReportDocument(ds)
            Dim ExportFileType As ExportFormatType
            Dim ContentType As String = ""

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"

            Dim MS As New MemoryStream
            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "CostTableOverview"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTAC05", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: report_KTAC
    '	Discription	    : Export Income/Payment
    '	Return Value	: nothing
    '	Create User	    : Komsan L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTAC(ByVal reportName As String)
        Dim ds As New DataSet("KTAC02DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim dtReportSort As New DataTable
        Dim report As New ReportDocument
        Dim objAccountSer As New Service.ImpAccountingService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from income  page(KTAC06.aspx) into data table
            dtReport = objAccountSer.GetTableReport(Session("dtReport"), "ReceiptDate ASC, JobOrder ASC")
            dtReport.TableName = "KTAC02DataSet"

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            If objUtility.GetQueryString("Page") = "KTAC02" Then
                ExportFileName = "Income"
            Else
                ExportFileName = "Payment"
            End If

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTAC", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: report_KTAC_Voucher
    '	Discription	    : Export Income/Payment Voucher
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTAC_Voucher(ByVal reportName As String)
        Dim ds As New DataSet("KTAC03_VoucherDataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from income  page(KTAC06.aspx) into data table
            dtReport = Session("dtVoucherRpt")
            dtReport.TableName = "KTAC03_VoucherDataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            If objUtility.GetQueryString("Page") = "KTAC02" Then
                ExportFileName = "Income"
            Else
                ExportFileName = "Payment"
            End If

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_Voucher_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTAC_Voucher", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: report_KTPU01
    '	Discription	    : Export Purchase to pdf
    '	Return Value	: Nothing
    '	Create User	    : Boonyarit
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU01(ByVal reportName As String)
        Dim ds As New DataSet("KTPU01DataSet") 'dataset table name
        Dim report As New ReportDocument
        Dim objPurchaseSer As New Service.ImpPurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility
        Dim intPurchaseId As Integer = objUtility.GetQueryString("ID")


        Try
            'Set data from purchase  page(KTPU01_Detail.aspx) into data_set
            ds = objPurchaseSer.GetDataReport(intPurchaseId)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "PurchaseOrder"

            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU01", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTPU03
    '	Discription	    : Export Income/Payment Paid
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU03(ByVal reportName As String)
        Dim ds As New DataSet("KTPU03DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objInvoice_PurchaseService As New Service.ImpInvoice_PurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search invoice (KTPU03.aspx) into data table
            'dtReport = objInvoice_PurchaseService.GetTableReport(Session("dtPaymentPaid"))
            dtReport = Session("dtPaymentPaid")
            dtReport.TableName = "KTPU03DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "PurchasePaidReport"

            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU03", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTJB02_Issue
    '	Discription	    : Export Job order
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 28-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTJB02_Issue(ByVal reportName As String)
        Dim ds As New DataSet("KTJB02_IssueDataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objJobOrderSer As New Service.ImpJobOrderService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search job order (KTJB01.aspx) into data table
            dtReport = objJobOrderSer.GetTableReport(Session("dtJobOrderReport"), Session("dtSumJobOrderReport"))
            dtReport.TableName = "KTJB02_IssueDataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "IssueJobOrderReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTJB02_Issue", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: report_KTWH02
    '	Discription	    : Export Working Hour
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTWH02(ByVal reportName As String)
        Dim ds As New DataSet("KTWH02DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objWorkingHourSer As New Service.ImpWorkingHourService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search Working Hour (KTWH02.aspx) into data table
            dtReport = objWorkingHourSer.GetTableReportSearch(Session("dtWorkingHourReportSearch"))
            dtReport.TableName = "KTWH02DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "WorkingHourReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTWH02", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: report_KTWH03
    '	Discription	    : Export Working Hour
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTWH03(ByVal reportName As String)
        Dim ds As New DataSet("KTWH03DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objWorkingHourSer As New Service.ImpWorkingHourService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search Working Hour (KTWH03.aspx) into data table
            dtReport = objWorkingHourSer.GetTableReport(Session("dtWorkingHourReport"), Session("status"))
            dtReport.TableName = "KTWH03DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "WorkingHourReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTWH03", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTPU06
    '	Discription	    : Export Rating Purchase report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU06(ByVal reportName As String)
        Dim ds As New DataSet("KTPU06DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objInvoice_PurchaseService As New Service.ImpInvoice_PurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search invoice (KTPU03.aspx) into data table
            dtReport = Session("dtGetVendorRatingReport")
            dtReport.TableName = "KTPU06DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "VendorRatingReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU06", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: report_KTPU06_2
    '	Discription	    : Export Yearly Rating Purchase report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU06_2(ByVal reportName As String)
        Dim ds As New DataSet("KTPU06_2DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objInvoice_PurchaseService As New Service.ImpInvoice_PurchaseService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search invoice (KTPU03.aspx) into data table
            dtReport = Session("dtYearGetVendorRatingReport")
            dtReport.TableName = "KTPU06_2DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "VendorYearlyRatingReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU06_2", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: report_KTPU09
    '	Discription	    : Export Purchase History report
    '	Return Value	: nothing
    '	Create User	    : Nisa S.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTPU09(ByVal reportName As String)
        Dim ds As New DataSet("KTPU09DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objPurchase_HistoryService As New Service.ImpPurchase_HistoryService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try

            'Set data from search Purchase History  (KTPU09.aspx) into data table
            dtReport = Session("dtPurchaseHistoryReport")
            dtReport.TableName = "KTPU09DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "PurchaseHistoryReport"

            'Dim FileName As String = ReportReportPath & ExportFileName & "_" & Rnd() & ".pdf"
            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTPU09", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: report_KTJB05
    '	Discription	    : Export Job Order Income
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub report_KTJB05(ByVal reportName As String)
        Dim ds As New DataSet("KTJB05DataSet") 'dataset table name
        Dim dtReport As New DataTable
        Dim report As New ReportDocument
        Dim objSaleInvoiceSer As New Service.ImpSale_InvoiceService
        Dim RptDoc As ReportDocument
        Dim ExportFileType As ExportFormatType
        Dim ContentType As String = ""
        Dim MS As New MemoryStream
        Dim objUtility As New Common.Utilities.Utility

        Try
            'Session("dtValuesBankFree")
            'Set data from confirm receive (KTJB05.aspx) into data table
            'dtReport = objSaleInvoiceSer.GetTableConfirmReport(Session("dtJobOrderReport"), Session("objSumDataReportDto"), Session("VoucherNoConfirm"), Session("dtValuesBankFree"))
            dtReport = objSaleInvoiceSer.GetTableConfirmReport(Session("dtJobOrderReport"), Session("objSumDataReportDto"), Session("VoucherNoConfirm"), Session("objSumBankFee"))

            dtReport.TableName = "KTJB05DataSet"
            'add column header

            If dtReport.Rows.Count <= 0 Then ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                Exit Sub
            End If

            'add datatable into dataset
            ds.Tables.Add(dtReport)

            RptDoc = GetReportDocument(ds)

            ExportFileType = ExportFormatType.PortableDocFormat
            ContentType = "application/pdf"


            MS = RptDoc.ExportToStream(ExportFileType)

            ExportFileName = "JobOrderIncomeReport"

            Dim FileName As String = ReportReportPath & ExportFileName & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".pdf"
            WriteFile(MS, FileName)
            Response.Redirect(FileName)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("report_KTJB05", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
