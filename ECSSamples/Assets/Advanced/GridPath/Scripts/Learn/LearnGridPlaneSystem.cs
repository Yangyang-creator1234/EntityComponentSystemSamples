using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public unsafe partial class LearnGridPlaneSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Dependency.Complete();

        Entities.WithStructuralChanges().ForEach((Entity entity, ref LearnGridPlaneGenerator learnGridPlaneGenerator) =>
        {
            ref var floorPrefab = ref learnGridPlaneGenerator.Blob.Value.FloorPrefab;
            var wallPrefab = learnGridPlaneGenerator.Blob.Value.WallPrefab;
            var rowCount = learnGridPlaneGenerator.Blob.Value.RowCount;
            var colCount = learnGridPlaneGenerator.Blob.Value.ColCount;
        }).Run();
    }
}
