using FluentAssertions;
using OpenFTTH.APIGateway.Util;
using System;
using Xunit;

namespace OpenFTTH.APIGateway.IntegrationTests.CoordinateConversion
{
    public class TestUTM32toWGS84Conversion
    {
        [Fact]
        public void TestWgs84ConversionAlgorithm()
        {
            var coordinateConverter = new UTM32WGS84Converter();

            var result = coordinateConverter.ConvertFromUTM32NToWGS84(552000, 6190000);

            Math.Round(result[0], 2).Should().Be(9.83);
            Math.Round(result[1], 2).Should().Be(55.85);
        }


        [Fact]
        public void TestLineStringConverter()
        {
            string[] lines = new string[] { "[[552000,6190000], [552001,6190001]]", "[[552000,6190000], [552001,6190001]]" };

            var coordinateConverter = new UTM32WGS84Converter();

            var result = coordinateConverter.ConvertGeoJsonLineStringsToWgs84(lines);

            result.WGS84GeoJsonStrings.Length.Should().Be(2);
            result.WGS84GeoJsonStrings[0].Should().Contain("9.83");
            result.WGS84GeoJsonStrings[0].Should().NotContain("{");

            result.ETRS89BoundingBox.MinX.Should().Be(552000);
            result.ETRS89BoundingBox.MinY.Should().Be(6190000);
            result.ETRS89BoundingBox.MaxX.Should().Be(552001);
            result.ETRS89BoundingBox.MaxY.Should().Be(6190001);

            Math.Round(result.WGS84BoundingBox.MinX,5).Should().Be(9.83062);
            Math.Round(result.WGS84BoundingBox.MinY,5).Should().Be(55.85272);
            Math.Round(result.WGS84BoundingBox.MaxX,5).Should().Be(9.83063);
            Math.Round(result.WGS84BoundingBox.MaxY,5).Should().Be(55.85273);
        }
    }
}
