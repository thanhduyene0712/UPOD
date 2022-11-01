using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.SERVICES.Enum;
using UPOD.SERVICES.Services;

namespace UPOD.API.Controllers
{
    [ApiController]
    [Route("api/maintenance_schedules")]
    public partial class MaintenanceSchedulesController : ControllerBase
    {

        private readonly IMaintenanceScheduleService _maintenanceSchedule_sv;
        public MaintenanceSchedulesController(IMaintenanceScheduleService maintenanceSchedule_sv)
        {
            _maintenanceSchedule_sv = maintenanceSchedule_sv;
        }
        
        [HttpPut]
        [Route("notifications")]
        public async Task<ActionResult> Notifications()
        {
            try
            {
                var listSchedule = await _maintenanceSchedule_sv.GetMaintenanceSchedulesNotify();
                var listScheduleMissing = await _maintenanceSchedule_sv.GetMaintenanceSchedulesNotifyMissing();
                foreach (var item in listSchedule)
                {
                    //send notifications
                    await _maintenanceSchedule_sv.SetStatus(ScheduleStatus.NOTIFIED, item.Value);
                }
                foreach (var item in listScheduleMissing)
                {
                    //send notifications
                    await _maintenanceSchedule_sv.SetStatus(ScheduleStatus.MISSED, item.Value);
                }
                var timeShedule = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(12), DateTimeKind.Utc);
                BackgroundJob.Schedule(() => Notifications(), timeShedule);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get_list_maintenance_schedules")]
        public async Task<ActionResult<ResponseModel<MaintenanceScheduleResponse>>> GetListMaintenanceSchedules([FromQuery] PaginationRequest model, [FromQuery] FilterStatusRequest value)
        {
            try
            {
                return await _maintenanceSchedule_sv.GetListMaintenanceSchedules(model, value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_list_maintenance_schedules_by_technician_id")]
        public async Task<ActionResult<ResponseModel<MaintenanceScheduleResponse>>> GetListMaintenanceSchedulesTechnician([FromQuery] PaginationRequest model, Guid id,[FromQuery] FilterStatusRequest value)
        {
            try
            {
                return await _maintenanceSchedule_sv.GetListMaintenanceSchedulesTechnician(model, id, value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("get_list_maintenance_schedules_by_agency_id")]
        public async Task<ActionResult<ResponseModel<MaintenanceScheduleResponse>>> GetListMaintenanceSchedulesAgency([FromQuery] PaginationRequest model, Guid id, [FromQuery] FilterStatusRequest value)
        {
            try
            {
                return await _maintenanceSchedule_sv.GetListMaintenanceSchedulesAgency(model, id, value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("maintaining_maintenance_schedule_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> MaintainingSchedule(Guid id)

        {
            try
            {
                return await _maintenanceSchedule_sv.MaintainingSchedule(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update_maintenance_schedule_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> UpdateMaintenanceSchedule(Guid id, MaintenanceScheduleRequest model)
        {
            try
            {
                return await _maintenanceSchedule_sv.UpdateMaintenanceSchedule(id, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("disable_maintenance_schedule_by_id")]
        public async Task<ActionResult<ObjectModelResponse>> DisableMaintenanceSchedule(Guid id)
        {
            try
            {
                return await _maintenanceSchedule_sv.DisableMaintenanceSchedule(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
