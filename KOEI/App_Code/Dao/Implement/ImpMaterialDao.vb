#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpMaterialDao
'	Class Discription	: Class of table stock
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
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
    Public Class ImpMaterialDao
        Implements IMaterialDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log

#Region "Function"

        '/**************************************************************
        '	Function name	: GetMaterialList
        '	Discription	    : Get Material List
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetMaterialList( _
            ByVal objMaterialEnt As Entity.IMaterialEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMaterialEntity) Implements IMaterialDao.GetMaterialList
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetMaterialList = New List(Of Entity.ImpMaterialEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objMaterial As Entity.ImpMaterialEntity

                ' assign sql command
                With strSql
                    'Modify 2013/09/26 Start
                    '.AppendLine("select id, A.job_order, po_no, A.invoice_no, IF(isnull(B.amount),A.amount,B.amount) amount, vendor, description            ")
                    '.AppendLine(", date_in, qty_in, date_out, qty_out, qty_left from (                                                                      ")
                    '.AppendLine("	SELECT s.id, s.job_order, s.po_no, s.invoice_no	                                                                        ")
                    '.AppendLine("	, IFNULL(s.amount,0)*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1) AS amount  ")
                    '.AppendLine("	, v.name AS vendor, s.item_id, i.name AS description                                                                    ")
                    '.AppendLine("	, s.delivery_date AS date_in, IFNULL(s.qty_in, 0) AS qty_in                                                             ")
                    '.AppendLine("	, s.delivery_date as date_out, IFNULL(s.qty_out,0) AS qty_out                                                           ")
                    '.AppendLine("	, IFNULL(s.qty_in, 0) - IFNULL(s.qty_out,0) AS qty_left                                                                 ")
                    '.AppendLine("	FROM stock s                                                                                                            ")
                    '.AppendLine("	INNER JOIN mst_item i ON s.item_id = i.id                                                                                ")
                    '.AppendLine("	INNER JOIN mst_vendor v ON s.vendor_id = v.id                                                                            ")
                    '.AppendLine("	INNER JOIN payment_detail pd ON s.payment_detail_id = pd.id                                                                   ")
                    '.AppendLine("	INNER JOIN payment_header ph ON pd.payment_header_id = ph.id                                                                  ")
                    '.AppendLine("	INNER JOIN po_header po ON pd.po_header_id = po.id                                                                            ")
                    '.AppendLine("	INNER join mst_currency H on po.currency_id=H.id                                                                         ")
                    '.AppendLine("   INNER JOIN mst_ie ie ON s.ie_id = ie.id                                                                                  ")
                    '.AppendLine("	INNER join (select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate                                                        ")
                    '.AppendLine("		from (                                                                                                              ")
                    '.AppendLine("	select po.currency_id,A.delivery_date,max(B.ef_date) max_ef_date                                                        ")
                    '.AppendLine("	from stock A                                                                                                            ")
                    '.AppendLine("	INNER JOIN payment_detail pd ON A.payment_detail_id = pd.id                                                                   ")
                    '.AppendLine("	INNER JOIN payment_header ph ON pd.payment_header_id = ph.id                                                                  ")
                    '.AppendLine("	INNER JOIN po_header po ON pd.po_header_id = po.id                                                                            ")
                    '.AppendLine("   INNER JOIN mst_ie ie ON a.ie_id = ie.id                                                                                  ")
                    '.AppendLine("	INNER join mst_schedule_rate B on B.currency_id=po.currency_id and B.ef_date<=A.delivery_date                            ")
                    '.AppendLine("	where ph.status_id<>6 and B.delete_fg=0                                                                                 ")
                    '.AppendLine("       AND ie.code like 'B04%' or ie.code like 'B15%'                                                                      ")
                    '.AppendLine("	group by po.currency_id,A.delivery_date                                                                                 ")
                    '.AppendLine("	) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date                            ")
                    '.AppendLine("	) G on G.currency_id=po.currency_id and G.delivery_date=ph.delivery_date                                                ")
                    '.AppendLine("	WHERE v.type1 = 0 and ph.status_id <> 6                                                                                 ")
                    '.AppendLine("       AND ie.code like 'B04%' or ie.code like 'B15%'                                                                       ")
                    ''Modify 2013/09/26 End
                    '.AppendLine(" AND (ISNULL(?job_order) OR s.job_order LIKE CONCAT('%', ?job_order , '%'))                                                ")
                    '.AppendLine(" AND (ISNULL(?vendor) OR v.name LIKE CONCAT('%', ?vendor , '%'))                                                           ")
                    '.AppendLine(" AND (ISNULL(?invoice_no) OR s.invoice_no LIKE CONCAT('%', ?invoice_no , '%'))                                             ")
                    '.AppendLine(" AND (ISNULL(?po_no) OR s.po_no LIKE CONCAT('%', ?po_no , '%'))                                                            ")
                    '.AppendLine(" AND (ISNULL(?item_name) OR i.name LIKE CONCAT('%', ?item_name , '%'))                                                     ")
                    '.AppendLine(" AND ((ISNULL(?delivery_date_from) OR s.delivery_date >= ?delivery_date_from)                                              ")
                    '.AppendLine(" AND (ISNULL(?delivery_date_to) OR s.delivery_date <= ?delivery_date_to))                                                  ")
                    '.AppendLine("   ) A INNER join (                                                                                                         ")
                    '.AppendLine("	select invoice_no, job_order, ac.item_id, sum(sub_total) amount                                                         ")
                    '.AppendLine("	from accounting ac join payment_header ph on ac.ref_id=ph.id                                                            ")
                    '.AppendLine("   INNER JOIN mst_item i ON ac.item_id = i.id                                                                               ")
                    '.AppendLine("   INNER JOIN mst_vendor v ON ac.vendor_id = v.id                                                                           ")
                    '.AppendLine("   where type=3                                                                                                            ")
                    '.AppendLine("   AND (ISNULL(?job_order) OR job_order LIKE CONCAT('%', ?job_order , '%'))                                              ")
                    '.AppendLine("   AND (ISNULL(?vendor) OR v.name LIKE CONCAT('%', ?vendor , '%'))                                                       ")
                    '.AppendLine("   AND (ISNULL(?item_name) OR i.name LIKE CONCAT('%', ?item_name , '%'))                                                 ")
                    '.AppendLine("   GROUP BY invoice_no, job_order, ac.item_id                                                                              ")
                    '.AppendLine("   ) B on A.invoice_no=B.invoice_no and A.job_order=B.job_order and A.item_id=B.item_id                                    ")
                    '.AppendLine(" ORDER BY job_order desc, po_no desc, invoice_no desc;                                                                     ")

                    ' Modify By Rawikarn 
                    ' Change Sql Statement 
                    ' At 2014/07/30
                    .AppendLine("  SELECT s.job_order, s.po_no, s.invoice_no , s.amount                                                                     ")
                    .AppendLine(" ,it.id as id_item, it.`name` as description  , ie.code , ie.name as ie_name                                               ")
                    .AppendLine(" ,ph.id , v.`name` as vendor , s.delivery_date AS date_in, IFNULL(s.qty_in, 0) AS qty_in                                   ")
                    .AppendLine(" , s.delivery_date as date_out, IFNULL(s.qty_out,0) AS qty_out                                                             ")
                    .AppendLine(" , IFNULL(s.qty_in, 0) - IFNULL(s.qty_out,0) AS qty_left                                                                   ")
                    .AppendLine(" , IFNULL(s.amount,0)*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1) AS amount    ")
                    .AppendLine(" FROM stock s                                                                                                              ")
                    .AppendLine(" LEFT JOIN mst_item it ON s.item_id = it.id                                                                                ")
                    .AppendLine(" LEFT JOIN mst_ie ie ON s.ie_id = ie.id                                                                                    ")
                    .AppendLine(" LEFT JOIN mst_vendor v ON s.vendor_id = v.id                                                                              ")
                    .AppendLine(" LEFT JOIN payment_detail pd ON s.payment_detail_id = pd.id                                                                ")
                    .AppendLine(" LEFT JOIN payment_header ph	ON pd.payment_header_id = ph.id                                                             ")
                    .AppendLine(" LEFT JOIN accounting acc ON pd.id = acc.ref_id                                                                            ")
                    .AppendLine(" LEFT JOIN po_header po ON pd.po_header_id = po.id                                                                         ")
                    .AppendLine(" LEFT join mst_currency H on po.currency_id=H.id                                                                           ")
                    .AppendLine(" LEFT join (                                                                                                               ")
                    .AppendLine(" select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate                                                                     ")
                    .AppendLine(" 	from (                                                                                                                  ")
                    .AppendLine(" select po.currency_id,A.delivery_date,max(B.ef_date) max_ef_date                                                          ")
                    .AppendLine(" from stock A                                                                                                              ")
                    .AppendLine(" 	LEFT JOIN payment_detail pd ON A.payment_detail_id = pd.id                                                              ")
                    .AppendLine(" 	LEFT JOIN payment_header ph ON pd.payment_header_id = ph.id                                                             ")
                    .AppendLine(" 	LEFT JOIN po_header po ON pd.po_header_id = po.id                                                                       ")
                    .AppendLine(" 	LEFT JOIN mst_ie ie ON a.ie_id = ie.id                                                                                  ")
                    .AppendLine(" 	LEFT join mst_schedule_rate B on B.currency_id=po.currency_id and B.ef_date<=A.delivery_date                            ")
                    .AppendLine("                 where(ph.status_id <> 6 And B.delete_fg = 0)                                                              ")
                    .AppendLine("    AND ie.code like 'B04%' or ie.code like 'B15%'                                                                         ")
                    .AppendLine(" 			AND a.job_order = 140232                                                                                        ")
                    .AppendLine(" group by po.currency_id,A.delivery_date                                                                                   ")
                    .AppendLine(" ) Z LEFT join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date                              ")
                    .AppendLine(" ) G on G.currency_id=po.currency_id and G.delivery_date=ph.delivery_date                                                  ")
                    .AppendLine("   WHERE ie.code like 'B04%'  and v.type1 = 0  and ph.status_id <> 6                                                       ")
                    .AppendLine("       AND (ISNULL(?job_order) OR s.job_order LIKE CONCAT('%', ?job_order , '%'))                                          ")
                    .AppendLine("       AND (ISNULL(?invoice_no) OR s.invoice_no LIKE CONCAT('%', ?invoice_no , '%'))                                       ")
                    .AppendLine("       AND (ISNULL(?vendor) OR v.name LIKE CONCAT('%', ?vendor , '%'))                                                     ")
                    .AppendLine("       AND (ISNULL(?po_no) OR s.po_no LIKE CONCAT('%', ?po_no , '%'))                                                      ")
                    .AppendLine("       AND (ISNULL(?item_name) OR it.name LIKE CONCAT('%', ?item_name , '%'))                                              ")
                    .AppendLine("       AND ((ISNULL(?delivery_date_from) OR s.delivery_date >= ?delivery_date_from)                                        ")
                    .AppendLine("       AND (ISNULL(?delivery_date_to) OR s.delivery_date <= ?delivery_date_to))                                            ")
                    .AppendLine("       GROUP BY s.job_order, s.po_no, s.invoice_no , s.amount , it.`name`, ie.code, ie.name                                ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", IIf(String.IsNullOrEmpty(objMaterialEnt.job_order), DBNull.Value, objMaterialEnt.job_order))
                objConn.AddParameter("?vendor", IIf(String.IsNullOrEmpty(objMaterialEnt.vendor), DBNull.Value, objMaterialEnt.vendor))
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objMaterialEnt.invoice_no), DBNull.Value, objMaterialEnt.invoice_no))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objMaterialEnt.po_no), DBNull.Value, objMaterialEnt.po_no))
                objConn.AddParameter("?item_name", IIf(String.IsNullOrEmpty(objMaterialEnt.item_name), DBNull.Value, objMaterialEnt.item_name))
                objConn.AddParameter("?delivery_date_from", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_from), DBNull.Value, objMaterialEnt.delivery_date_from))
                objConn.AddParameter("?delivery_date_to", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_to), DBNull.Value, objMaterialEnt.delivery_date_to))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)


                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objMaterial = New Entity.ImpMaterialEntity
                        ' assign data from db to entity object
                        With objMaterial
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .vendor = IIf(IsDBNull(dr.Item("vendor")), Nothing, dr.Item("vendor"))
                            .item_name = IIf(IsDBNull(dr.Item("description")), Nothing, dr.Item("description"))
                            .delivery_date_in = IIf(IsDBNull(dr.Item("date_in")), Nothing, dr.Item("date_in"))
                            .qty_in = IIf(IsDBNull(dr.Item("qty_in")), Nothing, dr.Item("qty_in"))
                            .delivery_date_out = IIf(IsDBNull(dr.Item("date_out")), Nothing, dr.Item("date_out"))
                            .qty_out = IIf(IsDBNull(dr.Item("qty_out")), Nothing, dr.Item("qty_out"))
                            .qty_left = IIf(IsDBNull(dr.Item("qty_left")), Nothing, dr.Item("qty_left"))

                        End With
                        ' add Material to list
                        GetMaterialList.Add(objMaterial)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaterialList(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetMaterialList(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMaterialListReport
        '	Discription	    : Get Material List Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetMaterialListReport( _
            ByVal objMaterialEnt As Entity.IMaterialEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMaterialEntity) Implements IMaterialDao.GetMaterialListReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetMaterialListReport = New List(Of Entity.ImpMaterialEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objMaterial As Entity.ImpMaterialEntity

                ' assign sql command
                With strSql
                    ' Modify 2013/09/26 Start
                    '.AppendLine(" SELECT   ")
                    '.AppendLine("   s.id ")
                    '.AppendLine("   , s.job_order ")
                    '.AppendLine("   , s.po_no ")
                    '.AppendLine("   , s.invoice_no ")
                    '.AppendLine("   , SUM(IFNULL(s.amount,0)) AS amount ")
                    '.AppendLine("   , v.name AS vendor ")
                    '.AppendLine("   , i.name AS description ")
                    '.AppendLine("   , s.delivery_date AS date_in ")
                    '.AppendLine("   , SUM(IFNULL(s.qty_in, 0)) AS qty_in ")
                    '.AppendLine("   , s.delivery_date as date_out ")
                    '.AppendLine("   , SUM(IFNULL(s.qty_out,0)) AS qty_out ")
                    '.AppendLine("   , IFNULL(s.qty_in, 0) - IFNuLL(s.qty_out,0) AS qty_left ")
                    '.AppendLine(" FROM stock s ")
                    '.AppendLine(" LEFT JOIN mst_item i  ")
                    '.AppendLine(" ON s.item_id = i.id ")
                    '.AppendLine(" LEFT JOIN mst_vendor v  ")
                    '.AppendLine(" ON s.vendor_id = v.id  ")
                    '.AppendLine(" AND v.type1  = 0")
                    '.AppendLine(" LEFT JOIN payment_detail pd ON s.payment_detail_id = pd.id ")
                    '.AppendLine(" INNER JOIN payment_header ph ON pd.payment_header_id = ph.id AND ph.status_id <> 6  ")
                    '.AppendLine(" WHERE 1=1 ")
                    .AppendLine("	SELECT s.id		                                                                                 ")
                    .AppendLine("	, s.job_order	                                                                           	     ")
                    .AppendLine("	, s.po_no		                                                    ")
                    .AppendLine("	, s.invoice_no	                                            	    ")
                    .AppendLine("	, SUM(IFNULL(s.amount,0))*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1) AS amount 													  ")
                    .AppendLine("	, v.name AS vendor			                                        ")
                    .AppendLine("	, i.name AS description 										    ")
                    .AppendLine("	, s.delivery_date AS date_in						                ")
                    .AppendLine("	, SUM(IFNULL(s.qty_in, 0)) AS qty_in						        ")
                    .AppendLine("	, s.delivery_date as date_out						                ")
                    .AppendLine("	, SUM(IFNULL(s.qty_out,0)) AS qty_out 						        ")
                    .AppendLine("	, IFNULL(s.qty_in, 0) - IFNuLL(s.qty_out,0) AS qty_left				")
                    .AppendLine("	FROM stock s						                                ")
                    .AppendLine("	LEFT JOIN mst_item i ON s.item_id = i.id						    ")
                    .AppendLine("	LEFT JOIN mst_vendor v ON s.vendor_id = v.id						")
                    .AppendLine("	JOIN payment_detail pd ON s.payment_detail_id = pd.id				")
                    .AppendLine("	JOIN payment_header ph ON pd.payment_header_id = ph.id				")
                    .AppendLine("	JOIN po_header po ON pd.po_header_id = po.id						")
                    .AppendLine("	left join mst_currency H on po.currency_id=H.id						")
                    .AppendLine("	left join (select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate							  ")
                    .AppendLine("		from (						  ")
                    .AppendLine("	select po.currency_id,A.delivery_date,max(B.ef_date) max_ef_date							  ")
                    .AppendLine("	from stock A							  ")
                    .AppendLine("	JOIN payment_detail pd ON A.payment_detail_id = pd.id							  ")
                    .AppendLine("	JOIN payment_header ph ON pd.payment_header_id = ph.id							  ")
                    .AppendLine("	JOIN po_header po ON pd.po_header_id = po.id							  ")
                    .AppendLine("	left join mst_schedule_rate B on B.currency_id=po.currency_id and B.ef_date<=A.delivery_date										  ")
                    .AppendLine("	where ph.status_id<>6 and B.delete_fg=0							  ")
                    .AppendLine("	group by po.currency_id,A.delivery_date							  ")
                    .AppendLine("	) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date										  ")
                    .AppendLine("	) G on G.currency_id=po.currency_id and G.delivery_date=ph.delivery_date								  ")
                    .AppendLine("	WHERE v.type1 = 0 and ph.status_id <> 6							  ")
                    'Modify 2013/09/26 End

                    .AppendLine(" AND (ISNULL(?job_order) OR s.job_order LIKE CONCAT('%', ?job_order , '%')) ")
                    .AppendLine(" AND (ISNULL(?vendor) OR v.name LIKE CONCAT('%', ?vendor , '%')) ")
                    .AppendLine(" AND (ISNULL(?invoice_no) OR s.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine(" AND (ISNULL(?po_no) OR s.po_no LIKE CONCAT('%', ?po_no , '%')) ")
                    .AppendLine(" AND (ISNULL(?item_name) OR i.name LIKE CONCAT('%', ?item_name , '%')) ")
                    .AppendLine(" AND ((ISNULL(?delivery_date_from) OR s.delivery_date >= ?delivery_date_from)    ")
                    .AppendLine(" AND (ISNULL(?delivery_date_to) OR s.delivery_date <= ?delivery_date_to))  ")
                    .AppendLine(" GROUP BY s.invoice_no, s.item_id ")
                    .AppendLine(" ORDER BY s.job_order, s.po_no ")
                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", IIf(String.IsNullOrEmpty(objMaterialEnt.job_order), DBNull.Value, objMaterialEnt.job_order))
                objConn.AddParameter("?vendor", IIf(String.IsNullOrEmpty(objMaterialEnt.vendor), DBNull.Value, objMaterialEnt.vendor))
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objMaterialEnt.invoice_no), DBNull.Value, objMaterialEnt.invoice_no))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objMaterialEnt.po_no), DBNull.Value, objMaterialEnt.po_no))
                objConn.AddParameter("?item_name", IIf(String.IsNullOrEmpty(objMaterialEnt.item_name), DBNull.Value, objMaterialEnt.item_name))
                objConn.AddParameter("?delivery_date_from", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_from), DBNull.Value, objMaterialEnt.delivery_date_from))
                objConn.AddParameter("?delivery_date_to", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_to), DBNull.Value, objMaterialEnt.delivery_date_to))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objMaterial = New Entity.ImpMaterialEntity
                        ' assign data from db to entity object
                        With objMaterial
                            .id = IIf(IsDBNull(dr.Item("id")), Nothing, dr.Item("id"))
                            .job_order = IIf(IsDBNull(dr.Item("job_order")), Nothing, dr.Item("job_order"))
                            .po_no = IIf(IsDBNull(dr.Item("po_no")), Nothing, dr.Item("po_no"))
                            .invoice_no = IIf(IsDBNull(dr.Item("invoice_no")), Nothing, dr.Item("invoice_no"))
                            .amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))
                            .vendor = IIf(IsDBNull(dr.Item("vendor")), Nothing, dr.Item("vendor"))
                            .item_name = IIf(IsDBNull(dr.Item("description")), Nothing, dr.Item("description"))
                            .delivery_date_in = IIf(IsDBNull(dr.Item("date_in")), Nothing, dr.Item("date_in"))
                            .qty_in = IIf(IsDBNull(dr.Item("qty_in")), Nothing, dr.Item("qty_in"))
                            .delivery_date_out = IIf(IsDBNull(dr.Item("date_out")), Nothing, dr.Item("date_out"))
                            .qty_out = IIf(IsDBNull(dr.Item("qty_out")), Nothing, dr.Item("qty_out"))
                            .qty_left = IIf(IsDBNull(dr.Item("qty_left")), Nothing, dr.Item("qty_left"))

                        End With
                        ' add Material to list
                        GetMaterialListReport.Add(objMaterial)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaterialListReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetMaterialListReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumMaterialListReport
        '	Discription	    : Get Sum Material List Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 03-07-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetSumMaterialListReport( _
            ByVal objMaterialEnt As Entity.IMaterialEntity _
        ) As System.Collections.Generic.List(Of Entity.ImpMaterialEntity) Implements IMaterialDao.GetSumMaterialListReport
            ' variable for keep sql command
            Dim strSql As New Text.StringBuilder
            ' set default list
            GetSumMaterialListReport = New List(Of Entity.ImpMaterialEntity)
            Try
                ' data reader object
                Dim dr As MySqlDataReader
                Dim objMaterial As Entity.ImpMaterialEntity

                ' assign sql command
                With strSql
                    'Modify 2013/09/26 Start
                    '.AppendLine(" SELECT   ")
                    '.AppendLine("   SUM(IFNULL(s.amount,0)) AS amount ")
                    '.AppendLine(" FROM stock s ")
                    '.AppendLine(" LEFT JOIN mst_item i  ")
                    '.AppendLine(" ON s.item_id = i.id ")
                    '.AppendLine(" LEFT JOIN mst_vendor v  ")
                    '.AppendLine(" ON s.vendor_id = v.id  ")
                    '.AppendLine(" AND v.type1  = 0")
                    '.AppendLine(" LEFT JOIN payment_detail pd ON s.payment_detail_id = pd.id ")
                    '.AppendLine(" INNER JOIN payment_header ph ON pd.payment_header_id = ph.id AND ph.status_id <> 6   ")
                    '.AppendLine(" WHERE 1=1 ")

                    .AppendLine("	SELECT	  ")
                    .AppendLine("	 SUM(IFNULL(s.amount,0))*ifnull(case upper(H.name) when 'THB' then 1 when 'JPY' then G.rate/1 else G.rate end,1) AS amount    ")
                    .AppendLine("	FROM stock s						  ")
                    .AppendLine("	LEFT JOIN mst_item i ON s.item_id = i.id						  ")
                    .AppendLine("	LEFT JOIN mst_vendor v ON s.vendor_id = v.id						  ")
                    .AppendLine("	JOIN payment_detail pd ON s.payment_detail_id = pd.id						  ")
                    .AppendLine("	JOIN payment_header ph ON pd.payment_header_id = ph.id							  ")
                    .AppendLine("	JOIN po_header po ON pd.po_header_id = po.id							  ")
                    .AppendLine("	left join mst_currency H on po.currency_id=H.id							  ")
                    .AppendLine("	left join (select Z.currency_id,Z.delivery_date,Y.ef_date,Y.rate							  ")
                    .AppendLine("		from (						  ")
                    .AppendLine("	select po.currency_id,A.delivery_date,max(B.ef_date) max_ef_date							  ")
                    .AppendLine("	from stock A							  ")
                    .AppendLine("	JOIN payment_detail pd ON A.payment_detail_id = pd.id							  ")
                    .AppendLine("	JOIN payment_header ph ON pd.payment_header_id = ph.id							  ")
                    .AppendLine("	JOIN po_header po ON pd.po_header_id = po.id							  ")
                    .AppendLine("	left join mst_schedule_rate B on B.currency_id=po.currency_id and B.ef_date<=A.delivery_date										  ")
                    .AppendLine("	where ph.status_id<>6 and B.delete_fg=0							  ")
                    .AppendLine("	group by po.currency_id,A.delivery_date							  ")
                    .AppendLine("	) Z left join mst_schedule_rate Y on Y.currency_id=Z.currency_id and Y.ef_date=Z.max_ef_date										  ")
                    .AppendLine("	) G on G.currency_id=po.currency_id and G.delivery_date=ph.delivery_date								  ")
                    .AppendLine("	WHERE v.type1 = 0 and ph.status_id <> 6							  ")

                    'Modify 2013/09/26 End
                    .AppendLine(" AND (ISNULL(?job_order) OR s.job_order LIKE CONCAT('%', ?job_order , '%')) ")
                    .AppendLine(" AND (ISNULL(?vendor) OR v.name LIKE CONCAT('%', ?vendor , '%')) ")
                    .AppendLine(" AND (ISNULL(?invoice_no) OR s.invoice_no LIKE CONCAT('%', ?invoice_no , '%')) ")
                    .AppendLine(" AND (ISNULL(?po_no) OR s.po_no LIKE CONCAT('%', ?po_no , '%')) ")
                    .AppendLine(" AND (ISNULL(?item_name) OR i.name LIKE CONCAT('%', ?item_name , '%')) ")
                    .AppendLine(" AND ((ISNULL(?delivery_date_from) OR s.delivery_date >= ?delivery_date_from)    ")
                    .AppendLine(" AND (ISNULL(?delivery_date_to) OR s.delivery_date <= ?delivery_date_to))  ")

                End With

                ' new connection
                objConn = New Common.DBConnection.MySQLAccess
                ' assign parameter
                objConn.AddParameter("?job_order", IIf(String.IsNullOrEmpty(objMaterialEnt.job_order), DBNull.Value, objMaterialEnt.job_order))
                objConn.AddParameter("?vendor", IIf(String.IsNullOrEmpty(objMaterialEnt.vendor), DBNull.Value, objMaterialEnt.vendor))
                objConn.AddParameter("?invoice_no", IIf(String.IsNullOrEmpty(objMaterialEnt.invoice_no), DBNull.Value, objMaterialEnt.invoice_no))
                objConn.AddParameter("?po_no", IIf(String.IsNullOrEmpty(objMaterialEnt.po_no), DBNull.Value, objMaterialEnt.po_no))
                objConn.AddParameter("?item_name", IIf(String.IsNullOrEmpty(objMaterialEnt.item_name), DBNull.Value, objMaterialEnt.item_name))
                objConn.AddParameter("?delivery_date_from", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_from), DBNull.Value, objMaterialEnt.delivery_date_from))
                objConn.AddParameter("?delivery_date_to", IIf(String.IsNullOrEmpty(objMaterialEnt.delivery_date_to), DBNull.Value, objMaterialEnt.delivery_date_to))

                ' execute reader
                dr = objConn.ExecuteReader(strSql.ToString)

                ' check exist data
                If dr.HasRows Then
                    While dr.Read
                        ' new object entity
                        objMaterial = New Entity.ImpMaterialEntity
                        ' assign data from db to entity object
                        With objMaterial
                            .sum_amount = IIf(IsDBNull(dr.Item("amount")), Nothing, dr.Item("amount"))

                        End With
                        ' add Material to list
                        GetSumMaterialListReport.Add(objMaterial)
                    End While
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumMaterialListReport(Dao)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
                ' write sql command
                objLog.InfoLog("GetSumMaterialListReport(Dao)", strSql.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


#End Region

        
    End Class
End Namespace
