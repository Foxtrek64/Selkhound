//
//  IChannelCategory.cs
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

using System.Collections.Generic;
using JetBrains.Annotations;
using OneOf;
using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects
{
    /// <summary>
    /// Represents a category in which channels are held.
    /// </summary>
    [PublicAPI]
    public interface IChannelCategory
    {
        /// <summary>
        /// Gets a reference to this category's parent.
        /// </summary>
        /// <remarks>
        /// This field will be omitted for top-level categories where <see cref="Depth"/> is <c>0</c>.
        /// </remarks>
        Optional<IChannelCategory> ParentCategory { get; }

        /// <summary>
        /// Gets the layer on the tree this category belongs to.
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// Gets a list of text channels contained in this category.
        /// </summary>
        IReadOnlyList<OneOf<IChannelCategory, IClubChannel>> ClubChannels { get; }

        /// <summary>
        /// Gets a list of voice channels contained in this category.
        /// </summary>
        // IReadOnlyList<IVoiceChannel> VoiceChannels { get; }
    }
}
