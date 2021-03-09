using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AfrikanEchoes.Data
{
    public class AfrikanEchoesUserDbContext : IdentityDbContext
    {
        public AfrikanEchoesUserDbContext(DbContextOptions<AfrikanEchoesUserDbContext> options)
            : base(options)
        {
        }
    }
}
