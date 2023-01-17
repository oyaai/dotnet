'Imports Microsoft.VisualBasic

'Public Class ImpRating_PurchaseEntity

'End Class
#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpRating_PurchaseEntity
'	Class Discription	: Class of table accounting
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
Imports System.Data
#End Region

Namespace Entity
    Public Class ImpRating_PurchaseEntity
        Implements IRating_PurchaseEntity


        'Receive value from screen search
        Private _strSearchType As String
        Private _strPO As String
        Private _strDeliveryDateFrom As String
        Private _strDeliveryDateTo As String
        Private _strPaymentDateFrom As String
        Private _strPaymentDateTo As String
        Private _strVendor_name As String
        Private _strRating_start As String
        Private _strRating_end As String
        Private _strInvoce_no As String

        'item from database vendor_rating
        Private _id As String
        Private _payment_header_id As String
        Private _quality As String
        Private _delivery As String
        Private _service As String
        Private _created_by As String
        Private _created_date As String
        Private _updated_by As String
        Private _updated_date As String

        'item from database
        Private _invoice_no As String
        Private _po_no As String
        Private _payment_date As String
        Private _delivery_date As String
        Private _score As String

        Private objInvPurchase As New Dao.ImpRating_PurchaseDao

#Region "Function"
        Public Function GetRating_PurchaseList( _
            ByVal objRatingEnt As IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseEntity.GetRating_PurchaseList
            Return objInvPurchase.GetRating_PurchaseList(objRatingEnt)
        End Function
        Public Function GetVendorRatingReport( _
            ByVal objRatingEnt As IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseEntity.GetVendorRatingReport
            Return objInvPurchase.GetVendorRatingReport(objRatingEnt)
        End Function
        Public Function GetYearVendorRatingReport( _
            ByVal objRatingEnt As IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseEntity.GetYearVendorRatingReport
            Return objInvPurchase.GetYearVendorRatingReport(objRatingEnt)
        End Function
        Public Function DeleteRatingInvoice( _
            ByVal intRatingId As Integer _
        ) As Integer Implements IRating_PurchaseEntity.DeleteRatingInvoice
            Return objInvPurchase.DeleteRatingInvoice(intRatingId)
        End Function
        Public Function GetPurchaseList( _
            ByVal objRatingEnt As IRating_PurchaseEntity _
        ) As System.Collections.Generic.List(Of ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseEntity.GetPurchaseList
            Return objInvPurchase.GetPurchaseList(objRatingEnt)
        End Function
        Public Function GetRatingVendor( _
            ByVal ratingId As String, _
            ByVal payment_header_id As String _
        ) As System.Collections.Generic.List(Of ImpRating_PurchaseDetailEntity) Implements IRating_PurchaseEntity.GetRatingVendor
            Return objInvPurchase.GetRatingVendor(ratingId, payment_header_id)
        End Function
        Public Function InsUpdVendor_Rating( _
            ByVal mode As String, _
            ByVal strId As String, _
            ByVal strPayment_header_id As String, _
            ByVal strQuality As String, _
            ByVal strDelivery As String, _
            ByVal strService As String _
        ) As Integer Implements IRating_PurchaseEntity.InsUpdVendor_Rating
            Return objInvPurchase.InsUpdVendor_Rating(mode, strId, strPayment_header_id, strQuality, strDelivery, strService)
        End Function
#End Region

#Region "Property"
        Public Property strInvoce_no() As String Implements IRating_PurchaseEntity.strInvoce_no
            Get
                Return _strInvoce_no
            End Get
            Set(ByVal value As String)
                _strInvoce_no = value
            End Set
        End Property
        Public Property strSearchType() As String Implements IRating_PurchaseEntity.strSearchType
            Get
                Return _strSearchType
            End Get
            Set(ByVal value As String)
                _strSearchType = value
            End Set
        End Property
        Public Property strPO() As String Implements IRating_PurchaseEntity.strPO
            Get
                Return _strPO
            End Get
            Set(ByVal value As String)
                _strPO = value
            End Set
        End Property
        Public Property strDeliveryDateFrom() As String Implements IRating_PurchaseEntity.strDeliveryDateFrom
            Get
                Return _strDeliveryDateFrom
            End Get
            Set(ByVal value As String)
                _strDeliveryDateFrom = value
            End Set
        End Property
        Public Property strDeliveryDateTo() As String Implements IRating_PurchaseEntity.strDeliveryDateTo
            Get
                Return _strDeliveryDateTo
            End Get
            Set(ByVal value As String)
                _strDeliveryDateTo = value
            End Set
        End Property
        Public Property strPaymentDateFrom() As String Implements IRating_PurchaseEntity.strPaymentDateFrom
            Get
                Return _strPaymentDateFrom
            End Get
            Set(ByVal value As String)
                _strPaymentDateFrom = value
            End Set
        End Property
        Public Property strPaymentDateTo() As String Implements IRating_PurchaseEntity.strPaymentDateTo
            Get
                Return _strPaymentDateTo
            End Get
            Set(ByVal value As String)
                _strPaymentDateTo = value
            End Set
        End Property
        Public Property strVendor_name() As String Implements IRating_PurchaseEntity.strVendor_name
            Get
                Return _strVendor_name
            End Get
            Set(ByVal value As String)
                _strVendor_name = value
            End Set
        End Property
        Public Property strRating_start() As String Implements IRating_PurchaseEntity.strRating_start
            Get
                Return _strRating_start
            End Get
            Set(ByVal value As String)
                _strRating_start = value
            End Set
        End Property
        Public Property strRating_end() As String Implements IRating_PurchaseEntity.strRating_end
            Get
                Return _strRating_end
            End Get
            Set(ByVal value As String)
                _strRating_end = value
            End Set
        End Property
        Public Property id() As String Implements IRating_PurchaseEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property payment_header_id() As String Implements IRating_PurchaseEntity.payment_header_id
            Get
                Return _payment_header_id
            End Get
            Set(ByVal value As String)
                _payment_header_id = value
            End Set
        End Property
        Public Property quality() As String Implements IRating_PurchaseEntity.quality
            Get
                Return _quality
            End Get
            Set(ByVal value As String)
                _quality = value
            End Set
        End Property
        Public Property delivery() As String Implements IRating_PurchaseEntity.delivery
            Get
                Return _delivery
            End Get
            Set(ByVal value As String)
                _delivery = value
            End Set
        End Property
        Public Property service() As String Implements IRating_PurchaseEntity.service
            Get
                Return _service
            End Get
            Set(ByVal value As String)
                _service = value
            End Set
        End Property
        Public Property created_by() As String Implements IRating_PurchaseEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As String)
                _created_by = value
            End Set
        End Property
        Public Property created_date() As String Implements IRating_PurchaseEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As String)
                _created_date = value
            End Set
        End Property
        Public Property updated_by() As String Implements IRating_PurchaseEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As String)
                _updated_by = value
            End Set
        End Property
        Public Property updated_date() As String Implements IRating_PurchaseEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As String)
                _updated_date = value
            End Set
        End Property
        Public Property invoice_no() As String Implements IRating_PurchaseEntity.invoice_no
            Get
                Return _invoice_no
            End Get
            Set(ByVal value As String)
                _invoice_no = value
            End Set
        End Property
        Public Property po_no() As String Implements IRating_PurchaseEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property
        Public Property payment_date() As String Implements IRating_PurchaseEntity.payment_date
            Get
                Return _payment_date
            End Get
            Set(ByVal value As String)
                _payment_date = value
            End Set
        End Property
        Public Property delivery_date() As String Implements IRating_PurchaseEntity.delivery_date
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As String)
                _delivery_date = value
            End Set
        End Property
        Public Property score() As String Implements IRating_PurchaseEntity.score
            Get
                Return _score
            End Get
            Set(ByVal value As String)
                _score = value
            End Set
        End Property

#End Region
    End Class
End Namespace


