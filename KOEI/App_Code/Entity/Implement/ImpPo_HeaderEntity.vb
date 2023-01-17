#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : ImpMst_VendorEntity
'	Class Discription	: Class of table po_header
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
#End Region

Namespace Entity
    Public Class ImpPo_HeaderEntity
        Implements IPo_HeaderEntity

#Region "Fields"
        Private _id As Integer
        Private _po_no As String
        Private _po_type As Integer
        Private _vendor_id As Integer
        Private _vendor_branch_id As Integer
        Private _quotation_no As String
        Private _issue_date As DateString
        Private _delivery_date As DateString
        Private _payment_term_id As Integer
        Private _vat_id As Integer
        Private _wt_id As Integer
        Private _currency_id As Integer
        Private _remark As String
        Private _discount_total As Decimal
        Private _sub_total As Decimal
        Private _vat_amount As Decimal
        Private _wt_amount As Decimal
        Private _total_amount As Decimal
        Private _attn As String
        Private _deliver_to As String
        Private _contact As String
        Private _user_id As Integer
        Private _status_id As Integer
        Private _created_by As Integer
        Private _created_date As DateString
        Private _updated_by As Integer
        Private _updated_date As DateString

#End Region

        Private objPo As New Dao.ImpPo_HeaderDao

#Region "Function"
        Public Function InsertTaskPurchase(ByVal intPoId_New As Integer) As Integer Implements IPo_HeaderEntity.InsertTaskPurchase
            Return objPo.DB_InsertTaskPurchase(intPoId_New)
        End Function

        Public Function UpdateTaskPurchase(ByVal intPoId_New As Integer) As Integer Implements IPo_HeaderEntity.UpdateTaskPurchase
            Return objPo.DB_UpdateTaskPurchase(intPoId_New)
        End Function

        Public Function GetPoNo(Optional ByRef intPoNo As Integer = 0) As String Implements IPo_HeaderEntity.GetPoNo
            Return objPo.DB_GetPoNo(intPoNo)
        End Function

        Public Function InsertPurchase(ByVal objPurchase As ImpPurchaseEntity, Optional ByRef strPoNo_New As String = "", Optional ByRef intPoId_New As Integer = 0) As Integer Implements IPo_HeaderEntity.InsertPurchase
            Return objPo.DB_InsertPurchase(objPurchase, strPoNo_New, intPoId_New)
        End Function

        Public Function UpdatePurchase(ByRef objPurchase As ImpPurchaseEntity) As Integer Implements IPo_HeaderEntity.UpdatePurchase
            Return objPo.DB_UpdatePurchase(objPurchase)
        End Function
        Public Function ModifyPurchase(ByRef objPurchase As ImpPurchaseEntity) As Integer Implements IPo_HeaderEntity.ModifyPurchase
            Return objPo.DB_ModifyPurchase(objPurchase)
        End Function

        Public Function SearchPurchase(ByVal intPurchaseId As Integer) As ImpPurchaseEntity Implements IPo_HeaderEntity.SearchPurchase
            Return objPo.DB_SearchPurchase(intPurchaseId)
        End Function

        Public Function SearchPurchasePDF(ByVal intPurchaseId As Integer) As ImpPurchasePDFEntty Implements IPo_HeaderEntity.SearchPurchasePDF
            Return objPo.DB_SearchPurchasePDF(intPurchaseId)
        End Function

        Public Function SearchPurchaseReport(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As System.Collections.Generic.List(Of ImpPurchaseReportEntity) Implements IPo_HeaderEntity.SearchPurchaseReport
            Return objPo.DB_SearchPurchaseReport(objSearchPurchase)
        End Function

        Public Function SearchPurchaseDetail(ByVal intPurchaseId As Integer) As ImpPurchaseEntity Implements IPo_HeaderEntity.SearchPurchaseDetail
            Return objPo.DB_SearchPurchaseDetail(intPurchaseId)
        End Function

        Public Function DeletePurchase(ByVal intPurchaseId As Integer) As Boolean Implements IPo_HeaderEntity.DeletePurchase
            Return objPo.DB_DeletePurchase(intPurchaseId)
        End Function

        Public Function SearchPurchase(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As List(Of ImpPurchaseEntity) Implements IPo_HeaderEntity.SearchPurchase
            Return objPo.DB_SearchPurchase(objSearchPurchase)
        End Function

        Public Function CheckPoByVendor(ByVal intVendor_id As Integer) As Boolean Implements IPo_HeaderEntity.CheckPoByVendor
            Return objPo.DB_CheckPoByVendor(intVendor_id)
        End Function

        Public Function GetPurchaseApproveList(ByVal objSearchPurchase As Dto.PurchaseSearchDto) As System.Collections.Generic.List(Of ImpPurchaseEntity) Implements IPo_HeaderEntity.GetPurchaseApproveList
            Return objPo.GetPurchaseApproveList(objSearchPurchase)
        End Function

        Public Function UpdatePurchaseStatus(ByVal strPurchaseId As String, ByVal intStatus As Integer) As Integer Implements IPo_HeaderEntity.UpdatePurchaseStatus
            Return objPo.UpdatePurchaseStatus(strPurchaseId, intStatus)
        End Function

        Public Function GetPOApprove(ByVal strPoId As String) As System.Collections.Generic.List(Of IPo_HeaderEntity) Implements IPo_HeaderEntity.GetPOApprove
            Return objPo.GetPOApprove(strPoId)
        End Function

#End Region

#Region "Property"
        Public Property attn() As String Implements IPo_HeaderEntity.attn
            Get
                Return _attn
            End Get
            Set(ByVal value As String)
                _attn = value
            End Set
        End Property

        Public Property contact() As String Implements IPo_HeaderEntity.contact
            Get
                Return _contact
            End Get
            Set(ByVal value As String)
                _contact = value
            End Set
        End Property

        Public Property created_by() As Integer Implements IPo_HeaderEntity.created_by
            Get
                Return _created_by
            End Get
            Set(ByVal value As Integer)
                _created_by = value
            End Set
        End Property

        Public Property created_date() As DateString Implements IPo_HeaderEntity.created_date
            Get
                Return _created_date
            End Get
            Set(ByVal value As DateString)
                _created_date = value
            End Set
        End Property

        Public Property currency_id() As Integer Implements IPo_HeaderEntity.currency_id
            Get
                Return _currency_id
            End Get
            Set(ByVal value As Integer)
                _currency_id = value
            End Set
        End Property

        Public Property deliver_to() As String Implements IPo_HeaderEntity.deliver_to
            Get
                Return _deliver_to
            End Get
            Set(ByVal value As String)
                _deliver_to = value
            End Set
        End Property

        Public Property delivery_date() As DateString Implements IPo_HeaderEntity.delivery_date
            Get
                Return _delivery_date
            End Get
            Set(ByVal value As DateString)
                _delivery_date = value
            End Set
        End Property

        Public Property discount_total() As Decimal Implements IPo_HeaderEntity.discount_total
            Get
                Return _discount_total
            End Get
            Set(ByVal value As Decimal)
                _discount_total = value
            End Set
        End Property

        Public Property id() As Integer Implements IPo_HeaderEntity.id
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property issue_date() As DateString Implements IPo_HeaderEntity.issue_date
            Get
                Return _issue_date
            End Get
            Set(ByVal value As DateString)
                _issue_date = value
            End Set
        End Property

        Public Property payment_term_id() As Integer Implements IPo_HeaderEntity.payment_term_id
            Get
                Return _payment_term_id
            End Get
            Set(ByVal value As Integer)
                _payment_term_id = value
            End Set
        End Property

        Public Property po_no() As String Implements IPo_HeaderEntity.po_no
            Get
                Return _po_no
            End Get
            Set(ByVal value As String)
                _po_no = value
            End Set
        End Property

        Public Property po_type() As Integer Implements IPo_HeaderEntity.po_type
            Get
                Return _po_type
            End Get
            Set(ByVal value As Integer)
                _po_type = value
            End Set
        End Property

        Public Property quotation_no() As String Implements IPo_HeaderEntity.quotation_no
            Get
                Return _quotation_no
            End Get
            Set(ByVal value As String)
                _quotation_no = value
            End Set
        End Property

        Public Property remark() As String Implements IPo_HeaderEntity.remark
            Get
                Return _remark
            End Get
            Set(ByVal value As String)
                _remark = value
            End Set
        End Property

        Public Property status_id() As Integer Implements IPo_HeaderEntity.status_id
            Get
                Return _status_id
            End Get
            Set(ByVal value As Integer)
                _status_id = value
            End Set
        End Property

        Public Property sub_total() As Decimal Implements IPo_HeaderEntity.sub_total
            Get
                Return _sub_total
            End Get
            Set(ByVal value As Decimal)
                _sub_total = value
            End Set
        End Property

        Public Property total_amount() As Decimal Implements IPo_HeaderEntity.total_amount
            Get
                Return _total_amount
            End Get
            Set(ByVal value As Decimal)
                _total_amount = value
            End Set
        End Property

        Public Property updated_by() As Integer Implements IPo_HeaderEntity.updated_by
            Get
                Return _updated_by
            End Get
            Set(ByVal value As Integer)
                _updated_by = value
            End Set
        End Property

        Public Property updated_date() As DateString Implements IPo_HeaderEntity.updated_date
            Get
                Return _updated_date
            End Get
            Set(ByVal value As DateString)
                _updated_date = value
            End Set
        End Property

        Public Property user_id() As Integer Implements IPo_HeaderEntity.user_id
            Get
                Return _user_id
            End Get
            Set(ByVal value As Integer)
                _user_id = value
            End Set
        End Property

        Public Property vat_amount() As Decimal Implements IPo_HeaderEntity.vat_amount
            Get
                Return _vat_amount
            End Get
            Set(ByVal value As Decimal)
                _vat_amount = value
            End Set
        End Property

        Public Property vat_id() As Integer Implements IPo_HeaderEntity.vat_id
            Get
                Return _vat_id
            End Get
            Set(ByVal value As Integer)
                _vat_id = value
            End Set
        End Property

        Public Property vendor_id() As Integer Implements IPo_HeaderEntity.vendor_id
            Get
                Return _vendor_id
            End Get
            Set(ByVal value As Integer)
                _vendor_id = value
            End Set
        End Property

        Public Property vendor_branch_id() As Integer Implements IPo_HeaderEntity.vendor_branch_id
            Get
                Return _vendor_branch_id
            End Get
            Set(ByVal value As Integer)
                _vendor_branch_id = value
            End Set
        End Property

        Public Property wt_amount() As Decimal Implements IPo_HeaderEntity.wt_amount
            Get
                Return _wt_amount
            End Get
            Set(ByVal value As Decimal)
                _wt_amount = value
            End Set
        End Property

        Public Property wt_id() As Integer Implements IPo_HeaderEntity.wt_id
            Get
                Return _wt_id
            End Get
            Set(ByVal value As Integer)
                _wt_id = value
            End Set
        End Property

#End Region

    End Class
End Namespace





