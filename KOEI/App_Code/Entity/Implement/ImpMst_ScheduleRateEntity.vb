#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_ScheduleRateEntity
'	Class Discription	: Class of table Mst_Schedule_Rate
'	Create User 		: Boon
'	Create Date		    : 02-07-2013
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
    Public Class ImpMst_ScheduleRateEntity
        Implements IMst_ScheduleRateEntity

        Private _id As Integer
        Private _currency_id As Integer
        Private _ef_date As DateString
        Private _rate As Decimal
        Private _delete_fg As Integer
        Private _created_by As Integer
        Private _created_date As DateString
        Private _updated_by As Integer
        Private _updated_date As DateString
        Private _currency As String

        Private objScheduleRateDao As New Dao.ImpMst_ScheduleRateDao

        Public Function GetScheduleRateByPurchase(ByVal intCurrency_id As Integer, Optional ByVal strDate As String = "") As Decimal Implements IMst_ScheduleRateEntity.GetScheduleRateByPurchase
            Return objScheduleRateDao.DB_GetScheduleRateByPurchase(intCurrency_id, strDate)
        End Function

        Public Function GetScheduleRateByCurrency(ByVal intCurrency_id As Integer, Optional ByVal strEFDate As String = "", Optional ByVal intRanking As Integer = 11) As System.Collections.Generic.List(Of IMst_ScheduleRateEntity) Implements IMst_ScheduleRateEntity.GetScheduleRateByCurrency
            Return objScheduleRateDao.DB_GetScheduleRateByCurrency(intCurrency_id, strEFDate, intRanking)
        End Function

        Public Function CancelScheduleRate(ByVal intScheduleRate_id As Integer) As Boolean Implements IMst_ScheduleRateEntity.CancelScheduleRate
            Return objScheduleRateDao.DB_CancelScheduleRate(intScheduleRate_id)
        End Function

        Public Function CheckIsDupScheduleRate(ByVal intCurrency_id As Integer, ByVal strEF_date As String, Optional ByVal intScheduleRate_id As Integer = 0) As Boolean Implements IMst_ScheduleRateEntity.CheckIsDupScheduleRate
            Return objScheduleRateDao.DB_CheckIsDupScheduleRate(intCurrency_id, strEF_date, intScheduleRate_id)
        End Function

        Public Function GetScheduleRateById(ByVal intScheduleRate_id As Integer) As IMst_ScheduleRateEntity Implements IMst_ScheduleRateEntity.GetScheduleRateById
            Return objScheduleRateDao.DB_GetScheduleRateById(intScheduleRate_id)
        End Function

        Public Function InsertScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer Implements IMst_ScheduleRateEntity.InsertScheduleRate
            Return objScheduleRateDao.DB_InsertScheduleRate(objScheduleRate)
        End Function

        Public Function UpdateScheduleRate(ByVal objScheduleRate As IMst_ScheduleRateEntity) As Integer Implements IMst_ScheduleRateEntity.UpdateScheduleRate
            Return objScheduleRateDao.DB_UpdateScheduleRate(objScheduleRate)
        End Function

#Region "Property"
        Public Property updated_date() As DateString Implements IMst_ScheduleRateEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As DateString)
                _updated_date = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IMst_ScheduleRateEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property created_date() As DateString Implements IMst_ScheduleRateEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As DateString)
                _created_date = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IMst_ScheduleRateEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property delete_fg() As Integer Implements IMst_ScheduleRateEntity.delete_fg
            Get
                Return _delete_fg
            End Get
            Set(ByVal value As Integer)
                _delete_fg = value
            End Set
        End Property

        Public Property rate() As Decimal Implements IMst_ScheduleRateEntity.rate
            Get
                Return _rate
            End Get
            Set(ByVal value As Decimal)
                _rate = value
            End Set
        End Property

        Public Property ef_date() As DateString Implements IMst_ScheduleRateEntity.ef_date
            Get
                Return _ef_date
            End Get
            Set(ByVal value As DateString)
                _ef_date = value
            End Set
        End Property

        Public Property currency_id() As Integer Implements IMst_ScheduleRateEntity.currency_id
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property id() As Integer Implements IMst_ScheduleRateEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property currency() As String Implements IMst_ScheduleRateEntity.currency
            Get
                Return _currency
            End Get
            Set(ByVal value As String)
                _currency = value
            End Set
        End Property

#End Region

    End Class
End Namespace

