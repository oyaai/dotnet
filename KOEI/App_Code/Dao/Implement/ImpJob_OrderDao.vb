#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpJob_OrderDao
'	Class Discription	: Class of table job_order
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
Imports System.Web.Configuration
Imports System.IO
Imports System.Data
Imports System.Exception
#End Region

Namespace Dao
    Public Class ImpJob_OrderDao
        Implements IJob_OrderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty
        Private strPathPO As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "PO/")
        Private strPathQuo As String = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings("FilePath") & "Quotation/")


#Region "Function"
        '/**************************************************************
        '	Function name	: DB_CheckJobOrderByPurchase
        '	Discription	    : Check data job_order
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckJobOrderByPurchase(ByVal strJobOrder As String) As Boolean Implements IJob_OrderDao.DB_CheckJobOrderByPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckJobOrderByPurchase = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine("SELECT Count(*)")
                    .AppendLine("FROM (")
                    .AppendLine("select job_order from job_order WHERE status_id <> 6 and finish_fg <> 1")
                    .AppendLine("UNION")
                    .AppendLine("select job_order from job_order_special WHERE delete_fg = 0")
                    .AppendLine(") A")
                    .AppendLine("where job_order = ?JobOrder")
                    ' assign parameter
                    objConn.AddParameter("?JobOrder", strJobOrder)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckJobOrderByPurchase = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckJobOrderByPurchase = False
                objLog.ErrorLog("DB_CheckJobOrderByPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckJobOrderByPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_GetJobOrderForList
        '	Discription	    : Get data list Job order
        '	Return Value	: Ilist of IJob_OrderEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_GetJobOrderForList() As System.Collections.Generic.List(Of Entity.IJob_OrderEntity) Implements IJob_OrderDao.DB_GetJobOrderForList
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable keep sql statement, datatable and ilist
                Dim objDT As System.Data.DataTable
                Dim objJobOrder As Entity.IJob_OrderEntity

                DB_GetJobOrderForList = Nothing

                ' set new object
                strSQL = New Text.StringBuilder

                ' assign sql statement and parameter
                With strSQL
                    .AppendLine(" SELECT id ")
                    .AppendLine(" 	,job_order ")
                    .AppendLine(" FROM job_order ")
                    .AppendLine(" ORDER BY job_order ")
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
                DB_GetJobOrderForList = New List(Of Entity.IJob_OrderEntity)

                ' assign value to entity object
                For Each objItem As System.Data.DataRow In objDT.Rows
                    objJobOrder = New Entity.ImpJob_OrderEntity
                    objJobOrder.id = objConn.CheckDBNull(objItem("id"), Common.DBConnection.DBType.DBDecimal)
                    objJobOrder.job_order = objConn.CheckDBNull(objItem("job_order"), Common.DBConnection.DBType.DBString)
                    DB_GetJobOrderForList.Add(objJobOrder)
                Next

            Catch ex As Exception
                ' write error log
                DB_GetJobOrderForList = Nothing
                objLog.ErrorLog("DB_GetJobOrderForList(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_GetJobOrderForList(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DB_CheckJobOrderByVendor
        '	Discription	    : Check Job order by vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckJobOrderByVendor(ByVal intVendor_id As Integer) As Boolean Implements IJob_OrderDao.DB_CheckJobOrderByVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckJobOrderByVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As joborder_count ")
                    .AppendLine(" FROM job_order ")
                    .AppendLine(" WHERE customer = ?VendorId ")
                    .AppendLine(" 	OR end_user = ?VendorId ")
                    ' assign parameter
                    objConn.AddParameter("?VendorId", intVendor_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckJobOrderByVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckJobOrderByVendor = False
                objLog.ErrorLog("DB_CheckJobOrderByVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckJobOrderByVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrder
        '	Discription	    : Delete Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrder( _
            ByVal intJobOrderID As Integer, _
            ByVal strJobOrder As String _
        ) As Integer Implements IJob_OrderDao.DeleteJobOrder

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrder = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect
                intEff = DelJobOrder(intJobOrderID)
                If intEff > 0 Then
                    intEff = DeleteJobOrderPO(intJobOrderID)
                    If intEff >= 0 Then
                        intEff = DeleteJobOrderQuo(intJobOrderID)
                    End If
                End If

                ' check row effect
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                DeleteJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderPO
        '	Discription	    : Delete Job Order Po
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderPO( _
            ByVal intJobOrderID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderPO = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order_po                             ")
                    .AppendLine("		SET delete_fg = 1  							")
                    .AppendLine("		WHERE job_order_id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteJobOrderPO = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: RestoreJobOrderPO
        '	Discription	    : Restore Job Order Po
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RestoreJobOrderPO( _
            ByVal intJobOrderID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            RestoreJobOrderPO = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order_po                             ")
                    .AppendLine("		SET delete_fg = 0  							")
                    .AppendLine("		WHERE job_order_id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                RestoreJobOrderPO = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RestoreJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("RestoreJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderQuo
        '	Discription	    : Delete Job Order Quo
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderQuo( _
            ByVal intJobOrderID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderQuo = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order_quo                             ")
                    .AppendLine("		SET delete_fg = 1  							")
                    .AppendLine("		WHERE job_order_id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteJobOrderQuo = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: RestoreJobOrderQuo
        '	Discription	    : Restore Job Order Quo
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RestoreJobOrderQuo( _
            ByVal intJobOrderID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            RestoreJobOrderQuo = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order_quo                             ")
                    .AppendLine("		SET delete_fg = 0  							")
                    .AppendLine("		WHERE job_order_id  = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                RestoreJobOrderQuo = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RestoreJobOrderQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("RestoreJobOrderQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DelJobOrder
        '	Discription	    : Delete Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DelJobOrder( _
            ByVal intJobOrderID As Integer _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DelJobOrder = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order                             ")
                    .AppendLine("		SET status_id = 6,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intJobOrderID)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DelJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DelJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DelJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: RestoreJobOrderData
        '	Discription	    : Restore Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RestoreJobOrderData( _
            ByVal intJobOrderID As Integer, _
            ByVal strJobOrder As String, _
            ByVal strRemark As String _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            RestoreJobOrderData = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       UPDATE job_order                             ")
                    .AppendLine("		SET status_id = 1,							")
                    .AppendLine("		    job_order = ?job_order,							")
                    .AppendLine("		    remark = ?remark,							")
                    .AppendLine("		    updated_by = ?update_by,							")
                    .AppendLine("		    updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE id = ?id							")
                End With

                ' assign parameter
                objConn.AddParameter("?update_by", HttpContext.Current.Session("UserID"))
                objConn.AddParameter("?id", intJobOrderID)
                objConn.AddParameter("?job_order", strJobOrder)
                objConn.AddParameter("?remark", strRemark)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                RestoreJobOrderData = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RestoreJobOrderData(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("RestoreJobOrderData(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteFileJobOrder
        '	Discription	    : Delete Job Order file
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteFileJobOrder( _
            ByVal strJobOrder As String _
        ) As Integer

            ' set default return value
            DeleteFileJobOrder = 0
            Try
                Dim strFolderTemp As String
                strFolderTemp = strJobOrder & "_Temp"

                'get PO job order folder path
                Dim strPathPoReal As String = strPathPO & strJobOrder
                Dim strPathPoTemp As String = strPathPO & strFolderTemp

                'get Quotation job order folder path
                Dim strPathQuoReal As String = strPathQuo & strJobOrder
                Dim strPathQuoTemp As String = strPathQuo & strFolderTemp

                'check exist folder job order po
                If Directory.Exists(strPathPoReal) And Not strPathPoReal.Equals(strPathPO) Then
                    'copy job order folder to temp folder on PO
                    My.Computer.FileSystem.CopyDirectory(strPathPoReal, strPathPoTemp, True)
                    'Delete po folder
                    Directory.Delete(strPathPoReal, True)
                End If
                'check exist folder job order Quotation
                If Directory.Exists(strPathQuoReal) And Not strPathQuoReal.Equals(strPathQuo) Then
                    'copy job order folder to temp folder on Quotation
                    My.Computer.FileSystem.CopyDirectory(strPathQuoReal, strPathQuoTemp, True)
                    'Delete Quotation folder
                    Directory.Delete(strPathQuoReal, True)
                End If

                ' set value to return variable
                DeleteFileJobOrder = 1
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteFileJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))

            End Try

        End Function

        ''/**************************************************************
        ''	Function name	: RollbackFile
        ''	Discription	    : Rollback Job Order file
        ''	Return Value	: Integer
        ''	Create User	    : Suwishaya L.
        ''	Create Date	    : 12-06-2013
        ''	Update User	    :
        ''	Update Date	    :
        ''*************************************************************/
        'Public Function RollbackFile( _
        '    ByVal strJobOrder As String _
        ') As Integer

        '    ' strSql for keep sql command
        '    Dim strSql As New Text.StringBuilder
        '    ' set default return value
        '    RollbackFile = 0
        '    Try

        '        Dim strFolderTemp As String
        '        strFolderTemp = strJobOrder & "_Temp"

        '        'get PO job order folder path
        '        Dim strPathPoReal As String = HttpContext.Current.Server.MapPath("~/FileSave/PO/" & strJobOrder)
        '        Dim strPathPoTemp As String = HttpContext.Current.Server.MapPath("~/FileSave/PO/" & strFolderTemp)

        '        'get Quotation job order folder path
        '        Dim strPathQuoReal As String = HttpContext.Current.Server.MapPath("~/FileSave/Quotation/" & strJobOrder)
        '        Dim strPathQuoTemp As String = HttpContext.Current.Server.MapPath("~/FileSave/Quotation/" & strFolderTemp)

        '        'check exist folder job order po
        '        If Directory.Exists(strPathPoTemp) Then
        '            'copy job order folder to temp folder on PO
        '            My.Computer.FileSystem.CopyDirectory(strPathPoTemp, strPathPoReal, True)
        '            'Delete po folder
        '            Directory.Delete(strPathPoTemp, True)
        '        End If
        '        'check exist folder job order Quotation
        '        If Directory.Exists(strPathQuoTemp) Then
        '            'copy job order folder to temp folder on Quotation
        '            My.Computer.FileSystem.CopyDirectory(strPathQuoTemp, strPathQuoReal, True)
        '            'Delete Quotation folder
        '            Directory.Delete(strPathQuoTemp, True)
        '        End If

        '        ' set value to return variable
        '        RollbackFile = 1
        '    Catch ex As Exception
        '        ' write error log
        '        objLog.ErrorLog("RollbackFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))

        '    End Try

        'End Function

        '/**************************************************************
        '	Function name	: RenameFolder
        '	Discription	    : Rename folder from ip address to Job Order 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RenameFolder( _
            ByVal strRenameFrom As String, _
            ByVal strRenameTo As String _
        ) As Integer

            ' set default return value
            RenameFolder = -1
            Try
                'get PO job order folder path
                Dim strPathRenameFromPo As String = strPathPO & strRenameFrom
                Dim strPathRenameToPo As String = strPathPO & strRenameTo

                'get Quotation job order folder path
                Dim strPathRenameFromQuo As String = strPathQuo & strRenameFrom
                Dim strPathRenameToQuo As String = strPathQuo & strRenameTo

                'check exist folder job order po
                If Directory.Exists(strPathRenameFromPo) And Not strPathRenameFromPo.Equals(strPathPO) Then
                    'copy job order folder to temp folder on PO
                    My.Computer.FileSystem.CopyDirectory(strPathRenameFromPo, strPathRenameToPo, True)
                    'Delete po folder
                    Directory.Delete(strPathRenameFromPo, True)
                End If
                'check exist folder job order Quotation
                If Directory.Exists(strPathRenameFromQuo) And Not strPathRenameFromQuo.Equals(strPathQuo) Then
                    'copy job order folder to temp folder on Quotation
                    My.Computer.FileSystem.CopyDirectory(strPathRenameFromQuo, strPathRenameToQuo, True)
                    'Delete Quotation folder
                    Directory.Delete(strPathRenameFromQuo, True)
                End If

                ' set value to return variable
                RenameFolder = 1
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RenameFolder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))

            End Try

        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderList
        '	Discription	    : Get Job Order list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderList( _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT j.id  ")
                    .AppendLine("		, j.job_order	 ")
                    .AppendLine("		, (CASE j.receive_po WHEN 0 THEN 'NO' WHEN 1 THEN 'YES' END) AS receive_po  ")
                    '.AppendLine("		, IF(ISNULL(j.finish_date), 'NO', 'YES') AS job_finished  ")
                    .AppendLine("		, IF(ISNULL(j.finish_fg) OR j.finish_fg = 0, 'NO', 'YES') AS job_finished  ")
                    .AppendLine("		, c.name AS customer  ")
                    .AppendLine("		, o.name AS job_order_type  ")
                    .AppendLine("		, j.part_name  ")
                    .AppendLine("		, j.part_no  ")
                    .AppendLine("		, j.total_amount   ")
                    .AppendLine("		, j.quotation_amount   ")
                    .AppendLine(" FROM job_order j 		  ")
                    .AppendLine(" LEFT JOIN mst_vendor c 		  ")
                    .AppendLine(" ON(j.customer =  c.id)		  ")
                    .AppendLine(" LEFT JOIN mst_job_type O 		  ")
                    .AppendLine(" ON (j.job_type_id = O.id) 		  ")
                    .AppendLine(" WHERE	j.status_id<> 6 	  ")
                    .AppendLine(" AND ((ISNULL(?JobOrderFrom) OR j.job_order >= ?JobOrderFrom)    ")
                    .AppendLine(" AND (ISNULL(?JobOrderTo) OR j.job_order <= ?JobOrderTo))  ")
                    .AppendLine(" AND (ISNULL(?Customer) OR c.name LIKE CONCAT('%', ?Customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?ReceivePo) OR j.receive_po = ?ReceivePo ) ")
                    .AppendLine(" AND ((ISNULL(?IssueDateFrom) OR j.issue_date >= ?IssueDateFrom)    ")
                    .AppendLine(" AND (ISNULL(?IssueDateTo) OR j.issue_date <= ?IssueDateTo))  ")
                    If objJobOrderEnt.Job_finish_search = "0" Then
                        .AppendLine(" AND ISNULL(j.finish_date)		  ")
                    Else
                        If objJobOrderEnt.Job_finish_search = "1" Then
                            .AppendLine(" AND NOT ISNULL(j.finish_date)		  ")
                        End If
                        .AppendLine(" AND ((ISNULL(?FinishDateFrom) OR j.finish_date >= ?FinishDateFrom)    ")
                        .AppendLine(" AND (ISNULL(?FinishDateTo) OR j.finish_date <= ?FinishDateTo))  ")
                    End If
                    .AppendLine(" AND (ISNULL(?PartName) OR j.part_name LIKE CONCAT('%', ?PartName , '%')) ")
                    .AppendLine(" AND (ISNULL(?PartNo) OR j.part_no LIKE CONCAT('%', ?PartNo , '%')) ")
                    If objJobOrderEnt.job_type_search <> "0" Then
                        .AppendLine(" AND (ISNULL(?JobORderType) OR j.job_type_id = ?JobORderType ) ")
                    End If
                    .AppendLine(" AND (ISNULL(?Boi) OR j.is_boi = ?Boi )  ")
                    .AppendLine(" ORDER BY j.job_order DESC ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?JobOrderFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_from_search), DBNull.Value, objJobOrderEnt.job_order_from_search))
                objConn.AddParameter("?JobOrderTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_to_search), DBNull.Value, objJobOrderEnt.job_order_to_search))
                objConn.AddParameter("?Customer", IIf(String.IsNullOrEmpty(objJobOrderEnt.customer_search), DBNull.Value, objJobOrderEnt.customer_search))
                objConn.AddParameter("?ReceivePo", IIf(String.IsNullOrEmpty(objJobOrderEnt.receive_po_search), DBNull.Value, objJobOrderEnt.receive_po_search))
                objConn.AddParameter("?IssueDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_from_search), DBNull.Value, objJobOrderEnt.issue_date_from_search))
                objConn.AddParameter("?IssueDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_to_search), DBNull.Value, objJobOrderEnt.issue_date_to_search))
                objConn.AddParameter("?FinishDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_from_search), DBNull.Value, objJobOrderEnt.finish_date_from_search))
                objConn.AddParameter("?FinishDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_to_search), DBNull.Value, objJobOrderEnt.finish_date_to_search))
                objConn.AddParameter("?PartName", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_name_search), DBNull.Value, objJobOrderEnt.part_name_search))
                objConn.AddParameter("?PartNo", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_no_search), DBNull.Value, objJobOrderEnt.part_no_search))
                objConn.AddParameter("?JobORderType", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_type_search), DBNull.Value, objJobOrderEnt.job_type_search))
                objConn.AddParameter("?Boi", IIf(String.IsNullOrEmpty(objJobOrderEnt.boi_search), DBNull.Value, objJobOrderEnt.boi_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .receive_po_Detail = IIf(IsDBNull(dr.Item("receive_po")), Nothing, dr.Item("receive_po"))
                            .job_finished_Detail = IIf(IsDBNull(dr.Item("job_finished")), Nothing, dr.Item("job_finished"))
                            .customer_Detail = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .job_order_type_Detail = IIf(IsDBNull(dr.Item("job_order_type")), Nothing, dr.Item("job_order_type"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .total_amount_Detail = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .quotation_amount_Detail = IIf(IsDBNull(dr.Item("quotation_amount")), Nothing, dr.Item("quotation_amount"))
                        End With
                        ' add Job order to list
                        GetJobOrderList.Add(objJobOrderDetail)
                    End While
                End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))

            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderReportList
        '	Discription	    : Get Job Order Report list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderReportList( _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderReportList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderReportList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("   SELECT j.id, j.job_order, j.issue_date, us.last_name AS issue_by	")
                    .AppendLine("   	, if(ifnull(c1.abbr,'')='',c1.name,c1.abbr) AS customer, j.part_name, j.part_no 											")
                    .AppendLine("   	, (CASE j.part_type WHEN 1 THEN 'S/C' WHEN 2 THEN 'D/C' ELSE '' END) AS part_type  		")
                    .AppendLine("   	, j.job_type_id, (CASE j.job_type_id WHEN 1 THEN 'X' END) AS job_new  					")
                    .AppendLine("   	, (CASE WHEN  j.job_type_id > 1 THEN 'X' END) AS job_mod  								")
                    .AppendLine("   	, j.Detail, q.quo_date, q.quo_no,cu.name AS currency, j.hontai_amount  					")
                    .AppendLine("   	, j.hontai_amount * IFNULL(b.rate, 1) AS hontai_amount_thb 								")
                    .AppendLine("   FROM job_order j 																			")
                    .AppendLine("   	LEFT JOIN mst_vendor c1 ON(j.customer =  c1.id) AND c1.type1 = 1  						")
                    .AppendLine("   	LEFT JOIN (  																			")
                    .AppendLine("   		SELECT MAX(id) AS id, job_order_id, quo_date, quo_no   								")
                    .AppendLine("   		FROM job_order_quo GROUP BY job_order_id) q ON q.job_order_id = j.id  				")
                    .AppendLine("   	LEFT JOIN mst_currency cu ON (j.currency_id = cu.id)  									")
                    .AppendLine("   	LEFT JOIN ( 																			")
                    .AppendLine("   		SELECT j.id, j.job_order, j.issue_date, j.currency_id, j.hontai_amount 				")
                    .AppendLine("   			, r.ef_date, r.rate, MAX(DATE(r.ef_date)) AS max_ef_date	 					")
                    .AppendLine("   		FROM job_order j				 													")
                    .AppendLine("   			LEFT OUTER JOIN ( 																")
                    .AppendLine("   				SELECT id, rate, ef_date, currency_id  										")
                    .AppendLine("   				FROM mst_schedule_rate  													")
                    .AppendLine("   				WHERE delete_fg <>1) r ON r.currency_id = j.currency_id		 				")
                    .AppendLine("   		WHERE DATE(r.ef_date)<= DATE(j.issue_date) 											")
                    .AppendLine("   		GROUP BY   j.id) AS b ON b.id = j.id 												")
                    .AppendLine("   	LEFT JOIN user us ON (j.person_in_charge = us.id AND us.delete_fg <> 1)  				")
                    .AppendLine("   WHERE j.status_id<> 6  																		")
                    .AppendLine(" AND ((ISNULL(?JobOrderFrom) OR j.job_order >= ?JobOrderFrom)    ")
                    .AppendLine(" AND (ISNULL(?JobOrderTo) OR j.job_order <= ?JobOrderTo))  ")
                    .AppendLine(" AND (ISNULL(?Customer) OR c1.name LIKE CONCAT('%', ?Customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?ReceivePo) OR j.receive_po = ?ReceivePo ) ")
                    .AppendLine(" AND ((ISNULL(?IssueDateFrom) OR j.issue_date >= ?IssueDateFrom)    ")
                    .AppendLine(" AND (ISNULL(?IssueDateTo) OR j.issue_date <= ?IssueDateTo))  ")
                    If objJobOrderEnt.Job_finish_search = "0" Then
                        .AppendLine(" AND ISNULL(j.finish_date)		  ")
                    Else
                        If objJobOrderEnt.Job_finish_search = "1" Then
                            .AppendLine(" AND NOT ISNULL(j.finish_date)		  ")
                        End If
                        .AppendLine(" AND ((ISNULL(?FinishDateFrom) OR j.finish_date >= ?FinishDateFrom)    ")
                        .AppendLine(" AND (ISNULL(?FinishDateTo) OR j.finish_date <= ?FinishDateTo))  ")
                    End If
                    .AppendLine(" AND (ISNULL(?PartName) OR j.part_name LIKE CONCAT('%', ?PartName , '%')) ")
                    .AppendLine(" AND (ISNULL(?PartNo) OR j.part_no LIKE CONCAT('%', ?PartNo , '%')) ")
                    If objJobOrderEnt.job_type_search <> "0" Then
                        .AppendLine(" AND (ISNULL(?JobORderType) OR j.job_type_id = ?JobORderType ) ")
                    End If
                    .AppendLine(" AND (ISNULL(?Boi) OR j.is_boi = ?Boi )  ")
                    '.AppendLine(" ORDER BY id ")
                    .AppendLine(" ORDER BY job_order ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?JobOrderFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_from_search), DBNull.Value, objJobOrderEnt.job_order_from_search))
                objConn.AddParameter("?JobOrderTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_to_search), DBNull.Value, objJobOrderEnt.job_order_to_search))
                objConn.AddParameter("?Customer", IIf(String.IsNullOrEmpty(objJobOrderEnt.customer_search), DBNull.Value, objJobOrderEnt.customer_search))
                objConn.AddParameter("?ReceivePo", IIf(String.IsNullOrEmpty(objJobOrderEnt.receive_po_search), DBNull.Value, objJobOrderEnt.receive_po_search))
                objConn.AddParameter("?IssueDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_from_search), DBNull.Value, objJobOrderEnt.issue_date_from_search))
                objConn.AddParameter("?IssueDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_to_search), DBNull.Value, objJobOrderEnt.issue_date_to_search))
                objConn.AddParameter("?FinishDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_from_search), DBNull.Value, objJobOrderEnt.finish_date_from_search))
                objConn.AddParameter("?FinishDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_to_search), DBNull.Value, objJobOrderEnt.finish_date_to_search))
                objConn.AddParameter("?PartName", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_name_search), DBNull.Value, objJobOrderEnt.part_name_search))
                objConn.AddParameter("?PartNo", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_no_search), DBNull.Value, objJobOrderEnt.part_no_search))
                objConn.AddParameter("?JobORderType", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_type_search), DBNull.Value, objJobOrderEnt.job_type_search))
                objConn.AddParameter("?Boi", IIf(String.IsNullOrEmpty(objJobOrderEnt.boi_search), DBNull.Value, objJobOrderEnt.boi_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .issue_by = IIf(IsDBNull(dr.Item("issue_by")), Nothing, dr.Item("issue_by"))
                            .customer_Detail = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .part_type_name = IIf(IsDBNull(dr.Item("part_type")), Nothing, dr.Item("part_type"))
                            .job_type_id = IIf(IsDBNull(dr.Item("job_type_id")), Nothing, dr.Item("job_type_id"))
                            .job_new = IIf(IsDBNull(dr.Item("job_new")), Nothing, dr.Item("job_new"))
                            .job_Mod = IIf(IsDBNull(dr.Item("job_mod")), Nothing, dr.Item("job_mod"))
                            .detail = IIf(IsDBNull(dr.Item("detail")), Nothing, dr.Item("detail"))
                            .quo_no = IIf(IsDBNull(dr.Item("quo_no")), Nothing, dr.Item("quo_no"))
                            .quo_date = IIf(IsDBNull(dr.Item("quo_date")), Nothing, dr.Item("quo_date"))
                            .rpt_hontai_amount = IIf(IsDBNull(dr.Item("hontai_amount")), Nothing, dr.Item("hontai_amount"))
                            .rpt_hontai_amount_thb = IIf(IsDBNull(dr.Item("hontai_amount_thb")), Nothing, dr.Item("hontai_amount_thb"))
                            .currency_name = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                        End With
                        ' add Job order to list
                        GetJobOrderReportList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderReportList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderReportList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumJobOrderReport
        '	Discription	    : Get Sum Amount Job Order Report 
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumHontaiAmountReport( _
        ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) _
        Implements IJob_OrderDao.GetSumHontaiAmountReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumHontaiAmountReport = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine("   SELECT SUM(j.hontai_amount * IFNULL(b.rate,1)) AS sum_hontai_amount 			")
                    .AppendLine("   FROM job_order j																")
                    .AppendLine("   	LEFT JOIN mst_vendor c1 ON(j.customer =  c1.id) AND c1.type1 = 1 			")
                    .AppendLine("   	LEFT JOIN mst_vendor c2 ON(j.end_user =  c2.id) AND c2.type1 = 1 			")
                    .AppendLine("   	LEFT JOIN ( 																")
                    .AppendLine("   		SELECT MAX(id) AS id, job_order_id, quo_date, quo_no  					")
                    .AppendLine("   		FROM job_order_quo 														")
                    .AppendLine("   		GROUP BY job_order_id) q ON q.job_order_id = j.id 						")
                    .AppendLine("   	LEFT JOIN (																	")
                    .AppendLine("   		SELECT j.id, j.job_order, j.issue_date, j.currency_id, j.hontai_amount	")
                    .AppendLine("   			, r.ef_date, r.rate, MAX(DATE(r.ef_date)) AS max_ef_date			")
                    .AppendLine("   		FROM job_order j														")
                    .AppendLine("   			LEFT OUTER JOIN (													")
                    .AppendLine("   				SELECT id, rate, ef_date, currency_id 							")
                    .AppendLine("   				FROM mst_schedule_rate 											")
                    .AppendLine("   				WHERE delete_fg <>1) r ON r.currency_id = j.currency_id			")
                    .AppendLine("   		WHERE DATE(r.ef_date)<= DATE(j.issue_date)								")
                    .AppendLine("   		GROUP BY   j.id) AS b ON b.id = j.id									")
                    .AppendLine("   WHERE j.status_id <> 6 															")
                    .AppendLine(" AND ((ISNULL(?JobOrderFrom) OR j.job_order >= ?JobOrderFrom)    ")
                    .AppendLine(" AND (ISNULL(?JobOrderTo) OR j.job_order <= ?JobOrderTo))  ")
                    .AppendLine(" AND (ISNULL(?Customer) OR c1.name LIKE CONCAT('%', ?Customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?ReceivePo) OR j.receive_po = ?ReceivePo ) ")
                    .AppendLine(" AND ((ISNULL(?IssueDateFrom) OR j.issue_date >= ?IssueDateFrom)    ")
                    .AppendLine(" AND (ISNULL(?IssueDateTo) OR j.issue_date <= ?IssueDateTo))  ")
                    If objJobOrderEnt.Job_finish_search = "0" Then
                        .AppendLine(" AND ISNULL(j.finish_date)		  ")
                    Else
                        If objJobOrderEnt.Job_finish_search = "1" Then
                            .AppendLine(" AND NOT ISNULL(j.finish_date)		  ")
                        End If
                        .AppendLine(" AND ((ISNULL(?FinishDateFrom) OR j.finish_date >= ?FinishDateFrom)    ")
                        .AppendLine(" AND (ISNULL(?FinishDateTo) OR j.finish_date <= ?FinishDateTo))  ")
                    End If
                    .AppendLine(" AND (ISNULL(?PartName) OR j.part_name LIKE CONCAT('%',  ?PartName , '%')) ")
                    .AppendLine(" AND (ISNULL(?PartNo) OR j.part_no LIKE CONCAT('%',  ?PartNo , '%')) ")
                    If objJobOrderEnt.job_type_search <> "0" Then
                        .AppendLine(" AND (ISNULL(?JobORderType) OR j.job_type_id = ?JobORderType ) ")
                    End If
                    .AppendLine(" AND (ISNULL(?Boi) OR j.is_boi = ?Boi )  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?JobOrderFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_from_search), DBNull.Value, objJobOrderEnt.job_order_from_search))
                objConn.AddParameter("?JobOrderTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_to_search), DBNull.Value, objJobOrderEnt.job_order_to_search))
                objConn.AddParameter("?Customer", IIf(String.IsNullOrEmpty(objJobOrderEnt.customer_search), DBNull.Value, objJobOrderEnt.customer_search))
                objConn.AddParameter("?ReceivePo", IIf(String.IsNullOrEmpty(objJobOrderEnt.receive_po_search), DBNull.Value, objJobOrderEnt.receive_po_search))
                objConn.AddParameter("?IssueDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_from_search), DBNull.Value, objJobOrderEnt.issue_date_from_search))
                objConn.AddParameter("?IssueDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_to_search), DBNull.Value, objJobOrderEnt.issue_date_to_search))
                objConn.AddParameter("?FinishDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_from_search), DBNull.Value, objJobOrderEnt.finish_date_from_search))
                objConn.AddParameter("?FinishDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.finish_date_to_search), DBNull.Value, objJobOrderEnt.finish_date_to_search))
                objConn.AddParameter("?PartName", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_name_search), DBNull.Value, objJobOrderEnt.part_name_search))
                objConn.AddParameter("?PartNo", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_no_search), DBNull.Value, objJobOrderEnt.part_no_search))
                objConn.AddParameter("?JobORderType", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_type_search), DBNull.Value, objJobOrderEnt.job_type_search))
                objConn.AddParameter("?Boi", IIf(String.IsNullOrEmpty(objJobOrderEnt.boi_search), DBNull.Value, objJobOrderEnt.boi_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .sum_hontai_amount = IIf(IsDBNull(dr.Item("sum_hontai_amount")), Nothing, dr.Item("sum_hontai_amount"))
                        End With
                        ' add Job order to list
                        GetSumHontaiAmountReport.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumHontaiAmountReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumHontaiAmountReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDeleteJobOrderList
        '	Discription	    : Get Job Order for display on delete job order screen
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDeleteJobOrderList( _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetDeleteJobOrderList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetDeleteJobOrderList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT j.id  ")
                    .AppendLine("   , j.job_order ")
                    .AppendLine("   ,(CASE j.receive_po WHEN 0 THEN 'NO' WHEN 1 THEN 'YES' END) AS receive_po ")
                    .AppendLine("   , v.name AS customer ,t.name AS job_order_type ")
                    .AppendLine("   ,(CASE j.is_boi WHEN 1 THEN 'BOI' WHEN 2 THEN 'Non-BOI' END)AS is_boi ")
                    .AppendLine("   , j.part_name ")
                    .AppendLine("   , j.part_no ")
                    .AppendLine("   , c.name AS currency ")
                    .AppendLine("   , j.total_amount ")
                    .AppendLine("   , u.user_name as person_in_charge ")
                    .AppendLine("   , j.remark ")
                    .AppendLine(" FROM job_order J  ")
                    .AppendLine(" LEFT JOIN mst_vendor v ON(j.customer =  v.id)  ")
                    .AppendLine(" LEFT JOIN mst_job_type t ON (j.job_type_id = t.id)  ")
                    .AppendLine(" LEFT JOIN mst_currency c ON j.currency_id = c.id  ")
                    .AppendLine(" LEFT JOIN user u ON j.person_in_charge = u.id  ")
                    .AppendLine(" WHERE v.type1 = 1 AND j.status_id=6   ")
                    .AppendLine(" AND ((ISNULL(?JobOrderFrom) OR j.job_order >= ?JobOrderFrom)    ")
                    .AppendLine(" AND (ISNULL(?JobOrderTo) OR j.job_order <= ?JobOrderTo))  ")
                    .AppendLine(" AND ((ISNULL(?IssueDateFrom) OR j.issue_date >= ?IssueDateFrom)    ")
                    .AppendLine(" AND (ISNULL(?IssueDateTo) OR j.issue_date <= ?IssueDateTo))  ")
                    .AppendLine(" AND (ISNULL(?Customer) OR c.name LIKE CONCAT('%', ?Customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?PartName) OR j.part_name LIKE CONCAT('%', ?PartName , '%')) ")
                    .AppendLine(" AND (ISNULL(?PartNo) OR j.part_no LIKE CONCAT('%', ?PartNo , '%')) ")
                    .AppendLine(" AND (ISNULL(?ReceivePo) OR j.receive_po = ?ReceivePo ) ")
                    .AppendLine(" AND (ISNULL(?PersonInCharge) OR j.person_in_charge = ?PersonInCharge ) ")
                    If objJobOrderEnt.job_type_search <> "0" Then
                        .AppendLine(" AND (ISNULL(?JobORderType) OR j.job_type_id = ?JobORderType ) ")
                    End If
                    .AppendLine(" AND (ISNULL(?Boi) OR j.is_boi = ?Boi )  ")
                    .AppendLine(" ORDER BY j.id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?JobOrderFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_from_search), DBNull.Value, objJobOrderEnt.job_order_from_search))
                objConn.AddParameter("?JobOrderTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_order_to_search), DBNull.Value, objJobOrderEnt.job_order_to_search))
                objConn.AddParameter("?Customer", IIf(String.IsNullOrEmpty(objJobOrderEnt.customer_search), DBNull.Value, objJobOrderEnt.customer_search))
                objConn.AddParameter("?ReceivePo", IIf(String.IsNullOrEmpty(objJobOrderEnt.receive_po_search), DBNull.Value, objJobOrderEnt.receive_po_search))
                objConn.AddParameter("?IssueDateFrom", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_from_search), DBNull.Value, objJobOrderEnt.issue_date_from_search))
                objConn.AddParameter("?IssueDateTo", IIf(String.IsNullOrEmpty(objJobOrderEnt.issue_date_to_search), DBNull.Value, objJobOrderEnt.issue_date_to_search))
                objConn.AddParameter("?PartName", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_name_search), DBNull.Value, objJobOrderEnt.part_name_search))
                objConn.AddParameter("?PartNo", IIf(String.IsNullOrEmpty(objJobOrderEnt.part_no_search), DBNull.Value, objJobOrderEnt.part_no_search))
                objConn.AddParameter("?JobORderType", IIf(String.IsNullOrEmpty(objJobOrderEnt.job_type_search), DBNull.Value, objJobOrderEnt.job_type_search))
                objConn.AddParameter("?PersonInCharge", IIf(String.IsNullOrEmpty(objJobOrderEnt.person_charge_search), DBNull.Value, objJobOrderEnt.person_charge_search))
                objConn.AddParameter("?Boi", IIf(String.IsNullOrEmpty(objJobOrderEnt.boi_search), DBNull.Value, objJobOrderEnt.boi_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .receive_po_Detail = IIf(IsDBNull(dr.Item("receive_po")), Nothing, dr.Item("receive_po"))
                            .customer_Detail = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .job_order_type_Detail = IIf(IsDBNull(dr.Item("job_order_type")), Nothing, dr.Item("job_order_type"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .total_amount_Detail = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .is_boi_name = IIf(IsDBNull(dr.Item("is_boi")), Nothing, dr.Item("is_boi"))
                            .currency_name = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                        End With
                        ' add job order to list
                        GetDeleteJobOrderList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDeleteJobOrderList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetDeleteJobOrderList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInPodetail 
        '	Discription	    : Count job order in used po_detail table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInPodetail( _
            ByVal strJobOrder As String _
        ) As Integer Implements IJob_OrderDao.CheckUseInPodetail
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInPodetail = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM po_detail 				")
                    .AppendLine("		WHERE job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CheckUseInPodetail = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInPodetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInPodetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInOrderPo
        '	Discription	    : Count job order in used job_order_po table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInOrderPo( _
            ByVal intJobOrderID As Integer _
        ) As Integer Implements IJob_OrderDao.CheckUseInOrderPo
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInOrderPo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_po  				")
                    .AppendLine("		WHERE po_fg = 1			")
                    .AppendLine("		AND job_order_id  = ?job_order_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckUseInOrderPo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInOrderPo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInOrderPo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInJobOrderPo
        '	Discription	    : Count job order in used job_order_po table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInJobOrderPo( _
            ByVal intJobOrderID As Integer _
        ) As Integer Implements IJob_OrderDao.CheckUseInJobOrderPo
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInJobOrderPo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		    FROM job_order_po  				")
                    .AppendLine("		WHERE delete_fg <> 1			")
                    .AppendLine("		AND (po_fg=1 OR hontai_fg1 = 1  OR hontai_fg2 = 1 OR hontai_fg3 = 1)			")
                    .AppendLine("		AND job_order_id  = ?job_order_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckUseInJobOrderPo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInJobOrderPo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInJobOrderPo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInOrderQuo
        '	Discription	    : Count job order in used job_order_po table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInOrderQuo( _
            ByVal intJobOrderID As Integer _
        ) As Integer Implements IJob_OrderDao.CheckUseInOrderQuo
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInOrderQuo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_quo   				")
                    .AppendLine("		WHERE delete_fg <> 1			")
                    .AppendLine("		AND job_order_id  = ?job_order_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckUseInOrderQuo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInOrderQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInOrderQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInRecDetail
        '	Discription	    : Count job order in used receive_detail table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInRecDetail( _
            ByVal intJobOrderID As Integer _
        ) As Integer Implements IJob_OrderDao.CheckUseInRecDetail
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInRecDetail = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM receive_detail   				")
                    .AppendLine("		WHERE job_order_id  = ?job_order_id				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckUseInRecDetail = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInRecDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInRecDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInAccounting
        '	Discription	    : Count job order in used accounting  table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInAccounting( _
            ByVal strJobOrder As String _
        ) As Integer Implements IJob_OrderDao.CheckUseInAccounting
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInAccounting = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM accounting  				")
                    .AppendLine("		WHERE job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CheckUseInAccounting = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInAccounting(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInAccounting(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInStock
        '	Discription	    : Count job order in used stock table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInStock( _
            ByVal strJobOrder As String _
        ) As Integer Implements IJob_OrderDao.CheckUseInStock
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInStock = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM stock ")
                    .AppendLine("		WHERE job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CheckUseInStock = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInStock(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInStock(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckUseInStockOut
        '	Discription	    : Count job order in used stock_out table
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckUseInStockOut( _
            ByVal strJobOrder As String _
        ) As Integer Implements IJob_OrderDao.CheckUseInStockOut
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckUseInStockOut = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM stock_out  ")
                    .AppendLine("		WHERE job_order = ?job_order				")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", strJobOrder)

                ' execute sql command
                CheckUseInStockOut = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckUseInStockOut(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckUseInStockOut(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistJobOrderPO
        '	Discription	    : Count job order PO
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistJobOrderPO( _
            ByVal intJobOrderID As Integer _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistJobOrderPO = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_po  ")
                    .AppendLine("		WHERE job_order_id = ?job_order_id				")
                End With

                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckExistJobOrderPO = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistJobOrderQuo
        '	Discription	    : Count job order Quo
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistJobOrderQuo( _
            ByVal intJobOrderID As Integer _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistJobOrderQuo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_quo  ")
                    .AppendLine("		WHERE job_order_id = ?job_order_id				")
                End With

                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute sql command
                CheckExistJobOrderQuo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistJobOrderQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistJobOrderQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderPOTemp
        '	Discription	    : Delete Job Order Po Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderPOTemp( _
            ByVal strIpAddress As String _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderPOTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'check exist data on job_order_po
                intEff = CheckExistJobOrderPOTemp(strIpAddress)
                If intEff = 0 Then
                    DeleteJobOrderPOTemp = 1
                    Exit Function
                End If

                ' assign sql command
                With strSql
                    .AppendLine("   DELETE FROM  job_order_po_tmp ")
                    .AppendLine("   WHERE ip_address = ?ip_address")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteJobOrderPOTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderPOTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderPOTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteJobPOTemp
        '	Discription	    : Delete Job Order Po Temp on case update job order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobPOTemp( _
            ByVal strJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobPOTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'check exist data on job_order_po
                intEff = CheckExistJobOrderPOTemp(strIpAddress)
                If intEff = 0 Then
                    DeleteJobPOTemp = 1
                    Exit Function
                End If

                ' assign sql command
                With strSql
                    .AppendLine("   DELETE FROM  job_order_po_tmp ")
                    .AppendLine("   WHERE ip_address = ?ip_address")
                    .AppendLine("   AND job_order_id = ?job_order_id")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)
                objConn.AddParameter("?job_order_id", strJobOrderId)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                'loginfo for Debug At 20140826
                objLog.InfoLog("ip_address", strIpAddress)
                objLog.InfoLog("job_order_id", strJobOrderId)

                ' set value to return variable
                DeleteJobPOTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobPOTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobPOTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderQuoTemp
        '	Discription	    : Delete Job Order Quo Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderQuoTemp( _
            ByVal strIpAddress As String _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderQuoTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'check exist data on job_order_quo Temp
                intEff = CheckExistJobOrderQuoTemp(strIpAddress)
                If intEff = 0 Then
                    DeleteJobOrderQuoTemp = 1
                    Exit Function
                End If
                ' assign sql command
                With strSql
                    .AppendLine("   DELETE FROM  job_order_quo_tmp ")
                    .AppendLine("   WHERE ip_address = ?ip_address  ")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteJobOrderQuoTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderQuoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: DeleteJobQuoTemp
        '	Discription	    : Delete Job Order Quo Temp on case update job order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobQuoTemp( _
             ByVal strJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobQuoTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                'check exist data on job_order_quo Temp
                intEff = CheckExistJobOrderQuoTemp(strIpAddress)
                If intEff = 0 Then
                    DeleteJobQuoTemp = 1
                    Exit Function
                End If
                ' assign sql command
                With strSql
                    .AppendLine("   DELETE FROM  job_order_quo_tmp ")
                    .AppendLine("   WHERE ip_address = ?ip_address  ")
                    .AppendLine("   AND job_order_id = ?job_order_id")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)
                objConn.AddParameter("?job_order_id", strJobOrderId)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                ' set value to return variable
                DeleteJobQuoTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobQuoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: CheckExistJobOrderPOTemp
        '	Discription	    : Count job order PO
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistJobOrderPOTemp( _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistJobOrderPOTemp = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_po_tmp  ")
                    .AppendLine("		WHERE ip_address = ?ip_address				")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute sql command
                CheckExistJobOrderPOTemp = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistJobOrderPOTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistJobOrderPOTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistJobOrderQuoTemp
        '	Discription	    : Count job order Quo Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistJobOrderQuoTemp( _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistJobOrderQuoTemp = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine("		SELECT COUNT(*) AS used_count 				")
                    .AppendLine("		FROM job_order_quo_tmp  ")
                    .AppendLine("		WHERE ip_address = ?ip_address				")
                End With

                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute sql command
                CheckExistJobOrderQuoTemp = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistJobOrderQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistJobOrderQuoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderTemp
        '	Discription	    : Delete Job Order Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderTemp( _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.DeleteJobOrderTemp

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobOrderTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect

                intEff = DeleteJobOrderPOTemp(strIpAddress)
                If intEff > 0 Then
                    intEff = DeleteJobOrderQuoTemp(strIpAddress)
                End If

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                DeleteJobOrderTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobOrderTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function


        '/**************************************************************
        '	Function name	: DeleteJobTemp
        '	Discription	    : Delete Job Order Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobTemp( _
            ByVal intJobOrderID As Integer, _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.DeleteJobTemp

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteJobTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect

                intEff = DeleteJobPOTemp(intJobOrderID, strIpAddress)
                If intEff > 0 Then
                    intEff = DeleteJobQuoTemp(intJobOrderID, strIpAddress)
                End If

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                DeleteJobTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteJobTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderByID
        '	Discription	    : Get Job order by ID
        '	Return Value	: IJob_OrderEntity Object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetJobOrderByID( _
            ByVal intJobOrderID As Integer _
        ) As Entity.IJob_OrderEntity Implements IJob_OrderDao.GetJobOrderByID
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            GetJobOrderByID = New Entity.ImpJob_OrderEntity
            Try
                ' variable datareader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine("SELECT distinct A.*,if(isnull(B.job_order_id),0,1) used FROM job_order A")
                    .AppendLine("left join (")
                    .AppendLine("	select B.* from receive_detail B")
                    .AppendLine("	join receive_header C on B.receive_header_id=C.id")
                    .AppendLine("	where C.status_id<>6")
                    .AppendLine(") B on A.id=B.job_order_id")
                    .AppendLine("WHERE A.id = ?id;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute sql command with data reader object
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetJobOrderByID
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .customer = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .end_user = IIf(IsDBNull(dr.Item("end_user")), Nothing, dr.Item("end_user"))
                            .receive_po = IIf(IsDBNull(dr.Item("receive_po")), Nothing, dr.Item("receive_po"))
                            .person_in_charge = IIf(IsDBNull(dr.Item("person_in_charge")), Nothing, dr.Item("person_in_charge"))
                            .job_type_id = IIf(IsDBNull(dr.Item("job_type_id")), Nothing, dr.Item("job_type_id"))
                            .is_boi = IIf(IsDBNull(dr.Item("is_boi")), Nothing, dr.Item("is_boi"))
                            .create_at = IIf(IsDBNull(dr.Item("create_at")), Nothing, dr.Item("create_at"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .part_type = IIf(IsDBNull(dr.Item("part_type")), Nothing, dr.Item("part_type"))
                            .payment_term_id = IIf(IsDBNull(dr.Item("payment_term_id")), Nothing, dr.Item("payment_term_id"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                            .hontai_amount = IIf(IsDBNull(dr.Item("hontai_amount")), Nothing, dr.Item("hontai_amount"))
                            .hontai_chk1 = IIf(IsDBNull(dr.Item("hontai_chk1")), Nothing, dr.Item("hontai_chk1"))
                            .hontai_date1 = IIf(IsDBNull(dr.Item("hontai_date1")), Nothing, dr.Item("hontai_date1"))
                            .hontai_amount1 = IIf(IsDBNull(dr.Item("hontai_amount1")), Nothing, dr.Item("hontai_amount1"))
                            .hontai_condition1 = IIf(IsDBNull(dr.Item("hontai_condition1")), Nothing, dr.Item("hontai_condition1"))
                            .hontai_chk2 = IIf(IsDBNull(dr.Item("hontai_chk2")), Nothing, dr.Item("hontai_chk2"))
                            .hontai_date2 = IIf(IsDBNull(dr.Item("hontai_date2")), Nothing, dr.Item("hontai_date2"))
                            .hontai_amount2 = IIf(IsDBNull(dr.Item("hontai_amount2")), Nothing, dr.Item("hontai_amount2"))
                            .hontai_condition2 = IIf(IsDBNull(dr.Item("hontai_condition2")), Nothing, dr.Item("hontai_condition2"))
                            .hontai_chk3 = IIf(IsDBNull(dr.Item("hontai_chk3")), Nothing, dr.Item("hontai_chk3"))
                            .hontai_date3 = IIf(IsDBNull(dr.Item("hontai_date3")), Nothing, dr.Item("hontai_date3"))
                            .hontai_amount3 = IIf(IsDBNull(dr.Item("hontai_amount3")), Nothing, dr.Item("hontai_amount3"))
                            .hontai_condition3 = IIf(IsDBNull(dr.Item("hontai_condition3")), Nothing, dr.Item("hontai_condition3"))
                            .total_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .quotation_amount = IIf(IsDBNull(dr.Item("quotation_amount")), Nothing, dr.Item("quotation_amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .detail = IIf(IsDBNull(dr.Item("detail")), Nothing, dr.Item("detail"))
                            .finish_fg = IIf(IsDBNull(dr.Item("finish_fg")), Nothing, dr.Item("finish_fg"))
                            '.status_id = IIf(IsDBNull(dr.Item("status_id")), Nothing, dr.Item("status_id"))
                            .created_by = IIf(IsDBNull(dr.Item("created_by")), Nothing, dr.Item("created_by"))
                            .created_date = IIf(IsDBNull(dr.Item("created_date")), Nothing, dr.Item("created_date"))
                            .create_at_remark = IIf(IsDBNull(dr.Item("create_at_remark")), Nothing, dr.Item("create_at_remark"))
                            .payment_condition_id = IIf(IsDBNull(dr.Item("payment_condition_id")), Nothing, dr.Item("payment_condition_id"))
                            .payment_condition_remark = IIf(IsDBNull(dr.Item("payment_condition_remark")), Nothing, dr.Item("payment_condition_remark"))
                            'Add 2013/09/26 (Req No.22)
                            .status_id = IIf(IsDBNull(dr.Item("used")), 0, dr.Item("used"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderByID(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderByID(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderRunning
        '	Discription	    : Get Job Order Running
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderRunning( _
            ByVal intIssueMonth As Integer, _
            ByVal intIssueYear As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderRunning
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderRunning = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT * FROM job_order_running  ")
                    .AppendLine(" WHERE job_year = ?job_year    ")
                    .AppendLine(" AND job_month = ?job_month  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_year", intIssueYear)
                objConn.AddParameter("?job_month", intIssueMonth)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .job_month_detail = IIf(IsDBNull(dr.Item("job_month")), Nothing, dr.Item("job_month"))
                            .job_year_detail = IIf(IsDBNull(dr.Item("job_year")), Nothing, dr.Item("job_year"))
                            .job_last_detail = IIf(IsDBNull(dr.Item("job_last")), Nothing, dr.Item("job_last"))
                        End With
                        ' add Country to list
                        GetJobOrderRunning.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderRunning(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderRunning(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderTemp
        '	Discription	    : Insert data Job Order Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderTemp( _
            ByVal intJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.InsertJobOrderTemp

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect

                intEff = InsertJobOrderPOTemp(intJobOrderId, strIpAddress)
                If intEff >= 0 Then
                    intEff = InsertJobOrderQuoTemp(intJobOrderId, strIpAddress)
                End If

                ' check row effect
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                InsertJobOrderTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertJobOrderTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderPOTemp
        '	Discription	    : Insert job order to job_order_po_tmp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderPOTemp( _
             ByVal intJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderPOTemp = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                objLog.InfoLog("Trace : ", "select a.*,'" & strIpAddress & "' as ip from job_order_po a where job_order_id=" & intJobOrderId.ToString(), HttpContext.Current.Session("UserName"))
                'Dim dr As MySqlDataReader = objConn.ExecuteReader("select a.*,'" & strIpAddress & "' as ip from job_order_po a where job_order_id=" & intJobOrderId.ToString())
                'If dr.HasRows Then
                '    While dr.Read
                '        objLog.InfoLog("Trace : ", dr.Item("po_no").ToString() & "," & dr.Item("po_file").ToString(), HttpContext.Current.Session("UserName"))
                '    End While
                'End If
                'dr.Close()
                ' assign sql command
                With strSql
                    .AppendLine("   INSERT INTO job_order_po_tmp 	 ")
                    .AppendLine("   (   	 ")
                    .AppendLine("	     job_order_id	 ")
                    .AppendLine("	    , po_type	 ")
                    .AppendLine("	    , po_no	 ")
                    .AppendLine("	    , po_amount	 ")
                    .AppendLine("	    , po_date	 ")
                    .AppendLine("	    , po_file	 ")
                    .AppendLine("	    , po_fg	 ")
                    .AppendLine("	    , hontai_fg1	 ")
                    .AppendLine("	    , hontai_fg2	 ")
                    .AppendLine("	    , hontai_fg3	 ")
                    .AppendLine("	    , receipt_date	 ")
                    .AppendLine("	    , ip_address) 	 ")
                    .AppendLine("	(SELECT 	 ")
                    .AppendLine("	     job_order_id	 ")
                    .AppendLine("	    , po_type ")
                    .AppendLine("	    , po_no	 ")
                    .AppendLine("	    , po_amount	 ")
                    .AppendLine("	    , po_date	 ")
                    .AppendLine("	    , po_file	 ")
                    .AppendLine("	    , po_fg	 ")
                    .AppendLine("	    , hontai_fg1	 ")
                    .AppendLine("	    , hontai_fg2	 ")
                    .AppendLine("	    , hontai_fg3	 ")
                    .AppendLine("	    , receipt_date	 ")
                    .AppendLine("	    , ?ip_address 	 ")
                    .AppendLine("	FROM job_order_po 	 ")
                    .AppendLine("	WHERE job_order_id = ?id)	 ")
                End With
                With objConn
                    ' assign parameter
                    .AddParameter("?id", intJobOrderId)
                    .AddParameter("?ip_address", strIpAddress)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                InsertJobOrderPOTemp = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertJobOrderPOTemp(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderPOTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobOrderPOTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderQuoTemp
        '	Discription	    : Insert job order to job_order_quo
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderQuoTemp( _
             ByVal intJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderQuoTemp = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   INSERT INTO job_order_quo_tmp 	 ")
                    .AppendLine("   (   	 ")
                    .AppendLine("	     job_order_id	 ")
                    .AppendLine("	    , quo_type	 ")
                    .AppendLine("	    , quo_no	 ")
                    .AppendLine("	    , quo_amount	 ")
                    .AppendLine("	    , quo_date	 ")
                    .AppendLine("	    , quo_file	 ")
                    .AppendLine("	    , ip_address) 	 ")
                    .AppendLine("	(SELECT 	 ")
                    .AppendLine("	     job_order_id	 ")
                    .AppendLine("	    , quo_type	 ")
                    .AppendLine("	    , quo_no	 ")
                    .AppendLine("	    , quo_amount	 ")
                    .AppendLine("	    , quo_date	 ")
                    .AppendLine("	    , quo_file	 ")
                    .AppendLine("	    , ?ip_address 	 ")
                    .AppendLine("	FROM job_order_quo 	 ")
                    .AppendLine("	WHERE job_order_id = ?id)	 ")
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?id", intJobOrderId)
                    .AddParameter("?ip_address", strIpAddress)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                InsertJobOrderQuoTemp = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertJobOrderQuoTemp(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobOrderQuoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumPoAmount
        '	Discription	    : Get Summary PO Amount  
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumPoAmount( _
            ByVal strIpAddress As String _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetSumPoAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumPoAmount = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(po_amount) as po_amount  ")
                    .AppendLine(" FROM job_order_po_tmp  ")
                    .AppendLine(" WHERE po_type = 0     ")
                    .AppendLine(" AND ip_address = ?ip_address  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .sum_po_amount = IIf(IsDBNull(dr.Item("po_amount")), Nothing, dr.Item("po_amount"))
                        End With
                        ' add Country to list
                        GetSumPoAmount.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumPoAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumPoAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumQuoAmount
        '	Discription	    : Get Summary Quotation Amount  
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-09-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 27-09-2013
        '*************************************************************/
        Public Function GetSumQuoAmount( _
            ByVal strIpAddress As String _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetSumQuoAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumQuoAmount = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(quo_amount) as quo_amount  ")
                    .AppendLine(" FROM job_order_quo_tmp  ")
                    'Modify 2013/09/27
                    .AppendLine(" WHERE quo_type <> 0 AND  ip_address = ?ip_address  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .sum_quo_amount = IIf(IsDBNull(dr.Item("quo_amount")), Nothing, dr.Item("quo_amount"))
                        End With
                        ' add Country to list
                        GetSumQuoAmount.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumQuoAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumQuoAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumUploadPoAmount
        '	Discription	    : Get Summary PO Amount  
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumUploadPoAmount( _
            ByVal strIpAddress As String, _
            ByVal intJobOrderID As Integer, _
            ByVal intMode As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetSumUploadPoAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumUploadPoAmount = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(po_amount) as po_amount  ")
                    'Mode 1:case insert,2:Edit
                    If intMode = 1 Then
                        .AppendLine(" FROM job_order_po_tmp  ")
                        .AppendLine(" WHERE ip_address = ?ip_address  ")
                    Else
                        .AppendLine(" FROM job_order_po  ")
                        .AppendLine(" WHERE job_order_id = ?job_order_id  ")
                    End If

                    .AppendLine(" AND po_type = 0     ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .sum_po_amount = IIf(IsDBNull(dr.Item("po_amount")), Nothing, dr.Item("po_amount"))
                        End With
                        ' add Country to list
                        GetSumUploadPoAmount.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumUploadPoAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumUploadPoAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTotalAmount
        '	Discription	    : Get Summary Total PO Amount  
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTotalAmount( _
            ByVal strIpAddress As String _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetTotalAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetTotalAmount = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(po_amount) as total_amount  ")
                    .AppendLine(" FROM job_order_po_tmp  ")
                    .AppendLine(" WHERE po_type <> 0 AND ip_address = ?ip_address  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .total_po_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                        End With
                        ' add Country to list
                        GetTotalAmount.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTotalAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetTotalAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUploadTotalAmount
        '	Discription	    : Get Summary Total PO Amount  
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUploadTotalAmount( _
            ByVal strIpAddress As String, _
            ByVal intJobOrderID As Integer, _
            ByVal intMode As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetUploadTotalAmount
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetUploadTotalAmount = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT SUM(po_amount) as total_amount  ")
                    If intMode = 1 Then
                        .AppendLine(" FROM job_order_po_tmp  ")
                        .AppendLine(" WHERE ip_address = ?ip_address  ")
                    Else
                        .AppendLine(" FROM job_order_po  ")
                        .AppendLine(" WHERE job_order_id = ?job_order_id  ")
                    End If

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)
                objConn.AddParameter("?job_order_id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .total_po_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                        End With
                        ' add Country to list
                        GetUploadTotalAmount.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUploadTotalAmount(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetUploadTotalAmount(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsJobOrder
        '	Discription	    : Insert data to job_order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsJobOrder( _
            ByVal strJobOrder As String, _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsJobOrder = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine(" INSERT INTO job_order 	 ")
                    .AppendLine("	(created_date	 ")
                    .AppendLine("	, created_by	 ")
                    .AppendLine("	, job_order	 ")
                    .AppendLine("	, issue_date	 ")
                    .AppendLine("	, customer	 ")
                    .AppendLine("	, end_user	 ")
                    .AppendLine("	, receive_po	 ")
                    .AppendLine("	, person_in_charge	 ")
                    .AppendLine("	, job_type_id	 ")
                    .AppendLine("	, is_boi	 ")
                    .AppendLine("	, create_at	 ")
                    .AppendLine("	, part_name	 ")
                    .AppendLine("	, part_no	 ")
                    .AppendLine("	, part_type	 ")
                    .AppendLine("	, payment_term_id	 ")
                    .AppendLine("	, currency_id	 ")
                    .AppendLine("	, hontai_chk1	 ")
                    .AppendLine("	, hontai_date1	 ")
                    .AppendLine("	, hontai_amount1	 ")
                    .AppendLine("	, hontai_condition1	 ")
                    .AppendLine("	, hontai_chk2	 ")
                    .AppendLine("	, hontai_date2	 ")
                    .AppendLine("	, hontai_amount2	 ")
                    .AppendLine("	, hontai_condition2	 ")
                    .AppendLine("	, hontai_chk3	 ")
                    .AppendLine("	, hontai_date3	 ")
                    .AppendLine("	, hontai_amount3	 ")
                    .AppendLine("	, hontai_condition3	 ")
                    .AppendLine("	, hontai_amount	 ")
                    .AppendLine("	, total_amount	 ")
                    'Add 2013/09/16 start
                    .AppendLine("	, quotation_amount	 ")
                    'Add 2013/09/16 end
                    .AppendLine("	, remark	 ")
                    .AppendLine("	, finish_fg	 ")
                    .AppendLine("	, status_id	 ")
                    .AppendLine("	, updated_date	 ")
                    .AppendLine("	, updated_by	 ")
                    .AppendLine("	, create_at_remark	 ")
                    .AppendLine("	, payment_condition_id	 ")
                    .AppendLine("	, payment_condition_remark  	 ")
                    .AppendLine("	, detail ) 	 ")
                    .AppendLine(" VALUES (REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	, ?user_id	 ")
                    .AppendLine("	, ?job_order	 ")
                    .AppendLine("	, ?issue_date	 ")
                    .AppendLine("	, ?customer	 ")
                    .AppendLine("	, ?end_user	 ")
                    .AppendLine("	, ?receive_po	 ")
                    .AppendLine("	, ?person_in_charge	 ")
                    .AppendLine("	, ?job_type_id	 ")
                    .AppendLine("	, ?is_boi	 ")
                    .AppendLine("	, ?create_at	 ")
                    .AppendLine("	, ?part_name	 ")
                    .AppendLine("	, ?part_no	 ")
                    .AppendLine("	, ?part_type	 ")
                    .AppendLine("	, ?payment_term_id	 ")
                    .AppendLine("	, ?currency_id	 ")
                    .AppendLine("	, ?hontai_chk1	 ")
                    .AppendLine("	, ?hontai_date1	 ")
                    .AppendLine("	, ?hontai_amount1	 ")
                    .AppendLine("	, ?hontai_condition1	 ")
                    .AppendLine("	, ?hontai_chk2	 ")
                    .AppendLine("	, ?hontai_date2	 ")
                    .AppendLine("	, ?hontai_amount2	 ")
                    .AppendLine("	, ?hontai_condition2	 ")
                    .AppendLine("	, ?hontai_chk3	 ")
                    .AppendLine("	, ?hontai_date3	 ")
                    .AppendLine("	, ?hontai_amount3	 ")
                    .AppendLine("	, ?hontai_condition3	 ")
                    .AppendLine("	, ?hontai_amount	 ")
                    .AppendLine("	, ?total_amount	 ")
                    'Add 2013/09/16 start
                    .AppendLine("	, ?quotation_amount	 ")
                    'Add 2013/09/16 end
                    .AppendLine("	, ?remark	 ")
                    .AppendLine("	, 0	 ")
                    .AppendLine("	, 1	 ")
                    .AppendLine("	, REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	, ?user_id	 ")
                    .AppendLine("	, ?create_at_remark	 ")
                    .AppendLine("	, ?payment_condition_id	 ")
                    .AppendLine("	, ?payment_condition_remark	 ")
                    .AppendLine("	, ?detail)	 ")


                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter                    
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?job_order", strJobOrder)
                    .AddParameter("?issue_date", objJobOrderEnt.issue_date)
                    .AddParameter("?customer", objJobOrderEnt.customer)
                    .AddParameter("?end_user", objJobOrderEnt.end_user)
                    .AddParameter("?receive_po", objJobOrderEnt.receive_po)
                    .AddParameter("?person_in_charge", objJobOrderEnt.person_in_charge)
                    .AddParameter("?job_type_id", objJobOrderEnt.job_type_id)
                    .AddParameter("?is_boi", objJobOrderEnt.is_boi)
                    .AddParameter("?create_at", objJobOrderEnt.create_at)
                    .AddParameter("?part_name", objJobOrderEnt.part_name)
                    .AddParameter("?part_no", objJobOrderEnt.part_no)
                    .AddParameter("?part_type", objJobOrderEnt.part_type)
                    .AddParameter("?payment_term_id", objJobOrderEnt.payment_term_id)
                    .AddParameter("?currency_id", objJobOrderEnt.currency_id)
                    .AddParameter("?hontai_chk1", objJobOrderEnt.hontai_chk1)
                    .AddParameter("?hontai_date1", objJobOrderEnt.hontai_date1)
                    .AddParameter("?hontai_amount1", objJobOrderEnt.hontai_amount1)
                    .AddParameter("?hontai_condition1", objJobOrderEnt.hontai_condition1)
                    .AddParameter("?hontai_chk2", objJobOrderEnt.hontai_chk2)
                    .AddParameter("?hontai_date2", objJobOrderEnt.hontai_date2)
                    .AddParameter("?hontai_amount2", objJobOrderEnt.hontai_amount2)
                    .AddParameter("?hontai_condition2", objJobOrderEnt.hontai_condition2)
                    .AddParameter("?hontai_chk3", objJobOrderEnt.hontai_chk3)
                    .AddParameter("?hontai_date3", objJobOrderEnt.hontai_date3)
                    .AddParameter("?hontai_amount3", objJobOrderEnt.hontai_amount3)
                    .AddParameter("?hontai_condition3", objJobOrderEnt.hontai_condition3)
                    .AddParameter("?hontai_amount", objJobOrderEnt.hontai_amount)
                    .AddParameter("?total_amount", objJobOrderEnt.total_amount)
                    .AddParameter("?quotation_amount", objJobOrderEnt.quotation_amount)
                    .AddParameter("?remark", objJobOrderEnt.remark)
                    .AddParameter("?create_at_remark", objJobOrderEnt.create_at_remark)
                    .AddParameter("?payment_condition_id", objJobOrderEnt.payment_condition_id)
                    .AddParameter("?payment_condition_remark", objJobOrderEnt.payment_condition_remark)
                    .AddParameter("?detail", objJobOrderEnt.detail)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                InsJobOrder = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsJobOrder(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertRunning
        '	Discription	    : check exist and update data on job order running
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertRunning( _
             ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertRunning = -1
            Try
                Dim intChk As Integer
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .Length = 0
                    .AppendLine(" SELECT count(*) as count ")
                    .AppendLine(" FROM job_order_running  ")
                    .AppendLine(" WHERE job_year = ?job_year    ")
                    .AppendLine(" AND job_month = ?job_month  ")

                End With

                ' assign parameter
                objConn.ClearParameter()
                objConn.AddParameter("?job_year", objJobOrderEnt.job_year)
                objConn.AddParameter("?job_month", objJobOrderEnt.job_month)

                ' execute sql command
                intChk = objConn.ExecuteScalar(strSql.ToString)

                If intChk = 0 Then
                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("   INSERT INTO job_order_running 	 ")
                        .AppendLine("   (   created_date	 ")
                        .AppendLine("	    , created_by	 ")
                        .AppendLine("	    , job_year	 ")
                        .AppendLine("	    , job_month	 ")
                        .AppendLine("	    , job_last	 ")
                        .AppendLine("	    , updated_date	 ")
                        .AppendLine("	    , updated_by)	 ")
                        .AppendLine("	VALUES ")
                        .AppendLine("	   ( REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                        .AppendLine("	    , ?user_id	 ")
                        .AppendLine("	    , ?job_year	 ")
                        .AppendLine("	    , ?job_month	 ")
                        .AppendLine("	    , ?job_last	 ")
                        .AppendLine("	    , REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                        .AppendLine("	    , ?user_id)	 ")
                    End With

                    With objConn
                        ' assign parameter 
                        objConn.ClearParameter()
                        .AddParameter("?job_year", objJobOrderEnt.job_year)
                        .AddParameter("?job_month", objJobOrderEnt.job_month)
                        .AddParameter("?job_last", objJobOrderEnt.job_last)
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With
                Else
                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("		UPDATE job_order_running 						")
                        .AppendLine("		SET job_last  = ?job_last 							")
                        .AppendLine("		  ,updated_by = ?updated_by							")
                        .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                        .AppendLine("		WHERE job_year = ?job_year							")
                        .AppendLine("		AND job_month = ?job_month							")
                    End With

                    With objConn
                        ' assign parameter
                        objConn.ClearParameter()
                        .AddParameter("?job_year", objJobOrderEnt.job_year)
                        .AddParameter("?job_month", objJobOrderEnt.job_month)
                        .AddParameter("?job_last", objJobOrderEnt.job_last)
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With
                End If

                InsertRunning = intEff

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertRunning(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertRunning(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertRunningJobNo
        '	Discription	    : check exist and update data on job order running
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertRunningJobNo( _
            ByVal strYear As String, _
            ByVal strMonth As String, _
            ByVal strJobLast As String _
        ) As Integer
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertRunningJobNo = -1
            Try
                Dim intChk As Integer
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .Length = 0
                    .AppendLine(" SELECT count(*) as count ")
                    .AppendLine(" FROM job_order_running  ")
                    .AppendLine(" WHERE job_year = ?job_year    ")
                    .AppendLine(" AND job_month = ?job_month  ")

                End With

                ' assign parameter
                objConn.ClearParameter()
                objConn.AddParameter("?job_year", strYear)
                objConn.AddParameter("?job_month", strMonth)

                ' execute sql command
                intChk = objConn.ExecuteScalar(strSql.ToString)

                If intChk = 0 Then
                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("   INSERT INTO job_order_running 	 ")
                        .AppendLine("   (   created_date	 ")
                        .AppendLine("	    , created_by	 ")
                        .AppendLine("	    , job_year	 ")
                        .AppendLine("	    , job_month	 ")
                        .AppendLine("	    , job_last	 ")
                        .AppendLine("	    , updated_date	 ")
                        .AppendLine("	    , updated_by)	 ")
                        .AppendLine("	VALUES ")
                        .AppendLine("	   ( REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                        .AppendLine("	    , ?user_id	 ")
                        .AppendLine("	    , ?job_year	 ")
                        .AppendLine("	    , ?job_month	 ")
                        .AppendLine("	    , ?job_last	 ")
                        .AppendLine("	    , REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                        .AppendLine("	    , ?user_id)	 ")
                    End With

                    With objConn
                        ' assign parameter 
                        objConn.ClearParameter()
                        .AddParameter("?job_year", strYear)
                        .AddParameter("?job_month", strMonth)
                        .AddParameter("?job_last", strJobLast)
                        .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With
                Else
                    ' assign sql command
                    With strSql
                        .Length = 0
                        .AppendLine("		UPDATE job_order_running 						")
                        .AppendLine("		SET job_last  = ?job_last 							")
                        .AppendLine("		  ,updated_by = ?updated_by							")
                        .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                        .AppendLine("		WHERE job_year = ?job_year							")
                        .AppendLine("		AND job_month = ?job_month							")
                    End With

                    With objConn
                        ' assign parameter
                        objConn.ClearParameter()
                        .AddParameter("?job_year", strYear)
                        .AddParameter("?job_month", strMonth)
                        .AddParameter("?job_last", strJobLast)
                        .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

                        ' execute sql command and return row effect to intEff variable
                        intEff = .ExecuteNonQuery(strSql.ToString)

                    End With
                End If

                InsertRunningJobNo = intEff

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertRunningJobNo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertRunningJobNo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderRunning
        '	Discription	    : Insert data to job_order_running
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderRunning( _
           ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderRunning = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   INSERT INTO job_order_running 	 ")
                    .AppendLine("   (   created_date	 ")
                    .AppendLine("	    , created_by	 ")
                    .AppendLine("	    , job_year	 ")
                    .AppendLine("	    , job_month	 ")
                    .AppendLine("	    , job_last	 ")
                    .AppendLine("	    , updated_date	 ")
                    .AppendLine("	    , updated_by)	 ")
                    .AppendLine("	VALUES ")
                    .AppendLine("	   ( REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	    , ?user_id	 ")
                    .AppendLine("	    , ?job_year	 ")
                    .AppendLine("	    , ?job_month	 ")
                    .AppendLine("	    , ?job_last	 ")
                    .AppendLine("	    , REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '') ")
                    .AppendLine("	    , ?user_id)	 ")
                End With

                With objConn
                    ' assign parameter                 
                    .AddParameter("?job_year", objJobOrderEnt.job_year)
                    .AddParameter("?job_month", objJobOrderEnt.job_month)
                    .AddParameter("?job_last", objJobOrderEnt.job_last)
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                InsertJobOrderRunning = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertJobOrderRunning(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderRunning(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobOrderRunning(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrderRunning
        '	Discription	    : Update data to job_order_running
        '	Return Value	: Integer
        '	Create User	    : Komsan L.
        '	Create Date	    : 05-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrderRunning( _
             ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateJobOrderRunning = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("		UPDATE job_order_running 						")
                    .AppendLine("		SET job_last  = ?job_last 							")
                    .AppendLine("		  ,updated_by = ?updated_by							")
                    .AppendLine("		  ,updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("		WHERE job_year = ?job_year							")
                    .AppendLine("		AND job_month = ?job_month							")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?job_year", objJobOrderEnt.job_year)
                    .AddParameter("?job_month", objJobOrderEnt.job_month)
                    .AddParameter("?job_last", objJobOrderEnt.job_last)
                    .AddParameter("?updated_by", HttpContext.Current.Session("UserID"))

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                UpdateJobOrderRunning = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateJobOrderRunning(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrderRunning(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateJobOrderRunning(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderPO
        '	Discription	    : Insert job order temp to job_order_po
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderPO( _
            ByVal strJobOrder As String, _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderPO = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   INSERT INTO job_order_po 	 ")
                    .AppendLine("   (   job_order_id 	 ")
                    .AppendLine("	    , po_type	 ")
                    .AppendLine("	    , po_no	 ")
                    .AppendLine("	    , po_amount	 ")
                    .AppendLine("	    , po_date	 ")
                    .AppendLine("	    , receipt_date	 ")
                    .AppendLine("	    , po_file	 ")
                    .AppendLine("	    , hontai_fg1	 ")
                    .AppendLine("	    , hontai_fg2	 ")
                    .AppendLine("	    , hontai_fg3	 ")
                    .AppendLine("	    , po_fg	) ")
                    .AppendLine("	(SELECT 	 ")
                    .AppendLine("	     j.id 	 ")
                    .AppendLine("	    , po_type  ")
                    .AppendLine("	    , po_no	 ")
                    .AppendLine("	    , po_amount	 ")
                    .AppendLine("	    , po_date	 ")
                    .AppendLine("	    , receipt_date	 ")
                    .AppendLine("	    , po_file	 ")
                    .AppendLine("	    , hontai_fg1	 ")
                    .AppendLine("	    , hontai_fg2	 ")
                    .AppendLine("	    , hontai_fg3	 ")
                    .AppendLine("	    , po_fg	 ")
                    .AppendLine("	FROM job_order_po_tmp 	 ")
                    .AppendLine("	LEFT JOIN job_order j  	 ")
                    .AppendLine("	ON j.job_order = ?job_order  	 ")
                    .AppendLine("	WHERE ip_address = ?ip_address)	 ")
                End With

                With objConn
                    ' assign parameter 
                    .AddParameter("?job_order", strJobOrder)
                    .AddParameter("?ip_address", strIpAddress)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    'loginfo for Debug At 20140826
                    objLog.InfoLog("job_order", strJobOrder)
                    objLog.InfoLog("ip_address", strIpAddress)


                End With

                ' assign return value
                InsertJobOrderPO = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertJobOrderPO(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderPO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobOrderPO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderQuo
        '	Discription	    : Insert job order to job_order_quo
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderQuo( _
            ByVal strJobOrder As String, _
            ByVal strIpAddress As String _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrderQuo = -1
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("   INSERT INTO job_order_quo 	 ")
                    .AppendLine("   (    job_order_id	         ")
                    .AppendLine("	    , quo_type	             ")
                    .AppendLine("	    , quo_no	             ")
                    .AppendLine("	    , quo_amount	         ")
                    .AppendLine("	    , quo_date	             ")
                    .AppendLine("	    , quo_file	)            ")
                    .AppendLine("	(SELECT 	                 ")
                    .AppendLine("	     j.id 	                ")
                    .AppendLine("	    , quo_type	 ")
                    .AppendLine("	    , quo_no	 ")
                    .AppendLine("	    , quo_amount	 ")
                    .AppendLine("	    , quo_date	 ")
                    .AppendLine("	    , quo_file	 ")
                    .AppendLine("	FROM job_order_quo_tmp 	 ")
                    .AppendLine("	LEFT JOIN job_order j  	 ")
                    .AppendLine("	ON j.job_order = ?job_order  	 ")
                    .AppendLine("	WHERE ip_address = ?ip_address)	 ")
                End With

                With objConn
                    ' assign parameter
                    .AddParameter("?job_order", strJobOrder)
                    .AddParameter("?ip_address", strIpAddress)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                    'loginfo for Debug At 20140826
                    objLog.InfoLog("job_order", strJobOrder)
                    objLog.InfoLog("ip_address", strIpAddress)


                End With

                ' assign return value
                InsertJobOrderQuo = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertJobOrderQuo(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertJobOrderQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrder
        '	Discription	    : Insert Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrder( _
            ByVal strYear As String, _
            ByVal strMonth As String, _
            ByVal strJobLast As String, _
            ByVal strJobOrder As String, _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer Implements IJob_OrderDao.InsertJobOrder

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertJobOrder = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' execute non query and keep row effect

                ' Insert data into job_order
                intEff = InsJobOrder(strJobOrder, objJobOrderEnt)

                If intEff > 0 Then
                    'Insert data into job_order_PO
                    intEff = InsertJobOrderPO(strJobOrder, objJobOrderEnt.ip_address)
                    If intEff >= 0 Then
                        'Delete data from Job_order_po_temp
                        intEff = DeleteJobOrderPOTemp(objJobOrderEnt.ip_address)
                        If intEff > 0 Then
                            'Insert data into job_order_Quo
                            intEff = InsertJobOrderQuo(strJobOrder, objJobOrderEnt.ip_address)
                            If intEff >= 0 Then
                                'Delete data from job_order_quo_temp
                                intEff = DeleteJobOrderQuoTemp(objJobOrderEnt.ip_address)
                                If intEff > 0 Then
                                    'Mod 2013/09/02
                                    'intEff = InsertRunning(objJobOrderEnt)
                                    intEff = InsertRunningJobNo(strYear, strMonth, strJobLast)
                                    If intEff > 0 Then
                                        'rename folder from ip address to job order
                                        intEff = RenameFolder(objJobOrderEnt.ip_address, strJobOrder)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                    'rollback folder from job order to ip address 
                    RenameFolder(strJobOrder, objJobOrderEnt.ip_address)
                End If

                ' set value to return variable
                InsertJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("InsertJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrder
        '	Discription	    : Update Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    : Wasan D.
        '	Update Date	    : 27-09-2013
        '*************************************************************/
        Public Function UpdateJobOrder( _
            ByVal strYear As String, _
            ByVal strMonth As String, _
            ByVal strJobLast As String, _
            ByVal strJobOrder As String, _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer Implements IJob_OrderDao.UpdateJobOrder

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateJobOrder = 0
            Try
                If strYear.Equals("") AndAlso strMonth.Equals("") Then
                    strJobOrder = objJobOrderEnt.job_order
                Else
                    If objJobOrderEnt.remark.IndexOf("Old Job Order No. :") > -1 Then
                        objJobOrderEnt.remark = objJobOrderEnt.remark & ", " & HttpContext.Current.Session("JobOrderNoOld")
                    Else
                        objJobOrderEnt.remark = objJobOrderEnt.remark & "Old Job Order No. : " & HttpContext.Current.Session("JobOrderNoOld")
                    End If
                End If
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans()
                ' execute non query and keep row effect
                ' Update data into job_order
                intEff = UpdJobOrder(strJobOrder, objJobOrderEnt)
                If intEff > 0 Then 'AndAlso CheckUseInJobOrderPo(objJobOrderEnt.id) = 0 Then Update aey
                    'delete job order po by id
                    intEff = DeletePO(objJobOrderEnt.id)
                    If intEff >= 0 Then
                        'Insert data into job_order_PO
                        intEff = InsertJobOrderPO(strJobOrder, objJobOrderEnt.ip_address)
                        If intEff >= 0 Then
                            'Delete data from Job_order_po_temp
                            intEff = DeleteJobPOTemp(objJobOrderEnt.id, objJobOrderEnt.ip_address)
                            If intEff > 0 Then
                                'delete job order quo by id
                                intEff = DeleteQuo(objJobOrderEnt.id)
                                If intEff >= 0 Then
                                    'Insert data into job_order_Quo
                                    intEff = InsertJobOrderQuo(strJobOrder, objJobOrderEnt.ip_address)
                                    If intEff >= 0 Then
                                        'Delete data from job_order_quo_temp
                                        intEff = DeleteJobQuoTemp(objJobOrderEnt.id, objJobOrderEnt.ip_address)
                                        If intEff > 0 Then
                                            'Mod 2013/09/02
                                            If Not strYear.Equals("") AndAlso Not strMonth.Equals("") Then
                                                intEff = InsertRunningJobNo(strYear, strMonth, strJobLast)
                                            End If
                                            If intEff > 0 Then
                                                'rename folder from ip address to job order
                                                If HttpContext.Current.Session("JobOrderNoOld") <> strJobOrder Then
                                                    intEff = RenameFolder(HttpContext.Current.Session("JobOrderNoOld"), strJobOrder)
                                                End If

                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

                ' check row effect
                If intEff > 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                    'rollback folder from job order to ip address
                    If HttpContext.Current.Session("JobOrderNoOld") <> strJobOrder Then
                        RenameFolder(strJobOrder, HttpContext.Current.Session("JobOrderNoOld"))
                    End If
                End If

                ' set value to return variable
                UpdateJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("UpdateJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: RestoreJobOrder
        '	Discription	    : Restore Job Order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RestoreJobOrder( _
             ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer Implements IJob_OrderDao.RestoreJobOrder

            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            RestoreJobOrder = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess

                ' begin transaction
                objConn.BeginTrans(IsolationLevel.Serializable)
                ' execute non query and keep row effect

                ' Update data into job_order
                intEff = RestoreJobOrderData(objJobOrderEnt.id, objJobOrderEnt.job_order, objJobOrderEnt.remark)
                If intEff > 0 Then
                    'Update Job order po 
                    intEff = RestoreJobOrderPO(objJobOrderEnt.id)
                    If intEff >= 0 Then
                        'Update Job order quo 
                        intEff = RestoreJobOrderQuo(objJobOrderEnt.id)
                        If intEff >= 0 Then
                            'check exist and updata/Insert data
                            intEff = InsertRunning(objJobOrderEnt)
                            If intEff > 0 Then
                                'Rename folder on server
                                intEff = RenameFolder(objJobOrderEnt.old_job_order, objJobOrderEnt.job_order)
                            End If
                        End If
                    End If
                End If

                ' check row effect
                If intEff >= 0 Then
                    ' case row effect more than 0 then commit transaction
                    objConn.CommitTrans()
                Else
                    ' case row effect less than 1 then rollback transaction
                    objConn.RollbackTrans()
                End If

                ' set value to return variable
                RestoreJobOrder = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RestoreJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("RestoreJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function

        '/**************************************************************
        '	Function name	: UpdJobOrder
        '	Discription	    : Update data to job_order
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function UpdJobOrder( _
            ByVal strJobOrder As String, _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdJobOrder = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("	UPDATE job_order							")
                    .AppendLine("	    SET issue_date = ?issue_date	 ")
                    .AppendLine("	    , customer = ?customer	 ")
                    'Add 2013/09/26 (Update job_order)
                    .AppendLine("	    , job_order  = ?job_order	 ")
                    .AppendLine("	    , end_user = ?end_user	 ")
                    .AppendLine("	    , receive_po = ?receive_po	 ")
                    .AppendLine("	    , person_in_charge = ?person_in_charge	 ")
                    .AppendLine("	    , job_type_id = ?job_type_id	 ")
                    .AppendLine("	    , is_boi = ?is_boi	 ")
                    .AppendLine("	    , create_at = ?create_at	 ")
                    .AppendLine("	    , part_name = ?part_name	 ")
                    .AppendLine("	    , part_no = ?part_no	 ")
                    .AppendLine("	    , part_type = ?part_type	 ")
                    .AppendLine("	    , payment_term_id = ?payment_term_id	 ")
                    .AppendLine("	    , currency_id=?currency_id	 ")
                    .AppendLine("	    , hontai_chk1=?hontai_chk1	 ")
                    .AppendLine("	    , hontai_date1=?hontai_date1	 ")
                    .AppendLine("	    , hontai_amount1=?hontai_amount1	 ")
                    .AppendLine("	    , hontai_condition1=?hontai_condition1	 ")
                    .AppendLine("	    , hontai_chk2=?hontai_chk2	 ")
                    .AppendLine("	    , hontai_date2=?hontai_date2	 ")
                    .AppendLine("	    , hontai_amount2=?hontai_amount2	 ")
                    .AppendLine("	    , hontai_condition2=?hontai_condition2	 ")
                    .AppendLine("	    , hontai_chk3=?hontai_chk3	 ")
                    .AppendLine("	    , hontai_date3=?hontai_date3	 ")
                    .AppendLine("	    , hontai_amount3=?hontai_amount3	 ")
                    .AppendLine("	    , hontai_condition3=?hontai_condition3	 ")
                    .AppendLine("	    , hontai_amount=?hontai_amount	 ")
                    .AppendLine("	    , total_amount=?total_amount	 ")
                    .AppendLine("	    , quotation_amount=?quotation_amount	 ")
                    .AppendLine("	    , remark=?remark	 ")
                    .AppendLine("	    , detail=?detail	 ")
                    .AppendLine("	    , create_at_remark= ?create_at_remark	 ")
                    .AppendLine("	    , payment_condition_id=?payment_condition_id	 ")
                    .AppendLine("	    , payment_condition_remark=?payment_condition_remark	 ")
                    .AppendLine("		, updated_by = ?user_id							")
                    .AppendLine("		, updated_date = REPLACE(REPLACE(REPLACE(NOW(), '-', ''), ' ', ''), ':', '')							")
                    .AppendLine("	WHERE id = ?id;")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    .AddParameter("?id", objJobOrderEnt.id)
                    .AddParameter("?job_order", strJobOrder)
                    .AddParameter("?issue_date", objJobOrderEnt.issue_date)
                    .AddParameter("?customer", objJobOrderEnt.customer)
                    .AddParameter("?end_user", objJobOrderEnt.end_user)
                    .AddParameter("?receive_po", objJobOrderEnt.receive_po)
                    .AddParameter("?person_in_charge", objJobOrderEnt.person_in_charge)
                    .AddParameter("?job_type_id", objJobOrderEnt.job_type_id)
                    .AddParameter("?is_boi", objJobOrderEnt.is_boi)
                    .AddParameter("?create_at", objJobOrderEnt.create_at)
                    .AddParameter("?part_name", objJobOrderEnt.part_name)
                    .AddParameter("?part_no", objJobOrderEnt.part_no)
                    .AddParameter("?part_type", objJobOrderEnt.part_type)
                    .AddParameter("?payment_term_id", objJobOrderEnt.payment_term_id)
                    .AddParameter("?currency_id", objJobOrderEnt.currency_id)
                    .AddParameter("?hontai_chk1", objJobOrderEnt.hontai_chk1)
                    .AddParameter("?hontai_date1", objJobOrderEnt.hontai_date1)
                    .AddParameter("?hontai_amount1", objJobOrderEnt.hontai_amount1)
                    .AddParameter("?hontai_condition1", objJobOrderEnt.hontai_condition1)
                    .AddParameter("?hontai_chk2", objJobOrderEnt.hontai_chk2)
                    .AddParameter("?hontai_date2", objJobOrderEnt.hontai_date2)
                    .AddParameter("?hontai_amount2", objJobOrderEnt.hontai_amount2)
                    .AddParameter("?hontai_condition2", objJobOrderEnt.hontai_condition2)
                    .AddParameter("?hontai_chk3", objJobOrderEnt.hontai_chk3)
                    .AddParameter("?hontai_date3", objJobOrderEnt.hontai_date3)
                    .AddParameter("?hontai_amount3", objJobOrderEnt.hontai_amount3)
                    .AddParameter("?hontai_condition3", objJobOrderEnt.hontai_condition3)
                    .AddParameter("?hontai_amount", objJobOrderEnt.hontai_amount)
                    .AddParameter("?total_amount", objJobOrderEnt.total_amount)
                    .AddParameter("?quotation_amount", objJobOrderEnt.quotation_amount)
                    .AddParameter("?remark", objJobOrderEnt.remark)
                    .AddParameter("?detail", objJobOrderEnt.detail)
                    .AddParameter("?create_at_remark", objJobOrderEnt.create_at_remark)
                    .AddParameter("?payment_condition_id", objJobOrderEnt.payment_condition_id)
                    .AddParameter("?payment_condition_remark", objJobOrderEnt.payment_condition_remark)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                UpdJobOrder = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdJobOrder(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdJobOrder(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdJobOrder(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderDetailList
        '	Discription	    : Get Job Order Detail list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderDetailList( _
            ByVal intJobOrderID As Integer _
        ) As Entity.IJob_OrderEntity Implements IJob_OrderDao.GetJobOrderDetailList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderDetailList = New Entity.ImpJob_OrderEntity
            Try
                ' data reader object
                Dim dr As MySqlDataReader

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT j.job_order	                    ")
                    .AppendLine(" 	, j.issue_date	                        ")
                    .AppendLine(" 	, v.name AS customer	                ")
                    .AppendLine(" 	, v2.name AS end_user	                ")
                    .AppendLine(" 	, (CASE  j.receive_po WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS receive_po	 ")
                    '.AppendLine(" 	, u.user_name AS person_in_charge	 ")
                    .AppendLine(" 	, CONCAT(u.first_name,' ',u.last_name) AS person_in_charge	 ")
                    .AppendLine(" 	, jt.name AS job_order_type	 ")
                    .AppendLine(" 	, (CASE  j.IS_BOI WHEN 0 THEN 'Not Selected' WHEN 1 THEN 'BOI' WHEN 2 THEN 'Non-BOI' END) AS IS_BOI	 ")
                    .AppendLine(" 	, (CASE  j.create_at WHEN 0 THEN 'Not Selected' WHEN 1 THEN concat('Own Company : ',j.create_at_remark) WHEN 2 THEN 'Other Company' END) AS create_at	 ")
                    .AppendLine(" 	, j.part_name	 ")
                    .AppendLine(" 	, j.part_no	 ")
                    .AppendLine(" 	, (CASE  j.part_type WHEN 0 THEN 'Not Selected' WHEN 1 THEN 'S/C' WHEN 2 THEN 'D/C' END) AS part_type 	 ")
                    .AppendLine(" 	, j.payment_condition_id	 ")
                    .AppendLine(" 	, p.term_day	 ")
                    .AppendLine("   , j.currency_id ")
                    .AppendLine(" 	, c.name AS currency	 ")
                    .AppendLine(" 	, j.hontai_chk1	 ")
                    .AppendLine(" 	, j.hontai_date1	 ")
                    .AppendLine(" 	, j.hontai_amount1	 ")
                    .AppendLine(" 	, j.hontai_condition1	 ")
                    .AppendLine(" 	, j.hontai_chk2	 ")
                    .AppendLine(" 	, j.hontai_date2	 ")
                    .AppendLine(" 	, j.hontai_amount2	 ")
                    .AppendLine(" 	, j.hontai_condition2	 ")
                    .AppendLine(" 	, j.hontai_chk3	 ")
                    .AppendLine(" 	, j.hontai_date3	 ")
                    .AppendLine(" 	, j.hontai_amount3	 ")
                    .AppendLine(" 	, j.hontai_condition3	 ")
                    .AppendLine(" 	, j.hontai_amount	 ")
                    .AppendLine(" 	, j.total_amount	 ")
                    .AppendLine(" 	, j.quotation_amount	 ")
                    .AppendLine(" 	, j.remark 	 ")
                    .AppendLine(" 	, CONCAT(pc.1st,'%',pc.2nd,'%',pc.3rd,'%') AS payment_condition  	 ")
                    .AppendLine(" FROM job_order J 	 ")
                    .AppendLine(" LEFT JOIN mst_vendor v 	 ")
                    .AppendLine(" ON (j.customer = v.id) 	 ")
                    .AppendLine(" LEFT JOIN mst_vendor v2 	 ")
                    .AppendLine(" ON (j.end_user = v2.id) 	 ")
                    .AppendLine(" LEFT JOIN user u 	 ")
                    .AppendLine(" ON (j.person_in_charge = u.id) 	 ")
                    .AppendLine(" LEFT JOIN mst_job_type JT 	 ")
                    .AppendLine(" ON (j.job_type_id = jt.id) 	 ")
                    .AppendLine(" LEFT JOIN mst_payment_term p 	 ")
                    .AppendLine(" ON (j.payment_term_id = p.id) 	 ")
                    .AppendLine(" LEFT JOIN mst_currency C 	 ")
                    .AppendLine(" ON (j.currency_id = c.id) 	 ")
                    .AppendLine(" LEFT JOIN mst_payment_condition pc  ")
                    .AppendLine(" ON (j.payment_condition_id = pc.id)")
                    .AppendLine(" WHERE j.id = ?id	 ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' assign data from db to entity object
                        With GetJobOrderDetailList
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .end_user_name = IIf(IsDBNull(dr.Item("end_user")), Nothing, dr.Item("end_user"))
                            .receive_po_name = IIf(IsDBNull(dr.Item("receive_po")), Nothing, dr.Item("receive_po"))
                            .person_in_charge_name = IIf(IsDBNull(dr.Item("person_in_charge")), Nothing, dr.Item("person_in_charge"))
                            .job_order_type_Detail = IIf(IsDBNull(dr.Item("job_order_type")), Nothing, dr.Item("job_order_type"))
                            .is_boi_name = IIf(IsDBNull(dr.Item("is_boi")), Nothing, dr.Item("is_boi"))
                            .create_at_name = IIf(IsDBNull(dr.Item("create_at")), Nothing, dr.Item("create_at"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .part_type_name = IIf(IsDBNull(dr.Item("part_type")), Nothing, dr.Item("part_type"))
                            .payment_condition_id = IIf(IsDBNull(dr.Item("payment_condition_id")), Nothing, dr.Item("payment_condition_id"))
                            .term_day = IIf(IsDBNull(dr.Item("term_day")), Nothing, dr.Item("term_day"))
                            .currency_id = IIf(IsDBNull(dr.Item("currency_id")), Nothing, dr.Item("currency_id"))
                            .currency_name = IIf(IsDBNull(dr.Item("currency")), Nothing, dr.Item("currency"))
                            .hontai_chk1 = IIf(IsDBNull(dr.Item("hontai_chk1")), Nothing, dr.Item("hontai_chk1"))
                            .hontai_date1 = IIf(IsDBNull(dr.Item("hontai_date1")), Nothing, dr.Item("hontai_date1"))
                            .hontai_amount1 = IIf(IsDBNull(dr.Item("hontai_amount1")), Nothing, dr.Item("hontai_amount1"))
                            .hontai_condition1 = IIf(IsDBNull(dr.Item("hontai_condition1")), Nothing, dr.Item("hontai_condition1"))
                            .hontai_chk2 = IIf(IsDBNull(dr.Item("hontai_chk2")), Nothing, dr.Item("hontai_chk2"))
                            .hontai_date2 = IIf(IsDBNull(dr.Item("hontai_date2")), Nothing, dr.Item("hontai_date2"))
                            .hontai_amount2 = IIf(IsDBNull(dr.Item("hontai_amount2")), Nothing, dr.Item("hontai_amount2"))
                            .hontai_condition2 = IIf(IsDBNull(dr.Item("hontai_condition2")), Nothing, dr.Item("hontai_condition2"))
                            .hontai_chk3 = IIf(IsDBNull(dr.Item("hontai_chk3")), Nothing, dr.Item("hontai_chk3"))
                            .hontai_date3 = IIf(IsDBNull(dr.Item("hontai_date3")), Nothing, dr.Item("hontai_date3"))
                            .hontai_amount3 = IIf(IsDBNull(dr.Item("hontai_amount3")), Nothing, dr.Item("hontai_amount3"))
                            .hontai_condition3 = IIf(IsDBNull(dr.Item("hontai_condition3")), Nothing, dr.Item("hontai_condition3"))
                            .hontai_amount = IIf(IsDBNull(dr.Item("hontai_amount")), Nothing, dr.Item("hontai_amount"))
                            .total_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .remark = IIf(IsDBNull(dr.Item("remark")), Nothing, dr.Item("remark"))
                            .payment_condition_name = IIf(IsDBNull(dr.Item("payment_condition")), Nothing, dr.Item("payment_condition"))
                            .quotation_amount = IIf(IsDBNull(dr.Item("quotation_amount")), Nothing, dr.Item("quotation_amount"))
                        End With
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderDetailList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderDetailList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderPOList
        '	Discription	    : Get Job Order PO list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderPOList( _
            ByVal intJobOrderID As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderPOList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderPOList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT  	 ")
                    .AppendLine(" 	 job_order_id	 ")
                    .AppendLine(" 	 , (CASE po_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others' END) AS po_type	 ")
                    .AppendLine(" 	 , po_no	 ")
                    .AppendLine(" 	 , po_amount	 ")
                    .AppendLine(" 	 , po_date	 ")
                    .AppendLine(" 	 , receipt_date	 ")
                    .AppendLine(" 	 , po_file 	 ")
                    .AppendLine(" FROM job_order_po   ")
                    .AppendLine(" WHERE job_order_id = ?id 	 ")
                    .AppendLine(" ORDER BY id	 ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .po_type_detail = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .po_no_detail = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_amount_detail = IIf(IsDBNull(dr.Item("po_amount")), Nothing, dr.Item("po_amount"))
                            .po_date_detail = IIf(IsDBNull(dr.Item("po_date")), Nothing, dr.Item("po_date"))
                            .po_receipt_date_detail = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .po_file_detail = IIf(IsDBNull(dr.Item("po_file")), Nothing, dr.Item("po_file"))

                        End With
                        ' add Country to list
                        GetJobOrderPOList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderPOList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderPOList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderQuoList
        '	Discription	    : Get Job Order Quo list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderQuoList( _
            ByVal intJobOrderID As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderQuoList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderQuoList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT  	 ")
                    .AppendLine(" 	 job_order_id	 ")
                    .AppendLine(" 	 ,(CASE quo_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' WHEN 4 THEN 'Others' END) AS quo_type	 ")
                    .AppendLine(" 	 , quo_no	 ")
                    .AppendLine(" 	 , quo_amount	 ")
                    .AppendLine(" 	 , quo_date	 ")
                    .AppendLine(" 	 , quo_file	 ")
                    .AppendLine(" FROM job_order_quo   ")
                    .AppendLine(" WHERE job_order_id = ?id 	 ")
                    .AppendLine(" ORDER BY id	 ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .quo_type_detail = IIf(IsDBNull(dr.Item("quo_type")), Nothing, dr.Item("quo_type"))
                            .quo_no_detail = IIf(IsDBNull(dr.Item("quo_no")), Nothing, dr.Item("quo_no"))
                            .quo_amount_detail = IIf(IsDBNull(dr.Item("quo_amount")), Nothing, dr.Item("quo_amount"))
                            .quo_date_detail = IIf(IsDBNull(dr.Item("quo_date")), Nothing, dr.Item("quo_date"))
                            .quo_file_detail = IIf(IsDBNull(dr.Item("quo_file")), Nothing, dr.Item("quo_file"))

                        End With
                        ' add Country to list
                        GetJobOrderQuoList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderQuoList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderQuoList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderInvoiceList
        '	Discription	    : Get Job Order invoice list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderInvoiceList( _
            ByVal intJobOrderID As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderInvoiceList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderInvoiceList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                'Modify By wall 29/08/2013 (Mod Spec)
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT (CASE h.status_id WHEN 4 THEN 'Paid' ELSE 'Not Pay' END) AS status	 ")
                    .AppendLine(" 	, h.invoice_no	 ")
                    .AppendLine(" 	, h.receipt_date	 ")
                    '.AppendLine(" 	, (CASE j.po_type WHEN 0 THEN (CASE d.hontai_type WHEN 1 THEN '1 st' WHEN 2 THEN '2 nd' WHEN 3 THEN '3 rd' END) WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' ELSE 'Others' END) AS po_type	 ")
                    .AppendLine(" 	, GROUP_CONCAT((CASE j.po_type WHEN 0 THEN (CASE d.hontai_type WHEN 1 THEN '1 st' WHEN 2 THEN '2 nd' WHEN 3 THEN '3 rd' END) WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' ELSE 'Others' END)) AS po_type	 ")
                    .AppendLine(" 	, CONCAT(i.code,' - ',i.name) AS account_title	 ")
                    .AppendLine(" 	, h.issue_date ")
                    .AppendLine(" 	, SUM(d.amount*d.actual_rate) AS total_amount 	 ")
                    '.AppendLine(" 	, h.total_amount AS total_amount    ")
                    .AppendLine(" FROM receive_header h 	 ")
                    .AppendLine(" LEFT JOIN mst_ie i ON h.ie_id = i.id LEFT JOIN receive_detail d ON d.receive_header_id = h.id  	 ")
                    .AppendLine(" LEFT JOIN job_order_po j ON d.job_order_po_id = j.id 	 ")
                    .AppendLine(" LEFT JOIN job_order k ON j.job_order_id = k.id 	 ")
                    .AppendLine(" WHERE h.status_id <> 6 AND d.job_order_id = ?id 	 ")
                    .AppendLine(" GROUP BY d.receive_header_id,d.job_order_id	 ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intJobOrderID)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .status = IIf(IsDBNull(dr.Item("status")), Nothing, dr.Item("status"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .inv_receipt_date = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .account_title = IIf(IsDBNull(dr.Item("account_title")), Nothing, dr.Item("account_title"))
                            .inv_issue_date = IIf(IsDBNull(dr.Item("issue_date")), Nothing, dr.Item("issue_date"))
                            .inv_total_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                            .po_type_detail = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                        End With
                        ' add Country to list
                        GetJobOrderInvoiceList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderInvoiceList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderInvoiceList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPaymentConditionDetail
        '	Discription	    : Get payment condition detail 
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentConditionDetail( _
            ByVal intPayment_condition_id As Integer _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetPaymentConditionDetail
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetPaymentConditionDetail = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT CONCAT(p.1st,'%',p.2nd,'%',p.3rd,'%')  as payment_condition_detail  ")
                    .AppendLine(" FROM mst_payment_condition p   ")
                    .AppendLine(" WHERE id = ?id  ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intPayment_condition_id)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .payment_condition_detail = IIf(IsDBNull(dr.Item("payment_condition_detail")), Nothing, dr.Item("payment_condition_detail"))
                        End With
                        ' add Country to list
                        GetPaymentConditionDetail.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentConditionDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetPaymentConditionDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderPOTempList
        '	Discription	    : Get Job Order PO Temp list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderPOTempList( _
            ByVal strIpAddress As String _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderPOTempList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderPOTempList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT  ")
                    .AppendLine("   @s:=@s+1 as no ")
                    .AppendLine("   ,id ")
                    .AppendLine("   ,(CASE po_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' ELSE 'Other' END) AS po_type ")
                    .AppendLine("   ,po_no ")
                    .AppendLine("   ,po_amount ")
                    .AppendLine("   ,po_date ")
                    .AppendLine("   ,receipt_date ")
                    .AppendLine("   ,po_file ")
                    'Select Check Use PO Add date 2014/04/23
                    .AppendLine("   , CASE   ")
                    .AppendLine("       WHEN   ")
                    .AppendLine("   hontai_fg1 = 1 or  ")
                    .AppendLine("   hontai_fg2 = 1 or ")
                    .AppendLine("   hontai_fg3 = 1 or ")
                    .AppendLine("   po_fg = 1   ")
                    .AppendLine("   then  1  else  0  ")
                    .AppendLine("   end as check_use ")
                    .AppendLine(" from job_order_po_tmp ")
                    .AppendLine("   ,(SELECT @s:= 0) AS s ")
                    .AppendLine(" WHERE	   ")
                    .AppendLine("   ip_address = ?ip_address    ")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .no = IIf(IsDBNull(dr.Item("no")), Nothing, dr.Item("no"))
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type_detail = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .po_no_detail = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_amount_detail = IIf(IsDBNull(dr.Item("po_amount")), Nothing, dr.Item("po_amount"))
                            .po_date_detail = IIf(IsDBNull(dr.Item("po_date")), Nothing, dr.Item("po_date"))
                            .po_receipt_date_detail = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .po_file_detail = IIf(IsDBNull(dr.Item("po_file")), Nothing, dr.Item("po_file"))
                            .check_use = IIf(IsDBNull(dr.Item("check_use")), Nothing, dr.Item("check_use"))
                        End With
                        ' add job order po temp to list
                        GetJobOrderPOTempList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderPOTempList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderPOTempList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderQuoTempList
        '	Discription	    : Get Job Order Quo Temp list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderQuoTempList( _
            ByVal strIpAddress As String _
        ) As System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) Implements IJob_OrderDao.GetJobOrderQuoTempList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetJobOrderQuoTempList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT  ")
                    .AppendLine("   @s:=@s+1 as no ")
                    .AppendLine("   ,id ")
                    .AppendLine("   ,(CASE quo_type WHEN 0 THEN 'Hontai' WHEN 1 THEN 'Sample' WHEN 2 THEN 'Material' WHEN 3 THEN 'Delivery' ELSE 'Other' END) AS quo_type ")
                    .AppendLine("   ,quo_no ")
                    .AppendLine("   ,quo_amount ")
                    .AppendLine("   ,quo_date ")
                    .AppendLine("   ,quo_file ")
                    .AppendLine(" from job_order_quo_tmp ")
                    .AppendLine("   ,(SELECT @s:= 0) AS s ")
                    .AppendLine(" WHERE	   ")
                    .AppendLine("   ip_address = ?ip_address    ")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .no = IIf(IsDBNull(dr.Item("no")), Nothing, dr.Item("no"))
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .quo_type_detail = IIf(IsDBNull(dr.Item("quo_type")), Nothing, dr.Item("quo_type"))
                            .quo_no_detail = IIf(IsDBNull(dr.Item("quo_no")), Nothing, dr.Item("quo_no"))
                            .quo_amount_detail = IIf(IsDBNull(dr.Item("quo_amount")), Nothing, dr.Item("quo_amount"))
                            .quo_date_detail = IIf(IsDBNull(dr.Item("quo_date")), Nothing, dr.Item("quo_date"))
                            .quo_file_detail = IIf(IsDBNull(dr.Item("quo_file")), Nothing, dr.Item("quo_file"))
                        End With
                        ' add job order po temp to list
                        GetJobOrderQuoTempList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderQuoTempList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetJobOrderQuoTempList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoType
        '	Discription	    : check exist PO type on job order po temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoType( _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.CheckExistPoType
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistPoType = -1
            Try

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt  ")
                    .AppendLine(" FROM job_order_po_tmp ")
                    .AppendLine(" WHERE po_type = 0    ")
                    .AppendLine(" AND ip_address = ?ip_address   ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)

                ' execute sql command
                CheckExistPoType = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoType(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistPoType(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoNoTemp
        '	Discription	    : check exist PO No on job order po temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoNoTemp( _
            ByVal strPoNo As String _
        ) As Integer Implements IJob_OrderDao.CheckExistPoNoTemp
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistPoNoTemp = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt    ")
                    .AppendLine(" FROM job_order_po_tmp ")
                    .AppendLine(" WHERE UPPER(po_no) = ?po_no    ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?po_no", strPoNo.ToUpper)

                ' execute sql command
                CheckExistPoNoTemp = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoNoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistPoNoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoNoTemp
        '	Discription	    : check exist Quo No on job order quo temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoNoTemp( _
            ByVal strQuoNo As String _
        ) As Integer Implements IJob_OrderDao.CheckExistQuoNoTemp
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistQuoNoTemp = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt    ")
                    .AppendLine(" FROM job_order_quo_tmp  ")
                    .AppendLine(" WHERE UPPER(quo_no) = ?quo_no    ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?quo_no", strQuoNo.ToUpper)

                ' execute sql command
                CheckExistQuoNoTemp = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoNoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistQuoNoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoNo
        '	Discription	    : check exist PO No on job order po 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoNo( _
            ByVal strPoNo As String, _
            Optional ByVal strJobOrderId As String = "" _
        ) As Integer Implements IJob_OrderDao.CheckExistPoNo
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistPoNo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt    ")
                    .AppendLine(" FROM job_order_po  ")
                    .AppendLine(" WHERE UPPER(po_no) = ?po_no    ")
                    'Modify 2013/09/23 (it's can't duplicate same job order but others job order can dupplicate )
                    '.AppendLine(" AND (ISNULL(?job_order_id) or job_order_id <> ?job_order_id )   ")
                    .AppendLine(" AND (ISNULL(?job_order_id) or job_order_id = ?job_order_id )   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?po_no", strPoNo.ToUpper)
                objConn.AddParameter("?job_order_id", strJobOrderId)

                ' execute sql command
                CheckExistPoNo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoNo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistPoNo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoNo
        '	Discription	    : check exist Quo No on job order quo 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoNo( _
            ByVal strQuoNo As String, _
            Optional ByVal strJobOrderId As String = "" _
        ) As Integer Implements IJob_OrderDao.CheckExistQuoNo
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistQuoNo = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt    ")
                    .AppendLine(" FROM job_order_quo   ")
                    .AppendLine(" WHERE UPPER(quo_no) = ?quo_no    ")
                    'Modify 2013/09/23 (it's can't duplicate same job order but others job order can dupplicate )
                    '.AppendLine(" AND (ISNULL(?job_order_id) or job_order_id <> ?job_order_id )   ")
                    .AppendLine(" AND (ISNULL(?job_order_id) or job_order_id = ?job_order_id )   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?quo_no", strQuoNo.ToUpper)
                objConn.AddParameter("?job_order_id", strJobOrderId)

                ' execute sql command
                CheckExistQuoNo = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoNo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistQuoNo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoFile
        '	Discription	    : check exist PO File on job order po temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoFile( _
            ByVal strPoFile As String, _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.CheckExistPoFile
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistPoFile = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt    ")
                    .AppendLine(" FROM job_order_po_tmp   ")
                    .AppendLine(" WHERE UPPER(po_file) = ?po_file    ")
                    .AppendLine(" AND ip_address = ?ip_address   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?po_file", strPoFile.ToUpper)
                objConn.AddParameter("?ip_address", strIpAddress.ToUpper)

                ' execute sql command
                CheckExistPoFile = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistPoFile(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoFile
        '	Discription	    : check exist Quo File on job order quo temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoFile( _
            ByVal strQuoFile As String, _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.CheckExistQuoFile
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistQuoFile = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*)  AS cnt    ")
                    .AppendLine(" FROM job_order_quo_tmp   ")
                    .AppendLine(" WHERE UPPER(quo_file) = ?quo_file    ")
                    .AppendLine(" AND ip_address = ?ip_address   ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?quo_file", strQuoFile.ToUpper)
                objConn.AddParameter("?ip_address", strIpAddress.ToUpper)

                ' execute sql command
                CheckExistQuoFile = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoFile(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistQuoFile(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertPoTemp
        '	Discription	    : Insert data to job_order_po_temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPoTemp( _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer Implements IJob_OrderDao.InsertPoTemp
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertPoTemp = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine(" INSERT INTO job_order_po_tmp 	 ")
                    .AppendLine("	(job_order_id	 ")
                    .AppendLine("	, po_type	 ")
                    .AppendLine("	, po_no	 ")
                    .AppendLine("	, po_amount	 ")
                    .AppendLine("	, po_date	 ")
                    .AppendLine("	, receipt_date	 ")
                    .AppendLine("	, po_file	 ")
                    .AppendLine("	, ip_address	 ")
                    .AppendLine("	, po_fg	) ")
                    .AppendLine(" VALUES (?id  ")
                    .AppendLine("	, ?po_type	 ")
                    .AppendLine("	, ?po_no	 ")
                    .AppendLine("	, ?po_amount	 ")
                    .AppendLine("	, ?po_date	 ")
                    .AppendLine("	, ?receipt_date	 ")
                    .AppendLine("	, ?po_file	 ")
                    .AppendLine("	, ?ip_address	 ")
                    .AppendLine("	, 0 )	 ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter      
                    If objJobOrderEnt.id > 0 Then
                        .AddParameter("?id", objJobOrderEnt.id)
                    Else
                        .AddParameter("?id", DBNull.Value)
                    End If
                    .AddParameter("?po_type", objJobOrderEnt.po_type)
                    .AddParameter("?po_no", objJobOrderEnt.po_no)
                    .AddParameter("?po_amount", objJobOrderEnt.po_amount)
                    .AddParameter("?po_date", objJobOrderEnt.po_date)
                    .AddParameter("?receipt_date", objJobOrderEnt.po_receipt_date)
                    .AddParameter("?po_file", objJobOrderEnt.po_file)
                    .AddParameter("?ip_address", objJobOrderEnt.ip_address)

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
                InsertPoTemp = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertPoTemp(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertPoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertQuoTemp
        '	Discription	    : Insert data to job_order_quo_temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertQuoTemp( _
            ByVal objJobOrderEnt As Entity.IJob_OrderEntity _
        ) As Integer Implements IJob_OrderDao.InsertQuoTemp
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            InsertQuoTemp = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine(" INSERT INTO job_order_quo_tmp 	 ")
                    .AppendLine("	(job_order_id	                 ")
                    .AppendLine("	, quo_type	                     ")
                    .AppendLine("	, quo_no	                     ")
                    .AppendLine("	, quo_amount	                 ")
                    .AppendLine("	, quo_date	                     ")
                    .AppendLine("	, quo_file	                     ")
                    .AppendLine("	, ip_address )	                 ")
                    .AppendLine(" VALUES (?id                        ")
                    .AppendLine("	, ?quo_type	 ")
                    .AppendLine("	, ?quo_no	 ")
                    .AppendLine("	, ?quo_amount	 ")
                    .AppendLine("	, ?quo_date	 ")
                    .AppendLine("	, ?quo_file	 ")
                    .AppendLine("	, ?ip_address )	 ")

                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter       
                    If objJobOrderEnt.id > 0 Then
                        .AddParameter("?id", objJobOrderEnt.id)
                    Else
                        .AddParameter("?id", DBNull.Value)
                    End If
                    .AddParameter("?quo_type", objJobOrderEnt.quo_type)
                    .AddParameter("?quo_no", objJobOrderEnt.quo_no)
                    .AddParameter("?quo_amount", objJobOrderEnt.quo_amount)
                    .AddParameter("?quo_date", objJobOrderEnt.quo_date)
                    .AddParameter("?quo_file", objJobOrderEnt.quo_file)
                    .AddParameter("?ip_address", objJobOrderEnt.ip_address)

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
                InsertQuoTemp = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("InsertQuoTemp(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("InsertPoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoTemp
        '	Discription	    : check data before delete data on job_order_po_tmp  
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoTemp( _
            ByVal intId As Integer, _
            ByVal strIpAddress As String _
        ) As Integer Implements IJob_OrderDao.CheckExistPoTemp
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistPoTemp = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt     ")
                    .AppendLine(" FROM job_order_po_tmp   ")
                    .AppendLine(" WHERE po_fg = 1 ")
                    .AppendLine(" AND id = ?id ")
                    .AppendLine(" AND ip_address = ?ip_address  ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?ip_address", strIpAddress)
                objConn.AddParameter("?id", intId)

                ' execute sql command
                CheckExistPoTemp = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistPoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistReceiveDetail 
        '	Discription	    : check data on receive_detail  
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistReceiveDetail( _
            ByVal intJobOrderId As Integer _
        ) As Integer Implements IJob_OrderDao.CheckExistReceiveDetail
            ' variable keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            CheckExistReceiveDetail = -1
            Try
                ' assign sql command
                With strSql
                    .AppendLine(" SELECT COUNT(*) AS cnt      ")
                    .AppendLine(" FROM receive_detail    ")
                    .AppendLine(" WHERE job_order_id= ?job_order_id ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_id", intJobOrderId)

                ' execute sql command
                CheckExistReceiveDetail = objConn.ExecuteScalar(strSql.ToString)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistReceiveDetail(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("CheckExistReceiveDetail(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeletePO
        '	Discription	    : Delete job order po 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePO( _
            ByVal intId As Integer _
        ) As Integer
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePO = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("   SET FOREIGN_KEY_CHECKS=0;   ")
                    .AppendLine("   DELETE FROM job_order_po WHERE job_order_id  = ?id; ")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intId)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                'loginfo for Debug At 20140826
                objLog.InfoLog("id", intId)

                ' set value to return variable
                DeletePO = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePO(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePO(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteQuo
        '	Discription	    : Delete job order quo 
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteQuo( _
            ByVal intId As Integer _
        ) As Integer
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteQuo = -1
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE FROM job_order_quo               ")
                    .AppendLine("		WHERE job_order_id  = ?id				")
                End With

                ' assign parameter
                objConn.AddParameter("?id", intId)

                ' execute non query and keep row effect
                intEff = objConn.ExecuteNonQuery(strSql.ToString)

                'loginfo for Debug At 20140826
                objLog.InfoLog("id", intId)

                ' set value to return variable
                DeleteQuo = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteQuo(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteQuo(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeletePOTemp
        '	Discription	    : Delete job order po temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePOTemp( _
            ByVal intId As Integer _
        ) As Integer Implements IJob_OrderDao.DeletePOTemp
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeletePOTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE FROM JOB_ORDER_PO_TMP     ")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intId)
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
                DeletePOTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePOTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeletePOTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteQuoTemp
        '	Discription	    : Delete job order quo temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteQuoTemp( _
            ByVal intId As Integer _
        ) As Integer Implements IJob_OrderDao.DeleteQuoTemp
            ' strSql for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            DeleteQuoTemp = 0
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' assign sql command
                With strSql
                    .AppendLine("       DELETE FROM job_order_quo_tmp   ")
                    .AppendLine("		WHERE id = ?id							")
                End With
                ' new object connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intId)
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
                DeleteQuoTemp = intEff
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteQuoTemp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.ErrorLog("DeleteQuoTemp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetOneJobOrderPOTempList
        '	Discription	    : 
        '	Return Value	: String
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 13-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetOneJobOrderPOTempList( _
        ByVal intPoId As Integer _
        ) As System.Collections.Generic.List _
        (Of Entity.ImpJob_OrderDetailEntity) _
        Implements IJob_OrderDao.GetOneJobOrderPOTempList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetOneJobOrderPOTempList = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                ' assign sql command
                With strSql
                    .AppendLine(" SELECT  ")
                    .AppendLine("   id ")
                    .AppendLine("   ,po_type ")
                    .AppendLine("   ,po_no ")
                    .AppendLine("   ,po_amount ")
                    .AppendLine("   ,po_date ")
                    .AppendLine("   ,receipt_date ")
                    .AppendLine("   ,po_file ")
                    .AppendLine(" from job_order_po_tmp ")
                    .AppendLine(" WHERE	   ")
                    .AppendLine("   id = ?id    ")
                    .AppendLine(" ORDER BY id ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?id", intPoId)

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            '.id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .po_type_detail = IIf(IsDBNull(dr.Item("po_type")), Nothing, dr.Item("po_type"))
                            .po_no_detail = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .po_amount_detail = IIf(IsDBNull(dr.Item("po_amount")), Nothing, dr.Item("po_amount"))
                            .po_date_detail = IIf(IsDBNull(dr.Item("po_date")), Nothing, dr.Item("po_date"))
                            .po_receipt_date_detail = IIf(IsDBNull(dr.Item("receipt_date")), Nothing, dr.Item("receipt_date"))
                            .po_file_detail = IIf(IsDBNull(dr.Item("po_file")), Nothing, dr.Item("po_file"))
                        End With
                        ' add job order po temp to list
                        GetOneJobOrderPOTempList.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetOneJobOrderPOTempList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetOneJobOrderPOTempList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                'Close connection
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderTemp
        '	Discription	    : Insert data Job Order Temp
        '	Return Value	: Integer
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 12-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrderPOToTempList( _
        ByVal intJobOrderId As Integer, _
        ByVal objJobOrderEnt As Entity.IJob_OrderEntity, _
        ByVal strIpAddress As String) _
        As Integer Implements IJob_OrderDao.UpdateJobOrderPOToTempList

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default return value
            UpdateJobOrderPOToTempList = 0
            Try
                ' variable keep row effect
                Dim intEff As Integer

                ' assign sql command
                With strSql
                    .AppendLine("	update job_order_po_tmp SET              ")
                    '.AppendLine("	    job_order_id = ?job_order ,          ")
                    '.AppendLine("	    po_type = ?po_type ,                ")
                    .AppendLine("	    po_no = ?po_no ,                   ")
                    .AppendLine("	    po_amount = ?po_amount ,            ")
                    .AppendLine("	    po_date = ?po_date ,               ")
                    .AppendLine("	    po_file = ?po_file ,                ")
                    .AppendLine("	    receipt_date = ?po_receipt_date ,      ")
                    .AppendLine("	    ip_address = ?ip_address           ")
                    .AppendLine("	WHERE  id = ?id                       ")
                End With

                ' new connection object
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    ' assign parameter
                    .AddParameter("?user_id", HttpContext.Current.Session("UserID"))
                    '.AddParameter("?job_order_id", objJobOrderEnt.job_order)
                    '.AddParameter("?po_type", objJobOrderEnt.po_type)
                    .AddParameter("?po_no", objJobOrderEnt.po_no)
                    .AddParameter("?po_amount", objJobOrderEnt.po_amount)
                    .AddParameter("?po_date", objJobOrderEnt.po_date)
                    .AddParameter("?po_file", objJobOrderEnt.po_file)
                    .AddParameter("?po_receipt_date", objJobOrderEnt.po_receipt_date)
                    .AddParameter("?ip_address", strIpAddress)
                    .AddParameter("?id", intJobOrderId)

                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)

                End With

                ' assign return value
                UpdateJobOrderPOToTempList = intEff
            Catch exSql As MySqlException
                ' write error log
                objLog.ErrorLog("UpdateJobOrderPOToTempList(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
                ' throw exception
                Throw
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrderPOToTempList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("UpdateJobOrderPOToTempList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetOneQuoFromTmp
        '	Discription	    : Select Quotation From Tmp Table
        '	Return Value	: List
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 28-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function GetOneQuoFromTmp( _
        ByVal intQuoId As Integer) As  _
        System.Collections.Generic.List(Of Entity.ImpJob_OrderDetailEntity) _
        Implements IJob_OrderDao.GetOneQuoFromTmp

            Dim strSql As New Text.StringBuilder
            ' set default list
            GetOneQuoFromTmp = New List(Of Entity.ImpJob_OrderDetailEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objJobOrderDetail As Entity.ImpJob_OrderDetailEntity

                With strSql
                    .AppendLine(" SELECT  ")
                    .AppendLine("   id ")
                    .AppendLine("   ,job_order_id ")
                    .AppendLine("   ,quo_type ")
                    .AppendLine("   ,quo_no ")
                    .AppendLine("   ,quo_amount ")
                    .AppendLine("   ,quo_date ")
                    .AppendLine("   ,quo_file ")
                    .AppendLine(" from job_order_quo_tmp ")
                    .AppendLine(" WHERE	   ")
                    .AppendLine("   id = ?id    ")
                End With

                objConn = New Common.DBConnection.MySQLAccess
                objConn.AddParameter("?id", intQuoId)
                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objJobOrderDetail = New Entity.ImpJob_OrderDetailEntity
                        ' assign data from db to entity object
                        With objJobOrderDetail
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order_id")), Nothing, dr.Item("job_order_id"))
                            .quo_type = IIf(IsDBNull(dr.Item("quo_type")), Nothing, dr.Item("quo_type"))
                            .quo_no = IIf(IsDBNull(dr.Item("quo_no")), Nothing, dr.Item("quo_no"))
                            .quo_amount = IIf(IsDBNull(dr.Item("quo_amount")), Nothing, dr.Item("quo_amount"))
                            .quo_date = IIf(IsDBNull(dr.Item("quo_date")), Nothing, dr.Item("quo_date"))
                            .quo_file = IIf(IsDBNull(dr.Item("quo_file")), Nothing, dr.Item("quo_file"))
                        End With
                        ' add job order po temp to list
                        GetOneQuoFromTmp.Add(objJobOrderDetail)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetOneQuoFromTmp(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetOneQuoFromTmp(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateQuotationToTmp
        '	Discription	    : Update Quotation To Tmp Table
        '	Return Value	: List
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 28-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function UpdateQuotationToTmp( _
        ByVal objJobQuo As Entity.IJob_OrderEntity, _
        ByVal intQuoId As Integer) _
        As Integer Implements IJob_OrderDao.UpdateQuotationToTmp
            Try
                Dim strSql As New Text.StringBuilder
                UpdateQuotationToTmp = 0
                Dim intEff As Integer

                With strSql
                    .AppendLine("   UPDATE job_order_quo_tmp        ")
                    .AppendLine("   SET                             ")
                    .AppendLine("       quo_type = ?quo_type ,        ")
                    .AppendLine("       quo_no = ?quo_no ,            ")
                    .AppendLine("       quo_amount = ?quo_amount ,    ")
                    .AppendLine("       quo_date = ?quo_date ,        ")
                    .AppendLine("       quo_file = ?quo_file        ")
                    .AppendLine("   WHERE                           ")
                    .AppendLine("       id = ?id                    ")
                End With
                objConn = New Common.DBConnection.MySQLAccess

                With objConn
                    objConn.AddParameter("?id", intQuoId)
                    objConn.AddParameter("?quo_type", objJobQuo.quo_type)
                    objConn.AddParameter("?quo_no", objJobQuo.quo_no)
                    objConn.AddParameter("?quo_amount", objJobQuo.quo_amount)
                    objConn.AddParameter("?quo_date", objJobQuo.quo_date)
                    objConn.AddParameter("?quo_file", objJobQuo.quo_file)
                    ' execute sql command and return row effect to intEff variable
                    intEff = .ExecuteNonQuery(strSql.ToString)
                End With
                ' assign return value
                UpdateQuotationToTmp = intEff

            Catch exSql As MySqlException
                objLog.ErrorLog("UpdateQuotationToTmp(Dao)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function



#End Region
    End Class
End Namespace

