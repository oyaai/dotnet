Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
'Imports Microsoft.Office.Interop

#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Accounting
'	Class Name		    : KTAC01
'	Class Discription	: Searching data from [Accounting] table
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 07-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
'Namespace Account
Partial Class KTAC01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
    Private objEnum As New Enums.RecordStatus
    'connect with service
    Private objAccountingService As New Service.ImpAccountingService
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
            objLog.StartLog("KTAC01", Session("UserName"))
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
    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            ' call function search data
            SearchData("1")
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtStartDate") = txtStartDate.SelectedValue
            Session("txtEndDate") = txtEndDate.SelectedValue
            Session("txtStartIE") = txtStartIE.Text.Trim
            Session("txtEndIE") = txtEndIE.Text.Trim
            Session("txtStartJobOrder") = txtStartJobOrder.Text.Trim
            Session("txtEndJobOrder") = txtEndJobOrder.Text.Trim
            Session("txtVendorName") = txtVendorName.Text.Trim
            Session("rblSearchType") = rblSearchType.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : export data to excel file
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim dtAccounting As New DataTable
            'Get data
            SearchData("2")

            ' get table object from session 
            dtAccounting = Session("dtAccounting")

            If Not IsNothing(dtAccounting) AndAlso dtAccounting.Rows.Count > 0 Then
                If ExportExcel() = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTAC_01_001"))
                End If
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
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashItemID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_WorkingCategoryDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_WorkingCategoryDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInquery.ItemDataBound
        Try
            ' object link button
            Dim btnDetail As New LinkButton

            ' find linkbutton and assign to variable
            btnDetail = DirectCast(e.Item.FindControl("btnDetail"), LinkButton)

            'Set id to hashtable (for case link to detail page)
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_WorkingCategoryDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_AccountingCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 09-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_AccountingCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intItemID As Integer = CInt(hashItemID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Detail"
                    'redirect to KTAC01_Detail
                    objMessage.ShowPagePopup("KTAC01_Detail.aspx?id=" & intItemID, 590, 350)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_WorkingCategoryCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashItemID() As Hashtable
        Get
            If IsNothing(ViewState("hashItemID")) Then
                ViewState("hashItemID") = New Hashtable()
            End If
            Return CType(ViewState("hashItemID"), Hashtable)
        End Get
    End Property
#End Region

#Region "Function"
    '/**************************************************************
    ' Function name : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-06-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim DT As DataTable = Nothing
            Dim rowCount As Integer = 5
            Dim maxRow As Long = 0
            Dim tempAccountType As String = ""
            Dim tempSheet As String = "ACCOUNT_TYPE_NAME"
            Dim temp As String = "temp"
            Dim workSheet As String = ""
            Dim vendor_id_pre As String = ""
            Dim cntAc As Long = 0
            Dim account_date_pre As String = ""
            Dim voucher_no_pre As String = ""
            Dim cheque_no_pre As String = ""
            Dim vendor_name_pre As String = ""
            Dim sumExpense_pre As Double = 0
            Dim sumIncome_pre As Double = 0
            Dim rowCountSav As Long = 5
            Dim acc As String = ""
            Dim accWrite As String = ""
            Dim intWrite As Long = 0
            Dim cntRowSav As Long = 0

            DT = Session("dtAccounting")
            maxRow = DT.Rows.Count - 1
            workSheet = DT.Rows(0)("account_type").ToString()

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "AccountRepoertV2.xlsx"))))

            'Sheet name
            pck.Workbook.Worksheets.Copy(tempSheet, workSheet)

            With pck.Workbook
                'header
                .Worksheets(workSheet).Cells(2, 2).Value = DT.Rows(0)("account_type").ToString()
                .Worksheets(workSheet).Cells(3, 2).Value = DT.Rows(0)("month").ToString()
                .Worksheets(workSheet).Cells(3, 3).Value = DT.Rows(0)("year").ToString()

                'detail
                For i As Integer = 0 To maxRow
                    If tempAccountType <> DT.Rows(i)("account_type").ToString() And i <> 0 Then

                        'sum data at footer
                        If tempAccountType = "SAVING ACCOUNT" Then
                            'case saving account
                        Else
                            'delete row
                            .Worksheets(workSheet).DeleteRow(rowCount + 1, 1)

                            ''case current account and cash
                            .Worksheets(workSheet).InsertRow(rowCount + 2, 1)
                            .Worksheets(temp).Cells("7:7").Copy(.Worksheets(workSheet).Cells(rowCount + 1 & ":" & rowCount + 1))

                            'case current account and cash
                            .Worksheets(workSheet).Cells(rowCount + 1, 6).Formula = "=SUM(F6:F" & rowCount & ")"
                            .Worksheets(workSheet).Cells(rowCount + 1, 7).Formula = "=SUM(G6:G" & rowCount & ")"
                            .Worksheets(workSheet).Cells(rowCount + 1, 8).Formula = "=H" & rowCount
                        End If

                        workSheet = DT.Rows(i)("account_type").ToString()
                        'create new sheet
                        pck.Workbook.Worksheets.Copy(tempSheet, workSheet)

                        'header
                        .Worksheets(workSheet).Cells(2, 2).Value = DT.Rows(i)("account_type").ToString()
                        .Worksheets(workSheet).Cells(3, 2).Value = DT.Rows(i)("month").ToString()
                        .Worksheets(workSheet).Cells(3, 3).Value = DT.Rows(i)("year").ToString()

                        rowCount = 5
                    End If

                    'copy row
                    If tempAccountType = DT.Rows(i)("account_type").ToString() Then
                        'Add new row
                        If DT.Rows(i)("account_type").ToString() = "SAVING ACCOUNT" Then
                            'case saving account
                            If accWrite = "write" Then
                                .Worksheets(workSheet).InsertRow(rowCountSav + 2, 1)
                                '.Worksheets(temp).Cells("6:6").Copy(.Worksheets(workSheet).Cells(rowCountSav + 2 & ":" & rowCountSav + 2))
                                .Worksheets(temp).Cells("6:6").Copy(.Worksheets(workSheet).Cells(rowCountSav + 1 & ":" & rowCountSav + 1))
                            End If
                        Else
                            'case current account and cash
                            .Worksheets(workSheet).InsertRow(rowCount + 2, 1)
                            '.Worksheets(temp).Cells("6:6").Copy(.Worksheets(workSheet).Cells(rowCount + 2 & ":" & rowCount + 2))
                            .Worksheets(temp).Cells("6:6").Copy(.Worksheets(workSheet).Cells(rowCount + 1 & ":" & rowCount + 1))
                        End If
                    End If
                    'End If

                    'Set detail
                    If DT.Rows(i)("account_type").ToString() = "SAVING ACCOUNT" Then
                        If (account_date_pre = DT.Rows(i)("account_date").ToString() _
                            And vendor_id_pre = DT.Rows(i)("vendor_id").ToString() _
                            And cheque_no_pre = DT.Rows(i)("cheque_no").ToString()) _
                            Or cntAc = 0 Then

                            'keep old value
                            account_date_pre = DT.Rows(i)("account_date").ToString()
                            If DT.Rows(i)("voucher_no").ToString().Trim <> "" Then
                                If voucher_no_pre = "" Then
                                    voucher_no_pre = voucher_no_pre & DT.Rows(i)("voucher_no").ToString()
                                Else
                                    voucher_no_pre = voucher_no_pre & "," & DT.Rows(i)("voucher_no").ToString()
                                End If
                            End If
                            vendor_id_pre = DT.Rows(i)("vendor_id").ToString()
                            cheque_no_pre = DT.Rows(i)("cheque_no").ToString()
                            vendor_name_pre = DT.Rows(i)("vendor_name").ToString()

                            'sum data of each date and vendor_id
                            sumExpense_pre = sumExpense_pre + CDbl(DT.Rows(i)("Expense").ToString())
                            sumIncome_pre = sumIncome_pre + CDbl(DT.Rows(i)("income").ToString())
                            acc = "SAVING"
                            accWrite = ""
                        Else
                            'write data
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 1).Value = rowCountSav - 4
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 2).Value = account_date_pre
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 3).Value = voucher_no_pre
                            If cheque_no_pre = "" Then
                                cheque_no_pre = "Transfer"
                            End If
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 4).Value = cheque_no_pre
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 5).Value = vendor_name_pre
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 6).Value = sumExpense_pre
                            .Worksheets(workSheet).Cells(rowCountSav + 1, 7).Value = sumIncome_pre

                            'clear value
                            sumExpense_pre = 0
                            sumIncome_pre = 0
                            voucher_no_pre = ""

                            'keep old value
                            account_date_pre = DT.Rows(i)("account_date").ToString()
                            If DT.Rows(i)("voucher_no").ToString().Trim <> "" Then
                                If voucher_no_pre = "" Then
                                    voucher_no_pre = voucher_no_pre & DT.Rows(i)("voucher_no").ToString()
                                Else
                                    voucher_no_pre = voucher_no_pre & "," & DT.Rows(i)("voucher_no").ToString()
                                End If
                            End If
                            cheque_no_pre = DT.Rows(i)("cheque_no").ToString()
                            vendor_name_pre = DT.Rows(i)("vendor_name").ToString()

                            'sum data of each date and vendor_id
                            sumExpense_pre = sumExpense_pre + CDbl(DT.Rows(i)("Expense").ToString())
                            sumIncome_pre = sumIncome_pre + CDbl(DT.Rows(i)("income").ToString())

                            acc = "SAVING"
                            accWrite = "write"
                            rowCountSav += 1
                        End If
                        cntAc += 1
                    Else
                        If acc = "SAVING" Then
                            'write data
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 1).Value = rowCountSav - 4
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 2).Value = account_date_pre
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 3).Value = voucher_no_pre
                            If cheque_no_pre = "" Then
                                cheque_no_pre = "Transfer"
                            End If
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 4).Value = cheque_no_pre
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 5).Value = vendor_name_pre
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 6).Value = sumExpense_pre
                            .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 1, 7).Value = sumIncome_pre

                            If intWrite = 1 And maxRow <> 0 Then 'maxRow <> 0 Then
                                'copy row from template sheet to current sheet
                                .Worksheets("SAVING ACCOUNT").InsertRow(rowCountSav + 2, 1)
                                .Worksheets(temp).Cells("7:7").Copy(.Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 2 & ":" & rowCountSav + 2))

                                .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 2, 6).Formula = "=SUM(F6:F" & rowCountSav + 1 & ")"
                                .Worksheets("SAVING ACCOUNT").Cells(rowCountSav + 2, 7).Formula = "=SUM(G6:G" & rowCountSav + 1 & ")"
                            End If
                            intWrite += 1
                        End If
                        'case current account and cash
                        .Worksheets(workSheet).Cells(rowCount + 1, 1).Value = rowCount - 4
                        .Worksheets(workSheet).Cells(rowCount + 1, 2).Value = DT.Rows(i)("account_date").ToString()
                        .Worksheets(workSheet).Cells(rowCount + 1, 3).Value = DT.Rows(i)("voucher_no").ToString()
                        If DT.Rows(i)("cheque_no").ToString().Trim = "" Then
                            .Worksheets(workSheet).Cells(rowCount + 1, 4).Value = "Transfer"
                        Else
                            .Worksheets(workSheet).Cells(rowCount + 1, 4).Value = DT.Rows(i)("cheque_no").ToString()
                        End If
                        .Worksheets(workSheet).Cells(rowCount + 1, 5).Value = DT.Rows(i)("vendor_name").ToString()
                        .Worksheets(workSheet).Cells(rowCount + 1, 6).Value = CDbl(DT.Rows(i)("Expense").ToString())
                        .Worksheets(workSheet).Cells(rowCount + 1, 7).Value = CDbl(DT.Rows(i)("income").ToString())

                        acc = ""
                        End If

                        'set value
                        rowCount += 1

                        'keep current row
                        tempAccountType = DT.Rows(i)("account_type").ToString()
                Next

                If acc = "SAVING" Then
                    'intWrite += 1
                    'copy fotter row
                    If intWrite = 1 And maxRow <> 0 Then 'maxRow <> 0 Then
                        .Worksheets(workSheet).InsertRow(rowCountSav + 2, 1)
                        '.Worksheets(temp).Cells("7:7").Copy(.Worksheets(workSheet).Cells(rowCountSav + 2 & ":" & rowCountSav + 2))
                        .Worksheets(temp).Cells("6:6").Copy(.Worksheets(workSheet).Cells(rowCountSav + 1 & ":" & rowCountSav + 1))

                    End If

                    'write data
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 1).Value = rowCountSav - 4
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 2).Value = account_date_pre
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 3).Value = voucher_no_pre
                    If cheque_no_pre = "" Then
                        cheque_no_pre = "Transfer"
                    End If
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 4).Value = cheque_no_pre
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 5).Value = vendor_name_pre
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 6).Value = sumExpense_pre
                    .Worksheets(workSheet).Cells(rowCountSav + 1, 7).Value = sumIncome_pre

                    'copy fotter row
                    If intWrite = 1 And maxRow <> 0 Then 'maxRow <> 0 Then
                        .Worksheets(workSheet).InsertRow(rowCountSav + 2, 1)
                        '.Worksheets(temp).Cells("7:7").Copy(.Worksheets(workSheet).Cells(rowCountSav + 2 & ":" & rowCountSav + 2))
                        .Worksheets(temp).Cells("7:7").Copy(.Worksheets(workSheet).Cells(rowCountSav + 2 & ":" & rowCountSav + 2))

                        'Sum value of last sheet
                        .Worksheets(workSheet).Cells(rowCount + 1, 6).Formula = "=SUM(F6:F" & rowCount & ")"
                        .Worksheets(workSheet).Cells(rowCount + 1, 7).Formula = "=SUM(G6:G" & rowCount & ")"
                        .Worksheets(workSheet).Cells(rowCount + 1, 8).Formula = "=H" & rowCount
                    End If
                Else
                    'delete row
                    If rowCount > 6 Then
                        .Worksheets(workSheet).DeleteRow(rowCount + 1, 1)
                    End If
                    ''case current account and cash
                    'copy fotter row
                    If maxRow <> 0 Then
                        .Worksheets(workSheet).InsertRow(rowCount + 2, 1)
                        .Worksheets(temp).Cells("7:7").Copy(.Worksheets(workSheet).Cells(rowCount + 1 & ":" & rowCount + 1))
                    End If

                    'Sum value of last sheet
                    .Worksheets(workSheet).Cells(rowCount + 1, 6).Formula = "=SUM(F6:F" & rowCount & ")"
                    .Worksheets(workSheet).Cells(rowCount + 1, 7).Formula = "=SUM(G6:G" & rowCount & ")"
                    .Worksheets(workSheet).Cells(rowCount + 1, 8).Formula = "=H" & rowCount
                End If

                'set header
                .Worksheets(workSheet).HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)
                .Worksheets(workSheet).PrinterSettings.RepeatRows = .Worksheets(workSheet).Cells("$1:$4")

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(temp)

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(tempSheet)
            End With


            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=AccountingReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ExportExcel = False
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function
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
            'get year
            LoadYearList()

            ' set value to from session to textbox 
            txtStartDate.SelectedValue = Session("txtStartDate")
            txtEndDate.SelectedValue = Session("txtEndDate")
            txtStartIE.Text = Session("txtStartIE")
            txtEndIE.Text = Session("txtEndIE")
            txtStartJobOrder.Text = Session("txtStartJobOrder")
            txtEndJobOrder.Text = Session("txtEndJobOrder")
            txtVendorName.Text = Session("txtVendorName")
            rblSearchType.Text = Session("rblSearchType")

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0

                'get month
                setDefaultMonth()

                'Set default year
                Dim year As Integer = DateTime.Now.Year
                txtEndDate.SelectedValue = year
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: LoadListVendor
    '	Discription	    : Load list Vendor function
    '	Return Value	: nothing
    '	Create User	    : Komsan Luecha
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadYearList()
        Try
            'object Vendor service
            Dim listYearDto As New DataTable
            listYearDto = objAccountingService.GetYearList()

            txtEndDate.DataSource = listYearDto
            txtEndDate.DataTextField = listYearDto.Columns(1).ToString()
            txtEndDate.DataValueField = listYearDto.Columns(1).ToString()
            txtEndDate.DataBind()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setDefaultMonth
    '	Discription	    : set Default Month
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-02-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub setDefaultMonth()
        Try
            Dim month As Integer = DateTime.Now.Month
            txtStartDate.SelectedValue = month
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setDefaultMonth", ex.Message.ToString, Session("UserName"))
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
    Private Sub SearchData(ByVal dataType As String)
        Try
            ' table object keep value from item service
            Dim dtAccounting As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtAccounting = objAccountingService.GetAccountingList(Session("objAccountingDto"), dataType)
            ' set table object to session
            Session("dtAccounting") = dtAccounting
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
            Dim objAccountingDto As New Dto.AccountingDto

            'set data from condition search into dto object
            With objAccountingDto
                .strAccountMonth = txtStartDate.SelectedValue
                .strAccountYear = txtEndDate.SelectedValue
                .strJoborder_start = txtStartJobOrder.Text.Trim
                .strJoborder_end = txtEndJobOrder.Text.Trim

                If rblSearchType.SelectedIndex.ToString.Trim = "0" Or rblSearchType.SelectedIndex.ToString.Trim = "" Then
                    .strAccount_type = ""
                Else
                    .strAccount_type = rblSearchType.SelectedIndex.ToString
                End If
                .strIe_start_code = txtStartIE.Text.Trim
                .strIe_end_code = txtEndIE.Text.Trim
                .strVendor_name = txtVendorName.Text.Trim
                .strStatus_id = Enums.RecordStatus.Completed & "," & Enums.RecordStatus.Approved
                .accType = "accounting"
            End With

            ' set dto object to session
            Session("objAccountingDto") = objAccountingDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
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
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtAccounting As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtAccounting = Session("dtAccounting")

            ' check record for display
            If Not IsNothing(dtAccounting) AndAlso dtAccounting.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtAccounting)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtAccounting.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()

            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(12)

            ' set permission 
            btnExcel.Enabled = objAction.actList
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actList") = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
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
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtAccounting") = Nothing
            Session("txtStartDate") = Nothing
            Session("txtEndDate") = Nothing
            Session("txtStartIE") = Nothing
            Session("txtEndIE") = Nothing
            Session("txtStartJobOrder") = Nothing
            Session("txtEndJobOrder") = Nothing
            Session("txtVendorName") = Nothing
            Session("rblSearchType") = Nothing
            Session("actList") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
'End Namespace