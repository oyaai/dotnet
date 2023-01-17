#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IWTEntity
'	Class Discription	: Interface of table mst_wt
'	Create User 		: Nisa S.
'	Create Date		    : 01-07-2013
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
Imports System.Linq.Expressions
#End Region

Namespace Entity
    Public Interface IWTEntity

#Region "Properties"

        Property CreatedBy() As Long

        '''<summary>
        '''
        '''</summary>
        Property CreatedDate() As String


        '''<summary>
        '''
        '''</summary>
        Property DeleteFlag() As Byte


        '''<summary>
        '''
        '''</summary>
        Property ID() As Byte


        '''<summary>
        '''
        '''</summary>
        Property Percent() As String


        '''<summary>
        '''
        '''</summary>

        Property Type() As String


        '''<summary>
        '''
        '''</summary>
        Property UpdatedBy() As Long?


        '''<summary>
        '''
        '''</summary>
        Property UpdatedDate() As String

        Property IsInUsed() As Boolean

#End Region

#Region "Functions"

        Function GetWTList(ByVal strID As String, ByVal strWT As String) As List(Of IWTEntity)
        Function CountUsedInPO(ByVal intWTID As Integer) As Integer
        Function DeleteWT(ByVal intWTID As Integer) As Integer
        Function GetWTByID(ByVal intWTID As Integer) As IWTEntity
        Function InsertWT(ByVal objWTEntity As IWTEntity) As Integer
        Function UpdateWT(ByVal objWTEntity As IWTEntity) As Integer
        Function CheckDupWT(ByVal intWTID As String, ByVal intWT As String) As Integer

        Function GetWTForList() As List(Of IWTEntity)

#End Region

    End Interface
End Namespace