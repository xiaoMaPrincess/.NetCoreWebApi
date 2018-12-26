using Core.IRepository;
using Core.IServices;
using Core.Model.Models;
using Core.Repository;
using Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        public int Sum(int i, int j)
        {
            throw new NotImplementedException();
        }
    }
}
