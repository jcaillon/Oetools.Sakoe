#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (HelpTextGenerator.cs) is part of Oetools.Sakoe.
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

using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Oetools.Utilities.Lib;

namespace Oetools.Sakoe.Utilities {
    
    public class HelpTextGenerator : DefaultHelpTextGenerator {
        
        protected override void GenerateHeader(CommandLineApplication application, TextWriter output) {
            if (!string.IsNullOrEmpty(application.Description)) {
                output.WriteLine(application.Description);
                output.WriteLine();
            }
        }

        protected override void GenerateFooter(CommandLineApplication application, TextWriter output) {
            output.WriteLine();
            output.WriteLine("Extended help:");
            output.WriteLine(application.ExtendedHelpText);
            output.WriteLine();
        }

        public static void DrawLogo(TextWriter output) {
            output.WriteLine();
            output.WriteLine(@"                '`.        ");
            output.WriteLine(@" '`.    .^      \  \       == SAKOE ==");
            output.WriteLine(@"  \ \  /;/       \ \\      a Swiss Army Knife for OpenEdge");
            output.WriteLine(@"   \ \/_/_________\  \     " + $"running with {(Utils.IsNetFrameworkBuild ? ".netframework" : $".netcore-{(Utils.IsRuntimeWindowsPlatform ? "win" : "unix")}")}");
            output.WriteLine(@"    `/ .           _  \    " + $"v{typeof(HelpTextGenerator).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
            output.WriteLine(@"     \________________/    ");
            output.WriteLine();
        }
    }
    
}