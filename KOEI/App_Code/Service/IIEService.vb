
#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IIEService
'	Class Discription	: Interface class Account title service
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
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

Imports Dto
Imports System.Data

#End Region


Namespace Interfaces

    Public Interface IIEService

#Region "Functions"
        Function GetIEListForDropdownList(Optional ByVal showCode As Boolean = False) As List(Of Dto.IEDto)
        Function SetListIE(ByRef objValue As DropDownList) As Boolean
        Function GetIEList(ByVal strID As String, ByVal strIECategory As String, ByVal strIECode As String, ByVal strIEName As String) As DataTable
        Function IsUsedInPO(ByVal intIEID As Integer) As Boolean
        Function IsUsedInPO2(ByVal intIEID As Integer) As Boolean
        Function GetIEByID(ByVal intIEID As Integer) As Dto.IEDto
        Function DeleteIE(ByVal intIEID As Integer) As Boolean
        Function InsertIE(ByVal objIEDto As Dto.IEDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateIE(ByVal objIEDto As Dto.IEDto, Optional ByRef strMsg As String = "") As Boolean
        Function CheckDupIE(ByVal intIEID As String, ByVal strIECode As String, ByVal strIECategory As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function GetAccountTitleForList() As List(Of Dto.IEDto)
        ' Add by Wasan D. to get data Account Title to dropdownlist #15/07/2013#
        Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As List(Of Dto.IEDto)
#End Region

    End Interface

End Namespace