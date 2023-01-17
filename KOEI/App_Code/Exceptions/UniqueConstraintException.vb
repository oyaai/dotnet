Namespace Exceptions

    ''' <summary>
    ''' Handles when insert/update the duplicate value
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UniqueConstraintException
        Inherits ApplicationException

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
