using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using Validation;

namespace ParkingAbilityServer.Models
{
    public class MetadataRequest
    {
        public string UserAgent { get; private set; }

        public string Session { get; private set; }

        public MetadataRequest(IHeaderDictionary headers)
        {
            Requires.NotNull(headers, nameof(headers));

            if (headers.TryGetValue("User-Agent", out StringValues values))
            {
                UserAgent = values.FirstOrDefault();
            }

            if (headers.TryGetValue("Cookie", out StringValues cookies))
            {
                var lookup = cookies.FirstOrDefault(v => v.Contains("ai_session", StringComparison.InvariantCulture));
                if (!string.IsNullOrEmpty(lookup))
                {
                    Session = lookup.Split('=')[1];
                }
            }
        }
    }
}
