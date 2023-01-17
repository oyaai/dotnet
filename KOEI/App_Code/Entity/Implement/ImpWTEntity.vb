#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpWTEntity
'	Class Discription	: Class of table mst_wt
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
Imports Dao
Imports Dto
Imports System.Linq.Expressions
Imports Interfaces
Imports Microsoft.VisualBasic
#End Region

Namespace Entity

    Public Class ImpWTEntity
        Implements IWTEntity

#Region "Construcctors"

        Public Sub New()
            _wtDao = New ImpWTDao()
        End Sub

#End Region

#Region "Fields"

        Private _createdBy As Long
        Private _createdDate As String
        Private _deleteFlag As Byte
        Private _id As Byte
        Private _percent As String
        Private _type As String
        Private _updatedBy As Long?
        Private _updatedDate As String

        Private _isInUsed As Boolean

        Private Shared _wtDao As IWTDao

        Private objWT As New Dao.ImpWTDao

#End Region

#Region "Properties"

        Public Property CreatedBy() As Long Implements IWTEntity.CreatedBy
            Get
                Return Me._createdBy
            End Get
            Set(ByVal value As Long)
                Me._createdBy = value
            End Set
        End Property


        '''<summary>
        '''
        '''</summary>
        Public Property CreatedDate() As String Implements IWTEntity.CreatedDate
            Get
                Return Me._createdDate
            End Get
            Set(ByVal value As String)

                Me._createdDate = value

            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property DeleteFlag() As Byte Implements IWTEntity.DeleteFlag
            Get
                Return Me._deleteFlag
            End Get
            Set(ByVal value As Byte)
                Me._deleteFlag = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property ID() As Byte Implements IWTEntity.ID
            Get
                Return Me._id
            End Get
            Set(ByVal value As Byte)
                Me._id = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property Percent() As String Implements IWTEntity.Percent
            Get
                Return Me._percent
            End Get
            Set(ByVal value As String)
                Me._percent = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>

        Public Property Type() As String Implements IWTEntity.Type
            Get
                Return Me._type
            End Get
            Set(ByVal value As String)
                Me._type = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property UpdatedBy() As Long? Implements IWTEntity.UpdatedBy
            Get
                Return Me._updatedBy
            End Get
            Set(ByVal value As Long?)
                Me._updatedBy = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property updated_date in the schema.
        '''</summary>
        Public Property UpdatedDate() As String Implements IWTEntity.UpdatedDate
            Get
                Return Me._updatedDate
            End Get
            Set(ByVal value As String)
                Me._updatedDate = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsInUsed() As Boolean Implements IWTEntity.IsInUsed
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
            End Set
        End Property


#End Region

#Region "Functions"

        Public Function GetWTList(ByVal strID As String, ByVal strWT As String) As System.Collections.Generic.List(Of IWTEntity) Implements IWTEntity.GetWTList
            Return objWT.GetWTList(strID, strWT)
        End Function

        Public Function CountUsedInPO(ByVal intWTID As Integer) As Integer Implements IWTEntity.CountUsedInPO
            Return objWT.CountUsedInPO(intWTID)
        End Function

        Public Function DeleteWT(ByVal intWTID As Integer) As Integer Implements IWTEntity.DeleteWT
            Return objWT.DeleteWT(intWTID)
        End Function

        Public Function GetWTByID(ByVal intWTID As Integer) As IWTEntity Implements IWTEntity.GetWTByID
            Return objWT.GetWTByID(intWTID)
        End Function

        Public Function InsertWT(ByVal objWTEntity As IWTEntity) As Integer Implements IWTEntity.InsertWT
            Return objWT.InsertWT(objWTEntity)
        End Function

        Public Function UpdateWT(ByVal objWTEntity As IWTEntity) As Integer Implements IWTEntity.UpdateWT
            Return objWT.UpdateWT(objWTEntity)
        End Function

        Public Function CheckDupWT(ByVal intWTID As String, ByVal intWT As String) As Integer Implements IWTEntity.CheckDupWT
            Return objWT.CheckDupWT(intWTID, intWT)
        End Function

        Public Overridable Function GetWTForList() As List(Of IWTEntity) Implements IWTEntity.GetWTForList
            Dim objWTDao As New Dao.ImpWTDao
            Return objWTDao.GetWTForList
        End Function


#End Region


    End Class
End Namespace