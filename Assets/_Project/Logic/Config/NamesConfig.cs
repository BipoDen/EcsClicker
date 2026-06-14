using UnityEngine;

namespace _Project.Logic.Config
{
    [CreateAssetMenu(fileName = "NamesConfig", menuName = "Config/NamesConfig")]
    public class NamesConfig : ScriptableObject
    {
        public BusinessNames[] Names;
    }
}