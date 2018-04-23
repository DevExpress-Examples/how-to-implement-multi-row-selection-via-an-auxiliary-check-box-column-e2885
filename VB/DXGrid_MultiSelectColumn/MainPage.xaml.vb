Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Controls
Imports DevExpress.Xpf.Grid

Namespace DXGrid_MultiSelectColumn
	Partial Public Class MainPage
		Inherits UserControl
		Private list As List(Of TestData)
		Private selectedValues As New Dictionary(Of Guid, Boolean)()
		Public Sub New()
			InitializeComponent()
			list = New List(Of TestData)()
			For i As Integer = 0 To 19
				list.Add(New TestData() With {.Id = Guid.NewGuid(), .Number = i})
			Next i
			grid.ItemsSource = list
		End Sub

		Private Sub grid_CustomUnboundColumnData(ByVal sender As Object, ByVal e As GridColumnDataEventArgs)
			If e.Column.FieldName = "Selected" Then
				Dim key As Guid = CType(e.GetListSourceFieldValue("Id"), Guid)
				If e.IsGetData Then
					e.Value = GetIsSelected(key)
				End If
				If e.IsSetData Then
					SetIsSelected(key, CBool(e.Value))
				End If
			End If
		End Sub
		Private Function GetIsSelected(ByVal key As Guid) As Boolean
			Dim isSelected As Boolean
			If selectedValues.TryGetValue(key, isSelected) Then
				Return isSelected
			End If
			Return False
		End Function
		Private Sub SetIsSelected(ByVal key As Guid, ByVal value As Boolean)
			If value Then
				selectedValues(key) = value
			Else
				selectedValues.Remove(key)
			End If
		End Sub

		Private Sub BtnInvert_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			For i As Integer = 0 To list.Count - 1
				Dim newIsSelected As Boolean = Not GetIsSelected(list(i).Id)
				Dim rowHandle As Integer = grid.GetRowHandleByListIndex(i)
				grid.SetCellValue(rowHandle, "Selected", newIsSelected)
			Next i
		End Sub
		Private Sub BtnGetSelected_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim selectedIds As String = String.Empty
			For Each key As Guid In selectedValues.Keys
				selectedIds &= String.Format("{0}" & Constants.vbLf, key)
			Next key
			Dim caption As String = String.Format("Selected rows (Total: {0})", selectedValues.Count)
			MessageBox.Show(selectedIds, caption, MessageBoxButton.OK)
		End Sub
	End Class

	Public Class TestData
		Private privateId As Guid
		Public Property Id() As Guid
			Get
				Return privateId
			End Get
			Set(ByVal value As Guid)
				privateId = value
			End Set
		End Property
		Private privateNumber As Integer
		Public Property Number() As Integer
			Get
				Return privateNumber
			End Get
			Set(ByVal value As Integer)
				privateNumber = value
			End Set
		End Property
	End Class
End Namespace
