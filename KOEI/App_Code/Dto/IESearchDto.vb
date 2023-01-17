Imports Microsoft.VisualBasic

Namespace Dto

    <Serializable()> _
    Public Class IESearchDto

#Region "Fields"

        Private _id As Int32?
        Private _category_id As Byte?
        Private _code As String
        Private _name As String

        Private _user As UserDto

#End Region

#Region "Properties"

        Public Property ID() As Int32?
            Get
                Return Me._id
            End Get
            Set(ByVal value As Int32?)
                Me._id = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property created_date in the schema.
        '''</summary>
        Public Property CategoryID() As Byte?
            Get
                Return Me._category_id
            End Get
            Set(ByVal value As Byte?)
                Me._category_id = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property delete_fg in the schema.
        '''</summary>
        Public Property Code() As String
            Get
                Return Me._code
            End Get
            Set(ByVal value As String)
                Me._code = value
            End Set
        End Property

        '''<summary>
        '''There are no comments for Property id in the schema.
        '''</summary>
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
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