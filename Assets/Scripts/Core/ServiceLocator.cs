using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBound.Core
{
    /// <summary>
    /// Central dependency injection system for SoulBound RPG
    /// Provides type-safe service registration and resolution
    /// Used by Bootstrapper to wire up all core managers
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, object> _interfaceServices = new Dictionary<Type, object>();

        /// <summary>
        /// Register a service instance by its concrete type
        /// </summary>
        /// <typeparam name="T">Service type to register</typeparam>
        /// <param name="instance">Service instance</param>
        public static void Register<T>(T instance) where T : class
        {
            var type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: Overwriting existing service of type {type.Name}");
            }
            
            _services[type] = instance;
            Debug.Log($"ServiceLocator: Registered service {type.Name}");
        }

        /// <summary>
        /// Register a service instance by interface type
        /// Useful for registering concrete implementations against their interfaces
        /// </summary>
        /// <typeparam name="TInterface">Interface type to register against</typeparam>
        /// <param name="implementation">Concrete implementation instance</param>
        public static void RegisterInterface<TInterface>(TInterface implementation) where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            
            if (_interfaceServices.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"ServiceLocator: Overwriting existing interface service of type {interfaceType.Name}");
            }
            
            _interfaceServices[interfaceType] = implementation;
            Debug.Log($"ServiceLocator: Registered interface service {interfaceType.Name}");
        }

        /// <summary>
        /// Get a registered service instance by type
        /// Throws exception if service not found
        /// </summary>
        /// <typeparam name="T">Service type to retrieve</typeparam>
        /// <returns>Service instance</returns>
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            
            // Try concrete type first
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            
            // Try interface type
            if (_interfaceServices.TryGetValue(type, out var interfaceService))
            {
                return (T)interfaceService;
            }
            
            throw new InvalidOperationException($"ServiceLocator: Service of type {type.Name} not registered");
        }

        /// <summary>
        /// Try to get a registered service instance by type
        /// Returns null if service not found instead of throwing
        /// </summary>
        /// <typeparam name="T">Service type to retrieve</typeparam>
        /// <returns>Service instance or null if not found</returns>
        public static T TryGet<T>() where T : class
        {
            var type = typeof(T);
            
            // Try concrete type first
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            
            // Try interface type
            if (_interfaceServices.TryGetValue(type, out var interfaceService))
            {
                return (T)interfaceService;
            }
            
            return null;
        }

        /// <summary>
        /// Check if a service type is registered
        /// </summary>
        /// <typeparam name="T">Service type to check</typeparam>
        /// <returns>True if service is registered</returns>
        public static bool IsRegistered<T>() where T : class
        {
            var type = typeof(T);
            return _services.ContainsKey(type) || _interfaceServices.ContainsKey(type);
        }

        /// <summary>
        /// Unregister a service by type
        /// Useful for cleanup or testing scenarios
        /// </summary>
        /// <typeparam name="T">Service type to unregister</typeparam>
        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            
            if (_services.Remove(type))
            {
                Debug.Log($"ServiceLocator: Unregistered service {type.Name}");
            }
            else if (_interfaceServices.Remove(type))
            {
                Debug.Log($"ServiceLocator: Unregistered interface service {type.Name}");
            }
            else
            {
                Debug.LogWarning($"ServiceLocator: Attempted to unregister non-existent service {type.Name}");
            }
        }

        /// <summary>
        /// Clear all registered services
        /// Primarily for testing and cleanup scenarios
        /// </summary>
        public static void Clear()
        {
            int totalServices = _services.Count + _interfaceServices.Count;
            _services.Clear();
            _interfaceServices.Clear();
            Debug.Log($"ServiceLocator: Cleared {totalServices} registered services");
        }

        /// <summary>
        /// Get debug information about registered services
        /// </summary>
        /// <returns>Debug string with all registered services</returns>
        public static string GetDebugInfo()
        {
            var info = new System.Text.StringBuilder();
            info.AppendLine("ServiceLocator Registered Services:");
            
            info.AppendLine("Concrete Services:");
            foreach (var kvp in _services)
            {
                info.AppendLine($"  - {kvp.Key.Name}: {kvp.Value?.GetType().Name ?? "null"}");
            }
            
            info.AppendLine("Interface Services:");
            foreach (var kvp in _interfaceServices)
            {
                info.AppendLine($"  - {kvp.Key.Name}: {kvp.Value?.GetType().Name ?? "null"}");
            }
            
            return info.ToString();
        }
    }
} 