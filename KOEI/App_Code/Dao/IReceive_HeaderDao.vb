#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dao
'	Class Name		    : IReceive_HeaderDao
'	Class Discription	: Interface of table receive_header
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
    Public Interface IReceive_HeaderDao
        Function DB_CheckReceiveByVendor(ByVal intVendor_id As Integer) As Boolean
    End Interface
End Namespace

