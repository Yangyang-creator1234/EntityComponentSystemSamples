using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace LearnSample
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SpawnSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities
                .WithName("LeanSample_Spawn")
                .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
                .ForEach((
                    Entity entity,
                    int entityInQueryIndex,
                    DynamicBuffer<PathPointComponent> spawnPathPointsBuffer,
                    in SpawnComponent spawnComponent,
                    in LocalToWorld location) =>
                {
                    var random = new Random(888);
                    for (int x = 0; x < spawnComponent.CountX; x++)
                    {
                        for (int y = 0; y < spawnComponent.CountY; y++)
                        {
                            var instance = commandBuffer.Instantiate(entityInQueryIndex, spawnComponent.Prefab);
                            commandBuffer.AddComponent<TargetPosComponent>(entityInQueryIndex, instance);
                            commandBuffer.AddComponent<NextPathPointIndexComponent>(entityInQueryIndex, instance);
                            commandBuffer.AddComponent<MoveSpeedComponent>(entityInQueryIndex, instance);

                            var pathPointBuffer = commandBuffer.AddBuffer<PathPointComponent>(entityInQueryIndex, instance);
                            foreach (var point in spawnPathPointsBuffer)
                            {
                                pathPointBuffer.Add(new PathPointComponent() { Value = point.Value });
                            }

                            var postion = math.transform(location.Value, new float3(x, 0f, y));
                            commandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = postion });
                            commandBuffer.SetComponent(entityInQueryIndex, instance, new TargetPosComponent { Value = pathPointBuffer[0].Value });
                            commandBuffer.SetComponent(entityInQueryIndex, instance, new NextPathPointIndexComponent { Value = random.NextInt(1, pathPointBuffer.Length - 1) });
                            commandBuffer.SetComponent(entityInQueryIndex, instance, new MoveSpeedComponent { value = random.NextFloat(spawnComponent.MoveSpeed * 0.5f, spawnComponent.MoveSpeed * 1.5f) });
                        }
                    }

                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }).ScheduleParallel();

            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}