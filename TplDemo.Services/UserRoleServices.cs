using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Repository.BASE;

namespace TplDemo.Services
{
    public class UserRoleServices : BASE.BaseServices<UserRoleEntity>, UserRoleIServices
    {
        public IBaseRepository<UserRoleEntity> dal;

        public UserRoleServices(IBaseRepository<UserRoleEntity> _dal)
        {
            dal = _dal;
            base.BaseDal = dal;
        }
    }
}