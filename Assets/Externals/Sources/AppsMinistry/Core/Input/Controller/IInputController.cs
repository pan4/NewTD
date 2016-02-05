//
// IInputController.cs
//
// Author:
//       Maksym Pyvovarchuk <mpyvovarchuk@appsministry.com>
//
// Created:
//       5/19/2014
//
// Copyright (c) 2014 Maksym Pyvovarchuk

using System;

using UnityEngine;
using System.Collections.Generic;

namespace AppsMinistry.Core.Input
{
	public interface IInputController
	{
		event Action<Vector3> OnPressEvent;
		event Action<Vector3> OnMoveEvent;
		event Action<Vector3> OnReleaseEvent;

		event Action<Vector3> OnTapEvent;
		event Action<Vector3> OnDoubleTapEvent;
		event Action<Vector2> OnSwipeEvent;
		event Action<float> OnZoomEvent;

		event Action<Vector3> OnAccelerationEvent;
		event Action<Vector3> OnDragEvent;

		List<InputEvent> AvailebleEvents
		{
			get;
		}

		void OnUpdate();

		void EnableEvent(InputEvent inputEvent);
		void DisableEvent(InputEvent inputEvent);

		void Clear();
	}
}

