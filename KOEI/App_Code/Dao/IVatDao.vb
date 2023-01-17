#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IVatDao
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
Imports System.Data
Imports Entity
Imports Microsoft.VisualBasic
Imports Dto
#End Region

Namespace Interfaces
    Public Interface IVatDao

#Region "Functions"

      
        Function GetVatList(ByVal strID As String, ByVal strPercent As String) As List(Of ImpVatDetailEntity)
        Function CountUsedInPO(ByVal intVatID As Integer) As Integer
        Function DeleteVat(ByVal intVatID As Integer) As Integer
        Function GetVatByID(ByVal intVatID As Integer) As IVatEntity
        Function CheckDupVat(ByVal intVatID As String, ByVal intVat As String) As Integer
        Function InsertVat(ByVal objVatEnt As IVatEntity) As Integer
        Function UpdateVat(ByVal objVatEnt As IVatEntity) As Integer

        Function GetVatForList() As List(Of IVatEntity)

#End Region

#Region "Properties"
        ReadOnly Property ClassName() As String
#End Region

    End Interface
End Namespace