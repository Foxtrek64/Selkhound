//
//  WeatherForecastService.cs
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

namespace Selkhound.Client.Data
{
    /// <summary>
    /// A Weather Forecast Service which creates forecasts.
    /// </summary>
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <summary>
        /// Gets a five-day forecasts tarting at the specified <paramref name="startDate"/>.
        /// </summary>
        /// <param name="startDate">The start of the forecast week.</param>
        /// <returns>An array containing a five-day forecast.</returns>
        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            return Task.FromResult
            (
                Enumerable.Range(1, 5)
                          .Select
                          (
                              index => new WeatherForecast
                              {
                                  Date = startDate.AddDays(index),
                                  TemperatureC = Random.Shared.Next(-20, 55),
                                  Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                              }
                          )
                          .ToArray()
            );
        }
    }
}
