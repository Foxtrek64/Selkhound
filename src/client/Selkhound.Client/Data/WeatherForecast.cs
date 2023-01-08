//
//  WeatherForecast.cs
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

using System.Diagnostics.CodeAnalysis;

namespace Selkhound.Client.Data;

/// <summary>
/// A sample weather forecast model.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets the date of the weather forecast.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in Celcius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets a description of the forecast.
    /// </summary>
    [SuppressMessage(
        category: "StyleCop.CSharp.OrderingRules",
        checkId: "SA1206:Declaration keywords should follow order",
        Justification = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3527"
    )]
    public required string Summary { get; set; }
}
