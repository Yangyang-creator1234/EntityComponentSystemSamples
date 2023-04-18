using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Yangyang
{
    public partial class UserInputSystem : SystemBase
    {
        private Plane m_GroundPlane = new Plane(Vector3.up, Vector3.zero);
        protected override void OnUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var hitPoint = float3.zero;
            var isHit = false;
            if (Input.GetMouseButton(0))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (m_GroundPlane.Raycast(mouseRay, out var enter))
                {
                    hitPoint = mouseRay.GetPoint(enter);
                    isHit = true;
                }
            }

            Entities
                  .WithName("Yangyang_UserInputSystem")
                  .ForEach((ref UserInputComponent component) =>
                  {
                      component.MoveDir = new float2(horizontal, vertical);
                      if (isHit)
                      {
                          component.HitPoint = hitPoint;
                      }
                  }).Schedule();

        }
    }
}