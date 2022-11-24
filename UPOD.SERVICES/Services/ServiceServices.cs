using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{
    public interface IServiceService
    {
        Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model, SearchRequest value);
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
                guideline = a.Guideline,

            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(service!)
            {
                Type = "Service"
            };
        }
        public async Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model, SearchRequest value)
        {
            var total = await _context.Services.Where(a => a.IsDelete == false).ToListAsync();
            var services = new List<ServiceResponse>();
            if (value.search == null)
            {
                total = await _context.Services.Where(a => a.IsDelete == false).ToListAsync();
                services = await _context.Services.Where(a => a.IsDelete == false).Select(a => new ServiceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    service_name = a.ServiceName,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    guideline = a.Guideline,

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Services.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.Description!.Contains(value.search.Trim())
                || a.ServiceName!.Contains(value.search.Trim()))).ToListAsync();
                services = await _context.Services.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.Description!.Contains(value.search.Trim())
                || a.ServiceName!.Contains(value.search.Trim()))).Select(a => new ServiceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    service_name = a.ServiceName,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate,
                    guideline = a.Guideline,


                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
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
            while (true)
            {
                var code_dup = await _context.Services.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "SE-" + num++.ToString();
                }
            }
            var service = new Service
            {
                Id = service_id,
                Code = code,
                ServiceName = model.service_name,
                Description = model.description,
                IsDelete = false,
                Guideline = model.guideline,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7),

            };
            var data = new ServiceResponse();
            var message = "blank";
            var status = 500;
            var service_name = await _context.Services.Where(x => x.ServiceName!.Equals(service.ServiceName) && x.IsDelete == false).FirstOrDefaultAsync();
            if (service_name != null)
            {
                status = 400;
                message = "ServiceName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 200;
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
                    guideline = service.Guideline,

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
            var service = await _context.Services.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            var data = new ServiceResponse();
            var message = "blank";
            var status = 500;
            var service_name = await _context.Services.Where(x => x.ServiceName!.Equals(model.service_name) && x.IsDelete == false).FirstOrDefaultAsync();
            if (service_name != null && service!.ServiceName != model.service_name)
            {
                status = 400;
                message = "ServiceName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 200;
                service!.UpdateDate = DateTime.UtcNow.AddHours(7);
                service!.ServiceName = model.service_name;
                service.Description = model.description;
                service.Guideline = model.guideline;
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
                        guideline = service.Guideline,

                    };
                }
            }
            return new ObjectModelResponse(data)
            {
                Status = status,
                Message = message,
                Type = "Service"
            };
        }

        public async Task<ObjectModelResponse> DisableService(Guid id)
        {
            var service = await _context.Services.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            service!.IsDelete = true;
            service.UpdateDate = DateTime.UtcNow.AddHours(7);
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
                    guideline = service.Guideline,

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
