using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UnrealTeam.Common.Modules.Assets
{
    public interface IAssetProvider<in TReference>
    {
        public UniTask<T> Load<T>(TReference assetReference, CancellationToken cancellationToken = default) where T : Object;

        public UniTask<T[]> LoadAll<T>(string assetsLabel) where T : Object;
    }
}