#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpPaymentConditionService
'	Class Discription	: Class of  Payment Condition 
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
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
Imports System.Data
Imports MySql.Data.MySqlClient
#End Region


Namespace Service
    Public Class ImpPaymentConditionService
        Implements IPaymentConditionService

        Private objLog As New Common.Logs.Log
        Private objPayEnt As New Entity.ImpMst_PaymentConditionEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: GetPaymentConditionForList
        '	Discription	    : Get payment condition for dropdownlist
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 11-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPaymentConditionForList() As System.Collections.Generic.List(Of Dto.PaymentConditionDto) Implements IPaymentConditionService.GetPaymentConditionForList
            ' set default list
            GetPaymentConditionForList = New List(Of Dto.PaymentConditionDto)
            Try
                ' objPaymentConDto for keep value Dto 
                Dim objPaymentConDto As Dto.PaymentConditionDto
                ' listJobTypeEnt for keep value from entity
                Dim listPaymentConEnt As New List(Of Entity.IMst_PaymentConditionEntity)
                ' objVendorEnt for call function
                Dim objPaymentConEnt As New Entity.ImpMst_PaymentConditionEntity

                ' call function GetPaymentConditionForList
                listPaymentConEnt = objPaymentConEnt.GetPaymentConditionForList()

                ' loop listJobTypeEnt for assign value to Dto
                For Each values In listPaymentConEnt
                    ' new object
                    objPaymentConDto = New Dto.PaymentConditionDto
                    ' assign value to Dto
                    With objPaymentConDto
                        .codition_id = values.condition_id
                        .codition_name = values.condition_name
                    End With
                    ' add object Dto to list Dto
                    GetPaymentConditionForList.Add(objPaymentConDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentConditionForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function GetPaymentCondList( _
            ByVal strFirst As String, _
            ByVal strSecond As String, _
            ByVal strThird As String _
        ) As System.Data.DataTable Implements IPaymentConditionService.GetPaymentCondList
            '*****************************************************************************************************
            ' Function name	: GetPaymentCondList
            ' Discription	: Get data Payment Condition for search
            ' Return Value	: List
            ' Create User	: Wasan D.
            ' Create Date	: 03-07-2013
            '*****************************************************************************************************

            ' set default
            GetPaymentCondList = New DataTable
            Try
                ' variable for keep list from item entity
                Dim listPayEnt As New List(Of Entity.ImpMst_PaymentConditionEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetItemList from entity
                listPayEnt = objPayEnt.GetPaymentCondList(strFirst, strSecond, strThird)

                ' assign column header
                With GetPaymentCondList
                    .Columns.Add("PayID")
                    .Columns.Add("First")
                    .Columns.Add("Second")
                    .Columns.Add("Third")
                    .Columns.Add("Payment Condition")

                    ' assign row from listItemEny
                    For Each values In listPayEnt
                        row = .NewRow
                        row("PayID") = values.id
                        row("First") = values.first
                        row("Second") = values.second
                        row("Third") = values.third
                        row("Payment Condition") = values.payment_condition
                        '?'row("in_used") = values.delete_fg
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPaymentCondList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function
        '/**************************************************************
        '	Function name	: DeleteItem
        '	Discription	    : Delete item
        '	Return Value	: Boolean
        '	Create User	    : Wasan D.
        '	Create Date	    : 03-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeletePaymentCond(ByVal intPayID As Integer) As Boolean Implements IPaymentConditionService.DeletePaymentCond
            ' set default return value
            DeletePaymentCond = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteItem from Payment Condition Entity
                intEff = objPayEnt.DeletePaymentCond(intPayID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeletePaymentCond = True
                Else
                    ' case row less than 1 then return False
                    DeletePaymentCond = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeletePaymentCond(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function GetPaymentCondByID(ByVal intPayID As Integer) As Dto.PaymentConditionDto Implements IPaymentConditionService.GetPaymentCondByID
            '/**************************************************************
            '	Function name	: GetPaymentCondByID
            '	Discription	    : Get Payment_Condition by ID
            '	Return Value	: Payment_Condition dto object
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '	Update User	    :
            '	Update Date	    :
            '*************************************************************/
            ' set default return value
            GetPaymentCondByID = New Dto.PaymentConditionDto
            Try
                ' object for return value from Entity
                Dim objPayCondEntRet As New Entity.ImpMst_PaymentConditionEntity
                ' call function GetItemByID from Entity
                objPayCondEntRet = objPayEnt.GetPaymentCondByID(intPayID)

                ' assign value from Entity to Dto
                With GetPaymentCondByID
                    .codition_id = objPayCondEntRet.id
                    .codition_1st = objPayCondEntRet.first
                    .codition_2nd = objPayCondEntRet.second
                    .codition_3rd = objPayCondEntRet.third
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetItemByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function InsertPaymentCond(ByVal objPayDto As Dto.PaymentConditionDto, Optional ByRef strMsg As String = "") As Boolean Implements IPaymentConditionService.InsertPaymentCond
            '/**************************************************************
            '	Function name	: InsertPaymentCond
            '	Discription	    : Insert Payment Condition
            '	Return Value	: Boolean
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '	Update User	    :
            '	Update Date	    :
            '*************************************************************/
            ' set default return value
            InsertPaymentCond = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function GetPayCondDupInsert from Payment Condition Entity
                intEff = objPayEnt.GetPayCondDupInsert(SetDtoToEntity(objPayDto))

                If intEff = True Then
                    ' case Duplicate data
                    strMsg = "KTMS_26_004"
                    Exit Function
                End If

                ' call function InsertPaymentCond from Payment Condition Entity
                intEff = objPayEnt.InsertPaymentCond(SetDtoToEntity(objPayDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertPaymentCond = True
                Else
                    ' case row less than 1 then return False
                    InsertPaymentCond = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_26_004"
                Else
                    ' other case
                    strMsg = "KTMS_26_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertPaymentCond(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function IsUsedInPO(ByVal intPayID As Integer) As Integer Implements IPaymentConditionService.IsUsedInPO
            '*****************************************************************************************************
            ' Function name	: IsUsedInPO
            ' Discription	: Get data Payment Condition for search
            ' Return Value	: List
            ' Copy from     : Service.ImpItemService.ISUsedInPO
            ' Modified by   : Wasan D.
            ' Modified Date	: 03-07-2013
            '*****************************************************************************************************

            IsUsedInPO = -1
            Try
                ' call function CountUsedInPO from entity
                IsUsedInPO = objPayEnt.CountUsedInPO(intPayID)
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInPO(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        Public Function UpdatePaymentCond(ByVal objPayDto As Dto.PaymentConditionDto, Optional ByRef strMsg As String = "") As Boolean Implements IPaymentConditionService.UpdatePaymentCond
            '/**************************************************************
            '	Function name	: UpdateItem
            '	Discription	    : Update item
            '	Return Value	: Boolean
            '	Create User	    : Wasan D.
            '	Create Date	    : 04-07-2013
            '	Update User	    :
            '	Update Date	    :
            '*************************************************************/
            ' set default return value
            UpdatePaymentCond = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function GetPayCondDupUpdate from Payment Condition Entity
                intEff = objPayEnt.GetPayCondDupUpdate(SetDtoToEntity(objPayDto))

                If intEff = True Then
                    ' case Duplicate data
                    strMsg = "KTMS_26_007"
                    Exit Function
                End If

                ' call function UpdateItem from Payment Condition Entity
                intEff = objPayEnt.UpdatePaymentCond(SetDtoToEntity(objPayDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdatePaymentCond = True
                Else
                    ' case row less than 1 then return False
                    UpdatePaymentCond = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTMS_26_007"
                Else
                    ' other case
                    strMsg = "KTMS_26_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdatePaymentCond(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Wasan D.
        '	Create Date	    : 04-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objPayDto As Dto.PaymentConditionDto _
        ) As Entity.IMst_PaymentConditionEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_PaymentConditionEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objPayDto.codition_id
                    .first = objPayDto.codition_1st
                    .second = objPayDto.codition_2nd
                    .third = objPayDto.codition_3rd
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
    End Class
End Namespace

