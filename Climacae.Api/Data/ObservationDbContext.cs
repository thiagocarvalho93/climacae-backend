using Climacae.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Climacae.Api.Data;

public class ObservationDbContext(DbContextOptions<ObservationDbContext> options) : DbContext(options)
{
    public DbSet<ObservationModel> Observations { get; set; }
}