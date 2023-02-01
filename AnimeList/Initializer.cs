﻿using AnimeList.DAL.Interfaces;
using AnimeList.DAL.Repository;
using AnimeList.DAL.UnitOfWork;
using AnimeList.Services.Interfaces;
using AnimeList.Services.Services;

namespace AnimeList
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAnimeService, AnimeService>();
        }
    }
}