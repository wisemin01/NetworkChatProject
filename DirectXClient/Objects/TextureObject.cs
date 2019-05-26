﻿using GameFramework;
using GameFramework.Manager;
using GameFramework.Structure;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectXClient
{
    class TextureObject : GameObject
    {
        public GameTexture Texture { get; set; } = null;
        public Vector3 Position { get; set; } = default;
        public Vector3 Scale { get; set; } = default;
        public float Rotation { get; set; } = 0;
        public override void FrameRender()
        {
            D3D9Manager.Instance.DrawTexture(Texture, Position, Scale, Rotation);
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