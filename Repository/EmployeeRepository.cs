using Contracts;
using Entities.Models;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public EmployeeRepository(CompEmpDbContext repositoryContext)
        : base(repositoryContext)
        {
        }
    }

}
