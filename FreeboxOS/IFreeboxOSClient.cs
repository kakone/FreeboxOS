using System;
using System.Threading.Tasks;

namespace FreeboxOS
{
    /// <summary>
    /// Freebox OS client interface
    /// </summary>
    public interface IFreeboxOSClient : IDisposable
    {
        /// <summary>
        /// API method call
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="apiUrl">api URL</param>
        /// <param name="method">method to call</param>
        /// <param name="parameters">method parameters</param>
        /// <returns>call result</returns>
        Task<T> GetAsync<T>(string apiUrl, string method, params object[] parameters);
    }
}