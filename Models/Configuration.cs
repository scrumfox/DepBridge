using System;
using System.ComponentModel.DataAnnotations;

namespace depBridger.Models
{
    [Serializable]
    public class Configuration
    {
        [Required]
        public string ApiEndPoint { get; set; }

        [Required]
        public string ProjectUid { get; set; }

        [Required]
        public string ApiKey { get; set; }
    }
}
