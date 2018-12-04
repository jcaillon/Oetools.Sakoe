namespace Oetools.Sakoe.Utilities {
    public enum ConsoleLogThreshold {
        
        /// <summary>
        /// Min threshold, log everything.
        /// </summary>
        Debug,
        Info,
        Done,
        Warn,
        Error,
        Fatal,
        
        /// <summary>
        /// Max threshold level, logs nothing.
        /// </summary>
        None
    }
}