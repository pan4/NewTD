using System;

using UnityEngine;

namespace Scene
{
    public class LoadingSceneAttribute : Attribute { }

    public class SceneNameAttribute : Attribute
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private bool _isMultiplayer;
        public bool IsMultiplayer
        {
            get { return _isMultiplayer; }
        }

        public SceneNameAttribute(string name, bool isMultiplayer = false)
        {
            _name = name;
            _isMultiplayer = isMultiplayer;
        }
    }

    public class LeaderBoardIDAttribute : Attribute
    {
        private string _leaderBoardId;
        public string LeaderBoardId
        {
            get { return _leaderBoardId; }
        }

        public LeaderBoardIDAttribute(string leaderBoardId)
        {
            _leaderBoardId = leaderBoardId;
        }
    }

    public class SceneController : MonoBehaviour
    {
        protected virtual void OnLoad() { }
        private void Awake()
        {
            OnLoad();
        }

        protected virtual void OnLoaded() { }
        private void Start()
        {
            OnLoaded();
        }

        protected virtual void OnClose() { }
        private void OnDestroy()
        {
            OnClose();
        }

        public virtual void OnRespawn() { }
    }
}
