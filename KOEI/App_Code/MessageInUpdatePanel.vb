#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Web.UI
Imports System.Web.UI.WebControls
#End Region

Public Class MessageInUpdatePanel
    Inherits Page

    Private Clogs As Common.Logs.Log
    Private CCom As Common.Utilities.Message

    Sub New()
        Clogs = New Common.Logs.Log
        CCom = New Common.Utilities.Message
    End Sub

    '/**************************************************************
    '	Function name	: ChangeMessage(String)
    '	Discription		: Cheage message in javascript
    '	Return Value	: String
    '	Create User		: Boon
    '	Create Date		: 02-05-2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Private Function ChangeMessage(ByVal strValue As String) As String
        Try
            Dim strTmp As String = String.Empty

            'check ค่า " ให้แก้เป็น \"
            strTmp = strValue.Replace(Chr(34), ("\" & Chr(34)))
            'check ค่า ' ให้แก้เป็น \'
            strTmp = strTmp.Replace(Chr(39), "\'")
            'check ค่า new line ให้แก้เป็น \n
            strTmp = strTmp.Replace(vbCrLf, "\n")
            'check ค่า tab ให้แก้เป็น \n\t
            strTmp = strTmp.Replace(vbTab, "\n\t")
            'return string value of change
            ChangeMessage = strTmp
        Catch ex As Exception
            ChangeMessage = String.Empty
            Clogs.ErrorLog("CheangMessage", ex.Message.ToString)
        End Try
    End Function

#Region "Message in panel"
    '/**************************************************************
    '	Function name	: ShowConfirmInPanel
    '	Discription		: Show confirm Message in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowConfirmInPanel(ByVal strPageName As String, _
            ByVal strResultName As String, _
            ByVal strMessage As String, _
            Optional ByVal strMsgCode As String = "")

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler

            Dim strMsgValue As String = String.Empty
            If strMsgCode.Trim = String.Empty Then
                strMsgValue = ChangeMessage(strMessage)
            Else
                strMsgValue = ChangeMessage(CCom.GetXMLMessage(strMsgCode))
            End If

            Dim sb As New System.Text.StringBuilder()
            sb.Append("if (confirm('")
            sb.Append(strMsgValue)
            sb.Append("')){ ")
            If strPageName.IndexOf(".aspx") = -1 Then
                strPageName &= ".aspx"
            End If
            sb.Append("window.location = '" & strPageName & "?" & strResultName & "=True';")
            sb.Append("}")
            sb.Append("else{")
            sb.Append("};")

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            Clogs.ErrorLog("ShowConfirmInPanel", ex.Message.ToString)
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: ShowMsgInPanel
    '	Discription		: Show message in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowMsgInPanel(ByVal strMessage As String, _
                               Optional ByVal strMsgCode As String = "", _
                               Optional ByVal strURL As String = "")

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler

            Dim strMsgValue As String = String.Empty
            If strMsgCode.Trim = String.Empty Then
                strMsgValue = ChangeMessage(strMessage)
            Else
                strMsgValue = ChangeMessage(CCom.GetXMLMessage(strMsgCode))
            End If

            Dim sb As New System.Text.StringBuilder()
            sb.Append("alert('")
            sb.Append(strMsgValue)
            sb.Append("'); ")
            If strURL <> String.Empty Then
                sb.Append("window.location = '" & strURL & "'; ")
            End If

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            Clogs.ErrorLog("ShowMsgInPanel", ex.Message.ToString)
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: ShowMsgAndPopupInPanel
    '	Discription		: Show message and show page pupup in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowMsgAndPopupInPanel(ByVal strMessage As String, _
                                    ByVal urlNewPage As String, _
                                    Optional ByVal urlOldPage As String = "", _
                                    Optional ByVal width As Int32 = 100, _
                                    Optional ByVal height As Int32 = 100, _
                                    Optional ByVal window As String = "_blank", _
                                    Optional ByVal scrollbar As Boolean = False, _
                                    Optional ByVal strMsgCode As String = "")

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler

            Dim strMsgValue As String = String.Empty
            If strMsgCode.Trim = String.Empty Then
                strMsgValue = ChangeMessage(strMessage)
            Else
                strMsgValue = ChangeMessage(CCom.GetXMLMessage(strMsgCode))
            End If

            Dim sb As New System.Text.StringBuilder()
            sb.Append("alert('")
            sb.Append(strMsgValue)
            sb.Append("'); ")

            'Check link url of go to new page
            sb.Append("window.onload = window.open('")
            sb.Append(urlNewPage)
            sb.Append("','")
            sb.Append(window)
            sb.Append("','width=")
            sb.Append(width)
            sb.Append(",height=")
            sb.Append(height)
            sb.Append(",toolbar=no,location=no, directories=no,status=no,menubar=no,")
            If scrollbar = True Then
                sb.Append("scrollbars=yes")
            Else
                sb.Append("scrollbars=no")
            End If
            sb.Append(",resizable=no")
            sb.Append("'); ")

            'Go to old page 
            If ((Not urlOldPage Is Nothing) AndAlso urlOldPage.Trim <> String.Empty) Then
                sb.Append("window.location = '" & urlOldPage.Trim & "'; ")
            End If

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            Clogs.ErrorLog("ShowMsgAndPopupInPanel", ex.Message.ToString)
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: ShowPopupInPanel
    '	Discription		: Show page pupup in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowPopupInPanel(ByVal urlPage As String, _
                             Optional ByVal width As Int32 = 100, _
                             Optional ByVal height As Int32 = 100, _
                             Optional ByVal window As String = "_blank", _
                             Optional ByVal scrollbar As Boolean = False)

        Try
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler
            Dim sb As New System.Text.StringBuilder()
            sb.Append("popup = window.open('")
            sb.Append(urlPage)
            sb.Append("','")
            sb.Append(window)
            sb.Append("','width=")
            sb.Append(width)
            sb.Append(",height=")
            sb.Append(height)
            sb.Append(",toolbar=no,location=no, directories=no,status=no,menubar=no,")
            If scrollbar = True Then
                sb.Append("scrollbars=yes")
            Else
                sb.Append("scrollbars=no")
            End If
            sb.Append(",resizable=yes")
            sb.Append("');")

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            Clogs.ErrorLog("ShowPopupInPanel", ex.Message.ToString)
        End Try

    End Sub

    '/**************************************************************
    '	Function name	: ShowMultiplePopupInPanel
    '	Discription		: Show multiple page pupup in update panel
    '	Return Value	: 
    '	Create User		: Boon
    '	Create Date		: 02/08/2013
    '	Update User		: 
    '	Update Date		: 
    '**************************************************************/
    Public Sub ShowMultiplePopupInPanel(ByVal urlPage As List(Of String), _
                                     Optional ByVal width As Int32 = 100, _
                                     Optional ByVal height As Int32 = 100, _
                                     Optional ByVal window As String = "_blank", _
                                     Optional ByVal scrollbar As Boolean = False)
        Try
            Dim intLeft As Integer = 0
            Dim intTop As Integer = 0
            Dim objPage As Web.UI.Page = HttpContext.Current.CurrentHandler
            Dim sb As New System.Text.StringBuilder()

            For Each strUrlPage In urlPage
                sb.Append("popup = window.open('")
                sb.Append(strUrlPage)
                sb.Append("','")
                sb.Append(window)
                sb.Append("','width=")
                sb.Append(width)
                sb.Append(",height=")
                sb.Append(height)
                sb.Append(",left=")
                intLeft += 20
                sb.Append(intLeft)
                sb.Append(",top=")
                intTop += 20
                sb.Append(intTop)
                sb.Append(",toolbar=no,location=no, directories=no,status=no,menubar=no,")
                If scrollbar = True Then
                    sb.Append("scrollbars=yes")
                Else
                    sb.Append("scrollbars=no")
                End If
                sb.Append(",resizable=no")
                sb.Append("'); ")
            Next

            ScriptManager.RegisterClientScriptBlock(objPage, objPage.GetType(), Guid.NewGuid().ToString(), sb.ToString, True)

        Catch ex As Exception
            Clogs.ErrorLog("ShowMultiplePopupInPanel", ex.Message.ToString)
        End Try

    End Sub

#End Region

End Class
