#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IVatService
'	Class Discription	: Interface of Vat
'	Create User 		: Nisa S.
'	Create Date		    : 25-06-2013
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
Imports System.Linq.Expressions
Imports Dto
Imports System.Data
#End Region

Namespace Interfaces
    Public Interface IVatService

#Region "Functions"


        Function GetVatList(ByVal strID As String, ByVal strPercent As String) As DataTable
        Function IsUsedInPO(ByVal intVatID As Integer) As Boolean
        Function DeleteVat(ByVal intVatID As Integer) As Boolean
        Function CheckDupVat(ByVal intVatID As String, ByVal intVat As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function InsertVat(ByVal objVatDto As Dto.VatDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateVat(ByVal objVatDto As Dto.VatDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetVatByID(ByVal intVatID As Integer) As Dto.VatDto

        Function GetVatForList() As List(Of VatDto)
        Function SetListVat(ByRef objValue As DropDownList) As Boolean

#End Region

    End Interface
End Namespace