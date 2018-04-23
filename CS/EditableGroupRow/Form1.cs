using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;


namespace EditableGroupRow
{
    
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Rows.Add("Name 1", 25);
            dt.Rows.Add("Name 2", 25);
            dt.Rows.Add("Name 3", 35);
            dt.Rows.Add("Name 4", 55);
            dt.Rows.Add("Name 5", 55);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = dt;
            GroupRowEdit re = new GroupRowEdit(gridView1);
        }

    }
}