using Base.ST.DomainModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Base.ST.ServiceInterface
{
    public class UserService : Service
    {
        public object Get(getUserInfoByName userInfo)
        {
            sms_user _smsUser = new sms_user(); 
            _smsUser = Db.Single<sms_user>(x => x.name == userInfo.name);
            if (_smsUser != null)
            {
                _smsUser.password = string.Empty;
            }
            return _smsUser;
        }
    }
    [Authenticate]
    [Route("/SmsUser/{name}", "GET")]
    public class getUserInfoByName : IReturn<sms_user>
    {
        public string name { get; set; }
    }
}
