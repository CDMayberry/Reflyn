using System;
using System.ComponentModel;
using Reflyn.Declarations;

namespace Reflyn.Refly.Templates
{
    public abstract class Template : ITemplate
    {
        private string templateName;

        [Description("Format string to name the class")]
        [Category("Data")]
        public string NameFormat { get; set; }

        /*[Description("Tab used in generation of classes")]
        [Category("Data")]
        public string Tab
        {
            get => Compiler.Tab;
            set => Compiler.Tab = value;
        }*/

        [Category("Data")]
        [Description("Base directory of generated files")]
        public string OutputPath { get; set; } = "";

        [Category("Data")]
        [Description("Base namespace for generated classes")]
        public string Namespace { get; set; } = null;

        [Category("Data")]
        [Description("Namespace imports")]
        public ImportCollection Imports { get; set; } = new ImportCollection();

        [Description("Output language")]
        [Category("Data")]
        public CodeLanguage OutputLanguage { get; set; } = CodeLanguage.Cs;

        public NamespaceDeclaration NamespaceDeclaration { get; set; } = null;

        //protected CodeGenerator Compiler { get; } = new CodeGenerator();

        [Browsable(false)]
        public virtual string TemplateName
        {
            get => templateName;
            set => templateName = value;
        }

        public Template(string templateName, string nameFormat)
        {
            this.templateName = templateName;
            this.NameFormat = nameFormat;
            var import = new Import
            {
                Name = "System"
            };
            Imports.Add(import);
        }

        public virtual void Prepare()
        {
            if (string.IsNullOrEmpty(Namespace))
            {
                throw new ArgumentException("Namespace is empty");
            }
            NamespaceDeclaration = new NamespaceDeclaration(Namespace);
            foreach (Import import in Imports)
            {
                NamespaceDeclaration.AddImport(import.ToString());
            }
        }

        public abstract void Generate();

        protected virtual void Compile()
        {
            throw new NotImplementedException();
            /*switch (OutputLanguage)
            {
                case CodeLanguage.Cs:
                    Compiler.Provider = CodeGenerator.CsProvider;
                    break;
                case CodeLanguage.Vb:
                    Compiler.Provider = CodeGenerator.VbProvider;
                    break;
            }
            Compiler.GenerateCode(OutputPath, NamespaceDeclaration);*/
        }

        public override string ToString()
        {
            return TemplateName;
        }
    }
}
