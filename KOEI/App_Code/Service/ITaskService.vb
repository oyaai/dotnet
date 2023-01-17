Imports Microsoft.VisualBasic

Namespace Service
    Public Interface ITaskService
        Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer, ByRef objDT As System.Data.DataTable) As List(Of Dto.TaskDto)
        Function GetListTaskOfDDL(ByVal intUserId As Integer) As List(Of Dto.TaskDto)
        Function CheckTaskProcess(ByVal intUserId As Integer) As Integer
    End Interface
End Namespace

