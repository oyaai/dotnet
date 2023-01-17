#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IAcc_PaymentService
'	Class Discription	: Interface class Accounting payment service
'	Create User 		: Wasan D.
'	Create Date		    : 09-08-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Utils
Imports System.IO
Imports System.Data
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Web.Configuration

Namespace Service
    Public Class ImpAcc_PaymentService
        Implements IAcc_PaymentService

        Private objLog As New Common.Logs.Log
        Private objAccountingEnt As New Entity.ImpAcc_PaymentEntity
        Private objUtility As New Common.Utilities.Utility

#Region "Function"
        '/**************************************************************
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForVoucherReport(ByVal voucherList As String) As System.Data.DataTable Implements IAcc_PaymentService.GetDataForVoucherReport
            ' set default return value
            GetDataForVoucherReport = New DataTable
            Try
                Dim listVoucherDto As New List(Of Entity.ImpAcc_PaymentEntity)
                Dim listQuery As IEnumerable(Of Entity.ImpAcc_PaymentEntity)
                Dim dtReport As New DataTable
                Dim dr As DataRow

                listVoucherDto = objAccountingEnt.GetDataForVoucherReport(voucherList)
                listQuery = listVoucherDto.OrderBy(Function(voucherNo) voucherNo.voucherNoAsInt)
                ' set header columns
                With dtReport
                    .Columns.Add("voucher_no", System.Type.GetType("System.Int32"))
                    .Columns.Add("printdate")
                    .Columns.Add("account_date")
                    .Columns.Add("account_type")
                    .Columns.Add("vendorName")
                    .Columns.Add("account_name")
                    .Columns.Add("account_no")
                    .Columns.Add("cheque_no")
                    .Columns.Add("bank")
                    .Columns.Add("cheque_date")
                    .Columns.Add("subtotal")
                    .Columns.Add("vat")
                    .Columns.Add("wt")
                    .Columns.Add("total")
                    .Columns.Add("vat_percent")
                    .Columns.Add("wt_percent")
                    .Columns.Add("textBaht")
                End With

                ' loop set data to table report
                For Each values As Entity.ImpAcc_PaymentEntity In listQuery
                    dr = dtReport.NewRow
                    With values
                        dr.Item("voucher_no") = .voucherNoAsInt
                        dr.Item("printdate") = objUtility.String2Date(.dateNow, "yyyyMMdd").ToString("dd/MMM/yyyy")
                        dr.Item("account_date") = objUtility.String2Date(.receiptDate, "yyyyMMdd").ToString("dd/MMM/yyyy")
                        dr.Item("account_type") = .accountType
                        dr.Item("vendorName") = .vendorName
                        dr.Item("account_name") = .accountName
                        dr.Item("account_no") = .accountNo
                        dr.Item("cheque_no") = .chequeNo
                        dr.Item("bank") = .bank
                        dr.Item("cheque_date") = .chequeDate
                        dr.Item("subtotal") = CDec(.subtotal).ToString("#,##0.00")
                        dr.Item("vat") = CDec(.vatAmount).ToString("#,##0.00")
                        dr.Item("wt") = CDec(.wtAmount).ToString("#,##0.00")
                        dr.Item("total") = CDec(.total).ToString("#,##0.00")
                        dr.Item("vat_percent") = "Vat " & .vat & "%"
                        dr.Item("wt_percent") = "W/T " & .wt & "%"
                        dr.Item("textBaht") = Utils.BahtText(CDbl(.total))
                    End With
                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                GetDataForVoucherReport = dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForVoucherReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDataForWTReport
        '	Discription	    : Get table wit report
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForWTReport(ByVal voucherList As String) As System.Data.DataTable Implements IAcc_PaymentService.GetDataForWTReport
            ' set default return value
            GetDataForWTReport = New DataTable
            Try
                Dim listVoucherDto As New List(Of Entity.ImpAcc_PaymentEntity)
                Dim dtReport As New DataTable
                Dim dr As DataRow

                listVoucherDto = objAccountingEnt.GetDataForWTReport(voucherList)
                If listVoucherDto.Count > 0 Then
                    ' set header columns
                    With dtReport
                        .Columns.Add("voucher_no")
                        .Columns.Add("accID")
                        .Columns.Add("vendor_name")
                        .Columns.Add("vendor_address")
                        .Columns.Add("vendor_type1")
                        .Columns.Add("vendor_type2")
                        .Columns.Add("vendor_type2_no")
                        .Columns.Add("account_date")
                        .Columns.Add("subtotal")
                        .Columns.Add("wt_percent")
                        .Columns.Add("wt_amount")
                        .Columns.Add("wt_type")
                        .Columns.Add("textBaht")
                    End With

                    ' loop set data to table report
                    For Each values As Entity.ImpAcc_PaymentEntity In listVoucherDto
                        dr = dtReport.NewRow
                        With values
                            dr.Item("voucher_no") = .voucherNo
                            dr.Item("accID") = .accID
                            dr.Item("vendor_name") = .vendorName
                            dr.Item("vendor_address") = .vendorAddress
                            dr.Item("vendor_type1") = .vendor_type1
                            dr.Item("vendor_type2") = .vendor_type2
                            dr.Item("vendor_type2_no") = .vendor_type2_no
                            dr.Item("account_date") = objUtility.String2Date(.receiptDate, "yyyyMMdd").ToString("dd-MMM-yyyy")
                            dr.Item("subtotal") = CDec(.subtotal).ToString("#,##0.00")
                            dr.Item("wt_percent") = .wt & "%"
                            dr.Item("wt_amount") = CDec(.wtAmount).ToString("#,##0.00")
                            dr.Item("wt_type") = .wtType
                            dr.Item("textBaht") = Utils.BahtText(CDbl(.wtAmount))
                        End With
                        dtReport.Rows.Add(dr)
                    Next
                    ' return new datatable
                    GetDataForWTReport = dtReport
                Else
                    GetDataForWTReport = Nothing
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForWTReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetDataForWTReportV2
        '	Discription	    : Get table wit report
        '	Return Value	: Datatable
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDataForWTReportV2(ByVal voucherList As String) As System.Data.DataTable Implements IAcc_PaymentService.GetDataForWTReportV2
            ' set default return value
            GetDataForWTReportV2 = New DataTable
            Try
                Dim listVoucherDto As New List(Of Entity.ImpAcc_PaymentEntity)
                Dim dtReport As New DataTable
                Dim dr As DataRow

                listVoucherDto = objAccountingEnt.GetDataForWTReportV2(voucherList)
                If listVoucherDto.Count > 0 Then
                    ' set header columns
                    With dtReport
                        .Columns.Add("receiptYear")
                        .Columns.Add("receiptMonth")
                        .Columns.Add("accID")
                        .Columns.Add("vendor_name")
                        .Columns.Add("vendor_address")
                        .Columns.Add("vendor_type1")
                        .Columns.Add("vendor_type2")
                        .Columns.Add("vendor_type2_no")
                        .Columns.Add("account_date")
                        .Columns.Add("subtotal")
                        .Columns.Add("wt_percent")
                        .Columns.Add("wt_amount")
                        .Columns.Add("wt_type")
                    End With

                    ' loop set data to table report
                    For Each values As Entity.ImpAcc_PaymentEntity In listVoucherDto
                        dr = dtReport.NewRow
                        With values
                            dr.Item("receiptYear") = .receiptYear
                            dr.Item("receiptMonth") = .receiptMonth
                            dr.Item("accID") = .accID
                            dr.Item("vendor_name") = .vendorName
                            dr.Item("vendor_address") = .vendorAddress
                            dr.Item("vendor_type1") = .vendor_type1
                            dr.Item("vendor_type2") = .vendor_type2
                            dr.Item("vendor_type2_no") = .vendor_type2_no
                            dr.Item("account_date") = objUtility.String2Date(.receiptDate, "yyyy-MM-dd").ToString("dd-MMM-yyyy")
                            dr.Item("subtotal") = CDec(.subtotal).ToString("#,##0.00")
                            dr.Item("wt_percent") = .wt & "%"
                            dr.Item("wt_amount") = CDec(.wtAmount).ToString("#,##0.00")
                            dr.Item("wt_type") = .wtType
                        End With
                        dtReport.Rows.Add(dr)
                    Next
                    ' return new datatable
                    GetDataForWTReportV2 = dtReport
                Else
                    GetDataForWTReportV2 = Nothing
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDataForWTReportV2(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: checkVendorType2
        '	Discription	    : check vendorType2
        '	Return Value	: string as file name
        '	Create User	    : Wasan D.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkVendorType2(ByVal vendorType2 As String) As String
            checkVendorType2 = Nothing
            Try
                Select Case CInt(vendorType2.Trim)
                    Case 0 : checkVendorType2 = "WithholdingTaxReportForPerson.xlsx"
                    Case 1 : checkVendorType2 = "WithholdingTaxReportForCompany.xlsx"
                    Case Else : checkVendorType2 = Nothing
                End Select
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkVendorType2(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ExcelPaymentWTReport
        '	Discription	    : Export data Payment Withholding Tax report
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ExcelPaymentWTReport(ByVal dtPaymentWT As DataTable, ByVal callFromPage As String) As String Implements IAcc_PaymentService.ExcelPaymentWTReport
            ExcelPaymentWTReport = Nothing
            Try
                ' variable
                Dim pck As ExcelPackage = Nothing
                Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
                Dim newFileName As String
                Dim objComm As New Common.Utilities.Utility
                Dim strPath As String
                If dtPaymentWT.Rows.Count > 0 Then

                    strPath = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("ReportPath") & checkVendorType2(dtPaymentWT.Rows(0).Item("vendor_type2").ToString))
                    newFileName = callFromPage & "_WT_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".xlsx"
                    pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(strPath)))
                    For Each row As DataRow In dtPaymentWT.Rows
                        ' Copy sheet from Template sheet.
                        pck.Workbook.Worksheets.Copy("Template", "WithholdingTaxReport")
                        ' Select work sheet.
                        wBook = pck.Workbook.Worksheets("WithholdingTaxReport")
                        'With wBook
                        '    ' real document
                        '    .Cells("C14").Value = row("vendor_name")
                        '    .Cells("C16").Value = row("vendor_address")
                        '    .Cells(IIf(dtPaymentWT.Rows(0).Item("vendor_type2").ToString.Trim = "0", "J12", "J14")).Value = row("vendor_type2_no")
                        '    '.Cells("I48").Value = row("wt_percent") & row("account_date")
                        '    .Cells("I48").Value = row("account_date")
                        '    .Cells("J48").Value = CDec(row("subtotal"))
                        '    .Cells("L48").Value = CDec(row("wt_amount"))
                        '    ' copy document
                        '    .Cells("C73").Value = row("vendor_name")
                        '    .Cells("C75").Value = row("vendor_address")
                        '    .Cells(IIf(dtPaymentWT.Rows(0).Item("vendor_type2").ToString.Trim = "0", "J71", "J73")).Value = row("vendor_type2_no")
                        '    '.Cells("I107").Value = row("wt_percent") & row("account_date")
                        '    .Cells("I107").Value = row("account_date")
                        '    .Cells("J107").Value = CDec(row("subtotal"))
                        '    .Cells("L107").Value = CDec(row("wt_amount"))
                        'End With

                        While dtPaymentWT.Rows.Count

                        End While

                    Next
                    ' Delete Template sheet.
                    pck.Workbook.Worksheets.Delete("Template")
                    pck.Workbook.Worksheets.Item(1).Select("A1")
                    ' Save file as new name.
                    Dim fileStream As Stream = File.Create(HttpContext.Current.Server.MapPath("../Report/RptFileSave/" & newFileName))
                    pck.SaveAs(fileStream)
                    fileStream.Close()
                    ExcelPaymentWTReport = "../Report/RptFileSave/" & newFileName
                Else
                    ExcelPaymentWTReport = Nothing
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("ExcelPaymentWTReport", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: checkVendorType2
        '	Discription	    : check vendorType2
        '	Return Value	: string as file name
        '	Create User	    : Wasan D.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function checkVendorType2ForWTV2(ByVal vendorType2 As String) As String
            checkVendorType2ForWTV2 = Nothing
            Try
                Select Case CInt(vendorType2.Trim)
                    Case 0 : checkVendorType2ForWTV2 = "ภงด.3"
                    Case 1 : checkVendorType2ForWTV2 = "ภงด.53"
                    Case Else : checkVendorType2ForWTV2 = Nothing
                End Select
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("checkVendorType2ForWTV2(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ExcelPaymentWTReport
        '	Discription	    : Export data Payment Withholding Tax report
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 09-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function ExcelPaymentWTReportV2(ByVal dtPaymentWT As DataTable, ByVal callFromPage As String) As String Implements IAcc_PaymentService.ExcelPaymentWTReportV2
            ExcelPaymentWTReportV2 = Nothing
            Try
                ' variable
                Dim fileStream As Stream
                Dim pck As ExcelPackage = Nothing
                Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
                Dim objComm As New Common.Utilities.Utility
                Dim sheetName As String = Nothing
                Dim sheetNameType, newFileName As String
                Dim strPath As String
                Dim strYear, strMonth, MonthInThai As String
                Dim i As Integer = 4
                Dim ThaiCulture As New System.Globalization.CultureInfo("th-TH")
                Dim strColName() As String = {"vendor_name", "vendor_type2_no", "vendor_address", "account_date", "wt_type", "wt_percent", "subtotal", "wt_amount"}
                If dtPaymentWT.Rows.Count > 0 Then
                    strPath = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("ReportPath") & "WithholdingTaxReportV2.xlsx")
                    newFileName = callFromPage & "_WT2_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".xlsx"
                    pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(strPath)))
                    sheetNameType = checkVendorType2ForWTV2(dtPaymentWT.Rows(0).Item("vendor_type2").ToString)
                    strYear = Nothing
                    strMonth = Nothing
                    For Each row As DataRow In dtPaymentWT.Rows
                        If row.Item("receiptYear").ToString.Trim <> strYear Or row.Item("receiptMonth").ToString.Trim <> strMonth Then
                            ' Set new sheet name.
                            sheetName = sheetNameType & "_" & CDate(row("account_date")).ToString("MMM") & "_" & CDate(row("account_date")).ToString("yyyy")
                            ' Copy sheet from Template sheet.
                            pck.Workbook.Worksheets.Copy("temp", sheetName)
                            ' Select work sheet.
                            wBook = pck.Workbook.Worksheets(sheetName)
                            strYear = row.Item("receiptYear").ToString.Trim
                            strMonth = row.Item("receiptMonth").ToString.Trim
                            MonthInThai = CDate(row("account_date")).ToString("MMMM", ThaiCulture.DateTimeFormat)
                            'set header
                            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)
                            wBook.Cells("A1").Value = "รายงานภาษีหัก ณ ที่จ่ายประจำเดือน " & MonthInThai & " " & CInt(strYear) + 543
                            wBook.Cells("A2").Value = sheetNameType
                            wBook.PrinterSettings.RepeatRows = wBook.Cells("$1:$3")
                            i = 4
                            wBook.InsertRow(i + 1, 1, 4)
                            wBook.DeleteRow(i, 1)
                        Else
                            wBook.InsertRow(i, 1, 4)
                        End If
                        For ii = 1 To strColName.Count
                            If ii = 7 Or ii = 8 Then
                                wBook.Cells(i, ii).Value = CDbl(row(strColName(ii - 1)))
                            Else
                                wBook.Cells(i, ii).Value = row(strColName(ii - 1))
                            End If
                        Next
                        wBook.Cells("G" & i + 2).Formula = "=SUM(G4:G" & i & ")"
                        wBook.Cells("H" & i + 2).Formula = "=SUM(H4:H" & i & ")"
                        i += 1
                    Next
                    pck.Workbook.Worksheets.Item("tempSheet").Hidden = eWorkSheetHidden.Hidden
                    pck.Workbook.Worksheets.Item("temp").Hidden = eWorkSheetHidden.Hidden
                    pck.Workbook.Worksheets.Item(3).Select("A1")
                    ' Save file as new name.
                    fileStream = File.Create(HttpContext.Current.Server.MapPath("../Report/RptFileSave/" & newFileName))
                    pck.SaveAs(fileStream)
                    fileStream.Close()
                    ExcelPaymentWTReportV2 = "../Report/RptFileSave/" & newFileName
                Else
                    ExcelPaymentWTReportV2 = Nothing
                End If
            Catch ex As Exception
                ' Write error log
                objLog.ErrorLog("ExcelPaymentWTReportV2", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region
    End Class
End Namespace

