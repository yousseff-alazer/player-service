using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.CommonDefinitions.Records
{
    public class Console
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public class Debug
    {
        public LogLevel LogLevel { get; set; }
    }

    public class Environment
    {
        public string Name { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }

        [JsonProperty("Microsoft.Hosting.Lifetime")]
        public string MicrosoftHostingLifetime { get; set; }

        [JsonProperty("Microsoft.Hosting")]
        public string MicrosoftHosting { get; set; }

        [JsonProperty("Microsoft.Extensions.Hosting")]
        public string MicrosoftExtensionsHosting { get; set; }
    }

    public class RabbitMQ
    {
        public string RabbitMqRootUri { get; set; }
        public string RabbitMqUri { get; set; }
        public string PlayerGroup { get; set; }
        public string PlayerReview { get; set; }
        public string PlayerChallenge { get; set; }
        public string PlayerReservation { get; set; }
        public string PlayerOrder { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RabbitMqZoneCoachUri { get; set; }
        public string RabbitMqZoneGroupUri { get; set; }
        public string RabbitMqZoneNutritionistUri { get; set; }
        public string RabbitMqZonePhysiotherapistUri { get; set; }
        public string RabbitMqZoneVenueUri { get; set; }
        public string RabbitMqZoneChallengeUri { get; set; }
        public string RabbitMqZonePlayerUri { get; set; }
        public string RabbitMqZoneContentUri { get; set; }
        public string RabbitMqZoneLocalizeCoachUri { get; set; }
        public string RabbitMqZoneLocalizeNutritionistUri { get; set; }
        public string RabbitMqZoneLocalizePhysiotherapistUri { get; set; }
        public string RabbitMqZoneLocalizeVenueUri { get; set; }
        public string RabbitMqZoneLocalizeGroupUri { get; set; }
        public string RabbitMqZoneLocalizeChallengeUri { get; set; }
        public string RabbitMqZoneLocalizePlayerUri { get; set; }
        public string RabbitMqZoneLocalizeContentUri { get; set; }

        public string RabbitMqAreaCoachUri { get; set; }
        public string RabbitMqAreaGroupUri { get; set; }
        public string RabbitMqAreaNutritionistUri { get; set; }
        public string RabbitMqAreaPhysiotherapistUri { get; set; }
        public string RabbitMqAreaVenueUri { get; set; }
        public string RabbitMqAreaChallengeUri { get; set; }
        public string RabbitMqAreaPlayerUri { get; set; }
        public string RabbitMqAreaContentUri { get; set; }
        public string RabbitMqAreaLocalizeCoachUri { get; set; }
        public string RabbitMqAreaLocalizeNutritionistUri { get; set; }
        public string RabbitMqAreaLocalizePhysiotherapistUri { get; set; }
        public string RabbitMqAreaLocalizeVenueUri { get; set; }
        public string RabbitMqAreaLocalizeGroupUri { get; set; }
        public string RabbitMqAreaLocalizeChallengeUri { get; set; }
        public string RabbitMqAreaLocalizePlayerUri { get; set; }
        public string RabbitMqAreaLocalizeContentUri { get; set; }
    }

    public class AppSettingsRecord
    {
        public DatabaseSettings DatabaseSettings { get; set; }
        public Logging Logging { get; set; }
        public RabbitMQ RabbitMQ { get; set; }
        public Environment Environment { get; set; }
        public Debug Debug { get; set; }
        public Console Console { get; set; }
    }
}
