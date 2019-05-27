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

        public T AddObject<T>(T gameObject) where T : GameObject
        {
            gameObjectList.AddLast(gameObject);
            gameObject.Initialize();

            return gameObject;
        }

        public void UpdateObjects()
        {
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

            gameObjectList.Clear();
        }
    }
}
