using _Project.Logic.View;
using Leopotam.EcsLite;

namespace _Project.Logic.Components
{
    struct BusinessViewProvider : IEcsAutoReset<BusinessViewProvider>
    {
        public BusinessCardView View;
        
        public void AutoReset(ref BusinessViewProvider c) => c.View = null;
    }
}