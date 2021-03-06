﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EmptyKeys.UserInterface.Generator
{
    /// <summary>
    /// Implements resource dictionary generator
    /// </summary>
    public class ResourceDictionaryGenerator
    {
        private int uniqueId;

        /// <summary>
        /// Generates the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="classType">Type of the class.</param>
        /// <param name="initMethod">The initialize method.</param>
        /// <param name="fieldReference">The field reference.</param>
        public void Generate(ResourceDictionary dictionary, CodeTypeDeclaration classType, CodeMemberMethod initMethod, CodeExpression fieldReference)
        {
            foreach (var mergedDict in dictionary.MergedDictionaries)
            {
                if (mergedDict.Source.IsAbsoluteUri)
                {
                    string name = Path.GetFileNameWithoutExtension(mergedDict.Source.LocalPath);
                    CodeMethodInvokeExpression addMergedDictionary = new CodeMethodInvokeExpression(
                        fieldReference, "MergedDictionaries.Add", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(name), "Instance"));
                    initMethod.Statements.Add(addMergedDictionary);
                }
                else
                {
                    Console.WriteLine("Use absolute URI for merged dictionaries.");
                }
            }

            ValueGenerator valueGenerator = new ValueGenerator();
            foreach (var resourceKey in dictionary.Keys)
            {                
                object resourceValue = dictionary[resourceKey];                               

                CodeComment comment = new CodeComment("Resource - [" + resourceKey.ToString() + "] " + resourceValue.GetType().Name);
                initMethod.Statements.Add(new CodeCommentStatement(comment));

                CodeExpression keyExpression = CodeComHelper.GetResourceKeyExpression(resourceKey);
                CodeExpression valueExpression = valueGenerator.ProcessGenerators(classType, initMethod, resourceValue, "r_" + uniqueId, dictionary);

                if (valueExpression != null)
                {                    
                    CodeMethodInvokeExpression addResourceMethod = new CodeMethodInvokeExpression(fieldReference, "Add", keyExpression, valueExpression);
                    initMethod.Statements.Add(addResourceMethod);
                }

                uniqueId++;
            }
        }                       
    }
}
