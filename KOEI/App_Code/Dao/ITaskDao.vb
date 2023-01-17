Imports Microsoft.VisualBasic
Imports Entity

Namespace Dao
    Public Interface ITaskDao
        Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer) As List(Of Entity.ITaskEntity)
        Function GetListTaskOfDDL(ByVal intUserId As Integer) As List(Of ITaskEntity)
        Function CheckTaskProcess(ByVal intUsedId As Integer) As Integer
    End Interface
End Namespace

