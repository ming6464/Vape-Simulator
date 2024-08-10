using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game._Scripts
{
    public class RotateWithMouse : MonoBehaviour
    {
        [SerializeField]
        protected bool _is2D;
        
        //Reference
        protected Transform  _myTf;
        protected Input_Ctrl _inputCtrl;
        //
        protected bool    _hasInput;
        protected Vector3 _rotationAngle;

        protected virtual void Awake()
        {
            _myTf = transform;
        }

        protected virtual void Start()
        {
            if (Input_Ctrl.Instance)
            {
                _inputCtrl = Input_Ctrl.Instance;
                _hasInput  = true;
            }
        }

        protected virtual void LateUpdate()
        {
            if (_hasInput)
            {
                var subtractMousePosition = _inputCtrl.mousePosition - _inputCtrl.passMousePosition;

                if (subtractMousePosition.x != 0 || subtractMousePosition.y != 0)
                {
                    _rotationAngle.y -= subtractMousePosition.x;
                    if (_is2D)
                    {
                        _rotationAngle.z += subtractMousePosition.y;
                    }
                    else
                    {
                        _rotationAngle.x += subtractMousePosition.y;
                    }
                    OnRotate();
                }
                
            }
        }

        protected virtual void OnRotate()
        {
            _myTf.rotation =  Quaternion.Euler(_rotationAngle);
        }
    }
}