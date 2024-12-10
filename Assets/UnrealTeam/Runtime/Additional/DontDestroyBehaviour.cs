using UnityEngine;

namespace UnrealTeam.Additional
{
    public class DontDestroyBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        
        private void Awake()
        {
            foreach (GameObject obj in _objects) 
                DontDestroyOnLoad(obj);
        }
    }
}