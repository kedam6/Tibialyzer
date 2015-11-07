﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;


namespace Tibialyzer {
    class ListNotification : NotificationForm {
        private Label nameLabel;
        private Label commandLabel;

        public string type;
        private List<Command> commands;

        public ListNotification(List<Command> commands) {
            this.commands = commands;
            this.InitializeComponent();
        }

        private void InitializeComponent() {
            this.nameLabel = new System.Windows.Forms.Label();
            this.commandLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.nameLabel.Location = new System.Drawing.Point(13, 32);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(49, 16);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            // 
            // commandLabel
            // 
            this.commandLabel.AutoSize = true;
            this.commandLabel.BackColor = System.Drawing.Color.Transparent;
            this.commandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.commandLabel.Location = new System.Drawing.Point(156, 32);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(77, 16);
            this.commandLabel.TabIndex = 1;
            this.commandLabel.Text = "Command";
            // 
            // ListNotification
            // 
            this.ClientSize = new System.Drawing.Size(430, 169);
            this.Controls.Add(this.commandLabel);
            this.Controls.Add(this.nameLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ListNotification";
            this.Load += new System.EventHandler(this.ListNotification_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ListNotification_Load(object sender, EventArgs e) {
            this.SuspendForm();

            this.NotificationInitialize();
            int total_yoffset = this.commandLabel.Location.Y + this.commandLabel.Height + 10;
            foreach (Command c in commands) {
                Panel panel = new Panel();
                panel.Location = new Point(0, total_yoffset + 2);
                panel.Size = new Size(this.Size.Width, 20);
                panel.Click += panel_Click;
                panel.BackColor = Color.Transparent;
                panel.Name = c.command;

                Label playerLabel = new Label();
                playerLabel.Location = new Point(this.nameLabel.Location.X, total_yoffset);
                playerLabel.Text = c.player;
                playerLabel.Click += panel_Click;
                playerLabel.Name = c.command;
                playerLabel.Font = this.nameLabel.Font;
                playerLabel.ForeColor = MainForm.label_text_color;
                playerLabel.BackColor = Color.Transparent;
                this.Controls.Add(playerLabel);

                Label commandLabel = new Label();
                commandLabel.Location = new Point(this.commandLabel.Location.X, total_yoffset);
                commandLabel.Text = c.command;
                commandLabel.Click += panel_Click;
                commandLabel.Name = c.command;
                commandLabel.Font = this.nameLabel.Font;
                commandLabel.ForeColor = MainForm.label_text_color;
                commandLabel.Size = new Size(this.Size.Width - this.commandLabel.Location.X, 25);
                commandLabel.AutoSize = true;
                commandLabel.BackColor = Color.Transparent;
                this.Controls.Add(commandLabel);
                this.Controls.Add(panel);

                total_yoffset += 25;
            }
            total_yoffset += 10;
            this.Size = new Size(this.Size.Width, total_yoffset);
            base.NotificationFinalize();
            this.ResumeForm();
        }

        public static void OpenCommand(string command, string type) {
            if (type == "commands") {
                MainForm.mainForm.priority_command = command;
            } else if (type == "urls") {
                MainForm.OpenUrl(command);
            }
        }

        void panel_Click(object sender, EventArgs e) {
            this.ReturnFocusToTibia();
            OpenCommand((sender as Control).Name, type);
        }
    }
}