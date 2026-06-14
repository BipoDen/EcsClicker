using System;
using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Services;
using _Project.Logic.UI;
using _Project.Logic.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class ViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameplayUI _ui;
        private BusinessesConfig _config;
        private NamesConfig _namesConfig;

        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<BusinessComponent> _statesPool;
        private EcsPool<BusinessConfigRef> _businessRefPool;
        private EcsPool<BusinessViewProvider> _businessViewPool;

        public ViewSystem(GameplayUI ui, BusinessesConfig config, NamesConfig namesConfig)
        {
            _ui = ui;
            _config = config;
            _namesConfig = namesConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _balancePool = _world.GetPool<BalanceComponent>();
            _statesPool = _world.GetPool<BusinessComponent>();
            _businessViewPool = _world.GetPool<BusinessViewProvider>();
            _businessRefPool = _world.GetPool<BusinessConfigRef>();

            _businessFilter = _world.Filter<BusinessComponent>().Inc<BusinessConfigRef>().Inc<BusinessViewProvider>()
                .End();
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            UpdateBusinessViews();
            UpdateBalanceView();
        }

        private void UpdateBalanceView()
        {
            foreach (var entity in _balanceFilter)
            {
                ref BalanceComponent component = ref _balancePool.Get(entity);
                _ui.SetBalance(component.Value);
            }
        }

        private void UpdateBusinessViews()
        {
            foreach (var entity in _businessFilter)
            {
                ref BusinessComponent state = ref _statesPool.Get(entity);
                ref BusinessConfigRef configRef = ref _businessRefPool.Get(entity);
                ref BusinessViewProvider viewProvider = ref _businessViewPool.Get(entity);
                
                BusinessCardView view = viewProvider.View;
                BusinessConfig config = _config.Businesses[configRef.Index];
                BusinessNames names = _namesConfig.Names[configRef.Index];
                
                view.SetNameText(names.BusinessName);
                view.SetLevelText(state.Level);
                view.SetIncomeText(BusinessCalc.GetIncome(state, config));
                view.SetIncomeProgress(state.IncomeProgress);
                view.SetLevelUpgradeCost(BusinessCalc.GetLevelCost(state, config));
                
                view.SetUpgrade1Interactable(!state.Upgrade1Bought);
                view.SetUpgrade2Interactable(!state.Upgrade2Bought);
                view.SetUpgrade1Text(state.Upgrade1Bought ? "Куплено" :  $"{names.Upgrade1Name} \n Income +{config.Upgrade1.IncomeMultiplier * 100}% \n Price: {config.Upgrade1.Price}$");
                view.SetUpgrade2Text(state.Upgrade2Bought ? "Куплено" :  $"{names.Upgrade2Name} \n Income +{config.Upgrade2.IncomeMultiplier * 100}% \n Price: {config.Upgrade2.Price}$");
            }
        }
    }
}