#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IScheculeRateService
'	Class Discription	: Interface of Schecule Rate
'	Create User 		: Boonyarit
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

Namespace Service
    Public Interface IScheculeRateService
        Function GetScheculeRate(ByVal intCurrencyId As Integer, Optional ByVal strDate As String = "") As Decimal
        Function GetScheculeRateForSearch(ByVal intCurrency_id As Integer, ByVal strEFDate As String, ByVal intRanking As Integer, ByRef objDT As System.Data.DataTable) As Boolean
        Function CancelScheculeRate(ByVal intScheculeRateID As Integer) As Boolean
        Function CheckDataIsDup(ByVal objSRate As Dto.ScheculeRateDto) As Boolean
        Function SaveScheculeRate(ByVal objSRate As Dto.ScheculeRateDto, ByVal strMode As String) As Boolean
        Function GetScheculeRateForDetail(ByVal intScheculeRateID As Integer) As Dto.ScheculeRateDto

    End Interface
End Namespace

