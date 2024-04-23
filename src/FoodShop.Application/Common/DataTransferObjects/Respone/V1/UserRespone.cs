using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public class UserResponse
    {
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Facebook { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; } 
    }
    public class UserResponseList
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get;set; }
        public string FullName { get; set; }    
    }
}
