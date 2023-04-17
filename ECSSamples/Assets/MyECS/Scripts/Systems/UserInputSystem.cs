using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Yangyang
{
    public partial class UserInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            Entities
                  .WithName("UserInputSystem")
                  .ForEach((ref UserInputComponent component) =>
                  {
                      component.MoveDir = new float2(horizontal, vertical);
                  }).Schedule();

        }
    }
}