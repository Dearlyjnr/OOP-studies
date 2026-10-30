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

namespace Winform1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Conn;
        SqlCommand cmd;


        private void cleardata()
        {
            txt_teacherid.Clear();
            txt_name.Clear();
            txt_address.Clear();
            txt_salary.Clear();
            txt_find.Clear(); 
        
        }
        private void display()
        {
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " SELECT * FROM Teacher";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView.DataSource = dt;
            Conn.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Conn = new SqlConnection(@"Data Source=DW;Initial Catalog = School;Integrated Security = True");
            cmd = new SqlCommand();
            cmd.Connection = Conn;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void displaydata()
        {
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from teacher";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView.DataSource = dt;
            Conn.Close();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count)
                    return;
               DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                if (row.IsNewRow)

                    return;
                txt_teacherid.Text = row.Cells[0].Value.ToString() ?? string.Empty;
                txt_name.Text = row.Cells[0].Value.ToString() ?? string.Empty;
               txt_address.Text = row.Cells[0].Value.ToString() ?? string.Empty;
                txt_salary.Text = row.Cells[0].Value.ToString() ?? string.Empty;

            }
            catch( Exception ex)
            {
                MessageBox.Show("Invalid Data", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            String query = $"INSERT INTO teacher values('{txt_teacherid.Text.ToString()}','{txt_name.Text.ToString()}','{txt_address.Text.ToString()}','{txt_salary.Text.ToString()}')";
            cmd.CommandText = query;
            Conn.Open();
            cmd.ExecuteNonQuery();
            cleardata();
            Conn.Close();
            displaydata();
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            displaydata();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

            
                try
                {
                   
                    cmd.Parameters.Clear();

                    
                    cmd.CommandText = "UPDATE Teacher SET names = @name, Addresss = @address, Salary = @salary WHERE id = @id";
                    cmd.Connection = Conn;

                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txt_teacherid.Text));
                    cmd.Parameters.AddWithValue("@name", txt_name.Text);
                    cmd.Parameters.AddWithValue("@address", txt_address.Text);
                    cmd.Parameters.AddWithValue("@salary", Convert.ToInt32(txt_salary.Text));

                    Conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Conn.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Update",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No record found with that Teacher ID.", "Update",MessageBoxButtons.AbortRetryIgnore,MessageBoxIcon.Error);
                    }

                    cleardata();
                    displaydata();
                }
                catch (Exception ex)
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                    MessageBox.Show("Database Error: " + ex.Message, "Update",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = " Select id,namess,Addresss,Salary WHERE id = @ id";
                cmd.Connection = Conn;
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txt_find.Text));
                Conn.Open();

                // Use a SqlDataReader to fetch the record

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txt_teacherid.Text = reader["id"].ToString();
                        txt_name.Text = reader["namess"].ToString();
                        txt_address.Text= reader["Addresss"].ToString();
                        txt_salary.Text = reader["Salary"].ToString();
                    }
                    else
                    {
                       MessageBox.Show("No Teacher with that ID.", "Search", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    }
                    Conn.Close();
                }

            }
            catch (Exception ex)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();

                }
                else
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
    }
}
