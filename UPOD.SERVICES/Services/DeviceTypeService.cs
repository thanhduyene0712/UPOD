using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IDeviceTypeService
    {
        Task<ResponseModel<DeviceTypeResponse>> GetListDeviceTypes(PaginationRequest model);
        Task<ObjectModelResponse> CreateDeviceType(DeviceTypeRequest model);
        Task<ObjectModelResponse> UpdateDeviceType(Guid id, DeviceTypeRequest model);
        Task<ObjectModelResponse> DisableDeviceType(Guid id);
    }

    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly Database_UPODContext _context;
        public DeviceTypeService(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<DeviceTypeResponse>> GetListDeviceTypes(PaginationRequest model)
        {
            var DeviceTypes = await _context.DeviceTypes.Where(a => a.IsDelete == false).Select(a => new DeviceTypeResponse
            {
                id = a.Id,
                code = a.Code,
                service = new ServiceViewResponse
                {
                    id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                    code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                    description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),

                },
                device_type_name = a.DeviceTypeName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DeviceTypeResponse>(DeviceTypes)
            {
                Total = DeviceTypes.Count,
                Type = "DeviceTypes"
            };
        }
        private async Task<int> GetLastCode()
        {
            var device_type = await _context.DeviceTypes.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(device_type!.Code!);
        }
        public async Task<ObjectModelResponse> CreateDeviceType(DeviceTypeRequest model)
        {
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("DT", code_number + 1);
            var device_type = new DeviceType
            {
                Id = Guid.NewGuid(),
                Code = code,
                ServiceId = model.service_id,
                DeviceTypeName = model.device_type_name,
                Description = model.description,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var data = new DeviceTypeResponse();
            var message = "blank";
            var status = 500;
            var device_type_id = await _context.DeviceTypes.Where(x => x.Id.Equals(device_type.Id)).FirstOrDefaultAsync();
            if (device_type_id != null)
            {
                status = 400;
                message = "DeviceTypeId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.DeviceTypes.AddAsync(device_type);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new DeviceTypeResponse
                    {
                        id = device_type.Id,
                        code = device_type.Code,
                        service = new ServiceViewResponse
                        {
                            id = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                            service_name = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                            description = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Description).FirstOrDefault(),

                        },
                        device_type_name = device_type.DeviceTypeName,
                        description = device_type.Description,
                        is_delete = device_type.IsDelete,
                        create_date = device_type.CreateDate,
                        update_date = device_type.UpdateDate
                    };
                }

            }

            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "DeviceType"
            };
        }


        public async Task<ObjectModelResponse> DisableDeviceType(Guid id)
        {
            var device_type = await _context.DeviceTypes.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            device_type!.IsDelete = true;
            device_type.UpdateDate = DateTime.Now;
            _context.DeviceTypes.Update(device_type);
            var data = new DeviceTypeResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new DeviceTypeResponse
                {
                    id = device_type.Id,
                    code = device_type.Code,
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Description).FirstOrDefault(),

                    },
                    device_type_name = device_type.DeviceTypeName,
                    description = device_type.Description,
                    is_delete = device_type.IsDelete,
                    create_date = device_type.CreateDate,
                    update_date = device_type.UpdateDate
                };
            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "DeviceType"
            };
        }
        public async Task<ObjectModelResponse> UpdateDeviceType(Guid id, DeviceTypeRequest model)
        {
            var device_type = await _context.DeviceTypes.Where(a => a.Id.Equals(id)).Select(x => new DeviceType
            {
                Id = id,
                Code = x.Code,
                ServiceId = model.service_id,
                DeviceTypeName = model.device_type_name,
                Description = model.description,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
            }).FirstOrDefaultAsync();
            var data = new DeviceTypeResponse();
            _context.DeviceTypes.Update(device_type!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)

            {
                data = new DeviceTypeResponse
                {
                    id = device_type!.Id,
                    code = device_type.Code,
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(device_type.ServiceId)).Select(a => a.Description).FirstOrDefault(),

                    },
                    device_type_name = device_type.DeviceTypeName,
                    description = device_type.Description,
                    is_delete = device_type.IsDelete,
                    create_date = device_type.CreateDate,
                    update_date = device_type.UpdateDate
                };

            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "DeviceType"
            };

        }
    }
}
