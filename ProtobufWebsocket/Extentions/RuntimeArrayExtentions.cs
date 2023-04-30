using ProtobufWebsocket.Assembly_Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Extentions
{
    internal static class RuntimeArrayExtentions
    {
        public static object AppendStaticArray(this object array, object ToGetAppended)
        {
            var arrayType = array.GetType();
            if (!array.GetType().IsArray)
                throw new Exception($"The passed is object is not an instance of Type Array {nameof(AppendStaticArray)}");
            var name = arrayType.GetElementType()!.Name;
            if (!(arrayType.GetElementType()!.Name == ToGetAppended.GetType().Name)) //element type and object are not the same
                throw new Exception($"array and object are not of the same type (Reflection), thrown at {nameof(AppendStaticArray)}");

            var elementType = arrayType.GetElementType();                                                                               //used to Traverse inside the object
            var elementObject = Activator.CreateInstance(arrayType.GetElementType()!);

            var emptyelementType = elementObject.GetType();
            var test = emptyelementType.GetProperties();
            emptyelementType.GetProperties().ToList().ForEach(e =>
            {
                e.SetValue(elementObject, ToGetAppended.GetType().GetProperty(e.Name).GetValue(ToGetAppended)); //searches the to be cloned object for field name and sets the element value
            });

            //arrayType
            var AppendedArray = RuntimeArrayHelpers.AppendObjectToRuntimeArray(array, elementObject);

            return AppendedArray;


        }
    }
}
