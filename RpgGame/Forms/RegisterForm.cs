﻿using System;
using System.Windows.Forms;

namespace RpgGame.Forms
{
    using RpgGame.Data;
    using RpgGame.Data.Models;

    public partial class RegisterForm : Form
    {
        private RpgGameContext context;
        public RegisterForm(RpgGameContext context)
        {
            this.context = context;
            InitializeComponent();
        }
        private void RegisterForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void registerButton_Click_1(object sender, EventArgs e)
        {
            if (passwordTextBox.Text != repeatPasswordTextBox.Text)
            {
                MessageBox.Show("Passwords are different!");
            }
            else if (String.IsNullOrEmpty(usernameTextbox.Text) || String.IsNullOrEmpty(passwordTextBox.Text) || String.IsNullOrEmpty(repeatPasswordTextBox.Text))
            {
                MessageBox.Show("Field can't be empty!");
            }
            else
            {
                User newUser = new User()
                {
                    Username = usernameTextbox.Text,
                    Password = passwordTextBox.Text,
                    RegisteredDate = DateTime.Now,
                    LastLoginDate = DateTime.Now
                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }
    }
}