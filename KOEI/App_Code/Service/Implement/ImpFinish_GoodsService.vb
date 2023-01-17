#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpFinish_GoodsService
'	Class Discription	: Implement Finish Goods Service
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
Imports System.Data
Imports MySql.Data.MySqlClient
#End Region

Namespace Service
    Public Class ImpFinish_GoodsService
        Implements IFinish_GoodsService

        Private objLog As New Common.Logs.Log
        Private objFinishGoodsEnt As New Entity.ImpFinish_GoodsEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetFinishGoodsList
        '	Discription	    : Get Finish Goods list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetFinishGoodsList( _
            ByVal objFinishGoodsDto As Dto.FinishGoodsDto _
        ) As System.Data.DataTable Implements IFinish_GoodsService.GetFinishGoodsList
            ' set default
            GetFinishGoodsList = New DataTable
            Try
                ' variable for keep list
                Dim listFinishGoodsEnt As New List(Of Entity.ImpFinish_GoodsEntity)
                ' data row object
                Dim row As DataRow 
                Dim strFinishDate As String = ""

                ' call function GetFinishGoodsList from entity
                listFinishGoodsEnt = objFinishGoodsEnt.GetFinishGoodsList(SetDtoToEntity(objFinishGoodsDto))

                ' assign column header
                With GetFinishGoodsList
                    .Columns.Add("id")
                    .Columns.Add("receive_header_id")
                    .Columns.Add("job_order")
                    .Columns.Add("finish_date")
                    .Columns.Add("customer")
                    .Columns.Add("job_order_type")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("total_amount")

                    ' assign row from listFinishGoodsEnt
                    For Each values In listFinishGoodsEnt
                        row = .NewRow
                        row("id") = values.id
                        row("receive_header_id") = values.receive_header_id
                        row("job_order") = values.job_order
                        If values.finish_date <> Nothing Or values.finish_date <> "" Then
                            strFinishDate = Left(values.finish_date, 4) & "/" & Mid(values.finish_date, 5, 2) & "/" & Right(values.finish_date, 2)
                            row("finish_date") = CDate(strFinishDate).ToString("dd/MMM/yyyy")
                        Else
                            row("finish_date") = values.finish_date
                        End If
                        row("customer") = values.customer_name
                        row("job_order_type") = values.job_order_type_name
                        row("part_name") = values.part_name
                        row("part_no") = values.part_no
                        row("total_amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetFinishGoodsList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetFinishGoodsReport
        '	Discription	    : Get Finish Goods Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetFinishGoodsReport( _
            ByVal objFinishGoodsDto As Dto.FinishGoodsDto _
        ) As System.Data.DataTable Implements IFinish_GoodsService.GetFinishGoodsReport
            ' set default
            GetFinishGoodsReport = New DataTable
            Try
                ' variable for keep list
                Dim listFinishGoodsEnt As New List(Of Entity.ImpFinish_GoodsEntity)
                ' data row object
                Dim row As DataRow
                Dim strFinishDate As String = ""

                ' call function GetFinishGoodsReport from entity
                listFinishGoodsEnt = objFinishGoodsEnt.GetFinishGoodsReport(SetDtoToEntity(objFinishGoodsDto))

                ' assign column header
                With GetFinishGoodsReport
                    .Columns.Add("job_order")
                    .Columns.Add("finish_date")
                    .Columns.Add("customer")
                    .Columns.Add("job_order_type")
                    .Columns.Add("part_name")
                    .Columns.Add("part_no")
                    .Columns.Add("amount")

                    ' assign row from listFinishGoodsEnt
                    For Each values In listFinishGoodsEnt
                        row = .NewRow
                        row("job_order") = values.job_order
                        row("job_order_type") = values.job_order_type_name
                        row("customer") = values.customer_name
                        row("part_name") = values.part_name
                        row("part_no") = values.part_no

                        If values.finish_date <> Nothing Or values.finish_date <> "" Then
                            strFinishDate = Left(values.finish_date, 4) & "/" & Mid(values.finish_date, 5, 2) & "/" & Right(values.finish_date, 2)
                            row("finish_date") = CDate(strFinishDate).ToString("dd/MMM/yyyy")
                        Else
                            row("finish_date") = values.finish_date
                        End If

                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetFinishGoodsReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumFinishGoodsReport
        '	Discription	    : Get Sum Finish Goods Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumFinishGoodsReport( _
            ByVal objFinishGoodsDto As Dto.FinishGoodsDto _
        ) As System.Data.DataTable Implements IFinish_GoodsService.GetSumFinishGoodsReport
            ' set default
            GetSumFinishGoodsReport = New DataTable
            Try
                ' variable for keep list
                Dim listFinishGoodsEnt As New List(Of Entity.ImpFinish_GoodsEntity)
                ' data row object
                Dim row As DataRow
                Dim strFinishDate As String = ""

                ' call function GetSumFinishGoodsReport from entity
                listFinishGoodsEnt = objFinishGoodsEnt.GetSumFinishGoodsReport(SetDtoToEntity(objFinishGoodsDto))

                ' assign column header
                With GetSumFinishGoodsReport 
                    .Columns.Add("sum_amount")

                    ' assign row from listFinishGoodsEnt
                    For Each values In listFinishGoodsEnt
                        row = .NewRow
                        row("sum_amount") = Format(Convert.ToDouble(values.sum_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumFinishGoodsReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPersonInChangeForList
        '	Discription	    : Get Person in change for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPersonInChangeForList() As System.Collections.Generic.List(Of Dto.FinishGoodsDto) Implements IFinish_GoodsService.GetPersonInChangeForList
            ' set default list
            GetPersonInChangeForList = New List(Of Dto.FinishGoodsDto)
            Try
                ' objFinishGoodsDto for keep value Dto 
                Dim objFinishGoodsDto As Dto.FinishGoodsDto
                ' listFinishGoodsEnt for keep value from entity
                Dim listFinishGoodsEnt As New List(Of Entity.IFinish_GoodsEntity)
                ' objFinishGoodsEnt for call function
                Dim objFinishGoodsEnt As New Entity.ImpFinish_GoodsEntity

                ' call function GetPersonInChangeForList
                listFinishGoodsEnt = objFinishGoodsEnt.GetPersonInChangeForList()

                ' loop listFinishGoodsEnt for assign value to Dto
                For Each values In listFinishGoodsEnt
                    ' new object
                    objFinishGoodsDto = New Dto.FinishGoodsDto
                    ' assign value to Dto
                    With objFinishGoodsDto
                        .person_in_charge_id = values.person_in_charge_id
                        .person_in_charge_name = values.person_in_charge_name
                    End With
                    ' add object Dto to list Dto
                    GetPersonInChangeForList.Add(objFinishGoodsDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPersonInChangeForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objFinishGoodsDto As Dto.FinishGoodsDto _
        ) As Entity.IFinish_GoodsEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpFinish_GoodsEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search job order screen 
                    .job_order_from_search = objFinishGoodsDto.job_order_from_search
                    .job_order_to_search = objFinishGoodsDto.job_order_to_search
                    .job_type_search = objFinishGoodsDto.job_type_search
                    .part_no_search = objFinishGoodsDto.part_no_search
                    .part_name_search = objFinishGoodsDto.part_name_search
                    .customer_search = objFinishGoodsDto.customer_search
                    .person_charge_search = objFinishGoodsDto.person_charge_search
                    .issue_date_from_search = objFinishGoodsDto.issue_date_from_search
                    .issue_date_to_search = objFinishGoodsDto.issue_date_to_search
                    .finish_date_from_search = objFinishGoodsDto.finish_date_from_search
                    .finish_date_to_search = objFinishGoodsDto.finish_date_to_search
                    .boi_search = objFinishGoodsDto.boi_search
                    .receive_po_search = objFinishGoodsDto.receive_po_search
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
        
    End Class
End Namespace
