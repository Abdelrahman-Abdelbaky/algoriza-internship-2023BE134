using VezeetaProject.Core.Models.Users;
using VezeetaProject.Core.Repository;
using VezeetaProject.Core.Services;
using VezeetaProject.EF.Repository;

namespace VezeetaProject.EF
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext Context)
        {
           _context = Context;
            Specializations = new RepositoryItem<Specialization>(_context);
            Bookings = new RepositoryItem<Booking>(_context);
            Discounts = new RepositoryItem<Discount>(_context);
            DoctorAppointments = new RepositoryItem<DoctorAppointment>(_context);
            doctors = new RepositoryItem<Doctor>(_context);
            applicationUser = new RepositoryItem<ApplicationUser>(_context);
            AppointmentTimes = new RepositoryItem<AppointmentTimes>(_context);
        }

        public IBaseRepository<Specialization> Specializations { get; }
        public IBaseRepository<Booking> Bookings { get; }
        public IBaseRepository<Discount> Discounts { get; }
        public IBaseRepository<DoctorAppointment> DoctorAppointments { get; }
        public IBaseRepository<Doctor> doctors { get; }
        public IBaseRepository<ApplicationUser> applicationUser { get; }

        public IBaseRepository<AppointmentTimes> AppointmentTimes { get; }

        public IDatabaseTransaction BeginTransaction()
        {
           return new DatabaseTransaction(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

       
    }
}
