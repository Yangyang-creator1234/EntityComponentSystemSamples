using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class SimpleScaleAnimationAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public AnimationCurve ScaleCurve = AnimationCurve.EaseInOut(0, 1, 2, 1);

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var blob = SimpleScaleAnimationBlob.CreateBlob(ScaleCurve, Allocator.Persistent);
        conversionSystem.BlobAssetStore.AddUniqueBlobAsset(ref blob);

        dstManager.AddComponentData(entity, new SimpleScaleAnimationComponet { Anim = blob });
        dstManager.AddComponentData(entity, new Scale { Value = 1f });
    }
}

public struct SimpleScaleAnimationComponet : IComponentData
{
    public BlobAssetReference<SimpleScaleAnimationBlob> Anim;
    public float T;
}

public partial class SimpleScaleAnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;
        Entities.ForEach((ref SimpleScaleAnimationComponet anim, ref Scale scale) =>
        {
            anim.T += dt;
            scale.Value = anim.Anim.Value.Evaluate(anim.T);
        }).ScheduleParallel();
    }
}