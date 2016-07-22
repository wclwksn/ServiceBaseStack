using System;
namespace Base.ST
{
    /// <summary>
    /// Custom User DataModel for harvesting UserAuth info into your own DB table
    /// </summary>   
    public class User
    {
        public int Id { get; set; }
        public string userid { get; set; }
        public string name { get; set; } 

        public DateTime loginTime { get; set; } 
    }
}