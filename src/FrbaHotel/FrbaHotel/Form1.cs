using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FrbaHotel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string sCon = @"Data Source=localhost\SQLSERVER2008; user id=gd; password=gd2014; database=GD2C2014";
                SqlConnection dbConn;
                dbConn = new SqlConnection(sCon);
                dbConn.Open();
                MessageBox.Show(this, "se CONECTO !.\n", "DB Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlCommand myCommand = new SqlCommand("SELECT count(1) FROM gd_esquema.Maestra", dbConn);
                int asd = myCommand.ExecuteNonQuery();
                MessageBox.Show("asd" + asd);
                SqlCommand myCommand1 = new SqlCommand("SELECT TOP(1) * FROM gd_esquema.Maestra;", dbConn);
                //myCommand1.ExecuteReader();
                MessageBox.Show("asd1 -" + myCommand1.ExecuteReader().FieldCount);
                dbConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurd while attemtpting to connect to DB.\n" + ex.Message, "DB Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
