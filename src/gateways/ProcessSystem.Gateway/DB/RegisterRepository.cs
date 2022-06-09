using System;
using System.Threading.Tasks;
using Common.DB;
using Microsoft.EntityFrameworkCore;
using ProcessSystem.Contracts;

namespace ProcessSystem.DB
{
    public class RegisterRepository
        : IRegisterRepository
    {
        private readonly ProcessContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public RegisterRepository(ProcessContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Register> AddAsync(Register register)
        {

            return (await _context.Register
                    .AddAsync(register))
                .Entity;
        }
        public async Task<Register> FindByNameAndUrlAsync(Register register)
        {
            return await _context.Register
                .SingleOrDefaultAsync(sr => sr.Name == register.Name && sr.Url == register.Url);
        }


        public async Task<Register> FindByTokenAsync(string token)
        {
            return await _context.Register
                .SingleOrDefaultAsync(sr => sr.Token == token);

        }

        public async Task<Register> DeleteAsync(string token)
        {
            var swr =await FindByTokenAsync(token);

            if (swr is null) throw new ArgumentNullException(nameof(swr), "По токену не найдена запись");

            _context.Register.Remove(swr);

            return swr;
        }
    }
}
