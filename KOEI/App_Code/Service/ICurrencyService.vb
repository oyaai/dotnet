#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ICurrencyService
'	Class Discription	: Interface of Currency
'	Create User 		: Boonyarit
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Data
#End Region

Namespace Service
    Public Interface ICurrencyService
        'Function SetListCurrency(ByRef objValue As DropDownList, Optional ByVal optionAll As Boolean = False) As Boolean
        Function GetCurrencyForList() As List(Of Dto.CurrencyDto)
        Function GetCurrencyForDropdownList() As List(Of Dto.CurrencyDto)

        Function GetCurrencyList(ByVal strCurrency As String) As DataTable
        Function IsUsedInPO(ByVal intCurrencyID As Integer) As Boolean
        Function IsUsedInPO2(ByVal intCurrencyID As Integer) As Boolean
        Function DeleteCurrency(ByVal intCurrencyID As Integer) As Boolean
        Function GetCurrencyByID(ByVal intCurrencyID As Integer) As Dto.CurrencyDto
        Function CheckDupCurrency(ByVal intCurrencyID As String, ByVal intCurrency As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateCurrency(ByVal objCurrencyDto As Dto.CurrencyDto, Optional ByRef strMsg As String = "") As Boolean
        Function InsertCurrency(ByVal objCurrencyDto As Dto.CurrencyDto, Optional ByRef strMsg As String = "") As Boolean

    End Interface
End Namespace

