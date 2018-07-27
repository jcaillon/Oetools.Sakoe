#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ConfigV1EnumerationTest.cs) is part of Oetools.Sakoe.Test.
// 
// Oetools.Sakoe.Test is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.Sakoe.Test is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.Sakoe.Test. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oetools.HtmlExport.Config;
using Oetools.Packager.Core;
using Oetools.Packager.Core.Config;
using Oetools.Sakoe.Config.v1;
using Oetools.Utilities.Archive;
using Oetools.Utilities.Lib.Extension;

namespace Oetools.Sakoe.Test {
    
    [TestClass]
    public class ConfigV1EnumerationTest {

        [TestMethod]
        public void EnsureXmlCompressionLevelHasEquivalent() {
            typeof(XmlCompressionLevel).ForEach<XmlCompressionLevel>((name, value) => {
                Assert.IsTrue(Enum.TryParse(typeof(CompressionLvl), name, out _));
            });
        }

        [TestMethod]
        public void EnsureXmlDeploymentActionLevelHasEquivalent() {
            typeof(XmlDeploymentAction).ForEach<XmlDeploymentAction>((name, value) => {
                Assert.IsTrue(Enum.TryParse(typeof(DeploymentAction), name, out _));
            });
        }
        
        [TestMethod]
        public void EnsureXmlDeployTypeLevelHasEquivalent() {
            typeof(XmlDeployType).ForEach<XmlDeployType>((name, value) => {
                Assert.IsTrue(Enum.TryParse(typeof(DeployType), name, out _));
            });
        }
        
        [TestMethod]
        public void EnsureXmlErrorLevelLevelHasEquivalent() {
            typeof(XmlErrorLevel).ForEach<XmlErrorLevel>((name, value) => {
                Assert.IsTrue(Enum.TryParse(typeof(ErrorLevel), name, out _));
            });
        }
        
        [TestMethod]
        public void EnsureXmlReturnCodeLevelHasEquivalent() {
            typeof(XmlReturnCode).ForEach<XmlReturnCode>((name, value) => {
                Assert.IsTrue(Enum.TryParse(typeof(ReturnCode), name, out _));
            });
        }
        
    }
}