using Entities.Models;
using Repository.Context;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class EmployeeRepository : RepositoryBase<Company>, IEmployeeRepository
    {
        public EmployeeRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
    }

}
