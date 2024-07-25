﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModulesFramework.Data;
using UnityEngine;

namespace Modules.Extensions.Prototypes
{
    [Serializable]
    public class EntityPrototype : ISerializationCallbackReceiver
    {
        [SerializeReference]
        public List<MonoComponent> components = new();

        private HashSet<Type> _multipleTypes = new();

        public Entity Create()
        {
            return Create(ModulesFramework.MF.World);
        }

        public Entity Create(DataWorld world)
        {
            var ent = world.NewEntity();
            foreach (var monoComponent in components)
            {
                if (monoComponent == null)
                    continue;
                var isMultiple = _multipleTypes.Contains(monoComponent.ComponentType);
                if (isMultiple)
                    monoComponent.AddMultiple(ent);
                else
                    monoComponent.Add(ent);
            }

            return ent;
        }

        public void OnBeforeSerialize()
        {
            components.RemoveAll(c => c == null);
        }

        public void OnAfterDeserialize()
        {
            _multipleTypes.Clear();
            components.RemoveAll(c => c == null);
            var counter = new Dictionary<Type, int>();
            foreach (var component in components)
            {
                if (component == null) // it's null in EditMode
                    continue;
                if (!counter.TryAdd(component.ComponentType, 1))
                    counter[component.ComponentType]++;
            }

            foreach (var (type, _) in counter.Where(kvp => kvp.Value > 1))
            {
                _multipleTypes.Add(type);
            }
        }
    }
}