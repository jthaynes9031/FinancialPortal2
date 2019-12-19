using FinacialPortal2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinacialPortal2.Helpers
{
    public class RoleHelper
    {
        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(
           new UserStore<ApplicationUser>(
             new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return UserManager.IsInRole(userId, roleName);
        }
        public virtual ICollection<string> ListUserRoles(string userId)
        {
            return UserManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = UserManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = UserManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public virtual ICollection<ApplicationUser> UserInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }
        public virtual ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }

            return resultList;
        }
        public ICollection<ApplicationUser> ListUsersOnHouse(int? houseId)
        {
            //make list 
            var users = new List<ApplicationUser>();
            var list = db.Users.Where(u => u.HouseholdId == houseId).ToList();
            foreach (var user in list)
            {
                users.Add(user);
            }
            return (users);



        }
    }
}
