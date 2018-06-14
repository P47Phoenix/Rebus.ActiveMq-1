using System;
using System.Threading.Tasks;
using Rebus.ActiveMq;

namespace Rebus
{
    internal interface IActiveMqSessionHelper
    {
        Task UseSession(Func<ActiveMqSession, Task> action);

        Task<T> UseSession<T>(Func<ActiveMqSession, Task<T>> action);
    }
}