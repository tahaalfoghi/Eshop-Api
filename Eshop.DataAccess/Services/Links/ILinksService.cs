using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Links
{
    public interface ILinksService
    {
        void Generate(string endpoint, object? routeValues, string rel, string method);
    }
}
