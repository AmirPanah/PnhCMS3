using AutoMapper;
using PnhCMS.Services.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Services.Common
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PnhCMS.Repository.AppModel.Course, CourseVM>().ReverseMap();
           
        }
    }
}