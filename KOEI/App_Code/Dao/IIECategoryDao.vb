#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IIECategoryDao
'	Class Discription	: Interface of table mst_ie_category
'	Create User 		: Nisa s.
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

Imports Entity

#End Region


Namespace Interfaces
    Public Interface IIECategoryDao

#Region "Properties"
        ReadOnly Property ClassName() As String
#End Region

#Region "Functions"

        Function GetAll() As List(Of IIECategoryEntity)

#End Region

    End Interface
End Namespace