Namespace Exceptions
    Public Class ContentIsInUsedException
        Inherits Exception

#Region "Fields"
        Private Shared _message As String = "This content is in used"
#End Region

#Region "Constructors"

        Public Sub New()
            Me.New(_message)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal innerException As Exception)
            Me.New(_message, innerException)
        End Sub


        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

#End Region

    End Class
End Namespace