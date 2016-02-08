using UnityEngine;
using System;
using System.Collections.Generic;

namespace AppsMinistry.Core.Input.Strategy
{
    public abstract class BaseInputStrategy : IInputControllerStrategy, IInputEvents {
		
		#region interface events
		
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
		
		#endregion
		
		protected List<InputEvent> _enableEvents = new List<InputEvent>();
		protected InputManager _inputManager;
		
		public BaseInputStrategy SetInputController(InputManager inputManager)
		{
			_inputManager = inputManager;
			return this;
		}
		
		#region interface implementation
		public virtual void Activate()
		{
			
		}
		
		public virtual void Deactivate()
		{
			
		}
		
		public virtual void Update()
		{
			
		}
		
		public void EnableEvent(InputEvent inputEvent)
		{
			if (!_enableEvents.Contains(inputEvent))
				_enableEvents.Add(inputEvent);
		}
		
		public void DisableEvent(InputEvent inputEvent)
		{
			if (_enableEvents.Contains(inputEvent))
				_enableEvents.Remove(inputEvent);
		}
		
		public bool IsEventEnabled(InputEvent inputEvent)
		{
			return _enableEvents.Contains(inputEvent);
		}
		#endregion
		
		#region event call
		
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