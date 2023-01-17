Imports Enums

Partial Class Master_KTMS01
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS01: VENDOR SEARCH", Session("UserName"))
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Init", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Call IsDoPostBack()
            'If Session("UserName") Is Nothing Then Session("UserName") = "Boonyarit"
            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckUserPer()
                Call CheckModeDel()
                Call CheckChangePage()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSearch_Click
    '	Discription	    : btnSearch_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim objService As New Service.ImpVendorService
            Dim objDT As New System.Data.DataTable
            Dim objSearch As New Dto.VendorDto
            Dim objCom As New Common.Utilities.Message

            objSearch = SetDataSearch()
            If objService.GetVendorForSearch(objSearch, objDT) Then
                'พบข้อมูล
                Session("DataSearch") = objSearch
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                'ไม่พบข้อมูล
                Call ClearTB()
                objCom.AlertMessage(String.Empty, "Common_001")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnExcel_Click
    '	Discription	    : btnExcel_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 27-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim objService As New Service.ImpVendorService
            Dim objSearch As New Dto.VendorDto
            Dim objCom As New Common.Utilities.Message

            objSearch = SetDataSearch()
            If objService.GetVendorForReport(objSearch) = False Then
                'ไม่พบข้อมูล
                objCom.AlertMessage(String.Empty, "Common_002")
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnExcel_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : btnAdd_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS02.aspx?New=True&Mode=Add")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.Trim, Session("UserName"))
        End Try

    End Sub

