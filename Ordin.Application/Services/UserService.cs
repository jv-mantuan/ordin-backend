using Ordin.Application.Interfaces;

namespace Ordin.Application.Services
{
    public class UserService : ICurrentUserService
    {
        public Guid UserId => Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}