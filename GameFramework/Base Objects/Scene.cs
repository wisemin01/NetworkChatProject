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
