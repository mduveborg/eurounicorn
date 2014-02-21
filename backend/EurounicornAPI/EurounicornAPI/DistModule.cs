using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI
{
    public class DistModule : NancyModule
    {
        public DistModule()
        {
            Get["/{path*}"] = _ =>
            {
                string path = _.path;
                
                return Response.AsFile(string.Format("dist/{0}", path));
            };
            Get["/"] = _ =>
            {
                string path = _.path;

                return Response.AsFile("dist/index.html");
            };
        }
    }
}