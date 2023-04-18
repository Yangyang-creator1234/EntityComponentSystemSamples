using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace Yangyang
{
    /// <summary>
    /// 二阶系统
    /// </summary>
    public struct SecondOrderDynamicComponent : IComponentData
    {
        /// <summary>
        /// previous input
        /// </summary>
        public float3 xp;
        public float3 y, yd;
        public float k1, k2, k3;

        public quaternion Rotation;

        public void Update(float dt, float3 x, float3 moveDir = default(float3))
        {
            var xd = (x - xp) / dt;
            xp = x;

            y += dt * yd;
            yd += dt * (x + k3 * xd - y - k1 * yd) / k2;

            Rotation = quaternion.LookRotation(moveDir, math.up());
        }
    }
}