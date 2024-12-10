using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnrealTeam.Common.Modules.Configs
{
    public interface IConfigLoader
    {
        public UniTask LoadSingle<TConfig>(AssetReference assetReference)
            where TConfig : Object, ISingleConfig;

        public UniTask LoadMultiple<TConfig>(AssetReference assetReference)
            where TConfig : Object, IMultipleConfig;

        public UniTask LoadMultipleAll<TConfig>(string label)
            where TConfig : Object, IMultipleConfig;

        public void SetMultipleManually<TConfig>(TConfig config)
            where TConfig : Object, IMultipleConfig;

        public void SetSingleManually<TConfig>(TConfig config)
            where TConfig : Object, ISingleConfig;

        public void UnloadMultiple<TConfig>(string configId)
            where TConfig : Object, IMultipleConfig;
    }
}