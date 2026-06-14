using System;
using _Project.Logic.Config;
using _Project.Logic.Systems;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Logic.EntryPoint
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private BusinessesConfig _config;
        
        private EcsSystems _systems;
        private EcsWorld _world;
        
        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _systems
                .Add(new BusinessInitSystem(_config))
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