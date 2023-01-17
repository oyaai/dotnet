#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_ItemDetailEntity
'	Class Discription	: Class of Item detail
'	Create User 		: Komsan L.
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

Imports Microsoft.VisualBasic

Namespace Entity
    Public Class ImpMst_ItemDetailEntity
        Inherits Entity.ImpMst_ItemEntity

        Private _vendor_name As String = Nothing
        Private _inuse As Integer = Nothing

#Region "Property"
        Property vendor_name() As String
            Get
                Return _vendor_name
            End Get
            Set(ByVal value As String)
                _vendor_name = value
            End Set
        End Property

        Property inuse() As Integer
            Get
                Return _inuse
            End Get
            Set(ByVal value As Integer)
                _inuse = value
            End Set
        End Property
#End Region

    End Class
End Namespace

