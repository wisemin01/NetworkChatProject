using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Windows.Forms;

namespace GameFramework.Manager
{
    public partial class D3D9Manager
    {
        public event EventHandler<ClickChecker> OnMouseClickEvent;
        public event EventHandler<ClickChecker> OnMouseClickToMessageBoxEvent;

        private DirectInput directInput;
        private Keyboard keyboard;
        private Mouse mouse;

        private MouseState mouseState;

        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;

        public void InitializeDirectInput()
        {
            directInput = new DirectInput();

            keyboard = new Keyboard(directInput);
            mouse = new Mouse(directInput);

            keyboard.Acquire();
            mouse.Acquire();

            mainForm.Click += OnMouseClick;
        }

        public void OnMouseClick(object sender, EventArgs e)
        {
            ClickChecker clickChecker = new ClickChecker();

            if (!GameObjectManager.Instance.IsMessageBoxPopup)
            {
                OnMouseClickEvent?.Invoke(sender, clickChecker);
            }
            else
            {
                OnMouseClickToMessageBoxEvent?.Invoke(sender, clickChecker);
            }
        }

        public void KeyUpdate()
        {
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = keyboard.GetCurrentState();

            mouseState = mouse.GetCurrentState();
        }

        public bool IsKeyDown(Key key)
        {
            if (prevKeyboardState == null | currentKeyboardState == null)
                return false;

            if (prevKeyboardState.IsPressed(key) == false &&
                currentKeyboardState.IsPressed(key) == true)
            {
                return true;
            }
            return false;
        }

        public bool IsKeyUp(Key key)
        {
            if (prevKeyboardState == null | currentKeyboardState == null)
                return false;

            if (prevKeyboardState.IsPressed(key) == true &&
               currentKeyboardState.IsPressed(key) == false)
            {
                return true;
            }
            return false;
        }

        public bool IsKeyPress(Key key)
        {
            if (prevKeyboardState == null | currentKeyboardState == null)
                return false;

            if (prevKeyboardState.IsPressed(key) == true &&
                currentKeyboardState.IsPressed(key) == true)
            {
                return true;
            }
            return false;
        }

        public Vector2 MousePosition
        {
            get
            {
                System.Drawing.Point point = Control.MousePosition;
                point = mainForm.PointToClient(point);
                return new Vector2(point.X, point.Y);
            }
        }
    }
}