using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Yangyang
{
    public struct UserInputComponent : IComponentData
    {
        public float2 MoveDir;
    }
}