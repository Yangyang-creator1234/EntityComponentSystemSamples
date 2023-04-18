using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Yangyang
{
    [UpdateAfter(typeof(SpawnerSystem))]
    public partial class ProcessInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            Dependency = Entities
                  .WithName("Yangyang_ProcessInputSystem")
                  .WithAll<PlayerTag>()
                  .WithNone<InitializeTag>()
                  .ForEach((ref SecondOrderDynamicComponent secondOrder,
                            in Translation pos,
                            in SoliderMoveSpeed moveSpeed,
                            in UserInputComponent component) =>
                  {
                      var isMoving = false;
                      var moveDir = float3.zero;
                      if (component.MoveDir.x != 0f || component.MoveDir.y != 0f)
                      {
                          moveDir = new float3(component.MoveDir.x, 0f, component.MoveDir.y);
                          isMoving = true;
                      }
                      //else if (math.distancesq(component.HitPoint, pos.Value) > 0.1f)
                      //{
                      //    moveDir = math.normalize(component.HitPoint - pos.Value);
                      //    isMoving = true;
                      //}

                      if (isMoving)
                      {
                          var movement = moveDir * moveSpeed.Value;
                          secondOrder.Update(deltaTime, pos.Value + movement, moveDir);
                      }
                  }).ScheduleParallel(Dependency);

            Dependency = Entities
               .WithName("Yangyang_TranslationSystem")
               .WithNone<InitializeTag>()
               .ForEach(
                   (ref Translation pos, in SecondOrderDynamicComponent secondOrder) =>
                   {
                       pos.Value = secondOrder.y;
                   }).ScheduleParallel(Dependency);

            Dependency = Entities
            .WithName("Yangyang_RotationSystem")
             .WithNone<InitializeTag>()
             .ForEach(
                 (ref Rotation rot, in SoliderRotationSpeed rotateSpeed, in SecondOrderDynamicComponent secondOrder) =>
                 {
                     rot.Value = math.slerp(rot.Value, secondOrder.Rotation, deltaTime * rotateSpeed.Value);
                 }).ScheduleParallel(Dependency);
        }
    }
}