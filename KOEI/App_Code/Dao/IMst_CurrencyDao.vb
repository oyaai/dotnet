#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_CountryDao
'	Class Discription	: Interface of table IMst_CurrencyDao
'	Create User 		: Boon
'	Create Date		    : 17-06-2013
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

Namespace Dao
    Public Interface IMst_CurrencyDao
        Function DB_GetCurrencyForList() As List(Of Entity.IMst_CurrencyEntity)
        Function GetCurrencyForDropdownList() As List(Of Entity.IMst_CurrencyEntity)

        Function GetCurrencyList(ByVal strCurrency As String) As List(Of Entity.IMst_CurrencyEntity)
        Function CountUsedInPO(ByVal intCurrencyID As Integer) As Integer
        Function CountUsedInPO2(ByVal intCurrencyID As Integer) As Integer
        Function DeleteCurrency(ByVal intCurrencyID As Integer) As Integer
        Function GetCurrencyByID(ByVal intCurrencyID As Integer) As Entity.IMst_CurrencyEntity
        Function InsertCurrency(ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity) As Integer
        Function UpdateCurrency(ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity) As Integer
        Function CheckDupCurrency(ByVal intCurrencyID As String, ByVal intCurrency As String) As Integer
    End Interface
End Namespace

