using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Yangyang
{
    public partial class ProcessInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Dependency = Entities
                  .WithName("ProcessInputSystem")
                  .WithAll<PlayerTag>()
                  .ForEach((ref SecondOrderDynamicComponent secondOrder,
                            in Translation pos,
                            in SoliderMoveSpeed moveSpeed,
                            in UserInputComponent component) =>
                  {
                      var moveDir = component.MoveDir;
                      if (moveDir.x != 0f || moveDir.y != 0f)
                      {
                          var movement = new float3(moveDir.x, 0f, moveDir.y) * moveSpeed.Value;
                          secondOrder.Update(deltaTime, pos.Value + movement);
                      }
                  }).ScheduleParallel(Dependency);

            Dependency = Entities
                .WithName("SecondOrderDynamicSystem")
                .ForEach(
                    (ref Translation pos, in SecondOrderDynamicComponent secondOrder) =>
                    {
                        pos.Value = secondOrder.y;
                    }).ScheduleParallel(Dependency);
        }
    }
}