#Region "rptVendor"
    ''/**************************************************************
    ''	Function name	: rptVendor_DataBinding
    ''	Discription	    : rptVendor_DataBinding
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 21-05-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Protected Sub rptVendor_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles rptVendor.DataBinding
    '    Try
    '        Keys.Clear()
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("rptVendor_DataBinding", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub

    '/**************************************************************
    '	Function name	: rptVendor_ItemCommand
    '	Discription	    : rptVendor_ItemCommand
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptVendor_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptVendor.ItemCommand
        Try
            Dim intVendorId As Integer = CInt(e.CommandArgument.ToString)
            Session("VendorID") = intVendorId

            Select Case e.CommandName.Trim
                Case "View"
                    Call LinkDetails(intVendorId)

                Case "Edit"
                    Call LinkEdit(intVendorId)

                Case "Del"
                    Call LinkDel(intVendorId)

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptVendor_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptVendor_ItemDataBound
    '	Discription	    : rptVendor_ItemDataBound
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptVendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptVendor.ItemDataBound
        Try
            'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            '    'Set id is key of repeater
            '    Keys.Add(e.Item.ItemIndex, DataBinder.Eval(e.Item.DataItem, "id"))
            'End If

            Dim objActUser = GetActUser()
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton
            'Dim btnDetails As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)
            'btnDetails = DirectCast(e.Item.FindControl("btnDetails"), LinkButton)

            ' set permission on button
            btnEdit.Enabled = objActUser.actUpdate
            If objActUser.actUpdate = False Then
                btnEdit.CssClass = "icon_edit2 icon_center15"
            End If
            btnDel.Enabled = objActUser.actDelete
            If objActUser.actDelete = False Then
                btnDel.CssClass = "icon_del2 icon_center15"
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptVendor_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

#End Region
    
#Region "Function"
#Region "Not Use"
    ''/**************************************************************
    ''	Function name	: Keys
    ''	Discription	    : Set value of keys
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 21-05-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'ReadOnly Property Keys() As Hashtable
    '    Get
    '        If IsNothing(ViewState("Keys")) Then
    '            ViewState("Keys") = New Hashtable()
    '        End If
    '        Return CType(ViewState("Keys"), Hashtable)
    '    End Get
    'End Property

    ''/**************************************************************
    ''	Function name	: SetHeadTR
    ''	Discription	    : Set string head <Table><TR>
    ''	Return Value	: String
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 21-05-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Public Function SetHeadTR() As String
    '    Try
    '        intValueRow += 1
    '        If (intValueRow Mod 2) <> 0 Then
    '            SetHeadTR = "<tr class='table_item'>"
    '        Else
    '            SetHeadTR = "<tr class='table_alter'>"
    '        End If

    '    Catch ex As Exception
    '        ' Write error log
    '        SetHeadTR = "<tr class='table_item'>"
    '        objLog.ErrorLog("SetHeadTR", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Function

    ''/**************************************************************
    ''	Function name	: SetFootTR
    ''	Discription	    : Set string foot <Table><TR>
    ''	Return Value	: String
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 21-05-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Public Function SetFootTR() As String
    '    Try
    '        Return "</tr>"
    '    Catch ex As Exception
    '        ' Write error log
    '        SetFootTR = "</tr>"
    '        objLog.ErrorLog("SetFootTR", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Function

    ''/**************************************************************
    ''	Function name	: ClearValueRow
    ''	Discription	    : Clear data of intValueRow
    ''	Return Value	: 
    ''	Create User	    : Boonyarit
    ''	Create Date	    : 21-05-2013
    ''	Update User	    :
    ''	Update Date	    :
    ''*************************************************************/
    'Public Sub ClearValueRow()
    '    Try
    '        intValueRow = 0
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("ClearValueRow", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub

    'Private Sub IsDoPostBack()
    '    Try
    '        'Insure that the __doPostBack() JavaScript method is created...
    '        Me.ClientScript.GetPostBackEventReference(Me, String.Empty)
    '        If Me.Page.IsPostBack Then
    '            Dim eventTarget As String = IIf(Me.Request("__EVENTTARGET") = Nothing, String.Empty, Me.Request("__EVENTTARGET"))
    '            Dim eventArgument As String = IIf(Me.Request("__EVENTARGUMENT") = Nothing, String.Empty, Me.Request("__EVENTARGUMENT"))
    '            If eventTarget = "MyServerSideFunction" Then
    '                FunGet(eventArgument)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ' Write error log
    '        objLog.ErrorLog("IsDoPostBack", ex.Message.Trim, Session("UserName"))
    '    End Try
    'End Sub

    'Private Sub FunGet(ByVal arg As String)
    '    'your logic
    '    Dim args() As String = arg.Split("|")
    '    Dim firstValue As String = args(0)
    '    Dim secondValue As String = args(1)
    'End Sub
#End Region

    '/**************************************************************
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckChangePage()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strPageNo As String = objComm.GetQueryString("PageNo")
            If (strPageNo = "") Then strPageNo = Session("PageNo")
            If (Not strPageNo Is Nothing) AndAlso strPageNo.Trim <> String.Empty Then
                Session("PageNo") = strPageNo
                Call CheckDataSearch(Session("DataSearch"))
                Call ShowDataTable()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckChangePage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckBackPage
    '	Discription	    : Check mode back page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 12-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckBackPage()
        Try
            Dim objService As New Service.ImpVendorService
            Dim objDT As New System.Data.DataTable
            Dim objSearch As Dto.VendorDto = Session("DataSearch")
            Dim objCom As New Common.Utilities.Message

            ' check and set data search
            Call CheckDataSearch(objSearch)

            If objService.GetVendorForSearch(objSearch, objDT) Then
                'พบข้อมูล
                Session("DataSearch") = objSearch
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                'ไม่พบข้อมูล
                Call ClearTB()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckBackPage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckDataSearch
    '	Discription	    : Check mode change page show data search
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckDataSearch(ByRef objDataSearch As Dto.VendorDto)
        Try

            If objDataSearch Is Nothing Then
                objDataSearch = New Dto.VendorDto
                With objDataSearch
                    .intSCountry = 0
                    .intSType1 = -1
                    .intSType2 = -1
                    .strSName = String.Empty

                End With

            End If

            With objDataSearch
                'Check and Set data type1
                Select Case .intSType1
                    Case 0
                        rbtnSearchType1.Items(0).Selected = False
                        rbtnSearchType1.Items(1).Selected = True
                        rbtnSearchType1.Items(2).Selected = False
                    Case 1
                        rbtnSearchType1.Items(0).Selected = False
                        rbtnSearchType1.Items(1).Selected = False
                        rbtnSearchType1.Items(2).Selected = True
                    Case Else
                        rbtnSearchType1.Items(0).Selected = True
                        rbtnSearchType1.Items(1).Selected = False
                        rbtnSearchType1.Items(2).Selected = False
                End Select

                'Check and Set data type2
                Select Case .intSType2
                    Case 0
                        rbtnSearchType2.Items(0).Selected = False
                        rbtnSearchType2.Items(1).Selected = True
                        rbtnSearchType2.Items(2).Selected = False
                    Case 1
                        rbtnSearchType2.Items(0).Selected = False
                        rbtnSearchType2.Items(1).Selected = False
                        rbtnSearchType2.Items(2).Selected = True
                    Case Else
                        rbtnSearchType2.Items(0).Selected = True
                        rbtnSearchType2.Items(1).Selected = False
                        rbtnSearchType2.Items(2).Selected = False
                End Select

                'Check and Set data name
                If (Not .strSName Is Nothing) AndAlso .strSName.Trim <> String.Empty Then
                    txtSearchName.Text = .strSName.Trim
                End If

                'Check and Set data country
                If .intSCountry > 0 Then
                    ddlSearchCountry.SelectedValue = .intSCountry.ToString
                End If

            End With
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckModeDel
    '	Discription	    : Check mode delete 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckModeDel()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objComMsg As New Common.Utilities.Message
            Dim objService As New Service.ImpVendorService
            Dim strValue As String = objComm.GetQueryString("ModeDel")

            If (Not strValue Is Nothing) AndAlso strValue.Trim = "True" Then
                If objService.CheckVendorForDel(CInt(Session("VendorID"))) = False Then
                    'You can't cancel Vendor
                    objComMsg.AlertMessage(String.Empty, "KTMS_01_002")
                    Exit Sub
                End If

                If objService.CancelVendor(CInt(Session("VendorID"))) Then
                    'Delete or Cancel Vendor is successful
                    objComMsg.AlertMessage(String.Empty, "KTMS_01_003")

                    'Show data in table
                    Call CheckBackPage()
                Else
                    'Delete or Cancel Vender is fail
                    objComMsg.AlertMessage(String.Empty, "KTMS_01_004")
                End If
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckModeDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkEdit
    '	Discription	    : Set Link button edit
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkEdit(ByVal intVendorId As Integer)
        Try
            Call ClearSession()
            Response.Redirect("KTMS02.aspx?New=True&Mode=Edit&ID=" & intVendorId)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkEdit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkDel
    '	Discription	    : Set Link button delete
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDel(ByVal intVendorId As Integer)
        Try
            Dim objCom As New Common.Utilities.Message
            objCom.ConfirmMessage("KTMS01", "ModeDel", String.Empty, "KTMS_01_001")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkDetails
    '	Discription	    : Set Link button details
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDetails(ByVal intVendorId As Integer)
        Try
            Dim objComm As New Common.Utilities.Message
            Dim strPage As String = "KTMS01_Detail.aspx?ID=" & intVendorId.ToString

            objComm.ShowPagePopup(strPage, 1060, 600, , True)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDetails", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowDataTable
    '	Discription	    : Set Show data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDataTable()
        Try
            ' variable
            Dim objDT As System.Data.DataTable = Session("objDT")
            Dim objPageNo As Integer = Session("PageNo")
            Dim objComm As New Common.Utilities.Paging
            Dim objDataShow As PagedDataSource = objComm.DoPaging(objPageNo, objDT)
            Dim strFootTB As String
            ' set data show table
            rptVendor.DataSource = objDataShow
            rptVendor.DataBind()
            ' set data show foot table_1
            strFootTB = objComm.WriteDescription(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB1.Text = strFootTB
            ' set data show foot table_2
            strFootTB = objComm.DrawPaging(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB2.Text = strFootTB
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowDataTable", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTB
    '	Discription	    : Clear data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 27-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTB()
        Try
            rptVendor.DataSource = Nothing
            rptVendor.DataBind()
            lblFootTB1.Text = "&nbsp;"
            lblFootTB2.Text = "&nbsp;"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearTB", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data table to session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 21-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession(ByVal objDT As System.Data.DataTable)
        Try
            Session("objDT") = objDT
            Session("PageNo") = "1"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetDataToSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataSearch
    '	Discription	    : Set Data for search
    '	Return Value	: Dto.VendorDto
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function SetDataSearch() As Dto.VendorDto
        Try
            Dim objValue As New Dto.VendorDto
            SetDataSearch = Nothing
            With objValue
                'Check and Set data type1
                For Each objItem As ListItem In rbtnSearchType1.Items
                    If objItem.Selected = True Then
                        If objItem.Value.Trim <> String.Empty Then
                            .intSType1 = CInt(objItem.Value)
                            Exit For
                        End If
                    End If
                Next
                'Check and Set data type2
                For Each objItem As ListItem In rbtnSearchType2.Items
                    If objItem.Selected = True Then
                        If objItem.Value.Trim <> String.Empty Then
                            .intSType2 = CInt(objItem.Value)
                            Exit For
                        End If
                    End If
                Next
                'Check and Set data name
                If txtSearchName.Text.Trim <> String.Empty Then
                    .strSName = txtSearchName.Text.Trim
                End If
                'Check and Set data country
                If ddlSearchCountry.SelectedIndex > 0 Then
                    .intSCountry = ddlSearchCountry.SelectedValue
                End If

            End With
            SetDataSearch = objValue

        Catch ex As Exception
            ' Write error log
            SetDataSearch = Nothing
            objLog.ErrorLog("SetDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objService As New Service.ImpCountryService
            Dim objCom As New Common.Utilities.Utility
            Dim strNew As String = objCom.GetQueryString("New")

            'Clear data on Table
            Call ClearTB()
            'Clear Session in page
            'If strNew.Trim = "True" Then Call ClearSession()
            Select Case strNew.Trim
                Case "True"
                    Call ClearSession(1)
                Case "Back"
                    Call CheckBackPage()
            End Select

            'Get data list to DropDownList
            If objService.SetListCountryName(ddlSearchCountry, True) Then
                'Set enable of button = True
                Call SetBtnEnable(True)
            Else
                'Set enable of button = False
                Call SetBtnEnable(False)
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetBtnEnable
    '	Discription	    : Set value enable of button
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 20-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetBtnEnable(ByVal objValue As Boolean)
        Try
            btnAdd.Enabled = objValue
            btnSearch.Enabled = objValue
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetBtnEnable", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckUserPer
    '	Discription	    : Check user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            'Check permission user
            objActUser = objComUser.CheckPermission(MenuId.Vendor)
            Session("ActUser") = objActUser

            btnSearch.Enabled = objActUser.actList
            btnAdd.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetActUser
    '	Discription	    : Get action user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function GetActUser() As Common.UserPermissions.ActionPermission
        Try
            GetActUser = CType(Session("ActUser"), Common.UserPermissions.ActionPermission)
        Catch ex As Exception
            ' Write error log
            GetActUser = Nothing
            objLog.ErrorLog("GetActUser", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear Session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession(Optional ByVal intSel As Integer = 0)
        Try
            Session.Remove("objDT")
            Session.Remove("PageNo")
            Session.Remove("ActUser")
            Session.Remove("VendorID")
            If intSel = 1 Then Session.Remove("DataSearch")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region


End Class
