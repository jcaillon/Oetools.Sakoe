namespace Oetools.Sakoe.ConLog {
    
    /// <summary>
    /// The threshold above which to log events.
    /// </summary>
    public enum ConsoleLogThreshold {
        
        /// <summary>
        /// Min threshold, log everything.
        /// </summary>
        Debug,
        
        /// <summary>
        /// Log info.
        /// </summary>
        Info,
        
        /// <summary>
        /// Log done.
        /// </summary>
        Done,
        
        /// <summary>
        /// Log warn.
        /// </summary>
        Warn,
        
        /// <summary>
        /// Log errors.
        /// </summary>
        Error,
        
        /// <summary>
        /// Log fatal errors.
        /// </summary>
        Fatal,
        
        /// <summary>
        /// Max threshold level, logs nothing.
        /// </summary>
        None
    }
}