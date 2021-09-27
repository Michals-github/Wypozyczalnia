using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CarRentalApp
{
    public partial class customer : Form
    {
        public customer()
        {
            InitializeComponent();
            Autono();
            customerload();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-FLIISIF\\SQLEXPRESS; Initial Catalog = carrental; User ID=sa; Password=qwe");
        SqlCommand cmd;
        SqlDataReader dr;
        string proid;
        string sql;
        bool Mode = true;
        string id;

        public void Autono()
        {
            sql = "select custid from customer order by custid desc";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();


            if (dr.Read())
            {
                int id = int.Parse(dr[0].ToString()) + 1;

                proid = id.ToString("00000");

            }
            else if (Convert.IsDBNull(dr))
            {
                proid = ("00001");
            }
            else
            {
                proid = ("00001");

            }



            txtid.Text = proid.ToString();

            con.Close();

        }

       

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string custid = txtid.Text;
            string custname = txtname.Text;
            string address = txtaddress.Text;
            string mobile = txtmobile.Text;



            //id = dataGridView1.CurrentRow.Cells[0].Value.ToString();//id czyli jakby regno któe jest aktualnie zaznaczone
            //MessageBox.Show(id.ToString());

            if (Mode == true)
            {
                //sql = "insert into customer(custname,address,mobile)values(@custname,@address,@mobile)";
                sql = "sp_I_customer";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@custid", custid);
                cmd.Parameters.AddWithValue("@custname", custname);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@mobile", mobile);
                cmd.ExecuteNonQuery();


                MessageBox.Show("Dodano rekord");

                txtid.Clear();
                txtname.Clear();
                txtmobile.Clear();
                txtname.Focus();
            }
            else
            {
                sql = "update carreg set make=@make,model=@model,available=@available where regno=@regno";
                con.Open();
                cmd = new SqlCommand(sql, con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Rekord zaktualizowany");
                /*
                cmd.Parameters.AddWithValue("@make", make);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@available", avl);
                cmd.Parameters.AddWithValue("@regno", id);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Rekord zaktualizowany");
                txtregno.Enabled = true;
                Mode = true;

                txtmake.Clear();
                txtmodel.Clear();
                txtavl.Items.Clear();
                txtmake.Focus();*/

            }

            con.Close();
        }

        public void customerload()
        {
            sql = "select * from customer";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            con.Close();
        }

        public void getcustomerid(String id)
        {
            //sql = "select * from customer where custid = '" + id + "' ";
            sql = "sp_S_customer";
            con.Open();
            cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txtid.Text = dr[0].ToString();
                txtname.Text = dr[1].ToString();
                txtaddress.Text = dr[2].ToString();
                txtmobile.Text = dr[3].ToString();
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["edit"].Index && e.RowIndex >= 0)
            {
                //UpdateBtn.Enabled = true;
                Mode = false;
                txtid.Enabled = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getcustomerid(id);
            }
            else if (e.ColumnIndex == dataGridView1.Columns["delete"].Index && e.RowIndex >= 0)
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć pozycję?", "Alert", MessageBoxButtons.YesNo);

                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                if (dialogResult == DialogResult.Yes)
                {
                    sql = "delete from carreg where regno = @id";
                    con.Open();
                    cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Rekord został usuniety");
                    con.Close();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Rekord nie został usunięty");
                }


            }
        }
    }
}
