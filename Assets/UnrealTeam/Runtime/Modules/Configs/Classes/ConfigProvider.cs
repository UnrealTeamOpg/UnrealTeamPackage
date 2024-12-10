using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnrealTeam.Common.Extensions;
using UnrealTeam.Common.Modules.Assets;
using Object = UnityEngine.Object;

namespace UnrealTeam.Common.Modules.Configs
{
    public class ConfigProvider : IConfigLoader, IConfigAccess
    {
        private readonly IAssetProvider<AssetReference> _assetProvider;
        private readonly Dictionary<Type, Object> _singleConfigs = new();
        private readonly Dictionary<Type, Dictionary<string, IMultipleConfig>> _multipleConfigs = new();


        public ConfigProvider(IAssetProvider<AssetReference> assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask LoadSingle<TConfig>(AssetReference assetReference)
            where TConfig : Object, ISingleConfig
            => _singleConfigs[typeof(TConfig)] = await _assetProvider.Load<TConfig>(assetReference);

        public async UniTask LoadMultiple<TConfig>(AssetReference path)
            where TConfig : Object, IMultipleConfig
        {
            Dictionary<string, IMultipleConfig> configMap =
                _multipleConfigs.GetOrAdd(typeof(TConfig), () => new Dictionary<string, IMultipleConfig>());

            var config = await _assetProvider.Load<TConfig>(path);
            configMap[config.Id] = config;
        }

        public async UniTask LoadMultipleAll<TConfig>(string path)
            where TConfig : Object, IMultipleConfig
        {
            Dictionary<string, IMultipleConfig> configMap =
                _multipleConfigs.GetOrAdd(typeof(TConfig), () => new Dictionary<string, IMultipleConfig>());

            TConfig[] configs = await _assetProvider.LoadAll<TConfig>(path);
            foreach (TConfig config in configs)
                configMap[config.Id] = config;
        }

        public void SetMultipleManually<TConfig>(TConfig config)
            where TConfig : Object, IMultipleConfig
        {
            Dictionary<string, IMultipleConfig> configMap =
                _multipleConfigs.GetOrAdd(typeof(TConfig), () => new Dictionary<string, IMultipleConfig>());

            configMap[config.Id] = config;
        }

        public void SetSingleManually<TConfig>(TConfig config)
            where TConfig : Object, ISingleConfig
            => _singleConfigs[typeof(TConfig)] = config;

        public void UnloadMultiple<TConfig>(string configId)
            where TConfig : Object, IMultipleConfig
            => _multipleConfigs[typeof(TConfig)].Remove(configId);

        public TConfig GetSingle<TConfig>()
            where TConfig : Object, ISingleConfig
            => _singleConfigs[typeof(TConfig)] as TConfig;

        public TConfig GetMultiple<TConfig>(string configId)
            where TConfig : Object, IMultipleConfig
        {
            if (!_multipleConfigs.TryGetValue(typeof(TConfig), out Dictionary<string, IMultipleConfig> configMap))
                return null;

            if (!configMap.TryGetValue(configId, out IMultipleConfig config))
                return null;

            return (TConfig)config;
        }

        public TConfig[] GetMultipleAll<TConfig>()
            where TConfig : Object, IMultipleConfig
        {
            if (!_multipleConfigs.TryGetValue(typeof(TConfig), out Dictionary<string, IMultipleConfig> configMap))
                return Array.Empty<TConfig>();

            return configMap.Values.Cast<TConfig>().ToArray();
        }
    }
}