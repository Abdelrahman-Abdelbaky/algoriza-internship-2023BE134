namespace VezeetaProject.Core.Dtos
{
    public class SearchDto
    {
        [Required]
        public int page { get; set; }
        [Required]
        public int pageSize { get; set; }
        [Required]
        public string search { get; set; }
    }
}
