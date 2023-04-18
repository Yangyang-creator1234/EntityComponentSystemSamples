using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Yangyang
{
    public struct SpawnerComponent : IComponentData
    {
        public int CountX;
        public int CountY;
        public Entity Prefab;
    }
}