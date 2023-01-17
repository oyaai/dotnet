#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IWTService
'	Class Discription	: Interface of WT
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Dto
Imports System.Linq.Expressions
Imports System.Data

Namespace Interfaces

    Public Interface IWTService

#Region "Functions"

        Function GetWTList(ByVal strID As String, ByVal strWT As String) As DataTable
        Function IsUsedInPO(ByVal intWTID As Integer) As Boolean
        Function DeleteWT(ByVal intWTID As Integer) As Boolean
        Function InsertWT(ByVal objWTDto As Dto.WTDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetWTByID(ByVal intWTID As Integer) As Dto.WTDto
        Function CheckDupWT(ByVal intWTID As String, ByVal intWT As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateWT(ByVal objWTDto As Dto.WTDto, Optional ByRef strMsg As String = "") As Boolean


        Function GetWTForList() As List(Of Dto.WTDto)
        Function SetListWT(ByRef objValue As DropDownList) As Boolean

#End Region

    End Interface

End Namespace