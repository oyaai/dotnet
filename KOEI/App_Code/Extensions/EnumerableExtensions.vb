Imports System.Reflection
Imports System.Data
Imports System.Runtime.CompilerServices

Namespace Extensions
    Public Module EnumerableExtensions

        <Extension()> _
        Public Function ToDataSet(Of T)(ByVal list As List(Of T)) As DataSet
            Dim resultDataSet As New DataSet()
            Dim resultDataTable As New DataTable("results")
            Dim resultDataRow As DataRow
            Dim itemProperties() As PropertyInfo
            '    
            ' Meta Data. 
            '
            itemProperties = list.Item(0).GetType().GetProperties()
            For Each p As PropertyInfo In itemProperties
                resultDataTable.Columns.Add(p.Name, If(Not IsNothing(Nullable.GetUnderlyingType(p.PropertyType)), Nullable.GetUnderlyingType(p.PropertyType), p.GetGetMethod.ReturnType()))
            Next
            '
            ' Data
            '
            For Each item As T In list
                '
                ' Get the data from this item into a DataRow
                ' then add the DataRow to the DataTable.
                ' Eeach items property becomes a colunm.
                '
                itemProperties = item.GetType().GetProperties()
                resultDataRow = resultDataTable.NewRow()
                For Each p As PropertyInfo In itemProperties
                    resultDataRow(p.Name) = If(IsNothing(p.GetValue(item, Nothing)), DBNull.Value, p.GetValue(item, Nothing))
                Next
                resultDataTable.Rows.Add(resultDataRow)
            Next
            '
            ' Add the DataTable to the DataSet, We are DONE!
            '
            resultDataSet.Tables.Add(resultDataTable)
            Return resultDataSet
        End Function

    End Module
End Namespace