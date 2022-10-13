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
        public async Task<ActionResult<ResponseModel<CustomerResponse>>> GetAllCustomers([FromQuery] PaginationRequest model)
        {
            try
            {
                return await _customer_sv.GetAll(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_customer_details_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetCustomerDetails(Guid id)
        {
            try
            {
                return await _customer_sv.GetCustomerDetails(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_requests_by_customer_id")]
        public async Task<ActionResult<ResponseModel<RequestListResponse>>> GetListRequestsByCustomerId([FromQuery] PaginationRequest model, [FromQuery] FilterRequest status, Guid id)
        {
            try
            {
                return await _customer_sv.GetListRequestsByCustomerId(model, status, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get_services_by_customer_id")]
        public async Task<ActionResult<ObjectModelResponse>> GetServiceByCustomerId(Guid id)
        {
            try
            {
                return await _customer_sv.GetServiceByCustomerId(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create_customer")]
        public async Task<ActionResult<ObjectModelResponse>> CreateCustomer(CustomerRequest model)
        {
            try
            {
                return await _customer_sv.CreateCustomer(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_customer_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateCustomer(Guid id, CustomerRequest model)
        {
            try
            {
                return await _customer_sv.UpdateCustomer(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("disable_customer_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableCompany(Guid id)
        {
            try
            {
                return await _customer_sv.DisableCustomer(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
