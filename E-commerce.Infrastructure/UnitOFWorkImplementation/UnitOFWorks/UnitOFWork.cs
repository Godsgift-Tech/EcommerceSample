using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.UnitOFWorkImplementation.UnitOFWorks
{
    public class UnitOFWork : IUnitOFWorks, IDisposable
    {
        private readonly EcomDbContext _db;

        public IProductRepository ProductRepository {  get;  }

        public ICategoryRepository CategoryRepository {  get; }

        public IOrderRepository OrderRepository {  get; }

        public UnitOFWork(EcomDbContext db, IProductRepository productRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository)
        {
            _db = db;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            OrderRepository = orderRepository;
        }
        public async Task<int> Completed() => await _db.SaveChangesAsync();
        public void Dispose() => _db.Dispose();


    }
}
