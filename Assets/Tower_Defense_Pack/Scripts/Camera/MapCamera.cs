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
		private float _minZoom = 2;
		[SerializeField]
		private float _maxZoom = 3;
		private float _zoom;

		[SerializeField]
		private Vector4 _bounds = new Vector4(-8f, 1f, 18f, 7f);

		private bool isLocked = false;

        private UnityEngine.Camera _camera;
        private float _windowaspect;
        private float _widthSize;
        private float _pixelToUnitX;
        private float _pixelToUnitY;

        private void Start()
		{
			_cameraTransform = transform;
			_cameraTransform.position = new Vector3(0, 0, -10);

			_basePosition = _cameraTransform.position;

			_offset = Vector3.zero;

            _camera = GetComponent<UnityEngine.Camera>();
            _windowaspect = (float)Screen.width / (float)Screen.height;
            _widthSize = _camera.orthographicSize * _windowaspect;
            SetBounds();

            _camera.orthographicSize = _maxZoom;

            _pixelToUnitX = _widthSize * 2 / (float)Screen.width;
            _pixelToUnitY = _camera.orthographicSize * 2 / (float)Screen.height;
        }

        private void SetBounds()        {

            float borderX = (10.6f - _widthSize * 2) / 2;
            float borderY = (9 - _camera.orthographicSize * 2) / 2;
            _bounds = new Vector4(-borderX, -borderY, borderX, borderY);
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

			_zoom -= (zoom / 60f);
			_zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

            _camera.orthographicSize = _zoom;
            SetBounds();
            float x = Mathf.Clamp(_cameraTransform.position.x, _bounds.x, _bounds.z);
            float y = Mathf.Clamp(_cameraTransform.position.y, _bounds.y, _bounds.w);

            _cameraTransform.position = new Vector3(x, y, -10f);
        }

        private void OnMove(Vector3 position)
		{
			if (isLocked)
				return;

            //Debug.Log(position);

			if (_prevMovePos == Vector3.zero)
			{
				_prevMovePos = position;
				_offset = Vector3.zero;
				_basePosition = _cameraTransform.position;
			}

            //Vector3 delta = -(position - _prevMovePos);
            //_movement = new Vector3(delta.x, delta.y, 0);
            //_offset += (_movement * _sensitive);

            float deltaX = _prevMovePos.x - position.x;
            float deltaY = _prevMovePos.y - position.y;

            _offset += new Vector3(deltaX * _pixelToUnitX, deltaY * _pixelToUnitY, 0);
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