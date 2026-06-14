using System;
using _Project.Logic.Components;
using _Project.Logic.Config;
using Leopotam.EcsLite;

namespace _Project.Logic.Systems
{
    public class UpgradeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        private BusinessesConfig _config;
        
        private EcsFilter _requestFilter;
        private EcsFilter _balanceFilter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<BusinessComponent> _statesPool;
        private EcsPool<BusinessConfigRef> _businessRefPool;
        private EcsPool<BuyUpgradeRequest> _upgradeRequestPool;

        public UpgradeSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _balancePool = _world.GetPool<BalanceComponent>();
            _statesPool = _world.GetPool<BusinessComponent>();
            _businessRefPool = _world.GetPool<BusinessConfigRef>();
            _upgradeRequestPool = _world.GetPool<BuyUpgradeRequest>();
            
            _balanceFilter = _world.Filter<BalanceComponent>().End();
            _requestFilter = _world.Filter<BuyUpgradeRequest>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter)
            {
                ref BuyUpgradeRequest request = ref _upgradeRequestPool.Get(entity);
                ref BusinessComponent state = ref _statesPool.Get(entity);
                ref BusinessConfigRef configRef = ref _businessRefPool.Get(entity);

                BusinessConfig config = _config.Businesses[configRef.Index];
                
                TryUpgrade(request.UpgradeIndex, config, ref state);

                _upgradeRequestPool.Del(entity);
            }
        }

        private void TryUpgrade(int index, BusinessConfig config, ref BusinessComponent state)
        {
            switch (index)
            {
                case 0:
                    if (state.Upgrade1Bought) return;
                    if (!TrySpend(config.Upgrade1.Price)) return;
                    state.Upgrade1Bought = true;
                    break;

                case 1:
                    if (state.Upgrade2Bought) return;
                    if (!TrySpend(config.Upgrade2.Price)) return;
                    state.Upgrade2Bought = true;
                    break;
            }
        }
        
        private bool TrySpend(int cost)
        {
            foreach (var entity in _balanceFilter)
            {
                ref BalanceComponent balance = ref _balancePool.Get(entity);
                if (balance.Value >= cost)
                {
                    balance.Value -= cost;
                    return true;
                }
            }
            return false;
        }
    }
}