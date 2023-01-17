#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : IECategoryDto
'	Class Discription	: Dto class IE
'	Create User 		: Nisa S.
'	Create Date		    : 24-06-2013
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

Imports Microsoft.VisualBasic

#End Region

Namespace Dto

    <Serializable()> _
    Public Class IECategoryDto

#Region "Fields"

        Private _id As Byte
        Private _name As String
        Private _name_jp As String
        Private _delete_flag As Byte
        Private _created_by As Int32?
        Private _created_date As String
        Private _updated_by As Int32?
        Private _updated_date As String

        Private _user As UserDto

#End Region

#Region "Properties"

        Public Property ID() As Byte
            Get
                Return Me._id
            End Get
            Set(ByVal value As Byte)
                Me._id = value
            End Set
        End Property


        '''<summary>
        '''There are no comments for Property created_date in the schema.
        '''</summary>
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)

                Me._name = value

            End Set
        End Property

        '''<summary>
        '''There are no comments for Property delete_fg in the schema.
        '''</summary>
        Public Property NameJp() As String
            Get
                Return Me._name_jp
            End Get
            Set(ByVal value As String)
                Me._name_jp = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property id in the schema.
        '''</summary>
        Public Property DeleteFlag() As Byte
            Get
                Return Me._delete_flag
            End Get
            Set(ByVal value As Byte)
                Me._delete_flag = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property type in the schema.
        '''</summary>
        Public Property CreatedBy() As Int32?
            Get
                Return Me._created_by
            End Get
            Set(ByVal value As Int32?)
                Me._created_by = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property updated_by in the schema.
        '''</summary>
        Public Property CreatedDate() As String
            Get
                Return Me._created_date
            End Get
            Set(ByVal value As String)
                Me._created_date = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property updated_date in the schema.
        '''</summary>
        Public Property UpdatedBy() As Int32?
            Get
                Return Me._updated_by
            End Get
            Set(ByVal value As Int32?)
                Me._updated_by = value
            End Set
        End Property

        Public Property UpdatedDate() As String
            Get
                Return Me._updated_date
            End Get
            Set(ByVal value As String)
                Me._updated_date = value
            End Set
        End Property

        Public Property User() As UserDto
            Get
                Return _user
            End Get
            Set(ByVal value As UserDto)
                _user = value
            End Set
        End Property
#End Region

    End Class
End Namespace