﻿using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmptyKeys.UserInterface.Generator.Types
{
    /// <summary>
    /// Implements UI Element generator
    /// </summary>
    public class ElementGeneratorType : IGeneratorType
    {
        private static int nameUniqueId;

        /// <summary>
        /// Gets the type of the xaml.
        /// </summary>
        /// <value>
        /// The type of the xaml.
        /// </value>
        public virtual Type XamlType
        {
            get { return typeof(UIElement); }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="source">The dependency object</param>
        /// <returns></returns>
        public virtual IEnumerable GetChildren(DependencyObject source)
        {
            return null;
        }

        /// <summary>
        /// Generates control fields
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="classType">Type of the class.</param>
        /// <param name="method">The initialize method.</param>
        /// <param name="generateField">if set to <c>true</c> [generate field].</param>
        /// <returns></returns>
        public virtual CodeExpression Generate(DependencyObject source, CodeTypeDeclaration classType, CodeMemberMethod method, bool generateField)
        {
            FrameworkElement element = source as FrameworkElement;
            string typeName = element.GetType().Name;

            if (string.IsNullOrEmpty(element.Name))
            {
                element.Name = "e_" + nameUniqueId;
                nameUniqueId++;
            }

            if (generateField)
            {
                CodeMemberField field = new CodeMemberField(typeName, element.Name);
                classType.Members.Add(field);
            }
            
            CodeComment comment = new CodeComment(element.Name + " element");
            method.Statements.Add(new CodeCommentStatement(comment));

            CodeExpression fieldReference = null;
            if (!generateField)
            {
                fieldReference = new CodeVariableReferenceExpression(element.Name);
                CodeTypeReference variableType = new CodeTypeReference(typeName);
                CodeVariableDeclarationStatement declaration = new CodeVariableDeclarationStatement(variableType, element.Name);
                declaration.InitExpression = new CodeObjectCreateExpression(typeName);
                method.Statements.Add(declaration);
            }
            else
            {
                fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), element.Name);
                method.Statements.Add(new CodeAssignStatement(fieldReference, new CodeObjectCreateExpression(typeName)));
            }

            CodeComHelper.GenerateField<string>(method, fieldReference, source, FrameworkElement.NameProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.HeightProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.MaxHeightProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.MinHeightProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.WidthProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.MaxWidthProperty);
            CodeComHelper.GenerateFieldDoubleToFloat(method, fieldReference, source, FrameworkElement.MinWidthProperty);
            CodeComHelper.GenerateField<object>(method, fieldReference, source, FrameworkElement.TagProperty);
            CodeComHelper.GenerateField<bool>(method, fieldReference, source, FrameworkElement.IsEnabledProperty);
            CodeComHelper.GenerateField<bool>(method, fieldReference, source, FrameworkElement.IsHitTestVisibleProperty);
            CodeComHelper.GenerateField<bool>(method, fieldReference, source, FrameworkElement.SnapsToDevicePixelsProperty);
            CodeComHelper.GenerateField<bool>(method, fieldReference, source, UIElement.FocusableProperty);
            CodeComHelper.GenerateEnumField<Visibility>(method, fieldReference, source, FrameworkElement.VisibilityProperty);
            CodeComHelper.GenerateThicknessField(method, fieldReference, source, FrameworkElement.MarginProperty);
            CodeComHelper.GenerateEnumField<HorizontalAlignment>(method, fieldReference, source, FrameworkElement.HorizontalAlignmentProperty);
            CodeComHelper.GenerateEnumField<VerticalAlignment>(method, fieldReference, source, FrameworkElement.VerticalAlignmentProperty);
            CodeComHelper.GenerateTemplateStyleField(classType, method, fieldReference, source, FrameworkElement.StyleProperty);
            CodeComHelper.GenerateToolTipField(classType, method, fieldReference, source, FrameworkElement.ToolTipProperty);

            return fieldReference;
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        /// <param name="initMethod">The initialize method.</param>
        /// <param name="index">The index.</param>
        public virtual void AddChild(CodeExpression parent, CodeExpression child, CodeMemberMethod initMethod, int index)
        {            
        }
    }
}