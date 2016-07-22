using System;
namespace Base.ST.DomainModel
{
    /// <summary>
    /// 登录日志列表
    /// </summary>   
    public class sms_loginlog
    {
        public string id { get; set; }
        public DateTime createdate { get; set; }  
        public string sessionid { get; set; }
        public string status { get; set; }
       
        public string ipaddress { get; set; }
        public string ipaddressname { get; set; }
        public string username { get; set; }
    }
}