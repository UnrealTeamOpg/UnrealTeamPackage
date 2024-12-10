using Cysharp.Threading.Tasks;

namespace UnrealTeam.Common.Modules.Loading
{
    public interface ISceneLoader
    {
        public UniTask LoadAsync(string sceneName);
        public UniTask ReloadAsync();
    }
}