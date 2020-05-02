using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FreeboxOS
{
    /// <summary>
    /// API base class
    /// </summary>
    public abstract class Api
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class
        /// </summary>
        /// <param name="freeboxOSClient"></param>
        [SuppressMessage("Style", "IDE0057:Use range operator")]
        public Api(IFreeboxOSClient freeboxOSClient)
        {
            FreeboxOSClient = freeboxOSClient;
            var typeName = GetType().Name;
            ApiName = typeName.Substring(0, typeName.Length - 3).ToLower();
        }

        private IFreeboxOSClient FreeboxOSClient { get; }

        /// <summary>
        /// Gets the API name
        /// </summary>
        protected string ApiName { get; }

        /// <summary>
        /// API method call
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="method">method to call</param>
        /// <param name="parameters">method parameters</param>
        /// <returns>call result</returns>
        protected Task<T> GetAsync<T>(string method, params object[] parameters) where T : class
        {
            return FreeboxOSClient.GetAsync<T>(ApiName, method, parameters);
        }
    }
}
