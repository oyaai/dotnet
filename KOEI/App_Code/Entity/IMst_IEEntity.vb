#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMst_IEEntity
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
    Public Interface IMst_IEEntity

#Region "Properties"

        Property ID() As Integer
        Property CategoryID() As Byte
        Property Code() As String
        Property Name() As String
        Property DeleteFg() As Byte
        Property CreatedBy() As Int32?
        Property CreatedDate() As String
        Property UpdatedBy() As Int32?
        Property UpdatedDate() As String
        Property Category() As ImpIECategoryEntity
        Property IsInUsed() As Boolean

#End Region

#Region "Function"

        Function GetIEForList(Optional ByVal showCode As Boolean = False) As List(Of ImpIEEntity)
        Function GetIEList(ByVal strID As String, ByVal strIECategory As String, ByVal strIECode As String, ByVal strIEName As String) As List(Of Entity.ImpMst_IEDetailEntity)
        Function GetIEByID(ByVal intIEID As Integer) As IMst_IEEntity
        Function CountUsedInPO(ByVal intIEID As Integer) As Integer
        Function CountUsedInPO2(ByVal intIEID As Integer) As Integer
        Function DeleteIE(ByVal intIEID As Integer) As Integer
        Function InsertIE(ByVal objIEEnt As IMst_IEEntity) As Integer
        Function UpdateIE(ByVal objIEEnt As IMst_IEEntity) As Integer
        Function CheckDupIE(ByVal intIEID As String, ByVal strIECode As String, ByVal strIECategory As String) As Integer
        Function GetAccountTitleForList() As List(Of Entity.ImpIEEntity)
        ' Add by Wasan D. to get data Account Title to dropdownlist #15/07/2013#
        Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As List(Of Entity.ImpIEEntity)
#End Region

    End Interface
End Namespace