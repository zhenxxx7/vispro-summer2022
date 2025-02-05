﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace visproProject
{
    public partial class UpdateForm : Form
    {
        public string filterBox { get; set; }
        private MySqlConnection con;
        private string server, database, uid, password;
        public UpdateForm()
        {
            InitializeComponent();
            server = "localhost";
            database = "visprosummer";
            uid = "root";
            password = "";

            string conString;
            conString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            con = new MySqlConnection(conString);
        }

        private void roomNmBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT price FROM room WHERE room_name = '" + roomNmBox.Text + "'";
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    /*hitung hari dikali price*/
                    int days = (coutDate.Value - cinDate.Value).Days;
                    TotalPymntTxt.Text = (Convert.ToInt32(reader["price"]) * days).ToString();
                }
                con.Close();
            }
        }

        private void addAdult_Click_1(object sender, EventArgs e)
        {
            adultLbl.Text = (Convert.ToInt32(adultLbl.Text) + 1).ToString();
        }

        private void minAdult_Click_1(object sender, EventArgs e)
        {
            if (Convert.ToInt32(adultLbl.Text) > 0)
            {
                adultLbl.Text = (Convert.ToInt32(adultLbl.Text) - 1).ToString();
            }
        }

        private void addChild_Click(object sender, EventArgs e)
        {
            childLbl.Text = (Convert.ToInt32(childLbl.Text) + 1).ToString();
        }

        private void minChild_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(childLbl.Text) > 0)
            {
                childLbl.Text = (Convert.ToInt32(childLbl.Text) - 1).ToString();
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string query = "UPDATE customer SET Full_Name = '" + NameBox.Text + "', Phone_Number = '" + PNBox.Text + "', Email = '" + EmailBox.Text + "', Address = '" + AddressBox.Text + "' WHERE Full_Name = '" + NameBox.Text + "';";
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                MessageBox.Show("Update Success!");
                reader.Close();
                con.Close();
            }
            query = "UPDATE booking SET room_name = '" + roomNmBox.Text + "', adult = '" + adultLbl.Text + "', child = '" + childLbl.Text + "', cin = '" + cinDate.Value.ToString("yyyy-MM-dd") + "', cout = '" + coutDate.Value.ToString("yyyy-MM-dd") + "', price = '" + TotalPymntTxt.Text + "' WHERE Name = '" + filterBox + "' ;";
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                MessageBox.Show("Update Success!");
                reader.Close();
                con.Close();
            }
        }

        private bool openConnection()
        {
            try
            {
                con.Open();
                return true;

            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Connection to server failed!");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username or password, please try again");
                        break;
                }
                return false;
            }
        }

        private void Update_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM customer WHERE Full_Name = '" + filterBox + "';";
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString("Full_Name");
                    string phone = reader.GetString("Phone_Number");
                    string email = reader.GetString("Email");
                    string address = reader.GetString("Address");
                    NameBox.Text = name;
                    PNBox.Text = phone;
                    EmailBox.Text = email;
                    AddressBox.Text = address;
                }
                reader.Close();
                con.Close();
            }

            query = "SELECT * FROM booking WHERE Name = '" + filterBox + "';";
            if (openConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string room = reader.GetString("room_name");
                    string adult = reader.GetString("adult");
                    string child = reader.GetString("child");
                    string cin = reader.GetString("cin");
                    string cout = reader.GetString("cout");
                    string price = reader.GetString("price");
                    roomNmBox.Text = room;
                    adultLbl.Text = adult;
                    childLbl.Text = child;
                    cinDate.Text = cin;
                    coutDate.Text = cout;
                    TotalPymntTxt.Text = price;
                }
                reader.Close();
                con.Close();
            }
        }

        

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
