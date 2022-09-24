using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Services;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public partial class CustomersController : ControllerBase
    {

        private readonly ICustomerService _customer_sv;
        public CustomersController(ICustomerService customer_sv)
        {
            _customer_sv = customer_sv;
        }

        [HttpGet]
        [Route("get_all_customers")]
        public async Task<ResponseModel<CustomerResponse>> GetAllCustomers([FromQuery] PaginationRequest model)
        {
            return await _customer_sv.GetAll(model);
        }
        [HttpGet]
        [Route("get_customer_details_by_id")]
        public async Task<ObjectModelResponse> GetCustomerDetails(Guid id)
        {
            return await _customer_sv.GetCustomerDetails(id);
        }

        [HttpPost]
        [Route("create_customer")]
        public async Task<ObjectModelResponse> CreateCustomer(CustomerRequest model)
        {
            return await _customer_sv.CreateCustomer(model);
        }
        [HttpPut]
        [Route("update_customer_by_id")]
        public async Task<ObjectModelResponse> UpdateCustomer(Guid id, CustomerRequest model)
        {
            return await _customer_sv.UpdateCustomer(id, model);
        }

        [HttpPut]
        [Route("disable_customer_by_id")]
        public async Task<ObjectModelResponse> DisableCompany(Guid id)
        {
            return await _customer_sv.DisableCustomer(id);
        }


    }
}
