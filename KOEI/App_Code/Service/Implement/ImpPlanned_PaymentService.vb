#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPlanned_PaymentService
'	Class Discription	: Implement Planned Payment Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-08-2013
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
    Public Class ImpPlanned_PaymentService
        Implements IPlanned_PaymentService

        Private objLog As New Common.Logs.Log
        Private objPlannedPaymentEnt As New Entity.ImpPlanned_PaymentEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetYearList
        '	Discription	    : Get Year for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 05-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetYearList() As System.Data.DataTable Implements IPlanned_PaymentService.GetYearList
            ' set default list
            GetYearList = New DataTable
            Try
                ' listPlannedPaymentEnt for keep value Entity 
                Dim listPlannedPaymentEnt As New List(Of Entity.IPlanned_PaymentEntity)
                Dim startYear As Integer = 0
                Dim currentYear As Integer = 0
                Dim row As DataRow
                Dim j As Integer = 0

                ' call function GetYearList
                listPlannedPaymentEnt = objPlannedPaymentEnt.GetYearList()

                With GetYearList
                    .Columns.Add("id")
                    .Columns.Add("name")

                    'loop year assign value to Dto
                    For Each values In listPlannedPaymentEnt
                        startYear = values.min_year - 1
                        currentYear = DateTime.Now.Year
                        If values.max_year > currentYear Then
                            currentYear = values.max_year
                        End If
                    Next

                    For i As Integer = startYear To currentYear - 1
                        row = .NewRow
                        row("id") = j
                        row("name") = startYear + 1
                        ' add data row to table
                        .Rows.Add(row)
                        startYear = startYear + 1
                        j += 1
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetYearList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAmountThbForReport
        '	Discription	    : Get Amount Thb for report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAmountThbForReport( _
            ByVal intYear As Integer _
        ) As System.Data.DataTable Implements IPlanned_PaymentService.GetAmountThbForReport
            ' set default
            GetAmountThbForReport = New DataTable
            Try
                ' variable for keep list
                Dim listPlannedPaymentEnt As New List(Of Entity.ImpPlanned_PaymentEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetInvoiceForReport from entity
                listPlannedPaymentEnt = objPlannedPaymentEnt.GetAmountThbForReport(intYear)

                ' assign column header
                With GetAmountThbForReport
                    .Columns.Add("job_order_id")
                    .Columns.Add("pay_date")
                    .Columns.Add("amount_thb")

                    ' assign row from listPlannedPaymentEnt
                    For Each values In listPlannedPaymentEnt
                        row = .NewRow
                        row("job_order_id") = values.job_order_id
                        row("pay_date") = values.pay_date
                        row("amount_thb") = values.amount_thb

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAmountThbForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetInvoiceForReport
        '	Discription	    : Get invoice for report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetInvoiceForReport( _
            ByVal intYear As Integer _
        ) As System.Data.DataTable Implements IPlanned_PaymentService.GetInvoiceForReport
            ' set default
            GetInvoiceForReport = New DataTable
            Try
                ' variable for keep list
                Dim listPlannedPaymentEnt As New List(Of Entity.ImpPlanned_PaymentEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetInvoiceForReport from entity
                listPlannedPaymentEnt = objPlannedPaymentEnt.GetInvoiceForReport(intYear)

                ' assign column header
                With GetInvoiceForReport
                    .Columns.Add("job_order_id")
                    .Columns.Add("invoice_no")
                    .Columns.Add("receive_header_id")

                    ' assign row from listPlannedPaymentEnt
                    For Each values In listPlannedPaymentEnt
                        row = .NewRow
                        row("job_order_id") = values.job_order_id
                        row("invoice_no") = values.invoice_no
                        row("receive_header_id") = values.receive_header_id

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetInvoiceForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetJobOrderForReport
        '	Discription	    : Get job order for report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetJobOrderForReport( _
            ByVal intYear As Integer _
        ) As System.Data.DataTable Implements IPlanned_PaymentService.GetJobOrderForReport
            ' set default
            GetJobOrderForReport = New DataTable
            Try
                ' variable for keep list
                Dim listPlannedPaymentEnt As New List(Of Entity.ImpPlanned_PaymentEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetJobOrderForReport from entity
                listPlannedPaymentEnt = objPlannedPaymentEnt.GetJobOrderForReport(intYear)

                ' assign column header
                With GetJobOrderForReport
                    .Columns.Add("job_order_id")
                    .Columns.Add("job_order")
                    .Columns.Add("customer")
                    .Columns.Add("description")

                    ' assign row from listPlannedPaymentEnt
                    For Each values In listPlannedPaymentEnt
                        row = .NewRow
                        row("job_order_id") = values.job_order_id
                        row("job_order") = values.job_order
                        row("customer") = values.customer
                        row("description") = values.description

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetJobOrderForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetMaxPayDateForReport
        '	Discription	    : Get max pay datefor report
        '	Return Value	: Planned_Payment dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetMaxPayDateForReport( _
            ByVal intYear As Integer _
        ) As Dto.PlannedPaymentDto Implements IPlanned_PaymentService.GetMaxPayDateForReport
            ' set default return value
            GetMaxPayDateForReport = New Dto.PlannedPaymentDto
            Try
                ' object for return value from Entity
                Dim objPlannedPaymentEntRet As New Entity.ImpPlanned_PaymentEntity
                ' call function GetMaxPayDateForReport from Entity
                objPlannedPaymentEntRet = objPlannedPaymentEntRet.GetMaxPayDateForReport(intYear)

                ' assign value from Entity to Dto
                With GetMaxPayDateForReport
                    .max_pay_date = objPlannedPaymentEntRet.max_pay_date

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetMaxPayDateForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSumAmountThbForReport
        '	Discription	    : Get sum amount thb for report
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSumAmountThbForReport( _
            ByVal intYear As Integer _
        ) As System.Data.DataTable Implements IPlanned_PaymentService.GetSumAmountThbForReport
            ' set default
            GetSumAmountThbForReport = New DataTable
            Try
                ' variable for keep list
                Dim listPlannedPaymentEnt As New List(Of Entity.ImpPlanned_PaymentEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetSumAmountThbForReport from entity
                listPlannedPaymentEnt = objPlannedPaymentEnt.GetSumAmountThbForReport(intYear)

                ' assign column header
                With GetSumAmountThbForReport
                    .Columns.Add("job_order_id")
                    .Columns.Add("pay_date")
                    .Columns.Add("sum_amount_thb")

                    ' assign row from listPlannedPaymentEnt
                    For Each values In listPlannedPaymentEnt
                        row = .NewRow
                        row("job_order_id") = values.job_order_id
                        row("pay_date") = values.pay_date
                        row("sum_amount_thb") = values.sum_amount_thb

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSumAmountThbForReport(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
#End Region

    End Class
End Namespace
