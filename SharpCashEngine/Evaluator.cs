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
        private ScriptScope scope;

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
            this.scope = engine.CreateScope();
        }

        public object Evaluate(string expression)
        {
            ScriptSource source = this.engine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
            return source.Execute(this.scope);
        }
    }
}
