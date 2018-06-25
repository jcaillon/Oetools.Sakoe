#region header
// ========================================================================
// Copyright (c) 2018 - Julien Caillon (julien.caillon@gmail.com)
// This file (ImagesResource.cs) is part of Oetools.HtmlExport.
// 
// Oetools.HtmlExport is a free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Oetools.HtmlExport is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Oetools.HtmlExport. If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
#endregion

using System;
using System.Collections.Generic;

namespace Oetools.HtmlExport.Resources {
    
    public static class ImagesResource {
        
        /// <summary>
        /// Get a resource image as bytes array
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static byte[] GetImageFromResources(string fileName) {
            return Resources.GetBytesFromResource($"{nameof(Oetools)}.{nameof(HtmlExport)}.{nameof(Resources)}.Images.{fileName}");
        }

        private static Dictionary<string, string> _imagesAsBase64String = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        
        /// <summary>
        /// Returns the given image as a string encoded in base 64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetImageBase64Encoded(string value) {
            if (!_imagesAsBase64String.ContainsKey(value)) {
                _imagesAsBase64String.Add(value, Convert.ToBase64String(GetImageFromResources(value)));
            }
            return _imagesAsBase64String[value];
        }
    }
}