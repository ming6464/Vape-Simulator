using System;
using UnityEngine;
using VInspector;

public abstract class LayerBase : MonoBehaviour
{
   // [Tab("Base")]
   [SerializeField]
   protected Transform _content;
   
   protected virtual void Awake()
   {
   }

   protected virtual void OnEnable()
   {

   }
   
   protected virtual void OnDisable()
   {

   }

   protected virtual void Start()
   {
      
   }

   protected virtual void Update()
   {
   }

   protected virtual void LateUpdate()
   {
   }
}