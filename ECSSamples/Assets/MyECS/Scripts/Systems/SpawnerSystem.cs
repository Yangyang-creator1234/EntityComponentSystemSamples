using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

namespace Yangyang
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SpawnerSystem : SystemBase
    {
        private BeginInitializationEntityCommandBufferSystem m_BeginInitializationECB;

        protected override void OnCreate()
        {
            m_BeginInitializationECB = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float width = 3f;
            float height = 3f;

            var commandBuffer = m_BeginInitializationECB.CreateCommandBuffer().AsParallelWriter();
            Entities
                .WithName("Yangyang_SpawnerSystem")
                .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
                .ForEach((Entity entity, int entityInQueryIndex, in SpawnerComponent spawner, in LocalToWorld location) =>
                {
                    for (int y = 0; y < spawner.CountX; y++)
                    {
                        for (int x = 0; x < spawner.CountY; x++)
                        {
                            var instance = commandBuffer.Instantiate(entityInQueryIndex, spawner.Prefab);
                            var position = math.transform(location.Value, new float3(x * width, 0f, y * height));

                            commandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = position });
                            commandBuffer.AddComponent<InitializeTag>(entityInQueryIndex, instance);
                        }
                    }

                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }).ScheduleParallel();

        }
    }
}