namespace HtmlElements
{
    /// <summary>
    ///     Lists possible document loading states
    /// </summary>
    public enum DocumentReadyState {

        /// <summary>
        ///     Document not started loading yet.
        /// </summary>
        Uninitialized,

        /// <summary>
        ///     Document is loading.
        /// </summary>
        Loading,

        /// <summary>
        ///     Document has loaded enough and the user can interact with it.
        /// </summary>
        Interactive,

        /// <summary>
        ///     Document has been loaded.
        /// </summary>
        Loaded,

        /// <summary>
        ///     Document has fully loaded.
        /// </summary>
        Complete

    }
}