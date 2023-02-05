using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class XmlForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        StreamReader xmlYolu;
        string xml, strXml, strXsl, strXsltYeni;

        public XmlForm()
        {
            InitializeComponent();


            string[] arguman = Environment.GetCommandLineArgs();

            foreach (string arg in arguman.Skip(1))
                xml = arg;

           

           // XmlOku();
          //  XmlGoruntule();
        }

        void XmlOku()
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

        private void btnYenile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XmlOku();
            XmlGoruntule();
        }

        private void ribbonControl_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                xml = s[0];
            }
        }

        private void ribbonControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;

            }
        }

        private void ribbonControl_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string filename in filenames)
                {
                    if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".XML")
                    {
                        
                    }
                }
            }

            else
            {
                dropEnabled = false;

            }

            if (!dropEnabled)
            {
                e.Effect = DragDropEffects.None;

               // xml = filename;
                XmlOku();
                XmlGoruntule();

            }

        }

        void XmlGoruntule()
        {
            var xsltMetin = Convert.FromBase64String(strXsl);
            strXsltYeni = Encoding.UTF8.GetString(xsltMetin);

            string ErrMsg = "";

            xmlYolu = new StreamReader(xml);
            strXml = xmlYolu.ReadToEnd();
            btnXmlYol.Caption = xml;


            string strResult = Transform(strXml, strXsltYeni, out ErrMsg);

            if (ErrMsg != "")
                XtraMessageBox.Show(ErrMsg);
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start(xml);
        }
    }
}