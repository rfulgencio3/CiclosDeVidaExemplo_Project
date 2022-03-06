using CiclosDeVida.API.Services;

namespace CiclosDeVida.API.Model
{
    public class CicloDeVida : ICicloDeVidaSingleton, ICicloDeVidaScoped, ICicloDeVidaTransient
    {
        public CicloDeVida()
        {
            CicloDeVidaId = Guid.NewGuid();
        }
        public Guid CicloDeVidaId { get; }
    }
}
