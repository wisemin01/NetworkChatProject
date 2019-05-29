using System;
using System.Collections.Generic;

namespace GameFramework.Manager
{
    public class SceneManager
    {
        // Singleton
        private static SceneManager instance = null;

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();

                return instance;
            }
        }

        // Member
        private readonly Dictionary<string, Scene> scenes
            = new Dictionary<string, Scene>();

        Scene nowScene = null;
        Scene nextScene = null;

        public event EventHandler<string> OnChangeSceneEvent;

        public void AddScene<T>(string key) where T : Scene, new()
        {
            if (scenes.ContainsKey(key))
            {
                throw new Exception("이미 해당 키를 가진 Scene이 존재합니다.");
            }

            T newScene = new T();

            scenes.Add(key, newScene);
        }

        public void ChangeScene(string key)
        {
            Scene scene = scenes[key];
            nextScene = scene ?? throw new Exception("해당 Scene이 존재하지 않습니다.");
            OnChangeSceneEvent.Invoke(this, key);
        }

        public void FrameUpdate()
        {
            if (nextScene != null)
            {
                if (nowScene != null)
                {
                    nowScene.Release();
                    GameObjectManager.Instance.ReleaseObjects();
                }

                nextScene.Initialize();

                nowScene = nextScene;
                nextScene = null;
            }

            if (nowScene != null)
            {
                nowScene.FrameUpdate();
                GameObjectManager.Instance.UpdateObjects();
            }
        }

        public void FrameRender()
        {
            if (nowScene != null)
            {
                nowScene.FrameRender();
                GameObjectManager.Instance.RenderObjects();
            }
        }

        public void Release()
        {
            if (nowScene != null)
            {
                nowScene.Release();
                GameObjectManager.Instance.ReleaseObjects();
            }

            scenes.Clear();
            OnChangeSceneEvent = null;
        }
    }
}
