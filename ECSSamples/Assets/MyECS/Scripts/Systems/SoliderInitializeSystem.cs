using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Yangyang
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SoliderInitializeSystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem _entityCommandBuffer;

        protected override void OnCreate()
        {
            _entityCommandBuffer = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = _entityCommandBuffer.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithName("Yangyang_SoliderInitializeSystem")
                .WithAll<InitializeTag>()
                .ForEach((Entity entity, int entityInQueryIndex, ref SecondOrderDynamicComponent secondOrder, in Translation translation) =>
                {
                    secondOrder.xp = translation.Value;
                    secondOrder.y = translation.Value;

                    commandBuffer.RemoveComponent<InitializeTag>(entityInQueryIndex, entity);
                }).ScheduleParallel();
        }
    }
}