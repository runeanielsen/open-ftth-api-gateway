using System;
using System.Threading.Tasks;

namespace OpenFTTH.WorkService
{
    public interface IWorkServiceAPI
    {
        IQueryResult Query(IQueryCommand queryCommand);

        IMutationResult Mutate(IMutationCommand mutationCommand);
    }
}
