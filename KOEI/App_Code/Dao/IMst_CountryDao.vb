#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_CountryDao
'	Class Discription	: Interface of table mst_country
'	Create User 		: Boon
'	Create Date		    : 14-05-2013
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
    Public Interface IMst_CountryDao
        Function DB_GetListCountryName() As List(Of Entity.IMst_CountryEntity)
        Function GetCountryList(ByVal strCountryName As String) As List(Of Entity.ImpMst_CountryDetailEntity)
        Function DeleteCountry(ByVal intCountryID As Integer) As Integer
        Function GetCountryByID(ByVal intCountryID As Integer) As Entity.IMst_CountryEntity
        Function InsertCountry(ByVal objCountryEnt As Entity.IMst_CountryEntity) As Integer
        Function UpdateCountry(ByVal objCountryEnt As Entity.IMst_CountryEntity) As Integer
        Function CheckDupCountry(ByVal strCountryName As String, ByVal id As String) As Integer
        Function CountUsedInVendor(ByVal intCountryID As Integer) As Integer
    End Interface
End Namespace

