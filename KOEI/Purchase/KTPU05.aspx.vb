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
'	Class Name		    : KTPU05
'	Class Discription	: Searching data of Rating Purchase
'	Create User 		: Pranitda Sroengklang
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
Partial Class KTPU05
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const constDelete As String = "Delete"
    Private Const constConfirm As String = "Confirm"
    Private itemConfirm As String = ""
    Private objValidate As New Common.Validations.Validation

    'connect with service
    Private objRatingPurchaseService As New Service.ImpRating_PurchaseService

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
            objLog.StartLog("KTPU05", Session("UserName"))
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

            ' set search text to session
            Session("txtInvoiceNo") = txtInvoiceNo.Text.Trim
            Session("rblSearchType") = rblSearchType.Text.Trim
            Session("txtPO") = txtPO.Text.Trim
            Session("txtPaymentDateFrom") = txtPaymentDateFrom.Text.Trim
            Session("txtPaymentDateTo") = txtPaymentDateTo.Text.Trim
            Session("txtDeliveryDateFrom") = txtDeliveryDateFrom.Text.Trim
            Session("txtDeliveryDateTo") = txtDeliveryDateTo.Text.Trim
            Session("txtVendor_Name") = txtVendor_Name.Text.Trim
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
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
            hashItemID.Clear()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_Rating_PurchaseDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_Rating_PurchaseDataBound( _
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
            hashItemID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Rating_PurchaseDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: rptInquery_RatingPurchaseCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptInquery_RatingPurchaseCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptInquery.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intItemID As Integer = CInt(hashItemID(e.Item.ItemIndex).ToString())

            ' set ItemID to session
            Session("intItemID") = intItemID

            Select Case e.CommandName
                Case "Edit"
                    ' redirect to KTMS04
                    Response.Redirect("KTPU06_Rating.aspx?Mode=Edit&id=" & intItemID)
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTPU05", constDelete, objMessage.GetXMLMessage("KTPU_05_001"))
                Case Else
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Rating_PurchaseCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Open add screen
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' redirect to KTMS20 with Add mode
            Response.Redirect("KTPU06.aspx?New=True")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnVendorRatingRpt_Click
    '	Discription	    : call export pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnVendorRatingRpt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVendorRatingRpt.Click
        Try
            Dim dtGetVendorRatingReport As New DataTable

            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            'Get data
            GetVendorRatingReport()

            ' get table object from session 
            dtGetVendorRatingReport = Session("dtGetVendorRatingReport")

            If Not IsNothing(dtGetVendorRatingReport) AndAlso dtGetVendorRatingReport.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU06", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnVendorRatingRpt_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnPDFYearly
    '	Discription	    : call export pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnPDFYearly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPDFYearly.Click
        Try
            Dim dtYearGetVendorRatingReport As New DataTable

            'check error
            If CheckError() = False Then
                Exit Sub
            End If

            'Get data
            GetYearVendorRatingReport()

            ' get table object from session 
            dtYearGetVendorRatingReport = Session("dtYearGetVendorRatingReport")

            If Not IsNothing(dtYearGetVendorRatingReport) AndAlso dtYearGetVendorRatingReport.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU06_2", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnPDFYearly", ex.Message.ToString, Session("UserName"))
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
            ' set value to from session to textbox 
            rblSearchType.Text = Session("rblSearchType")
            txtPO.Text = Session("txtPO")
            txtDeliveryDateFrom.Text = Session("txtDeliveryDateFrom")
            txtDeliveryDateTo.Text = Session("txtDeliveryDateTo")
            txtPaymentDateFrom.Text = Session("txtPaymentDateFrom")
            txtPaymentDateTo.Text = Session("txtPaymentDateTo")
            txtVendor_Name.Text = Session("txtVendor_Name")
            txtInvoiceNo.Text = Session("txtInvoiceNo")

            iniMode = objUtility.GetQueryString("New")

            ' check case new enter
            If iniMode = "True" Then
                ' call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0
            ElseIf iniMode = "Update" Or iniMode = "Insert" Then 'Case come back from insert rating
                ' call function search data
                SearchData()

                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
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
            Dim dtRatingPurchase As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtRatingPurchase = objRatingPurchaseService.GetRating_PurchaseList(Session("objRatingPurchaseDto"))
            ' set table object to session
            Session("dtRatingPurchase") = dtRatingPurchase
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetVendorRatingReport
    '	Discription	    : Get Vendor Rating Report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetVendorRatingReport()
        Try
            ' table object keep value from item service
            Dim dtGetVendorRatingReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtGetVendorRatingReport = objRatingPurchaseService.GetVendorRatingReport(Session("objRatingPurchaseDto"))
            ' set table object to session
            Session("dtGetVendorRatingReport") = dtGetVendorRatingReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetVendorRatingReport", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: GetYearVendorRatingReport
    '	Discription	    : Get Year Vendor Rating Report
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetYearVendorRatingReport()
        Try
            ' table object keep value from item service
            Dim dtYearGetVendorRatingReport As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtYearGetVendorRatingReport = objRatingPurchaseService.GetYearVendorRatingReport(Session("objRatingPurchaseDto"))
            ' set table object to session
            Session("dtYearGetVendorRatingReport") = dtYearGetVendorRatingReport
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetYearVendorRatingReport", ex.Message.ToString, Session("UserName"))
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
            Dim intItemID As Integer = 0
            intItemID = Session("intItemID")
            Dim boolInuse As Boolean = False

            ' check state of delete item
            If objRatingPurchaseService.DeleteRatingInvoice(intItemID) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_05_002"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTPU_05_003"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteItem", ex.Message.ToString, Session("UserName"))
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
            ' RatingPurchase dto object
            Dim objRatingPurchaseDto As New Dto.Rating_PurchaseDto
            Dim startDeliveryDate As String = ""
            Dim endDeliveryDate As String = ""
            Dim arrDeliveryStartDate() As String = Split(txtDeliveryDateFrom.Text.Trim(), "/")
            Dim arrDeliveryEndDate() As String = Split(txtDeliveryDateTo.Text.Trim(), "/")

            Dim startPaymentDate As String = ""
            Dim endPaymentDate As String = ""
            Dim arrPaymentStartDate() As String = Split(txtPaymentDateFrom.Text.Trim(), "/")
            Dim arrPaymentEndDate() As String = Split(txtPaymentDateTo.Text.Trim(), "/")

            'set data from condition search into dto object
            With objRatingPurchaseDto
                If UBound(arrDeliveryStartDate) > 0 Then
                    startDeliveryDate = arrDeliveryStartDate(2) & arrDeliveryStartDate(1) & arrDeliveryStartDate(0)
                End If
                If UBound(arrDeliveryEndDate) > 0 Then
                    endDeliveryDate = arrDeliveryEndDate(2) & arrDeliveryEndDate(1) & arrDeliveryEndDate(0)
                End If

                If UBound(arrPaymentStartDate) > 0 Then
                    startPaymentDate = arrPaymentStartDate(2) & arrPaymentStartDate(1) & arrPaymentStartDate(0)
                End If
                If UBound(arrPaymentEndDate) > 0 Then
                    endPaymentDate = arrPaymentEndDate(2) & arrPaymentEndDate(1) & arrPaymentEndDate(0)
                End If

                .strInvoce_no = txtInvoiceNo.Text.Trim
                .strSearchType = rblSearchType.SelectedValue.ToString
                .strPO = txtPO.Text.Trim
                .strPaymentDateFrom = startPaymentDate
                .strPaymentDateTo = endPaymentDate
                .strVendor_name = txtVendor_Name.Text.Trim()
                .strDeliveryDateFrom = startDeliveryDate
                .strDeliveryDateTo = endDeliveryDate
            End With

            ' set dto object to session
            Session("objRatingPurchaseDto") = objRatingPurchaseDto

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
            Dim dtRatingPurchase As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtRatingPurchase = Session("dtRatingPurchase")

            ' check record for display
            If Not IsNothing(dtRatingPurchase) AndAlso dtRatingPurchase.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtRatingPurchase)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtRatingPurchase.Rows.Count)
            Else
                ' case not exist data
                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptInquery.DataSource = Nothing
                rptInquery.DataBind()

                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))

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
            If txtPaymentDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtPaymentDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtPaymentDateTo.Text.Trim <> "" And txtPaymentDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtPaymentDateFrom.Text.Trim) > objValidate.IsDate(txtPaymentDateTo.Text.Trim) Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_005"))
                    Exit Function
                End If
            End If

            'check start date
            If txtDeliveryDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateFrom.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check end date
            If txtDeliveryDateTo.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateTo.Text.Trim) = False Then
                    ' show message box
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'check date startDate >  endDate display "Please verify Date from must <= Date to"
            If txtDeliveryDateTo.Text.Trim <> "" And txtDeliveryDateFrom.Text.Trim <> "" Then
                If objValidate.IsDate(txtDeliveryDateFrom.Text.Trim) > objValidate.IsDate(txtDeliveryDateTo.Text.Trim) Then
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
            objAction = objPermission.CheckPermission(9)

            ' set permission 
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList
            btnVendorRatingRpt.Enabled = objAction.actList
            btnPDFYearly.Enabled = objAction.actList

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
            Session("dtRatingPurchase") = Nothing
            Session("rblSearchType") = Nothing
            Session("txtPO") = Nothing
            Session("txtDeliveryDateFrom") = Nothing
            Session("txtDeliveryDateTo") = Nothing
            Session("txtPaymentDateFrom") = Nothing
            Session("txtPaymentDateTo") = Nothing
            Session("txtVendor_Name") = Nothing
            Session("txtInvoiceNo") = Nothing
            Session("actList") = Nothing
            Session("itemConfirm") = Nothing
            Session("cblConfirmPayment") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: exportPdf
    '	Discription	    : export Pdf
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub exportPdf()
        Try
            Dim dtPaymentPaid As New DataTable

            'Get data
            'SearchDataReport()

            ' get table object from session 
            dtPaymentPaid = Session("dtPaymentPaid")

            If Not IsNothing(dtPaymentPaid) AndAlso dtPaymentPaid.Rows.Count > 0 Then
                objMessage.ShowPagePopup("../Report/ReportViewer.aspx?ReportName=KTPU03", 1000, 990)
            Else
                ' show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("exportPdf", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
