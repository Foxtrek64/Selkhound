//
//  SemanticVersion.cs
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
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Remora.Results;
using Selkhound.Common.DataTypes.Errors;

namespace Selkhound.Common.DataTypes
{
    /// <summary>
    /// Represents a Semantic Version as defined by the Semantic Versioning specification.
    /// </summary>
    /// <param name="Major">The <see cref="Major"/> component, to be incremented for breaking additions or changes.</param>
    /// <param name="Minor">The <see cref="Minor"/> component, to be incremented for non-breaking additions or changes.</param>
    /// <param name="Patch">The <see cref="Patch"/> component, to be incremented for non-breaking bug fixes.</param>
    /// <param name="PreReleaseLabel">The <see cref="PreReleaseLabel"/> component, marking this component as being in beta and providing .</param>
    /// <param name="BuildLabel">The value of the <see cref="BuildLabel"/> component of this <see cref="SemanticVersion"/>.</param>
    /// <remarks>This type is compliant with <see href="https://semver.org/spec/v2.0.0.html">SemVer v2.0.0</see>.</remarks>
    public readonly partial record struct SemanticVersion
    (
        [ValueRange(0, int.MaxValue)]
        int Major,
        [ValueRange(0, int.MaxValue)]
        int Minor,
        [ValueRange(0, int.MaxValue)]
        int Patch,
        string? PreReleaseLabel = null,
        string? BuildLabel = null
    )
    : IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>, IParsable<SemanticVersion>, IComparisonOperators<SemanticVersion, SemanticVersion, bool>
    {
        private static readonly Regex SemVerRegex = MyRegex();

        /// <summary>
        /// Major version X (X.y.z | X > 0) MUST be incremented if any backwards incompatible changes are introduced to the public API.
        /// It MAY also include minor and patch level changes. Patch and minor versions MUST be reset to 0 when major version is incremented.
        /// </summary>
        public readonly int Major = Major;

        /// <summary>
        /// Minor version Y (x.Y.z | x > 0) MUST be incremented if new, backwards compatible functionality is introduced to the public API.
        /// It MUST be incremented if any public API functionality is marked as deprecated. It MAY be incremented if substantial new functionality
        /// or improvements are introduced within the private code. It MAY include patch level changes. Patch version MUST be reset to 0 when minor version is incremented.
        /// </summary>
        public readonly int Minor = Minor;

        /// <summary>
        /// Patch version Z (x.y.Z | x > 0) MUST be incremented if only backwards compatible bug fixes are introduced.
        /// A bug fix is defined as an internal change that fixes incorrect behavior.
        /// </summary>
        public readonly int Patch = Patch;

        /// <summary>
        /// A pre-release version MAY be denoted by appending a hyphen and a series of dot separated identifiers immediately following the patch version.
        /// Identifiers MUST comprise only ASCII alphanumerics and hyphens [0-9A-Za-z-]. Identifiers MUST NOT be empty. Numeric identifiers MUST NOT include leading zeroes.
        /// Pre-release versions have a lower precedence than the associated normal version. A pre-release version indicates that the version is unstable and might not satisfy
        /// the intended compatibility requirements as denoted by its associated normal version. Examples: 1.0.0-alpha, 1.0.0-alpha.1, 1.0.0-0.3.7, 1.0.0-x.7.z.92, 1.0.0-x-y-z.–.
        /// </summary>
        public readonly string? PreReleaseLabel = PreReleaseLabel;

        /// <summary>
        /// Build metadata MAY be denoted by appending a plus sign and a series of dot separated identifiers immediately following the patch or pre-release version.
        /// Identifiers MUST comprise only ASCII alphanumerics and hyphens [0-9A-Za-z-]. Identifiers MUST NOT be empty. Build metadata MUST be ignored when determining version precedence.
        /// Thus two versions that differ only in the build metadata, have the same precedence. Examples: 1.0.0-alpha+001, 1.0.0+20130313144700, 1.0.0-beta+exp.sha.5114f85, 1.0.0+21AF26D3—-117B344092BD.
        /// </summary>
        public readonly string? BuildLabel = BuildLabel;

        /// <summary>
        /// Compares two values to compute which is greater.
        /// </summary>
        /// <param name="x">The value to compare with <paramref name="y"/>.</param>
        /// <param name="y">The value to compare with <paramref name="x"/>.</param>
        /// <returns><paramref name="x"/> if it is greater than <paramref name="y"/>; otherwise, <paramref name="y"/>.</returns>
        public static SemanticVersion Max(SemanticVersion x, SemanticVersion y)
        {
            return x.CompareTo(y) == 1
                ? x
                : y;
        }

        /// <summary>
        /// Compares two values to compute which is lesser.
        /// </summary>
        /// <param name="x">The value to compare with <paramref name="y"/>.</param>
        /// <param name="y">The value to compare with <paramref name="x"/>.</param>
        /// <returns><paramref name="x"/> if it is less than <paramref name="y"/>; otherwise, <paramref name="y"/>.</returns>
        public static SemanticVersion Min(SemanticVersion x, SemanticVersion y)
        {
            return x.CompareTo(y) == -1
                ? x
                : y;
        }

        /// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)"/>
        public static SemanticVersion Parse(string s, IFormatProvider? provider)
        {
            var parseResult = ParseResult(s, provider);
            if (parseResult.IsDefined(out SemanticVersion entity))
            {
                return entity;
            }

            throw new ArgumentException("The provided value could not be parsed into an instance of SemanticVersion.", nameof(s));
        }

        /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)"/>
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SemanticVersion result)
        {
            var parseResult = ParseResult(s, provider);
            return parseResult.IsDefined(out result);
        }

        /// <summary>
        /// Tries to parse a string into a value result.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="value"/>.</param>
        /// <returns>A <see cref="Result{TEntity}"/> containing the result or an error that caused the parse to fail.</returns>
        public static Result<SemanticVersion> ParseResult(string? value, IFormatProvider? provider)
        {
            var match = SemVerRegex.Match(value ?? string.Empty);
            if (!match.Success)
            {
                return new ParseError("Input string was in the incorrect format.");
            }

            // Regex specifies this must be an integer.
            int major = int.Parse(match.Groups["major"].Value);
            int minor = int.Parse(match.Groups["minor"].Value);
            int patch = int.Parse(match.Groups["patch"].Value);
            Group preReleaseGroup = match.Groups["prerelease"];
            string? preRelease = preReleaseGroup.Success ? preReleaseGroup.Value : null;
            Group buildGroup = match.Groups["buildmetadata"];
            string? build = buildGroup.Success ? buildGroup.Value : null;

            return new SemanticVersion(major, minor, patch, preRelease, build);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is SemanticVersion || obj is Version)
            {
                return CompareTo((SemanticVersion)obj);
            }
            else
            {
                throw new ArgumentException("Value must be an instance of SemanticVersion or Version", nameof(obj));
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Precedence refers to how versions are compared to each other when ordered.
        /// <list type="number">
        ///     <item>Precedence MUST be calculated by separating the version into major, minor, patch, and pre-release identifiers in that order. Build metadata does not figure into precedence.</item>
        ///     <item>
        ///         Precedence is determined by the first difference when comparing each of these identifiers from left to right as follows: major, minor, and patch versions are always compared numerically.
        ///         <code>1.0.0 &lt; 2.0.0 &lt; 2.1.0 &lt; 2.1.1</code>
        ///     </item>
        ///     <item>
        ///         When major, minor, and patch are equal, a pre-release version has lower precedence than a normal version.
        ///         <code>1.0.0-alpha &lt; 1.0.0</code>
        ///     </item>
        ///     <item>
        ///         Precedence for two pre-release versions with the same major, minor, and patch version MUST be determined by comparing each dot-separated identifier from left to right until a difference is found, as follows:
        ///         <list type="number">
        ///             <item>Identifiers consisting only of digits are compared numerically.</item>
        ///             <item>Identifiers with letters or hyphens are compared lexically in ASCII sort order.</item>
        ///             <item>Numeric identifiers always have lower precedence than non-numeric identifiers.</item>
        ///             <item>A larger set of pre-release fields has a higher precedence than a smaller set if all of the proceeding identifiers are equal.</item>
        ///         </list>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <example>1.0.0-alpha &lt; 1.0.0-alpha.1 &lt; 1.0.0-alpha.beta &lt; 1.0.0-beta &lt; 1.0.0-beta.2 &lt; 1.0.0-beta.11 &lt; 1.0.0-rc.1 &lt; 1.0.0.</example>
        public int CompareTo(SemanticVersion other)
        {
            // First, compare major, minor, and patch values.
            int result =
                Major != other.Major ? Major > other.Major ? 1 : -1 :
                Minor != other.Minor ? Minor > other.Minor ? 1 : -1 :
                Patch != other.Patch ? Patch > other.Patch ? 1 : -1 :
                0;

            // If the major, minor, and patch values do not match, return that value.
            if (result != 0)
            {
                return result;
            }

            // Major, minor, and patch match. Compare pre-release label.
            string? left = PreReleaseLabel?.ToLowerInvariant();
            string? right = other.PreReleaseLabel?.ToLowerInvariant();
            return (left, right) switch
            {
                (null, null) => 0,
                (null, { }) => 1,
                ({ }, null) => -1,
                _ => ComparePreRelease(left, right)
            };
        }

        private static int ComparePreRelease(string left, string right)
        {
            var leftArray = left.Split('.');
            var rightArray = right.Split(".");

            for (int i = 0; ; i++)
            {
                if (i == leftArray.Length || i == rightArray.Length)
                {
                    break;
                }

                // We have remaining elements to compare.
                var leftEntry = leftArray[i];
                var rightEntry = rightArray[i];

                int? leftInt = int.TryParse(leftEntry, out int parseResult) ? parseResult : null;
                int? rightInt = int.TryParse(rightEntry, out parseResult) ? parseResult : null;

                int comparison = (leftInt, rightInt) switch
                {
                    ({ }, { }) => leftInt.Value.CompareTo(rightInt),
                    ({ }, null) => -1, // Numeric identifiers always have lower precedence.
                    (null, { }) => 1,
                    (null, null) => string.Compare(leftEntry, rightEntry, StringComparison.OrdinalIgnoreCase)
                };

                if (comparison != 0)
                {
                    // Short circuit from comparison.
                    return comparison;
                }
            }

            // All elements were equal and we've run out of elements to compare.
            // If one is larger, that one has higher precedence.
            // If they're the same length, then they are equal.
            return leftArray.Length.CompareTo(rightArray.Length);
        }

        /// <inheritdoc/>
        /// <remarks>As per the specifications, <see cref="BuildLabel"/> is ignored in this operation.</remarks>
        public bool Equals(SemanticVersion other)
        {
            return Major == other.Major &&
                Minor == other.Minor &&
                Patch == other.Patch &&
                PreReleaseLabel == other.PreReleaseLabel;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Major, Minor, Patch, PreReleaseLabel);
        }

        /// <inheritdoc/>
        public override string ToString()
            => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a string representation of a Semantic Version in the requested format.
        /// </summary>
        /// <param name="format">
        /// <para>The format to use to represent the SemanticVersion.</para>
        /// <para><c>G</c>: General. <c>Major.Minor.Patch-PreRelease+Build</c>.</para>
        /// <para><c>N</c>: Neutral. <c>Major.Minor.Patch</c>.</para>
        /// </param>
        /// <param name="formatProvider">An optional object that provides culture-specific formatting.</param>
        /// <returns>A string representation of the current SemanticVersion.</returns>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            format ??= "G";
            return format switch
            {
                "G" => ToString(5),
                "N" => ToString(3),
                _ => throw new ArgumentException($"'{format}' is not a valid format value.", nameof(format))
            };
        }

        /// <summary>
        /// Gets a string representation of the SemanticVersion using up to the specified number of fields.
        /// </summary>
        /// <param name="fieldCount">The maximum number of fields, between 1 and 5, inclusive.</param>
        /// <returns>A string representation of the current SemanticVersion.</returns>
        public string ToString([ValueRange(1, 5)] int fieldCount)
        {
            return fieldCount switch
            {
                1 => Major.ToString(),
                2 => $"{Major}.{Minor}",
                3 => $"{Major}.{Minor}.{Patch}",
                4 => ToString(3) + PreReleaseLabel is { }
                    ? $"-{PreReleaseLabel}"
                    : BuildLabel is { }
                        ? $"+{BuildLabel}"
                        : string.Empty,
                5 => ToString(3) + (PreReleaseLabel is { } ? $"-{PreReleaseLabel}" : string.Empty) + (BuildLabel is { } ? $"+{BuildLabel}" : string.Empty),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldCount), "Value must be between 1 and 5 inclusive.")
            };
        }

        // Do not comment on operators.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
        public static bool operator <(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) >= 0;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.

        /// <summary>
        /// Implicitly converts a <see cref="Version"/> to an instance of <see cref="SemanticVersion"/>.
        /// </summary>
        /// <param name="version">The version to convert.</param>
        public static implicit operator SemanticVersion(Version version)
            => new(version.Major, version.Minor, version.Build, PreReleaseLabel: null, BuildLabel: version.Revision.ToString());

        [GeneratedRegex("^(?<major>0|[1-9]\\d*)\\.(?<minor>0|[1-9]\\d*)\\.(?<patch>0|[1-9]\\d*)(?:-(?<prerelease>(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}
