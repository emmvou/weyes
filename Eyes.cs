using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weyes
{
    internal class Eyes : Panel
    {
        private bool left_wink;
        private bool right_wink;

        public Eyes()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override async void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            await Task.Delay(100);

            if (MouseButtons == MouseButtons.Left)
            {
                var local = e.X;
                Debug.WriteLine(local);

                var height = ClientSize.Height;
                var width = ClientSize.Width;

        
                if (local >= width / 2)
                {
                    right_wink = true;
                    //left_wink = false;

                }
                else
                {
                    left_wink = true;
                    //right_wink = false;

                }
            }
            else
            {
                //left_wink = false;
                //right_wink = false;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            left_wink = false;
            right_wink = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var height = ClientSize.Height;
            var width = ClientSize.Width;

            var eyes_padding = (width / 50, height / 50);
            var eyes_width = (width - eyes_padding.Item1) / 2;
            (int w, int h) eyes_border = (width / 40, height / 20);
            var pupil_thickness = (eyes_border.w * 6, eyes_border.h * 6);
            var spacing_fac = 10;

            void draw_eye(float x)
            {
                //drawing eyes border
                e.Graphics.FillEllipse(Brushes.Black, x, 0, eyes_width, height);
                e.Graphics.FillEllipse(Brushes.White, x + eyes_border.w, eyes_border.h, eyes_width - 2 * eyes_border.w, height - 2 * eyes_border.h);

                (float x, float y) eye_center = (x + eyes_width / 2, height / 2);
                var pointer_position = PointToClient(Cursor.Position);
                (float x, float y) mouse_rel = (pointer_position.X - eye_center.x, pointer_position.Y - eye_center.y);

                var rx = (eyes_width - eyes_border.w * spacing_fac) / 2f;
                var ry = (height - eyes_border.h * spacing_fac) / 2f;
                var chk = Math.Pow(mouse_rel.x / rx, 2) + Math.Pow(mouse_rel.y / ry, 2);
                double px, py;
                if (chk <= 1)
                {
                    px = mouse_rel.x;
                    py = mouse_rel.y;
                }
                else
                {
                    var mouse_ang = Math.Atan2(mouse_rel.y / ry, mouse_rel.x / rx);

                    px = Math.Cos(mouse_ang) * rx;
                    py = Math.Sin(mouse_ang) * ry;
                }

                e.Graphics.FillEllipse(Brushes.Black,
                    (float)(eye_center.x + px - pupil_thickness.Item1 / 2f),
                    (float)(eye_center.y + py - pupil_thickness.Item2 / 2f),
                    pupil_thickness.Item1, pupil_thickness.Item2);
            }

            void draw_wink(float x)
            {
                e.Graphics.FillEllipse(Brushes.Black, x, 0, eyes_width, height);
                e.Graphics.FillEllipse(Brushes.Yellow, x + eyes_border.w, eyes_border.h, eyes_width - 2 * eyes_border.w, height - 2 * eyes_border.h);
                //on veut une ligne qui est dans le cercle noir (voire le jaune au mieux)
                e.Graphics.DrawLine(new Pen(Color.Black, eyes_border.h), new Point((int)x, height/2), new Point((int)x + eyes_width, height / 2 + eyes_border.h * (x < ClientSize.Width / 2 ? -1 : 1))) ;
            }

            if(right_wink)
            {
                draw_wink(width - eyes_width);
            }
            else
            {
                draw_eye(width - eyes_width);
            }

            if(left_wink)
            {
                draw_wink(0);
            }
            else
            {
                draw_eye(0);
            }
/*
            var bmp = new Bitmap(width, height);

            using (var g = Graphics.FromImage(bmp))
            {
               
            }
            e.Graphics.DrawImageUnscaled(bmp, new Point(0, 0));
            
            var ico = Icon.FromHandle(bmp.GetHicon());
            ((Form)Parent).Icon = ico;*/
        }
    }
}
