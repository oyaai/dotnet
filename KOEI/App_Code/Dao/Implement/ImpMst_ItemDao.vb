#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_ItemDao
'	Class Discription	: Class of table mst_item
'	Create User 		: Boon
'	Create Date		    : 17-05-2013
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
Imports MySql.Data.MySqlClient
Imports System.Exception

#End Region

Namespace Dao
    Public Class ImpMst_ItemDao
        Implements IMst_ItemDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Function"
        '/**************************************************************
        '	Function name	: DB_GetListItem
        '	Discription	    : Get data item to list
        '	Return Value	: IList Entity IMst_ItemEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetListItem(Optional ByVal strVendorId As String = Nothing) As System.Collections.Generic.List(Of Entity.ImpMst_ItemEntity) Implements IMst_ItemDao.DB_GetListItem
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            DB_GetListItem = Nothing
            Try
                ' data reader object
                Dim objDT As New System.Data.DataTable
                Dim objItemEntity As Entity.ImpMst_ItemEntity

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT id AS item_id			")
                    .AppendLine("			,name AS item_name		")
                    .AppendLine("	FROM mst_item 					")
                    .AppendLine("	WHERE delete_fg <> 1			")
                    .AppendLine("	AND vendor_id=?vendor_id        ")
                    .AppendLine("	OR ISNULL(?vendor_id)       ")
                    .AppendLine("	ORDER BY id;					")
                    objConn.AddParameter("?vendor_id", strVendorId)
                End With

                ' execute datatable
                objDT = objConn.ExecuteDataTable(strSql.ToString)

                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetListItem = New List(Of Entity.ImpMst_ItemEntity)
                ' check exist data
                For Each objItem As System.Data.DataRow In objDT.Rows
                    ' new object entity
                    objItemEntity = New Entity.ImpMst_ItemEntity
                    ' assign data from db to entity object
                    With objItemEntity
                        .id = objConn.CheckDBNull(objItem("item_id"), Common.DBConnection.DBType.DBDecimal)
                        .name = objConn.CheckDBNull(objItem("item_name"), Common.DBConnection.DBType.DBString)
                    End With
                    ' add item to list
                    DB_GetListItem.Add(objItemEntity)
                Next

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DB_GetListItem(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("DB_GetListItem(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CheckItemByVendor
        '	Discription	    : Check Item by vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckItemByVendor(ByVal intVendor_id As Integer) As Boolean Implements IMst_ItemDao.DB_CheckItemByVendor
            Try
                Dim strSQL As Text.StringBuilder
                Dim intFlagCount As Integer = 0

                DB_CheckItemByVendor = False

                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder
                With strSQL
                    .AppendLine(" SELECT Count(*) As item_count ")
                    .AppendLine(" FROM mst_item ")
                    .AppendLine(" WHERE vendor_id = ?VendorId ")
                    .AppendLine(" 	AND delete_fg <> 1 ")
                    objConn.AddParameter("?VendorId", intVendor_id)
                End With

                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                If intFlagCount > 0 Then
                    DB_CheckItemByVendor = True
                End If

            Catch ex As Exception
                DB_CheckItemByVendor = False
                objLog.ErrorLog("DB_CheckItemByVendor", ex.Message.Trim)
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetItemList
        '	Discription	    : Get item list
        '	Return Value	: List
        '	Create User	    : Komsan L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetItemList( _
            ByVal strItemName As String, _
            ByVal strVendorID As String _
        ) As System.Collections.Generic.List(Of Entity.ImpMst_ItemDetailEntity) Implements IMst_ItemDao.GetItemList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetItemList = New List(Of Entity.ImpMst_ItemDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objItemDetail As Entity.ImpMst_ItemDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT vendor.id AS vendor_id									")
                    .AppendLine("			,item.id AS item_id							")
                    .AppendLine("			,item.name AS item_name							")
                    .AppendLine("			,vendor.name AS vendor_name							")
                    .AppendLine("			,(CASE WHEN IFNULL(po.used_count, 0) <= 0 THEN 0 ELSE 1 END) AS in_used							")
                    .AppendLine("	FROM mst_vendor vendor 									")
                    .AppendLine("	INNER JOIN mst_item item ON vendor.id = item.vendor_id									")
                    .AppendLine("	LEFT JOIN (SELECT item_id									")
                    .AppendLine("		,COUNT(item_id) AS used_count 								")
                    .AppendLine("	 FROM po_detail po 									")
                    .AppendLine("	 GROUP BY item_id) po ON item.id = po.item_id									")
                    .AppendLine("	WHERE (ISNULL(?item_name) OR item.name LIKE CONCAT('%', ?item_name , '%'))									")
                    .AppendLine("	AND (vendor.id = IFNULL(?vendor_id, vendor.id))									")
                    .AppendLine("	AND (vendor.delete_fg <> 1 AND item.delete_fg <> 1)									")
                    .AppendLine("	ORDER BY item.id;									")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_name", IIf(String.IsNullOrEmpty(strItemName), DBNull.Value, strItemName))
                If String.IsNullOrEmpty(strVendorID) Then
                    objConn.AddParameter("?vendor_id", DBNull.Value)
                Else
                    objConn.AddParameter("?vendor_id", CInt(strVendorID))
                End If


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objItemDetail = New Entity.ImpMst_ItemDetailEntity
                        ' assign data from db to entity object
                        With objItemDetail
                            .id = IIf(IsDBNull(dr.Item("item_id")), Nothing, dr.Item("item_id"))
                            .name = IIf(IsDBNull(dr.Item("item_name")), Nothing, dr.Item("item_name"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                            .vendor_name = IIf(IsDBNull(dr.Item("vendor_name")), Nothing, dr.Item("vendor_name"))
                            .inuse = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))
                        End With
                        ' add item to list
                        GetItemList.Add(objItemDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetItemList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetItemList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteItem
        '	Discription	    : Delete item
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteItem( _
            ByVal intItemID As Integer _
        ) As Integer Implements IMst_ItemDao.DeleteItem
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteItem = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_item                             ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intItemID)
                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If
                ' set value to return variable
                DeleteItem = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteItem(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteItem(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetItemByID
        '	Discription	    : Get Item by ID
        '	Return Value	: IMst_ItemEntity Object
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetItemByID( _
            ByVal intItemID As Integer _
        ) As Entity.IMst_ItemEntity Implements IMst_ItemDao.GetItemByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetItemByID = New Entity.ImpMst_ItemEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,name	")
                    .AppendLine("		,vendor_id	")
                    .AppendLine("	FROM mst_item		")
                    .AppendLine("	WHERE id = ?item_id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intItemID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetItemByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .name = IIf(IsDBNull(dr.Item("name")), Nothing, dr.Item("name"))
                            .vendor_id = IIf(IsDBNull(dr.Item("vendor_id")), Nothing, dr.Item("vendor_id"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetItemByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetItemByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertItem
        '	Discription	    : Insert item to mst_item
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertItem( _
            ByVal objItemEnt As Entity.IMst_ItemEntity _
        ) As Integer Implements IMst_ItemDao.InsertItem
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertItem = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_item (name						")
                    .AppendLine("				,vendor_id				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date				")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?name						")
                    .AppendLine("			,?vendor_id					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')					")
                    .AppendLine("			,NULL					")
                    .AppendLine("			,NULL);					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objItemEnt.name)
                    .AddParameter("?vendor_id", objItemEnt.vendor_id)
                    .AddParameter("?created_by", HttpContext.Current.Session("UserID"))

                    ' begin transaction
                    .BeginTrans()

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    ' check effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        .CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        .RollbackTrans()
                    End If
                End With

                ' assign return value
                InsertItem = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateItem(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertItem(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertItem(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateItem
        '	Discription	    : Update item to mst_item
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateItem( _
            ByVal objItemEnt As Entity.IMst_ItemEntity _
        ) As Integer Implements IMst_ItemDao.UpdateItem
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateItem = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_item							")
                    .AppendLine("		SET name = ?name							")
                    .AppendLine("			,vendor_id = ?vendor_id 						")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE (id = ?id);							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?name", objItemEnt.name)
                    .AddParameter("?vendor_id", objItemEnt.vendor_id)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objItemEnt.id)

                    ' begin transaction
                    .BeginTrans()

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    ' check effect
                    If intEff > 0 Then
                        ' case row effect more than 0 then commit transaction
                        .CommitTrans()
                    Else
                        ' case row effect less than 1 then rollback transaction
                        .RollbackTrans()
                    End If
                End With

                ' assign return value
                UpdateItem = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateItem(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateItem(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateItem(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count item in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 06-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intItemID As Integer _
        ) As Integer Implements IMst_ItemDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(item_id) AS used_count 				")
                    .AppendLine("		FROM po_detail po 				")
                    .AppendLine("		WHERE po.item_id = ?item_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?item_id", intItemID)

                ' execute sql command
                CountUsedInPO = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

        
    End Class
End Namespace

