using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Common.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProcessSystem.Contracts;

namespace ProcessSystem.DB
{
    public partial class ProcessContext : DbContext, IUnitOfWork
    {
        public ProcessContext()
        {
        }

        public ProcessContext(DbContextOptions<ProcessContext> options)
            : base(options)
        {
            Database.EnsureCreatedAsync();
        }

        public virtual DbSet<Register> Register { get; set; }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        private IDbContextTransaction _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>(entity =>
            {
                entity.ToTable("register", "process");

                entity.HasIndex(e => new {Channel = e.Name, e.Url })
                    .HasDatabaseName("register_url_name_uidx")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token");

                entity.Property(e => e.ProcessTypes)
                    .IsRequired()
                    .HasColumnName("process_types")
                    .HasColumnType("jsonb");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url");
            });

        }


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}