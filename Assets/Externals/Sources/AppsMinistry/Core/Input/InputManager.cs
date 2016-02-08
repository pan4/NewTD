using System;
using System.Collections.Generic;

using UnityEngine;

using AppsMinistry.Core.Input.Strategy;
using Scene;

namespace AppsMinistry.Core.Input
{
	public class InputManager : DonDestroyMonoSingleton<InputManager>, IInputEvents
	{
		public event Action<Vector3> OnSwipeStart;
		public event Action<Vector2> OnSwipe;
		public event Action<Vector3> OnSwipeEnd;

		public event Action<Vector3> OnDragStart;
		public event Action<Vector2> OnDrag;
		public event Action<Vector3> OnDragEnd;

		public event Action<Vector3> OnTouchStart;
		public event Action<Gesture> OnTouch;
		public event Action<Vector3> OnTap;
		public event Action<Vector3> OnTouchEnd;

		public event Action<Vector3> OnDoubleTap;
		
		public event Action<float> OnZoom;
		public event Action<Vector3> OnAcceleration;

		//FIXME: A.Shapoval. Check this event later
		public event Action<bool> OnChangeInputState;

		private List<int> _ignoreLayer = new List<int>();
		private List<UnityEngine.Camera> _ignoreCameras = new List<Camera>();
		
		private BaseInputStrategy _inputStrategy = new TouchInputStrategy();

		protected override void OnCreate()
		{
			base.OnCreate();
			
			if(Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_inputStrategy.SetInputController(this);
			
			BindEvents();

			SceneLoader.OnSceneChangedEvent += FindIgnoreCamera;

			EnableInputEvents();
		}

		private void OnEnable()
		{
			if(_inputStrategy != null)
				_inputStrategy.Activate();
		}

		private void OnDisable()
		{
			if(_inputStrategy != null)
				_inputStrategy.Deactivate();
		}
		
		public void ChangeStrategy(BaseInputStrategy newStrategy)
		{
			_inputStrategy.Deactivate();
			
			UnBindEvents();
			
			_inputStrategy = newStrategy;
			_inputStrategy.SetInputController(this).Activate();
			
			BindEvents();
		}
		
		protected override void Update()
		{
			base.Update();
			if(_inputStrategy != null)
				_inputStrategy.Update();
		}

		public void EnableInputEvents()
		{
			EnableInputEvent(InputEvent.Press);
			EnableInputEvent(InputEvent.Move);
			EnableInputEvent(InputEvent.Release);

			EnableInputEvent(InputEvent.Tap);
			EnableInputEvent(InputEvent.DoubleTap);
			EnableInputEvent(InputEvent.Swipe);
			EnableInputEvent(InputEvent.Zoom);

			EnableInputEvent(InputEvent.Acceleration);
			EnableInputEvent(InputEvent.Drag);

			if (OnChangeInputState != null)
				OnChangeInputState(true);
		}

		public void DisableInputEvents()
		{
			DisableInputEvent(InputEvent.Press);
			DisableInputEvent(InputEvent.Move);
			DisableInputEvent(InputEvent.Release);

			DisableInputEvent(InputEvent.Tap);
			DisableInputEvent(InputEvent.DoubleTap);
			DisableInputEvent(InputEvent.Swipe);
			DisableInputEvent(InputEvent.Zoom);

			DisableInputEvent(InputEvent.Acceleration);
			DisableInputEvent(InputEvent.Drag);

			if (OnChangeInputState != null)
				OnChangeInputState(false);
		}

		public void EnableInputEvent(InputEvent inputEvent)
		{
			_inputStrategy.EnableEvent(inputEvent);
		}

		public void DisableInputEvent(InputEvent inputEvent)
		{
			_inputStrategy.DisableEvent(inputEvent);
		}

		public void RegisterIgnoreLayer(int layer)
		{
			AddIgnorLayer(layer);
		}

		public void Clear()
		{

		}

		public void AddIgnorLayer(int layer)
		{
			_ignoreLayer.Add(layer);

			FindIgnoreCamera();
		}

		public void FindIgnoreCamera()
		{
			_ignoreCameras.Clear();

			UnityEngine.Camera[] cameras = GameObject.FindObjectsOfType<UnityEngine.Camera>();

			foreach (UnityEngine.Camera camera in cameras)
				if (_ignoreLayer.Contains(camera.gameObject.layer))
					_ignoreCameras.Add(camera);
		}

		public bool IsTapOnIngnoreLayer(Vector3 pressPosition)
		{
			foreach (UnityEngine.Camera camera in _ignoreCameras)
			{
				if (camera == null)
					continue;

				Ray ray = camera.ScreenPointToRay(new Vector3(pressPosition.x, pressPosition.y, 0));
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					if (_ignoreLayer.Contains(hit.collider.gameObject.layer))
						return true;
				}
			}

			return false;
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			SceneLoader.OnSceneChangedEvent -= FindIgnoreCamera;
		}
		
