using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Dapper
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();

    }
}
