using UnityEngine;
using System;

namespace AppsMinistry.Core.Input
{
	public interface IInputEvents {
		event Action<Vector3> OnSwipeStart;
		event Action<Vector2> OnSwipe;
		event Action<Vector3> OnSwipeEnd;
	
		event Action<Vector3> OnDragStart;
		event Action<Vector2> OnDrag;
		event Action<Vector3> OnDragEnd;
	
		event Action<Vector3> OnTouchStart;
		event Action<Gesture> OnTouch;
		event Action<Vector3> OnTap;
		event Action<Vector3> OnTouchEnd;
	
		event Action<Vector3> OnDoubleTap;
			
		event Action<float> OnZoom;
		event Action<Vector3> OnAcceleration;
	
	}
}