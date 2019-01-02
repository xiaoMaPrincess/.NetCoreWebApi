using AutoMapper;
using Core.Model.Models;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.AutoMapper
{
    /// <summary>
    /// 自动映射
    /// </summary>
    public class CustomProfile: Profile
    {
        public CustomProfile()
        {
            CreateMap<BlogArticle, BlogViewModel>();
        }
    }
}
