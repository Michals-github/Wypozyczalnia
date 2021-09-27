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
//using System.DateTime;



namespace CarRentalApp
{
    public partial class returncar : Form
    {
        public returncar()
        {
            InitializeComponent();
            load();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-FLIISIF\\SQLEXPRESS; Initial Catalog = carrental; User ID=sa; Password=qwe");
        SqlCommand cmd;
        SqlCommand cmd1;
        SqlDataReader dr;
        SqlDataReader dr1;
        string sql;
        string sql1;


        private void txtcarid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                DateTime teraz = DateTime.Now;

                string terazz = teraz.ToString("dd.MM.yyyy");

                DateTime newTeraz = Convert.ToDateTime(terazz);

                //MessageBox.Show(teraz.ToString("dd.MM.yyyy"));

                cmd = new SqlCommand("select car_id,cust_id,since_date,due_date from rental where car_id = '"+ txtcarid.Text+"'", con);
                //cmd = new SqlCommand("select car_id,cust_id,since_date,due_date,DATEDIFF(dd,due_date,GETDATE()) as elap from rental  where car_id = '" + txtcarid.Text + "'", con);
                con.Open();
                
                dr = cmd.ExecuteReader();

                if(dr.Read())
                {
                    txtcustid.Text = dr["cust_id"].ToString();
                    txtdate.Text = dr["due_date"].ToString();

                    string DueDate = dr["due_date"].ToString();

                    MessageBox.Show(DueDate);

                    DateTime newDueDate = Convert.ToDateTime(DueDate);
                    //MessageBox.Show(DueDate);

                    //string elap = dr["elap"].ToString();
                    //string elap = (teraz - DueDate).TotalDays.ToString();
                    //System.DateTimeSpan elap = newDueDate.Subtract(newTeraz);

                    String elap = (newTeraz - newDueDate).TotalDays.ToString();



                    string newElap = elap.ToString();

                    MessageBox.Show(newElap);

                    int elappsed = int.Parse(newElap);

                    txtelp.Text = newElap;

                    if (elappsed > 0)
                    {
                        
                        int fine = elappsed * 100;
                        txtfine.Text = fine.ToString();


                    }
                    else if (elappsed<0)
                    {

                        txtelp.Text = "0";
                        txtfine.Text = "0";
                    }
                    else
                    {
                        txtfine.Text = "0";
                    }

                }
                con.Close();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DateTime teraz = DateTime.Now;

            string terazz = teraz.ToString("dd.MM.yyyy");

            string carid = txtcarid.Text;
            string custid = txtcustid.Text;
            string date = terazz;
            string elp = txtelp.Text;
            string fine = txtfine.Text;

            try
            {
                sql = "insert into returncar(car_id,cust_id,date,elp,fine)values(@car_id,@cust_id,@date,@elp,@fine)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@car_id", carid);
                cmd.Parameters.AddWithValue("@cust_id", custid);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@elp", elp);
                cmd.Parameters.AddWithValue("@fine", fine);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Dodano rekord");

                //sql1 = "insert into returncar(car_id,cust_id,date,elp,fine)values(@car_id,@cust_id,@date,@elp,@fine)";
                sql1 = "delete from rental where car_id = @car_id";

                //con.Open();
                cmd1 = new SqlCommand(sql1, con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@car_id", carid);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Usunięto");

            }
            catch
            {
                MessageBox.Show("Błąd");
            }

            con.Close();

            load();

        }


        public void load()
        {
            sql = "select * from returncar";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[1], dr[2], dr[3], dr[4], dr[5]);
            }

            con.Close();
        }

      
    }
}
