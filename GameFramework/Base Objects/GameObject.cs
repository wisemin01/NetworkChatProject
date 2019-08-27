using SharpDX;

namespace GameFramework
{
    public abstract class GameObject
    {
        private bool isLive = true;
        private bool isActive = true;

        public GameObject Parent { get; set; } = null;

        public bool IsLive { get => isLive; }

        public bool IsActive {
            get {
                if (Parent != null && Parent.IsActive == false)
                    return false;

                return isActive;
            }
            set {
                if (isActive != value)
                {
                    isActive = value;

                    if (isActive == true)
                        OnEnable();
                    else
                        OnDisable();
                }
            }
        }

        public Vector3 Position { get; set; } = default;
        public Vector3 Scale { get; set; } = Vector3.One;

        public GameObject()
        {
        }

        public abstract void Initialize();
        public abstract void FrameUpdate();
        public abstract void FrameRender();
        public abstract void Release();

        public void Destroy(GameObject target)
        {
            if (target != null)
            {
                target.isLive = false;
            }
        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual Matrix GetWorldMatrix()
        {
            Matrix local = Matrix.Scaling(Scale) * Matrix.Translation(Position);

            if (Parent != null)
                local *= Parent.GetWorldMatrix();

            return local;
        }
    }
}
