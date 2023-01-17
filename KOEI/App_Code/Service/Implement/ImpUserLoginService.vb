#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : ImpUserLoginService
'	Class Discription	: Implement UserLogin Service
'	Create User 		: Nisa S.
'	Create Date		    : 10-07-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Namespace Service
    Public Class ImpUserLoginService
        Implements IUserLoginService

        Private objLog As New Common.Logs.Log
        Private objUserLoginEnt As New Entity.ImpUserLoginEntity



        '/**************************************************************
        '	Function name	: GetUserLoginList
        '	Discription	    : Get UserLogin list
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserLoginList( _
        ByVal strID As String, _
        ByVal strUserName As String, _
        ByVal strFirstName As String, _
        ByVal strLastName As String, _
        ByVal strDepartment As String _
        ) As System.Data.DataTable Implements IUserLoginService.GetUserLoginList
            ' set default
            GetUserLoginList = New DataTable
            Try
                ' variable for keep list from UserLogin entity
                Dim listUserLoginEnt As New List(Of Entity.ImpUserLoginDetailEntity)
                ' data row object
                Dim row As DataRow
                'Dim strLastLogin As String = ""

                ' call function GetUserLoginList from entity
                listUserLoginEnt = objUserLoginEnt.GetUserLoginList(strID, strUserName, strFirstName, strLastName, strDepartment)

                ' assign column header
                With GetUserLoginList
                    .Columns.Add("id")
                    .Columns.Add("user_name")
                    .Columns.Add("password")
                    .Columns.Add("first_name")
                    .Columns.Add("last_name")
                    .Columns.Add("department")
                    .Columns.Add("last_login")



                    ' assign row from listUserLoginEny
                    For Each values In listUserLoginEnt
                        row = .NewRow

                        row("id") = values.id
                        row("user_name") = values.user_name
                        row("password") = values.password
                        row("first_name") = values.first_name
                        row("last_name") = values.last_name
                        row("department") = values.name
                        'row("last_login") = values.last_login

                        If values.last_login <> Nothing Or values.last_login <> "" Then
                            'strLastLogin = Left(values.last_login, 4) & "/" & Mid(values.last_login, 5, 2) & "/" & Right(values.last_login, 2)
                            row("last_login") = CDate(values.last_login).ToString("dd/MMM/yyyy hh:mm:ss")
                        Else
                            row("last_login") = values.last_login
                        End If

                        ' add data row to table
                        .Rows.Add(row)
                    Next
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserLoginList(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetDepartmentForList
        '	Discription	    : Get Department for dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 10-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetDepartmentForList() As System.Collections.Generic.List(Of Dto.UserLoginDto) Implements IUserLoginService.GetDepartmentForList
            ' set default list
            GetDepartmentForList = New List(Of Dto.UserLoginDto)
            Try
                ' objDepartmentDto for keep value Dto 
                Dim objDepartmentDto As Dto.UserLoginDto
                ' listDepartmentEnt for keep value from entity
                Dim listDepartmentEnt As New List(Of Entity.IUserLoginEntity)
                ' objWorkCategoryEnt for call function
                Dim objDepartmentEnt As New Entity.ImpUserLoginEntity

                ' call function GetVendorForList
                listDepartmentEnt = objDepartmentEnt.GetDepartmentForList()

                ' loop listVendorEnt for assign value to Dto
                For Each values In listDepartmentEnt
                    ' new object
                    objDepartmentDto = New Dto.UserLoginDto
                    ' assign value to Dto
                    With objDepartmentDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetDepartmentForList.Add(objDepartmentDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetDepartmentForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetAccount_Next_ApproveForList
        '	Discription	    : Get Account_Next_Approve for dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetAccount_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Dto.UserLoginDto) Implements IUserLoginService.GetAccount_Next_ApproveForList
            ' set default list
            GetAccount_Next_ApproveForList = New List(Of Dto.UserLoginDto)
            Try
                ' objDepartmentDto for keep value Dto 
                Dim objAccount_Next_ApproveDto As Dto.UserLoginDto
                ' listDepartmentEnt for keep value from entity
                Dim listAccount_Next_ApproveEnt As New List(Of Entity.IUserLoginEntity)
                ' objWorkCategoryEnt for call function
                Dim objAccount_Next_ApproveEnt As New Entity.ImpUserLoginEntity

                ' call function GetVendorForList
                listAccount_Next_ApproveEnt = objAccount_Next_ApproveEnt.GetAccount_Next_ApproveForList(id)

                ' loop listVendorEnt for assign value to Dto
                For Each values In listAccount_Next_ApproveEnt
                    ' new object
                    objAccount_Next_ApproveDto = New Dto.UserLoginDto
                    ' assign value to Dto
                    With objAccount_Next_ApproveDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetAccount_Next_ApproveForList.Add(objAccount_Next_ApproveDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetAccount_Next_ApproveForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetPurchase_Next_ApproveForList
        '	Discription	    : Get Purchase_Next_Approve for dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetPurchase_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Dto.UserLoginDto) Implements IUserLoginService.GetPurchase_Next_ApproveForList
            ' set default list
            GetPurchase_Next_ApproveForList = New List(Of Dto.UserLoginDto)
            Try
                ' objDepartmentDto for keep value Dto 
                Dim objPurchase_Next_ApproveDto As Dto.UserLoginDto
                ' listDepartmentEnt for keep value from entity
                Dim listPurchase_Next_ApproveEnt As New List(Of Entity.IUserLoginEntity)
                ' objWorkCategoryEnt for call function
                Dim objPurchase_Next_ApproveEnt As New Entity.ImpUserLoginEntity

                ' call function GetVendorForList
                listPurchase_Next_ApproveEnt = objPurchase_Next_ApproveEnt.GetPurchase_Next_ApproveForList(id)

                ' loop listVendorEnt for assign value to Dto
                For Each values In listPurchase_Next_ApproveEnt
                    ' new object
                    objPurchase_Next_ApproveDto = New Dto.UserLoginDto
                    ' assign value to Dto
                    With objPurchase_Next_ApproveDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetPurchase_Next_ApproveForList.Add(objPurchase_Next_ApproveDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetPurchase_Next_ApproveForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetOutsource_Next_ApproveForList
        '	Discription	    : Get Outsource_Next_Approve for dropdownlist
        '	Return Value	: List
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetOutsource_Next_ApproveForList(ByVal id As String) As System.Collections.Generic.List(Of Dto.UserLoginDto) Implements IUserLoginService.GetOutsource_Next_ApproveForList
            ' set default list
            GetOutsource_Next_ApproveForList = New List(Of Dto.UserLoginDto)
            Try
                ' objDepartmentDto for keep value Dto 
                Dim objOutsource_Next_ApproveDto As Dto.UserLoginDto
                ' listDepartmentEnt for keep value from entity
                Dim listOutsource_Next_ApproveEnt As New List(Of Entity.IUserLoginEntity)
                ' objWorkCategoryEnt for call function
                Dim objOutsource_Next_ApproveEnt As New Entity.ImpUserLoginEntity

                ' call function GetVendorForList
                listOutsource_Next_ApproveEnt = objOutsource_Next_ApproveEnt.GetOutsource_Next_ApproveForList(id)

                ' loop listVendorEnt for assign value to Dto
                For Each values In listOutsource_Next_ApproveEnt
                    ' new object
                    objOutsource_Next_ApproveDto = New Dto.UserLoginDto
                    ' assign value to Dto
                    With objOutsource_Next_ApproveDto
                        .id = values.id
                        .name = values.name
                    End With
                    ' add object Dto to list Dto
                    GetOutsource_Next_ApproveForList.Add(objOutsource_Next_ApproveDto)
                Next
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetOutsource_Next_ApproveForList", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: IsUsedInPO
        '	Discription	    : Check UserLogin in used PO_Detail
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function IsUsedInPO( _
            ByVal intUserLoginID As Integer _
        ) As Boolean Implements IUserLoginService.IsUsedInPO
            ' set default return value
            IsUsedInPO = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CountUsedInPO from entity
                intCount = objUserLoginEnt.CountUsedInPO(intUserLoginID)

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
        '	Function name	: DeleteUserLogin
        '	Discription	    : Delete UserLogin
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DeleteUserLogin( _
            ByVal intUserLoginID As Integer _
        ) As Boolean Implements IUserLoginService.DeleteUserLogin
            ' set default return value
            DeleteUserLogin = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function DeleteStaff from Staff Entity
                intEff = objUserLoginEnt.DeleteUserLogin(intUserLoginID)

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    DeleteUserLogin = True
                Else
                    ' case row less than 1 then return False
                    DeleteUserLogin = False
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("DeleteUserLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        ' Function name   : CheckDupUserLogin
        ' Discription     : Check duplication UserLogin Admin
        ' Return Value    : Boolean
        ' Create User     : Nisa S.
        ' Create Date     : 11-07-2013
        ' Update User     :
        ' Update Date     :
        '*************************************************************/
        Public Function CheckDupUserLogin( _
            ByVal intUserLoginID As String, _
            ByVal strUserName As String, _
            ByVal mode As String, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IUserLoginService.CheckDupUserLogin
            ' set default return value
            CheckDupUserLogin = False
            Try
                ' intEff keep row effect
                Dim intCount As Integer

                ' call function CheckDupUserLogin from entity
                intCount = objUserLoginEnt.CheckDupUserLogin(intUserLoginID, strUserName)

                ' check count used
                If intCount <> 0 Then
                    ' case not equal 0 then return True
                    CheckDupUserLogin = True
                Else
                    ' case equal 0 then return False
                    CheckDupUserLogin = False
                End If
            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTAD_02_007"
                Else
                    If mode = "1" Then ' insert
                        ' other case
                        strMsg = "KTAD_02_003"
                    Else 'Moidfy
                        ' other case
                        strMsg = "KTAD_02_006"
                    End If
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("CheckDupUserLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: InsertUserLogin
        '	Discription	    : Insert UserLogin
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function InsertUserLogin( _
            ByVal objUserLoginDto As Dto.UserLoginDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IUserLoginService.InsertUserLogin
            ' set default return value
            InsertUserLogin = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function InsertStaff from Staff Entity
                intEff = objUserLoginEnt.InsertUserLogin(SetDtoToEntity(objUserLoginDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    InsertUserLogin = True
                Else
                    ' case row less than 1 then return False
                    InsertUserLogin = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTAD_02_007"
                Else
                    ' other case
                    strMsg = "KTAD_02_003"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("InsertUserLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: UpdateUserLogin
        '	Discription	    : Update UserLogin
        '	Return Value	: Boolean
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function UpdateUserLogin( _
            ByVal objUserLoginDto As Dto.UserLoginDto, _
            Optional ByRef strMsg As String = "" _
        ) As Boolean Implements IUserLoginService.UpdateUserLogin
            ' set default return value
            UpdateUserLogin = False
            Try
                ' intEff keep row effect
                Dim intEff As Integer
                ' call function UpdateStaff from Staff Entity
                intEff = objUserLoginEnt.UpdateUserLogin(SetDtoToEntity(objUserLoginDto))

                ' check row effect
                If intEff > 0 Then
                    ' case row more than 0 then return True
                    UpdateUserLogin = True
                Else
                    ' case row less than 1 then return False
                    UpdateUserLogin = False
                End If

            Catch exSql As MySqlException
                ' check error of mysql return
                If exSql.Number = 1062 Then
                    ' case 1062 mean duplicate data
                    strMsg = "KTAD_02_007"
                Else
                    ' other case
                    strMsg = "KTAD_02_006"
                End If
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("UpdateUserLogin(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function


        '/**************************************************************
        '	Function name	: SetDtoToEntity
        '	Discription	    : Set data from Dto to Entity
        '	Return Value	: UserLogin Entity object
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Private Function SetDtoToEntity( _
            ByVal objUserLoginDto As Dto.UserLoginDto _
        ) As Entity.IUserLoginEntity
            ' set default return value
            SetDtoToEntity = New Entity.ImpUserLoginEntity
            Try
                ' assign value to entity
                With SetDtoToEntity
                    .id = objUserLoginDto.id
                    .user_name = objUserLoginDto.user_name
                    .password = objUserLoginDto.password
                    .first_name = objUserLoginDto.first_name
                    .last_name = objUserLoginDto.last_name
                    .department_id = objUserLoginDto.department_id
                    .account_next_approve = objUserLoginDto.account_next_approve
                    .purchase_next_approve = objUserLoginDto.purchase_next_approve
                    .outsource_next_approve = objUserLoginDto.outsource_next_approve
                    .last_login = objUserLoginDto.last_login
                    .delete_fg = objUserLoginDto.delete_fg
                    .created_by = objUserLoginDto.created_by
                    .created_date = objUserLoginDto.created_date
                    .updated_by = objUserLoginDto.updated_by
                    .updated_date = objUserLoginDto.updated_date
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("SetDtoToEntity(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserLoginByID
        '	Discription	    : Get UserLogin by ID
        '	Return Value	: UserLogin dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserLoginByID( _
           ByVal intUserLoginID As String _
       ) As Dto.UserLoginDto Implements IUserLoginService.GetUserLoginByID
            ' set default return value
            GetUserLoginByID = New Dto.UserLoginDto
            Try
                ' object for return value from Entity
                Dim objUserLoginEntRet As New Entity.ImpUserLoginEntity
                ' call function GetStaffByID from Entity
                objUserLoginEntRet = objUserLoginEnt.GetUserLoginByID(intUserLoginID)

                ' assign value from Entity to Dto
                With GetUserLoginByID
                    .id = objUserLoginEntRet.id
                    .user_name = objUserLoginEntRet.user_name
                    .password = objUserLoginEntRet.password
                    .first_name = objUserLoginEntRet.first_name
                    .last_name = objUserLoginEntRet.last_name
                    .department_id = objUserLoginEntRet.department_id
                    .account_next_approve = objUserLoginEntRet.account_next_approve
                    .purchase_next_approve = objUserLoginEntRet.purchase_next_approve
                    .outsource_next_approve = objUserLoginEntRet.outsource_next_approve
                End With
            Catch ex As Exception
                ' write error log
                objLog.ErrorLog("GetUserLoginByID(Service)", ex.Message.ToString, HttpContext.Current.Session("UserName"))
            End Try
        End Function

        '/**************************************************************
        '	Function name	: GetUserLoginForDetail
        '	Discription	    : Get UserLogin for Detail
        '	Return Value	: UserLogin dto object
        '	Create User	    : Nisa S.
        '	Create Date	    : 11-07-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function GetUserLoginForDetail(ByVal intUserLoginID As Integer) As Dto.UserLoginDto Implements IUserLoginService.GetUserLoginForDetail
            Try
                ' variable
                Dim objUserLogin As New Entity.ImpUserLoginEntity
                Dim objDataUserLogin As New Entity.ImpUserLoginEntity

                GetUserLoginForDetail = Nothing
                'Get data vendor
                objDataUserLogin = objUserLogin.GetUserLoginForDetail(intUserLoginID)
                If objDataUserLogin Is Nothing Then Exit Function

                ' assign object vendor dto
                GetUserLoginForDetail = New Dto.UserLoginDto
                With GetUserLoginForDetail
                    .id = objDataUserLogin.id
                    '***Start Boon add
                    .user_name = objDataUserLogin.user_name
                    .password = objDataUserLogin.password
                    .first_name = objDataUserLogin.first_name
                    .last_name = objDataUserLogin.last_name
                    .name = objDataUserLogin.name
                    .account_next_approve = objDataUserLogin.account_next_approve
                    .purchase_next_approve = objDataUserLogin.purchase_next_approve
                    .outsource_next_approve = objDataUserLogin.outsource_next_approve

                    '***End Boon add
                    '.type1_text = objDataVendor.type1_text.Trim
                    '.type2_text = objDataVendor.type2_text.Trim
                    '.type2_no = objDataVendor.type2_no.Trim
                    '.name = objDataVendor.name.Trim
                    '.person_in_charge1 = objDataVendor.person_in_charge1.Trim
                    '.person_in_charge2 = objDataVendor.person_in_charge2.Trim
                    '.payment_term = objDataVendor.payment_term.Trim
                    '.payment_condition = objDataVendor.payment_condition.Trim
                    '.country_name = objDataVendor.country_name.Trim
                    '.zipcode = objDataVendor.zipcode.Trim
                    '.address = objDataVendor.address.Trim
                    '.tel = objDataVendor.tel.Trim
                    '.fax = objDataVendor.fax.Trim
                    '.email = objDataVendor.email.Trim
                    '.type_of_goods_text = objDataVendor.type_of_goods_text.Trim
                    '.remarks = objDataVendor.remarks.Trim
                    '.file = objDataVendor.file.Trim
                End With

            Catch ex As Exception
                ' Write error log
                GetUserLoginForDetail = Nothing
                objLog.ErrorLog("GetUserLoginForDetail", ex.Message.Trim, HttpContext.Current.Session("UserName"))
            End Try
        End Function
    End Class
End Namespace
