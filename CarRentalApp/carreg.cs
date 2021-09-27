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
    public partial class carreg : Form
    {
        public carreg()
        {
            InitializeComponent();
            Autono();
            load();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-FLIISIF\\SQLEXPRESS; Initial Catalog = carrental; User ID=sa; Password=qwe");
        SqlCommand cmd;
        SqlDataReader dr;
        string proid;
        string sql;
        bool Mode = true;
        string id;
        //carreg obj = (carreg)Application.OpenForms["carreg.cs"];

        /*
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        BindingSource bs = new BindingSource();
        */
        private DataTable dt = new DataTable();
        private DataSet ds = new DataSet();

        public void Autono()
        {
            sql = "select id from carreg order by id desc";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
           

            if(dr.Read())
            {

               
                int id = int.Parse(dr[0].ToString()) + 1;
                
                proid = id.ToString("00000");

                MessageBox.Show("Pierwszy if id = " + proid);


            }
            else if(Convert.IsDBNull(dr))
            {
                proid = ("00000");
                MessageBox.Show("Else if = " + proid);

            }
            else
            {
                proid = ("00000");
                MessageBox.Show("Else = " + proid);

            }

         

            txtregno.Text = proid.ToString();

            con.Close();

        }

        public void carreg_Load(object sender, EventArgs e)
        {
            
        }


       
        

        private void button1_Click(object sender, EventArgs e)
        {
            string regno = txtregno.Text;
            string make = txtmake.Text;
            string model = txtmodel.Text;
            string avl = txtavl.SelectedItem.ToString();


            
            
             id = dataGridView1.CurrentRow.Cells[0].Value.ToString();//id czyli jakby regno któe jest aktualnie zaznaczone
             MessageBox.Show(id.ToString());
            
            

            if (Mode == true)
            {
                //sql = "insert into carreg(regno,make,model,available)values(@regno,@make,@model,@available)";
                sql = "sp_I_car";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@regno", regno);
                cmd.Parameters.AddWithValue("@make", make);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@available", avl);
                cmd.ExecuteNonQuery();
                

                MessageBox.Show("Dodano rekord");
                con.Close();
                

                txtmake.Clear();
                txtmodel.Clear();
                //txtavl.Items.Clear();
                txtavl.Text = "";
                txtmake.Focus();
                


                //bs.DataSource = ds.Tables[0];
                //dataGridView1.DataSource = bs;
                //BindingSource.ResetBindings(false);

                /*
                sql = "select * from carreg";
                con.Open();
                SqlDataAdapter SDA = new SqlDataAdapter(sql, con);
                DataSet DS = new System.Data.DataSet();
                
                SDA.Fill(DS, "carrerg");
                
                dataGridView1.DataSource = DS.Tables[0];
                */



                /*
                if(cmd.ExecuteNonQuery()>0)
                {
                    //obj.load();
                    con.Close();
                    sql = "select * from carreg";
                    cmd = new SqlCommand(sql, con);
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(dr[1], dr[2], dr[3], dr[4]);
                    }
                    con.Close();

                    dataGridView1.Update();
                    dataGridView1.Refresh();
                    MessageBox.Show("Prawidlowo dodano");
                    //int regnoint = int.Parse(regno);
                    // regnoint += 1;
                    //txtregno.Text = regnoint.ToString();
                    //cmd.Parameters.AddWithValue("@regno", regno);
                    //regno = txtregno.Text;
                    //MessageBox.Show(regno);
                    //txtmake.Text = String.Empty;
                }*/

                /*
                int test = int.Parse(regno);
                test += 1;
                regno = test.ToString("00000");
                */
                //Application.Run(new carreg());

                //MessageBox.Show("Record Addedd");




            }
            else
            {
                sql = "update carreg set make=@make,model=@model,available=@available where regno=@regno";
                con.Open();
                cmd = new SqlCommand(sql, con);

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
                txtmake.Focus();
                
            }

            con.Close();
        }

        public void load()
        {
            sql = "select * from carreg";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while(dr.Read())
            {
                dataGridView1.Rows.Add(dr[1],dr[2],dr[3],dr[4]);
            }

            con.Close();
        }


        public void getid(String id)
        {
            sql = "select * from carreg where id = '" + id + "' ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txtregno.Text = dr[1].ToString();
                txtmake.Text = dr[2].ToString();
                txtmodel.Text = dr[3].ToString();
                txtavl.Text = dr[4].ToString();
            }
            con.Close();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==dataGridView1.Columns["edit"].Index && e.RowIndex>=0)
            {
                UpdateBtn.Enabled = true;
                Mode = false;
                txtregno.Enabled = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getid(id);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }



        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string regno = txtregno.Text;
            string make = txtmake.Text;
            string model = txtmodel.Text;
            string avl = txtavl.SelectedItem.ToString();

            id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            sql = "update carreg set make=@make,model=@model,available=@available where regno=@regno";
            con.Open();
            cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@make", make);
            cmd.Parameters.AddWithValue("@model", model);
            cmd.Parameters.AddWithValue("@available", avl);
            cmd.Parameters.AddWithValue("@regno", id);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Rekord zaktualizowany");
            txtregno.Enabled = true;
            Mode = true;


            con.Close();
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            load();
            Autono();
            /*
            sql = "select * from carreg";
            con.Open();
            //cmd = new SqlCommand(sql, con);
            //cmd.ExecuteNonQuery();
            MessageBox.Show(cmd.ToString());
            SqlDataAdapter SDA = new SqlDataAdapter(sql, con);
            DataSet DS = new System.Data.DataSet();
            SDA.Fill(DS, "carrerg");
            dataGridView1.DataSource = DS.Tables[0];
            dataGridView1.Update();
            dataGridView1.Refresh();*/

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        
    }
}
