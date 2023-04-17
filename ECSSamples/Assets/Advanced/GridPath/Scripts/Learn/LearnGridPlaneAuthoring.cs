using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class LearnGridPlaneAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [Range(2, 100)]
    public int ColumnCount;
    [Range(2, 100)]
    public int RowCount;

    public GameObject[] FloorPrefab;
    public GameObject WallPrefab;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(FloorPrefab);
        referencedPrefabs.Add(WallPrefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        using (var blobBuilder = new BlobBuilder(Allocator.Temp))
        {
            ref var blob = ref blobBuilder.ConstructRoot<LearnGridPlaneGeneratorBlob>();
            blob.RowCount = RowCount;
            blob.ColCount = ColumnCount;
            blob.WallPrefab = conversionSystem.GetPrimaryEntity(WallPrefab);

            var floorPrefab = blobBuilder.Allocate(ref blob.FloorPrefab, FloorPrefab.Length);
            for (int i = 0; i < FloorPrefab.Length; i++)
            {
                floorPrefab[i] = conversionSystem.GetPrimaryEntity(FloorPrefab[i]);
            }

            dstManager.AddComponentData(entity, new LearnGridPlaneGenerator
            {
                Blob = blobBuilder.CreateBlobAssetReference<LearnGridPlaneGeneratorBlob>(Allocator.Persistent)
            });
        }
    }
}
