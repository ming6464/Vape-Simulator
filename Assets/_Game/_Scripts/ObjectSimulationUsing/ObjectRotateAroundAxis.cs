using System;
using System.Threading.Tasks;
using UnityEngine;

namespace _Game._Scripts.ObjectSimulationUsing
{
    public class ObjectRotateAroundAxis : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField]
        private float _timeAnimRotateToDefault;

        [SerializeField]
        private float _speedRotateUp;

        [SerializeField]
        private float _speedRotateRight;

        private bool  _activeRotate;
        private float _timeAnimDelta;

        [SerializeField]
        private Transform _objRotateTransform;

        private Quaternion _rotateSave;

        protected void Awake()
        {
            if (_objRotateTransform != null)
            {
                return;
            }

            Debug.LogError("Object to rotate is not assigned.");
            _activeRotate = false;
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.OnRotateMode, OnFeatureRotate3D);
            EventDispatcher.Instance.RegisterListener(EventID.OnBackToDefaultLayerMain, OnBackToDefaultLayerMain);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnRotateMode, OnFeatureRotate3D);
            EventDispatcher.Instance.RemoveListener(EventID.OnBackToDefaultLayerMain, OnBackToDefaultLayerMain);
        }

        private async Task GoBackToDefault()
        {
            while (_timeAnimDelta < _timeAnimRotateToDefault)
            {
                _timeAnimDelta += Time.deltaTime;
                var progress = _timeAnimDelta / _timeAnimRotateToDefault;
                _objRotateTransform.rotation = Quaternion.Slerp(_rotateSave, Quaternion.identity, progress);
                await Task.Yield();
            }
        }

        private void OnBackToDefaultLayerMain(object obj)
        {
            _activeRotate  = false;
            _timeAnimDelta = 0;
            _rotateSave    = _objRotateTransform.rotation;
            _              = GoBackToDefault();
        }

        private void OnFeatureRotate3D(object obj)
        {
            _activeRotate = true;
        }

        private void OnRotate(Vector2 startPoint, Vector2 previousPoint, Vector2 currentPoint)
        {
            if (!_activeRotate)
                return;

            var delta = currentPoint - previousPoint;

            var angleUp    = -delta.x * _speedRotateUp * Time.deltaTime;
            var angleRight = delta.y * _speedRotateRight * Time.deltaTime;

            _objRotateTransform.RotateAround(_objRotateTransform.position, Vector3.up, angleUp);
            _objRotateTransform.RotateAround(_objRotateTransform.position, Vector3.right, angleRight);
        }
    }
}