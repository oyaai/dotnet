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
'	Class Name		    : JobOrder_KTJB02_PO
'	Class Discription	: Webpage for Upload PO
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

Partial Class JobOrder_KTJB02_PO
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
    Private strMsg As String = String.Empty
    Private strPathConfigPO As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "PO/")
    Private Const strEditPo As String = "EditPO"

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
            objLog.StartLog("KTJB02_PO : Job Order")

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

                If Session("po_id") Is Nothing Then
                    'Get folder name
                    If FileAttach.HasFile Then
                        If Session("Mode") = "Add" Then
                            strFolderName = hidIpAddress.Value
                        ElseIf Session("Mode") = "Edit" Then
                            strFolderName = Session("job_order")
                        End If

                        'set path
                        strPath = strPathConfigPO & strFolderName
                        'check exist path
                        If (Not System.IO.Directory.Exists(strPath)) Then
                            Directory.CreateDirectory(strPath)
                        End If
                        'Upload file to server
                        Session("PathFolderUpload") = strPath & "/" & Session("fileName")

                        'check exist file job order po
                        If File.Exists(Session("PathFolderUpload")) Then
                            My.Computer.FileSystem.DeleteFile(Session("PathFolderUpload"))
                        End If
                        'save as file to folder on server
                        FileAttach.SaveAs(strPath & "/" & Session("fileName"))

                    Else
                        ' show message box when Browse file not succuss
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_011"))
                        Exit Sub
                    End If

                End If

                If Session("Mode") = "Add" Then
                    ConfirmMessage("KTJB02_PO", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_001"))
                Else
                    If Session("po_id") <> "" Then
                        ConfirmMessage("KTJB02_PO", strEditPo, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_013"))
                    Else
                        ConfirmMessage("KTJB02_PO", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_001"))
                    End If
                End If
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnUpload_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rbtPoType_SelectedIndexChanged
    '	Discription	    : Event rbtPoType is SelectedIndexChanged
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 11-02-2013
    '*************************************************************/
    Protected Sub rbtPoType_SelectedIndexChanged( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles rbtPoType.SelectedIndexChanged
        Try
            'when select PO Type is "Hotei" not check req
            If rbtPoType.SelectedValue = "0" Then
                txtPOAmount.Enabled = False
                reqPOAmount.Visible = False
                lblChkReq.Visible = False
                reqValidatorReceiptDate.Visible = False
                lblReqPOAmount.Visible = False

                If Not Request.QueryString("HontaiAmount") Is Nothing Then
                    txtPOAmount.Text = Request.QueryString("HontaiAmount")
                End If

            Else
                lblChkReq.Visible = True
                ' set ReqPOAmount by Aey
                reqPOAmount.Visible = True
                reqValidatorReceiptDate.Visible = True
                lblReqPOAmount.Visible = True
                txtPOAmount.Enabled = True
                txtPOAmount.Text = String.Empty

            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rbtPoType_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderPO_DataBinding
    '	Discription	    : Event repeater binding data
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
            hashID.Clear()
            hashFile.Clear()

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
            Dim intID As Integer = CInt(hashID(e.Item.ItemIndex).ToString())
            Dim strFile As String = hashFile(e.Item.ItemIndex).ToString()
            Dim boolInuse As Boolean = objJobOrderSer.CheckExistPoTemp(intID, hidIpAddress.Value)
            Dim boolInuseJobPO As Boolean = objJobOrderSer.IsUsedInJobOrderPo(Session("job_order_id"))
            Dim po_id As Integer = CInt(hashID(e.Item.ItemIndex).ToString())

            ' set job order id to session
            Session("intID") = intID
            Session("boolInuse") = boolInuse
            Session("strFile") = strFile
            Session("boolInuseJobPO") = boolInuseJobPO
            Session("po_id") = po_id

            Select Case e.CommandName
                Case "Delete"
                    ' case not used then confirm message to delete
                    objMessage.ConfirmMessage("KTJB02_PO", strResult, objMessage.GetXMLMessage("KTJB_02_PO_004"))
                Case "Edit_PO"
                    ' redirect in self  by Aey
                    Response.Redirect("KTJB02_PO.aspx?Mode=Edit&selfMode=EditPO&id=" & Session("job_order_id") & "&po_id=" & po_id & "&job_order=" & Session("job_order"))

            End Select

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderPO_ItemCommand", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptJobOrderPO_ItemDataBound
    '	Discription	    : Event repeater bound data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    : Rawikarn K.
    '	Update Date	    : 23-04-2014
    '*************************************************************/
    Protected Sub rptJobOrderPO_ItemDataBound( _
       ByVal sender As Object, _
       ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
   ) Handles rptJobOrderPO.ItemDataBound
        Try

            ' object link button
            Dim btnDel As New LinkButton
            Dim lblAmount As New Label
            Dim btnEdit As New LinkButton
            Dim dtJobOrderPO As New DataTable

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            lblAmount = DirectCast(e.Item.FindControl("lblAmount"), Label)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            dtJobOrderPO = Session("dtJobOrderPO")


            ' Check Use PO for Enabled Button  Add 2014/04/23
            Dim i As Integer = 0

            For i = 0 To dtJobOrderPO.Copy.Rows.Count
                If dtJobOrderPO.Copy.Rows(e.Item.ItemIndex).Item("check_use").ToString > "0" Then
                    btnDel.CssClass = "icon_del2 icon_center15"
                    btnEdit.CssClass = "icon_edit2 icon_center15"
                    btnDel.Enabled = False
                    btnEdit.Enabled = False
                Else
                    'Session("actDelete") = True
                    btnDel.Enabled = True
                    btnEdit.Enabled = True
                End If
            Next
           

            'set permission on button check out 2014/04/23
            'If Not Session("actDelete") Then
            '    btnDel.Enabled = False
            'End If

            'set permission on button
            If Not Session("actUpdate") Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
                btnEdit.Enabled = False
            End If

            'set permission for amount item
            If Not Session("actAmount") Then
                lblAmount.Text = "******"
            End If

            ' Set id to hashtable
            hashID.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            hashFile.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "po_file"))


        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptJobOrderPO_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnUpload_2_Click
    '	Discription	    : Event btnUpload_2 is clicked 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 25-04-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnUpload_2_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnUpload_2.Click
        Try
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            If Session("po_id") = "" Then
                'Check input Criteria data
                If CheckCriteriaInput() = False Then
                    Exit Sub
                Else
                    ' call function set session dto
                    SetValueToDto()

                    'Get folder name
                    If FileAttach.HasFile Then
                        If Session("Mode") = "Add" Then
                            strFolderName = hidIpAddress.Value
                        ElseIf Session("Mode") = "Edit" Then
                            strFolderName = Session("job_order")
                        End If

                        'set path
                        strPath = strPathConfigPO & strFolderName
                        'check exist path
                        If (Not System.IO.Directory.Exists(strPath)) Then
                            Directory.CreateDirectory(strPath)
                        End If
                        'Upload file to server
                        Session("PathFolderUpload") = strPath & "/" & Session("fileName")

                        'check exist file job order po
                        If File.Exists(Session("PathFolderUpload")) Then
                            My.Computer.FileSystem.DeleteFile(Session("PathFolderUpload"))
                        End If
                        'save as file to folder on server
                        FileAttach.SaveAs(strPath & "/" & Session("fileName"))

                    Else
                        ' show message box when Browse file not succuss
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_011"))
                        Exit Sub
                    End If
                End If

            Else
                'When Edit mode don't Check PO File 

                If txtPOAmount.Text <> "" Then
                    If txtPODate.Text <> "" Then
                        If txtPONo.Text <> "" Then
                            If txtReceiptDate.Text <> "" Then
                                Dim filename As String = FileAttach.FileName
                                Session("fileName") = filename

                            End If
                        End If
                    End If
                End If
                SetValueToDto()

            End If

                If Session("Mode") = "Add" Then
                    ConfirmMessage("KTJB02_PO", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_001"))
                Else
                    If Session("po_id") <> "" Then
                    ConfirmMessage("KTJB02_PO", strEditPo, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_013"))
                    Else
                        ConfirmMessage("KTJB02_PO", strConfirmIns, strCancelIns, objMessage.GetXMLMessage("KTJB_02_PO_001"))
                    End If

                End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnUpload_Click", ex.Message.ToString, Session("UserName"))
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
    '	Update User	    : Rawikarn K.
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try 
            'get ip address
            GetIpAddress()

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
                btnUpload_2.Enabled = False
                btnUpload_2.Visible = False
                btnUpload.Enabled = True
                btnUpload.Visible = True

                If Not String.IsNullOrEmpty(Request.QueryString("HontaiAmount")) And Request.QueryString("HontaiAmount") Is Nothing Then
                    Session("HontaiAmount") = String.Empty
                Else
                    Session("HontaiAmount") = Request.QueryString("HontaiAmount")
                End If
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                btnUpload_2.Enabled = False
                btnUpload_2.Visible = False
                btnUpload.Enabled = True
                btnUpload.Visible = True

                'set data QueryString to session
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

                If Not String.IsNullOrEmpty(Request.QueryString("HontaiAmount")) And Request.QueryString("HontaiAmount") Is Nothing Then
                    Session("HontaiAmount") = String.Empty
                Else
                    Session("HontaiAmount") = Request.QueryString("HontaiAmount")
                End If


                If Not String.IsNullOrEmpty(Request.QueryString("selfMode")) And Request.QueryString("selfMode") Is Nothing Then
                    Session("selfMode") = String.Empty

                ElseIf Not Request.QueryString("selfMode") Is Nothing Then
                    Session("selfMode") = Request.QueryString("selfMode")
                    Session("po_id") = Request.QueryString("po_id")
                    LoadIntUpdate()
                    Exit Sub
                End If
            Else
                btnUpload_2.Enabled = False
                btnUpload_2.Visible = False
                btnUpload.Enabled = True
                btnUpload.Visible = True
            End If

                ' call function check permission
                CheckPermission()

            ' call function check job order po
            ' don't check use in jb po then check in function SearchData() 2014/04/23
            ' CheckUseInJobOrderPO()

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
            rbtPoType.SelectedValue = Session("rbtPoType")
            txtPONo.Text = Session("txtPONo")
            txtPOAmount.Text = Session("txtPOAmount")
            txtReceiptDate.Text = Session("txtReceiptDate")
            txtPODate.Text = Session("txtPODate")


            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                ' call function clear session
                ClearSession()
                'Insert Job Order
                InsertJobOrderPOTemp()
                Exit Sub
            End If

            ' Case cancel when upload PO
            If objUtility.GetQueryString(strCancelIns) = "True" Then
                DeleteFileUpload()
                Exit Sub
            End If

            ' check delete item
            If objUtility.GetQueryString(strResult) = "True" Then
                DeleteJobOrderPOTemp()
            End If

            ' Update Job Order PO Add 20140428
            If objUtility.GetQueryString(strEditPo) = "True" Then
                ' call function clear sessions
                ClearSession()
                UpdatePOData()
                Exit Sub
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
            Session("dtJobOrderPO") = Nothing
            Session("rbtPoType") = Nothing
            Session("txtPONo") = Nothing
            Session("txtPOAmount") = Nothing
            Session("txtPODate") = Nothing
            Session("txtReceiptDate") = Nothing 
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
            rbtPoType.SelectedValue = Nothing
            txtPONo.Text = String.Empty
            txtPOAmount.Text = String.Empty
            txtPODate.Text = String.Empty
            txtReceiptDate.Text = String.Empty

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
            ' table object keep value from item service
            Dim dtJobOrderPO As New DataTable
            Dim objPage As New Common.Utilities.Paging

            ' get table object from session 
            dtJobOrderPO = Session("dtJobOrderPO")

            ' check record for display
            If Not IsNothing(dtJobOrderPO) AndAlso dtJobOrderPO.Rows.Count > 0 Then
                ' get page source for repeater
                pagedData = objPage.DoPaging(intPageNo, dtJobOrderPO)
                ' write paging
                lblPaging.Text = objPage.DrawPaging(intPageNo, pagedData.PageCount)
                ' bound data between pageDate with repeater
                rptJobOrderPO.DataSource = pagedData
                rptJobOrderPO.DataBind()
                ' call fucntion set description
                lblDescription.Text = objPage.WriteDescription(intPageNo, pagedData.PageCount, dtJobOrderPO.Rows.Count)
            Else
                ' case not exist data
                ' clear binding data and clear description
                lblPaging.Text = Nothing
                lblDescription.Text = "&nbsp;"
                rptJobOrderPO.DataSource = Nothing
                rptJobOrderPO.DataBind()
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
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUseInJobOrderPO()
        Try
            Session("boolInuseJobPO") = False
            'call function IsUsedInJobOrderPo from joborderService
            Dim boolInuseJobPO As Boolean = objJobOrderSer.IsUsedInJobOrderPo(Session("job_order_id"))
            Session("boolInuseJobPO") = boolInuseJobPO

            'set permission to item 
            ' dont' check permission to delete
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
    '	Discription	    : Search job order po temp data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    : Rawikarn
    '	Update Date	    : 03-02-2014
    '*************************************************************/
    Private Sub SearchData()
        Try
            ' table object keep value from item service
            Dim dtJobOrderPO As New DataTable

            ' call function GetJobOrderPOTempList from JobOrderService 
            dtJobOrderPO = objJobOrderSer.GetJobOrderPOTempList(hidIpAddress.Value)

            ' set table object to session
            Session("dtJobOrderPO") = dtJobOrderPO

            'Modify 2013/09/19 Start
            'cal function calculate hontai amount
            'GetSumPoAmount()
            'Cal function calculate total amount
            GetTotalAmount()
            'cal functon calculate sum other amount
            'SetSumOtherAmount()
            'Modify 2013/09/19 End 

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SearchData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: DeleteJobOrderPOTemp
    '	Discription	    : Delete job order po temp data
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub DeleteJobOrderPOTemp()
        Try
            ' check flag in_used
            If Session("boolInuse") = False Then
                ' case in_used then alert message
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_PO_005"))
                Exit Sub
            End If

            ' check state of delete item
            If objJobOrderSer.DeletePOTemp(Session("intID")) Then
                ' case delete success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_PO_006"))
                'delete file upload
                DeleteFile(Session("strFile"))
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
            Else
                ' case delete not success show message box
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_PO_007"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteJobOrderPOTemp", ex.Message.ToString, Session("UserName"))
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


            If Session("po_id") Is Nothing And FileAttach.HasFile = False Then

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
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_011"))
                    Exit Function
                End If

            Else

                If FileAttach.HasFile = False Then
                    Dim filename As String = Session("po_file")
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
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_011"))
                        Exit Function
                    End If
                End If



            End If



            'Check format date of field po Date From
            If txtPODate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtPODate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check format date of field receipt Date From
            If txtReceiptDate.Text.Trim <> "" Then
                If objIsDate.IsDate(txtReceiptDate.Text.Trim) = False Then
                    ' show message box display "Invalid date format. Date format should be dd/mm/yyyy"
                    objMessage.AlertMessage(objMessage.GetXMLMessage("Common_004"))
                    Exit Function
                End If
            End If

            'Check exist po type on job order po temp (1 Job Order have PO Type = Hontai one only)
            'If objJobOrderSer.CheckExistPoType(Session("ipAddress")) = False Then
            If rbtPoType.SelectedValue = 0 Then
                If Session("po_id") Is Nothing Then
                    If objJobOrderSer.CheckExistPoType(hidIpAddress.Value) = False Then
                        ' show message box when po type > 1
                        objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_008"))
                        Exit Function
                    End If
                End If

            End If

            'Check exist po no on job order po 
            Dim strJob_order_id As String
            If Session("Mode") = "Add" Then
                strJob_order_id = ""
            Else
                strJob_order_id = Session("job_order_id")
            End If


            ' When edit PO don't check exist PoNo 
            If Session("po_id") Is Nothing Then
                'call function CheckExistPoNo
                If objJobOrderSer.CheckExistPoNo(txtPONo.Text.Trim, strJob_order_id) = False Then
                    ' show message box when po no > 0
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_009"))
                    'Exit Function
                End If
            End If


            'Check exist po no on job order po temp
            'If objJobOrderSer.CheckExistPoNoTemp(txtPONo.Text.Trim) = False Then
            ' show message box when po no > 0
            'objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_009"))
            'Exit Function
            'End If

            ' don't when edit po mode
            ' Update Job Order PO Add 20140428
            If Session("po_id") Is Nothing Then
                'Check exist po file on job order po temp
                If objJobOrderSer.CheckExistPoFile(Session("fileName"), hidIpAddress.Value) = False Then
                    ' show message box when po no > 0
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_010"))
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
            Dim objJobOrderDtoPO As New Dto.JobOrderDto
            Dim strPoDate As String = ""
            Dim strReceiptDate As String = ""

            'Replace date dd/mm/yyyy to array
            Dim arrPoDate() As String = Split(txtPODate.Text.Trim(), "/")
            Dim arrReceiptDate() As String = Split(txtReceiptDate.Text.Trim(), "/")
           
            'set po date to yyyymmdd format
            If UBound(arrPoDate) > 0 Then
                strPoDate = arrPoDate(2) & arrPoDate(1) & arrPoDate(0)
            End If

            'set Receipt date to yyyymmdd format
            If UBound(arrReceiptDate) > 0 Then
                strReceiptDate = arrReceiptDate(2) & arrReceiptDate(1) & arrReceiptDate(0)
            End If

            If Session("po_id") <> "" Then
                Session("HontaiAmount") = txtPOAmount.Text

            End If


            'Modify by Aey
            Dim tPOAmount As String = ""
            If rbtPoType.SelectedIndex = 0 Then
                If Session("HontaiAmount") = String.Empty Then
                    tPOAmount = "0.00"
                Else
                    tPOAmount = Session("HontaiAmount")
                End If
            Else
                If Not txtPOAmount Is Nothing Then
                    tPOAmount = txtPOAmount.Text
                Else
                    tPOAmount = "0.00"
                End If
            End If

            ' assign value to dto object
            With objJobOrderDtoPO
                .id = Session("job_order_id")
                .po_type = rbtPoType.SelectedValue
                .po_no = txtPONo.Text
                .po_amount = tPOAmount.Replace(",", "")
                .po_date = strPoDate
                .po_receipt_date = strReceiptDate
                .po_file = Session("fileName")
                .ip_address = hidIpAddress.Value
            End With

            ' set dto object to session
            Session("objJobOrderDtoPO") = objJobOrderDtoPO

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertJobOrderPOTemp
    '	Discription	    : Insert Job Order po temp
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertJobOrderPOTemp()
        Try
            'Delete 2013/09/16
            'call function upload file
            'If UploadFilePO(Session("FileAttach")) = False Then
            '    Exit Sub
            'End If

            ' call function set value to control
            SetValueToControl()

            ' call function InsertPoTemp from service and alert message
            If objJobOrderSer.InsertPoTemp(Session("objJobOrderDtoPO"), strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_PO_002"))
                'Check Permission
                CheckPermission()
                'Check data use job order po table check out 20140424
                ' CheckUseInJobOrderPO()
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
            objLog.ErrorLog("InsertJobOrderPOTemp", ex.Message.ToString, Session("UserName"))
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
            Dim objJobOrderDtoPO As New Dto.JobOrderDto
            Dim strPODate As String = ""
            Dim strRecDate As String = ""
            ' set value to dto object from session
            objJobOrderDtoPO = Session("objJobOrderDtoPO")

            ' set value to control
            With objJobOrderDtoPO
                rbtPoType.SelectedValue = .po_type
                txtPONo.Text = .po_no
                txtPOAmount.Text = Format(.po_amount, "#,##0.00")

                'set format date dd/MM/yyyy to text
                If .po_date.ToString.Trim.Length > 0 Then
                    strPODate = Left(.po_date, 4) & "/" & Mid(.po_date, 5, 2) & "/" & Right(.po_date, 2)
                    txtPODate.Text = CDate(strPODate).ToString("dd/MM/yyyy")
                Else
                    txtPODate.Text = .po_date
                End If
                'set format date dd/MM/yyyy to text
                If .po_receipt_date.ToString.Trim.Length > 0 Then
                    strRecDate = Left(.po_receipt_date, 4) & "/" & Mid(.po_receipt_date, 5, 2) & "/" & Right(.po_receipt_date, 2)
                    txtReceiptDate.Text = CDate(strRecDate).ToString("dd/MM/yyyy")
                Else
                    txtReceiptDate.Text = .po_receipt_date
                End If

            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UploadFilePO
    '	Discription	    : Upload file po to server
    '	Return Value	: Boolean
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 24-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function UploadFilePO(ByVal FileAttach As FileUpload) As Boolean
        Try
            UploadFilePO = False
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            If FileAttach.HasFile Then
                If Session("Mode") = "Add" Then
                    strFolderName = hidIpAddress.Value
                ElseIf Session("Mode") = "Edit" Then
                    strFolderName = Session("job_order")
                End If
                strPath = strPathConfigPO & strFolderName
                'check exist path
                If (Not System.IO.Directory.Exists(strPath)) Then
                    Directory.CreateDirectory(strPath)
                End If
                'Upload file to server
                Session("PathFolderUpload") = strPath & "/" & Session("fileName")
                'Delete 2013/09/16
                'If File.Exists(Session("PathFolderUpload")) Then
                '    File.Delete(Session("PathFolderUpload"))
                'End If

                'check exist file job order po
                If File.Exists(Session("PathFolderUpload")) Then
                    My.Computer.FileSystem.DeleteFile(Session("PathFolderUpload"))
                End If
                'save as file to server
                FileAttach.SaveAs(strPath & "/" & Session("fileName"))

            Else
                ' show message box when Browse file not succuss
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_01_PO_011"))
                Exit Function
            End If

            UploadFilePO = True
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UploadFilePO", ex.Message.ToString, Session("UserName"))
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
            'set path file
            strPath = strPathConfigPO & strFolderName & "/" & strFileName

            'check exist file job order po
            If File.Exists(strPath) Then
                My.Computer.FileSystem.DeleteFile(strPath)
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("DeleteFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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

            ' call function GetSumPoAmount from JobOrderService
           dtJobOrderPoTemp = objJobOrderSer.GetSumPoAmount(hidIpAddress.Value)
            ' set table object to session

            If Not IsNothing(dtJobOrderPoTemp) AndAlso dtJobOrderPoTemp.Rows.Count > 0 Then
                Session("SumPoAmountPO") = dtJobOrderPoTemp.Rows(0).Item("sum_po_amount").ToString
            Else
                Session("SumPoAmountPO") = 0.0
            End If

            ' set session to text
            hidHontaiAmount.Value = Format(Convert.ToDouble(Session("SumPoAmountPO")), "#,##0.00")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetSumPoAmount", ex.Message.ToString, Session("UserName"))
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

            ' call function GetTotalAmount from JobOrderService
            dtTotalAmount = objJobOrderSer.GetTotalAmount(hidIpAddress.Value)

            ' set table object to session
            If Not IsNothing(dtTotalAmount) AndAlso dtTotalAmount.Rows.Count > 0 Then
                Session("TotalPoAmountPO") = dtTotalAmount.Rows(0).Item("total_po_amount").ToString
            Else
                Session("TotalPoAmountPO") = 0.0
            End If

            ' set session to text
            hidTotalAmount.Value = Format(Convert.ToDouble(Session("TotalPoAmountPO")), "#,##0.00")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("GetTotalAmount", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetSumOtherAmount
    '	Discription	    : calculate hotai Amount
    '	Return Value	: nothing
    '	Create User	    : Suwishaya L.
    '	Create Date	    : 19-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetSumOtherAmount()
        Try

            Dim dblHotaiAmount As Decimal
            Dim dblTotalAmount As Decimal
            Dim dblSumOtherAmount As Decimal

            'set data to duble            
            dblHotaiAmount = CDbl(hidHontaiAmount.Value.Trim.Replace(",", ""))
            dblTotalAmount = CDbl(hidTotalAmount.Value.Trim.Replace(",", ""))

            'calculate data to Sum Other Amount = Total Amount – Hontai  Amount
            dblSumOtherAmount = dblTotalAmount - dblHotaiAmount
            hidSumAmount.Value = Format(Convert.ToDouble(dblSumOtherAmount), "#,##0.00")

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetSumOtherAmount", ex.Message.ToString, Session("UserName"))
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
    '	Function name	: LoadintUpdate()
    '	Discription		: Po to Field
    '	Return Value	: 
    '	Create User		: Rawikarn K.
    '	Create Date		: 11-02-2014
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Sub LoadIntUpdate()

        Try
            Dim objDto As New DataTable
            Dim intPONO As Integer = 0
            Dim strFolderName As String = ""
            Dim strPath As String = ""

            intPONO = Request.QueryString("po_id")
            'MsgBox(intPONO)

            ' call function GetVatByID from service
            objDto = objJobOrderSer.GetOneJobOrderPOTempList(intPONO)


            If Session("Mode") = "Add" Then
                strFolderName = hidIpAddress.Value
            ElseIf Session("Mode") = "Edit" Then
                strFolderName = Session("job_order")
            End If
            'set path file
            strPath = strPathConfigPO & strFolderName

            If Session("Mode") = "Edit" Then
                rbtPoType.Enabled = False
                txtPONo.Enabled = False
            End If

            If Session("Mode") = "Edit" Then
                'reqValidatorFileAttach.Enabled = False
                reqValidatorFileAttach.Visible = False
            End If

            With objDto
                rbtPoType.SelectedValue = objDto.Rows(0).Item("po_type").ToString
                txtPONo.Text = objDto.Rows(0).Item("po_no").ToString
                txtPOAmount.Text = objDto.Rows(0).Item("po_amount").ToString
                txtPODate.Text = CDate(objDto.Rows(0).Item("po_date")).ToString("dd/MM/yyyy")
                txtReceiptDate.Text = CDate(objDto.Rows(0).Item("po_receipt_date")).ToString("dd/MM/yyyy")
                FileAttach.Attributes.Add("value", objDto.Rows(0).Item("po_file").ToString)
                lblFileAttach.Text = strPath & objDto.Rows(0).Item("po_file").ToString

            End With

            Session("objDto") = objDto
            Session("intPOId") = intPONO
            Session("po_file") = objDto.Rows(0).Item("po_file").ToString

        Catch ex As Exception
            objLog.ErrorLog("LoadIntUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: btnUpdate_Click
    '	Discription	    : Event Update PO 
    '	Return Value	: nothing
    '	Create User	    : Rawikarn K.
    '	Create Date	    : 17-02-2014
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/

    Private Sub UpdatePOData()
        Try

            SetValueToControl()

            ' call function UpdateJobOrderPOToTempList from service and alert message
            If objJobOrderSer.UpdateJobOrderPOToTempList(Session("po_id"), Session("objJobOrderDtoPO"), hidIpAddress.Value, strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTJB_02_PO_012"))

                'Check Permission
                CheckPermission()
                'Check data use job order po table check out 20140424
                ' CheckUseInJobOrderPO()
                ' call function search new data
                SearchData()
                ' call function display page
                DisplayPage(Request.QueryString("PageNo"))
                'Cleat Item
                ClearItem()
            Else
                ClearItem()
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If
        Catch ex As Exception
            objLog.ErrorLog("UpdatePOData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region


End Class
