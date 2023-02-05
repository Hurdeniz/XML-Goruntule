using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XMLGoruntule
{
    public partial class Form1 : Form
    {
        StreamReader xmlYolu;
        string xml,strXml,strXsl,strXsltYeni;

        public Form1()
        {
            InitializeComponent();

            string[] arguman = Environment.GetCommandLineArgs();

            foreach (string arg in arguman.Skip(1))
                xml = arg;

            okuma();
            deco();

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            
        }
     
        private void button2_Click(object sender, EventArgs e)
        {
      
        }

        void okuma()
        {
            string xmlAdress = xml;

            XmlTextReader XMLDosyam = new XmlTextReader(xmlAdress);

            while (XMLDosyam.Read())
            {
                if (XMLDosyam.NodeType == XmlNodeType.Element && XMLDosyam.Name == "cbc:EmbeddedDocumentBinaryObject")
                {
                    XMLDosyam.Read();
                    strXsl = XMLDosyam.Value;

                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        void deco ()
        {
            var metin = Convert.FromBase64String(strXsl);
            strXsltYeni = Encoding.UTF8.GetString(metin);

            string ErrMsg = "";

            xmlYolu = new StreamReader(xml);
            strXml = xmlYolu.ReadToEnd();


            string strResult = Transform(strXml, strXsltYeni, out ErrMsg);

            if (ErrMsg != "")
                MessageBox.Show(ErrMsg);
            else
            {
                webBrowser1.DocumentText = strResult;

                xmlYolu.Dispose();


            }
        }

     


        private string Transform(string XMLPage, string XSLStylesheet, out string ErrorMessage)
        {
           

            string result = "";
            ErrorMessage = "";
            try
            {
                // Reading XML
                TextReader textReader1 = new StringReader(XMLPage);
                XmlTextReader xmlTextReader1 = new XmlTextReader(textReader1);
                XPathDocument xPathDocument = new XPathDocument(xmlTextReader1);

                //Reading XSLT
                TextReader textReader2 = new StringReader(XSLStylesheet);
                XmlTextReader xmlTextReader2 = new XmlTextReader(textReader2);


                //Define XslCompiledTransform
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xmlTextReader2);


                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);

                xslt.Transform(xPathDocument, null, tw);


                result = sb.ToString();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
