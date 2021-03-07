using Models.dto;
using Models.Models;
using Models.Models.Statistic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Statistika
{
    public class StatistikaRepo : IStatistikaRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Statistika (StatistikosId, TreniruotesPradzia, TreniruotesPabaiga, VartotojoId) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _deleteQueryString = "DELETE FROM Statistika WHERE StatistikosId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Statistika";
        private readonly string _getUserGeneralStatistics = "SELECT * FROM Statistika as s WHERE s.VartotojoId = '{0}'";

        private readonly string _updateQueryString =
            "UPDATE Statistika SET TreniruotesPabaiga='{0}' WHERE StatistikosId='{1}'";

        public StatistikaRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string VartotojoId)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, SukurimoData, SukurimoData, VartotojoId);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<StatistikaDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<StatistikaDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new StatistikaDo
            {
                StatistikosId = new Guid(d.StatistikosId),
                TreniruotesPradzia = DateTime.Parse(d.TreniruotesPradzia),
                TreniruotesPabaiga = DateTime.Parse(d.TreniruotesPabaiga),
                VartotojoId = new Guid(d.VartotojoId)
            });

            return resultTask;
        }

        private async Task<StatistikaDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var StatistikosId = await reader.GetFieldValueAsync<string>("StatistikosId");
            var TreniruotesPradzia = await reader.GetFieldValueAsync<DateTime>("TreniruotesPradzia");
            var TreniruotesPabaiga = await reader.GetFieldValueAsync<DateTime>("TreniruotesPabaiga");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");

            return new StatistikaDto
            {
                StatistikosId = StatistikosId,
                TreniruotesPradzia = TreniruotesPradzia.ToString(),
                TreniruotesPabaiga = TreniruotesPabaiga.ToString(),
                VartotojoId = VartotojoId
            };
        }

        public async Task Update(Guid id, string baigimoData) //galima nedet baigimo laiko, o paimti call system time
        {
            var queryString = string.Format(_updateQueryString, baigimoData, id);

            await _sqlClient.ExecuteNonQuery(queryString);
        }

        public async Task<StatisticGeneralDo> GetUserGeneralStatistic(string VartotojoId)
        {
            var resultTask = new StatisticGeneralDo();
            var getAllQuery = string.Format(_getUserGeneralStatistics, VartotojoId);

            List<string> chartLabel = new List<string>(); 

            var result = await _sqlClient.ExecuteQueryList<StatistikaDto>(getAllQuery, Func);

            double temp = 0;
            int count = 0;
            foreach(var d in result)
            {
                chartLabel.Add(d.TreniruotesPradzia);
                var start = DateTime.Parse(d.TreniruotesPradzia);
                var pabaiga = DateTime.Parse(d.TreniruotesPabaiga);
                var skirt = pabaiga - start;
                temp += skirt.TotalHours;
                count++;
            }
            var tempTimeMean = temp / count;
            resultTask.meanTime = Math.Round(tempTimeMean, 2);
            

            //reikalaujama per treniruote
            //esama
            //max
            //min


            resultTask.meanCount = 15;
            resultTask.chartLabels = chartLabel;

            return resultTask;
        }

        private async Task<StatistikaDto> Func2(SqlDataReader reader) //pagalbine fnkc
        {
            var StatistikosId = await reader.GetFieldValueAsync<string>("StatistikosId");
            var TreniruotesPradzia = await reader.GetFieldValueAsync<DateTime>("TreniruotesPradzia");
            var TreniruotesPabaiga = await reader.GetFieldValueAsync<DateTime>("TreniruotesPabaiga");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");

            return new StatistikaDto
            {
                StatistikosId = StatistikosId,
                TreniruotesPradzia = TreniruotesPradzia.ToString(),
                TreniruotesPabaiga = TreniruotesPabaiga.ToString(),
                VartotojoId = VartotojoId
            };
        }
    }
}
