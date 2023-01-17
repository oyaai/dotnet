#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMenuDao
'	Class Discription	: Interface Menu Dao
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

Imports Microsoft.VisualBasic

Namespace Dao
    Public Interface IMenuDao
        Function GetMenuList(ByVal intUserID As Integer) As List(Of Entity.IMenuEntity)
    End Interface
End Namespace

