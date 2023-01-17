Imports System.Runtime.CompilerServices

Namespace Extensions
    Public Module EnumerationExtensions

        <Extension()> _
        Public Function ConvertTo(Of T)(ByVal enumType As System.Enum) As T
            Dim value As Object = Nothing
            Dim type As System.Type = GetType(T)

            If (type Is GetType(Int32)) Then
                value = Convert.ToInt32(enumType)
            ElseIf (type Is GetType(Long)) Then
                value = Convert.ToInt64(enumType)
            ElseIf (type Is GetType(Short)) Then
                value = Convert.ToInt16(enumType)
            Else
                value = Convert.ToString(enumType)
            End If

            Return CType(value, T)
        End Function

    End Module
End Namespace