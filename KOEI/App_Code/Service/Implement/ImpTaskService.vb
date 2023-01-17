Imports Microsoft.VisualBasic
Imports Entity

Namespace Service
    Public Class ImpTaskService
        Implements ITaskService

        Private objLog As New Common.Logs.Log
        Private objTaskEntity As ImpTaskEntity

        '/**************************************************************
        '	Function name	: GetListTaskOfDDL
        '	Discription	    : Get list task for ddl
        '	Return Value	: List of Dto.TaskDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetListTaskOfDDL(ByVal intUserId As Integer) As System.Collections.Generic.List(Of Dto.TaskDto) Implements ITaskService.GetListTaskOfDDL
            Try
                objTaskEntity = New ImpTaskEntity
                GetListTaskOfDDL = Nothing

                If intUserId = 0 Then Exit Function

                GetListTaskOfDDL = ChangeTaskEntToDto(objTaskEntity.GetListTaskOfDDL(intUserId))

            Catch ex As Exception
                ' Write error log
                GetListTaskOfDDL = Nothing
                objLog.ErrorLog("GetListTaskOfDDL", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetTaskSearch
        '	Discription	    : Get task search
        '	Return Value	: List of Dto.TaskDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetTaskSearch(ByVal strTask As String, ByVal intUserId As Integer, ByRef objDT As System.Data.DataTable) As System.Collections.Generic.List(Of Dto.TaskDto) Implements ITaskService.GetTaskSearch
            Try
                Dim objTaskListEnt As New List(Of Entity.ITaskEntity)
                objTaskEntity = New ImpTaskEntity
                GetTaskSearch = Nothing

                If intUserId = 0 Then Exit Function

                objTaskListEnt = objTaskEntity.GetTaskSearch(strTask, intUserId)
                If objTaskListEnt Is Nothing Then Exit Function
                If objTaskListEnt.Count = 0 Then Exit Function
                objDT = SetTaskListToDT(objTaskListEnt)
                GetTaskSearch = ChangeTaskEntToDto(objTaskListEnt)

            Catch ex As Exception
                ' Write error log
                GetTaskSearch = Nothing
                objLog.ErrorLog("GetTaskSearch", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangeTaskEntToDto
        '	Discription	    : Chenge data task to Dto
        '	Return Value	: Dto.TaskDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangeTaskEntToDto(ByVal objTaskEnt As List(Of Entity.ITaskEntity)) As List(Of Dto.TaskDto)
            Try
                Dim objCom As New Common.Utilities.Utility
                Dim objTaskDto As New Dto.TaskDto

                ChangeTaskEntToDto = Nothing
                If (Not objTaskEnt Is Nothing) AndAlso objTaskEnt.Count > 0 Then
                    ChangeTaskEntToDto = New List(Of Dto.TaskDto)
                    For Each objDetail In objTaskEnt
                        objTaskDto = New Dto.TaskDto
                        With objTaskDto
                            'Property id() As Integer
                            .id = objDetail.id
                            'Property schedule() As String
                            If (Not objDetail.schedule Is Nothing) Then
                                .schedule = objDetail.schedule
                            End If
                            'Property task() As String
                            If (Not objDetail.task Is Nothing) Then
                                .task = objDetail.task
                            End If
                            'Property note() As String
                            If (Not objDetail.task Is Nothing) Then
                                .note = objDetail.note
                            End If
                            'Property refpage() As String
                            If (Not objDetail.task Is Nothing) Then
                                .refpage = objDetail.refpage
                            End If
                            'Property tskpage() As String
                            If (Not objDetail.tskpage Is Nothing) Then
                                .tskpage = objDetail.tskpage.Replace("@id", .id)
                            End If
                            'Property user_id() As Integer
                            .user_id = objDetail.user_id
                            'Property refkey() As Integer
                            .refkey = objDetail.refkey
                        End With
                        ChangeTaskEntToDto.Add(objTaskDto)
                    Next
                End If

            Catch ex As Exception
                ' Write error log
                ChangeTaskEntToDto = Nothing
                objLog.ErrorLog("ChangeTaskEntToDto", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetTaskListToDT
        '	Discription	    : Set data task to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetTaskListToDT(ByVal objValue As List(Of Entity.ITaskEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow
                Dim objCom As New Common.Utilities.Utility

                SetTaskListToDT = Nothing

                With objDT.Columns
                    .Add("id")
                    .Add("schedule")
                    .Add("task")
                    .Add("note")
                    .Add("refpage")
                    .Add("tskpage")
                    .Add("user_id")
                    .Add("refkey")
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        objDR("id") = .id
                        objDR("schedule") = .schedule
                        objDR("task") = .task
                        objDR("note") = .note
                        objDR("refpage") = .refpage
                        objDR("tskpage") = .tskpage.Replace("@id", .id)
                        objDR("user_id") = .user_id
                        objDR("refkey") = .refkey
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetTaskListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetTaskListToDT = Nothing
                objLog.ErrorLog("SetTaskListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckTaskProcess
        '	Discription	    : Check task process by user_id
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 13-08-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckTaskProcess(ByVal intUserId As Integer) As Integer Implements ITaskService.CheckTaskProcess
            Try
                objTaskEntity = New ImpTaskEntity
                CheckTaskProcess = 0

                If intUserId = 0 Then Exit Function
                CheckTaskProcess = objTaskEntity.CheckTaskProcess(intUserId)

            Catch ex As Exception
                ' Write error log
                CheckTaskProcess = Nothing
                objLog.ErrorLog("SetTaskListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class

End Namespace

