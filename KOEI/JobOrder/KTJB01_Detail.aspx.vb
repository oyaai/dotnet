Imports System.Data
Imports System.IO
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Job Order
'	Class Name		    : JobOrder_KTJB01_Detail
'	Class Discription	: Webpage for Job Order Detail
'	Create User 		: Suwishaya L.
'	Create Date		    : 20-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB01_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objJobOrderSer As New Service.ImpJobOrderService
    Private objPage As New Utilities.Paging
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private strPathConfigPO As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "PO/")
    Private strPathConfigQuo As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Quotation/")
    Private strPath As String = HttpContext.Current.Server.MapPath("KTB01_Detail.aspx")

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB01_Detail : Job Order")
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
    '	Create Date	    : 20-06-2013
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
    '	Function name	: rptJobOrderInv_ItemDataBound
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderInv_ItemDataBound( _
        ByVal sender As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
    ) Handles rptJobOrderInv.ItemDataBound
        Try
            ' object label 
            Dim lblAmountInv As New Label
            ' find label amount and assign to variable 
            lblAmountInv = DirectCast(e.Item.FindControl("lblAmountInv"), Label)

            'set permission of amount item
            If Not Session("actAmount") Then
                lblAmountInv.Text = "******"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderInv_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderPO_DataBinding
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderPO_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptJobOrderPO.DataBinding
        Try
            ' clear hashtable data
            hashFilePO.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderPO_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderPO_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderPO_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptJobOrderPO.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim strFilePO As String = hashFilePO(e.Item.ItemIndex).ToString()
            Dim strPathPO As String = strPathConfigPO & lblJobOrder.Text.Trim & "\" & strFilePO

            Select Case e.CommandName
                Case "FilePO"
                    'call function OpenFiles for open files.
                    OpenFiles(strPathPO)

            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderPO_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderQuo_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderQuo_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptJobOrderQuo.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim strFileQuo As String = hashFileQuo(e.Item.ItemIndex).ToString()
            Dim strPathQuo As String = strPathConfigQuo & lblJobOrder.Text.Trim & "\" & strFileQuo

            Select Case e.CommandName
                Case "FileQuo"
                    'call function OpenFiles for open files.
                    OpenFiles(strPathQuo)

            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderQuo_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderPO_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderPO_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptJobOrderPO.ItemDataBound
        Try
            ' object label 
            Dim lblAmountPO As New Label
            ' find label amount and assign to variable 
            lblAmountPO = DirectCast(e.Item.FindControl("lblAmountPO"), Label)

            'set permission for amount item
            If Not Session("actAmount") Then
                lblAmountPO.Text = "******"
            End If

            ' Set po file to hashtable
            hashFilePO.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "po_file"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderPO_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderQuo_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderQuo_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptJobOrderQuo.ItemDataBound
        Try
            ' object label 
            Dim lblAmountQuo As New Label
            ' find label amount and assign to variable 
            lblAmountQuo = DirectCast(e.Item.FindControl("lblAmountQuo"), Label)

            'set permission for amount item
            If Not Session("actAmount") Then
                lblAmountQuo.Text = "******"
            End If

            ' Set quo file to hashtable
            hashFileQuo.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "quo_file"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderQuo_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the id keys in ViewState
    ReadOnly Property hashFilePO() As Hashtable
        Get
            If IsNothing(ViewState("hashFilePO")) Then
                ViewState("hashFilePO") = New Hashtable()
            End If
            Return CType(ViewState("hashFilePO"), Hashtable)
        End Get
    End Property

    ' Stores the id keys in ViewState
    ReadOnly Property hashFileQuo() As Hashtable
        Get
            If IsNothing(ViewState("hashFileQuo")) Then
                ViewState("hashFileQuo") = New Hashtable()
            End If
            Return CType(ViewState("hashFileQuo"), Hashtable)
        End Get
    End Property
