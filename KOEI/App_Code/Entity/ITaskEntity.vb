Imports Microsoft.VisualBasic

Namespace Entity
    Public Interface ITaskEntity
        'id
        Property id() As Integer
        'schedule
        Property schedule() As String
        'task
        Property task() As String
        'note
        Property note() As String
        'refpage
        Property refpage() As String
        'tskpage
        Property tskpage() As String
        'user_id
        Property user_id() As Integer
        'refkey
        Property refkey() As Integer

        Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer) As List(Of Entity.ITaskEntity)
        Function GetListTaskOfDDL(ByVal intUserId As Integer) As List(Of ITaskEntity)
        Function CheckTaskProcess(ByVal intUserId As Integer) As Integer

    End Interface
End Namespace

