#region header

// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (AProgressBar.cs) is part of Oetools.Sakoe.
// 
// Oetools.Sakoe is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Sakoe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================

#endregion

using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Oetools.Sakoe.ShellProgressBar;

namespace Oetools.Sakoe.Utilities {
    
    public class ConsoleProgressBar : IDisposable {
        
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor? ForegroundColorDone { get; set; } = ConsoleColor.Green;

        public ConsoleColor? BackgroundColor { get; set; } = ConsoleColor.DarkGray;
        
        public ConsoleColor? ForegroundColorUncomplete { get; set; } = ConsoleColor.Red;

        public ConsoleColor TextColor { get; set; }

        public char ForegroundCharacter { get; set; } = '■'; // \u2593

        public char BackgroundCharacter { get; set; } = '■';

        /// <summary>
        /// Clear the progress bar when it stops.
        /// </summary>
        public bool ClearProgressBarOnStop { get; set; }

        /// <summary>
        /// The minimum time interval that should elapse between 2 refresh of the progress bar.
        /// The lower the smoother the animation. But low value degrade performances.
        /// </summary>
        public int MinimumRefreshRateInMilliseconds { get; set; } = 100;
        
        /// <summary>
        /// The maximum time interval that should elapse between 2 refresh of the progress bar.
        /// Should be less than 1s to correctly display the clock.
        /// </summary>
        public int MaximumRefreshRateInMilliseconds { get; set; } = 900;

        private Stopwatch _stopwatch;
        private Stopwatch _drawStopWatch;
        private static object _lock = new object();

        private int _maxTicks;
        private int _currentTick;
        private string _message;

        private int _lastWindowWidth;
        private int _lastLine2Width;

        private System.Timers.Timer _timer;

        private readonly IConsoleImplementation _console;

        /// <summary>
        /// New progress bar.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="maxTicks"></param>
        /// <param name="message"></param>
        public ConsoleProgressBar(IConsoleImplementation console, int maxTicks, string message) {
            _console = console;
            TextColor = _console.ForegroundColor;
            _maxTicks = Math.Max(1, maxTicks);
            _message = message;
        }

        public void Dispose() {
            Stop();
        }

        /// <summary>
        /// Is the progress bar running?
        /// </summary>
        public bool IsRunning => _stopwatch != null;

        /// <summary>
        /// Current progress value.
        /// </summary>
        public int CurrentTick => _currentTick;

        /// <summary>
        /// Maximum value = the value of the <see cref="CurrentTick"/> when the progress bar is shown as full.
        /// </summary>
        public int MaxTicks {
            get => _maxTicks;
            set {
                if (Monitor.TryEnter(_lock, 100)) {
                    _maxTicks = Math.Max(1, value);
                    Monitor.Exit(_lock);
                }
            }
        }

        /// <summary>
        /// Stop the progress bar, resetting the console state as it was before displaying the bar.
        /// </summary>
        /// <returns></returns>
        public bool Stop() {
            if (_stopwatch == null) {
                return false;
            }
            
            _console.CursorVisible = true;
            _console.ResetColor();
            _timer?.Close();
            _timer?.Dispose();
            _timer = null;

            // make sure to draw the latest state
            if (ClearProgressBarOnStop) {
                ClearProgressBar();
                _console.SetCursorPosition(0, _console.CursorTop - 2);
            } else {
                if (Monitor.TryEnter(_lock, 500)) {
                    try {
                        DrawProgressBar(true);
                    } finally {
                        Monitor.Exit(_lock);
                    }
                }
            }
            
            _stopwatch = null;
            TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
            return true;
        }

        /// <summary>
        /// Report a progression that should be displayed.
        /// </summary>
        /// <param name="newTickCount">The value should be between 0 and <see cref="MaxTicks"/>.</param>
        /// <param name="message"></param>
        public void Tick(int newTickCount, string message = null) {
            if (_stopwatch == null) {
                InitializeProgressBar();
            }
            if (Monitor.TryEnter(_lock, 100)) {
                _currentTick = Math.Min(Math.Max(newTickCount, 0), _maxTicks);
                _message = message;
                DrawProgressBar();
                Monitor.Exit(_lock);
            }
        }

