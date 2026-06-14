using System;

namespace _Project.Logic.Config
{
    [Serializable]
    public class BusinessConfig
    {
        public float IncomeDelay;
        public int BaseCost;
        public int BaseIncome;
        public UpgradeConfig Upgrade1;
        public UpgradeConfig Upgrade2;
    }
}