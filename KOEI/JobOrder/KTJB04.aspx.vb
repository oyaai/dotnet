#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : JobOrder_KTJB04
'	Class Discription	: Webpage for maintenance Special Job Order master
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

Partial Class JobOrder_KTJB04
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objSpecialJobOrderSer As New Service.ImpSpecialJobOrderService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

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
            objLog.StartLog("KTJB04 : section Order Master")

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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            'call function GetSession for get data from search condition
            GetSession()
            Response.Redirect("KTJB03.aspx?New=True")

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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()
            'get data from search condition
            GetSession()
            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTJB04", strConfirmIns, objMessage.GetXMLMessage("KTJB_04_002"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTJB04", strConfirmUpd, objMessage.GetXMLMessage("KTJB_04_005"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
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
            ' call function check permission
            CheckPermission()
            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                ' call function clear session
                ClearSession()
                'Insert Special Job Order
                InsertSpecialJobOrder()
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateSpecialJobOrder()
                Exit Sub
            End If

            'Set QueryString
            If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                Session("job_order_id") = 0
            Else
                Session("job_order_id") = Request.QueryString("id")
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
                txtJobOrder.Enabled = True
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                txtJobOrder.Enabled = False
                LoadInitialUpdate()
            End If 

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objSpecialJobOrderDto As New Dto.SpecialJobOrderDto
            Dim intJobOrderID As Integer = 0

            ' check item id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intJobOrderID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objSpecialJobOrderDto = objSpecialJobOrderSer.GetSpecialJobOrderByID(intJobOrderID)

            ' assign value to control
            With objSpecialJobOrderDto
                txtJobOrder.Text = .job_order
                txtRemark.Text = .remark
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
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
            'clear data on textbox
            If Session("Mode") = "Add" Then
                txtJobOrder.Text = String.Empty
            End If
            txtRemark.Text = String.Empty

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
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objSpecialJobOrderDto As New Dto.SpecialJobOrderDto

            ' assign value to dto object
            With objSpecialJobOrderDto
                .id = Session("job_order_id")
                .job_order = txtJobOrder.Text.Trim
                .remark = txtRemark.Text.Trim
            End With

            ' set dto object to session
            Session("objSpecialJobOrderDto") = objSpecialJobOrderDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objSpecialJobOrderDto As New Dto.SpecialJobOrderDto
            ' set value to dto object from session
            objSpecialJobOrderDto = Session("objSpecialJobOrderDto")

            ' set value to control
            With objSpecialJobOrderDto
                txtJobOrder.Text = .job_order
                txtRemark.Text = .remark
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertSpecialJobOrder
    '	Discription	    : Insert JobOrder
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertSpecialJobOrder()
        Try
            ' call function set value to control
            SetValueToControl()

            'call function CheckDupCountry from service and alert message
            If objSpecialJobOrderSer.CheckDupSpecialJobOrder(Session("job_order_id"), txtJobOrder.Text) Then
                ' call function InsertSpecialJobOrder from service and alert message
                If objSpecialJobOrderSer.InsertSpecialJobOrder(Session("objSpecialJobOrderDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_04_003"), Nothing, "KTJB03.aspx?New=True")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_04_001"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertSpecialJobOrder", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateSpecialJobOrder
    '	Discription	    : Update JobOrder
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 10-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateSpecialJobOrder()
        Try
            ' call function set value to control
            SetValueToControl()
            'call function CheckDupCountry from service and alert message
            If objSpecialJobOrderSer.CheckDupSpecialJobOrder(Session("job_order_id"), txtJobOrder.Text) Then
                ' call function UpdateSpecialJobOrder from service and alert message
                If objSpecialJobOrderSer.UpdateSpecialJobOrder(Session("objSpecialJobOrderDto"), strMsg) Then
                    txtJobOrder.Enabled = False
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_04_006"), Nothing, "KTJB03.aspx?New=True")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_04_008"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateSpecialJobOrder", ex.Message.ToString, Session("UserName"))
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
            Session("job_order_id") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
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
            ' set permission Create
            btnSave.Enabled = objAction.actCreate
            btnClear.Enabled = objAction.actList

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetSession
    '	Discription	    : Get session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetSession()
        Try
            ' clear session before used in this page
            Session("flagAddMod") = Nothing
            Session("flagAddMod") = "1" 'Flag check screen : 1: Menagemant screen , Nothing : Search screen

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
