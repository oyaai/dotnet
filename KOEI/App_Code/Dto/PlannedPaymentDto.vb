#Region "History"
'******************************************************************
' Copyright Konishi Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Dto
'	Class Name		    : PlannedPaymentDto
'	Class Discription	: Dto class Planned Payment Report
'	Create User 		: Suwishaya L.
'	Create Date		    : 05-08-2013
'
' UPDATE INFORMATION
'	Update User		:
'	Update Date		:	
'	Update User		:
'	Update Date		:
'
'******************************************************************/
#End Region

#Region "Import"
Imports Microsoft.VisualBasic
#End Region

Namespace Dto
    Public Class PlannedPaymentDto
 
        Private _year As String
        Private _min_year As String
        Private _max_year As String
        Private _max_pay_date As String

#Region "Property"
        Public Property max_pay_date() As String
            Get
                Return _max_pay_date
            End Get
            Set(ByVal value As String)
                _max_pay_date = value
            End Set
        End Property

        Public Property year() As String
            Get
                Return _year
            End Get
            Set(ByVal value As String)
                _year = value
            End Set
        End Property

        Public Property min_year() As String
            Get
                Return _min_year
            End Get
            Set(ByVal value As String)
                _min_year = value
            End Set
        End Property

        Public Property max_year() As String
            Get
                Return _max_year
            End Get
            Set(ByVal value As String)
                _max_year = value
            End Set
        End Property
#End Region
    End Class
End Namespace
