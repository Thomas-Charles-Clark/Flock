using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using Flock.Models;

namespace Flock.Controllers
{
    public partial class AssetsController : Controller
    {
        private static string labelTemplateFile = AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\Template.docx";
        private static string resultantTestFile = AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\NewTest.docx";

        public void CreateDocument(string sourceFile, string destinationFile, List<int> thisSet)
        {
            try
            {
                List<Member_Brochure> reqs = UnitOfWork.GetRepository<Member_Brochure>().GetAll().Where(p => thisSet.Contains(p.Member_Brochure_ID)).ToList();

                List<Country> countries = UnitOfWork.GetRepository<Country>().GetAll().ToList();

                // Create a copy of the template file and open the copy
                System.IO.File.Copy(sourceFile, destinationFile, true);
                using (WordprocessingDocument document = WordprocessingDocument.Open(destinationFile, true))
                {
                    // Change the document type to Document
                    document.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

                    int ctr = 0;

                    System.Globalization.TextInfo ti = new System.Globalization.CultureInfo("en-GB", true).TextInfo;

                    TableProperties tp = document.MainDocumentPart.RootElement.Descendants<TableProperties>().First();
                    TableCellMarginDefault tcmd = tp.Descendants<TableCellMarginDefault>().First();
                    TableCellLeftMargin tclm = tcmd.Descendants<TableCellLeftMargin>().First();
                    tclm.Width = 120;
                    
                    foreach (Run r in document.MainDocumentPart.RootElement.Descendants<Run>().Where(p => p.Descendants<Text>().Count() > 0))
                    {
                        var labelField = r.GetFirstChild<Text>();
                        r.RemoveChild<Text>(labelField);

                        if(ctr < reqs.Count)
                        {
                            Member_Brochure req = reqs[ctr];
                            Country country = countries.Where(p => p.CountryID == req.CountryID).SingleOrDefault();

                            if (req.FullName != null && req.FullName != "")
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(req.FullName);
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (req.AddressLine1 != null && req.AddressLine1 != "")
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(req.AddressLine1);
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (req.AddressLine2 != null && req.AddressLine2 != "")
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(req.AddressLine2);
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (req.Town != null && req.Town != "")
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(req.Town);
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (req.County != null && req.County != "")
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(req.County);
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (req.Postcode != null && req.Postcode != "")
                            {
                                var newLine = new Text();
                                newLine.Text = req.Postcode.ToUpper();
                                r.Append(newLine);
                                r.Append(new Break());
                            }

                            if (country != null)
                            {
                                var newLine = new Text();
                                newLine.Text = ti.ToTitleCase(country.Name);
                                r.Append(newLine);
                            }

                            ctr++;

                            if(req.ProcessedDT == null)
                                req.ProcessedDT = DateTime.Now;
                        }
                    }

                    UnitOfWork.Save();

                    // If the Document has settings remove them so the end user doesn't get prompted to use the data source
                    DocumentSettingsPart settingsPart = document.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();

                    var oxeSettings = settingsPart.Settings.Where(a => a.LocalName == "mailMerge").FirstOrDefault();

                    if (oxeSettings != null)
                    {
                        settingsPart.Settings.RemoveChild(oxeSettings);

                        settingsPart.Settings.Save();
                    }

                    document.MainDocumentPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }
    }
}