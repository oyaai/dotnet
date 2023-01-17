#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpWTDao
'	Class Discription	: Class of table mst_wt
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
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
Imports Dto
Imports System.Linq.Expressions
Imports Entity
Imports Interfaces
Imports Common.DBConnection
Imports System.Data
Imports Extensions
Imports MySql.Data.MySqlClient
Imports Exceptions
#End Region

Namespace Dao
    Public Class ImpWTDao
        Implements IWTDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

#Region "Constructors"

        Friend Sub New()
        End Sub

#End Region

#Region "Properties"

        Protected Overridable ReadOnly Property ClassName() As String Implements IWTDao.ClassName
            Get
                Return Convert.ToString(GetType(ImpWTDao))
            End Get
        End Property

#End Region

#Region "Functions"

        '/**************************************************************
        '	Function name	: GetWTList
        '	Discription	    : Get W/T list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWTList(ByVal strID As String, ByVal strWT As String) _
        As System.Collections.Generic.List(Of Entity.IWTEntity) Implements IWTDao.GetWTList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetWTList = New List(Of Entity.IWTEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objWTDetail As Entity.ImpWTEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,percent ")
                    .AppendLine(" 	,type ")
                    .AppendLine(" 	,(CASE WHEN IFNULL(acc.acc_used, 0) <= 0 THEN 0 ELSE 1 END) AS in_used  ")
                    .AppendLine(" FROM mst_wt wt ")
                    .AppendLine(" LEFT JOIN (SELECT wt_id ")
                    .AppendLine("   ,sum(acc_used) as acc_used ")
                    .AppendLine("   from (SELECT wt_id ")
                    .AppendLine("   ,COUNT(wt_id) AS acc_used  ")
                    .AppendLine(" FROM accounting ")
                    .AppendLine(" GROUP BY wt_id UNION ALL  ")
                    .AppendLine("   SELECT wt_id ")
                    .AppendLine("   ,COUNT(wt_id) AS acc_used   ")
                    .AppendLine(" FROM po_header ")
                    .AppendLine(" WHERE status_id<>6 ")
                    .AppendLine(" GROUP BY wt_id)  ")
                    .AppendLine(" A GROUP BY wt_id) acc ON wt.id = acc.wt_id ")
                    .AppendLine(" WHERE delete_fg <> 1 ")
                    .AppendLine("	AND (wt.id = IFNULL(?id, wt.id))	")
                    .AppendLine("	AND (percent = IFNULL(?percentage, percent)) 	")
                    .AppendLine(" ORDER BY wt.id; ")
                End With


                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(strID), DBNull.Value, strID))
                objConn.AddParameter("?percentage", IIf(String.IsNullOrEmpty(strWT), DBNull.Value, strWT))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objWTDetail = New Entity.ImpWTEntity
                        ' assign data from db to entity object
                        With objWTDetail
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Percent = IIf(IsDBNull(dr.Item("percent")), Nothing, dr.Item("percent"))
                            .Type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                            .IsInUsed = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))

                        End With
                        ' add W/T to list
                        GetWTList.Add(objWTDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWTList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count WT in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intWTID As Integer _
        ) As Integer Implements IWTDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(wt_id) AS used_count 				")
                    .AppendLine("		FROM accounting 				")
                    .AppendLine("		WHERE wt_id = ?id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intWTID)

                ' execute sql command
                CountUsedInPO = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CountUsedInPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CountUsedInPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: DeleteWT
        '	Discription	    : Delete WT
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteWT( _
            ByVal intWTID As Integer _
        ) As Integer Implements IWTDao.DeleteWT
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteWT = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_wt                            ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id						")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intWTID)
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
                DeleteWT = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteWT(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteWT(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetWTByID
        '	Discription	    : Get WT by ID
        '	Return Value	: IWTEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetWTByID( _
            ByVal intWTID As Integer _
        ) As IWTEntity Implements IWTDao.GetWTByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetWTByID = New Entity.ImpWTEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT 	id	")
                    .AppendLine("		,percent	")
                    .AppendLine("		,type	")
                    .AppendLine("	FROM mst_wt		")
                    .AppendLine("	WHERE id = ?id		")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intWTID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetWTByID
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Percent = IIf(IsDBNull(dr.Item("percent")), Nothing, dr.Item("percent"))
                            .Type = IIf(IsDBNull(dr.Item("type")), Nothing, dr.Item("type"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetWTByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertWT
        '	Discription	    : Insert WT to mst_wt
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertWT( _
            ByVal objWTEntity As IWTEntity _
        ) As Integer Implements IWTDao.InsertWT
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertWT = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO mst_wt (percent					")
                    .AppendLine("				,type				")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by				")
                    .AppendLine("				,created_date			")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)			")
                    .AppendLine("		VALUES (?percentage						")
                    .AppendLine("			,?type					")
                    .AppendLine("			,0					")
                    .AppendLine("			,?created_by					")
                    .AppendLine("			,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')					")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?percentage", objWTEntity.Percent)
                    .AddParameter("?type", objWTEntity.Type)
                    .AddParameter("?created_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

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
                InsertWT = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateWT(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertWT(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertWT(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateWT
        '	Discription	    : Update WT to mst_wt
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateWT( _
            ByVal objWTEntity As IWTEntity _
        ) As Integer Implements IWTDao.UpdateWT
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateWT = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_wt							")
                    .AppendLine("		SET percent = ?percentage							")
                    .AppendLine("		  ,type = ?type							")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?percentage", objWTEntity.Percent)
                    .AddParameter("?type", objWTEntity.Type)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objWTEntity.ID)

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
                UpdateWT = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateWT(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateWT(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateWT(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupWT
        ' Discription     : Check duplication WT Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 01-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupWT( _
            ByVal intWTID As String, _
            ByVal intWT As String _
        ) As Integer Implements IWTDao.CheckDupWT
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupWT = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT COUNT(id) AS cnt     ")
                    .AppendLine("  FROM mst_wt     ")
                    .AppendLine("  WHERE (delete_fg <> 1)  ")
                    .AppendLine("   AND id <> ?id     ")
                    .AppendLine("   AND percent = ?percent     ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intWTID)
                objConn.AddParameter("?percent", intWT)

                ' execute sql command
                CheckDupWT = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupWT(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupWT(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupWT(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetWTForList
        '	Discription	    : Get WT for List
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************
        Public Function GetWTForList( _
       ) As System.Collections.Generic.List(Of Entity.IWTEntity) Implements Interfaces.IWTDao.GetWTForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            ' variable connection object
            Dim objConn As New MySQLAccess
            ' variable log object
            Dim objLog As New Common.Logs.Log
            ' default return value
            GetWTForList = New List(Of IWTEntity)
            Try
                ' variable datatable object
                Dim dt As DataTable
                ' variable WT entity object
                Dim objWTEnt As IWTEntity
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT wt.`id`			")
                    .AppendLine("			 , wt.`percent` 		")
                    .AppendLine("		FROM `mst_wt` wt 			")
                    .AppendLine("		WHERE wt.`delete_fg` <> 1 			")
                    .AppendLine("		ORDER BY wt.`percent`;			")
                End With
                ' execute datatable
                dt = objConn.ExecuteDataTable(strSql.ToString)

                ' loop and transform datatable to list of entity object
                For Each drVat As DataRow In dt.Rows
                    objWTEnt = New ImpWTEntity
                    With objWTEnt
                        .ID = drVat.GetByte("id")
                        .Percent = drVat.GetNullableByte("percent")
                    End With
                    GetWTForList.Add(objWTEnt)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetWTForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                objLog.InfoLog("GetWTForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
#End Region


    End Class
End Namespace