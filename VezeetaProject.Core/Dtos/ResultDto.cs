namespace VezeetaProject.Core.Dtos
{
    public class ResultDto<T> where T : class
    {
        public bool IsDone { get; set;}
        public string ErrorMassage { get; set; }
        public T Object { get; set; }
    }
}
