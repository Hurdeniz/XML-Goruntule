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
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;

namespace XMLGoruntule
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        StreamReader xmlYolu, xsltYolu;
        string strXml, strXslt;
        string strXmlYeni, strXsltYeni;


        public MainForm()
        {
            InitializeComponent();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGoruntule_Click(object sender, EventArgs e)
        {
            if (btnEditXml.Text == "" && btnEditXslt.Text == "")
            {
                XtraMessageBox.Show("Lütfen XML ve XSLT Dosyası Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (btnEditXml.Text == "")
            {
                XtraMessageBox.Show("Lütfen XML Dosyası Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (btnEditXslt.Text == "")
            {
                XtraMessageBox.Show("Lütfen XSLT Dosyası Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string ErrMsg = "";

                xmlYolu = new StreamReader(strXmlYeni.ToString());
                strXml = xmlYolu.ReadToEnd();

                xsltYolu = new StreamReader(strXsltYeni.ToString());
                strXslt = xsltYolu.ReadToEnd();

                string strResult = Transform(strXml, strXslt, out ErrMsg);

                if (ErrMsg != "")
                    MessageBox.Show(ErrMsg);
                else
                {
                    webBrowser1.DocumentText = strResult;
                    xmlYolu.Dispose();
                    xsltYolu.Dispose();

                }
            }
        }

        private void buttonEdit1_Properties_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {


        }

        private void btnEditXslt_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit btnXslt = (ButtonEdit)sender;
            int buttonindex = btnXslt.Properties.Buttons.IndexOf(e.Button);
            if (buttonindex == 0)
            {

                var dialog = new XtraOpenFileDialog()
                {
                    Title = "XSLT Seç",
                    Filter = "XSLT Dosyaları(*.xslt)| *.xslt;",
                    UseParentFormIcon = false,
                };


                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    xsltYolu = new StreamReader(dialog.FileName);
                    btnEditXslt.Text = dialog.FileName;
                    strXsltYeni = dialog.FileName;
                }
            }
            else if (buttonindex == 1)
            {
                if (btnEditXslt.Text != "")
                    Process.Start(btnEditXslt.Text);
                else
                    XtraMessageBox.Show("Lütfen XSLT Dosyası Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditXml_DragEnter(object sender, DragEventArgs e)
        {

            //Mousenin btnEdit üzerine geldiğinde yapmak istediğini buluyoruz.Geçiyor mu Dosya mı Bırakıyor.Dosya Bırakıyorsa Sonuç True

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void btnEditXml_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                btnEditXml.Text = s[0];
                strXmlYeni = s[0];

            }
        }


        private void btnEditXslt_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                btnEditXslt.Text = s[0];
                strXsltYeni = s[0];
            }
        }

        private void btnEditXslt_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;

            }
        }

        private void btnEditXml_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string filename in filenames)
                {
                    if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".XML")
                    {
                        dropEnabled = false;
                        break;
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
                
            }

        }

        private void btnEditXslt_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string filename in filenames)
                {
                    if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".XSLT")
                    {
                        dropEnabled = false;
                        break;
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

            }
        }

        private void btnGoruntule_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnGoruntule.PerformClick();
            }
        }

        //belge okumak istersek
        //private string BelgeyiOku(string dosya_yolu)
        //{
        //    StreamReader dosyaOku = new StreamReader(dosya_yolu, Encoding.GetEncoding("windows-1254"));
        //    string yazi = dosyaOku.ReadLine();
        //    while (yazi != null)
        //    {
        //        btnEditXml.Text += (yazi) + Environment.NewLine;
        //        yazi = dosyaOku.ReadLine();
        //    }
        //    dosyaOku.Close();
        //    return yazi;
        //}

        private void btnEditXml_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {


        }

        private void btnEditXml_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit btnXml = (ButtonEdit)sender;
            int buttonindex = btnXml.Properties.Buttons.IndexOf(e.Button);
            if (buttonindex == 0)
            {
                var dialog = new XtraOpenFileDialog()
                {
                    Title = "XML Seç",
                    Filter = "XML Dosyaları(*.xml)| *.xml;",
                    UseParentFormIcon = false,
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    xmlYolu = new StreamReader(dialog.FileName);
                    btnEditXml.Text = dialog.FileName;
                    strXmlYeni = dialog.FileName;
                }

            }
            else if (buttonindex == 1)
            {
                if (btnEditXml.Text != "")
                    Process.Start(btnEditXml.Text);
                else
                    XtraMessageBox.Show("Lütfen XLM Dosyası Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
