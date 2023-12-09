namespace VezeetaProject.Core.Dtos.Booking
{
    public class ResponeBookingDto
    {
    
      public byte[] image { get; set; }
      public string doctorName { get; set; }
      public string specialize { get; set; }
      public string day { get; set; }
      public string time { get; set; }
      public string discoundCode { get; set; }
      public decimal finalPrice { get; set; }
      public string status { get; set; }

        
    }
}
