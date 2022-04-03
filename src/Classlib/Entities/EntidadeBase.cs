
namespace Classlib.Entities
{
    public abstract class EntidadeBase
    {
        public string Id { get; private set; }

        protected EntidadeBase(string Id) => this.Id = Id;
        
    }
}