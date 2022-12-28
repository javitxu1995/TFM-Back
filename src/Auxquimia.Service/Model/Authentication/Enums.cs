namespace Auxquimia.Model.Authentication
{
    /// <summary>
    /// Application Types for OAuth clients. The main difference between a javascript and a nativeconfidential application is that the former does not require a secret.
    /// </summary>
    public enum ApplicationTypes
    {
        /// <summary>
        /// Defines the JavaScript
        /// </summary>
        JavaScript = 0,
        /// <summary>
        /// Defines the NativeConfidential
        /// </summary>
        NativeConfidential = 1
    };
}
