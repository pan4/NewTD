using System;

using UnityEngine;

namespace AppsMinistry.Core
{
	public class AppSingletonAttribute : Attribute {}

	public class MonoSingleton <T> : MonoBehaviour where T : MonoSingleton <T>
	{
		private static int _levelId;
		private static T _instance;
		private static bool _isQuit;
		protected static bool IsQuit
		{
			get
			{
				return _isQuit;
			}
		}

		private static AppSingletonAttribute singletonAttribute;

		static MonoSingleton ()
		{
			_isQuit = false;
			singletonAttribute = null;
		}

		public static T Instance
		{
			get
			{
				if ( _instance == null && !_isQuit )
				{
					_instance = GameObject.FindObjectOfType <T> ();

					if ( _instance == null )
					{
						GameObject gameObject = new GameObject ( typeof ( T ).Name );
						_instance = gameObject.AddComponent <T> ();

						Attribute[] attributes = System.Attribute.GetCustomAttributes ( typeof ( T ) );
						
						foreach ( Attribute attribute in attributes )
						{
							if ( attribute is AppSingletonAttribute )
								singletonAttribute = attribute as AppSingletonAttribute;
						}

						if ( singletonAttribute != null )
							DontDestroyOnLoad ( _instance );

						gameObject.transform.parent = GetBaseObject ().transform;
					}
				}

				return _instance;
			}
		}

		protected virtual void OnCreate () {}
		private void Awake ()
		{
			OnCreate ();
			_levelId = Application.loadedLevel;
		}

		protected virtual void Update ()
		{
			//if ( Application.isLoadingLevel && singletonAttribute == null )
			if ( singletonAttribute == null && _levelId != Application.loadedLevel )
				Destroy ( gameObject );
		}

		protected virtual void OnReleaseResource () {}
		private void OnDestroy ()
		{
			_instance = null;
			OnReleaseResource ();
		}


        protected virtual void OnQuit() { }
		private void OnApplicationQuit ()

		{
			_isQuit = true;
            OnQuit();
		}

		private static string COMPANY_NAME = "AppsMinistry";
		private static GameObject _baseGameObject;
		public static GameObject GetBaseObject ()
		{
			if ( _baseGameObject == null )
			{
				_baseGameObject = GameObject.Find ( COMPANY_NAME );

				if ( _baseGameObject == null )
				{
					_baseGameObject = new GameObject ( COMPANY_NAME );
					DontDestroyOnLoad ( _baseGameObject );
				}
			}

			return _baseGameObject;
		}
	}


	public class MonoSingletonWithUpdate <T> : MonoSingleton <T> where T:MonoSingleton <T>
	{
		private float _updateTime;
		private float _nextUpdate;

		private float _scheduleUpdateTime;
		private float _nextScheduleUpdate;
		private bool _isScheduleLoop;
		private bool _isScheduleUpdate;

		protected float CurrentTime 
		{
			get
			{
				return Time.time;
			}
		}

		public MonoSingletonWithUpdate ( float updateTime ) : base ()
		{
			_updateTime = updateTime;
			//_nextUpdate = CurrentTime + _updateTime;
		}

		protected virtual void OnUpdate () {}
		protected virtual void OnScheduleUpdate () {}
		protected sealed override void Update ()
		{
			base.Update ();

			if ( _nextUpdate <= CurrentTime )
			{
				OnUpdate ();

				_nextUpdate = CurrentTime + _updateTime;
			}

			if ( _isScheduleUpdate && _nextScheduleUpdate <= CurrentTime )
			{
				OnScheduleUpdate ();

				if ( _isScheduleLoop )
					_nextScheduleUpdate = CurrentTime + _scheduleUpdateTime;
				else
					_isScheduleUpdate = false;
			}
		}

		public void ScheduleUpdate ( float time, bool loop = false )
		{
			_scheduleUpdateTime = time;
			_isScheduleLoop = loop;

			_nextScheduleUpdate = CurrentTime + _scheduleUpdateTime;
			_isScheduleUpdate = true;
		}
	}

	public class ManualMonoSingleton <T> : MonoBehaviour where T : ManualMonoSingleton <T>
	{
		private static T _instance;

		public static T ManualInstance
		{
			get
			{
				return _instance;
			}
		}
		
		protected virtual void OnCreate () {}
		private void Awake ()
		{
			if (_instance != null) 
			{
				Debug.LogErrorFormat("Instance of {0} already exists. GameObject \"{1}\" will be destroyed.", GetType().ToString(), gameObject.name);
				DestroyImmediate(this);

				return;
			}

			_instance = GetComponent<T>();

			OnCreate ();
		}
		
		protected virtual void OnReleaseResource () {}
		private void OnDestroy ()
		{
			_instance = null;
			OnReleaseResource ();
		}
	}

	public class DonDestroyMonoSingleton<T> : MonoBehaviour where T : DonDestroyMonoSingleton<T>
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<T>();

					if (_instance == null)
					{
						GameObject gameObject = new GameObject(typeof(T).Name);
						_instance = gameObject.AddComponent<T>();
						DontDestroyOnLoad(_instance);

						gameObject.transform.parent = GetBaseObject().transform;
					}
				}

				return _instance;
			}
		}

		protected virtual void OnCreate() { }
		private void Awake()
		{
			OnCreate();
		}

		protected virtual void OnReleaseResource() { }
		private void OnDestroy()
		{
			OnReleaseResource();
		}
		//
		protected virtual void Update()
		{
		
		}

		private static string COMPANY_NAME = "AppsMinistry";
		private static GameObject _baseGameObject;
		public static GameObject GetBaseObject()
		{
			if (_baseGameObject == null)
			{
				_baseGameObject = GameObject.Find(COMPANY_NAME);

				if (_baseGameObject == null)
					_baseGameObject = new GameObject(COMPANY_NAME);
			}

			return _baseGameObject;
		}
	}
}

