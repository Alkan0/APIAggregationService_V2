namespace APIAggregationService.Services.Cat
{
    public interface IVoteService
    {
        Task VoteForCatAsync(string imageId, int value, string subId);
    }
}
