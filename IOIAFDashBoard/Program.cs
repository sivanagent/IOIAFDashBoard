using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOIAFDashBoard
{


    public class IOTNodeData
    {
        public string oimsId;
        public string strIotNodeId;
        public string sensorId;
        public string senValue;
        public string timeStamp;

        public IOTNodeData()
        {
            oimsId = new string('c', 20);
            strIotNodeId = new string('c', 20);
            sensorId = new string('c', 20);
            senValue = new string('c', 20);
            timeStamp = new string('c', 20);
        }

        public string OimsId
        {
            get { return oimsId; }
            set { oimsId = value; }
        }

        public string StrIotNodeId
        {
            get { return strIotNodeId; }
            set { strIotNodeId = value; }
        }

        public string SensorId
        {
            get { return sensorId; }
            set { sensorId = value; }
        }

        public string SenValue
        {
            get { return senValue; }
            set { senValue = value; }
        }
    }//end of IOTNodeData

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
            Application.Run(new IOIAFDashBoard());
        }
    }
}
