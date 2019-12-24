using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServer.BusinessLayer
{
    public class StorageLocationsRepository : ILocationsRepository
    {
        private static readonly long hourTicks = TimeSpan.FromHours(1).Ticks;
        private readonly CloudStorageAccount storageAccount;

        public StorageLocationsRepository(CloudStorageAccount storageAccount)
        {
            this.storageAccount = storageAccount;
        }

        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "This value is calculated by server.")]
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Blob storage containers require lowercase.")]
        public async Task CreateAsync(Location location)
        {
            Requires.NotNull(location, nameof(location));

            var blobClient = storageAccount.CreateCloudBlobClient();
            string containerName = location.ParentEntity.ToLower(CultureInfo.InvariantCulture);
            if (containerName.Length < 3)
            {
                containerName += "pad";
            }

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            long ticks = DateTime.UtcNow.Ticks / hourTicks;
            var hourly = new DateTime(ticks * hourTicks, DateTimeKind.Utc);
            location.HourlyTimestampUtc = hourly;
            var blob = container.GetBlockBlobReference($"{hourly.ToString("yyyy-dd-M-HH")}/{location.ClientTimestamp}.json");
            blob.Properties.ContentType = "application/json";

            string content = JsonConvert.SerializeObject(location);
            await blob.UploadTextAsync(content, Encoding.UTF8, AccessCondition.GenerateEmptyCondition(), null, null);
        }
    }
}
