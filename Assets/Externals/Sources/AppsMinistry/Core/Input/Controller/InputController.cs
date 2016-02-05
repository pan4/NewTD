//
// InputController.cs
//
// Author:
//       Maksym Pyvovarchuk <mpyvovarchuk@appsministry.com>
//
// Created:
//       5/19/2014
//
// Copyright (c) 2014 Maksym Pyvovarchuk

using System;
using System.Collections.Generic;

using UnityEngine;


namespace AppsMinistry.Core.Input
{
	public abstract class InputController : IInputController
	{
		protected virtual float DoubleTapTime
		{
			get
			{
				return 0.25f;
			}
		}

		private const float DETECT_SWIPE_TIME = 0.1f;
		private float _lastDetectSwipe;

		protected virtual float MinSwipeDistance
		{
			get
			{
				return 10f;
			}
		}

		protected virtual float ZoomSensetive
		{
			get
			{
				return 0.005f;
			}
		}

		protected float CurrentTime
		{
			get
			{
				return Time.unscaledTime;
			}
		}

		protected virtual float MinDragTime
		{
			get
			{
				return 0.6f;
			}
		}

		protected InputEvent _currentState;
		protected List<InputEvent> _availebleEvents;
		public List<InputEvent> AvailebleEvents
		{
			get
			{
				return _availebleEvents;
			}
		}

		protected float _lastTapTime;

		protected Vector3 _pressPosition;
		protected float _pressTime;

		protected Vector3 _lastSwipePosition;
		protected Vector2 _swipeVelocity;

		protected float _baseZoomDistance;

		protected Vector3 _previousDragPos = Vector3.zero;
		protected float MaxDrag
		{
			get
			{
				return 15f;
			}
		}

		public InputController()
		{
			_availebleEvents = new List<InputEvent>();

			_lastTapTime = 0;
			_currentState = InputEvent.None;

			_swipeVelocity = Vector2.zero;
		}

		#region IInputController implementation

		public event Action<Vector3> OnPressEvent;
		public event Action<Vector3> OnMoveEvent;
		public event Action<Vector3> OnReleaseEvent;

		public event Action<Vector3> OnTapEvent;
		public event Action<Vector3> OnDoubleTapEvent;
		public event Action<Vector2> OnSwipeEvent;
		public event Action<float> OnZoomEvent;

		public event Action<Vector3> OnAccelerationEvent;
		public event Action<Vector3> OnDragEvent;

		public abstract void OnUpdate();

		public void EnableEvent(InputEvent inputEvent)
		{
			if (!_availebleEvents.Contains(inputEvent))
				_availebleEvents.Add(inputEvent);
		}

		public void DisableEvent(InputEvent inputEvent)
		{
			if (_availebleEvents.Contains(inputEvent))
				_availebleEvents.Remove(inputEvent);
		}

		public void Clear()
		{
			_lastTapTime = 0;
			_currentState = InputEvent.None;
			_pressPosition = Vector3.zero;
			_lastSwipePosition = Vector3.zero;
			_lastDetectSwipe = 0;
			_pressTime = 0;
		}

		#endregion

		protected void Tap(Vector3 position)
		{
			_currentState = InputEvent.Tap;
			if (CurrentTime - _lastTapTime <= DoubleTapTime && _availebleEvents.Contains(InputEvent.DoubleTap))
				_currentState = InputEvent.DoubleTap;

			_lastTapTime = CurrentTime;

			_pressPosition = position;
			_lastSwipePosition = _pressPosition;
			_pressTime = CurrentTime;
			_lastDetectSwipe = CurrentTime + DETECT_SWIPE_TIME;

			if (!InputManager.Instance.IsTapOnIngnoreLayer(position))
				OnPress(position);
		}

		protected void Swipe(Vector3 position)
		{
			float swipeDistance = (position - _lastSwipePosition).magnitude;

			if (swipeDistance > MinSwipeDistance)
			{
				_currentState = InputEvent.Swipe;
				_swipeVelocity = (position - _lastSwipePosition);
			}
			else
				_swipeVelocity = Vector2.zero;
		}

		protected void Drag(Vector3 position)
		{
			if (CurrentTime - _pressTime < MinDragTime)
				return;

			if (_previousDragPos == Vector3.zero)
				_previousDragPos = position;

			Vector3 delta = position - _previousDragPos;

			Vector3 drag = new Vector3(delta.x, 0, delta.y);
			drag.x = Mathf.Abs(drag.x) < MaxDrag ? drag.x : MaxDrag * Mathf.Sign(drag.x);
			drag.z = Mathf.Abs(drag.z) < MaxDrag ? drag.z : MaxDrag * Mathf.Sign(drag.z);

			_previousDragPos = position;

			OnDrag(drag);
		}

		protected void EndInput(Vector3 position)
		{
			if (InputManager.Instance.IsTapOnIngnoreLayer(position))
				_currentState = InputEvent.None;
			else
			{
				if (_currentState != InputEvent.None && _currentState != InputEvent.Zoom)
					Swipe(position);

				if (CurrentTime - _pressTime > MinDragTime && OnDragEvent != null)
					_previousDragPos = Vector3.zero;
				else if (_currentState == InputEvent.Swipe)
					OnSwipe(_swipeVelocity);
				else if (_currentState == InputEvent.Tap)
					OnTap(position);
				else if (_currentState == InputEvent.DoubleTap)
					OnDoubleTap(position);
				else if (_currentState == InputEvent.Zoom)
					_currentState = InputEvent.None;
			}

			OnRelease(position);
		}

		protected void OnPress(Vector3 position)
		{
			if (OnPressEvent != null && _availebleEvents.Contains(InputEvent.Press))
			{
				OnPressEvent(position);
			}
		}

		protected void OnMove(Vector3 position)
		{
			if (_lastDetectSwipe <= CurrentTime)
			{
				_lastSwipePosition = position;
				_lastDetectSwipe = CurrentTime + DETECT_SWIPE_TIME;
			}

			if (OnMoveEvent != null && _availebleEvents.Contains(InputEvent.Move))
				OnMoveEvent(position);
		}

		protected void OnRelease(Vector3 position)
		{
			if (OnReleaseEvent != null && _availebleEvents.Contains(InputEvent.Release))
				OnReleaseEvent(position);
		}

		protected void OnTap(Vector3 position)
		{
			if (OnTapEvent != null && _availebleEvents.Contains(InputEvent.Tap))
				OnTapEvent(position);

			_currentState = InputEvent.None;
		}

		protected void OnDoubleTap(Vector3 position)
		{
			if (OnDoubleTapEvent != null && _availebleEvents.Contains(InputEvent.DoubleTap))
				OnDoubleTapEvent(position);

			_currentState = InputEvent.None;
		}

		protected void OnSwipe(Vector2 velocity)
		{
			if (OnSwipeEvent != null && _availebleEvents.Contains(InputEvent.Swipe))
				OnSwipeEvent(velocity);

			_currentState = InputEvent.None;
		}

		protected void OnZoom(float zoom)
		{
			if (OnZoomEvent != null && _availebleEvents.Contains(InputEvent.Zoom))
				OnZoomEvent(zoom);
		}

		protected void OnAcceleration(Vector3 acceleration)
		{
			if (OnAccelerationEvent != null && _availebleEvents.Contains(InputEvent.Acceleration))
				OnAccelerationEvent(acceleration);
		}

		protected void OnDrag(Vector3 velocity)
		{
			if (OnDragEvent != null && _availebleEvents.Contains(InputEvent.Swipe))
				OnDragEvent(velocity);
		}
	}
}

