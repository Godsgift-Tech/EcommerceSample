using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse
{
    public record ServiceResponse<T>(T Data, bool success = false, string message = null!);
   
}
