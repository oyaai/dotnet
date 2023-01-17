#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpScheculeRateService
'	Class Discription	: Class of Schecule Rate
'	Create User 		: Boonyarit
'	Create Date		    : 02-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports Dto
Imports Entity
Imports Extensions
#End Region

Namespace Service
    Public Class ImpScheculeRateService
        Implements IScheculeRateService

        Private objLog As New Common.Logs.Log

        '/**************************************************************
        '	Function name	: GetScheculeRateForSearch(Currency_id, o
        '	Update User	    :bjDT)
        '	Discription	    : Get Schecule Rate for search
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update Date	    :
        '*************************************************************/
        Public Function GetScheculeRateForSearch(ByVal intCurrency_id As Integer, ByVal strEFDate As String, ByVal intRanking As Integer, ByRef objDT As System.Data.DataTable) As Boolean Implements IScheculeRateService.GetScheculeRateForSearch
            Try
                ' variable
                Dim objSRate As New ImpMst_ScheduleRateEntity
                Dim objListSRate As New List(Of IMst_ScheduleRateEntity)
                Dim objCom As New Common.Utilities.Utility

                If strEFDate.Trim <> String.Empty Then
                    strEFDate = objCom.String2Date(strEFDate.Trim).ToString("yyyyMMdd")
                End If

                GetScheculeRateForSearch = False
                ' get data for search
                objListSRate = objSRate.GetScheduleRateByCurrency(intCurrency_id, strEFDate, intRanking)
                ' check data result
                If objListSRate Is Nothing Then Exit Function
                If objListSRate.Count > 0 Then
                    objDT = SetSRateListToDT(objListSRate)
                    GetScheculeRateForSearch = True
                End If

            Catch ex As Exception
                ' Write error log
                GetScheculeRateForSearch = False
                objLog.ErrorLog("GetScheculeRateForSearch", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetSRateListToDT
        '	Discription	    : Set data schecule rate to datatable
        '	Return Value	: DataTable
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetSRateListToDT(ByVal objValue As List(Of IMst_ScheduleRateEntity)) As System.Data.DataTable
            Try
                ' variable
                Dim objDT As New System.Data.DataTable
                Dim objDR As System.Data.DataRow
                Dim objCom As New Common.Utilities.Utility

                SetSRateListToDT = Nothing

                With objDT.Columns
                    .Add("id")
                    .Add("currency")
                    .Add("ef_date")
                    .Add("rate")
                End With

                For Each objItem In objValue
                    With objItem
                        objDR = objDT.NewRow
                        objDR("id") = .id
                        objDR("currency") = .currency
                        objDR("ef_date") = .ef_date.ToDate.ToString("d-MMM-yyyy")
                        objDR("rate") = objCom.FormatNumeric(.rate, 5, True)
                        objDT.Rows.Add(objDR)
                    End With
                Next
                SetSRateListToDT = objDT

            Catch ex As Exception
                ' Write error log
                SetSRateListToDT = Nothing
                objLog.ErrorLog("SetPurchaseListToDT", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CancelScheculeRate
        '	Discription	    : Cancel data schecule_rate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 02-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CancelScheculeRate(ByVal intScheculeRateID As Integer) As Boolean Implements IScheculeRateService.CancelScheculeRate
            Try
                ' variable
                Dim objSRateEntity As New Entity.ImpMst_ScheduleRateEntity

                CancelScheculeRate = False
                ' check data will delete or cancel
                If intScheculeRateID < 1 Then Exit Function
                CancelScheculeRate = objSRateEntity.CancelScheduleRate(intScheculeRateID)

            Catch ex As Exception
                ' Write error log
                CancelScheculeRate = False
                objLog.ErrorLog("CancelScheculeRate", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckDataIsDup
        '	Discription	    : Check data schecule rate is duplicate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDataIsDup(ByVal objSRate As Dto.ScheculeRateDto) As Boolean Implements IScheculeRateService.CheckDataIsDup
            Try
                Dim objSRateEntity As Entity.IMst_ScheduleRateEntity = New Entity.ImpMst_ScheduleRateEntity

                objSRateEntity = ChangeDtoToEntity(objSRate)
                With objSRateEntity
                    CheckDataIsDup = .CheckIsDupScheduleRate(.currency_id, .ef_date.ToString, .id)
                End With

            Catch ex As Exception
                ' Write error log
                CheckDataIsDup = False
                objLog.ErrorLog("CheckDataIsDup", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangeDtoToEntity
        '	Discription	    : Change data dto to entity
        '	Return Value	: Entity.IMst_ScheduleRateEntity
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangeDtoToEntity(ByVal objValue As Dto.ScheculeRateDto) As Entity.IMst_ScheduleRateEntity
            Try
                Dim objCom As New Common.Utilities.Utility
                Dim objSRateEntity As Entity.IMst_ScheduleRateEntity = New Entity.ImpMst_ScheduleRateEntity

                With objSRateEntity
                    .id = objValue.id
                    .currency_id = objValue.currency_id
                    .currency = objValue.currency
                    .ef_date = objCom.String2Date(objValue.ef_date).ToString("yyyyMMdd")
                    .rate = objValue.rate
                    .created_by = HttpContext.Current.Session("UserID")
                    .created_date = Now().ToString("yyyyMMddHHmmss")
                    .updated_by = HttpContext.Current.Session("UserID")
                    .updated_date = Now().ToString("yyyyMMddHHmmss")
                End With

                ChangeDtoToEntity = objSRateEntity

            Catch ex As Exception
                ' Write error log
                ChangeDtoToEntity = Nothing
                objLog.ErrorLog("ChangeDtoToEntity", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: ChangeEntityToDto
        '	Discription	    : Change data entity to dto
        '	Return Value	: Dto.ScheculeRateDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function ChangeEntityToDto(ByVal objValue As Entity.IMst_ScheduleRateEntity) As Dto.ScheculeRateDto
            Try
                Dim objCom As New Common.Utilities.Utility
                Dim objSRateDto As New Dto.ScheculeRateDto

                With objSRateDto
                    .id = objValue.id
                    .currency_id = objValue.currency_id
                    If (Not objValue.currency Is Nothing) Then
                        .currency = objValue.currency
                    End If
                    .ef_date = objValue.ef_date.ToDate.ToString("dd/MM/yyyy")
                    .rate = objValue.rate
                End With

                ChangeEntityToDto = objSRateDto

            Catch ex As Exception
                ' Write error log
                ChangeEntityToDto = Nothing
                objLog.ErrorLog("ChangeEntityToDto", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SaveScheculeRate
        '	Discription	    : Save data ScheculeRate
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SaveScheculeRate(ByVal objSRate As Dto.ScheculeRateDto, ByVal strMode As String) As Boolean Implements IScheculeRateService.SaveScheculeRate
            Try
                Dim objSRateEntity As IMst_ScheduleRateEntity = New ImpMst_ScheduleRateEntity

                SaveScheculeRate = False
                If objSRate Is Nothing Then Exit Function
                objSRateEntity = ChangeDtoToEntity(objSRate)

                If strMode = "Add" Then
                    If objSRateEntity.InsertScheduleRate(objSRateEntity) > 0 Then SaveScheculeRate = True
                ElseIf strMode = "Edit" Then
                    If objSRateEntity.UpdateScheduleRate(objSRateEntity) > 0 Then SaveScheculeRate = True
                End If

            Catch ex As Exception
                ' Write error log
                SaveScheculeRate = False
                objLog.ErrorLog("SaveScheculeRate", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetScheculeRateForDetail
        '	Discription	    : Get data ScheculeRate
        '	Return Value	: Dto.ScheculeRateDto
        '	Create User	    : Boonyarit
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetScheculeRateForDetail(ByVal intScheculeRateID As Integer) As Dto.ScheculeRateDto Implements IScheculeRateService.GetScheculeRateForDetail
            Try
                Dim objSRateEntity As IMst_ScheduleRateEntity = New ImpMst_ScheduleRateEntity
                Dim objSRateDto As New Dto.ScheculeRateDto

                GetScheculeRateForDetail = Nothing
                If intScheculeRateID < 1 Then Exit Function
                objSRateEntity = objSRateEntity.GetScheduleRateById(intScheculeRateID)
                objSRateDto = ChangeEntityToDto(objSRateEntity)
                GetScheculeRateForDetail = objSRateDto

            Catch ex As Exception
                ' Write error log
                GetScheculeRateForDetail = Nothing
                objLog.ErrorLog("GetScheculeRateForDetail", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetScheculeRate
        '	Discription	    : Get data Schedule rate 
        '	Return Value	: Integer
        '	Create User	    : Boonyarit
        '	Create Date	    : 16-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetScheculeRate(ByVal intCurrencyId As Integer, Optional ByVal strDate As String = "") As Decimal Implements IScheculeRateService.GetScheculeRate
            Try
                Dim objSRate As New Entity.ImpMst_ScheduleRateEntity
                Dim intSRate As Decimal

                intSRate = objSRate.GetScheduleRateByPurchase(intCurrencyId, strDate)
                'ปรับเปลี่ยนค่าที่ส่งคืนตามพี่ปิง (19/08/2013)
                GetScheculeRate = intSRate

                ''ไม่พบข้อมูลให้คืนค่าเป็น 1 จาก PGS ของพี่ปิง
                'If intSRate = 0 Then
                '    GetScheculeRate = 1
                'Else
                '    GetScheculeRate = intSRate
                'End If

            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetScheculeRate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class
End Namespace


