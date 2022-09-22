using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;

namespace UPOD.SERVICES.Services
{

    public interface IAreaService
    {
        Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model);
        Task<ResponseModel<AreaResponse>> CreateArea(AreaRequest model);
        Task<ResponseModel<AreaResponse>> UpdateArea(Guid id, AreaRequest model);
        Task<ResponseModel<AreaResponse>> DisableArea(Guid id);
    }

    public class AreaService : IAreaService
    {
        private readonly Database_UPODContext _context;
        public AreaService(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model)
        {
            var areas = await _context.Areas.Where(a => a.IsDelete == false).Select(a => new AreaResponse
            {
                id = a.Id,
                area_name = a.AreaName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AreaResponse>(areas)
            {
                Total = areas.Count,
                Type = "Areas"
            };
        }

        public async Task<ResponseModel<AreaResponse>> CreateArea(AreaRequest model)
        {

            var area = new Area
            {
                Id = Guid.NewGuid(),
                Description = model.description,
                AreaName = model.area_name,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var list = new List<AreaResponse>();
            var message = "blank";
            var status = 500;
            var area_id = await _context.Areas.Where(x => x.Id.Equals(area.Id)).FirstOrDefaultAsync();
            if (area_id != null)
            {
                status = 400;
                message = "AreaId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Areas.AddAsync(area);
                await _context.SaveChangesAsync();
                list.Add(new AreaResponse
                {
                    id = area.Id,
                    area_name = area.AreaName,
                    description = area.Description,
                    is_delete = area.IsDelete,
                    create_date = area.CreateDate,
                    update_date = area.UpdateDate
                });
            }

            return new ResponseModel<AreaResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Area"
            };
        }


        public async Task<ResponseModel<AreaResponse>> DisableArea(Guid id)
        {
            var area = await _context.Areas.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            area.IsDelete = true;
            area.UpdateDate = DateTime.Now;
            _context.Areas.Update(area);
            await _context.SaveChangesAsync();
            var list = new List<AreaResponse>();
            list.Add(new AreaResponse
            {
                is_delete = area.IsDelete,
            });
            return new ResponseModel<AreaResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Area"
            };
        }
        public async Task<ResponseModel<AreaResponse>> UpdateArea(Guid id, AreaRequest model)
        {
            var area = await _context.Areas.Where(a => a.Id.Equals(id)).Select(x => new Area
            {
                Id = id,
                AreaName = model.area_name,
                Description = model.description,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
            }).FirstOrDefaultAsync();
            _context.Areas.Update(area);
            await _context.SaveChangesAsync();
            var list = new List<AreaResponse>();
            list.Add(new AreaResponse
            {
                id = area.Id,
                area_name = area.AreaName,
                description = area.Description,
                is_delete = area.IsDelete,
                create_date = area.CreateDate,
                update_date = area.UpdateDate
            });
            return new ResponseModel<AreaResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Area"
            };
        }

    }
}
