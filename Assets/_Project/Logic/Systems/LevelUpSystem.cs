using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Services;
using Leopotam.EcsLite;

namespace _Project.Logic.Systems
{
    public class LevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        private BusinessesConfig _config;

        private EcsFilter _requestFilter;
        private EcsFilter _balanceFilter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<BusinessComponent> _statesPool;
        private EcsPool<BusinessConfigRef> _businessRefPool;
        private EcsPool<LevelUpRequest> _levelRequestPool;

        public LevelUpSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _balancePool = _world.GetPool<BalanceComponent>();
            _statesPool = _world.GetPool<BusinessComponent>();
            _businessRefPool = _world.GetPool<BusinessConfigRef>();
            _levelRequestPool = _world.GetPool<LevelUpRequest>();
            
            _balanceFilter = _world.Filter<BalanceComponent>().End();
            _requestFilter = _world.Filter<LevelUpRequest>().Inc<BusinessComponent>().Inc<BusinessConfigRef>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _requestFilter)
            {
                ref BusinessComponent state = ref _statesPool.Get(entity);
                ref BusinessConfigRef configRef = ref _businessRefPool.Get(entity);

                BusinessConfig config = _config.Businesses[configRef.Index];
                
                int cost = BusinessCalc.GetLevelCost(state, config);
                
                TryBuyLevelUp(cost, ref state);
                
                _levelRequestPool.Del(entity);
            }
        }

        private void TryBuyLevelUp(int cost, ref BusinessComponent state)
        {
            foreach (var entity in _balanceFilter)
            {
                ref BalanceComponent balance = ref _balancePool.Get(entity);
                if (balance.Value >= cost)
                {
                    balance.Value -= cost;
                    state.Level++;
                }
            }
        }
    }
}