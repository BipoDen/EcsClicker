using System;
using _Project.Logic.Components;
using _Project.Logic.Config;
using _Project.Logic.Systems;
using _Project.Logic.UI;
using _Project.Logic.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.EntryPoint
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private BusinessesConfig _config;
        [SerializeField] private NamesConfig _namesConfig;
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private BusinessCardView _cardPrefab;
        
        private EcsSystems _systems;
        private EcsWorld _world;
        
        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _systems
                .Add(new BusinessInitSystem(_config))
                .Add(new BusinessViewInitSystem(_gameplayUI, _cardPrefab))
                .Add(new ViewSystem(_gameplayUI, _config, _namesConfig))
                .Add(new IncomeProgressSystem(_config))
                .Add(new LevelUpSystem(_config))
                .Add(new UpgradeSystem(_config))
                .Add(new SaveSystem(_config))
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if(_systems == null)
                return;
            
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}