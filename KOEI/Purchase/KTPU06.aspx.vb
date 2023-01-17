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
'	Class Name		    : KTPU06
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
Partial Class KTPU06
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
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
            objLog.StartLog("KTPU06", Session("UserName"))
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

            setTextToSession()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
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
            Session("txtInvoiceNo1") = txtInvoiceNo.Text.Trim
            Session("rblSearchType1") = rblSearchType.Text.Trim
            Session("txtPO1") = txtPO.Text.Trim
            Session("txtPaymentDateFrom1") = txtPaymentDateFrom.Text.Trim
            Session("txtPaymentDateTo1") = txtPaymentDateTo.Text.Trim
            Session("txtDeliveryDateFrom1") = txtPaymentDateFrom.Text.Trim
            Session("txtDeliveryDateTo1") = txtDeliveryDateTo.Text.Trim
            Session("txtVendor_Name1") = txtVendor_Name.Text.Trim
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
            rblSearchType.Text = Session("rblSearchType1")
            txtPO.Text = Session("txtPO1")
            txtPaymentDateFrom.Text = Session("txtDeliveryDateFrom1")
            txtDeliveryDateTo.Text = Session("txtDeliveryDateTo1")
            txtPaymentDateFrom.Text = Session("txtPaymentDateFrom1")
            txtPaymentDateTo.Text = Session("txtPaymentDateTo1")
            txtVendor_Name.Text = Session("txtVendor_Name1")
            txtInvoiceNo.Text = Session("txtInvoiceNo1")
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
            Session("dtInsertPurchase1") = Nothing
            Session("rblSearchType1") = Nothing
            Session("txtPO1") = Nothing
            Session("txtDeliveryDateFrom1") = Nothing
            Session("txtDeliveryDateTo1") = Nothing
            Session("txtPaymentDateFrom1") = Nothing
            Session("txtPaymentDateTo1") = Nothing
            Session("txtVendor_Name1") = Nothing
            Session("txtInvoiceNo1") = Nothing
            Session("itemConfirm1") = Nothing
            Session("cblConfirmPayment1") = Nothing
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 15-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            ClearSession()
            Response.Redirect("KTPU05.aspx?New=Insert")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
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
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

            ' set permission on button
            If Not Session("actCreate") Then
                btnEdit.CssClass = "icon_rating2 icon_center15"
                btnEdit.Enabled = False
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
                    Response.Redirect("KTPU06_Rating.aspx?Mode=Add&id=" & intItemID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptInquery_Rating_PurchaseCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    ' Stores the Item_ID keys in ViewState
    ReadOnly Property hashItemID() As Hashtable
        Get
            If IsNothing(ViewState("hashItemID1")) Then
                ViewState("hashItemID1") = New Hashtable()
            End If
            Return CType(ViewState("hashItemID1"), Hashtable)
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
            setSessionToText()

            iniMode = objUtility.GetQueryString("New")

            ' check case new enter
            If iniMode = "True" Then
                ' call function clear session
                ClearSession()
                'set default of Type of Purchase
                rblSearchType.SelectedIndex = 0
            ElseIf iniMode = "Insert" Then 'Case come back from insert rating
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
            Dim dtInsertPurchase As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetItemList from ItemService
            dtInsertPurchase = objRatingPurchaseService.GetPurchaseList(Session("objRatingPurchaseDto1"))
            ' set table object to session
            Session("dtInsertPurchase1") = dtInsertPurchase
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
            Session("objRatingPurchaseDto1") = objRatingPurchaseDto

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
            Dim dtInsertPurchase As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtInsertPurchase = Session("dtInsertPurchase1")

            ' check record for display
            If Not IsNothing(dtInsertPurchase) AndAlso dtInsertPurchase.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtInsertPurchase)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptInquery.DataSource = pagedData
                rptInquery.DataBind()

                ' call fucntion set description
                ShowDescription(intPageNo, pagedData.PageCount, dtInsertPurchase.Rows.Count)
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
            btnSearch.Enabled = objAction.actList

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
    
#End Region

End Class
