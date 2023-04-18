using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Yangyang
{
    public class SoliderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public bool IsPlayer;
        public float MoveSpeedPerSeconds;
        public float RotationDegreePerSeconds;

        public float SecondOrderParamf;
        public float SecondOrderParamz;
        public float SecondOrderParamr;

        public AnimationCurve SecondOrderCurve;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (IsPlayer)
            {
                dstManager.AddComponent(entity, typeof(PlayerTag));
                dstManager.AddComponent(entity, typeof(UserInputComponent));
            }
            dstManager.AddComponentData(entity, new SoliderMoveSpeed { Value = MoveSpeedPerSeconds });
            dstManager.AddComponentData(entity, new SoliderRotationSpeed { Value = math.radians(RotationDegreePerSeconds) }); ;
            //dstManager.SetComponentData(entity, new Translation { Value = transform.position });
          
            if (SecondOrderParamf > 0f)
            {
                var div = math.PI * SecondOrderParamf;
                var paramk1 = SecondOrderParamz / div;
                var paramk2 = 1f / ((2f * div) * (2f * div));
                var paramk3 = SecondOrderParamr * SecondOrderParamz / (2f * div);
                dstManager.AddComponentData(entity, new SecondOrderDynamicComponent
                {
                    xp = transform.position,
                    y = transform.position,
                    yd = 0f,
                    k1 = paramk1,
                    k2 = paramk2,
                    k3 = paramk3,
                });
            }
        }
    }
}