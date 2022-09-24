//using Microsoft.EntityFrameworkCore;
//using UPOD.REPOSITORIES.Models;
//using UPOD.REPOSITORIES.RequestModels;
//using UPOD.REPOSITORIES.ResponeModels;

//namespace UPOD.SERVICES.Services
//{
//    public interface IServiceService
//    {
//        Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model);
//        Task<ResponseModel<ServiceResponse>> GetServiceDetails(Guid id, PaginationRequest model);
//        Task<ResponseModel<ServiceResponse>> UpdateService(Guid id, ServiceRequest model);
//        Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model);
//        Task<ResponseModel<ServiceResponse>> DisableService(Guid id);


//    }

//    public class ServiceService : IServiceService
//    {
//        private readonly Database_UPODContext _context;
//        public ServiceService(Database_UPODContext context)
//        {
//            _context = context;
//        }
//        public async Task<ResponseModel<ServiceResponse>> GetServiceDetails(Guid id, PaginationRequest model)
//        {
//            var service = await _context.Services.Where(a => a.Id.Equals(id) && a.IsDelete == false).Select(a => new ServiceResponse
//            {
//                id = a.Id,
//                service_name = a.ServiceName,
//                desciption = a.Desciption,
//                is_delete = a.IsDelete,
//                create_date = a.CreateDate,
//                update_date = a.UpdateDate,


//            }).OrderBy(x => x.create_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
//            return new ResponseModel<ServiceResponse>(service)
//            {
//                Total = service.Count,
//                Type = "Service"
//            };
//        }
//        public async Task<ResponseModel<ServiceResponse>> GetAll(PaginationRequest model)
//        {
//            var services = await _context.Services.Where(a => a.IsDelete == false).Select(a => new ServiceResponse
//            {
//                id = a.Id,
//                service_name = a.ServiceName,
//                desciption = a.Desciption,
//                is_delete = a.IsDelete,
//                create_date = a.CreateDate,
//                update_date = a.UpdateDate,


//            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
//            return new ResponseModel<ServiceResponse>(services)
//            {
//                Total = services.Count,
//                Type = "Services"
//            };
//        }

//        public async Task<ResponseModel<ServiceResponse>> CreateService(ServiceRequest model)
//        {
//            var service = new Service
//            {
//                Id = Guid.NewGuid(),
//                ServiceName = model.service_name,
//                Desciption = model.desciption,
//                IsDelete = false,
//                CreateDate = DateTime.Now,
//                UpdateDate = DateTime.Now,

//            };
//            var list = new List<ServiceResponse>();
//            var message = "blank";
//            var status = 500;
//            var service_name = await _context.Services.Where(x => x.ServiceName.Equals(service.ServiceName)).FirstOrDefaultAsync();
//            if (service_name != null)
//            {
//                status = 400;
//                message = "ServiceName is already exists!";
//            }
//            else
//            {
//                message = "Successfully";
//                status = 201;
//                await _context.Services.AddAsync(service);
//                await _context.SaveChangesAsync();
//                list.Add(new ServiceResponse
//                {
//                    id = service.Id,
//                    service_name = service.ServiceName,
//                    desciption = service.Desciption,
//                    is_delete = service.IsDelete,
//                    create_date = service.CreateDate,
//                    update_date = service.UpdateDate,
//                });
//            }
//            return new ResponseModel<ServiceResponse>(list)
//            {
//                Message = message,
//                Status = status,
//                Total = list.Count,
//                Type = "Service"
//            };
//        }
//        public async Task<ResponseModel<ServiceResponse>> UpdateService(Guid id, ServiceRequest model)
//        {
//            var service = await _context.Services.Where(x => x.Id.Equals(id)).Select(x => new Service
//            {
//                Id = id,
//                ServiceName = model.service_name,
//                Desciption = model.desciption,
//                IsDelete = x.IsDelete,
//                CreateDate = x.CreateDate,
//                UpdateDate = DateTime.Now,
//            }).FirstOrDefaultAsync();
//            _context.Services.Update(service);
//            await _context.SaveChangesAsync();
//            var list = new List<ServiceResponse>();
//            list.Add(new ServiceResponse
//            {
//                id = service.Id,
//                service_name = service.ServiceName,
//                desciption = service.Desciption,
//                is_delete = service.IsDelete,
//                create_date = service.CreateDate,
//                update_date = service.UpdateDate,
//            });
//            return new ResponseModel<ServiceResponse>(list)
//            {
//                Status = 201,
//                Total = list.Count,
//                Type = "Service"
//            };
//        }

//        public async Task<ResponseModel<ServiceResponse>> DisableService(Guid id)
//        {
//            var service = await _context.Services.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
//            service.IsDelete = true;
//            service.UpdateDate = DateTime.Now;
//            _context.Services.Update(service);
//            await _context.SaveChangesAsync();
//            var list = new List<ServiceResponse>();
//            list.Add(new ServiceResponse
//            {
//                id = service.Id,
//                service_name = service.ServiceName,
//                desciption = service.Desciption,
//                is_delete = service.IsDelete,
//                create_date = service.CreateDate,
//                update_date = service.UpdateDate,
//            });
//            return new ResponseModel<ServiceResponse>(list)
//            {
//                Status = 201,
//                Total = list.Count,
//                Type = "Service"
//            };
//        }

//    }
//}
