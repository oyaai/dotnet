#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IVatEntity
'	Class Discription	: Interface of table mst_vat
'	Create User 		: Nisa S.
'	Create Date		    : 25-06-2013
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
Imports Microsoft.VisualBasic
#End Region

Namespace Interfaces

    Public Interface IVatEntity

#Region "Properties"

        Property CreatedBy() As Long?
        Property CreatedDate() As String
        Property DeleteFlag() As Byte
        Property ID() As Byte
        Property Percent() As Byte?
        Property UpdatedBy() As Long?
        Property UpdatedDate() As String

        Property IsInUsed() As Boolean

#End Region

#Region "Functions"

        
        Function GetVatList(ByVal strID As String, ByVal strPercent As String) As List(Of ImpVatDetailEntity)
        Function CountUsedInPO(ByVal intVatID As Integer) As Integer
        Function DeleteVat(ByVal intVatID As Integer) As Integer
        Function CheckDupVat(ByVal intVatID As String, ByVal intVat As String) As Integer
        Function InsertVat(ByVal objVatEnt As IVatEntity) As Integer
        Function UpdateVat(ByVal objVatEnt As IVatEntity) As Integer
        Function GetVatByID(ByVal intVatID As Integer) As IVatEntity

        Function GetVatForList() As List(Of IVatEntity)

#End Region

    End Interface
End Namespace