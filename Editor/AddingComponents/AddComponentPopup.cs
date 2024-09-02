﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Modules.Extensions.Prototypes.Editor.AddingComponents
{
    public class AddComponentPopup : EditorWindow
    {
        private AddComponentFilteredView _filteredView;
        private AddComponentCategoryView _categoryView;
        public event Action<Type> OnAddClicked;

        public void Show(IEnumerable<Type> serializedTypes)
        {
            titleContent = new GUIContent("Add proto-component");
            var styles = Resources.Load<StyleSheet>("ModulesPrototypesUSS");
            rootVisualElement.styleSheets.Add(styles);
            rootVisualElement.AddToClassList("modules-proto--add-component-modal");

            DrawSearch();

            CreateCategoryView(serializedTypes);
            CreateFilterView(serializedTypes);

            ShowAuxWindow();
        }

        private void CreateCategoryView(IEnumerable<Type> serializedTypes)
        {
            _categoryView = new AddComponentCategoryView();
            _categoryView.OnAddClicked += type => OnAddClicked?.Invoke(type);
            _categoryView.AddTypes(serializedTypes);
            rootVisualElement.Add(_categoryView);
        }

        private void CreateFilterView(IEnumerable<Type> serializedTypes)
        {
            _filteredView = new AddComponentFilteredView();
            _filteredView.OnAddClicked += type => OnAddClicked?.Invoke(type);
            foreach (var serializedType in serializedTypes.OrderBy(s => s.Name))
                _filteredView.AddType(serializedType);
            _filteredView.style.display = DisplayStyle.None;
            rootVisualElement.Add(_filteredView);
        }

        private void DrawSearch()
        {
            var input = new TextField();
            input.label = "Search: ";
            input.AddToClassList("modules-proto--add-component--search-input");
            input.RegisterValueChangedCallback(ev => Filter(ev.newValue));
            rootVisualElement.Add(input);
        }

        private void Filter(string value)
        {
            _filteredView.style.display = string.IsNullOrWhiteSpace(value) ? DisplayStyle.None : DisplayStyle.Flex;
            _categoryView.style.display = string.IsNullOrWhiteSpace(value) ? DisplayStyle.Flex : DisplayStyle.None;
            _filteredView.Filter(value);
        }
    }
}