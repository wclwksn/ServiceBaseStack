using Base.ST.DomainModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ST.ServiceInterface
{
    public class HelloWorldService : Service
    { 
        public Object Get(Hello hello)
        {
            string _restStr = string.Format("hello {0}!", hello.name);

            return new HelloWorldModel() { name = _restStr, userid = "test" };
        }
    }
    [Authenticate]
    [Route("/hello/{name}", "GET")]
    public class Hello : IReturn<HelloWorldModel>
    {
        public string name { get; set; }
    }
}
