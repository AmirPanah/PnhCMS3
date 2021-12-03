using AutoMapper;
using PnhCMS.Services;
using PnhCMS.Repository.Common;
using PnhCMS.Repository;
using PnhCMS.Services.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Services.Course
{
    public class CourseService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PnhCMS.Repository.AppContext _appContext;
        public CourseService(IUnitOfWork unitOfWork, PnhCMS.Repository.AppContext appContext)
        {
            _unitOfWork = unitOfWork;
            _appContext = appContext;
        }
        public async Task<List<CourseVM>>  GetAll()
        {
            var list = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().GetAll().Where(x => x.Deleted == false)
            .Select(x => new CourseVM
            {
                CourseId = x.CourseId,
                CourseCode = x.CourseCode,
                CourseName = x.CourseName,
                CourseCredit = x.CourseCredit
            }).ToListAsync();

            return list;
           
        }

        public IQueryable<CourseVM> GetCourseById(string searchBy)
        {
            var list = _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().GetAll().Where(x => x.Deleted == false)
                   .Select(x => new CourseVM
                   {
                       CourseId = x.CourseId,
                       CourseCode = x.CourseCode,
                       CourseName = x.CourseName,
                       CourseCredit = x.CourseCredit
                   })
               .OrderByDescending(x => x.CourseId)
               .AsQueryable();
            if (!string.IsNullOrEmpty(searchBy))
            {
                list = list.Where(x => x.CourseName.ToLower().Contains(searchBy.ToLower()) || x.CourseCode.ToLower().Contains(searchBy.ToLower())).AsQueryable();
            }
            return list;
        }
        public async Task<PnhCMS.Repository.AppModel.Course> GetById(int id)
        {
            var entity = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().FindAsync(x => x.CourseId == id);
            return entity;
        }
        public async Task<CourseVM> GetCourseById(int id)
        {
            var entity = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().FindAsync(x => x.CourseId == id);
            if (entity != null) {
                var data = new CourseVM
                {
                    CourseId = entity.CourseId,
                    CourseCode = entity.CourseCode,
                    CourseName = entity.CourseName,
                    CourseCredit = entity.CourseCredit
                    
                };
                return data;
            }
            else
            {
                var data = new CourseVM();
                return data;
            }
           

        }


        public async Task<List<CourseListVM>> GetCourseDetails()
        {
            var list = await (from c in _appContext.Courses
                        join te in _appContext.TeacherEnrollments on c.CourseId equals te.CourseId
                        join se in _appContext.StudentEnrollments on c.CourseId equals se.CourseId
                        join ge in _appContext.Grades on se.GradeId equals ge.GradeId

                        where c.Deleted == false
                        group new { c, te, se, ge } by c.CourseId into cts
                        select new CourseListVM
                        {
                            CourseId = cts.Key,
                            CourseName = cts.Select(x => x.c.CourseName).FirstOrDefault(),
                           
                            NoOfTeacher = cts.Select(x => x.te.TeacherId).Distinct().Count(),
                            NoOfStudent = cts.Select(x => x.se.StudentId).Distinct().Count(),
                            AvgOfGrade = cts.Select(x => x.ge.GradePoint).Average(),
                        }).ToListAsync();

            return list;
        }


        public PnhCMS.Repository.AppModel.Course GetByName(string name)
        {
            return _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().GetAll().Where(x => x.CourseName.Trim().Equals(name.Trim())).SingleOrDefault();
        }
        public PnhCMS.Repository.AppModel.Course GetByCode(string name)
        {
            return _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().GetAll().Where(x => x.CourseCode.Trim().Equals(name.Trim())).SingleOrDefault();
        }
        public async Task<ResultModel> Insert(CourseVM model)
        {
            try
            {
                if (GetByName(model.CourseName) != null)
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.ExistMessage(model.CourseName) };
                if (GetByCode(model.CourseCode) != null)
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.ExistMessage(model.CourseCode) };

                var entity = new PnhCMS.Repository.AppModel.Course
                {
                    CourseCode = model.CourseCode,
                    CourseName = model.CourseName,
                    CourseCredit = model.CourseCredit,
                    CreateDate = DateTime.Now
                };

                await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Course>().InsertAsync(entity);

                return new ResultModel { Id = entity.CourseId.ToString(), Result = true, Message = NotificationMessageConfig.InsertSuccessMessage(model.CourseName) };
            }
            catch (Exception)
            {
                return new ResultModel { Result = false, Message = NotificationMessageConfig.InsertErrorMessage(model.CourseName) };
            }
        }

        public async Task<ResultModel> Update(CourseVM model)
        {
            try
            {
                var entity = await GetById(model.CourseId);
                entity.CourseCode = model.CourseCode;
                entity.CourseName = model.CourseName;
                entity.CourseCredit = model.CourseCredit;
                entity.UpdateDate = DateTime.Now;
            
                await _unitOfWork.CommitAsync();

                return new ResultModel { Id = entity.CourseId.ToString(), Result = true, Message = NotificationMessageConfig.UpdateSuccessMessage(model.CourseName) };
            }
            catch (Exception)
            {
                return new ResultModel { Id = model.CourseId.ToString(), Result = false, Message = NotificationMessageConfig.UpdateErrorMessage(model.CourseName) };
            }
        }

        public async Task<ResultModel> Delete(int id)
        {
            var model = await GetById(id);
            try
            {
                if(model !=null)
                {
                    model.Deleted = true;
                    model.DeleteDate = DateTime.Now;

                    _unitOfWork.Commit();
                    return new ResultModel { Result = true, Message = NotificationMessageConfig.DeleteSuccessMessage(model.CourseName), Id = model.CourseId.ToString() };
                }
                else
                {
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.NotFoundError};
                }

            }
            catch (Exception)
            {
                return new ResultModel { Result = false, Message = NotificationMessageConfig.DeleteErrorMessage(model.CourseName) };
            }
        }

    }
}
