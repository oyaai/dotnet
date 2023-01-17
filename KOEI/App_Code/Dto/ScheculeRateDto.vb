#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : ScheculeRateDto
'	Class Discription	: Dto class ScheculeRate
'	Create User 		: Boonyarit
'	Create Date		    : 03-07-2013
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

Namespace Dto
    Public Class ScheculeRateDto
        Private _id As Integer
        Private _currency_id As Integer
        Private _currency As String
        Private _ef_date As String
        Private _rate As Decimal

#Region "Property"
        Public Property rate() As Decimal
            Get
                Return _rate
            End Get
            Set(ByVal value As Decimal)
                _rate = value
            End Set
        End Property

        Public Property ef_date() As String
            Get
                Return _ef_date
            End Get
            Set(ByVal value As String)
                _ef_date = value
            End Set
        End Property

        Public Property currency() As String
            Get
                Return _currency
            End Get
            Set(ByVal value As String)
                _currency = value
            End Set
        End Property

        Public Property currency_id() As Integer
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
#End Region

    End Class
End Namespace

