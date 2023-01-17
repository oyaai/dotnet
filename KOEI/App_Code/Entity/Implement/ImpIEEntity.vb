#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpIEEntity
'	Class Discription	: Class of table mst_ie
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
Imports Dto
Imports Interfaces
Imports Microsoft.VisualBasic

#End Region


Namespace Entity

    Public Class ImpIEEntity
        Implements IMst_IEEntity

        Private objIE As New Dao.ImpIEDao

#Region "Constructors"

        Public Sub New()
            _category = New ImpIECategoryEntity()
            _ieDao = New ImpIEDao()
        End Sub

#End Region

#Region "Fields"

        Private _id As Integer
        Private _category_id As Byte
        Private _code As String
        Private _name As String
        Private _delete_fg As Byte
        Private _created_by As Int32?
        Private _created_date As String
        Private _updated_by As Int32?
        Private _updated_date As String

        Private _isInUsed As Boolean
        Private _category As ImpIECategoryEntity

        Private Shared _ieDao As IIEDao

#End Region

#Region "Properties"

        Public Property ID() As Integer Implements IMst_IEEntity.ID
            Get
                Return Me._id
            End Get
            Set(ByVal value As Integer)
                Me._id = value
            End Set
        End Property


        '''<summary>
        '''
        '''</summary>
        Public Property CategoryID() As Byte Implements IMst_IEEntity.CategoryID
            Get
                Return Me._category_id
            End Get
            Set(ByVal value As Byte)

                Me._category_id = value

            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property Code() As String Implements IMst_IEEntity.Code
            Get
                Return Me._code
            End Get
            Set(ByVal value As String)
                Me._code = value
            End Set
        End Property

        '''<summary>
        '''
        '''</summary>
        Public Property Name() As String Implements IMst_IEEntity.Name
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
        Public Property DeleteFg() As Byte Implements IMst_IEEntity.DeleteFg
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

        Public Property CreatedBy() As Int32? Implements IMst_IEEntity.CreatedBy
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
        Public Property CreatedDate() As String Implements IMst_IEEntity.CreatedDate
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
        Public Property UpdatedBy() As Int32? Implements IMst_IEEntity.UpdatedBy
            Get
                Return Me._updated_by
            End Get
            Set(ByVal value As Int32?)
                Me._updated_by = value
            End Set
        End Property

        Public Property UpdatedDate() As String Implements IMst_IEEntity.UpdatedDate
            Get
                Return Me._updated_date
            End Get
            Set(ByVal value As String)
                Me._updated_date = value
            End Set
        End Property

        Public Property Category() As ImpIECategoryEntity Implements IMst_IEEntity.Category
            Get
                Return _category
            End Get
            Set(ByVal value As ImpIECategoryEntity)
                _category = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsInUsed() As Boolean Implements IMst_IEEntity.IsInUsed
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
            End Set
        End Property

#End Region

#Region "Functions"


        Public Function GetIEList(ByVal strID As String, ByVal strIECategory As String, ByVal strIECode As String, ByVal strIEName As String) As System.Collections.Generic.List(Of ImpMst_IEDetailEntity) Implements IMst_IEEntity.GetIEList
            Return objIE.GetIEList(strID, strIECategory, strIECode, strIEName)
        End Function

        Public Function GetIEByID(ByVal intIEID As Integer) As IMst_IEEntity Implements IMst_IEEntity.GetIEByID
            Return objIE.GetIEByID(intIEID)
        End Function

        Public Function CountUsedInPO(ByVal intIEID As Integer) As Integer Implements IMst_IEEntity.CountUsedInPO
            Return objIE.CountUsedInPO(intIEID)
        End Function

        Public Function CountUsedInPO2(ByVal intIEID As Integer) As Integer Implements IMst_IEEntity.CountUsedInPO2
            Return objIE.CountUsedInPO2(intIEID)
        End Function

        Public Function DeleteIE(ByVal intIEID As Integer) As Integer Implements IMst_IEEntity.DeleteIE
            Return objIE.DeleteIE(intIEID)
        End Function

        Public Function InsertIE(ByVal objIEEnt As IMst_IEEntity) As Integer Implements IMst_IEEntity.InsertIE
            Return objIE.InsertIE(objIEEnt)
        End Function

        Public Function UpdateIE(ByVal objIEEnt As IMst_IEEntity) As Integer Implements IMst_IEEntity.UpdateIE
            Return objIE.UpdateIE(objIEEnt)
        End Function

        Public Function GetIEForList(Optional ByVal showCode As Boolean = False) As System.Collections.Generic.List(Of ImpIEEntity) Implements Interfaces.IMst_IEEntity.GetIEForList
            Dim objIEDao As New Dao.ImpIEDao
            Return objIEDao.DB_GetIEForList(showCode)
        End Function

        Public Function CheckDupIE(ByVal intIEID As String, ByVal strIECode As String, ByVal strIECategory As String) As Integer Implements IMst_IEEntity.CheckDupIE
            Return objIE.CheckDupIE(intIEID, strIECode, strIECategory)
        End Function

        Public Function GetListAccountTitleToDDL(ByVal intCategoryType As Integer) As System.Collections.Generic.List(Of ImpIEEntity) Implements Interfaces.IMst_IEEntity.GetListAccountTitleToDDL
            Return objIE.GetListAccountTitleToDDL(intCategoryType)
        End Function

        Public Function GetAccountTitleForList() As System.Collections.Generic.List(Of ImpIEEntity) Implements Interfaces.IMst_IEEntity.GetAccountTitleForList
            Return objIE.GetAccountTitleForList()
        End Function
#End Region

    End Class
End Namespace