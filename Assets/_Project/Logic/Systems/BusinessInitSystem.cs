using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Constants;
using _Project.Logic.Save;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.Systems
{
    public class BusinessInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private GameSaveData _data;
        private BusinessesConfig _config;

        public BusinessInitSystem(BusinessesConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _data = LoadSave();
            
            CreateBalance();
            CreateBusinesses();
        }

        private void CreateBalance()
        {
            var entity = _world.NewEntity();
            ref BalanceComponent balance = ref _world.GetPool<BalanceComponent>().Add(entity);
            balance.Value = _data?.Balance ?? 0;
        }

        private void CreateBusinesses()
        {
            EcsPool<BusinessComponent> statePool = _world.GetPool<BusinessComponent>();
            EcsPool<BusinessConfigRef> configRefPool = _world.GetPool<BusinessConfigRef>();

            for (int i = 0; i < _config.Businesses.Length; i++)
            {
                var entity = _world.NewEntity();
                
                ref BusinessConfigRef configRef = ref configRefPool.Add(entity);
                configRef.Index = i;
                
                ref BusinessComponent state = ref statePool.Add(entity);
                
                BusinessSaveData businessSave = GetBusinessSave(_data, i);

                if (businessSave != null)
                {
                    state.Level = businessSave.Level;
                    state.IncomeProgress = businessSave.IncomeProgress;
                    state.Upgrade1Bought = businessSave.Upgrade1Bought;
                    state.Upgrade2Bought = businessSave.Upgrade2Bought;
                }
                else
                {
                    state.Level = (i == 0) ? 1 : 0;
                    state.IncomeProgress = 0f;
                    state.Upgrade1Bought = false;
                    state.Upgrade2Bought = false;
                }
            }
        }
        
        private GameSaveData LoadSave()
        {
            if (!PlayerPrefs.HasKey(GameConstants.SAVE_KEY)) return null;

            string json = PlayerPrefs.GetString(GameConstants.SAVE_KEY);
            if (string.IsNullOrEmpty(json)) return null;

            return JsonUtility.FromJson<GameSaveData>(json);
        }
        
        private BusinessSaveData GetBusinessSave(GameSaveData save, int index)
        {
            if (save?.Businesses == null) return null;
            if (index >= save.Businesses.Length) return null;
            return save.Businesses[index];
        }
    }
}