using Microsoft.AspNetCore.Identity;

namespace InventoryWorld.Models
{
    public class RoleData
    {

        public static async Task CreateRolesAdmin(IServiceProvider serviceProvider, IConfiguration configuration)

        {



            var rolemanager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var usermanager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] Roles = new string[] { "SuperAdmin", "Staff" };

            foreach (var rolename in Roles)
            {
                bool roleExists = await rolemanager.RoleExistsAsync(rolename);
                if (!roleExists)
                {
                    var roleResult = await rolemanager.CreateAsync(new AppRole { Name = rolename });

                }
            }

            var superadmin = new AppUser
            {

                UserName = configuration.GetSection("UserSettings")["UserEmail"],
                Email = configuration.GetSection("UserSettings")["UserEmail"],

            };

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            string? password = configuration.GetSection("UserSettings")["UserPassword"];
            var user = await usermanager.FindByEmailAsync(configuration.GetSection("UserSettings")["UserEmail"]);
            if (user == null)
            {
                var createsuperadmin = await usermanager.CreateAsync(superadmin, password);

                if (createsuperadmin.Succeeded)
                {
                    await usermanager.AddToRoleAsync(superadmin, "SuperAdmin");
                }
            }

        }

    }
}
