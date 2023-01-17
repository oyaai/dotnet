#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpMenuService
'	Class Discription	: Implement of menu Service
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

Namespace Service
    Public Class ImpMenuService
        Implements Service.IMenuService

        Private cLog As New Common.Logs.Log
        Private ceMenu As New Entity.ImpMenuEntity

#Region "Property"
        '/**************************************************************
        '	Function name	: GetMenuList
        '	Discription	    : Get menu list
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 21-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMenuList( _
            ByVal intUserID As Integer _
        ) As System.Collections.Generic.List(Of Dto.MenuDto) Implements IMenuService.GetMenuList
            ' new list menu object
            GetMenuList = New List(Of Dto.MenuDto)
            Try
                ' variable keep value from entity
                Dim cdMenu As Dto.MenuDto
                ' variable list keep list of menu
                Dim listMenu As New List(Of Entity.IMenuEntity)

                ' call entity for getmenulist
                listMenu = ceMenu.GetMenuList(intUserID)

                ' assign value to dto
                For Each values In listMenu
                    cdMenu = New Dto.MenuDto
                    With cdMenu
                        .category_id = values.category_id
                        .category_name = values.category_name
                        .priority = values.priority
                        .menu_text = values.menu_text
                        .navigate_url = values.navigate_url
                    End With
                    ' add dto value to list dto object
                    GetMenuList.Add(cdMenu)
                Next
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetMenuList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region
        
    End Class
End Namespace

