Imports Microsoft.VisualBasic

Namespace Entity
    Public Class ImpTaskEntity
        Implements ITaskEntity

        'id
        Private _id As Integer
        'schedule
        Private _schedule As String
        'task
        Private _task As String
        'note
        Private _note As String
        'refpage
        Private _refpage As String
        'tskpage
        Private _tskpage As String
        'user_id
        Private _user_id As Integer
        'refkey
        Private _refkey As Integer

        Private _objTaskDao As New Dao.ImpTaskDao

        Public Function GetListTaskOfDDL(ByVal intUserId As Integer) As System.Collections.Generic.List(Of ITaskEntity) Implements ITaskEntity.GetListTaskOfDDL
            Return _objTaskDao.GetListTaskOfDDL(intUserId)
        End Function

        Public Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer) As System.Collections.Generic.List(Of ITaskEntity) Implements ITaskEntity.GetTaskSearch
            Return _objTaskDao.GetTaskSearch(strTask, intUserId)
        End Function

        Public Function CheckTaskProcess(ByVal intUserId As Integer) As Integer Implements ITaskEntity.CheckTaskProcess
            Return _objTaskDao.CheckTaskProcess(intUserId)
        End Function

#Region "Property"
        Public Property id() As Integer Implements ITaskEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property note() As String Implements ITaskEntity.note
            Get
                Return _note
            End Get
            Set(ByVal value As String)
                _note = value
            End Set
        End Property

        Public Property refkey() As Integer Implements ITaskEntity.refkey
            Get
                Return _refkey
            End Get
            Set(ByVal value As Integer)
                _refkey = value
            End Set
        End Property

        Public Property refpage() As String Implements ITaskEntity.refpage
            Get
                Return _refpage
            End Get
            Set(ByVal value As String)
                _refpage = value
            End Set
        End Property

        Public Property schedule() As String Implements ITaskEntity.schedule
            Get
                Return _schedule
            End Get
            Set(ByVal value As String)
                _schedule = value
            End Set
        End Property

        Public Property task() As String Implements ITaskEntity.task
            Get
                Return _task
            End Get
            Set(ByVal value As String)
                _task = value
            End Set
        End Property

        Public Property tskpage() As String Implements ITaskEntity.tskpage
            Get
                Return _tskpage
            End Get
            Set(ByVal value As String)
                _tskpage = value
            End Set
        End Property

        Public Property user_id() As Integer Implements ITaskEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property
#End Region

        
    End Class
End Namespace

