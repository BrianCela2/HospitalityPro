using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.StaticFunc
{
    public class StaticFunc
    {
        public static Guid ConvertGuid(Claim receiverIdClaim) {
            Guid receiverId = Guid.Empty;
            if (receiverIdClaim != null && Guid.TryParse(receiverIdClaim.Value, out var parsedGuid))
            {
                receiverId = parsedGuid;
            }
            return receiverId;
        }
    }
}
