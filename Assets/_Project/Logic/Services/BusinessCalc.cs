using _Project.Logic.Components;
using _Project.Logic.Config;

namespace _Project.Logic.Services
{
    public static class BusinessCalc
    {
        public static int GetIncome(BusinessComponent state, BusinessConfig cfg)
        {
            float multiplier = 1f;
            if (state.Upgrade1Bought) multiplier += cfg.Upgrade1.IncomeMultiplier;
            if (state.Upgrade2Bought) multiplier += cfg.Upgrade2.IncomeMultiplier;
            return (int)(state.Level * cfg.BaseIncome * multiplier);
        }

        public static int GetLevelCost(BusinessComponent state, BusinessConfig cfg)
        {
            return (state.Level + 1) * cfg.BaseCost;
        }
    }
}