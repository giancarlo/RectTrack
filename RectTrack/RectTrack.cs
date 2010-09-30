using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace RectTrack
{

    public enum CursorPosition : byte
    {
        NONE, N, NE, E, SE, S, SW, W, NW, INSIDE, OUTSIDE
    }

    public class Tracker : ContainerControl
    {
        Control _control;
        Bitmap _bm;
        Point _mouseStart;
        Point _offset;
        CursorPosition _mouseDown = CursorPosition.NONE;
        Size _gridsize;
        Boolean _snapToGrid;

        int _handleSize = 7;
        int _border = 3;

        public Tracker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            GridSize = new Size(8, 8);
            SnapToGrid = false;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            OffSet = new Point(0, 0);
        }

        public Point OffSet
        {
            get { return _offset; }
            set { _offset = value; }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                Parent.MouseMove += this.OnParentMouseMove;
                Parent.MouseUp += this.OnParentMouseUp;
            }
        }

        int snapX(int x)
        {
            return SnapToGrid ? x - x % GridSize.Width : x;
        }

        int snapY(int y)
        {
            return SnapToGrid ? y - y % GridSize.Height : y;
        }

        protected void ResizeW(int newx)
        {
            int d = snapX(newx - _mouseStart.X);
            Width -= d - Left;
            Left = d;
        }

        protected void ResizeE(int newx)
        {
            Width = snapX(newx - Left);
        }

        protected void ResizeS(int newY)
        {
            Height = snapY(newY - Top);
        }

        protected void ResizeN(int newY)
        {
            int d = snapY(newY - _mouseStart.Y);
            Height -= d - Top;
            Top = d;
        }

        protected void DoMove(int newX, int newY)
        {
            Left = snapX(newX - _mouseStart.X);
            Top = snapY(newY - _mouseStart.Y);
        }

        protected void OnParentMouseMove(Object sender, MouseEventArgs e)
        {
            switch (_mouseDown)
            {
                case CursorPosition.NONE: return;
                case CursorPosition.INSIDE: DoMove(e.X, e.Y); break;
                case CursorPosition.W: ResizeW(e.X); break;
                case CursorPosition.E: ResizeE(e.X); break;
                case CursorPosition.S: ResizeS(e.Y); break;
                case CursorPosition.N: ResizeN(e.Y); break;
                case CursorPosition.NE: ResizeN(e.Y); ResizeE(e.X); break;
                case CursorPosition.NW: ResizeN(e.Y); ResizeW(e.X); break;
                case CursorPosition.SE: ResizeS(e.Y); ResizeE(e.X); break;
                case CursorPosition.SW: ResizeS(e.Y); ResizeW(e.X); break;
            }
            Parent.Refresh();
        }

        // Make Object Transparent
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        public CursorPosition GetCursorPosition(int x, int y)
        {
            int bw = HandleSize;
            if (x < bw)
            {
                if (y <= bw)
                    return CursorPosition.NW;
                else if (y >= Height - bw)
                    return CursorPosition.SW;
                else
                    return CursorPosition.W;
            }
            else if (x >= Width - bw)
            {
                if (y <= bw)
                    return CursorPosition.NE;
                else if (y >= Height - bw)
                    return CursorPosition.SE;
                else
                    return CursorPosition.E;
            }
            else if (y < bw)
                return CursorPosition.N;
            else if (y >= Height - bw)
                return CursorPosition.S;

            return CursorPosition.INSIDE;
        }

        /**
         * Gets the Mouse cursor based on the x and y position.
         */
        Cursor GetCursor(int x, int y)
        {
            switch (GetCursorPosition(x, y))
            {
                case CursorPosition.S:
                case CursorPosition.N:
                    return Cursors.SizeNS;
                case CursorPosition.E:
                case CursorPosition.W:
                    return Cursors.SizeWE;
                case CursorPosition.NW:
                case CursorPosition.SE:
                    return Cursors.SizeNWSE;
                case CursorPosition.NE:
                case CursorPosition.SW:
                    return Cursors.SizeNESW;
            }
            return Cursors.SizeAll;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Cursor = GetCursor(e.X, e.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_mouseDown == CursorPosition.NONE)
            {
                Parent.Capture = true;
                _mouseStart = new Point(e.X, e.Y);
                _mouseDown = GetCursorPosition(e.X, e.Y);
                CreateBitmap();
                Invalidate();
            }

        }

        protected void OnParentMouseUp(object sender, MouseEventArgs e)
        {
            if (_mouseDown != CursorPosition.NONE)
            {
                Control.SetBounds(Left + BorderWidth - OffSet.X, Top + BorderWidth - OffSet.Y, Width - BorderWidth * 2, Height - BorderWidth * 2);
                Parent.Capture = false;
                _mouseDown = CursorPosition.NONE;
                Parent.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            DrawBorder(pevent.Graphics);
            DrawRect(pevent);
        }

        public int BorderWidth
        {
            get { return _border; }
            set { _border = value; }
        }

        public int HandleSize
        {
            get { return _handleSize; }
            set { _handleSize = value; }
        }

        public Control Control
        {
            get { return _control; }
            set
            {
                // Set Cursor To Normal
                if (_control != null)
                {
                    Parent.Invalidate(Bounds);
                }
                _control = value;
                if (_control == null)
                    Visible = false;
                else
                {
                    Visible = true;
                    SetPosition();
                }
            }
        }

        public void SetPosition()
        {
            if (Control != null)
            {
                //SelectedControl.Parent.Invalidate(SelectedControl.Bounds);

                SetBounds(
                    OffSet.X + (int)Control.Left - BorderWidth,
                    OffSet.Y + (int)Control.Top - BorderWidth,
                    (int)Control.Width + BorderWidth * 2,
                    (int)Control.Height + BorderWidth * 2
                );

                /*if (Parent == Sprite.Parent)
                    Parent.Controls.SetChildIndex(this, Parent.Controls.GetChildIndex(Sprite) - 1);*/

                Invalidate();
            }
        }

        private void SetPositionHandler(Object sender, EventArgs e)
        {
            SetPosition();
        }

        protected void CreateBitmap()
        {
            _bm = new Bitmap((int)Control.Width, (int)Control.Height);
            _control.DrawToBitmap(_bm, new Rectangle(0, 0, (int)Control.Width, (int)Control.Height));
        }

        protected void DrawBorder(Graphics g)
        {
            /*    g.FillRectangle(Brushes.Silver, new Rectangle(0, 0, BorderWidth, Height));
                g.FillRectangle(Brushes.Silver, new Rectangle(0, 0, Width, BorderWidth));
                g.FillRectangle(Brushes.Silver, new Rectangle(0, Height - BorderWidth, Width, BorderWidth));
                g.FillRectangle(Brushes.Silver, new Rectangle(Width - BorderWidth, 0, BorderWidth, Height));                */
        }

        public Rectangle ControlBounds
        {
            get
            {
                return (Control != null) ? new Rectangle(BorderWidth, BorderWidth, (int)Control.Width, (int)Control.Height) : Rectangle.Empty;
            }
        }

        protected void DrawRect(PaintEventArgs e)
        {
            int bw = BorderWidth;
            int hs = HandleSize;

            Rectangle r = new Rectangle(0, 0, Width, Height);
            Rectangle cr = ControlBounds;

            if (_mouseDown != CursorPosition.NONE)
                e.Graphics.DrawImage(Image.FromHbitmap(_bm.GetHbitmap()), cr);

            ControlPaint.DrawSelectionFrame(e.Graphics, true, r, cr, Color.DarkGray);

            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(0, 0, hs, hs), true, true);
            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(0, (Height - hs) / 2, hs, hs), true, true);
            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(0, Height - hs, hs, hs), true, true);

            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(Width - hs, 0, hs, hs), true, true);
            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(Width - hs, (Height - hs) / 2, hs, hs), true, true);
            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle(Width - hs, Height - hs, hs, hs), true, true);

            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle((Width - hs) / 2, 0, hs, hs), true, true);
            ControlPaint.DrawGrabHandle(e.Graphics, new Rectangle((Width - hs) / 2, Height - hs, hs, hs), true, true);

        }

        public Size GridSize
        {
            get { return _gridsize; }
            set { _gridsize = value; }
        }

        public Boolean SnapToGrid
        {
            get { return _snapToGrid; }
            set { _snapToGrid = value; }
        }

    }

}