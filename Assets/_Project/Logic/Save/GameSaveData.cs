using System;

namespace _Project.Logic.Save
{
    [Serializable]
    public class GameSaveData
    {
        public int Balance;
        public BusinessSaveData[] Businesses;
    }
}