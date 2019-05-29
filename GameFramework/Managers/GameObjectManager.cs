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

        public T AddObject<T>(T gameObject) where T : GameObject
        {
            gameObjectList.AddLast(gameObject);
            gameObject.Initialize();

            return gameObject;
        }
        public void AddMessageBox(GameObject messageBox)
        {
            messageBoxStack.Push(messageBox);
            messageBox.Initialize();
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
            if (messageBoxStack.Count != 0)
            {
                if (messageBoxStack.Peek().IsLive == false)
                    messageBoxStack.Pop();
                else
                    messageBoxStack.Peek().FrameUpdate();

                return;
            }

            for (LinkedListNode<GameObject> Iter = gameObjectList.First; Iter != null;)
            {
                if (Iter.Value.IsLive == false)
                {
                    LinkedListNode<GameObject> next = Iter.Next;
                    gameObjectList.Remove(Iter);
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
