using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using EFCore.Seeder.Configuration;
using EFCore.Seeder.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Cortside.WebApiStarter.WebApi.IntegrationTests.Helpers {
    public static class DbSetExtensions {

        /// <summary>
        /// Seeds a DBSet from a CSV file that will be read from the specified stream
        /// </summary>
        /// <typeparam name="T">The type of entity to load</typeparam>
        /// <param name="dbSet">The DbSet to populate</param>
        /// <param name="stream">The stream containing the CSV file contents</param>
        /// <param name="additionalMapping">Any additonal complex mappings required</param>
        public static List<T> SeedFromFile<T>(this DbSet<T> dbSet, string filename, params CsvColumnMapping<T>[] additionalMapping)
            where T : class {
            try {
                using (var reader = new StreamReader(filename)) {
                    var results = new List<T>();

                    var csvReader = new CsvReader(reader, SeederConfiguration.CsvConfiguration);
                    var map = csvReader.Configuration.AutoMap<T>();
                    map.ReferenceMaps.Clear();
                    csvReader.Configuration.RegisterClassMap(map);

                    while (csvReader.Read()) {
                        var csvEntity = csvReader.GetRecord<T>();
                        foreach (var csvColumnMapping in additionalMapping) {
                            csvColumnMapping.Execute(csvEntity, csvReader.GetField(csvColumnMapping.CsvColumnName));
                        }

                        SeederConfiguration.DbSetHelper.AddOrUpdate(dbSet, csvEntity);

                        results.Add(csvEntity);
                    }

                    return results;
                }
            } catch (Exception exception) {
                var message = $"Error Seeding DbSet<{typeof(T).Name}>: {exception}";
                throw new Exception(message, exception);
            }
        }
    }
}
