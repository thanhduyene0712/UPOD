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
    //public interface IContractService
    //{
    //    Task<ResponseModel<ContractRespone>> GetAll(PaginationRequest model);
    //    //Task<ResponseModel<ContractRespone>> SearchAgencies(PaginationRequest model, String value);
    //    //Task<ResponseModel<ContractRespone>> UpdateContract(Guid id, ContractRequest model);
    //    Task<ResponseModel<ContractRespone>> CreateContract(ContractRequest model);
    //    //Task<ResponseModel<ContractRespone>> DisableContract(Guid id, ContractDisableRequest model);


    //}
    public interface IContractServiceService
    {
        Task<ResponseModel<ContractRespone>> GetAll(PaginationRequest model);
        //Task<ResponseModel<ContractRespone>> SearchAgencies(PaginationRequest model, String value);
        //Task<ResponseModel<ContractRespone>> UpdateContract(Guid id, ContractRequest model);
        Task<ResponseModel<ContractRespone>> CreateContract(ContractRequest model);
        //Task<ResponseModel<ContractRespone>> DisableContract(Guid id, ContractDisableRequest model);


    }

    public class ContractServiceService : IContractServiceService
    {
        private readonly Database_UPODContext _context;
        public ContractServiceService(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ContractRespone>> GetAll(PaginationRequest model)
        {
            var contracts = await _context.Contracts.Select(a => new ContractRespone
            {
                Id = a.Id,
                CompanyId = a.CompanyId,
                ContractName = a.ContractName,
                ContractPrice = a.ContractPrice,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                IsDelete = a.IsDelete,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,
                TimeCommit = a.TimeCommit,
                Priority = a.Priority,
                PunishmentForCustomer = a.PunishmentForCustomer,
                PunishmentForIt = a.PunishmentForIt,
                Desciption = a.PunishmentForIt,
                ServiceId = _context.ContractServices.Where(x => x.ContactId.Equals(a.Id)).Select(x => x.ServiceId).FirstOrDefault(),


            }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ContractRespone>(contracts)
            {
                Total = contracts.Count,
                Type = "Contracts"
            };
        }
        //public async Task<ResponseModel<ContractRespone>> SearchCom(PaginationRequest model, string value)
        //{
        //    var agencies = await _context.Agencies.Where(a => a.Contract.ContractName.Contains(value) || a.Account.Username.Contains(value)
        //    || a.ManagerName.Contains(value) || a.ContractName.Contains(value) || a.Address.Contains(value) || a.Telephone.Contains(value)).Select(a => new ContractRespone
        //    {
        //        Id = a.Id,
        //        ContractName = a.ContractName,
        //        Username = _context.Accounts.Where(x => x.Id.Equals(a.AccountId)).Select(x => x.Username).FirstOrDefault(),
        //        Address = a.Address,
        //        ContractName = _context.Companies.Where(x => x.Id.Equals(a.ContractId)).Select(x => x.ContractName).FirstOrDefault(),
        //        ManagerName = a.ManagerName,
        //        Telephone = a.Telephone,
        //        IsDelete = a.IsDelete,
        //        CreateDate = a.CreateDate,
        //        UpdateDate = a.UpdateDate,
        //    }).OrderBy(x => x.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
        //    return new ResponseModel<ContractRespone>(agencies)
        //    {
        //        Total = agencies.Count,
        //        Type = "Agencies"
        //    };
        //}
        public async Task<ResponseModel<ContractRespone>> CreateContract(ContractRequest model)
        {

            var contract = new Contract
            {
                Id = Guid.NewGuid(),
                CompanyId = model.CompanyId,
                ContractName = model.ContractName,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TimeCommit = model.TimeCommit,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,
                ContractPrice = model.ContractPrice,
                Priority = model.Priority,
                PunishmentForCustomer = model.PunishmentForCustomer,
                PunishmentForIt = model.PunishmentForIt,
                Desciption = model.Desciption,

            };
            var contract_service = new ContractService
            {
                Id = Guid.NewGuid(),
                ContactId = contract.Id,
                ServiceId = model.ServiceId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,
            };
            var list = new List<ContractRespone>();
            var message = "blank";
            var status = 500;
            var Contract_name = await _context.Contracts.Where(x => x.ContractName.Equals(contract.ContractName)).FirstOrDefaultAsync();
            if (Contract_name != null)
            {
                status = 400;
                message = "ContractName is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Contracts.AddAsync(contract);
                await _context.ContractServices.AddAsync(contract_service);
                await _context.SaveChangesAsync();
                list.Add(new ContractRespone
                {
                    Id = contract.Id,
                    CompanyId = contract.CompanyId,
                    ContractName = contract.ContractName,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    TimeCommit = contract.TimeCommit,
                    IsDelete = contract.IsDelete,
                    CreateDate = contract.CreateDate,
                    UpdateDate = contract.UpdateDate,
                    ContractPrice = contract.ContractPrice,
                    Priority = contract.Priority,
                    PunishmentForCustomer = contract.PunishmentForCustomer,
                    PunishmentForIt = contract.PunishmentForIt,
                    Desciption = contract.Desciption,
                    ServiceId = _context.ContractServices.Where(x => x.ContactId.Equals(contract.Id)).Select(x => x.ServiceId).FirstOrDefault(),
                });
            }

            return new ResponseModel<ContractRespone>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Contract"
            };
        }
        //public async Task<ResponseModel<ContractRespone>> UpdateContract(Guid id, ContractRequest model)
        //{
        //    var Contract = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Contract
        //    {
        //        Id = id,
        //        ContractName = model.ContractName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(model.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = model.Address,
        //        ContractId = _context.Companies.Where(x => x.ContractName.Equals(model.ContractName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = model.ManagerName,
        //        Telephone = model.Telephone,
        //        IsDelete = x.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Contract);
        //    await _context.SaveChangesAsync();
        //    var list = new List<ContractRespone>();
        //    list.Add(new ContractRespone
        //    {
        //        Id = Contract.Id,
        //        ContractName = Contract.ContractName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Contract.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Contract.Address,
        //        ContractName = await _context.Companies.Where(x => x.Id.Equals(Contract.ContractId)).Select(x => x.ContractName).FirstOrDefaultAsync(),
        //        ManagerName = Contract.ManagerName,
        //        Telephone = Contract.Telephone,
        //        IsDelete = Contract.IsDelete,
        //        CreateDate = Contract.CreateDate,
        //        UpdateDate = Contract.UpdateDate,
        //    });
        //    return new ResponseModel<ContractRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Contract"
        //    };
        //}

        //public async Task<ResponseModel<ContractRespone>> DisableContract(Guid id, ContractDisableRequest model)
        //{
        //    var Contract = await _context.Agencies.Where(x => x.Id.Equals(id)).Select(x => new Contract
        //    {
        //        Id = id,
        //        ContractName = x.ContractName,
        //        AccountId = _context.Accounts.Where(x => x.Username.Equals(x.Username)).Select(x => x.Id).FirstOrDefault(),
        //        Address = x.Address,
        //        ContractId = _context.Companies.Where(x => x.ContractName.Equals(x.ContractName)).Select(x => x.Id).FirstOrDefault(),
        //        ManagerName = x.ManagerName,
        //        Telephone = x.Telephone,
        //        IsDelete = model.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = DateTime.Now,
        //    }).FirstOrDefaultAsync();
        //    _context.Agencies.Update(Contract);
        //    await _context.SaveChangesAsync();
        //    var list = new List<ContractRespone>();
        //    list.Add(new ContractRespone
        //    {
        //        Id = Contract.Id,
        //        ContractName = Contract.ContractName,
        //        Username = await _context.Accounts.Where(x => x.Id.Equals(Contract.AccountId)).Select(x => x.Username).FirstOrDefaultAsync(),
        //        Address = Contract.Address,
        //        ContractName = await _context.Companies.Where(x => x.Id.Equals(Contract.ContractId)).Select(x => x.ContractName).FirstOrDefaultAsync(),
        //        ManagerName = Contract.ManagerName,
        //        Telephone = Contract.Telephone,
        //        IsDelete = Contract.IsDelete,
        //        CreateDate = Contract.CreateDate,
        //        UpdateDate = Contract.UpdateDate,
        //    });
        //    return new ResponseModel<ContractRespone>(list)
        //    {
        //        Status = 201,
        //        Total = list.Count,
        //        Type = "Contract"
        //    };
        //}

    }
}
