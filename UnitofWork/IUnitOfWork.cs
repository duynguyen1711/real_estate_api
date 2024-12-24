using real_estate_api.Interface.Repository;

namespace real_estate_api.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        IPostDetailRepository PostDetailRepository { get; }
        ISavedPostRepository SavedPostRepository { get; }
        Task SaveChangesAsync();
    }
}
