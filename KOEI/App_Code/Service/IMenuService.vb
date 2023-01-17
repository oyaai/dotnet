#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IMenuService
'	Class Discription	: Interface of menu Service
'	Create User 		: Komsan L.
'	Create Date		    : 21-05-2013
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
Imports System.Collections.Generic
#End Region

Namespace Service
    Public Interface IMenuService
        Function GetMenuList(ByVal intUserID As Integer) As List(Of Dto.MenuDto)
    End Interface
End Namespace

