using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
<<<<<<< Updated upstream:src/WebApp/Areas/Identity/Data/ApplicationUser.cs
    public bool Subscribe { get; set; }
}

=======
    public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
}
>>>>>>> Stashed changes:src/WebApp/Data/ApplicationUser.cs
