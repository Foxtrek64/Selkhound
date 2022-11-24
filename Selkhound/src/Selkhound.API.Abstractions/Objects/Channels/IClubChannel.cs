//
//  IClubChannel.cs
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
using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects
{
    /// <summary>
    /// Represents a channel that belongs to a Club.
    /// </summary>
    [PublicAPI]
    public interface IClubChannel : IChannel
    {
        /// <summary>
        /// Gets the sorting position of the channel.
        /// </summary>
        Optional<int> Position { get; }

        /// <summary>
        /// Gets a list of explicit permission overwrites for members and roles.
        /// </summary>
        // IReadOnlyList<IPermissionOverwrite> PermissionOverwrites { get; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Gets the topic of this channel.
        /// </summary>
        string? Topic { get; }

        /// <summary>
        /// Gets a value indicating whether the channel is not safe for work.
        /// </summary>
        bool IsNSFW { get; }

        /// <summary>
        /// Gets the category this channel belongs to.
        /// </summary>
        IChannelCategory? Category { get; }
    }
}
