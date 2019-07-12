using System;
using System.Collections.Generic;

namespace GameFramework.Manager
{
    public class GameObjectManager
    {
        // Singleton
        private static GameObjectManager instance = null;

        public static GameObjectManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObjectManager();

                return instance;
            }
        }

        // Member
        LinkedList<GameObject> gameObjectList = new LinkedList<GameObject>();
        Stack<GameObject> messageBoxStack = new Stack<GameObject>();

        private readonly object locker = new object();

        public T AddObject<T>(T gameObject) where T : GameObject
        {
            lock (locker)
            {
                gameObjectList.AddLast(gameObject);
                gameObject.Initialize();

                return gameObject;
            }
        }
        public void AddMessageBox(GameObject messageBox)
        {
            lock (locker)
            {
                messageBoxStack.Push(messageBox);
                messageBox.Initialize();
            }
        }

        public bool IsMessageBoxPopup
        {
            get
            {
                return (messageBoxStack.Count != 0);
            }
        }

        public void UpdateObjects()
        {
            if (IsMessageBoxPopup)
            {
                if (messageBoxStack.Peek().IsLive == false)
                    messageBoxStack.Pop();
                else
                    messageBoxStack.Peek().FrameUpdate();

                return;
            }

            for (var Iter = gameObjectList.First; Iter != null;)
            {
                if (Iter.Value.IsLive == false)
                {
                    var next = Iter.Next;
                    gameObjectList.Remove(Iter);
                    Iter.Value.Release();
                    Iter = next;
                }
                else
                {
                    Iter.Value.FrameUpdate();
                    Iter = Iter.Next;
                }
            }
        }

        public void RenderObjects()
        {
            try
            {
                foreach (GameObject gameObject in gameObjectList)
                {
                    gameObject.FrameRender();
                }

                foreach (GameObject messageBox in messageBoxStack)
                {
                    messageBox.FrameRender();
                }
            }
            catch (Exception)
            {

            }
        }

        public void ReleaseObjects()
        {
            lock (locker)
            {
                foreach (GameObject gameObject in gameObjectList)
                {
                    gameObject.Release();
                }
                foreach (GameObject messageBox in messageBoxStack)
                {
                    messageBox.Release();
                }

                gameObjectList.Clear();
                messageBoxStack.Clear();
            }
        }
    }
}
