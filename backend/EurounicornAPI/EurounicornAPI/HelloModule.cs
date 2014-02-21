using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI
{
    public class HelloModule : Nancy.NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World";
        }
    }
}