using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;
using nsEnums;
using nsInterfaces;

namespace nsNotenizerObjects
{
    public class NotenizerSentence
    {
		private Annotation _annotation;
        private Sentence _sentence;
        private NotenizerStructure _structure;

		public NotenizerSentence(Annotation annotation)
		{
			_annotation = annotation;
            _sentence = new Sentence(this.ToString());
            _structure = new NotenizerStructure(GetDepencencies(annotation));
		}

        public NotenizerSentence(Sentence sentence, NotenizerStructure structure)
        {
            _sentence = sentence;
            _structure = structure;
        }

        public Sentence Sentence
        {
            get { return this._sentence; }
            set { this._sentence = value; }
        }

        public NotenizerStructure Structure
        {
            get
            {
                return _structure;
            }

            set
            {
                _structure = value;
            }
        }

        private NotenizerDependencies GetDepencencies(Annotation annotation)
		{
			Tree tree = annotation.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
			TreebankLanguagePack treeBankLangPack = new PennTreebankLanguagePack();
			GrammaticalStructureFactory gramStructFact = treeBankLangPack.grammaticalStructureFactory();
			GrammaticalStructure gramStruct = gramStructFact.newGrammaticalStructure(tree);
			java.util.Collection typedDependencies = gramStruct.typedDependenciesCollapsed();

            NotenizerDependencies dependencies = new NotenizerDependencies();
            NotenizerDependency dep;

            foreach (TypedDependency typedDependencyLoop in (typedDependencies as java.util.ArrayList))
            {
                dep = new NotenizerDependency(typedDependencyLoop);

                dependencies.Add(dep);

                if (dep.Relation.IsNominalSubject())
                {
                    NotenizerDependency nsubjComplement = new NotenizerDependency(typedDependencyLoop);

                    nsubjComplement.TokenType = dep.TokenType == TokenType.Dependent ? TokenType.Governor : TokenType.Dependent;
                    dependencies.Add(nsubjComplement);
                }
            }

            return dependencies;
		}

        private void AddToCompressedDependencies(NotenizerDependency dep, ref Dictionary<String, List<NotenizerDependency>> map)
        {
            if (!map.ContainsKey(dep.Relation.ShortName))
                map.Add(dep.Relation.ShortName, new List<NotenizerDependency>() { dep });
            else
                map[dep.Relation.ShortName].Add(dep);
        }

        public override String ToString()
        {
            return _annotation.toString();
        }
    }
}
