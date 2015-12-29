﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Tibialyzer {
    class SimpleLootNotification : SimpleNotification {
        private Label creatureDropLabel;
        private PictureBox creatureBox;

        public SimpleLootNotification(Creature cr, List<Tuple<Item, int>> items) : base() {
            this.InitializeComponent();

            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;

            int max_x = 300;
            int base_x = 64, base_y = 20;
            int x = 0;
            int y = 0;
            int item_spacing = 4;
            Size item_size = new Size(32, 32);

            List<Tuple<Item, int>> updatedItems = new List<Tuple<Item, int>>();
            foreach (Tuple<Item, int> tpl in items) {
                if (tpl.Item1.name.ToLower() == "gold coin" && tpl.Item2 > 100) {
                    updatedItems.Add(new Tuple<Item, int>(MainForm.mainForm.itemNameMap["platinum coin"], tpl.Item2 / 100));
                    updatedItems.Add(new Tuple<Item,int>(tpl.Item1, tpl.Item2 % 100));
                } else {
                    updatedItems.Add(tpl);
                }
            }

            foreach (Tuple<Item, int> tpl in updatedItems) {
                Item item = tpl.Item1;
                int count = tpl.Item2;
                Image[] stacks = null;
                if (item.stackable) {
                    stacks = LootDropForm.GetFrames(item.image);
                }
                while (count > 0) {
                    if (x >= (max_x - item_size.Width - item_spacing)) {
                        x = 0;
                        y = y + item_size.Height + item_spacing;
                    }
                    int mitems = 1;
                    if (item.stackable) mitems = Math.Min(count, 100);
                    count -= mitems;

                    PictureBox picture_box = new PictureBox();
                    picture_box.Location = new System.Drawing.Point(base_x + x, base_y + y);
                    picture_box.Name = item.name;
                    picture_box.Size = new System.Drawing.Size(item_size.Width, item_size.Height);
                    picture_box.TabIndex = 1;
                    picture_box.TabStop = false;
                    if (item.stackable) {
                        Bitmap image = new Bitmap(LootDropForm.GetStackImage(stacks, mitems, item));
                        Graphics gr = Graphics.FromImage(image);
                        int numbers = (int)Math.Floor(Math.Log(mitems, 10)) + 1;
                        int xoffset = 1, logamount = mitems;
                        for (int i = 0; i < numbers; i++) {
                            int imagenr = logamount % 10;
                            xoffset = xoffset + MainForm.image_numbers[imagenr].Width + 1;
                            gr.DrawImage(MainForm.image_numbers[imagenr],
                                new Point(image.Width - xoffset, image.Height - MainForm.image_numbers[imagenr].Height - 3));
                            logamount /= 10;
                        }
                        picture_box.Image = image;
                    } else {
                        picture_box.Image = item.image;
                    }

                    picture_box.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box.BackgroundImage = MainForm.item_background;
                    value_tooltip.SetToolTip(picture_box, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.name) + " value: " + Math.Max(item.actual_value, item.vendor_value) * mitems);
                    this.Controls.Add(picture_box);

                    x += item_size.Width + item_spacing;
                }
            }

            this.creatureBox.Image = cr.image;
            this.creatureDropLabel.Text = String.Format("Loot of {0}.", cr.name);

            this.InitializeSimpleNotification();
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleLootNotification));
            this.creatureDropLabel = new System.Windows.Forms.Label();
            this.creatureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.creatureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // creatureDropLabel
            // 
            this.creatureDropLabel.AutoSize = true;
            this.creatureDropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.creatureDropLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.creatureDropLabel.Location = new System.Drawing.Point(64, 3);
            this.creatureDropLabel.Name = "creatureDropLabel";
            this.creatureDropLabel.Size = new System.Drawing.Size(121, 13);
            this.creatureDropLabel.TabIndex = 0;
            this.creatureDropLabel.Text = "Loot of a behemoth.";
            // 
            // creatureBox
            // 
            this.creatureBox.Location = new System.Drawing.Point(0, 0);
            this.creatureBox.Name = "creatureBox";
            this.creatureBox.Size = new System.Drawing.Size(60, 60);
            this.creatureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.creatureBox.TabIndex = 1;
            this.creatureBox.TabStop = false;
            // 
            // SimpleLootNotification
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(354, 60);
            this.Controls.Add(this.creatureBox);
            this.Controls.Add(this.creatureDropLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimpleLootNotification";
            this.Text = "Loot Notification";
            ((System.ComponentModel.ISupportInitialize)(this.creatureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}