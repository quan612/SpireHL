using CatalogModule.Contracts;
using SpireHL.Core.Extensions;
using SpireHL.Core.Repository;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf.Barcode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace CatalogModule.Services.Word
{
    public abstract class BaseWordService : IWordCatalogService
    {
        private const string CatalogExportPathParams = "CatalogExportPath";
        private const string BarCodeFieldName = "Barcode";
        private const string ProductImageFieldName = "ProductImage";

        protected string ProjectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Templates\";

        public WordDocument Document { get; set; }
        public DataTable DataTableItems { get; set; }

        public abstract string SourceTemplate { get; }
        public abstract string ExportTemplate { get; }

        public IUserDefine UserDefine { get; }

        public BaseWordService()
        {
            Document = new WordDocument(SourceTemplate);
            Document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(InsertBarcode);
            Document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MergeField_ProductImage);
        }

        public BaseWordService(IUserDefine userDefine) : this()
        {
            UserDefine = userDefine;
        }

        abstract protected void AppendItemsToDataTable<T>(IEnumerable<T> items);

        public virtual string MakeCatalog<T>(IEnumerable<T> items)
        {
            DataTableItems = items.ListToDataTable("Product_PriceList");
            AppendItemsToDataTable(items);
            Document.MailMerge.ExecuteGroup(DataTableItems);

            var savedPath = SaveFile(Document);
            return savedPath;
        }

        #region Helper methods
        private string SaveFile(WordDocument document)
        {
            var configs = ModuleConfigs.GetConfigs(CatalogConstants.Module, CatalogConstants.Section);
            var path = configs.Find(e => e.ParameterName == CatalogExportPathParams).ParameterValue;
            document.Save(path + @"\" + ExportTemplate);
            document.Close();
            return path;
        }

        /// <summary>
        /// Inserts barcode in the Word document 
        /// </summary>
        private static void InsertBarcode(object sender, MergeImageFieldEventArgs args)
        {
            if (args.FieldName == BarCodeFieldName)
            {
                Image barcodeImage = GenerateBarcodeImage(args.FieldValue.ToString());
                args.Image = barcodeImage;
            }
        }

        /// <summary>
        /// Generates barcode image.
        /// </summary>
        /// <param name="barcodeText">Barcode text</param>
        /// <returns>Barcode image</returns>
        private static Image GenerateBarcodeImage(string barcodeText)
        {
            //Initialize a new PdfCode39Barcode instance
            PdfCode39Barcode barcode = new PdfCode39Barcode();
            //Set the height and text for barcode

            barcode.Text = barcodeText;
            barcode.TextDisplayLocation = TextLocation.None;
            //Convert the barcode to image
            Image barcodeImage = barcode.ToImage(new SizeF(145, 25));
            return barcodeImage;
        }

        /// <summary>
        /// Method to handle MergeImageField event.
        /// </summary>
        private static void MergeField_ProductImage(object sender, MergeImageFieldEventArgs args)
        {
            // Gets the image from file system during mail merge
            if (args.FieldName == ProductImageFieldName)
            {
                string ProductFilePath = args.FieldValue.ToString();

                if (string.IsNullOrEmpty(ProductFilePath))
                {
                    return;
                }

                if (!File.Exists(ProductFilePath))
                {
                    return;
                }

                args.Image = Image.FromFile(ProductFilePath);
                WPicture picture = args.Picture;
                WTextBoxFormat textBoxFormat = (args.CurrentMergeField.OwnerParagraph.OwnerTextBody.Owner as WTextBox).TextBoxFormat;

                // Resizes the picture to fit within text box
                var currentWidth = args.Image.Width;
                var currentHeight = args.Image.Height;

                if (currentWidth > textBoxFormat.Width || currentHeight > textBoxFormat.Height)
                {
                    if (currentWidth > currentHeight)
                    {
                        var shapeHeight = Convert.ToInt16(currentHeight * textBoxFormat.Width / currentWidth);
                        picture.Height = shapeHeight;
                        picture.Width = textBoxFormat.Width - 10.75F;
                    }
                    else
                    {
                        var shapeWidth = Convert.ToInt16(currentWidth * textBoxFormat.Width / currentHeight);
                        picture.Height = textBoxFormat.Height - 10.75F;
                        picture.Width = shapeWidth;
                    }
                }
            }
        }

        #endregion
    }
}
