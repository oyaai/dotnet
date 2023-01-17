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
'	Package Name	    : Rating Purchase
'	Class Name		    : KTPU07
'	Class Discription	: Searching data of Rating Purchase
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 15-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region
Partial Class KTPU07
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const constDelete As String = "Delete"
    Private CommonValidation As New Validations.CommonValidation
    Private objValidate As New Common.Validations.Validation
    Private ReportReportPath As String = "../Report/RptFileSave/"
    Private objDate As New Common.Utilities.Utility

    'connect with service
    Private objChequeService As New Service.ImpCheque_PurchaseService

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
            objLog.StartLog("KTPU07", Session("UserName"))
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
    '	Create Date	    : 12-07-2013
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
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            Session("search") = "search"

            setTextToSession()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Open add screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If CommonValidation.IsExistAccountApprove = False Then
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_005"))
            Else
                clearManagePageSession()

                ' redirect to KTMS20 with Add mode
                Response.Redirect("KTPU08.aspx?Mode=Add")
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInquery.DataBinding
        Try
            ' clear hashtable data
            hashId.Clear()
            hashChequeNo.Clear()
            hashChequeDate.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_Cheque_PurchaseDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_Cheque_PurchaseDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInquery.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            'Set id to hashtable (for case link to detail page)
            hashId.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashChequeNo.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "cheque_no"))
            hashChequeDate.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "cheque_date"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Cheque_PurchaseDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_ChequePurchaseCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_ChequePurchaseCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim strId As String = hashId(e.Item.ItemIndex).ToString()
            Dim strChequeNo As String = hashChequeNo(e.Item.ItemIndex).ToString()
            Dim strChequeDate As String = hashChequeDate(e.Item.ItemIndex).ToString()

            ' set ItemID to session
            
            Session("strId") = strId
            Session("strChequeNo") = strChequeNo
            'Session("strChequeDate") = strChequeDate
            Dim dt As DateTime = strChequeDate
            Session("strChequeDate") = Right("00" & dt.Day, 2) & "/" & Right("00" & dt.Month, 2) & "/" & dt.Year

            Select Case e.CommandName
                Case "Edit"
                    If CommonValidation.IsExistAccountApprove = False Then
                        ' show message box
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_005"))
                    Else
                        clearManagePageSession()
                        ' redirect to KTMS04
                        'Response.Redirect("KTPU08.aspx?Mode=Edit&strChequeNo=" & strChequeNo & "&strChequeDate=" & Session("strChequeDate"))
                        Response.Redirect("KTPU08.aspx?Mode=Edit&strId=" & strId)
                    End If
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTPU07", constDelete, objMessage.GetXMLMessage("KTPU_07_001"))
                Case "Detail"
                    'redirect to KTAC01_Detail
                    'objMessage.ShowPagePopup("KTPU07_Detail.aspx?strChequeNo=" & strChequeNo & "&strChequeDate=" & Session("strChequeDate"), 990, 690, "_blank", True)
                    'objMessage.ShowPagePopup("KTPU07_Detail.aspx?strId=" & strId, 990, 690, "_blank", True)
                    Dim strPage As String = "KTPU07_Detail.aspx?strId=" & strId.ToString()
                    Dim sb As New System.Text.StringBuilder()
                    sb.Append("var w = parseInt(screen.availWidth * 0.73);")
                    sb.Append("var h = parseInt(screen.availHeight * 0.50);")
                    sb.Append("var l = parseInt((screen.availWidth / 2) - (w / 2));")
                    sb.Append("var t = parseInt((screen.availHeight / 2) - (h / 2));")
                    sb.AppendFormat("popup = window.open('{0}','{1}'", strPage, "_blank")
                    sb.Append(",'width='+w+',height='+h+',left='+l+',top='+t+'")
                    sb.Append(",resizable,scrollbars=1');")
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowDetail", sb.ToString(), True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Cheque_PurchaseCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashId() As Hashtable
        Get
            If IsNothing(ViewState("hashId")) Then
                ViewState("hashId") = New Hashtable()
            End If
            Return CType(ViewState("hashId"), Hashtable)
        End Get
    End Property
    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashChequeNo() As Hashtable
        Get
            If IsNothing(ViewState("hashChequeNo")) Then
                ViewState("hashChequeNo") = New Hashtable()
            End If
            Return CType(ViewState("hashChequeNo"), Hashtable)
        End Get
    End Property
    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashChequeDate() As Hashtable
        Get
            If IsNothing(ViewState("hashChequeDate")) Then
                ViewState("hashChequeDate") = New Hashtable()
            End If
            Return CType(ViewState("hashChequeDate"), Hashtable)
        End Get
    End Property
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: setTextToSession
    '	Discription	    : set Text To Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub setTextToSession()
        Try
            ' set search text to session
            Session("txtChequeNoSrch") = txtChequeNoSrch.Text.Trim
            Session("txtChequeDateFrom") = txtChequeDateFrom.Text.Trim
            Session("txtChequeDateTo") = txtChequeDateTo.Text.Trim
            Session("txtVendor_NameLst") = txtVendor_NameLst.Text.Trim
            Session("rblSearchType") = rblSearchType.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setTextToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: setSessionToText
    '	Discription	    : set Session To Text
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub setSessionToText()
        Try
            ' set search text to session
            txtChequeNoSrch.Text = Session("txtChequeNoSrch")
            txtChequeDateFrom.Text = Session("txtChequeDateFrom")
            txtChequeDateTo.Text = Session("txtChequeDateTo")
            txtVendor_NameLst.Text = Session("txtVendor_NameLst")
            rblSearchType.Text = Session("rblSearchType")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setSessionToText", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtGetChequeList") = Nothing
            Session("txtChequeNoSrch") = Nothing
            Session("txtChequeDateFrom") = Nothing
            Session("txtChequeDateTo") = Nothing
            Session("txtVendor_NameLst") = Nothing
            Session("rblSearchType") = Nothing
            Session("dtPaymentVoucher") = Nothing
            Session("itemConfirm") = Nothing
            Session("dtPaymentPaid") = Nothing
            Session("objChequePurchaseDto1") = Nothing
            Session("actList") = Nothing
            Session("actDelete") = Nothing
            Session("actUpdate") = Nothing
            Session("actCreate") = Nothing
            Session("strChequeNo") = Nothing
            Session("strChequeDate") = Nothing
            Session("search") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            Dim iniMode As String = ""
            Dim dtApprover As New DataTable

            ' set value to from session to textbox 
            setSessionToText()

            If IsNothing(Request.QueryString("ins_mode")) = False Then
                If Request.QueryString("ins_mode") = "Completed" Then
                    'delete file
                    CommonValidation.DeleteReportFile() 

                    Session("pathTaxReport") = Nothing
                    Session("pathAccountingReport") = Nothing

                    exportTaxExcel()
                    ExportAccountExcel()

                    'Display pdf report
                    exportPdf()

                    If IsNothing(Session("search")) = False Then
                        ' call function search data
                        SearchData()

                        ' case not new enter then display page with page no
                        DisplayPage(Request.QueryString("PageNo"))
                    End If

                    Exit Sub
                End If
            End If

            iniMode = objUtility.GetQueryString("New")

            'check case new enter
            If iniMode = "True" Then
                'call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0
            ElseIf iniMode = "Insert" Then 'Case come back from insert rating
                If IsNothing(Session("search")) = False Then
                    ' call function search data
                    SearchData()

                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                End If
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(constDelete) = "True" Then
                DeleteItem()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: exportExcel
    '	Discription	    : export Excel
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub exportTaxExcel()
        Try
            Dim dtTaxReport As New DataTable
            ' get table object from session 
            dtTaxReport = Session("dtTaxReport")

            If Not IsNothing(dtTaxReport) AndAlso dtTaxReport.Rows.Count > 0 Then
                If ExportExcel() = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_004"))
                End If
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("exportExcel", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    ' Function name   : ExportTaxExcel
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
            Dim workSheet As String = ""
            Dim fileTemp As String = ""


            DT = Session("dtTaxReport")

            If DT.Rows(0)("wt_amount").ToString() = "" Or CDbl(DT.Rows(0)("wt_amount").ToString()) = 0 Then
                ExportExcel = True
                Exit Function
            End If
            'type2 = 0 ,KTPU07_08_1
            'type2 = 1 ,KTPU07_08_2
            If DT.Rows(0)("type2").ToString() = "0" Then
                fileTemp = "KTPU07_08_1.xlsx"
            Else
                fileTemp = "KTPU07_08_2.xlsx"
            End If

            pck = New ExcelPackage(New MemoryStream(), _
                                   New MemoryStream(File.ReadAllBytes( _
                                   HttpContext.Current.Server.MapPath( _
                                   WebConfigurationManager.AppSettings("excelReportPath") & _
                                   fileTemp))))

            With pck.Workbook

                'detail
                For i As Integer = 0 To DT.Rows.Count - 1
                    workSheet = "ฉบับ 1,2"
                    .Worksheets(workSheet).Cells(14, 3).Value = DT.Rows(i)("name").ToString()
                    .Worksheets(workSheet).Cells(16, 3).Value = DT.Rows(i)("address").ToString()
                    '.Worksheets(workSheet).Cells(12, 10).Value = DT.Rows(i)("type2_no").ToString()
                    If DT.Rows(0)("type2").ToString() = "0" Then
                        .Worksheets(workSheet).Cells(12, 10).Value = DT.Rows(i)("type2_no").ToString()
                    Else
                        .Worksheets(workSheet).Cells(14, 10).Value = DT.Rows(i)("type2_no").ToString()
                    End If
                    .Worksheets(workSheet).Cells(49, 9).Value = DT.Rows(i)("cheque_date").ToString()
                    .Worksheets(workSheet).Cells(49, 10).Value = CDbl(DT.Rows(i)("amount").ToString())
                    .Worksheets(workSheet).Cells(49, 12).Value = CDbl(DT.Rows(i)("wt_amount").ToString())

                    workSheet = "ฉบับ 3,4"
                    .Worksheets(workSheet).Cells(14, 3).Value = DT.Rows(i)("name").ToString()
                    .Worksheets(workSheet).Cells(16, 3).Value = DT.Rows(i)("address").ToString()
                    '.Worksheets(workSheet).Cells(12, 10).Value = DT.Rows(i)("type2_no").ToString()
                    If DT.Rows(0)("type2").ToString() = "0" Then
                        .Worksheets(workSheet).Cells(12, 10).Value = DT.Rows(i)("type2_no").ToString()
                    Else
                        .Worksheets(workSheet).Cells(14, 10).Value = DT.Rows(i)("type2_no").ToString()
                    End If
                    .Worksheets(workSheet).Cells(49, 9).Value = DT.Rows(i)("cheque_date").ToString()
                    .Worksheets(workSheet).Cells(49, 10).Value = CDbl(DT.Rows(i)("amount").ToString())
                    .Worksheets(workSheet).Cells(49, 12).Value = CDbl(DT.Rows(i)("wt_amount").ToString())
                Next
            End With

            'Dim fileRnd As String = Rnd() & ".xlsx"
            'Dim fileName As String = ReportReportPath & "TaxReport" & fileRnd
            Dim FileName As String = ReportReportPath & "TaxReport" & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".xlsx"
            Dim path As String = HttpContext.Current.Server.MapPath(fileName)
            Session("pathTaxReport") = fileName

            Dim stream As Stream = File.Create(path)
            pck.SaveAs(stream)
            stream.Close()

            ExportExcel = True
        Catch ex As Exception
            ExportExcel = False
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function
    '/**************************************************************
    ' Function name : ExportAccountExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 09-06-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportAccountExcel() As Boolean
        ExportAccountExcel = False
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

            DT = Session("dtAccountReport")
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
                End If

                'set header
                .Worksheets(workSheet).HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)
                .Worksheets(workSheet).PrinterSettings.RepeatRows = .Worksheets(workSheet).Cells("$1:$4")

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(temp)

                'Delete template sheet
                pck.Workbook.Worksheets.Delete(tempSheet)
            End With

            'crate file name
            'Dim path As String = "C:\temp\AccountingReport" & Rnd() & ".xlsx"
            'Dim path As String = HttpContext.Current.Server.MapPath(ReportReportPath & "\AccountingReport" & Rnd() & ".xlsx")
            'Session("pathAccountingReport") = path

            'Dim fileRnd As String = Rnd() & ".xlsx"
            'Dim fileName As String = ReportReportPath & "AccountingReport" & fileRnd
            Dim FileName As String = ReportReportPath & "AccountingReport" & "_" & DateTime.Now.ToString("yyyyMMddhhmmssffftt") & ".xlsx"
            Dim path As String = HttpContext.Current.Server.MapPath(fileName)

            Session("pathAccountingReport") = fileName

            Dim stream As Stream = File.Create(path)
            pck.SaveAs(stream)
            stream.Close()

            'open file
            'System.Diagnostics.Process.Start(path)

            ExportAccountExcel = True
        Catch ex As Exception
            ExportAccountExcel = False
            ' Write error log
            objLog.ErrorLog("ExportAccountExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function
    '/**************************************************************
    '	Function name	: exportPdf
    '	Discription	    : export Pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub exportPdf()
        Try
            Dim objPdf As New List(Of String)
            objPdf.Add(Session("FileNamePaidReport"))
            objPdf.Add(Session("FileNameVoucherReport"))

            If IsNothing(Session("pathTaxReport")) = False Then
                objPdf.Add(Session("pathTaxReport"))
            End If
            If IsNothing(Session("pathAccountingReport")) = False Then
                objPdf.Add(Session("pathAccountingReport"))
            End If

            objMessage.ShowMultiplePagePopup(objPdf, 1000, 990, "_blank", True)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("exportPdf", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtGetChequeList As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtGetChequeList = objChequeService.GetCheque_PurchaseList(Session("objChequePurchaseDto1"))
            ' set table object to session
            Session("dtGetChequeList") = dtGetChequeList
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
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' ChequePurchase dto object
            Dim objChequePurchaseDto As New Dto.Cheque_PurchaseDto
            Dim startChequeDate As String = ""
            Dim endChequeDate As String = ""
            Dim arrChequeStartDate() As String = Split(txtChequeDateFrom.Text.Trim(), "/")
            Dim arrChequeEndDate() As String = Split(txtChequeDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objChequePurchaseDto
                If UBound(arrChequeStartDate) > 0 Then
                    startChequeDate = arrChequeStartDate(2) & arrChequeStartDate(1) & arrChequeStartDate(0)
                End If
                If UBound(arrChequeEndDate) > 0 Then
                    endChequeDate = arrChequeEndDate(2) & arrChequeEndDate(1) & arrChequeEndDate(0)
                End If

                .strChequeNo = txtChequeNoSrch.Text.Trim
                .strChequeDateFrom = startChequeDate
                .strChequeDateTo = endChequeDate
                .strVendor_name = txtVendor_NameLst.Text.Trim()
                .strSearchType = rblSearchType.SelectedValue.ToString
            End With

            ' set dto object to session
            Session("objChequePurchaseDto1") = objChequePurchaseDto

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
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtGetChequeList As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtGetChequeList = Session("dtGetChequeList")

            ' check record for display
            If Not IsNothing(dtGetChequeList) AndAlso dtGetChequeList.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtGetChequeList)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtGetChequeList.Rows.Count)
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
    ' Function name : IsDate
    ' Discription     : Check Is date format
    ' Return Value    : True , False
    ' Create User     : Pranitda Sroengklang
    ' Create Date     : 12-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function CheckError() As Boolean
        CheckError = False
        Try
            'check start date
            If txtChequeDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtChequeDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtChequeDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtChequeDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtChequeDateTo.Text.Trim <> "" And txtChequeDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtChequeDateFrom.Text.Trim) > objValidate.IsDate(txtChequeDateTo.Text.Trim) Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            CheckError = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckError", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(10)

            ' set permission 
            btnSearch.Enabled = objAction.actList
            btnAdd.Enabled = objAction.actCreate

            ' set action permission to session
            Session("actList") = objAction.actList
            Session("actDelete") = objAction.actDelete
            Session("actUpdate") = objAction.actUpdate
            Session("actCreate") = objAction.actCreate

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
    '	Create Date	    : 12-07-2013
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
    '	Function name	: DeleteItem
    '	Discription	    : Delete data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteItem()
        Try
            'Dim strChequeNo As String = Session("strChequeNo")
            'Dim strChequeDate As String = ""
            Dim strId As String = Session("strId")
            'strChequeDate = Replace(CDate(Session("strChequeDate")).ToString("yyyy/MM/dd"), "/", "")
            'Dim arrPaymentDate() As String = Split(Session("strChequeDate"), "/")
            'If UBound(arrPaymentDate) > 0 Then
            '    strChequeDate = arrPaymentDate(2) & arrPaymentDate(1) & arrPaymentDate(0)
            'End If

            ' check state of delete item
            If objChequeService.DeleteCheque(strId) = True Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_002"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_07_003"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: clearManagePageSession
    '	Discription	    : clear Session
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 01-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub clearManagePageSession()
        Try
            Session("dtAccountReport") = Nothing
            Session("dtTaxReport") = Nothing
            Session("dtPaymentPaid") = Nothing
            Session("dtPaymentVoucher") = Nothing
            Session("FileNamePaidReport") = Nothing
            Session("FileNameVoucherReport") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("clearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region


End Class
