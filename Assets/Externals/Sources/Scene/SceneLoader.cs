using AppsMinistry.Core.Input;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Scene
{
    public static class SceneLoader
    {
        public static event Action OnSceneChangedEvent;
        public static event Action OnSceneReload;

        public static bool IsNextActionLevelLoading;

        private static Type _baseLoadingScene;

        private static List<Type> _prevScenes;

        private static Type _nextScene;

        private static AsyncOperation _currentAsyncOperation;
        public static AsyncOperation CurrentAsyncOperation
        {
            get
            {
                return _currentAsyncOperation;
            }
        }

        static SceneLoader()
        {
            //_nextScene = typeof ( MainSceneController );
            //_baseLoadingScene = typeof ( LoadingSceneController );

            //_prevScenes = new List <Type> ();
            //_prevScenes.Add ( typeof ( MainSceneController ) );
            _prevScenes = new List<Type>();
        }

        public static void LoadScene<T>(bool withLoader = false) where T : SceneController
        {
            if (withLoader)
                LoadScene(_baseLoadingScene, typeof(T));
            else
                LoadScene(typeof(T));
        }

        public static void ReloadScene()
        {
            if (OnSceneReload != null)
                OnSceneReload();

            LoadScene(_baseLoadingScene, _nextScene);
        }

        public static void LoadScene(Type nextScene, bool withLoader = false, bool isPrev = false, bool isAsync = false)
        {
            if (withLoader)
            {
                LoadScene(_baseLoadingScene, nextScene);
            }
            else
            {
                InputManager.Instance.Clear();

                Attribute[] attributes = System.Attribute.GetCustomAttributes(nextScene);

                SceneNameAttribute sceneNameAttribute = null;

                foreach (Attribute attribute in attributes)
                    if (attribute is SceneNameAttribute)
                        sceneNameAttribute = attribute as SceneNameAttribute;

                if (isAsync)
                    _currentAsyncOperation = Application.LoadLevelAsync(sceneNameAttribute.Name);
                else
                    Application.LoadLevel(sceneNameAttribute.Name);

                if (!isPrev && (_prevScenes.Count == 0 || _prevScenes[_prevScenes.Count - 1] != _nextScene))
                    _prevScenes.Add(_nextScene);

                _nextScene = nextScene;
            }
        }

        private static void LoadScene(Type loadingScene, Type nextScene, bool isPrev = false, bool isAsync = false)
        {
            InputManager.Instance.Clear();
            IsNextActionLevelLoading = true;

            if (loadingScene == null)
            {
                LoadScene(nextScene);
                return;
            }

            Attribute[] attributes = System.Attribute.GetCustomAttributes(loadingScene);

            SceneNameAttribute sceneNameAttribute = null;

            bool isLoadingScene = false;

            foreach (Attribute attribute in attributes)
            {
                if (attribute is SceneNameAttribute)
                    sceneNameAttribute = attribute as SceneNameAttribute;

                if (attribute is LoadingSceneAttribute)
                    isLoadingScene = true;
            }

            if (isLoadingScene)
            {
                Time.timeScale = 1.0f;

                if (!isPrev && (_prevScenes.Count == 0 || _prevScenes[_prevScenes.Count - 1] != _nextScene))
                    _prevScenes.Add(_nextScene);

                _nextScene = nextScene;

                if (isAsync)
                    _currentAsyncOperation = Application.LoadLevelAsync(sceneNameAttribute.Name);
                else
                    Application.LoadLevel(sceneNameAttribute.Name);
            }
            else
            {
                LoadScene(nextScene);
            }
        }

        public static void LoadPrevScene(bool withLoader = false)
        {
            Type prevScene = null;//typeof(EndlessSceneController);
            if (_prevScenes.Count > 0)
            {
                prevScene = _prevScenes[_prevScenes.Count - 1];
                _prevScenes.RemoveAt(_prevScenes.Count - 1);
            }

            if (!withLoader)
                LoadScene(prevScene, false, true);
            else
                LoadScene(_baseLoadingScene, prevScene, true);
        }

        public static void LoadFromPrev<TNext>(bool withLoader = false) where TNext : SceneController
        {
            int findIndex = -1;
            for (int index = _prevScenes.Count - 1; index >= 0; index--)
            {
                if (_prevScenes[index].Equals(typeof(TNext)))
                {
                    findIndex = index;
                    break;
                }
            }

            _prevScenes.RemoveRange(findIndex + 1, _prevScenes.Count - findIndex - 1);

            LoadPrevScene(withLoader);
        }

        public static void LoadNextScene(bool isAsync = false)
        {
            LoadScene(_nextScene, false, false, isAsync);
        }

        //     public static void LoadAdditiveScene(bool isAsync = false)
        //     {
        //         Attribute[] attributes = System.Attribute.GetCustomAttributes(DailySceneManager.GetDaily().Logic);

        //         SceneNameAttribute sceneNameAttribute = null;

        //         foreach (Attribute attribute in attributes)
        //             if (attribute is SceneNameAttribute)
        //                 sceneNameAttribute = attribute as SceneNameAttribute;

        //if (isAsync)
        //	_currentAsyncOperation = Application.LoadLevelAdditiveAsync(sceneNameAttribute.Name);
        //else
        //	Application.LoadLevelAdditive(sceneNameAttribute.Name);
        //     }

        public static void OnSceneChanged()
        {
            IsNextActionLevelLoading = false;
            if (OnSceneChangedEvent != null)
                OnSceneChangedEvent();
        }
    }
}

