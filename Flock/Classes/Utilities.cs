using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using Flock.Models;

namespace Flock.Classes
{
    public static class Utilities
    {
        public static string buildCallToActionStyleString(Style call)
        {
            var result = "";

            if (call.Margin != null)
                result += String.Format("margin: {0};", call.Margin);
            if (call.Padding != null)
                result += String.Format("padding: {0};", call.Padding);
            if (call.BackgroundColour != null)
                result += String.Format("background-color: {0};", call.BackgroundColour);
            if (call.Colour != null)
                result += String.Format("color: {0};", call.Colour);
            if (call.BorderStyle != null)
                result += String.Format("border: {0}px {1} {2};", call.BorderWidth, call.BorderStyle, call.BorderColour);
            if (call.Opacity != null)
                result += String.Format("opacity: {0};", call.Opacity);

            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LinkageMutex : ValidationAttribute
    {
        private readonly string _article;
        private readonly string _page;
        private readonly string _collection;
        private readonly string _productPage;
        private readonly string _externalURL;

        public LinkageMutex(string article, string page, string collection, string productPage, string externalURL)
        {
            _article = article;
            _page = page;
            _collection = collection;
            _productPage = productPage;
            _externalURL = externalURL;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var articleProperty = validationContext.ObjectType.GetProperty(_article);
            var pageProperty = validationContext.ObjectType.GetProperty(_page);
            var collectionProperty = validationContext.ObjectType.GetProperty(_collection);
            var productPageProperty = validationContext.ObjectType.GetProperty(_productPage);
            var externalURLProperty = validationContext.ObjectType.GetProperty(_externalURL);

            if (articleProperty == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _article)
                );
            }
            if (pageProperty == null)
            {
                return new ValidationResult (
                    string.Format("Unknown property: {0}", _page)
                );
            }
            if (collectionProperty == null)
            {
                return new ValidationResult (
                    string.Format("Unknown property: {0}", _collection)
                );
            }
            if (productPageProperty == null)
            {
                return new ValidationResult (
                    string.Format("Unknown property: {0}", _productPage)
                );
            }
            if (externalURLProperty == null)
            {
                return new ValidationResult (
                    string.Format("Unknown property: {0}", _externalURL)
                );
            }

            var articleValue = articleProperty.GetValue(validationContext.ObjectInstance, null);
            var pageValue = pageProperty.GetValue(validationContext.ObjectInstance, null);
            var collectionValue = collectionProperty.GetValue(validationContext.ObjectInstance, null);
            var productPageValue = productPageProperty.GetValue(validationContext.ObjectInstance, null);
            var externalURLValue = externalURLProperty.GetValue(validationContext.ObjectInstance, null);

            List<bool> has = new List<bool>();

            if (articleValue != null)
                has.Add(true);

            if (pageValue != null)
                has.Add(true);

            if (collectionValue != null)
                has.Add(true);

            if (productPageValue != null)
                has.Add(true);

            if (externalURLValue != null)
                has.Add(true);

            if ((has.Count > 1) && (value != null))
            {
                // but you could do any custom validation you like
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}