using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;
using Contract = UPOD.REPOSITORIES.Models.Contract;

namespace UPOD.SERVICES.Services
{

    public interface IContractServiceService
    {
        Task<ResponseModel<ContractResponse>> GetAll(PaginationRequest model);
        Task<ObjectModelResponse> CreateContract(ContractRequest model);
        Task<ObjectModelResponse> GetDetailsContract(Guid id);
        Task<ObjectModelResponse> DisableContract(Guid id);
    }

    public class ContractServiceService : IContractServiceService
    {
        private readonly Database_UPODContext _context;
        public ContractServiceService(Database_UPODContext context)
        {
            _context = context;
        }
        public async Task<ObjectModelResponse> DisableContract(Guid id)
        {
            var contract = await _context.Contracts.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            contract!.IsDelete = true;
            contract.UpdateDate = DateTime.Now;
            _context.Contracts.Update(contract);
            var data = new ContractResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new ContractResponse
                {
                    id = id,
                    code = contract.Code,
                    contract_name = contract.ContractName,
                    customer = new CustomerViewResponse
                    {
                        id = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                        code = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                        name = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                        description = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                        percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                        percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                        percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                    },
                    start_date = contract.StartDate,
                    end_date = contract.EndDate,
                    is_delete = contract.IsDelete,
                    create_date = contract.CreateDate,
                    update_date = contract.UpdateDate,
                    contract_price = contract.ContractPrice,
                    priority = contract.Priority,
                    description = contract.Description,
                    attachment = contract.Attachment,
                    img = contract.Img,
                    frequency_maintain = contract.FrequencyMaintain,
                    service = _context.ContractServices.Where(a => a.ContractId.Equals(contract.Id)).Select(x => new ServiceViewResponse
                    {
                        id = x.ServiceId,
                        code = x.Service!.Code,
                        service_name = x.Service!.ServiceName,
                        description = x.Service!.Description,
                    }).ToList()

                };
            }

            return new ObjectModelResponse(data)
            {
                Type = "Contract"
            };
        }

        public async Task<ResponseModel<ContractResponse>> GetAll(PaginationRequest model)
        {
            var contracts = await _context.Contracts.Where(a => a.IsDelete == false).Select(a => new ContractResponse
            {
                id = a.Id,
                code = a.Code,
                contract_name = a.ContractName,
                customer = new CustomerViewResponse
                {
                    id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                    name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                    description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                    percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                    percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                },
                start_date = a.StartDate,
                end_date = a.EndDate,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                contract_price = a.ContractPrice,
                priority = a.Priority,
                description = a.Description,
                attachment = a.Attachment,
                frequency_maintain = a.FrequencyMaintain,
                img = a.Img,
                service = _context.ContractServices.Where(x => x.ContractId.Equals(a.Id)).Select(x => new ServiceViewResponse
                {
                    id = x.ServiceId,
                    code = x.Service!.Code,
                    service_name = x.Service!.ServiceName,
                    description = x.Service!.Description,
                }).ToList()

            }).OrderByDescending(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<ContractResponse>(contracts)
            {
                Total = contracts.Count,
                Type = "Contracts"
            };
        }


        public async Task<ObjectModelResponse> GetDetailsContract(Guid id)
        {
            var contract = await _context.Contracts.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new ContractResponse
            {
                id = a.Id,
                code = a.Code,
                contract_name = a.ContractName,
                customer = new CustomerViewResponse
                {
                    id = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                    code = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                    name = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                    description = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                    percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                    percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                    percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(a.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                },
                start_date = a.StartDate,
                end_date = a.EndDate,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate,
                contract_price = a.ContractPrice,
                priority = a.Priority,
                description = a.Description,
                attachment = a.Attachment,
                frequency_maintain = a.FrequencyMaintain,
                img = a.Img,
                service = _context.ContractServices.Where(x => x.ContractId.Equals(a.Id)).Select(x => new ServiceViewResponse
                {
                    id = x.ServiceId,
                    code = x.Service!.Code,
                    service_name = x.Service!.ServiceName,
                    description = x.Service!.Description,
                }).ToList(),
            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(contract!)
            {
                Type = "Contract"
            };
        }
        public async Task<ObjectModelResponse> CreateContract(ContractRequest model)
        {
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("CON", code_number + 1);
            var contract = new Contract
            {
                Id = Guid.NewGuid(),
                Code = code,
                CustomerId = model.customer_id,
                ContractName = model.contract_name,
                StartDate = model.start_date,
                EndDate = model.end_date,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                ContractPrice = model.contract_price,
                Priority = model.priority,
                Img = model.img,
                Attachment = model.attachment,
                Description = model.description!,
                TerminalTime = null,
                TerminalContent = null,
                FrequencyMaintain = model.frequency_maintain
            };
            foreach (var item in model.service_id)
            {
                var contract_service = new ContractService
                {
                    Id = Guid.NewGuid(),
                    Code = null,
                    ContractId = contract.Id,
                    ServiceId = item,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    UpdateDate = null,
                };
                _context.ContractServices.Add(contract_service);
            }
            var data = new ContractResponse();
            var message = "blank";
            var status = 500;
            var contract_name = await _context.Contracts.Where(x => x.ContractName!.Equals(contract.ContractName)).FirstOrDefaultAsync();
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
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new ContractResponse
                    {
                        id = contract.Id,
                        code = contract.Code,
                        contract_name = contract.ContractName,
                        customer = new CustomerViewResponse
                        {
                            id = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Id).FirstOrDefault(),
                            code = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Code).FirstOrDefault(),
                            name = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Name).FirstOrDefault(),
                            description = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.Description).FirstOrDefault(),
                            percent_for_technican_exp = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianExp).FirstOrDefault(),
                            percent_for_technican_familiar_with_agency = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianFamiliarWithAgency).FirstOrDefault(),
                            percent_for_technican_rate = _context.Customers.Where(x => x.Id.Equals(contract.CustomerId)).Select(x => x.PercentForTechnicianRate).FirstOrDefault(),
                        },
                        start_date = contract.StartDate,
                        end_date = contract.EndDate,
                        is_delete = contract.IsDelete,
                        create_date = contract.CreateDate,
                        update_date = contract.UpdateDate,
                        contract_price = contract.ContractPrice,
                        priority = contract.Priority,
                        description = contract.Description,
                        attachment = contract.Attachment,
                        img = contract.Img,
                        frequency_maintain = contract.FrequencyMaintain,
                        service = _context.ContractServices.Where(x => x.ContractId.Equals(contract.Id)).Select(x => new ServiceViewResponse
                        {
                            id = x.ServiceId,
                            code = x.Service!.Code,
                            service_name = x.Service!.ServiceName,
                            description = x.Service!.Description,
                        }).ToList(),
                    };
                }
            }


            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Contract"
            };
        }

        private async Task<int> GetLastCode()
        {
            var contract = await _context.Contracts.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(contract!.Code!);
        }
    }
}