#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            'set page no for list item 
            Dim flagPage As String = ""
            Dim intPageNoPO As Integer = 1
            Dim intPageNoQuo As Integer = 1
            Dim intPageNoInv As Integer = 1
            If Not Request.QueryString("PageNo") Is Nothing Then
                flagPage = Request.QueryString("FlagPage") & ""
                Select Case flagPage
                    Case 1
                        intPageNoPO = Request.QueryString("PageNo")
                    Case 2
                        intPageNoQuo = Request.QueryString("PageNo")
                    Case 3
                        intPageNoInv = Request.QueryString("PageNo")
                End Select
            End If

            'set menu id to session
            If Not Request.QueryString("menuId") Is Nothing Then
                Session("menuId") = Request.QueryString("menuId")
            End If

            'get data for display on screen
            LoadInitialDetail()
            'get Schedule Rate (THB)
            GetScheduleRate()

            'get data for display PO on screen
            LoadInitialPO()
            ' call function display page
            DisplayPagePO(intPageNoPO)

            'get data for display Quo on screen
            LoadInitialQuo()
            ' call function display page
            DisplayPageQuo(intPageNoQuo)

            'get data for display Invoice on screen
            LoadInitialInvoice()
            ' call function display page
            DisplayPageInv(intPageNoInv)

            ' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialDetail
    '	Discription	    : Load initial data for display on screen
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialDetail()
        Try
            ' Item Dto object for keep return value from service
            Dim objJobOrderDto As New Dto.JobOrderDto
            Dim intJobOrderID As Integer = 0
            Dim sumOtherAmount As Decimal = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intJobOrderID = CInt(objUtility.GetQueryString("id"))
                Session("job_order_id") = intJobOrderID
            Else
                intJobOrderID = Session("job_order_id")
            End If

            ' call function GetJobOrderByID from service
            objJobOrderDto = objJobOrderSer.GetJobOrderDetailList(intJobOrderID)

            ' assign value to control
            With objJobOrderDto
                lblJobOrder.Text = .job_order
                lblIssueDate.Text = .issue_date
                lblCustomer.Text = .customer_name
                lblEndUser.Text = .end_user_name
                lblReceivePo.Text = .receive_po_name
                lblPersonCharge.Text = .person_in_charge_name
                lblJobOrderType.Text = .job_order_type_Detail
                lblBoi.Text = .is_boi_name
                lblCreateAt.Text = .create_at_name
                lblPartname.Text = .part_name
                lblPartNo.Text = .part_no
                lblPartType.Text = .part_type_name
                Session("payment_condition_id") = .payment_condition_id
                lblPaymentCondition.Text = .payment_condition_name
                lblPayment.Text = .term_day
                Session("currency_id") = .currency_id
                lblCurrency.Text = .currency_name
                lblDate1.Text = .hontai_date1
                lblDate2.Text = .hontai_date2
                lblDate3.Text = .hontai_date3
                lblCondition1.Text = .hontai_condition1
                lblCondition2.Text = .hontai_condition2
                lblCondition3.Text = .hontai_condition3
                lblAmount1.Text = Format(Convert.ToDouble(.hontai_amount1), "#,##0.00")
                lblAmount2.Text = Format(Convert.ToDouble(.hontai_amount2), "#,##0.00")
                lblAmount3.Text = Format(Convert.ToDouble(.hontai_amount3), "#,##0.00")
                lblHontaiAmount.Text = Format(Convert.ToDouble(.hontai_amount), "#,##0.00")
                lblTotalAmount.Text = Format(Convert.ToDouble(.total_amount), "#,##0.00")
                'Modify 2013/09/20 (Request by user )
                'sumOtherAmount = CDbl(.total_amount.ToString.Trim.Replace(",", "")) - CDbl(.hontai_amount.ToString.Trim.Replace(",", ""))
                'If .quotation_amount.ToString = "" Or .quotation_amount Is Nothing Then
                '    lblSumOtherAmount.Text = ""
                'Else
                '    lblSumOtherAmount.Text = Format(Convert.ToDouble(.quotation_amount), "#,##0.00")
                'End If

                lblSumOtherAmount.Text = Format(Convert.ToDouble(.total_amount - .hontai_amount), "#,##0.00")

                lblRemarks.Text = .remark 
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialDetail", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialPO
    '	Discription	    : Search job order PO data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialPO()
        Try
            ' table object keep value from item service
            Dim dtJobOrderPO As New DataTable
            ' call function GetJobOrderPOList from JobOrderService
            dtJobOrderPO = objJobOrderSer.GetJobOrderPOList(Session("job_order_id"))
            ' set table object to session
            Session("dtJobOrderPO") = dtJobOrderPO

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialQou
    '	Discription	    : Search job order Quo data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialQuo()
        Try
            ' table object keep value from item service
            Dim dtJobOrderQuo As New DataTable
            ' call function GetJobOrderQuoList from JobOrderService
            dtJobOrderQuo = objJobOrderSer.GetJobOrderQuoList(Session("job_order_id"))
            ' set table object to session
            Session("dtJobOrderQuo") = dtJobOrderQuo

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialQuo", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialInvoice
    '	Discription	    : Search job order Quo data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 07-03-2014
    '*************************************************************/
    Private Sub LoadInitialInvoice()
        Try
            ' table object keep value from item service
            Dim dtJobOrderInvoice As New DataTable

            ' get New Total Sale Invoice Add 2014-03-07
            Dim objTotalSaleInvoiceDto As New DataTable

            ' call function GetJobOrderInvoiceList from JobOrderService
            dtJobOrderInvoice = objJobOrderSer.GetJobOrderInvoiceList(Session("job_order_id"))

            ' get New Total Sale Invoice Add 2014-03-07
            ' objTotalSaleInvoiceDto = objJobOrderSer.getJobOrderInvTotalAmount(Session("job_order_id"))

            ' set table object to session
            Session("dtJobOrderInvoice") = dtJobOrderInvoice

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPagePO
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPagePO(ByVal intPageNo As Integer)
        Try
            ' table object keep value from item service
            Dim dtJobOrderPO As New DataTable
            ' get table object from session 
            dtJobOrderPO = Session("dtJobOrderPO")

            ' check record for display
            If Not IsNothing(dtJobOrderPO) AndAlso dtJobOrderPO.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrderPO)
                ' write paging
                lblPagingPO.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount, 1)
                ' bound data between pageDate with repeater
                rptJobOrderPO.DataSource = pagedData
                rptJobOrderPO.DataBind()
                ' call fucntion set description
                lblDescriptionPO.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrderPO.Rows.Count)
            Else
                ' case not exist data               
                ' clear binding data and clear description
                lblPagingPO.Text = Nothing
                lblDescriptionPO.Text = "&nbsp;"
                rptJobOrderPO.DataSource = Nothing
                rptJobOrderPO.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPagePO", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
            objUtility.RemQueryString("FlagPage")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPageQuo
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageQuo(ByVal intPageNo As Integer)
        Try
            ' table object keep value from item service
            Dim dtJobOrderQuo As New DataTable
            ' get table object from session 
            dtJobOrderQuo = Session("dtJobOrderQuo")

            ' check record for display
            If Not IsNothing(dtJobOrderQuo) AndAlso dtJobOrderQuo.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrderQuo)
                ' write paging
                lblPagingQuo.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount, 2)
                ' bound data between pageDate with repeater
                rptJobOrderQuo.DataSource = pagedData
                rptJobOrderQuo.DataBind()
                ' call fucntion set description
                lblDescriptionQuo.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrderQuo.Rows.Count)
            Else
                ' clear binding data and clear description
                lblPagingQuo.Text = Nothing
                lblDescriptionQuo.Text = "&nbsp;"
                rptJobOrderQuo.DataSource = Nothing
                rptJobOrderQuo.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageQuo", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
            objUtility.RemQueryString("FlagPage")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPageInv
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageInv(ByVal intPageNo As Integer)
        Try
            ' table object keep value from item service
            Dim dtJobOrderInvoice As New DataTable

            ' get table object from session 
            dtJobOrderInvoice = Session("dtJobOrderInvoice")

            ' check record for display
            If Not IsNothing(dtJobOrderInvoice) AndAlso dtJobOrderInvoice.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrderInvoice)
                ' write paging
                lblPagingInv.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount, 3)
                ' bound data between pageDate with repeater
                rptJobOrderInv.DataSource = pagedData
                rptJobOrderInv.DataBind()
                ' call fucntion set description
                lblDescriptionInv.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrderInvoice.Rows.Count)
            Else
                ' case not exist data
                ' clear binding data and clear description
                lblPagingInv.Text = Nothing
                lblDescriptionInv.Text = "&nbsp;"
                rptJobOrderInv.DataSource = Nothing
                rptJobOrderInv.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageInv", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
            objUtility.RemQueryString("FlagPage")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetPaymentCondition
    '	Discription	    : Get payment condition detail
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetPaymentCondition()
        Try
            ' table object keep value from item service
            Dim dtPaymentCon As New DataTable

            ' call function GetItemList from ItemService
            dtPaymentCon = objJobOrderSer.GetPaymentConditionDetail(Session("payment_condition_id"))
            ' set table object to session
            If Not IsNothing(dtPaymentCon) AndAlso dtPaymentCon.Rows.Count > 0 Then
                lblPaymentCondition.Text = dtPaymentCon.Rows(0).Item("payment_condition_detail").ToString & Space(1) & "Days"
            Else
                lblPaymentCondition.Text = ""
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetPaymentCondition", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetScheduleRate
    '	Discription	    : Get Schedule Rate (THB)
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetScheduleRate()
        Try
            Dim intScheculeRate As Decimal
            Dim objScheculeRateSer As New Service.ImpScheculeRateService
            'set format date to yyymmdd
            Dim strDate As String = CDate(lblIssueDate.Text).ToString("yyyyMMdd").ToString
            
            'call GetScheculeRate from ScheculeRate Service 
            intScheculeRate = objScheculeRateSer.GetScheculeRate(Session("currency_id"), strDate)
            'set data to ScheduleRate item
            lblScheduleRate.Text = Format(Convert.ToDouble(intScheculeRate), "#,##0.00000")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetScheduleRate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: OpenFiles
    '	Discription	    : Open file on server
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub OpenFiles(ByVal strPath As String)
        Try
            Dim file As FileInfo = New System.IO.FileInfo(strPath)
            'Case exist file 
            If System.IO.File.Exists(strPath) = True Then
                'Show dialog box for save/open file
                Response.Clear()
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name)
                Response.AddHeader("Content-Length", file.Length.ToString())
                Response.ContentType = "application/...."
                Response.TransmitFile(file.FullName)
                Response.End()
            Else
                ' case load data to temp not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("OpenFiles", ex.Message.ToString, Session("UserName"))
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
            'set menu id from session
            Dim intMenuId As Integer = 1
            If Not Session("menuId") Is Nothing Then
                intMenuId = Session("menuId")
            End If

            ' check permission of Item menu
            objAction = objPermission.CheckPermission(intMenuId)
            ' set action permission to session
            Session("actAmount") = objAction.actAmount

            'set permission for amount item
            If Not Session("actAmount") Then
                lblAmount1.Text = "******"
                lblAmount2.Text = "******"
                lblAmount3.Text = "******"
                lblHontaiAmount.Text = "******"
                lblSumOtherAmount.Text = "******"
                lblTotalAmount.Text = "******"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region
    
End Class
