using MvcMovie.Models;

namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcMovie.Models.MovieDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcMovie.Models.MovieDbContext context)
        {
            context.Movies.AddOrUpdate(i => i.Title,
                                       new Movie
                                           {
                                               Title = "When Harry Met Sally",
                                               ReleaseDate = DateTime.Parse("1989-1-11"),
                                               Genre = "Romantic Comedy",
                                               Rating = "G",
                                               Price = 7.99M
                                           },

                                       new Movie
                                           {
                                               Title = "Ghostbusters ",
                                               ReleaseDate = DateTime.Parse("1984-3-13"),
                                               Genre = "Comedy",
                                               Price = 8.99M
                                           },

                                       new Movie
                                           {
                                               Title = "Ghostbusters 2",
                                               ReleaseDate = DateTime.Parse("1986-2-23"),
                                               Genre = "Comedy",
                                               Price = 9.99M
                                           },

                                       new Movie
                                           {
                                               Title = "Rio Bravo",
                                               ReleaseDate = DateTime.Parse("1959-4-15"),
                                               Genre = "Western",
                                               Price = 3.99M
                                           });
        }
    }
}