using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tennis.Helpers
{
    public static class ModelStateDictionaryExtensions
    {
        public static void ExceptionToErrorMessage(this ModelStateDictionary modelstate)
        {
            List<Tuple<string, string>> errorsToBeAdded = new List<Tuple<string, string>>();
            foreach (var keyValuePair in modelstate)
            {
                foreach (var error in keyValuePair.Value.Errors) 
                {
                    if (error.Exception != null)
                    {

                        errorsToBeAdded.Add(new Tuple<string, string>(keyValuePair.Key, error.Exception.Message));
                    }
                }
            }
            foreach(var item in errorsToBeAdded)
            {
                modelstate.AddModelError(item.Item1, item.Item2);
            }
        }
    }
}
