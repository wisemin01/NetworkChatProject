using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace GameFramework.Manager
{
    public partial class Direct3D9Manager
    {
        private DirectInput     directInput;
        private Keyboard        keyboard;
        private Mouse           mouse;

        private MouseState      currentMouseState;
        private KeyboardState   currentKeyboardState;

        private MouseState      prevMouseState;
        private KeyboardState   prevKeyboardState;

        public void InitializeDirectInput()
        {
            directInput = new DirectInput();

            keyboard    = new Keyboard(directInput);
            mouse       = new Mouse(directInput);

            keyboard.Acquire();
            mouse.Acquire();
        }

        public void KeyUpdate()
        {
            prevKeyboardState = currentKeyboardState;
            prevMouseState = currentMouseState;

            currentKeyboardState = keyboard.GetCurrentState();
            currentMouseState = mouse.GetCurrentState();
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
    }
}