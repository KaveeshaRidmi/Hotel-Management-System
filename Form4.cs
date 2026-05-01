using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Hotels
{
    public partial class Form4 : Form
    {
        private string connectionString = "Server=localhost;Port=3306;Database=HotelDB;Uid=root;Pwd=;";
        public Form4()
        {
            InitializeComponent();
        }
        private MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Name and Phone Number are required fields.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string roomType = "";
            if (radioButton1.Checked)
            {
                roomType = "Standard";
            }
            else if (radioButton2.Checked)
            {
                roomType = "Deluxe";
            }
            else if (radioButton3.Checked)
            {
                roomType = "Suite";
            }
            else
            {
                MessageBox.Show("Please select a Room Type.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            MySqlConnection connection = null;
            try
            {
                connection = GetConnection();
                if (connection == null) return;

                string query = @"INSERT INTO Bookings 
                                (Name, Address, PhoneNumber, Email, CheckInDate, CheckOutDate, NumberOfGuests, RoomType) 
                                VALUES (@name, @address, @phone, @email, @checkIn, @checkOut, @guests, @roomType)";

                MySqlCommand command = new MySqlCommand(query, connection);


                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@address", textBox2.Text);
                command.Parameters.AddWithValue("@phone", textBox3.Text);
                command.Parameters.AddWithValue("@email", textBox4.Text);

                command.Parameters.AddWithValue("@checkIn", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@checkOut", dateTimePicker2.Value.ToString("yyyy-MM-dd"));


                command.Parameters.AddWithValue("@guests", (int)numericUpDown1.Value);

                command.Parameters.AddWithValue("@roomType", roomType);


                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Booking successfully saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFormFields();
                }
                else
                {
                    MessageBox.Show("Failed to save booking. No rows affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private void ClearFormFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            numericUpDown1.Value = 0;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        
        private void Form4_Load(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
    
