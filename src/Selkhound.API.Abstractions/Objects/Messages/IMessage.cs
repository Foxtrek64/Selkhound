//
//  IMessage.cs
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

using OneOf;
using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects.Messages
{
    /// <summary>
    /// Represents a message sent by a user, webhook, or bot.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the unique id of this message.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        /// Gets the creator of this message.
        /// </summary>
        OneOf<IUser, IWebhook> Author { get; }
    }
}
