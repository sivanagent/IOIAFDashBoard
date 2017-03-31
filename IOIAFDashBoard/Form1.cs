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


    public class SensorData
    {
        public string sensorId = new string('c', 20);
        public string value = new string('c', 20);
    }

    public class IOTNodeData
    {
        public bool boolValue = true;
        public string strIOTNodeId = "Hello ";
        public SensorData[] sensorData;
        public IOTNodeData()
        {
            sensorData = new SensorData[4];
            sensorData[0] = new SensorData();
            sensorData[1] = new SensorData();
            sensorData[2] = new SensorData();
            sensorData[3] = new SensorData();

        }

        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }
        public string StrIOTNodeId
        {
            get { return strIOTNodeId; }
            set { strIOTNodeId = value; }
        }
    }

    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }


    public partial class IOIAFDashBoard : Form
    {
        public IOIAFDashBoard()
        {
            InitializeComponent();
        }

        private void AcquireOIMSData_Click(object sender, EventArgs e)
        {
            System.Uri uri= new Uri ("http://localhost/OIMS/OIMSService.svc");
            WebServiceInvoker wsInv = new WebServiceInvoker(uri);

            List<string> mthdList = new List<string>();
            mthdList = wsInv.EnumerateServiceMethods("OIMSServiceClient");

            for (int i=0; i< mthdList.Count; i++)
            Console.WriteLine(mthdList[i]);

            string str = wsInv.InvokeMethod<string>("OIMSServiceClient", "GetData", new object[] { 32 });

            Console.WriteLine("===============================");
            Console.WriteLine("===============================");
            Console.WriteLine("===============================");

            Console.WriteLine(str);

            CompositeType cmpTyp = new CompositeType();
            cmpTyp.BoolValue = true;
            cmpTyp.StringValue = "---venkat";

            object[] objListPtr = new object[1];
            objListPtr[0] = cmpTyp;
            dynamic cmpTyp1 = new object(); 
            cmpTyp1= wsInv.InvokeMethod<dynamic>("OIMSServiceClient", "GetDataUsingDataContract", null); // objListPtr);

            //var type = typeof(cmpTyp1);


            Console.WriteLine("=======comp type=====================");
            Console.WriteLine("=======comp type=====================");
            Console.WriteLine("=======comp type=====================");

            Console.WriteLine("cmpTyp1.BoolValue=== " + cmpTyp1.BoolValue);
            Console.WriteLine("cmpTyp1.StringValue====" + cmpTyp1.StringValue);

            dynamic cmpTyp2 = new object (); 
            cmpTyp2 = wsInv.InvokeMethod<dynamic>("OIMSServiceClient", "GetIOTNodeData", null);

            //cmpType2 is of IOTNodeData type..
            Console.WriteLine("cmpTyp2.iot-node-ID === " + cmpTyp2[0].strIOTNodeId);
            Console.WriteLine("cmpTyp2. sensor Data ID === " + cmpTyp2[0].sensorData[0].sensorId);
            Console.WriteLine("cmpTyp2. sensor Data value === " + cmpTyp2[0].sensorData[0].value);


        } // AcquireOIMSData_Click method..
    }
}



/**
 {
String uriStr = "http://" + "192.168.1.36" + "/IOTNodesvc/IOTNodesvc.svc";
System.Uri uri1 = new Uri(uriStr);

//create webservice invoker...
WebServiceInvoker wsInv1 = new WebServiceInvoker(uri1);

//enumerate webservice methods...
List<string> mthdList1 = new List<string>();
mthdList1 = wsInv1.EnumerateServiceMethods("IOTNodesvcClient");

//...print webservice methods of IOTNodesvc

//for (int i = 0; i < mthdList1.Count ;  i++) {
 //   Console.WriteLine(mthdList[i]);
//}

//invoke IOT-node websvc to gather data..
dynamic cmpTyp2 = new object();
cmpTyp2 = wsInv1.InvokeMethod<dynamic>("IOTNodesvcClient", "GetIotData", null);

***/


/***
            var list = new List<IOTNodeData>();
            string[] ipAddrList = { "localhost", "localhost" };
            System.Net.WebClient client = new System.Net.WebClient();

            for (int i = 0; i < 2; i++)
            {
                string url = "http://" + ipAddrList[i] + ":12926/OIMSService.svc?wsdl";
                //:10814/IOTNodesvc.svc?wsdl
                //System.IO.Stream stream = client.OpenRead("http://192.168.137.230/add1.asmx?wsdl");

                System.IO.Stream stream = client.OpenRead(url);
                // Get a WSDL file describing a service.

                ServiceDescription description = ServiceDescription.Read(stream);

                // Initialize a service description importer.

                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                importer.ProtocolName = "Soap12";  // Use SOAP 1.2.
                importer.AddServiceDescription(description, null, null);


                // Report on the service descriptions.
                MessageBox.Show("Importing { 0} service descriptions with { 1} associated schemas." + importer.ServiceDescriptions.Count + importer.Schemas.Count);

                // Generate a proxy client.
                importer.Style = ServiceDescriptionImportStyle.Client;


                // Generate properties to represent primitive values.
                importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;

                // Initialize a Code-DOM tree into which we will import the service.
                CodeNamespace nmspace = new CodeNamespace();
                CodeCompileUnit unit1 = new CodeCompileUnit();

                unit1.Namespaces.Add(nmspace);

                // Import the service into the Code-DOM tree. This creates proxy code
                // that uses the service.

                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit1);
                if (warning == 0)
                {
                    // Generate and print the proxy code in C#.
                    CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");
                    // Compile the assembly with the appropriate references
                    string[] assemblyReferences = new string[2] { "System.Web.Services.dll", "System.Xml.dll" };
                    CompilerParameters parms = new CompilerParameters(assemblyReferences);
                    CompilerResults results = provider1.CompileAssemblyFromDom(parms, unit1);
                    foreach (CompilerError oops in results.Errors)
                    {
                        MessageBox.Show("======== Compiler error ============");
                        MessageBox.Show(oops.ErrorText);
                    }
                    //Invoke the web service method
                    object objWS = results.CompiledAssembly.CreateInstance("OIMSService");
                    Type t = objWS.GetType();
                    //Console.WriteLine(t.InvokeMember("HelloWorld", System.Reflection.BindingFlags.InvokeMethod, null, objWS, null //i/p object//));
                    //MessageBox.Show("add  value is   " + t.InvokeMember("GetIotData", System.Reflection.BindingFlags.InvokeMethod, null, objWS, new Object[] { 32, 32 }));

                    object objData = t.InvokeMember("GetIOTNodeData", System.Reflection.BindingFlags.InvokeMethod, null, objWS, null);

                    IOTNodeData iotNodeData = (IOTNodeData)objData;


                    Console.WriteLine(iotNodeData.boolValue);
                    Console.WriteLine(iotNodeData.strIOTNodeId);

                }
                else
                {
                    // Print an error message.
                    Console.WriteLine("Warning: " + warning);
                }

            }//end  of  loop
}
****/
