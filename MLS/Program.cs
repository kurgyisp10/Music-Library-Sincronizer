using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Log/log.txt",
                              rollingInterval: RollingInterval.Day,
                              rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information("Begin Log");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*MusicDatabase.MusicBrainz.MusicBrainzSyncronizer MBS = new MusicDatabase.MusicBrainz.MusicBrainzSyncronizer();
            await MBS.authorize();*/
        }
    }
}
