#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_ScheduleRateEntity
'	Class Discription	: Interface of table Mst_Schedule_Rate
'	Create User 		: Boon
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
#End Region

Namespace Entity
    Public Interface IMst_ScheduleRateEntity
        Property id() As Integer
        Property currency_id() As Integer
        Property ef_date() As DateString
        Property rate() As Decimal
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As DateString
        Property updated_by() As Integer
        Property updated_date() As DateString
        'เพิ่มในส่วนของ table search
        Property currency() As String

        Function GetScheduleRateByPurchase(ByVal intCurrency_id As Integer, Optional ByVal strDate As String = "") As Decimal
        Function GetScheduleRateByCurrency(ByVal intCurrency_id As Integer, Optional ByVal strEFDate As String = "", Optional ByVal intRanking As Integer = 11) As List(Of IMst_ScheduleRateEntity)
        Function CancelScheduleRate(ByVal intScheduleRate_id As Integer) As Boolean
        Function CheckIsDupScheduleRate(ByVal intCurrency_id As Integer, ByVal strEF_date As String, Optional ByVal intScheduleRate_id As Integer = 0) As Boolean
        Function InsertScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer
        Function GetScheduleRateById(ByVal intScheduleRate_id As Integer) As IMst_ScheduleRateEntity
        Function UpdateScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer

    End Interface
End Namespace

