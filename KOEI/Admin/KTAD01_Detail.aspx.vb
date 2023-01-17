Imports System.Web.Configuration

Partial Class Admin_KTAD01_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTAD01_Detail: User Login Detail", Session("UserName"))
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Init", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Page_Load
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If Session("UserName") Is Nothing Then Session("UserName") = "Boonyarit"
            If Not Page.IsPostBack Then
                'Set data page default
                Call SetInit()
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("Page_Load", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objService As New Service.ImpUserLoginService
            Dim objUserLoginDto As New Dto.UserLoginDto
            Dim intUserLoginID As Integer = CInt(objComm.GetQueryString("id"))

            objUserLoginDto = objService.GetUserLoginForDetail(intUserLoginID)
            If (Not objUserLoginDto Is Nothing) Then
                Call ShowData(objUserLoginDto)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data UserLogin
    '	Return Value	: 
    '	Create User	    : Nisa S.
    '	Create Date	    : 12-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData(ByVal objUserLogin As Dto.UserLoginDto)
        Try
            'Dim strType2_TextAll As String = String.Empty

            With objUserLogin
                lblUserName.Text = .user_name.Trim
                lblPassword.Text = .password.Trim
                lblFirstName.Text = .first_name.Trim
                lblLastName.Text = .last_name.Trim
                lblDepartment.Text = .name.Trim
                lblAccountNextApprove.Text = .account_next_approve.Trim
                lblPurchaseNextApprove.Text = .purchase_next_approve.Trim
                lblOutsourceNextApprove.Text = .outsource_next_approve.Trim

                'Session("FileName") = .file.Trim

            End With

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
