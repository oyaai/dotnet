#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Service
'	Class Name		    : IPaymentTermService
'	Class Discription	: Interface of PaymentTerm
'	Create User 		: Boonyarit
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
#End Region

Namespace Service
    Public Interface IPaymentTermService
        Function GetPaymentList(ByVal strPayment As String) As DataTable
        Function SetListPaymentDay(ByRef objValue As DropDownList) As Boolean
        Function IsUsedInPO(ByVal intPaymentTermID As Integer) As Boolean
        Function IsUsedInPO2(ByVal intPaymentTermID As Integer) As Boolean
        Function DeletePaymentTerm(ByVal intPaymentTermID As Integer) As Boolean
        Function InsertPaymentTerm(ByVal objPaymentTermDto As Dto.IPayment_TermDto, Optional ByRef strMsg As String = "") As Boolean
        Function GetPaymentTermByID(ByVal intPaymentTermID As Integer) As Dto.IPayment_TermDto
        Function CheckDupInsert(ByVal intPayment As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function CheckDupUpdate(ByVal intPaymentTermID As String, ByVal intPayment As String, ByVal mode As String, Optional ByRef strMsg As String = "") As Boolean
        Function UpdatePaymentTerm(ByVal objPaymentTermDto As Dto.IPayment_TermDto, Optional ByRef strMsg As String = "") As Boolean
        'Get data for drobdrown list
        Function GetPaymentDayForList() As List(Of Dto.IPayment_TermDto)

       
    End Interface
End Namespace

