using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net.Sockets;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IDeviceService
    {
        Task<ResponseModel<DeviceResponse>> GetListDevices(PaginationRequest model, SearchRequest value);
        Task<ResponseModel<DeviceResponse>> GetListDevicesByDeviceType(PaginationRequest model, SearchRequest value, Guid id);
        Task<ObjectModelResponse> GetDetailsDevice(Guid id);
        Task<ObjectModelResponse> CreateDevice(DeviceRequest model);
        Task<ObjectModelResponse> UpdateDevice(Guid id, DeviceUpdateRequest model);
        Task<ObjectModelResponse> DisableDevice(Guid id);
        Task<ResponseModel<DeviceResponse>> GetListDevicesAgency(PaginationRequest model, Guid id, SearchRequest value);
    }

    public class DeviceServices : IDeviceService
    {
        private readonly Database_UPODContext _context;
        public DeviceServices(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<DeviceResponse>> GetListDevices(PaginationRequest model, SearchRequest value)
        {
            var total = await _context.Devices.Where(a => a.IsDelete == false).ToListAsync();
            var devices = new List<DeviceResponse>();
            if (value.search == null)
            {
                total = await _context.Devices.Where(a => a.IsDelete == false).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false).Include(a => a.Agency).Include(a => a.DeviceType).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Devices.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search))).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search))).Include(a => a.DeviceType).Include(a => a.Agency).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<DeviceResponse>(devices)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<DeviceResponse>> GetListDevicesByDeviceType(PaginationRequest model, SearchRequest value, Guid id)
        {
            var total = await _context.Devices.Where(a => a.IsDelete == false).ToListAsync();
            var devices = new List<DeviceResponse>();
            if (value.search == null)
            {
                total = await _context.Devices.Where(a => a.IsDelete == false && a.DeviceTypeId.Equals(id)).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false && a.DeviceTypeId.Equals(id)).Include(a => a.Agency).Include(a => a.DeviceType).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Devices.Where(a => a.IsDelete == false && a.DeviceTypeId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search))).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false && a.DeviceTypeId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search))).Include(a => a.Agency).Include(a => a.DeviceType).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<DeviceResponse>(devices)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ResponseModel<DeviceResponse>> GetListDevicesAgency(PaginationRequest model, Guid id, SearchRequest value)
        {
            var total = await _context.Devices.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
            var devices = new List<DeviceResponse>();
            if (value.search == null)
            {
                total = await _context.Devices.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Devices.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search)
                || a.Ip!.Contains(value.search)
                || a.DeviceAccount!.Contains(value.search)
                || a.Other!.Contains(value.search))).ToListAsync();
                devices = await _context.Devices.Where(a => a.IsDelete == false && a.AgencyId.Equals(id)
                && (a.Code!.Contains(value.search)
                || a.DeviceName!.Contains(value.search)
                || a.Ip!.Contains(value.search)
                || a.DeviceAccount!.Contains(value.search)
                || a.Other!.Contains(value.search))).Select(a => new DeviceResponse
                {
                    id = a.Id,
                    code = a.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                        phone = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Telephone).FirstOrDefault(),
                    },
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    },
                    service = new ServiceNotInContractViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = a.DeviceName,
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
                    update_date = a.UpdateDate,
                    img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<DeviceResponse>(devices)
            {
                Total = total.Count,
                Type = "Devices"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsDevice(Guid id)
        {
            var device = await _context.Devices.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new DeviceResponse
            {
                id = a.Id,
                code = a.Code,
                agency = new AgencyViewResponse
                {
                    id = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                    agency_name = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                    address = _context.Agencies.Where(x => x.Id.Equals(a.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                },
                customer = new CustomerViewResponse
                {
                    id = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                    name = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                    description = _context.Customers.Where(x => x.Id.Equals(a.Agency!.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                },
                service = new ServiceNotInContractViewResponse
                {
                    id = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.Code).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(a.DeviceType!.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
                },
                devicetype = new DeviceTypeViewResponse
                {
                    id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                    service_id = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                    device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                    code = _context.DeviceTypes.Where(x => x.Id.Equals(a.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                },
                technician = new TechnicianViewResponse
                {
                    id = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                    name = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                    code = _context.Technicians.Where(x => x.Id.Equals(a.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                },
                device_name = a.DeviceName,
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
                update_date = a.UpdateDate,
                img = _context.Images.Where(d => d.ObjectId.Equals(a.Id)).Select(x => new ImageResponse
                {
                    id = x.Id,
                    link = x.Link,
                    object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                }).ToList(),
            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(device!)
            {
                Type = "Devices"
            };
        }


        public async Task<ObjectModelResponse> CreateDevice(DeviceRequest model)
        {
            var device_id = Guid.NewGuid();
            while (true)
            {
                var device_dup = await _context.Devices.Where(x => x.Id.Equals(device_id)).FirstOrDefaultAsync();
                if (device_dup == null)
                {
                    break;
                }
                else
                {
                    device_id = Guid.NewGuid();
                }
            }
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("DE", code_number + 1);
            var device = new Device
            {
                Id = device_id,
                Code = code,
                AgencyId = model.agency_id,
                DeviceTypeId = model.devicetype_id,
                DeviceName = model.device_name,
                GuarantyStartDate = model.guaranty_start_date,
                GuarantyEndDate = model.guaranty_end_date,
                Ip = model.ip,
                Port = model.port,
                DeviceAccount = model.device_account,
                DevicePassword = model.device_password,
                SettingDate = model.setting_date,
                Other = model.other,
                IsDelete = false,
                CreateBy = model.create_by,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7)
            };
            foreach (var item in model.img!)
            {
                var img_id = Guid.NewGuid();
                while (true)
                {
                    var img_dup = await _context.Images.Where(x => x.Id.Equals(img_id)).FirstOrDefaultAsync();
                    if (img_dup == null)
                    {
                        break;
                    }
                    else
                    {
                        img_id = Guid.NewGuid();
                    }
                }
                var imgTicket = new Image
                {
                    Id = img_id,
                    Link = item,
                    ObjectId = device.Id,
                };
                await _context.Images.AddAsync(imgTicket);
            }
            var data = new DeviceResponse();

            await _context.Devices.AddAsync(device);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new DeviceResponse
                {
                    id = device.Id,
                    code = device.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = device.DeviceName,
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
                    update_date = device.UpdateDate,
                    img = _context.Images.Where(a => a.ObjectId.Equals(device.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                };
            }



            return new ObjectModelResponse(data)
            {

                Type = "Device"
            };
        }


        public async Task<ObjectModelResponse> DisableDevice(Guid id)
        {
            var device = await _context.Devices.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            device!.IsDelete = true;
            device.UpdateDate = DateTime.UtcNow.AddHours(7);
            _context.Devices.Update(device);
            var data = new DeviceResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new DeviceResponse
                {
                    id = device.Id,
                    code = device.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = device.DeviceName,
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
                    update_date = device.UpdateDate,
                    img = _context.Images.Where(a => a.ObjectId.Equals(device.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                };
            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Device"
            };

        }
        public async Task<ObjectModelResponse> UpdateDevice(Guid id, DeviceUpdateRequest model)
        {
            var device = await _context.Devices.Where(a => a.Id.Equals(id)).Select(x => new Device
            {
                Id = id,
                Code = x.Code,
                AgencyId = x.AgencyId,
                DeviceTypeId = model.devicetype_id,
                DeviceName = model.device_name,
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
                UpdateDate = DateTime.UtcNow.AddHours(7)

            }).FirstOrDefaultAsync();
            _context.Devices.Update(device!);
            var imgs = await _context.Images.Where(a => a.ObjectId.Equals(device!.Id)).ToListAsync();
            foreach (var item in imgs)
            {
                _context.Images.Remove(item);
            }
            foreach (var item in model.img!)
            {
                var img_id = Guid.NewGuid();
                while (true)
                {
                    var img_dup = await _context.Images.Where(x => x.Id.Equals(img_id)).FirstOrDefaultAsync();
                    if (img_dup == null)
                    {
                        break;
                    }
                    else
                    {
                        img_id = Guid.NewGuid();
                    }
                }
                var imgTicket = new Image
                {
                    Id = img_id,
                    Link = item,
                    ObjectId = device!.Id,
                };
                await _context.Images.AddAsync(imgTicket);

            }
            var data = new DeviceResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new DeviceResponse
                {
                    id = device!.Id,
                    code = device.Code,
                    agency = new AgencyViewResponse
                    {
                        id = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Code).FirstOrDefault(),
                        agency_name = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.AgencyName).FirstOrDefault(),
                        address = _context.Agencies.Where(x => x.Id.Equals(device.AgencyId)).Select(x => x.Address).FirstOrDefault(),
                    },
                    devicetype = new DeviceTypeViewResponse
                    {
                        id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Id).FirstOrDefault(),
                        service_id = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.ServiceId).FirstOrDefault(),
                        device_type_name = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.DeviceTypeName).FirstOrDefault(),
                        code = _context.DeviceTypes.Where(x => x.Id.Equals(device.DeviceTypeId)).Select(x => x.Code).FirstOrDefault(),
                    },
                    technician = new TechnicianViewResponse
                    {
                        id = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.Id).FirstOrDefault(),
                        name = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.TechnicianName).FirstOrDefault(),
                        code = _context.Technicians.Where(x => x.Id.Equals(device.CreateBy)).Select(x => x.Code).FirstOrDefault(),
                    },
                    device_name = device.DeviceName,
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
                    update_date = device.UpdateDate,
                    img = _context.Images.Where(a => a.ObjectId.Equals(device.Id)).Select(x => new ImageResponse
                    {
                        id = x.Id,
                        link = x.Link,
                        object_name = _context.Devices.Where(a => a.Id.Equals(x.ObjectId)).Select(a => a.DeviceName).FirstOrDefault(),
                    }).ToList(),
                };
            };
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Device"
            };
        }
        private async Task<int> GetLastCode()
        {
            var device = await _context.Devices.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(device!.Code!);
        }
    }
}
