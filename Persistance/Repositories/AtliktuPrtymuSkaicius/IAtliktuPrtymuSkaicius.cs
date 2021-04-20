using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.AtliktuPrtymuSkaicius
{
    public interface IAtliktuPrtymuSkaicius
    {
        public Task<Guid> Insert(Guid TreniruotesId, Guid StatistikosId, Guid AtpazyntoPratymoId, int Priejimas, int Skaicius);
    }
}
