#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : ImpPayment_HeaderDao
'	Class Discription	: Class of table payment_header
'	Create User 		: Boonyarit
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
    Public Class ImpPayment_HeaderDao
        Implements IPayment_HeaderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckPaymentByPurchase
        '	Discription	    : Check data payment by purchase
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 14-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckPaymentByPurchase(ByVal intPurchase_id As Integer) As Boolean Implements IPayment_HeaderDao.DB_CheckPaymentByPurchase
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckPaymentByPurchase = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine("SELECT Count(*) As payment_count")
                    .AppendLine("FROM payment_header")
                    .AppendLine("WHERE status_id<>6 and po_header_id = ?PurchaseId;")
                    ' assign parameter
                    objConn.AddParameter("?PurchaseId", intPurchase_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckPaymentByPurchase = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckPaymentByPurchase = False
                objLog.ErrorLog("DB_CheckPaymentByPurchase(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckPaymentByPurchase(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace

