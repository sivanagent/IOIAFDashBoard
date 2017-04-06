using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Security.Permissions;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Serialization;

namespace IOIAFDashBoard
{
    public partial class IOIAFDashBoard : Form
    {
        public iniFileReader mOIMSConfigFile;
        public string[] mListOfOIMSNode;
        List<IOTNodeData> mListofOIMSreturnedIOTNodeData = new List<IOTNodeData>();

        public IOIAFDashBoard()
        {
            InitializeComponent();

            //ioiafconfig.ini
            mOIMSConfigFile = new iniFileReader();
            mOIMSConfigFile.IniParser("d:\\ioiafconfig.ini");
            mListOfOIMSNode = mOIMSConfigFile.EnumSection("ListofOIMS");

        }

        private void AcquireOIMSData_Click(object sender, EventArgs e)
        {
            
            clearDashboard();

            string oimsNd = "10.1.173.147";
            //foreach (string oimsNd in mListOfOIMSNode)
            while (1!= 1)
            {
                string uriLink = "http://" + oimsNd + "/OIMS/OIMSService.svc";
                System.Uri uri = new Uri(uriLink);
                WebServiceInvoker wsInv = new WebServiceInvoker(uri);

                List<string> mthdList = new List<string>();
                mthdList = wsInv.EnumerateServiceMethods("OIMSServiceClient");

                for (int i = 0; i < mthdList.Count; i++)
                {
                    //Console.WriteLine(mthdList[i]);
                }

                string str = wsInv.InvokeMethod<string>("OIMSServiceClient", "GetData", new object[] { 32 });
                Console.WriteLine("===============================");
                Console.WriteLine(str);

                dynamic cmpTyp1 = new object();
                cmpTyp1 = wsInv.InvokeMethod<dynamic>("OIMSServiceClient", "GetDataUsingDataContract", null);

                Type str1 = cmpTyp1.GetType();

                Console.WriteLine("cmpTyp1.BoolValue=== " + cmpTyp1.BoolValue);
                Console.WriteLine("cmpTyp1.StringValue====" + cmpTyp1.StringValue);

                dynamic dyn_mListofOIMSreturnedIOTNodeData = new object();

                dyn_mListofOIMSreturnedIOTNodeData = wsInv.InvokeMethod<dynamic>("OIMSServiceClient", "GetIOTNodeData", null);
                /**
                System.Uri uri1 = new Uri("http://localhost/IOTNodesvc/IOTNodesvc.svc");
                WebServiceInvoker wsInv1 = new WebServiceInvoker(uri1);

                List<string> mthdList1 = new List<string>();
                mthdList1 = wsInv1.EnumerateServiceMethods("IOTNodesvcClient");
                dyn_mListofOIMSreturnedIOTNodeData = wsInv1.InvokeMethod<dynamic>("IOTNodesvcClient", "GetIoTNodeData", null);
                **/

                int x = dyn_mListofOIMSreturnedIOTNodeData.Length;
                int y = 0;

                while (x > y)
                {
                    //populate IOT--NodeDEtails 
                    IOTNodeData iotNdData = new IOTNodeData();

                    iotNdData.StrIotNodeId = dyn_mListofOIMSreturnedIOTNodeData[y].StrIoTNodeId;
                    iotNdData.SensorId = dyn_mListofOIMSreturnedIOTNodeData[y].SensorId;
                    iotNdData.SenValue = dyn_mListofOIMSreturnedIOTNodeData[y].SenValue;

                    mListofOIMSreturnedIOTNodeData.Add(iotNdData);

                    y++;
                }
            }

            fillDummyData("Oims-10.1.173.147");
            fillDummyData("Oims-10.1.173.141");
            populateDashboard();

        } // AcquireOIMSData_Click method..


        private void fillDummyData(string arg_oimsID)
        {
            int x = 10, y = 0;

            while (x > y)
            {
                for (int i = 0; i < 4; i++)
                {
                    //populate IOT--NodeDEtails 
                    IOTNodeData iotNdData = new IOTNodeData();
                    iotNdData.OimsId = arg_oimsID;
                    iotNdData.StrIotNodeId = "NODE-ID-" + Convert.ToString(y);
                    iotNdData.SensorId = "SensId" + Convert.ToString(i);
                    iotNdData.SenValue = Convert.ToString(i);

                    mListofOIMSreturnedIOTNodeData.Add(iotNdData);
                }

                y++;
            }
        } //end fillDummyData

        void clearDashboard()
        {
            mOIMSTreeView.Nodes.Clear();
        }

        void populateDashboard()
        {
            for (int i=0; i<  mListofOIMSreturnedIOTNodeData.Count; i++)
            {
                TreeNode treeOimsNode = new TreeNode(mListofOIMSreturnedIOTNodeData[i].OimsId);
                mOIMSTreeView.Nodes.Add(treeOimsNode);

                //now add all the IOT-Nodes of this OIMS..middleware..
                int count = i;
                bool firstTimeSeeingTheIOTNodeID = true;
                for (;;)
                {
                    
                    //check not at end of data buffer..
                    if (count >= mListofOIMSreturnedIOTNodeData.Count)
                    {
                        //reached end of buffer..
                        break;
                    }

                    //first add IOTNodeID under OIMS Id...
                    if (firstTimeSeeingTheIOTNodeID)
                    {
                        TreeNode childNode = new TreeNode(mListofOIMSreturnedIOTNodeData[count].strIotNodeId);
                        treeOimsNode.Nodes.Add(childNode);
                        firstTimeSeeingTheIOTNodeID = false;
                    }

                    //check if the next sensor also from the same IOT-node; if yes ...skip..
                    if ((count + 1 == mListofOIMSreturnedIOTNodeData.Count) || 
                        (String.Compare(mListofOIMSreturnedIOTNodeData[count].StrIotNodeId, mListofOIMSreturnedIOTNodeData[count+1].StrIotNodeId) ==0))
                    {
                        //skip the same IOT nodeID
                            count++;
                            continue;
                    }

                    firstTimeSeeingTheIOTNodeID = true;
                    if ((count + 1 == mListofOIMSreturnedIOTNodeData.Count) ||
                            (String.Compare(mListofOIMSreturnedIOTNodeData[count].OimsId, mListofOIMSreturnedIOTNodeData[count + 1].OimsId) != 0))
                    {
                        //do not skip; we have come to next OIMS servicer & its IOTNode's data..

                        break;
                    }
                    else
                    {
                        count++;
                    }
                }
                //reached end of one OIMS sensor list data..now go on to next OIMS data buffer array..
                i = count;

            }//end of for loop

        }//end of populate-dashboard function
    }
}


