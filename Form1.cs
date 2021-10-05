using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DataBaseCSharp
{
    public partial class Form1 : Form
    {
       private SqlConnection sqlConnection =null;
       private SqlConnection northwndConnection = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["University"].ConnectionString);
            sqlConnection.Open();
            northwndConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["northwnd"].ConnectionString);
            northwndConnection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO [Students] (Name, FamalyName, HomeworkCompleted, DateOfEntry) VALUES (@Name, @FamalyName, @HomeworkCompleted, @DateOfEntry)",
                sqlConnection);
            DateTime dataTime = DateTime.Parse(textBox4.Text);


            sqlCommand.Parameters.AddWithValue("Name", textBox1.Text);
            sqlCommand.Parameters.AddWithValue("FamalyName", textBox2.Text);
            sqlCommand.Parameters.AddWithValue("HomeworkCompleted", textBox3.Text);
            sqlCommand.Parameters.AddWithValue("DateOfEntry", $"{dataTime.Month}/{dataTime.Day}/{dataTime.Year}");

            MessageBox.Show(sqlCommand.ExecuteNonQuery().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(textBox5.Text,
                northwndConnection);
            DataSet dataSet = new DataSet();
            try
            {
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch (Exception ex)
            {

                MessageBox.Show($"ERROR: {ex.Message}");
                
            }
            

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            SqlDataReader sqlDataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    northwndConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                while (sqlDataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(sqlDataReader["ProductName"]),
                     Convert.ToString(sqlDataReader["QuantityPerUnit"]),
                      Convert.ToString(sqlDataReader["UnitPrice"])
                    });

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"ERROR: {ex.Message}");
            }

            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
        }

      
    }
}
