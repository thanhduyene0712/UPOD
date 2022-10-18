using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IServiceService
    {
        Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model);
        Task<ObjectModelResponse> GetServiceDetails(Guid id);
        Task<ObjectModelResponse> UpdateService(Guid id, ServiceRequest model);
        Task<ObjectModelResponse> CreateService(ServiceRequest model);
        Task<ObjectModelResponse> DisableService(Guid id);


    }

    public class ServiceServices : IServiceService
    {
        private readonly Database_UPODContext _context;
        public ServiceServices(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ObjectModelResponse> GetServiceDetails(Guid id)
        {
            var service = await _context.Services.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new ServiceResponse
            {
                id = a.Id,
                code = a.Code,
                service_name = a.ServiceName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,

            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(service!)
            {
                Type = "Service"
            };
        }
        public async Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model)
        {
            var total = await _context.Services.Where(a => a.IsDelete == false).ToListAsync();
            var services = await _context.Services.Where(a => a.IsDelete == false).Select(a => new ServiceResponse
            {
                id = a.Id,
                code = a.Code,
                service_name = a.ServiceName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ServiceResponse>(services)
            {
                Total = total.Count,
                Type = "Services"
            };
        }
        private async Task<int> GetLastCode()
        {
            var service = await _context.Services.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(service!.Code!);
        }
        public async Task<ObjectModelResponse> CreateService(ServiceRequest model)
        {
            var service_id = Guid.NewGuid();
            while (true)
            {
                var service_dup = await _context.Services.Where(x => x.Id.Equals(service_id)).FirstOrDefaultAsync();
                if (service_dup == null)
                {
                    break;
                }
                else
                {
                    service_id = Guid.NewGuid();
                }
            }
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("SE", num + 1);
            var service = new Service
            {
                Id = service_id,
                Code = code,
                ServiceName = model.service_name,
                Description = model.description,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,

            };
            var data = new ServiceResponse();
            var message = "blank";
            var status = 500;
            var service_name = await _context.Services.Where(x => x.ServiceName!.Equals(service.ServiceName)).FirstOrDefaultAsync();
            if (service_name != null)
            {
                status = 400;
                message = "ServiceName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                data = new ServiceResponse
                {
                    id = service.Id,
                    code = service.Code,
                    service_name = service.ServiceName,
                    description = service.Description,
                    is_delete = service.IsDelete,
                    create_date = service.CreateDate,
                    update_date = service.UpdateDate,
                };
            }
            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Service"
            };
        }
        public async Task<ObjectModelResponse> UpdateService(Guid id, ServiceRequest model)
        {
            var service = await _context.Services.Where(x => x.Id.Equals(id)).Select(x => new Service
            {
                Id = id,
                Code = x.Code,
                ServiceName = model.service_name,
                Description = model.description,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now,
            }).FirstOrDefaultAsync();
            var data = new ServiceResponse();
            _context.Services.Update(service!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new ServiceResponse
                {
                    id = service!.Id,
                    code = service.Code,
                    service_name = service.ServiceName,
                    description = service.Description,
                    is_delete = service.IsDelete,
                    create_date = service.CreateDate,
                    update_date = service.UpdateDate,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Service"
            };
        }

        public async Task<ObjectModelResponse> DisableService(Guid id)
        {
            var service = await _context.Services.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            service!.IsDelete = true;
            service.UpdateDate = DateTime.Now;
            var data = new ServiceResponse();
            _context.Services.Update(service);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new ServiceResponse
                {
                    id = service.Id,
                    code = service.Code,
                    service_name = service.ServiceName,
                    description = service.Description,
                    is_delete = service.IsDelete,
                    create_date = service.CreateDate,
                    update_date = service.UpdateDate,
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Service"
            };
        }

    }
}
