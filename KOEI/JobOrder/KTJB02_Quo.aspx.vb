Imports System.Data
Imports System.IO
Imports System.Web.Configuration
Imports System.Web.Services

#Region "History"
'******************************************************************
' Copyright KOEI TOOL (Thailand) co., ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Job Order
'	Class Name		    : JobOrder_KTJB02_Quo
'	Class Discription	: Webpage for Upload Quo
'	Create User 		: Suwishaya L.
'	Create Date		    : 24-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class JobOrder_KTJB02_Quo
    Inherits System.Web.UI.Page

    Private objSetSession As New Utilities.SetSession
    Private objLog As New Common.Logs.Log
    Private objJobOrderSer As New Service.ImpJobOrderService
    Private objUtility As New Common.Utilities.Utility
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objAction As New Common.UserPermissions.ActionPermission
    Private pagedData As New PagedDataSource
    Private objMessage As New Common.Utilities.Message
    Private Const strResult As String = "Result"
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strCancelIns As String = "CancelIns"
    Private Const strEdit As String = "EditQuo"
    Private strMsg As String = String.Empty
    Private strPathConfigQuo As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Quotation/")

#Region "Event"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
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
            objLog.StartLog("KTJB02_Quo : Job Order")

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
    '	Create Date	    : 24-06-2013
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
    '	Function name	: btnUpload_Click
    '	Discription	    : Event btnUpload is clicked
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnUpload_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnUpload.Click
        Try
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            'Check input Criteria data
            If CheckCriteriaInput() = False Then
                Exit Sub
            Else
                ' call function set session dto
                SetValueToDto()
                'check mode
                If FileAttach.HasFile Then
                    If Session("Mode") = "Add" Then
                        strFolderName = hidIpAddress.Value
                    ElseIf Session("Mode") = "Edit" Then
                        strFolderName = Session("job_order")
                    End If
                    'set path
                    strPath = strPathConfigQuo & strFolderName
                    'check exist path
                    If (Not System.IO.Directory.Exists(strPath)) Then
                        Directory.CreateDirectory(strPath)
                    End If
                    'Upload file to server
                    Session("PathFolderUpload") = strPath & "/" & Session("fileName")
                    If File.Exists(Session("PathFolderUpload")) Then
                        My.Computer.FileSystem.DeleteFile(Session("PathFolderUpload"))
                    End If
                    'save as file to server
                    FileAttach.SaveAs(strPath & "/" & Session("fileName"))

                Else
                    ' show message box when Browse file not succuss
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_011"))
                    Exit Sub
                End If


                If Session("Mode") = "Add" Then
                    ConfirmMessage("KTJB02_Quo", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_Quo_001"))
                Else
                    If Session("quo_id") <> "" Then
                        ConfirmMessage("KTJB02_Quo", strEdit, strCancelIns, objMessage.GetXMLMessage("KTJB_01_Quo_012"))
                    Else
                        ConfirmMessage("KTJB02_Quo", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_Quo_001"))
                    End If
                End If


            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnUpload_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderQuo_DataBinding
    '	Discription	    : Event repeater binding data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptJobOrderQuo_DataBinding( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rptJobOrderQuo.DataBinding
        Try
            ' clear hashtable data
            hashID.Clear()
            hashFile.Clear()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderQuo_DataBinding", ex.Message.ToString, Session("UserName"))
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
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())
            'CheckOut By Rawikarn K. 2014/07/07
            'When Delete Don't Check Issue Sale Invoice 
            ' Dim boolInuse As Boolean = objJobOrderSer.CheckExistReceiveDetail(Session("job_order_id"))
            Dim strFile As String = hashFile(e.Item.ItemIndex).ToString()
            Dim boolInuseJobPO As Boolean = objJobOrderSer.IsUsedInJobOrderPo(Session("job_order_id"))

            ' set ItemID to session
            Session("intID") = intID
            'CheckOut By Rawikarn K. 2014/07/07
            ' Session("boolInuse") = boolInuse
            Session("strFile") = strFile
            Session("boolInuseJobPO") = boolInuseJobPO

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB02_Quo", strResult, objMessage.GetXMLMessage("KTJB_02_Quo_004"))
                Case "EditQuo"
                    Response.Redirect("KTJB02_Quo.aspx?Mode=Edit&SelfMode=EditQuo&job_id=" & Session("job_order_id") & "&quo_id=" & intID)
            End Select
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderQuo_ItemCommand", ex.Message.ToString, Session("UserName"))
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
            ' object link button
            Dim btnDel As New LinkButton
            Dim lblAmount As New Label
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)


            'set permission button Modify can Delete everytime 20140228
            'If Not Session("actDelete") Then
            '    btnDel.CssClass = "icon_del2 icon_center15"
            '    btnDel.Enabled = False
            'End If
            'set permission amount item
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
            End If

            ' Set id to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashFile.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "quo_file"))

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderQuo_ItemDataBound", ex.Message.ToString, Session("UserName"))
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

    ' Stores the po file in ViewState
    ReadOnly Property hashFile() As Hashtable
        Get
            If IsNothing(ViewState("hashFile")) Then
                ViewState("hashFile") = New Hashtable()
            End If
            Return CType(ViewState("hashFile"), Hashtable)
        End Get
    End Property

