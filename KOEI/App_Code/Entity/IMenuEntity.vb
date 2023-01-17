#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMenuEntity
'	Class Discription	: Interface Menu entity
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

Namespace Entity
    Public Interface IMenuEntity
        Property id() As Integer
        Property category_id() As Integer
        Property category_name() As String
        Property menu_text() As String
        Property value() As String
        Property priority() As Integer
        Property navigate_url() As String

        Function GetMenuList(ByVal intUserID As Integer) As List(Of Entity.IMenuEntity)
    End Interface
End Namespace

