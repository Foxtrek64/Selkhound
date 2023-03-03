//
//  AppTokenProvider.cs
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

namespace Selkhound.Client.Shared.Services
{
    /// <summary>
    /// A service which provides an application authentication token.
    /// </summary>
    public sealed class AppTokenProvider : ITokenProvider
    {
        private string? _token = null;

        /// <inheritdoc />
        public async Task<string> GetTokenAsync()
        {
            if (_token is null)
            {
                // App code to resolve the token
                _token = "token";
            }

            return _token;
        }
    }
}
