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
    Partial Class KTAC04
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message

    'connect with service
    Private objAccountingService As New Service.ImpAccountingService
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
            objLog.StartLog("KTAC04", Session("UserName"))
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
    '	Create Date	    : 12-06-2013
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
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs _
        ) Handles btnSearch.Click
        Try
            'set screen to session
            setScreenToSession()

            ' call function search data
            SearchData("1")
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setScreenToSession
    '	Discription	    : set Screen To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub setScreenToSession()
        Try
            ' set value to from session to textbox 
            Session("txtStartDate") = txtStartDate.SelectedValue
            Session("txtEndDate") = txtEndDate.SelectedValue
            Session("txtStartPONo") = txtStartPONo.Text
            Session("txtEndPONo") = txtEndPONo.Text
            Session("txtStartJobOrder") = txtStartJobOrder.Text
            Session("txtEndJobOrder") = txtEndJobOrder.Text
            Session("txtVendorName") = txtVendorName.Text
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setScreenToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setSessionToScreen
    '	Discription	    : set Session To Screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub setSessionToScreen()
        Try
            ' set value to from session to textbox 
            txtStartDate.SelectedValue = Session("txtStartDate")
            ' set select Vendor from session
            If Not IsNothing(Session("txtEndDate")) Then
                txtEndDate.SelectedValue = Session("txtEndDate")
            End If
            txtStartPONo.Text = Session("txtStartPONo")
            txtEndPONo.Text = Session("txtEndPONo")
            txtStartJobOrder.Text = Session("txtStartJobOrder")
            txtEndJobOrder.Text = Session("txtEndJobOrder")
            txtVendorName.Text = Session("txtVendorName")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setSessionToScreen", ex.Message.ToString, Session("UserName"))
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
            Dim dtWithholding As New DataTable
            'Get data
            SearchData("2")

            ' get table object from session 
            dtWithholding = Session("dtWithholding")

            If Not IsNothing(dtWithholding) AndAlso dtWithholding.Rows.Count > 0 Then
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
            Dim rowCount As Integer = 4
            Dim maxRow As Long = 0
            Dim tempAccountType As String = ""
            Dim tempSheet53 As String = "ภงด. 53"
            Dim tempSheet3 As String = "ภงด. 3"
            Dim temp As String = "temp"
            Dim tempSheet As String = "tempSheet"
            Dim workSheet As String = ""
            Dim tempVendor_type2 As String = ""

            DT = Session("dtWithholding")
            maxRow = DT.Rows.Count - 1

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "WithholdingTaxReportV2.xlsx"))))

            With pck.Workbook

                'detail
                For i As Integer = 0 To maxRow

                    If tempVendor_type2 <> DT.Rows(i)("vendor_type2").ToString() And i <> 0 Then
                        .Worksheets(workSheet).Cells(rowCount + 1, 7).Formula = "=SUM(G4:G" & rowCount - 1 & ")"
                        .Worksheets(workSheet).Cells(rowCount + 1, 8).Formula = "=SUM(H4:H" & rowCount - 1 & ")"

                        rowCount = 4
                    End If

                    'Start new sheet
                    If rowCount = 4 Then
                        'set Sheet name
                        If DT.Rows(i)("vendor_type2").ToString() = 1 Then 'ภงด. 53
                            workSheet = tempSheet53
                        Else
                            workSheet = tempSheet3
                        End If

                        'copy sheet
                        pck.Workbook.Worksheets.Copy(tempSheet, workSheet)

                        'set header name
                        .Worksheets(workSheet).Cells(2, 1).Value = workSheet

                    End If

                    'header
                    .Worksheets(workSheet).Cells(1, 1).Value = "รายงานภาษีหัก ณ ที่จ่ายประจำเดือน " & DT.Rows(i)("month").ToString() & " " & DT.Rows(i)("year").ToString()

                    If rowCount <> 4 Then
                        'copy row
                        .Worksheets(workSheet).InsertRow(rowCount, 1)
                        .Worksheets(temp).Cells("4:4").Copy(.Worksheets(workSheet).Cells(rowCount & ":" & rowCount))
                    End If

                    'Set detail
                    .Worksheets(workSheet).Cells(rowCount, 1).Value = DT.Rows(i)("vendor_name").ToString()
                    .Worksheets(workSheet).Cells(rowCount, 2).Value = DT.Rows(i)("vendor_type2_no").ToString()
                    .Worksheets(workSheet).Cells(rowCount, 3).Value = DT.Rows(i)("address").ToString()
                    .Worksheets(workSheet).Cells(rowCount, 4).Value = DT.Rows(i)("account_date").ToString()
                    .Worksheets(workSheet).Cells(rowCount, 5).Value = DT.Rows(i)("wt_type").ToString()
                    .Worksheets(workSheet).Cells(rowCount, 6).Value = CDbl(DT.Rows(i)("wt_percentage").ToString())
                    .Worksheets(workSheet).Cells(rowCount, 7).Value = CDbl(DT.Rows(i)("sub_total").ToString())
                    .Worksheets(workSheet).Cells(rowCount, 8).Value = CDbl(DT.Rows(i)("wt_amount").ToString())

                    'set value
                    rowCount += 1
                    tempVendor_type2 = DT.Rows(i)("vendor_type2").ToString()
                Next
                'sum data last sheet
                .Worksheets(workSheet).Cells(rowCount + 1, 7).Formula = "=SUM(G4:G" & rowCount - 1 & ")"
                .Worksheets(workSheet).Cells(rowCount + 1, 8).Formula = "=SUM(H4:H" & rowCount - 1 & ")"

                'set header
                .Worksheets(workSheet).HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)
                .Worksheets(workSheet).PrinterSettings.RepeatRows = .Worksheets(workSheet).Cells("$1:$3")

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(tempSheet)
                pck.Workbook.Worksheets.Delete(temp)
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
    '	Function name	: SearchExcelData
    '	Discription	    : Search Excel Data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData(ByVal dataType As String)
        Try
            ' table object keep value from item service
            Dim dtWithholding As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtWithholding = objAccountingService.GetWithholdingList(Session("objAccountingDto"), dataType)
            ' set table object to session
            Session("dtWithholding") = dtWithholding
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
    '	Create Date	    : 12-06-2013
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
                .strPo_startno = txtStartPONo.Text.Trim
                .strPo_endno = txtEndPONo.Text.Trim
                .strVendor_name = txtVendorName.Text.Trim
                .strStatus_id = Enums.RecordStatus.Completed & "," & Enums.RecordStatus.Approved
                .accType = "withholding"
                '.strIe_category_type = Enums.AccountRecordTypes.Payment
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
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtWithholding As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtWithholding = Session("dtWithholding")

            ' check record for display
            If Not IsNothing(dtWithholding) AndAlso dtWithholding.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtWithholding)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtWithholding.Rows.Count)
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
    '	Function name	: ShowDescription
    '	Discription	    : Show description
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-06-2013
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
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            'get year
            LoadYearList()

            setSessionToScreen()

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' call function check permission
            CheckPermission()

            'get month
            setDefaultMonth()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtWithholding") = Nothing
            Session("txtStartDate") = Nothing
            Session("txtEndDate") = Nothing
            Session("txtStartPONo") = Nothing
            Session("txtEndPONo") = Nothing
            Session("txtStartJobOrder") = Nothing
            Session("txtEndJobOrder") = Nothing
            Session("txtVendorName") = Nothing
            Session("actList") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
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
            If Not IsNothing(Session("txtStartDate")) Then
                txtStartDate.SelectedValue = Session("txtStartDate")
            Else
                txtStartDate.SelectedValue = month
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setDefaultMonth", ex.Message.ToString, Session("UserName"))
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

            Dim year As Integer = DateTime.Now.Year
            txtEndDate.SelectedValue = year

            ' set select Vendor from session
            If Not IsNothing(Session("txtEndDate")) And txtEndDate.Items.Count > 0 Then
                txtEndDate.SelectedValue = Session("txtEndDate")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
