﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.Settings
{
    public class KafkaSetting
    {
        public string Server { get; set; }
        public string CertificateFilename { get; set; }
        public string RouteNetworkEventTopic { get; set; }
        public string GeographicalAreaUpdatedTopic { get; set; }
        public string UtilityNetworkNotificationsTopic { get; set; }
    }
}
