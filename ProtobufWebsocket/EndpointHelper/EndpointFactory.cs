using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointHelper
{
    internal class EndpointHelper
    {
        public static void PullEndpointThroughAssembly(Assembly assembly)
        {
            var globalTypes = AssemblyHelper.loadAssemblyTypes(assembly);

                //retrieves all the assemblies consuming these attributes
            var Handlers = globalTypes.Where(t =>
                t.GetInterfaces().ToList()
                .Where( I => I.Name == typeof(IDynamicEndpoint).Name).Any());

            EndpointsHandleProvider.CreateEndpointHandlerSingleton(Handlers.ToList()); //endpoints saved
        }

        public static void ResolveRequest(byte[] incomingBytes)//service provider instance that is passed from the application builder context
        {
            var CalledEndpoint = getAssociatedEndpoints(incomingBytes);


        }

        //gets called each time a message is recieved, invokes the appropriate endpoint 
        private static List<EndpointTypeProperties> getAssociatedEndpoints(byte[] incomingBytes)
        {
           
            var requestEndpointType = EndpointsTypeProvider.getRequestInstance();

            var RequestEndpointObject = ProtobufAccessHelper.Decode(requestEndpointType, incomingBytes); //class includes list of objects of extended type irequest

            var EndpointList = new List<EndpointTypeProperties>();

            requestEndpointType.GetRuntimeFields().ToList().ForEach(field =>
            {
                if (field.FieldType.IsArray)
                {
                    var fieldArr = field.GetValue(RequestEndpointObject);
                    if (fieldArr != null)
                    {
                        
                        RuntimeArrayHelpers.loopRuntimeArray(fieldArr!, (element) =>
                        {
                            //get the Request's coresponding endpoint
                            EndpointList.Add(RequestMappingHelper.GetEndpoint(element.GetType()));

                        });
                    }
                }
            });

            return EndpointList;
        }

        //intializes an endpoint along with its constructor parameters
        private static object prepareEndpoint(EndpointTypeProperties endpoint, IServiceProvider serviceProvider) //endpopint is created, dependencies resolved.
        {
            var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;

            //an array of objects is constructed using dependency injection
            var constructorObjects = new List<object>();

            foreach(var param in endpoint.EndpointConstructorParams)
            {
                var constructedService = provider.GetRequiredService(param); //services gets constructed using DI
                if(constructedService != null)
                {
                    constructorObjects.Add(constructedService);
                }
            }

            var endpointType = endpoint.EndpointType;

            object endpointInstance = Activator.CreateInstance(endpointType, constructorObjects.ToArray())!; //might have issues with object placment

            return endpointInstance;

        }

        public void invokeEndpointHandle()
        {

        }
        
    }
}
