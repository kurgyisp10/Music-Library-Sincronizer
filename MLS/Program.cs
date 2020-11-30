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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            /*MusicDatabase.MusicBrainz.MusicBrainzSyncronizer MBS = new MusicDatabase.MusicBrainz.MusicBrainzSyncronizer();
            MBS.authorize();*/
        }
    }
}
