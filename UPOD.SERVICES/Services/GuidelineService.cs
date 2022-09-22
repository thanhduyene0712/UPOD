using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.Services;

namespace UPOD.SERVICES.Services
{

    public interface IGuidelineService
    {
        Task<ResponseModel<GuidelineResponse>> GetListGuidelines(PaginationRequest model);
        Task<ResponseModel<GuidelineResponse>> CreateGuideline(GuidelineRequest model);
        Task<ResponseModel<GuidelineResponse>> UpdateGuideline(Guid id, GuidelineRequest model);
        Task<ResponseModel<GuidelineResponse>> DisableGuideline(Guid id);
    }

    public class GuidelineService : IGuidelineService
    {
        private readonly Database_UPODContext _context;
        public GuidelineService(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<GuidelineResponse>> GetListGuidelines(PaginationRequest model)
        {
            var guidelines = await _context.Guidelines.Where(a => a.IsDelete == false).Select(a => new GuidelineResponse
            {
                id = a.Id,
                service_id = a.ServiceId,
                guideline_des = a.GuidelineDes,
                guideline_name = a.GuidelineName,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<GuidelineResponse>(guidelines)
            {
                Total = guidelines.Count,
                Type = "Guidelines"
            };
        }

        public async Task<ResponseModel<GuidelineResponse>> CreateGuideline(GuidelineRequest model)
        {

            var guideline = new Guideline
            {
                Id = Guid.NewGuid(),
                ServiceId = model.service_id,
                GuidelineName = model.guideline_name,
                GuidelineDes = model.guideline_des,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var list = new List<GuidelineResponse>();
            var message = "blank";
            var status = 500;
            var guideline_id = await _context.Guidelines.Where(x => x.Id.Equals(guideline.Id)).FirstOrDefaultAsync();
            if (guideline_id != null)
            {
                status = 400;
                message = "GuidelineId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
                await _context.Guidelines.AddAsync(guideline);
                await _context.SaveChangesAsync();
                list.Add(new GuidelineResponse
                {
                    id = guideline.Id,
                    service_id = guideline.ServiceId,
                    guideline_name = guideline.GuidelineName,
                    guideline_des = guideline.GuidelineDes,
                    is_delete = guideline.IsDelete,
                    create_date = guideline.CreateDate,
                    update_date = guideline.UpdateDate
                });
            }

            return new ResponseModel<GuidelineResponse>(list)
            {
                Message = message,
                Status = status,
                Total = list.Count,
                Type = "Guideline"
            };
        }


        public async Task<ResponseModel<GuidelineResponse>> DisableGuideline(Guid id)
        {
            var guideline = await _context.Guidelines.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            guideline.IsDelete = true;
            guideline.UpdateDate = DateTime.Now;
            _context.Guidelines.Update(guideline);
            await _context.SaveChangesAsync();
            var list = new List<GuidelineResponse>();
            list.Add(new GuidelineResponse
            {
                is_delete = guideline.IsDelete,
            });
            return new ResponseModel<GuidelineResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Guideline"
            };
        }
        public async Task<ResponseModel<GuidelineResponse>> UpdateGuideline(Guid id, GuidelineRequest model)
        {
            var guideline = await _context.Guidelines.Where(a => a.Id.Equals(id)).Select(x => new Guideline
            {
                Id = id,
                ServiceId  = model.service_id,
                GuidelineName = model.guideline_name,
                GuidelineDes = model.guideline_des,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
            }).FirstOrDefaultAsync();
            _context.Guidelines.Update(guideline);
            await _context.SaveChangesAsync();
            var list = new List<GuidelineResponse>();
            list.Add(new GuidelineResponse
            {
                id = guideline.Id,
                service_id = guideline.ServiceId,
                guideline_name = guideline.GuidelineName,
                guideline_des = guideline.GuidelineDes,
                is_delete = guideline.IsDelete,
                create_date = guideline.CreateDate,
                update_date = guideline.UpdateDate,
            });
            return new ResponseModel<GuidelineResponse>(list)
            {
                Status = 201,
                Total = list.Count,
                Type = "Guideline"
            };
        }

    }
}
