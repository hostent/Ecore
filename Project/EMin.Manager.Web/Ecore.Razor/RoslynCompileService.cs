using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;


namespace Ecore.Razor
{
    public class RoslynCompileService
    {

        public static Assembly viewEngine_Razor_Assembly = typeof(RoslynCompileService).GetTypeInfo().Assembly;
        private static List<MetadataReference> ApplicationReferences = GetApplicationReferences();


        public Type Compile(string compilationContent)
        {
            var assemblyName = Path.GetRandomFileName();

            var sourceText = SourceText.From(compilationContent, Encoding.UTF8);
            var syntaxTree = CSharpSyntaxTree.ParseText(
                sourceText,
                path: assemblyName,
                options: new CSharpParseOptions()
                );


            var compilation = CSharpCompilation.Create(assemblyName,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                    syntaxTrees: new[] { syntaxTree },
                    references: ApplicationReferences
                );

            using (var assemblyStream = new MemoryStream())
            {

                var result = compilation.Emit(
                    assemblyStream,
                    options: new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb));


                if (!result.Success)
                {
                    if (!compilation.References.Any() && !ApplicationReferences.Any())
                    {
                        throw new InvalidOperationException("project.json preserveCompilationContext");
                    }

                    return null;
                }


                var templateType = LoadTypeForAssemblyStream(assemblyStream, null);

                return templateType;


            }

        }


        protected virtual DependencyContext GetDependencyContext(Assembly assembly)
        {

            return DependencyContext.Load(assembly);
        }

        private Type LoadTypeForAssemblyStream(MemoryStream assemblyStream, MemoryStream pdbStream)
        {
            assemblyStream.Seek(0, SeekOrigin.Begin);
            Assembly assembly = null;

            assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(assemblyStream);

            var type = assembly.GetExportedTypes().FirstOrDefault(a => !a.IsNested);
            return type;

        }

        private static List<MetadataReference> GetApplicationReferences()
        {
            var metadataReferences = new List<MetadataReference>();
            var assembly = Assembly.GetEntryAssembly();

            string runtimePath = System.IO.Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);

            var mscorlibFile = Path.Combine(runtimePath, "mscorlib.dll");
            if (File.Exists(mscorlibFile))
                metadataReferences.Add(CreateMetadataFileReference(mscorlibFile));
            else
                metadataReferences.Add(CreateMetadataFileReference(Path.Combine(runtimePath, "mscorlib.ni.dll")));

            metadataReferences.Add(CreateMetadataFileReference(typeof(object).GetTypeInfo().Assembly.Location));
            metadataReferences.Add(CreateMetadataFileReference(typeof(DynamicObject).GetTypeInfo().Assembly.Location));

            metadataReferences.Add(CreateMetadataFileReference(assembly.Location));


            var referencedAssemblies = assembly.GetReferencedAssemblies();



            //reference razor view engine by this assembly 
            if (!referencedAssemblies.Contains(viewEngine_Razor_Assembly.GetName()))
                metadataReferences.Add(CreateMetadataFileReference(viewEngine_Razor_Assembly.Location));


            foreach (var refAssemblyName in referencedAssemblies)
            {
                var refAssembly = Assembly.Load(refAssemblyName);
                metadataReferences.Add(CreateMetadataFileReference(refAssembly.Location));

            }



            return metadataReferences;
        }

        private static MetadataReference CreateMetadataFileReference(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                return assemblyMetadata.GetReference(filePath: path);
            }
        }





    }
}
