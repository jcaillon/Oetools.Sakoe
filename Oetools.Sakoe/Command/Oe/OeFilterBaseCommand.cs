#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (OeBaseCommand.cs) is part of Oetools.Sakoe.
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
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Builder.Utilities;
using Oetools.Sakoe.Command.Exceptions;
using Oetools.Utilities.Openedge;

namespace Oetools.Sakoe.Command.Oe {
    
    public abstract class OeFilterBaseCommand : OeBaseCommand {
        
        [Option("-if|--include-filter", "", CommandOptionType.SingleValue)]
        protected string IncludeFilter { get; set; }
        
        [Option("-ef|--exclude-filter", "", CommandOptionType.SingleValue)]
        protected string ExcludeFilter { get; set; }
        
        [Option("-irf|--include-regex-filter", "", CommandOptionType.SingleValue)]
        protected string IncludeRegexFilter { get; set; }
        
        [Option("-erf|--exclude-regex-filter", "", CommandOptionType.SingleValue)]
        protected string ExcludeRegexFilter { get; set; }
        
    }
}