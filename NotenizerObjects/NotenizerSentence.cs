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
    /// <summary>
    /// Sentence of article.
    /// </summary>
    public class NotenizerSentence
    {
        #region Variables

        private Annotation _annotation;
        private Sentence _sentence;
        private NotenizerStructure _structure;

        #endregion Variables

        #region Constructors

        public NotenizerSentence(Annotation annotation, Article article)
        {
            _annotation = annotation;
            _sentence = new Sentence(this.ToString(), article);
            _structure = new NotenizerStructure(GetDepencencies(annotation));
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Persistable sentence.
        /// </summary>
        public Sentence Sentence
        {
            get { return this._sentence; }
            set { this._sentence = value; }
        }

        /// <summary>
        /// Structure of sentence.
        /// </summary>
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets depeendencies from sentence.
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        private NotenizerDependencies GetDepencencies(Annotation annotation)
        {
            Tree tree;
            NotenizerDependency dep;
            GrammaticalStructure gramStruct;
            NotenizerDependencies dependencies;
            NotenizerDependency nsubjComplement;
            TreebankLanguagePack treeBankLangPack;
            java.util.Collection typedDependencies;
            GrammaticalStructureFactory gramStructFact;

            tree = annotation.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
            treeBankLangPack = new PennTreebankLanguagePack();
            gramStructFact = treeBankLangPack.grammaticalStructureFactory();
            gramStruct = gramStructFact.newGrammaticalStructure(tree);
            typedDependencies = gramStruct.typedDependenciesCollapsed();
            dependencies = new NotenizerDependencies();

            foreach (TypedDependency typedDependencyLoop in (typedDependencies as java.util.ArrayList))
            {
                dep = new NotenizerDependency(typedDependencyLoop);
                dependencies.Add(dep);

                if (dep.Relation.IsNominalSubject())
                {
                    nsubjComplement = new NotenizerDependency(typedDependencyLoop);
                    nsubjComplement.TokenType = dep.TokenType == TokenType.Dependent ? TokenType.Governor : TokenType.Dependent;
                    dependencies.Add(nsubjComplement);
                }
            }

            return dependencies;
        }

        public override String ToString()
        {
            return _annotation.toString();
        }

        #endregion Methods
    }
}
