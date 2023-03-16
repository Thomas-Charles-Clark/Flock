using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flock.Classes
{
    public class CustomValidatorProvider : AssociatedValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context,
                                                                     IEnumerable<Attribute> attributes)
        {
            if(metadata.DisplayName != null)
            {
                if (metadata.DisplayName.EndsWith("X"))
                {
                    yield return new MyRequiredValidator(metadata, context);
                }
            }
        }

        public class MyRequiredValidator : ModelValidator
        {
            internal MyRequiredValidator(ModelMetadata metadata, ControllerContext context)
                : base(metadata, context)
            {
            }

            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                var result = new List<ModelValidationResult>();
                if (Metadata.Model == null || Metadata.Model.ToString() == "1")
                    result.Add(new ModelValidationResult
                    {
                        MemberName = Metadata.PropertyName,
                        Message = string.Format("{0} is required.", Metadata.DisplayName)
                    });

                //Metadata.AdditionalValues["ValidationMessage"].ToString()
                return result;
            }
            public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
            {
                yield return new ModelClientValidationRequiredRule("required.");
            }
        }
    }
                    
}