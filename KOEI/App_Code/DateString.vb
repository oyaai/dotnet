Public Class DateString
    Private ReadOnly _dateString As String

    Public Sub New(ByVal dateString As String)
        _dateString = dateString
    End Sub

    Public Shared Widening Operator CType(ByVal dateString As String) As DateString
        Return New DateString(dateString)
    End Operator

    Public Overrides Function ToString() As String
        Return If(String.IsNullOrEmpty(_dateString), String.Empty, _dateString)
    End Function

End Class
