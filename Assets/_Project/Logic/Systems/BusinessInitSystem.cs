using _Project.Logic.Components;
using _Project.Logic.Config;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class BusinessInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private BusinessesConfig _config;

        public BusinessInitSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            CreateBalance();
            CreateBusinesses();
        }

        private void CreateBalance()
        {
            var entity = _world.NewEntity();
            ref BalanceComponent balance = ref _world.GetPool<BalanceComponent>().Add(entity);
            balance.Value = 0;
        }

        private void CreateBusinesses()
        {
            EcsPool<BusinessComponent> statePool = _world.GetPool<BusinessComponent>();
            EcsPool<BusinessConfigRef> configRefPool = _world.GetPool<BusinessConfigRef>();

            for (int i = 0; i < _config.Businesses.Length; i++)
            {
                var entity = _world.NewEntity();
                
                var configRef = configRefPool.Add(entity);
                configRef.Index = i;
                
                var state = statePool.Add(entity);
                state.Level = (i == 0) ? 1 : 0;
                state.IncomeProgress = 0f;
                state.Upgrade1Bought = false;
                state.Upgrade2Bought = false;
                
                Debug.Log($"Businesses {i} created");
            }
        }
    }
}