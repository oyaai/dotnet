#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMst_UnitDao
'	Class Discription	: Class of table mst_unit
'	Create User 		: Boon
'	Create Date		    : 04-06-2013
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
#End Region

Namespace Dao
    Public Class ImpMst_UnitDao
        Implements IMst_UnitDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_GetUnitForSearch
        '	Discription	    : Get data unit for search
        '	Return Value	: Ilist IMst_UnitEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetUnitForSearch(ByVal strName As String) As System.Collections.Generic.List(Of Entity.IMst_UnitEntity) Implements IMst_UnitDao.DB_GetUnitForSearch
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objUnit As Entity.ImpMst_UnitEntity

                DB_GetUnitForSearch = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,name ")
                    .AppendLine(" FROM mst_unit ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    If (Not strName Is Nothing) AndAlso strName.Trim <> String.Empty Then
                        .AppendLine(" 	AND NAME LIKE '%" & strName.Trim & "%' ")
                    End If
                    .AppendLine(" ORDER BY id ")
                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetUnitForSearch = New List(Of Entity.IMst_UnitEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objUnit = New Entity.ImpMst_UnitEntity
                    objUnit.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objUnit.name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                    DB_GetUnitForSearch.Add(objUnit)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetUnitForSearch = Nothing
                objLog.ErrorLog("DB_GetUnitForSearch(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetUnitForSearch(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CancelUnit
        '	Discription	    : Cancel data unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CancelUnit(ByVal intUnit As Integer) As Boolean Implements IMst_UnitDao.DB_CancelUnit
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_CancelUnit = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_unit ")
                    .AppendLine(" SET delete_fg = 1 ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?unit_id ")
                    ' assign parameter
                    objConn.AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    objConn.AddParameter("?unit_id", intUnit)
                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_CancelUnit = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CancelUnit = False
                objLog.ErrorLog("DB_CancelVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CancelVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CancelUnit
        '	Discription	    : Check unit_name is duplicate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean Implements IMst_UnitDao.DB_CheckIsDupUnit
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckIsDupUnit = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) AS unit_count ")
                    .AppendLine(" FROM mst_unit ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" 	AND UPPER(NAME) = UPPER(?unit) ")
                    ' assign parameter
                    objConn.AddParameter("?unit", strUnitName.ToUpper)
                    ' check unit_id
                    If intUnitId > 0 Then
                        .AppendLine(" 	AND id <> ?id ")
                        objConn.AddParameter("?id", intUnitId)
                    End If
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckIsDupUnit = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckIsDupUnit = False
                objLog.ErrorLog("DB_CheckIsDupUnit(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckIsDupUnit(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_InsertUnit
        '	Discription	    : Insert data unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_InsertUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean Implements IMst_UnitDao.DB_InsertUnit
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_InsertUnit = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" INSERT INTO mst_unit ( ")
                    .AppendLine(" 	name ")
                    .AppendLine(" 	,delete_fg ")
                    .AppendLine(" 	,created_by ")
                    .AppendLine(" 	,created_date ")
                    .AppendLine(" 	,updated_by ")
                    .AppendLine(" 	,updated_date ")
                    .AppendLine(" 	) ")
                    .AppendLine(" VALUES ( ")
                    .AppendLine(" 	?unit ")
                    .AppendLine(" 	,0 ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	,?user_id ")
                    .AppendLine(" 	,date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" 	) ")
                End With

                ' assign parameter
                With objConn
                    .AddParameter("?unit", objUnit.name.Trim)
                    .AddParameter("?user_id", objUnit.created_by)
                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_InsertUnit = True
                End If

            Catch ex As Exception
                ' write error log
                DB_InsertUnit = False
                objLog.ErrorLog("DB_InsertUnit(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_InsertUnit(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_UpdateUnit
        '	Discription	    : Update data unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_UpdateUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean Implements IMst_UnitDao.DB_UpdateUnit
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlag As Integer = 0

                DB_UpdateUnit = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" UPDATE mst_unit ")
                    .AppendLine(" SET name = ?unit ")
                    .AppendLine(" 	,updated_by = ?user_id ")
                    .AppendLine(" 	,updated_date = date_format(now(), '%Y%m%d%H%i%s') ")
                    .AppendLine(" WHERE id = ?id ")
                End With

                ' assign parameter
                With objConn
                    .AddParameter("?unit", objUnit.name.Trim)
                    .AddParameter("?user_id", objUnit.created_by)
                    .AddParameter("?id", objUnit.id)
                End With

                ' execute by nonquery
                intFlag = objConn.ExecuteNonQuery(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlag > 0 Then
                    DB_UpdateUnit = True
                End If

            Catch ex As Exception
                ' write error log
                DB_UpdateUnit = False
                objLog.ErrorLog("DB_UpdateUnit(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_UpdateUnit(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetUnitForList
        '	Discription	    : Get data unit for list
        '	Return Value	: Ilist IMst_UnitEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetUnitForList() As System.Collections.Generic.List(Of Entity.IMst_UnitEntity) Implements IMst_UnitDao.DB_GetUnitForList
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objUnit As Entity.ImpMst_UnitEntity

                DB_GetUnitForList = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,name ")
                    .AppendLine(" FROM mst_unit ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine(" ORDER BY id ")
                End With

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                objDT = New System.Data.DataTable

                ' execute by datatable
                objDT = objConn.ExecuteDataTable(strSQL.ToString)
                strMsgErr = objConn.MessageError

                ' check data
                If objDT Is Nothing Then Exit Function
                If objDT.Rows.Count = 0 Then Exit Function
                DB_GetUnitForList = New List(Of Entity.IMst_UnitEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objUnit = New Entity.ImpMst_UnitEntity
                    objUnit.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objUnit.name = objConn.CheckDBNull(objItem("name"), Common.DBConnection.DBType.DBString)
                    DB_GetUnitForList.Add(objUnit)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetUnitForList = Nothing
                objLog.ErrorLog("DB_GetUnitForList(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetUnitForList(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace

