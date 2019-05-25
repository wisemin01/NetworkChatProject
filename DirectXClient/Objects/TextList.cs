using System.Collections.Generic;
using System.Windows.Forms;
using TCPNetwork;
using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;

namespace DirectXClient
{
    class TextList : GameObject, ITextDraw
    {
        private readonly object listLock = new object();

        private int fontSize = 50;
        private Vector3 position = new Vector3(0, 0, 0);
        private string fontName = string.Empty;

        public Vector3 Position { get => position; set => position = value; }

        List<Tuple<string, Color>> list = new List<Tuple<string, Color>>();

        public int FontSize { get => fontSize; set => fontSize = value; }

        public TextList(int fontSize, Vector3 position, string fontName)
        {
            this.fontSize = fontSize;
            this.position = position;
            this.fontName = fontName;
        }

        void ITextDraw.DrawColorText(string text, int r, int g, int b, int a)
        {
            lock (listLock)
            {
                list.Add(new Tuple<string, Color>(text, new Color(r, g, b, a)));
            }
        }

        void ITextDraw.DrawText(string text)
        {
            lock (listLock)
            {
                list.Add(new Tuple<string, Color>(text, new Color(255, 255, 255, 255)));
            }
        }

        void ITextDraw.ClearText()
        {
            lock (listLock)
            {
                list.Clear();
            }
        }

        void ITextDraw.ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

        public override void FrameRender()
        {
            if (FontSize < 0)
                return;

            lock (listLock)
            {
                if (list.Count == 0)
                    return;

                int yIndex = (int)position.Y - list.Count * FontSize;
                int xIndex = (int)position.X;

                foreach (var Iter in list)
                {
                    if (yIndex < 0)
                    {
                        yIndex += FontSize;
                        continue;
                    }
                    else
                    {
                        Direct3D9Manager.Instance.DrawFont(fontName,
                            new Vector3(xIndex, yIndex, 0), Iter.Item1,
                            Iter.Item2);
                        yIndex += FontSize;
                    }
                }
            }
        }

        public override void FrameUpdate()
        {

        }

        public override void Initialize()
        {
        }

        public override void Release()
        {

        }

        public void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

    }
}
