using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NoteLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "select * from Data";
            DataTable dt = SqliteDB.SqlTable(sql);
            if(dt == null)
            {
                return;
            }
            FillTreeView(dt, treeView1);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            {
                MessageBox.Show("Category不可为空！","Note",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if(textBox1.Text == "")
            {
                MessageBox.Show("Item不可为空！","Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(richTextBox2.Text == "")
            {
                MessageBox.Show("内容不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sql = "insert into Data values ('" + comboBox1.Text + "','" + textBox1.Text + "','" + richTextBox2.Text + "')";
            int flag = SqliteDB.ExecuteNonQuery(sql);
            if(flag == 1)
            {
                Log.ActionLog("插入一条记录：" + sql);
                MessageBox.Show("增加成功", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("增加失败，详情请查看日志文件", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Category不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox3.Text == "")
            {
                MessageBox.Show("Item不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (richTextBox3.Text == "")
            {
                MessageBox.Show("内容不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sql = "update Data set Content = '" + richTextBox3.Text + "' where Category = '" + comboBox2.Text + "' and Item = '" + comboBox3.Text + "'";
            int flag = SqliteDB.ExecuteNonQuery(sql);
            if (flag == 1)
            {
                Log.ActionLog("修改一条记录：" + sql);
                MessageBox.Show("修改成功", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败，详情请查看日志文件", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Category不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox3.Text == "")
            {
                MessageBox.Show("Item不可为空！", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sql = "delete from Data where Category = '" + comboBox2.Text + "' and Item = '" + comboBox3.Text + "'";
            int flag = SqliteDB.ExecuteNonQuery(sql);
            if (flag == 1)
            {
                Log.ActionLog("删除一条记录：" + sql);
                MessageBox.Show("删除成功", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("删除失败，详情请查看日志文件", "Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillComboBox(DataTable dt, ComboBox cbo, string ColumnName)
        {
            cbo.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                cbo.Items.Add(dr[ColumnName].ToString());
            }
        }

        private void FillTreeView(DataTable dt,TreeView tv)
        {
            tv.Nodes.Clear();           
            DataView dataview = dt.DefaultView;
            DataTable dt1 = dataview.ToTable(true, "Category");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                TreeNode tn = treeView1.Nodes.Add(dt1.Rows[i][0].ToString());
                tn.Name = "root";
                DataRow[] row = dt.Select("Category = '" + dt1.Rows[i][0] + "'");
                foreach (DataRow dr in row)
                {
                    tn.Nodes.Add(dr["Item"].ToString());
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select Item from Data where Category = '" + comboBox2.Text + "'";
            DataTable dt = SqliteDB.SqlTable(sql);
            FillComboBox(dt, comboBox3, "Item");
            comboBox3.Text = "";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select Content from Data where Category = '" + comboBox2.Text + "' and Item = '" + comboBox3.Text + "'";
            DataTable dt = SqliteDB.SqlTable(sql);
            richTextBox3.Text = dt.Rows[0][0].ToString();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab.Name == "show")
            {
                string sql = "select * from Data";
                DataTable dt = SqliteDB.SqlTable(sql);
                FillTreeView(dt, treeView1);
                richTextBox1.Text = "";
            }
            else if(tabControl1.SelectedTab.Name == "add")
            {
                string sql = "select distinct Category from Data";
                DataTable dt = SqliteDB.SqlTable(sql);
                FillComboBox(dt, comboBox1, "Category");
                comboBox1.Text = "";
                textBox1.Text = "";
                richTextBox2.Text = "";
            }
            else
            {
                string sql = "select distinct Category from Data";
                DataTable dt = SqliteDB.SqlTable(sql);
                FillComboBox(dt, comboBox2, "Category");
                comboBox2.Text = "";
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                richTextBox3.Text = "";
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            string Name = treeView1.SelectedNode.Name;
            string Text = treeView1.SelectedNode.Text;
            if(Name == "root")
            {
                return;
            }
            string PreText = treeView1.SelectedNode.Parent.Text;
            string sql = "select Content from Data where Category = '" + PreText + "' and Item = '" + Text + "'";
            DataTable dt = SqliteDB.SqlTable(sql);
            richTextBox1.Text = dt.Rows[0][0].ToString();
        }
    }
}
