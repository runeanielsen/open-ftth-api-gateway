using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.WorkService.QueryModel
{
    /// <summary>
    /// AKA work order
    /// </summary>
    public class WorkTask
    {
        public Guid MRID { get; }
        public String Name { get; }
        public Geometry? Location { get; }

        public WorkTask(Guid mRID, string name, Geometry? location = null)
        {
            MRID = mRID;
            Name = name;
            Location = location;
        }
    }
}
