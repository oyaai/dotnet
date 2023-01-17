#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IIECategoryService
'	Class Discription	: Interface class IE service
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
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

#End Region


Namespace Interfaces
    Public Interface IIECategoryService

#Region "Functions"

        Function GetAll() As List(Of IECategoryDto)

#End Region

    End Interface
End Namespace