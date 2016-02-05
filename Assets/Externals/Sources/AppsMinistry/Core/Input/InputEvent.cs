//
// InputEvent.cs
//
// Author:
//       Maksym Pyvovarchuk <mpyvovarchuk@appsministry.com>
//
// Created:
//       5/19/2014
//
// Copyright (c) 2014 Maksym Pyvovarchuk

using System;

namespace AppsMinistry.Core.Input
{
	public enum InputEvent
	{
		None,
		Press,
		Move,
		Release,
		Tap,
		DoubleTap,
		Swipe,
		Zoom,
		Acceleration,
        Drag
	}
}

