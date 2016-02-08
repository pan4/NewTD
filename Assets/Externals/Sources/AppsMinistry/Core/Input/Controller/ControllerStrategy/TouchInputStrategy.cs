using UnityEngine;
using System.Collections;
using AppsMinistry.Core.Input;
using System.Collections.Generic;

namespace AppsMinistry.Core.Input.Strategy
{
	public class TouchInputStrategy : BaseInputStrategy {
		
		private EasyTouch easyTouch;
		
		public override void Activate()
		{
			CreateEasyTouch();
			
			EasyTouch.On_SwipeStart += On_SwipeStart;
			EasyTouch.On_Swipe += On_Swipe;
			EasyTouch.On_SwipeEnd += On_SwipeEnd;

			EasyTouch.On_DoubleTap += On_DoubleTap;

			EasyTouch.On_DragStart += On_DragStart;
			EasyTouch.On_Drag += On_Drag;
			EasyTouch.On_DragEnd += On_DragEnd;

			EasyTouch.On_TouchStart += On_TouchStart;
			EasyTouch.On_TouchDown += On_TouchUpdate;
			EasyTouch.On_TouchUp += On_TouchEnd;

			EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
			EasyTouch.On_PinchIn += On_PinchIn;
			EasyTouch.On_PinchOut += On_PinchOut;
			EasyTouch.On_PinchEnd += On_PinchEnd;
			EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
		}
		
		private void CreateEasyTouch()
		{
			Debug.Log("Create easy touch object in strategy!");
			easyTouch = GameObject.FindObjectOfType<EasyTouch>();
			if (easyTouch == null)
			{
				easyTouch = new GameObject("EasyTouch").AddComponent<EasyTouch>();
				easyTouch.transform.parent = InputManager.Instance.transform;
			}
		}
		
		private void DestroyEasyTouch()
		{
			if(easyTouch != null)
				GameObject.Destroy(easyTouch);
		}
		
		public override void Deactivate()
		{
			EasyTouch.On_SwipeStart -= On_SwipeStart;
			EasyTouch.On_Swipe -= On_Swipe;
			EasyTouch.On_SwipeEnd -= On_SwipeEnd;

			EasyTouch.On_DoubleTap -= On_DoubleTap;

			EasyTouch.On_DragStart -= On_DragStart;
			EasyTouch.On_Drag -= On_Drag;
			EasyTouch.On_DragEnd -= On_DragEnd;

			EasyTouch.On_TouchStart -= On_TouchStart;
			EasyTouch.On_TouchDown -= On_TouchUpdate;
			EasyTouch.On_TouchUp -= On_TouchEnd;

			EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
			EasyTouch.On_PinchIn -= On_PinchIn;
			EasyTouch.On_PinchOut -= On_PinchOut;
			EasyTouch.On_PinchEnd -= On_PinchEnd;
			EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
			
			DestroyEasyTouch();
		}
		
		#region easy touch handlers
		
		private void On_SwipeStart(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Swipe))
				return;

			Use_OnSwipeStart(gesture.position);
		}

		// During the swipe
		private void On_Swipe(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Swipe))
				return;
				
			Use_OnSwipe(gesture.swipeVector);
		}

		// At the swipe end 
		private void On_SwipeEnd(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Swipe))
				return;
				
			Use_OnSwipeEnd(gesture.position);
		}

		private void On_DoubleTap(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.DoubleTap))
				return;
				
			if(!_inputManager.IsTapOnIngnoreLayer(gesture.position))
				Use_OnDoubleTap(gesture.position);
		}

		private void On_DragStart(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Drag))
				return;
			
			Use_OnDragStart(gesture.position);
		}

		// During the drag
		private void On_Drag(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Drag))
				return;

			Use_OnDrag(gesture.position);
		}

		// At the drag end
		private void On_DragEnd(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Drag))
				return;

			Use_OnDragEnd(gesture.position);
		}

		// At the touch beginning 
		private void On_TouchStart(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Press) || IsEventEnabled(InputEvent.Tap))
			{
				if(!_inputManager.IsTapOnIngnoreLayer(gesture.position))
					Use_OnTouchStart(gesture.position);
			}
		}

		// During the touch is down
		private void On_TouchUpdate(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Press))
			{
				if(!_inputManager.IsTapOnIngnoreLayer(gesture.position))
					Use_OnTouchUpdate(gesture);
			}
		}

		// At the touch end
		private void On_TouchEnd(Gesture gesture)
		{
			if (IsEventEnabled(InputEvent.Press))
			{
				if(!_inputManager.IsTapOnIngnoreLayer(gesture.position))
					Use_OnTouchEnd(gesture.position);
			}

			if (IsEventEnabled(InputEvent.Tap))
			{
				if(!_inputManager.IsTapOnIngnoreLayer(gesture.position))
					Use_OnTap(gesture.position);
			}
		}

		// At the 2 fingers touch beginning
		private void On_TouchStart2Fingers(Gesture gesture)
		{
			EasyTouch.SetEnablePinch(true);
		}

		// At the pinch in
		private void On_PinchIn(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Zoom))
				return;

			float zoom = -1.0f * gesture.deltaPinch;

			Use_OnZoom(zoom);
		}
		
		// At the pinch out
		private void On_PinchOut(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Zoom))
				return;

			float zoom = gesture.deltaPinch;

			Use_OnZoom(zoom);
		}

		// At the pinch end
		private void On_PinchEnd(Gesture gesture)
		{
			if (!IsEventEnabled(InputEvent.Zoom))
				return;
			
		}

		// If the two finger gesture is finished
		private void On_Cancel2Fingers(Gesture gesture)
		{
			
		}
		
		#endregion
	}
}