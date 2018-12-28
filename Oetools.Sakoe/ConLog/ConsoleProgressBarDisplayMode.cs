namespace Oetools.Sakoe.ConLog {
    
    /// <summary>
    /// The display mode of progress bars.
    /// </summary>
    public enum ConsoleProgressBarDisplayMode {
        
        /// <summary>
        /// Show progress bars, but hide them once finished.
        /// </summary>
        On,
        
        /// <summary>
        /// Do not show progress bars.
        /// </summary>
        Off,
        
        /// <summary>
        /// Show progress bars and leave them on once finished.
        /// </summary>
        Stay
    }
}