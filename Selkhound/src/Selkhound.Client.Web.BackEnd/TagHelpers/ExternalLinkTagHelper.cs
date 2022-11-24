//
//  ExternalLinkTagHelper.cs
//
//  Author:
//       LuzFaltex Contributors
//
//  LGPL-3.0 License
//
//  Copyright (c) 2022 LuzFaltex
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Selkhound.Client.Web.BackEnd.TagHelpers
{
    /// <summary>
    /// An HTML Anchor tag which embodies an external link.
    /// </summary>
    /// <remarks>
    /// Used to indicate when a link will open in the browser rather than in the app.
    /// </remarks>
    [HtmlTargetElement("a", Attributes = ExternalAttributeName)]
    internal class ExternalLinkTagHelper : TagHelper
    {
        private const string ExternalAttributeName = "is-external";
        private const string ShowIconAttributeName = "show-icon";

        /// <inheritdoc/>
        public override int Order => -1500;

        /// <summary>
        /// Gets or sets a value indicating whether the anchor should be considered external.
        /// </summary>
        [HtmlAttributeName(ExternalAttributeName)]
        public bool IsExternal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the external link icon.
        /// </summary>
        [HtmlAttributeName(ShowIconAttributeName)]
        public bool ShowIcon { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = output ?? throw new ArgumentNullException(nameof(output));

            var content = await output.GetChildContentAsync();

            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (IsExternal)
            {
                if (ShowIcon)
                {
                    output.AddClass("external", HtmlEncoder.Default);
                }

                output.Attributes.Add("target", "_blank");
                output.Attributes.Add("rel", "noopener");
            }

            output.Content.SetHtmlContent(content);
        }
    }
}
