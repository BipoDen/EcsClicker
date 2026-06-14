using System;
using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class IncomeProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private BusinessesConfig _config;
        
        private EcsWorld _world;
        
        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<BusinessComponent> _statesPool;
        private EcsPool<BusinessConfigRef> _businessRefPool;

        public IncomeProgressSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _balancePool = _world.GetPool<BalanceComponent>();
            _statesPool = _world.GetPool<BusinessComponent>();
            _businessRefPool = _world.GetPool<BusinessConfigRef>();
            
            _businessFilter = _world.Filter<BusinessComponent>().Inc<BusinessConfigRef>().End();
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _businessFilter)
            {
                ref BusinessComponent state = ref _statesPool.Get(entity);
                
                if(state.Level == 0) return;
                
                ref BusinessConfigRef configRef = ref _businessRefPool.Get(entity);
                BusinessConfig config = _config.Businesses[configRef.Index];

                state.IncomeProgress += Time.deltaTime / config.IncomeDelay;

                if (state.IncomeProgress >= 1)
                {
                    AddIncome(state, config);
                    state.IncomeProgress = 0;
                }
            }
        }

        private void AddIncome(BusinessComponent state, BusinessConfig config)
        {
            int income = BusinessCalc.GetIncome(state, config);
            foreach (var entity in _balanceFilter)
            {
                ref BalanceComponent balance = ref _balancePool.Get(entity);
                balance.Value += income;
            }
        }
    }
}