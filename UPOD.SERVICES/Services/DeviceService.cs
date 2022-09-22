using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;

namespace UPOD.SERVICES.Services
{

    public interface IDeviceService
    {
        Task<ResponseModel<DeviceResponse>> GetListDevice(PaginationRequest model);
        Task<ResponseModel<DeviceResponse>> GetDetailDevice(Guid id);
        Task<ResponseModel<DeviceResponse>> CreateDevice(DeviceRequest model);
        Task<ResponseModel<DeviceResponse>> UpdateDevice(Guid id, DeviceUpdateRequest model);
        Task<ResponseModel<DeviceResponse>> DisableDevice(Guid id);
    }

    public class DeviceService : IDeviceService
    {
        private readonly Database_UPODContext _context;
        public DeviceService(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<DeviceResponse>> GetListDevice(PaginationRequest model)
        {
            var devices = await _context.Devices.Where(a => a.IsDelete == false).Select(a => new DeviceResponse
            {
                id = a.Id,
                conpany_id = a.ConpanyId,
                devicetype_id = a.DeviceTypeId,
                device_name = a.DeviceName,
                device_code = a.DeviceCode,
                guaranty_start_date = a.GuarantyStartDate,
                guaranty_end_date = a.GuarantyEndDate,
                ip = a.Ip,
                port = a.Port,
                device_account = a.DeviceAccount,
                device_password = a.DevicePassword,
                setting_date = a.SettingDate,
                other = a.Other,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate
            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<DeviceResponse>(devices)
            {
                Total = devices.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<DeviceResponse>> GetDetailDevice(Guid id)
        {
            var devices = await _context.Devices.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new DeviceResponse
            {
                id = a.Id,
                conpany_id = a.ConpanyId,
                devicetype_id = a.DeviceTypeId,
                device_name = a.DeviceName,
                device_code = a.DeviceCode,
                guaranty_start_date = a.GuarantyStartDate,
                guaranty_end_date = a.GuarantyEndDate,
                ip = a.Ip,
                port = a.Port,
                device_account = a.DeviceAccount,
                device_password = a.DevicePassword,
                setting_date = a.SettingDate,
                other = a.Other,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).ToListAsync();
            return new ResponseModel<DeviceResponse>(devices)
            {
                Total = devices.Count,
                Type = "Devices"
            };
        }


        public async Task<ResponseModel<DeviceResponse>> CreateDevice(DeviceRequest model)
        {

            var device = new Device
            {
                Id = Guid.NewGuid(),
                ConpanyId = model.conpany_id,
                DeviceTypeId = model.devicetype_id,
                DeviceName = model.device_name,
                DeviceCode = model.device_code,
                GuarantyStartDate = model.guaranty_start_date,
                GuarantyEndDate = model.guaranty_end_date,
                Ip = model.ip,
                Port = model.port,
                DeviceAccount = model.device_account,
                DevicePassword = model.device_password,
                SettingDate = model.setting_date,
                Other = model.other,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var list = new List<DeviceResponse>();
            var message = "blank";
            var status = 500;
            var device_id = await _context.Devices.Where(x => x.Id.Equals(device.Id)).FirstOrDefaultAsync();
            if (device_id != null)
            {
                status = 400;
                message = "DeviceId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Devices.AddAsync(device);
                await _context.SaveChangesAsync();
                list.Add(new DeviceResponse
                {
                    id = device.Id,
                    conpany_id = device.ConpanyId,
                    devicetype_id = device.DeviceTypeId,
                    device_name = device.DeviceName,
                    device_code = device.DeviceCode,
                    guaranty_start_date = device.GuarantyStartDate,
                    guaranty_end_date = device.GuarantyEndDate,
                    ip = device.Ip,
                    port = device.Port,
                    device_account = device.DeviceAccount,
                    device_password = device.DevicePassword,
                    setting_date = device.SettingDate,
                    other = device.Other,
                    is_delete = device.IsDelete,
                    create_date = device.CreateDate,
                    update_date = device.UpdateDate
                });
            }

            return new ResponseModel<DeviceResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Device"
            };
        }


        public async Task<ResponseModel<DeviceResponse>> DisableDevice(Guid id)
        {
            var device = await _context.Devices.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            device.IsDelete = true;
            device.UpdateDate = DateTime.Now;
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
            var list = new List<DeviceResponse>();
            list.Add(new DeviceResponse
            {
                is_delete = device.IsDelete,
            });
            return new ResponseModel<DeviceResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Device"
            };
        }
        public async Task<ResponseModel<DeviceResponse>> UpdateDevice(Guid id, DeviceUpdateRequest model)
        {
            var device = await _context.Devices.Where(a => a.Id.Equals(id)).Select(x => new Device
            {
                Id = id,
                ConpanyId = x.ConpanyId,
                DeviceTypeId = model.devicetype_id,
                DeviceName = model.device_name,
                DeviceCode = model.device_code,
                GuarantyStartDate = model.guaranty_start_date,
                GuarantyEndDate = model.guaranty_end_date,
                Ip = model.ip,
                Port = model.port,
                DeviceAccount = model.device_account,
                DevicePassword = model.device_password,
                SettingDate = model.setting_date,
                Other = model.other,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now

            }).FirstOrDefaultAsync();
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
            var list = new List<DeviceResponse>();
            list.Add(new DeviceResponse
            {
                id = device.Id,
                conpany_id = device.ConpanyId,
                devicetype_id = device.DeviceTypeId,
                device_name = device.DeviceName,
                device_code = device.DeviceCode,
                guaranty_start_date = device.GuarantyStartDate,
                guaranty_end_date = device.GuarantyEndDate,
                ip = device.Ip,
                port = device.Port,
                device_account = device.DeviceAccount,
                device_password = device.DevicePassword,
                setting_date = device.SettingDate,
                other = device.Other,
                is_delete = device.IsDelete,
                create_date = device.CreateDate,
                update_date = device.UpdateDate
            });
            return new ResponseModel<DeviceResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Device"
            };
        }

    }
}
