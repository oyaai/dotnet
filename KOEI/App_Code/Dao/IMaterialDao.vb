#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMaterialDao
'	Class Discription	: Interface of table stock
'	Create User 		: Suwishaya L.
'	Create Date		    : 03-07-2013
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
    Public Interface IMaterialDao
        Function GetMaterialList(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)
        Function GetMaterialListReport(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)
        Function GetSumMaterialListReport(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)

    End Interface
End Namespace
