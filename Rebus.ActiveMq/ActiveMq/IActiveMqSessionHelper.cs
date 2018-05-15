using System;
using System.Threading.Tasks;

namespace Rebus.ActiveMq
{
    internal interface IActiveMqSessionHelper
    {
        Task UseSession(Func<ActiveMqSession, Task> action);

        Task<T> UseSession<T>(Func<ActiveMqSession, Task<T>> action);
    }
}