#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            'get ip address
            GetIpAddress()

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"

            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                'Set data to session from QueryString
                If Not String.IsNullOrEmpty(Request.QueryString("id")) And Request.QueryString("id") Is Nothing Then
                    Session("job_order_id") = 0
                Else
                    Session("job_order_id") = Request.QueryString("id")
                End If

                If Not String.IsNullOrEmpty(Request.QueryString("job_order")) And Request.QueryString("job_order") Is Nothing Then
                    Session("job_order") = String.Empty
                Else
                    Session("job_order") = Request.QueryString("job_order")
                End If


                If Not String.IsNullOrEmpty(Request.QueryString("SelfMode")) And Request.QueryString("SelfMode") Is Nothing Then

                    Session("SelfMode") = String.Empty

                ElseIf Not Request.QueryString("SelfMode") Is Nothing Then
                    Session("quo_id") = Request.QueryString("quo_id")
                    Session("SelfMode") = Request.QueryString("SelfMode")
                    SelectQuotoUpdate()

                    Exit Sub
                End If

            End If

                ' call function check permission
                CheckPermission()
                ' call function check job order po
                CheckUseInJobOrderPO()

                ' call function search data
                SearchData()

                ' check case new enter
                If objUtility.GetQueryString("New") = "True" Then
                    ' call function clear session
                    ClearSession()
                Else
                    ' case not new enter then display page with page no
                    DisplayPage(Request.QueryString("PageNo"))
                End If

                ' set search text to session
                rbtQuoType.SelectedValue = Session("rbtQuoType")
                txtQuoNo.Text = Session("txtQuoNo")
                'txtQuoAmount.Text = Session("txtQuoAmount")
                txtQuoDate.Text = Session("txtQuoDate")

                'Delete 2013/09/23
                '' call function check permission
                'CheckPermission()
                '' call function check job order po
                'CheckUseInJobOrderPO()

                ' check insert item
                If objUtility.GetQueryString(strConfirmIns) = "True" Then
                    ' call function clear session
                    ClearSession()
                    'Insert Job Order
                    InsertJobOrderQuoTemp()
                    Exit Sub
                End If

                ' Case cancel when upload Quotation
                If objUtility.GetQueryString(strCancelIns) = "True" Then
                    DeleteFileUpload()
                    Exit Sub
                End If

                ' check delete item
                If objUtility.GetQueryString(strResult) = "True" Then
                    DeleteJobOrderQuoTemp()
                End If


                If objUtility.GetQueryString(strEdit) = "True" Then
                    ClearSession()
                    UpdateQuoToTmp()
                End If


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetIpAddress
    '	Discription	    : Get ip address
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
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
    '	Function name	: ClearSession
    '	Discription	    : Clear session
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            ' clase all session used in this page
            Session("dtJobOrderQuo") = Nothing
            Session("rbtQuoType") = Nothing
            Session("txtQuoNo") = Nothing
            Session("txtQuoAmount") = Nothing
            Session("txtQuoDate") = Nothing 
            Session("intID") = Nothing 
            Session("boolInuse") = Nothing
            Session("boolInuseJobPO") = Nothing
            Session("actDelete") = Nothing
            Session("actAmount") = Nothing

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearSession", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearItem
    '	Discription	    : Clear data on item
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearItem()
        Try
            ' clase all item used in this page
            rbtQuoType.SelectedValue = String.Empty
            txtQuoNo.Text = String.Empty
            'txtQuoAmount.Text = String.Empty
            txtQuoDate.Text = String.Empty 

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearItem", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DisplayPage
    '	Discription	    : Display page
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DisplayPage(ByVal intPageNo As Integer)
        Try
            Dim dtJobOrderQuo As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtJobOrderQuo = Session("dtJobOrderQuo")

            ' check record for display
            If Not IsNothing(dtJobOrderQuo) AndAlso dtJobOrderQuo.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrderQuo)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptJobOrderQuo.DataSource = pagedData
                rptJobOrderQuo.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrderQuo.Rows.Count)
            Else
                ' case not exist data
                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptJobOrderQuo.DataSource = Nothing
                rptJobOrderQuo.DataBind()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DisplayPage", ex.Message.ToString, Session("UserName"))
        Finally
            objUtility.RemQueryString("PageNo")
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
            ' check permission of Item menu
            objAction = objPermission.CheckPermission(1)
            ' set permission Create
            btnUpload.Enabled = objAction.actCreate

            ' set action permission to session
            Session("actDelete") = objAction.actDelete
            Session("actAmount") = objAction.actAmount

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUseInJobOrderPO
    '	Discription	    : Check job order use in job order po
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 08-08-2013
    '	Update User	    : Rawikarn K.  
    '	Update Date	    : 
    '*************************************************************/
    Private Sub CheckUseInJobOrderPO()
        Try
            Session("boolInuseJobPO") = False
            'call function IsUsedInJobOrderPo from jobOrderService
            Dim boolInuseJobPO As Boolean = objJobOrderSer.IsUsedInJobOrderPo(Session("job_order_id"))
            Session("boolInuseJobPO") = boolInuseJobPO
            'set permission 
            If Session("boolInuseJobPO") Then
                btnUpload.Enabled = True
                Session("actDelete") = True
            Else
                btnUpload.Enabled = True
                Session("actDelete") = True
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckUseInJobOrderPO", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search job order quo temp data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtJobOrderQuo As New DataTable
            ' call function GetJobOrderQuoTempList from JobOrderService
            dtJobOrderQuo = objJobOrderSer.GetJobOrderQuoTempList(hidIpAddress.Value)
            ' set table object to session
            Session("dtJobOrderQuo") = dtJobOrderQuo
            'cal function calculate quotation amount
            GetQuotationAmount()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteJobOrderQuoTemp
    '	Discription	    : Delete job order quo temp data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteJobOrderQuoTemp()
        Try
            'CheckOut By Rawikarn K. 2014/07/07
            ' check flag in_used
            'If Session("boolInuse") = False Then
            '    ' case in_used then alert message
            '    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_Quo_005"))
            '    Exit Sub
            'End If
            ' check state of delete item
            If objJobOrderSer.DeleteQuoTemp(Session("intID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_Quo_006"))
                'delete file upload
                DeleteFile(Session("strFile"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_Quo_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteJobOrderQuoTemp", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckCriteriaInput
    '	Discription	    : Check Criteria input data 
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function CheckCriteriaInput() As Boolean
        Try
            CheckCriteriaInput = False
            Dim objIsDate As New Common.Validations.Validation

            If Session("quo_id") Is Nothing And FileAttach.HasFile = False Then

                'Check FileAttach
                If FileAttach.HasFile Then
                    'Check Length of file attach
                    If FileAttach.PostedFile.ContentLength > 4194304 Then
                        ' show message box when file > 4MB
                        objMessage.AlertMessage(objMessage.GetXMLMessage("Common_003"))
                        Exit Function
                    Else
                        Dim filename As String = FileAttach.FileName
                        Session("fileName") = filename
                    End If
                Else
                    ' show message box when Browse file not succuss
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_011"))
                    Exit Function
                End If
            Else
                If FileAttach.HasFile = False Then
                    Dim filename As String = Session("quo_file")
                    Session("fileName") = filename
                Else
                    'Check FileAttach
                    If FileAttach.HasFile Then
                        'Check Length of file attach
                        If FileAttach.PostedFile.ContentLength > 4194304 Then
                            ' show message box when file > 4MB
                            objMessage.AlertMessage(objMessage.GetXMLMessage("Common_003"))
                            Exit Function
                        Else
                            Dim filename As String = FileAttach.FileName
                            Session("fileName") = filename
                        End If
                    Else
                        ' show message box when Browse file not succuss
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_011"))
                        Exit Function
                    End If
                End If

            End If


            'Check format date of field quotation Date From
            If txtQuoDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtQuoDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check exist quo no on job order quo 
            Dim strJob_order_id As String
            If Session("Mode") = "Add" Then
                strJob_order_id = ""
            Else
                strJob_order_id = Session("job_order_id")
            End If


            If Session("quo_id") Is Nothing Then
                If objJobOrderSer.CheckExistQuoNo(txtQuoNo.Text.Trim, strJob_order_id) = False Then
                    ' show message box when quo no > 0
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_009"))
                    Exit Function
                End If


                'Check exist quo no on job order quo temp
                If objJobOrderSer.CheckExistQuoNoTemp(txtQuoNo.Text.Trim) = False Then
                    ' show message box when quo no > 0
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_009"))
                    Exit Function
                End If


            End If


            'Check exist quo file on job order quo temp
            If objJobOrderSer.CheckExistQuoFile(Session("fileName"), hidIpAddress.Value) = False Then
                ' show message box when quo no > 0
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_010"))
                Exit Function
            End If

            CheckCriteriaInput = True

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckCriteriaInput", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' Item dto object
            Dim objJobOrderDtoQuo As New Dto.JobOrderDto
            Dim strQuoDate As String = ""

            'Replace date dd/mm/yyyy to array
            Dim arrQuoDate() As String = Split(txtQuoDate.Text.Trim(), "/")

            'set quo date to yyyymmdd format
            If UBound(arrQuoDate) > 0 Then
                strQuoDate = arrQuoDate(2) & arrQuoDate(1) & arrQuoDate(0)
            End If

            ' fix QuoAmount 
            Dim QuoAmount As String = "0.00"
            'If txtQuoAmount.Text = String.Empty Then
            '    QuoAmount = "0.00"
            'Else
            '    QuoAmount = txtQuoAmount.Text
            'End If

            ' assign value to dto object
            With objJobOrderDtoQuo
                .id = Session("job_order_id")
                .quo_type = rbtQuoType.SelectedValue
                .quo_no = txtQuoNo.Text
                .quo_amount = QuoAmount.Replace(",", "")
                .quo_date = strQuoDate
                .quo_file = Session("fileName")
                .ip_address = hidIpAddress.Value

            End With

            ' set dto object to session
            Session("objJobOrderDtoQuo") = objJobOrderDtoQuo

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertJobOrderQuoTemp
    '	Discription	    : Insert Job Order quo temp
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertJobOrderQuoTemp()
        Try
            'Delete 2013/08/16
            'call function upload file
            'If UploadFileQuo(Session("FileAttach")) = False Then
            '    Exit Sub
            'End If

            ' call function set value to control
            SetValueToControl()

            ' call function InsertQuoTemp from service and alert message
            If objJobOrderSer.InsertQuoTemp(Session("objJobOrderDtoQuo"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_Quo_002"))
                'Check Permission
                CheckPermission()
                'Check job order Quotation
                CheckUseInJobOrderPO()
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
                'Cleat Item
                ClearItem()
            Else
                'delete file upload
                DeleteFileUpload()
                'alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertJobOrderQuoTemp", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objJobOrderDtoQuo As New Dto.JobOrderDto
            Dim strQuoDate As String = ""
            ' set value to dto object from session
            objJobOrderDtoQuo = Session("objJobOrderDtoQuo")

            ' set value to control
            With objJobOrderDtoQuo
                rbtQuoType.SelectedValue = .quo_type
                txtQuoNo.Text = .quo_no
                'txtQuoAmount.Text = Format(.quo_amount, "#,##0.00")
                'set format date to dd/mm/yyyy to text
                If .quo_date.ToString.Trim.Length > 0 Then
                    strQuoDate = Left(.quo_date, 4) & "/" & Mid(.quo_date, 5, 2) & "/" & Right(.quo_date, 2)
                    txtQuoDate.Text = CDate(strQuoDate).ToString("dd/MMM/yyyy")
                Else
                    txtQuoDate.Text = .quo_date
                End If

            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UploadFileQuo
    '	Discription	    : Upload file quo to server
    '	Return Value	: Boolean
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function UploadFileQuo(ByVal FileAttach As FileUpload) As Boolean
        Try
            UploadFileQuo = False
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            If FileAttach.HasFile Then
                If Session("Mode") = "Add" Then
                    strFolderName = hidIpAddress.Value
                ElseIf Session("Mode") = "Edit" Then
                    strFolderName = Session("job_order")
                End If
                strPath = strPathConfigQuo & strFolderName
                'check exist path
                If (Not System.IO.Directory.Exists(strPath)) Then
                    Directory.CreateDirectory(strPath)
                End If
                'Upload file to server
                Session("PathFolderUpload") = strPath & "/" & Session("fileName")
                If File.Exists(Session("PathFolderUpload")) Then
                    File.Delete(Session("PathFolderUpload"))
                End If
                FileAttach.SaveAs(strPath & "/" & Session("fileName"))

            Else
                ' show message box when Browse file not succuss
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_Quo_011"))
                Exit Function
            End If

            UploadFileQuo = True
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UploadFileQuo", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: DeleteFileUpload
    '	Discription	    : Delete Job Order quo file
    '	Return Value	: Integer
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub DeleteFileUpload()
        Try
            'check exist file job order po
            If File.Exists(Session("PathFolderUpload")) Then
                'delete file
                My.Computer.FileSystem.DeleteFile(Session("PathFolderUpload"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteFileUpload(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: DeleteFile
    '	Discription	    : Delete Job Order po file
    '	Return Value	: Integer
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Public Sub DeleteFile(ByVal strFileName As String)
        Try
            Dim strFolderName As String = ""
            Dim strPath As String = ""
            If Session("Mode") = "Add" Then
                strFolderName = hidIpAddress.Value
            ElseIf Session("Mode") = "Edit" Then
                strFolderName = Session("job_order")
            End If
            strPath = strPathConfigQuo & strFolderName & "/" & strFileName

            'check exist file job order po
            If File.Exists(strPath) Then
                'delete file
                My.Computer.FileSystem.DeleteFile(strPath)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: GetQuotationAmount
    '	Discription	    : Get Total Summary Quotation Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub GetQuotationAmount()
        Try
            ' table object keep value from item service
            Dim dtQuoAmount As New DataTable

            ' call function GetTotalAmount from ItemService
            dtQuoAmount = objJobOrderSer.GetSumQuoAmount(hidIpAddress.Value)
            ' set table object to session
            If Not IsNothing(dtQuoAmount) AndAlso dtQuoAmount.Rows.Count > 0 Then
                Session("sum_quo_amount") = dtQuoAmount.Rows(0).Item("sum_quo_amount").ToString
            Else
                Session("sum_quo_amount") = 0.0
            End If

            ' set session to text
            hidSumQuoAmount.Value = Format(Convert.ToDouble(Session("sum_quo_amount")), "#,##0.00")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetQuotationAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ConfirmMessage(strPageName, strResultName, strMesage)
    '	Discription		: Show Confirm Message
    '	Return Value	: 
    '	Create User		: Boon  Cr.P'Lin
    '	Create Date		: 14-03-2013
    '	Update User		: Boon
    '	Update Date		: 21-05-2013
    '**************************************************************/
    Public Sub ConfirmMessage( _
            ByVal strPageName As String, _
            ByVal strResultName As String, _
            ByVal strResultNameCancel As String, _
            ByVal strMessage As String)

        Try
            Dim Page As New Web.UI.Page
            'Gets the executing web page 
            Page = HttpContext.Current.CurrentHandler

            Dim sb As New System.Text.StringBuilder()
            sb.Append("<script type = 'text/javascript'>")
            sb.Append("window.onload=function(){")
            sb.Append("if (confirm('")
            sb.Append(strMessage)
            sb.Append("')){ ")
            sb.Append("window.location = '" & strPageName & ".aspx?" & strResultName & "=True';")
            sb.Append("}")
            sb.Append("else{")
            sb.Append("window.location = '" & strPageName & ".aspx?" & strResultNameCancel & "=True';")
            sb.Append("}};</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "confirm", sb.ToString())
        Catch ex As Exception
            objLog.ErrorLog("ConfirmMessage", ex.Message.ToString)
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SelectQuotoUpdate()
    '	Discription		: Select Quotation in textbox
    '	Return Value	: 
    '	Create User		: Rawikarn k.
    '	Create Date		: 28-02-2014
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Sub SelectQuotoUpdate()
        Try
            ' table object keep value from item service
            Dim dtJobOrderQuo As New DataTable
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            ' call function GetJobOrderQuoTempList from JobOrderService
            dtJobOrderQuo = objJobOrderSer.GetOneQuoFromTmp(Session("quo_id"))

            Dim i As Integer
            i = dtJobOrderQuo.Rows.Count

            If Session("Mode") = "Add" Then
                strFolderName = hidIpAddress.Value
            ElseIf Session("Mode") = "Edit" Then
                strFolderName = Session("job_order")
            End If
            'set path file
            strPath = strPathConfigQuo & strFolderName

            If Session("Mode") = "Edit" Then
                rbtQuoType.Enabled = False
                txtQuoNo.Enabled = False
            End If


            ' set data to field
            With dtJobOrderQuo
                rbtQuoType.SelectedIndex = dtJobOrderQuo.Rows(0).Item("quo_type").ToString()
                txtQuoNo.Text = dtJobOrderQuo.Rows(0).Item("quo_no").ToString()
                txtQuoDate.Text = CDate(dtJobOrderQuo.Rows(0).Item("quo_date")).ToString("dd/MM/yyyy")
                FileAttach.Attributes.Add("value", dtJobOrderQuo.Rows(0).Item("quo_file").ToString)
                lblFileAttach.Text = strPath & dtJobOrderQuo.Rows(0).Item("quo_file").ToString

            End With

            ' set table object to session
            Session("dtJobOrderQuo") = dtJobOrderQuo
            Session("quo_file") = dtJobOrderQuo.Rows(0).Item("quo_file").ToString
            'cal function calculate quotation amount
            GetQuotationAmount()

        Catch ex As Exception
            objLog.ErrorLog("SelectQuotoUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateQuoToTmp()
    '	Discription		: Update Quotation
    '	Return Value	: 
    '	Create User		: Rawikarn k.
    '	Create Date		: 28-02-2014
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Protected Sub UpdateQuoToTmp()
        Try
            SetValueToControl()

            ' call function InsertQuoTemp from service and alert message
            If objJobOrderSer.UpdateQuotationToTmp(Session("objJobOrderDtoQuo"), Session("quo_id"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_Quo_002"))
                'Check Permission
                CheckPermission()
                'Check job order Quotation
                CheckUseInJobOrderPO()
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
                'Cleat Item
                ClearItem()
            Else
                'delete file upload
                DeleteFileUpload()
                'alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If

        Catch ex As Exception
            objLog.ErrorLog("UpdateQuoToTmp", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
