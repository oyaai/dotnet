#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpVatDetailEntity
'	Class Discription	: Class of Vat detail
'	Create User 		: Nisa S.
'	Create Date		    : 25-06-2013
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
Imports Entity
#End Region

Namespace Entity
    Public Class ImpVatDetailEntity
        Inherits ImpVatEntity

        
        Private _inuse As Integer = Nothing

#Region "Property"
        
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