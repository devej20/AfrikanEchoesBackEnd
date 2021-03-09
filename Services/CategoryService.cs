using AfrikanEchoes.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfrikanEchoes.Services
{
    public interface ICategoryService
    {

        IEnumerable<Category> GetAll();
        Category GetById(long id);

    }

    public class CategoryService : ICategoryService
    {

        private AfrikanEchoesDbContext _context;

        public CategoryService(AfrikanEchoesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category GetById(long id)
        {
            return _context.Categories.Find(id);
        }
    }
}
