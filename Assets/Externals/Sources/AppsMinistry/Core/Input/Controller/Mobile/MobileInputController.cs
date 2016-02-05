//
// MobileInputController.cs
//
// Author:
//       Maksym Pyvovarchuk <mpyvovarchuk@appsministry.com>
//
// Created:
//       5/20/2014
//
// Copyright (c) 2014 Maksym Pyvovarchuk

using System;

using UnityEngine;

namespace AppsMinistry.Core.Input
{
	public class MobileInputController : InputController
	{
		#region implemented abstract members of InputController

		public override void OnUpdate ()
		{
			if ( UnityEngine.Input.touchCount == 1 )
			{
				Touch touch = UnityEngine.Input.touches [ 0 ];
				if ( touch.phase == TouchPhase.Began )
					Tap ( touch.position );
                else if ( touch.phase == TouchPhase.Moved )
                {
                    OnMove ( touch.position );
                    Drag ( touch.position );
                }
                else if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
                    EndInput ( touch.position );
			}
			else if ( UnityEngine.Input.touchCount == 2 )
			{
				Touch firstTouch = UnityEngine.Input.touches [ 0 ];
				Touch secondTouch = UnityEngine.Input.touches [ 1 ];
				
				if ( firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began )
				{
					_baseZoomDistance = Mathf.Abs ( Vector2.SqrMagnitude ( firstTouch.position - secondTouch.position ) );
				}
				else if ( firstTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Moved )
				{
					float currentZoomDistance = Mathf.Abs ( Vector2.SqrMagnitude ( firstTouch.position - secondTouch.position ) );

					_currentState = InputEvent.Zoom;
					OnZoom ( (currentZoomDistance - _baseZoomDistance) * ZoomSensetive );

					_baseZoomDistance = currentZoomDistance;
				}
			}
			
			OnAcceleration ( UnityEngine.Input.acceleration );
		}

		#endregion
	}
}

