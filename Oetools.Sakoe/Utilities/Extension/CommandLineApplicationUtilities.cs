#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (CommandLineApplicationUtilities.cs) is part of Oetools.Sakoe.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Oetools.Sakoe.Command;

namespace Oetools.Sakoe.Utilities.Extension {
    public static class CommandLineApplicationUtilities {
        
        public static Type GetTypeFromCommandLine(this CommandLineApplication app) {
            var stack = new Stack<CommandLineApplication>();
            stack.Push(app);
            var rootApp = app;
            while (rootApp.Parent != null) {
                rootApp = rootApp.Parent;
                stack.Push(rootApp);
            }

            Type currentType = typeof(MainCommand);
            while (stack.Count > 0) {
                var subCommands = Attribute.GetCustomAttributes(currentType, typeof(SubcommandAttribute), true).OfType<SubcommandAttribute>().ToList();
                var commandName = stack.Pop().Name;
                foreach (var subCommand in subCommands) {
                    foreach (var subCommandType in subCommand.Types) {
                        var commandAttr = Attribute.GetCustomAttribute(subCommandType, typeof(CommandAttribute), true) as CommandAttribute;
                        if (commandAttr != null && commandAttr.Name.Equals(commandName)) {
                            currentType = subCommandType;
                        }
                    }
                }
            }
            
            return currentType;
        }

        /// <summary>
        /// Get the command line that calls the given type.
        /// For instance, it will return "sakoe project init" if the type given is the command for init.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string GetFullCommandLine(this CommandLineApplication app) {
            var sb = new StringBuilder();
            var stack = new Stack<string>();
            for (var cmd = app; cmd != null; cmd = cmd.Parent) {
                stack.Push(cmd.Name);
            }
            while (stack.Count > 0) {
                sb.Append(stack.Pop());
                if (stack.Count > 0) {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get the command line that calls the given type.
        /// For instance, it will return "sakoe project init" if the type given is the command for init.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFullCommandLine(this Type type) {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var attribute in GetCommandStackFromType(type)) {
                if (i > 0) {
                    sb.Append(" ");
                }
                sb.Append(attribute.Name);
                i++;
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Returns a "stack" of <see cref="CommandAttribute"/> to reach the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<CommandAttribute> GetCommandStackFromType(this Type type) {
            var subCommands = Attribute.GetCustomAttributes(typeof(MainCommand), typeof(SubcommandAttribute), true).OfType<SubcommandAttribute>().ToList();
            var stack = new Stack<Tuple<List<CommandAttribute>, List<SubcommandAttribute>>>();
            stack.Push(new Tuple<List<CommandAttribute>, List<SubcommandAttribute>>(new List<CommandAttribute>{
                (CommandAttribute) Attribute.GetCustomAttribute(typeof(MainCommand), typeof(CommandAttribute))
            }, subCommands));
            while (stack.Count > 0) {
                var tuple = stack.Pop();
                subCommands = tuple.Item2;
                foreach (var subCommand in subCommands) {
                    if (subCommand.Types == null) {
                        continue;
                    }
                    foreach (var subCommandType in subCommand.Types) {
                        var commandStack = tuple.Item1.ToList();
                        commandStack.Add((CommandAttribute) Attribute.GetCustomAttribute(subCommandType, typeof(CommandAttribute), true));
                        if (subCommandType == type) {
                            return commandStack;
                        }
                        var subCommandList = Attribute.GetCustomAttributes(subCommandType, true).OfType<SubcommandAttribute>().ToList();
                        stack.Push(new Tuple<List<CommandAttribute>, List<SubcommandAttribute>>(commandStack, subCommandList));
                    }
                }
            }
            return null;
        }
    }
}