        /// <summary>
        /// Draw / update the progress bar on the console.
        /// </summary>
        /// <param name="forceRedraw"></param>
        private void DrawProgressBar(bool forceRedraw = false) {
            if (!forceRedraw) {
                if (_drawStopWatch == null) {
                    _drawStopWatch = Stopwatch.StartNew();
                } else if (_drawStopWatch.ElapsedMilliseconds < MinimumRefreshRateInMilliseconds) {
                    return;
                } else {
                    _drawStopWatch?.Restart();
                }
            }
            
            if (_currentTick.Equals(_maxTicks)) {
                _stopwatch.Stop();
            }

            if (_lastWindowWidth != _console.WindowWidth) {
                _console.CursorVisible = false;
                ClearProgressBar();
            }

            _console.SetCursorPosition(0, _console.CursorTop - 2);

            var maxWidth = _lastWindowWidth - 1;

            // line1, progress bar
            _console.ForegroundColor = _timer == null ? (_maxTicks == _currentTick ? ForegroundColorDone : ForegroundColorUncomplete) ?? ForegroundColor : ForegroundColor;
            var progress = Math.Min(1, (double) _currentTick / _maxTicks);
            TaskbarProgress.SetValue(progress * 100, 100);
            var progressWidth = (int) Math.Round(progress * maxWidth);
            if (progressWidth < maxWidth) {
                _console.Write(new string(ForegroundCharacter, progressWidth));
                if (BackgroundColor != null) {
                    _console.ForegroundColor = (ConsoleColor) BackgroundColor;
                }

                _console.WriteLine(new string(BackgroundCharacter, maxWidth - progressWidth));
            } else {
                _console.WriteLine(new string(ForegroundCharacter, maxWidth));
            }

            // line2, info
            _console.ForegroundColor = TextColor;
            var line2 = $"{(int) (progress * 100)}%".PadRight(4, ' ');
            if (maxWidth > 15) {
                var elapsed = _stopwatch.Elapsed;
                line2 = $"[{elapsed.Minutes:D2}:{elapsed.Seconds:D2}] {line2}";
            }

            if (!string.IsNullOrEmpty(_message)) {
                line2 = $"{line2} {_message}";
                if (line2.Length > maxWidth) {
                    line2 = $"{line2.Substring(0, maxWidth - 3)}...";
                }
            }

            _lastLine2Width = line2.Length;
            if (line2.Length < maxWidth) {
                line2 = line2.PadRight(maxWidth, ' ');
            }

            _console.WriteLine(line2);

            _lastWindowWidth = _console.WindowWidth;
        }

        /// <summary>
        /// Clear the progress bar from the console.
        /// </summary>
        private void ClearProgressBar() {
            int nbLines = _lastWindowWidth <= 0 ? 0 : (int) Math.Ceiling((double) (_lastWindowWidth - 1) / _console.WindowWidth);
            if (_lastLine2Width > 0) {
                nbLines += (int) Math.Ceiling((double) _lastLine2Width / _console.WindowWidth);
            }

            if (nbLines > 1) {
                _console.SetCursorPosition(0, _console.CursorTop - nbLines);
                _console.ResetColor();
                _console.Write(new string(' ', _console.WindowWidth * nbLines));
                _console.SetCursorPosition(0, _console.CursorTop - nbLines + 2);
            }

            _lastWindowWidth = _console.WindowWidth;
        }

        /// <summary>
        /// Initialize progress bar.
        /// </summary>
        private void InitializeProgressBar() {
            _console.CursorVisible = false;
            _stopwatch = Stopwatch.StartNew();
            _console.WriteLine();
            _console.WriteLine();
            _lastWindowWidth = _console.WindowWidth;
            TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);

            _timer = new System.Timers.Timer(MaximumRefreshRateInMilliseconds);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        private void OnTimerElapsed(object sender, ElapsedEventArgs e) {
            if (_timer == null) {
                return;
            }

            _timer.Enabled = false;

            if (Monitor.TryEnter(_lock, 100)) {
                try {
                    DrawProgressBar();
                } finally {
                    Monitor.Exit(_lock);
                    _timer.Enabled = true;
                }
            }
        }
    }
}