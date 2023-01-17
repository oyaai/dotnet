#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpVatEntity
'	Class Discription	: Class of table mst_vat
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
Imports Dao
Imports Dto
Imports Interfaces
Imports Microsoft.VisualBasic
#End Region

Namespace Entity

    Public Class ImpVatEntity
        Implements IVatEntity

#Region "Constructors"

        Public Sub New()
            _vatDao = New ImpVatDao()
        End Sub

#End Region

#Region "Fields"
        Private _id As Byte
        Private _percent As Byte?
        Private _deleteFlag As Byte
        Private _createdBy As Long?
        Private _createdDate As String
        Private _updatedBy As Long?
        Private _updatedDate As String

        Private _isInUsed As Boolean

        Private Shared _vatDao As IVatDao
        Private objVat As New Dao.ImpVatDao
#End Region

#Region "Properties"

        Public Property CreatedBy() As Long? Implements IVatEntity.CreatedBy
            Get
                Return Me._createdBy
            End Get
            Set(ByVal value As Long?)
                Me._createdBy = value
            End Set
        End Property

        Public Property CreatedDate() As String Implements IVatEntity.CreatedDate
            Get
                Return Me._createdDate
            End Get
            Set(ByVal value As String)
                Me._createdDate = value
            End Set
        End Property

        Public Property DeleteFlag() As Byte Implements IVatEntity.DeleteFlag
            Get
                Return Me._deleteFlag
            End Get
            Set(ByVal value As Byte)
                Me._deleteFlag = value
            End Set
        End Property

        Public Property ID() As Byte Implements IVatEntity.ID
            Get
                Return Me._id
            End Get
            Set(ByVal value As Byte)
                Me._id = value
            End Set
        End Property

        Public Property Percent() As Byte? Implements IVatEntity.Percent
            Get
                Return Me._percent
            End Get
            Set(ByVal value As Byte?)
                Me._percent = value
            End Set
        End Property

        Public Property UpdatedBy() As Long? Implements IVatEntity.UpdatedBy
            Get
                Return Me._updatedBy
            End Get
            Set(ByVal value As Long?)
                Me._updatedBy = value
            End Set
        End Property

        Public Property UpdatedDate() As String Implements IVatEntity.UpdatedDate
            Get
                Return Me._updatedDate
            End Get
            Set(ByVal value As String)
                Me._updatedDate = value
            End Set
        End Property

        Public Property IsInUsed() As Boolean Implements IVatEntity.IsInUsed
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
            End Set
        End Property

#End Region

#Region "Functions"

        Public Function GetVatForList() As System.Collections.Generic.List(Of Interfaces.IVatEntity) Implements Interfaces.IVatEntity.GetVatForList
            Dim objVatDao As New Dao.ImpVatDao
            Return objVatDao.GetVatForList
        End Function

        Public Function GetVatList(ByVal strID As String, ByVal strPercent As String) As System.Collections.Generic.List(Of ImpVatDetailEntity) Implements IVatEntity.GetVatList
            Return objVat.GetVatList(strID, strPercent)
        End Function

        Public Function CountUsedInPO(ByVal intVatID As Integer) As Integer Implements IVatEntity.CountUsedInPO
            Return objVat.CountUsedInPO(intVatID)
        End Function

        Public Function DeleteVat(ByVal intVatID As Integer) As Integer Implements IVatEntity.DeleteVat
            Return objVat.DeleteVat(intVatID)
        End Function

        Public Function CheckDupVat(ByVal intVatID As String, ByVal intVat As String) As Integer Implements IVatEntity.CheckDupVat
            Return objVat.CheckDupVat(intVatID, intVat)
        End Function

        Public Function InsertVat(ByVal objVatEnt As IVatEntity) As Integer Implements IVatEntity.InsertVat
            Return objVat.InsertVat(objVatEnt)
        End Function

        Public Function UpdateVat(ByVal objVatEnt As IVatEntity) As Integer Implements IVatEntity.UpdateVat
            Return objVat.UpdateVat(objVatEnt)
        End Function

        Public Function GetVatByID(ByVal intVatID As Integer) As IVatEntity Implements IVatEntity.GetVatByID
            Return objVat.GetVatByID(intVatID)
        End Function

#End Region

    End Class
End Namespace