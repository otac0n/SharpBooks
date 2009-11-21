using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using System.IO;

namespace SharpCash
{
    public class Evaluator
    {
        private ScriptEngine engine;
        private ScriptSource functions;

        public Evaluator()
        {
            this.engine = Python.CreateEngine();
            using(var r = Assembly.GetExecutingAssembly().GetManifestResourceStream("SharpCash.financial.py"))
            {
                using(var s = new StreamReader(r))
                {
                    this.functions = this.engine.CreateScriptSourceFromString(s.ReadToEnd(), SourceCodeKind.Statements);
                }
            }
        }

        public object Evaluate(string expression)
        {
            return this.Evaluate(expression, null);
        }

        public object Evaluate(string expression, Dictionary<string, object> parameters)
        {
            var scope = engine.CreateScope();

            this.functions.Execute(scope);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    scope.SetVariable(param.Key, param.Value);
                }
            }

            ScriptSource source = this.engine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
            return source.Execute(scope);
        }
    }
}
