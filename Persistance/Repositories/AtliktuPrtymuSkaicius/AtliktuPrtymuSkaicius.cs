using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.AtliktuPrtymuSkaicius
{
    public class AtliktuPrtymuSkaicius: IAtliktuPrtymuSkaicius
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO AtliktuPratymuSkaicius(TreniruotesId, StatistikosId, AtpazyntoPratymoId, Priejimas, Skaicius) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')";

        public AtliktuPrtymuSkaicius(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(Guid TreniruotesId, Guid StatistikosId, Guid AtpazyntoPratymoId, int Priejimas, int Skaicius)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(_insertQueryString, TreniruotesId, StatistikosId, AtpazyntoPratymoId, Priejimas, Skaicius);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

    }
}
