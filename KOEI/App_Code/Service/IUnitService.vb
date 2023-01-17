#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IUnitService
'	Class Discription	: Interface of Unit
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

Namespace Service
    Public Interface IUnitService
        Function GetUnitListForDropdownList() As List(Of Dto.UnitDto)
        Function SetListUnit(ByRef objValue As DropDownList) As Boolean
        Function GetUnitForSearch(ByVal strName As String, ByRef objDT As System.Data.DataTable) As Boolean
        Function CheckUnitForDel(ByVal intUnitId As Integer) As Boolean
        Function CancelUnit(ByVal intUnitId As Integer) As Boolean
        Function CheckIsDupUnit(ByVal strUnitName As String, Optional ByVal intUnitId As Integer = 0) As Boolean
        Function SaveUnit(ByVal objUnitDto As Dto.UnitDto, ByVal strMode As String) As Boolean

    End Interface
End Namespace


