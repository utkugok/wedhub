using Microsoft.AspNetCore.Identity;

namespace WedAPI.Models
{
    public class AppUser : IdentityUser
    {
        //public string Name { get; set; }
        //public string Surname { get; set; }
        public int State { get; set; }

        public DateTime CreatedDT { get; set; }
        
        public DateTime LastUpdatedDT { get; set; }
    }
}
