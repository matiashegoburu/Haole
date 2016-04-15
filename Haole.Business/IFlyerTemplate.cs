using Haole.Business.Model;
using System;

namespace Haole.Business
{
    public interface IFlyerTemplate<TFlyerModel> : IDisposable
    {
        void Generate(TFlyerModel model);
    }
}