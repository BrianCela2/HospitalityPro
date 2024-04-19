using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
        public static int GetDayDiff(int Datedifference,DateTime checkin, DateTime checkout)
        {
           int diffOfDates = (checkout - checkin).Days;
            if (diffOfDates > Datedifference)
            {
                Datedifference = diffOfDates;
            }
            return Datedifference;
        }
        public static decimal GetTotalPrice(int Datedifferences,decimal price)
        {
            if (Datedifferences > 30)
            {
                return price * 0.75m; ;
            }
            else if (Datedifferences > 5)
            {
                return price * 0.80m; ;
            }
            else
            {
                return price;
            }
          
        }
        public static Guid GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var receiverIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId;
            if (receiverIdClaim != null)
            {
                return userId = ConvertGuid(receiverIdClaim);
            }
            else
            {
                throw new Exception("User doesn't not exist");
            }
        }
    }
}
