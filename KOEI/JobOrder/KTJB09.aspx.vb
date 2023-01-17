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
'	Class Name		    : JobOrder_KTJB09
'	Class Discription	: Webpage for Deleted Job Order Management
'	Create User 		: Suwishaya L.
'	Create Date		    : 27-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB09
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

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            'set new session when session is nothing
            If Session("UserID") Is Nothing Then
                If Not Request.QueryString("UserID") Is Nothing Then
                    Session("UserID") = Request.QueryString("UserID") & ""
                    objSetSession.SetSession()
                End If
            End If
            ' write start log
            objLog.StartLog("KTJB09 : Deleted Job Order Management")
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
    '	Create Date	    : 27-06-2013
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
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
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
    '	Create Date	    : 27-06-2013
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
            hashRemark.Clear()

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
    '	Create Date	    : 27-06-2013
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
            Dim strOldJobOrder As String = hashJobOrder(e.Item.ItemIndex).ToString()
            Dim strOldRemark As String = hashRemark(e.Item.ItemIndex).ToString()

            ' set ItemID to session
            Session("intJobOrderID") = intJobOrderID
            Session("strOldJobOrder") = strOldJobOrder
            Session("remark") = strOldRemark & Space(1) & "Old Job Order No. : " & strOldJobOrder

            Select Case e.CommandName
                Case "Restore"
                    ' confirm message to restore
                    objMessage.ConfirmMessage("KTJB09", strResult, objMessage.GetXMLMessage("KTJB_09_001"))

                Case "Detail"
                    'redirect to KTJB01_Detail
                    objMessage.ShowPagePopup("KTJB01_Detail.aspx?id=" & intJobOrderID & "&menuId=6", 900, 950, "", True)
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
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrder_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptJobOrder.ItemDataBound
        Try
            ' object link button
            Dim btnRestore As New LinkButton
            Dim lblAmount As New Label

            ' find linkbutton and assign to variable
            btnRestore = DirectCast(e.Item.FindControl("btnRestore"), LinkButton)
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            'set permission button
            If Not Session("actDelete") Then
                btnRestore.CssClass = "icon_retore2 icon_center15"
                btnRestore.Enabled = False
            End If
            'set permission amount
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
            End If

            ' Set data to hashtable
            hashJobOrderID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashJobOrder.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order"))
            hashRemark.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "remark"))

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

    ' Stores the job order keys in ViewState
    ReadOnly Property hashJobOrder() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrder")) Then
                ViewState("hashJobOrder") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrder"), Hashtable)
        End Get
    End Property

    ' Stores the remark in ViewState
    ReadOnly Property hashRemark() As Hashtable
        Get
            If IsNothing(ViewState("hashRemark")) Then
                ViewState("hashRemark") = New Hashtable()
            End If
            Return CType(ViewState("hashRemark"), Hashtable)
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
            ' call function check permission
            CheckPermission()

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                If objUtility.GetQueryString("Restore") = "True" Then
                    ' call function SearchData
                    SearchData()
                End If
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' call function set user(Person in charge) dropdownlist
            LoadListUser()

            ' call function set job Type dropdownlist
            LoadListJobType()

            ' set search text to session
            SetSessionToItem()

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                ' call function RestoreJobOrder
                RestoreJobOrder()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSessionToItem
    '	Discription	    : Set data to item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSessionToItem()
        Try
            ' set search text to session 
            txtJobOrderFrom.Text = Session("txtJobOrderFrom")
            txtJobOrderTo.Text = Session("txtJobOrderTo")
            ddlJobOrderType.SelectedValue = Session("ddlJobOrderType")
            txtPartNo.Text = Session("txtPartNo")
            txtPartName.Text = Session("txtPartName")
            txtCustomer.Text = Session("txtCustomer")
            ddlPersonInCharge.SelectedValue = Session("ddlPersonInCharge")
            txtIssueDateFrom.Text = Session("txtIssueDateFrom")
            txtIssueDateTo.Text = Session("txtIssueDateTo")
            rbtReceivePO.SelectedValue = Session("rbtReceivePo")
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
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession()
        Try
            'set data from item to session
            Session("txtJobOrderFrom") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderTo") = txtJobOrderTo.Text.Trim
            Session("ddlJobOrderType") = ddlJobOrderType.SelectedValue
            Session("txtPartNo") = txtPartNo.Text.Trim
            Session("txtPartName") = txtPartName.Text.Trim
            Session("txtCustomer") = txtCustomer.Text.Trim
            Session("ddlPersonInCharge") = ddlPersonInCharge.SelectedValue
            Session("rbtReceivePo") = rbtReceivePO.SelectedValue

            If rbtReceivePo.SelectedIndex = -1 Then
                Session("rbtReceivePo") = Nothing
            Else
                Session("rbtReceivePo") = rbtReceivePo.SelectedValue
            End If
            Session("txtIssueDateFrom") = txtIssueDateFrom.Text.Trim
            Session("txtIssueDateTo") = txtIssueDateTo.Text.Trim 
           

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
    '	Function name	: LoadListUser
    '	Discription	    : Load list user function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadListUser()
        Try
            ' object user service
            Dim objUserSer As New Service.ImpUserService
            ' listUserDto for keep value from service
            Dim listUserDto As New List(Of Dto.UserDto)
            ' call function GetUserForList from service
            listUserDto = objUserSer.GetUserForList

            ' call function for bound data with customer dropdownlist
            objUtility.LoadList(ddlPersonInCharge, listUserDto, "UserName", "UserID", True)

            ' set select Vendor from session
            If Not IsNothing(Session("ddlPersonInCharge")) And ddlPersonInCharge.Items.Count > 0 Then
                ddlPersonInCharge.SelectedValue = Session("ddlPersonInCharge")
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
    '	Create Date	    : 27-06-2013
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
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
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

            'Check Issue Date From > Issue Date To
            If txtIssueDateFrom.Text.Trim <> "" And txtIssueDateTo.Text.Trim <> "" Then
                If objIsDate.IsDateFromTo(txtIssueDateFrom.Text.Trim, txtIssueDateTo.Text.Trim) = False Then
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
    '	Create Date	    : 27-06-2013
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
                objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
 
                ' clear binding data and clear description
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
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtJobOrder As New DataTable

            'Set data from condition search into Dto
            SetValueToDto()

            ' call function GetDeleteJobOrderList from JobOrderService
            dtJobOrder = objJobOrderSer.GetDeleteJobOrderList(Session("objJobOrderDto"))
            ' set table object to session
            Session("dtJobOrder") = dtJobOrder
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(6)
            ' set permission Create 
            btnSearch.Enabled = objAction.actList

            ' set action permission to session 
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
    '	Create Date	    : 27-06-2013
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
            Session("ddlPersonInCharge") = Nothing
            Session("txtPartNo") = Nothing
            Session("txtPartName") = Nothing
            Session("ddlJobOrderType") = Nothing
            Session("rbtBoi") = Nothing
            Session("intJobOrderID") = Nothing
            Session("JobOrderNo") = Nothing
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
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Job Order dto object
            Dim objJoborderDto As New Dto.JobOrderDto 
            Dim issueDateDateFrom As String = ""
            Dim issueDateDateTo As String = "" 
            Dim arrIssueDateFrom() As String = Split(txtIssueDateFrom.Text.Trim(), "/")
            Dim arrIssueDateTo() As String = Split(txtIssueDateTo.Text.Trim(), "/") 

            'set data from condition search into dto object
            With objJoborderDto
                'Set Issue date to format yyymmdd
                If UBound(arrIssueDateFrom) > 0 Then
                    issueDateDateFrom = arrIssueDateFrom(2) & arrIssueDateFrom(1) & arrIssueDateFrom(0)
                End If
                If UBound(arrIssueDateTo) > 0 Then
                    issueDateDateTo = arrIssueDateTo(2) & arrIssueDateTo(1) & arrIssueDateTo(0)
                End If

                .job_order_from_search = txtJobOrderFrom.Text.Trim
                .job_order_to_search = txtJobOrderTo.Text.Trim
                .customer_search = txtCustomer.Text.Trim
                .receive_po_search = rbtReceivePo.SelectedValue
                .issue_date_from_search = issueDateDateFrom
                .issue_date_to_search = issueDateDateTo
                .part_name_search = txtPartName.Text.Trim
                .part_no_search = txtPartNo.Text.Trim
                .job_type_search = ddlJobOrderType.SelectedValue
                .boi_search = rbtBoi.SelectedValue
                .person_charge_search = ddlPersonInCharge.SelectedValue

            End With

            ' set dto object to session
            Session("objJoborderDto") = objJoborderDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDtoForRestore
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDtoForRestore()
        Try
            ' Job Order dto object
            Dim objDelJoborderDto As New Dto.JobOrderDto

            'set data from condition search into dto object
            With objDelJoborderDto
                .id = Session("intJobOrderID")
                .job_order = Session("JobOrderNo")
                .old_job_order = Session("strOldJobOrder")
                .remark = Session("remark")
                .job_month = Session("Month")
                .job_year = Session("Year")
                .job_last = Session("JobLast")

            End With

            ' set dto object to session
            Session("objDelJoborderDto") = objDelJoborderDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetDtoForRestore", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: RestoreJobOrder
    '	Discription	    : Restore job order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub RestoreJobOrder()
        Try
            Dim strMsgJobOrder As String = ""
            'Generate new job order no
            GenerateJobOrderNo()
            'set data to dto
            SetDtoForRestore()

            'Set message for show job order no
            strMsgJobOrder = objMessage.GetXMLMessage("KTJB_09_002") & Space(1) & "New Job Order No : " & Session("JobOrderNo")
            ' check state of restore job order
            If objJobOrderSer.RestoreJobOrder(Session("objDelJoborderDto")) Then
                objMessage.AlertMessage(strMsgJobOrder, Nothing, "KTJB09.aspx?New=False&Restore=True&UserID=" & Session("UserID"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_09_003"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("RestoreJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GenerateJobOrderNo
    '	Discription	    : Generate Job Order No
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 27-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GenerateJobOrderNo()
        Try
            ' table object keep value from item service
            Dim dtJobOrderRunning As New DataTable
            Dim strYear As String = ""
            Dim strMonth As String = ""
            Dim runNo As String = ""
            Dim intMonth As Integer
            Dim intYear As Integer
            'set year and month from current date
            intYear = CInt(DateTime.Now.Year.ToString)
            intMonth = CInt(DateTime.Now.Month.ToString)
            
            'set format year to yy and month to mm 
            strYear = Right("0" & CStr(intYear), 2)
            strMonth = Right("0" & CStr(intMonth), 2)

            ' call function GetItemList from ItemService
            dtJobOrderRunning = objJobOrderSer.GetJobOrderRunning(intMonth, intYear)
            'case no data add to new month
            If Not IsNothing(dtJobOrderRunning) AndAlso dtJobOrderRunning.Rows.Count > 0 Then
                runNo = Right("0" & CInt(dtJobOrderRunning.Rows(0).Item("job_last")) + 1, 2)
            Else
                runNo = "01"
            End If

            ' set table object to session
            Session("Year") = intYear
            Session("Month") = intMonth
            Session("JobLast") = runNo
            Session("JobOrderNo") = strYear & strMonth & runNo

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GenerateJobOrderNo", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