		private void BindEvents()
		{
			_inputStrategy.OnSwipeStart += Use_OnSwipeStart;
			_inputStrategy.OnSwipe += Use_OnSwipe;
			_inputStrategy.OnSwipeEnd += Use_OnSwipeEnd;
			_inputStrategy.OnDragStart += Use_OnDragStart;
			
			_inputStrategy.OnDrag += Use_OnDrag;
			_inputStrategy.OnDragEnd += Use_OnDragEnd;

			_inputStrategy.OnTouchStart += Use_OnTouchStart;
			_inputStrategy.OnTouch += Use_OnTouchUpdate;
			_inputStrategy.OnTap += Use_OnTap;
			_inputStrategy.OnTouchEnd += Use_OnTouchEnd;

			_inputStrategy.OnDoubleTap += Use_OnDoubleTap;
		
			_inputStrategy.OnZoom += Use_OnZoom;
			_inputStrategy.OnAcceleration += Use_OnAcceleration;
		}
		
		private void UnBindEvents()
		{
			_inputStrategy.OnSwipeStart -= Use_OnSwipeStart;
			_inputStrategy.OnSwipe -= Use_OnSwipe;
			_inputStrategy.OnSwipeEnd -= Use_OnSwipeEnd;
			_inputStrategy.OnDragStart -= Use_OnDragStart;
			
			_inputStrategy.OnDrag -= Use_OnDrag;
			_inputStrategy.OnDragEnd -= Use_OnDragEnd;

			_inputStrategy.OnTouchStart -= Use_OnTouchStart;
			_inputStrategy.OnTouch -= Use_OnTouchUpdate;
			_inputStrategy.OnTap -= Use_OnTap;
			_inputStrategy.OnTouchEnd -= Use_OnTouchEnd;

			_inputStrategy.OnDoubleTap -= Use_OnDoubleTap;
		
			_inputStrategy.OnZoom -= Use_OnZoom;
			_inputStrategy.OnAcceleration -= Use_OnAcceleration;
		}
		
		#region event handlers
				
		protected void Use_OnSwipeStart(Vector3 position)
		{
			if(OnSwipeStart != null)
				OnSwipeStart(position);
		}
		
		protected void Use_OnSwipe(Vector2 swipeDir)
		{
			if(OnSwipe != null)
				OnSwipe(swipeDir);
		}
		
		protected void Use_OnSwipeEnd(Vector3 position)
		{
			if(OnSwipeEnd != null)
				OnSwipeEnd(position);
		}
		
		protected void Use_OnDoubleTap(Vector3 position)
		{
			if(OnDoubleTap != null)
				OnDoubleTap(position);
		}
		
		protected void Use_OnDragStart(Vector3 position)
		{
			if(OnDragStart != null)
				OnDragStart(position);
		}
		
		protected void Use_OnDrag(Vector2 position)
		{
			if(OnDrag != null)
				OnDrag(position);
		}
		
		protected void Use_OnDragEnd(Vector3 position)
		{
			if(OnDragEnd != null)
				OnDragEnd(position);
		}
		
		protected void Use_OnTouchStart(Vector3 position)
		{
			if(OnTouchStart != null)
				OnTouchStart(position);
		}
		
		protected void Use_OnTouchUpdate(Gesture gesture)
		{
			if(OnTouch != null)
				OnTouch(gesture);
		}
		
		protected void Use_OnTouchEnd(Vector3 position)
		{
			if(OnTouchEnd != null)
				OnTouchEnd(position);
		}
		
		protected void Use_OnTap(Vector3 position)
		{
			if(OnTap != null)
				OnTap(position);
		}
		
		protected void Use_OnZoom(float zoom)
		{
			if(OnZoom != null)
				OnZoom(zoom);
		}
		
		protected void Use_OnAcceleration(Vector3 acceleration)
		{
			if(OnAcceleration != null)
				OnAcceleration(acceleration);
		}
				
		#endregion
	}
}

