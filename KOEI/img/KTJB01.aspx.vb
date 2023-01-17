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
'	Class Name		    : JobOrder_KTJB01
'	Class Discription	: Webpage for Job Order
'	Create User 		: Suwishaya L.
'	Create Date		    : 12-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB01
    Inherits System.Web.UI.Page

    Private objSetSession As New Utilities.SetSession
    Private objLog As New Common.Logs.Log
    Private objJobOrderSer As New Service.ImpJobOrderService
    Private csUser As New Service.ImpUserService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
    Private strPathConfigPO As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "PO/")
    Private strPathConfigQuo As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Quotation/")

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            'case session is nothing ,set new session
            If Session("UserID") Is Nothing Then
                If Not Request.QueryString("UserID") Is Nothing Then
                    Session("UserID") = Request.QueryString("UserID") & ""
                    objSetSession.SetSession()
                End If
            End If
            ' write start log
            objLog.StartLog("KTJB01 : Job Order")
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
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnAdd.Click
        Try 
            ' redirect to KTJB02 with Add mode
            Response.Redirect("KTJB02.aspx?Mode=Add")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnIssueReport_Click
    '	Discription	    : export data to pdf file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnIssueReport_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnIssueReport.Click
        Try
            Dim dtJobOrderReport As New DataTable
            Dim dtSumJobOrderReport As New DataTable

            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data
            SearchDataReport()

            'Get Sum Amount data
            SearchSumDataReport()

            ' get table object from session 
            dtJobOrderReport = Session("dtJobOrderReport")
            dtSumJobOrderReport = Session("dtSumJobOrderReport")

            If Not IsNothing(dtJobOrderReport) AndAlso dtJobOrderReport.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTJB02_Issue", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnIssueReport_Click", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: rbtJobFinished_SelectedIndexChanged
    '	Discription	    : Event rbtJobFinishe is select index cheaged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rbtJobFinished_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rbtJobFinished.SelectedIndexChanged
        Try

            Dim intJobFinish As Integer
            intJobFinish = rbtJobFinished.SelectedValue
            'Set enable/Disable to Finish Date 
            SetFinishDate(intJobFinish)
            btnSearch_Click(sender, e)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rbtJobFinished_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
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
    '	Function name	: rptJobOrder_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrder_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptJobOrder.DataBinding
        Try
            ' clear hashtable data
            hashJobOrderID.Clear()
            hashJobOrder.Clear()
            hashJobFinished.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrder_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrder_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrder_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptJobOrder.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intJobOrderID As Integer = CInt(hashJobOrderID(e.Item.ItemIndex).ToString())
            Dim strJobOrder As String = hashJobOrder(e.Item.ItemIndex).ToString()
            Dim boolInuse As Boolean = objJobOrderSer.IsUsedInJobOrder(intJobOrderID, strJobOrder)

            ' set ItemID to session
            Session("intJobOrderID") = intJobOrderID
            Session("strJobOrder") = strJobOrder
            Session("boolInuse") = boolInuse

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB01", strResult, objMessage.GetXMLMessage("KTJB_01_001"))

                Case "Edit"
                    ' redirect to KTJB02
                    Response.Redirect("KTJB02.aspx?Mode=Edit&id=" & intJobOrderID)

                Case "Detail"
                    'redirect to KTJB01_Detail
                    objMessage.ShowPagePopup("KTJB01_Detail.aspx?id=" & intJobOrderID & "&menuId=1", 900, 950, "", True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrder_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrder_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrder_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptJobOrder.ItemDataBound
        Try
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton
            Dim lblAmount As New Label
            Dim lblQuoAmount As New Label 

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            lblQuoAmount = DirectCast(e.Item.FindControl("lblQuoAmount"), Label) 

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If
            'set permission on amount item
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
                lblQuoAmount.Text = "******"
            End If

            ' Set data to hashtable
            hashJobOrderID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashJobOrder.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order"))
            hashJobFinished.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_finished"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrder_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the id keys in ViewState
    ReadOnly Property hashJobOrderID() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrderID")) Then
                ViewState("hashJobOrderID") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrderID"), Hashtable)
        End Get
    End Property

    ' Stores the job_order keys in ViewState
    ReadOnly Property hashJobOrder() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrder")) Then
                ViewState("hashJobOrder") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrder"), Hashtable)
        End Get
    End Property

    ' Stores the job finish keys in ViewState
    ReadOnly Property hashJobFinished() As Hashtable
        Get
            If IsNothing(ViewState("hashJobFinished")) Then
                ViewState("hashJobFinished") = New Hashtable()
            End If
            Return CType(ViewState("hashJobFinished"), Hashtable)
        End Get
    End Property

    Protected Sub txtCustomer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCustomer.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtJobOrderFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJobOrderFrom.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub rbtReceivePo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceivePo.SelectedIndexChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtPartNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartNo.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtPartName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartName.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtIssueDateFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDateFrom.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtFinishDateFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFinishDateFrom.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtJobOrderTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtJobOrderTo.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtIssueDateTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssueDateTo.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub txtFinishDateTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFinishDateTo.TextChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub ddlJobOrderType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJobOrderType.SelectedIndexChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub rbtBoi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtBoi.SelectedIndexChanged
        btnSearch_Click(sender, e)
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim dtJobOrderReport As New DataTable

            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data
            SearchDataReport()

            ' get table object from session 
            dtJobOrderReport = Session("dtJobOrderReport")

            If Not IsNothing(dtJobOrderReport) AndAlso dtJobOrderReport.Rows.Count > 0 Then
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
            ' call function check permission
            CheckPermission()

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                If Request.QueryString("Flag") = "" Or Request.QueryString("Flag") Is Nothing Then
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                Else
                    'set data on session to item
                    SetSessionToItem()
                    ' call function search data
                    SearchData()
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                End If
            End If

            ' call function set job Type dropdownlist
            LoadListJobType()

            ' set search text to session
            SetSessionToItem()

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteJobOrder()
            End If

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
    '	Create Date	    : 10-06-2013
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

            'Check format date of field Finished Date From
            If txtFinishDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtFinishDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Finished Date To
            If txtFinishDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtFinishDateTo.Text.Trim) = False Then
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
            If txtFinishDateFrom.Text.Trim <> "" And txtFinishDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtFinishDateFrom.Text.Trim, txtFinishDateTo.Text.Trim) = False Then
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
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtJobOrder As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtJobOrder = Session("dtJobOrder")

            ' check record for display
            If Not IsNothing(dtJobOrder) AndAlso dtJobOrder.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrder)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptJobOrder.DataSource = pagedData
                rptJobOrder.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrder.Rows.Count)
            Else
                ' case not exist data
                ' show message box         
                If Request.QueryString("Flag") = "" Or Request.QueryString("Flag") Is Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                ' clear binding data and clear description
                Request.QueryString("Flag") = ""
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptJobOrder.DataSource = Nothing
                rptJobOrder.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtJobOrder As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetJobOrderList from JobOrderService
            dtJobOrder = objJobOrderSer.GetJobOrderList(Session("objJobOrderDto"))

            ' set table object to session
            Session("dtJobOrder") = dtJobOrder

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtJobOrderReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetJobOrderList from JobOrderService
            Session("dtJobOrderReport") = Nothing
            dtJobOrderReport = objJobOrderSer.GetJobOrderReportList(Session("objJobOrderDto"), Session("actAmount"))

            ' set table object to session
            Session("dtJobOrderReport") = dtJobOrderReport

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchSumDataReport
    '	Discription	    : Search job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchSumDataReport()
        Try
            ' table object keep value from item service
            Dim dtSumJobOrderReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetJobOrderList from JobOrderService
            Session("dtSumJobOrderReport") = Nothing
            dtSumJobOrderReport = objJobOrderSer.GetSumHontaiAmountReport(Session("objJobOrderDto"), Session("actAmount"))

            ' set table object to session
            Session("dtSumJobOrderReport") = dtSumJobOrderReport

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchSumDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of job order screen
            objAction = objPermission.CheckPermission(1)

            ' set permission to button
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList
            btnIssueReport.Enabled = objAction.actList

            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete
            Session("actAmount") = objAction.actAmount

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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtJobOrder") = Nothing
            Session("txtJobOrderFrom") = Nothing
            Session("txtJobOrderTo") = Nothing
            Session("txtCustomer") = Nothing
            Session("rbtReceivePo") = Nothing
            Session("txtIssueDateFrom") = Nothing
            Session("txtIssueDateTo") = Nothing
            Session("rbtJobFinished") = Nothing
            Session("txtFinishDateFrom") = Nothing
            Session("txtFinishDateTo") = Nothing
            Session("txtPartNo") = Nothing
            Session("txtPartName") = Nothing
            Session("ddlJobOrderType") = Nothing 
            Session("rbtBoi") = Nothing
            Session("intJobOrderID") = Nothing
            Session("strJobOrder") = Nothing
            Session("boolInuse") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing
            Session("actAmount") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Job Order dto object and set parameter
            Dim objJoborderDto As New Dto.JobOrderDto
            Dim strJobFinish As String = ""
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = ""
            Dim finishDateDateFrom As String = ""
            Dim finishDateDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtIssueDateFrom.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtIssueDateTo.Text.Trim(), "/")
            Dim arrFinishedDateFrom() As String = Split(txtFinishDateFrom.Text.Trim(), "/")
            Dim arrFinishedDateTo() As String = Split(txtFinishDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objJoborderDto
                'Set Issue date from to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                'Set Issue date To to format yyymmdd
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If
                'Set finish date From to format yyymmdd
                If UBound(arrFinishedDateFrom) > 0 Then
                    finishDateDateFrom = arrFinishedDateFrom(2) & arrFinishedDateFrom(1) & arrFinishedDateFrom(0)
                End If
                'Set finish date To to format yyymmdd
                If UBound(arrFinishedDateTo) > 0 Then
                    finishDateDateTo = arrFinishedDateTo(2) & arrFinishedDateTo(1) & arrFinishedDateTo(0)
                End If
                'set job finished
                If rbtJobFinished.SelectedValue = "" Then
                    If finishDateDateFrom.Trim.Length > 0 Or finishDateDateTo.Trim.Length > 0 Then
                        strJobFinish = "1"
                    End If
                End If
                If rbtJobFinished.SelectedValue = "" Then
                    .Job_finish_search = strJobFinish
                Else
                    .Job_finish_search = rbtJobFinished.SelectedValue
                End If

                .job_order_from_search = txtJobOrderFrom.Text.Trim
                .job_order_to_search = txtJobOrderTo.Text.Trim
                .customer_search = txtCustomer.Text.Trim
                .receive_po_search = rbtReceivePo.SelectedValue
                .issue_date_from_search = issueDateDateFrom
                .issue_date_to_search = issueDateDateTo
                .finish_date_from_search = finishDateDateFrom
                .finish_date_to_search = finishDateDateTo
                .part_name_search = txtPartName.Text.Trim
                .part_no_search = txtPartNo.Text.Trim
                .job_type_search = ddlJobOrderType.SelectedValue
                .boi_search = rbtBoi.SelectedValue

            End With

            ' set dto object to session
            Session("objJoborderDto") = objJoborderDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteJobOrder
    '	Discription	    : Delete job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteJobOrder()
        Try
            ' check flag in_used
            If Session("boolInuse") Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_002"))
                Exit Sub
            End If

            ' check state of delete item
            If objJobOrderSer.DeleteJobOrder(Session("intJobOrderID"), Session("strJobOrder")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteFolderTemp
    '	Discription	    : Delete Folder Temp
    '	Return Value	: Integer
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub DeleteFolderTemp(ByVal strJobOrder As String)
        Try
            Dim strFolderTemp As String
            strFolderTemp = strJobOrder & "_Temp"

            'get PO job order folder Temp path
            Dim strPathPoTemp As String = strPathConfigPO & strFolderTemp

            'get Quotation job order folder Temp path
            Dim strPathQuoTemp As String = strPathConfigQuo & strFolderTemp

            'Delete po folder
            If Directory.Exists(strPathPoTemp) And Not strPathPoTemp.Equals(strPathConfigPO) Then
                Directory.Delete(strPathPoTemp, True)
            End If
            'Delete Quotation folder
            If Directory.Exists(strPathQuoTemp) And Not strPathQuoTemp.Equals(strPathConfigQuo) Then
                Directory.Delete(strPathQuoTemp, True)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteFolderTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetFinishDate
    '	Discription	    : Set Enable/disable finish date 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetFinishDate(ByVal intJobFinish As Integer)
        Try
            'case select Finished Job is "No" then Job Finished date can't used.
            If intJobFinish = 0 Then
                txtFinishDateFrom.Enabled = False
                txtFinishDateTo.Enabled = False
            Else
                'case select Finished Job is "Yes" then Job Finished date can used.
                txtFinishDateFrom.Enabled = True
                txtFinishDateTo.Enabled = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetFinishDate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListJobType
    '	Discription	    : Load list Job Type function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListJobType()
        Try
            ' object Vendor service
            Dim objJobTypeSer As New Service.ImpJobTypeService
            ' listJobTypeDto for keep value from service
            Dim listJobTypeDto As New List(Of Dto.JobTypeDto)
            ' call function GetJobTypeForList from service
            listJobTypeDto = objJobTypeSer.GetJobTypeForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlJobOrderType, listJobTypeDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlJobOrderType")) And ddlJobOrderType.Items.Count > 0 Then
                ddlJobOrderType.SelectedValue = Session("ddlJobOrderType")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListJobType", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSessionToItem
    '	Discription	    : Set data to item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set search text to session
            txtJobOrderFrom.Text = Session("txtJobOrderFrom")
            txtJobOrderTo.Text = Session("txtJobOrderTo")
            txtCustomer.Text = Session("txtCustomer")
            rbtReceivePo.SelectedValue = Session("rbtReceivePo")
            txtIssueDateFrom.Text = Session("txtIssueDateFrom")
            txtIssueDateTo.Text = Session("txtIssueDateTo")
            rbtJobFinished.SelectedValue = Session("rbtJobFinished")
            txtFinishDateFrom.Text = Session("txtFinishDateFrom")
            txtFinishDateTo.Text = Session("txtFinishDateTo")
            txtPartNo.Text = Session("txtPartNo")
            txtPartName.Text = Session("txtPartName")
            ddlJobOrderType.SelectedValue = Session("ddlJobOrderType")
            rbtBoi.SelectedValue = Session("rbtBoi")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtJobOrderFrom") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderTo") = txtJobOrderTo.Text.Trim
            Session("txtCustomer") = txtCustomer.Text.Trim
            If rbtReceivePo.SelectedIndex = -1 Then
                Session("rbtReceivePo") = Nothing
            Else
                Session("rbtReceivePo") = rbtReceivePo.SelectedValue
            End If
            Session("txtIssueDateFrom") = txtIssueDateFrom.Text.Trim
            Session("txtIssueDateTo") = txtIssueDateTo.Text.Trim
            Session("rbtJobFinished") = rbtJobFinished.SelectedValue
            Session("txtFinishDateFrom") = txtFinishDateFrom.Text.Trim
            Session("txtFinishDateTo") = txtFinishDateTo.Text.Trim
            Session("txtPartNo") = txtPartNo.Text.Trim
            Session("txtPartName") = txtPartName.Text.Trim
            Session("ddlJobOrderType") = ddlJobOrderType.SelectedValue
            If rbtReceivePo.SelectedIndex = -1 Then
                Session("rbtBoi") = Nothing
            Else
                Session("rbtBoi") = rbtBoi.SelectedValue
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    ' Function name : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Suwishaya L.
    ' Create Date     : 02-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim rowCount As Integer = 3

            Dim pck As ExcelPackage = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "IssueJobOrderReport.xlsx"))))
            Dim wBook As OfficeOpenXml.ExcelWorksheet = pck.Workbook.Worksheets(1)

            'set header
            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)

            Dim DT As DataTable = Session("dtJobOrderReport")

            'Header already have in templete file
            For i As Integer = 0 To DT.Rows.Count - 1
                wBook.Cells(rowCount + 1, 1).Value = DT.Rows(i)("job_order").ToString()
                wBook.Cells(rowCount + 1, 2).Value = DT.Rows(i)("issue_date").ToString()
                wBook.Cells(rowCount + 1, 3).Value = DT.Rows(i)("issue_by").ToString()
                wBook.Cells(rowCount + 1, 4).Value = DT.Rows(i)("customer").ToString()
                wBook.Cells(rowCount + 1, 5).Value = DT.Rows(i)("part_name").ToString()
                wBook.Cells(rowCount + 1, 6).Value = DT.Rows(i)("part_no").ToString()
                wBook.Cells(rowCount + 1, 7).Value = DT.Rows(i)("part_type").ToString()
                wBook.Cells(rowCount + 1, 8).Value = DT.Rows(i)("job_mod").ToString()
                wBook.Cells(rowCount + 1, 9).Value = DT.Rows(i)("job_new").ToString()
                wBook.Cells(rowCount + 1, 10).Value = DT.Rows(i)("Detail").ToString()
                wBook.Cells(rowCount + 1, 11).Value = DT.Rows(i)("quo_date").ToString()
                wBook.Cells(rowCount + 1, 12).Value = DT.Rows(i)("quo_no").ToString()
                wBook.Cells(rowCount + 1, 13).Value = DT.Rows(i)("currency").ToString()
                'set amount follow permission
                If Not Session("actAmount") Then
                    wBook.Cells(rowCount + 1, 14).Value = "******"
                    wBook.Cells(rowCount + 1, 15).Value = "******"
                Else
                    wBook.Cells(rowCount + 1, 14).Value = CDbl(DT.Rows(i)("hontai_amount"))
                    wBook.Cells(rowCount + 1, 15).Value = CDbl(DT.Rows(i)("hontai_amount_thb"))
                End If

                'write border
                wBook.Cells(rowCount + 1, 1).Style.Border.Left.Style = ExcelBorderStyle.Thin
                For z As Integer = 1 To 15
                    wBook.Cells(rowCount + 1, z).Style.Border.Right.Style = ExcelBorderStyle.Thin
                    wBook.Cells(rowCount + 1, z).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                Next

                rowCount += 1
            Next

            'Summary footer report 
            wBook.Cells(rowCount + 1, 13).Value = "Total"
            wBook.Cells(rowCount + 1, 13).Style.Font.Bold = True
            'set amount follow permission
            If Not Session("actAmount") Then
                wBook.Cells(rowCount + 1, 14).Value = "******"
                wBook.Cells(rowCount + 1, 15).Value = "******"
            Else
                wBook.Cells(rowCount + 1, 14).Formula = "=SUM(N4:N" & rowCount & ")"
                wBook.Cells(rowCount + 1, 15).Formula = "=SUM(O4:O" & rowCount & ")"
            End If

            'write border
            wBook.Cells(rowCount + 1, 14).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 14).Style.Border.Left.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 14).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 15).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 15).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=IssueJobOrderReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function
#End Region

End Class
