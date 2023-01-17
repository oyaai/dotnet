#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Validations
'	Class Name		    : CommonValidation
'	Class Discription	: 
'	Create User 		: Suwishaya L.
'	Create Date		    : 26-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports System.Data
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports Common.Validations
Imports System.Web.Configuration
Imports System.IO
#End Region

Namespace Validations
    Public Class CommonValidation
        Inherits Validation

        Private objLog As New Common.Logs.Log
        Private autoApproveAccount As String = WebConfigurationManager.AppSettings("AutoApproveAccount")
        Private autoApprovePurchase As String = WebConfigurationManager.AppSettings("AutoApprovePurchase")
#Region "Function"

        '/**************************************************************
        '	Function name	: IsExistAccountApprove
        '	Discription	    : Check exist account approve
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 26-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsExistAccountApprove() As Boolean
            ' set default return value
            IsExistAccountApprove = False
            Try
                Dim arrApproveId() As String
                Dim user As String = HttpContext.Current.Session("UserName")

                'Check exist Account Approve
                If HttpContext.Current.Session("AccountNextApprove") Is Nothing Or _
                    String.IsNullOrEmpty(HttpContext.Current.Session("AccountNextApprove")) Or _
                    HttpContext.Current.Session("AccountNextApprove").ToString = "0" Then

                    arrApproveId = Split(autoApproveAccount, ";")
                    For i As Integer = 0 To arrApproveId.Length - 1
                        If user = arrApproveId(i) Then
                            HttpContext.Current.Session("AccountNextApprove") = HttpContext.Current.Session("UserID")
                            IsExistAccountApprove = True
                            Exit For
                        End If
                    Next
                Else
                    IsExistAccountApprove = True
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsExistAccountApprove(Validations)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: IsExistPurchaseApprove
        '	Discription	    : Check exist purchase approve
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 30-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsExistPurchaseApprove() As Boolean
            ' set default return value
            IsExistPurchaseApprove = False
            Try
                Dim arrApproveId() As String
                Dim user As String = HttpContext.Current.Session("UserName")

                'Check exist Account Approve
                If HttpContext.Current.Session("AccountNextApprove") Is Nothing Or _
                    String.IsNullOrEmpty(HttpContext.Current.Session("AccountNextApprove")) Or _
                    HttpContext.Current.Session("AccountNextApprove").ToString = "0" Then

                    arrApproveId = Split(autoApprovePurchase, ";")
                    For i As Integer = 0 To arrApproveId.Length - 1
                        If user = arrApproveId(i) Then
                            IsExistPurchaseApprove = True
                            Exit For
                        End If
                    Next
                Else
                    IsExistPurchaseApprove = True
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsExistPurchaseApprove(Validations)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeleteReportFile
        '	Discription	    : Delete Report File
        '	Return Value	: Boolean
        '	Create User	    : Pranitda Sroengklang
        '	Create Date	    : 03-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Sub DeleteReportFile()
            Try
                Dim path = "../Report/RptFileSave"
                If Directory.Exists(HttpContext.Current.Server.MapPath(path)) Then
                    Dim files() As String = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(path))
                    Dim fileNm As String = ""
                    Dim stringDt As String = ""
                    Dim stringDate As String = ""

                    For Each fileNm In files
                        If UCase(Right(fileNm, 3)) = "PDF" Then 'pdf file
                            stringDt = Right(fileNm, 23)
                        ElseIf UCase(Right(fileNm, 4)) = "XLSX" Then 'excel file
                            stringDt = Right(fileNm, 24)
                        End If

                        stringDate = Left(stringDt, 8)
                        If CDate(Left(stringDate, 4) & "/" & Mid(stringDate, 5, 2) & "/" & Right(stringDate, 2)) < CDate(Date.Now.ToString("yyyy/MM/dd")) Then
                            File.Delete(fileNm)
                        End If
                    Next

                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteReportFile(Validations)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Sub
#End Region

    End Class
End Namespace

