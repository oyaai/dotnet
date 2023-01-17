Imports System.Data
Imports System.Web.Configuration

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Special Job Order
'	Class Name		    : JobOrder_KTJB03
'	Class Discription	: Webpage for Section Order
'	Create User 		: Suwishaya L.
'	Create Date		    : 10-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB03
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objJobOrderSer As New Service.ImpSpecialJobOrderService
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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTJB03 : Section Order")

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
    '	Create Date	    : 10-06-2013
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
                'set initialpage when back from magement screen
                InitialData()
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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSearch.Click
        Try
            Dim objComMsg As New Common.Utilities.Message
            'Check field Special Job Order From, Special Job Order To  
            If CheckSpecialJobOrder() Then
                txtJobOrderFrom.Focus()
                objComMsg.AlertMessage(String.Empty, "KTJB_03_005")
                Exit Sub
            End If
            ' call function search data
            SearchData()
            ' call function display page
            DisplayPage(Request.QueryString("PageNo"))

            ' set search text to session
            Session("txtJobOrderFrom") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderTo") = txtJobOrderTo.Text.Trim

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptSpJobOrder_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptSpJobOrder_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptSpJobOrder.DataBinding
        Try
            ' clear hashtable data
            hashJobOrderID.Clear()
            hashJobOrder.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptSpJobOrder_DataBinding", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptSpJobOrder_ItemCommand
    '	Discription	    : Event repeater item command
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptSpJobOrder_ItemCommand( _
        ByVal source As Object, _
        ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs _
    ) Handles rptSpJobOrder.ItemCommand
        Try
            ' variable for keep data from hashtable
            Dim intJobOrderID As Integer = CInt(hashJobOrderID(e.Item.ItemIndex).ToString())
            Dim strJobOrder As String = hashJobOrder(e.Item.ItemIndex).ToString()
            Dim boolInuse As Boolean = objJobOrderSer.IsUsedInAccounting(strJobOrder)

            ' set ItemID to session
            Session("intJobOrderID") = intJobOrderID
            Session("strJobOrder") = strJobOrder
            Session("boolInuse") = boolInuse

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB03", strResult, objMessage.GetXMLMessage("KTJB_03_001"))

                Case "Edit"
                    'set data search into Session
                    Session("txtJobOrderFromSearch") = txtJobOrderFrom.Text.Trim
                    Session("txtJobOrderToSearch") = txtJobOrderTo.Text.Trim
                    ' redirect to KTMS04
                    Response.Redirect("KTJB04.aspx?Mode=Edit&id=" & intJobOrderID)
            End Select

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptSpJobOrder_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptSpJobOrder_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptSpJobOrder_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptSpJobOrder.ItemDataBound
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

            ' Set Job Order to hashtable
            hashJobOrderID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashJobOrder.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "job_order"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptSpJobOrder_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : Event btnAdd is clicked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnAdd.Click
        Try
            'set data search into Session
            Session("txtJobOrderFromSearch") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderToSearch") = txtJobOrderTo.Text.Trim

            ' redirect to KTMS04 with Add mode
            Response.Redirect("KTJB04.aspx?Mode=Add")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    ' Stores the Job_order keys in ViewState
    ReadOnly Property hashJobOrderID() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrderID")) Then
                ViewState("hashJobOrderID") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrderID"), Hashtable)
        End Get
    End Property

    ' Stores the Job_order keys in ViewState
    ReadOnly Property hashJobOrder() As Hashtable
        Get
            If IsNothing(ViewState("hashJobOrder")) Then
                ViewState("hashJobOrder") = New Hashtable()
            End If
            Return CType(ViewState("hashJobOrder"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try

            ' check case new enter
            If objUtility.GetQueryString("New") = "True" Then
                ' call function clear session
                ClearSession()
            Else
                ' case not new enter then display page with page no
                DisplayPage(Request.QueryString("PageNo"))
            End If

            ' set value to txtJobOrderFrom from session
            txtJobOrderFrom.Text = Session("txtJobOrderFrom")
            ' set value to txtJobOrderTo from session
            txtJobOrderTo.Text = Session("txtJobOrderTo")

            ' call function check permission
            CheckPermission()

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteSpecialJobOrder()
            End If

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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Special Job Order menu
            objAction = objPermission.CheckPermission(2)
            ' set permission button
            btnAdd.Enabled = objAction.actCreate
            btnSearch.Enabled = objAction.actList

            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckSpecialJobOrder
    '	Discription	    : Check field Special Job Order From, Special Job Order To 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckSpecialJobOrder() As Boolean
        Try
            'case job order to have data.
            If txtJobOrderTo.Text.Trim.Length > 0 Then
                'Check job order from can't less than job order to
                If txtJobOrderFrom.Text > txtJobOrderTo.Text Then
                    CheckSpecialJobOrder = True
                Else
                    CheckSpecialJobOrder = False
                End If
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckSpecialJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search Item data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtSpecialJobOrder As New DataTable

            ' call function GetSpecialJobOrderList from Service
            dtSpecialJobOrder = objJobOrderSer.GetSpecialJobOrderList( _
                                                txtJobOrderFrom.Text.Trim, _
                                                txtJobOrderTo.Text.Trim)
            ' set table object to session
            Session("dtSpecialJobOrder") = dtSpecialJobOrder
            'set data search into Session
            Session("txtJobOrderFromSearch") = txtJobOrderFrom.Text.Trim
            Session("txtJobOrderToSearch") = txtJobOrderTo.Text.Trim

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtSpecialJobOrder As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtSpecialJobOrder = Session("dtSpecialJobOrder")

            ' check record for display
            If Not IsNothing(dtSpecialJobOrder) AndAlso dtSpecialJobOrder.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtSpecialJobOrder)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptSpJobOrder.DataSource = pagedData
                rptSpJobOrder.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtSpecialJobOrder.Rows.Count)
            Else
                ' case not exist data
                ' show message box
                If Session("flagAddMod") = "" Or Session("flagAddMod") = Nothing Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_001"))
                End If

                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptSpJobOrder.DataSource = Nothing
                rptSpJobOrder.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteSpecialJobOrder
    '	Discription	    : Delete Special Job Order data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteSpecialJobOrder()
        Try
            ' check flag in_used
            If Session("boolInuse") Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_03_002"))
                Exit Sub
            End If
            ' check state of delete item
            If objJobOrderSer.DeleteSpecialJobOrder(Session("intJobOrderID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_03_003"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_03_004"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteSpecialJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtSpecialJobOrder") = Nothing
            Session("txtJobOrderFrom") = Nothing
            Session("txtJobOrderTo") = Nothing
            Session("intJobOrderID") = Nothing
            Session("strJobOrder") = Nothing
            Session("boolInuse") = Nothing
            Session("actUpdate") = Nothing
            Session("actDelete") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialData
    '	Discription	    : Initial data function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialData()
        Try
            If Session("flagAddMod") = "1" Then
                ' set value to job order from session
                txtJobOrderFrom.Text = Session("txtJobOrderFromSearch")
                txtJobOrderTo.Text = Session("txtJobOrderToSearch")

                ' call function search data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))

                ' set search text to session
                Session("txtJobOrderFrom") = txtJobOrderFrom.Text.Trim
                Session("txtJobOrderTo") = txtJobOrderTo.Text.Trim

                ' call function check permission
                CheckPermission()

                'Clear session
                Session("flagAddMod") = Nothing
                Session("txtJobOrderFromSearch") = Nothing
                Session("txtJobOrderToSearch") = Nothing
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region
    
End Class
