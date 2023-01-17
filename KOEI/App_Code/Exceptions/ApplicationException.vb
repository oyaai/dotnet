Namespace Exceptions
    Public Class ApplicationException
        Inherits Exception

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal innerException As Exception)
            MyBase.New(String.Empty, innerException)
        End Sub


        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

#End Region

    End Class
End Namespace