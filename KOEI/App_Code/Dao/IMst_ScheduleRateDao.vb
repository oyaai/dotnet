#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_ScheduleRateDao
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
Imports Entity
#End Region

Namespace Dao
    Public Interface IMst_ScheduleRateDao
        Function DB_GetScheduleRateByPurchase(ByVal intCurrency_id As Integer, Optional ByVal strDate As String = "") As Decimal
        Function DB_GetScheduleRateByCurrency(ByVal intCurrency_id As Integer, Optional ByVal strEFDate As String = "", Optional ByVal intRanking As Integer = 11) As List(Of IMst_ScheduleRateEntity)
        Function DB_CancelScheduleRate(ByVal intScheduleRate_id As Integer) As Boolean
        Function DB_CheckIsDupScheduleRate(ByVal intCurrency_id As Integer, ByVal strEF_date As String, Optional ByVal intScheduleRate_id As Integer = 0) As Boolean
        Function DB_InsertScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer
        Function DB_GetScheduleRateById(ByVal intScheduleRate_id As Integer) As IMst_ScheduleRateEntity
        Function DB_UpdateScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer

    End Interface
End Namespace


