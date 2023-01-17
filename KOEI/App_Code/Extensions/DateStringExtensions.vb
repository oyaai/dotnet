Imports System.Runtime.CompilerServices
Imports System.Globalization

Namespace Extensions
    Public Module DateStringExtensions

        <Extension()> _
        Public Function Format(ByVal dateString As DateString, ByVal provider As CultureInfo, ByVal dateFormat As String) As String
            Dim newDate As Date
            Dim year, month, day As Int32

            year = Convert.ToInt32(dateString.ToString().Substring(0, 4))
            month = Convert.ToInt32(dateString.ToString().Substring(4, 2))
            day = Convert.ToInt32(dateString.ToString().Substring(6, 2))

            newDate = New Date(year, month, day)

            Return newDate.ToString(dateFormat)
        End Function

        <Extension()> _
        Public Function ToDate(ByVal dateString As DateString, ByVal provider As CultureInfo) As Date
            Dim formatted As String = dateString.Format(provider, provider.DateTimeFormat.ShortDatePattern)

            Return Convert.ToDateTime(formatted, provider)
        End Function

        <Extension()> _
        Public Function ToDate(ByVal dateString As DateString) As Date
            Return dateString.ToDate(Threading.Thread.CurrentThread.CurrentCulture)
        End Function
    End Module
End Namespace