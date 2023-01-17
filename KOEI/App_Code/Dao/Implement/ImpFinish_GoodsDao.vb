#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpFinish_GoodsDao
'	Class Discription	: Class of table job order
'	Create User 		: Suwishaya L.
'	Create Date		    : 02-07-2013
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
#End Region

Namespace Dao
    Public Class ImpFinish_GoodsDao
        Implements IFinish_GoodsDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: GetFinishGoodsList
        '	Discription	    : Get Finish Goods List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetFinishGoodsList( _
            ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpFinish_GoodsEntity) Implements IFinish_GoodsDao.GetFinishGoodsList

            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetFinishGoodsList = New List(Of Entity.ImpFinish_GoodsEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objFinishGoods As Entity.ImpFinish_GoodsEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT 				")
                    .AppendLine("			r.job_order_id AS receive_header_id			")
                    .AppendLine("			, j.id 			")
                    .AppendLine("			, j.job_order			")
                    .AppendLine("			, j.finish_date			")
                    .AppendLine("			, v.name AS customer			")
                    .AppendLine("			, jt.name AS job_order_type			")
                    .AppendLine("			, j.part_name			")
                    .AppendLine("			, j.part_no			")
                    .AppendLine("			, r.amount_thb AS amount_thb			")
                    .AppendLine("		FROM job_order j				")
                    .AppendLine("		INNER JOIN (SELECT rd.job_order_id, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) AS amount_thb			")
                    .AppendLine("			FROM receive_detail rd		")
                    .AppendLine("			INNER JOIN receive_header rh ON rd.receive_header_id = rh.id AND rh.status_id <> 6 			")
                    .AppendLine("	        GROUP BY rd.job_order_id) r ON r.job_order_id = j.id  ")
                    .AppendLine("		LEFT JOIN mst_job_type jt 				")
                    .AppendLine("		ON j.job_type_id = jt.id 				")
                    .AppendLine("		AND jt.delete_fg <> 1				")
                    .AppendLine("		LEFT JOIN mst_vendor v 				")
                    .AppendLine("		ON j.customer = v.id 				")
                    .AppendLine("		AND v.delete_fg <> 1 				")
                    .AppendLine("		AND v.type1 = 1				")
                    .AppendLine("		LEFT JOIN user u 				")
                    .AppendLine("		ON j.person_in_charge = u.id  				")
                    .AppendLine("		AND u.delete_fg <> 1				")
                    .AppendLine("		WHERE j.status_id <> 6 				")
                    .AppendLine("		AND j.finish_fg = 1				")
                    .AppendLine("       AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("       AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("       AND (ISNULL(?person_in_charge) OR j.person_in_charge = ?person_in_charge) ")
                    .AppendLine("       AND (ISNULL(?job_order_type) OR j.job_type_id = ?job_order_type) ")
                    .AppendLine("       AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("       AND (ISNULL(?part_name) OR j.part_name LIKE CONCAT('%', ?part_name , '%')) ")
                    .AppendLine("       AND (ISNULL(?part_no) OR j.part_no LIKE CONCAT('%', ?part_no , '%')) ")
                    .AppendLine("       AND (ISNULL(?boi) OR j.is_boi = ?boi )  ")
                    .AppendLine("       AND ((ISNULL(?issue_date_from) OR j.issue_date >= ?issue_date_from)    ")
                    .AppendLine("       AND (ISNULL(?issue_date_to) OR j.issue_date <= ?issue_date_to))  ")
                    .AppendLine("       AND ((ISNULL(?finish_date_from) OR j.finish_date >= ?finish_date_from)    ")
                    .AppendLine("       AND (ISNULL(?finish_date_to) OR j.finish_date <= ?finish_date_to))  ")
                    .AppendLine("       AND (ISNULL(?receive_po) OR j.receive_po = ?receive_po  ) ")
                    .AppendLine("		ORDER BY j.Job_Order				")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_from_search), DBNull.Value, objFinishGoodsEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_to_search), DBNull.Value, objFinishGoodsEnt.job_order_to_search))
                objConn.AddParameter("?person_in_charge", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.person_charge_search), DBNull.Value, objFinishGoodsEnt.person_charge_search))
                objConn.AddParameter("?job_order_type", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_type_search), DBNull.Value, objFinishGoodsEnt.job_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.customer_search), DBNull.Value, objFinishGoodsEnt.customer_search))
                objConn.AddParameter("?part_name", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_name_search), DBNull.Value, objFinishGoodsEnt.part_name_search))
                objConn.AddParameter("?part_no", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_no_search), DBNull.Value, objFinishGoodsEnt.part_no_search))
                objConn.AddParameter("?boi", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.boi_search), DBNull.Value, objFinishGoodsEnt.boi_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_from_search), DBNull.Value, objFinishGoodsEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_to_search), DBNull.Value, objFinishGoodsEnt.issue_date_to_search))
                objConn.AddParameter("?finish_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_from_search), DBNull.Value, objFinishGoodsEnt.finish_date_from_search))
                objConn.AddParameter("?finish_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_to_search), DBNull.Value, objFinishGoodsEnt.finish_date_to_search))
                objConn.AddParameter("?receive_po", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.receive_po_search), DBNull.Value, objFinishGoodsEnt.receive_po_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objFinishGoods = New Entity.ImpFinish_GoodsEntity
                        ' assign data from db to entity object
                        With objFinishGoods
                            .receive_header_id = IIf(IsDBNull(dr.Item("receive_header_id")), Nothing, dr.Item("receive_header_id"))
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .finish_date = IIf(IsDBNull(dr.Item("finish_date")), Nothing, dr.Item("finish_date"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .job_order_type_name = IIf(IsDBNull(dr.Item("job_order_type")), Nothing, dr.Item("job_order_type"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .amount = IIf(IsDBNull(dr.Item("amount_thb")), Nothing, dr.Item("amount_thb"))
                        End With
                        ' add Job order to list
                        GetFinishGoodsList.Add(objFinishGoods)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetFinishGoodsList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetFinishGoodsList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: GetFinishGoodsReport
        '	Discription	    : Get data for Finish Goods Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetFinishGoodsReport( _
            ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpFinish_GoodsEntity) Implements IFinish_GoodsDao.GetFinishGoodsReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetFinishGoodsReport = New List(Of Entity.ImpFinish_GoodsEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objFinishGoods As Entity.ImpFinish_GoodsEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT r.job_order_id								")
                    .AppendLine("		, j.job_order								")
                    .AppendLine("		, j.finish_date								")
                    .AppendLine("		, if(ifnull(v.abbr,'')='',v.name,v.abbr) AS customer						")
                    .AppendLine("		, jt.name AS job_order_type								")
                    .AppendLine("		, j.part_name								")
                    .AppendLine("		, j.part_no								")
                    .AppendLine("		, r.amount_thb								")
                    .AppendLine("		FROM job_order j								")
                    .AppendLine("		INNER JOIN (								")
                    .AppendLine("			SELECT rd.job_order_id							")
                    .AppendLine("			, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) AS amount_thb							")
                    .AppendLine("			FROM receive_detail rd							")
                    .AppendLine("			INNER JOIN receive_header rh 							")
                    .AppendLine("			ON rd.receive_header_id = rh.id 							")
                    .AppendLine("			AND rh.status_id <> 6							")
                    .AppendLine("			GROUP BY rd.job_order_id) r 							")
                    .AppendLine("		ON r.job_order_id = j.id 								")
                    .AppendLine("		LEFT JOIN mst_job_type jt ON j.job_type_id = jt.id AND jt.delete_fg <> 1								")
                    .AppendLine("		LEFT JOIN mst_vendor v ON j.customer = v.id AND v.delete_fg <> 1 AND v.type1 = 1								")
                    .AppendLine("		LEFT JOIN user u ON j.person_in_charge = u.id  AND u.delete_fg <> 1								")
                    .AppendLine("		WHERE j.status_id <> 6 				")
                    .AppendLine("		AND j.finish_fg = 1				")
                    .AppendLine("       AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine("       AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine("       AND (ISNULL(?person_in_charge) OR j.person_in_charge = ?person_in_charge) ")
                    .AppendLine("       AND (ISNULL(?job_order_type) OR j.job_type_id = ?job_order_type) ")
                    .AppendLine("       AND (ISNULL(?boi) OR j.is_boi = ?boi )  ")
                    .AppendLine("       AND (ISNULL(?receive_po) OR j.receive_po = ?receive_po  ) ")
                    .AppendLine("       AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine("       AND (ISNULL(?part_name) OR j.part_name LIKE CONCAT('%', ?part_name , '%')) ")
                    .AppendLine("       AND (ISNULL(?part_no) OR j.part_no LIKE CONCAT('%', ?part_no , '%')) ")
                    .AppendLine("       AND ((ISNULL(?issue_date_from) OR j.issue_date >= ?issue_date_from)    ")
                    .AppendLine("       AND (ISNULL(?issue_date_to) OR j.issue_date <= ?issue_date_to))  ")
                    .AppendLine("       AND ((ISNULL(?finish_date_from) OR j.finish_date >= ?finish_date_from)    ")
                    .AppendLine("       AND (ISNULL(?finish_date_to) OR j.finish_date <= ?finish_date_to))  ")
                    .AppendLine("       ORDER BY j.Job_Order")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_from_search), DBNull.Value, objFinishGoodsEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_to_search), DBNull.Value, objFinishGoodsEnt.job_order_to_search))
                objConn.AddParameter("?person_in_charge", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.person_charge_search), DBNull.Value, objFinishGoodsEnt.person_charge_search))
                objConn.AddParameter("?job_order_type", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_type_search), DBNull.Value, objFinishGoodsEnt.job_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.customer_search), DBNull.Value, objFinishGoodsEnt.customer_search))
                objConn.AddParameter("?part_name", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_name_search), DBNull.Value, objFinishGoodsEnt.part_name_search))
                objConn.AddParameter("?part_no", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_no_search), DBNull.Value, objFinishGoodsEnt.part_no_search))
                objConn.AddParameter("?boi", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.boi_search), DBNull.Value, objFinishGoodsEnt.boi_search))
                objConn.AddParameter("?receive_po", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.receive_po_search), DBNull.Value, objFinishGoodsEnt.receive_po_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_from_search), DBNull.Value, objFinishGoodsEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_to_search), DBNull.Value, objFinishGoodsEnt.issue_date_to_search))
                objConn.AddParameter("?finish_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_from_search), DBNull.Value, objFinishGoodsEnt.finish_date_from_search))
                objConn.AddParameter("?finish_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_to_search), DBNull.Value, objFinishGoodsEnt.finish_date_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objFinishGoods = New Entity.ImpFinish_GoodsEntity
                        ' assign data from db to entity object
                        With objFinishGoods
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .finish_date = IIf(IsDBNull(dr.Item("finish_date")), Nothing, dr.Item("finish_date"))
                            .customer_name = IIf(IsDBNull(dr.Item("customer")), Nothing, dr.Item("customer"))
                            .job_order_type_name = IIf(IsDBNull(dr.Item("job_order_type")), Nothing, dr.Item("job_order_type"))
                            .part_name = IIf(IsDBNull(dr.Item("part_name")), Nothing, dr.Item("part_name"))
                            .part_no = IIf(IsDBNull(dr.Item("part_no")), Nothing, dr.Item("part_no"))
                            .amount = IIf(IsDBNull(dr.Item("amount_thb")), Nothing, dr.Item("amount_thb"))
                        End With
                        ' add Job order to list
                        GetFinishGoodsReport.Add(objFinishGoods)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetFinishGoodsReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetFinishGoodsReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumFinishGoodsReport
        '	Discription	    : Get data for Sum Finish Goods Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumFinishGoodsReport( _
            ByVal objFinishGoodsEnt As Entity.IFinish_GoodsEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpFinish_GoodsEntity) Implements IFinish_GoodsDao.GetSumFinishGoodsReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumFinishGoodsReport = New List(Of Entity.ImpFinish_GoodsEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objFinishGoods As Entity.ImpFinish_GoodsEntity

                ' assign sql command
                With strSql
                    .AppendLine("		SELECT SUM(r.amount_thb) as total_amount			")
                    .AppendLine("		FROM job_order j				")
                    .AppendLine("		INNER JOIN 				")
                    .AppendLine("			(SELECT rd.job_order_id, SUM(IFNULL(rd.actual_rate,1) * IFNULL(rd.amount,0)) AS amount_thb			")
                    .AppendLine("			FROM receive_detail rd			")
                    .AppendLine("			INNER JOIN receive_header rh ON rd.receive_header_id = rh.id AND rh.status_id <> 6			")
                    .AppendLine("			GROUP BY rd.job_order_id) r ON r.job_order_id = j.id  			")
                    .AppendLine("		LEFT JOIN mst_job_type jt ON j.job_type_id = jt.id AND jt.delete_fg <> 1				")
                    .AppendLine("		LEFT JOIN mst_vendor v ON j.customer = v.id AND v.delete_fg <> 1 AND v.type1 = 1 				")
                    .AppendLine("		LEFT JOIN user u ON j.person_in_charge = u.id  AND u.delete_fg <> 1				")
                    .AppendLine("		WHERE j.status_id <> 6 				")
                    .AppendLine("		AND j.finish_fg = 1				")
                    .AppendLine(" AND ((ISNULL(?job_order_from) OR j.job_order >= ?job_order_from)    ")
                    .AppendLine(" AND (ISNULL(?job_order_to) OR j.job_order <= ?job_order_to))  ")
                    .AppendLine(" AND (ISNULL(?person_in_charge) OR j.person_in_charge = ?person_in_charge) ")
                    .AppendLine(" AND (ISNULL(?job_order_type) OR j.job_type_id = ?job_order_type) ")
                    .AppendLine(" AND (ISNULL(?boi) OR j.is_boi = ?boi )  ")
                    .AppendLine(" AND (ISNULL(?receive_po) OR j.receive_po = ?receive_po  ) ")
                    .AppendLine(" AND (ISNULL(?customer) OR v.name LIKE CONCAT('%', ?customer , '%')) ")
                    .AppendLine(" AND (ISNULL(?part_name) OR j.part_name LIKE CONCAT('%', ?part_name , '%')) ")
                    .AppendLine(" AND (ISNULL(?part_no) OR j.part_no LIKE CONCAT('%', ?part_no , '%')) ")
                    .AppendLine(" AND ((ISNULL(?issue_date_from) OR j.issue_date >= ?issue_date_from)    ")
                    .AppendLine(" AND (ISNULL(?issue_date_to) OR j.issue_date <= ?issue_date_to))  ")
                    .AppendLine(" AND ((ISNULL(?finish_date_from) OR j.finish_date >= ?finish_date_from)    ")
                    .AppendLine(" AND (ISNULL(?finish_date_to) OR j.finish_date <= ?finish_date_to))  ")
                    .AppendLine(" ORDER BY j.Job_Order")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_from_search), DBNull.Value, objFinishGoodsEnt.job_order_from_search))
                objConn.AddParameter("?job_order_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_order_to_search), DBNull.Value, objFinishGoodsEnt.job_order_to_search))
                objConn.AddParameter("?person_in_charge", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.person_charge_search), DBNull.Value, objFinishGoodsEnt.person_charge_search))
                objConn.AddParameter("?job_order_type", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.job_type_search), DBNull.Value, objFinishGoodsEnt.job_type_search))
                objConn.AddParameter("?customer", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.customer_search), DBNull.Value, objFinishGoodsEnt.customer_search))
                objConn.AddParameter("?part_name", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_name_search), DBNull.Value, objFinishGoodsEnt.part_name_search))
                objConn.AddParameter("?part_no", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.part_no_search), DBNull.Value, objFinishGoodsEnt.part_no_search))
                objConn.AddParameter("?boi", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.boi_search), DBNull.Value, objFinishGoodsEnt.boi_search))
                objConn.AddParameter("?receive_po", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.receive_po_search), DBNull.Value, objFinishGoodsEnt.receive_po_search))
                objConn.AddParameter("?issue_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_from_search), DBNull.Value, objFinishGoodsEnt.issue_date_from_search))
                objConn.AddParameter("?issue_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.issue_date_to_search), DBNull.Value, objFinishGoodsEnt.issue_date_to_search))
                objConn.AddParameter("?finish_date_from", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_from_search), DBNull.Value, objFinishGoodsEnt.finish_date_from_search))
                objConn.AddParameter("?finish_date_to", IIf(String.IsNullOrEmpty(objFinishGoodsEnt.finish_date_to_search), DBNull.Value, objFinishGoodsEnt.finish_date_to_search))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objFinishGoods = New Entity.ImpFinish_GoodsEntity
                        ' assign data from db to entity object
                        With objFinishGoods
                            .sum_amount = IIf(IsDBNull(dr.Item("total_amount")), Nothing, dr.Item("total_amount"))
                        End With
                        ' add Job order to list
                        GetSumFinishGoodsReport.Add(objFinishGoods)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumFinishGoodsReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumFinishGoodsReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPersonInChangeForList
        '	Discription	    : Get data person in change for set dropdownlist
        '	Return Value	: List 
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPersonInChangeForList() As System.Collections.Generic.List(Of Entity.IFinish_GoodsEntity) Implements IFinish_GoodsDao.GetPersonInChangeForList
            ' variable keep sql statement
            Dim strSql As New Text.StringBuilder
            GetPersonInChangeForList = New List(Of Entity.IFinish_GoodsEntity)
            Try
                ' object variable data reader
                Dim dr As MySqlDataReader
                ' object variable job type entity
                Dim objPersEnt As Entity.IFinish_GoodsEntity

                ' assign sql statement
                With strSql
                    .AppendLine("	SELECT DISTINCT u.id, 		")
                    'Mod 2013/09/18
                    '.AppendLine("		 u.first_name 	")
                    .AppendLine("		 CONCAT(u.first_name,' ',u.last_name) as first_name 	")
                    .AppendLine("	FROM job_order j  		")
                    .AppendLine("	LEFT JOIN user u ON j.person_in_charge = u.id  		")
                    .AppendLine("	WHERE j.status_id <> 6		")
                    .AppendLine("	ORDER BY u.first_name		")
                End With
                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' execute sql statement
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new job type entity
                        objPersEnt = New Entity.ImpFinish_GoodsEntity
                        With objPersEnt
                            ' assign data to object job type entity
                            .person_in_charge_id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .person_in_charge_name = IIf(IsDBNull(dr.Item("first_name")), Nothing, dr.Item("first_name"))
                        End With
                        ' add object job type entity to list
                        GetPersonInChangeForList.Add(objPersEnt)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPersonInChangeForList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("GetPersonInChangeForList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            Finally
                ' Dispose object connection
                If Not IsNothing(objConn) Then objConn = Nothing
            End Try
        End Function
#End Region
        
    End Class
End Namespace
