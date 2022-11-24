//
//  SemanticVersionTests.cs
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

using Selkhound.Common.DataTypes;
using Xunit.Abstractions;

#pragma warning disable SA1600, CS1591

namespace Selkhound.Common.Tests
{
    public class SemanticVersionTests
    {
        private readonly ITestOutputHelper _output;

        public SemanticVersionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private class SemanticVersionSingle : TheoryData<SemanticVersion>
        {
            public SemanticVersionSingle()
            {
                Add(new SemanticVersion(1, 0, 0, "alpha"));
                Add(new SemanticVersion(1, 0, 0, "alpha.1"));
                Add(new SemanticVersion(1, 0, 0, "alpha.beta"));
                Add(new SemanticVersion(1, 0, 0, "beta"));
                Add(new SemanticVersion(1, 0, 0, "beta.2"));
                Add(new SemanticVersion(1, 0, 0, "beta.11"));
                Add(new SemanticVersion(1, 0, 0, "rc.1"));
                Add(new SemanticVersion(1, 0, 0));
            }
        }

        private class SemanticVersionPairs : TheoryData<SemanticVersion, SemanticVersion>
        {
            public SemanticVersionPairs()
            {
                Add(new SemanticVersion(1, 0, 0, "alpha"), new SemanticVersion(1, 0, 0, "alpha.1"));
                Add(new SemanticVersion(1, 0, 0, "alpha.1"), new SemanticVersion(1, 0, 0, "alpha.beta"));
                Add(new SemanticVersion(1, 0, 0, "alpha.beta"), new SemanticVersion(1, 0, 0, "beta"));
                Add(new SemanticVersion(1, 0, 0, "beta"), new SemanticVersion(1, 0, 0, "beta.2"));
                Add(new SemanticVersion(1, 0, 0, "beta.2"), new SemanticVersion(1, 0, 0, "beta.11"));
                Add(new SemanticVersion(1, 0, 0, "beta.11"), new SemanticVersion(1, 0, 0, "rc.1"));
                Add(new SemanticVersion(1, 0, 0, "rc.1"), new SemanticVersion(1, 0, 0));
            }
        }

        private readonly SemanticVersion _One = new SemanticVersion(1, 0, 0);
        private readonly SemanticVersion _Zero = new SemanticVersion(0, 0, 0);
        private readonly SemanticVersion _OneTwoThree = new SemanticVersion(1, 2, 3);

        [Fact]
        public void OneGreaterThanZero()
        {
            Assert.Equal(SemanticVersion.Max(_Zero, _One), _One);
        }

        [Fact]
        public void ZeroLessThanOne()
        {
            Assert.Equal(SemanticVersion.Min(_Zero, _One), _Zero);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1-beta")]
        public void ParseFailThrows(string input)
        {
            Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(input, null));
        }

        [Theory]
        [InlineData("abc")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1-beta")]
        public void ParseFailFalse(string input)
        {
            Assert.False(SemanticVersion.TryParse(input, null, out _));
        }

        [Theory]
        [InlineData("1.0.0-alpha")]
        [InlineData("1.0.0-alpha.1")]
        [InlineData("1.0.0-alpha.beta")]
        [InlineData("1.0.0-beta")]
        [InlineData("1.0.0-beta.2")]
        [InlineData("1.0.0-beta.11")]
        [InlineData("1.0.0-rc.1")]
        [InlineData("1.0.0")]
        public void ParseSuccess(string input)
        {
            Assert.True(SemanticVersion.ParseResult(input, null).IsDefined());
        }

        [Fact]
        public void ConversionTests()
        {
            var version = new Version(1, 2, 3, 4);
            var converted = (SemanticVersion)version;
            var semVer = new SemanticVersion(1, 2, 3, null, "4");

            Assert.Equal(semVer, converted);
        }

        [Theory]
        [ClassData(typeof(SemanticVersionPairs))]
        public void LessThanComparisonTests(SemanticVersion left, SemanticVersion right)
        {
            _output.WriteLine($"Assertion: '{left}' < '{right}'");
            Assert.True(left < right);
        }

        [Theory]
        [ClassData(typeof(SemanticVersionPairs))]
        public void GreaterThanComparisonTests(SemanticVersion left, SemanticVersion right)
        {
            _output.WriteLine($"Assertion: '{right}' > '{left}'");
            Assert.True(right > left);
        }

        [Fact]
        public void BuildMetadataNotImportant()
        {
            var semver1 = new SemanticVersion(1, 0, 0);
            var semver2 = new SemanticVersion(1, 0, 0, null, "100");

            Assert.Equal(semver1, semver2);
            Assert.Equal(0, semver1.CompareTo(semver2));
        }
    }
}
