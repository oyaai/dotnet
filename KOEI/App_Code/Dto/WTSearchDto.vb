Imports Microsoft.VisualBasic

Namespace Dto

    <Serializable()> _
    Public Class WTSearchDto

#Region "Fields"

        Private _id As Byte?
        Private _percent As Byte?
        Private _user As UserDto

#End Region

#Region "Properties"

        Public Property ID() As Byte?
            Get
                Return _id
            End Get
            Set(ByVal value As Byte?)
                _id = value
            End Set
        End Property

        Public Property Percent() As Byte?
            Get
                Return _percent
            End Get
            Set(ByVal value As Byte?)
                _percent = value
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