
Partial Class Overview_KTOV03
    Inherits MessageInUpdatePanel

    Private objLog As New Common.Logs.Log

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTOV03: SEARCH TASK", Session("UserName"))
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
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
                Call CheckChangePage()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Call SearchData()
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnSearch_Click", ex.Message.Trim, Session("UserName"))
        End Try

    End Sub

    Protected Sub rptTask_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Try
            Dim strPage As String = e.CommandArgument.ToString
            Session("strLinkPage") = strPage

            Select Case e.CommandName.Trim
                Case "Ref"
                    Call ShowPopupInPanel(strPage, 800, 1000, "_blank", True)

                Case "Todo"
                    Response.Redirect(strPage)

            End Select

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("rptTask_ItemCommand", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region
    
#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objCom As New Common.Utilities.Utility
            Dim strNew As String = objCom.GetQueryString("New")

            'Set DDL_Task
            Call SetTaskDDL()
            'Clear data on Form
            Call ClearForm()
            'Clear data on Table
            Call ClearTB()
            'Clear Session and get data search in page
            Select Case strNew.Trim
                Case "True"
                    Call ClearSession()
                    Call SearchData()
                    'Case "Back"
                    '    Call CheckBackPage()
            End Select
            

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetTaskDDL
    '	Discription	    : Set data to DDL_Task
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetTaskDDL()
        Try
            Dim objService As New Service.ImpTaskService
            Dim objListDto As List(Of Dto.TaskDto)
            Dim objComm As New Common.Utilities.Utility

            objListDto = objService.GetListTaskOfDDL(Session("UserID"))
            If objListDto Is Nothing Then Exit Sub
            If objListDto.Count = 0 Then Exit Sub
            objComm.LoadList(ddlSearchTask, objListDto, "task", "task", True)

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetTaskDDL", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearForm
    '	Discription	    : Clear data form
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearForm()
        Try
            ddlSearchTask.SelectedIndex = 0

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearForm", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearTB
    '	Discription	    : Clear data table
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearTB()
        Try
            rptTask.DataSource = Nothing
            rptTask.DataBind()
            lblFootTB1.Text = "&nbsp;"
            lblFootTB2.Text = "&nbsp;"
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearTB", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearSession
    '	Discription	    : Clear Session
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearSession()
        Try
            Session.Remove("strLinkPage")
            Session.Remove("DataSearch")
            Session.Remove("PageNo")
            Session.Remove("objDT")

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ClearSession", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: CheckChangePage
    '	Discription	    : Check mode change page 
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub CheckChangePage()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim strPageNo As String = objComm.GetQueryString("PageNo")

            If (Not strPageNo Is Nothing) AndAlso strPageNo.Trim <> String.Empty Then
                Session("PageNo") = strPageNo
                ddlSearchTask.SelectedValue = Session("DataSearch").ToString
                Call ShowDataTable()
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("CheckChangePage", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SearchData
    '	Discription	    : Search data task process
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 13-08-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SearchData()
        Try
            Dim objService As New Service.ImpTaskService
            Dim objDT As New System.Data.DataTable
            Dim objTaskList As List(Of Dto.TaskDto)

            If ddlSearchTask.SelectedIndex = 0 Then
                objTaskList = objService.GetTaskSearch(String.Empty, CInt(Session("UserID")), objDT)
                Session("DataSearch") = String.Empty
            Else
                objTaskList = objService.GetTaskSearch(ddlSearchTask.SelectedValue, CInt(Session("UserID")), objDT)
                Session("DataSearch") = ddlSearchTask.SelectedValue
            End If

            If (Not objTaskList Is Nothing) AndAlso objTaskList.Count > 0 Then
                'พบข้อมูล
                Call SetDataToSession(objDT)
                Call ShowDataTable()
            Else
                'ไม่พบข้อมูล
                Session.Remove("DataSearch")
                Call ClearTB()
                Call ShowMsgInPanel(String.Empty, "Common_001")
            End If

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
    '	Create Date	    : 13-08-2013
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
    '	Create Date	    : 13-08-2013
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
            rptTask.DataSource = objDataShow
            rptTask.DataBind()
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
#End Region

End Class
