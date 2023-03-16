using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;
using System.Globalization;
using System.Reflection;
using OpenXmlPowerTools;
using OpenXmlPowerTools.SpreadsheetWriter;
using DocumentFormat.OpenXml.Packaging;

namespace Flock.Util
{
    public static class OpenXmlHelper
    {

        public static void CreateBrochureRequestSpreadsheet(string filePath, string fileName, List<Member_Brochure> requests, List<Country> countries)
        {   
            TextInfo ti = new System.Globalization.CultureInfo("en-US", true).TextInfo;
            
            string[] columnHeadings = new[] {
                "MemberID",
                "Email",
                "Name",
                "Address 1",
                "Address 2",
                "Town",
                "County",
                "Postcode",
                "Country",
                "Phone"
            };

            var data = new[] {
                new {
                    SheetName = "Sheet1",
                    NumberColumns = 10,
                    NumberRows = requests.Count
                }
            };

            Workbook wb = new Workbook
            {
                Worksheets = data.Select(d => new Worksheet
                {
                    Name = d.SheetName,
                    ColumnHeadings = Enumerable.Range(0, d.NumberColumns)
                    .Select(c => new Cell
                    {
                        Value = columnHeadings[c],
                        Bold = true
                    }),
                    Rows = Enumerable.Range(0, d.NumberRows)
                    .Select(r => new Row()
                    {
                        Cells = Enumerable.Range(0, d.NumberColumns)
                        .Select(c =>
                        {
                            var text = "";

                            switch (c)
                            {
                                case 0:
                                    text = requests[r].MemberID.ToString();
                                    break;

                                case 1:
                                    text = requests[r].Email;
                                    break;

                                case 2:
                                    text = ti.ToTitleCase(requests[r].FullName ?? "");
                                    break;

                                case 3:
                                    text = ti.ToTitleCase(requests[r].AddressLine1 ?? "");
                                    break;

                                case 4:
                                    text = ti.ToTitleCase(requests[r].AddressLine2 ?? "");
                                    break;

                                case 5:
                                    text = ti.ToTitleCase(requests[r].Town ?? "");
                                    break;

                                case 6:
                                    text = ti.ToTitleCase(requests[r].County ?? "");
                                    break;

                                case 7:
                                    text = requests[r].Postcode?.ToUpper();
                                    break;

                                case 8:
                                    text = countries.Where(p => p.CountryID == requests[r].CountryID).Select(p => p.Name).SingleOrDefault();
                                    break;

                                case 9:
                                    text = requests[r].PhoneNumber;
                                    break;

                                default: break;
                            }

                            return new Cell
                            {
                                Value = text,
                                CellDataType = CellDataType.String
                            };
                        })
                    })
                })
            };

            SpreadsheetWriter.Write(filePath, wb);
        }

        public static void CreateProductImageExportSpreadsheet(string filePath, string fileName, List<ProductImage_Export_ProductImageResult> images)
        {
            string[] columnHeadings = new[] {
                "Launch",
                "Brand",
                "Product Type",
                "Collection",
                "Code",
                "Name",
                "Description",
                "SizeMetric",
                "SizeImperial",
                "ImageID",
                "Path",
                "FileName"
            };

            var data = new[] {
                new {
                    SheetName = fileName,
                    NumberColumns = 12,
                    NumberRows = images.Count
                }
            };

            Workbook wb = new Workbook
            {
                Worksheets = data.Select(d => new Worksheet
                {
                    Name = d.SheetName,
                    ColumnHeadings = Enumerable.Range(0, d.NumberColumns)
                        .Select(c => new Cell
                        {
                            Value = columnHeadings[c],
                            Bold = true
                        }),
                    Rows = Enumerable.Range(0, d.NumberRows)
                        .Select(r => new Row
                        {
                            Cells = Enumerable.Range(0, d.NumberColumns)
                            .Select(c => 
                            {
                                var text = "";

                                switch(c)
                                {
                                    case 0:
                                        text = images[r].Launch;
                                        break;

                                    case 1:
                                        text = images[r].Brand;
                                        break;

                                    case 2:
                                        text = images[r].ProductType;
                                        break;

                                    case 3:
                                        text = images[r].Collection;
                                        break;

                                    case 4:
                                        text = images[r].Code;
                                        break;

                                    case 5:
                                        text = images[r].Name;
                                        break;

                                    case 6:
                                        text = images[r].Description;
                                        break;

                                    case 7:
                                        text = images[r].SizeMetric;
                                        break;

                                    case 8:
                                        text = images[r].SizeImperial;
                                        break;

                                    case 9:
                                        text = images[r].ImageID.ToString();
                                        break;

                                    case 10:
                                        text = images[r].Path;
                                        break;

                                    case 11:
                                        text = images[r].ImageFileName;
                                        break;

                                    default: break;
                                }

                                return new Cell
                                {
                                    Value = text,
                                    CellDataType = CellDataType.String
                                };
                            })
                        }),
                })
            };

            SpreadsheetWriter.Write(filePath, wb);
        }


        public static void CreateCollectionImageExportSpreadsheet(string filePath, string fileName, List<CollectionImage_Export_CollectionImageResult> images)
        {
            string[] columnHeadings = new[] {
                "Launch",
                "Brand",
                "Product Type",
                "Collection",
                "Heading",
                "SubHeading",
                "Description",
                "ImageID",
                "Path",
                "FileName"
            };

            var data = new[] {
                new {
                    SheetName = fileName,
                    NumberColumns = 10,
                    NumberRows = images.Count
                }
            };

            Workbook wb = new Workbook
            {
                Worksheets = data.Select(d => new Worksheet
                {
                    Name = d.SheetName,
                    ColumnHeadings = Enumerable.Range(0, d.NumberColumns)
                        .Select(c => new Cell
                        {
                            Value = columnHeadings[c],
                            Bold = true
                        }),
                    Rows = Enumerable.Range(0, d.NumberRows)
                        .Select(r => new Row
                        {
                            Cells = Enumerable.Range(0, d.NumberColumns)
                            .Select(c =>
                            {
                                var text = "";

                                switch (c)
                                {
                                    case 0:
                                        text = images[r].Launch;
                                        break;

                                    case 1:
                                        text = images[r].Brand;
                                        break;

                                    case 2:
                                        text = images[r].ProductType;
                                        break;

                                    case 3:
                                        text = images[r].Collection;
                                        break;

                                    case 4:
                                        text = images[r].Heading;
                                        break;

                                    case 5:
                                        text = images[r].SubHeading;
                                        break;

                                    case 6:
                                        text = images[r].Description;
                                        break;

                                    case 7:
                                        text = images[r].ImageID.ToString();
                                        break;

                                    case 8:
                                        text = images[r].Path;
                                        break;

                                    case 9:
                                        text = images[r].ImageFileName;
                                        break;

                                    default: break;
                                }

                                return new Cell
                                {
                                    Value = text,
                                    CellDataType = CellDataType.String
                                };
                            })
                        }),
                })
            };

            SpreadsheetWriter.Write(filePath, wb);
        }
    }
}