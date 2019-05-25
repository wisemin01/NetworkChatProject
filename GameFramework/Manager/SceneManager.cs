using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        Scene nowScene  = null;
        Scene nextScene = null;

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

            if (scene == null)
                throw new Exception("해당 Scene이 존재하지 않습니다.");

            nextScene = scene;
        }

        public void FrameUpdate()
        {
            if (nextScene != null)
            {
                if (nowScene != null)
                {
                    nowScene.Release();
                }

                nextScene.Initialize();

                nowScene    = nextScene;
                nextScene   = null;
            }

            if (nowScene != null)
            {
                nowScene.FrameUpdate();
            }
        }

        public void FrameRender()
        {
            if (nowScene != null)
            {
                nowScene.FrameRender();
            }
        }

        public void Release()
        {
            if (nowScene != null)
            {
                nowScene.Release();
            }

            scenes.Clear();
        }
    }
}
