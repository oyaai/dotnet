Imports Microsoft.VisualBasic

Namespace Dto
    Public Class TaskDto
        'id
        Private _id As Integer
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        'schedule
        Private _schedule As String
        Public Property schedule() As String
            Get
                Return _schedule
            End Get
            Set(ByVal value As String)
                _schedule = value
            End Set
        End Property

        'task
        Private _task As String
        Public Property task() As String
            Get
                Return _task
            End Get
            Set(ByVal value As String)
                _task = value
            End Set
        End Property

        'note
        Private _note As String
        Public Property note() As String
            Get
                Return _note
            End Get
            Set(ByVal value As String)
                _note = value
            End Set
        End Property

        'refpage
        Private _refpage As String
        Public Property refpage() As String
            Get
                Return _refpage
            End Get
            Set(ByVal value As String)
                _refpage = value
            End Set
        End Property

        'tskpage
        Private _tskpage As String
        Public Property tskpage() As String
            Get
                Return _tskpage
            End Get
            Set(ByVal value As String)
                _tskpage = value
            End Set
        End Property

        'user_id
        Private _user_id As Integer
        Public Property user_id() As Integer
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        'refkey
        Private _refkey As Integer
        Public Property refkey() As Integer
            Get
                Return _refkey
            End Get
            Set(ByVal value As Integer)
                _refkey = value
            End Set
        End Property

    End Class
End Namespace

