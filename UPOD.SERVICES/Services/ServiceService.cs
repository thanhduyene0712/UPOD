using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Reso.Core.BaseConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.Repositories;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.Services;

namespace UPOD.SERVICES.Services
{
    public interface IServiceService
    {
        Task<ResponseModel<ServiceRespone>> GetAll(PaginationRequest model);
        //Task<ResponseModel<ServiceRespone>> SearchAgencies(PaginationRequest model, String value);
        //Task<ResponseModel<ServiceRespone>> UpdateService(Guid id, ServiceRequest model);
        Task<ResponseModel<ServiceRespone>> CreateService(ServiceRequest model);
        //Task<ResponseModel<ServiceRespone>> DisableService(Guid id, ServiceDisableRequest model);


    }

    public class ServiceService : IServiceService
    {
        private readonly Database_UPODContext _context;
        public ServiceService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ServiceRespone>> GetAll(PaginationRequest model)
        {
            var services = await _context.Services.Select(a => new ServiceRespone
            {
                Id = a.Id,
                DepId = a.DepId,
                ServiceName = a.ServiceName,
                Desciption = a.Desciption,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,


            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ServiceRespone>(services)
            {
                Total = services.Count,
                Type = "Services"
            };
        }
        //public async Task<ResponseModel<ServiceRespone>> SearchCom(PaginationRequest model, string value)
        //{
        //    var agencies = await _context.Agencies.Where(a => a.Service.ServiceName.Contains(value) || a.Account.Username.Contains(value)
        //    || a.ManagerName.Contains(value) || a.ServiceName.Contains(value) || a.Address.Contains(value) || a.Telephone.Contains(value)).Select(a => new ServiceRespone
        //    {
        //        Id = a.Id,
        //        ServiceName = a.ServiceName,
        //        Username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
        //        Address = a.Address,
        //        ServiceName = _context.Companies.Where(x => x.Id.Equals(a.ServiceId)).Select(x => x.ServiceName).FirstOrDefault(),
        //        ManagerName = a.ManagerName,
        //        Telephone = a.Telephone,
        //        IsDelete = a.IsDelete,
        //        CreateDate = a.CreateDate,
        //        UpdateDate = a.UpdateDate,
        //    }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
        //    return new ResponseModel<ServiceRespone>(agencies)
        //    {
        //        Total = agencies.Count,
        //        Type = "Agencies"
        //    };
        //}
        public async Task<ResponseModel<ServiceRespone>> CreateService(ServiceRequest model)
        {
            var Service = new Service
            {
                Id = Guid.NewGuid(),
                DepId = model.DepId,
                ServiceName = model.ServiceName,
                Desciption = model.Desciption,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,

            };
            var list = new List<ServiceRespone>();
            var message = "blank";
            var status = 500;
            var Service_name = await _context.Services.Where(x => x.ServiceName.Equals(Service.ServiceName)).FirstOrDefaultAsync();
            if (Service_name != null)
            {
                status = 400;
                message = "ServiceName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Services.AddAsync(Service);
                await _context.SaveChangesAsync();
                list.Add(new ServiceRespone
                {
                    Id = Service.Id,
                    DepId = Service.DepId,
                    ServiceName = Service.ServiceName,
                    Desciption = Service.Desciption,
                    IsDelete = Service.IsDelete,
                    CreateDate = Service.CreateDate,
                    UpdateDate = Service.UpdateDate,
                });
            }
            return new ResponseModel<ServiceRespone>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Service"
            };
        }
        //public async Task<ResponseModel<ServiceRespone>> UpdateService(Guid id, ServiceRequest model)
        //{
        //    var Service = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Service
        //    {
        //        Id = id,
        //        ServiceName = model.ServiceName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(model.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = model.Address,
        //        ServiceId = _context.Companies.Where(x => x.ServiceName.Equals(model.ServiceName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = model.ManagerName,
        //        Telephone = model.Telephone,
        //        IsDelete = x.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Service);
        //    await _context.SaveChangesAsync();
        //    var list = new List<ServiceRespone>();
        //    list.Add(new ServiceRespone
        //    {
        //        Id = Service.Id,
        //        ServiceName = Service.ServiceName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Service.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Service.Address,
        //        ServiceName = await _context.Companies.Where(x => x.Id.Equals(Service.ServiceId)).Select(x => x.ServiceName).FirstOrDefaultAsync(),
        //        ManagerName = Service.ManagerName,
        //        Telephone = Service.Telephone,
        //        IsDelete = Service.IsDelete,
        //        CreateDate = Service.CreateDate,
        //        UpdateDate = Service.UpdateDate,
        //    });
        //    return new ResponseModel<ServiceRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Service"
        //    };
        //}

        //public async Task<ResponseModel<ServiceRespone>> DisableService(Guid id, ServiceDisableRequest model)
        //{
        //    var Service = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Service
        //    {
        //        Id = id,
        //        ServiceName = x.ServiceName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(x.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = x.Address,
        //        ServiceId = _context.Companies.Where(x => x.ServiceName.Equals(x.ServiceName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = x.ManagerName,
        //        Telephone = x.Telephone,
        //        IsDelete = model.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Service);
        //    await _context.SaveChangesAsync();
        //    var list = new List<ServiceRespone>();
        //    list.Add(new ServiceRespone
        //    {
        //        Id = Service.Id,
        //        ServiceName = Service.ServiceName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Service.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Service.Address,
        //        ServiceName = await _context.Companies.Where(x => x.Id.Equals(Service.ServiceId)).Select(x => x.ServiceName).FirstOrDefaultAsync(),
        //        ManagerName = Service.ManagerName,
        //        Telephone = Service.Telephone,
        //        IsDelete = Service.IsDelete,
        //        CreateDate = Service.CreateDate,
        //        UpdateDate = Service.UpdateDate,
        //    });
        //    return new ResponseModel<ServiceRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Service"
        //    };
        //}

    }
}
