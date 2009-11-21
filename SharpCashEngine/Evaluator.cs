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
        public Evaluator()
        {
            this.engine = Python.CreateEngine();
            using(var r = Assembly.GetExecutingAssembly().GetManifestResourceStream("SharpCashEngine.pmt.py"))
            {
                using(var s = new StreamReader(r))
                {
                    this.engine.Execute(s.ReadToEnd());
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
