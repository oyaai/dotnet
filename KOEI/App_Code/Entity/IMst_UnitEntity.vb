#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_UnitEntity
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

Namespace Entity
    Public Interface IMst_UnitEntity

#Region "Property"
        Property id() As Integer
        Property name() As String
        Property delete_fg() As Integer
        Property created_by() As Integer
        Property created_date() As String
        Property update_by() As Integer
        Property update_date() As String
#End Region
        
#Region "Function"
        Function GetUnitForSearch(ByVal strName As String) As List(Of IMst_UnitEntity)
        Function CancelUnit(ByVal intUnitId As Integer) As Boolean
        Function CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean
        Function InsertUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean
        Function UpdateUnit(ByVal objUnit As Entity.IMst_UnitEntity) As Boolean
        Function GetUnitForList() As List(Of Entity.IMst_UnitEntity)

#End Region

    End Interface
End Namespace

