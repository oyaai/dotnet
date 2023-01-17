#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ICenterService
'	Class Discription	: Interface of CenterService
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
#End Region

Namespace Service
    Public Interface ICenterService
        Function SaveFile(ByVal strPathFile As String, ByVal fileSave As FileUpload) As Boolean
        Function CheckFile(ByVal intFileSize As Integer) As Boolean
        Function DeleteFile(ByVal strPathFile As String) As Boolean
    End Interface
End Namespace

