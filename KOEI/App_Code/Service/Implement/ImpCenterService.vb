Option Explicit On

#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpCenterService
'	Class Discription	: Class of CenterService
'	Create User 		: Boon
'	Create Date		    : 30-05-2013
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
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Web.Configuration
#End Region

Namespace Service
    Public Class ImpCenterService
        Implements ICenterService

        Private objLog As New Common.Logs.Log

        '/**************************************************************
        '	Function name	: DeleteFile
        '	Discription	    : Delete File
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteFile(ByVal strPathFile As String) As Boolean Implements ICenterService.DeleteFile
            Try
                Dim fileVendor As New FileInfo(strPathFile)

                DeleteFile = False
                'Check data for delete
                If fileVendor.Exists = True Then
                    fileVendor.Delete()

                    'Check data is delete successful
                    If New FileInfo(strPathFile).Exists = True Then
                        'พบข้อมูล ลบไม่สำเร็จ
                        DeleteFile = False
                    Else
                        'ไม่พบข้อมูล ลบสำเร็จ
                        DeleteFile = True
                    End If
                End If

            Catch ex As Exception
                DeleteFile = False
                objLog.ErrorLog("DeleteFile", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveFile
        '	Discription	    : Save File
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveFile(ByVal strPathFile As String, ByVal fileSave As FileUpload) As Boolean Implements ICenterService.SaveFile
            Try
                Dim fileItem As FileInfo

                SaveFile = False

                'Save file as
                fileSave.SaveAs(strPathFile)
                fileItem = New FileInfo(strPathFile)
                If fileItem.Exists = True Then
                    SaveFile = True
                Else
                    SaveFile = False
                End If

            Catch ex As Exception
                SaveFile = False
                objLog.ErrorLog("SaveFile", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckFile
        '	Discription	    : Check File
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 30-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckFile(ByVal intFileSize As Integer) As Boolean Implements ICenterService.CheckFile
            Try
                'HttpRuntimeSection httpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection

                'maxRequestLength = httpRuntimeSection.MaxRequestLength;
                'maxRequestLengthBytes = 1024 * maxRequestLength;

                'Dim objRuntimeSection As HttpRuntimeSection = ConfigurationManager.GetSection("system.web/httpRuntime")
                'Dim maxRequestLengthBytes As Integer = objRuntimeSection.MaxRequestLength * 1024

                'If intFileSize > maxRequestLengthBytes Then
                '    CheckFile = False
                'Else
                CheckFile = True
                'End If

            Catch ex As Exception
                CheckFile = False
                objLog.ErrorLog("CheckFile", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

    End Class

End Namespace

