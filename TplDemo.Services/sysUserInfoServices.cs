using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Common.IsWhatExtenions;
using TplDemo.Common.Message;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;
using TplDemo.Repository.BASE;
using TplDemo.Repository.UnitOfWork;

namespace TplDemo.Services
{
    public class sysUserInfoServices : BASE.BaseServices<sysUserInfoEntity>, sysUserInfoIServices
    {
        public IBaseRepository<sysUserInfoEntity> dal;

        public IUnitOfWork unitOfWork;

        public sysUserInfoServices(IUnitOfWork _unitOfWork, IBaseRepository<sysUserInfoEntity> _dal)
        {
            dal = _dal;
            base.BaseDal = dal;
            unitOfWork = _unitOfWork;
        }

        /// <summary></summary>
        /// <returns></returns>
        public async Task<sysUserInfoEntity> Verificationlogin(ViewLogin model)
        {
            return null;
        }
    }
}