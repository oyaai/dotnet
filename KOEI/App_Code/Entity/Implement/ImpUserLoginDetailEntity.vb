#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpUserLoginDetailEntity
'	Class Discription	: Class of UserLogin detail
'	Create User 		: Nisa S.
'	Create Date		    : 10-07-2013
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

    Public Class ImpUserLoginDetailEntity
        Inherits Entity.ImpUserLoginEntity

        Private _department_name As String = Nothing
        Private _inuse As Integer = Nothing

#Region "Property"
        Property department_name() As String
            Get
                Return _department_name
            End Get
            Set(ByVal value As String)
                _department_name = value
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