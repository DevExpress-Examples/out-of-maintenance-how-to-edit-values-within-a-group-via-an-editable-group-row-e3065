using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace EditableGroupRow
{
    public class GroupRowEdit
    {
        int rowToChangeHandle = GridControl.InvalidRowHandle;
        GridView View;
        public GroupRowEdit(DevExpress.XtraGrid.Views.Grid.GridView view)
        { 
            View = view;
            View.PopupMenuShowing +=new PopupMenuShowingEventHandler(View_PopupMenuShowing);
        }

        private void View_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (View.IsGroupRow(e.HitInfo.RowHandle))
            {
                e.Menu.Items.Clear();
                DXMenuItem item = new DXMenuItem("Edit");
                item.Click += new EventHandler(item_Click);
                e.Menu.Items.Add(item);
            }
        }
        void item_Click(object sender, EventArgs e)
        {
            ShowGroupEdit();
        }

        void ShowGroupEdit()
        {
            TextEdit te = new TextEdit();
            this.View.GridControl.Controls.Add( te);
            rowToChangeHandle = this.View.FocusedRowHandle;
            GridViewInfo gvi = new GridViewInfo(this.View);
            Graphics g = this.View.GridControl.CreateGraphics();
            gvi.Calc(g, this.View.ViewRect);
            GridGroupRowInfo info = gvi.RowsInfo.GetInfoByHandle(rowToChangeHandle) as GridGroupRowInfo;
            string newText = this.View.GetGroupRowValue(rowToChangeHandle).ToString();
            te.Text = newText;
            te.Bounds = info.DataBounds;
            te.Visible = true;
            te.Focus();
            te.SelectAll();
            te.Leave += new System.EventHandler(this.textEdit_Leave);
            te.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEdit_KeyDown);
        }
        void HideGroupEdit(object sender)
        {
                rowToChangeHandle = GridControl.InvalidRowHandle;
                (sender as TextEdit).Visible = false;
                View.GridControl.Controls.Remove(sender as TextEdit);
        }

        private void textEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateNewRowString(sender);
            }
            if (e.KeyCode == Keys.Escape)
            {
                HideGroupEdit(sender);
            }
        }

        private void textEdit_Leave(object sender, EventArgs e)
        {
            UpdateNewRowString(sender);
        }

        void UpdateNewRowString(object sender)
        {
            string newValue = (sender as TextEdit).Text;
            View.BeginSort();
            GridColumn grColumn = View.GroupedColumns[View.GetRowLevel(rowToChangeHandle)];
            List<int> indexList = new List<int>();
            for (int i = 0; i < View.GetChildRowCount(rowToChangeHandle); i++)
            {
                int index = View.GetChildRowHandle(rowToChangeHandle, i);
                View.SetRowCellValue(index, grColumn, newValue);
            }
            View.EndSort();
            HideGroupEdit(sender);
        }
    }
}
