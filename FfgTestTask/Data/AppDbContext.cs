﻿using FfgTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace FfgTestTask.Data
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Таблица с данными
        /// </summary>
        public DbSet<DataRow> DataTable { get; set; }

        /// <summary>
        /// Таблица с логами
        /// </summary>
        public DbSet<LogRow> Log { get; set; }
    }
}
