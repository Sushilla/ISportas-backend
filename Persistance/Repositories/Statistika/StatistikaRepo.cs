﻿using Models.Classes;
using Models.dto;
using Models.dto.Statistics;
using Models.Models;
using Models.Models.Statistic;
using Persistance.Repositories.AtliktuPrtymuSkaicius;
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
        private readonly IAtliktuPrtymuSkaicius _AtliktuPrtymuSkaicius;

        private readonly string _insertQueryString = "INSERT INTO Statistika (StatistikosId, TreniruotesPradzia, TreniruotesPabaiga, VartotojoId) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _deleteQueryString = "DELETE FROM Statistika WHERE StatistikosId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Statistika";
        private readonly string _getUserGeneralStatistics = "SELECT * FROM Statistika as s WHERE s.VartotojoId = '{0}'";
        private readonly string _getUserMadedStat = "SELECT s.StatistikosId, p.Priejimai * p.Skaicius as ReikiaPadaryti, a.Skaicius as Padare FROM Statistika as s, AtliktuPratymuSkaicius as a, PratymuSkaicius as p WHERE s.VartotojoId = '{0}' AND s.StatistikosId=a.StatistikosId AND p.PratymoId=a.AtpazyntoPratymoId and p.TreniruotesId=a.TreniruotesId";
        private readonly string _getUserMadedStatForSelectedWorkout = "SELECT s.StatistikosId, p.Skaicius as ReikiaPadaryti, a.Skaicius as Padare FROM Statistika as s, AtliktuPratymuSkaicius as a, PratymuSkaicius as p WHERE s.VartotojoId = '{0}' AND s.StatistikosId=a.StatistikosId AND p.PratymoId=a.AtpazyntoPratymoId AND p.TreniruotesId=a.TreniruotesId AND a.TreniruotesId='{1}'";
        private readonly string _updateQueryString =
            "UPDATE Statistika SET TreniruotesPabaiga='{0}' WHERE StatistikosId='{1}'";

        public StatistikaRepo(ISqlClient sqlclient, IAtliktuPrtymuSkaicius iAtliktuPrtymuSkaicius)
        {
            _sqlClient = sqlclient;
            _AtliktuPrtymuSkaicius = iAtliktuPrtymuSkaicius;
        }

        public async Task<Guid> Insert(string VartotojoId)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            var insertQuery = string.Format(_insertQueryString, id, SukurimoData, SukurimoData, VartotojoId);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }
        public async Task Update(Guid id, IEnumerable<AtliktiPrat> prat)
        {
            var baigimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            var queryString = string.Format(_updateQueryString, baigimoData, id);

            await _sqlClient.ExecuteNonQuery(queryString);

            foreach(var exercise in prat){
                await _AtliktuPrtymuSkaicius.Insert(exercise.TreniruotesId, exercise.StatistikosId, exercise.AtpazyntoPratymoId, exercise.Priejimas, exercise.Skaicius);
            }
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

        public async Task<StatisticGeneralDo> GetUserGeneralStatistic(string VartotojoId)
        {
            var resultTask = new StatisticGeneralDo();
            var getAllQuery = string.Format(_getUserGeneralStatistics, VartotojoId);
            var getAllQuery2 = string.Format(_getUserMadedStat, VartotojoId);

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
            List<int> goalEx = new List<int>();
            List<int> userEx= new List<int>();
            List<int> maxEx = new List<int>();
            List<int> minEx = new List<int>();



            List<string> tempArr = new List<string>();

            var userMadeStat = await _sqlClient.ExecuteQueryList<UserExerciseCountDto>(getAllQuery2, Func2);

            foreach (var u in userMadeStat)
            {
                if (tempArr.Any())
                {
                    int index = tempArr.FindIndex(x => x == u.StatistikosId);
                    if (index == -1)//jei nerado
                    {
                        tempArr.Add(u.StatistikosId);
                        goalEx.Add(u.ReikiaPadaryti);
                        userEx.Add(u.Padare);
                        maxEx.Add(u.Padare);
                        minEx.Add(u.Padare);
                    }
                    else //rado
                    {
                        goalEx[index] += u.ReikiaPadaryti;
                        userEx[index] += u.Padare;
                        if(maxEx[index] < u.Padare)
                        {
                            maxEx[index] = u.Padare;
                        }

                        if(minEx[index] > u.Padare)
                        {
                            minEx[index] = u.Padare;
                        }
                    }
                }
                else
                {
                    tempArr.Add(u.StatistikosId);
                    goalEx.Add(u.ReikiaPadaryti);
                    userEx.Add(u.Padare);
                    maxEx.Add(u.Padare);
                    minEx.Add(u.Padare);
                }
                
            }

            tableDataa[] tblData = new tableDataa[4];
            tblData[0] = new tableDataa();
            tblData[1] = new tableDataa();
            tblData[2] = new tableDataa();
            tblData[3] = new tableDataa();
            tblData[0].label = "Workout goal";
            tblData[0].data = goalEx;
            tblData[1].label = "User's workout exercise count";
            tblData[1].data = userEx;
            tblData[2].label = "Max count per exercise";
            tblData[2].data = maxEx;
            tblData[3].label = "Min count per exercise";
            tblData[3].data = minEx;

            double tempCount = 0;
            foreach(var a in userEx)
            {
                tempCount += a;
            }

            var avg = tempCount / count;
            resultTask.meanCount = Math.Round(avg, 2);
            resultTask.chartLabels = chartLabel;
            resultTask.dataForTable = tblData;

            return resultTask;
        }

        private async Task<UserExerciseCountDto> Func2(SqlDataReader reader) //pagalbine fnkc
        {
            var StatistikosId = await reader.GetFieldValueAsync<string>("StatistikosId");
            var reikiaPadaryti = await reader.GetFieldValueAsync<int>("ReikiaPadaryti");
            var padare = await reader.GetFieldValueAsync<int>("Padare");

            return new UserExerciseCountDto
            {
                StatistikosId = StatistikosId,
                ReikiaPadaryti = reikiaPadaryti,
                Padare = padare
            };
        }

        public async Task<StatisticGeneralDo2> GetUserlStatisticForTrainer(string VartotojoId, string WorkoutId)
        {
            var resultTask = new StatisticGeneralDo2();
            var getAllQuery = string.Format(_getUserGeneralStatistics, VartotojoId);
            var getAllQuery2 = string.Format(_getUserMadedStatForSelectedWorkout, VartotojoId, WorkoutId);

            List<string> chartLabel = new List<string>();

            var result = await _sqlClient.ExecuteQueryList<StatistikaDto>(getAllQuery, Func);


            List<double> averageTime = new List<double>();
            double temp = 0;
            int count = 0;
            foreach (var d in result)
            {
                chartLabel.Add(d.TreniruotesPradzia);
                var start = DateTime.Parse(d.TreniruotesPradzia);
                var pabaiga = DateTime.Parse(d.TreniruotesPabaiga);
                var skirt = pabaiga - start;
                temp += skirt.TotalHours;
                count++;
                averageTime.Add(Math.Round(temp / count, 2));
            }


            tableDataa2[] tblData2 = new tableDataa2[1];
            tblData2[0] = new tableDataa2();
            tblData2[0].label = "User's average training time";
            tblData2[0].data = averageTime;


            var tempTimeMean = temp / count;
            List<int> goalEx = new List<int>();
            List<int> userEx = new List<int>();

            List<string> tempArr = new List<string>();

            var userMadeStat = await _sqlClient.ExecuteQueryList<UserExerciseCountDto>(getAllQuery2, Func2);

            foreach (var u in userMadeStat)
            {
                if (tempArr.Any())
                {
                    int index = tempArr.FindIndex(x => x == u.StatistikosId);
                    if (index == -1)//jei nerado
                    {
                        tempArr.Add(u.StatistikosId);
                        goalEx.Add(u.ReikiaPadaryti);
                        userEx.Add(u.Padare);
                    }
                    else //rado
                    {
                        goalEx[index] += u.ReikiaPadaryti;
                        userEx[index] += u.Padare;
                    }
                }
                else
                {
                    tempArr.Add(u.StatistikosId);
                    goalEx.Add(u.ReikiaPadaryti);
                    userEx.Add(u.Padare);
                }

            }

            tableDataa[] tblData = new tableDataa[2];
            tblData[0] = new tableDataa();
            tblData[1] = new tableDataa();
            tblData[0].label = "Workout goal";
            tblData[0].data = goalEx;
            tblData[1].label = "User's workout exercise count";
            tblData[1].data = userEx;


            /*double tempCount = 0;
            foreach (var a in userEx)
            {
                tempCount += a;
            }

            var avg = tempCount / count;
            resultTask.meanTime = Math.Round(tempTimeMean, 2);

            resultTask.meanCount = Math.Round(avg, 2);*/
            resultTask.chartLabels = chartLabel;
            resultTask.dataForTable = tblData;
            resultTask.dataForTable2 = tblData2;

            return resultTask;
        }
    }
}
