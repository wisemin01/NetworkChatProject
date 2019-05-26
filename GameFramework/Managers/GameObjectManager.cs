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
        List<GameObject> gameObjectList = new List<GameObject>();

        public T AddObject<T>(T gameObject) where T : GameObject
        {
            gameObjectList.Add(gameObject);
            gameObject.Initialize();

            return gameObject;
        }

        public void UpdateObjects()
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.FrameUpdate();
            }
        }

        public void RenderObjects()
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.FrameRender();
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
