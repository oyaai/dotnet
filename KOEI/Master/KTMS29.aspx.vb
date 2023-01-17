Imports Enums

Partial Class Master_KTMS29
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS29: SEARCH SCHEDULE RATE", Session("UserName"))
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
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim objCom As New Common.Validations.Validation

            'Check data currency 
            'If ddlSearchCurrency.SelectedIndex = 0 Then
            '    Call ClearValueSearch()
            '    lblMsgCurrency.Visible = True
            '    Exit Sub
            'Else
            '    lblMsgCurrency.Visible = False
            'End If

            'Check data Effective Date
            If txtEF_Date.Text.Trim <> String.Empty Then
                If objCom.IsDate(txtEF_Date.Text) = False Then
                    Call ClearTB()
                    lblMsgEF_date.Visible = True
                    Exit Sub
                Else
                    lblMsgEF_date.Visible = False
                End If
            End If

            Call SearchData()

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnAdd_Click
    '	Discription	    : btnAdd_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS30.aspx?Mode=Add")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptScheculeRate_ItemCommand
    '	Discription	    : rptScheculeRate_ItemCommand
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptScheculeRate_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptScheculeRate.ItemCommand
        Try
            Dim intScheculeRateId As Integer = CInt(e.CommandArgument.ToString)
            Session("ScheculeRateId") = intScheculeRateId

            Select Case e.CommandName.Trim
                Case "Edit"
                    Call LinkEdit(intScheculeRateId)

                Case "Del"
                    Call LinkDel()

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptScheculeRate_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptScheculeRate_ItemDataBound
    '	Discription	    : rptScheculeRate_ItemDataBound
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptScheculeRate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptScheculeRate.ItemDataBound
        Try
            Dim objActUser = GetActUser()
            ' object link button
            Dim btnDel As New LinkButton
            Dim btnEdit As New LinkButton

            ' find linkbutton and assign to variable
            btnDel = DirectCast(e.Item.FindControl("btnDel"), LinkButton)
            btnEdit = DirectCast(e.Item.FindControl("btnEdit"), LinkButton)

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
            objLog.ErrorLog("rptScheculeRate_ItemDataBound", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objService As New Service.ImpCurrencyService
            Dim objCom As New Common.Utilities.Utility
            Dim objMsg As New Common.Utilities.Message
            Dim strNew As String = objCom.GetQueryString("New")
            Dim objListDto As New List(Of Dto.CurrencyDto)

            'reqCurrency.ErrorMessage = objMsg.GetXMLMessage("KTMS_29_004")
            lblMsgEF_date.Text = objMsg.GetXMLMessage("Common_004")

            'Get data list to DropDownList
            objListDto = objService.GetCurrencyForList
            If (Not objListDto Is Nothing) AndAlso (objListDto.Count > 0) Then
                Call objCom.LoadList(ddlSearchCurrency, objListDto, "name", "id")
                'Set enable of button = True
                Call SetBtnEnable(True)
            Else
                'Set enable of button = False
                Call SetBtnEnable(False)
            End If

            'Clear message currency
            lblMsgCurrency.Visible = False
            'Clear data on Table
            Call ClearTB()
            'Clear Session in page
            Select Case strNew.Trim
                Case "True"
                    Call ClearSession(1)
                Case "Back"
                    If (Not Session("CurrencyId") Is Nothing) Or (Not Session("EF_Date") Is Nothing) Then
                        Call SearchData(True)
                    End If
            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTB
    '	Discription	    : Clear data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTB()
        Try
            rptScheculeRate.DataSource = Nothing
            rptScheculeRate.DataBind()
            lblFootTB1.Text = "&nbsp;"
            lblFootTB2.Text = "&nbsp;"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearTB", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetBtnEnable
    '	Discription	    : Set value enable of button
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
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
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckUserPer()
        Try
            Dim objComUser As New Common.UserPermissions.UserPermission
            Dim objActUser As New Common.UserPermissions.ActionPermission

            'Check permission user
            objActUser = objComUser.CheckPermission(MenuId.ScheduleExchange)
            Session("ActUser") = objActUser

            btnSearch.Enabled = objActUser.actList
            btnAdd.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear Session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession(Optional ByVal intSel As Integer = 0)
        Try
            Session.Remove("ScheculeRateId")
            If intSel = 1 Then Session.Remove("CurrencyId") : Session.Remove("EF_Date")
            Session.Remove("ActUser")
            Session.Remove("PageNo")
            Session.Remove("objDT")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearValueSearch
    '	Discription	    : Clear Session("CurrencyId")
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 23-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearValueSearch()
        Try
            Session.Remove("CurrencyId")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearValueSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: GetValueDDL
    '	Discription	    : Get value ddl(DropDownList)
    '	Return Value	: Integer
    '	Create User	    : Boonyarit
    '	Create Date	    : 16-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Function GetValueDDL(ByVal strValue As String) As Integer
        Try
            If strValue = String.Empty Then
                GetValueDDL = 0
            Else
                GetValueDDL = CInt(strValue)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("GetValueDDL", ex.Message.Trim, Session("UserName"))
        End Try
    End Function

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search data Schecule Rate
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData(Optional ByVal SetValue As Boolean = False)
        Try
            Dim objService As New Service.ImpScheculeRateService
            Dim objDT As New System.Data.DataTable
            Dim objCom As New Common.Utilities.Message
            Dim intCurrency As Integer = 0
            Dim strEF_Date As String = String.Empty
            Dim intRanking As Integer = 0

            'กรณี มีเลือกข้อมูลอยู่แล้ว
            If SetValue = True Then
                Call CheckDataSearch()
            End If

            '**ยกเลิกการตรวจสอบว่า มีการเลือกรายการ ให้ทำการค้นหารายการ (16/08/2013)
            'If ddlSearchCurrency.SelectedIndex > 0 Then

            intCurrency = GetValueDDL(ddlSearchCurrency.SelectedValue)
            If txtEF_Date.Text.Trim <> String.Empty Then
                strEF_Date = txtEF_Date.Text.Trim
            End If
            intRanking = CInt(System.Web.Configuration.WebConfigurationManager.AppSettings("ranking"))

            If objService.GetScheculeRateForSearch(intCurrency, strEF_Date, intRanking, objDT) Then
                'พบข้อมูล
                Session("CurrencyId") = intCurrency
                Session("EF_Date") = strEF_Date
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                'ไม่พบข้อมูล
                If SetValue = True Then
                    Call ShowDataTable()
                Else
                    Call ClearValueSearch()
                    Call ClearTB()
                    objCom.AlertMessage(String.Empty, "Common_001")
                End If
            End If
            'End If
            

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SearchData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data table to session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
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
    '	Function name	: ShowDataTable
    '	Discription	    : Set Show data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
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
            rptScheculeRate.DataSource = objDataShow
            rptScheculeRate.DataBind()
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
    '	Function name	: GetActUser
    '	Discription	    : Get action user permission
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
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
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckChangePage()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strPageNo As String = objComm.GetQueryString("PageNo")

            If (Not strPageNo Is Nothing) AndAlso strPageNo.Trim <> String.Empty Then
                Session("PageNo") = strPageNo
                Call CheckDataSearch()
                Call ShowDataTable()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckChangePage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckDataSearch
    '	Discription	    : Check data search
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckDataSearch()
        Try
            Dim intCurrency As Integer
            Dim strEFDate As String
            'Check data currency_id 
            If Session("CurrencyId") Is Nothing Then
                Exit Sub
            Else
                intCurrency = Session("CurrencyId")
                ddlSearchCurrency.SelectedValue = intCurrency
            End If
            'Check data ef_date
            If Session("EF_Date") Is Nothing Then
                Exit Sub
            Else
                strEFDate = Session("EF_Date").ToString
                If strEFDate <> String.Empty Then txtEF_Date.Text = strEFDate.Trim
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDataSearch", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkEdit
    '	Discription	    : Set Link button edit
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkEdit(ByVal intSRateId As Integer)
        Try
            Call ClearSession()
            Response.Redirect("KTMS30.aspx?Mode=Edit&ID=" & intSRateId)
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
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDel()
        Try
            Dim objCom As New Common.Utilities.Message
            objCom.ConfirmMessage("KTMS29", "ModeDel", String.Empty, "KTMS_29_001")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("LinkDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckModeDel
    '	Discription	    : Check mode delete 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 02-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckModeDel()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objComMsg As New Common.Utilities.Message
            Dim objService As New Service.ImpScheculeRateService
            Dim strValue As String = objComm.GetQueryString("ModeDel")

            If (Not strValue Is Nothing) AndAlso strValue.Trim = "True" Then
                If objService.CancelScheculeRate(CInt(Session("ScheculeRateId"))) Then
                    'Delete or Cancel Schecule Rate is successful
                    objComMsg.AlertMessage(String.Empty, "KTMS_29_002")
                    Call SearchData(True)
                Else
                    'Delete or Cancel Schecule Rate is fail
                    objComMsg.AlertMessage(String.Empty, "KTMS_29_003")
                    'Call ClearTB()
                    Call SearchData(True)
                End If
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckModeDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region

End Class
