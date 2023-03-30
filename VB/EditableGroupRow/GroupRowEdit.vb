Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms

Imports DevExpress.Utils.Menu
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Namespace EditableGroupRow
	Public Class GroupRowEdit
		Private rowToChangeHandle As Integer = GridControl.InvalidRowHandle
		Private View As GridView
		Public Sub New(ByVal view As DevExpress.XtraGrid.Views.Grid.GridView)
			Me.View = view
			AddHandler Me.View.PopupMenuShowing, AddressOf View_PopupMenuShowing
		End Sub

		Private Sub View_PopupMenuShowing(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs)
			If View.IsGroupRow(e.HitInfo.RowHandle) Then
				If e.Menu Is Nothing Then e.Menu = New DevExpress.XtraGrid.Menu.GridViewMenu(View)
				e.Menu.Items.Clear()
				Dim item As New DXMenuItem("Edit")
				AddHandler item.Click, AddressOf item_Click
				e.Menu.Items.Add(item)
			End If
		End Sub
		Private Sub item_Click(ByVal sender As Object, ByVal e As EventArgs)
			ShowGroupEdit()
		End Sub

		Private Sub ShowGroupEdit()
			Dim te As New TextEdit()
			Me.View.GridControl.Controls.Add(te)
			rowToChangeHandle = Me.View.FocusedRowHandle
			Dim gvi As New GridViewInfo(Me.View)
			Dim g As Graphics = Me.View.GridControl.CreateGraphics()
			gvi.Calc(g, Me.View.ViewRect)
			Dim info As GridGroupRowInfo = TryCast(gvi.RowsInfo.GetInfoByHandle(rowToChangeHandle), GridGroupRowInfo)
			Dim newText As String = Me.View.GetGroupRowValue(rowToChangeHandle).ToString()
			te.Text = newText
			te.Bounds = info.DataBounds
			te.Visible = True
			te.Focus()
			te.SelectAll()
			AddHandler te.Leave, AddressOf textEdit_Leave
			AddHandler te.KeyDown, AddressOf textEdit_KeyDown
		End Sub
		Private Sub HideGroupEdit(ByVal sender As Object)
				rowToChangeHandle = GridControl.InvalidRowHandle
				TryCast(sender, TextEdit).Visible = False
				View.GridControl.Controls.Remove(TryCast(sender, TextEdit))
		End Sub

		Private Sub textEdit_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.KeyCode = Keys.Enter Then
				UpdateNewRowString(sender)
			End If
			If e.KeyCode = Keys.Escape Then
				HideGroupEdit(sender)
			End If
		End Sub

		Private Sub textEdit_Leave(ByVal sender As Object, ByVal e As EventArgs)
			UpdateNewRowString(sender)
		End Sub

		Private Sub UpdateNewRowString(ByVal sender As Object)
			Dim newValue As String = (TryCast(sender, TextEdit)).Text
			View.BeginSort()
			Dim grColumn As GridColumn = View.GroupedColumns(View.GetRowLevel(rowToChangeHandle))
			Dim indexList As New List(Of Integer)()
			For i As Integer = 0 To View.GetChildRowCount(rowToChangeHandle) - 1
				Dim index As Integer = View.GetChildRowHandle(rowToChangeHandle, i)
				View.SetRowCellValue(index, grColumn, newValue)
			Next i
			View.EndSort()
			HideGroupEdit(sender)
		End Sub
	End Class
End Namespace
