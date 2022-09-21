using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;

namespace UPOD.SERVICES.Services
{

    public interface IContractServiceService
    {
        Task<ResponseModel<ContractResponse>> GetAll(PaginationRequest model);
        Task<ResponseModel<ContractResponse>> CreateContract(ContractRequest model);
        Task<ResponseModel<ContractDetailResponse>> GetDetailContract(Guid id);
        Task<ResponseModel<ContractListResponse>> GetListContract(PaginationRequest model);
        Task<ResponseModel<ContractResponse>> DisableContract(Guid id);
    }

    public class ContractServiceService : IContractServiceService
    {
        private readonly Database_UPODContext _context;
        public ContractServiceService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<ContractResponse>> DisableContract(Guid id)
        {
            var contract = await _context.Contracts.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            contract.IsDelete = true;
            contract.UpdateDate = DateTime.Now;
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            var list = new List<ContractResponse>();
            list.Add(new ContractResponse
            {
                is_delete = contract.IsDelete
            });
            return new ResponseModel<ContractResponse>(list)
            {
                Total = list.Count,
                Type = "Contract"
            };
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
            var contract = await _context.Contracts.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new ContractDetailResponse
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
            var contract_name = await _context.Contracts.Where(x => x.ContractName.Equals(contract.ContractName)).FirstOrDefaultAsync();
            if (contract_name != null)
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
      

    }
}
