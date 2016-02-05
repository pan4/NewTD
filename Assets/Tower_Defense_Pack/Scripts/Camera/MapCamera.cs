//
// MapCamera.cs
//
// Author:
//       Alexander Panchenko <apanchenko@neoline.biz>
//
// Created:
//       12/10/2014 4:47:33 PM
//
// Copyright (c) 2014 Alexander Panchenko

using System;
using System.Collections.Generic;
using UnityEngine;

using AppsMinistry.Core.Input;


namespace Slugterra.UI.Game.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	class MapCamera : MonoBehaviour
	{
		private const float HEIGHT = 12;

		[SerializeField]
		private float _sensitive = 0.016f;

		[SerializeField]
		private float _smoothTime = 0.05f;

		private Transform _cameraTransform;

		private Vector3 _prevMovePos;

		private Vector3 _movement;
		private Vector3 _offset;
		private Vector3 _basePosition;

		[SerializeField]
		private float _minZoom = 30;
		[SerializeField]
		private float _maxZoom = 60;
		private float _zoom;

		[SerializeField]
		private float _minHeight = 8;
		[SerializeField]
		private float _maxHeight = 12;
		private float _height;

		[SerializeField]
		private Vector4 _bounds = new Vector4(-8f, 1f, 18f, 7f);

		private bool isLocked = false;

		private void Start()
		{
			_cameraTransform = transform;

			_zoom = _maxZoom;
			_height = _maxHeight;

			_cameraTransform.position = new Vector3(0, 0, -10);

			//Vector3 initialPosition = Vector3.zero;			
			//initialPosition = new Vector3(initialPosition.x + 2, _height, initialPosition.z - 12.4f);				
			//initialPosition.x = Mathf.Clamp(initialPosition.x, _bounds.x, _bounds.z);
			//initialPosition.y = Mathf.Clamp(initialPosition.y, _bounds.y, _bounds.w);				
			//_cameraTransform.position = initialPosition;

			_basePosition = _cameraTransform.position;

			_offset = Vector3.zero;
		}

		private void OnEnable()
		{
			InputManager.Instance.OnTouch += OnMove;
			InputManager.Instance.OnTouchEnd += OnTouchEnd;
			InputManager.Instance.OnZoom += OnZoom;
		}

		private void OnDisable()
		{
			if (InputManager.Instance != null)
			{
				InputManager.Instance.OnTouch -= OnMove;
				InputManager.Instance.OnTouchEnd -= OnTouchEnd;
				InputManager.Instance.OnZoom -= OnZoom;
			}
		}

		private void OnZoom(float zoom)
		{
			if (isLocked)
				return;

			_zoom -= zoom;
			_zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
			_height -= zoom / 5;
			_height = Mathf.Clamp(_height, _minHeight, _maxHeight);
		}

		private void OnMove(Vector3 position)
		{
			if (isLocked)
				return;

			if (_prevMovePos == Vector3.zero)
			{
				_prevMovePos = position;
				_offset = Vector3.zero;
				_basePosition = _cameraTransform.position;
			}

			Vector3 delta = -(position - _prevMovePos);
			_movement = new Vector3(delta.x, delta.y, 0);
			_offset += (_movement * _sensitive);
			_prevMovePos = position;
		}

		private void OnTouchEnd(Vector3 position)
		{
			if (isLocked)
				return;

			_movement = Vector3.zero;
			_prevMovePos = Vector3.zero;
		}

		private Vector3 _velocity;

		private void LateUpdate()
		{
			if (isLocked)
				return;

			Vector3 cameraTarget = _basePosition + _offset;

			cameraTarget.x = Mathf.Clamp(cameraTarget.x, _bounds.x, _bounds.z);
			cameraTarget.y = Mathf.Clamp(cameraTarget.y, _bounds.y, _bounds.w);

			float newPositionX = Mathf.SmoothDamp(_cameraTransform.position.x, cameraTarget.x, ref _velocity.x, _smoothTime);

			//float newPositionZ = Mathf.SmoothDamp(_cameraTransform.position.z, cameraTarget.z, ref _velocity.z, _smoothTime);

			float newPositionY = Mathf.SmoothDamp(_cameraTransform.position.y, cameraTarget.y, ref _velocity.y, _smoothTime);

			_cameraTransform.position = new Vector3(newPositionX, newPositionY, -10f);
		}
	}
}