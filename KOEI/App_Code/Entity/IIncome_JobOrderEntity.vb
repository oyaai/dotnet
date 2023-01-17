#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IIncome_JobOrderEntity
'	Class Discription	: Interface of table receive_header
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
#End Region

Namespace Entity
    Public Interface IIncome_JobOrderEntity

#Region "Property"
        Property id() As Integer
        Property invoice_no() As String
        Property receipt_date() As String
        Property ie_id() As Integer
        Property vendor_id() As Integer
        Property account_type() As Integer
        Property invoice_type() As Integer
        Property bank_fee() As Decimal
        Property total_amount() As Decimal
        Property user_id() As Integer
        Property status_id() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
        Property issue_date() As String
        Property customer() As Integer

        'Receive data from screen (condition search)
        Property invoice_no_search() As String
        Property job_order_from_search() As String
        Property job_order_to_search() As String
        Property customer_search() As String
        Property issue_date_from_search() As String
        Property issue_date_to_search() As String
        Property receipt_date_from_search() As String
        Property receipt_date_to_search() As String

        'Receive data for detail search
        Property invoice_type_name() As String
        Property customer_name() As String
        Property amount_detail() As String

        'Receive data for report
        Property receive_header_id() As Integer
        Property receive_detail_id() As Integer
        Property payment_condition_id() As Integer
        Property job_order_id() As Integer
        Property job_order() As String
        Property hontai_type() As String
        Property stage() As String
        Property po_no() As String
        Property percent() As String
        Property price() As String
        Property vat() As String
        Property amount() As String
        Property remark() As String
        Property sum_amount() As String
        Property sum_price() As String
        Property sum_vat() As String
        Property actual_rate() As String
#End Region

#Region "Function"
        Function GetIncomeList(ByVal objIncomeJobOrderEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
        Function GetMonthlySaleReport(ByVal objIncomeJobOrderEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
        Function GetSumMonthlySaleReport(ByVal objIncomeJobOrderEnt As Entity.IIncome_JobOrderEntity) As List(Of Entity.ImpIncome_JobOrderEntity)
#End Region

    End Interface
End Namespace

