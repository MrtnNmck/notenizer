using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerStructure
    {
        private NotenizerDependencies _dependencies;
        private Structure _structure;

        public NotenizerStructure(NotenizerDependencies dependencies)
        {
            _dependencies = dependencies;
            _structure = new Structure(DateTime.Now, DateTime.Now);
        }

        public Structure Structure
        {
            get { return this._structure; }
            set { this._structure = value; }
        }

        public NotenizerDependencies Dependencies
        {
            get
            {
                return this._dependencies;
            }

            set
            {
                this._dependencies = value;
            }
        }
    }
}
