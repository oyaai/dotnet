
Partial Class Overview_KTOV02
    Inherits System.Web.UI.Page

    Private cLog As New Common.Logs.Log

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim objTaskService As New Service.ImpTaskService
            Dim intTask As Integer = 1

            'Check task process by user_id (Boon add 13/08/2013)
            'intTask = objTaskService.CheckTaskProcess(CInt(Session("UserID")))
            'If intTask > 0 Then
            '    Response.Redirect("KTOV03.aspx?New=True")
            'End If
        Catch ex As Exception
            ' Write error log
            cLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
End Class


