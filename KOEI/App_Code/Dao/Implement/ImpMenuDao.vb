#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMenuDao
'	Class Discription	: Implement Menu Dao
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
Imports MySql.Data.MySqlClient

Namespace Dao
    Public Class ImpMenuDao
        Implements Dao.IMenuDao

        Private Conn As Common.DBConnection.MySQLAccess
        Private cLog As New Common.Logs.Log
#Region "Menu"

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
        ) As System.Collections.Generic.List(Of Entity.IMenuEntity) Implements IMenuDao.GetMenuList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            ' new list object
            GetMenuList = New List(Of Entity.IMenuEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                ' menu entity object
                Dim ceMenu As Entity.IMenuEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT						")
                    .AppendLine("	m.category_id,						")
                    .AppendLine("	mc.name AS category_name,						")
                    .AppendLine("	m.priority,						")
                    .AppendLine("	m.menu_text,						")
                    .AppendLine("	m.navigate_url						")
                    .AppendLine("	FROM menu m						")
                    .AppendLine("	INNER JOIN menu_category mc ON m.category_id = mc.id 						")
                    .AppendLine("	INNER JOIN user_permission up ON up.menu_id = m.id						")
                    .AppendLine("	WHERE up.delete_fg <> 1 						")
                    .AppendLine("	AND up.user_id = ?user_id 						")
                    .AppendLine("	AND (up.fn_create = 1 OR 						")
                    .AppendLine("		 up.fn_update = 1 OR 				")
                    .AppendLine("		 up.fn_delete = 1 OR 				")
                    .AppendLine("		 up.fn_list = 1 OR				")
                    .AppendLine("		 up.fn_confirm = 1 OR 					")
                    .AppendLine("		 up.fn_approve = 1)				")
                    .AppendLine("	ORDER BY m.category_id						")
                    .AppendLine("		    ,m.priority					")
                End With

                ' new connection object
                Conn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                Conn.AddParameter("?user_id", intUserID)

                ' execute data to data reader object
                dr = Conn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    ' assign value to entity
                    While dr.Read
                        ceMenu = New Entity.ImpMenuEntity
                        With ceMenu
                            .category_id = IIf(IsDBNull(dr.Item("category_id")), Nothing, dr.Item("category_id"))
                            .category_name = IIf(IsDBNull(dr.Item("category_name")), Nothing, dr.Item("category_name"))
                            .priority = IIf(IsDBNull(dr.Item("priority")), Nothing, dr.Item("priority"))
                            .menu_text = IIf(IsDBNull(dr.Item("menu_text")), Nothing, dr.Item("menu_text"))
                            .navigate_url = IIf(IsDBNull(dr.Item("navigate_url")), Nothing, dr.Item("navigate_url"))
                        End With
                        ' add data to list object
                        GetMenuList.Add(ceMenu)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                cLog.ErrorLog("GetMenuList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                cLog.InfoLog("GetMenuList(Dao)", strSql.ToString)
            Finally
                ' close connection
                If Not IsNothing(Conn) Then Conn.Close()
            End Try
        End Function
#End Region

    End Class
End Namespace

