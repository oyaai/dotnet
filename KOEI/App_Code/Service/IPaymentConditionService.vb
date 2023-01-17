#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPaymentConditionService
'	Class Discription	: Interface class payment condition service
'	Create User 		: Suwishaya L.
'	Create Date		    : 17-06-2013
'
' UPDATE INFORMATION
'	Update User		: Wasan D.
'	Update Date		: 03-07-2013	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Imports"
Imports Microsoft.VisualBasic
Imports System.Data
#End Region


Namespace Service
    Public Interface IPaymentConditionService
        Function GetPaymentConditionForList() As List(Of Dto.PaymentConditionDto)

        'Update by Wasan D. On 02-07-2013
        Function GetPaymentCondList(ByVal strFirst As String, ByVal strSecond As String, ByVal strThird As String) As DataTable
        'Update by Wasan D. On 03-07-2013
        Function DeletePaymentCond(ByVal intPayID As Integer) As Boolean
        Function GetPaymentCondByID(ByVal intPayID As Integer) As Dto.PaymentConditionDto
        Function InsertPaymentCond(ByVal objPayDto As Dto.PaymentConditionDto, Optional ByRef strMsg As String = "") As Boolean
        Function UpdatePaymentCond(ByVal objPayDto As Dto.PaymentConditionDto, Optional ByRef strMsg As String = "") As Boolean
        Function IsUsedInPO(ByVal intPayID As Integer) As Integer
        'End Update by Wasan

    End Interface
End Namespace
