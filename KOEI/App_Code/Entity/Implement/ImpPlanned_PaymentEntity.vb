#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpPlanned_PaymentEntity
'	Class Discription	: Class of Planned Payment Report
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

#Region "Imports"
Imports Microsoft.VisualBasic
#End Region

Namespace Entity
    Public Class ImpPlanned_PaymentEntity
        Implements IPlanned_PaymentEntity

        'Receive data for search screen
        Private _min_year As String
        Private _max_year As String

        'Receive data for excel report
        Private _job_order_id As Integer
        Private _job_order As String
        Private _customer As String
        Private _description As String
        Private _receive_header_id As Integer
        Private _invoice_no As String
        Private _pay_date As String
        Private _amount_thb As String
        Private _sum_amount_thb As String
        Private _max_pay_date As String

        Private objPlannedPaymentDao As New Dao.ImpPlanned_PaymentDao

#Region "Function"
        Public Function GetYearList() As System.Collections.Generic.List(Of IPlanned_PaymentEntity) Implements IPlanned_PaymentEntity.GetYearList
            Return objPlannedPaymentDao.GetYearList()
        End Function

        Public Function GetAmountThbForReport(ByVal intYear As Integer) As System.Collections.Generic.List(Of ImpPlanned_PaymentEntity) Implements IPlanned_PaymentEntity.GetAmountThbForReport
            Return objPlannedPaymentDao.GetAmountThbForReport(intYear)
        End Function

        Public Function GetInvoiceForReport(ByVal intYear As Integer) As System.Collections.Generic.List(Of ImpPlanned_PaymentEntity) Implements IPlanned_PaymentEntity.GetInvoiceForReport
            Return objPlannedPaymentDao.GetInvoiceForReport(intYear)
        End Function

        Public Function GetJobOrderForReport(ByVal intYear As Integer) As System.Collections.Generic.List(Of ImpPlanned_PaymentEntity) Implements IPlanned_PaymentEntity.GetJobOrderForReport
            Return objPlannedPaymentDao.GetJobOrderForReport(intYear)
        End Function

        Public Function GetMaxPayDateForReport(ByVal intYear As Integer) As IPlanned_PaymentEntity Implements IPlanned_PaymentEntity.GetMaxPayDateForReport
            Return objPlannedPaymentDao.GetMaxPayDateForReport(intYear)
        End Function

        Public Function GetSumAmountThbForReport(ByVal intYear As Integer) As System.Collections.Generic.List(Of ImpPlanned_PaymentEntity) Implements IPlanned_PaymentEntity.GetSumAmountThbForReport
            Return objPlannedPaymentDao.GetSumAmountThbForReport(intYear)
        End Function
#End Region

#Region "Property"
        Public Property max_pay_date() As String Implements IPlanned_PaymentEntity.max_pay_date
            Get
                Return _max_pay_date
            End Get
            Set(ByVal value As String)
                _max_pay_date = value
            End Set
        End Property

        Public Property max_year() As String Implements IPlanned_PaymentEntity.max_year
            Get
                Return _max_year
            End Get
            Set(ByVal value As String)
                _max_year = value
            End Set
        End Property

        Public Property min_year() As String Implements IPlanned_PaymentEntity.min_year
            Get
                Return _min_year
            End Get
            Set(ByVal value As String)
                _min_year = value
            End Set
        End Property

        Public Property amount_thb() As String Implements IPlanned_PaymentEntity.amount_thb
            Get
                Return _amount_thb
            End Get
            Set(ByVal value As String)
                _amount_thb = value
            End Set
        End Property

        Public Property customer() As String Implements IPlanned_PaymentEntity.customer
            Get
                Return _customer
            End Get
            Set(ByVal value As String)
                _customer = value
            End Set
        End Property

        Public Property description() As String Implements IPlanned_PaymentEntity.description
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property invoice_no() As String Implements IPlanned_PaymentEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property

        Public Property job_order() As String Implements IPlanned_PaymentEntity.job_order
            Get
                Return _job_order
            End Get
            Set(ByVal value As String)
                _job_order = value
            End Set
        End Property

        Public Property job_order_id() As Integer Implements IPlanned_PaymentEntity.job_order_id
            Get
                Return _job_order_id
            End Get
            Set(ByVal value As Integer)
                _job_order_id = value
            End Set
        End Property

        Public Property pay_date() As String Implements IPlanned_PaymentEntity.pay_date
            Get
                Return _pay_date
            End Get
            Set(ByVal value As String)
                _pay_date = value
            End Set
        End Property

        Public Property receive_header_id() As Integer Implements IPlanned_PaymentEntity.receive_header_id
            Get
                Return _receive_header_id
            End Get
            Set(ByVal value As Integer)
                _receive_header_id = value
            End Set
        End Property

        Public Property sum_amount_thb() As String Implements IPlanned_PaymentEntity.sum_amount_thb
            Get
                Return _sum_amount_thb
            End Get
            Set(ByVal value As String)
                _sum_amount_thb = value
            End Set
        End Property

#End Region

    End Class
End Namespace
