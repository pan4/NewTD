//
// StandaloneInputController.cs
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
	public class StandaloneInputController : InputController
	{
		#region implemented abstract members of InputController

		public override void OnUpdate ()
		{
			if ( UnityEngine.Input.GetMouseButtonDown (0) )
				Tap ( UnityEngine.Input.mousePosition );
            else if ( UnityEngine.Input.GetMouseButton (0) )
            {
                OnMove ( UnityEngine.Input.mousePosition );
                Drag ( UnityEngine.Input.mousePosition );
            }
            else if ( UnityEngine.Input.GetMouseButtonUp (0) )
                EndInput ( UnityEngine.Input.mousePosition );
            
			if ( UnityEngine.Input.GetAxis ("Vertical") > 0 || UnityEngine.Input.GetAxis ("Vertical") < 0 )
                OnZoom ( UnityEngine.Input.GetAxis ("Vertical") * 0.002f );

			OnAcceleration ( UnityEngine.Input.acceleration );
		}

		#endregion
	}
}

