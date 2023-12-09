
namespace VezeetaProject.Core.Services
{
  
        public interface IDatabaseTransaction : IDisposable
        {
            void Commit();

            void Rollback();
        }
    
}
