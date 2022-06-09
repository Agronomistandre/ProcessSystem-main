using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProcessSystem.UI.Blazor.Client.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = false)]
    public class CheckEmptyCollectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is ICollection collection)
                return collection.Count > 0;

            throw new NotSupportedException("This attribute can only work with ICollection");
        }
    }

    //Попытка атрибутом проверять дубликаты в коллекции, ну не особо выходит
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    //    AllowMultiple = false)]
    //public class CheckDublicateInCollectionAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        if (value is ICollection<string> collection)
    //        {
    //            var distinctCollection = collection.Distinct();
    //            return collection.Count() == distinctCollection.Count();
    //        }

    //        throw new NotSupportedException("This attribute can only work with ICollection");
    //    }
    //}
}