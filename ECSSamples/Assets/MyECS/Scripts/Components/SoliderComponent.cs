using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Yangyang
{
    public struct SoliderMoveSpeed : IComponentData
    {
        public float Value;
    }

    public struct SoliderRotationSpeed : IComponentData
    {
        public float Value;
    }

}