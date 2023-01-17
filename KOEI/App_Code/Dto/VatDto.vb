

Namespace Dto

    <Serializable()> _
    Public Class VatDto

#Region "Fields"
        Private _id As Byte?
        Private _percent As Byte?
        Private _deleteFlag As Byte
        Private _createdBy As Long?
        Private _createdDate As String
        Private _updatedBy As Long?
        Private _updatedDate As String

        Private _isInUsed As Boolean
        Private _user As UserDto

        Private _percent_string As String
#End Region

#Region "Properties"
        Public Property CreatedBy() As Int64?
            Get
                Return Me._createdBy
            End Get
            Set(ByVal value As Int64?)
                Me._createdBy = value
            End Set
        End Property

        Public Property CreatedDate() As String
            Get
                Return Me._createdDate
            End Get
            Set(ByVal value As String)
                Me._createdDate = value
            End Set
        End Property

        Public Property DeleteFlag() As Byte
            Get
                Return Me._deleteFlag
            End Get
            Set(ByVal value As Byte)
                Me._deleteFlag = value
            End Set
        End Property

        Public Property ID() As Byte?
            Get
                Return Me._id
            End Get
            Set(ByVal value As Byte?)
                Me._id = value
            End Set
        End Property

        Public Property Percent() As Byte?
            Get
                Return Me._percent
            End Get
            Set(ByVal value As Byte?)
                Me._percent = value
            End Set
        End Property

        Public Property UpdatedBy() As Int64?
            Get
                Return Me._updatedBy
            End Get
            Set(ByVal value As Int64?)
                Me._updatedBy = value
            End Set
        End Property

        Public Property UpdatedDate() As String
            Get
                Return Me._updatedDate
            End Get
            Set(ByVal value As String)
                Me._updatedDate = value
            End Set
        End Property

        Public Property IsInUsed() As Boolean
            Get
                Return _isInUsed
            End Get
            Friend Set(ByVal value As Boolean)
                _isInUsed = value
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

        Public Property PercentString() As String
            Get
                Return _percent_string
            End Get
            Set(ByVal value As String)
                _percent_string = value
            End Set
        End Property
#End Region

    End Class
End Namespace