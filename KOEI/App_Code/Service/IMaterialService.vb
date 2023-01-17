#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IMaterialService
'	Class Discription	: Interface of Material
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
Imports System.Data
#End Region

Namespace Service
    Public Interface IMaterialService
        Function GetMaterialList(ByVal objMaterialDto As Dto.MaterialDto) As DataTable
        Function GetMaterialListReport(ByVal objMaterialDto As Dto.MaterialDto) As DataTable
        Function GetSumMaterialListReport(ByVal objMaterialDto As Dto.MaterialDto) As DataTable
    End Interface
End Namespace
