
using VezeetaProject.Core.Repository;
using VezeetaProject.Core;
using VezeetaProject.Core.Services;

namespace VezeetaProject.Core
{
    public interface IUnitOfWork:IDisposable
    {
        IBaseRepository<Specialization> Specializations { get; }
        IBaseRepository<Booking> Bookings { get; }
        IBaseRepository<Discount> Discounts { get; }
        IBaseRepository<DoctorAppointment> DoctorAppointments { get; }
        IBaseRepository<AppointmentTimes> AppointmentTimes { get; }
        IBaseRepository<Doctor> doctors { get; }
        IBaseRepository<ApplicationUser> applicationUser { get; }
        IDatabaseTransaction BeginTransaction();
        void Commit();
    }
}
