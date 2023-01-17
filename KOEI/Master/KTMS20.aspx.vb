#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Master
'	Class Name		    : Master_KTMS20
'	Class Discription	: Webpage for maintenance WorkingCategory master
'	Create User 		: Pranitda Sroengklang
'	Create Date		    : 03-06-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

Partial Class Master_KTMS20
    Inherits System.Web.UI.Page

    Private objUtility As New Common.Utilities.Utility
    Private objLog As New Common.Logs.Log
    Private objWorkingCategorySer As New Service.ImpWorkingCategoryService
    Private objMessage As New Common.Utilities.Message
    Private Const strConfirmIns As String = "ConfirmIns"
    Private Const strConfirmUpd As String = "ConfirmUpd"
    Private strMsg As String = String.Empty

#Region "Event"
    '/**************************************************************
    '	Function name	: Page_Init
    '	Discription	    : Event page initial
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Init( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Init
        Try
            ' write start log
            objLog.StartLog("KTMS20 : Working Category Master")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Init", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: Page_Load
    '	Discription	    : Event page load
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub Page_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load
        Try
            ' check postback page
            If Not IsPostBack Then
                ' case not post back then call function initialpage
                InitialPage()
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("Page_Load", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnBack_Click
    '	Discription	    : Event btnBack is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnBack_Click( _
       ByVal sender As Object, _
       ByVal e As System.EventArgs _
   ) Handles btnBack.Click
        Try
            Response.Redirect("KTMS19.aspx?New=Back")
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnBack_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnClear_Click
    '	Discription	    : Event btnClear is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnClear_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnClear.Click
        Try
            ' call function ClearControl
            ClearControl()
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnClear_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: btnSave_Click
    '	Discription	    : Event btnSave is click
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Protected Sub btnSave_Click( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSave.Click
        Try
            ' call function set session dto
            SetValueToDto()

            ' check mode then show confirm message box
            If Session("Mode") = "Add" Then
                objMessage.ConfirmMessage("KTMS20", strConfirmIns, objMessage.GetXMLMessage("KTMS_20_001"))
            ElseIf Session("Mode") = "Edit" Then
                objMessage.ConfirmMessage("KTMS20", strConfirmUpd, objMessage.GetXMLMessage("KTMS_20_007"))
            End If
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("btnSave_Click", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

#Region "Function"
    '/**************************************************************
    '	Function name	: InitialPage
    '	Discription	    : Initial page function
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 07-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InitialPage()
        Try
            ' check insert Working Category
            If objUtility.GetQueryString(strConfirmIns) = "True" Then
                InsertWorkingCategory()
                Exit Sub
            End If

            ' check update Working Category
            If objUtility.GetQueryString(strConfirmUpd) = "True" Then
                UpdateWorkingCategory()
                Exit Sub
            End If

            ' check mode
            If Request.QueryString("Mode") = "Add" Then
                Session("Mode") = "Add"
            ElseIf Request.QueryString("Mode") = "Edit" Then
                Session("Mode") = "Edit"
                LoadInitialUpdate()
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InitialPage", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
    '/**************************************************************
    '	Function name	: LoadInitialUpdate
    '	Discription	    : Load initial for update data
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub LoadInitialUpdate()
        Try
            ' Item Dto object for keep return value from service
            Dim objWorkingCategoryDto As New Dto.WorkingCategoryDto
            Dim intWorkingCategoryID As Integer = 0

            ' check Working Category id then convert to integer
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                intWorkingCategoryID = CInt(objUtility.GetQueryString("id"))
            End If

            ' call function GetItemByID from service
            objWorkingCategoryDto = objWorkingCategorySer.GetWorkingCategoryByID(intWorkingCategoryID)

            ' assign value to control
            With objWorkingCategoryDto
                txtWorkingCategoryID.Text = .id
                txtWorkCat.Text = .name
                Session("id") = .id
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("LoadInitialUpdate", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: ClearControl
    '	Discription	    : Clear data each control
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub ClearControl()
        Try
            ' clear control
            txtWorkCat.Text = String.Empty
        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("ClearControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToDto
    '	Discription	    : Set value to Dto
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToDto()
        Try
            ' WorkingCategory dto object
            Dim objWorkingCategoryDto As New Dto.WorkingCategoryDto

            ' assign value to dto object
            With objWorkingCategoryDto
                If String.IsNullOrEmpty(txtWorkingCategoryID.Text.Trim) Then
                    .id = 0
                Else
                    .id = txtWorkingCategoryID.Text.Trim
                End If

                .name = txtWorkCat.Text.Trim
            End With

            ' set dto object to session
            Session("objWorkingCategoryDto") = objWorkingCategoryDto

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToDto", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: SetValueToControl
    '	Discription	    : Set value to control
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub SetValueToControl()
        Try
            ' Item dto object
            Dim objWorkingCategoryDto As New Dto.WorkingCategoryDto
            ' set value to dto object from session
            objWorkingCategoryDto = Session("objWorkingCategoryDto")

            ' set value to control
            With objWorkingCategoryDto
                If .id = 0 Then
                    txtWorkingCategoryID.Text = String.Empty
                Else
                    txtWorkingCategoryID.Text = .id
                End If

                txtWorkCat.Text = .name
            End With

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("SetValueToControl", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: InsertWorkingCategory
    '	Discription	    : Insert WorkingCategory
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub InsertWorkingCategory()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objWorkingCategorySer.CheckDupWorkCategory(txtWorkCat.Text, "", strMsg) = False And strMsg = "" Then
                ' call function InsertWorkingCategory from service and alert message
                If objWorkingCategorySer.InsertWorkingCategory(Session("objWorkingCategoryDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_20_003"), Nothing, "KTMS19.aspx?New=Insert")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            ElseIf strMsg <> "" Then
                objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_20_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("InsertWorkingCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub

    '/**************************************************************
    '	Function name	: UpdateWorkingCategory
    '	Discription	    : Update WorkingCategory
    '	Return Value	: nothing
    '	Create User	    : Pranitda Sroengklang
    '	Create Date	    : 05-06-2013
    '	Update User	    :
    '	Update Date	    :
    '*************************************************************/
    Private Sub UpdateWorkingCategory()
        Try
            ' call function set value to control
            SetValueToControl()

            'check duplicate
            If objWorkingCategorySer.CheckDupWorkCategory(txtWorkCat.Text, Session("id")) = False Then
                ' call function UpdateWorkingCategory from service and alert message
                If objWorkingCategorySer.UpdateWorkingCategory(Session("objWorkingCategoryDto"), strMsg) Then
                    objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_20_005"), Nothing, "KTMS19.aspx?New=Update")
                Else
                    objMessage.AlertMessage(objMessage.GetXMLMessage(strMsg))
                End If
            Else
                objMessage.AlertMessage(objMessage.GetXMLMessage("KTMS_20_002"))
            End If

        Catch ex As Exception
            ' write error log
            objLog.ErrorLog("UpdateWorkingCategory", ex.Message.ToString, Session("UserName"))
        End Try
    End Sub
#End Region

End Class
