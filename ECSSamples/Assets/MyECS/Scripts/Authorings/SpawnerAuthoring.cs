using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Yangyang
{
    public class SpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public GameObject SoldierPrefab;
        public int CountX = 10;
        public int CountY = 10;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(SoldierPrefab);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new SpawnerComponent
            {
                CountX = CountX,
                CountY = CountY,
                Prefab = conversionSystem.GetPrimaryEntity(SoldierPrefab)
            });
        }
    }
}