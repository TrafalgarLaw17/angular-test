using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Tools.Utils
{
    public static class Migrator
    {
        public static void RunMigrations(IServiceProvider serviceProvider, string[] args)
        {
            using var scope = serviceProvider.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            if (args.Length > 0)
            {
                var command = args[0].ToLower();
                switch (command)
                {
                    case "revert":
                        try
                        {
                            GenericLogger.LogRevertingMigration();
                            
                            var migrations = migrator.MigrationLoader.LoadMigrations();
                            if (migrations.Count > 1)
                            {
                                var previousMigrationVersion = migrations.Keys.OrderByDescending(v => v).Skip(1).First();
                                migrator.MigrateDown(previousMigrationVersion);
                                GenericLogger.LogRevertSuccess(previousMigrationVersion);
                            }
                            else
                            {
                                GenericLogger.LogRevertWarning();
                            }

                            Environment.Exit(0);
                        }
                        catch (Exception ex)
                        {
                            GenericLogger.LogRevertFailure(ex.Message);
                            Environment.Exit(1);
                        }
                        break;

                    case "delete":
                        try
                        {
                            GenericLogger.LogDeletingTables();
                            migrator.MigrateDown(0);
                            GenericLogger.LogDeleteSuccess();
                            Environment.Exit(0);
                        }
                        catch (Exception ex)
                        {
                            GenericLogger.LogDeleteFailure(ex.Message);
                            Environment.Exit(1);
                        }
                        break;
                }
            }
            else
            {
                try
                {
                    GenericLogger.LogMigrationStarted();
                    migrator.MigrateUp();
                    GenericLogger.LogMigrationSuccess();
                }
                catch (Exception ex)
                {
                    GenericLogger.LogMigrationFailure(ex.Message);
                }
            }
        }
    }
}
