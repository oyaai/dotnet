Imports System.Data
Imports System.Web.Configuration
Imports OfficeOpenXml.Style
Imports OfficeOpenXml
Imports System.IO
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Services.WebMethodAttribute

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Sale Invoice
'	Class Name		    : JobOrder_KTJB05
'	Class Discription	: Webpage for Sale Invoice
'	Create User 		: Suwishaya L.
'	Create Date		    : 09-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB05
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objSaleInvoiceSer As New Service.ImpSale_InvoiceService
    Private objSaleInvoiceVal As New Validations.CommonValidation
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
    Private Const strSave As String = "Save"
    Private Const constConfirm As String = "Confirm"
    Private strEvent As String = ""
    Private itemConfirm As String = ""
    Private conExChangeRate As Boolean = True

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB05 : Sale Invoice")

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
    '	Create Date	    : 09-07-2013
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
                'set event status
                strEvent = "Page_Load"
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
    '	Create Date	    : 25-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnAdd.Click
        Try
            'check user have account approve
            If objSaleInvoiceVal.IsExistAccountApprove Then
                ' redirect to KTJB06 with Add mode
                Response.Redirect("KTJB06.aspx?Mode=Add")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_013"))
                Exit Sub
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : export data to excel file
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnExcel.Click
        Try
            Dim dtSaleInvoiceReport As New DataTable 

            'check error
            If CheckCriteriaInput() = False Then
                Exit Sub
            End If

            'Get data for export excel report
            SearchDataReport()

            ' get table object from session 
            dtSaleInvoiceReport = Session("dtSaleInvoiceReport") 

            If Not IsNothing(dtSaleInvoiceReport) AndAlso dtSaleInvoiceReport.Rows.Count > 0 Then
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
    '	Function name	: btnSearch_Click
    '	Discription	    : Event btnSearch is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
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
            If Session("menuId") = 42 Then 'case link from accounting menu
                DisplayPageAccount(Request.QueryString("PageNo"))
            Else ' case link from job order menu
                DisplayPageJobOrder(Request.QueryString("PageNo"))
            End If

            ' set search text to session
            SetDataToSession()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnConfirmReceive_Click
    '	Discription	    : Event btnConfirmReceive is click
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnConfirmReceive_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnConfirmReceive.Click
        Try
            'Keep data of each record that is already checked
            GetInvoiceID()
            If Session("itemConfirm") = "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_08_009"))
                objMessage.ConfirmMessage(" Input Actual Rate ", constConfirm, objMessage.GetXMLMessage("KTJB_05_005"))
                Exit Sub
            End If
            'call function GetSaleInvoiceTable
            GetSaleInvoiceTable()
            Dim Test As Integer

            objMessage.ShowPagePopup("KTJB05_Exchange.aspx?Id=" & Session("itemConfirm"), 1000, 990, "", True)

            'Test = PopUpActualRate(Session("itemConfirm"))

            'Modify Function By Rawikarn 2014/09/16 
            If Test = "" Then
                Exit Sub
            End If

            ' case not used then confirm message to delete
            objMessage.ConfirmMessage("KTJB05", constConfirm, objMessage.GetXMLMessage("KTJB_05_005"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnConfirmReceive_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceJobOrder_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceJobOrder_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInvoiceJobOrder.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashCurrency.Clear()
            hashReceiptDate.Clear()
            hashStatusId.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceJobOrder_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceAccount_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceAccount_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptInvoiceAccount.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashCurrency.Clear()
            hashReceiptDate.Clear()
            hashStatusId.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceAccount_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceAccount_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceAccount_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInvoiceAccount.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())
            Dim boolInuse As Boolean = objSaleInvoiceSer.IsUsedInAccounting(intID)

            'Get data from textbox on reprater
            Dim txtExchangeRate As New TextBox
            Dim txtActualAmount As New TextBox
            Dim exchangeRate As String
            Dim actualAmount As String
            txtExchangeRate = DirectCast(e.Item.FindControl("txtExchangeRate"), TextBox)
            txtActualAmount = DirectCast(e.Item.FindControl("txtActualAmount"), TextBox)
            exchangeRate = txtExchangeRate.Text
            actualAmount = txtActualAmount.Text

            '=============================================================
            'Update DataTable ///Session("dtSaleInvoice")
            Dim dtUpdate As New DataTable
            ' get table object from session 
            dtUpdate = Session("dtSaleInvoice")

            For Each row As DataRow In dtUpdate.Rows
                If CInt(row(0)) = intID Then
                    row(7) = exchangeRate
                    row(8) = actualAmount
                End If
                row.EndEdit()
                dtUpdate.AcceptChanges()
            Next
            Session("dtSaleInvoice") = dtUpdate
            '=============================================================

            ' set data to session
            Session("intID") = intID
            Session("boolInuse") = boolInuse
            Session("decExchangeRate") = exchangeRate
            Session("decActualAmount") = actualAmount

            Select Case e.CommandName
                Case "Save"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB05", strSave, objMessage.GetXMLMessage("KTJB_05_010"))

                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB05", strResult, objMessage.GetXMLMessage("KTJB_05_001"))

                Case "Edit"
                    'check user have account approve
                    If objSaleInvoiceVal.IsExistAccountApprove Then
                        ' redirect to KTJB02
                        Response.Redirect("KTJB06.aspx?Mode=Edit&id=" & intID)
                    Else
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_014"))
                        Exit Sub
                    End If

                Case "Detail"
                    'redirect to KTJB05_Detail
                    objMessage.ShowPagePopup("KTJB05_Detail.aspx?id=" & intID & "&menuId=42", 900, 950, "", True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceAccount_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceJobOrder_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceJobOrder_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInvoiceJobOrder.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString()) 

            ' set Item ID to session
            Session("intID") = intID 

            Select Case e.CommandName
                Case "Detail"
                    'redirect to KTJB05_Detail
                    objMessage.ShowPagePopup("KTJB05_Detail.aspx?id=" & intID & "&menuId=3", 900, 950, "", True)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceJobOrder_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceAccount_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceAccount_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInvoiceAccount.ItemDataBound
        Try
            'cal function SetRepeaterAcount for set item on Repeater
            SetRepeaterAcount(sender, e)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceAccount_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptInvoiceJobOrder_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInvoiceJobOrder_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptInvoiceJobOrder.ItemDataBound
        Try
            'cal function SetRepeaterJobOrder for set item on Repeater
            SetRepeaterJobOrder(sender, e)
             
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInvoiceJobOrder_ItemDataBound", ex.Message.ToString, Session("UserName"))
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

    ' Stores the Currency in ViewState
    ReadOnly Property hashCurrency() As Hashtable
        Get
            If IsNothing(ViewState("hashCurrency")) Then
                ViewState("hashCurrency") = New Hashtable()
            End If
            Return CType(ViewState("hashCurrency"), Hashtable)
        End Get
    End Property

    ' Stores the Receipt Date in ViewState
    ReadOnly Property hashReceiptDate() As Hashtable
        Get
            If IsNothing(ViewState("hashReceiptDate")) Then
                ViewState("hashReceiptDate") = New Hashtable()
            End If
            Return CType(ViewState("hashReceiptDate"), Hashtable)
        End Get
    End Property

    ' Stores the status id in ViewState
    ReadOnly Property hashStatusId() As Hashtable
        Get
            If IsNothing(ViewState("hashStatusId")) Then
                ViewState("hashStatusId") = New Hashtable()
            End If
            Return CType(ViewState("hashStatusId"), Hashtable)
        End Get
    End Property

    ' Stores the Bank Exchange Rate in ViewState
    ReadOnly Property hashExchangeRate() As Hashtable
        Get
            If IsNothing(ViewState("hashExchangeRate")) Then
                ViewState("hashExchangeRate") = New Hashtable()
            End If
            Return CType(ViewState("hashExchangeRate"), Hashtable)
        End Get
    End Property

    ' Stores the Actual Amount in ViewState
    ReadOnly Property hashActualAmount() As Hashtable
        Get
            If IsNothing(ViewState("hashActualAmount")) Then
                ViewState("hashActualAmount") = New Hashtable()
            End If
            Return CType(ViewState("hashActualAmount"), Hashtable)
        End Get
    End Property

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
            Dim strQueryFlag As String
            'set meni id to session
            If Not String.IsNullOrEmpty(Request.QueryString("MenuId")) Then
                Session("menuId") = CInt(objUtility.GetQueryString("MenuId"))
            End If

            'case from Accounting menu
            If Session("menuId") = 42 Then 
                divAccount.Visible = True
                divTableAccount.Visible = True
                divJobOrder.Visible = False
                divTableJobOrder.Visible = False
            Else 'case come from Job order menu
                divAccount.Visible = False
                divTableAccount.Visible = False
                divJobOrder.Visible = True
                divTableJobOrder.Visible = True
            End If
            'hidden total invoice 
            lblTotalInvoice.Visible = False
            lblTotal.Visible = False
            lblTotalAccount.Visible = False
            lblTotalInvoiceAccount.Visible = False

            ' set search text to session
            SetSessionToItem()

            strQueryFlag = objUtility.GetQueryString("New")
            ' check case new enter
            If strQueryFlag = "True" Then
                ' call function clear session
                ClearSession()
            Else
                If strQueryFlag = "False" Then
                    SearchData()
                End If
                ' case not new enter then display page with page no
                If Session("menuId") = 42 Then
                    DisplayPageAccount(Request.QueryString("PageNo"))
                Else
                    DisplayPageJobOrder(Request.QueryString("PageNo"))
                End If
            End If
            'delete 2013/08/16
            '' set search text to session
            'SetSessionToItem()

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteInvoice()
            End If

            ' check save item
            If objUtility.GetQueryString(strSave) = "True" Then
                SaveInvoice()
            End If

            ' check Confirm Receive  item 
            ' Add New Edit ExChangeRate 
            If objUtility.GetQueryString(constConfirm) = "True" Then
                If NewActualRate(Session("item"), Session("ExchangeRate")) = False Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_015"))
                    Exit Sub
                End If
                ConfirmReceive()
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
            If txtJobOrderInvFrom.Text.Trim.Length > 0 And txtJobOrderInvTo.Text.Trim.Length > 0 Then
                If txtJobOrderInvFrom.Text > txtJobOrderInvTo.Text Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_005"))
                    txtJobOrderInvFrom.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date From
            If txtIssueDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtIssueDateFrom.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field Issue Date To
            If txtIssueDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtIssueDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtIssueDateTo.Focus()
                    Exit Function
                End If
            End If

            'Check Issue Date From > Issue Date To
            If txtIssueDateFrom.Text.Trim <> "" And txtIssueDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtIssueDateFrom.Text.Trim, txtIssueDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    txtIssueDateFrom.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field Receipt Date From
            If txtReceiptDateFrom.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiptDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtReceiptDateFrom.Focus()
                    Exit Function
                End If
            End If

            'Check format date of field Receipt Date To
            If txtReceiptDateTo.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiptDateTo.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    txtReceiptDateTo.Focus()
                    Exit Function
                End If
            End If

            'Check Finished Date From > Finished Date To
            If txtReceiptDateFrom.Text.Trim <> "" And txtReceiptDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtReceiptDateFrom.Text.Trim, txtReceiptDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    txtReceiptDateFrom.Focus()
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
    '	Function name	: DisplayPageAccount
    '	Discription	    : Display page when link accounting menu
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageAccount(ByVal intPageNo As Integer)
        Try
            Dim dtSaleInvoice As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtSaleInvoice = Session("dtSaleInvoice")

            ' check record for display
            If Not IsNothing(dtSaleInvoice) AndAlso dtSaleInvoice.Rows.Count > 0 Then
                ' bound data between pageDate with repeater
                rptInvoiceAccount.DataSource = dtSaleInvoice
                rptInvoiceAccount.DataBind()
                'show Total Sale Invoice Amount
                lblTotalInvoiceAccount.Visible = True
                lblTotalAccount.Visible = True

                ' set permission on amount           
                If Not Session("actAmount") Then
                    lblTotalInvoiceAccount.Text = "******"
                Else
                    lblTotalInvoiceAccount.Text = Session("totalInvoice")
                End If
            Else
                ' case not exist data
                ' show message box         
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                Session("flagAddMod") = Nothing
                ' clear binding data and clear description               
                rptInvoiceAccount.DataSource = Nothing
                rptInvoiceAccount.DataBind()
                lblTotalInvoiceAccount.Visible = False
                lblTotalAccount.Visible = False
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageAccount", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPageJobOrder
    '	Discription	    : Display page when link job order menu
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPageJobOrder(ByVal intPageNo As Integer)
        Try
            Dim dtSaleInvoice As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtSaleInvoice = Session("dtSaleInvoice")

            ' check record for display
            If Not IsNothing(dtSaleInvoice) AndAlso dtSaleInvoice.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtSaleInvoice)
                ' write paging
                lblPaging1.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInvoiceJobOrder.DataSource = pagedData
                rptInvoiceJobOrder.DataBind()
                ' call fucntion set description
                lblDescription1.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtSaleInvoice.Rows.Count)
                'show Total Sale Invoice Amount
                lblTotalInvoice.Visible = True
                lblTotal.Visible = True

                ' set permission on amount           
                If Not Session("actAmount") Then
                    lblTotalInvoice.Text = "******"
                Else
                    lblTotalInvoice.Text = Session("totalInvoice")
                End If
            Else
                ' case not exist data
                ' show message box         
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                Session("flagAddMod") = Nothing
                ' clear binding data and clear description
                lblPaging1.Text = Nothing
                lblDescription1.Text = "&nbsp;"
                rptInvoiceJobOrder.DataSource = Nothing
                rptInvoiceJobOrder.DataBind()
                lblTotalInvoice.Visible = False
                lblTotal.Visible = False
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPageJobOrder", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtSaleInvoice As New DataTable
            Dim objTotalSaleInvoiceDto As New Dto.SaleInvoiceDto

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetSaleInvoiceList from ISale_InvoiceService
            dtSaleInvoice = objSaleInvoiceSer.GetSaleInvoiceList(Session("objSaleInvoiceDto"))
            'cal function GetTotalSaleInvoiceAmount from ISale_InvoiceService
            objTotalSaleInvoiceDto = objSaleInvoiceSer.GetTotalSaleInvoiceAmount(Session("objSaleInvoiceDto"))

            With objTotalSaleInvoiceDto
                Session("totalInvoice") = .total_invoice_amount 
            End With

            ' set table object to session
            Session("dtSaleInvoice") = dtSaleInvoice
            Session("objTotalSaleInvoiceDto") = objTotalSaleInvoiceDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchDataReport
    '	Discription	    : Search Sale Invoice data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchDataReport()
        Try
            ' table object keep value from item service
            Dim dtSaleInvoiceReport As New DataTable 
            Dim objSumSaleInvoiceReportDto As New Dto.SaleInvoiceDto

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetSaleInvoiceReportList from ISale_InvoiceService
            dtSaleInvoiceReport = objSaleInvoiceSer.GetSaleInvoiceReportList(Session("objSaleInvoiceDto"))
            ' call function GetSumSaleInvoiceReportList from ISale_InvoiceService
            objSumSaleInvoiceReportDto = objSaleInvoiceSer.GetSumSaleInvoiceReportList(Session("objSaleInvoiceDto"))

            ' set table object to session
            Session("dtSaleInvoiceReport") = dtSaleInvoiceReport
            Session("objSumSaleInvoiceReportDto") = objSumSaleInvoiceReportDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchDataReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    ' Function name   : ExportExcel
    ' Discription     : Export data to excel
    ' Return Value    : True,False
    ' Create User     : Suwishaya L.
    ' Create Date     : 24-07-2013
    ' Update User     :
    ' Update Date     :
    '*************************************************************/
    Private Function ExportExcel() As Boolean
        ExportExcel = False
        Try
            Dim pck As ExcelPackage = Nothing
            Dim wBook As OfficeOpenXml.ExcelWorksheet = Nothing
            Dim DT As DataTable = Nothing
            Dim objSumReportDto As Dto.SaleInvoiceDto
            Dim sum_price As String
            Dim sum_vat As String
            Dim sum_amount As String
            Dim rowCount As Integer = 3

            pck = New ExcelPackage(New MemoryStream(), New MemoryStream(File.ReadAllBytes(HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("excelReportPath") & "InvoiceReport.xlsx"))))
            wBook = pck.Workbook.Worksheets(1)

            'set header
            wBook.HeaderFooter.OddHeader.RightAlignedText = String.Format("Date : {0} Page : {1}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"), ExcelHeaderFooter.PageNumber)

            DT = Session("dtSaleInvoiceReport")
            objSumReportDto = Session("objSumSaleInvoiceReportDto")

            'Header already have in templete file
            For i As Integer = 0 To DT.Rows.Count - 1
                wBook.Cells(rowCount + 1, 1).Value = DT.Rows(i)("invoice_no").ToString()
                wBook.Cells(rowCount + 1, 2).Value = DT.Rows(i)("issue_date").ToString()
                wBook.Cells(rowCount + 1, 3).Value = DT.Rows(i)("job_order").ToString()
                wBook.Cells(rowCount + 1, 4).Value = DT.Rows(i)("customer").ToString()
                wBook.Cells(rowCount + 1, 5).Value = DT.Rows(i)("po_no").ToString()
                wBook.Cells(rowCount + 1, 6).Value = DT.Rows(i)("stage").ToString()
                wBook.Cells(rowCount + 1, 7).Value = DT.Rows(i)("percent").ToString()
                wBook.Cells(rowCount + 1, 8).Value = DT.Rows(i)("actual_rate").ToString()
                'set amount follow permission      
                If Not Session("actAmount") Then
                    wBook.Cells(rowCount + 1, 9).Value = "******"
                    wBook.Cells(rowCount + 1, 10).Value = "******"
                    wBook.Cells(rowCount + 1, 11).Value = "******"
                Else
                    'wBook.Cells(rowCount + 1, 9).Value = DT.Rows(i)("price").ToString()
                    'wBook.Cells(rowCount + 1, 10).Value = DT.Rows(i)("vat").ToString()
                    'wBook.Cells(rowCount + 1, 11).Value = DT.Rows(i)("amount").ToString()
                    wBook.Cells(rowCount + 1, 9).Value = CDbl(DT.Rows(i)("price"))
                    wBook.Cells(rowCount + 1, 10).Value = CDbl(DT.Rows(i)("vat"))
                    wBook.Cells(rowCount + 1, 11).Value = CDbl(DT.Rows(i)("amount"))

                End If
                
                wBook.Cells(rowCount + 1, 12).Value = DT.Rows(i)("remark").ToString()

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
                wBook.Cells(rowCount + 1, 12).Style.Border.Right.Style = ExcelBorderStyle.Thin
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
                wBook.Cells(rowCount + 1, 12).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

                rowCount += 1
            Next

            'Summary footer report 
            With objSumReportDto
                sum_price = .sum_price
                sum_vat = .sum_vat
                sum_amount = .sum_amount
            End With
            wBook.Cells(rowCount + 1, 8).Value = "Total"
            wBook.Cells(rowCount + 1, 8).Style.Font.Bold = True
            'set amount follow permission      
            If Not Session("actAmount") Then
                wBook.Cells(rowCount + 1, 9).Value = "******"
                wBook.Cells(rowCount + 1, 10).Value = "******"
                wBook.Cells(rowCount + 1, 11).Value = "******"
            Else
                wBook.Cells(rowCount + 1, 9).Value = sum_price
                wBook.Cells(rowCount + 1, 10).Value = sum_vat
                wBook.Cells(rowCount + 1, 11).Value = sum_amount
            End If
            

            'Write border footer
            wBook.Cells(rowCount + 1, 8).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Left.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 8).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 9).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 9).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 10).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 10).Style.Border.Bottom.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 11).Style.Border.Right.Style = ExcelBorderStyle.Thin
            wBook.Cells(rowCount + 1, 11).Style.Border.Bottom.Style = ExcelBorderStyle.Thin

            Response.Clear()
            pck.SaveAs(Response.OutputStream)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=InvoiceReport.xlsx")
            Response.End()

            ExportExcel = True
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ExportExcel", ex.Message.ToString, Session("UserName"))
        End Try

    End Function

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item 
            If Session("menuId") = 3 Then
                objAction = objPermission.CheckPermission(3)
                ' set permission 
                btnAdd.Visible = False
                btnExcel.Visible = False
            Else
                objAction = objPermission.CheckPermission(42)
                ' set permission 
                btnAdd.Visible = True
                btnExcel.Visible = True 

                btnAdd.Enabled = objAction.actCreate
                btnExcel.Enabled = objAction.actList
                btnConfirmReceive.Enabled = objAction.actUpdate
            End If

            btnSearch.Enabled = objAction.actList
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
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtSaleInvoice") = Nothing
            Session("objTotalSaleInvoiceDto") = Nothing
            Session("dtSaleInvoiceReport") = Nothing
            Session("objSumSaleInvoiceReportDto") = Nothing
            Session("txtInvoiceNoSale") = Nothing
            Session("txtIssueDateFrom") = Nothing
            Session("txtIssueDateTo") = Nothing
            Session("txtCustomer") = Nothing
            Session("txtReceiptDateFrom") = Nothing
            Session("txtReceiptDateTo") = Nothing
            Session("rbtInvoiceType") = Nothing
            Session("txtJobOrderInvFrom") = Nothing
            Session("txtJobOrderInvTo") = Nothing
            Session("intID") = Nothing
            Session("decExchangeRate") = Nothing
            Session("decActualAmount") = Nothing
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
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Job Order dto object
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim strJobFinish As String = ""
            Dim issueDateFrom As String = ""
            Dim issueDateTo As String = ""
            Dim receiptDateFrom As String = ""
            Dim receiptDateTo As String = ""
            Dim arrIssueDateFrom() As String = Split(txtIssueDateFrom.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtIssueDateTo.Text.Trim(), "/")
            Dim arrReceiptDateFrom() As String = Split(txtReceiptDateFrom.Text.Trim(), "/")
            Dim arrReceiptDateTo() As String = Split(txtReceiptDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objSaleInvoiceDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If
                'Set finish date to format yyymmdd
                If UBound(arrReceiptDateFrom) > 0 Then
                    receiptDateFrom = arrReceiptDateFrom(2) & arrReceiptDateFrom(1) & arrReceiptDateFrom(0)
                End If
                If UBound(arrReceiptDateTo) > 0 Then
                    receiptDateTo = arrReceiptDateTo(2) & arrReceiptDateTo(1) & arrReceiptDateTo(0)
                End If

                .invoice_no_search = txtInvoiceNoSale.Text.Trim
                .invoice_type_search = rbtInvoiceType.SelectedValue
                .job_order_from_search = txtJobOrderInvFrom.Text.Trim
                .job_order_to_search = txtJobOrderInvTo.Text.Trim
                .customer_search = txtCustomer.Text.Trim
                .issue_date_from_search = issueDateFrom
                .issue_date_to_search = issueDateTo
                .receipt_date_from_search = receiptDateFrom
                .receipt_date_to_search = receiptDateTo

            End With

            ' set dto object to session
            Session("objSaleInvoiceDto") = objSaleInvoiceDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
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
            txtInvoiceNoSale.Text = Session("txtInvoiceNoSale")
            txtIssueDateFrom.Text = Session("txtIssueDateFrom")
            txtIssueDateTo.Text = Session("txtIssueDateTo")
            txtCustomer.Text = Session("txtCustomer")
            txtReceiptDateFrom.Text = Session("txtReceiptDateFrom")
            txtReceiptDateTo.Text = Session("txtReceiptDateTo")
            rbtInvoiceType.SelectedValue = Session("rbtInvoiceType")
            txtJobOrderInvFrom.Text = Session("txtJobOrderInvFrom")
            txtJobOrderInvTo.Text = Session("txtJobOrderInvTo")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSessionToItem", ex.Message.ToString, Session("UserName"))
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
    Private Sub SetDtoToItem(ByVal objDataSearch As Dto.SaleInvoiceDto)
        Try
            'set data from dto to text
            Dim strIssueDateFrom As String = ""
            Dim strIssueDateTo As String = ""
            Dim strReceiptDateFrom As String = ""
            Dim strReceiptDateTo As String = ""
            If objDataSearch.issue_date_from_search <> Nothing Or objDataSearch.issue_date_from_search <> "" Then
                strIssueDateFrom = Right(objDataSearch.issue_date_from_search, 2) & "/" & Mid(objDataSearch.issue_date_from_search, 5, 2) & "/" & Left(objDataSearch.issue_date_from_search, 4)
            End If
            If objDataSearch.issue_date_to_search <> Nothing Or objDataSearch.issue_date_to_search <> "" Then
                strIssueDateTo = Right(objDataSearch.issue_date_to_search, 2) & "/" & Mid(objDataSearch.issue_date_to_search, 5, 2) & "/" & Left(objDataSearch.issue_date_to_search, 4)
            End If
            If objDataSearch.receipt_date_from_search <> Nothing Or objDataSearch.receipt_date_from_search <> "" Then
                strReceiptDateFrom = Right(objDataSearch.receipt_date_from_search, 2) & "/" & Mid(objDataSearch.receipt_date_from_search, 5, 2) & "/" & Left(objDataSearch.receipt_date_from_search, 4)
            End If
            If objDataSearch.receipt_date_to_search <> Nothing Or objDataSearch.receipt_date_to_search <> "" Then
                strReceiptDateTo = Right(objDataSearch.receipt_date_to_search, 2) & "/" & Mid(objDataSearch.receipt_date_to_search, 5, 2) & "/" & Left(objDataSearch.receipt_date_to_search, 4)
            End If

            ' set search text to session 
            txtInvoiceNoSale.Text = objDataSearch.invoice_no_search
            txtIssueDateFrom.Text = strIssueDateFrom
            txtIssueDateTo.Text = strIssueDateTo
            txtCustomer.Text = objDataSearch.customer_search
            txtReceiptDateFrom.Text = strReceiptDateFrom
            txtReceiptDateTo.Text = strReceiptDateTo
            rbtInvoiceType.SelectedValue = objDataSearch.invoice_type_search
            txtJobOrderInvFrom.Text = objDataSearch.job_order_from_search
            txtJobOrderInvTo.Text = objDataSearch.job_order_to_search

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDtoToItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtInvoiceNoSale") = txtInvoiceNoSale.Text.Trim
            Session("txtIssueDateFrom") = txtIssueDateFrom.Text.Trim
            Session("txtIssueDateTo") = txtIssueDateTo.Text.Trim
            Session("txtCustomer") = txtCustomer.Text.Trim
            Session("txtReceiptDateFrom") = txtReceiptDateFrom.Text.Trim
            Session("txtReceiptDateTo") = txtReceiptDateTo.Text.Trim
            Session("rbtInvoiceType") = rbtInvoiceType.SelectedValue
            Session("txtJobOrderInvFrom") = txtJobOrderInvFrom.Text.Trim
            Session("txtJobOrderInvTo") = txtJobOrderInvTo.Text.Trim

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteInvoice
    '	Discription	    : Delete Sale invoice data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteInvoice()
        Try
            ' check flag in_used
            If Session("boolInuse") Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_002"))
                Exit Sub
            End If
            'Get data from receive_detail for update Job_order_po
            Dim dtDelete As New DataTable
            dtDelete = objSaleInvoiceSer.GetDataReceiveDetail(Session("intID"))
            Session("dtDelete") = dtDelete

            ' check state of delete item
            If objSaleInvoiceSer.DeleteSaleInvoice(Session("intID"), Session("dtDelete")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPageAccount(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SaveInvoice
    '	Discription	    : Save Sale invoice data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 25-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SaveInvoice()
        Try
            'convert data to decimal
            Dim decExchangeRate As Decimal
            Dim decActualAmount As Decimal
            If String.IsNullOrEmpty(Session("decExchangeRate")) Then
                decExchangeRate = 0
            Else
                If Session("decExchangeRate").ToString.Split(".").Length > 2 Then
                    decExchangeRate = 0
                Else
                    decExchangeRate = CDec(Session("decExchangeRate"))
                End If
            End If

            If String.IsNullOrEmpty(Session("decActualAmount")) Then
                decActualAmount = 0
            Else
                If Session("decActualAmount").ToString.Split(".").Length > 2 Then
                    decActualAmount = 0
                Else
                    decActualAmount = CDec(Session("decActualAmount"))
                End If
            End If

            ' check Bank Exchange Rate < 0
            If decExchangeRate <= 0 Then
                'show Total Sale Invoice Amount
                lblTotalInvoiceAccount.Visible = True
                lblTotalAccount.Visible = True 
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_008"))
                Exit Sub
            Else
                decExchangeRate.ToString.IndexOf(".")
            End If
            ' check Actual Amount < 0
            If decActualAmount <= 0 Then
                'show Total Sale Invoice Amount
                lblTotalInvoiceAccount.Visible = True
                lblTotalAccount.Visible = True 
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_009"))
                Exit Sub
            End If
            'cal function GetHontaiFinish
            Dim objSaleInvoiceDto As New Dto.SaleInvoiceDto
            Dim strJobOrder As String = ""
            objSaleInvoiceDto = objSaleInvoiceSer.GetHontaiFinish(Session("intID"))
            With objSaleInvoiceDto
                strJobOrder = objSaleInvoiceDto.strJobOrder1
            End With

            ' check state of save item
            If objSaleInvoiceSer.SaveSaleInvoice(Session("intID"), _
                                                 decExchangeRate, _
                                                 decActualAmount, _
                                                 strJobOrder) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_011"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPageAccount(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_012"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SaveInvoice", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: ConfirmReceive
    '	Discription	    : Confirm Receive and Sale invoice 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 25-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ConfirmReceive()
        Try
            ' table object keep value from item service
            Dim dtDataBankFree As New DataTable
            'table object keep value reprot
            Dim dtJobOrderReport As New DataTable
            Dim dtSumVatReport As New DataTable
            Dim dtSumWtReport As New DataTable

            ' call function GetDataBankFreeList from ISale_InvoiceService
            dtDataBankFree = objSaleInvoiceSer.GetDataBankFreeList(Session("itemConfirm"))
            Session("dtValuesBankFree") = dtDataBankFree

            ' check state of save item
            If objSaleInvoiceSer.ConfirmReceive(Session("itemConfirm"), Session("dtValues"), Session("dtValuesBankFree")) Then
                'Modify 2013/09/18 (Add Report process from request from user) Start
                ' case delete success show message box
                'objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_006"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPageAccount(Request.QueryString("PageNo"))
                'Get data for display on Job order income report

                '1.Get detail
                SearchConfirmReceiveReport(Session("itemConfirm"))
                ''2.Get sum vat and sum w/t
                SearchSumDataReport(Session("itemConfirm"))

                '3.Get table object from session 
                dtJobOrderReport = Session("dtJobOrderReport")

                '3.1 Get Bank Fee
                GetReDetailBankFee(Session("itemConfirm"))

                '4.Output report when exist data
                If Not IsNothing(dtJobOrderReport) AndAlso dtJobOrderReport.Rows.Count > 0 Then
                    objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTJB05", 1000, 990)
                Else
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
                End If

            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_05_007"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ConfirmReceive", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchConfirmReceiveReport
    '	Discription	    : Search job order data for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchConfirmReceiveReport(ByVal strReceiveHeaderId As String)
        Try
            ' table object keep value from item service
            Dim dtJobOrderReport As New DataTable

            ' call function GetConfirmReceiveReportList from SaleInvoice Service
            Session("dtJobOrderReport") = Nothing
            dtJobOrderReport = objSaleInvoiceSer.GetConfirmReceiveForReport(strReceiveHeaderId)
            ' set table object to session
            Session("dtJobOrderReport") = dtJobOrderReport

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchConfirmReceiveReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchSumDataReport
    '	Discription	    : Search sum vat and w/t for report
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 18-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchSumDataReport(ByVal strReceiveHeaderId As String)
        Try
            ' table object keep value from item service
            Dim objSumDataReportDto As New Dto.SaleInvoiceDto
            ' call function GetSumVatReportList from SaleInvoice Service
            Session("objSumDataReportDto") = Nothing
            objSumDataReportDto = objSaleInvoiceSer.GetSumConfirmReport(strReceiveHeaderId)
            ' set table object to session
            Session("objSumDataReportDto") = objSumDataReportDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchSumVatReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetInvoiceID
    '	Discription	    : Keep data of each record that is already checked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetInvoiceID()
        Try
            'Keep data of each record that is already checked
            Dim i As Integer = 0
            itemConfirm = String.Empty

            For Each item As RepeaterItem In rptInvoiceAccount.Items
                Dim intID As Integer = CInt(hashID(i))
                Dim chkBox As HtmlInputCheckBox
                chkBox = item.FindControl("chkApprove")
                If chkBox.Checked = True Then
                    itemConfirm &= intID & ","
                End If
                i = i + 1
            Next

            'Set itemConfirm into session
            Session("itemConfirm") = Left(itemConfirm, itemConfirm.Length - 1)

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetInvoiceID", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: GetSaleInvoiceTable
    '	Discription	    : Keep data of each record that is already checked and Actual Amount 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSaleInvoiceTable()
        Try
            ' set data table
            Dim dt As New DataTable
            ' data row object
            Dim row As DataRow
            'Dim chkBox As HtmlInputCheckBox
            'Dim txtExchangeRate As New TextBox
            'Dim txtActualAmount As New TextBox
            ' assign column header
            With dt
                .Columns.Add("receive_header_id")
                .Columns.Add("exchange_rate")
                .Columns.Add("actual_amount")

                For Each item As RepeaterItem In rptInvoiceAccount.Items
                    'Keep data to datatable
                    If DirectCast(item.FindControl("chkApprove"), HtmlInputCheckBox).Checked = True Then
                        row = .NewRow
                        row("receive_header_id") = CInt(hashID(item.ItemIndex))
                        row("exchange_rate") = DirectCast(item.FindControl("txtExchangeRate"), TextBox).Text
                        row("actual_amount") = DirectCast(item.FindControl("txtActualAmount"), TextBox).Text
                        .Rows.Add(row)
                    End If
                Next
            End With
            'Set itemConfirm into session
            Session("dtValues") = dt

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetInvoiceID", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: SetRepeaterAcount
    '	Discription	    : Set item on Repeater
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetRepeaterAcount( _
     ByVal sender As Object, _
     ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

        Try
            Dim objIsDate As New Common.Validations.Validation
            ' object on repaeter
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton
            Dim btnSave As New LinkButton
            Dim txtExchangeRate As New TextBox
            Dim txtActualAmount As New TextBox
            Dim lblInvNo As New Label
            'Dim lblInvType As New Label
            Dim lblIssueDate As New Label
            Dim lblRecDate As New Label
            Dim lblCustomer As New Label
            Dim lblTotalamount As New Label
            Dim chkApprove As HtmlInputCheckBox

            ' find linkbutton and assign to variable
            chkApprove = e.Item.FindControl("chkApprove")
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            btnSave = DirectCast(e.Item.FindControl("btnSave"), LinkButton)
            txtExchangeRate = DirectCast(e.Item.FindControl("txtExchangeRate"), TextBox)
            txtActualAmount = DirectCast(e.Item.FindControl("txtActualAmount"), TextBox)
            lblInvNo = DirectCast(e.Item.FindControl("lblInvNo"), Label)
            'lblInvType = DirectCast(e.Item.FindControl("lblInvType"), Label)
            lblIssueDate = DirectCast(e.Item.FindControl("lblIssueDate"), Label)
            lblRecDate = DirectCast(e.Item.FindControl("lblRecDate"), Label)
            lblCustomer = DirectCast(e.Item.FindControl("lblCustomer"), Label)
            lblTotalamount = DirectCast(e.Item.FindControl("lblTotalamount"), Label)

            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashCurrency.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "currency"))
            hashReceiptDate.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "receipt_date"))
            hashStatusId.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "status_id"))
            Dim strCurrency As String = hashCurrency(e.Item.ItemIndex).ToString()
            Dim strReceiptDate As String = hashReceiptDate(e.Item.ItemIndex).ToString()
            Dim intStatusID As Integer = CInt(hashStatusId(e.Item.ItemIndex).ToString())

            'set item visable for case Normal and Decline
            If intStatusID = 1 Or intStatusID = 5 Then
                btnEdit.Enabled = True
                btnDel.Enabled = True
                btnSave.Enabled = True
                txtExchangeRate.Enabled = True
                txtActualAmount.Enabled = True
                chkApprove.Disabled = False
            Else
                btnEdit.CssClass = "icon_edit2 icon_left"
                btnEdit.Enabled = False
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
                btnSave.CssClass = "icon_save2 icon_center9"
                btnSave.Enabled = False
                txtExchangeRate.Enabled = False
                txtActualAmount.Enabled = False
                chkApprove.Disabled = True
            End If

            'case currency is 'THB',not show TextBox Actual Exchange Rate, Actual Amount and Save button
            If strCurrency.ToUpper = "THB" Then
                txtExchangeRate.Visible = False
                txtActualAmount.Visible = False
                btnSave.Visible = False
            Else
                txtExchangeRate.Visible = True
                txtActualAmount.Visible = True
                btnSave.Visible = True
            End If

            'Check Receipt Date > Now 
            Dim strDateNow As String = Date.Now.ToShortDateString
            Dim dateRec As Date = strReceiptDate
            Dim month As String = Right("0" & dateRec.Month.ToString, 2)
            Dim arrReceiptDate() As String = Split(strReceiptDate, "/")
            Dim receiptDate As String = ""
            If UBound(arrReceiptDate) > 0 Then
                receiptDate = arrReceiptDate(0) & "/" & month & "/" & arrReceiptDate(2)
            End If
            'case Receipt Date > Now , show font is red
            'If objIsDate.IsDateFromTo(receiptDate, strDateNow) = False Then
            'Ping
            'case Now > Receipt Date and status is 1,5 -> show font is red
            If objIsDate.IsDateFromTo(strDateNow, receiptDate) = False AndAlso (intStatusID = 1 Or intStatusID = 5) Then
                txtExchangeRate.CssClass = "font_red tb_Fix100 text_right"
                txtActualAmount.CssClass = "font_red tb_Fix100 text_right"
                lblInvNo.CssClass = "font_red"
                'lblInvType.CssClass = "font_red"
                lblIssueDate.CssClass = "font_red"
                lblRecDate.CssClass = "font_red"
                lblCustomer.CssClass = "font_red"
                lblTotalamount.CssClass = "font_red"
            End If

            ' set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            If Not Session("actDelete") Then
                btnDel.CssClass = "icon_del2 icon_center15"
                btnDel.Enabled = False
            End If

            If Not Session("actUpdate") Then
                btnSave.CssClass = "icon_save2 icon_center15"
                btnSave.Enabled = False
            End If

            ' set permission on amount           
            If Not Session("actAmount") Then
                lblTotalamount.Text = "******"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetRepeaterAcount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetRepeaterJobOrder
    '	Discription	    : Set item on Repeater
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetRepeaterJobOrder( _
     ByVal sender As Object, _
     ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Try
            Dim objIsDate As New Common.Validations.Validation
            ' object on repaeter
            Dim lblInvNo1 As New Label
            'Dim lblInvType1 As New Label
            Dim lblIssueDate1 As New Label
            Dim lblRecDate1 As New Label
            Dim lblCustomer1 As New Label
            Dim lblAmount As New Label

            ' find label and assign to variable 
            lblInvNo1 = DirectCast(e.Item.FindControl("lblInvNo1"), Label)
            'lblInvType1 = DirectCast(e.Item.FindControl("lblInvType1"), Label)
            lblIssueDate1 = DirectCast(e.Item.FindControl("lblIssueDate1"), Label)
            lblRecDate1 = DirectCast(e.Item.FindControl("lblRecDate1"), Label)
            lblCustomer1 = DirectCast(e.Item.FindControl("lblCustomer1"), Label)
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)

            ' Set data to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashReceiptDate.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "receipt_date"))
            hashStatusId.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "status_id"))
            Dim strReceiptDate As String = hashReceiptDate(e.Item.ItemIndex).ToString()
            Dim intStatusID As Integer = CInt(hashStatusId(e.Item.ItemIndex).ToString())

            'Check Receipt Date > Now 
            Dim strDateNow As String = Date.Now.ToShortDateString
            Dim dateRec As Date = strReceiptDate
            Dim month As String = Right("0" & dateRec.Month.ToString, 2)
            Dim arrReceiptDate() As String = Split(strReceiptDate, "/")
            Dim receiptDate As String = ""
            If UBound(arrReceiptDate) > 0 Then
                receiptDate = arrReceiptDate(0) & "/" & month & "/" & arrReceiptDate(2)
            End If
            'case Receipt Date > Now , show font is red
            'If objIsDate.IsDateFromTo(receiptDate, strDateNow) = False Then
            'Ping
            'case Now > Receipt Date and status is 1,5 -> show font is red
            If objIsDate.IsDateFromTo(strDateNow, receiptDate) = False AndAlso (intStatusID = 1 Or intStatusID = 5) Then
                lblInvNo1.CssClass = "font_red"
                'lblInvType1.CssClass = "font_red"
                lblIssueDate1.CssClass = "font_red"
                lblRecDate1.CssClass = "font_red"
                lblCustomer1.CssClass = "font_red"
                lblAmount.CssClass = "font_red"
            End If

            ' set permission on amount           
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetRepeaterJobOrder", ex.Message.ToString, Session("UserName"))
        End Try

    End Sub
    '/**************************************************************
    '	Function name	: GetReDetailBankFee
    '	Discription	    : get Bank Fee for Confirm Report  
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 07-05-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetReDetailBankFee(ByVal strReceiveHeaderId As String)
        Try
            ' table object keep value from item service
            Dim objSumBankFee As New Dto.SaleInvoiceDto
            ' call function GetSumVatReportList from SaleInvoice Service
            Session("objSumBankFee") = Nothing
            objSumBankFee = objSaleInvoiceSer.SumBankfeeConfirmReport(strReceiveHeaderId)
            ' set table object to session
            Session("objSumBankFee") = objSumBankFee

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetReDetailBankFee", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ConfirmActualRate  
    '	Discription	    : Input Confirm Actual Rate  
    '	Return Value	: Integer 
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-06-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function NewActualRate(ByVal strReceiveHeaderId As String, ByVal ExchangeRate As Integer) As Boolean
        Try

            Dim objExChangeRate As Boolean = False
            Session("objExChageRate") = Nothing
            ' Call Service for Modify ExChangeRate 
            objExChangeRate = objSaleInvoiceSer.NewExChangeRate(strReceiveHeaderId, ExchangeRate)
            Session("objExChangeRate") = objExChangeRate


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetReDetailBankFee", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: PopUpActualRate  
    '	Discription	    : Input Confirm Actual Rate  
    '	Return Value	: Integer 
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 06-06-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function PopUpActualRate(ByVal IntID As Integer) As Integer
        Try
            objMessage.ShowPagePopup("KTJB05_Exchange.aspx?ID=" & IntID.ToString, 200, 100)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetReDetailBankFee", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

#End Region


End Class
