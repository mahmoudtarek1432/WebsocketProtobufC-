using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Assembly_Helpers
{
    internal class RuntimeArrayHelpers
    {

        //adds an element to an array, the ToGetAppended in this case is of class that inherits IRequest or IResponse
        //the passed object gets cloned into a an object of a type that is associated with protocontract attributs
        //then the values are cloned to the runtime created type
        public static object AppendToRuntimeArray(object array, object ToGetAppended)
        {
            var arrayType = array.GetType();
            if (!array.GetType().IsArray)
                throw new Exception($"The passed is object is not an instance of Type Array {nameof(GetRuntimeArrayLength)}");
            var name = arrayType.GetElementType()!.Name;
            if (!(arrayType.GetElementType()!.Name == ToGetAppended.GetType().Name)) //element type and object are not the same
                throw new Exception($"array and object are not of the same type (Reflection), thrown at {nameof(AppendToRuntimeArray)}");

            var elementType = arrayType.GetElementType();                                                                               //used to Traverse inside the object
            var elementObject = Activator.CreateInstance(arrayType.GetElementType()!);

            var filledElement = cloneObjectValue(elementObject!, ToGetAppended);  //object values got cloned

            //arrayType
            var AppendedArray = AppendObjectToRuntimeArray(array, filledElement);

            return AppendedArray;


        }

        public static object cloneObjectValue(object emptyObj, object Origin)
        {
            var elementType = emptyObj.GetType();
            var test = elementType.GetProperties();
            elementType.GetRuntimeFields().ToList().ForEach(e =>
            {
                var isArray = e.FieldType.IsArray;
                if (isArray)
                {
                    var RuntimeArr = (object) Array.CreateInstance(e.FieldType.GetElementType(), 1);
                    var OriginArray = Origin.GetType().GetProperty(e.Name).GetValue(Origin);
                    if (OriginArray != null)
                    {

                        loopRuntimeArray(OriginArray, (element) =>
                        {
                            RuntimeArr = AppendToRuntimeArray(RuntimeArr, element);
                        });
                        e.SetValue(emptyObj, RuntimeArr);
                    }
                }
                else
                {
                    e.SetValue(emptyObj, Origin.GetType().GetProperty(e.Name).GetValue(Origin)); //searches the to be cloned object for field name and sets the element value

                }
            });

            return emptyObj;
        }

        public static int GetRuntimeArrayLength(object Array)
        {
            if (!Array.GetType().IsArray)
                throw new Exception($"The passed is object is not an instance of Type Array {nameof(GetRuntimeArrayLength)}");

            var LengthMember = Array.GetType().GetMember("Length");
            var methodAccessor = LengthMember.GetType().GetMethod("get_Length");
            if (methodAccessor == null)
                throw new Exception($"method is implemented in Type {LengthMember} at {nameof(GetRuntimeArrayLength)}");

            var length = (int)methodAccessor.Invoke(Array, new object[] { })!; //function int32 get_Length();
            return length;
        }

        public static object AppendObjectToRuntimeArray(object Array, object element)
        {
            if (!Array.GetType().IsArray)
                throw new Exception($"Passed object is not an array {nameof(AppendObjectToRuntimeArray)}");

            var arrayType = Array.GetType();
            var ArrayGetMethod = arrayType.GetMethod("Get"); //Get(index)
            var ArraySetMethod = arrayType.GetMethod("Set"); //Set(index,Object)

            var length = GetRuntimeArrayLength(Array);

            int index;
            for (index = 0; index < length; index++)
            {
                if (ArrayGetMethod.Invoke(Array, new object[] { index }) == null)  //the array index is null
                {
                    ArraySetMethod.Invoke(Array, new object[] { index, element });

                    return Array; //object with element added
                }
            }

            //completed loop signifies that the array's slots are all full

            var extendedArray = extendRuntimeArray(Array); // array is cloned and size is doubled

            return AppendObjectToRuntimeArray(extendedArray, element);
        }

        //called in case of a full runtimeArray Object, clones the array and returns an extended array (doubles the length)
        public static object extendRuntimeArray(object FromArray)
        {
            var isarray = FromArray.GetType();
            if (!FromArray.GetType().IsArray)
                throw new Exception($"Passed object is not an array {nameof(extendRuntimeArray)}");

            var initialLength = GetRuntimeArrayLength(FromArray);
            var NewLength = initialLength * 2; //doubles the initial size

            var elementType = FromArray.GetType().GetElementType();
            var createdArray = Array.CreateInstance(elementType!, NewLength); // the returned array

            var ArrayGetMethod = FromArray.GetType().GetMethod("Get"); //Get(index)
            var ArraySetMethod = FromArray.GetType().GetMethod("Set"); //Set(index,Object)

            for (int index = 0; index < initialLength; index++)
            {
                var passElement = ArrayGetMethod!.Invoke(FromArray, new object[] { index });
                ArraySetMethod!.Invoke(createdArray, new object[] { index, passElement! });
            }

            return createdArray;
        }

        public static void loopRuntimeArray(object Array, Action<object> process)
        {
            if (!Array.GetType().IsArray)
                throw new Exception($"Passed object is not an array {nameof(extendRuntimeArray)}");

            var initialLength = GetRuntimeArrayLength(Array);
            var elementType = Array.GetType().GetElementType();

            var ArrayGetMethod = Array.GetType().GetMethod("Get"); //Get(index)


            for (int index = 0; index < initialLength; index++)
            {
                var Element = ArrayGetMethod!.Invoke(Array, new object[] { index });
                process(Element);
            }
        }

        public static void loopRuntimeArray(object Array, Action<object,int> process)
        {
            if (!Array.GetType().IsArray)
                throw new Exception($"Passed object is not an array {nameof(extendRuntimeArray)}");

            var initialLength = GetRuntimeArrayLength(Array);
            var elementType = Array.GetType().GetElementType();

            var ArrayGetMethod = Array.GetType().GetMethod("Get"); //Get(index)


            for (int index = 0; index < initialLength; index++)
            {
                var Element = ArrayGetMethod!.Invoke(Array, new object[] { index });
                process(Element,index);
            }
        }
    }
}
