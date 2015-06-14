using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NCalc;
using Tornado.Server.ServiceModel;

namespace Tornado.Server.ServiceModel
{
}

namespace Tornado.Business
{


    public abstract class Rule
    {
        public Rule()
        {
            Condition = "False";
        }

        public string Condition { get; set; }

        public void Execute(string filepath, FileResponse file)
        {
            if (CanExecute(filepath, file))
                DoExecute(filepath, file);
        }

        private bool CanExecute(string filename, FileResponse file)
        {
            string condition = Condition;

            Expression expression = new Expression(condition, EvaluateOptions.IterateParameters);
            foreach (var metadata in file.Metadatas)
            {
                expression.Parameters[metadata.Key] = metadata.Value;
            }

            var result = expression.Evaluate();
            if (result is IList<object>)
                return (result as IList<object>).Any(k => (bool)k);
            else
                return (bool)expression.Evaluate();
        }

        protected abstract void DoExecute(string filename, FileResponse file);
    }
}