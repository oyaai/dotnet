#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IDepartmentService
'	Class Discription	: Interface of Department
'	Create User 		: Suwishaya L.
'	Create Date		    : 04-06-2013
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
    Public Interface ICountryService
        Function SetListCountryName(ByRef objValue As DropDownList, Optional ByVal OptionAll As Boolean = False, Optional ByVal OptionText As String = "") As Boolean
        Function GetCountryList(ByVal strCountryName As String) As DataTable
        Function DeleteCountry(ByVal intCountryID As Integer) As Boolean
        Function GetCountryByID(ByVal intCountryID As Integer) As Dto.CountryDto
        Function InsertCountry(ByVal objCountryDto As Dto.CountryDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateCountry(ByVal objCountryDto As Dto.CountryDto, Optional ByRef strMsg As String = "") As Boolean
        Function CheckDupCountry(ByVal strCountryName As String, ByVal id As String) As Boolean
        Function IsUsedInVendor(ByVal intCountryID As Integer) As Boolean
    End Interface
End Namespace
