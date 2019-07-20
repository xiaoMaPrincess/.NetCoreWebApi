﻿using Core.IRepository.Mis;
using Core.IServices.Mis;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Mis
{
    public class HomeServices : IHomeServices
    {
        private readonly IHomeRepository _homeRepository;
        public HomeServices(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        public async Task<List<MenuInfo>> GetMenuList(Guid userID)
        {
            return await _homeRepository.GetMenuList(userID);
        }
    }
}
