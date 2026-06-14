using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Constants;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class SaveSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private BusinessesConfig _config;

        private EcsWorld _world;
        
        private EcsFilter _businessFilter;
        private EcsFilter _balanceFilter;
        private EcsPool<BusinessComponent> _statePool;
        private EcsPool<BusinessConfigRef> _configPool;
        private EcsPool<BalanceComponent> _balancePool;

        public SaveSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _statePool = _world.GetPool<BusinessComponent>();
            _configPool = _world.GetPool<BusinessConfigRef>();
            _balancePool = _world.GetPool<BalanceComponent>();
            
            _businessFilter = _world.Filter<BusinessComponent>().Inc<BusinessConfigRef>().End();
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }

        private void Save()
        {
            var data = new Save.GameSaveData
            {
                Businesses = new Save.BusinessSaveData[_config.Businesses.Length]
            };

            foreach (int e in _balanceFilter)
                data.Balance = _balancePool.Get(e).Value;

            foreach (int entity in _businessFilter)
            {
                ref BusinessComponent state = ref _statePool.Get(entity);
                ref BusinessConfigRef configRef = ref _configPool.Get(entity);

                data.Businesses[configRef.Index] = new Save.BusinessSaveData
                {
                    Level = state.Level,
                    IncomeProgress = state.IncomeProgress,
                    Upgrade1Bought = state.Upgrade1Bought,
                    Upgrade2Bought = state.Upgrade2Bought
                };
            }
            
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(GameConstants.SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public void Destroy(IEcsSystems systems)
        {
            Save();
        }
    }
}