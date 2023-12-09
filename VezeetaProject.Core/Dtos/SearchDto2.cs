namespace VezeetaProject.Core.Dtos
{
    public class SearchDto2
    {
        [Required]
        public int page { get; set; }
        [Required]
        public int pageSize { get; set; }
        [Required]
        public Days Day { get; set; }
    }
}
