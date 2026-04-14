using Portfolio.Application.Abstractions;
using Portfolio.Application.DTOs;

namespace Portfolio.Application.Services;

public interface IHomeService
{
    Task<int?> GetUnreadMessagesCountAsync();
    Task<bool> SaveLocationAsync(LocationRequest request);
}

public class HomeService(IHomeRepo repo) : IHomeService
{
    private readonly IHomeRepo _repo = repo;

    public Task<int?> GetUnreadMessagesCountAsync() => _repo.GetUnreadMessagesCountAsync();
    public Task<bool> SaveLocationAsync(LocationRequest request) => _repo.SaveLocationAsync(request);
}
