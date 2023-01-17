#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_CountryDetailEntity
'	Class Discription	: Class of Country detail
'	Create User 		: Suwishaya L.
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

Namespace Entity
    Public Class ImpMst_CountryDetailEntity
        Inherits Entity.ImpMst_CountryEntity

        Private _country_name As String = Nothing

#Region "Property"
        Property country_name() As String
            Get
                Return _country_name
            End Get
            Set(ByVal value As String)
                _country_name = value
            End Set
        End Property
#End Region

    End Class
End Namespace
