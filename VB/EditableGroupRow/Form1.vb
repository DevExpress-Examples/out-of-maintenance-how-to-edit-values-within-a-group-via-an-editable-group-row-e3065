Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms

Namespace EditableGroupRow

    Public Partial Class Form1
        Inherits Form

        Private dt As DataTable = New DataTable()

        Public Sub New()
            InitializeComponent()
            dt.Columns.Add("Name", GetType(String))
            dt.Columns.Add("Age", GetType(Integer))
            dt.Rows.Add("Name 1", 25)
            dt.Rows.Add("Name 2", 25)
            dt.Rows.Add("Name 3", 35)
            dt.Rows.Add("Name 4", 55)
            dt.Rows.Add("Name 5", 55)
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            gridControl1.DataSource = dt
            Dim re As GroupRowEdit = New GroupRowEdit(gridView1)
        End Sub
    End Class
End Namespace
