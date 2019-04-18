using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        Graphics gBitmap;
        Bitmap bitmap;
        Pen pen, eraser_pen;
        Rectangle rec, elli;
        string tool;
        int x, y, triangle_x, triangle_y, star_x, star_y, text_x, text_y; 
        int eraser_width = 1, eraser_height = 1;   
        bool moving = false;
        Queue<Point> q = new Queue<Point>();


        public Form1()
        {
            InitializeComponent();
           
            bitmap = new Bitmap(pictureBox5.Width, pictureBox5.Height);
            gBitmap = Graphics.FromImage(bitmap);
            pictureBox5.Image = bitmap;

            pen = new Pen(Color.Black, 1);
            eraser_pen = new Pen(Color.White, 1);
        }

        private void color_Clicked(object sender, EventArgs e)
        {
            PictureBox p = sender as PictureBox;
            pen.Color = p.BackColor;
        }

        private void thickness_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (tool == "pen" || tool == "rectangle")
            {
                pen.Width = float.Parse(btn.Text);
            }
            if (tool == "eraser")
            {
                eraser_width = int.Parse(btn.Text);
                eraser_height = int.Parse(btn.Text);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            x = e.X;
            y = e.Y;

            if (tool == "brush")
            {
                Color color = bitmap.GetPixel(e.X, e.Y);
                q.Enqueue(e.Location);
                Point p;
                int a, b, c, d;
                while (q.Count != 0)
                {
                    p = q.Dequeue();
                    bitmap.SetPixel(p.X, p.Y, pen.Color);
                    a = p.X + 1;
                    b = p.X - 1;
                    c = p.Y + 1;
                    d = p.Y - 1;

                    if (bitmap.GetPixel(a, p.Y) == color && !q.Contains(new Point(a, p.Y)) && a != pictureBox1.Width - 1) q.Enqueue(new Point(a, p.Y));
                    if (bitmap.GetPixel(b, p.Y) == color && !q.Contains(new Point(b, p.Y)) && b != 0) q.Enqueue(new Point(b, p.Y));
                    if (bitmap.GetPixel(p.X, c) == color && !q.Contains(new Point(p.X, c)) && c != pictureBox1.Height - 1) q.Enqueue(new Point(p.X, c));
                    if (bitmap.GetPixel(p.X, d) == color && !q.Contains(new Point(p.X, d)) && d != 0) q.Enqueue(new Point(p.X, d));
                }
                if (tool == "text")
                {
                    text_x = e.X;
                    text_y = e.Y;
                }
                pictureBox5.Refresh();
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving && tool == "pen")
            {
                gBitmap.DrawLine(pen, new Point(x, y), e.Location);
                x = e.X;
                y = e.Y;
                pictureBox5.Refresh();
            }
            if (moving && tool == "rectangle")
            {
                int x1 = Math.Min(x, e.X);
                int y1 = Math.Min(y, e.Y);
                int x2 = Math.Max(x, e.X) - Math.Min(x, e.X);
                int y2 = Math.Max(y, e.Y) - Math.Min(y, e.Y);
                rec = new Rectangle(x1, y1, x2, y2);
                pictureBox5.Refresh();
            }
            if (moving && tool == "ellipse")
            {
                int x1 = Math.Min(x, e.X);
                int y1 = Math.Min(y, e.Y);
                int x2 = Math.Max(x, e.X) - Math.Min(x, e.X);
                int y2 = Math.Max(y, e.Y) - Math.Min(y, e.Y);
                elli = new Rectangle(x1, y1, x2, y2);
                pictureBox5.Refresh();
            }
            if (moving && tool == "triangle") {
                triangle_x = e.X;
                triangle_y = e.Y;
                pictureBox5.Refresh();
            }
            if (moving && tool == "star")
            {
                star_x = e.X;
                star_y = e.Y;
                pictureBox5.Refresh();
            }
            if (moving && tool == "eraser")
            {
                gBitmap.DrawRectangle(eraser_pen, x, y, eraser_width, eraser_height);
                SolidBrush brush = new SolidBrush(Color.White);
                gBitmap.FillRectangle(brush, new Rectangle(x, y, eraser_width, eraser_height));
                x = e.X;
                y = e.Y;
                pictureBox5.Refresh();
            }
        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {
            if (tool == "rectangle")
                e.Graphics.DrawRectangle(pen, rec);
            if (tool == "ellipse")
                e.Graphics.DrawEllipse(pen, elli);
            if (tool == "triangle")
            {
                e.Graphics.DrawLine(pen, (x + triangle_x) / 2, Math.Min(y, triangle_y), Math.Min(x, triangle_x), Math.Max(y, triangle_y));
                e.Graphics.DrawLine(pen, (x + triangle_x) / 2, Math.Min(y, triangle_y), Math.Max(x, triangle_x), Math.Max(y, triangle_y));
                e.Graphics.DrawLine(pen, Math.Min(x, triangle_x), Math.Max(y, triangle_y), Math.Max(x, triangle_x), Math.Max(y, triangle_y));
            }
            if (tool == "star")
            {
                e.Graphics.DrawLine(pen, (x + star_x) / 2, Math.Min(y, star_y), Math.Min(x, star_x), Math.Max(y, star_y));
                e.Graphics.DrawLine(pen, (x + star_x) / 2, Math.Min(y, star_y), Math.Max(x, star_x), Math.Max(y, star_y));
                e.Graphics.DrawLine(pen, Math.Min(x, star_x)-10, (y + star_y) / 2, Math.Max(x, star_x), Math.Max(y, star_y));
                e.Graphics.DrawLine(pen, Math.Min(x, star_x)-10, (y + star_y) / 2, Math.Max(x, star_x)+10, (y + star_y) / 2);
                e.Graphics.DrawLine(pen, Math.Max(x, star_x)+10, (y + star_y) / 2, Math.Min(x, star_x), Math.Max(y, star_y));
            }
        }

        private void text_Click(object sender, EventArgs e)
        {
            tool = "text";
        }

        private void pen_Click(object sender, EventArgs e)
        {
            tool = "pen";
        }

        private void brush_Click(object sender, EventArgs e)
        {
            tool = "brush";
        }

        private void triangle_Click(object sender, EventArgs e)
        {
            tool = "triangle";
        }

        private void elli_Click(object sender, EventArgs e)
        {
            tool = "ellipse";
        }

        private void eraser_Click(object sender, EventArgs e)
        {
            tool = "eraser";
        }

        private void rec_Click(object sender, EventArgs e)
        {
            tool = "rectangle";
        }

        private void star_Click(object sender, EventArgs e)
        {
            tool = "star";
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            if (tool == "rectangle")
                gBitmap.DrawRectangle(pen, rec);
            if (tool == "ellipse")
                gBitmap.DrawEllipse(pen, elli);
            if (tool == "triangle")
            {
                gBitmap.DrawLine(pen, (x + triangle_x) / 2, Math.Min(y, triangle_y), Math.Min(x, triangle_x), Math.Max(y, triangle_y));
                gBitmap.DrawLine(pen, (x + triangle_x) / 2, Math.Min(y, triangle_y), Math.Max(x, triangle_x), Math.Max(y, triangle_y));
                gBitmap.DrawLine(pen, Math.Min(x, triangle_x), Math.Max(y, triangle_y), Math.Max(x, triangle_x), Math.Max(y, triangle_y));
            }
            if (tool == "star")
            {
                gBitmap.DrawLine(pen, (x + star_x) / 2, Math.Min(y, star_y), Math.Min(x, star_x), Math.Max(y, star_y));
                gBitmap.DrawLine(pen, (x + star_x) / 2, Math.Min(y, star_y), Math.Max(x, star_x), Math.Max(y, star_y));
                gBitmap.DrawLine(pen, Math.Min(x, star_x)-10, (y + star_y) / 2, Math.Max(x, star_x), Math.Max(y, star_y));
                gBitmap.DrawLine(pen, Math.Min(x, star_x)-10, (y + star_y) / 2, Math.Max(x, star_x)+10, (y + star_y) / 2);
                gBitmap.DrawLine(pen, Math.Max(x, star_x)+10, (y + star_y) / 2, Math.Min(x, star_x), Math.Max(y, star_y));
            }
            if (tool == "text")
            {
                text_x = e.X;
                text_y = e.Y;
                TextBox tb = new TextBox();
                tb.Name = "textBox1";
                tb.Text = " ";
                tb.BorderStyle = BorderStyle.None;
                tb.Location = new Point(text_x, text_y);
                tb.Size = new Size(100, 30);
                tb.BackColor = Color.White;
                pictureBox5.Controls.Add(tb);
            }
        }
    }
}
