namespace OpenFTTH.Work.API
{
    public interface IWorkServiceAPI
    {
        IQueryResult Query(IQueryCommand queryCommand);

        IMutationResult Mutate(IMutationCommand mutationCommand);
    }
}
