#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IPo_DetailDao
'	Class Discription	: Class of table po_detail
'	Create User 		: Boon
'	Create Date		    : 04-06-2013
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
    Public Class ImpPo_DetailDao
        Implements IPo_DetailDao

        Private objConn As Common.DBConnection.MySQLAccess
        Private objLog As New Common.Logs.Log
        Private strMsgErr As String = String.Empty

        '/**************************************************************
        '	Function name	: DB_CheckPoByUnit
        '	Discription	    : Check po_detail by unit
        '	Return Value	: Boolean
        '	Create User	    : Boonyarit
        '	Create Date	    : 04-06-2013
        '	Update User	    :
        '	Update Date	    :
        '*************************************************************/
        Public Function DB_CheckPoByUnit(ByVal intUnit_id As Integer) As Boolean Implements IPo_DetailDao.DB_CheckPoByUnit
            Dim strSQL As New Text.StringBuilder
            Try
                ' variable
                Dim intFlagCount As Integer = 0

                DB_CheckPoByUnit = False

                ' set new object
                objConn = New Common.DBConnection.MySQLAccess
                strSQL = New Text.StringBuilder

                ' assign sql statement
                With strSQL
                    .AppendLine(" SELECT Count(*) As po_count ")
                    .AppendLine(" FROM po_detail ")
                    .AppendLine(" WHERE unit_id = ?UnitId ")
                    ' assign parameter
                    objConn.AddParameter("?UnitId", intUnit_id)
                End With

                ' execute by scalar
                intFlagCount = objConn.ExecuteScalar(strSQL.ToString)
                strMsgErr = objConn.MessageError
                ' check data
                If intFlagCount > 0 Then
                    DB_CheckPoByUnit = True
                End If

            Catch ex As Exception
                ' write error log
                DB_CheckPoByUnit = False
                objLog.ErrorLog("DB_CheckPoByUnit(Dao)", ex.Message.Trim, HttpContext.Current.Session("UserName"))
                ' write sql statement
                objLog.InfoLog("DB_CheckPoByUnit(Dao)", strSQL.ToString, HttpContext.Current.Session("UserName"))
            Finally
                If Not objConn Is Nothing Then objConn.Close()
            End Try

        End Function
    End Class
End Namespace

