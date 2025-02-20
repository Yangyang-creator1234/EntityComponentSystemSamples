using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace HelloCube.GameObjectSync
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct RotatorInitSystem : ISystem
    {
        private EntityQuery query;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DirectoryManaged>();
            state.RequireForUpdate<Execute.GameObjectSync>();
        }

        // This OnUpdate accesses managed objects, so it cannot be burst compiled.
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var directory = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();

            // Instantiate the associated GameObject from the prefab.
            foreach (var (goPrefab, entity) in
                     SystemAPI.Query<RotationSpeed>()
                         .WithNone<RotatorGO>()
                         .WithEntityAccess())
            {
                var go = GameObject.Instantiate(directory.RotatorPrefab);

                // We can't add components to entities as we iterate over them, so we defer the change with an ECB.
                ecb.AddComponent(entity, new RotatorGO(go));
            }
        }
    }

    public class RotatorGO : IComponentData
    {
        public GameObject Value;

        public RotatorGO(GameObject value)
        {
            Value = value;
        }

        // Every IComponentData class must have a no-arg constructor.
        public RotatorGO()
        {
        }
    }
}

