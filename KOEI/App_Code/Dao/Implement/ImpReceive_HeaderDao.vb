#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IReceive_HeaderDao
'	Class Discription	: Class of table receive_header
'	Create User 		: Boon
'	Create Date		    : 13-05-2013
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
    Public Class ImpReceive_HeaderDao
        Implements IReceive_HeaderDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckReceiveByVendor
        '	Discription	    : Check Receive_Header by vendor
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 20-05-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckReceiveByVendor(ByVal intVendor_id As Integer) As Boolean Implements IReceive_HeaderDao.DB_CheckReceiveByVendor
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckReceiveByVendor = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As receive_count ")
                    .AppendLine(" FROM receive_header ")
                    .AppendLine(" WHERE vendor_id = ?VendorId ")
                    ' assign parameter
                    objConn.AddParameter("?VendorId", intVendor_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckReceiveByVendor = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckReceiveByVendor = False
                objLog.ErrorLog("DB_CheckReceiveByVendor(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckReceiveByVendor(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try
        End Function
    End Class
End Namespace

