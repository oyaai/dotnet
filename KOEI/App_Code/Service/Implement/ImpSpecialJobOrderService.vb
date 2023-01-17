#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpSpecialJobOrderService
'	Class Discription	: Implement Special Job Order Service
'	Create User 		: Suwishaya L.
'	Create Date		    : 07-06-2013
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
    Public Class ImpSpecialJobOrderService
        Implements ISpecialJobOrderService

        Private objLog As New Common.Logs.Log
        Private objSpecialJobOrderEnt As New Entity.ImpMst_SpecialJobOrderEntity

#Region "Function"

        '/**************************************************************
        '	Function name	: DeleteSpecialJobOrder
        '	Discription	    : Delete Special Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteSpecialJobOrder( _
            ByVal intJobOrderID As Integer _
        ) As Boolean Implements ISpecialJobOrderService.DeleteSpecialJobOrder
            ' set default return value
            DeleteSpecialJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteSpecialJobOrder from Special Job Orde Entity
                intEff = objSpecialJobOrderEnt.DeleteSpecialJobOrder(intJobOrderID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteSpecialJobOrder = True
                Else
                    ' case row less than 1 then return False
                    DeleteSpecialJobOrder = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteSpecialJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSpecialJobOrderByID
        '	Discription	    : Get Special Job Order by ID
        '	Return Value	: Special Job Order dto object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSpecialJobOrderByID( _
            ByVal intJobOrderID As Integer _
        ) As Dto.SpecialJobOrderDto Implements ISpecialJobOrderService.GetSpecialJobOrderByID
            ' set default return value
            GetSpecialJobOrderByID = New Dto.SpecialJobOrderDto
            Try
                ' object for return value from Entity
                Dim objSpecialJobOrderEntRet As New Entity.ImpMst_SpecialJobOrderEntity
                ' call function GetItemByID from Entity
                objSpecialJobOrderEntRet = objSpecialJobOrderEnt.GetSpecialJobOrderByID(intJobOrderID)

                ' assign value from Entity to Dto
                With GetSpecialJobOrderByID
                    .id = objSpecialJobOrderEntRet.id
                    .job_order = objSpecialJobOrderEntRet.job_order
                    .remark = objSpecialJobOrderEntRet.remark
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSpecialJobOrderByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetSpecialJobOrderList
        '	Discription	    : Get Special Job Order list
        '	Return Value	: List
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetSpecialJobOrderList( _
            ByVal strJobOrderFrom As String, _
            ByVal strJobOrderTo As String _
        ) As System.Data.DataTable Implements ISpecialJobOrderService.GetSpecialJobOrderList
            ' set default
            GetSpecialJobOrderList = New DataTable
            Try
                ' variable for keep list from item entity
                Dim listJobOrderEnt As New List(Of Entity.ImpMst_SpecialJobOrderEntity)
                ' data row object
                Dim row As DataRow

                ' call function GetSpecialJobOrderList from entity
                listJobOrderEnt = objSpecialJobOrderEnt.GetSpecialJobOrderList(strJobOrderFrom, strJobOrderTo)

                ' assign column header
                With GetSpecialJobOrderList
                    .Columns.Add("id")
                    .Columns.Add("job_order")
                    .Columns.Add("remark")

                    ' assign row from listJobOrderEnt
                    For Each values In listJobOrderEnt
                        row = .NewRow
                        row("id") = values.id
                        row("job_order") = values.job_order
                        row("remark") = values.remark
                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetSpecialJobOrderList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertSpecialJobOrder
        '	Discription	    : Insert Special Job Order
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertSpecialJobOrder( _
            ByVal objSpecialJobOrderDto As Dto.SpecialJobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ISpecialJobOrderService.InsertSpecialJobOrder
            ' set default return value
            InsertSpecialJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertItem from Item Entity
                intEff = objSpecialJobOrderEnt.InsertSpecialJobOrder(SetDtoToEntity(objSpecialJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertSpecialJobOrder = True
                Else
                    ' case row less than 1 then return False
                    InsertSpecialJobOrder = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_04_001"
                Else
                    ' other case
                    strMsg = "KTJB_04_004"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertSpecialJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateSpecialJobOrder
        '	Discription	    : Update SpecialJobOrder
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateSpecialJobOrder( _
            ByVal objSpecialJobOrderDto As Dto.SpecialJobOrderDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements ISpecialJobOrderService.UpdateSpecialJobOrder
            ' set default return value
            UpdateSpecialJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateItem from Item Entity
                intEff = objSpecialJobOrderEnt.UpdateSpecialJobOrder(SetDtoToEntity(objSpecialJobOrderDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateSpecialJobOrder = True
                Else
                    ' case row less than 1 then return False
                    UpdateSpecialJobOrder = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTJB_04_008"
                Else
                    ' other case
                    strMsg = "KTJB_04_007"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateSpecialJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: Item Entity object
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 07-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objSpecialJobOrderDto As Dto.SpecialJobOrderDto _
        ) As Entity.IMst_SpecialJobOrderEntiy
            ' set default return value
            SetDtoToEntity = New Entity.ImpMst_SpecialJobOrderEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objSpecialJobOrderDto.id
                    .job_order = objSpecialJobOrderDto.job_order
                    .remark = objSpecialJobOrderDto.remark
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInAccounting
        '	Discription	    : Check Job Order in used Accounting
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInAccounting( _
            ByVal strJobOrder As String _
        ) As Boolean Implements ISpecialJobOrderService.IsUsedInAccounting
            ' set default return value
            IsUsedInAccounting = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInVendor from entity
                intCount = objSpecialJobOrderEnt.CountUsedInSpecialJobOrder(strJobOrder)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    IsUsedInAccounting = True
                Else
                    ' call function CountUsedInVendor from entity
                    intCount = objSpecialJobOrderEnt.CountUsedInReceiveDetail(strJobOrder)
                    ' check count used
                    If intCount <> 0 Then
                        ' case not equal 0 then return True
                        IsUsedInAccounting = True
                    Else
                        ' case equal 0 then return False
                        IsUsedInAccounting = False
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("IsUsedInAccounting(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: CheckDupSpecialJobOrder
        '	Discription	    : Check duplicate Special Job Order 
        '	Return Value	: Boolean
        '	Create User	    : Suwishaya L.
        '	Create Date	    : 10-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function CheckDupSpecialJobOrder( _
            ByVal intJobOrderID As Integer, _
            ByVal strJobOrder As String _
        ) As Boolean Implements ISpecialJobOrderService.CheckDupSpecialJobOrder
            ' set default return value
            CheckDupSpecialJobOrder = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function CheckDupSpecialJobOrder from Special Job Order Entity
                intEff = objSpecialJobOrderEnt.CheckDupSpecialJobOrder(intJobOrderID, strJobOrder)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    CheckDupSpecialJobOrder = False
                Else
                    ' call function CheckDupJobOrder from Special Job Order Entity
                    intEff = objSpecialJobOrderEnt.CheckDupJobOrder(strJobOrder)
                    ' check row effect
                    If intEff > 0 Then
                        ' case row more than 0 then return True
                        CheckDupSpecialJobOrder = False
                    Else
                        ' case row less than 1 then return False
                        CheckDupSpecialJobOrder = True
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupSpecialJobOrder(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

#End Region
        
    End Class
End Namespace
