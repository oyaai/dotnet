Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Sale Invoice
'	Class Name		    : JobOrder_KTJB06
'	Class Discription	: Webpage for maintenance Sale Invoice
'	Create User 		: Suwishaya L.
'	Create Date		    : 26-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB06
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objSaleInvoiceSer As New Service.ImpSale_InvoiceService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private Const strConfirmDel As String = "ConfirmDel"
    Private Const strConfirmSel As String = "ConfirmSel"
    Private strMsg As String = String.Empty

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB06 : Sale Invoice")

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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            'check postback page
            If Not IsPostBack Then
                'set session 
                SetSession()
                ' case not post back then call function initialpage
                InitialPage()
            End If

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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            If Session("dtDeleteFlag") Is Nothing Then
                Response.Redirect("KTJB05.aspx?New=False")
            Else
                'Update flag on job_order_po
                If UpdateFlagJobOrderPO() Then
                    Session("dtDeleteFlag") = Nothing
                    Response.Redirect("KTJB05.aspx?New=False")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_019"))
                End If
            End If
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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            'Update flag on job_order_po
            If Session("dtDeleteFlag") Is Nothing Then
                ' call function ClearControl
                ClearControl()
            Else
                If UpdateFlagJobOrderPO() Then
                    Session("dtDeleteFlag") = Nothing
                    ' call function ClearControl
                    ClearControl()
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_020"))
                End If
            End If

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
    '	Create Date	    : 31-07-2013
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
                Exit Sub
            End If
            'call function CheckHontaiData
            If CheckHontaiData() = False Then
                Exit Sub
            End If
            'call function CheckHontaiData
            If CheckBankFee() > 1 Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_022"))
                Exit Sub
            End If

            ' call function set session dto
            SetValueToDto()
            'Set data to datatable 
            InsertDataTable()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTJB06", strConfirmIns, objMessage.GetXMLMessage("KTJB_06_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTJB06", strConfirmUpd, objMessage.GetXMLMessage("KTJB_06_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            ' call function set session dto
            SetValueToDto()
            'Set data to datatable 
            InsertDataTable()

            'Cehck exist job_order in job-order table
            If objSaleInvoiceSer.IsUsedInJobOrder(txtJobOrder.Text.Trim) = False Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_008"))
                Exit Sub
            End If
            'Check new customer <> old customer 
            If objSaleInvoiceSer.IsCustomerUsedInJobOrder(txtJobOrder.Text.Trim, ddlCustomer.SelectedValue) = False Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_010"))
                Exit Sub
            End If
            Session("txtJobOrder") = txtJobOrder.Text.Trim
            Session("txtExchangeRate") = txtExchangeRate.Text.Trim
            'call function SearchSaleInvoiceDetail
            SearchSaleInvoiceDetail()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSelect_Click
    '	Discription	    : Event btnSelect is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSelect_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSelect.Click
        Try
            ' call function set session dto
            SetValueToDto()
            'Set data to datatable 
            InsertDataTable()

            'Check duplicate data
            If CheckDuplicateData() = False Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_017"))
                Exit Sub
            End If

            'Keep data of each record that is already checked
            SetDataTable()

            If Session("intChkSelect") = 0 Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_018"))
                Exit Sub
            End If

            objMessage.ConfirmMessage("KTJB06", strConfirmSel, objMessage.GetXMLMessage("KTJB_06_009"))


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSelect_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailFirst_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailFirst_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptDetailFirst.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashVatID.Clear()
            hashWtID.Clear()
            hashRemark.Clear()
            hashHontaiFlg.Clear()
            hashJobPOId.Clear()
            hashJobOrderId.Clear()
            hashHontaiType.Clear()
            hashPoTypeFirst.Clear()
            hashJobPoIdFirst.Clear()
            hashJobOrderIdFirst.Clear()
            hashHontaiTypeFirst.Clear()
            hashHeaderAmount.Clear()
            hashDetailAmount.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailFirst_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailFirst_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailFirst_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptDetailFirst.ItemDataBound
        Try
            ' Set data to hashtable
            hashHontaiFlg.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "hontai_fg"))
            hashPoTypeFirst.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "po_type"))
            hashHontaiTypeFirst.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "hontai_type"))
            hashJobPoIdFirst.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashJobOrderIdFirst.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order_id"))
            hashHeaderAmount.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "amount"))

            ' variable for keep data from hashtable
            Dim strHontaiFlg As String = hashHontaiFlg(e.Item.ItemIndex).ToString()
            Dim lblHeaderAmount As New Label
            lblHeaderAmount = DirectCast(e.Item.FindControl("lblHeaderAmount"), Label)

            ' set permission on amount           
            If Not Session("actAmount") Then
                lblHeaderAmount.Text = "******"
            Else
                lblHeaderAmount.Text = hashHeaderAmount(e.Item.ItemIndex).ToString()
            End If

            ' object on repaeter
            Dim chkApprove As HtmlInputCheckBox
            chkApprove = e.Item.FindControl("chkApprove")

            'ถ้า Hontai งวดไหนถูกสร้าง Sale invoice แล้วให้ Disable CheckBox Hontai ของงวดนั้น  
            If strHontaiFlg = "1" Then
                chkApprove.Disabled = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailFirst_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailFirst_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailFirst_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptDetailFirst.ItemCommand
        Try

            ' set data to session
            Session("po_type_first") = hashPoTypeFirst(e.Item.ItemIndex).ToString()
            Session("job_order_po_id_first") = hashJobPoIdFirst(e.Item.ItemIndex).ToString()
            Session("job_order_id_first") = hashJobOrderIdFirst(e.Item.ItemIndex).ToString()
            Session("hontai_type_first") = hashHontaiTypeFirst(e.Item.ItemIndex).ToString()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailFirst_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailSecond_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailSecond_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptDetailSecond.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashVatID.Clear()
            hashWtID.Clear()
            hashRemark.Clear()
            hashHontaiFlg.Clear()
            hashJobPOId.Clear()
            hashJobOrderId.Clear()
            hashHontaiType.Clear()
            hashPoTypeFirst.Clear()
            hashJobPoIdFirst.Clear()
            hashJobOrderIdFirst.Clear()
            hashHontaiTypeFirst.Clear()
            hashHeaderAmount.Clear()
            hashDetailAmount.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailSecond_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailSecond_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailSecond_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptDetailSecond.ItemCommand
        Try
            '' variable for keep data from hashtable
            Dim intID As Integer
            If hashID(e.Item.ItemIndex).ToString() = "" Then
                intID = 0
            Else
                intID = CInt(hashID(e.Item.ItemIndex).ToString())
            End If

            ' set data to session
            Session("intID") = intID
            Session("job_order_po_id") = hashJobPOId(e.Item.ItemIndex).ToString()
            Session("job_order_id") = hashJobOrderId(e.Item.ItemIndex).ToString()
            Session("hontai_type") = hashHontaiType(e.Item.ItemIndex).ToString()

            Select Case e.CommandName
                Case "Delete"
                    ' call function set session dto
                    SetValueToDto()
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB06", strConfirmDel, objMessage.GetXMLMessage("KTJB_06_011"))

            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailSecond_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptDetailSecond_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptDetailSecond_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptDetailSecond.ItemDataBound
        Try
            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashVatID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "vat_id"))
            hashWtID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "wt_id"))
            hashJobPOId.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order_po_id"))
            hashJobOrderId.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order_id"))
            hashHontaiType.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "hontai_type"))
            hashDetailAmount.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "amount_thb"))

            ' variable for keep data from hashtable
            Dim intVatID As Integer
            Dim intWtID As Integer
            Dim intID As Integer
            If Not hashID(e.Item.ItemIndex).ToString() Is Nothing And hashID(e.Item.ItemIndex).ToString() <> "" Then
                intID = CInt(hashID(e.Item.ItemIndex).ToString())
            End If
            If Not hashVatID(e.Item.ItemIndex).ToString() Is Nothing And hashVatID(e.Item.ItemIndex).ToString() <> "" Then
                intVatID = CInt(hashVatID(e.Item.ItemIndex).ToString())
            End If
            If Not hashWtID(e.Item.ItemIndex).ToString() Is Nothing And hashWtID(e.Item.ItemIndex).ToString() <> "" Then
                intWtID = CInt(hashWtID(e.Item.ItemIndex).ToString())
            End If

            ' object on repaeter
            Dim btnDel As New LinkButton
            Dim ddlVat As New DropDownList
            Dim ddlWt As New DropDownList
            Dim txtRemark As New TextBox
            Dim txtBankfee As New TextBox
            Dim lblDetailAmount As New Label

            ddlVat = DirectCast(e.Item.FindControl("ddlVat"), DropDownList)
            ddlWt = DirectCast(e.Item.FindControl("ddlWt"), DropDownList)
            txtRemark = DirectCast(e.Item.FindControl("txtRemark"), TextBox)
            txtBankfee = DirectCast(e.Item.FindControl("txtBankfee"), TextBox)
            lblDetailAmount = DirectCast(e.Item.FindControl("lblDetailAmount"), Label)

            'cal function get vat to dropdrownlist
            LoadListVat(intVatID, ddlVat)
            LoadListWT(intWtID, ddlWt)

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            ' set permission on amount           
            If Not Session("actAmount") Then
                lblDetailAmount.Text = "******"
            Else
                lblDetailAmount.Text = hashDetailAmount(e.Item.ItemIndex).ToString()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptDetailSecond_ItemDataBound", ex.Message.ToString, Session("UserName"))
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

    ' Stores the id keys in ViewState
    ReadOnly Property hashVatID() As Hashtable
        Get
            If IsNothing(ViewState("hashVatID")) Then
                ViewState("hashVatID") = New Hashtable()
            End If
            Return CType(ViewState("hashVatID"), Hashtable)
        End Get
    End Property

    ' Stores the id keys in ViewState
    ReadOnly Property hashWtID() As Hashtable
        Get
            If IsNothing(ViewState("hashWtID")) Then
                ViewState("hashWtID") = New Hashtable()
            End If
            Return CType(ViewState("hashWtID"), Hashtable)
        End Get
    End Property

    ' Stores the id keys in ViewState
    ReadOnly Property hashRemark() As Hashtable
        Get
            If IsNothing(ViewState("hashRemark")) Then
                ViewState("hashRemark") = New Hashtable()
            End If
            Return CType(ViewState("hashRemark"), Hashtable)
        End Get
    End Property

    ' Stores the id keys in ViewState
    ReadOnly Property hashHontaiFlg() As Hashtable
        Get
            If IsNothing(ViewState("hashHontaiFlg")) Then
                ViewState("hashHontaiFlg") = New Hashtable()
            End If
            Return CType(ViewState("hashHontaiFlg"), Hashtable)
        End Get
    End Property

    ' Stores the job_order_po_id in ViewState
    ReadOnly Property hashJobPOId() As Hashtable
        Get
            If IsNothing(ViewState("hashJobPOId")) Then
                ViewState("hashJobPOId") = New Hashtable()
            End If
            Return CType(ViewState("hashJobPOId"), Hashtable)
        End Get
    End Property

    ' Stores the job_order_id in ViewState
    ReadOnly Property hashJobOrderId() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrderId")) Then
                ViewState("hashJobOrderId") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrderId"), Hashtable)
        End Get
    End Property

    ' Stores the hontai_type in ViewState
    ReadOnly Property hashHontaiType() As Hashtable
        Get
            If IsNothing(ViewState("hashHontaiType")) Then
                ViewState("hashHontaiType") = New Hashtable()
            End If
            Return CType(ViewState("hashHontaiType"), Hashtable)
        End Get
    End Property

    ' Stores the po_type in ViewState
    ReadOnly Property hashPoTypeFirst() As Hashtable
        Get
            If IsNothing(ViewState("hashPoTypeFirst")) Then
                ViewState("hashPoTypeFirst") = New Hashtable()
            End If
            Return CType(ViewState("hashPoTypeFirst"), Hashtable)
        End Get
    End Property

    ' Stores the hontai_type in ViewState
    ReadOnly Property hashHontaiTypeFirst() As Hashtable
        Get
            If IsNothing(ViewState("hashHontaiTypeFirst")) Then
                ViewState("hashHontaiTypeFirst") = New Hashtable()
            End If
            Return CType(ViewState("hashHontaiTypeFirst"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashJobPoIdFirst() As Hashtable
        Get
            If IsNothing(ViewState("hashJobPoIdFirst")) Then
                ViewState("hashJobPoIdFirst") = New Hashtable()
            End If
            Return CType(ViewState("hashJobPoIdFirst"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashJobOrderIdFirst() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrderIdFirst")) Then
                ViewState("hashJobOrderIdFirst") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrderIdFirst"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashDetailAmount() As Hashtable
        Get
            If IsNothing(ViewState("hashDetailAmount")) Then
                ViewState("hashDetailAmount") = New Hashtable()
            End If
            Return CType(ViewState("hashDetailAmount"), Hashtable)
        End Get
    End Property

    ReadOnly Property hashHeaderAmount() As Hashtable
        Get
            If IsNothing(ViewState("hashHeaderAmount")) Then
                ViewState("hashHeaderAmount") = New Hashtable()
            End If
            Return CType(ViewState("hashHeaderAmount"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' call function check permission
            CheckPermission()

            'Set data to drowndrown list
            SetDataDropdrownList()

            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                SetValueToControl()
                'Insert sale invoice
                InsertSaleInvoice()
                lblInvoiceAmount.Text = Format(Convert.ToDouble(Session("decSaleInvoiceAmount")), "#,##0.00")
                DisplayPageFirst(Request.QueryString("PageNo"))
                DisplayPageSecond(Request.QueryString("PageNo"))
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                SetValueToControl()
                'Update sale invoice
                UpdateSaleInvoice()
                lblInvoiceAmount.Text = Format(Convert.ToDouble(Session("decSaleInvoiceAmount")), "#,##0.00")
                DisplayPageFirst(Request.QueryString("PageNo"))
                DisplayPageSecond(Request.QueryString("PageNo"))
                Exit Sub
            End If


            ' check delete item
            If objUtility.GetQueryString(strConfirmDel) = "True" Then
                SetValueToControl()
                DeleteSaleInvoice()
                'GetDataDetailSecond()
                lblInvoiceAmount.Text = Format(Convert.ToDouble(Session("decSaleInvoiceAmount")), "#,##0.00")

                Dim pageNo As String = Request.QueryString("PageNo")

                objLog.InfoLog("PageNo", pageNo.ToString)

                DisplayPageFirst(Request.QueryString("PageNo"))
                DisplayPageSecond(Request.QueryString("PageNo"))
                Exit Sub
            End If

            ' check select item
            If objUtility.GetQueryString(strConfirmSel) = "True" Then
                SetValueToControl()
                UpdateDataTable()
                lblInvoiceAmount.Text = Format(Convert.ToDouble(Session("decSaleInvoiceAmount")), "#,##0.00")
                DisplayPageFirst(Request.QueryString("PageNo"))
                DisplayPageSecond(Request.QueryString("PageNo"))
                Exit Sub
            End If

            'Set QueryString
            If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                Session("receive_header_id") = 0
            Else
                Session("receive_header_id") = Request.QueryString("id")
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                ClearSession()
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                ClearSession()
                'get data for display on screen
                LoadInitialUpdate()
            End If
            'set focus on invoice no item
            txtInvoiceNo.Focus()

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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Special Job Order menu
            objAction = objPermission.CheckPermission(42)
            ' set permission Create
            btnSave.Enabled = objAction.actCreate
            btnClear.Enabled = objAction.actList
            btnSearch.Enabled = objAction.actList
            btnSelect.Enabled = objAction.actCreate
            Session("actAmount") = objAction.actAmount
            ' set permission on amount           
            If Not Session("actAmount") Then
                lblInvoiceAmount.Visible = False
                lblInvoiceHidAmount.Visible = True
            Else
                lblInvoiceAmount.Visible = True
                lblInvoiceHidAmount.Visible = False
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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataDropdrownList()
        Try
            ' call function set Customer dropdownlist
            LoadListVendor()
            ' call function set Account Title dropdownlist
            LoadListIE()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataDropdrownList", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSession
    '	Discription	    : Set session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSession()
        Try
            ' clear session before used in this page
            Session("flagAddMod") = Nothing
            Session("flagAddMod") = "1" 'Flag check screen : 1: Menagemant screen , Nothing : Search screen

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page 
            Session("receive_header_id") = Nothing
            Session("txtInvoiceNo") = Nothing
            Session("txtIssueDate") = Nothing
            Session("txtReceiveDate") = Nothing
            Session("ddlAccountTitle") = Nothing
            Session("ddlCustomer") = Nothing
            Session("rblAccountType") = Nothing
            Session("rblInvoiceType") = Nothing
            Session("txtBankFee") = Nothing
            Session("lblInvoiceAmount") = Nothing
            Session("txtJobOrder") = Nothing
            Session("txtCurrency") = Nothing
            Session("txtExchangeRate") = Nothing
            Session("job_order_po_id") = Nothing
            Session("job_order_id") = Nothing
            Session("hontai_type") = Nothing
            Session("idDelete") = Nothing
            Session("dtJobOrderSaleInvoice") = Nothing
            Session("dtJobOrderSaleInvoiceEdit") = Nothing
            Session("dtBeforeDelete") = Nothing
            Session("dtDeleteFlag") = Nothing

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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            'Redirect to this screen for reload data
            If Session("Mode") = "Add" Then
                Response.Redirect("KTJB06.aspx?Mode=Add")
            Else
                Response.Redirect("KTJB06.aspx?Mode=Edit&id=" & Session("receive_header_id"))
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
    '	Create Date	    : 26-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim issueDate As String = ""
            Dim receiveDate As String = ""

            'Replace date dd/mm/yyyy to array
            Dim arrIssueDate() As String = Split(txtIssueDate.Text.Trim(), "/")
            Dim arrReceiveDate() As String = Split(txtReceiveDate.Text.Trim(), "/")
            'set issue date to yyyymmdd format
            If UBound(arrIssueDate) > 0 Then
                issueDate = arrIssueDate(2) & arrIssueDate(1) & arrIssueDate(0)
            End If
            'set Receive date to yyyymmdd format
            If UBound(arrReceiveDate) > 0 Then
                receiveDate = arrReceiveDate(2) & arrReceiveDate(1) & arrReceiveDate(0)
            End If

            ' assign value to dto object
            With objSaleInvoiceDto
                .id = Session("receive_header_id")
                .invoice_no = txtInvoiceNo.Text.Trim
                If txtIssueDate.Text <> "" Then
                    .issue_date = issueDate
                End If
                If txtReceiveDate.Text <> "" Then
                    .receipt_date = receiveDate
                End If
                .account_title = ddlAccountTitle.SelectedValue
                .customer = ddlCustomer.SelectedValue
                .account_type = rblAccountType.SelectedValue
                .invoice_type = rblInvoiceType.SelectedValue
                '.bank_fee = txtBankFee.Text.Trim
                If lblInvoiceAmount.Text <> "" Then
                    .total_amount = lblInvoiceAmount.Text
                End If
                .job_order = txtJobOrder.Text.Trim
                .currency = txtCurrency.Text
                .schedule_rate = txtScheduleRate.Text
                .actual_rate = txtExchangeRate.Text.Trim
                .account_next_approve = Session("AccountNextApprove")
            End With

            ' set dto object to session
            Session("objSaleInvoiceDto") = objSaleInvoiceDto

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
    '	Create Date	    : 30-07-2013
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

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVendor", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListIE
    '	Discription	    : Load list ie function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListIE()
        Try
            ' object Sale Invoice service
            Dim objAccountTitleSer As New Service.ImpSale_InvoiceService
            ' listAccountTitleDto for keep value from service
            Dim listAccountTitleDto As New List(Of Dto.SaleInvoiceDto)
            ' call function GetAccountTitleForList from service
            listAccountTitleDto = objAccountTitleSer.GetAccountTitleForList

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlAccountTitle, listAccountTitleDto, "name", "id", True)

            ' set select Account Title from session
            If Not IsNothing(Session("ddlAccountTitle")) And ddlAccountTitle.Items.Count > 0 Then
                ddlAccountTitle.SelectedValue = Session("ddlAccountTitle")
            Else
                ddlAccountTitle.SelectedValue = "2"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListIE", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListVat
    '	Discription	    : Load list vat function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListVat(ByVal vat_id As Integer, ByVal ddlVat As DropDownList)
        Try
            ' object vat service
            Dim objVatSer As New Service.ImpVatService
            ' listVatDto for keep value from service
            Dim listVatDto As New List(Of Dto.VatDto)
            ' call function GetVatForList from service
            listVatDto = objVatSer.GetVatForList

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlVat, listVatDto, "PercentString", "ID", True)

            ' set select Account Title from session
            If Not String.IsNullOrEmpty(vat_id) And ddlVat.Items.Count > 0 Then
                ddlVat.SelectedValue = vat_id
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListVat", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadListWT
    '	Discription	    : Load list wt function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListWT(ByVal wt_id As Integer, ByVal ddlWT As DropDownList)
        Try
            ' object wt service
            Dim objWTSer As New Service.ImpWTService
            ' listWtDto for keep value from service
            Dim listWtDto As New List(Of Dto.WTDto)
            ' call function GetVatForList from service
            listWtDto = objWTSer.GetWTForList

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlWT, listWtDto, "PercentString", "ID", True)

            ' set select Account Title from session
            If Not String.IsNullOrEmpty(wt_id) And ddlWT.Items.Count > 0 Then
                ddlWT.SelectedValue = wt_id
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadListWT", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' sale invoice Dto object for keep return value from service
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim intSaleInvoiceID As Integer = 0
            Dim strIssueDate As String = ""
            Dim strReceiptDate As String = ""

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intSaleInvoiceID = CInt(objUtility.GetQueryString("id"))
                Session("receive_header_id") = intSaleInvoiceID
            Else
                intSaleInvoiceID = Session("receive_header_id")
            End If

            '---Header Screen
            ' call function GetSaleInvoiceHeaderByID from service 
            objSaleInvoiceDto = objSaleInvoiceSer.GetSaleInvoiceHeaderByID(intSaleInvoiceID)

            ' assign value to control
            With objSaleInvoiceDto
                'Set format date to dd/mm/yyyy 
                If .issue_date <> Nothing Or .issue_date <> "" Then
                    strIssueDate = Right(.issue_date, 2) & "/" & Mid(.issue_date, 5, 2) & "/" & Left(.issue_date, 4)
                End If
                If .receipt_date <> Nothing Or .receipt_date <> "" Then
                    strReceiptDate = Right(.receipt_date, 2) & "/" & Mid(.receipt_date, 5, 2) & "/" & Left(.receipt_date, 4)
                End If

                txtInvoiceNo.Text = .invoice_no
                txtIssueDate.Text = strIssueDate
                txtReceiveDate.Text = strReceiptDate
                ddlAccountTitle.SelectedValue = .ie_id
                ddlCustomer.SelectedValue = .vendor_id
                rblAccountType.SelectedValue = .account_type
                rblInvoiceType.SelectedValue = .invoice_type
                'txtBankFee.Text = .bank_fee
                lblInvoiceAmount.Text = Format(Convert.ToDouble(.total_amount), "#,##0.00")
            End With

            '---Detail Screen
            ' call function GetSaleInvoiceDetailByID from service 
            objSaleInvoiceDto = objSaleInvoiceSer.GetSaleInvoiceDetailByID(intSaleInvoiceID)

            'set format date to yyymmdd
            Dim issueDate As String = ""
            'Replace date dd/mm/yyyy to array
            Dim arrIssueDate() As String = Split(txtIssueDate.Text.Trim(), "/")
            'set issue date to yyyymmdd format
            If UBound(arrIssueDate) > 0 Then
                issueDate = arrIssueDate(2) & arrIssueDate(1) & arrIssueDate(0)
            End If

            ' assign value to control
            With objSaleInvoiceDto
                txtJobOrder.Text = .job_order
                txtCurrency.Text = .currency
                'cal function get ScheduleRate
                GetScheduleRate(issueDate, .currency_id)

                If Session("txtExchangeRate") Is Nothing Or Session("txtExchangeRate") = "" Then
                    If .currency = "THB" And (.actual_rate = "" Or .actual_rate Is Nothing) Then
                        txtExchangeRate.Text = "1.00000"
                    Else
                        txtExchangeRate.Text = .actual_rate
                    End If
                Else
                    txtExchangeRate.Text = Session("txtExchangeRate")
                End If

            End With

            '---ส่วนSearch Job Order Table ที่ยังไม่ได้ออก Sale invoice
            ' cal funtion GetDataDetailFrist
            GetDataDetailFrist()
            DisplayPageFirst(Request.QueryString("PageNo"))

            '--- Sale invoice Detail ส่วนที่ 2
            ' cal funtion GetDataDetailSecond
            GetDataDetailSecond()
            DisplayPageSecond(Request.QueryString("PageNo"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: GetScheduleRate
    '	Discription	    : Get Schedule Rate (THB)
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetScheduleRate(ByVal issue_date As String, ByVal currency_id As Integer)
        Try
            Dim intScheculeRate As Decimal
            Dim intChk As Decimal
            Dim objScheculeRateSer As New Service.ImpScheculeRateService

            'call GetScheculeRate from ScheculeRate Service 
            intScheculeRate = objScheculeRateSer.GetScheculeRate(currency_id, issue_date)
            intChk = intScheculeRate

            If intScheculeRate <= 0 Then
                intScheculeRate = 1
            End If
            'set data to ScheduleRate item
            txtScheduleRate.Text = Format(Convert.ToDouble(intScheculeRate), "#,##0.00000")

            'check Schecule Rate = 0
            If intChk <= 0 Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetScheduleRate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetDataDetailFrist
    '	Discription	    : Get data ส่วน Search Job Order Table ที่ยังไม่ได้ออก Sale invoice
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetDataDetailFrist()
        Try
            ' table object keep value from item service
            Dim dtJobOrderSaleInvoice As New DataTable
            Dim objTotalSaleInvoiceDto As New Dto.SaleInvoiceDto

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetJobOrerSaleInvoiceDetail from ISale_InvoiceService
            dtJobOrderSaleInvoice = objSaleInvoiceSer.GetJobOrerSaleInvoiceDetail(txtJobOrder.Text)

            ' set table object to session
            Session("dtJobOrderSaleInvoice") = Nothing
            Session("dtJobOrderSaleInvoice") = dtJobOrderSaleInvoice

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetDataDetailFrist", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetDataDetailSecond
    '	Discription	    : Get data ส่วน Search Sale invoice on detail part
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetDataDetailSecond()
        Try
            ' table object keep value from item service
            Dim dtJobOrderSaleInvoiceEdit As New DataTable
            Dim objTotalSaleInvoiceDto As New Dto.SaleInvoiceDto

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetJobOrerSaleInvoiceDetail from ISale_InvoiceService
            dtJobOrderSaleInvoiceEdit = objSaleInvoiceSer.GetJobOrerSaleInvoiceDetailEdit(Session("receive_header_id"))

            ' set table object to session
            Session("dtJobOrderSaleInvoiceEdit") = dtJobOrderSaleInvoiceEdit

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetDataDetailSecond", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPageFirst
    '	Discription	    : Display page when link sale invoice menu
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageFirst(ByVal intPageNo As Integer)
        Try
            Dim dtJobOrderSaleInvoice As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtJobOrderSaleInvoice = Session("dtJobOrderSaleInvoice")

            ' check record for display
            If Not IsNothing(dtJobOrderSaleInvoice) AndAlso dtJobOrderSaleInvoice.Rows.Count > 0 Then
                ' bound data between pageDate with repeater
                rptDetailFirst.DataSource = dtJobOrderSaleInvoice
                rptDetailFirst.DataBind()
            Else
                ' case not exist data
                ' clear binding data and clear description               
                rptDetailFirst.DataSource = Nothing
                rptDetailFirst.DataBind()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageFirst", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPageSecond
    '	Discription	    : Display page when link sale invoice menu
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 30-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageSecond(ByVal intPageNo As Integer)
        Try
            Dim dtJobOrderSaleInvoiceEdit As New DataTable
            Dim objPage As New Common.Utilities.Paging
            Dim dtJobOrderForDelete As New DataTable

            If Session("dtDeleteflag") Is Nothing Then
                dtJobOrderForDelete = Session("dtDeleteFlag")
            End If

            ' get table object from session 
            dtJobOrderSaleInvoiceEdit = Session("dtJobOrderSaleInvoiceEdit")

            ' check record for display
            If Not IsNothing(dtJobOrderSaleInvoiceEdit) AndAlso dtJobOrderSaleInvoiceEdit.Rows.Count > 0 Then
                ' bound data between pageDate with repeater
                rptDetailSecond.DataSource = dtJobOrderSaleInvoiceEdit
                rptDetailSecond.DataBind()


            Else ' case not exist data
                ' clear binding data and clear description               
                rptDetailSecond.DataSource = Nothing
                rptDetailSecond.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageSecond", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            Dim objIsDate As New Common.Validations.Validation

            CheckCriteriaInput = False
            'Check format date of field Issue Date 
            If txtIssueDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field Receive Date
            If txtReceiveDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiveDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check bank free >= 0
            'If txtBankFee.Text <> "" Then
            '    If CInt(txtBankFee.Text) < 0 Then
            '        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_014"))
            '        Exit Function
            '    End If
            'End If

            If Session("Mode") = "Add" Then
                'Check exist invoice no on receive_header table 
                If objSaleInvoiceSer.IsUsedInReceiveHeader(txtInvoiceNo.Text.Trim) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_007"))
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
    '	Function name	: SearchSaleInvoiceDetail
    '	Discription	    : Get data for show on detail screen
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchSaleInvoiceDetail()
        Try
            ' sale invoice Dto object for keep return value from service
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            'set format date to yyymmdd
            Dim issueDate As String = ""
            'Replace date dd/mm/yyyy to array
            Dim arrIssueDate() As String = Split(txtIssueDate.Text.Trim(), "/")
            'set issue date to yyyymmdd format
            If UBound(arrIssueDate) > 0 Then
                issueDate = arrIssueDate(2) & arrIssueDate(1) & arrIssueDate(0)
            End If

            '---Detail Screen
            ' call function GetSaleInvoiceByJobOrder from service 
            objSaleInvoiceDto = objSaleInvoiceSer.GetSaleInvoiceByJobOrder(Session("txtJobOrder"))

            ' assign value to control
            With objSaleInvoiceDto
                If Not .job_order Is Nothing Then
                    txtJobOrder.Text = .job_order
                End If
                txtCurrency.Text = .currency
                'cal function get ScheduleRate
                GetScheduleRate(issueDate, .currency_id)
                'Mod 2013/09/11
                'If Session("txtExchangeRate") Is Nothing Or Session("txtExchangeRate") = "" Then
                If .currency = "THB" And (.actual_rate = "" Or .actual_rate Is Nothing) Then
                    txtExchangeRate.Text = "1.00000"
                Else
                    txtExchangeRate.Text = .actual_rate
                End If
                'Else
                'txtExchangeRate.Text = Session("txtExchangeRate")
                'End If
            End With

            '---ส่วนSearch Job Order Table ที่ยังไม่ได้ออก Sale invoice
            ' cal funtion GetDataDetailFrist
            GetDataDetailFrist()
            DisplayPageFirst(Request.QueryString("PageNo"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchSaleInvoiceDetail", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataTable
    '	Discription	    : Keep data of each record that is already checked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    : Wasan D.
    '	Update Date	    : 28-10-2013
    '*************************************************************/
    Private Sub SetDataTable()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            Dim intChkSelect As Integer = 0
            Dim decActualRate As Decimal
            Dim decSaleInvoiceAmount As Decimal = 0
            If Not String.IsNullOrEmpty(txtExchangeRate.Text.Trim) Then
                decActualRate = CDec(txtExchangeRate.Text.Trim)
            Else
                decActualRate = 0
            End If
            ' set data table
            Dim dt As New DataTable

            Dim dtFirst As New DataTable
            dtFirst = Session("dtJobOrderSaleInvoice")
            Session("dtJobOrderSaleInvoiceEditTemp") = Nothing
            ' data row object
            Dim row As DataRow
            Dim vat_id As String = WebConfigurationManager.AppSettings("KTJB06_vat_id")
            Dim wt_id As String = WebConfigurationManager.AppSettings("KTJB06_wt_id")

            If Session("dtJobOrderSaleInvoiceEditTemp") Is Nothing Then
                ' assign column header
                With dt
                    .Columns.Add("id")
                    .Columns.Add("receive_header_id")
                    .Columns.Add("job_order_po_id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("po_no")
                    .Columns.Add("po_type_name")
                    .Columns.Add("hontai")
                    .Columns.Add("amount_thb")
                    .Columns.Add("actual_rate")
                    .Columns.Add("po_date")
                    .Columns.Add("vat_id")
                    .Columns.Add("wt_id")
                    .Columns.Add("remark")
                    .Columns.Add("po_type")
                    .Columns.Add("hontai_type")
                    .Columns.Add("amount")
                    .Columns.Add("hontai_fg1")
                    .Columns.Add("hontai_fg2")
                    .Columns.Add("hontai_fg3")
                    .Columns.Add("po_fg")
                    .Columns.Add("job_order")
                    .Columns.Add("bank_fee")
                    .Columns.Add("hontai_cond")
                    .Columns.Add("job_type")

                    For Each item As RepeaterItem In rptDetailFirst.Items
                        Dim amount As Decimal
                        Dim chkBox As HtmlInputCheckBox
                        chkBox = item.FindControl("chkApprove")

                        'Keep data to datatable
                        If chkBox.Checked = True Then
                            row = .NewRow

                            amount = CDec(dtFirst.Rows(i)("amount").ToString()) * decActualRate
                            row("id") = ""
                            row("job_order_po_id") = dtFirst.Rows(i)("id").ToString()
                            row("receive_header_id") = ""
                            row("job_order_id") = dtFirst.Rows(i)("job_order_id").ToString()
                            row("po_no") = dtFirst.Rows(i)("po_no").ToString()
                            row("po_type") = dtFirst.Rows(i)("po_type").ToString()
                            row("po_type_name") = dtFirst.Rows(i)("po_type_name").ToString()
                            row("hontai") = dtFirst.Rows(i)("hontai").ToString()
                            row("hontai_type") = dtFirst.Rows(i)("hontai_type").ToString()
                            row("amount") = dtFirst.Rows(i)("amount").ToString()
                            row("amount_thb") = Format(Convert.ToDouble(amount), "#,##0.00")
                            row("actual_rate") = decActualRate
                            row("po_date") = dtFirst.Rows(i)("po_date").ToString()
                            row("vat_id") = vat_id
                            row("wt_id") = wt_id
                            row("remark") = ""
                            row("hontai_fg1") = ""
                            row("hontai_fg2") = ""
                            row("hontai_fg3") = ""
                            row("po_fg") = ""
                            row("job_order") = dtFirst.Rows(i)("job_order").ToString()
                            row("bank_fee") = ""
                            row("hontai_cond") = dtFirst.Rows(i)("hontai_cond").ToString()
                            row("job_type") = dtFirst.Rows(i)("job_type").ToString()

                            decSaleInvoiceAmount = decSaleInvoiceAmount + amount
                            dt.Rows.Add(row)
                            intChkSelect = intChkSelect + 1
                        End If
                        i = i + 1

                    Next
                End With

            Else
                dt = Session("dtJobOrderSaleInvoiceEditTemp")

                ' assign column header
                With dt
                    For Each item As RepeaterItem In rptDetailFirst.Items
                        Dim amount As Decimal
                        Dim chkBox As HtmlInputCheckBox
                        chkBox = item.FindControl("chkApprove")

                        'Keep data to datatable
                        If chkBox.Checked = True Then
                            row = .NewRow

                            amount = CDec(dtFirst.Rows(i)("amount").ToString()) * decActualRate
                            row("id") = ""
                            row("job_order_po_id") = dtFirst.Rows(i)("id").ToString()
                            row("receive_header_id") = ""
                            row("job_order_id") = dtFirst.Rows(i)("job_order_id").ToString()
                            row("po_no") = dtFirst.Rows(i)("po_no").ToString()
                            row("po_type") = dtFirst.Rows(i)("po_type").ToString()
                            row("po_type_name") = dtFirst.Rows(i)("po_type_name").ToString()
                            row("hontai") = dtFirst.Rows(i)("hontai").ToString()
                            row("hontai_type") = dtFirst.Rows(i)("hontai_type").ToString()
                            row("amount") = dtFirst.Rows(i)("amount").ToString()
                            row("amount_thb") = Format(Convert.ToDouble(amount), "#,##0.00")
                            row("actual_rate") = decActualRate
                            row("po_date") = dtFirst.Rows(i)("po_date").ToString()
                            row("vat_id") = 2 'vat_id
                            row("wt_id") = 1 'wt_id
                            row("remark") = ""
                            row("hontai_fg1") = ""
                            row("hontai_fg2") = ""
                            row("hontai_fg3") = ""
                            row("po_fg") = ""
                            row("job_order") = dtFirst.Rows(i)("job_order").ToString()
                            row("bank_fee") = ""
                            row("hontai_cond") = dtFirst.Rows(i)("hontai_cond").ToString()
                            row("job_type") = dtFirst.Rows(i)("job_type").ToString()

                            decSaleInvoiceAmount = decSaleInvoiceAmount + amount
                            dt.Rows.Add(row)
                            intChkSelect = intChkSelect + 1
                        End If
                        i = i + 1

                    Next
                End With
            End If

            'Set dataTable into session
            Session("dtJobOrderSaleInvoiceEditTemp") = dt
            Session("decSaleInvoiceAmountTmp") = decSaleInvoiceAmount
            Session("intChkSelect") = intChkSelect

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataTable", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: SelectSaleInvoice
    '	Discription	    : Select Sale invoice
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SelectSaleInvoice()
        Try
            ' call function set value to control
            SetValueToControl()

            'Add data to Detail Table
            SetDataTable()
            DisplayPageSecond(Request.QueryString("PageNo"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SelectSaleInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 31-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim issueDate As String = ""
            Dim receiveDate As String = ""

            ' set value to dto object from session
            objSaleInvoiceDto = Session("objSaleInvoiceDto")

            ' set value to control
            With objSaleInvoiceDto
                txtInvoiceNo.Text = .invoice_no
                'txtBankFee.Text = .bank_fee
                ddlAccountTitle.SelectedValue = .account_title
                ddlCustomer.SelectedValue = .customer
                rblAccountType.SelectedValue = .account_type
                rblInvoiceType.SelectedValue = .invoice_type
                txtJobOrder.Text = .job_order
                txtExchangeRate.Text = .actual_rate
                txtScheduleRate.Text = .schedule_rate
                txtCurrency.Text = .currency

                If Not .issue_date Is Nothing Or .issue_date <> "" Then
                    issueDate = Right(.issue_date, 2) & "/" & Mid(.issue_date, 5, 2) & "/" & Left(.issue_date, 4)
                    txtIssueDate.Text = issueDate
                Else
                    txtIssueDate.Text = .issue_date
                End If
                If Not .receipt_date Is Nothing Or .receipt_date <> "" Then
                    receiveDate = Right(.receipt_date, 2) & "/" & Mid(.receipt_date, 5, 2) & "/" & Left(.receipt_date, 4)
                    txtReceiveDate.Text = receiveDate
                Else
                    txtReceiveDate.Text = .receipt_date
                End If

            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteSaleInvoice
    '	Discription	    : Delete Sale invoice data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteSaleInvoice()
        Try

            ' check state of delete item
            If DeleteDetailTable() Then
                If objSaleInvoiceSer.DeleteJobOrderPOFlag(Session("dtDeleteFlag")) Then
                    ' case delete success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_012"))

                Else
                    Session("dtJobOrderSaleInvoiceEdit") = Session("dtBeforeDelete")
                    ' case delete not success show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_013"))
                End If
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_013"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteDetailTable
    '	Discription	    : Delete data on detail table
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    : Wasan D.
    '	Update Date	    : 11-10-2013
    '*************************************************************/
    Private Function DeleteDetailTable() As Boolean
        Try
            DeleteDetailTable = False

            'Keep data of each record that is already checked
            Dim intChkSelect As Integer = 0
            Dim decActualRate As Decimal
            Dim decSaleInvoiceAmount As Decimal = 0
            Dim strIdDelete As String = ""
            Dim strJob_Order_id As String = ""
            Dim strPo_type As String = ""
            Dim strHontai_fg1 As String = ""
            Dim strHontai_fg2 As String = ""
            Dim strHontai_fg3 As String = ""
            Dim strPO_fg As String = ""
            Dim dtDelete As New DataTable
            Dim amount As Decimal
            Dim job_order_po_id As String
            Dim job_order_id As String
            Dim hontai_type As String
            'Get Actual Exchange Rate (THB)
            If Not String.IsNullOrEmpty(txtExchangeRate.Text.Trim) Then
                decActualRate = CDec(txtExchangeRate.Text.Trim)
            Else
                decActualRate = 0
            End If

            ' set data table
            Dim dt As New DataTable
            Dim dtSecond As New DataTable
            dtSecond = Session("dtJobOrderSaleInvoiceEdit")
            Session("dtBeforeDelete") = Session("dtJobOrderSaleInvoiceEdit")
            ' data row object
            Dim row As DataRow

            'Set data table for update flag
            If Session("dtDeleteFlag") Is Nothing Then
                With dtDelete
                    .Columns.Add("job_order_po_id")
                    .Columns.Add("hontai_type")
                    .Columns.Add("po_type")
                    .Columns.Add("hontai_fg1")
                    .Columns.Add("hontai_fg2")
                    .Columns.Add("hontai_fg3")
                    .Columns.Add("po_fg")
                End With
            Else
                dtDelete = Session("dtDeleteFlag")
            End If

            ' assign column header
            With dt
                .Columns.Add("id")
                .Columns.Add("receive_header_id")
                .Columns.Add("job_order_po_id")
                .Columns.Add("job_order_id")
                .Columns.Add("po_no")
                .Columns.Add("po_type_name")
                .Columns.Add("hontai")
                .Columns.Add("amount_thb")
                .Columns.Add("actual_rate")
                .Columns.Add("po_date")
                .Columns.Add("vat_id")
                .Columns.Add("wt_id")
                .Columns.Add("remark")
                .Columns.Add("po_type")
                .Columns.Add("hontai_type")
                .Columns.Add("amount")
                .Columns.Add("hontai_fg1")
                .Columns.Add("po_fg")
                .Columns.Add("hontai_fg2")
                .Columns.Add("hontai_fg3")
                ' Start Edit 11-10-2013
                .Columns.Add("job_order")
                .Columns.Add("bank_fee")
                ' End Edit 11-10-2013
                For i As Integer = 0 To dtSecond.Rows.Count - 1

                    job_order_po_id = dtSecond.Rows(i)("job_order_po_id").ToString()
                    job_order_id = dtSecond.Rows(i)("job_order_id").ToString()
                    hontai_type = dtSecond.Rows(i)("hontai_type").ToString()

                    'Keep data to datatable
                    If job_order_po_id = Session("job_order_po_id") And job_order_id = Session("job_order_id") And hontai_type = Session("hontai_type") Then
                        If strIdDelete = "" Then
                            strIdDelete = dtSecond.Rows(i)("id").ToString()
                        Else
                            If dtSecond.Rows(i)("id").ToString() <> "" Then
                                strIdDelete = strIdDelete & "," & dtSecond.Rows(i)("id").ToString()
                            End If
                        End If
                        'Set data into datatable for update
                        dtDelete.Rows.Add(dtSecond.Rows(i)("job_order_po_id").ToString(), _
                                          dtSecond.Rows(i)("hontai_type").ToString(), _
                                          dtSecond.Rows(i)("po_type").ToString(), _
                                          dtSecond.Rows(i)("hontai_fg1").ToString(), _
                                          dtSecond.Rows(i)("hontai_fg2").ToString(), _
                                          dtSecond.Rows(i)("hontai_fg3").ToString(), _
                                          dtSecond.Rows(i)("po_fg").ToString())
                    Else
                        row = .NewRow

                        amount = CDec(dtSecond.Rows(i)("amount").ToString()) * decActualRate
                        row("id") = dtSecond.Rows(i)("id").ToString()
                        row("job_order_po_id") = dtSecond.Rows(i)("job_order_po_id").ToString()
                        row("receive_header_id") = dtSecond.Rows(i)("receive_header_id").ToString()
                        row("job_order_id") = dtSecond.Rows(i)("job_order_id").ToString()
                        row("po_no") = dtSecond.Rows(i)("po_no").ToString()
                        row("po_type") = dtSecond.Rows(i)("po_type").ToString()
                        row("po_type_name") = dtSecond.Rows(i)("po_type_name").ToString()
                        row("hontai") = dtSecond.Rows(i)("hontai").ToString()
                        row("hontai_type") = dtSecond.Rows(i)("hontai_type").ToString()
                        row("amount") = dtSecond.Rows(i)("amount").ToString()
                        row("amount_thb") = dtSecond.Rows(i)("amount_thb").ToString()
                        row("actual_rate") = dtSecond.Rows(i)("actual_rate").ToString()
                        row("po_date") = dtSecond.Rows(i)("po_date").ToString()
                        row("vat_id") = dtSecond.Rows(i)("vat_id").ToString()
                        row("wt_id") = dtSecond.Rows(i)("wt_id").ToString()
                        row("remark") = dtSecond.Rows(i)("remark").ToString()
                        row("hontai_fg1") = dtSecond.Rows(i)("hontai_fg1").ToString()
                        row("hontai_fg2") = dtSecond.Rows(i)("hontai_fg2").ToString()
                        row("hontai_fg3") = dtSecond.Rows(i)("hontai_fg3").ToString()
                        row("po_fg") = dtSecond.Rows(i)("po_fg").ToString()
                        ' Start Edit 11-10-2013
                        row("job_order") = dtSecond.Rows(i)("job_order").ToString()
                        row("bank_fee") = dtSecond.Rows(i)("bank_fee").ToString()
                        ' End Edit 11-10-2013
                        decSaleInvoiceAmount = decSaleInvoiceAmount + amount
                        dt.Rows.Add(row)
                    End If
                Next
            End With

            'Set dataTable into session
            Session("dtJobOrderSaleInvoiceEdit") = dt
            Session("decSaleInvoiceAmount") = decSaleInvoiceAmount
            Session("idDelete") = Session("idDelete") & "," & strIdDelete
            Session("dtDeleteFlag") = dtDelete

            DeleteDetailTable = True
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteDetailTable", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: InsertSaleInvoice
    '	Discription	    : Insert Sale invoice 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertSaleInvoice()
        Try
            Dim strJobOrder1 As String
            Dim strJobOrder2 As String
            Dim strJobOrder3 As String
            'cal function GetJobOrderHontai
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            objSaleInvoiceDto = objSaleInvoiceSer.GetJobOrderHontai(Session("dtJobOrderSaleInvoiceEdit"))

            With objSaleInvoiceDto
                strJobOrder1 = objSaleInvoiceDto.strJobOrder1
                strJobOrder2 = objSaleInvoiceDto.strJobOrder2
                strJobOrder3 = objSaleInvoiceDto.strJobOrder3

            End With

            ' cal function InsertSaleInvoice for insert data
            If objSaleInvoiceSer.InsertSaleInvoice( _
                                                    strJobOrder1, strJobOrder2, strJobOrder3, _
                                                    Session("objSaleInvoiceDto"), _
                                                    Session("dtJobOrderSaleInvoiceEdit")) Then
                ' case update success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_002"), Nothing, "KTJB05.aspx?New=False")

            Else
                ' case delete not success show message box

                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_003"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertSaleInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateSaleInvoice
    '	Discription	    : Update Sale invoice 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateSaleInvoice()
        Try
            ' check state of save item
            If objSaleInvoiceSer.UpdateSaleInvoice(Session("idDelete"), Session("objSaleInvoiceDto"), Session("dtJobOrderSaleInvoiceEdit")) Then
                ' case update success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_005"), Nothing, "KTJB05.aspx?New=False")
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_006"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateSaleInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Item_IndexChanged
    '	Discription	    : item on repeater is change 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Item_IndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Update DataTable 
            Dim dtUpdate As New DataTable
            Dim remark As String
            Dim vat As String
            Dim wt As String
            Dim bankfee As String
            ' get table object from session 
            dtUpdate = Session("dtJobOrderSaleInvoiceEdit")

            For Each item As RepeaterItem In rptDetailSecond.Items
                Dim ddlVat As New DropDownList
                Dim ddlWt As New DropDownList
                Dim txtRemark As New TextBox
                Dim txtBankfee As New TextBox

                ddlVat = DirectCast(item.FindControl("ddlVat"), DropDownList)
                ddlWt = DirectCast(item.FindControl("ddlWt"), DropDownList)
                txtRemark = DirectCast(item.FindControl("txtRemark"), TextBox)
                txtBankfee = DirectCast(item.FindControl("txtBankfee"), TextBox)
                remark = txtRemark.Text
                vat = ddlVat.SelectedValue
                wt = ddlWt.SelectedValue
                bankfee = txtBankfee.Text
            Next
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Item_IndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteDetailTable
    '	Discription	    : Delete data on detail table
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertDataTable()
        Try
            ' set data table
            Dim dt As New DataTable
            Dim i As Integer = 0
            Dim dtSecond As New DataTable
            dtSecond = Session("dtJobOrderSaleInvoiceEdit")

            For Each item As RepeaterItem In rptDetailSecond.Items
                Dim ddlVat As New DropDownList
                Dim ddlWt As New DropDownList
                Dim txtRemark As New TextBox
                Dim txtBankfee As New TextBox

                ddlVat = DirectCast(item.FindControl("ddlVat"), DropDownList)
                ddlWt = DirectCast(item.FindControl("ddlWt"), DropDownList)
                txtRemark = DirectCast(item.FindControl("txtRemark"), TextBox)
                txtBankfee = DirectCast(item.FindControl("txtBankfee"), TextBox)

                dtSecond.Rows(i)("vat_id") = ddlVat.SelectedValue
                dtSecond.Rows(i)("wt_id") = ddlWt.SelectedValue
                dtSecond.Rows(i)("remark") = txtRemark.Text
                dtSecond.Rows(i)("bank_fee") = txtBankfee.Text

                dtSecond.AcceptChanges()
                i = i + 1
            Next

            'Set dataTable into session
            Session("dtJobOrderSaleInvoiceEdit") = dtSecond

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertDataTable", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: UpdateDataTable
    '	Discription	    : Update data on detail table
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateDataTable()
        Try
            ' set data table  
            Dim decActualRate As Decimal
            Dim decSaleInvoiceAmount As Decimal = 0
            Dim amountDt As Decimal = 0
            Dim dt As New DataTable
            Dim dtTemp As New DataTable
            dtTemp = Session("dtJobOrderSaleInvoiceEditTemp")
            dt = Session("dtJobOrderSaleInvoiceEdit")

            'Actual Exchange Rate (THB)
            If Not String.IsNullOrEmpty(txtExchangeRate.Text.Trim) Then
                decActualRate = CDec(txtExchangeRate.Text.Trim)
            Else
                decActualRate = 0
            End If

            'case data table temp have data
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    'Loop for keep data into datatable
                    For i As Integer = 0 To dtTemp.Rows.Count - 1

                        With dt
                            Dim amount As Decimal
                            amount = CDec(dtTemp.Rows(i)("amount").ToString()) * decActualRate
                            'WebConfigurationManager.AppSettings("KTJB06_vat_id"), _
                            'WebConfigurationManager.AppSettings("KTJB06_wt_id"), _

                            dt.Rows.Add(dtTemp.Rows(i)("id").ToString(), _
                                        dtTemp.Rows(i)("receive_header_id").ToString(), _
                                        dtTemp.Rows(i)("job_order_po_id").ToString(), _
                                        dtTemp.Rows(i)("job_order_id").ToString(), _
                                        dtTemp.Rows(i)("po_no").ToString(), _
                                        dtTemp.Rows(i)("po_type_name").ToString(), _
                                        dtTemp.Rows(i)("hontai").ToString(), _
                                        Format(Convert.ToDouble(amount), "#,##0.00"), _
                                        decActualRate, _
                                        dtTemp.Rows(i)("po_date").ToString(), _
                                        WebConfigurationManager.AppSettings("KTJB06_vat_id"), _
                                        WebConfigurationManager.AppSettings("KTJB06_wt_id"), _
                                        "", _
                                        dtTemp.Rows(i)("po_type").ToString(), _
                                        dtTemp.Rows(i)("hontai_type").ToString(), _
                                        dtTemp.Rows(i)("amount").ToString(), _
                                        dtTemp.Rows(i)("hontai_fg1").ToString(), _
                                        dtTemp.Rows(i)("hontai_fg2").ToString(), _
                                        dtTemp.Rows(i)("hontai_fg3").ToString(), _
                                        dtTemp.Rows(i)("po_fg").ToString(), _
                                        dtTemp.Rows(i)("job_order").ToString(), _
                                        dtTemp.Rows(i)("bank_fee").ToString(), _
                                        dtTemp.Rows(i)("hontai_cond").ToString(), _
                                        dtTemp.Rows(i)("job_type").ToString())
                        End With
                    Next
                End If
            End If

            'Set dataTable into session
            If dt Is Nothing Then
                Session("dtJobOrderSaleInvoiceEdit") = dtTemp
                Session("decSaleInvoiceAmount") = Session("decSaleInvoiceAmountTmp")
            ElseIf dt.Rows.Count = 0 Then
                Session("dtJobOrderSaleInvoiceEdit") = dtTemp
                Session("decSaleInvoiceAmount") = Session("decSaleInvoiceAmountTmp")
            Else
                Session("dtJobOrderSaleInvoiceEdit") = dt
                'Keep Sale Invoice Amount
                For j As Integer = 0 To dt.Rows.Count - 1
                    amountDt = 0
                    amountDt = CDec(dt.Rows(j)("amount").ToString()) * decActualRate
                    decSaleInvoiceAmount = decSaleInvoiceAmount + amountDt

                Next

                Session("decSaleInvoiceAmount") = decSaleInvoiceAmount
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateDataTable", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: CheckDuplicateData
    '	Discription	    : Check Duplicate Data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckDuplicateData() As Boolean
        Try
            CheckDuplicateData = False

            'Keep data of each record that is already checked          
            Dim job_order_po_id As String
            Dim job_order_id As String
            Dim hontai_type As String
            Dim po_type As String
            Dim i As Integer = 0

            '---------------
            Dim job_order_po_id1 As String
            Dim job_order_id1 As String
            Dim hontai_type1 As String = ""
            Dim po_type1 As String = ""

            ' set data table
            Dim dt As New DataTable
            Dim dtSecond As New DataTable
            dtSecond = Session("dtJobOrderSaleInvoiceEdit")
            Dim dtFirst As New DataTable
            dtFirst = Session("dtJobOrderSaleInvoice")

            'Get data on rptDetailFirst
            For Each item As RepeaterItem In rptDetailFirst.Items
                Dim chkBox As HtmlInputCheckBox
                chkBox = item.FindControl("chkApprove")
                'Keep data to datatable
                If chkBox.Checked = True Then
                    'Keep data from rptDetailFirst
                    job_order_po_id1 = dtFirst.Rows(i)("id").ToString()
                    job_order_id1 = dtFirst.Rows(i)("job_order_id").ToString()
                    po_type1 = dtFirst.Rows(i)("po_type").ToString()
                    hontai_type1 = dtFirst.Rows(i)("hontai_type").ToString()

                    If Not dtSecond Is Nothing Then
                        If dtSecond.Rows.Count > 0 Then
                            For j As Integer = 0 To dtSecond.Rows.Count - 1
                                job_order_po_id = dtSecond.Rows(j)("job_order_po_id").ToString()
                                job_order_id = dtSecond.Rows(j)("job_order_id").ToString()
                                hontai_type = dtSecond.Rows(j)("hontai_type").ToString()
                                po_type = dtSecond.Rows(j)("po_type").ToString()

                                'check duplicate
                                If job_order_po_id = job_order_po_id1 And job_order_id = job_order_id1 _
                                    And hontai_type = hontai_type1 And po_type = po_type1 Then
                                    CheckDuplicateData = False
                                    Exit Function
                                End If
                            Next
                        End If
                    End If
                End If

                i = i + 1

            Next

            CheckDuplicateData = True
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckDuplicateData", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: CheckBankFee
    '	Discription	    : Check Bankfee Data
    '	Return Value	: Boolean
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckBankFee() As Integer
        Try
            Dim hasValueCount As Integer = 0
            For Each rptItem As RepeaterItem In rptDetailSecond.Items
                If CType(rptItem.FindControl("txtBankFee"), TextBox).Text <> "" Then
                    If CDec(CType(rptItem.FindControl("txtBankFee"), TextBox).Text) > 0 Then
                        hasValueCount += 1
                    End If
                End If
            Next
            Return hasValueCount
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckBankFee", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: CheckHontaiData
    '	Discription	    : Check hontai Data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 01-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckHontaiData() As Boolean
        Try
            CheckHontaiData = False

            'Keep data of each record that is already checked          
            'Dim job_order_po_id As String
            Dim job_order_id As String
            Dim hontai_type As String
            Dim po_type As String
            Dim chkHontai1 As Integer = 0
            Dim chkHontai2 As Integer = 0
            Dim chkHontai3 As Integer = 0

            ' set data table
            Dim dt As New DataTable
            Dim dtSecond As New DataTable
            dtSecond = Session("dtJobOrderSaleInvoiceEdit")

            If Not dtSecond Is Nothing Then
                If dtSecond.Rows.Count > 0 Then
                    For j As Integer = 0 To dtSecond.Rows.Count - 1
                        'job_order_po_id = dtSecond.Rows(j)("job_order_po_id").ToString()
                        job_order_id = dtSecond.Rows(j)("job_order_id").ToString()
                        hontai_type = dtSecond.Rows(j)("hontai_type").ToString()
                        po_type = dtSecond.Rows(j)("po_type").ToString()

                        If po_type = 0 Then
                            Select Case hontai_type
                                Case 1
                                    chkHontai1 = chkHontai1 + 1
                                Case 2
                                    'Check exist invoice no on receive_header table 
                                    If objSaleInvoiceSer.IsUsedInHontai(job_order_id, 1) Then
                                        chkHontai1 = chkHontai1 + 1
                                    End If
                                    chkHontai2 = chkHontai2 + 1
                                Case 3
                                    'Check exist invoice no on receive_header table 
                                    If objSaleInvoiceSer.IsUsedInHontai(job_order_id, 2) Then
                                        chkHontai2 = chkHontai2 + 1
                                    End If
                                    'Check exist invoice no on receive_header table 
                                    If objSaleInvoiceSer.IsUsedInHontai(job_order_id, 1) Then
                                        chkHontai1 = chkHontai1 + 1
                                    End If
                                    chkHontai3 = chkHontai3 + 1
                            End Select
                        End If
                    Next
                End If
            End If

            If chkHontai2 > 0 Then
                If chkHontai1 = 0 Then
                    If Session("Mode") = "Add" Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_015"))
                    Else
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_016"))
                    End If
                    Exit Function
                End If
            End If

            If chkHontai3 > 0 Then
                If chkHontai1 = 0 Or chkHontai2 = 0 Then
                    If Session("Mode") = "Add" Then
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_015"))
                    Else
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_06_016"))
                    End If
                    Exit Function
                End If
            End If

            CheckHontaiData = True
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckHontaiData", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: UpdateFlagJobOrderPO
    '	Discription	    : Update flag on job order po 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 22-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function UpdateFlagJobOrderPO() As Boolean
        UpdateFlagJobOrderPO = False
        Try
            ' check state of save item
            UpdateFlagJobOrderPO = objSaleInvoiceSer.UpdateJobOrderPOFlag(Session("dtDeleteFlag"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateFlagJobOrderPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

   

#End Region

End Class
