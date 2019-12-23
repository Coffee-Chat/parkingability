using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public class MemoryFlightProvider : IFlightProvider
    {
        private static readonly Random randomNumberGenerator = new Random(1);

        private static readonly Dictionary<string, Func<bool>> viewToFlight = new Dictionary<string, Func<bool>>()
        {
            { "WA",  () => { return randomNumberGenerator.Next(0, 100) > 50; }},
            { "Seattle",  () => { return randomNumberGenerator.Next(0, 100) > 50; }}
        };

        public Task<string> CalculateFlightAsync(string id)
        {
            if (viewToFlight.TryGetValue(id, out Func<bool> evaluation) && evaluation.Invoke())
            {
                return Task.FromResult("Custom");
            }

            return Task.FromResult("Standard");
        }
    }
}
