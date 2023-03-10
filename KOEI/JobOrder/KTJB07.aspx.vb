Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Job Order
'	Class Name		    : JobOrder_KTJB07
'	Class Discription	: Webpage for Income Job Order
'	Create User 		: Suwishaya L.
'	Create Date		    : 01-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB07
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objIncomeJobOrderSer As New Service.ImpIncome_JobOrderService
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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB07 : Income")
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
    '	Create Date	    : 01-07-2013
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
    '	Create Date	    : 01-07-2013
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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnExcel.Click
        Try
            Dim dtIncomeReport As New DataTable
            Dim dtSumIncomeReport As New DataTable

            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data for export excel report
            SearchDataReport()

            ' get table object from session 
            dtIncomeReport = Session("dtIncomeReport")
            dtSumIncomeReport = Session("dtSumIncomeReport")

            If Not IsNothing(dtIncomeReport) AndAlso dtIncomeReport.Rows.Count > 0 Then
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

    '/**************************************************************
    '	Function name	: rptIncome_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptIncome_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptIncome.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptIncome_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptIncome_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptIncome_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptIncome.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())

            ' set ID to session
            Session("intID") = intID

            Select Case e.CommandName
                Case "Detail"
                    'redirect to KTJB05_Detail
                    objMessage.ShowPagePopup("KTJB05_Detail.aspx?id=" & intID & "&menuId=4", 900, 950, "", True)
            End Select

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptIncome_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptIncome_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptIncome_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptIncome.ItemDataBound
        Try
            ' object link button          
            Dim lblAmount As New Label

            ' find linkbutton and assign to variable        
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            'set permission amount
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
            End If

            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptIncome_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the id keys in ViewState
    ReadOnly Property hashID() As Hashtable
        Get
            If IsNothing(ViewState("hashID")) Then
                ViewState("hashID") = New Hashtable()
            End If
            Return CType(ViewState("hashID"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            'Check job order from > job order to
            If txtJobOrderFrom.Text.Trim.Length > 0 And txtJobOrderTo.Text.Trim.Length > 0 Then
                If txtJobOrderFrom.Text > txtJobOrderTo.Text Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_005"))
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date From
            If txtIssueDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date To
            If txtIssueDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Receipt Date From
            If txtReceiptDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiptDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Receipt Date To
            If txtReceiptDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiptDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check Issue Date From > Issue Date To
            If txtIssueDateFrom.Text.Trim <> "" And txtIssueDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtIssueDateFrom.Text.Trim, txtIssueDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            'Check Finished Date From > Finished Date To
            If txtReceiptDateFrom.Text.Trim <> "" And txtReceiptDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtReceiptDateFrom.Text.Trim, txtReceiptDateTo.Text.Trim) = False Then
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
    ' Function name : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Suwishaya L.
    ' Create Date     : 01-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
            Dim DT As DataTable = Nothing
            Dim sumDT As DataTable = Nothing
            Dim rowCount As Integer = 3 

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "MonthlySaleReport.xlsx"))))
            wBook = pck.Workbook.Worksheets(1)

            'set header
            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)

            DT = Session("dtIncomeReport")
            sumDT = Session("dtSumIncomeReport")

            'Header already have in templete file
            For i As Integer = 0 To DT.Rows.Count - 1
                wBook.Cells(rowCount + 1, 1).Value = DT.Rows(i)("issue_date").ToString()
                wBook.Cells(rowCount + 1, 2).Value = DT.Rows(i)("invoice_no").ToString()
                wBook.Cells(rowCount + 1, 3).Value = DT.Rows(i)("customer").ToString()
                wBook.Cells(rowCount + 1, 4).Value = DT.Rows(i)("job_order").ToString()
                wBook.Cells(rowCount + 1, 5).Value = CDbl(DT.Rows(i)("actual_rate").ToString())

                'set amount follow permission      
                If Not Session("actAmount") Then
                    wBook.Cells(rowCount + 1, 6).Value = "******"
                    wBook.Cells(rowCount + 1, 7).Value = "******"
                    wBook.Cells(rowCount + 1, 8).Value = "******"
                Else
                    wBook.Cells(rowCount + 1, 6).Value = CDbl(DT.Rows(i)("price").ToString())
                    wBook.Cells(rowCount + 1, 7).Value = CDbl(DT.Rows(i)("vat").ToString())
                    wBook.Cells(rowCount + 1, 8).Value = CDbl(DT.Rows(i)("amount").ToString())
                End If

                wBook.Cells(rowCount + 1, 9).Value = DT.Rows(i)("remark").ToString()

                'write border
                wBook.Cells(rowCount + 1, 1).Style.Border.Left.Style = ExcelBorderStyle.Thin
                For z As Integer = 1 To 9
                    wBook.Cells(rowCount + 1, z).Style.Border.Right.Style = ExcelBorderStyle.Thin
                    wBook.Cells(rowCount + 1, z).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Next

                rowCount += 1
            Next

            'Summary footer report 
            wBook.Cells(rowCount + 1, 5).Value = "Total"
            wBook.Cells(rowCount + 1, 5).Style.Font.Bold = True
            'set amount follow permission 
            If Not Session("actAmount") Then
                wBook.Cells(rowCount + 1, 6).Value = "******"
                wBook.Cells(rowCount + 1, 7).Value = "******"
                wBook.Cells(rowCount + 1, 8).Value = "******"
            Else
                wBook.Cells(rowCount + 1, 6).Value = CDbl(sumDT.Rows(0)("sum_price").ToString())
                wBook.Cells(rowCount + 1, 7).Value = CDbl(sumDT.Rows(0)("sum_vat").ToString())
                wBook.Cells(rowCount + 1, 8).Value = CDbl(sumDT.Rows(0)("sum_amount").ToString())
            End If

            'write border footer
            wBook.Cells(rowCount + 1, 5).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 5).Style.Border.Left.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 5).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 6).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 6).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 7).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 7).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=MonthlySaleReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Income job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtIncomeJobOrder As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetIncomeList from JobOrderService
            dtIncomeJobOrder = objIncomeJobOrderSer.GetIncomeList(Session("objIncomeJobOrderDto"))
            ' set table object to session
            Session("dtIncomeJobOrder") = dtIncomeJobOrder
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search Income job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtIncomeReport As New DataTable
            Dim dtSumIncomeReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetSumMonthlySaleReport from Income_JobOrderService
            dtIncomeReport = objIncomeJobOrderSer.GetMonthlySaleReport(Session("objIncomeJobOrderDto"))
            ' call function GetSumSumMonthlySaleReport from Income_JobOrderService
            dtSumIncomeReport = objIncomeJobOrderSer.GetSumMonthlySaleReport(Session("objIncomeJobOrderDto"))

            ' set table object to session
            Session("dtIncomeReport") = dtIncomeReport
            Session("dtSumIncomeReport") = dtSumIncomeReport

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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Job Order dto object
            Dim objIncomeJobOrderDto As New Dto.IncomeJobOrderDto
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim receiptDateDateFrom As String = ""
            Dim receiptDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtIssueDateFrom.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtIssueDateTo.Text.Trim(), "/")
            Dim arrReceiptDateFrom() As String = Split(txtReceiptDateFrom.Text.Trim(), "/")
            Dim arrReceiptDateTo() As String = Split(txtReceiptDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objIncomeJobOrderDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If
                'Set receipt date to format yyymmdd
                If UBound(arrReceiptDateFrom) > 0 Then
                    receiptDateDateFrom = arrReceiptDateFrom(2) & arrReceiptDateFrom(1) & arrReceiptDateFrom(0)
                End If
                If UBound(arrReceiptDateTo) > 0 Then
                    receiptDateDateTo = arrReceiptDateTo(2) & arrReceiptDateTo(1) & arrReceiptDateTo(0)
                End If

                .job_order_from_search = txtJobOrderFrom.Text.Trim
                .job_order_to_search = txtJobOrderTo.Text.Trim
                .customer_search = txtCustomer.Text.Trim
                .issue_date_from_search = issueDateDateFrom
                .issue_date_to_search = issueDateDateTo
                .receipt_date_from_search = receiptDateDateFrom
                .receipt_date_to_search = receiptDateDateTo
                .invoice_no_search = txtInvoiceNo.Text.Trim

            End With

            ' set dto object to session
            Session("objIncomeJobOrderDto") = objIncomeJobOrderDto

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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtJobOrderFrom") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderTo") = txtJobOrderTo.Text.Trim
            Session("txtCustomer") = txtCustomer.Text.Trim
            Session("txtIssueDateFrom") = txtIssueDateFrom.Text.Trim
            Session("txtIssueDateTo") = txtIssueDateTo.Text.Trim
            Session("txtReceiptDateFrom") = txtReceiptDateFrom.Text.Trim
            Session("txtReceiptDateTo") = txtReceiptDateTo.Text.Trim
            Session("txtInvoiceNo") = txtInvoiceNo.Text.Trim

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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtIncomeJobOrder As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtIncomeJobOrder = Session("dtIncomeJobOrder")

            ' check record for display
            If Not IsNothing(dtIncomeJobOrder) AndAlso dtIncomeJobOrder.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtIncomeJobOrder)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptIncome.DataSource = pagedData
                rptIncome.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtIncomeJobOrder.Rows.Count)
            Else
                ' case not exist data
                ' show message box     
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptIncome.DataSource = Nothing
                rptIncome.DataBind()
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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtIncomeJobOrder") = Nothing
            Session("dtIncomeReport") = Nothing
            Session("dtSumIncomeReport") = Nothing
            Session("txtJobOrderFrom") = Nothing
            Session("txtJobOrderTo") = Nothing
            Session("txtCustomer") = Nothing
            Session("txtIssueDateFrom") = Nothing
            Session("txtIssueDateTo") = Nothing
            Session("txtReceiptDateFrom") = Nothing
            Session("txtReceiptDateTo") = Nothing
            Session("txtInvoiceNo") = Nothing
            Session("intID") = Nothing
            Session("actAmount") = Nothing

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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set search text to session
            txtJobOrderFrom.Text = Session("txtJobOrderFrom")
            txtJobOrderTo.Text = Session("txtJobOrderTo")
            txtCustomer.Text = Session("txtCustomer") 
            txtIssueDateFrom.Text = Session("txtIssueDateFrom")
            txtIssueDateTo.Text = Session("txtIssueDateTo") 
            txtReceiptDateFrom.Text = Session("txtReceiptDateFrom")
            txtReceiptDateTo.Text = Session("txtReceiptDateTo")
            txtInvoiceNo.Text = Session("txtInvoiceNo") 

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
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Income menu
            objAction = objPermission.CheckPermission(4)
            ' set permission Create 
            btnSearch.Enabled = objAction.actList
            btnExcel.Enabled = objAction.actList

            ' set action permission to session 
            Session("actAmount") = objAction.actAmount

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
