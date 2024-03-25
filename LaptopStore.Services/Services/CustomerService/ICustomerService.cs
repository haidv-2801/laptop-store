using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.CustomerService
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById(string id);

        Task<int> SaveCustomer(CustomerSaveDTO customerSaveDTO);

        Task<PagingResponse> GetCustomerPaging(PagingRequest paging);

        Task<bool> UpdateCustomer(string id, CustomerSaveDTO customerSaveDTO);

        Task<int> DeleteCustomer(string id);

    }
}
