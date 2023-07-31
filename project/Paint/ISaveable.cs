namespace Paint
{
    /// <summary>
    /// This interfaces declares functionality that should save/load data to the filesystem
    /// </summary>
    interface ISaveable
    {
        /// <summary>
        /// Write instance to disk
        /// </summary>
        /// <param name="path">Path to write to</param>
        /// <returns>Whether or not the operation succeeded</returns>
        bool Save(string path);

        /// <summary>
        /// Read a new instance of class from disk and overwrite instance that the load operation is being performed on
        /// </summary>
        /// <param name="path">Path to read instance from</param>
        /// <returns>Whether or not the operation succeeded</returns>
        bool Load(string path);
    }
}
