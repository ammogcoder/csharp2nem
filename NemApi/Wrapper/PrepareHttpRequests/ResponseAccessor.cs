using System;

namespace CSharp2nem.PrepareHttpRequests
{
    /// <summary>
    /// The response accessor for the opperation performed.
    /// </summary>
    /// <typeparam name="TReturnType">The deserialised object to return</typeparam>
    /// <remarks>
    /// The content type is inferred by the opperation performed.
    /// </remarks>
    public class ResponseAccessor<TReturnType>
    {
        /// <summary>
        /// The content of variable type to be returned.
        /// </summary>
        public TReturnType Content { get; set; }

        /// <summary>
        /// The exception to be returned if an exception occured.
        /// </summary>
        public Exception Ex { get; set; }
    }
}
