using Haole.Business.Model;
using System;
using System.Drawing;

namespace Haole.Business
{
    public interface IFlyerGenerator : IDisposable
    {
        void Generate(FlyerModel model);
    }
}