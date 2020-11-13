using OpenFTTH.WorkService.Queries;
using System;
using System.Threading.Tasks;

namespace OpenFTTH.WorkService
{
    public interface IWorkServiceAPI
    {
        IQueryResult Query(IQueryCommand queryCommand);
    }
}
