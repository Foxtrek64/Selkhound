//
//  IUser.cs
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

using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects
{
    /// <summary>
    /// Represents a Selkhound user.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets the unique id of the user.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        /// Gets the username of the user. This is unique per domain.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the domain name of the Home Server the user belongs to.
        /// </summary>
        string HomeServerDomain { get; }

        /// <summary>
        /// Gets the user's avatar hash.
        /// </summary>
        IImageHash? Avatar { get; }
    }
}
