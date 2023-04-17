using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct LearnGridPlaneGenerator : IComponentData
{
    public BlobAssetReference<LearnGridPlaneGeneratorBlob> Blob;
}

public struct LearnGridPlaneGeneratorBlob
{
    public int RowCount;
    public int ColCount;
    public Entity WallPrefab;
    public BlobArray<Entity> FloorPrefab;
}
