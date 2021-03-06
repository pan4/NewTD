﻿//
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
		[SerializeField]
		private float _smoothTime = 0.05f;

		private Transform _cameraTransform;

		private Vector3 _prevMovePos;

		private Vector3 _offset;
		private Vector3 _basePosition;

		[SerializeField]
		private float _minZoom = 2f;
		[SerializeField]
		private float _maxZoom = 3f;
		private float _zoom;

		[SerializeField]
		private Vector4 _currentBounds = new Vector4(-8f, 1f, 18f, 7f);

		private bool isLocked = false;

        private UnityEngine.Camera _camera;
        private float _windowaspect;
        private float _widthSize;
        private float _pixelToUnitX;
        private float _pixelToUnitY;

        private SpriteRenderer _mapRenderer;
        private Bounds _bounds;

        private void Start()
		{
			_cameraTransform = transform;

            _mapRenderer = GameObject.Find("BaseMap").GetComponent<SpriteRenderer>();
            _bounds = _mapRenderer.sprite.bounds;
            _bounds.center = _mapRenderer.transform.position;

            _cameraTransform.position = new Vector3(_mapRenderer.transform.position.x, _mapRenderer.transform.position.y, -10);

			_basePosition = _cameraTransform.position;

			_offset = Vector3.zero;

            _camera = GetComponent<UnityEngine.Camera>();
            _windowaspect = (float)Screen.width / (float)Screen.height;
            _widthSize = _camera.orthographicSize * _windowaspect;
            SetBounds(_camera.orthographicSize);

            _camera.orthographicSize = _maxZoom;
            _zoom = _maxZoom;

            _pixelToUnitX = _widthSize * 2 / (float)Screen.width;
            _pixelToUnitY = _camera.orthographicSize * 2 / (float)Screen.height;


        }

        private void SetBounds(float hightSize)
        {            
            _widthSize = hightSize * _windowaspect;
            float borderX = (_bounds.size.x - _widthSize * 2 ) / 2 ;
            float borderY = (_bounds.size.y - _camera.orthographicSize * 2) / 2 + _bounds.center.y;
            _currentBounds = new Vector4(_bounds.center.x - borderX, _bounds.center.y - borderY,
                                         _bounds.center.x + borderX, _bounds.center.y + borderY);
        }

		private void OnEnable()
		{
			InputManager.Instance.OnTouch += OnMove;
			InputManager.Instance.OnTouchEnd += OnTouchEnd;
			InputManager.Instance.OnZoom += OnZoom;
            EasyTouch.On_PinchEnd -= On_PinchEnd;
        }

		private void OnDisable()
		{
			if (InputManager.Instance != null)
			{
				InputManager.Instance.OnTouch -= OnMove;
				InputManager.Instance.OnTouchEnd -= OnTouchEnd;
				InputManager.Instance.OnZoom -= OnZoom;
                EasyTouch.On_PinchEnd -= On_PinchEnd;
            }
		}

        private void OnZoom(float pinchDelta)
		{
			if (isLocked)
				return;

            _zoom -= pinchDelta * Time.deltaTime;
	
			_zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

            _basePosition = _cameraTransform.position;
            _offset = Vector3.zero;
            _prevMovePos = Vector3.zero;
        }

        private void On_PinchEnd(Gesture gesture)
        {
            _offset = Vector3.zero;
        }

        private void OnMove(Gesture gesture)
		{
			if (isLocked)
				return;

            if (gesture.touchCount > 1)
                return;

			if (_prevMovePos == Vector3.zero)
			{
				_prevMovePos = gesture.position;
				_offset = Vector3.zero;
				_basePosition = _cameraTransform.position;
			}


            float deltaX = _prevMovePos.x - gesture.position.x;
            float deltaY = _prevMovePos.y - gesture.position.y;

            _offset += new Vector3(deltaX * _pixelToUnitX, deltaY * _pixelToUnitY, 0);
			_prevMovePos = gesture.position;
		}

		private void OnTouchEnd(Vector3 position)
		{
			if (isLocked)
				return;

			_prevMovePos = Vector3.zero;
		}

		private Vector3 _velocity;

        private void LateUpdate()
        {
            if (isLocked)
                return;

            Vector3 cameraTarget = _basePosition + _offset;

            float x = Mathf.Clamp(_cameraTransform.position.x, _currentBounds.x, _currentBounds.z);
            float y = Mathf.Clamp(_cameraTransform.position.y, _currentBounds.y, _currentBounds.w);

            cameraTarget.x = Mathf.Clamp(cameraTarget.x, _currentBounds.x, _currentBounds.z);
            cameraTarget.y = Mathf.Clamp(cameraTarget.y, _currentBounds.y, _currentBounds.w);

            float newPositionX;
            float newPositionY;

            if (Mathf.Abs(_cameraTransform.position.x - x) < 0.01f && Mathf.Abs(_cameraTransform.position.y - y) < 0.01f)
            {
                newPositionX = Mathf.SmoothDamp(_cameraTransform.position.x, cameraTarget.x, ref _velocity.x, _smoothTime);
                newPositionY = Mathf.SmoothDamp(_cameraTransform.position.y, cameraTarget.y, ref _velocity.y, _smoothTime);
            }
            else
            {
                newPositionX = Mathf.SmoothDamp(_cameraTransform.position.x, x, ref _velocity.x, 0.01f);
                newPositionY = Mathf.SmoothDamp(_cameraTransform.position.y, y, ref _velocity.y, 0.01f);
            }

            _cameraTransform.position = new Vector3(newPositionX, newPositionY, -10f);

            float velocity = 0;
            float newOrthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _zoom, ref velocity, 0.2f);
            SetBounds(newOrthographicSize);
            _camera.orthographicSize = newOrthographicSize;

#if UNITY_EDITOR
            #region Wheel mouse zoom

            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    _zoom += 10 * Time.deltaTime;
                else
                    _zoom -= 10 * Time.deltaTime;

                _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
                _basePosition = _cameraTransform.position;
                _offset = Vector3.zero;
                _prevMovePos = Vector3.zero;

            }

            #endregion
#endif
        }
    }
}