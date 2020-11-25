using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Model
{
    public class UserWorkContext
    {
        public string UserName { get; set; }

        public WorkTask CurrentWorkTask { get; set; }
    }
}
