Imports System.Web.Configuration
Imports System.IO
Imports System.Data

Partial Class Master_KTMS01_Detail
    Inherits System.Web.UI.Page

    Private objLog As New Common.Logs.Log
    Private objComm As New Common.Utilities.Message


#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Page_Init
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ' Write start log
            objLog.StartLog("KTMS01_Detail: Vendor Detail", Session("UserName"))
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
    '	Create Date	    : 22-05-2013
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

    '/**************************************************************
    '	Function name	: btnFileAttached_Click
    '	Discription	    : btnFileAttached_Click
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 29-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnFileAttached_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFileAttached.Click
        Dim strPath As String = WebConfigurationManager.AppSettings("FilePaths") & "Vendor/" & Session("FileName")
        Dim strFullPath As String = Server.MapPath(strPath)
        Dim strNoFileFound As String = WebConfigurationManager.AppSettings("FilePaths") & "Vendor/FileNotFound.html"
        Dim objwriter As StreamWriter

        Try
            ' Check exists file
            If File.Exists(strFullPath) = True Then
                Dim Page As New Web.UI.Page
                Dim sb As New System.Text.StringBuilder()
                'Gets the executing web page 
                Page = HttpContext.Current.CurrentHandler

                sb.Append("<script type = 'text/javascript'>")
                sb.Append("window.onload = function OpenPopup() {")
                sb.Append("popup = window.open('")
                sb.Append(strPath)
                sb.Append("','")
                sb.Append("_blank")
                sb.Append("','width=800")
                sb.Append(",height=600")
                sb.Append(",toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes');")
                sb.Append("};</script>")

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "OpenPopup", sb.ToString())
            Else
                ' Create File "FileNotFound.txt"
                objwriter = New StreamWriter(Server.MapPath(strNoFileFound))
                ' Write text in File
                objwriter.Write("<html><html><head><title></title></head><body><h1>File not found.</h1></body></html>")
                objwriter.Close()
                objComm.ShowPagePopup(strNoFileFound, 800, 600, "_blank", True)
            End If
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("btnFileAttached_Click", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"

    '/**************************************************************
    '	Function name	: NoExistsNoFileFound
    '	Discription	    : Check Exists file "NoFileFound.txt"
    '	Return Value	: 
    '	Create User	    : Wasan D.
    '	Create Date	    : 17-07-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub NoExistsNoFileFound()
        Try
            Dim strNoFileFound As String = WebConfigurationManager.AppSettings("FilePaths") & "Vendor/FileNotFound.txt"
            Dim objWriter As New StreamWriter(Server.MapPath(strNoFileFound))
            '
            If Not File.Exists(Server.MapPath(strNoFileFound)) Then
                ' Create File "FileNotFound.txt"
                File.Create(Server.MapPath(strNoFileFound)).Dispose()
                ' Write text in File
                objWriter.Write("File Not Found.")
                objWriter.Close()
            End If
            objComm.ShowPagePopup(strNoFileFound, 800, 600, "_blank", True)
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("NoExistsNoFileFound", ex.Message.ToString.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetInit
    '	Discription	    : Set Init page
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetInit()
        Try
            Dim objComm As New Common.Utilities.Utility
            Dim objVendorService As New Service.ImpVendorService
            Dim objVendorBranchSer As New Service.ImpVendorBranchService
            Dim objVendorDto As New Dto.VendorDto
            Dim dtBranch As New DataTable
            Dim intVendorId As Integer = CInt(objComm.GetQueryString("ID"))

            objVendorDto = objVendorService.GetVendorForDetail(intVendorId)
            dtBranch = objVendorBranchSer.GetBranchWithVendorID(intVendorId)
            If (Not objVendorDto Is Nothing) Then
                Call ShowData(objVendorDto)
                ShowBranchDetail(dtBranch)
            End If

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("SetInit", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowBranchDetail
    '	Discription	    : Show data BranchDetail
    '	Return Value	: Nothing
    '	Create User	    : Wasan D.
    '	Create Date	    : 09-10-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowBranchDetail(ByVal dtBranch As System.Data.DataTable)
        Try
            rptVendor.DataSource = dtBranch
            rptVendor.DataBind()
        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowBranchDetail", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ShowData
    '	Discription	    : Show data vendor
    '	Return Value	: 
    '	Create User	    : Boonyarit
    '	Create Date	    : 22-05-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ShowData(ByVal objVendor As Dto.VendorDto)
        Try
            Dim strType2_TextAll As String = String.Empty

            With objVendor
                lblID.Text = .id
                lblType1.Text = .type1_text.Trim
                strType2_TextAll = .type2_text.Trim
                If .type2_text.ToUpper = "PERSON" Then
                    strType2_TextAll &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Identification Card No: "
                Else
                    strType2_TextAll &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Taxpayer Identification no: "
                End If
                strType2_TextAll &= (" " & .type2_no.Trim)
                lblType2_TextAll.Text = strType2_TextAll
                lblName.Text = .name.Trim
                lblShortName.Text = .short_name.Trim
                lblPerson1.Text = .person_in_charge1.Trim
                lblPerson2.Text = .person_in_charge2.Trim
                'lblPaymentTerm.Text = .payment_term.Trim
                'lblPaymentCondition.Text = .payment_condition.Trim
                lblCountry.Text = .country_name.Trim
                lblZipCode.Text = .zipcode.Trim
                lblAddress.Text = .address.Trim
                lblTelNo.Text = .tel.Trim
                lblFaxNo.Text = .fax.Trim
                lblRemarks.Text = .remarks.Trim
                'linkFileAttached.Text = .file.Trim
                'linkFileAttached.NavigateUrl = WebConfigurationManager.AppSettings("VendorPath") & .file.Trim
                btnFileAttached.Text = IIf(.file.Trim <> Nothing, .file.Trim, "")
                Session("FileName") = .file.Trim

            End With

        Catch ex As Exception
            ' Write error log
            objLog.ErrorLog("ShowData", ex.Message.Trim, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
