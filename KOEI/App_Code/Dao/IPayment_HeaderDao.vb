#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IPayment_HeaderDao
'	Class Discription	: Interface of table payment_header
'	Create User 		: Boon
'	Create Date		    : 14-06-2013
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
#End Region

Namespace Dao
    Public Interface IPayment_HeaderDao
        Function DB_CheckPaymentByPurchase(ByVal intPurchase_id As Integer) As Boolean
    End Interface
End Namespace

