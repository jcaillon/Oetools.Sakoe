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

namespace Oetools.Sakoe.Utilities {
    
    public class AProgressBar {
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor? ForegroundColorDone { get; set; } = ConsoleColor.Green;

        public ConsoleColor? BackgroundColor { get; set; } = ConsoleColor.DarkGray;

        public ConsoleColor TextColor { get; set; }

        public char ForegroundCharacter { get; set; } = '■'; // 

        public char BackgroundCharacter { get; set; } = '■';

        private Stopwatch _stopwatch;
        private static object _lock = new object();

        private int _maxTicks;
        private int _currentTick;
        private string _message;

        private int _lastWindowWidth;
        private int _lastLine2Width;

        private System.Timers.Timer _timer;
        
        public AProgressBar(int maxTicks, string message) {
            TextColor = Console.ForegroundColor;
            _maxTicks = Math.Max(1, maxTicks);
            _message = message;
        }

        public int CurrentTick => _currentTick;

        public int MaxTicks {
            get => _maxTicks;
            set {
                Interlocked.Exchange(ref _maxTicks, Math.Max(1, value));
                Tick(_currentTick);
            }
        }

        public void Stop() {
            Console.CursorVisible = true;
            Console.ResetColor();
            _timer?.Close();
            _timer?.Dispose();
            _timer = null;
            
            // make sure to draw the latest state
            if (Monitor.TryEnter(_lock)) {
                try {
                    DrawProgressBar();
                } finally {
                    Monitor.Exit(_lock);     
                }
            }
            
            _stopwatch = null;
        }

        public void Tick(int newTickCount, string message = null) {
            if (Monitor.TryEnter(_lock, 500)) {
                try {
                    _currentTick = newTickCount;
                    _message = message;
                } finally {
                    Monitor.Exit(_lock);
                }
            }
            if (_stopwatch == null) {
                InitializeProgressBar();
            }
        }

        private void DrawProgressBar() {
            if (_currentTick.Equals(_maxTicks)) {
                _stopwatch.Stop();
            }

            if (_lastWindowWidth != Console.WindowWidth) {
                Console.CursorVisible = false;
                int nbLines = _lastWindowWidth <= 0 ? 0 : (int) Math.Ceiling((double) (_lastWindowWidth - 1) / Console.WindowWidth);
                if (_lastLine2Width > 0) {
                    nbLines += (int) Math.Ceiling((double) _lastLine2Width / Console.WindowWidth);
                }
                if (nbLines > 1) {
                    Console.SetCursorPosition(0, Console.CursorTop - nbLines);
                    Console.ResetColor();
                    Console.Write(new string(' ', Console.WindowWidth * nbLines));
                    Console.SetCursorPosition(0, Console.CursorTop - nbLines + 2);
                }
                _lastWindowWidth = Console.WindowWidth;
            }
            
            Console.SetCursorPosition(0, Console.CursorTop - 2);

            var maxWidth = _lastWindowWidth - 1;

            // line1, progress bar
            Console.ForegroundColor = !_stopwatch.IsRunning ? ForegroundColorDone ?? ForegroundColor : ForegroundColor;
            var progress = Math.Min(1, (decimal) _currentTick / _maxTicks);
            var progressWidth = (int) Math.Round(progress * maxWidth);
            if (progressWidth < maxWidth) {
                Console.Write(new string(ForegroundCharacter, progressWidth));
                if (BackgroundColor != null) {
                    Console.ForegroundColor = (ConsoleColor) BackgroundColor;
                }

                Console.WriteLine(new string(BackgroundCharacter, maxWidth - progressWidth));
            } else {
                Console.WriteLine(new string(ForegroundCharacter, maxWidth));
            }

            // line2, info
            Console.ForegroundColor = TextColor;
            var line2 = $"{((int) progress * 100).ToString().PadLeft(3, ' ')}%";
            if (maxWidth > 15) {
                var elapsed = _stopwatch.Elapsed;
                line2 = $"[{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}] {line2}";
            }

            line2 = $"{line2} {_message}";
            if (line2.Length > maxWidth) {
                line2 = $"{line2.Substring(0, maxWidth - 3)}...";
            }
            _lastLine2Width = line2.Length;
            if (line2.Length < maxWidth) {
                line2 = line2.PadRight(maxWidth, ' ');
            }
            Console.WriteLine(line2);
            
            _lastWindowWidth = Console.WindowWidth;
        }

        private void InitializeProgressBar() {
            Console.CursorVisible = false;
            _stopwatch = Stopwatch.StartNew();
            Console.WriteLine();
            Console.WriteLine();
            _lastWindowWidth = Console.WindowWidth;
            //TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);

            _timer = new System.Timers.Timer(500); // it was 15ms, set to 33ms for 30 frames per second
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e) {
            // We turn the timer off while we process the tick, in case the
            // actions take longer than the tick itself...
            if (_timer == null) {
                return;
            }
            _timer.Enabled = false;

            if (Monitor.TryEnter(_lock)) {
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