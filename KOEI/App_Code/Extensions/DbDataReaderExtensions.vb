Imports System.Runtime.CompilerServices
Imports System.Data.Common
Imports System.Data

Namespace Extensions
    Public Module DbDataReaderExtensions

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetString(ByVal dataReader As DbDataReader, ByVal columnName As String) As String
            Return If(IsDBNull(dataReader(columnName)), Nothing, Convert.ToString(dataReader(columnName)))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetInt32(ByVal dataReader As DbDataReader, ByVal columnName As String) As Int32
            Dim int32Value As Int32
            Dim valid As Boolean = Int32.TryParse(Convert.ToString(dataReader(columnName)), int32Value)

            Return If(valid, int32Value, 0)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableInt32(ByVal dataReader As DbDataReader, ByVal columnName As String) As Int32?
            Return If(Not IsDBNull(dataReader(columnName)), Int32.Parse(Convert.ToString(dataReader(columnName))), Nothing)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetInt64(ByVal dataReader As DbDataReader, ByVal columnName As String) As Int64
            Dim int64Value As Int64
            Dim valid As Boolean = Int64.TryParse(Convert.ToString(dataReader(columnName)), int64Value)

            Return If(valid, int64Value, 0)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableInt64(ByVal dataReader As DbDataReader, ByVal columnName As String) As Int64?
            Return If(Not IsDBNull(dataReader(columnName)), Int64.Parse(Convert.ToString(dataReader(columnName))), Nothing)
        End Function

        <Extension()> _
        Public Function GetByte(ByVal dataReader As DbDataReader, ByVal columnName As String) As Byte
            Dim byteValue As Byte
            Dim valid As Boolean = Byte.TryParse(Convert.ToString(dataReader(columnName)), byteValue)

            Return byteValue
        End Function

        <Extension()> _
        Public Function GetNullableByte(ByVal dataReader As DbDataReader, ByVal columnName As String) As Byte?
            Dim byteValue As Byte
            Dim valid As Boolean = Not (IsDBNull(dataReader(columnName))) AndAlso Byte.TryParse(Convert.ToString(dataReader(columnName)), byteValue)

            Return If(valid, byteValue, Nothing)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetBoolean(ByVal dataReader As DbDataReader, ByVal columnName As String) As Boolean
            Dim boolValue As Boolean
            Dim valid = Boolean.TryParse(Convert.ToString(dataReader(columnName)), boolValue)

            If (IsDBNull(dataReader(columnName))) Then
                Return False
            End If

            Return Convert.ToBoolean(dataReader(columnName))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetDateTime(ByVal dataReader As DbDataReader, ByVal columnName As String) As DateTime
            Dim datetimeValue As DateTime
            Dim valid As Boolean = DateTime.TryParse(Convert.ToString(dataReader(columnName)), datetimeValue)

            Return datetimeValue
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="dataReader"></param>
        ''' <param name="columnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> _
        Public Function GetNullableDateTime(ByVal dataReader As DbDataReader, ByVal columnName As String) As DateTime?
            Dim datetimeValue As DateTime
            Dim valid As Boolean = Not (IsDBNull(dataReader(columnName))) AndAlso DateTime.TryParse(Convert.ToString(dataReader(columnName)), datetimeValue)

            Return If(valid, datetimeValue, Nothing)
        End Function

    End Module
End Namespace
