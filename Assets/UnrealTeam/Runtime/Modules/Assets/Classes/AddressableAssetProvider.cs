using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace UnrealTeam.Common.Modules.Assets
{
    public class AddressableAssetProvider : IAssetProvider<AssetReference>
    {
        public async UniTask<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken = default) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            T asset = await handle.ToUniTask(cancellationToken: cancellationToken);
            return asset;
        }

        public async UniTask<T[]> LoadAll<T>(string assetsLabel) where T : Object
        {
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(assetsLabel);
            IList<IResourceLocation> locations = await locationsHandle.ToUniTask();
            AsyncOperationHandle<IList<T>> assetsHandle = Addressables.LoadAssetsAsync<T>(locations, null, true);
            IList<T> assets = await assetsHandle.ToUniTask();
            return assets.ToArray();
        }
    }
}