#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (IConsoleImplementation.cs) is part of Oetools.Sakoe.
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

using McMaster.Extensions.CommandLineUtils;

namespace Oetools.Sakoe.Utilities {
    public interface IConsoleImplementation : IConsole {
        int CursorTop { get; set; }
        int WindowWidth { get; set; }
        bool CursorVisible { get; set; }
        void SetCursorPosition(int left, int top);
        void Write(string text = null);
        void WriteLine(string text = null);
        bool IsConsoleFullFeatured { get; }
    }
}