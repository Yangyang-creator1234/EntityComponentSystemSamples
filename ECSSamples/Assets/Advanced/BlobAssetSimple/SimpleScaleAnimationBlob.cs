using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public struct SimpleScaleAnimationBlob
{
    BlobArray<float> Keys;
    float InvLength;
    float KeyCount;

    public float CalculateNormalizedTime(float t)
    {
        float normalizedT = t * InvLength;
        return math.frac(normalizedT);
    }

    public float Evaluate(float t)
    {
        t = CalculateNormalizedTime(t);

        float sampleT = t * KeyCount;
        var sampleFloor = math.floor(sampleT);
        float interp = sampleT - sampleFloor;
        var index = (int)sampleFloor;

        return math.lerp(Keys[index], Keys[index + 1], interp);
    }

    public static BlobAssetReference<SimpleScaleAnimationBlob> CreateBlob(AnimationCurve curve, Allocator allocator)
    {
        using (var blob = new BlobBuilder(Allocator.TempJob))
        {
            ref var anim = ref blob.ConstructRoot<SimpleScaleAnimationBlob>();
            int keyCount = 12;

            float endTime = curve[curve.length - 1].time;
            anim.InvLength = 1f / endTime;
            anim.KeyCount = keyCount;

            var array = blob.Allocate(ref anim.Keys, keyCount + 1);
            for (int i = 0; i < keyCount; i++)
            {
                var t = (float)i / (float)(keyCount - 1) * endTime;
                array[i] = curve.Evaluate(t);
            }
            array[keyCount] = array[keyCount - 1];

            return blob.CreateBlobAssetReference<SimpleScaleAnimationBlob>(allocator);
        }
    }
}
