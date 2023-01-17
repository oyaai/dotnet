#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_UnitDao
'	Class Discription	: Interface of table mst_unit
'	Create User 		: Boon
'	Create Date		    : 04-06-2013
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
    Public Interface IMst_UnitDao
        Function DB_GetUnitForSearch(ByVal strName As String) As List(Of Entity.IMst_UnitEntity)
        Function DB_CancelUnit(ByVal intUnit As Integer) As Boolean
        Function DB_CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean
        Function DB_InsertUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean
        Function DB_UpdateUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean
        Function DB_GetUnitForList() As List(Of Entity.IMst_UnitEntity)
    End Interface
End Namespace

