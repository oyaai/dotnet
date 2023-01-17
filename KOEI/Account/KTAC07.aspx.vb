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
'	Package Name	    : Cost Table Overview
'	Class Name		    : KTAC07
'	Class Discription	: Searching data from [Accounting] table
'	Create User 		: Pranitda Sroengklang
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
Partial Class KTAC07
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private ReportReportPath As String = "../Report/RptFileSave/"

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
            objLog.StartLog("KTAC07", Session("UserName"))
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

    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : call export excel
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim dtAdvanceIncome As New DataTable

            'Get data
            SearchDataReport()

            ' get table object from session 
            dtAdvanceIncome = Session("dtAdvanceIncome")

            If Not IsNothing(dtAdvanceIncome) AndAlso dtAdvanceIncome.Rows.Count > 0 Then
                If ExportExcel() = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_004"))
                End If
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnPdf_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#Region "Function"
    '/**************************************************************
    ' Function name   : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 26-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim DT As DataTable = Nothing
            Dim workSheet As String = ""
            Dim fileTemp As String = ""
            Dim tempSheet As String = "tmp"
            Dim i As Long = 0
            Dim max As Long = 0

            DT = Session("dtAdvanceIncome")
            max = DT.Rows.Count - 1
            fileTemp = "AdvanceIncomeWIP.xlsx"

            pck = New ExcelPackage(New MemoryStream(), _
                                   New MemoryStream(File.ReadAllBytes( _
                                   HttpContext.Current.Server.MapPath( _
                                   WebConfigurationManager.AppSettings("excelReportPath") & _
                                   fileTemp))))

            With pck.Workbook
                workSheet = "AdvanceIncome"
                'detail
                For i = 0 To max
                    'copy row from template sheet to current sheet
                    .Worksheets(workSheet).InsertRow(i + 5, 1)
                    .Worksheets(tempSheet).Cells("5:5").Copy(.Worksheets(workSheet).Cells(i + 5 & ":" & i + 5))

                    .Worksheets(workSheet).Cells(i + 5, 1).Value = i + 1
                    .Worksheets(workSheet).Cells(i + 5, 2).Value = DT.Rows(i)("job_order").ToString()
                    .Worksheets(workSheet).Cells(i + 5, 3).Value = DT.Rows(i)("customer").ToString()
                    If DT.Rows(i)("jh_mold_amount").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 4).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 4).Value = CDbl(DT.Rows(i)("jh_mold_amount").ToString())
                    End If
                    If DT.Rows(i)("jo_other_amount").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 5).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 5).Value = CDbl(DT.Rows(i)("jo_other_amount").ToString())
                    End If
                    If DT.Rows(i)("jho_total").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 6).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 6).Value = CDbl(DT.Rows(i)("jho_total").ToString())
                    End If
                    If DT.Rows(i)("ji_mold_amount").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 7).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 7).Value = CDbl(DT.Rows(i)("ji_mold_amount").ToString())
                    End If
                    If DT.Rows(i)("ratio").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 8).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 8).Value = CDbl(DT.Rows(i)("ratio").ToString())
                    End If
                    If DT.Rows(i)("ji_other_amount").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 9).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 9).Value = CDbl(DT.Rows(i)("ji_other_amount").ToString())
                    End If
                    If DT.Rows(i)("ji_total").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 10).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 10).Value = CDbl(DT.Rows(i)("ji_total").ToString())
                    End If
                    If DT.Rows(i)("KTS_MTR_Currency").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 11).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 11).Value = CDbl(DT.Rows(i)("KTS_MTR_Currency").ToString())
                    End If
                    If DT.Rows(i)("KTS_MTR_THB").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 12).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 12).Value = CDbl(DT.Rows(i)("KTS_MTR_THB").ToString())
                    End If
                    .Worksheets(workSheet).Cells(i + 5, 13).Value = DT.Rows(i)("KTS_INV").ToString()
                    If DT.Rows(i)("KTC_MTR_Currency").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 14).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 14).Value = CDbl(DT.Rows(i)("KTC_MTR_Currency").ToString())
                    End If
                    If DT.Rows(i)("KTC_MTR_THB").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 15).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 15).Value = CDbl(DT.Rows(i)("KTC_MTR_THB").ToString())
                    End If
                    .Worksheets(workSheet).Cells(i + 5, 16).Value = DT.Rows(i)("KTC_INV").ToString()
                    If DT.Rows(i)("OTHER_MTR").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 17).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 17).Value = CDbl(DT.Rows(i)("OTHER_MTR").ToString())
                    End If
                    If DT.Rows(i)("TotalMTR").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 18).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 18).Value = CDbl(DT.Rows(i)("TotalMTR").ToString())
                    End If
                    If DT.Rows(i)("hh").ToString() = "" Then
                        .Worksheets(workSheet).Cells(i + 5, 20).Value = ""
                    Else
                        .Worksheets(workSheet).Cells(i + 5, 20).Value = CDbl(DT.Rows(i)("hh").ToString())
                    End If

                Next

                'summary
                .Worksheets(workSheet).Cells(i + 6, 4).Formula = "=SUM(D5:D" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 5).Formula = "=SUM(E5:E" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 6).Formula = "=SUM(F5:F" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 7).Formula = "=SUM(G5:G" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 9).Formula = "=SUM(I5:I" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 10).Formula = "=SUM(J5:J" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 11).Formula = "=SUM(K5:K" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 12).Formula = "=SUM(L5:L" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 14).Formula = "=SUM(N5:N" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 15).Formula = "=SUM(O5:O" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 17).Formula = "=SUM(Q5:Q" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 18).Formula = "=SUM(R5:R" & max + 5 & ")"
                .Worksheets(workSheet).Cells(i + 6, 20).Formula = "=SUM(T5:T" & max + 5 & ")"

                'add year 
                Dim dtTo As DateTime = New DateTime(DateTime.Now.Year, 12, 1).AddMonths(1)
                .Worksheets(workSheet).Cells(1, 5).Formula = "=TEXT(""" & dtTo.AddDays(-(dtTo.Day)) & """,""dd-MMM-yyyy"")"

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(tempSheet)
            End With

            Dim FileName As String = ReportReportPath & "AdvanceIncomeWIP" & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".xlsx"

            Dim path As String = HttpContext.Current.Server.MapPath(fileName)

            Dim stream As Stream = File.Create(path)
            pck.SaveAs(stream)
            stream.Close()

            popupExcel(fileName)

            ExportExcel = True
        Catch ex As Exception
            ExportExcel = False
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function
    '/**************************************************************
    '	Function name	: popupExcel
    '	Discription	    : popup Excel
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub popupExcel(ByVal path As String)
        Try
            Dim objPdf As New List(Of String)
            objPdf.Add(path)

            objMessage.ShowMultiplePagePopup(objPdf, 1000, 990, "_blank", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("exportPdf", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Get data for export crystal report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtAdvanceIncome As New DataTable

            ' call function GetItemList from ItemService
            dtAdvanceIncome = objAccountingService.GetAdvanceIncomeReport(ddlYear.Text)
            ' set table object to session
            Session("dtAdvanceIncome") = dtAdvanceIncome
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
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

            ' set value to from session to textbox 
            ddlYear.SelectedValue = Session("ddlYear")

            ' call function check permission
            CheckPermission()

            'get year
            LoadYearList()
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
            listYearDto = objAccountingService.GetWIPYear()

            ddlYear.DataSource = listYearDto
            ddlYear.DataTextField = listYearDto.Columns(1).ToString()
            ddlYear.DataValueField = listYearDto.Columns(1).ToString()

            ddlYear.DataBind()

            Dim year As Integer = DateTime.Now.Year
            ddlYear.SelectedValue = year

            ' set select Vendor from session
            If Not IsNothing(Session("ddlYear")) And ddlYear.Items.Count > 0 Then
                ddlYear.SelectedValue = Session("ddlYear")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtAdvanceIncome") = Nothing
            Session("dtCostTableOverViewList") = Nothing
            Session("ddlYear") = Nothing
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
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(44)

            ' set permission 
            btnExcel.Enabled = objAction.actList
            'btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actList") = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
