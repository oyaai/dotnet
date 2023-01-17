#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_CountryEntity
'	Class Discription	: Interface of table mst_country
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Interface IMst_CountryEntity
        Property id() As Integer
        Property name() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String

        Function GetListCountryName() As List(Of Entity.IMst_CountryEntity)
        Function GetCountryList(ByVal strCountryName As String) As List(Of Entity.ImpMst_CountryDetailEntity)
        Function DeleteCountry(ByVal intCountryID As Integer) As Integer
        Function GetCountryByID(ByVal intCountryID As Integer) As Entity.IMst_CountryEntity
        Function InsertCountry(ByVal objCountryEnt As Entity.IMst_CountryEntity) As Integer
        Function UpdateCountry(ByVal objCountryEnt As Entity.IMst_CountryEntity) As Integer
        Function CheckDupCountry(ByVal strCountryName As String, ByVal id As String) As Integer
        Function CountUsedInVendor(ByVal intCountryID As Integer) As Integer
    End Interface
End Namespace


