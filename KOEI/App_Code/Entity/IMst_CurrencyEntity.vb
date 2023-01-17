#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_CurrencyEntity
'	Class Discription	: Interface of table mst_currency
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

Namespace Entity
    Public Interface IMst_CurrencyEntity

#Region "Properyt"
        Property id() As Integer
        Property name() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property updated_by() As Integer
        Property updated_date() As String
#End Region

#Region "Function"
        Function GetCurrencyForList() As List(Of Entity.IMst_CurrencyEntity)
        Function GetCurrencyForDropdownList() As List(Of Entity.IMst_CurrencyEntity)

        Function GetCurrencyList(ByVal strCurrency As String) As List(Of Entity.IMst_CurrencyEntity)
        Function CountUsedInPO(ByVal intCurrencyID As Integer) As Integer
        Function CountUsedInPO2(ByVal intCurrencyID As Integer) As Integer
        Function DeleteCurrency(ByVal intCurrencyID As Integer) As Integer
        Function GetCurrencyByID(ByVal intCurrencyID As Integer) As Entity.IMst_CurrencyEntity
        Function InsertCurrency(ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity) As Integer
        Function UpdateCurrency(ByVal objCurrencyEnt As Entity.IMst_CurrencyEntity) As Integer
        Function CheckDupCurrency(ByVal intCurrencyID As String, ByVal intCurrency As String) As Integer


#End Region

    End Interface
End Namespace

