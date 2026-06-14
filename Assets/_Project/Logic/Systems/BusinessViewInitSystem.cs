using _Project.Logic.Components;
using _Project.Logic.UI;
using _Project.Logic.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class BusinessViewInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private readonly GameplayUI _ui;
        private readonly BusinessCardView _cardPrefab;

        public BusinessViewInitSystem(GameplayUI ui, BusinessCardView cardPrefab)
        {
            _ui = ui;
            _cardPrefab = cardPrefab;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            EcsPool<BusinessViewProvider> viewPool = _world.GetPool<BusinessViewProvider>();
            EcsPool<LevelUpRequest> levelRequestPool = _world.GetPool<LevelUpRequest>();
            EcsPool<BuyUpgradeRequest> buyUpgradePool = _world.GetPool<BuyUpgradeRequest>();

            _filter = _world.Filter<BusinessComponent>().Inc<BusinessConfigRef>().End();

            foreach (var entity in _filter)
            {
                BusinessCardView view = Object.Instantiate(_cardPrefab, _ui.CardsContainer);
                ref BusinessViewProvider viewProvider = ref viewPool.Add(entity);
                viewProvider.View = view;

                view.OnLevelUpClick.AddListener(() =>
                {
                    if (!levelRequestPool.Has(entity))
                        levelRequestPool.Add(entity);
                });

                view.OnUpgrade1Click.AddListener(() =>
                { 
                    if (!buyUpgradePool.Has(entity)) 
                        buyUpgradePool.Add(entity).UpgradeIndex = 0;
                });

                view.OnUpgrade2Click.AddListener(() =>
                {
                    if (!buyUpgradePool.Has(entity))
                        buyUpgradePool.Add(entity).UpgradeIndex = 1;
                        
                });
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            EcsPool<BusinessViewProvider> viewPool = world.GetPool<BusinessViewProvider>();
            EcsFilter filter = world.Filter<BusinessViewProvider>().End();

            foreach (int entity in filter)
            {
                BusinessCardView card = viewPool.Get(entity).View;
                if (card == null) continue;

                card.OnLevelUpClick.RemoveAllListeners();
                card.OnUpgrade1Click.RemoveAllListeners();
                card.OnUpgrade2Click.RemoveAllListeners();
            }
        }
    }
}