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
    public interface IContractService
    {
        //Task<ResponseModel<ContractDetailResponse>> GetDetailContract(Guid id);
        //Task<ResponseModel<ContractListResponse>> GetListContract(PaginationRequest model);


    }
    public interface IContractServiceService
    {
        Task<ResponseModel<ContractResponse>> GetAll(PaginationRequest model);
        //Task<ResponseModel<ContractRespone>> SearchAgencies(PaginationRequest model, String value);
        //Task<ResponseModel<ContractRespone>> UpdateContract(Guid id, ContractRequest model);
        Task<ResponseModel<ContractResponse>> CreateContract(ContractRequest model);
        Task<ResponseModel<ContractDetailResponse>> GetDetailContract(Guid id);
        Task<ResponseModel<ContractListResponse>> GetListContract(PaginationRequest model);
        //Task<ResponseModel<ContractRespone>> DisableContract(Guid id, ContractDisableRequest model);


    }

    public class ContractServiceService : IContractServiceService
    {
        private readonly Database_UPODContext _context;
        public ContractServiceService(Database_UPODContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ContractResponse>> GetAll(PaginationRequest model)
        {
            var contracts = await _context.Contracts.Where(a => a.IsDelete == false).Select(a => new ContractResponse
            {
                id = a.Id,
                company_id = a.CompanyId,
                contract_name = a.ContractName,
                contract_price = a.ContractPrice,
                start_date = a.StartDate,
                end_date = a.EndDate,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                time_commit = a.TimeCommit,
                priority = a.Priority,
                punishment_for_customer = a.PunishmentForCustomer,
                punishment_for_it = a.PunishmentForIt,
                desciption = a.PunishmentForIt,
                service_id = _context.ContractServices.Where(x => x.ContactId.Equals(a.Id)).Select(x => x.ServiceId).ToList(),


            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ContractResponse>(contracts)
            {
                Total = contracts.Count,
                Type = "Contracts"
            };
        }
        public async Task<ResponseModel<ContractListResponse>> GetListContract(PaginationRequest model)
        {
            var contracts = await _context.Contracts.Where(a => a.IsDelete == false).Select(a => new ContractListResponse
            {
                company_name = a.Company.CompanyName,
                contract_name = a.ContractName,
                start_date = a.StartDate,
                end_date = a.EndDate,
                create_date = a.CreateDate,

            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ContractListResponse>(contracts)
            {
                Total = contracts.Count,
                Type = "Contracts"
            };
        }

        public async Task<ResponseModel<ContractDetailResponse>> GetDetailContract(Guid id)
        {
            var contract = await _context.Contracts.Where(a => a.Id.Equals(id) || a.IsDelete == false).Select(a => new ContractDetailResponse
            {
                contract_name = a.ContractName,
                company_name = a.Company.CompanyName,
                create_date = a.CreateDate,
                start_date = a.StartDate,
                end_date = a.EndDate,
                contract_price = a.ContractPrice,
                time_commit = a.TimeCommit,
                priority = a.Priority,
                punishment_for_customer = a.PunishmentForCustomer,
                punishment_for_it = a.PunishmentForIt,
                desciption = a.Desciption,
            }).ToListAsync();
            return new ResponseModel<ContractDetailResponse>(contract)
            {
                Total = contract.Count,
                Type = "Contract"
            };
        }
        public async Task<ResponseModel<ContractResponse>> CreateContract(ContractRequest model)
        {

            var contract = new Contract
            {
                Id = Guid.NewGuid(),
                CompanyId = model.company_id,
                ContractName = model.contract_name,
                StartDate = model.start_date,
                EndDate = model.end_date,
                TimeCommit = model.time_commit,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = null,
                ContractPrice = model.contract_price,
                Priority = model.priority,
                PunishmentForCustomer = model.punishment_for_customer,
                PunishmentForIt = model.punishment_for_it,
                Desciption = model.desciption,

            };
            foreach (var item in model.service_id)
            {
                var contract_service = new ContractService
                {
                    Id = Guid.NewGuid(),
                    ContactId = contract.Id,
                    ServiceId = item,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    UpdateDate = null,
                };
                _context.ContractServices.Add(contract_service);
            }
            var list = new List<ContractResponse>();
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
                await _context.SaveChangesAsync();
                list.Add(new ContractResponse
                {
                    id = contract.Id,
                    company_id = contract.CompanyId,
                    contract_name = contract.ContractName,
                    start_date = contract.StartDate,
                    end_date = contract.EndDate,
                    time_commit = contract.TimeCommit,
                    is_delete = contract.IsDelete,
                    create_date = contract.CreateDate,
                    update_date = contract.UpdateDate,
                    contract_price = contract.ContractPrice,
                    priority = contract.Priority,
                    punishment_for_customer = contract.PunishmentForCustomer,
                    punishment_for_it = contract.PunishmentForIt,
                    desciption = contract.Desciption,
                    service_id = _context.ContractServices.Where(x => x.ContactId.Equals(contract.Id)).Select(x => x.ServiceId).ToList(),
                });
            }

            return new ResponseModel<ContractResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Contract"
            };
        }
        //public async Task<ResponseModel<ContractDetailResponse>> GetDetailContract(Guid Id)
        //{
        //    var contract = await _context.Contracts.Where(x => x.Id.Equals(Id)).Select(x => new Contract
        //    {
        //        Id = Id,
        //        CompanyId = x.CompanyId,
        //        ContractName = x.ContractName,
        //        StartDate = x.StartDate,
        //        EndDate = x.EndDate,
        //        TimeCommit = x.TimeCommit,
        //        IsDelete = x.IsDelete,
        //        CreateDate = x.CreateDate,
        //        UpdateDate = x.UpdateDate,
        //        ContractPrice = x.ContractPrice,
        //        Priority = x.Priority,
        //        PunishmentForCustomer = x.PunishmentForCustomer,
        //        PunishmentForIt = x.PunishmentForIt,
        //        Desciption = x.Desciption,
        //    }).FirstOrDefaultAsync();
        //    _context.Contracts.Update(contract);
        //    await _context.SaveChangesAsync();
        //    var list = new List<ContractDetailResponse>();
        //    list.Add(new ContractDetailResponse
        //    {
        //         contract_name = contract.ContractName,
        //         company_name = contract.Company.CompanyName,
        //         create_date = contract.CreateDate,
        //         start_date = contract.StartDate,
        //         end_date = contract.EndDate,
        //         contract_price = contract.ContractPrice,
        //         time_commit = contract.TimeCommit,
        //         priority = contract.Priority,
        //         punishment_for_customer=contract.PunishmentForCustomer,
        //         punishment_for_it=contract.PunishmentForIt,
        //         desciption = contract.Desciption,
        //    });
        //    return new ResponseModel<ContractDetailResponse>(list)
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
