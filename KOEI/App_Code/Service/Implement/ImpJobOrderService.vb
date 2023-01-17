#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpJobOrderService
'	Class Discription	: Implement Job Order Service
'	Create User 		: Suwishaya L.
'	Create Date		    : 10-06-2013
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
Imports System.Data
Imports MySql.Data.MySqlClient
#End Region

Namespace Service
    Public Class ImpJobOrderService
        Implements IJobOrderService

        Private objLog As New Common.Logs.Log
        Private objJobOrderEnt As New Entity.ImpJob_OrderEntity

#Region "Function"
        '/**************************************************************
        '	Function name	: CheckJobOrderByPurchase 
        '	Discription	    : Check job_order_id by purchase
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckJobOrderByPurchase(ByVal strJobOrderId As String) As Boolean Implements IJobOrderService.CheckJobOrderByPurchase
            Try
                ' variable
                Dim objJobOrder As New Entity.ImpJob_OrderEntity

                CheckJobOrderByPurchase = False
                If strJobOrderId.Trim = String.Empty Then Exit Function

                Return objJobOrder.CheckJobOrderByPurchase(strJobOrderId)

            Catch ex As Exception
                ' Write error log
                CheckJobOrderByPurchase = False
                objLog.ErrorLog("CheckJobOrderByPurchase", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetListJobOrder
        '	Discription	    : Set list joborder to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 17-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListJobOrder(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements IJobOrderService.SetListJobOrder
            Try
                ' variable
                Dim objJobOrder As New Entity.ImpJob_OrderEntity
                Dim objListJobOrder As List(Of Entity.IJob_OrderEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListJobOrder = False
                ' get data list job_order
                objListJobOrder = objJobOrder.GetJobOrderForList
                If objListJobOrder.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListJobOrder, "job_order", "id")
                If objValue.Items.Count > 0 Then SetListJobOrder = True

            Catch ex As Exception
                ' Write error log
                SetListJobOrder = False
                objLog.ErrorLog("SetListJobOrder", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrder
        '	Discription	    : Delete Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrder( _
            ByVal intJobOrderID As Integer, _
            ByVal strJobOrder As String _
        ) As Boolean Implements IJobOrderService.DeleteJobOrder
            ' set default return value
            DeleteJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteJobOrder from job order Entity
                intEff = objJobOrderEnt.DeleteJobOrder(intJobOrderID, strJobOrder)

                ' check row effect
                If intEff >= 0 Then
                    ' case row more than 0 then return True
                    DeleteJobOrder = True
                Else
                    ' case row less than 1 then return False
                    DeleteJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderList
        '	Discription	    : Get Job Order list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderList( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderList
            ' set default
            GetJobOrderList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderList(SetDtoToEntity(objJobOrderDto))

                ' assign column header
                With GetJobOrderList
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("receive_po")
                    .Columns.Add("job_finished")
                    .Columns.Add("customer")
                    .Columns.Add("job_order_type")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("total_amount")
                    .Columns.Add("quotation_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("receive_po") = values.receive_po_Detail
                        row("job_finished") = values.job_finished_Detail
                        row("customer") = values.customer_Detail
                        row("job_order_type") = values.job_order_type_Detail
                        row("part_name") = values.part_name
                        row("part_no") = values.part_no
                        row("total_amount") = Format(Convert.ToDouble(values.total_amount_Detail.ToString.Trim), "#,##0.00")
                        If values.quotation_amount_Detail Is Nothing Or values.quotation_amount_Detail = "" Then
                            row("quotation_amount") = values.quotation_amount_Detail
                        Else
                            row("quotation_amount") = Format(Convert.ToDouble(values.quotation_amount_Detail.ToString.Trim), "#,##0.00")
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderReportList
        '	Discription	    : Get Job Order Report list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderReportList( _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            ByVal blnPermission As Boolean _
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderReportList
            ' set default
            GetJobOrderReportList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                Dim strIssueDate As String = ""
                Dim strQuoDate As String = ""

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderReportList(SetDtoToEntity(objJobOrderDto))

                ' assign column header
                With GetJobOrderReportList
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("issue_date")
                    .Columns.Add("issue_by")
                    .Columns.Add("customer")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("part_type")
                    .Columns.Add("job_type_id")
                    .Columns.Add("job_new")
                    .Columns.Add("job_mod")
                    .Columns.Add("Detail")
                    .Columns.Add("quo_date")
                    .Columns.Add("quo_no")
                    .Columns.Add("hontai_amount")
                    .Columns.Add("hontai_amount_thb")
                    .Columns.Add("currency")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("issue_by") = values.issue_by
                        row("customer") = values.customer_Detail
                        row("part_name") = values.part_name
                        row("part_no") = values.part_no
                        row("part_type") = values.part_type_name
                        row("job_type_id") = values.job_type_id
                        row("job_new") = values.job_new
                        row("job_mod") = values.job_Mod
                        row("Detail") = values.detail
                        row("quo_no") = values.quo_no
                        row("currency") = values.currency_name
                        If Not blnPermission Then
                            row("hontai_amount") = "******"
                            row("hontai_amount_thb") = "******"
                        Else
                            row("hontai_amount") = Format(Convert.ToDouble(values.rpt_hontai_amount.ToString.Trim), "#,##0.00")
                            row("hontai_amount_thb") = Format(Convert.ToDouble(values.rpt_hontai_amount_thb.ToString.Trim), "#,##0.00")
                        End If

                        row("issue_date") = values.issue_date
                        row("quo_date") = values.quo_date

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderReportList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumHontaiAmountReport
        '	Discription	    : Get sum amount Job Order Report list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumHontaiAmountReport( _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            ByVal blnPermission As Boolean _
        ) As System.Data.DataTable Implements IJobOrderService.GetSumHontaiAmountReport
            ' set default
            GetSumHontaiAmountReport = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                Dim strIssueDate As String = ""
                Dim strQuoDate As String = ""

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetSumHontaiAmountReport(SetDtoToEntity(objJobOrderDto))

                ' assign column header
                With GetSumHontaiAmountReport
                    .Columns.Add("sum_hontai_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow

                        If Not blnPermission Then
                            row("sum_hontai_amount") = "******"
                        Else
                            row("sum_hontai_amount") = Format(Convert.ToDouble(values.sum_hontai_amount.ToString.Trim), "#,##0.00")
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumHontaiAmountReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDeleteJobOrderList
        '	Discription	    : Get Delete Job Order list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDeleteJobOrderList( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As System.Data.DataTable Implements IJobOrderService.GetDeleteJobOrderList
            ' set default
            GetDeleteJobOrderList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetDeleteJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetDeleteJobOrderList(SetDtoToEntity(objJobOrderDto))

                ' assign column header
                With GetDeleteJobOrderList
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("receive_po")
                    .Columns.Add("customer")
                    .Columns.Add("job_order_type")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("is_boi_name")
                    .Columns.Add("total_amount")
                    .Columns.Add("currency")
                    .Columns.Add("remark")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("receive_po") = values.receive_po_Detail
                        row("customer") = values.customer_Detail
                        row("job_order_type") = values.job_order_type_Detail
                        row("part_name") = values.part_name
                        row("part_no") = values.part_no
                        row("is_boi_name") = values.is_boi_name
                        row("total_amount") = Format(Convert.ToDouble(values.total_amount_Detail.ToString.Trim), "#,##0.00")
                        row("currency") = values.currency_name
                        row("remark") = values.remark
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDeleteJobOrderList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInAccounting
        '	Discription	    : Check Job Order in used Accounting
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInJobOrder( _
        ByVal intJobOrderID As Integer, _
            ByVal strJobOrder As String _
        ) As Boolean Implements IJobOrderService.IsUsedInJobOrder

            ' set default return value
            IsUsedInJobOrder = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objJobOrderEnt.CheckUseInPodetail(strJobOrder)
                If intCount <= 0 Then
                    ' call function CheckUseInOrderPo from entity
                    intCount = objJobOrderEnt.CheckUseInOrderPo(intJobOrderID)
                    If intCount <= 0 Then
                        ' call function CheckUseInRecDetail from entity
                        intCount = objJobOrderEnt.CheckUseInRecDetail(intJobOrderID)
                        If intCount <= 0 Then
                            ' call function CheckUseInAccounting from entity
                            intCount = objJobOrderEnt.CheckUseInAccounting(strJobOrder)
                            If intCount <= 0 Then
                                ' call function CheckUseInStock from entity
                                intCount = objJobOrderEnt.CheckUseInStock(strJobOrder)
                                If intCount <= 0 Then
                                    ' call function CheckUseInStockOut from entity
                                    intCount = objJobOrderEnt.CheckUseInStockOut(strJobOrder)
                                End If
                            End If
                        End If
                    End If
                End If

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInJobOrder = True
                Else
                    ' case equal 0 then return False
                    IsUsedInJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try

        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As Entity.IJob_OrderEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpJob_OrderEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search job order screen 
                    .job_order_from_search = objJobOrderDto.job_order_from_search
                    .job_order_to_search = objJobOrderDto.job_order_to_search
                    .customer_search = objJobOrderDto.customer_search
                    .receive_po_search = objJobOrderDto.receive_po_search
                    .issue_date_from_search = objJobOrderDto.issue_date_from_search
                    .issue_date_to_search = objJobOrderDto.issue_date_to_search
                    .Job_finish_search = objJobOrderDto.Job_finish_search
                    .finish_date_from_search = objJobOrderDto.finish_date_from_search
                    .finish_date_to_search = objJobOrderDto.finish_date_to_search
                    .part_name_search = objJobOrderDto.part_name_search
                    .part_no_search = objJobOrderDto.part_no_search
                    .job_type_search = objJobOrderDto.job_type_search
                    .boi_search = objJobOrderDto.boi_search
                    .person_charge_search = objJobOrderDto.person_charge_search

                    'Receive data from menagement job order  screen 
                    .id = objJobOrderDto.id
                    .job_order = objJobOrderDto.job_order
                    .old_job_order = objJobOrderDto.old_job_order
                    .issue_date = objJobOrderDto.issue_date
                    .customer = objJobOrderDto.customer
                    .end_user = objJobOrderDto.end_user
                    .receive_po = objJobOrderDto.receive_po
                    .person_in_charge = objJobOrderDto.person_in_charge
                    .job_type_id = objJobOrderDto.job_type_id
                    .is_boi = objJobOrderDto.is_boi
                    .create_at = objJobOrderDto.create_at
                    .part_name = objJobOrderDto.part_name
                    .part_no = objJobOrderDto.part_no
                    .part_type = objJobOrderDto.part_type
                    .payment_term_id = objJobOrderDto.payment_term_id
                    .currency_id = objJobOrderDto.currency_id
                    .hontai_chk1 = objJobOrderDto.hontai_chk1
                    .hontai_date1 = objJobOrderDto.hontai_date1
                    .hontai_amount1 = objJobOrderDto.hontai_amount1
                    .hontai_condition1 = objJobOrderDto.hontai_condition1
                    .hontai_chk2 = objJobOrderDto.hontai_chk2
                    .hontai_date2 = objJobOrderDto.hontai_date2
                    .hontai_amount2 = objJobOrderDto.hontai_amount2
                    .hontai_condition2 = objJobOrderDto.hontai_condition2
                    .hontai_chk3 = objJobOrderDto.hontai_chk3
                    .hontai_date3 = objJobOrderDto.hontai_date3
                    .hontai_amount3 = objJobOrderDto.hontai_amount3
                    .hontai_condition3 = objJobOrderDto.hontai_condition3
                    .hontai_amount = objJobOrderDto.hontai_amount
                    .total_amount = objJobOrderDto.total_amount
                    .quotation_amount = objJobOrderDto.quotation_amount
                    .remark = objJobOrderDto.remark
                    .finish_fg = objJobOrderDto.finish_fg
                    .status_id = objJobOrderDto.status_id
                    .create_at_remark = objJobOrderDto.create_at_remark
                    .payment_condition_id = objJobOrderDto.payment_condition_id
                    .payment_condition_remark = objJobOrderDto.payment_condition_remark
                    .finish_date = objJobOrderDto.finish_date
                    .detail = objJobOrderDto.detail

                    'Receive data from upload job order po screen 
                    .job_month = objJobOrderDto.job_month
                    .job_year = objJobOrderDto.job_year
                    .job_last = objJobOrderDto.job_last
                    .ip_address = objJobOrderDto.ip_address


                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoPOUploadToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoPOUploadToEntity( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As Entity.IJob_OrderEntity
            ' set default return value
            SetDtoPOUploadToEntity = New Entity.ImpJob_OrderEntity
            Try
                ' assign value to entity
                With SetDtoPOUploadToEntity
                    'Receive data from menagement job order po screen 
                    .id = objJobOrderDto.id
                    .ip_address = objJobOrderDto.ip_address
                    .po_type = objJobOrderDto.po_type
                    .po_no = objJobOrderDto.po_no
                    .po_amount = objJobOrderDto.po_amount
                    .po_date = objJobOrderDto.po_date
                    .po_file = objJobOrderDto.po_file
                    .po_receipt_date = objJobOrderDto.po_receipt_date

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoPOUploadToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoQuoUploadToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoQuoUploadToEntity( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As Entity.IJob_OrderEntity
            ' set default return value
            SetDtoQuoUploadToEntity = New Entity.ImpJob_OrderEntity
            Try
                ' assign value to entity
                With SetDtoQuoUploadToEntity
                    'Receive data from menagement job order quotation screen 
                    .id = objJobOrderDto.id
                    .ip_address = objJobOrderDto.ip_address
                    .quo_type = objJobOrderDto.quo_type
                    .quo_no = objJobOrderDto.quo_no
                    .quo_amount = objJobOrderDto.quo_amount
                    .quo_date = objJobOrderDto.quo_date
                    .quo_file = objJobOrderDto.quo_file

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoQuoUploadToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobOrderTemp
        '	Discription	    : Delete Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobOrderTemp( _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.DeleteJobOrderTemp
            ' set default return value
            DeleteJobOrderTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteJobOrderTemp from job order Entity
                intEff = objJobOrderEnt.DeleteJobOrderTemp(strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteJobOrderTemp = True
                Else
                    ' case row less than 1 then return False
                    DeleteJobOrderTemp = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobOrderTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteJobTemp
        '	Discription	    : Delete Job Order temp table
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteJobTemp( _
            ByVal intJobOrderID As Integer, _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.DeleteJobTemp
            ' set default return value
            DeleteJobTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteJobTemp from job order Entity
                intEff = objJobOrderEnt.DeleteJobTemp(intJobOrderID, strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteJobTemp = True
                Else
                    ' case row less than 1 then return False
                    DeleteJobTemp = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteJobTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderByID
        '	Discription	    : Get Job Order by ID
        '	Return Value	: Special Job Order dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    : Suwishaya L.
        '	Update Date	    : 26-09-2013
        '*************************************************************/
        Public Function GetJobOrderByID( _
            ByVal intJobOrderID As Integer _
        ) As Dto.JobOrderDto Implements IJobOrderService.GetJobOrderByID
            ' set default return value
            GetJobOrderByID = New Dto.JobOrderDto
            Try
                ' object for return value from Entity
                Dim objJobOrderEntRet As New Entity.ImpJob_OrderEntity
                ' call function GetJobOrderByID from Entity
                objJobOrderEntRet = objJobOrderEntRet.GetJobOrderByID(intJobOrderID)

                ' assign value from Entity to Dto
                With GetJobOrderByID
                    .id = objJobOrderEntRet.id
                    .job_order = objJobOrderEntRet.job_order
                    .issue_date = objJobOrderEntRet.issue_date
                    .customer = objJobOrderEntRet.customer
                    .end_user = objJobOrderEntRet.end_user
                    .receive_po = objJobOrderEntRet.receive_po
                    .person_in_charge = objJobOrderEntRet.person_in_charge
                    .job_type_id = objJobOrderEntRet.job_type_id
                    .is_boi = objJobOrderEntRet.is_boi
                    .create_at = objJobOrderEntRet.create_at
                    .part_name = objJobOrderEntRet.part_name
                    .part_no = objJobOrderEntRet.part_no
                    .part_type = objJobOrderEntRet.part_type
                    .payment_term_id = objJobOrderEntRet.payment_term_id
                    .currency_id = objJobOrderEntRet.currency_id
                    .hontai_amount = objJobOrderEntRet.hontai_amount
                    .hontai_chk1 = objJobOrderEntRet.hontai_chk1
                    .hontai_date1 = objJobOrderEntRet.hontai_date1
                    .hontai_amount1 = objJobOrderEntRet.hontai_amount1
                    .hontai_condition1 = objJobOrderEntRet.hontai_condition1
                    .hontai_chk2 = objJobOrderEntRet.hontai_chk2
                    .hontai_date2 = objJobOrderEntRet.hontai_date2
                    .hontai_amount2 = objJobOrderEntRet.hontai_amount2
                    .hontai_condition2 = objJobOrderEntRet.hontai_condition2
                    .hontai_chk3 = objJobOrderEntRet.hontai_chk3
                    .hontai_date3 = objJobOrderEntRet.hontai_date3
                    .hontai_amount3 = objJobOrderEntRet.hontai_amount3
                    .hontai_condition3 = objJobOrderEntRet.hontai_condition3
                    .hontai_amount = objJobOrderEntRet.hontai_amount
                    .total_amount = objJobOrderEntRet.total_amount
                    .quotation_amount = objJobOrderEntRet.quotation_amount
                    .remark = objJobOrderEntRet.remark
                    .detail = objJobOrderEntRet.detail
                    .finish_fg = objJobOrderEntRet.finish_fg
                    .status_id = objJobOrderEntRet.status_id
                    .finish_date = objJobOrderEntRet.finish_date
                    .create_at_remark = objJobOrderEntRet.create_at_remark
                    .payment_condition_id = objJobOrderEntRet.payment_condition_id
                    .payment_condition_remark = objJobOrderEntRet.payment_condition_remark
                    'Add 2013/09/26 (Req No.22)
                    .status_id = objJobOrderEntRet.status_id
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderRunning
        '	Discription	    : Get Job Order Running No
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderRunning( _
            ByVal intIssueMonth As Integer, _
            ByVal intIssueYear As Integer _
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderRunning
            ' set default
            GetJobOrderRunning = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderRunning(intIssueMonth, intIssueYear)

                ' assign column header
                With GetJobOrderRunning
                    .Columns.Add("job_month")
                    .Columns.Add("job_year")
                    .Columns.Add("job_last")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("job_month") = values.job_month_detail
                        row("job_year") = values.job_year_detail
                        row("job_last") = values.job_last_detail
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderRunning(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrderTemp
        '	Discription	    : Insert Job Order Temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertJobOrderTemp( _
            ByVal intJobOrderId As Integer, _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.InsertJobOrderTemp
            ' set default return value
            InsertJobOrderTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteJobOrderTemp from job order Entity
                intEff = objJobOrderEnt.InsertJobOrderTemp(intJobOrderId, strIpAddress)

                ' check row effect
                If intEff >= 0 Then
                    ' case row more than 0 then return True
                    InsertJobOrderTemp = True
                Else
                    ' case row less than 1 then return False
                    InsertJobOrderTemp = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrderTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumPoAmount
        '	Discription	    : Get sum po amount
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumPoAmount( _
            ByVal strIpAddress As String _
        ) As System.Data.DataTable Implements IJobOrderService.GetSumPoAmount
            ' set default
            GetSumPoAmount = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetSumPoAmount(strIpAddress)

                ' assign column header
                With GetSumPoAmount
                    .Columns.Add("sum_po_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("sum_po_amount") = Format(Convert.ToDouble(values.sum_po_amount.ToString.Trim), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumPoAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumQuoAmount
        '	Discription	    : Get sum quotation amount
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-09-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumQuoAmount( _
            ByVal strIpAddress As String _
        ) As System.Data.DataTable Implements IJobOrderService.GetSumQuoAmount
            ' set default
            GetSumQuoAmount = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetSumQuoAmount(strIpAddress)

                ' assign column header
                With GetSumQuoAmount
                    .Columns.Add("sum_quo_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("sum_quo_amount") = Format(Convert.ToDouble(values.sum_quo_amount.ToString.Trim), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumQuoAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTotalAmount
        '	Discription	    : Get total sum po amount
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTotalAmount( _
            ByVal strIpAddress As String _
        ) As System.Data.DataTable Implements IJobOrderService.GetTotalAmount
            ' set default
            GetTotalAmount = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetTotalAmount(strIpAddress)

                ' assign column header
                With GetTotalAmount
                    .Columns.Add("total_po_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("total_po_amount") = Format(Convert.ToDouble(values.total_po_amount.ToString.Trim), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTotalAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumUploadPoAmount
        '	Discription	    : Get sum po amount
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
        ) As System.Data.DataTable Implements IJobOrderService.GetSumUploadPoAmount
            ' set default
            GetSumUploadPoAmount = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetSumUploadPoAmount from entity
                listJobOrderEnt = objJobOrderEnt.GetSumUploadPoAmount(strIpAddress, intJobOrderID, intMode)

                ' assign column header
                With GetSumUploadPoAmount
                    .Columns.Add("sum_po_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("sum_po_amount") = Format(Convert.ToDouble(values.sum_po_amount.ToString.Trim), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumUploadPoAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUploadTotalAmount
        '	Discription	    : Get total sum po amount
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUploadTotalAmount( _
            ByVal strIpAddress As String, _
            ByVal intJobOrderID As Integer, _
            ByVal intMode As Integer _
        ) As System.Data.DataTable Implements IJobOrderService.GetUploadTotalAmount
            ' set default
            GetUploadTotalAmount = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetUploadTotalAmount(strIpAddress, intJobOrderID, intMode)

                ' assign column header
                With GetUploadTotalAmount
                    .Columns.Add("total_po_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("total_po_amount") = Format(Convert.ToDouble(values.total_po_amount.ToString.Trim), "#,##0.00")
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUploadTotalAmount(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPaymentConditionDetail
        '	Discription	    : Get total sum po amount
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 18-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentConditionDetail( _
            ByVal intPayment_condition_id As Integer _
        ) As System.Data.DataTable Implements IJobOrderService.GetPaymentConditionDetail
            ' set default
            GetPaymentConditionDetail = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderList from entity
                listJobOrderEnt = objJobOrderEnt.GetPaymentConditionDetail(intPayment_condition_id)

                ' assign column header
                With GetPaymentConditionDetail
                    .Columns.Add("payment_condition_detail")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("payment_condition_detail") = values.payment_condition_detail.ToString.Trim
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentConditionDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertJobOrder
        '	Discription	    : Insert Job Order
        '	Return Value	: Boolean
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
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobOrderService.InsertJobOrder
            ' set default return value
            InsertJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertJobOrder from Job Order Entity
                intEff = objJobOrderEnt.InsertJobOrder(strYear, strMonth, strJobLast, strJobOrder, SetDtoToEntity(objJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertJobOrder = True
                Else
                    ' case row less than 1 then return False
                    InsertJobOrder = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_02_003"
                Else
                    ' other case
                    strMsg = "KTJB_02_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrder
        '	Discription	    : Update Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrder( _
            ByVal strYear As String, _
            ByVal strMonth As String, _
            ByVal strJobLast As String, _
            ByVal strJobOrder As String, _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobOrderService.UpdateJobOrder
            ' set default return value
            UpdateJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateJobOrder from Job Order Entity
                intEff = objJobOrderEnt.UpdateJobOrder(strYear, strMonth, strJobLast, strJobOrder, SetDtoToEntity(objJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateJobOrder = True
                Else
                    ' case row less than 1 then return False
                    UpdateJobOrder = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_02_006"
                Else
                    ' other case
                    strMsg = "KTJB_02_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: RestoreJobOrder
        '	Discription	    : Restore Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 27-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function RestoreJobOrder( _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobOrderService.RestoreJobOrder
            ' set default return value
            RestoreJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateJobOrder from Job Order Entity
                intEff = objJobOrderEnt.RestoreJobOrder(SetDtoToEntity(objJobOrderDto))

                ' check row effect
                If intEff >= 0 Then
                    ' case row more than 0 then return True
                    RestoreJobOrder = True
                Else
                    ' case row less than 1 then return False
                    RestoreJobOrder = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_09_003"
                Else
                    ' other case
                    strMsg = "KTJB_09_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("RestoreJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderDetailList
        '	Discription	    : Get Job Order Detail list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderDetailList( _
            ByVal intJobOrderID As Integer _
        ) As Dto.JobOrderDto Implements IJobOrderService.GetJobOrderDetailList
            ' set default
            GetJobOrderDetailList = New Dto.JobOrderDto
            Try
                Dim strDate1 As String = ""
                Dim strDate2 As String = ""
                Dim strDate3 As String = ""
                Dim strIssueDate As String = ""

                ' object for return value from Entity
                Dim objJobOrderEntRet As New Entity.ImpJob_OrderEntity
                ' call function GetJobOrderByID from Entity
                objJobOrderEntRet = objJobOrderEntRet.GetJobOrderDetailList(intJobOrderID)

                ' assign value from Entity to Dto
                With GetJobOrderDetailList
                    .id = objJobOrderEntRet.id
                    .job_order = objJobOrderEntRet.job_order

                    If objJobOrderEntRet.issue_date.ToString.Trim.Length > 0 Then
                        strIssueDate = Left(objJobOrderEntRet.issue_date, 4) & "/" & Mid(objJobOrderEntRet.issue_date, 5, 2) & "/" & Right(objJobOrderEntRet.issue_date, 2)
                        .issue_date = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                    Else
                        .issue_date = objJobOrderEntRet.issue_date
                    End If

                    .customer_name = objJobOrderEntRet.customer_name
                    .end_user_name = objJobOrderEntRet.end_user_name
                    .receive_po_name = objJobOrderEntRet.receive_po_name
                    .person_in_charge_name = objJobOrderEntRet.person_in_charge_name
                    .job_order_type_Detail = objJobOrderEntRet.job_order_type_Detail
                    .is_boi_name = objJobOrderEntRet.is_boi_name
                    .create_at_name = objJobOrderEntRet.create_at_name
                    .part_name = objJobOrderEntRet.part_name
                    .part_no = objJobOrderEntRet.part_no
                    .part_type_name = objJobOrderEntRet.part_type_name
                    .term_day = objJobOrderEntRet.term_day
                    .payment_condition_id = objJobOrderEntRet.payment_condition_id
                    .currency_id = objJobOrderEntRet.currency_id
                    .currency_name = objJobOrderEntRet.currency_name
                    .hontai_chk1 = objJobOrderEntRet.hontai_chk1
                    .hontai_amount1 = Format(Convert.ToDouble(objJobOrderEntRet.hontai_amount1.ToString.Trim), "#,##0.00")
                    .hontai_condition1 = objJobOrderEntRet.hontai_condition1
                    .hontai_chk2 = objJobOrderEntRet.hontai_chk2
                    .hontai_amount2 = Format(Convert.ToDouble(objJobOrderEntRet.hontai_amount2.ToString.Trim), "#,##0.00")
                    .hontai_condition2 = objJobOrderEntRet.hontai_condition2
                    .hontai_chk3 = objJobOrderEntRet.hontai_chk3
                    If objJobOrderEntRet.hontai_date1.ToString.Trim.Length > 0 Then
                        strDate1 = Left(objJobOrderEntRet.hontai_date1, 4) & "/" & Mid(objJobOrderEntRet.hontai_date1, 5, 2) & "/" & Right(objJobOrderEntRet.hontai_date1, 2)
                        .hontai_date1 = CDate(strDate1).ToString("dd/MMM/yyyy")
                    Else
                        .hontai_date1 = objJobOrderEntRet.hontai_date1
                    End If

                    If objJobOrderEntRet.hontai_date2.ToString.Trim.Length > 0 Then
                        strDate2 = Left(objJobOrderEntRet.hontai_date2, 4) & "/" & Mid(objJobOrderEntRet.hontai_date2, 5, 2) & "/" & Right(objJobOrderEntRet.hontai_date2, 2)
                        .hontai_date2 = CDate(strDate2).ToString("dd/MMM/yyyy")
                    Else
                        .hontai_date2 = objJobOrderEntRet.hontai_date2
                    End If

                    If objJobOrderEntRet.hontai_date3.ToString.Trim.Length > 0 Then
                        strDate3 = Left(objJobOrderEntRet.hontai_date3, 4) & "/" & Mid(objJobOrderEntRet.hontai_date3, 5, 2) & "/" & Right(objJobOrderEntRet.hontai_date3, 2)
                        .hontai_date3 = CDate(strDate3).ToString("dd/MMM/yyyy")
                    Else
                        .hontai_date3 = objJobOrderEntRet.hontai_date3
                    End If

                    .hontai_amount3 = Format(Convert.ToDouble(objJobOrderEntRet.hontai_amount3.ToString.Trim), "#,##0.00")
                    .hontai_condition3 = objJobOrderEntRet.hontai_condition3
                    .hontai_amount = Format(Convert.ToDouble(objJobOrderEntRet.hontai_amount.ToString.Trim), "#,##0.00")
                    .total_amount = Format(Convert.ToDouble(objJobOrderEntRet.total_amount.ToString.Trim), "#,##0.00")
                    .remark = objJobOrderEntRet.remark
                    .finish_fg = objJobOrderEntRet.finish_fg
                    .status_id = objJobOrderEntRet.status_id
                    .finish_date = objJobOrderEntRet.finish_date
                    .create_at_remark = objJobOrderEntRet.create_at_remark
                    .payment_condition_id = objJobOrderEntRet.payment_condition_id
                    .payment_condition_remark = objJobOrderEntRet.payment_condition_remark
                    .payment_condition_name = objJobOrderEntRet.payment_condition_name
                    .quotation_amount = objJobOrderEntRet.quotation_amount
                End With

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderDetailList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderPOList
            ' set default
            GetJobOrderPOList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderPOList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderPOList(intJobOrderID)

                ' assign column header
                With GetJobOrderPOList
                    .Columns.Add("job_order_id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("po_amount")
                    .Columns.Add("po_date")
                    .Columns.Add("receipt_date")
                    .Columns.Add("po_file")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("job_order_id") = values.id
                        row("po_type") = values.po_type_detail
                        row("po_no") = values.po_no_detail
                        row("po_amount") = Format(Convert.ToDouble(values.po_amount_detail.ToString.Trim), "#,##0.00")
                        row("po_date") = values.po_date_detail
                        row("receipt_date") = values.po_receipt_date_detail
                        row("po_file") = values.po_file_detail

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderPOList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderQuoList
        '	Discription	    : Get Job Order Quo list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 20-06-2013
        '	Update User	    : Rawikarn K.
        '	Update Date	    : 10-02-2014
        '*************************************************************/
        Public Function GetJobOrderQuoList( _
            ByVal intJobOrderID As Integer _
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderQuoList
            ' set default
            GetJobOrderQuoList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderQuoList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderQuoList(intJobOrderID)

                ' assign column header
                With GetJobOrderQuoList
                    .Columns.Add("job_order_id")
                    .Columns.Add("quo_type")
                    .Columns.Add("quo_no")
                    '.Columns.Add("quo_amount")
                    .Columns.Add("quo_date")
                    .Columns.Add("quo_file")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("job_order_id") = values.id
                        row("quo_type") = values.quo_type_detail
                        row("quo_no") = values.quo_no_detail
                        'row("quo_amount") = Format(Convert.ToDouble(values.quo_amount_detail.ToString.Trim), "#,##0.00")
                        row("quo_date") = values.quo_date_detail
                        row("quo_file") = values.quo_file_detail

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderQuoList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderInvoiceList
            ' set default
            GetJobOrderInvoiceList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim strReceipt_date As String = ""
                Dim strIssue_date As String = ""

                ' call function GetJobOrderInvoiceList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderInvoiceList(intJobOrderID)

                ' assign column header
                With GetJobOrderInvoiceList
                    .Columns.Add("status")
                    .Columns.Add("invoice_no")
                    .Columns.Add("receipt_date")
                    .Columns.Add("po_type")
                    .Columns.Add("account_title")
                    .Columns.Add("issue_date")
                    .Columns.Add("total_amount")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow

                        If values.inv_receipt_date.ToString.Trim.Length > 0 Then
                            strReceipt_date = Left(values.inv_receipt_date, 4) & "/" & Mid(values.inv_receipt_date, 5, 2) & "/" & Right(values.inv_receipt_date, 2)
                            row("receipt_date") = CDate(strReceipt_date).ToString("dd/MMM/yyyy")
                        Else
                            row("receipt_date") = values.inv_receipt_date
                        End If

                        If values.inv_issue_date.ToString.Trim.Length > 0 Then
                            strIssue_date = Left(values.inv_issue_date, 4) & "/" & Mid(values.inv_issue_date, 5, 2) & "/" & Right(values.inv_issue_date, 2)
                            row("issue_date") = CDate(strIssue_date).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.inv_issue_date
                        End If

                        row("po_type") = values.po_type_detail
                        row("status") = values.status
                        row("invoice_no") = values.invoice_no
                        row("account_title") = values.account_title
                        row("total_amount") = Format(Convert.ToDouble(values.inv_total_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderInvoiceList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderPOTempList
            ' set default
            GetJobOrderPOTempList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim strReceipt_date As String = ""
                Dim strPodate As String = ""

                ' call function GetJobOrderPOTempList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderPOTempList(strIpAddress)

                ' assign column header
                With GetJobOrderPOTempList
                    .Columns.Add("no")
                    .Columns.Add("id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("po_amount")
                    .Columns.Add("po_date")
                    .Columns.Add("po_receipt_date")
                    .Columns.Add("po_file")
                    .Columns.Add("check_use")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("no") = values.no
                        row("id") = values.id
                        row("po_type") = values.po_type_detail
                        row("po_no") = values.po_no_detail
                        row("po_amount") = Format(Convert.ToDouble(values.po_amount_detail.ToString.Trim), "#,##0.00")
                        row("po_file") = values.po_file_detail

                        'Set po_date to dd/MMM/yyyy format
                        If values.po_date_detail.ToString.Trim.Length > 0 Then
                            strPodate = Left(values.po_date_detail, 4) & "/" & Mid(values.po_date_detail, 5, 2) & "/" & Right(values.po_date_detail, 2)
                            row("po_date") = CDate(strPodate).ToString("dd/MMM/yyyy")
                        Else
                            row("po_date") = values.po_date_detail
                        End If

                        'Set receipt_date to dd/MMM/yyyy format
                        If values.po_receipt_date_detail.ToString.Trim.Length > 0 Then
                            strReceipt_date = Left(values.po_receipt_date_detail, 4) & "/" & Mid(values.po_receipt_date_detail, 5, 2) & "/" & Right(values.po_receipt_date_detail, 2)
                            row("po_receipt_date") = CDate(strReceipt_date).ToString("dd/MMM/yyyy")
                        Else
                            row("po_receipt_date") = values.po_receipt_date_detail
                        End If

                        row("check_use") = values.check_use

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderPOTempList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
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
        ) As System.Data.DataTable Implements IJobOrderService.GetJobOrderQuoTempList
            ' set default
            GetJobOrderQuoTempList = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim strQuodate As String = ""

                ' call function GetJobOrderQuoTempList from entity
                listJobOrderEnt = objJobOrderEnt.GetJobOrderQuoTempList(strIpAddress)

                ' assign column header
                With GetJobOrderQuoTempList
                    .Columns.Add("no")
                    .Columns.Add("id")
                    .Columns.Add("quo_type")
                    .Columns.Add("quo_no")
                    .Columns.Add("quo_amount")
                    .Columns.Add("quo_date")
                    .Columns.Add("quo_file")


                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("no") = values.no
                        row("quo_type") = values.quo_type_detail
                        row("quo_no") = values.quo_no_detail
                        row("quo_amount") = Format(Convert.ToDouble(values.quo_amount_detail.ToString.Trim), "#,##0.00")
                        row("quo_file") = values.quo_file_detail

                        'Set quo_date to dd/MMM/yyyy format
                        If values.quo_date_detail.ToString.Trim.Length > 0 Then
                            strQuodate = Left(values.quo_date_detail, 4) & "/" & Mid(values.quo_date_detail, 5, 2) & "/" & Right(values.quo_date_detail, 2)
                            row("quo_date") = CDate(strQuodate).ToString("dd/MMM/yyyy")
                        Else
                            row("quo_date") = values.quo_date_detail
                        End If
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderQuoTempList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoFile
        '	Discription	    : Check exist PO File on job order po temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoFile( _
            ByVal strPoFile As String, _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.CheckExistPoFile
            ' set default return value
            CheckExistPoFile = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistPoFile from job order Entity
                intEff = objJobOrderEnt.CheckExistPoFile(strPoFile, strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistPoFile = False
                Else
                    ' case row less than 1 then return False
                    CheckExistPoFile = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoFile(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoNo
        '	Discription	    : Check exist PO No on job order po 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoNo( _
            ByVal strPoNo As String, _
            Optional ByVal strJobOrderId As String = "" _
        ) As Boolean Implements IJobOrderService.CheckExistPoNo
            ' set default return value
            CheckExistPoNo = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistPoNo from job order Entity
                intEff = objJobOrderEnt.CheckExistPoNo(strPoNo, strJobOrderId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistPoNo = False
                Else
                    ' case row less than 1 then return False
                    CheckExistPoNo = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoNo(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoNoTemp
        '	Discription	    : Check exist PO No on job order po temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoNoTemp( _
            ByVal strPoNo As String _
        ) As Boolean Implements IJobOrderService.CheckExistPoNoTemp
            ' set default return value
            CheckExistPoNoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistPoNoTemp from job order Entity
                intEff = objJobOrderEnt.CheckExistPoNoTemp(strPoNo)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistPoNoTemp = False
                Else
                    ' case row less than 1 then return False
                    CheckExistPoNoTemp = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoNoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoNoTemp
        '	Discription	    : Check data before delete data on job_order_po_tmp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoTemp( _
            ByVal intId As Integer, _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.CheckExistPoTemp
            ' set default return value
            CheckExistPoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistPoTemp from job order Entity
                intEff = objJobOrderEnt.CheckExistPoTemp(intId, strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistPoTemp = False
                Else
                    ' case row less than 1 then return False
                    CheckExistPoTemp = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistPoType
        '	Discription	    : Check exist PO type on job order po temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistPoType( _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.CheckExistPoType
            ' set default return value
            CheckExistPoType = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistPoType from job order Entity
                intEff = objJobOrderEnt.CheckExistPoType(strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistPoType = False
                Else
                    ' case row less than 1 then return False
                    CheckExistPoType = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistPoType(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoFile
        '	Discription	    : Check exist Quo File on job order quo temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoFile( _
            ByVal strQuoFile As String, _
            ByVal strIpAddress As String _
        ) As Boolean Implements IJobOrderService.CheckExistQuoFile
            ' set default return value
            CheckExistQuoFile = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistQuoFile from job order Entity
                intEff = objJobOrderEnt.CheckExistQuoFile(strQuoFile, strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistQuoFile = False
                Else
                    ' case row less than 1 then return False
                    CheckExistQuoFile = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoFile(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoNo
        '	Discription	    : Check exist Quo No on job order quo 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoNo( _
            ByVal strQuoNo As String, _
            Optional ByVal strJobOrderId As String = "" _
        ) As Boolean Implements IJobOrderService.CheckExistQuoNo
            ' set default return value
            CheckExistQuoNo = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistQuoNo from job order Entity
                intEff = objJobOrderEnt.CheckExistQuoNo(strQuoNo, strJobOrderId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistQuoNo = False
                Else
                    ' case row less than 1 then return False
                    CheckExistQuoNo = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoNo(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistQuoNoTemp
        '	Discription	    : Check exist Quo No on job order quo temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistQuoNoTemp( _
            ByVal strQuoNo As String _
        ) As Boolean Implements IJobOrderService.CheckExistQuoNoTemp
            ' set default return value
            CheckExistQuoNoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistQuoNoTemp from job order Entity
                intEff = objJobOrderEnt.CheckExistQuoNoTemp(strQuoNo)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistQuoNoTemp = False
                Else
                    ' case row less than 1 then return False
                    CheckExistQuoNoTemp = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistQuoNoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckExistReceiveDetail
        '	Discription	    : Check data on receive_detail  
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckExistReceiveDetail( _
            ByVal intJobOrderId As Integer _
        ) As Boolean Implements IJobOrderService.CheckExistReceiveDetail
            ' set default return value
            CheckExistReceiveDetail = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckExistReceiveDetail from job order Entity
                intEff = objJobOrderEnt.CheckExistReceiveDetail(intJobOrderId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckExistReceiveDetail = False
                Else
                    ' case row less than 1 then return False
                    CheckExistReceiveDetail = True
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckExistReceiveDetail(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeletePOTemp
        '	Discription	    : Delete job order po temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePOTemp( _
            ByVal intId As Integer _
        ) As Boolean Implements IJobOrderService.DeletePOTemp
            ' set default return value
            DeletePOTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeletePOTemp from job order Entity
                intEff = objJobOrderEnt.DeletePOTemp(intId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeletePOTemp = True
                Else
                    ' case row less than 1 then return False
                    DeletePOTemp = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePOTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: DeleteQuoTemp
        '	Discription	    : Delete job order quo temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteQuoTemp( _
            ByVal intId As Integer _
        ) As Boolean Implements IJobOrderService.DeleteQuoTemp
            ' set default return value
            DeleteQuoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeletePOTemp from job order Entity
                intEff = objJobOrderEnt.DeleteQuoTemp(intId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteQuoTemp = True
                Else
                    ' case row less than 1 then return False
                    DeleteQuoTemp = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteQuoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertPoTemp
        '	Discription	    : Insert job order po temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPoTemp( _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobOrderService.InsertPoTemp
            ' set default return value
            InsertPoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function InsertPoTemp from Job order Entity
                intEff = objJobOrderEnt.InsertPoTemp(SetDtoPOUploadToEntity(objJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertPoTemp = True
                Else
                    ' case row less than 1 then return False
                    InsertPoTemp = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return

                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_01_PO_009"
                Else
                    ' other case
                    strMsg = "KTJB_02_PO_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertQuoTemp
        '	Discription	    : Insert job order quo temp
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 24-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertQuoTemp( _
            ByVal objJobOrderDto As Dto.JobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IJobOrderService.InsertQuoTemp
            ' set default return value
            InsertQuoTemp = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function InsertQuoTemp from Job order Entity
                intEff = objJobOrderEnt.InsertQuoTemp(SetDtoQuoUploadToEntity(objJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertQuoTemp = True
                Else
                    ' case row less than 1 then return False
                    InsertQuoTemp = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_01_Quo_009"
                Else
                    ' other case
                    strMsg = "KTJB_02_Quo_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertQuoTemp(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTableReport
        '	Discription	    : Get table report
        '	Return Value	: Datatable
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 28-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTableReport( _
            ByVal dtValues As System.Data.DataTable, _
            ByVal dtSumValues As System.Data.DataTable _
        ) As System.Data.DataTable Implements IJobOrderService.GetTableReport
            ' set default return value
            GetTableReport = New DataTable
            Try
                Dim dtReport As New DataTable
                Dim dr As DataRow

                ' set header columns
                With dtReport
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("issue_date")
                    .Columns.Add("issue_by")
                    .Columns.Add("customer")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("part_type")
                    .Columns.Add("job_type_id")
                    .Columns.Add("job_new")
                    .Columns.Add("job_mod")
                    .Columns.Add("Detail")
                    .Columns.Add("quo_date")
                    .Columns.Add("quo_no")
                    .Columns.Add("currency")
                    .Columns.Add("hontai_amount")
                    .Columns.Add("hontai_amount_thb")
                    .Columns.Add("sum_hontai_amount")
                End With

                ' loop set data to table report
                For Each values As DataRow In dtValues.Rows
                    dr = dtReport.NewRow

                    dr.Item("id") = values.Item("id")
                    dr.Item("job_order") = values.Item("job_order")
                    dr.Item("issue_date") = values.Item("issue_date")
                    dr.Item("issue_by") = values.Item("issue_by")
                    dr.Item("customer") = values.Item("customer")
                    dr.Item("part_name") = values.Item("part_name")
                    dr.Item("part_no") = values.Item("part_no")
                    dr.Item("part_type") = values.Item("part_type")
                    dr.Item("job_type_id") = values.Item("job_type_id")
                    dr.Item("job_new") = values.Item("job_new")
                    dr.Item("job_mod") = values.Item("job_mod")
                    dr.Item("Detail") = values.Item("Detail")
                    dr.Item("quo_date") = values.Item("quo_date")
                    dr.Item("quo_no") = values.Item("quo_no")
                    dr.Item("currency") = values.Item("currency")
                    dr.Item("hontai_amount") = values.Item("hontai_amount")
                    dr.Item("hontai_amount_thb") = values.Item("hontai_amount_thb")
                    dr.Item("sum_hontai_amount") = dtSumValues.Rows(0).Item("sum_hontai_amount")

                    dtReport.Rows.Add(dr)
                Next
                ' return new datatable
                Return dtReport
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetTableReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInJobOrderPo
        '	Discription	    : Check data in used job order po
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 08-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInJobOrderPo( _
            ByVal intJobOrderID As Integer _
        ) As Boolean Implements IJobOrderService.IsUsedInJobOrderPo
            ' set default return value
            IsUsedInJobOrderPo = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CheckUseInJobOrderPo from entity
                intCount = objJobOrderEnt.CheckUseInJobOrderPo(intJobOrderID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInJobOrderPo = True
                Else
                    ' case equal 0 then return False
                    IsUsedInJobOrderPo = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInJobOrderPo(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: GetOneJobOrderPOTempList
        '	Discription	    : Check data in used job order po
        '	Return Value	: 
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 13-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function GetOneJobOrderPOTempList(ByVal intPoId As Integer) As System.Data.DataTable Implements IJobOrderService.GetOneJobOrderPOTempList
            ' set default return value
            GetOneJobOrderPOTempList = New DataTable
            Try
                '' object f  or return value from Entity
                'Dim objJBPoId As New Entity.ImpJob_OrderEntity
                ' variable for keep list
                Dim objJBPoId As New List(Of Entity.ImpJob_OrderDetailEntity)
                Dim row As DataRow
                Dim strReceipt_date As String = ""
                Dim strPodate As String = ""


                ' call function GetItemByID from Entity
                objJBPoId = objJobOrderEnt.GetOneJobOrderPOTempList(intPoId)

                ' assign column header
                With GetOneJobOrderPOTempList
                    .Columns.Add("no")
                    .Columns.Add("id")
                    .Columns.Add("po_type")
                    .Columns.Add("po_no")
                    .Columns.Add("po_amount")
                    .Columns.Add("po_date")
                    .Columns.Add("po_receipt_date")
                    .Columns.Add("po_file")

                    ' assign row from listJobOrderEnt
                    For Each values In objJBPoId
                        row = .NewRow
                        row("no") = values.no
                        row("id") = values.id
                        row("po_type") = values.po_type_detail
                        row("po_no") = values.po_no_detail
                        row("po_amount") = Format(Convert.ToDouble(values.po_amount_detail.ToString.Trim), "#,##0.00")
                        row("po_file") = values.po_file_detail

                        'Set po_date to dd/MMM/yyyy format
                        If values.po_date_detail.ToString.Trim.Length > 0 Then
                            strPodate = Left(values.po_date_detail, 4) & "/" & Mid(values.po_date_detail, 5, 2) & "/" & Right(values.po_date_detail, 2)
                            row("po_date") = CDate(strPodate).ToString("dd/MMM/yyyy")
                        Else
                            row("po_date") = values.po_date_detail
                        End If

                        'Set receipt_date to dd/MMM/yyyy format
                        If values.po_receipt_date_detail.ToString.Trim.Length > 0 Then
                            strReceipt_date = Left(values.po_receipt_date_detail, 4) & "/" & Mid(values.po_receipt_date_detail, 5, 2) & "/" & Right(values.po_receipt_date_detail, 2)
                            row("po_receipt_date") = CDate(strReceipt_date).ToString("dd/MMM/yyyy")
                        Else
                            row("po_receipt_date") = values.po_receipt_date_detail
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                objLog.ErrorLog("GetOneJobOrderPOTempList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateJobOrderPOToTempList
        '	Discription	    : Update Job Order Temp
        '	Return Value	: Boolean
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 17-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateJobOrderPOToTempList( _
        ByVal intPOId As Integer, _
        ByVal objJobOrderDto As Dto.JobOrderDto, _
        ByVal strIpAddress As String, _
        Optional ByRef strMsg As String = "") _
        As Boolean Implements IJobOrderService.UpdateJobOrderPOToTempList
            ' set default return value
            UpdateJobOrderPOToTempList = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function UpdateJobOrderPOToTempList from Job order Entity
                intEff = objJobOrderEnt.UpdateJobOrderPOToTempList(intPOId, SetDtoPOUpDateToEntity(objJobOrderDto), strIpAddress)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateJobOrderPOToTempList = True
                Else
                    ' case row less than 1 then return False
                    UpdateJobOrderPOToTempList = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return

                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_01_PO_009"
                Else
                    ' other case
                    strMsg = "KTJB_02_PO_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateJobOrderPOToTempList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoPOUpDateToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 17-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetDtoPOUpDateToEntity( _
            ByVal objJobOrderDto As Dto.JobOrderDto _
        ) As Entity.IJob_OrderEntity
            ' set default return value
            SetDtoPOUpDateToEntity = New Entity.ImpJob_OrderEntity
            Try
                ' assign value to entity
                With SetDtoPOUpDateToEntity
                    'Receive data from menagement job order po screen 
                    .id = objJobOrderDto.id
                    .ip_address = objJobOrderDto.ip_address
                    .po_type = objJobOrderDto.po_type
                    .po_no = objJobOrderDto.po_no
                    .po_amount = objJobOrderDto.po_amount
                    .po_date = objJobOrderDto.po_date
                    .po_file = objJobOrderDto.po_file
                    .po_receipt_date = objJobOrderDto.po_receipt_date

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoPODateToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetOneQuoFromTmp
        '	Discription	    : Get Job Order Quo Temp list
        '	Return Value	: Datatable
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 28-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetOneQuoFromTmp( _
             ByVal intQuoId As Integer _
        ) As System.Data.DataTable Implements IJobOrderService.GetOneQuoFromTmp
            ' set default
            GetOneQuoFromTmp = New DataTable
            Try
                ' variable for keep list
                Dim listJobOrderEnt As New List(Of Entity.ImpJob_OrderDetailEntity)
                ' data row object
                Dim row As DataRow
                Dim strQuodate As String = ""

                ' call function GetJobOrderQuoTempList from entity
                listJobOrderEnt = objJobOrderEnt.GetOneQuoFromTmp(intQuoId)

                ' assign column header
                With GetOneQuoFromTmp
                    .Columns.Add("no")
                    .Columns.Add("id")
                    .Columns.Add("quo_type")
                    .Columns.Add("quo_no")
                    .Columns.Add("quo_amount")
                    .Columns.Add("quo_date")
                    .Columns.Add("quo_file")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("no") = values.job_order
                        row("quo_type") = values.quo_type
                        row("quo_no") = values.quo_no
                        row("quo_amount") = Format(Convert.ToDouble(values.quo_amount.ToString.Trim), "#,##0.00")
                        row("quo_file") = values.quo_file

                        'Set quo_date to dd/MMM/yyyy format
                        If values.quo_date.ToString.Trim.Length > 0 Then
                            strQuodate = Left(values.quo_date, 4) & "/" & Mid(values.quo_date, 5, 2) & "/" & Right(values.quo_date, 2)
                            row("quo_date") = CDate(strQuodate).ToString("dd/MMM/yyyy")
                        Else
                            row("quo_date") = values.quo_date
                        End If
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderQuoTempList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateQuotationToTmp
        '	Discription	    : Update Quotation 
        '	Return Value	: Datatable
        '	Create User	    : Rawikarn K.
        '	Create Date	    : 28-02-2014
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/

        Public Function UpdateQuotationToTmp( _
        ByVal objJobQuoDto As Dto.JobOrderDto, _
        ByVal intQuoId As Integer, _
        Optional ByRef Msg As String = "") _
        As Boolean Implements IJobOrderService.UpdateQuotationToTmp
            Try
                UpdateQuotationToTmp = False
                ' intEff keep row effect
                Dim intEff As Integer

                ' call function UpdateJobOrderPOToTempList from Job order Entity
                intEff = objJobOrderEnt.UpdateQuotationToTmp(SetDtoQuoUploadToEntity(objJobQuoDto), intQuoId)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateQuotationToTmp = True
                Else
                    ' case row less than 1 then return False
                    UpdateQuotationToTmp = False
                End If

            Catch exSql As MySqlException
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    Msg = "KTJB_01_PO_009"
                Else
                    ' other case
                    Msg = "KTJB_02_PO_003"
                End If
                objLog.ErrorLog("UpdateQuotationToTmp(Service)", exSql.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


#End Region


    End Class
End Namespace

