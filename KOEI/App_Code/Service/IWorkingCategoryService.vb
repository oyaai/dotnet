#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IWorkingCategoryService
'	Class Discription	: Interface class WorkingCategory service
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

Imports Microsoft.VisualBasic
Imports System.Data

Namespace Service
    Public Interface IWorkingCategoryService
        Function GetWorkingCategoryList(ByVal strWorkingCategoryName As String) As DataTable
        Function DeleteWorkingCategory(ByVal intWorkingCategoryID As Integer) As Boolean
        Function GetWorkingCategoryByID(ByVal intWorkingCategoryID As Integer) As Dto.WorkingCategoryDto
        Function InsertWorkingCategory(ByVal objWorkingCategoryDto As Dto.WorkingCategoryDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdateWorkingCategory(ByVal objWorkingCategoryDto As Dto.WorkingCategoryDto, Optional ByRef strMsg As String = "") As Boolean
        Function IsUsedInPO(ByVal intItemID As Integer) As Boolean
        Function CheckDupWorkCategory( _
            ByVal itemName_new As String, _
            ByVal id As String, _
            Optional ByRef strMsg As String = "") As Boolean
    End Interface
End Namespace

