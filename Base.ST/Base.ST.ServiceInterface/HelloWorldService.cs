using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.ST.ServiceInterface
{
    public class HelloWorldService : Service
    {
        public object Get(Hello hello)
        {
            string _restStr = string.Format("hello {0}!", hello.name); 

            return _restStr;
        }
    }
    [Authenticate]
    [Route("/hello/{name}", "GET")]
    public class Hello : IReturn<string>
    {
        public string name { get; set; }
    }
}
