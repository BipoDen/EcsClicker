using UnityEngine;

namespace _Project.Logic.Config
{
    [CreateAssetMenu(fileName = "BusinessesConfig", menuName = "Config/BusinessesConfig")]
    public class BusinessesConfig : ScriptableObject
    {
        public BusinessConfig[] Businesses;
    }
}