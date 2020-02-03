using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;


namespace VITV.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? EmployeeId { get; set; }

        [NotMapped]
        [System.ComponentModel.DataAnnotations.DataType(DataType.Text)]
        public override string Email { get; set; }
        [NotMapped]
        public override bool EmailConfirmed {get;set;}
        [NotMapped]
        public override string PhoneNumber { get; set; }
        [NotMapped]
        public override bool PhoneNumberConfirmed { get; set; }

        public bool Locked { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}