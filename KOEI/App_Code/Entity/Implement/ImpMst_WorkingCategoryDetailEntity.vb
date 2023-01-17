#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_WorkingCategoryDetailEntity
'	Class Discription	: Class of WorkingCategory detail
'	Create User 		: Pranitda Sroengklang
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
    Public Class ImpMst_WorkingCategoryDetailEntity
        Inherits Entity.ImpMst_WorkingCategoryEntity

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

