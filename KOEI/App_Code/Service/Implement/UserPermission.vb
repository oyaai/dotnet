Imports Microsoft.VisualBasic

Public Class UserPermission
    Private cLog As New Common.Logs.Log

#Region "Constance"
    ' constance menu id
    Private Const JobOrder As Integer = 1
    Private Const SpecialJobOrder As Integer = 2
    Private Const InvoiceJobOrder As Integer = 3
    Private Const IncomeJobOrder As Integer = 4
    Private Const FinishedGoods As Integer = 5
    Private Const DeletedJobOrderManagement As Integer = 6
    Private Const PurchaseRequest As Integer = 7
    Private Const InvoicePurchase As Integer = 8
    Private Const VenderRating As Integer = 9
    Private Const InputCheque As Integer = 10
    Private Const PurchaseHistoryReport As Integer = 11
    Private Const Accounting As Integer = 12
    Private Const IncomePurchase As Integer = 13
    Private Const Payment As Integer = 14
    Private Const WitholdingTax As Integer = 15
    Private Const CostTableOverview As Integer = 16
    Private Const CostTableDetailed As Integer = 17
    Private Const PurchaseApprove As Integer = 18
    Private Const AccountingApprove As Integer = 19
    Private Const StockIn As Integer = 20
    Private Const StockOut As Integer = 21
    Private Const StockAdjust As Integer = 22
    Private Const WorkingHour As Integer = 23
    Private Const WorkingHourReport As Integer = 24
    Private Const Vendor As Integer = 25
    Private Const Item As Integer = 26
    Private Const IE As Integer = 27
    Private Const VAT As Integer = 28
    Private Const WT As Integer = 29
    Private Const Unit As Integer = 30
    Private Const PaymentTerm As Integer = 31
    Private Const JobOrderType As Integer = 32
    Private Const Currency As Integer = 33
    Private Const WorkingCategory As Integer = 34
    Private Const Country As Integer = 35
    Private Const Department As Integer = 36
    Private Const UserLogin As Integer = 37
    Private Const UserPermission As Integer = 38

    Private Const actCreate As String = "actCreate" 'เพิ่มข้อมูล        
    Private Const actUpdate As String = "actUpdate" 'แก้ไขข้อมูล
    Private Const actDelete As String = "actDelete" 'ลบข้อมูล
    Private Const actList As String = "actList" 'ค้นหา, ดูข้อมูล, report
    Private Const actConfirm As String = "actConfirm" 'ยืนยัน
    Private Const actApprove As String = "actApprove" 'อนุมัติ
    Private Const actAmount As String = "actAmount" 'ดูราคา
#End Region
    

#Region "Function"
    '/**************************************************************
    '	Function name	: CheckPermission(MenuId, ActionName)
    '	Discription		: Check permission of button
    '	Return Value	: Boolean
    '	Create User		: Komsan L.
    '	Create Date		: 2013-06-03
    '	Update User		:
    '	Update Date		:
    '**************************************************************/
    Public Function CheckPermission(ByVal MenuId As Integer, ByVal ActionName As String) As Boolean
        ' set defualt of return value
        CheckPermission = False
        Try
            ' variable keep list of permission
            Dim listPermission As New List(Of Dto.UserPermissionDto)
            Dim dtoPermission As New Dto.UserPermissionDto

            ' check nothing of session(ListPermission)
            If Not IsNothing(HttpContext.Current.Session("ListPermission")) Then
                ' assign value to variable
                listPermission = HttpContext.Current.Session("ListPermission")
                ' get item by menu id
                dtoPermission = listPermission.Find(Function(up As Dto.UserPermissionDto) up.MenuID = MenuId)

                ' check permission of function 
                If ActionName = "actCreate" AndAlso dtoPermission.Fn_Create = 1 Then Return True
                If ActionName = "actUpdate" AndAlso dtoPermission.Fn_Update = 1 Then Return True
                If ActionName = "actDelete" AndAlso dtoPermission.Fn_Delete = 1 Then Return True
                If ActionName = "actList" AndAlso dtoPermission.Fn_List = 1 Then Return True
                If ActionName = "actConfirm" AndAlso dtoPermission.Fn_Confirm = 1 Then Return True
                If ActionName = "actApprove" AndAlso dtoPermission.Fn_Approve = 1 Then Return True
                If ActionName = "actAmount" AndAlso dtoPermission.Fn_Amount = 1 Then Return True
            End If

        Catch ex As Exception
            ' write error log
            cLog.ErrorLog("CheckPermission", ex.Message.ToString, HttpContext.Current.Session("UserName"))
        End Try
    End Function
#End Region
End Class
