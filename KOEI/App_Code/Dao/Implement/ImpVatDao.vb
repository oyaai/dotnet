#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpVatDao
'	Class Discription	: Class of table mst_vat
'	Create User 		: Nisa S.
'	Create Date		    : 25-06-2013
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
Imports Common.DBConnection
Imports System.Data
Imports Dto
Imports Interfaces
Imports Entity
Imports Common.Logs
Imports Microsoft.VisualBasic
Imports Extensions
Imports MySql.Data.MySqlClient
Imports Exceptions
#End Region

' DAO = Database Access Object
Namespace Dao

    Public Class ImpVatDao
        Implements IVatDao

#Region "Fields"
        Private _logger As Common.Logs.Log

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty
#End Region

#Region "Constructors"

        Friend Sub New()
            _logger = New Common.Logs.Log()
        End Sub

#End Region

#Region "Properties"

        Protected Overridable ReadOnly Property ClassName() As String Implements IVatDao.ClassName
            Get
                Return Convert.ToString(GetType(ImpVatDao))
            End Get
        End Property

        Private ReadOnly Property PageNameFormat() As String
            Get
                Return ClassName & "{0}()"
            End Get
        End Property

#End Region

#Region "Functions"

        '/**************************************************************
        '	Function name	: GetVatList
        '	Discription	    : Get vat list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVatList(ByVal strID As String, ByVal strPercent As String) As System.Collections.Generic.List(Of ImpVatDetailEntity) Implements IVatDao.GetVatList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetVatList = New List(Of ImpVatDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objVatDetail As ImpVatDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT id			")
                    .AppendLine("		, percent		")
                    .AppendLine("		, (CASE WHEN IFNULL(acc.acc_used, 0) <= 0 THEN 0 ELSE 1 END) AS in_used		")
                    .AppendLine("	FROM mst_vat vat			")
                    .AppendLine("	LEFT JOIN (SELECT vat_id			")
                    .AppendLine("		, sum(acc_used) as acc_used  		")
                    .AppendLine("	FROM (SELECT vat_id		")
                    .AppendLine("		, COUNT(vat_id) AS acc_used 		")
                    .AppendLine("	FROM accounting			")
                    .AppendLine("	GROUP BY vat_id Union All ")
                    .AppendLine("   SELECT vat_id ")
                    .AppendLine("		, COUNT(vat_id) AS acc_used 		")
                    .AppendLine("	FROM po_header  		")
                    .AppendLine("	WHERE status_id<>6  			")
                    .AppendLine("   GROUP BY vat_id)		")
                    .AppendLine("   A GROUP BY vat_id) acc ON vat.id = acc.vat_id			")
                    .AppendLine("	WHERE (delete_fg <> 1) 			")
                    .AppendLine("	AND (id = IFNULL(?id, id))			")
                    .AppendLine("	AND (percent = IFNULL(?percentage, percent))			")
                    .AppendLine("	ORDER BY vat.id;			")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", IIf(String.IsNullOrEmpty(strID), DBNull.Value, strID))
                objConn.AddParameter("?percentage", IIf(String.IsNullOrEmpty(strPercent), DBNull.Value, strPercent))


                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objVatDetail = New ImpVatDetailEntity
                        ' assign data from db to entity object
                        With objVatDetail
                            .ID = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .Percent = IIf(IsDBNull(dr.Item("percent")), Nothing, dr.Item("percent"))
                            .inuse = IIf(IsDBNull(dr.Item("in_used")), Nothing, dr.Item("in_used"))
                        End With
                        ' add vat to list
                        GetVatList.Add(objVatDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetVatList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CountUsedInPO
        '	Discription	    : Count vat in used PO_Detail
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CountUsedInPO( _
            ByVal intVatID As Integer _
        ) As Integer Implements IVatDao.CountUsedInPO
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CountUsedInPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(vat_id) AS used_count 				")
                    .AppendLine("		FROM accounting 				")
                    .AppendLine("		WHERE vat_id = ?ID				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ID", intVatID)

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
        '	Function name	: DeleteVat
        '	Discription	    : Delete vat
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteVat( _
            ByVal intVatID As Integer _
        ) As Integer Implements IVatDao.DeleteVat
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteVat = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE mst_vat                            ")
                    .AppendLine("		SET delete_fg = 1,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;						")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intVatID)
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
                DeleteVat = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteVat(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteVat(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetVatByID
        '	Discription	    : Get Vat by ID
        '	Return Value	: IVatEntity Object
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetVatByID( _
            ByVal intVatID As Integer _
        ) As IVatEntity Implements IVatDao.GetVatByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetVatByID = New Entity.ImpVatEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("	SELECT *  	")
                    .AppendLine("	FROM `mst_vat` vat 	")
                    .AppendLine("	WHERE (vat.`delete_fg` <> 1)	")
                    .AppendLine("	AND (vat.`id` = ?id)		")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intVatID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetVatByID
                            .ID = IIf(IsDBNull(dr.Item("ID")), Nothing, dr.Item("ID"))
                            .Percent = IIf(IsDBNull(dr.Item("Percent")), Nothing, dr.Item("Percent"))
                        End With
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetVatByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertVat
        '	Discription	    : Insert vat to mst_vat
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertVat( _
            ByVal objVatEnt As IVatEntity _
        ) As Integer Implements IVatDao.InsertVat
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertVat = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		INSERT INTO `mst_vat` (percent						")
                    .AppendLine("				,delete_fg				")
                    .AppendLine("				,created_by 				")
                    .AppendLine("				,created_date 			")
                    .AppendLine("				,updated_by				")
                    .AppendLine("				,updated_date)				")
                    .AppendLine("		VALUES (?percentage						")
                    .AppendLine("			,0 				")
                    .AppendLine("			,?created_by				")
                    .AppendLine("			,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')					")
                    .AppendLine("	        ,?updated_by ")
                    .AppendLine("           ,DATE_FORMAT(NOW(),'%Y%m%d%H%i%s'));					")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?percentage", objVatEnt.Percent)
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
                InsertVat = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateVat(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertVat(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertVat(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateVat
        '	Discription	    : Update vat to mst_vat
        '	Return Value	: Integer
        '	Create User	    : Nisa S.
        '	Create Date	    : 26-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateVat( _
            ByVal objVatEnt As IVatEntity _
        ) As Integer Implements IVatDao.UpdateVat
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateVat = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE mst_vat						")
                    .AppendLine("		SET percent = ?percentage							")
                    .AppendLine("			,updated_by = ?updated_by						")
                    .AppendLine("		    ,updated_date = DATE_FORMAT(NOW(),'%Y%m%d%H%i%s')							")
                    .AppendLine("		WHERE id = ?id;							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn

                    ' assign parameter
                    .AddParameter("?percentage", objVatEnt.Percent)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objVatEnt.ID)

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
                UpdateVat = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateVat(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateVat(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateVat(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupVat
        ' Discription     : Check duplication Vat Master
        ' Return Value    : Integer
        ' Create User     : Nisa S.
        ' Create Date     : 26-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupVat( _
            ByVal intVatID As String, _
            ByVal intVat As String _
        ) As Integer Implements IVatDao.CheckDupVat
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckDupVat = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("  SELECT COUNT(id) AS intCount      ")
                    .AppendLine("  FROM mst_vat     ")
                    .AppendLine("  WHERE (delete_fg <> 1)   ")
                    .AppendLine("   AND id <> ?id  ")
                    .AppendLine("   AND percent = ?percent;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intVatID)
                objConn.AddParameter("?percent", intVat)


                ' execute sql command
                CheckDupVat = objConn.ExecuteScalar(strSql.ToString)
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("CheckDupVat(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupVat(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckDupVat(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetVatForList
        '	Discription	    : Get Vat for List
        '	Return Value	: List
        '	Create User	    : Komsan Luecha
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************
        Public Function GetVatForList( _
        ) As System.Collections.Generic.List(Of Interfaces.IVatEntity) Implements Interfaces.IVatDao.GetVatForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            ' variable connection object
            Dim objConn As New MySQLAccess
            ' variable log object
            Dim objLog As New Common.Logs.Log
            ' default return value
            GetVatForList = New List(Of IVatEntity)
            Try
                ' variable datatable object
                Dim dt As DataTable
                ' variable Vat entity object
                Dim objVatEnt As IVatEntity
                ' assign sql command
                With strSql
                    .AppendLine("	SELECT vat.`id`			")
                    .AppendLine("		  ,vat.`percent` 		")
                    .AppendLine("	FROM `mst_vat` vat 			")
                    .AppendLine("	WHERE vat.`delete_fg` <> 1 			")
                    .AppendLine("	ORDER BY vat.`percent`			")
                End With
                ' execute datatable
                dt = objConn.ExecuteDataTable(strSql.ToString)

                ' loop and transform datatable to list of entity object
                For Each drVat As DataRow In dt.Rows
                    objVatEnt = New ImpVatEntity
                    With objVatEnt
                        .ID = drVat.GetByte("id")
                        .Percent = drVat.GetNullableByte("percent")
                    End With
                    GetVatForList.Add(objVatEnt)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetVatForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                objLog.InfoLog("GetVatForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
#End Region

    End Class

End Namespace