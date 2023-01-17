#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IWTDao
'	Class Discription	: Interface of table mst_wt
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

#Region "Imports"
Imports Dto
Imports System.Linq.Expressions
Imports Entity
#End Region

Namespace Interfaces

    Public Interface IWTDao

#Region "Properties"
        ReadOnly Property ClassName() As String
#End Region

#Region "Functions"

        Function GetWTList(ByVal strID As String, ByVal strWT As String) As List(Of Entity.IWTEntity)
        Function CountUsedInPO(ByVal intWTID As Integer) As Integer
        Function DeleteWT(ByVal intWTID As Integer) As Integer
        Function GetWTByID(ByVal intWTID As Integer) As IWTEntity
        Function InsertWT(ByVal objWTEntity As IWTEntity) As Integer
        Function UpdateWT(ByVal objWTEntity As IWTEntity) As Integer
        Function CheckDupWT(ByVal intWTID As String, ByVal intWT As String) As Integer


        Function GetWTForList() As List(Of IWTEntity)

#End Region

    End Interface
End Namespace