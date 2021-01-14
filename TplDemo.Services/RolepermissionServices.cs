using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Repository.BASE;

namespace TplDemo.Services
{
    public class RolepermissionServices : BASE.BaseServices<RolePermissions>, RolepermissionIServices
    {
        private IBaseRepository<RolePermissions> dal = null;

        public RolepermissionServices(IBaseRepository<RolePermissions> _dal)
        {
            dal = _dal;
            this.BaseDal = dal;
        }
    }
}