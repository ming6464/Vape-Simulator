using System;
using _Game._Scripts.Support;
using Unity.Mathematics;
using UnityEngine;

namespace _Game._Scripts
{
    public class RotateAroundAxisWithMouse : MonoBehaviour
    {
        // [SerializeField]
        //          protected ObjRotateFlexibleInfo[] _objRotateFlexibleInfos;
        //  
        //          protected Vector3 _passRotationAngle;
        //          protected bool    _isCanRotate;
        //  
        //          protected void Awake()
        //          {
        //              _isCanRotate = true;
        //              foreach (var objInfo in _objRotateFlexibleInfos)
        //              {
        //                  if (objInfo.objRotateTf)
        //                  {
        //                      continue;
        //                  }
        //  
        //                  _isCanRotate = false;
        //                  break;
        //              }
        //          }
        //  
        //          protected void OnRotate()
        //          {
        //              if (!_isCanRotate)
        //              {
        //                  Debug.LogError("Object truyền vào bị null");
        //                  return;
        //              }
        //  
        //              var subtractRotateAngle = _rotationAngle - _passRotationAngle;
        //              _passRotationAngle = _rotationAngle;
        //              foreach (var objRotaInfo in _objRotateFlexibleInfos)
        //              {
        //                  LoadAxisRotate(subtractRotateAngle, objRotaInfo.axisRotate, out Vector3 axisRotate, out float angle);
        //                  var angleRota = angle * objRotaInfo.speedRotate * Time.deltaTime;
        //                  RotateAround(objRotaInfo.objRotateTf,axisRotate,angleRota);
        //              }
        //              
        //          }
        //  
        //          protected virtual void LoadAxisRotate(Vector3 vtAngle, AxisRotate axisRotateType, out Vector3 axisRotate,out float angle)
        //          {
        //              axisRotate = default;
        //              angle      = 0;
        //              switch (axisRotateType)
        //              {
        //                  case AxisRotate.Up:
        //                      axisRotate = Vector3.up;
        //                      angle      = vtAngle.y;
        //                      break;
        //                  case AxisRotate.Down: 
        //                      axisRotate = -Vector3.up;
        //                      angle      = -vtAngle.y;
        //                      break;
        //                  case AxisRotate.Right: 
        //                      axisRotate = Vector3.right;
        //                      angle      = vtAngle.x;
        //                      break;
        //                  case AxisRotate.Left: 
        //                      axisRotate = -Vector3.right;
        //                      angle      = -vtAngle.x;
        //                      break;
        //                  case AxisRotate.Forward:
        //                      axisRotate = Vector3.forward;
        //                      angle      = vtAngle.z;
        //                      break;
        //                  case AxisRotate.Backward: 
        //                      axisRotate = -Vector3.forward;
        //                      angle      = -vtAngle.z;
        //                      break;
        //              }
        //          }
        //          
        //          [Obsolete("Obsolete")]
        //          protected virtual void RotateAround(Transform objTfRotate, Vector3 axis, float angle)
        //          {
        //              objTfRotate.RotateAround(axis,angle);
        //          }
    }

    [Serializable]
    public class ObjRotateFlexibleInfo
    {
        public Transform  objRotateTf;
        public float      speedRotate;
        public AxisRotate axisRotate;
    }
    
}