﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class Scene
    {
        public Scene() { }

        public abstract void Initialize();
        public abstract void FrameUpdate();
        public abstract void FrameRender();
        public abstract void Release();
    }
}