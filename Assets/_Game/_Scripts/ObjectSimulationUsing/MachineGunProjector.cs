using System;
using UnityEngine;

namespace _Game._Scripts.ObjectSimulationUsing
{
    public class MachineGunProjector : Projector
    {
        protected override void Play(object obj)
        {
            Debug.Log("MachineGun Projector play");
        }
    }
}