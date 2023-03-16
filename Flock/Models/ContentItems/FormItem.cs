using Flock.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flock.Models.ContentItems
{
    public class FormItem : ContentItem
    {
        public Form Record { get; set; }
        public string ClassName { get; set; }
    }

    public class FormModelBuilder
    {
        private UnitOfWork<FlockDataContext> UoW;

        public FormModelBuilder(UnitOfWork<FlockDataContext> UoW)
        {
            this.UoW = UoW;
        }

        public FormItem Build(long FormID, long PageSectionID)
        {
            FormItem Form = new FormItem();
            
            Form.Record = UoW.GetRepository<Form>().GetAll().Where(p => p.FormID == FormID).Single();
            Form.ClassName = UoW.GetRepository<PageSection_Form>().GetAll().Where(p => p.FormID == FormID && p.PageSectionID == PageSectionID).Select(p => p.ClassName).ToString();
            return Form;
        }

        public void Assign(long PageSectionID, long FormID, string className)
        {
            UoW.GetRepository<PageSection_Form>().Add(new PageSection_Form() { FormID = FormID, PageSectionID = PageSectionID, ClassName = className });
            UoW.Save();
        }

        public void AddElement(string ElementType, long FormID)
        {
            List<long> Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
            List<FormElement> currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).ToList();

            byte lastSort = currentElements.Max(p => p.SortOrder);

            switch (ElementType)
            {
                case "name":
                    FormElement ddl = new FormElement() { FormElementTypeID = 3, InitialValue = "", LabelText = "Title", LabelDisplayed = true, Name = "title", Required = false, SortOrder = (byte)(lastSort + 1) };
                    FormElement fn = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "First Name", LabelDisplayed = true, Name = "forename", Required = false, SortOrder = (byte)(lastSort + 2) };
                    FormElement ln = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Second Name", LabelDisplayed = true, Name = "surname", Required = false, SortOrder = (byte)(lastSort + 3) };
                    UoW.GetRepository<FormElement>().Add(ddl);
                    UoW.GetRepository<FormElement>().Add(fn);
                    UoW.GetRepository<FormElement>().Add(ln);
                    UoW.Save();

                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = ddl.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = fn.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = ln.FormElementID, FormID = FormID });
                    FormElement sub = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub.SortOrder = (byte)(lastSort + 4);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "textarea":
                    FormElement errata = new FormElement() { FormElementTypeID = 2, InitialValue = "", LabelText = "Errata", LabelDisplayed = true, Name = "errata", Required = false, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(errata);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = errata.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub1 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub1.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "phone":
                    FormElement phone = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Phone", LabelDisplayed = true, Name = "phone", Required = false, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(phone);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = phone.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub2 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub2.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "affiliatecheckbox":
                    FormElement chk = new FormElement() { FormElementTypeID = 4, InitialValue = "true", LabelText = "Affiliate", LabelDisplayed = true, Name = "affiliate", Required = false, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(chk);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add( new Form_FormElement() { FormElementID = chk.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub3 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub3.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "submit":
                    FormElement su = new FormElement() { FormElementTypeID = 7, InitialValue = "", LabelText = "Submit", LabelDisplayed = true, Name = "submit", Required = false, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(su);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = su.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub4 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub4.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "address":
                    FormElement addr1 = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Address Line 1", LabelDisplayed = true, Name = "address1", Required = false, SortOrder = (byte)(lastSort + 1) };
                    FormElement addr2 = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Address Line 2", LabelDisplayed = true, Name = "address2", Required = false, SortOrder = (byte)(lastSort + 2) };
                    FormElement city = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "City", LabelDisplayed = true, Name = "city", Required = false, SortOrder = (byte)(lastSort + 3) };
                    FormElement state = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "State", LabelDisplayed = true, Name = "state", Required = false, SortOrder = (byte)(lastSort + 4) };
                    FormElement postcode = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Post Code", LabelDisplayed = true, Name = "postcode", Required = false, SortOrder = (byte)(lastSort + 5) };
                    FormElement country = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Country", LabelDisplayed = true, Name = "country", Required = false, SortOrder = (byte)(lastSort + 6) };
                    UoW.GetRepository<FormElement>().Add(addr1);
                    UoW.GetRepository<FormElement>().Add(addr2);
                    UoW.GetRepository<FormElement>().Add(city);
                    UoW.GetRepository<FormElement>().Add(state);
                    UoW.GetRepository<FormElement>().Add(postcode);
                    UoW.GetRepository<FormElement>().Add(country);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = addr1.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = addr2.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = city.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = state.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = postcode.FormElementID, FormID = FormID });
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = country.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub5 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub5.SortOrder = (byte)(lastSort + 7);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "email":
                    FormElement email = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Email", LabelDisplayed = true, Name = "email", Required = true, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(email);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = email.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub8 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub8.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "country":
                    FormElement country2 = new FormElement() { FormElementTypeID = 1, InitialValue = "", LabelText = "Country", LabelDisplayed = true, Name = "country", Required = false, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(country2);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = country2.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub9 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub9.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                case "captcha":
                    FormElement captcha = new FormElement() { FormElementTypeID = 5, InitialValue = "", LabelText = "Captcha", LabelDisplayed = false, Name = "captcha", Required = true, SortOrder = (byte)(lastSort + 1) };
                    UoW.GetRepository<FormElement>().Add(captcha);
                    UoW.Save();
                    UoW.GetRepository<Form_FormElement>().Add(new Form_FormElement() { FormElementID = captcha.FormElementID, FormID = FormID });
                    UoW.Save();
                    FormElement sub10 = currentElements.Where(p => p.FormElementTypeID == 6).Single();
                    sub10.SortOrder = (byte)(lastSort + 2);
                    UoW.Save();
                    
                    Linker = UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormID == FormID).Select(p => p.FormElementID).ToList();
                    currentElements = UoW.GetRepository<FormElement>().GetAll().Where(p => Linker.Contains(p.FormElementID)).OrderBy(p => p.SortOrder).ToList();

                    for (byte i = 0; i < currentElements.Count; i++)
                    {
                        currentElements[i].SortOrder = i;
                    }
                    UoW.Save();
                    break;
                default: break;
            }
        }

        public void Create(string InternalName, string Title, string Description, byte FormType)
        {
            Form Form = new Form();
            Form.DescriptionText = Description;
            Form.DisplayTitle = Title;
            Form.Name = InternalName;
            Form.MarketID = 0;
            Form.FormType = FormType;
            UoW.GetRepository<Form>().Add(Form);
            UoW.Save();
        }

        public void Update(long FormID, string InternalName, string Title, string Description, byte FormType)
        {
            Form Form = UoW.GetRepository<Form>().GetAll().Where(p => p.FormID == FormID).Single();

            Form.Name = InternalName;
            Form.DisplayTitle = Title;
            Form.DescriptionText = Description;
            Form.FormType = FormType;
        }

        public void Delete(long FormID)
        {
            throw new NotImplementedException();
        }

        public void RemoveElement(long FormID, long FormElementID)
        {
            UoW.GetRepository<FormElement>().Delete(UoW.GetRepository<FormElement>().GetAll().Where(p => p.FormElementID == FormElementID).Single());
            UoW.GetRepository<Form_FormElement>().Delete(UoW.GetRepository<Form_FormElement>().GetAll().Where(p => p.FormElementID == FormElementID).Single());

            UoW.Save(); 
        }
    }
}