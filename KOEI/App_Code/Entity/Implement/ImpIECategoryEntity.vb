#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpIECategoryEntity
'	Class Discription	: Class of table mst_ie_category
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

Imports Dao
Imports Interfaces
Imports Microsoft.VisualBasic

#End Region


Namespace Entity

    Public Class ImpIECategoryEntity
        Implements IIECategoryEntity

#Region "Constructors"
        Public Sub New()
            _categoryDao = New ImpIECategoryDao()
        End Sub
#End Region

#Region "Fields"

        Private _id As Byte
        Private _name As String
        Private _name_jp As String
        Private _delete_fg As Byte
        Private _created_by As Int32?
        Private _created_date As String
        Private _updated_by As Int32?
        Private _updated_date As String

        Private Shared _categoryDao As IIECategoryDao

#End Region

#Region "Properties"

        Public Property ID() As Byte Implements IIECategoryEntity.ID
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
        Public Property Name() As String Implements IIECategoryEntity.Name
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)

                Me._name = value

            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property NameJp() As String Implements IIECategoryEntity.NameJp
            Get
                Return Me._name_jp
            End Get
            Set(ByVal value As String)
                Me._name_jp = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property DeleteFg() As Byte Implements IIECategoryEntity.DeleteFg
            Get
                Return Me._delete_fg
            End Get
            Set(ByVal value As Byte)
                Me._delete_fg = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>

        Public Property CreatedBy() As Int32? Implements IIECategoryEntity.CreatedBy
            Get
                Return Me._created_by
            End Get
            Set(ByVal value As Int32?)
                Me._created_by = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property CreatedDate() As String Implements IIECategoryEntity.CreatedDate
            Get
                Return Me._created_date
            End Get
            Set(ByVal value As String)
                Me._created_date = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property UpdatedBy() As Int32? Implements IIECategoryEntity.UpdatedBy
            Get
                Return Me._updated_by
            End Get
            Set(ByVal value As Int32?)
                Me._updated_by = value
            End Set
        End Property

        Public Property UpdatedDate() As String Implements IIECategoryEntity.UpdatedDate
            Get
                Return Me._updated_date
            End Get
            Set(ByVal value As String)
                Me._updated_date = value
            End Set
        End Property



#End Region

#Region "Functions"
        Public Function GetAll() As List(Of IIECategoryEntity) Implements IIECategoryEntity.GetAll
            Return _categoryDao.GetAll()
        End Function
#End Region

    End Class
End Namespace