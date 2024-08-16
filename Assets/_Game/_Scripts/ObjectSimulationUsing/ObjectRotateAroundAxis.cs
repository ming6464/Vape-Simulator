using System;
using UnityEngine;

namespace _Game._Scripts.ObjectSimulationUsing
{
    public class ObjectRotateAroundAxis : RotateAroundAxisWithMouse
    {
        [Header("Info")]
        [SerializeField]
        private float _timeAnimRotateToDefault;
        //
        private bool             _activeRotate;
        private float            _timeAnimDelta;
        private bool             _goToDefault;
        private ObjectRotateInfo[] _objectRotateInfos;

        protected override void Awake()
        {
            base.Awake();
            var length = _objRotateFlexibleInfos.Length;
            _objectRotateInfos = new ObjectRotateInfo[length];
            for (var i = 0; i < length; i++)
            {
                _objectRotateInfos[i] = new(_objRotateFlexibleInfos[i]);
            }
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener(EventID.OnRotateMode,OnFeatureRotate3D);
            EventDispatcher.Instance.RegisterListener(EventID.OnBackToDefaultLayerMain,OnBackToDefaultLayerMain);
        }
        
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnRotateMode,OnFeatureRotate3D);
            EventDispatcher.Instance.RemoveListener(EventID.OnBackToDefaultLayerMain,OnBackToDefaultLayerMain);
        }

        private void Update()
        {
            if (_goToDefault)
            {
                _timeAnimDelta += Time.deltaTime;

                if (_timeAnimDelta - _timeAnimRotateToDefault >= 0)
                {
                    _timeAnimDelta = _timeAnimRotateToDefault;
                    _goToDefault   = false;
                }

                foreach (var objInfo in _objectRotateInfos)
                {
                    objInfo.GoToZero(_timeAnimDelta/_timeAnimRotateToDefault);
                }
            }
        }

        private void OnBackToDefaultLayerMain(object obj)
        {
            _activeRotate  = false;
            _timeAnimDelta = 0;
            _goToDefault   = true;

            foreach (var objInfo in _objectRotateInfos)
            {
                objInfo.SaveRotate();
            }
        }

        private void OnFeatureRotate3D(object obj)
        {
            _activeRotate = true;
        }

        protected override void OnRotate()
        {
            if(!_activeRotate) return;
            base.OnRotate();
        }
        //
        private class ObjectRotateInfo
        {
            public Transform  objTf;
            public Quaternion rotateSave;

            public ObjectRotateInfo(ObjRotateFlexibleInfo objFlexible)
            {
                objTf = objFlexible.objRotateTf;
            }
            
            public void SaveRotate()
            {
                rotateSave = objTf.rotation;
            }

            public void GoToZero(float progress)
            {
                objTf.rotation = Quaternion.Slerp(rotateSave, Quaternion.identity, progress);
            }
            
        }
    }
}