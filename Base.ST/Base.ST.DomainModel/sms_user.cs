using System;
namespace Base.ST.DomainModel
{
    /// <summary>
    /// 用户信息列表
    /// </summary>   
    public class sms_user
    {
        public string userid { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string realname { get; set; }
        public string spell { get; set; }
        public string alias { get; set; }
        public string roleid { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public DateTime birthday { get; set; }
        public string email { get; set; }
        public string dutyid { get; set; }
        public string companyid { get; set; }
        public string departmentid { get; set; }
        public string workgroupid { get; set; }
        public string description { get; set; }
        public string openid { get; set; } 
        public DateTime createdate { get; set; }
    }
}