using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office;
using System.Reflection;

namespace Flock.Util
{
    //http://devpinoy.org/blogs/keithrull/archive/2007/06/09/updated-how-to-merge-multiple-microsoft-word-documents.aspx thanks dude!
    public class WordInteropUtilities
    {
        public static void Merge(string[] filesToMerge, string outputFilename, bool insertPageBreaks, string docTitle)
        {
            object missing = System.Type.Missing;
            object pageBreak = Word.WdBreakType.wdPageBreak;
            object outputFile = outputFilename;
            Word._Document wordDocument = null;

            // Create  a new Word application
            Word._Application wordApplication = new Word.Application();

            try
            {
                // Create a new file based on our template
                wordDocument = wordApplication.Documents.Add(
                                              AppDomain.CurrentDomain.BaseDirectory + "\\Brochures\\LabelTemplate\\Blank.dotx"
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                // Make a Word selection object.
                Word.Selection selection = wordApplication.Selection;

                //Count the number of documents to insert;
                int documentCount = filesToMerge.Length;

                //A counter that signals that we shoudn't insert a page break at the end of document.
                int breakStop = 0;

                // Loop thru each of the Word documents
                foreach (string file in filesToMerge)
                {
                    breakStop++;
                    // Insert the files to our template
                    selection.InsertFile(
                                                file
                                            , ref missing
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                    //Do we want page breaks added after each documents?
                    if (insertPageBreaks && breakStop != documentCount)
                    {
                        selection.InsertBreak(ref pageBreak);
                    }
                }

                var properties = wordDocument.BuiltInDocumentProperties;

                Type typeDocBuiltInProps = properties.GetType();

                //string strIndex = "Author";
                //string strValue;
                //object oDocAuthorProp = typeDocBuiltInProps.InvokeMember("Item",
                //                           BindingFlags.Default |
                //                           BindingFlags.GetProperty,
                //                           null, properties,
                //                           new object[] { strIndex });
                //Type typeDocAuthorProp = oDocAuthorProp.GetType();

                //Set the Title property.
                var strIndex = "Subject";
                var strValue = docTitle;
                typeDocBuiltInProps.InvokeMember("Item",
                                           BindingFlags.Default |
                                           BindingFlags.SetProperty,
                                           null, properties,
                                           new object[] { strIndex, strValue });


                // Save the document to it's output file.
                wordDocument.SaveAs(
                                ref outputFile
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Clean up!
                wordDocument.Close();
                wordApplication.NormalTemplate.Saved = true;
                wordApplication.Quit(ref missing, ref missing, ref missing);
            }
        }
    }
}