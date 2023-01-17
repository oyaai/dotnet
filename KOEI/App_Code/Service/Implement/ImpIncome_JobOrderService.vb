#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpIncome_JobOrderService
'	Class Discription	: Implement income job order Service
'	Create User 		: Suwishaya L.
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
Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient
#End Region

Namespace Service
    Public Class ImpIncome_JobOrderService
        Implements IIncome_JobOrderService

        Private objLog As New Common.Logs.Log
        Private objIncomeJobOrderEnt As New Entity.ImpIncome_JobOrderEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetIncomeList
        '	Discription	    : Get Income Job Order list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetIncomeList( _
            ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto _
        ) As System.Data.DataTable Implements IIncome_JobOrderService.GetIncomeList
            ' set default
            GetIncomeList = New DataTable
            Try
                ' variable for keep list
                Dim listIncomeJobOrderEnt As New List(Of Entity.ImpIncome_JobOrderEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = ""
                Dim strReceiptDate As String = ""

                ' call function GetIncomeList from entity
                listIncomeJobOrderEnt = objIncomeJobOrderEnt.GetIncomeList(SetDtoToEntity(objIncomeJobOrderDto))

                ' assign column header
                With GetIncomeList
                    .Columns.Add("id")
                    .Columns.Add("invoice_no")
                    .Columns.Add("invoice_type")
                    .Columns.Add("issue_date")
                    .Columns.Add("receipt_date")
                    .Columns.Add("customer")
                    .Columns.Add("total_amount") 

                    ' assign row from listIncomeJobOrderEnt
                    For Each values In listIncomeJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("invoice_no") = values.invoice_no
                        row("invoice_type") = values.invoice_type_name  
                        row("customer") = values.customer_name

                        If values.issue_date <> Nothing Or values.issue_date <> "" Then
                            strIssueDate = Left(values.issue_date, 4) & "/" & Mid(values.issue_date, 5, 2) & "/" & Right(values.issue_date, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.issue_date
                        End If

                        If values.receipt_date <> Nothing Or values.receipt_date <> "" Then
                            strReceiptDate = Left(values.receipt_date, 4) & "/" & Mid(values.receipt_date, 5, 2) & "/" & Right(values.receipt_date, 2)
                            row("receipt_date") = CDate(strReceiptDate).ToString("dd/MMM/yyyy")
                        Else
                            row("receipt_date") = values.receipt_date
                        End If

                        row("total_amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetIncomeList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMonthlySaleReport
        '	Discription	    : Get Monthly Sale Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMonthlySaleReport( _
            ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto _
        ) As System.Data.DataTable Implements IIncome_JobOrderService.GetMonthlySaleReport
            ' set default
            GetMonthlySaleReport = New DataTable
            Try
                ' variable for keep list
                Dim listIncomeJobOrderEnt As New List(Of Entity.ImpIncome_JobOrderEntity)
                ' data row object
                Dim row As DataRow
                Dim strIssueDate As String = "" 

                ' call function GetMonthlySaleReport from entity
                listIncomeJobOrderEnt = objIncomeJobOrderEnt.GetMonthlySaleReport(SetDtoToEntity(objIncomeJobOrderDto))

                ' assign column header
                With GetMonthlySaleReport
                    .Columns.Add("id")
                    .Columns.Add("receive_header_id")
                    .Columns.Add("job_order_id")
                    .Columns.Add("issue_date")
                    .Columns.Add("invoice_no")
                    .Columns.Add("job_order")
                    .Columns.Add("customer")
                    .Columns.Add("price")
                    .Columns.Add("vat")
                    .Columns.Add("amount")
                    .Columns.Add("remark")
                    .Columns.Add("actual_rate")

                    ' assign row from listIncomeJobOrderEnt
                    For Each values In listIncomeJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("receive_header_id") = values.receive_header_id
                        row("job_order_id") = values.job_order_id
                        If values.issue_date <> Nothing Or values.issue_date <> "" Then
                            strIssueDate = Left(values.issue_date, 4) & "/" & Mid(values.issue_date, 5, 2) & "/" & Right(values.issue_date, 2)
                            row("issue_date") = CDate(strIssueDate).ToString("dd/MMM/yyyy")
                        Else
                            row("issue_date") = values.issue_date
                        End If
                        row("invoice_no") = values.invoice_no
                        row("job_order") = values.job_order
                        row("customer") = values.customer_name
                        row("price") = Format(Convert.ToDouble(values.price.ToString.Trim), "#,##0.00")
                        row("vat") = Format(Convert.ToDouble(values.vat.ToString.Trim), "#,##0.00")
                        row("amount") = Format(Convert.ToDouble(values.amount.ToString.Trim), "#,##0.00")
                        row("remark") = values.remark
                        row("actual_rate") = Format(Convert.ToDouble(values.actual_rate), "###0.00000")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMonthlySaleReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumMonthlySaleReport
        '	Discription	    : Get Sum Monthly Sale Report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumMonthlySaleReport( _
            ByVal objIncomeJobOrderDto As Dto.IncomeJobOrderDto _
        ) As System.Data.DataTable Implements IIncome_JobOrderService.GetSumMonthlySaleReport
            ' set default
            GetSumMonthlySaleReport = New DataTable
            Try
                ' variable for keep list
                Dim listIncomeJobOrderEnt As New List(Of Entity.ImpIncome_JobOrderEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetSumMonthlySaleReport from entity
                listIncomeJobOrderEnt = objIncomeJobOrderEnt.GetSumMonthlySaleReport(SetDtoToEntity(objIncomeJobOrderDto))

                ' assign column header
                With GetSumMonthlySaleReport
                    .Columns.Add("sum_price")
                    .Columns.Add("sum_vat")
                    .Columns.Add("sum_amount") 

                    ' assign row from listIncomeJobOrderEnt
                    For Each values In listIncomeJobOrderEnt
                        row = .NewRow
                        row("sum_price") = Format(Convert.ToDouble(values.sum_price.ToString.Trim), "#,##0.00")
                        row("sum_vat") = Format(Convert.ToDouble(values.sum_vat.ToString.Trim), "#,##0.00")
                        row("sum_amount") = Format(Convert.ToDouble(values.sum_amount.ToString.Trim), "#,##0.00")

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumMonthlySaleReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 01-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objincomeJobOrderDto As Dto.IncomeJobOrderDto _
        ) As Entity.IIncome_JobOrderEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpIncome_JobOrderEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    'Receive data from search job order screen 
                    .job_order_from_search = objincomeJobOrderDto.job_order_from_search
                    .job_order_to_search = objincomeJobOrderDto.job_order_to_search
                    .customer_search = objincomeJobOrderDto.customer_search
                    .issue_date_from_search = objincomeJobOrderDto.issue_date_from_search
                    .issue_date_to_search = objincomeJobOrderDto.issue_date_to_search
                    .invoice_no_search = objincomeJobOrderDto.invoice_no_search
                    .receipt_date_from_search = objincomeJobOrderDto.receipt_date_from_search
                    .receipt_date_to_search = objincomeJobOrderDto.receipt_date_to_search

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region
       
    End Class
End Namespace

