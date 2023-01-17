Imports System.Data
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic

Namespace Extensions

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Module DataRowExtensions

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetString(ByVal dataRow As DataRow, ByVal columnName As String) As String
            Return If(IsDBNull(dataRow(columnName)), Nothing, Convert.ToString(dataRow(columnName)))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetInt32(ByVal dataRow As DataRow, ByVal columnName As String) As Int32
            Dim int32Value As Int32
            Dim valid As Boolean = Int32.TryParse(Convert.ToString(dataRow(columnName)), int32Value)

            Return If(valid, int32Value, 0)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableInt32(ByVal dataRow As DataRow, ByVal columnName As String) As Int32?
            Return If(Not IsDBNull(dataRow(columnName)), Int32.Parse(Convert.ToString(dataRow(columnName))), Nothing)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetInt64(ByVal dataRow As DataRow, ByVal columnName As String) As Int64
            Dim int64Value As Int64
            Dim valid As Boolean = Int64.TryParse(Convert.ToString(dataRow(columnName)), int64Value)

            Return If(valid, int64Value, 0)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableInt64(ByVal dataRow As DataRow, ByVal columnName As String) As Int64?
            Return If(Not IsDBNull(dataRow(columnName)), Int64.Parse(Convert.ToString(dataRow(columnName))), Nothing)
        End Function

        <Extension()> _
        Public Function GetByte(ByVal dataRow As DataRow, ByVal columnName As String) As Byte
            Dim byteValue As Byte
            Dim valid As Boolean = Byte.TryParse(Convert.ToString(dataRow(columnName)), byteValue)

            Return byteValue
        End Function

        <Extension()> _
        Public Function GetNullableByte(ByVal dataRow As DataRow, ByVal columnName As String) As Byte?
            Dim byteValue As Byte
            Dim valid As Boolean = Not (IsDBNull(dataRow(columnName))) AndAlso Byte.TryParse(Convert.ToString(dataRow(columnName)), byteValue)

            Return If(valid, byteValue, Nothing)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetBoolean(ByVal dataRow As DataRow, ByVal columnName As String) As Boolean
            Dim boolValue As Boolean
            Dim valid = Boolean.TryParse(Convert.ToString(dataRow(columnName)), boolValue)

            Return Convert.ToBoolean(dataRow(columnName))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetDateTime(ByVal dataRow As DataRow, ByVal columnName As String) As DateTime
            Dim datetimeValue As DateTime
            Dim valid As Boolean = DateTime.TryParse(Convert.ToString(dataRow(columnName)), datetimeValue)

            Return datetimeValue
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataRow"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableDateTime(ByVal dataRow As DataRow, ByVal columnName As String) As DateTime?
            Dim datetimeValue As DateTime
            Dim valid As Boolean = Not (IsDBNull(dataRow(columnName))) AndAlso DateTime.TryParse(Convert.ToString(dataRow(columnName)), datetimeValue)

            Return If(valid, datetimeValue, Nothing)
        End Function

    End Module
End Namespace

