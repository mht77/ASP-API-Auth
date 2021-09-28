using System.ComponentModel.DataAnnotations;
using IdentityServer4.Models;
using Newtonsoft.Json;

namespace IdentitySample.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [DataType(DataType.EmailAddress)][Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)] [JsonIgnore][Required]
        public string Password { get; private set; }
        
        [JsonProperty("Password")]
        private string PasswordSetter
        {
            set => Password = value.Sha256();
        }
    }
}