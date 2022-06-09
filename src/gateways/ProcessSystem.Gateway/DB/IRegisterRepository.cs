using System.Threading.Tasks;
using Common.DB;
using ProcessSystem.Contracts;

namespace ProcessSystem.DB
{

    public interface IRegisterRepository : IRepository<Register>
    {
        Task<Register> AddAsync(Register register);
        Task<Register> DeleteAsync(string token);
        Task<Register> FindByTokenAsync(string token);
        Task<Register> FindByNameAndUrlAsync(Register register);
    }
}
