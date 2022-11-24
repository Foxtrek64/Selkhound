//
//  IClub.cs
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
using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects
{
    /// <summary>
    /// Represents a Selkhound Club.
    /// </summary>
    // TODO: Finalize shape.
    public interface IClub
    {
        /// <summary>
        /// Gets the unique id of the club.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        /// Gets the name of the club.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the club's icon.
        /// </summary>
        IImageHash? Icon { get; }

        /// <summary>
        /// Gets the club's splash banner.
        /// </summary>
        IImageHash? Splash { get; }

        /// <summary>
        /// Gets the id of the club's owner.
        /// </summary>
        Snowflake OwnerId { get; }

        /// <summary>
        /// Gets the verification level required for this club.
        /// </summary>
        // VerificationLevel VerificationLevel { get; }

        /// <summary>
        /// Gets the default notification level for the club.
        /// </summary>
        // MessageNotificationLevel DefaultMessageNotifications { get; }

        /// <summary>
        /// Gets the explicit content filter level.
        /// </summary>
        // ExplicitContentFilterLevel ExplicitContentFilter { get; }

        /// <summary>
        /// Gets a list of roles in the club.
        /// </summary>
        // TODO: Replace with IRole.
        IReadOnlyList<Snowflake> Roles { get; }
    }
}
