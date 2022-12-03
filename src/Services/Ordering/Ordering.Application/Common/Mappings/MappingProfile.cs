using AutoMapper;
using System.Reflection;

namespace Ordering.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool HasInterFace(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var typyes = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(HasInterFace)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in typyes)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if(methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterFace).ToList();

                    if (interfaces.Count <= 0) continue;

                    foreach (var iface in interfaces)
                    {
                        var interfaceMethodInfo = iface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
