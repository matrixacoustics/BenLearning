using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;



namespace BenLearning.Models
{
    class ApplicationDbContext : DbContext
    {


        #region CONTEXT


        public ApplicationDbContext()
            : base("DefaultConnection")
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public override int SaveChanges()
        //{
        //    //TrackSavedDates();
        //    return base.SaveChanges();
        //}

        //public override async Task<int> SaveChangesAsync()
        //{
        //    //TrackSavedDates();
        //    return await base.SaveChangesAsync();
        //}

        #endregion

        #region TABLES
        public virtual DbSet<LoggerInfo> LoggerInfoTable { get; set; }

        public virtual DbSet<NoiseMeasurement> NoiseMeasurements { get; set; }
        public virtual DbSet<NoiseMeasurementDetail> NoiseMeasurementDetails { get; set; }
        #endregion
    }
}
