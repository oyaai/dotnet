Imports Enums

Partial Class Master_KTMS11
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private check_menu_id As MenuId = MenuId.Unit

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS11: Search Unit", Session("UserName"))
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
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim objService As New Service.ImpUnitService
            Dim objDT As New System.Data.DataTable
            Dim objComMsg As New Common.Utilities.Message
            Dim strName As String = txtUnit.Text.Trim

            If objService.GetUnitForSearch(strName, objDT) Then
                ' result is data meet                
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                ' result is data not meet
                Call ClearTB()
                objComMsg.AlertMessage(String.Empty, "Common_001")
            End If

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
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Call ClearSession()
            Response.Redirect("KTMS12.aspx?Mode=Add")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnAdd_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptUnit_ItemCommand
    '	Discription	    : rptUnit_ItemCommand
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUnit_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptUnit.ItemCommand
        Try
            Dim strAry() As String = e.CommandArgument.ToString.Split(",")
            Dim intUnitId As Integer = CInt(strAry(0))
            Dim strUnitName As String = strAry(1)
            Session("UnitID") = intUnitId

            Select Case e.CommandName.Trim
                Case "Edit"
                    Call LinkEdit(intUnitId, strUnitName)

                Case "Del"
                    Call LinkDel(intUnitId)

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptVendor_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: rptUnit_ItemDataBound
    '	Discription	    : rptUnit_ItemDataBound
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub rptUnit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUnit.ItemDataBound
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
            ' write error log
            objLog.ErrorLog("rptInquery_ItemDataBound", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
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
    '	Create Date	    : 03-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTB()
        Try
            'Clear value table rptUnit
            rptUnit.DataSource = Nothing
            rptUnit.DataBind()
            lblFootTB1.Text = "&nbsp;"
            lblFootTB2.Text = "&nbsp;"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearTB", ex.Message.Trim, Session("UserName"))
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

            objActUser = objComUser.CheckPermission(check_menu_id)
            Session("ActUser") = objActUser

            btnSearch.Enabled = objActUser.actList
            btnAdd.Enabled = objActUser.actCreate

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckUserPer", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetDataToSession
    '	Discription	    : Set data to session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetDataToSession(ByVal objDT As System.Data.DataTable)
        Try
            ' Set data to session
            If txtUnit.Text.Trim <> String.Empty Then
                Session("DataSearch") = txtUnit.Text.Trim
            Else
                Session("DataSearch") = Nothing
            End If
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowDataTable()
        Try
            Dim objDT As System.Data.DataTable = Session("objDT")
            Dim objPageNo As Integer = Session("PageNo")
            Dim objComm As New Common.Utilities.Paging
            Dim objDataShow As PagedDataSource = objComm.DoPaging(objPageNo, objDT)
            Dim strFootTB As String = objComm.DrawPaging(objPageNo, objDataShow.Count, objDT.Rows.Count)
            ' set data show table
            rptUnit.DataSource = objDataShow
            rptUnit.DataBind()
            ' set data show foot table_1
            strFootTB = objComm.WriteDescription(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB1.Text = strFootTB.Trim
            ' set data show foot table_2
            strFootTB = objComm.DrawPaging(objPageNo, objDataShow.PageCount, objDT.Rows.Count)
            lblFootTB2.Text = strFootTB.Trim
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowDataTable", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: LinkEdit
    '	Discription	    : Set Link button edit
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkEdit(ByVal intUnitId As Integer, ByVal strUnitName As String)
        Try
            Call ClearSession()
            strUnitName = Server.HtmlEncode(strUnitName)
            Response.Redirect("KTMS12.aspx?Mode=Edit&ID=" & intUnitId & "&Name=" & strUnitName)
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub LinkDel(ByVal intVendorId As Integer)
        Try
            Dim objCom As New Common.Utilities.Message
            objCom.ConfirmMessage("KTMS11", "ModeDel", String.Empty, "KTMS_11_001")
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckModeDel()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objComMsg As New Common.Utilities.Message
            Dim objService As New Service.ImpUnitService
            Dim strValue As String = objComm.GetQueryString("ModeDel")

            If (Not strValue Is Nothing) AndAlso strValue.Trim = "True" Then
                If objService.CheckUnitForDel(CInt(Session("UnitID"))) = False Then
                    'You can't cancel Unit
                    objComMsg.AlertMessage(String.Empty, "KTMS_11_002", "KTMS11.aspx?New=Back")
                    Exit Sub
                End If

                If objService.CancelUnit(CInt(Session("UnitID"))) Then
                    'Delete or Cancel Unit is successful
                    objComMsg.AlertMessage(String.Empty, "KTMS_11_003", "KTMS11.aspx?New=Back")
                Else
                    'Delete or Cancel Unit is fail
                    objComMsg.AlertMessage(String.Empty, "KTMS_11_004", "KTMS11.aspx?PageNo=1")
                End If
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckModeDel", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 04-06-2013
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
            Dim objService As New Service.ImpUnitService
            Dim objDT As New System.Data.DataTable
            Dim objComMsg As New Common.Utilities.Message
            Dim strName As String = String.Empty

            ' check and set data search
            Call CheckDataSearch()
            strName = txtUnit.Text.Trim

            If objService.GetUnitForSearch(strName, objDT) Then
                ' result is data meet                
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                ' result is data not meet
                'Call ClearTB()
                Call ShowDataTable()
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
    '	Create Date	    : 04-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckDataSearch()
        Try
            Dim strName As String = Session("DataSearch") 'Session("UnitName")
            'Check and Set data name
            If (Not strName Is Nothing) AndAlso strName.Trim <> String.Empty Then
                txtUnit.Text = strName.Trim
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckDataSearch", ex.Message.Trim, Session("UserName"))
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
            Session.Remove("UnitID")
            Session.Remove("PageNo")
            Session.Remove("ActUser")
            Session.Remove("UnitName")
            If intSel = 1 Then Session.Remove("DataSearch")
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
