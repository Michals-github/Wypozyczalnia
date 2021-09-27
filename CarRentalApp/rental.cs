﻿using System;
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
    public partial class rental : Form
    {
        public rental()
        {
            InitializeComponent();
            carload();
            rentalload();

        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-FLIISIF\\SQLEXPRESS; Initial Catalog = carrental; User ID=sa; Password=qwe");
        SqlCommand cmd;
        SqlCommand cmd1;
        SqlDataReader dr;
        string proid;
        string sql;
        string sql1;
        bool Mode = true;
        string id;

        public void carload()
        {
            cmd = new SqlCommand("select * from carreg", con);
            con.Open();
            dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                txtcarid.Items.Add(dr["regno"].ToString());
            }
            con.Close();
        }

        public void rentalload()
        {
            sql = "select * from rental";
            cmd = new SqlCommand(sql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();

            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]) ;
            }

            con.Close();
        }
        private void rental_Load(object sender, EventArgs e)
        {

        }

        private void txtcarid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string aval;
            cmd = new SqlCommand("select * from carreg where regno = '" + txtcarid.Text + "' ", con);
            con.Open();
            dr = cmd.ExecuteReader();

           if (dr.Read())
           {
                

                aval =  dr["available"].ToString();

                status.Text = aval;

                if(aval == "No")
                {
                    
                    txtcustid.Enabled = false;
                    txtcustname.Enabled = false;
                    txtfee.Enabled = false;
                    txtdate.Enabled = false;
                    txtdue.Enabled = false;

                }
                else
                {
                    txtcustid.Enabled = true;
                    txtcustname.Enabled = true;
                    txtfee.Enabled = true;
                    txtdate.Enabled = true;
                    txtdue.Enabled = true;
                }
           }
            else
            {
                aval = "Samochód nie jest dostęny";
            }
            con.Close();
        }

        private void txtcustid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                cmd = new SqlCommand("select * from customer where custid = '" + txtcustid.Text + "' ", con);
                con.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtcustname.Text = dr["custname"].ToString();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono ID Klienta");
                }

                con.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string carid = txtcarid.SelectedItem.ToString();
            string custid = txtcustid.Text;
            string custname = txtcustname.Text;
            string fee = txtfee.Text;
            string date = txtdate.Value.Date.ToString("dd.MM.yyyy");
            string due = txtdue.Value.Date.ToString("dd.MM.yyyy");

            sql = "insert into rental(car_id,cust_id,custname,fee,since_date,due_date)values(@car_id,@cust_id,@custname,@fee,@since_date,@due_date)";
            con.Open();
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@car_id", carid);
            cmd.Parameters.AddWithValue("@cust_id", custid);
            cmd.Parameters.AddWithValue("@custname", custname);
            cmd.Parameters.AddWithValue("@fee", fee);
            cmd.Parameters.AddWithValue("@since_date", date);
            cmd.Parameters.AddWithValue("@due_date", due);
            cmd.ExecuteNonQuery();
            
            

            sql1 = "update carreg set available ='No' where regno = @regno ";
            cmd1 = new SqlCommand(sql1, con);
            cmd1.Parameters.AddWithValue("@regno", carid);
            cmd1.ExecuteNonQuery();

            MessageBox.Show("Dodano rekord");

            con.Close();

            rentalload();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
