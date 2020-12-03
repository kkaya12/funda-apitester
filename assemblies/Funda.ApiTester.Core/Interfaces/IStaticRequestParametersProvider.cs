namespace Funda.ApiTester.Core.Interfaces
{
    public interface IStaticRequestParametersProvider
    {
        /// <summary>
        /// Returns the API key to use for authorization.
        /// </summary>
        /// <returns></returns>
        string GetKey();

        /// <summary>
        /// Returns the base address for the API.
        /// </summary>
        /// <returns></returns>
        string GetAddressBase();
    }
}
