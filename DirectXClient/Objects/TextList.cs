﻿using GameFramework;
using GameFramework.Manager;
using SharpDX;
using System;
using System.Collections.Generic;

namespace DirectXClient
{
    public class TextList : GameObject
    {
        private readonly object listLock = new object();
        private readonly string fontName = string.Empty;

        public int renderTop { get; set; } = 200;

        List<Tuple<string, Color>> list = new List<Tuple<string, Color>>();

        public int TextDepth { get; set; } = 50;

        public TextList(int textDepth, Vector3 position, string fontName)
        {
            this.TextDepth = textDepth;
            this.Position = position;
            this.fontName = fontName;
        }
        public TextList(string fontName)
        {
            this.fontName = fontName;
        }

        public void DrawColorText(string text, int r, int g, int b, int a)
        {
            lock (listLock)
            {
                list.Add(new Tuple<string, Color>(text, new Color(r, g, b, a)));
            }
        }

        public void DrawText(string text)
        {
            lock (listLock)
            {
                list.Add(new Tuple<string, Color>(text, new Color(255, 255, 255, 255)));
            }
        }

        public void ClearText()
        {
            lock (listLock)
            {
                list.Clear();
            }
        }

        void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

        public override void FrameRender()
        {
            if (TextDepth < 0)
                return;

            lock (listLock)
            {
                if (list.Count == 0)
                    return;

                int yIndex = (int)Position.Y - list.Count * TextDepth;
                int xIndex = (int)Position.X;

                foreach (Tuple<string, Color> Iter in list)
                {
                    if (yIndex < renderTop)
                    {
                        yIndex += TextDepth;
                        continue;
                    }
                    else
                    {
                        D3D9Manager.Instance.DrawFont(fontName,
                            new Vector3(xIndex, yIndex, 0), Iter.Item1,
                            Iter.Item2);
                        yIndex += TextDepth;
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
    }
}
