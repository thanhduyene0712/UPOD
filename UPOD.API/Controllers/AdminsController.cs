using Microsoft.AspNetCore.Mvc;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.SERVICES.Services;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public partial class AdminsController : ControllerBase
    {

        private readonly IAdminService _admin_sv;

        public AdminsController(IAdminService admin_sv)
        {
            _admin_sv = admin_sv;
        }
        [HttpGet]
        [Route("get_all_admins")]
        public async Task<ActionResult<ResponseModel<AdminResponse>>> GetListAdmins([FromQuery] PaginationRequest model, [FromQuery] SearchRequest value)
        {
            try
            {
                return await _admin_sv.GetListAdmins(model, value);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("get_admin_details")]
        public async Task<ActionResult<ObjectModelResponse>> GetDetailsAdmin(Guid id)

        {
            try
            {
                return await _admin_sv.GetDetailsAdmin(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [Route("create_admin")]
        public async Task<ActionResult<ObjectModelResponse>> CreateAdmin(AdminRequest model)
        {
            try
            {
                return await _admin_sv.CreateAdmin(model);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("update_admin")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateAdmin(Guid id, AdminRequest model)
        {
            try
            {
                return await _admin_sv.UpdateAdmin(id, model);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("disable_admin")]
        public async Task<ActionResult<ObjectModelResponse>> DisableAdmin(Guid id)
        {
            try
            {
                return await _admin_sv.DisableAdmin(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}




