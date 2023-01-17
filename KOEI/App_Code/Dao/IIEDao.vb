#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IIEDao
'	Class Discription	: Interface of table mst_ie
'	Create User 		: Nisa S.
'	Create Date		    : 24-05-2013
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
Imports Entity

#End Region


Namespace Interfaces
    Public Interface IIEDao

#Region "Properties"
        ReadOnly Property ClassName() As String
#End Region

#Region "Function"

        Function DB_GetIEForList(Optional ByVal showCode As Boolean = False) As List(Of Entity.ImpIEEntity)
        Function GetIEList(ByVal strID As String, ByVal strIECategory As String, ByVal strIECode As String, ByVal strIEName As String) As List(Of Entity.ImpMst_IEDetailEntity)
        Function CountUsedInPO(ByVal intIEID As Integer) As Integer
        Function CountUsedInPO2(ByVal intIEID As Integer) As Integer
        Function GetIEByID(ByVal intIEID As Integer) As IMst_IEEntity
        Function DeleteIE(ByVal intIEID As Integer) As Integer
        Function InsertIE(ByVal objIEEnt As IMst_IEEntity) As Integer
        Function UpdateIE(ByVal objIEEnt As IMst_IEEntity) As Integer
        Function DB_CheckIEByCategory(ByVal intCategory_id As Integer) As Boolean
        Function CheckDupIE(ByVal intIEID As String, ByVal strIECode As String, ByVal strIECategory As String) As Integer
        Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As List(Of Entity.ImpIEEntity)
        Function GetAccountTitleForList() As List(Of Entity.ImpIEEntity)
#End Region

    End Interface
End Namespace