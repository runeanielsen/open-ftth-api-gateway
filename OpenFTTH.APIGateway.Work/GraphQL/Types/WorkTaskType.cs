using GraphQL.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenFTTH.APIGateway.Work.GraphQL.Types
{
    public class WorkTaskType : ObjectGraphType<WorkTask>
    {
        public WorkTaskType(ILogger<ProjectType> logger)
        {
            Field(x => x.MRID, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name/descrription of work task");

            Field<GeometryType>(
               name: "geometry",
               description: "The location of the work task (geojson)",
               resolve: context =>
               {
                   return Geometry.MapFromNTSGeometry(context.Source.Location);
               }
           );
        }
    }

    
}
