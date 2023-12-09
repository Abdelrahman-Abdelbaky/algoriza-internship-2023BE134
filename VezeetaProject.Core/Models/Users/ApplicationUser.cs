namespace VezeetaProject.Core.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Byte[]? Image { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<Booking> Bookings { get; set; }

        public Doctor Doctor { get; set; }

        public DateTime TimeStamp { get; set; }



     
    }
}