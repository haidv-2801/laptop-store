using LaptopStore.Core.Utilities;
using LaptopStore.Data.Context;
using LaptopStore.Data.Models;
using LaptopStore.Data.ModelDTO.Customer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopStore.Core;
using System.Net.Http.Json;
using Newtonsoft.Json;
using LaptopStore.Core.Enums;
using LaptopStore.Services.Services.BaseService;

namespace LaptopStore.Services.Services.CustomerService
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(ApplicationDbContext dbContext):base(dbContext)
        {
        }

        public async Task<List<Customer>> GetAll()
        {
            return await dbSet.OrderByDescending(e => e.FirstName).ToListAsync();
        }

        public async Task<Customer> GetById(string id)
        {
            return await GetEntityByIDAsync(id);
        }
        public async Task<int> SaveCustomer(CustomerSaveDTO customerSaveDTO)
        {
            var customer = Mapper.MapInit<CustomerSaveDTO, Customer>(customerSaveDTO);
            await AddEntityAsync(customer);
            return 1;
        }

        public async Task<bool> UpdateCustomer(string id, CustomerSaveDTO customerSaveDTO)
        {
            var customer = await GetEntityByIDAsync(id);
            if (customer == null)
                return false;
            customer.FirstName = customerSaveDTO.FirstName;
            customer.LastName = customerSaveDTO.LastName;
            customer.Email = customerSaveDTO.Email;
            customer.Phone = customerSaveDTO.Phone;
            customer.Address = customerSaveDTO.Address;
            await UpdateEntityAsync(customer);
            return true;
        }

        public async Task<int> DeleteCustomer(string id)
        {
            var customer = await GetEntityByIDAsync(id);
            if (customer == null)
                return 0;

            return await DeleteEntityAsync(customer);
        }

        public async Task<PagingResponse> GetCustomerPaging(PagingRequest paging)
        {
            var pagingResponse = new PagingResponse();
            pagingResponse.Page = paging.Page;
            pagingResponse.PageSize = paging.PageSize;

            //f => true có thể sửa theo nghiệp vụ ví dụ như f.Status = true;
            var result = await FilterEntitiesPagingAsync(f => true, paging.Search, paging.SearchField, paging.Sort, paging.Page, paging.PageSize);
                pagingResponse.Data = result.Data;
                pagingResponse.Total = result.TotalRecords;

            return pagingResponse;
        }
    }
}
