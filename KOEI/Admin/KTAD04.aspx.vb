#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Admin
'	Class Name		    : Admin_KTAD04
'	Class Discription	: Webpage for User_Permission Management
'	Create User 		: Wasan D.
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
Imports System.Data

Partial Class Admin_KTAD04
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objMessage As New Common.Utilities.Message
    Private objUtility As New Common.Utilities.Utility
    Private objAction As New Common.UserPermissions.ActionPermission
    Private objPermission As New Common.UserPermissions.UserPermission
    Private objUserList As New Service.ImpUserService
    Private objPermissionList As New Service.ImpUserPermissionService
    Private pagedData As New PagedDataSource
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty
    Private Const strResult As String = "Result"

#Region "Events"

    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
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
            objLog.StartLog("KTAD04 : User Permission Management")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptUserPermission_ItemDataBound
    '	Discription	    : Event ItemDateBound set value to checkbox
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUserPermission_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUserPermission.ItemDataBound
        Try
            ' Variable Checkbox control
            Dim FN_Check As CheckBox
            ' Variable Datatable
            Dim listUserPermissionDt As New DataTable
            ' Variable Checkbox name in array
            Dim ChbName() As String = {"FN_Create", "Fn_Update", "Fn_Delete", _
                                       "Fn_List", "Fn_Amount", "Fn_Confirm", "Fn_Approve"}
            ' Set datatable in session to Variable
            listUserPermissionDt = Session("USPDtoList")
            ' Loop over the array.
            For Each value As String In ChbName
                ' Find checkbox name
                FN_Check = CType(e.Item.FindControl("cb" & value), CheckBox)
                If Not listUserPermissionDt Is Nothing Then
                    ' Set value to checkbox
                    FN_Check.Checked = IIf(listUserPermissionDt.Rows(e.Item.ItemIndex).Item(value) = 1, True, False)
                End If
            Next
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("rptUserPermission_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    
    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                initialpage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ddlUserName_SelectedIndexChanged
    '	Discription	    : Event ddlUserName Selected Index Changed
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub ddlUserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserName.SelectedIndexChanged
        Try
            Session("intUserID") = ddlUserName.SelectedValue
            ' call function SetValueToTextbox
            SetValueToTextbox()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ddlUserName_SelectedIndexChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            If IsNothing(Session("PageNo")) Then
                Response.Redirect("KTAD03.aspx")
            Else
                Response.Redirect("KTAD03.aspx?PageNo=" & Session("PageNo"))
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
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
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
    '	Discription	    : Event button Save on click
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 04-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Session("intUserID") = ddlUserName.SelectedValue
            ' Check Mode page and call function
            If Session("Mode") = "Add" Then
                DataTableUserPermissionUpdate(Session("listDtForAdd"))
                objMessage.ConfirmMessage("KTAD04", strConfirmIns, objMessage.GetXMLMessage("KTAD_04_001"))
            ElseIf Session("Mode") = "Edit" Then
                DataTableUserPermissionUpdate(Session("USPDtoList"))
                objMessage.ConfirmMessage("KTAD04", strConfirmUpd, objMessage.GetXMLMessage("KTAD_04_004"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: checkAll_CheckedChanged
    '	Discription	    : Select All Checkbox
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 17-09-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub checkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkAll.CheckedChanged
        Try
            ' Variable Checkbox control
            Dim FN_CheckBox As CheckBox
            Dim CHBValue As Boolean = checkAll.Checked
            ' Variable Checkbox name in array
            Dim ChbName() As String = {"Fn_Create", "Fn_Update", "Fn_Delete", _
                                       "Fn_List", "Fn_Amount", "Fn_Confirm", "Fn_Approve"}

            For Each rptItem As RepeaterItem In rptUserPermission.Items
                ' Loop over the array.
                For Each value As String In ChbName
                    ' Find checkbox name
                    FN_CheckBox = CType(rptItem.FindControl("cb" & value), CheckBox)
                    ' Set value to Datatable
                    If FN_CheckBox.Visible = True Then FN_CheckBox.Checked = CHBValue
                Next
            Next
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("checkAll_CheckedChanged", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Functions"

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub DataTableUserPermissionUpdate(ByVal listUSPDt As DataTable)
        Try
            ' Variable Checkbox control
            Dim FN_CheckBox As CheckBox
            ' Variable Checkbox name in array
            Dim ChbName() As String = {"Fn_Create", "Fn_Update", "Fn_Delete", _
                                       "Fn_List", "Fn_Amount", "Fn_Confirm", "Fn_Approve"}

            For Each rptItem As RepeaterItem In rptUserPermission.Items
                ' Loop over the array.
                For Each value As String In ChbName
                    ' Find checkbox name
                    FN_CheckBox = CType(rptItem.FindControl("cb" & value), CheckBox)
                    ' Set value to Datatable
                    listUSPDt.Rows(rptItem.ItemIndex).Item(value) = IIf(FN_CheckBox.Checked = True, 1, 0)
                Next
            Next
            ' Call function
            Session("USPDtoList") = listUSPDt
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("UpdateUserPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: DataTable
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Function SetPermissionValueToDto(ByVal listUSPDt As DataTable) As List(Of Dto.UserPermissionDto)
        SetPermissionValueToDto = Nothing
        Try
            ' Variable list of Userpermission Dto
            Dim objUSPDto As New List(Of Dto.UserPermissionDto)
            ' Variable Userpermission Dto
            Dim objDto As Dto.UserPermissionDto
            ' Set DataTable to Dto
            For Each row As DataRow In listUSPDt.Rows
                objDto = New Dto.UserPermissionDto
                With objDto
                    .Fn_Create = row("Fn_Create")
                    .Fn_Update = row("Fn_Update")
                    .Fn_Delete = row("Fn_Delete")
                    .Fn_List = row("Fn_List")
                    .Fn_Amount = row("Fn_Amount")
                    .Fn_Confirm = row("Fn_Confirm")
                    .Fn_Approve = row("Fn_Approve")
                    .id = row("id")
                    .UserID = row("user_id")
                    .MenuID = row("menu_id")
                    .menu_text = row("menu_text")

                End With
                objUSPDto.Add(objDto)
            Next
            SetPermissionValueToDto = objUSPDto
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetPermissionValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: AfterSaveData
    '	Discription	    : Show Data after click save button
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 02-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub AfterSaveData()
        Try
            ' call function LoadUserList to set Username dropdownlist
            LoadUserList()
            ' set select Username value
            ddlUserName.SelectedValue = CInt(Session("intUserID"))
            SetValueToTextbox()
            LoadPermissionSetting()
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("AfterSaveData", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 08-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub initialpage()
        Try
            ' check insert item
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                AfterSaveData()
                PermissionInsert(SetPermissionValueToDto(Session("USPDtoList")), Session("intUserID"))
                Exit Sub
            End If

            ' check update item
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                AfterSaveData()
                PermissionUpdate(SetPermissionValueToDto(Session("USPDtoList")))
                Exit Sub
            End If
            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("USPDtoList") = Nothing
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
            End If

            ' call function LoadUserList to set Username dropdownlist
            LoadUserList()
            ' call function LoadInitialUpdate
            If Session("Mode") = "Edit" Then
                LoadInitialUpdate()
            End If
            ' call function LoadPermissionSetting to set Permission Page setting
            LoadPermissionSetting()
            '' call function check permission
            CheckPermission()

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: PermissionUpdate
    '	Discription	    : Update User Permission
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub PermissionInsert(ByVal UserPerDtoList As List(Of Dto.UserPermissionDto), ByVal intUserID As String)
        Try
            ' call function InsertUserPermission from service and alert message
            If objPermissionList.InsertUserPermission(UserPerDtoList, intUserID, strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_04_002"), Nothing, "KTAD03.aspx?New=Insert")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("PermissionInsert", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: PermissionUpdate
    '	Discription	    : Update User Permission
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 11-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub PermissionUpdate(ByVal UserPerDtoList As List(Of Dto.UserPermissionDto))
        Try
            ' call function UpdateItem from service and alert message
            If objPermissionList.UpdateUserPermission(UserPerDtoList, strMsg) Then
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTAD_04_005"), Nothing, "KTAD03.aspx?New=Update")
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("PermissionUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckPermission
    '	Discription	    : Check permission
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckPermission()
        Try
            ' check permission of Payment Condition menu
            objAction = objPermission.CheckPermission(Enums.MenuId.UserPermission)
            ' set permission Create
            btnSave.Enabled = objAction.actCreate
            btnClear.Enabled = objAction.actCreate

            ' set action permission to session
            Session("actUpdate") = objAction.actUpdate
            Session("actDelete") = objAction.actDelete

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("CheckPermission", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadUserList
    '	Discription	    : Load User list to dropdown list
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LoadUserList()
        Try
            ' object Department service
            Dim objUserNonPermiss As New Service.ImpUserPermissionService
            ' listDepDto for keep value from service
            Dim listUserDto As New List(Of Dto.UserDto)

            ' Check Mode page and call function GetDepartmentList from service
            If Session("Mode") = "Add" Then
                listUserDto = objUserNonPermiss.GetUserNonSetPermissionList
            ElseIf Session("Mode") = "Edit" Then
                listUserDto = objUserNonPermiss.GetUserSetPermissionList
            End If

            ' call function for bound data with dropdownlist
            objUtility.LoadList(ddlUserName, listUserDto, "UserName", "UserID", IIf(Session("Mode") = "Add", True, False))

            '' set select Department from session
            'If Not IsNothing(Session("ddlUserName")) And ddlUserName.Items.Count > 0 Then
            '    ddlUserName.SelectedValue = Session("ddlUserName")
            'End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadUserList", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub


    '/**************************************************************
    '	Function name	: LoadPermissionSetting
    '	Discription	    : Load permission setting page
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LoadPermissionSetting()
        Try
            ' table object keep value from Userpermission service
            Dim dtInquiry As New DataTable

            ' call function GetUserPermissionList from ImpUserPermissionService
            dtInquiry = objPermissionList.GetPermissionList

            If Not IsNothing(dtInquiry) AndAlso dtInquiry.Rows.Count > 0 Then
                ' call function display page
                rptUserPermission.DataSource = dtInquiry
                rptUserPermission.DataBind()
            End If
            ' Set dtInquiry to session
            Session("listDtForAdd") = dtInquiry
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadPermissionSetting", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Display page on Edit mode
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LoadInitialUpdate()
        Try
            ' set default value to ingUserID
            Dim intUserID As Integer = 0

            ' check intUserID then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("ID")) Then
                intUserID = CInt(objUtility.GetQueryString("ID"))
            End If
            ' Set intUserID to session
            Session("intUserID") = intUserID
            ' set select Username value
            ddlUserName.SelectedValue = intUserID
            ' call function SetValueToTextbox
            SetValueToTextbox()
            ' disable username dropdownlist
            ddlUserName.Enabled = False
            ' Call function GetValueUserPermission
            GetValueUserPermission(intUserID)
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToTextbox
    '	Discription	    : Function set value to textbox
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub SetValueToTextbox()
        Try
            ' table object keep value from User service
            Dim dtUserDtail As New DataTable
            ' call function GetUserDetail from ImpUserPermissionService
            dtUserDtail = objPermissionList.GetUserDetail(ddlUserName.SelectedItem.ToString.Trim)
            If dtUserDtail.Rows.Count = 0 Then
                ' set value to textbox
                txtFirstName.Text = ""
                txtLastName.Text = ""
                txtDepartment.Text = ""
            Else
                ' set value to textbox
                txtFirstName.Text = dtUserDtail.Rows(0).Item("first_name").ToString
                txtLastName.Text = dtUserDtail.Rows(0).Item("last_name").ToString
                txtDepartment.Text = dtUserDtail.Rows(0).Item("department").ToString
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToTextbox", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 10-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' Variable Checkbox control
            Dim FN_Check As CheckBox
            ' Variable Checkbox name in array
            Dim ChbName() As String = {"FN_Create", "Fn_Update", "Fn_Delete", _
                                       "Fn_List", "Fn_Amount", "Fn_Confirm", "Fn_Approve"}
            If Session("Mode") <> "Edit" Then
                ' clear control
                ddlUserName.SelectedIndex = 0
                txtFirstName.Text = String.Empty
                txtLastName.Text = String.Empty
                txtDepartment.Text = String.Empty
            End If
            ' Loop over the array.
            For Each rptItem As RepeaterItem In rptUserPermission.Items
                For Each value As String In ChbName
                    ' Find checkbox name
                    FN_Check = CType(rptItem.FindControl("cb" & value), CheckBox)
                    FN_Check.Checked = False
                Next
            Next
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LoadUserList
    '	Discription	    : Load User list to dropdown list
    '	Return Value	: nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub GetValueUserPermission(ByVal intUserID As Integer)
        Try
            ' object Department service
            Dim objUPService As New Service.ImpUserPermissionService
            ' listDepDto for keep value from service
            Dim listUserPermissionDt As New DataTable

            ' Check Mode page and call function GetDepartmentList from service
            listUserPermissionDt = objUPService.GetUserPermissionSettingList(intUserID)

            Session("USPDtoList") = listUserPermissionDt
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadUserList", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region
End Class
