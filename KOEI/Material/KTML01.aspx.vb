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
'	Package Name	    : Material
'	Class Name		    : Material_KTML01
'	Class Discription	: Webpage for Material
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Material_KTML01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objMaterialSer As New Service.ImpMaterialService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTML01 : Material")
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
    '	Create Date	    : 03-07-2013
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
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click

        Try
            'Check input Criteria data
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()

            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            SetDataToSession()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : export data to excel file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnExcel.Click
        Try
            Dim dtMaterialReport As New DataTable
            Dim dtSumMaterialReport As New DataTable

            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data for export excel report
            SearchDataReport()

            ' get table object from session 
            dtMaterialReport = Session("dtMaterialReport")
            dtSumMaterialReport = Session("dtSumMaterialReport")

            If Not IsNothing(dtMaterialReport) AndAlso dtMaterialReport.Rows.Count > 0 Then
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
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' set search text to session
            SetSessionToItem()

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check format date of field Delivery Date From
            If txtDelivertyDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDelivertyDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Delivery Date To
            If txtDelivertyDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDelivertyDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check Delivery Date From > Delivery Date To
            If txtDelivertyDateFrom.Text.Trim <> "" And txtDelivertyDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtDelivertyDateFrom.Text.Trim, txtDelivertyDateTo.Text.Trim) = False Then
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
    ' Function name   : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Suwishaya L.
    ' Create Date     : 03-07-2013
    ' Update User     : Suwishaya L.
    ' Update Date     : 26-09-2013
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
            Dim DT As DataTable = Nothing
            Dim sumDT As DataTable = Nothing
            Dim rowCount As Integer = 3
            Dim amount As Decimal
            Dim qtyIn As Decimal
            Dim qtyOut As Decimal
            Dim qtyLeft As Decimal
            Dim sumAmount As Decimal = 0

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "MaterialListReport.xlsx"))))
            wBook = pck.Workbook.Worksheets(1)

            DT = Session("dtMaterialReport")
            sumDT = Session("dtSumMaterialReport")

            'set header
            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)

            'Header already have in templete file
            For i As Integer = 0 To DT.Rows.Count - 1
                amount = 0
                qtyIn = 0
                qtyOut = 0
                qtyLeft = 0
                If DT.Rows(i)("amount").ToString.Length > 0 Then amount = DT.Rows(i)("amount")
                If DT.Rows(i)("qty_in").ToString.Length > 0 Then qtyIn = DT.Rows(i)("qty_in")
                If DT.Rows(i)("qty_out").ToString.Length > 0 Then qtyOut = DT.Rows(i)("qty_out")
                If DT.Rows(i)("qty_left").ToString.Length > 0 Then qtyLeft = DT.Rows(i)("qty_left")
                sumAmount = sumAmount + amount

                'set data to cell
                wBook.Cells(rowCount + 1, 1).Value = DT.Rows(i)("job_order").ToString()
                wBook.Cells(rowCount + 1, 2).Value = DT.Rows(i)("po_no").ToString()
                wBook.Cells(rowCount + 1, 3).Value = DT.Rows(i)("invoice_no").ToString()
                'wBook.Cells(rowCount + 1, 4).Value = DT.Rows(i)("amount").ToString()
                wBook.Cells(rowCount + 1, 4).Value = amount
                wBook.Cells(rowCount + 1, 5).Value = DT.Rows(i)("vendor").ToString()
                wBook.Cells(rowCount + 1, 6).Value = DT.Rows(i)("item_name").ToString()
                wBook.Cells(rowCount + 1, 7).Value = DT.Rows(i)("delivery_date_in").ToString()
                'wBook.Cells(rowCount + 1, 8).Value = DT.Rows(i)("qty_in").ToString()
                wBook.Cells(rowCount + 1, 8).Value = qtyIn
                wBook.Cells(rowCount + 1, 9).Value = DT.Rows(i)("delivery_date_out").ToString()
                'wBook.Cells(rowCount + 1, 10).Value = DT.Rows(i)("qty_out").ToString()
                'wBook.Cells(rowCount + 1, 11).Value = DT.Rows(i)("qty_left").ToString()
                wBook.Cells(rowCount + 1, 10).Value = qtyOut
                wBook.Cells(rowCount + 1, 11).Value = qtyLeft

                'write border
                wBook.Cells(rowCount + 1, 1).Style.Border.Left.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 1).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 2).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 3).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 4).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 5).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 6).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 7).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 8).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 9).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 10).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 11).Style.Border.Right.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 1).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 2).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 3).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 4).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 5).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 6).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 7).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 8).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 9).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 10).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                wBook.Cells(rowCount + 1, 11).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

                'Add new row
                rowCount += 1
            Next

            'Write border
            wBook.Cells(rowCount + 1, 3).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 3).Style.Border.Left.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 3).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 4).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 4).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

            'Summary footer report 
            wBook.Cells(rowCount + 1, 3).Value = "Total Amount"
            wBook.Cells(rowCount + 1, 3).Style.Font.Bold = True
            wBook.Cells(rowCount + 1, 4).Value = sumAmount 'sumDT.Rows(0)("sum_amount").ToString()
            'AutoFit column PO No and Invoice No.
            wBook.Column(1).Width = 7
            wBook.Column(2).AutoFit()
            wBook.Column(3).AutoFit()
            wBook.Column(4).AutoFit()
            wBook.Column(5).Width = 15
            wBook.Column(6).Width = 15
            wBook.Column(7).AutoFit()
            wBook.Column(8).AutoFit()
            wBook.Column(9).AutoFit()
            wBook.Column(10).AutoFit()
            wBook.Column(11).AutoFit()


            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=MaterialListReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Material data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtMaterial As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetMaterialList from Material Service
            dtMaterial = objMaterialSer.GetMaterialList(Session("objMaterialDto"))
            ' set table object to session
            Session("dtMaterial") = dtMaterial
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search Material data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    : Suwishaya L.
    '	Update Date	    : 26-09-2013
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtMaterialReport As New DataTable
            Dim dtSumMaterialReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetSumMonthlySaleReport from MaterialService
            dtMaterialReport = objMaterialSer.GetMaterialListReport(Session("objMaterialDto"))
            'Delete 2013/09/26 
            ' call function GetSumSumMonthlySaleReport from Material Service
            'dtSumMaterialReport = objMaterialSer.GetSumMaterialListReport(Session("objMaterialDto"))

            ' set table object to session
            Session("dtMaterialReport") = dtMaterialReport
            Session("dtSumMaterialReport") = dtSumMaterialReport

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Material dto object
            Dim objMaterialDto As New Dto.MaterialDto

            Dim deliveryDateFrom As String = ""
            Dim deliveryDateTo As String = ""
            Dim arrDeliveryDateFrom() As String = Split(txtDelivertyDateFrom.Text.Trim(), "/")
            Dim arrDeliveryDateTo() As String = Split(txtDelivertyDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objMaterialDto

                'Set delivery date to format yyymmdd
                If UBound(arrDeliveryDateFrom) > 0 Then
                    deliveryDateFrom = arrDeliveryDateFrom(2) & arrDeliveryDateFrom(1) & arrDeliveryDateFrom(0)
                End If
                If UBound(arrDeliveryDateTo) > 0 Then
                    deliveryDateTo = arrDeliveryDateTo(2) & arrDeliveryDateTo(1) & arrDeliveryDateTo(0)
                End If

                .job_order = txtJobOrder.Text.Trim
                .vendor = txtVendorName.Text.Trim
                .invoice_no = txtInvoiceNo.Text.Trim
                .po_no = txtPONo.Text.Trim
                .item_name = txtItemName.Text.Trim
                .delivery_date_from = deliveryDateFrom
                .delivery_date_to = deliveryDateTo

            End With

            ' set dto object to session
            Session("objMaterialDto") = objMaterialDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtJobOrder") = txtJobOrder.Text.Trim
            Session("txtVendorName") = txtVendorName.Text.Trim
            Session("txtInvoiceNo") = txtInvoiceNo.Text.Trim
            Session("txtPONo") = txtPONo.Text.Trim
            Session("txtItemName") = txtItemName.Text.Trim
            Session("txtDelivertyDateFrom") = txtDelivertyDateFrom.Text.Trim
            Session("txtDelivertyDateTo") = txtDelivertyDateTo.Text.Trim

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtMaterial As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtMaterial = Session("dtMaterial")

            ' check record for display
            If Not IsNothing(dtMaterial) AndAlso dtMaterial.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtMaterial)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptMaterial.DataSource = pagedData
                rptMaterial.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtMaterial.Rows.Count)
            Else
                ' case not exist data
                ' show message box     
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptMaterial.DataSource = Nothing
                rptMaterial.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page

            Session("txtJobOrder") = Nothing
            Session("txtVendorName") = Nothing
            Session("txtInvoiceNo") = Nothing
            Session("txtPONo") = Nothing
            Session("txtItemName") = Nothing
            Session("txtDelivertyDateFrom") = Nothing
            Session("txtDelivertyDateTo") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSessionToItem
    '	Discription	    : Set data to item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set search text to session
            txtJobOrder.Text = Session("txtJobOrder")
            txtVendorName.Text = Session("txtVendorName")
            txtInvoiceNo.Text = Session("txtInvoiceNo")
            txtPONo.Text = Session("txtPONo")
            txtItemName.Text = Session("txtItemName")
            txtDelivertyDateFrom.Text = Session("txtDelivertyDateFrom")
            txtDelivertyDateTo.Text = Session("txtDelivertyDateTo")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 03-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Material menu
            objAction = objPermission.CheckPermission(20)
            ' set permission Create 
            btnSearch.Enabled = objAction.actList
            btnExcel.Enabled = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
