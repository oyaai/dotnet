#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IMst_WorkingCategoryDao
'	Class Discription	: Interface of table mst_WorkingCategory
'	Create User 		: Pranitda Sroengklang
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
    Public Interface IMst_WorkingCategoryDao
        Function GetWorkingCategoryList(ByVal strWorkingCategoryName As String) As List(Of Entity.ImpMst_WorkingCategoryDetailEntity)
        Function DeleteWorkingCategory(ByVal intWorkingCategoryID As Integer) As Integer
        Function GetWorkingCategoryByID(ByVal intWorkingCategoryID As Integer) As Entity.IMst_WorkingCategoryEntity
        Function InsertWorkingCategory(ByVal objWorkingCategoryEnt As Entity.IMst_WorkingCategoryEntity) As Integer
        Function UpdateWorkingCategory(ByVal objWorkingCategoryEnt As Entity.IMst_WorkingCategoryEntity) As Integer
        Function CountUsedInPO(ByVal intItemID As Integer) As Integer
        Function CheckDupWorkCategory(ByVal itemName_new As String, ByVal id As String) As Integer
    End Interface
End Namespace

