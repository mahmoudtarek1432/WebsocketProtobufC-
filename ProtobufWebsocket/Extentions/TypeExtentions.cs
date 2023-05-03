using ProtobufWebsocket.Model;
using System.Reflection;

namespace ProtobufWebsocket.Extentions
{
    public static class TypeExtentions
    {
        public static IEnumerable<Type> fetchClasses(this IEnumerable<Type> type)
        {
            return type.Where(t => t.IsClass);
        }

        public static IEnumerable<Type> getTypesWithAttributes(this IEnumerable<Type> type, Func<Attribute, bool> predicate)
        {
            return type.Where((t) => t.GetCustomAttributes().FirstOrDefault(predicate) != null); // has the attribute
        }

        public static IEnumerable<Type> RetriveConstructorParameters(this Type type)
        {
            var constructors = type.GetConstructors();
            var paramsList = new List<Type>();
            if (constructors.Length == 0)
                throw new Exception($"Type {type.FullName} has no constructors");
           
            foreach ( var constructor in constructors )
            {
                var parameters = constructor.GetParameters();
                foreach ( var param in parameters)
                {
                    paramsList.Add(param.ParameterType);
                }
            }
            return paramsList;
        }

        internal static object PopulateType(this Type StaticType, object runtimeObject) //clones runtime value, the runtime type has the exact same properties and fields
        {
            //getUninitializedObject returns an object of type, without getting instantiated.
            //this is used as the types passed are usually modles and dtos that does not have constructor implementations.

            var staticTypeInstance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(StaticType);

            foreach (var field in staticTypeInstance.GetType().GetProperties())
            {
                var runtimeFieldValue = runtimeObject.GetType().GetField(field.Name).GetValue(runtimeObject);
                field.SetValue(staticTypeInstance, runtimeFieldValue);
            }
            return staticTypeInstance;
        }

        //invokes the Handle function delegate that is present in classes inheriting endpoint base abstract class
        //handle is the fuction responsible for processing the incoming request
        /** <summary> invokes handle function </summary> */
        internal static object InvokeHandler(this object EndpointObject, object requestObject)
        {
            var endpointHandlerType = EndpointObject.GetType();
            var handleDelegate = endpointHandlerType.GetMethod("HandleAsync");

            var Requesttype = endpointHandlerType.BaseType!.GetGenericArguments().Where(A => A.BaseType.Name == typeof(IRequest).Name).FirstOrDefault();

            if (Requesttype == null) throw new ArgumentNullException(nameof(InvokeHandler));

            var HandlerRequest = Requesttype.PopulateType(requestObject); //returns an instance of the concrete class created as a request type

            return handleDelegate.Invoke(EndpointObject, new object[] { HandlerRequest })!; //second argument is the request object
        }

        //with no arguments
        internal static object InvokeHandler(this object EndpointObject)
        {
            var endpointHandlerType = EndpointObject.GetType();
            var handleDelegate = endpointHandlerType.GetMethod("HandleAsync");

            return handleDelegate.Invoke(EndpointObject, Array.Empty<object>())!; //second argument is the request object
        }
    }
}
