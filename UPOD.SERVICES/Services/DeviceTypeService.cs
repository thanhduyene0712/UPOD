//using Microsoft.EntityFrameworkCore;
//using UPOD.REPOSITORIES.Models;
//using UPOD.REPOSITORIES.RequestModels;
//using UPOD.REPOSITORIES.ResponeModels;

//namespace UPOD.SERVICES.Services
//{

//    public interface IDeviceTypeService
//    {
//        Task<ResponseModel<DeviceTypeResponse>> GetListDeviceType(PaginationRequest model);
//        Task<ResponseModel<DeviceTypeResponse>> CreateDeviceType(DeviceTypeRequest model);
//        Task<ResponseModel<DeviceTypeResponse>> UpdateDeviceType(Guid id, DeviceTypeRequest model);
//        Task<ResponseModel<DeviceTypeResponse>> DisableDeviceType(Guid id);
//    }

//    public class DeviceTypeService : IDeviceTypeService
//    {
//        private readonly Database_UPODContext _context;
//        public DeviceTypeService(Database_UPODContext context)
//        {
//            _context = context;
//        }

       
//        public async Task<ResponseModel<DeviceTypeResponse>> GetListDeviceType(PaginationRequest model)
//        {
//            var DeviceTypes = await _context.DeviceTypes.Where(a => a.IsDelete == false).Select(a => new DeviceTypeResponse
//            {
//                id = a.Id,
//                service_id = a.ServiceId,
//                device_type_name = a.DeviceTypeName,
//                desciption = a.Desciption,
//                is_delete = a.IsDelete,
//                create_date = a.CreateDate,
//                update_date = a.UpdateDate

//            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
//            return new ResponseModel<DeviceTypeResponse>(DeviceTypes)
//            {
//                Total = DeviceTypes.Count,
//                Type = "DeviceTypes"
//            };
//        }

//        public async Task<ResponseModel<DeviceTypeResponse>> CreateDeviceType(DeviceTypeRequest model)
//        {

//            var device_type = new DeviceType
//            {
//                Id = Guid.NewGuid(),
//                ServiceId = model.service_id,
//                DeviceTypeName = model.device_type_name,
//                Desciption = model.desciption,
//                IsDelete = false,
//                CreateDate = DateTime.Now,
//                UpdateDate = DateTime.Now

//            };
//            var list = new List<DeviceTypeResponse>();
//            var message = "blank";
//            var status = 500;
//            var device_type_id = await _context.DeviceTypes.Where(x => x.Id.Equals(device_type.Id)).FirstOrDefaultAsync();
//            if (device_type_id != null)
//            {
//                status = 400;
//                message = "DeviceTypeId is already exists!";
//            }
//            else
//            {
//                message = "Successfully";
//                status = 201;
//                await _context.DeviceTypes.AddAsync(device_type);
//                await _context.SaveChangesAsync();
//                list.Add(new DeviceTypeResponse
//                {
//                    id = device_type.Id,
//                    service_id = device_type.ServiceId,
//                    device_type_name = device_type.DeviceTypeName,
//                    desciption = device_type.Desciption,
//                    is_delete = device_type.IsDelete,
//                    create_date = device_type.CreateDate,
//                    update_date = device_type.UpdateDate
//                });
//            }

//            return new ResponseModel<DeviceTypeResponse>(list)
//            {
//                Message = message,
//                Status = status,
//                Total = list.Count,
//                Type = "DeviceType"
//            };
//        }


//        public async Task<ResponseModel<DeviceTypeResponse>> DisableDeviceType(Guid id)
//        {
//            var device_type = await _context.DeviceTypes.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
//            device_type.IsDelete = true;
//            device_type.UpdateDate = DateTime.Now;
//            _context.DeviceTypes.Update(device_type);
//            await _context.SaveChangesAsync();
//            var list = new List<DeviceTypeResponse>();
//            list.Add(new DeviceTypeResponse
//            {
//                is_delete = device_type.IsDelete,
//            });
//            return new ResponseModel<DeviceTypeResponse>(list)
//            {
//                Status = 201,
//                Total = list.Count,
//                Type = "DeviceType"
//            };
//        }
//        public async Task<ResponseModel<DeviceTypeResponse>> UpdateDeviceType(Guid id, DeviceTypeRequest model)
//        {
//            var device_type = await _context.DeviceTypes.Where(a => a.Id.Equals(id)).Select(x => new DeviceType
//            {
//                Id = id,
//                ServiceId = model.service_id,
//                DeviceTypeName = model.device_type_name,
//                Desciption = model.desciption,
//                IsDelete = x.IsDelete,
//                CreateDate = x.CreateDate,
//                UpdateDate = DateTime.Now
//            }).FirstOrDefaultAsync();
//            _context.DeviceTypes.Update(device_type);
//            await _context.SaveChangesAsync();
//            var list = new List<DeviceTypeResponse>();
//            list.Add(new DeviceTypeResponse
//            {
//                id = device_type.Id,
//                service_id = device_type.ServiceId,
//                device_type_name = device_type.DeviceTypeName,
//                desciption = device_type.Desciption,
//                is_delete = device_type.IsDelete,
//                create_date = device_type.CreateDate,
//                update_date = device_type.UpdateDate
//            });
//            return new ResponseModel<DeviceTypeResponse>(list)
//            {
//                Status = 201,
//                Total = list.Count,
//                Type = "DeviceType"
//            };
//        }

//    }
//}
