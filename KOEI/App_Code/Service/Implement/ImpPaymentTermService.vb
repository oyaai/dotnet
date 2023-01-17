#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpPaymentTermService
'	Class Discription	: Implement payment term Service
'	Create User 		: Nisa S.
'	Create Date		    : 19-06-2013
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
Imports System.Data
Imports Service
Imports MySql.Data.MySqlClient
#End Region

Namespace Service
    Public Class ImpPaymentTermService
        Implements Service.IPaymentTermService

        Private objPaymentTermEntity As New Entity.ImpMst_Payment_TermEntity
        Private objLog As New Common.Logs.Log

#Region "Function"
        '/**************************************************************
        '	Function name	: GetPaymentList
        '	Discription	    : Get Payment Term list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentList(ByVal strPayment As String) _
        As System.Data.DataTable Implements Service.IPaymentTermService.GetPaymentList
            ' set default
            GetPaymentList = New DataTable
            Try
                ' variable for keep list from item entity
                Dim listPaymentTermEntity As New List(Of Entity.IMst_Payment_TermEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listPaymentTermEntity = objPaymentTermEntity.GetPaymentList(strPayment)

                ' assign column header
                With GetPaymentList
                    .Columns.Add("id")
                    .Columns.Add("term_day")


                    ' assign row from listItemEny
                    For Each values In listPaymentTermEntity
                        row = .NewRow
                        row("id") = values.id
                        row("term_day") = values.term_day

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPaymentByID
        '	Discription	    : Get Payment Term by ID
        '	Return Value	: Payment Term dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentTermByID( _
           ByVal intPaymentTermID As Integer _
       ) As Dto.IPayment_TermDto Implements IPaymentTermService.GetPaymentTermByID
            ' set default return value
            GetPaymentTermByID = New Dto.IPayment_TermDto
            Try
                ' object for return value from Entity
                Dim objPaymentTermEntRet As New Entity.ImpMst_Payment_TermEntity
                ' call function GetItemByID from Entity
                objPaymentTermEntRet = objPaymentTermEntity.GetPaymentTermByID(intPaymentTermID)

                ' assign value from Entity to Dto
                With GetPaymentTermByID
                    .id = objPaymentTermEntRet.id
                    .term_day = objPaymentTermEntRet.term_day

                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check Payment Term in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intPaymentTermID As Integer _
        ) As Boolean Implements Service.IPaymentTermService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objPaymentTermEntity.CountUsedInPO(intPaymentTermID)

                intCount = intCount + objPaymentTermEntity.CountUsedInPO2(intPaymentTermID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInPO = True
                Else
                    ' case equal 0 then return False
                    IsUsedInPO = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO2
        '	Discription	    : Check Payment Term in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO2( _
           ByVal intPaymentTermID As Integer _
       ) As Boolean Implements Service.IPaymentTermService.IsUsedInPO2
            ' set default return value
            IsUsedInPO2 = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objPaymentTermEntity.CountUsedInPO(intPaymentTermID)

                intCount = intCount + objPaymentTermEntity.CountUsedInPO2(intPaymentTermID)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInPO2 = True
                Else
                    ' case equal 0 then return False
                    IsUsedInPO2 = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO2(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: DeletePaymentTerm
        '	Discription	    : Delete PaymentTerm
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 19-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePaymentTerm( _
            ByVal intPaymentTermID As Integer _
        ) As Boolean Implements IPaymentTermService.DeletePaymentTerm
            ' set default return value
            DeletePaymentTerm = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteItem from Item Entity
                intEff = objPaymentTermEntity.DeletePaymentTerm(intPaymentTermID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeletePaymentTerm = True
                Else
                    ' case row less than 1 then return False
                    DeletePaymentTerm = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePaymentTerm(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertPaymentTerm
        '	Discription	    : Insert PaymentTerm
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertPaymentTerm( _
            ByVal objPaymentTermDto As Dto.IPayment_TermDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IPaymentTermService.InsertPaymentTerm
            ' set default return value
            InsertPaymentTerm = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertItem from Item Entity
                intEff = objPaymentTermEntity.InsertPaymentTerm(SetDtoToEntity(objPaymentTermDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertPaymentTerm = True
                Else
                    ' case row less than 1 then return False
                    InsertPaymentTerm = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_004"
                Else
                    ' other case
                    strMsg = "KTMS_14_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPaymentTerm(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdatePaymentTerm
        '	Discription	    : Update PaymentTerm
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdatePaymentTerm( _
            ByVal objPaymentTermDto As Dto.IPayment_TermDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IPaymentTermService.UpdatePaymentTerm
            ' set default return value
            UpdatePaymentTerm = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdatePaymentTerm from Payment Term Entity
                intEff = objPaymentTermEntity.UpdatePaymentTerm(SetDtoToEntity(objPaymentTermDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdatePaymentTerm = True
                Else
                    ' case row less than 1 then return False
                    UpdatePaymentTerm = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_007"
                Else
                    ' other case
                    strMsg = "KTMS_14_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePaymentTerm(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 20-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objPaymentTermDto As Dto.IPayment_TermDto _
        ) As Entity.IMst_Payment_TermEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_Payment_TermEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objPaymentTermDto.id
                    .term_day = objPaymentTermDto.term_day
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name : CheckDupInsert
        ' Discription     : Check duplication PaymentTerm Master
        ' Return Value : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 20-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupInsert( _
            ByVal intPayment As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IPaymentTermService.CheckDupInsert
            ' set default return value
            CheckDupInsert = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objPaymentTermEntity.CheckDupInsert(intPayment)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupInsert = True
                Else
                    ' case equal 0 then return False
                    CheckDupInsert = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_14_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_14_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupInsert(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name : CheckDupUpdate
        ' Discription     : Check duplication PaymentTerm Master
        ' Return Value : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 20-06-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupUpdate( _
            ByVal intPaymentTermID As String, _
            ByVal intPayment As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IPaymentTermService.CheckDupUpdate
            ' set default return value
            CheckDupUpdate = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objPaymentTermEntity.CheckDupUpdate(intPaymentTermID, intPayment)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupUpdate = True
                Else
                    ' case equal 0 then return False
                    CheckDupUpdate = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_14_004"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTMS_14_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTMS_14_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupUpdate(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetListPaymentDay
        '	Discription	    : Set list paynemt to dropdownlist
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 18-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function SetListPaymentDay(ByRef objValue As System.Web.UI.WebControls.DropDownList) As Boolean Implements Service.IPaymentTermService.SetListPaymentDay
            Dim objLog As New Common.Logs.Log
            Try
                ' variable
                Dim objPayment As New Entity.ImpMst_Payment_TermEntity
                Dim objListPayment As List(Of Entity.IMst_Payment_TermEntity)
                Dim objComm As New Common.Utilities.Utility

                SetListPaymentDay = False
                ' get data list Payment_day
                objListPayment = objPayment.GetListPaymentDay
                If objListPayment.Count < 1 Then Exit Function
                Call objComm.LoadList(objValue, objListPayment, "term_day", "id")
                If objValue.Items.Count > 0 Then SetListPaymentDay = True

            Catch ex As Exception
                ' Write error log
                SetListPaymentDay = False
                objLog.ErrorLog("SetListPaymentDay", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPaymentDayForList
        '	Discription	    : Get job type for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 25-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentDayForList() As System.Collections.Generic.List(Of Dto.IPayment_TermDto) Implements IPaymentTermService.GetPaymentDayForList

            ' set default list
            GetPaymentDayForList = New List(Of Dto.IPayment_TermDto)
            Try
                ' objPaymentTermDto for keep value Dto 
                Dim objPaymentTermDto As Dto.IPayment_TermDto
                ' listPaymentTermEnt for keep value from entity
                Dim listPaymentTermEnt As New List(Of Entity.IMst_Payment_TermEntity)
                ' objPaymentTermEnt for call function
                Dim objPaymentTermEnt As New Entity.ImpMst_Payment_TermEntity

                ' call function GetJobTypeForList
                listPaymentTermEnt = objPaymentTermEnt.GetListPaymentDay()

                ' loop listPaymentTermEnt for assign value to Dto
                For Each values In listPaymentTermEnt
                    ' new object
                    objPaymentTermDto = New Dto.IPayment_TermDto
                    ' assign value to Dto
                    With objPaymentTermDto
                        .id = values.id
                        .term_day = values.term_day
                    End With
                    ' add object Dto to list Dto
                    GetPaymentDayForList.Add(objPaymentTermDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentDayForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
    End Class
End Namespace