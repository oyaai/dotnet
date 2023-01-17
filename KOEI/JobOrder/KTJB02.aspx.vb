Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Web.Configuration
Imports System.Web.Services.WebMethodAttribute

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : JobOrder_KTJB02
'	Class Discription	: Webpage for maintenance Job Order master
'	Create User 		: Suwishaya L.
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

Partial Class JobOrder_KTJB02
    Inherits System.Web.UI.Page

    Private objSetSession As New Utilities.SetSession
    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private csUser As New Service.ImpUserService
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objJobOrderSer As New Service.ImpJobOrderService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty
    Private strPathConfigPO As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "PO/")
    Private strPathConfigQuo As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Quotation/")
    Private objSaleInvoiceSer As New Service.ImpSale_InvoiceService

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' set new session when session is nothing 
            If Session("UserID") Is Nothing Then
                If Not Request.QueryString("UserID") Is Nothing Then
                    Session("UserID") = Request.QueryString("UserID") & ""
                    objSetSession.SetSession()
                End If
            End If
            ' write start log
            objLog.StartLog("KTJB02 : Job Order")

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
    '	Create Date	    : 17-06-2013
    '	Update User	    : Suwishaya L.
    '	Update Date	    : 26-09-2013
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            'check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                InitialPage()
            Else
                If lblJobOrderHid.Text.Length > 0 Then
                    Session("job_order_id") = Convert.ToInt32(lblJobOrderHid.Text)
                End If
            End If
            'Add 2013/09/26 (Req No.22)
            'Case modify re-generate job order no. follow changed Issue Date. 
            'If Session("Mode") = "Edit" Then
            '    If Not Session("status_id") Is Nothing Or Session("status_id").ToString <> "" Then
            '        'call function GenerateJobOrderNo
            '        GenerateJobOrderNo()
            '    End If
            'End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            'Redirect to job order screen
            Response.Redirect("KTJB01.aspx?New=False&Flag=Ins")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            'call funtion SetVisibleLable
            SetVisibleLable()
            'call function ClearTamp
            ClearTemp()
            ' call function ClearControl
            ClearControl()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            'Check input Criteria data
            If CheckCriteriaInput() = False Then
                'Delete 2013/09/20 (Delete call funtion GetSumPoAmount )
                'cal function for get sum po amount
                'GetSumPoAmount()

                'cal function for get sum quotation amount
                GetSumQuoAmount()
                'call function for get total amount
                GetTotalAmount()
                'calculate hotai amount
                SetHotaiAmount()
                Exit Sub
            End If

            ' call function set session dto
            SetValueToDto()

            'If rbtReceivePo.SelectedValue = "0" Then
            '    If txtHontaiAmount.Text = "" Then
            '        txtHontaiAmount.Focus()
            '    End If
            'Else

            'End If


            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTJB02", strConfirmIns, objMessage.GetXMLMessage("KTJB_02_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTJB02", strConfirmUpd, objMessage.GetXMLMessage("KTJB_02_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rbtCreateAt_SelectedIndexChanged
    '	Discription	    : Event rbtCreateAt is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rbtCreateAt_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rbtCreateAt.SelectedIndexChanged
        Try
            'cal function GetSumPoAmount for get sum PO amount
            'GetSumPoAmount()
            'cal function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()
            'call function GetScheduleRate for Get Schedule Rate (THB) 
            GetScheduleRate()

            'when select Own Company then can used on textbox of Own Company 
            If rbtCreateAt.SelectedValue = "1" Then
                txtOwnCompany.Enabled = True
                txtOwnCompany.Focus()
            Else
                txtOwnCompany.Enabled = False
                txtPartName.Focus()
                txtOwnCompany.Text = String.Empty
            End If

            'set Require check of CreateAt
            If ddlJobOrder.SelectedValue = "2" Or ddlJobOrder.SelectedValue = "3" Then
                lblChkReq.Visible = True
            Else
                lblChkReq.Visible = False
            End If
            'Delete 2013/09/20 (delete function GetSumPoAmount)
            'cal function for get sum po amount
            'GetSumPoAmount()

            'cal function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call SetHotaiAmount function for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rbtCreateAt_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rbtReceivePo_SelectedIndexChanged
    '	Discription	    : Event rbtReceivePo is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rbtReceivePo_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rbtReceivePo.SelectedIndexChanged
        Try
            'Delete 2013/09/20 (delete function GetSumPoAmount)
            'cal function for get sum po amount
            'GetSumPoAmount()

            'call function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()

            'Check Require of CreateAt
            If ddlJobOrder.SelectedValue = "2" Or ddlJobOrder.SelectedValue = "3" Then
                lblChkReq.Visible = True
            Else
                lblChkReq.Visible = False
            End If


            'set upload po/quotation can use or not
            If Session("boolInuse") Then
                lblUploadPO.Visible = True
                lblUploadQuo.Visible = True
                linkUploadPO.Visible = False
                linkUploadQuotation.Visible = False
            Else
                'when select Receive Po is NO ,Link Upload PO can't used
                If rbtReceivePo.SelectedValue = "0" Then
                    lblUploadPO.Visible = True
                    linkUploadPO.Visible = False
                    linkUploadPO.Enabled = False
                    reqValidatorHontaiAmount.Enabled = True
                Else
                    lblUploadPO.Visible = False
                    linkUploadPO.Visible = True
                    linkUploadPO.Enabled = True
                    reqValidatorHontaiAmount.Enabled = False
                End If
            End If

            If rbtReceivePo.SelectedValue = 1 Then
                txtHontaiAmount.Focus()

                If txtHontaiAmount.Text = String.Empty Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_009"))
                End If

            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rbtReceivePo_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ddlCurrency_SelectedIndexChanged
    '	Discription	    : Event ddlCurrency is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 16-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ddlCurrency_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ddlCurrency.SelectedIndexChanged
        Try
            'Delete 2013/09/20 (delete function GetSumPoAmount)
            'cal function for get sum po amount
            'GetSumPoAmount()

            'cal function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable 
            SetVisibleLable()
            'call function GetScheduleRate for Get Schedule Rate (THB) 
            GetScheduleRate()

            'Check Require of CreateAt
            If ddlJobOrder.SelectedValue = "2" Or ddlJobOrder.SelectedValue = "3" Then
                lblChkReq.Visible = True
            Else
                lblChkReq.Visible = False
            End If

            ddlCurrency.Focus()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ddlCurrency_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ddlJobOrder_SelectedIndexChanged
    '	Discription	    : Event ddlJobOrder is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ddlJobOrder_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ddlJobOrder.SelectedIndexChanged

        'cal function GetSumPoAmount for get sum PO amount
        'GetSumPoAmount()
        'cal function GetSumQuoAmount for get sum quotation amount
        GetSumQuoAmount()
        'call function GetTotalAmount for get total amount
        GetTotalAmount()
        'call function SetHotaiAmount for calculate hotai amount
        SetHotaiAmount()
        'call function SetVisibleLable
        SetVisibleLable()
        'call function GetScheduleRate for Get Schedule Rate (THB) 
        GetScheduleRate()

        'call function SetCreateAt for set enable on create at item  
        SetCreateAt(ddlJobOrder.SelectedValue)

    End Sub

    '/**************************************************************
    '	Function name	: ddlPaymentCondition_SelectedIndexChanged
    '	Discription	    : Event ddlPaymentCondition is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ddlPaymentCondition_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ddlPaymentCondition.SelectedIndexChanged
        Try
            'set data from payment condition to condition1,condition2,condition3
            Dim arrCodition() As String = Split(ddlPaymentCondition.SelectedItem.Text.Trim(), "%")
            If UBound(arrCodition) > 0 Then
                txtCondition1.Text = arrCodition(0)
                txtCondition2.Text = arrCodition(1)
                txtCondition3.Text = arrCodition(2)
            End If

            ' Set Input Payment Condition 
            If arrCodition(0).ToString = "0" Then
                ChkHontai1.Enabled = False
                ChkHontai1.Checked = False
                txtDate1.Text = String.Empty
                txtDate1.Enabled = False

            End If

            If arrCodition(1).ToString = "0" Then
                ChkHontai2.Enabled = False
                ChkHontai2.Checked = False
                txtDate2.Text = String.Empty
                txtDate2.Enabled = False
            Else

                If ChkHontai1.Checked = True Then
                    ChkHontai2.Enabled = True
                    txtDate2.Enabled = True
                End If

            End If

            If arrCodition(2).ToString = "0" Then
                ChkHontai3.Enabled = False
                ChkHontai3.Checked = False
                txtDate3.Text = String.Empty
                txtDate3.Enabled = False
            Else
                If ChkHontai2.Checked = True Then
                    ChkHontai3.Enabled = True
                    txtDate3.Enabled = True
                End If
            End If

            Session("PaymentCodition1") = arrCodition(0).ToString
            Session("PaymentCodition2") = arrCodition(1).ToString
            Session("PaymentCodition3") = arrCodition(2).ToString

            'Delete 2013/09/20 (delete function GetSumPoAmount)
            'cal function for get sum po amount
            'GetSumPoAmount()

            'cal function SetVisibleLable
            SetVisibleLable()
            'cal function for get sum quotation amount
            GetSumQuoAmount()
            'call function for get total amount
            GetTotalAmount()
            'calculate hotai amount
            SetHotaiAmount()

            'Check Require of CreateAt
            If ddlJobOrder.SelectedValue = "2" Or ddlJobOrder.SelectedValue = "3" Then
                lblChkReq.Visible = True
            Else
                lblChkReq.Visible = False
            End If
            'set focus on payment condition 
            txtPaymentCondition.Focus()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ddlPaymentCondition_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: linkUploadPO_Click
    '	Discription	    : Event linkUploadPO item click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/ 
    Protected Sub linkUploadPO_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles linkUploadPO.Click
        Try
            'cal function GetSumPoAmount for get sum PO amount
            'GetSumPoAmount()
            'cal function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()
            'call function GetScheduleRate for Get Schedule Rate (THB) 
            GetScheduleRate()


            'redirect to KTJB02_Po screen
            If Session("Mode") = "Add" Then
                objMessage.ShowPagePopup("KTJB02_PO.aspx?Mode=Add&HontaiAmount=" & txtHontaiAmount.Text, 920, 1000, "", True)
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ShowPagePopup("KTJB02_PO.aspx?Mode=Edit&id=" & Session("job_order_id") & "&job_order=" & lblJobOrder.Text & "&HontaiAmount=" & txtHontaiAmount.Text, 920, 1000, "", True)
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("linkUploadPO_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: linkUploadQuotation_Click
    '	Discription	    : Event linkUploadQuotation item click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/ 
    Protected Sub linkUploadQuotation_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles linkUploadQuotation.Click
        Try
            'cal function GetSumPoAmount for get sum PO amount
            'GetSumPoAmount()
            'cal function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()
            'call function GetScheduleRate for Get Schedule Rate (THB) 
            GetScheduleRate()

            'redirect to KTJB02_Po 
            If Session("Mode") = "Add" Then
                objMessage.ShowPagePopup("KTJB02_Quo.aspx?Mode=Add", 920, 1000, "", True)
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ShowPagePopup("KTJB02_Quo.aspx?Mode=Edit&id=" & Session("job_order_id") & "&job_order=" & lblJobOrder.Text, 920, 1000, "", True)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("linkUploadPO_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: txtIssueDate_TextChanged
    '	Discription	    : Event issue date is change
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L
    '	Create Date	    : 19-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/ 
    Protected Sub txtIssueDate_TextChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles txtIssueDate.TextChanged
        Try
            'set job order no
            GenerateJobOrderNo()
            'cal function GetSumPoAmount for get sum PO amount
            'GetSumPoAmount()
            'call function GetSumQuoAmount for get sum quotation amount
            GetSumQuoAmount()
            'call function GetTotalAmount for get total amount
            GetTotalAmount()
            'call function SetHotaiAmount for calculate hotai amount
            SetHotaiAmount()
            'call function SetVisibleLable
            SetVisibleLable()
            'cal function calulate schedule rate
            GetScheduleRate()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("txtIssueDate_TextChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' call function check permission
            CheckPermission()
            'get ip address
            GetIpAddress()
            'call function SetVisibleLable
            SetVisibleLable()
            'Set data to drowndrown list
            SetDataDropdrownList()

            'setEnabledChk()
            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                ' call function clear session
                ClearSession()
                'Insert Job Order
                InsertJobOrder()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateJobOrder()

                'Select Invoice No.
                'SelectIntIdJobPO()

                'If Session("objIDSaleInv") <> "" Then
                '    'Update SaleInvoice 
                '    UpdateSaleInvoice()
                'End If

                Exit Sub
            End If

            'call function ClearTempTable for Clear data job order temp
            ClearTempTable()

            'Set QueryString
            If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                'Session("job_order_id") = 0
                Session("job_order_id") = lblJobOrderHid.Text
            Else
                Session("job_order_id") = Request.QueryString("id")
                lblJobOrderHid.Text = Request.QueryString("id")
            End If


            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
                'set job order no
                GenerateJobOrderNo()
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                'get data for display on screen
                LoadInitialUpdate()
                ' Set POTotalAmount with out Hontai
                ' table object keep value from item service
                Dim dtTotalAmount As New DataTable

                ' call function GetTotalAmount from JobOrderService
                dtTotalAmount = objJobOrderSer.GetTotalAmount(hidIpAddress.Value)
                hidPOTotalAmount.Value = dtTotalAmount.Rows(0).Item("total_po_amount").ToString
                'Delete 2013/09/23 : new request ->user can open upload PO/Quotation 
                'Check data use job order po table
                'CheckUseInJobOrderPO()

            End If

            'call function GetScheduleRate for Get Schedule Rate (THB) 
            GetScheduleRate()

            txtIssueDate.Focus()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Special Job Order menu
            objAction = objPermission.CheckPermission(1)
            ' set permission button
            btnSave.Enabled = objAction.actCreate
            btnClear.Enabled = objAction.actList
            Session("actAmount") = objAction.actAmount

            'set permission for amount item
            If Not Session("actAmount") Then
                '--Amount 
                lblHidHontaiAmount.Visible = True
                lblHidAmount1.Visible = True
                lblHidAmount2.Visible = True
                lblHidAmount3.Visible = True
                lblHidSumOtherAmount.Visible = True
                lblHidTotalAmount.Visible = True
                '-----
                lblHontaiAmount.Visible = False
                txtHontaiAmount.Visible = False
                txtAmount1.Visible = False
                txtAmount2.Visible = False
                txtAmount3.Visible = False
                'lblSumOtherAmount.Visible = False wall
                lblTotalAmount.Visible = False

                '--Amount THB
                lblHontaiAmountThb.Visible = False
                lblHontai1AmountThb.Visible = False
                lblHontai2AmountThb.Visible = False
                lblHontai3AmountThb.Visible = False
                lblTotalAmountThb.Visible = False
                lblSumOthersAmountThb.Visible = False

                lblHidHontaiAmountThb.Visible = True
                lblHidHontai1AmountThb.Visible = True
                lblHidHontai2AmountThb.Visible = True
                lblHidHontai3AmountThb.Visible = True
                lblHidSumOthersAmountThb.Visible = True
                lblHidTotalAmountThb.Visible = True
            Else
                '--Amount 
                lblHidHontaiAmount.Visible = False
                lblHidAmount1.Visible = False
                lblHidAmount2.Visible = False
                lblHidAmount3.Visible = False
                lblHidSumOtherAmount.Visible = False
                lblHidTotalAmount.Visible = False
                '---
                'Mod 2013/09/19
                'lblHontaiAmount.Visible = True
                lblHontaiAmount.Visible = False
                txtHontaiAmount.Visible = True
                txtAmount1.Visible = True
                txtAmount2.Visible = True
                txtAmount3.Visible = True

                lblSumOtherAmount.Visible = True
                lblTotalAmount.Visible = True

                '--Amount THB
                lblHontaiAmountThb.Visible = True
                lblHontai1AmountThb.Visible = True
                lblHontai2AmountThb.Visible = True
                lblHontai3AmountThb.Visible = True
                lblTotalAmountThb.Visible = True
                lblSumOthersAmountThb.Visible = True

                lblHidHontaiAmountThb.Visible = False
                lblHidHontai1AmountThb.Visible = False
                lblHidHontai2AmountThb.Visible = False
                lblHidHontai3AmountThb.Visible = False
                lblHidSumOthersAmountThb.Visible = False
                lblHidTotalAmountThb.Visible = False

            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataDropdrownList
    '	Discription	    : Set data into dropdrown list
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataDropdrownList()
        Try
            ' call function set Vendor dropdownlist
            LoadListVendor()
            ' call function set user(Person in charge) dropdownlist
            LoadListUser()
            ' call function set job type dropdownlist
            LoadListJobType()
            'call function set payment condition dropdownlist
            LoadListPayCon()
            'call function set payment term dropdownlist
            LoadListPayTerm()
            'call function set currency dropdownlist
            LoadListCurrency()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataDropdrownList", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("job_order_id") = Nothing
            Session("JobOrderNoOld") = Nothing
            Session("JobOrderNo") = Nothing
            Session("Month") = Nothing
            Session("JobLast") = Nothing
            Session("Year") = Nothing
            Session("ipAddress") = Nothing
            Session("ddlCustomer") = Nothing
            Session("ddlEndUser") = Nothing
            Session("ddlPersonCharge") = Nothing
            Session("ddlJobOrder") = Nothing
            Session("ddlPaymentCondition") = Nothing
            Session("ddlPayment") = Nothing
            Session("ddlCurrency") = Nothing 
            Session("SumPoAmount") = Nothing
            Session("TotalPoAmount") = Nothing
            Session("actAmount") = Nothing
            Session("status_id") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            'Redirect to KTJB02 screen
            If Session("Mode") = "Add" Then
                Response.Redirect("KTJB02.aspx?Mode=Add&UserID=" & Session("UserID"))
            Else
                Response.Redirect("KTJB02.aspx?Mode=Edit&id=" & Session("job_order_id") & "&UserID=" & Session("UserID"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    : Wasan D.
    '	Update Date	    : 27-09-2013
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objJobOrderDto As New Dto.JobOrderDto
            Dim issueDate As String = ""
            Dim hontaiDate1 As String = ""
            Dim hontaiDate2 As String = ""
            Dim hontaiDate3 As String = ""
            Dim hontai1 As String = ""
            Dim hontai2 As String = ""
            Dim hontai3 As String = ""

            'Replace date dd/mm/yyyy to array
            Dim arrIssueDate() As String = Split(txtIssueDate.Text.Trim(), "/")
            Dim arrHontaiDate1() As String = Split(txtDate1.Text.Trim(), "/")
            Dim arrHontaiDate2() As String = Split(txtDate2.Text.Trim(), "/")
            Dim arrHontaiDate3() As String = Split(txtDate3.Text.Trim(), "/")



            'set issue date to yyyymmdd format
            If UBound(arrIssueDate) > 0 Then
                issueDate = arrIssueDate(2) & arrIssueDate(1) & arrIssueDate(0)
            End If
            'set hontai date 1 to yyyymmdd format
            If UBound(arrHontaiDate1) > 0 Then
                hontaiDate1 = arrHontaiDate1(2) & arrHontaiDate1(1) & arrHontaiDate1(0)
            End If
            'set hontai date 2 to yyyymmdd format
            If UBound(arrHontaiDate2) > 0 Then
                hontaiDate2 = arrHontaiDate2(2) & arrHontaiDate2(1) & arrHontaiDate2(0)
            End If
            'set hontai date 3 to yyyymmdd format
            If UBound(arrHontaiDate3) > 0 Then
                hontaiDate3 = arrHontaiDate3(2) & arrHontaiDate3(1) & arrHontaiDate3(0)
            End If
            'Set hontai 1st (0=Unchecked, 1 = Checked)
            If ChkHontai1.Checked Then
                hontai1 = "1"
            Else
                hontai1 = "0"
            End If

            'Set hontai 2nd (0=Unchecked, 1 = Checked)
            If ChkHontai2.Checked Then
                hontai2 = "1"
            Else
                hontai2 = "0"
            End If

            'Set hontai 3rd (0=Unchecked, 1 = Checked)
            If ChkHontai3.Checked Then
                hontai3 = "1"
            Else
                hontai3 = "0"
            End If


            Dim tmpRbtReceivePo As String
            If rbtReceivePo.SelectedValue = "" Then
                tmpRbtReceivePo = 0
            Else
                tmpRbtReceivePo = rbtReceivePo.SelectedValue
            End If

            ' if don't input Hontai Amount, this's system fix 0.00 
            Dim tmpTxtHontaiAmount As String
            If txtHontaiAmount.Text = "" Then
                tmpTxtHontaiAmount = "0.00"
            Else
                tmpTxtHontaiAmount = txtHontaiAmount.Text
            End If


            ' assign value to dto object
            With objJobOrderDto
                .id = Session("job_order_id")
                '.job_order = Session("JobOrderNo")
                .issue_date = issueDate
                .customer = ddlCustomer.SelectedValue
                .end_user = ddlEndUser.SelectedValue
                .receive_po = tmpRbtReceivePo
                .person_in_charge = ddlPersonCharge.SelectedValue
                .hontai_amount = tmpTxtHontaiAmount.Replace(",", "")
                .job_type_id = ddlJobOrder.SelectedValue
                .is_boi = IIf(String.IsNullOrEmpty(rbtBOI.SelectedValue), 0, rbtBOI.SelectedValue)
                'Modify 2013/06/19
                '.create_at = IIf(String.IsNullOrEmpty(rbtCreateAt.SelectedValue), 0, rbtCreateAt.SelectedValue)
                If .job_type_id = 1 Then
                    .create_at = 0
                Else
                    .create_at = IIf(String.IsNullOrEmpty(rbtCreateAt.SelectedValue), 0, rbtCreateAt.SelectedValue)
                End If

                .part_name = txtPartName.Text.Trim
                .part_no = txtPartNo.Text.Trim
                .part_type = IIf(String.IsNullOrEmpty(rbtPartType.SelectedValue), 0, rbtPartType.SelectedValue)
                .payment_term_id = ddlPayment.SelectedValue
                .currency_id = ddlCurrency.SelectedValue
                .hontai_chk1 = hontai1
                .hontai_date1 = hontaiDate1.Trim
                If hidAmount1.Value = "" Then
                    If txtAmount1.Text.Trim = "" Then
                        .hontai_amount1 = 0
                    Else
                        .hontai_amount1 = txtAmount1.Text.Trim
                    End If
                Else
                    .hontai_amount1 = hidAmount1.Value
                    txtAmount1.Text = hidAmount1.Value
                End If

                .hontai_chk2 = hontai2
                .hontai_date2 = hontaiDate2.Trim

                If hidAmount2.Value = "" Then
                    If txtAmount2.Text.Trim = "" Then
                        .hontai_amount2 = 0
                    Else
                        .hontai_amount2 = txtAmount2.Text.Trim
                    End If
                Else
                    .hontai_amount2 = hidAmount2.Value
                    txtAmount2.Text = hidAmount2.Value
                End If

                .hontai_chk3 = hontai3
                .hontai_date3 = hontaiDate3.Trim
                '.hontai_amount3 = txtAmount3.Text.Trim

                If hidAmount3.Value = "" Then
                    If txtAmount3.Text.Trim = "" Then
                        .hontai_amount3 = 0
                    Else
                        .hontai_amount3 = txtAmount3.Text.Trim
                    End If
                Else
                    .hontai_amount3 = hidAmount3.Value
                    txtAmount3.Text = hidAmount3.Value
                End If

                'Modify 2013/09/19
                'If hidHontaiAmount.Value = "" Then
                '     .hontai_amount = lblHontaiAmount.Text.Trim
                ' Else
                '     .hontai_amount = hidHontaiAmount.Value
                '     lblHontaiAmount.Text = hidHontaiAmount.Value
                ' End If
                If hidHontaiAmount.Value = "" Then
                    If txtHontaiAmount.Text.Trim = "" Then
                        .hontai_amount = 0
                    Else
                        .hontai_amount = txtHontaiAmount.Text.Trim.Replace(",", "")
                    End If
                Else
                    .hontai_amount = hidHontaiAmount.Value
                    txtHontaiAmount.Text = hidHontaiAmount.Value
                End If
                ' Start Edit by Wasan D. 24-10-2013
                ' If hidTotalAmount.Value = "" Then
                '    If lblTotalAmount.Text.Trim = "" Then
                '        .total_amount = 0
                '    Else
                '        .total_amount = lblTotalAmount.Text.Trim
                '    End If
                'Else
                '    .total_amount = hidTotalAmount.Value
                '    lblTotalAmount.Text = hidTotalAmount.Value
                'End If

                'If .total_amount = 0 Then
                '    .total_amount = .hontai_amount
                'End If
                If lblTotalAmount.Text.Trim = "" Then
                    .total_amount = 0.0
                Else
                    .total_amount = CDec(hidPOTotalAmount.Value) + CDec(hidHTAmount.Value)
                End If
                ' End Edit
                'Case hontai = 0
                If CInt(.hontai_amount) = 0 Then
                    .hontai_condition1 = 0
                    .hontai_condition2 = 0
                    .hontai_condition3 = 0
                Else
                    Dim strConditon As String = ddlPaymentCondition.SelectedItem.Text
                    Dim strConditionS = strConditon.Split("%")
                    txtCondition1.Text = strConditionS(0)
                    txtCondition2.Text = strConditionS(1)
                    txtCondition3.Text = strConditionS(2)
                    .hontai_condition1 = txtCondition1.Text.Trim
                    .hontai_condition2 = txtCondition2.Text.Trim
                    .hontai_condition3 = txtCondition3.Text.Trim
                End If
                .remark = txtRemarks.Text.Trim
                .detail = txtDetail.Text.Trim
                .finish_fg = 0
                .status_id = 1
                .finish_date = String.Empty
                .create_at_remark = txtOwnCompany.Text.Trim
                .payment_condition_id = ddlPaymentCondition.SelectedValue
                .payment_condition_remark = txtPaymentCondition.Text.Trim

                'Modify 2013/09/19 
                If hidSumAmount.Value = "" Then
                    .quotation_amount = lblSumOtherAmount.Text.Trim.Replace(",", "").Trim
                Else
                    .quotation_amount = hidSumAmount.Value.Replace(",", "").Trim
                    lblSumOtherAmount.Text = hidSumAmount.Value
                End If
                'If lblSumOtherAmount.Text.Trim = "" Then
                '    .quotation_amount = ""
                'Else
                '    .quotation_amount = lblSumOtherAmount.Text.Replace(",", "").Trim
                'End If
                ' Set job order no
                .job_order = Session("JobOrderNo")
                .job_year = Session("Year")
                .job_month = Session("Month")
                .job_last = Session("JobLast")
                .ip_address = hidIpAddress.Value

            End With

            ' set dto object to session
            Session("objJobOrderDto") = objJobOrderDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListVendor
    '	Discription	    : Load list Vendor function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListVendor()
        Try
            ' object Vendor service
            Dim objVendorSer As New Service.ImpVendorService
            ' listVendorDto for keep value from service
            Dim listVendorDto As New List(Of Dto.VendorDto)
            ' call function GetVendorForList from service
            listVendorDto = objVendorSer.GetVendorListForJobOrder

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlCustomer, listVendorDto, "name", "id", True)

            ' set select Vendor from session
            If Not IsNothing(Session("ddlCustomer")) And ddlCustomer.Items.Count > 0 Then
                ddlCustomer.SelectedValue = Session("ddlCustomer")
            End If

            ' call function for bound data with End user dropdownlist
            objUtility.LoadList(ddlEndUser, listVendorDto, "name", "id", True)

            ' set select Vendor from session
            If Not IsNothing(Session("ddlEndUser")) And ddlEndUser.Items.Count > 0 Then
                ddlEndUser.SelectedValue = Session("ddlEndUser")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListUser
    '	Discription	    : Load list user function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    : Wasan D.
    '	Update Date	    : 25-10-2013
    '*************************************************************/
    Private Sub LoadListUser()
        Try
            ' object user service
            Dim objUserSer As New Service.ImpUserService
            ' listUserDto for keep value from service
            Dim listUserDto As New List(Of Dto.UserDto)
            ' call function GetUserForList from service
            listUserDto = objUserSer.GetUserForList("Sale")

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlPersonCharge, listUserDto, "UserName", "UserID", True)

            ' set select Vendor from session
            If Not IsNothing(Session("ddlPersonCharge")) And ddlPersonCharge.Items.Count > 0 Then
                ddlPersonCharge.SelectedValue = Session("ddlPersonCharge")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListUser", ex.Message.ToString, Session("UserName"))
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
            ' object job order service
            Dim objJobTypeSer As New Service.ImpJobTypeService
            ' listJobTypeDto for keep value from service
            Dim listJobTypeDto As New List(Of Dto.JobTypeDto)
            ' call function GetJobTypeForList from service
            listJobTypeDto = objJobTypeSer.GetJobTypeForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlJobOrder, listJobTypeDto, "name", "id", True)

            ' set select job type from session
            If Not IsNothing(Session("ddlJobOrder")) And ddlJobOrder.Items.Count > 0 Then
                ddlJobOrder.SelectedValue = Session("ddlJobOrder")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListJobType", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListPayCon
    '	Discription	    : Load list pament condition function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListPayCon()
        Try
            ' object payment condition service
            Dim objPayConSer As New Service.ImpPaymentConditionService
            ' listPayConDto for keep value from service
            Dim listPayConDto As New List(Of Dto.PaymentConditionDto)
            ' call function GetPaymentConditionForList from service
            listPayConDto = objPayConSer.GetPaymentConditionForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlPaymentCondition, listPayConDto, "codition_name", "codition_id", True)

            ' set select payment conditiion from session
            If Not IsNothing(Session("ddlPaymentCondition")) And ddlPaymentCondition.Items.Count > 0 Then
                ddlPaymentCondition.SelectedValue = Session("ddlPaymentCondition")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListPayCon", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListPayTerm
    '	Discription	    : Load list pament term function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListPayTerm()
        Try
            ' object payment Term service
            Dim objPayTermSer As New Service.ImpPaymentTermService
            ' listPayTermDto for keep value from service
            Dim listPayTermDto As New List(Of Dto.IPayment_TermDto)
            ' call function GetPaymentConditionForList from service
            listPayTermDto = objPayTermSer.GetPaymentDayForList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlPayment, listPayTermDto, "term_day", "id", True)

            ' set select payment conditiion from session
            If Not IsNothing(Session("ddlPayment")) And ddlPayment.Items.Count > 0 Then
                ddlPayment.SelectedValue = Session("ddlPayment")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListPayCon", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListCurrency
    '	Discription	    : Load list Currency function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    : Suwishaya L.
    '	Update Date	    : 26-09-2013
    '*************************************************************/
    Private Sub LoadListCurrency()
        Try
            ' object Currency service
            Dim objCurrencySer As New Service.ImpCurrencyService
            ' listCurrencyto for keep value from service
            Dim listCurrencyDto As New List(Of Dto.CurrencyDto)
            ' call function GetCurrencyList from service
            listCurrencyDto = objCurrencySer.GetCurrencyForDropdownList

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlCurrency, listCurrencyDto, "name", "id", True)

            'Modify 2013/09/26 (set default "THB")
            ' set select job type from session
            'If Not IsNothing(Session("ddlCurrency")) And ddlCurrency.Items.Count > 0 Then
            '    ddlCurrency.SelectedValue = Session("ddlCurrency")
            'End If
            If Not IsNothing(Session("ddlCurrency")) And ddlCurrency.Items.Count > 0 Then
                ddlCurrency.SelectedValue = Session("ddlCurrency")
            Else
                'Set default "THB"
                ddlCurrency.SelectedValue = 1
            End If
            If ddlCurrency.SelectedItem.Text = "THB" Then
                txtScheduleRate.Text = "1.00000"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListCurrency", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetIpAddress
    '	Discription	    : Get ip address
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetIpAddress()
        Try
            'set ip address
            Session("ipAddress") = Nothing
            Session("ipAddress") = Me.Page.Request.ServerVariables("REMOTE_ADDR") & ""
            hidIpAddress.Value = Me.Page.Request.ServerVariables("REMOTE_ADDR") & ""

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetIpAddress", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTempTable
    '	Discription	    : Delete job order temp
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTempTable()
        Try
            ' check state of delete item
            If objJobOrderSer.DeleteJobOrderTemp(hidIpAddress.Value) Then
                ' case delete success show message box 
                Exit Sub
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearTempTable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTemp
    '	Discription	    : Delete job order temp and file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTemp()
        Try
            'case add new job order
            If Session("Mode") = "Add" Then
                ' check state of delete job order temp table by ip address
                If objJobOrderSer.DeleteJobOrderTemp(hidIpAddress.Value) Then
                    ' case delete success clear file on server
                    ClearFile()
                Else
                    ' case delete not success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_007"))
                End If

            ElseIf Session("Mode") = "Edit" Then 'case edit job order
                ' check state of delete job order temp table by ip address and job order id
                If objJobOrderSer.DeleteJobTemp(Session("job_order_id"), hidIpAddress.Value) Then
                    ' case delete success clear file on server
                    'ClearFile()
                Else
                    ' case delete not success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_007"))
                End If

            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearTemp", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearFile
    '	Discription	    : Delete Job Order file
    '	Return Value	: Integer
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 20-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub ClearFile()
        Try
            Dim strPathPo As String = ""
            Dim strPathQuo As String = ""
            Dim strJobOrder As String = ""
            strJobOrder = lblJobOrder.Text.Trim

            'get PO and Quotation job order folder path
            If Session("Mode") = "Add" Then
                strPathPo = strPathConfigPO & hidIpAddress.Value
                strPathQuo = strPathConfigQuo & hidIpAddress.Value
            ElseIf Session("Mode") = "Edit" Then
                strPathPo = strPathConfigPO & strJobOrder
                strPathQuo = strPathConfigQuo & strJobOrder
            End If

            'check exist folder job order po
            If Directory.Exists(strPathPo) And Not strPathPo.Equals(strPathConfigPO) Then
                'Delete po folder
                Directory.Delete(strPathPo, True)
            End If

            'check exist folder job order Quotation
            If Directory.Exists(strPathQuo) And Not strPathQuo.Equals(strPathConfigQuo) Then
                'Delete Quotation folder
                Directory.Delete(strPathQuo, True)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 07-03-2014
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objJobOrderDto As New Dto.JobOrderDto
            Dim intJobOrderID As Integer = 0
            Dim strIssueDate As String = ""
            Dim strDate1 As String = ""
            Dim strDate2 As String = ""
            Dim strDate3 As String = ""
            Dim dblHotaiAmount As Decimal
            Dim dblTotalAmount As Decimal
            Dim dblScheduleRate As Decimal
            'set Schedule Rate
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intJobOrderID = CInt(objUtility.GetQueryString("id"))
                Session("job_order_id") = intJobOrderID

            Else
                intJobOrderID = Session("job_order_id")
            End If

            ' check state of Load data to temp table
            If objJobOrderSer.InsertJobOrderTemp(intJobOrderID, hidIpAddress.Value) Then
                ' case load data to temp table success  
                ' call function GetJobOrderByID from service
                objJobOrderDto = objJobOrderSer.GetJobOrderByID(intJobOrderID)

                ' assign value to control
                With objJobOrderDto
                    'Set format date to dd/mm/yyyy 
                    If .issue_date <> Nothing Or .issue_date <> "" Then
                        strIssueDate = Right(.issue_date, 2) & "/" & Mid(.issue_date, 5, 2) & "/" & Left(.issue_date, 4)
                    End If

                    If .hontai_date1 <> Nothing Or .hontai_date1 <> "" Then
                        strDate1 = Right(.hontai_date1, 2) & "/" & Mid(.hontai_date1, 5, 2) & "/" & Left(.hontai_date1, 4)
                    End If

                    If .hontai_date2 <> Nothing Or .hontai_date2 <> "" Then
                        strDate2 = Right(.hontai_date2, 2) & "/" & Mid(.hontai_date2, 5, 2) & "/" & Left(.hontai_date2, 4)
                    End If

                    If .hontai_date3 <> Nothing Or .hontai_date3 <> "" Then
                        strDate3 = Right(.hontai_date3, 2) & "/" & Mid(.hontai_date3, 5, 2) & "/" & Left(.hontai_date3, 4)
                    End If

                    lblJobOrder.Text = .job_order
                    txtIssueDate.Text = strIssueDate
                    ddlCustomer.SelectedValue = .customer
                    ddlEndUser.SelectedValue = .end_user
                    rbtReceivePo.SelectedValue = .receive_po
                    'when select Receive Po is NO ,Link Upload PO can't used
                    If rbtReceivePo.SelectedValue = "0" Then
                        lblUploadPO.Visible = True
                        linkUploadPO.Visible = False
                        linkUploadPO.Enabled = False
                    Else
                        lblUploadPO.Visible = False
                        linkUploadPO.Visible = True
                        linkUploadPO.Enabled = True
                    End If

                    ddlPersonCharge.SelectedValue = .person_in_charge
                    ddlJobOrder.SelectedValue = .job_type_id
                    'set enable on create at item  
                    SetCreateAt(ddlJobOrder.SelectedValue)

                    rbtBOI.SelectedValue = .is_boi
                    rbtCreateAt.SelectedValue = .create_at


                    If .job_type_id = 1 Then
                        rbtCreateAt.Enabled = False
                    ElseIf .job_type_id = 4 Then
                        rbtCreateAt.Enabled = False
                    Else
                        rbtCreateAt.Enabled = True
                    End If


                    If rbtCreateAt.SelectedValue = "1" Then
                        txtOwnCompany.Enabled = True
                    Else
                        txtOwnCompany.Enabled = False
                        txtOwnCompany.Text = Nothing
                        'rbtCreateAt.SelectedValue = Nothing

                    End If
                    txtOwnCompany.Text = .create_at_remark
                    txtPartName.Text = .part_name
                    txtPartNo.Text = .part_no
                    rbtPartType.SelectedValue = .part_type

                    If .payment_condition_id = 0 Then
                        ddlPaymentCondition.SelectedValue = Nothing
                    Else
                        ddlPaymentCondition.SelectedValue = .payment_condition_id
                    End If

                    txtPaymentCondition.Text = .payment_condition_remark
                    ddlPayment.SelectedValue = .payment_term_id
                    ddlCurrency.SelectedValue = .currency_id
                    'Modify 2013/09/19
                    'lblHontaiAmount.Text = Format(Convert.ToDouble(.hontai_amount), "#,##0.00")
                    txtHontaiAmount.Text = Format(Convert.ToDouble(.hontai_amount), "#,##0.00")
                    lblHontaiAmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount * dblScheduleRate), "#,##0.00") & " )"
                    hidHontaiAmount.Value = Format(Convert.ToDouble(.hontai_amount), "#,##0.00")
                    hidHTAmount.Value = CDec(.hontai_amount)
                    txtDate1.Text = strDate1
                    txtDate2.Text = strDate2
                    txtDate3.Text = strDate3
                    txtAmount1.Text = Format(Convert.ToDouble(.hontai_amount1), "#,##0.00")
                    txtAmount2.Text = Format(Convert.ToDouble(.hontai_amount2), "#,##0.00")
                    txtAmount3.Text = Format(Convert.ToDouble(.hontai_amount3), "#,##0.00")
                    hidAmount1.Value = Format(Convert.ToDouble(.hontai_amount1), "#,##0.00")
                    hidAmount2.Value = Format(Convert.ToDouble(.hontai_amount2), "#,##0.00")
                    hidAmount3.Value = Format(Convert.ToDouble(.hontai_amount3), "#,##0.00")
                    lblHontai1AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount1 * dblScheduleRate), "#,##0.00") & " )"
                    lblHontai2AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount2 * dblScheduleRate), "#,##0.00") & " )"
                    lblHontai3AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount3 * dblScheduleRate), "#,##0.00") & " )"

                    txtCondition1.Text = .hontai_condition1
                    txtCondition2.Text = .hontai_condition2
                    txtCondition3.Text = .hontai_condition3
                    ChkHontai1.Checked = .hontai_chk1
                    ChkHontai2.Checked = .hontai_chk2
                    ChkHontai3.Checked = .hontai_chk3
                    If CInt(.total_amount) = 0 Then
                        hidSumTotalAmount.Value = 0
                    Else
                        hidSumTotalAmount.Value = 1
                    End If
                    lblTotalAmount.Text = Format(Convert.ToDouble(.total_amount), "#,##0.00")
                    lblTotalAmountThb.Text = "( THB = " & Format(Convert.ToDouble(.total_amount * dblScheduleRate), "#,##0.00") & " )"
                    hidTotalAmount.Value = Format(Convert.ToDouble(.total_amount), "#,##0.00")
                    txtRemarks.Text = .remark
                    txtDetail.Text = .detail
                    If .quotation_amount Is Nothing Or .quotation_amount = "" Then
                        lblSumOtherAmount.Text = ""
                    Else
                        lblSumOtherAmount.Text = Format(Convert.ToDouble(.quotation_amount), "#,##0.00")
                    End If

                    'Modify 2013/09/19
                    'dblHotaiAmount = CDbl(lblHontaiAmount.Text.Trim.Replace(",", ""))
                    'dblTotalAmount = CDbl(lblTotalAmount.Text.Trim.Replace(",", ""))
                    dblHotaiAmount = CDbl(hidHontaiAmount.Value.Trim.Replace(",", ""))
                    dblTotalAmount = CDbl(hidTotalAmount.Value.Trim.Replace(",", ""))
                    'calculate data to Sum Other Amount = Total Amount – Hontai  Amount
                    'lblSumOtherAmount.Text = dblTotalAmount - dblHotaiAmount
                    'lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(lblSumOtherAmount.Text * dblScheduleRate), "#,##0.00") & " )"
                    'lblSumOtherAmount.Text = Format(Convert.ToDouble(lblSumOtherAmount.Text), "#,##0.00")
                    'Dim dblSumOtherAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(lblSumOtherAmount.Text.Trim), 0, lblSumOtherAmount.Text.Trim.Replace(",", "")))
                    'lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblSumOtherAmount * dblScheduleRate), "#,##0.00") & " )"

                    'Modify 2014/01/31 Aey
                    lblSumOtherAmount.Text = Format(Convert.ToDouble(.total_amount - .hontai_amount), "#,##0.00")
                    lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(.total_amount - .hontai_amount), "#,##0.00") & " )"

                    'lblSumOthersAmountThb.Text = dblSumOtherAmount

                    'Add 2013/09/26 (Req No.22)
                    Session("status_id") = .status_id
                    Session("JobOrderNo") = .job_order
                    Session("JobOrderNoOld") = .job_order
                    Session("HontaiAmount") = .hontai_amount
                End With
            Else
                ' case load data to temp not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GenerateJobOrderNo
    '	Discription	    : Generate Job Order No
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GenerateJobOrderNo()
        Try
            'Add 2013/09/26
            If Session("Mode") = "Edit" AndAlso _
                (Not Session("status_id") Is Nothing) AndAlso CInt(Session("status_id")) = 1 Then
                ' use in receipt detail
                'Session("JobOrderNo") = Session("JobOrderNoOld")
                'lblJobOrder.Text = Session("JobOrderNoOld")
                'ChkHontai1.Enabled = True
                'txtDate1.Enabled = True
                'ChkHontai2.Enabled = True
                'txtDate2.Enabled = True
                'ChkHontai3.Enabled = True
                'txtDate3.Enabled = True

            ElseIf Session("Mode") = "Edit" AndAlso _
                Session("JobOrderNoOld") = Session("objJobOrderDto").job_order Then
                ' not update job order no
                ' Enabled Checkbox 
                'ChkHontai1.Enabled = True
                'txtDate1.Enabled = True
                'ChkHontai2.Enabled = True
                'txtDate2.Enabled = True
                'ChkHontai3.Enabled = True
                'txtDate3.Enabled = True

            Else
                ' table object keep value from item service
                Dim dtJobOrderRunning As New DataTable
                Dim strYear As String = ""
                Dim strMonth As String = ""
                Dim runNo As String = ""
                Dim intIssueMonth As Integer
                Dim intIssueYear As Integer

                'case add new jop order
                If txtIssueDate.Text.Trim = "" Then
                    intIssueYear = CInt(DateTime.Now.Year.ToString)
                    intIssueMonth = CInt(DateTime.Now.Month.ToString)
                Else 'case edit job order
                    intIssueYear = Right(txtIssueDate.Text.Trim, 4)
                    intIssueMonth = Mid(txtIssueDate.Text.Trim, 4, 2)
                End If
                'set format year to yy and month to mm 
                strYear = Right("0" & CStr(intIssueYear), 2)
                strMonth = Right("0" & CStr(intIssueMonth), 2)
                ' call function GetJobOrderRunning from ItemService
                dtJobOrderRunning = objJobOrderSer.GetJobOrderRunning(intIssueMonth, intIssueYear)

                'case no data add to new month
                If Not IsNothing(dtJobOrderRunning) AndAlso dtJobOrderRunning.Rows.Count > 0 Then
                    runNo = Right("0" & CInt(dtJobOrderRunning.Rows(0).Item("job_last")) + 1, 2)
                Else
                    runNo = "01"
                End If

                ChkHontai1.Enabled = True
                txtDate1.Enabled = False
                ChkHontai2.Enabled = False
                txtDate2.Enabled = False
                ChkHontai3.Enabled = False
                txtDate3.Enabled = False


                ' set table object to session
                Session("Year") = intIssueYear
                Session("Month") = intIssueMonth
                Session("JobLast") = runNo
                Session("JobOrderNo") = strYear & strMonth & runNo
                'Set job order no to item
                lblJobOrder.Text = Session("JobOrderNo")
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GenerateJobOrderNo", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetSumPoAmount
    '	Discription	    : Get Summary PO Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSumPoAmount()
        Try
            ' table object keep value from item service
            Dim dtJobOrderPoTemp As New DataTable
            Dim dblScheduleRate As Decimal
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            ' call function GetItemList from ItemService
            dtJobOrderPoTemp = objJobOrderSer.GetSumPoAmount(hidIpAddress.Value)
            ' set table object to session
            If Not IsNothing(dtJobOrderPoTemp) AndAlso dtJobOrderPoTemp.Rows.Count > 0 Then
                Session("SumPoAmount") = dtJobOrderPoTemp.Rows(0).Item("sum_po_amount").ToString
            Else
                Session("SumPoAmount") = 0.0
            End If

            ' set session to text
            lblHontaiAmount.Text = Format(Convert.ToDouble(Session("SumPoAmount")), "#,##0.00")
            hidHontaiAmount.Value = Format(Convert.ToDouble(Session("SumPoAmount")), "#,##0.00")
            lblHontaiAmountThb.Text = "( THB = " & Format(Convert.ToDouble(Session("SumPoAmount") * dblScheduleRate), "#,##0.00") & " )"

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSumPoAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetSumQuoAmount
    '	Discription	    : Get Summary Quotation Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSumQuoAmount()
        Try
            ' table object keep value from item service
            Dim dtJobOrderQuoTemp As New DataTable
            Dim dblScheduleRate As Decimal
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            ' call function GetItemList from ItemService
            dtJobOrderQuoTemp = objJobOrderSer.GetSumQuoAmount(hidIpAddress.Value)
            ' set table object to session
            If Not IsNothing(dtJobOrderQuoTemp) AndAlso dtJobOrderQuoTemp.Rows.Count > 0 Then
                Session("SumQuoAmount") = dtJobOrderQuoTemp.Rows(0).Item("sum_quo_amount").ToString
            Else
                Session("SumQuoAmount") = 0.0
            End If

            ' set session to text
            lblSumOtherAmount.Text = Format(Convert.ToDouble(Session("SumQuoAmount")), "#,##0.00")
            hidSumAmount.Value = Format(Convert.ToDouble(Session("SumQuoAmount")), "#,##0.00")
            lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(Session("SumQuoAmount") * dblScheduleRate), "#,##0.00") & " )"

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSumQuoAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetTotalAmount
    '	Discription	    : Get Total Summary PO Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetTotalAmount()
        Try
            ' table object keep value from item service
            Dim dtTotalAmount As New DataTable
            Dim dblScheduleRate As Decimal

            'Set Schedule Rate
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            ' call function GetTotalAmount from ItemService
            dtTotalAmount = objJobOrderSer.GetTotalAmount(hidIpAddress.Value)
            ' set table object to session
            If Not IsNothing(dtTotalAmount) AndAlso dtTotalAmount.Rows.Count > 0 Then
                Session("TotalPoAmount") = CDec(dtTotalAmount.Rows(0).Item("total_po_amount").ToString) + CDec(hidHTAmount.Value)
            Else
                Session("TotalPoAmount") = 0.0 + CDec(hidHTAmount.Value)
            End If

            'If CInt(Session("TotalPoAmount")) = 0 Then
            '    hidSumTotalAmount.Value = "0"
            '    If CInt(hidHontaiAmount.Value) > 0 Then
            '        Session("TotalPoAmount") = hidHontaiAmount.Value
            '    End If
            'Else
            '    hidSumTotalAmount.Value = "1"
            'End If

            ' set session to text
            lblTotalAmount.Text = Format(Convert.ToDouble(Session("TotalPoAmount")), "#,##0.00")
            hidTotalAmount.Value = Format(Convert.ToDouble(Session("TotalPoAmount")), "#,##0.00")
            lblTotalAmountThb.Text = "( THB = " & Format(Convert.ToDouble(Session("TotalPoAmount") * dblScheduleRate), "#,##0.00") & " )"

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetTotalAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetHotaiAmount
    '	Discription	    : calculate hotai Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetHotaiAmount()
        Try
            Dim intCondition1 As Integer
            Dim intCondition2 As Integer
            Dim intCondition3 As Integer
            Dim dblHotaiAmount As Decimal
            Dim dblTotalAmount As Decimal
            'Dim dblSumOtherAmount As Decimal
            Dim dbAmount1 As Decimal
            Dim dbAmount2 As Decimal
            Dim dbAmount3 As Decimal
            Dim dblScheduleRate As Decimal
            'set Schedule Rate
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            'set data to integer
            intCondition1 = CInt(txtCondition1.Text.Trim)
            intCondition2 = CInt(txtCondition2.Text.Trim)
            intCondition3 = CInt(txtCondition3.Text.Trim)
            'Modify 2013/09/19
            'dblHotaiAmount = CDbl(lblHontaiAmount.Text.Trim.Replace(",", ""))
            'dblTotalAmount = CDbl(lblTotalAmount.Text.Trim.Replace(",", ""))
            If hidHontaiAmount.Value = "" And txtHontaiAmount.Text = "" Then
                dblHotaiAmount = 0
            Else
                dblHotaiAmount = CDbl(hidHontaiAmount.Value.Trim.Replace(",", ""))
            End If
            dblTotalAmount = CDbl(hidTotalAmount.Value.Trim.Replace(",", ""))

            'caculate data to hotai amount
            dbAmount1 = (intCondition1 * dblHotaiAmount) / 100
            dbAmount2 = (intCondition2 * dblHotaiAmount) / 100
            dbAmount3 = (intCondition3 * dblHotaiAmount) / 100

            txtAmount1.Text = Format(Convert.ToDouble(dbAmount1), "#,##0.00")
            txtAmount2.Text = Format(Convert.ToDouble(dbAmount2), "#,##0.00")
            txtAmount3.Text = Format(Convert.ToDouble(dbAmount3), "#,##0.00")

            hidAmount1.Value = Format(Convert.ToDouble(dbAmount1), "#,##0.00")
            hidAmount2.Value = Format(Convert.ToDouble(dbAmount2), "#,##0.00")
            hidAmount3.Value = Format(Convert.ToDouble(dbAmount3), "#,##0.00")

            lblHontai1AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dbAmount1 * dblScheduleRate), "#,##0.00") & " )"
            lblHontai2AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dbAmount2 * dblScheduleRate), "#,##0.00") & " )"
            lblHontai3AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dbAmount3 * dblScheduleRate), "#,##0.00") & " )"

            'Delete 2013/09/19
            'calculate data to Sum Other Amount = Total Amount – Hontai  Amount
            'dblSumOtherAmount = dblTotalAmount - dblHotaiAmount
            'lblSumOtherAmount.Text = Format(Convert.ToDouble(dblSumOtherAmount), "#,##0.00")
            'hidSumAmount.Value = Format(Convert.ToDouble(dblSumOtherAmount), "#,##0.00")
            'lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblSumOtherAmount * dblScheduleRate), "#,##0.00") & " )"

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetHotaiAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objJobOrderDto As New Dto.JobOrderDto
            Dim strIssueDate As String = ""
            Dim strDate1 As String = ""
            Dim strDate2 As String = ""
            Dim strDate3 As String = ""
            'Dim dblHotaiAmount As Decimal
            'Dim dblTotalAmount As Decimal
            Dim dblScheduleRate As Decimal
            'set Schedule Rate
            If txtScheduleRate.Text = "" Then
                dblScheduleRate = 0
            Else
                dblScheduleRate = txtScheduleRate.Text
            End If

            ' set value to dto object from session
            objJobOrderDto = Session("objJobOrderDto")

            ' set value to control
            With objJobOrderDto
                lblJobOrder.Text = .job_order
                'set format date to dd/mm/yyyy to text
                If .issue_date.ToString.Trim.Length > 0 Then
                    strIssueDate = Right(.issue_date, 2) & "/" & Mid(.issue_date, 5, 2) & "/" & Left(.issue_date, 4)
                    txtIssueDate.Text = strIssueDate
                Else
                    txtIssueDate.Text = .issue_date
                End If

                ddlCustomer.SelectedValue = .customer
                ddlEndUser.SelectedValue = .end_user
                rbtReceivePo.SelectedValue = .receive_po
                ddlPersonCharge.SelectedValue = .person_in_charge
                ddlJobOrder.SelectedValue = .job_type_id
                rbtBOI.SelectedValue = .is_boi
                rbtCreateAt.SelectedValue = .create_at
                txtOwnCompany.Text = .create_at_remark
                txtPartName.Text = .part_name
                txtPartNo.Text = .part_no
                rbtPartType.SelectedValue = .part_type
                ddlPaymentCondition.SelectedValue = .payment_condition_id
                txtPaymentCondition.Text = .payment_condition_remark
                ddlPayment.SelectedValue = .payment_term_id
                ddlCurrency.SelectedValue = .currency_id
                'Modify 2019/09/19
                'lblHontaiAmount.Text = .hontai_amount
                txtHontaiAmount.Text = Format(.hontai_amount, "#,##0.00")
                hidHontaiAmount.Value = .hontai_amount

                If .hontai_date1.ToString.Trim.Length > 0 Then
                    strDate1 = Right(.hontai_date1, 2) & "/" & Mid(.hontai_date1, 5, 2) & "/" & Left(.hontai_date1, 4)
                    txtDate1.Text = strDate1
                Else
                    txtDate1.Text = .hontai_date1
                End If

                If .hontai_date2.ToString.Trim.Length > 0 Then
                    strDate2 = Right(.hontai_date2, 2) & "/" & Mid(.hontai_date2, 5, 2) & "/" & Left(.hontai_date2, 4)
                    txtDate2.Text = strDate2
                Else
                    txtDate2.Text = .hontai_date2
                End If

                If .hontai_date3.ToString.Trim.Length > 0 Then
                    strDate3 = Right(.hontai_date3, 2) & "/" & Mid(.hontai_date3, 5, 2) & "/" & Left(.hontai_date3, 4)
                    txtDate3.Text = strDate3
                Else
                    txtDate3.Text = .hontai_date3
                End If
                txtAmount1.Text = .hontai_amount1
                txtAmount2.Text = .hontai_amount2
                txtAmount3.Text = .hontai_amount3
                hidAmount1.Value = .hontai_amount1
                hidAmount2.Value = .hontai_amount2
                hidAmount3.Value = .hontai_amount3

                txtCondition1.Text = .hontai_condition1
                txtCondition2.Text = .hontai_condition2
                txtCondition3.Text = .hontai_condition3
                ChkHontai1.Checked = .hontai_chk1
                ChkHontai2.Checked = .hontai_chk2
                ChkHontai3.Checked = .hontai_chk3
                lblTotalAmount.Text = .total_amount
                hidTotalAmount.Value = .total_amount

                'except hontai amout
                If .quotation_amount Is Nothing Or .quotation_amount = "" Then
                    lblSumOtherAmount.Text = ""
                Else
                    lblSumOtherAmount.Text = Format(Convert.ToDouble(.quotation_amount), "#,##0.00")
                End If

                txtRemarks.Text = .remark
                txtDetail.Text = .detail
                'Modify 2013/09/19
                'dblHotaiAmount = CDbl(lblHontaiAmount.Text.Trim.Replace(",", ""))
                'dblTotalAmount = CDbl(lblTotalAmount.Text.Trim.Replace(",", ""))
                'calculate data to Sum Other Amount = Total Amount – Hontai  Amount
                'lblSumOtherAmount.Text = dblTotalAmount - dblHotaiAmount
                'lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(lblSumOtherAmount.Text * dblScheduleRate), "#,##0.00") & " )"
                'lblSumOtherAmount.Text = Format(Convert.ToDouble(lblSumOtherAmount.Text), "#,##0.00")
                Dim dblSumOtherAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(lblSumOtherAmount.Text.Trim), 0, lblSumOtherAmount.Text.Trim.Replace(",", "")))
                lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblSumOtherAmount * dblScheduleRate), "#,##0.00") & " )"
                'lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblSumOtherAmount * dblScheduleRate), "#,##0.00") & " )"

                lblHontaiAmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount * dblScheduleRate), "#,##0.00") & " )"
                lblHontai1AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount1 * dblScheduleRate), "#,##0.00") & " )"
                lblHontai2AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount2 * dblScheduleRate), "#,##0.00") & " )"
                lblHontai3AmountThb.Text = "( THB = " & Format(Convert.ToDouble(.hontai_amount3 * dblScheduleRate), "#,##0.00") & " )"
                lblTotalAmountThb.Text = "( THB = " & Format(Convert.ToDouble(.total_amount * dblScheduleRate), "#,##0.00") & " )"

            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
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
            Dim strIssueYear As Integer
            Dim strYearNow As Integer
            Dim objIsDate As New Common.Validations.Validation

            Dim condition1 As Integer
            Dim condition2 As Integer
            Dim condition3 As Integer
            Dim chkCondition As Integer = 0

            condition1 = txtCondition1.Text
            condition2 = txtCondition2.Text
            condition3 = txtCondition3.Text

            CheckCriteriaInput = False
            'call function SetVisibleLable
            SetVisibleLable()

            'Check format date of field Issue Date 
            If txtIssueDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtIssueDate.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field hontai Date 1st
            If txtDate1.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDate1.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtDate1.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field hontai Date 2nd
            If txtDate2.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDate2.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtDate2.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field hontai Date 3rd
            If txtDate3.Text.Trim <> "" Then
                If objIsDate.IsDate(txtDate3.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtDate3.Focus()
                    Exit Function
                End If
            End If

            'Check Hontai Date 1 > Hontai Date 2
            If txtDate1.Text.Trim <> "" And txtDate2.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtDate1.Text.Trim, txtDate2.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_008"))
                    txtDate1.Focus()
                    Exit Function
                End If
            End If

            'Check Hontai Date 1 > Hontai Date 3
            If txtDate1.Text.Trim <> "" And txtDate3.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtDate1.Text.Trim, txtDate3.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_008"))
                    txtDate1.Focus()
                    Exit Function
                End If
            End If

            'Check Hontai Date 2 > Hontai Date 3
            If txtDate2.Text.Trim <> "" And txtDate3.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtDate2.Text.Trim, txtDate3.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_008"))
                    txtDate2.Focus()
                    Exit Function
                End If
            End If

            'Check year of Issue Date < year of date now
            If Session("Mode") = "Add" Then
                If txtIssueDate.Text.Trim <> "" Then
                    strIssueYear = CInt(Right(txtIssueDate.Text.Trim, 4))
                    strYearNow = CInt(DateTime.Now.Year.ToString)
                    If strYearNow > strIssueYear Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_007"))
                        txtIssueDate.Focus()
                        Exit Function
                    End If
                End If
            End If

            If hidHontaiAmount.Value = "" And txtHontaiAmount.Text = "" Then
                hidHontaiAmount.Value = 0
            End If
            If hidHontaiAmount.Value > 0 Then
                'case condition > 0 then check require on date
                If condition1 > 0 Then
                    lblReqHontai1.Visible = True
                    lblChkReqDate1.Visible = True 
                    If ChkHontai1.Checked = False Then
                        lblReq1.Visible = False
                        ChkHontai1.Focus()
                        chkCondition = 1
                    Else
                        lblReq1.Visible = False
                    End If

                    If txtDate1.Text = "" Then
                        lblDateReq1.Visible = True
                        txtDate1.Focus()
                        chkCondition = 1
                    Else
                        lblDateReq1.Visible = False
                    End If
                End If

                If condition2 > 0 Then
                    lblReqHontai2.Visible = True 
                    lblChkReqDate2.Visible = True 
                    If ChkHontai2.Checked = False Then
                        lblReq2.Visible = False
                        ChkHontai2.Focus()
                        chkCondition = 1
                    Else
                        lblReq2.Visible = False
                    End If
                    If txtDate2.Text = "" Then
                        lblDateReq2.Visible = True
                        txtDate2.Focus()
                        chkCondition = 1
                    Else
                        lblDateReq2.Visible = False
                    End If 
                End If

                If condition3 > 0 Then
                    lblReqHontai3.Visible = True
                    lblChkReqDate3.Visible = True
                    If ChkHontai3.Checked = False Then
                        lblReq3.Visible = False
                        ChkHontai3.Focus()
                        chkCondition = 1
                    Else
                        lblReq3.Visible = False
                    End If
                    If txtDate3.Text = "" Then
                        lblDateReq3.Visible = True
                        txtDate3.Focus()
                        chkCondition = 1
                    Else
                        lblDateReq3.Visible = False
                    End If
                End If
                If chkCondition = 1 Then
                    Exit Function
                End If
            End If

            'Check Require of CreateAt
            If ddlJobOrder.SelectedValue = "2" Or ddlJobOrder.SelectedValue = "3" Then
                If rbtCreateAt.SelectedValue = "" Then
                    lblReqCreateAt.Text = "*Require."
                    rbtCreateAt.Focus()
                    Exit Function
                Else
                    lblReqCreateAt.Text = ""
                End If
            Else
                lblReqCreateAt.Text = ""
            End If

            'Check UPload PO
            If rbtReceivePo.SelectedIndex = "0" Then
                If Session("Mode") = "Add" Then
                    If hidflagPO.Value = "1" Then
                        If txtHontaiAmount.Text = String.Empty Then
                            objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_009"))
                            Exit Function
                        End If
                    ElseIf hidflagPO.Value = String.Empty Then
                        If Not txtHontaiAmount.Text Is Nothing Then
                            objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_010"))
                            Exit Function
                        End If
                    End If
                ElseIf Session("Mode") = "Edit" Then
                    If txtHontaiAmount.Text = String.Empty Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_009"))
                        Exit Function
                    End If
                End If
            End If

            CheckCriteriaInput = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCriteriaInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: InsertJobOrder
    '	Discription	    : Insert Job Order
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertJobOrder()
        Try
            ' call function set value to control
            SetValueToControl()

            'Case add new job order ,Generate new job order no
            GenerateJobOrderNo()

            ' call function InsertJobOrder from service and alert message
            If objJobOrderSer.InsertJobOrder(Session("Year"), _
                                            Session("Month"), _
                                            Session("JobLast"), _
                                             lblJobOrder.Text, _
                                             Session("objJobOrderDto"), _
                                             strMsg) Then
                ' Update Sale Invoice 
                UpdateSaleInvoice()

                'Set message for show job order no
                Dim strMsgJobOrder As String = objMessage.GetXMLMessage("KTJB_02_002") & Space(1) & "Job Order No : " & lblJobOrder.Text
                objMessage.AlertMessage(strMsgJobOrder, Nothing, "KTJB01.aspx?New=False&Flag=Ins&UserID=" & Session("UserID"))
            Else
                'Clear data on temp table by ip address 
                ClearTempTable()
                'clear file on server
                ClearFile()
                'alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_003"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateJobOrder
    '	Discription	    : Update Job Order
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateJobOrder()
        Try
            ' call function set value to control
            SetValueToControl()

            'Case add new job order ,Generate new job order no
            GenerateJobOrderNo()

            ' call function UpdateJobOrder from service and alert message
            Dim strYear As String = Session("Year")
            Dim strMonth As String = Session("Month")
            ' check use in receive detail
            If Session("Mode") = "Edit" AndAlso (Not Session("status_id") Is Nothing) AndAlso CInt(Session("status_id")) = 1 Then
                strYear = String.Empty
                strMonth = String.Empty
            ElseIf Session("Mode") = "Edit" AndAlso _
                Session("JobOrderNoOld") = Session("objJobOrderDto").job_order Then
                ' not update job order no
                strYear = String.Empty
                strMonth = String.Empty
            End If
            If objJobOrderSer.UpdateJobOrder(strYear, _
                                            strMonth, _
                                            Session("JobLast"), _
                                            lblJobOrder.Text, _
                                            Session("objJobOrderDto"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_005"), Nothing, "KTJB01.aspx?New=False&Flag=Ins&UserID=" & Session("UserID"))
            Else
                'calculate schedule rate
                GetScheduleRate()
                'Clear data on temp table by ip address and job order id
                ClearTemp()
                'clear file on server
                ClearFile()
                'alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCondition
    '	Discription	    : Check condition for check require
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckCondition()
        Try
            Dim condition1 As String
            Dim condition2 As String
            Dim condition3 As String

            condition1 = txtCondition1.Text
            condition2 = txtCondition2.Text
            condition3 = txtCondition3.Text

            'case condition is 0 then not check require on date
            If condition1 = "0" Then
                lblChkReqDate1.Visible = False
                lblReqHontai1.Visible = False
                lblReq1.Visible = False
                lblDateReq1.Visible = False
            Else
                lblChkReqDate1.Visible = True
                lblReqHontai1.Visible = True
                lblDateReq1.Visible = True
                lblReq1.Visible = True
            End If

            If condition2 = "0" Then
                lblChkReqDate2.Visible = False 
                lblReqHontai2.Visible = False 
                lblDateReq2.Visible = False
                lblReq2.Visible = False
            Else
                lblChkReqDate2.Visible = True 
                lblReqHontai2.Visible = True 
                lblDateReq2.Visible = True
                lblReq2.Visible = True
            End If

            If condition3 = "0" Then
                lblChkReqDate3.Visible = False
                lblReqHontai3.Visible = False
                lblDateReq3.Visible = False
                lblReq3.Visible = False
            Else
                lblChkReqDate3.Visible = True
                lblReqHontai3.Visible = True
                lblDateReq3.Visible = True
                lblReq3.Visible = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCondition", ex.Message.ToString, Session("UserName"))
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
            Dim intChkSchedul As Decimal
            Dim objScheculeRateSer As New Service.ImpScheculeRateService
            'set format date to yyymmdd
            Dim issueDate As String = ""
            'Replace date dd/mm/yyyy to array
            Dim arrIssueDate() As String = Split(txtIssueDate.Text.Trim(), "/")
            'set issue date to yyyymmdd format
            If UBound(arrIssueDate) > 0 Then
                issueDate = arrIssueDate(2) & arrIssueDate(1) & arrIssueDate(0)
            End If

            'call GetScheculeRate from ScheculeRate Service 
            intScheculeRate = objScheculeRateSer.GetScheculeRate(ddlCurrency.SelectedValue, issueDate)
            intChkSchedul = intScheculeRate
            If intScheculeRate <= 0 Then
                intScheculeRate = 1
            End If

            'set data to ScheduleRate item
            txtScheduleRate.Text = Format(Convert.ToDouble(intScheculeRate), "#,##0.00000")

            'Set Amount THB
            'Modify 2013/09/19
            'Dim dblHotaiAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(lblHontaiAmount.Text.Trim), 0, lblHontaiAmount.Text.Trim.Replace(",", "")))
            Dim dblHotaiAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(txtHontaiAmount.Text.Trim), 0, txtHontaiAmount.Text.Trim.Replace(",", "")))
            Dim dblTotalAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(lblTotalAmount.Text.Trim), 0, lblTotalAmount.Text.Trim.Replace(",", "")))
            Dim dblSumOtherAmount As Decimal = CDbl(IIf(String.IsNullOrEmpty(lblSumOtherAmount.Text.Trim), 0, lblSumOtherAmount.Text.Trim.Replace(",", "")))
            Dim dblAmount1 As Decimal = CDbl(IIf(String.IsNullOrEmpty(txtAmount1.Text.Trim), 0, txtAmount1.Text.Trim.Replace(",", "")))
            Dim dblAmount2 As Decimal = CDbl(IIf(String.IsNullOrEmpty(txtAmount2.Text.Trim), 0, txtAmount2.Text.Trim.Replace(",", "")))
            Dim dblAmount3 As Decimal = CDbl(IIf(String.IsNullOrEmpty(txtAmount3.Text.Trim), 0, txtAmount3.Text.Trim.Replace(",", "")))

            lblHontaiAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblHotaiAmount * intScheculeRate), "#,##0.00") & " )"
            lblHontai1AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblAmount1 * intScheculeRate), "#,##0.00") & " )"
            lblHontai2AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblAmount2 * intScheculeRate), "#,##0.00") & " )"
            lblHontai3AmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblAmount3 * intScheculeRate), "#,##0.00") & " )"
            lblTotalAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblTotalAmount * intScheculeRate), "#,##0.00") & " )"
            lblSumOthersAmountThb.Text = "( THB = " & Format(Convert.ToDouble(dblSumOtherAmount * intScheculeRate), "#,##0.00") & " )"

            'check Schecule Rate = 0
            If intChkSchedul <= 0 Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetScheduleRate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUseInJobOrderPO
    '	Discription	    : Check job order use in job order po
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUseInJobOrderPO()
        Try
            Session("boolInuse") = False
            'call function IsUsedInJobOrderPo from jobOrderService
            Dim boolInuse As Boolean = objJobOrderSer.IsUsedInJobOrderPo(Session("job_order_id"))
            Session("boolInuse") = boolInuse
            'set upload PO/quotation item can use or not
            If Session("boolInuse") Then
                lblUploadPO.Visible = True
                lblUploadQuo.Visible = True
                linkUploadPO.Visible = False
                linkUploadQuotation.Visible = False
                txtHontaiAmount.Focus()
            Else
                lblUploadPO.Visible = False
                lblUploadQuo.Visible = False
                linkUploadPO.Visible = True
                linkUploadQuotation.Visible = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckUseInJobOrderPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetVisibleValidate
    '	Discription	    : set Visible for check require
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetVisibleValidate(ByVal intChk As Integer)
        Try

            'set visible item
            If intChk <= 0 Then
                'Hontai 1
                lblChkReqDate1.Visible = False
                lblReqHontai1.Visible = False
                lblReq1.Visible = False
                lblDateReq1.Visible = False

                'Hontai 2
                lblChkReqDate2.Visible = False
                lblReqHontai2.Visible = False
                lblReq2.Visible = False
                lblDateReq2.Visible = False

                'Hontai 3
                lblChkReqDate3.Visible = False
                lblReqHontai3.Visible = False
                lblReq3.Visible = False
                lblDateReq3.Visible = False

            Else
                'Hontai 1
                lblChkReqDate1.Visible = True
                lblReqHontai1.Visible = True
                lblReq1.Visible = True
                lblDateReq1.Visible = True

                'Hontai 2
                lblChkReqDate2.Visible = True
                lblReqHontai2.Visible = True
                lblReq2.Visible = True
                lblDateReq2.Visible = True

                'Hontai 3
                lblChkReqDate3.Visible = True
                lblReqHontai3.Visible = True
                lblReq3.Visible = True
                lblDateReq3.Visible = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetVisibleValidate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetVisibleLable
    '	Discription	    : set Visible for check require
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetVisibleLable()
        Try
            'Set visible to item check req
            lblReqHontai1.Visible = False
            lblChkReqDate1.Visible = False
            lblReqHontai2.Visible = False
            lblChkReqDate2.Visible = False
            lblReqHontai3.Visible = False
            lblChkReqDate3.Visible = False
            lblReq1.Visible = False
            lblDateReq1.Visible = False
            lblReq2.Visible = False
            lblDateReq2.Visible = False
            lblReq3.Visible = False
            lblDateReq3.Visible = False


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetVisibleLable", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetCreateAt
    '	Discription	    : set enable for create At
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 17-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetCreateAt(ByVal strJobType As String)
        Try
            'Case job order type is Modification or  Repair
            If strJobType = "2" Or strJobType = "3" Or strJobType = "5" Then
                lblChkReq.Visible = True
                rbtCreateAt.Enabled = True
            Else
                If strJobType = "1" Or strJobType = "4" Then ' New Mold or Insert
                    rbtCreateAt.Enabled = False
                    txtOwnCompany.Text = String.Empty
                    txtOwnCompany.Enabled = False

                    If Session("Mode") = "Add" Then
                        rbtCreateAt.SelectedValue = -1
                    End If

                Else
                    rbtCreateAt.Enabled = True
                    txtOwnCompany.Text = String.Empty

                    If Session("Mode") = "Add" Then
                        rbtCreateAt.SelectedValue = -1
                    End If

                End If
                lblChkReq.Visible = False
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetCreateAt", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************/
    '	Function name	: ClearTextBox
    '	Discription	    : Clear Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 28-01-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Private Sub ClearTextBox(ByVal root As Control)

        For Each cntrl As Control In root.Controls
            ClearTextBox(cntrl)

            If TypeOf cntrl Is TextBox Then
                CType(cntrl, TextBox).Text = String.Empty
            End If
        Next cntrl

    End Sub


    '/**************************************************************/
    '	Function name	: ClearTextBox
    '	Discription	    : Clear Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-02-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Protected Sub txtHontaiAmount_Changed( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles txtHontaiAmount.TextChanged
        Try
            'Modify Check Return Value PO By Aey
            If rbtReceivePo.SelectedValue = "1" Then
                'If hidPOTotalAmount.Value <> "" Then
                If txtHontaiAmount.Text = "" Then
                    'objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_009"))
                    MsgBox("Please Insert Hontal Amount")
                    txtHontaiAmount.Focus()
                End If
                'End If
            End If
        Catch ex As Exception
            objLog.ErrorLog("txtHontaiAmount_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************/
    '	Function name	: setEnabledChk
    '	Discription	    : Set Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-02-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Protected Sub ChkHontai_Checked( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ChkHontai1.CheckedChanged

        Try
            Dim Hontai As String = hidHontaiAmount.Value
            Dim Amount1 As String = hidAmount1.Value
            Dim Amount2 As String = hidAmount2.Value
            Dim Amount3 As String = hidAmount3.Value

            If ChkHontai1.Checked = True And Session("PaymentCodition1") <> "0" Then
                txtHontaiAmount.Text = Hontai
                txtAmount1.Text = Amount1
                txtAmount2.Text = Amount2
                txtAmount3.Text = Amount3

                txtDate1.Enabled = True

                If Session("PaymentCodition2") <> "0" Then
                    ChkHontai2.Enabled = True
                    txtDate2.Enabled = False
                    ChkHontai3.Enabled = False
                    txtDate3.Enabled = False

                End If
                
            End If

            Session("txtAmount1") = txtAmount1.Text
            Session("txtAmount2") = txtAmount2.Text
            Session("txtAmount3") = txtAmount3.Text
            Session("hidDate1") = hidDate1.Value
            Session("hidDate2") = hidDate2.Value
            Session("hidDate3") = hidDate3.Value

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setEnabledChk", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************/
    '	Function name	: setEnabledChk
    '	Discription	    : Set Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-02-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Protected Sub ChkHontai2_Checked( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ChkHontai2.CheckedChanged
        Try
            Dim Hontai As String = hidHontaiAmount.Value
            Dim Amount1 As String = hidAmount1.Value
            Dim Amount2 As String = hidAmount2.Value
            Dim Amount3 As String = hidAmount3.Value

            If ChkHontai2.Checked = True And Session("PaymentCodition2") <> "0" Then

                txtHontaiAmount.Text = Hontai
                txtAmount1.Text = Amount1
                txtAmount2.Text = Amount2
                txtAmount3.Text = Amount3

                txtDate1.Enabled = True
                ChkHontai2.Enabled = True
                txtDate2.Enabled = True

                If Session("PaymentCodition3") <> "0" Then
                    ChkHontai3.Enabled = True
                    txtDate3.Enabled = False

                    If hidDate2.ToString <> "" Then
                        txtDate2.Text = hidDate2.Value
                    End If

                End If

            End If

            Session("txtAmount1") = txtAmount1.Text
            Session("txtAmount2") = txtAmount2.Text
            Session("txtAmount3") = txtAmount3.Text
            Session("hidDate1") = hidDate1.Value
            Session("hidDate2") = hidDate2.Value
            Session("hidDate3") = hidDate3.Value

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setEnabledChk", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************/
    '	Function name	: setEnabledChk
    '	Discription	    : Set Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-02-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Protected Sub ChkHontai3_Checked( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ChkHontai3.CheckedChanged
        Try

            Dim Hontai As String = hidHontaiAmount.Value
            Dim Amount1 As String = hidAmount1.Value
            Dim Amount2 As String = hidAmount2.Value
            Dim Amount3 As String = hidAmount3.Value

            If ChkHontai3.Checked = True And Session("PaymentCodition3") <> "0" Then

                txtHontaiAmount.Text = Hontai
                txtAmount1.Text = Amount1
                txtAmount2.Text = Amount2
                txtAmount3.Text = Amount3

                txtDate1.Enabled = True
                ChkHontai2.Enabled = True
                txtDate2.Enabled = True
                ChkHontai3.Enabled = True
                txtDate3.Enabled = True

                If hidDate3.ToString <> "" Then
                    txtDate3.Text = hidDate3.Value
                End If
            End If

            Session("txtAmount1") = txtAmount1.Text
            Session("txtAmount2") = txtAmount2.Text
            Session("txtAmount3") = txtAmount3.Text
            Session("hidDate1") = hidDate1.Value
            Session("hidDate2") = hidDate2.Value
            Session("hidDate3") = hidDate3.Value

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("setEnabledChk", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************/
    '	Function name	: ddlCustomer
    '	Discription	    : Set Value 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-02-2014
    '	Update User	    :
    '	Update Date	    :
    '/**************************************************************/

    Protected Sub ddlCustomer_SelectChange( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles ddlCustomer.SelectedIndexChanged
        Try
            If ddlCustomer.SelectedValue Then
                ddlEndUser.SelectedValue = ddlCustomer.SelectedValue
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ddlCustomer_SelectChange", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: SelectIntIdJobPO
    '	Discription	    : Select New Job Order Po id   
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-05-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub SelectIntIdJobPO()
        Try
            Dim tbIntIdJBPO As DataTable
            Dim objIDSaleInv As Dao.ISale_InvoiceDao

            tbIntIdJBPO = objJobOrderSer.GetJobOrderPOList(Session("job_order_id"))
            objIDSaleInv = objSaleInvoiceSer.GetSaleInvoiceforUpdate(Session("job_order_id"))

            Session("tbIntIdJBPO") = tbIntIdJBPO
            Session("objIDSaleInv") = objIDSaleInv

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SelectIntIdJobPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: UpdateSaleInvoice
    '	Discription	    : Update Sale Invoice 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-05-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub UpdateSaleInvoice()
        Try
            SelectIntIdJobPO()

            Dim IntUpDateSaleInv As Integer

            IntUpDateSaleInv = objSaleInvoiceSer.UpdateReciveDetail(Session("tbIntIdJBPO"), Session("objIDSaleInv"), Session("job_order_id"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateSaleInvoice(KTJB02)", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
