#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IIECategoryEntity
'	Class Discription	: Interface of table mst_ie_category
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

Imports Entity

#End Region



Namespace Interfaces

    Public Interface IIECategoryEntity

#Region "Properties"
        Property ID() As Byte
        Property Name() As String
        Property NameJp() As String
        Property DeleteFg() As Byte
        Property CreatedBy() As Int32?
        Property CreatedDate() As String
        Property UpdatedBy() As Int32?
        Property UpdatedDate() As String
#End Region

#Region "Functions"
        Function GetAll() As List(Of IIECategoryEntity)

#End Region

    End Interface

End Namespace