#Region "History"
'******************************************************************
' Copyright KOEI Co., Ltd.
'
' INITIAL INFOMATION
'	Package Name	    : Entity
'	Class Name		    : IMaterialEntity
'	Class Discription	: Interface of table stock
'	Create User 		: Suwishaya L.
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

Namespace Entity
    Public Interface IMaterialEntity

#Region "Property"
        'Receive data from screen (condition search)
        Property job_order() As String
        Property vendor() As String
        Property invoice_no() As String
        Property po_no() As String
        Property item_name() As String
        Property delivery_date_from() As String
        Property delivery_date_to() As String

        'Receive data for detail search and report       
        Property id() As Integer
        Property amount() As String 
        Property delivery_date_in() As String
        Property qty_in() As String
        Property delivery_date_out() As String
        Property qty_out() As String
        Property qty_left() As String
        Property sum_amount() As String

#End Region

#Region "Function"
        Function GetMaterialList(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)
        Function GetMaterialListReport(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)
        Function GetSumMaterialListReport(ByVal objMaterialEnt As Entity.IMaterialEntity) As List(Of Entity.ImpMaterialEntity)

#End Region

    End Interface
End Namespace
