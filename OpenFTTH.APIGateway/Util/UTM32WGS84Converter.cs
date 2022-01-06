using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections.Generic;

namespace OpenFTTH.APIGateway.Util
{
    public static class UTM32WGS84Converter
    {
        static CoordinateSystem _fromCS;
        static CoordinateSystem _toCS;
        static CoordinateTransformationFactory _ctfac;
        static ICoordinateTransformation _trans;

        public static double[] ConvertFromUTM32NToWGS84(double x, double y)
        {
            Initialize();

            // Transform point to WGS84 latitude longitude 
            double[] fromPoint = new double[] { x, y };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);

            return toPoint;
        }

        public static string[] ConvertGeoJsonLineStringsToWgs84(string[] geoJsonLineStrings)
        {
            if (geoJsonLineStrings == null)
                return null;

            var geoJsonReader = new GeoJsonReader();
            var geoJsonWriter = new GeoJsonWriter();

            List<string> result = new();

            foreach (var inputGeoJson in geoJsonLineStrings)
            {
                var line = geoJsonReader.Read<LineString>("{ \"type\": \"LineString\",\"coordinates\":" + inputGeoJson + " }");

                foreach (var coord in line.Coordinates)
                {
                    var conversionResult = UTM32WGS84Converter.ConvertFromUTM32NToWGS84(coord.X, coord.Y);
                    coord.X = conversionResult[0];
                    coord.Y = conversionResult[1];
                }

                var newGeoJson = geoJsonWriter.Write(line);

                newGeoJson = newGeoJson.Replace("{\"type\":\"LineString\",\"coordinates\":", "");
                newGeoJson = newGeoJson.Replace("}", "");

                result.Add(newGeoJson);
            }

            return result.ToArray();
        }

        private static void Initialize()
        {
            if (_fromCS == null)
            {
                string utmWkt = @"PROJCS[""ETRS89 / UTM zone 32N"",
    GEOGCS[""ETRS89"",
        DATUM[""European_Terrestrial_Reference_System_1989"",
            SPHEROID[""GRS 1980"",6378137,298.257222101,
                AUTHORITY[""EPSG"",""7019""]],
            AUTHORITY[""EPSG"",""6258""]],
        PRIMEM[""Greenwich"",0,
            AUTHORITY[""EPSG"",""8901""]],
        UNIT[""degree"",0.01745329251994328,
            AUTHORITY[""EPSG"",""9122""]],
        AUTHORITY[""EPSG"",""4258""]],
    UNIT[""metre"",1,
        AUTHORITY[""EPSG"",""9001""]],
    PROJECTION[""Transverse_Mercator""],
    PARAMETER[""latitude_of_origin"",0],
    PARAMETER[""central_meridian"",9],
    PARAMETER[""scale_factor"",0.9996],
    PARAMETER[""false_easting"",500000],
    PARAMETER[""false_northing"",0],
    AUTHORITY[""EPSG"",""25832""],
    AXIS[""Easting"",EAST],
    AXIS[""Northing"",NORTH]]";


                // WGS 84
                string wgsWkt = @"
                GEOGCS[""GCS_WGS_1984"",
                    DATUM[""D_WGS_1984"",SPHEROID[""WGS_1984"",6378137,298.257223563]],
                    PRIMEM[""Greenwich"",0],
                    UNIT[""Degree"",0.0174532925199433]
                ]";

                var cf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
                _fromCS = cf.CreateFromWkt(utmWkt);
                _toCS = cf.CreateFromWkt(wgsWkt);

                _ctfac = new CoordinateTransformationFactory();
                _trans = _ctfac.CreateFromCoordinateSystems(_fromCS, _toCS);
            }
        }

    }

}
